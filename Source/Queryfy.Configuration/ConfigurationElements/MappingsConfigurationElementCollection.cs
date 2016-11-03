namespace JanHafner.Queryfy.Configuration
{
    using System;
    using System.Configuration;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="MappingsConfigurationElementCollection"/> class.
    /// </summary>
    [UsedImplicitly]
    internal sealed class MappingsConfigurationElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingsConfigurationElementCollection"/> class.
        /// </summary>
        public MappingsConfigurationElementCollection()
        {
            this.AddElementName = "TypeMapping";
        }

        /// <inheritdoc />
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeMappingConfigurationElement();
        }

        /// <inheritdoc />
        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((TypeMappingConfigurationElement)element).SourceType;
        }
    }
}