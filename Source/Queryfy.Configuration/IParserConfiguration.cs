namespace JanHafner.Queryfy.Configuration
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines the parser configuration.
    /// </summary>
    public interface IParserConfiguration
    {
        /// <summary>
        /// Gets a list of all defined <see cref="ITypeMappingConfiguration"/> elements.
        /// </summary>
        [NotNull]
        IEnumerable<ITypeMappingConfiguration> TypeMappings { get; }
    }
}