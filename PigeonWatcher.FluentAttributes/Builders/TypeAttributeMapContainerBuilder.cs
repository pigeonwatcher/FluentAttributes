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
            Type interfaceType = configType.GetInterfaces()
              .First(i => i.IsGenericType &&
                          i.GetGenericTypeDefinition() == configInterfaceType);

            Type entityType = interfaceType.GetGenericArguments()[0];

            object? configInstance = Activator.CreateInstance(configType);
            if (configInstance == null)
            {
                continue;
            }

            Type builderType = typeof(TypeAttributeMapBuilder<>).MakeGenericType(entityType);
            object? builderInstance = Activator.CreateInstance(builderType);
            if (builderInstance == null)
            {
                continue;
            }

            MethodInfo? configureMethod = interfaceType.GetMethod("Configure");
            if (configureMethod == null)
            {
                throw new InvalidOperationException("No Configure method found on the interface.");
            }

            configureMethod.Invoke(configInstance, [builderInstance]);

            MethodInfo? buildMethod = builderType.GetMethod("Build");
            if (buildMethod == null)
            {
                throw new InvalidOperationException("No Build method found on the builder.");
            }

            if (buildMethod.Invoke(builderInstance, null) is not TypeAttributeMap typeAttributeMap)
            {
                continue;
            }

            Container.Add(typeAttributeMap);
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
}
