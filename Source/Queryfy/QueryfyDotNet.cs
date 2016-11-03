namespace JanHafner.Queryfy
{
    using System;
    using Extensions;
    using Inspection;
    using JetBrains.Annotations;
    using Parsing;
    using Processing;
    using Queryfication;

    /// <summary>
    /// Provides the main functionality of this framework.
    /// </summary>
    public sealed class QueryfyDotNet : IQueryfyDotNet
    {
        [NotNull]
        private readonly IContextFactory contextFactory;

        [NotNull]
        private readonly IInstanceFactory instanceFactory;

        [NotNull]
        private readonly IQueryficationBuilder queryficationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryfyDotNet"/> class.
        /// </summary>
        /// <param name="contextFactory">An implementation of the <see cref="IContextFactory"/> interface.</param>
        /// <param name="instanceFactory">An implementation of the <see cref="IInstanceFactory"/> interface.</param>
        /// <param name="queryficationBuilder">An implementation of the <see cref="IQueryficationBuilder"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="contextFactory"/>', '<paramref name="instanceFactory"/>' and '<paramref name="queryficationBuilder"/>' cannot be null. </exception>
        public QueryfyDotNet([NotNull] IContextFactory contextFactory, [NotNull] IInstanceFactory instanceFactory, [NotNull] IQueryficationBuilder queryficationBuilder)
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException(nameof(contextFactory));
            }

            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            if (queryficationBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryficationBuilder));
            }

            this.contextFactory = contextFactory;
            this.instanceFactory = instanceFactory;
            this.queryficationBuilder = queryficationBuilder;
        }

        /// <summary>
        /// Creates a query from the supplied source object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>An <see cref="QueryficationResult"/> instance with information about the result.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' cannot be null. </exception>
        public QueryficationResult Queryfy(Object source)
        {
            var queryficationContext = this.CreateQueryficationContext();
            return this.Queryfy(source, queryficationContext);
        }

        /// <summary>
        /// Creates a query from the supplied <see cref="IQueryficationContext"/>.
        /// </summary>
        /// <returns>An <see cref="QueryficationResult"/> instance with information about the result.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryficationContext"/>' cannot be null. </exception>
        public QueryficationResult Queryfy(IQueryficationContext queryficationContext)
        {
            if (queryficationContext == null)
            {
                throw new ArgumentNullException(nameof(queryficationContext));
            }

            return this.queryficationBuilder.Queryfy(queryficationContext);
        }

        /// <summary>
        /// Creates a new instance of the implementation of the <see cref="IQueryficationContext"/> interface.
        /// </summary>
        /// <returns>The newly created <see cref="IQueryficationContext"/>.</returns>
        public IQueryficationContext CreateQueryficationContext()
        {
            return this.contextFactory.CreateQueryficationContext();
        }

        /// <summary>
        /// Creates a new instance of the implementation of the <see cref="IParserContext"/> interface.
        /// </summary>
        /// <returns>The newly created <see cref="IParserContext"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="query"/>' cannot be null. </exception>
        public IParserContext CreateParserContext(String query)
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            return this.contextFactory.CreateParserContext(query);
        }

        /// <summary>
        /// Creates a query fromt he supplied source <see cref="Object"/> and <see cref="IQueryficationContext"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="queryficationContext">The <see cref="IQueryficationContext"/>.</param>
        /// <returns>An <see cref="QueryficationResult"/> instance with information about the result.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="queryficationContext"/>' cannot be null. </exception>
        public QueryficationResult Queryfy(Object source, IQueryficationContext queryficationContext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (queryficationContext == null)
            {
                throw new ArgumentNullException(nameof(queryficationContext));
            }

            queryficationContext.Import(source);
            return this.Queryfy(queryficationContext);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Type"/> and fills it with the supplied mappings in the <see cref="IParserContext"/>.
        /// </summary>
        /// <param name="type">The destination.</param>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' and '<paramref name="parserContext"/>' cannot be null. </exception>
        /// <exception cref="CannotCreateInstanceOfTypeException">If no instance could be created.</exception>
        public Object Parse(Type type, IParserContext parserContext)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            var instance = this.instanceFactory.CreateInstance(type);
            this.Parse(instance, parserContext);
            return instance;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Type"/> and fills it by using the query string.
        /// </summary>
        /// <param name="type">The destination.</param>
        /// <param name="query">The query string.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="query"/>' and '<paramref name="type"/>' cannot be null. </exception>
        /// <exception cref="CannotCreateInstanceOfTypeException">If no instance could be created.</exception>
        public Object Parse(Type type, String query)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            var instance = this.instanceFactory.CreateInstance(type);
            this.Parse(instance, query);
            return instance;
        }

        /// <summary>
        /// Fills the destination object by using the supplied query string.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="query">The query string.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="query"/>' and '<paramref name="destination"/>' cannot be null. </exception>
        public void Parse(Object destination, String query)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            var parserContext = this.CreateParserContext(query);
            this.Parse(destination, parserContext);
        }

        /// <summary>
        /// Fills the destination object by using the supplied mappings in the <see cref="IParserContext"/>.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="destination"/>' and ''<paramref name="parserContext"/>' cannot be null. </exception>
        public void Parse(Object destination, IParserContext parserContext)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            parserContext.CreateMaps(destination);
            this.queryficationBuilder.Parse(parserContext);
        }

        /// <summary>
        /// Fills the destination object by using the supplied mappings in the <see cref="IParserContext"/>.
        /// </summary>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parserContext"/>' cannot be null. </exception>
        public void Parse(IParserContext parserContext)
        {
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            this.queryficationBuilder.Parse(parserContext);
        }
    }
}