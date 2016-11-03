namespace JanHafner.Queryfy.Inspection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JanHafner.Queryfy.Attributes;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JanHafner.Toolkit.Common.Reflection;
    using JetBrains.Annotations;
    using MetadataResolution;
    using Parsing;
    using Processing;
    using Queryfication;

    /// <summary>
    /// The <see cref="QueryficationEngine"/> class.
    /// </summary>
    public sealed class QueryficationEngine : IQueryficationEngine
    {
        [NotNull]
        private readonly IPropertyMetadataFactory propertyMetadataFactory;

        [NotNull]
        private readonly IPropertyReflectorSelector propertyReflectorSelector;

        [NotNull]
        private readonly IInstanceFactory instanceFactory;

        [NotNull]
        private readonly IValueProcessorFactory valueProcessorFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryficationEngine"/> class.
        /// </summary>
        /// <param name="propertyMetadataFactory">An implementation of the <see cref="IPropertyMetadataFactory"/> interface.</param>
        /// <param name="propertyReflectorSelector">An implementation of the <see cref="IPropertyReflectorSelector"/> interface.</param>
        /// <param name="instanceFactory">An implementation of the <see cref="IInstanceFactory"/> interface.</param>
        /// <param name="valueProcessorFactory">An implementation of the <see cref="IValueProcessorFactory"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyMetadataFactory"/>', '<paramref name="propertyReflectorSelector"/>', '<paramref name="instanceFactory"/>' and '<paramref name="valueProcessorFactory"/>' cannot be null. </exception>
        public QueryficationEngine(IPropertyMetadataFactory propertyMetadataFactory, IPropertyReflectorSelector propertyReflectorSelector, IInstanceFactory instanceFactory, IValueProcessorFactory valueProcessorFactory)
        {
            if (propertyMetadataFactory == null)
            {
                throw new ArgumentNullException(nameof(propertyMetadataFactory));
            }

            if (propertyReflectorSelector == null)
            {
                throw new ArgumentNullException(nameof(propertyReflectorSelector));
            }

            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            if (valueProcessorFactory == null)
            {
                throw new ArgumentNullException(nameof(valueProcessorFactory));
            }

            this.valueProcessorFactory = valueProcessorFactory;
            this.propertyMetadataFactory = propertyMetadataFactory;
            this.propertyReflectorSelector = propertyReflectorSelector;
            this.instanceFactory = instanceFactory;
        }

        /// <summary>
        /// Fills the <see cref="IQueryficationContext"/> with the properties from the supplied <see cref="object"/>.
        /// </summary>
        /// <param name="source">The <see cref="object"/> that gets analyzed.</param>
        /// <param name="queryficationContext">The <see cref="IQueryficationContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="queryficationContext"/>' cannot be null. </exception>
        public void Fill(Object source, IQueryficationContext queryficationContext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var instanceType = source.GetType();

            var propertyReflector = this.propertyReflectorSelector.GetPropertyReflector(instanceType);
            var sourceProperties = this.FilterNecessaryProperties(instanceType, propertyReflector.ReflectProperties(source));
            this.FillCore(source, sourceProperties, queryficationContext);
        }

        /// <summary>
        /// Fills the <see cref="IQueryficationContext"/> with the properties from the supplied <see cref="Object"/>.
        /// </summary>
        /// <param name="source">The <see cref="Object"/> that gets analyzed.</param>
        /// <param name="properties">An <see cref="IEnumerable{PropertyInfo}"/> of properties from the source type.</param>
        /// <param name="queryficationContext">The <see cref="IQueryficationContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>', '<paramref name="properties"/>' and '<paramref name="queryficationContext"/>' cannot be null. </exception>
        private void FillCore([NotNull] Object source, [NotNull] IEnumerable<PropertyInfo> properties, [NotNull] IQueryficationContext queryficationContext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (queryficationContext == null)
            {
                throw new ArgumentNullException(nameof(queryficationContext));
            }

            foreach (var propertyMetadata in properties.Select(info => this.propertyMetadataFactory.GetPropertyMetadata(info)))
            {
                if (propertyMetadata.IsIgnoredOnQueryfy)
                {
                    this.OnSkipProperty(source, propertyMetadata);
                    continue;
                }

                var propertyValue = propertyMetadata.Property.GetValue(source);
                if (propertyMetadata.IsParameterGroup && propertyValue != null)
                {
                    this.Fill(propertyValue, queryficationContext);
                }
                else
                {
                    if (propertyValue != null || propertyMetadata.ValueProcessorType == typeof (NullValueProcessor))
                    {
                        var queryParameter = this.GetParameterFromPropertyCore(propertyMetadata);
                        queryParameter.Value = propertyValue;
                        queryficationContext.AddParameter(queryParameter);
                    }
                }
            }

            var extendedQueryfication = source as INeedExtendedQueryfication;
            if (extendedQueryfication != null)
            {
                extendedQueryfication.Queryfy(queryficationContext);
            }
        }

        /// <summary>
        /// Fills the <see cref="IParserContext"/> with maps based on properties of the supplied <see cref="Object"/>.
        /// </summary>
        /// <param name="source">The <see cref="Object"/> that gets analyzed.</param>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <exception cref="CannotCreateInstanceOfTypeException">A instance of the supplied <see cref="Type"/> can not be constructed but it was necessary to provide an instance.</exception>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="parserContext"/>' cannot be null. </exception>
        /// <exception cref="PropertyNotFoundException">The propertyInfo parameter is not declared on the source.</exception>
        /// <exception cref="MemberIsNotWritableException">The CanWrite property of the propertyInfo parameter returned <c>false</c>.</exception>
        public void CreateMaps(Object source, IParserContext parserContext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var instanceType = source.GetType();

            var propertyReflector = this.propertyReflectorSelector.GetPropertyReflector(instanceType);
            var sourceProperties = this.FilterNecessaryProperties(instanceType, propertyReflector.ReflectProperties(source));
            this.CreateMapsCore(source, sourceProperties, parserContext);
        }

        /// <summary>
        /// Filters all returned <see cref="PropertyInfo"/> objects.
        /// </summary>
        /// <param name="instanceType">The <see cref="Type"/> of the delcaring <see cref="Type"/>.</param>
        /// <param name="propertyInfos">The <see cref="PropertyInfo"/>s.</param>
        /// <returns>A filtered list of <see cref="PropertyInfo"/>s.</returns>
        private IEnumerable<PropertyInfo> FilterNecessaryProperties(Type instanceType, IEnumerable<PropertyInfo> propertyInfos)
        {
            Func<PropertyInfo, Boolean> currentFilter = propertyInfo => true;
            var useOnlyAttributesAttribute = instanceType.GetAttribute<UseOnlyAttributesAttribute>();
            if (useOnlyAttributesAttribute != null && useOnlyAttributesAttribute.UseOnlyAttributes)
            {
                currentFilter = propertyInfo => propertyInfo.HasAttribute<QueryParameterAttribute>();
            }

            return propertyInfos.Where(currentFilter);
        }

        /// <summary>
        /// Fills the <see cref="IParserContext"/> with maps based on properties of the supplied <see cref="Object"/>.
        /// </summary>
        /// <param name="source">The <see cref="Object"/> that gets analyzed.</param>
        /// <param name="properties">An <see cref="IEnumerable{PropertyInfo}"/> of properties from the source type.</param>
        /// <exception cref="CannotCreateInstanceOfTypeException">A instance of the supplied <see cref="Type"/> can not be constructed but it was necessary to provide an instance.</exception>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>', '<paramref name="properties"/>' and '<paramref name="parserContext"/>' cannot be null. </exception>
        /// <exception cref="MemberIsNotWritableException">The CanWrite property of the propertyInfo parameter returned <c>false</c>.</exception>
        /// <exception cref="PropertyNotFoundException">The propertyInfo parameter is not declared on the source.</exception>
        private void CreateMapsCore([NotNull] Object source, [NotNull] IEnumerable<PropertyInfo> properties, [NotNull] IParserContext parserContext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            foreach (var propertyMetadata in properties.Select(info => this.propertyMetadataFactory.GetPropertyMetadata(info)))
            {
                if (propertyMetadata.IsIgnoredOnParse)
                {
                    this.OnSkipProperty(source, propertyMetadata);
                    continue;
                }

                if (propertyMetadata.IsParameterGroup)
                {
                    Object propertyValue = null;
                    if (propertyMetadata.Property.CanRead)
                    {
                        propertyValue = propertyMetadata.Property.GetValue(source);
                    }

                    if (propertyValue == null)
                    {
                        if (propertyMetadata.TryInstantiateOnNull && !this.instanceFactory.TryCreateInstance(propertyMetadata.Property.PropertyType, out propertyValue))
                        {
                            throw new CannotCreateInstanceOfTypeException(propertyMetadata.Property.PropertyType, this.instanceFactory);
                        }
                    }
                    else
                    {
                        this.CreateMaps(propertyValue, parserContext);
                    }
                }
                else
                {
                    var urlParameter = this.GetParameterFromPropertyCore(propertyMetadata);
                    var newPropertyParserMap = new PropertyParserMap(source, propertyMetadata.Property, urlParameter);
                    parserContext.TryAddPropertyMapping(newPropertyParserMap);
                }
            }

            var extendedParsing = source as INeedExtendedParsing;
            if (extendedParsing != null)
            {
                extendedParsing.Parse(parserContext);
            }
        }

        /// <summary>
        /// Is called if the property gets skipped.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="metadata">The <see cref="IPropertyMetadata"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="metadata"/>' cannot be null. </exception>
        private void OnSkipProperty([NotNull] Object source, [NotNull] IPropertyMetadata metadata)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (!metadata.TryInstantiateOnNull)
            {
                return;
            }

            Object instance;
            if (this.instanceFactory.TryCreateInstance(metadata.Property.PropertyType, out instance) && metadata.Property.CanWrite)
            {
                metadata.Property.SetValue(source, instance);
            }
        }

        /// <summary>
        /// Returns a new <see cref="QueryParameter"/> with metadata from the supplied <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <returns>The <see cref="QueryParameter"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyInfo"/>' cannot be null. </exception>
        public QueryParameter GetParameterFromProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var propertyMetadata = this.propertyMetadataFactory.GetPropertyMetadata(propertyInfo);
            return this.GetParameterFromPropertyCore(propertyMetadata);
        }

        /// <summary>
        /// Returns a new <see cref="QueryParameter"/> with metadata from the supplied <see cref="IPropertyMetadata"/>.
        /// </summary>
        /// <param name="propertyMetadata">The <see cref="IPropertyMetadata"/>.</param>
        /// <returns>The <see cref="QueryParameter"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyMetadata"/>' cannot be null. </exception>
        [NotNull]
        private QueryParameter GetParameterFromPropertyCore([NotNull] IPropertyMetadata propertyMetadata)
        {
            if(propertyMetadata == null)
            {
                throw new ArgumentNullException(nameof(propertyMetadata));
            }
            
            var urlValueProcessor = propertyMetadata.ValueProcessorType != null
                ? this.valueProcessorFactory.GetUrlValueProcessor(propertyMetadata.Property.PropertyType, propertyMetadata.ValueProcessorType)
                : this.valueProcessorFactory.GetUrlValueProcessorForType(propertyMetadata.Property.PropertyType);

            return new QueryParameter(propertyMetadata.ParameterName, null, urlValueProcessor)
                   {
                       TypeOfValue = propertyMetadata.Property.PropertyType
                   };
        }
    }
}