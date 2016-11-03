namespace JanHafner.Queryfy.Queryfication
{
    using System;
    using Configuration;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// Providers all necessary information for the <see cref="IValueProcessor"/>
    /// </summary>
    public sealed class QueryficationProcessingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryficationProcessingContext"/> class.
        /// </summary>
        /// <param name="sourceValue">The source value.</param>
        /// <param name="configuration">The <see cref="IQueryfyConfiguration"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="sourceValue"/>' and '<paramref name="configuration"/>' cannot be null. </exception>
        public QueryficationProcessingContext([NotNull] Object sourceValue, [NotNull] IQueryfyConfiguration configuration)
        {
            if (sourceValue == null)
            {
                throw new ArgumentNullException(nameof(sourceValue));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.SourceValue = sourceValue;
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets the source value.
        /// </summary>
        [NotNull]
        public Object SourceValue { get; private set; }

        /// <summary>
        /// Gets the <see cref="IQueryfyConfiguration"/>.
        /// </summary>
        [NotNull]
        public IQueryfyConfiguration Configuration { get; private set; }
        
        /// <summary>
        /// Creates a new <see cref="QueryficationProcessingContext"/> based on the current <see cref="QueryficationProcessingContext"/>.
        /// </summary>
        /// <param name="sourceValue">The source value.</param>
        /// <returns>A new <see cref="QueryficationProcessingContext"/> based on the current <see cref="QueryficationProcessingContext"/>.</returns>
        [NotNull]
        public QueryficationProcessingContext CreateProcessingContext([NotNull] Object sourceValue)
        {
            return new QueryficationProcessingContext(sourceValue, this.Configuration);
        }
    }
}