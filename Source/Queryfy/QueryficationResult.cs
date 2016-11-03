namespace JanHafner.Queryfy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using JetBrains.Annotations;

    /// <summary>
    /// The QueryficationResult class type.
    /// </summary>
    public sealed class QueryficationResult
    {
        [NotNull]
        private readonly IQueryfyConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryficationResult"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IQueryfyConfiguration"/>.</param>
        /// <param name="processedValues">The processed values.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="configuration"/>' and '<paramref name="processedValues"/>' cannot be null. </exception>
        public QueryficationResult([NotNull] IQueryfyConfiguration configuration, [NotNull] IEnumerable<KeyValuePair<String, String>> processedValues)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (processedValues == null)
            {
                throw new ArgumentNullException(nameof(processedValues));
            }
            
            this.configuration = configuration;
            this.ProcessedValues = processedValues;
            this.QueryString = this.GetQueryString();
            this.EncodedQueryString = this.GetQueryString(Uri.EscapeDataString);
        }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        [NotNull]
        public String QueryString { get; private set; }

        /// <summary>
        /// Gets ans <see cref="IEnumerable{KeyValuePair{String, String}}"/> with all processed values.
        /// </summary>
        [NotNull]
        public IEnumerable<KeyValuePair<String, String>> ProcessedValues { get; private set; }

        /// <summary>
        /// Gets the encoded query string.
        /// </summary>
        [NotNull]
        public String EncodedQueryString { get; private set; }

        /// <summary>
        /// Format the processed values as a query string.
        /// </summary>
        /// <returns>The query string.</returns>
        [NotNull]
        private String GetQueryString()
        {
            return this.GetQueryString(value => value);
        }

        /// <summary>
        /// Format the processed values as a query string and applies an converter function on each value.
        /// </summary>
        /// <param name="valueConverter">The value converter function.</param>
        /// <returns>The query string.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="valueConverter"/>' cannot be null. </exception>
        [NotNull]
        private String GetQueryString([NotNull] Func<String, String> valueConverter)
        {
            if (valueConverter == null)
            {
                throw new ArgumentNullException(nameof(valueConverter));
            }

            return String.Join(this.configuration.QueryParametersSeparatorChar.ToString(), this.ProcessedValues.Select(pair => String.Format("{0}{1}{2}", pair.Key, this.configuration.QueryParameterValueSeparatorChar, valueConverter(pair.Value))));
        }
    }
}