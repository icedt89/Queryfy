namespace JanHafner.Queryfy.Configuration
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IQueryfyConfiguration"/> interface provides common configuraton values used during parsing and queryfying.
    /// </summary>
    public interface IQueryfyConfiguration
    {
        /// <summary>
        /// Used to separate values. E.g. value1+value2+value3.
        /// </summary>
        Char QueryParameterValueToValueSeparatorChar { get; }

        /// <summary>
        /// Used to separate the name of the parameter from the value(s). E.g. List=value1.
        /// </summary>
        Char QueryParameterValueSeparatorChar { get; }

        /// <summary>
        /// Used to separate parameters. E.g. List=value1+value2&amp;Some=property.
        /// </summary>
        Char QueryParametersSeparatorChar { get; }

        /// <summary>
        /// Gets the pattern which splits query parameters from the query string.
        /// </summary>
        [NotNull]
        String QueryParameterRegexPattern { get; }

        /// <summary>
        /// Gets the pattern which splits array-like values from the query string.
        /// </summary>
        [NotNull]
        String QueryParameterSplitValuesRegexPattern { get; }

        /// <summary>
        /// Gets the <see cref="IParserConfiguration"/>.
        /// </summary>
        [CanBeNull]
        IParserConfiguration ParserConfiguration { get; }
    }
}