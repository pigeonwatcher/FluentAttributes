using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Builders;

/// <summary>
/// The builder for building <see cref="TypeAttributeMapContainer"/> instances.
/// </summary>
public class TypeAttributeMapContainerBuilder
{
    private TypeAttributeMapContainer? _container;
    /// <summary>
    /// The <see cref="TypeAttributeMapContainer"/> instance being built.
    /// </summary>
    private TypeAttributeMapContainer Container => _container ??= [];

    /// <summary>
    /// Adds a <see cref="TypeAttributeMap"/> to the container.
    /// </summary>
    /// <typeparam name="T">The mapped <see cref="Type"/>.</typeparam>
    /// <param name="configuration">The <see cref="ITypeAttributeMapConfiguration{T}"/> to add.</param>
    /// <returns>The same <see cref="TypeAttributeMapContainerBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is <see langword="null"/>.</exception>
    public TypeAttributeMapContainerBuilder ApplyConfiguration<T>(ITypeAttributeMapConfiguration<T> configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        ApplyConfigurationInternal(configuration);
        return this;
    }

    /// <summary>
    /// Applies all <see cref="ITypeAttributeMapConfiguration{T}"/>'s from the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly containing the configurations.</param>
    /// <returns>The same <see cref="TypeAttributeMapContainerBuilder"/> instance.</returns>
    public TypeAttributeMapContainerBuilder ApplyConfigurationsFromAssembly(Assembly assembly)
    {
        Type configInterfaceType = typeof(ITypeAttributeMapConfiguration<>);

        IEnumerable<Type> configTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface &&
                        t.GetInterfaces().Any(i => i.IsGenericType &&
                                                   i.GetGenericTypeDefinition() == configInterfaceType));

        foreach (Type configType in configTypes)
        {
            object? configInstance = Activator.CreateInstance(configType);
            if (configInstance != null)
            {
                ApplyConfigurationInternal(configInstance);
            }
        }

        return this;
    }

    /// <summary>
    /// Builds the <see cref="TypeAttributeMapContainer"/> instance.
    /// </summary>
    /// <returns>The built <see cref="TypeAttributeMapContainer"/> instance.</returns>
    public TypeAttributeMapContainer Build()
    {
        TypeAttributeMapContainer container = Container;
        _container = null;
        return container;
    }

    /// <summary>
    /// Add the specified <see cref="ITypeAttributeMapConfiguration{T}"/> to the container.
    /// </summary>
    /// <param name="configuration">The <see cref="ITypeAttributeMapConfiguration{T}"/> to add.</param>
    private void ApplyConfigurationInternal(object configuration)
    {
        Type configInterfaceType = typeof(ITypeAttributeMapConfiguration<>);

        Type interfaceType = configuration.GetType().GetInterfaces()
            .First(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == configInterfaceType);

        Type entityType = interfaceType.GetGenericArguments()[0];
        Type builderType = typeof(TypeAttributeMapBuilder<>).MakeGenericType(entityType);

        object? builderInstance = Activator.CreateInstance(builderType);
        if (builderInstance == null)
        {
            throw new InvalidOperationException("Failed to create builder instance.");
        }

        MethodInfo? configureMethod = interfaceType.GetMethod("Configure");
        if (configureMethod == null)
        {
            throw new InvalidOperationException("No Configure method found on the interface.");
        }

        configureMethod.Invoke(configuration, [builderInstance]);

        MethodInfo? buildMethod = builderType.GetMethod("Build");
        if (buildMethod == null)
        {
            throw new InvalidOperationException("No Build method found on the builder.");
        }

        if (buildMethod.Invoke(builderInstance, null) is not TypeAttributeMap typeAttributeMap)
        {
            throw new InvalidOperationException("Failed to build TypeAttributeMap.");
        }

        Container.Add(typeAttributeMap);
    }
}
