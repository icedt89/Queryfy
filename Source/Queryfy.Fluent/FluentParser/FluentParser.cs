namespace JanHafner.Queryfy.Fluent.FluentParser
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using Parsing;

    /// <summary>
    /// Implements <see cref="IFluentParser"/>.
    /// </summary>
    public sealed class FluentParser : IFluentParser
    {
        [NotNull]
        private readonly IQueryfyDotNet queryfyDotNet;

        [NotNull]
        private readonly IParserContext parserContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentParser"/> class.
        /// </summary>
        /// <param name="queryfyDotNet">An implementation of the <see cref="IQueryfyDotNet"/> interface.</param>
        /// <param name="queryString">The query string.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryfyDotNet"/>' and '<paramref name="queryString"/>' cannot be null. </exception>
        public FluentParser([NotNull] IQueryfyDotNet queryfyDotNet, [NotNull] String queryString)
        {
            if (queryfyDotNet == null)
            {
                throw new ArgumentNullException(nameof(queryfyDotNet));
            }

            if (String.IsNullOrEmpty(queryString))
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            this.queryfyDotNet = queryfyDotNet;
            this.parserContext = this.queryfyDotNet.CreateParserContext(queryString);
        }

        /// <summary>
        /// Provides a fluent syntax for mapping a property.
        /// </summary>
        /// <returns></returns>
        public IFluentParserMap Property()
        {
            return new FluentParserMap(this.parserContext, this);
        }

        /// <summary>
        /// Binds the source object and <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="lambdaExpression">The <see cref="LambdaExpression"/>.</param>
        /// <returns>Returns self.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="lambdaExpression"/>' cannot be null. </exception>
        public IFluentParserMap Property(Object source, LambdaExpression lambdaExpression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            if (lambdaExpression == null)
            {
                throw new ArgumentNullException(nameof(lambdaExpression));
            }

            return new FluentParserMap(this.parserContext, this).BindTo(source, lambdaExpression);
        }

        /// <summary>
        /// Processes the context.
        /// </summary>
        public void Parse()
        {
            this.queryfyDotNet.Parse(this.parserContext);
        }
    }
}