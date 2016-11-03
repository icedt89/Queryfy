namespace JanHafner.Queryfy.Inspection.PropertyRetrieval
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Reflection;
    using JetBrains.Annotations;
    using Properties;

    /// <summary>
    /// The <see cref="ExpandoObjectPropertyReflector"/> class is specialized in processing <see cref="ExpandoObject"/> instances.
    /// </summary>
    internal sealed class ExpandoObjectPropertyReflector : IPropertyReflector
    {
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> from the supplied instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>An <see cref="IEnumerable{PropertyInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="instance"/>' cannot be null. </exception>
        /// <exception cref="ArgumentException">The argument '<paramref name="instance"/>' is not of Type ExpandoObject.</exception>
        [Pure]
        [LinqTunnel]
        public IEnumerable<PropertyInfo> ReflectProperties(Object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (!(instance is ExpandoObject))
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNotExpandoObjectExceptionMessage, "instance"));
            }

            var expandoInstance = (IDictionary<String, Object>) instance;
            foreach (var expandoProperty in expandoInstance)
            {
                if (expandoProperty.Value != null)
                {
                    yield return new ExpandoObjectPropertyInfo(expandoProperty.Key, expandoProperty.Value.GetType(), instance.GetType());
                }

                yield return new ExpandoObjectPropertyInfo(expandoProperty.Key, null, instance.GetType());
            }
        }
    }
}