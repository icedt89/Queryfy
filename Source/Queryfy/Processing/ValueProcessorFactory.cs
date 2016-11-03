namespace JanHafner.Queryfy.Processing
{
    using System;
    using System.ComponentModel;
    using Attributes;
    using Extensions;
    using Inspection;
    using Toolkit.Common.ExtensionMethods;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="ValueProcessorFactory"/> class provides implementations of the <see cref="IValueProcessor"/> by reusing the built-in ValueProcessors.
    /// </summary>
    public class ValueProcessorFactory : IValueProcessorFactory
    {
        [NotNull]
        private readonly ITypeValueProcessorRegistry valueProcessorRegistry;

        [NotNull]
        private readonly IInstanceFactory instanceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueProcessorFactory"/> class.
        /// </summary>
        /// <param name="valueProcessorRegistry">An implementation of the <see cref="ITypeValueProcessorRegistry"/> interface.</param>
        /// <param name="instanceFactory">An implementation of the <see cref="ITypeValueProcessorRegistry"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="valueProcessorRegistry"/>' and '<paramref name="instanceFactory"/>' cannot be null. </exception>
        public ValueProcessorFactory([NotNull] ITypeValueProcessorRegistry valueProcessorRegistry, [NotNull] IInstanceFactory instanceFactory)
        {
            if (valueProcessorRegistry == null)
            {
                throw new ArgumentNullException(nameof(valueProcessorRegistry));
            }

            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            this.valueProcessorRegistry = valueProcessorRegistry;
            this.instanceFactory = instanceFactory;
        }

        /// <summary>
        /// Gets an <see cref="IValueProcessor"/> instance.
        /// </summary>
        /// <param name="propertyType">The <see cref="Type"/> of the property.</param>
        /// <returns>An implementation of the <see cref="IValueProcessor"/> interface.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyType"/>' cannot be null. </exception>
        public virtual IValueProcessor GetUrlValueProcessorForType(Type propertyType)
        {
            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            IValueProcessor lookupResult;
            if (this.valueProcessorRegistry.TryLookup(propertyType, out lookupResult))
            {
                // ReSharper disable once AssignNullToNotNullAttribute If TryLookup is true, the value can never be null
                return lookupResult;
            }

            if (propertyType.IsGenericType)
            {
                var singleGenericType = propertyType.GetSingleGenericParameter();
                var innerUrlValueProcessor = this.GetUrlValueProcessorForType(singleGenericType);
                if (propertyType.IsLazy())
                {
                    return new LazyValueProcessor(innerUrlValueProcessor);
                }

                if (propertyType.IsGenericIEnumerable() && !propertyType.IsPrimitive())
                {
                    return new EnumerableValueProcessor(innerUrlValueProcessor);
                }
            }

            if (propertyType.IsIEnumerable() && !propertyType.IsPrimitive())
            {
                return new EnumerableValueProcessor(new DefaultValueProcessor());
            }

            if (propertyType.IsEnum)
            {
                if (propertyType.HasAttribute<UseEnumValueNameAttribute>())
                {
                    return new EnumMemberAsNameValueProcessor();
                }

                return new EnumMemberAsUnderlyingTypeValueProcessor();
            }

            return new DefaultValueProcessor();
        }

        /// <summary>
        /// Gets an <see cref="IValueProcessor"/> instance.
        /// </summary>
        /// <param name="propertyType">The <see cref="Type"/> of the property.</param>
        /// <param name="valueProcessorType">The <see cref="Type"/> of the <see cref="IValueProcessor"/> to retrieve.</param>
        /// <returns>An implementation of the <see cref="IValueProcessor"/> interface.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyType"/>' cannot be null. </exception>
        /// <exception cref="CannotCreateInstanceOfTypeException">If no instance could be created.</exception>
        public virtual IValueProcessor GetUrlValueProcessor(Type propertyType, Type valueProcessorType)
        {
            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            if (valueProcessorType == null)
            {
                return this.GetUrlValueProcessorForType(propertyType);
            }

            if (valueProcessorType == typeof (ValueProcessorTypeConverterAdapter))
            {
                var valueConverter = TypeDescriptor.GetConverter(valueProcessorType);
                return new ValueProcessorTypeConverterAdapter(valueConverter);
            }

            return this.instanceFactory.CreateInstance<IValueProcessor>(valueProcessorType);
        }
    }
}