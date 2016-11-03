namespace JanHafner.Queryfy.Configuration
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines the type mappings.
    /// </summary>
    public interface ITypeMappingConfiguration
    {
        /// <summary>
        /// Gets the <see cref="Type"/> source.
        /// </summary>
        [NotNull]
        Type SourceType { get; }

        /// <summary>
        /// Gets a list of all defined <see cref="IPropertyMappingConfiguration"/> elements.
        /// </summary>
        [NotNull]
        IEnumerable<IPropertyMappingConfiguration> PropertyMappings { get; }
    }
}