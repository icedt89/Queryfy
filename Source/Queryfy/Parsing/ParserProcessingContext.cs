namespace JanHafner.Queryfy.Parsing
{
    using System;
    using Configuration;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// Providers all necessary information for the <see cref="IValueProcessor"/>
    /// </summary>
    public sealed class ParserProcessingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserProcessingContext"/> class.
        /// </summary>
        /// <param name="sourceValue">The source value.</param>
        /// <param name="destinationType">The <see cref="Type"/> of the destination.</param>
        /// <param name="configuration">The <see cref="IQueryfyConfiguration"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="sourceValue"/>', '<paramref name="configuration"/>' and '<paramref name="destinationType"/>' cannot be null. </exception>
        public ParserProcessingContext([NotNull] String sourceValue, [NotNull] Type destinationType, [NotNull] IQueryfyConfiguration configuration)
        {
            if (String.IsNullOrEmpty(sourceValue))
            {
                throw new ArgumentNullException(nameof(sourceValue));
            }

            if (destinationType == null)
            {
                throw new ArgumentNullException(nameof(destinationType));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.SourceValue = sourceValue;
            this.Configuration = configuration;
            this.DestinationType = destinationType;
        }

        /// <summary>
        /// Gets the source value.
        /// </summary>
        [NotNull]
        public String SourceValue { get; private set; }

        /// <summary>
        /// Gets the <see cref="Type"/> of the destination.
        /// </summary>
        [NotNull]
        public Type DestinationType { get; private set; }

        /// <summary>
        /// The <see cref="IQueryfyConfiguration"/>.
        /// </summary>
        [NotNull]
        public IQueryfyConfiguration Configuration { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ParserProcessingContext"/> based on the current <see cref="ParserProcessingContext"/>.
        /// </summary>
        /// <param name="sourceValue">The source value.</param>
        /// <param name="destinationType">The <see cref="Type"/> of the destination.</param>
        /// <returns>A new <see cref="ParserProcessingContext"/> based on the current <see cref="ParserProcessingContext"/>.</returns>
        [NotNull]
        public ParserProcessingContext CreateProcessingContext([NotNull] String sourceValue, [NotNull] Type destinationType)
        {
            return new ParserProcessingContext(sourceValue, destinationType, this.Configuration);
        }
    }
}