namespace JanHafner.Queryfy.Configuration
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Defines a property mapping.
    /// </summary>
    public interface IPropertyMappingConfiguration
    {
        /// <summary>
        /// Gets the property path.
        /// </summary>
        [NotNull]
        String PropertyPath { get; }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        [NotNull]
        String ParameterName { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> of the <see cref="IValueProcessor"/>.
        /// </summary>
        [CanBeNull]
        Type ValueProcessorType { get; }
    }
}