namespace JanHafner.Queryfy.Inspection.MetadataResolution
{
    using System;
    using System.Reflection;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// The <see cref="IPropertyMetadata"/> interface provides metadata about an <see cref="PropertyInfo"/>.
    /// </summary>
    public interface IPropertyMetadata
    {
        /// <summary>
        /// Gets or sets a value indicating, that an attempt should be made to instantiate the property if a <see langword="null"/> value would be retrieved.
        /// </summary>
        /// <value><see langword="true"/> if it should be tried to instantiate the property if no <see langword="null"/> would be retrieved; otherwise, <see langword="false"/>.</value>
        Boolean TryInstantiateOnNull { get; }

        /// <summary>
        /// Gets or sets a value indicating whether <see langword="this"/> instance is a parameter group.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if this instance is a parameter group; otherwise, <see langword="false"/>.
        /// </value>
        Boolean IsParameterGroup { get; }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        [NotNull]
        String ParameterName { get; }

        /// <summary>
        /// Gets or sets a value indicating whether <see langword="this"/> instance is ignored on parsing.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if <see langword="this"/> instance is ignored on parsing; otherwise, <see langword="false"/>.
        /// </value>
        Boolean IsIgnoredOnParse { get; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="IValueProcessor"/>.
        /// </summary>
        /// <value>The <see cref="Type"/> of the <see cref="IValueProcessor"/>.</value>
        [CanBeNull]
        Type ValueProcessorType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether <see langword="this"/> instance is ignored on queryfy.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if <see langword="this"/> instance is ignored on queryfy; otherwise, <see langword="falses"/>.
        /// </value>
        Boolean IsIgnoredOnQueryfy { get; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/>.
        /// </summary>
        [NotNull]
        PropertyInfo Property { get; }
    }
}