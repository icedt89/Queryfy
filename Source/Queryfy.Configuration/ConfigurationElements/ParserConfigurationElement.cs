namespace JanHafner.Queryfy.Configuration
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// The <see cref="ParserConfigurationElement"/> class provides access to parser specific configuration.
    /// </summary>
    internal sealed class ParserConfigurationElement : ConfigurationElement, IParserConfiguration
    {
        /// <summary>
        /// Gets the <see cref="MappingsConfigurationElementCollection"/>.
        /// </summary>
        [ConfigurationProperty("Mappings", DefaultValue = null, IsRequired = true)]
        public MappingsConfigurationElementCollection Mappings
        {
            get { return (MappingsConfigurationElementCollection) this["Mappings"]; }
        }

        /// <summary>
        /// Gets a list of all defined <see cref="ITypeMappingConfiguration"/> elements.
        /// </summary>
        public IEnumerable<ITypeMappingConfiguration> TypeMappings
        {
            get
            {
                var mappings = this.Mappings;
                if (mappings == null)
                {
                    return Enumerable.Empty<ITypeMappingConfiguration>();
                }

                return mappings.Cast<ITypeMappingConfiguration>();
            }
        }
    }
}