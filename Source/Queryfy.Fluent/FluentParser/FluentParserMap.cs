namespace JanHafner.Queryfy.Fluent.FluentParser
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using Parsing;
    using Processing;

    /// <summary>
    /// Implements <see cref="IFluentParserMap"/>.
    /// </summary>
    public sealed class FluentParserMap : IFluentParserMap
    {
        [NotNull]
        private readonly IParserContext parserContext;

        [NotNull]
        private readonly FluentParser fluentParser;

        private String name;

        private IValueProcessor valueProcessor;

        private LambdaExpression lambdaExpression;

        private Object source;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentParserMap"/> class.
        /// </summary>
        /// <param name="parserContext">An implementation of the <see cref="IParserContext"/> interface.</param>
        /// <param name="fluentParser">An implementation of the <see cref="IFluentParser"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parserContext"/>' and '<paramref name="fluentParser"/>' cannot be null. </exception>
        public FluentParserMap([NotNull] IParserContext parserContext, [NotNull] FluentParser fluentParser)
        {
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            if (fluentParser == null)
            {
                throw new ArgumentNullException(nameof(fluentParser));
            }
            
            this.parserContext = parserContext;
            this.fluentParser = fluentParser;
        }

        /// <summary>
        /// Binds the map to the supplied parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>Returns self.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="name"/>' cannot be null. </exception>
        public IFluentParserMap Named(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.name = name;
            return this;
        }

        /// <summary>
        /// Binds the map to the supplied <see cref="IValueProcessor"/>.
        /// </summary>
        /// <param name="valueProcessor">The <see cref="IValueProcessor"/>.</param>
        /// <returns>Returns self.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="valueProcessor"/>' cannot be null. </exception>
        public IFluentParserMap ProcessUsing(IValueProcessor valueProcessor)
        {
            if (valueProcessor == null)
            {
                throw new ArgumentNullException(nameof(valueProcessor));
            }

            this.valueProcessor = valueProcessor;
            return this;
        }

        /// <summary>
        /// Binds the source object and <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="lambdaExpression">The <see cref="LambdaExpression"/>.</param>
        /// <returns>Returns self.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="lambdaExpression"/>' cannot be null. </exception>
        public IFluentParserMap BindTo(Object source, LambdaExpression lambdaExpression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            if (lambdaExpression == null)
            {
                throw new ArgumentNullException(nameof(lambdaExpression));
            }

            this.lambdaExpression = lambdaExpression;
            this.source = source;
            return this;
        }

        /// <summary>
        /// Adds the map to the context.
        /// </summary>
        /// <returns>Returns the parent.</returns>
        public IFluentParser Map()
        {
            this.parserContext.AddPropertyMapping(this.source, this.lambdaExpression, this.name, this.valueProcessor);
            return this.fluentParser;
        }
    }
}