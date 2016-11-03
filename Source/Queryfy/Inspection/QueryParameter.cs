namespace JanHafner.Queryfy.Inspection
{
    using System;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// The <see cref="QueryParameter"/> class provides information about the parameter to process.
    /// </summary>
    public sealed class QueryParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameter"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parameterName"/>' cannot be null. </exception>
        public QueryParameter([NotNull] String parameterName, [CanBeNull] Object value)
            : this(parameterName, value, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameter"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">THe value of the parameter.</param>
        /// <param name="valueProcessor">An implementation of the <see cref="IValueProcessor"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parameterName"/>' cannot be null. </exception>
        public QueryParameter([NotNull] String parameterName, [CanBeNull] Object value, [CanBeNull] IValueProcessor valueProcessor)
        {
            if (String.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            this.ParameterName = parameterName;
            this.Value = value;
            this.ValueProcessor = valueProcessor;
            this.TypeOfValue = value == null ? null : value.GetType();
        }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        [NotNull]
        public String ParameterName { get; private set; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        [CanBeNull]
        public Object Value { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IValueProcessor"/>.
        /// </summary>
        [CanBeNull]
        public IValueProcessor ValueProcessor { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the value.
        /// </summary>
        [CanBeNull]
        public Type TypeOfValue { get; set; }
    }
}