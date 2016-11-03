namespace JanHafner.Queryfy.Inspection.PropertyRetrieval
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Reflection;
    using JetBrains.Annotations;
    using Properties;

    /// <summary>
    /// The <see cref="DynamicObjectPropertyReflector"/> class is specialized in processing dynamic objects instances.
    /// </summary>
    internal sealed class DynamicObjectPropertyReflector : IPropertyReflector
    {
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> from the supplied instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>An <see cref="IEnumerable{PropertyInfo}"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of 'instance' cannot be null. </exception>
        /// <exception cref="ArgumentException">The argument '<paramref name="instance"/>' is not of Type ExpandoObject.</exception>
        [LinqTunnel]
        [Pure]
        public IEnumerable<PropertyInfo> ReflectProperties(Object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            var dynamicInstance = instance as DynamicObject;
            if (dynamicInstance == null)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ArgumentIsNotDynamicObjectExceptionMessage, "instance"));
            }
            
            foreach (var dynamicMemberName in dynamicInstance.GetDynamicMemberNames())
            {
                Object value;
                if (dynamicInstance.TryGetMember(new DynamicObjectGetMemberBinder(dynamicMemberName), out value))
                {
                    yield return new DynamicObjectPropertyInfo(dynamicMemberName, value.GetType(), instance.GetType());
                }

                yield return new DynamicObjectPropertyInfo(dynamicMemberName, null, instance.GetType());
            }
        }
    }
}