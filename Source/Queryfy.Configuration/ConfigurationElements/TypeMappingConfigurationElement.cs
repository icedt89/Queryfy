namespace JanHafner.Queryfy.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// The <see cref="TypeMappingConfigurationElement"/> class provides access to mappings on a per type basis.
    /// </summary>
    internal sealed class TypeMappingConfigurationElement : ConfigurationElementCollection, ITypeMappingConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMappingConfigurationElement"/> class.
        /// </summary>
        public TypeMappingConfigurationElement()
        {
            this.AddElementName = "PropertyMapping";
        }

        /// <summary>
        /// Gets the <see cref="Type"/> source.
        /// </summary>
        [TypeConverter(typeof (TypeNameConverter))]
        [ConfigurationProperty("sourceType", DefaultValue = null, IsRequired = true)]
        public Type SourceType
        {
            get { return (Type) this["sourceType"]; }
        }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> which defines the scopes for which the authorization should be ganted.
        /// </summary>
        [ConfigurationProperty("PropertyMapping", IsRequired = true)]
        public IEnumerable<IPropertyMappingConfiguration> PropertyMappings
        {
            get
            {
                return this.Cast<PropertyMappingConfigurationElement>();
            }
        }

        /// <inheritdoc />
        protected override ConfigurationElement CreateNewElement()
        {
            return new PropertyMappingConfigurationElement();
        }

        /// <inheritdoc />
        protected override Object GetElementKey(ConfigurationElement element)
        {
            var propertyMappingElement = (PropertyMappingConfigurationElement) element;
            return String.Format("{0}=>{1}", propertyMappingElement.PropertyPath, propertyMappingElement.ParameterName);
        }
    }
}