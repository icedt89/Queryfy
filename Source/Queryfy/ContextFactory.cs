namespace JanHafner.Queryfy
{
    using System;
    using JetBrains.Annotations;
    using Ninject;
    using Ninject.Parameters;
    using Ninject.Syntax;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// Implementation of the <see cref="IContextFactory"/> interface.
    /// </summary>
    public sealed class ContextFactory : IContextFactory
    {
        [NotNull]
        private readonly IResolutionRoot resolutionRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextFactory"/> class.
        /// </summary>
        /// <param name="resolutionRoot">An implementation of the <see cref="IResolutionRoot"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of 'resolutionRoot' cannot be null. </exception>
        public ContextFactory([NotNull] IResolutionRoot resolutionRoot)
        {
            if (resolutionRoot == null)
            {
                throw new ArgumentNullException(nameof(resolutionRoot));
            }

            this.resolutionRoot = resolutionRoot;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="IQueryficationContext"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="IQueryficationContext"/> class.</returns>
        public IQueryficationContext CreateQueryficationContext()
        {
            return this.resolutionRoot.Get<IQueryficationContext>();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="IParserContext"/> class.
        /// </summary>
        /// <param name="query">The query that is used to build this instance.</param>
        /// <returns>A new instance of the <see cref="IParserContext"/> class.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="query"/>' cannot be null. </exception>
        public IParserContext CreateParserContext(String query)
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));    
            }

            return this.resolutionRoot.Get<IParserContext>(new ConstructorArgument("query", query));
        }
    }
}