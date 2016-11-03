namespace JanHafner.Queryfy.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Configuration;

    /// <summary>
    /// The <see cref="PropertyMappingConfigurationElement"/> class provides access to the configuration of a parser map.
    /// </summary>
    internal sealed class PropertyMappingConfigurationElement : ConfigurationElement, IPropertyMappingConfiguration
    {
        /// <summary>
        /// Gets the property path.
        /// </summary>
        [ConfigurationProperty("propertyPath", DefaultValue = "", IsRequired = true)]
        public String PropertyPath
        {
            get { return (String) this["propertyPath"]; }
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        [ConfigurationProperty("parameterName", DefaultValue = "", IsRequired = true)]
        public String ParameterName
        {
            get { return (String) this["parameterName"]; }
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the <see cref="IValueProcessor"/>.
        /// </summary>
        [TypeConverter(typeof (TypeNameConverter))]
        [ConfigurationProperty("valueProcessorType", DefaultValue = null)]
        public Type ValueProcessorType
        {
            get { return (Type) this["valueProcessorType"]; }
        }
    }
}