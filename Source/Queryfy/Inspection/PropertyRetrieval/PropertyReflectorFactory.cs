namespace JanHafner.Queryfy.Inspection.PropertyRetrieval
{
    using System;
    using System.Dynamic;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IPropertyReflectorFactory"/> class.
    /// </summary>
    [PublicAPI]
    public class PropertyReflectorFactory : IPropertyReflectorFactory
    {
        [NotNull]
        private static readonly IPropertyReflector ExpandoObjectPropertyReflector;

        [NotNull]
        private static readonly IPropertyReflector DynamicObjectPropertyReflector;

        [NotNull]
        private static readonly IPropertyReflector DefaultPropertyReflector;

        /// <summary>
        /// Initializes the <see cref="PropertyReflectorFactory"/> class.
        /// </summary>
        [PublicAPI]
        static PropertyReflectorFactory()
        {
            PropertyReflectorFactory.ExpandoObjectPropertyReflector = new ExpandoObjectPropertyReflector();
            PropertyReflectorFactory.DefaultPropertyReflector = new PropertyReflector();
            PropertyReflectorFactory.DynamicObjectPropertyReflector = new DynamicObjectPropertyReflector();
        }

        /// <summary>
        /// Gets an instance of an <see cref="IPropertyReflector"/>.
        /// </summary>
        /// <param name="instanceType">The <see cref="Type"/> in which to choose the right <see cref="IPropertyReflector"/>.</param>
        /// <exception cref="NotSupportedException">The supplied <see cref="Type"/> does not derive from <see cref="ExpandoObject"/> or <see cref="DynamicObject"/>.</exception>
        /// <returns>An <see cref="IPropertyReflector"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="instanceType"/>' cannot be null. </exception>
        [PublicAPI]
        [Pure]
        public virtual IPropertyReflector GetPropertyReflector(Type instanceType)
        {
            if (instanceType == null)
            {
                throw new ArgumentNullException("instanceType");    
            }

            if (!typeof (IDynamicMetaObjectProvider).IsAssignableFrom(instanceType))
            {
                return DefaultPropertyReflector;
            }

            if (typeof (ExpandoObject).IsAssignableFrom(instanceType))
            {
                return ExpandoObjectPropertyReflector;
            }

            if (typeof (DynamicObject).IsAssignableFrom(instanceType))
            {
                return DynamicObjectPropertyReflector;
            }

            throw new NotSupportedException("Types that implement IDynamicMetaObjectProvider and are not derived from ExpandoObject or DynamicObject are currently not supported.");
        }
    }
}