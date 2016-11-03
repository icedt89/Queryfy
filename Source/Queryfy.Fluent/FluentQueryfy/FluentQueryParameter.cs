namespace JanHafner.Queryfy.Fluent.FluentQueryfy
{
    using System;
    using JetBrains.Annotations;
    using Processing;
    using Queryfication;

    /// <summary>
    /// Implements <see cref="IFluentQueryParameter"/>.
    /// </summary>
    public sealed class FluentQueryParameter : IFluentQueryParameter
    {
        [NotNull]
        private readonly IQueryficationContext queryficationContext;

        [NotNull]
        private readonly IFluentQueryfy fluentQueryfy;

        private String name;

        private Object value;

        private IValueProcessor valueProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentQueryParameter"/> class.
        /// </summary>
        /// <param name="queryficationContext">An implementation of the <see cref="IQueryficationContext"/> interface.</param>
        /// <param name="fluentQueryfy">An implementation of the <see cref="IFluentQueryfy"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryficationContext"/>' and '<paramref name="fluentQueryfy"/>' cannot be null. </exception>
        public FluentQueryParameter([NotNull] IQueryficationContext queryficationContext, [NotNull] IFluentQueryfy fluentQueryfy)
        {
            if (queryficationContext == null)
            {
                throw new ArgumentNullException(nameof(queryficationContext));
            }
            
            if (fluentQueryfy == null)
            {
                throw new ArgumentNullException(nameof(fluentQueryfy));
            }

            this.queryficationContext = queryficationContext;
            this.fluentQueryfy = fluentQueryfy;
        }

        /// <summary>
        /// Sets the supplied value.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>Returns self.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="value"/>' cannot be null. </exception>
        public IFluentQueryParameter Value(Object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.value = value;
            return this;
        }

        /// <summary>
        /// Sets the supplied name.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>Returns self.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="name"/>' cannot be null. </exception>
        public IFluentQueryParameter Named(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.name = name;
            return this;
        }

        /// <summary>
        /// Sets the supplied <see cref="IValueProcessor"/>.
        /// </summary>
        /// <param name="valueProcessor">The <see cref="IValueProcessor"/>.</param>
        /// <returns>Returns self.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="valueProcessor"/>' cannot be null. </exception>
        public IFluentQueryParameter ProcessUsing(IValueProcessor valueProcessor)
        {
            if (valueProcessor == null)
            {
                throw new ArgumentNullException(nameof(valueProcessor));
            }

            this.valueProcessor = valueProcessor;
            return this;
        }

        /// <summary>
        /// Adds the parameter to the context.
        /// </summary>
        /// <returns>Returns the parent.</returns>
        public IFluentQueryfy Add()
        {
            this.queryficationContext.AddParameter(this.name, this.value, this.valueProcessor);
            return this.fluentQueryfy;
        }
    }
}