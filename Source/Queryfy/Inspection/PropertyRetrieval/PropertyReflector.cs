namespace JanHafner.Queryfy.Inspection.PropertyRetrieval
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using JetBrains.Annotations;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// The <see cref="PropertyReflector"/> class.
    /// </summary>
    [PublicAPI]
    public class PropertyReflector : IPropertyReflector
    {
        /// <summary>
        /// The BindingFlags used by the <see cref="PropertyReflector"/>.
        /// By default:  BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public.
        /// </summary>
        [PublicAPI]
        public static BindingFlags ReflectorBindingFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// Gets an <see cref="IEnumerable{PropertyInfo}"/> from the supplied instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>An <see cref="IEnumerable{PropertyInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="instance" />' cannot be null. </exception>
        [LinqTunnel]
        [Pure]
        [PublicAPI]
        public virtual IEnumerable<PropertyInfo> ReflectProperties(Object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            var instanceType = instance.GetType();
            Func<PropertyInfo, Boolean> currentFilter = propertyInfo => true;
            var useOnlyAttributesAttribute = instanceType.GetAttribute<UseOnlyAttributesAttribute>();
            if (useOnlyAttributesAttribute != null && useOnlyAttributesAttribute.UseOnlyAttributes)
            {
                currentFilter = propertyInfo => propertyInfo.HasAttribute<QueryParameterAttribute>();
            }

            return instanceType.GetProperties(ReflectorBindingFlags).Where(currentFilter);
        }
    }
}