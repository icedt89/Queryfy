namespace JanHafner.Queryfy
{
    using System;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// Defines methods that encapsulates the main functionality of this framework.
    /// </summary>
    public interface IQueryfyDotNet
    {
        /// <summary>
        /// Creates a query from the supplied <see cref="IQueryficationContext"/>.
        /// </summary>
        /// <returns>An <see cref="QueryficationResult"/> instance with information about the result.</returns>
        [NotNull]
        QueryficationResult Queryfy([NotNull] IQueryficationContext queryficationContext);

        /// <summary>
        /// Creates a new instance of the implementation of the <see cref="IQueryficationContext"/> interface.
        /// </summary>
        /// <returns>The newly created <see cref="IQueryficationContext"/>.</returns>
        [NotNull]
        IQueryficationContext CreateQueryficationContext();

        /// <summary>
        /// Creates a query from the supplied source object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>An <see cref="QueryficationResult"/> instance with information about the result.</returns>
        [NotNull]
        QueryficationResult Queryfy([NotNull] Object source);

        /// <summary>
        /// Creates a new instance of the implementation of the <see cref="IParserContext"/> interface.
        /// </summary>
        /// <returns>The newly created <see cref="IParserContext"/>.</returns>
        [NotNull]
        IParserContext CreateParserContext([NotNull] String query);

        /// <summary>
        /// Creates a query fromt he supplied source <see cref="Object"/> and <see cref="IQueryficationContext"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="queryficationContext">The <see cref="IQueryficationContext"/>.</param>
        /// <returns>An <see cref="QueryficationResult"/> instance with information about the result.</returns>
        [NotNull]
        QueryficationResult Queryfy([NotNull] Object source, [NotNull] IQueryficationContext queryficationContext);

        /// <summary>
        /// Fills the destination object by using the supplied query string.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="query">The query string.</param>
        void Parse([NotNull] Object destination, [NotNull] String query);

        /// <summary>
        /// Constructs a new instance of the <see cref="Type"/> and fills it with the supplied mappings in the <see cref="IParserContext"/>.
        /// </summary>
        /// <param name="type">The destination.</param>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        [NotNull]
        Object Parse([NotNull] Type type, [NotNull] IParserContext parserContext);

        /// <summary>
        /// Constructs a new instance of the <see cref="Type"/> and fills it by using the query string.
        /// </summary>
        /// <param name="type">The destination.</param>
        /// <param name="query">The query string.</param>
        [NotNull]
        Object Parse([NotNull] Type type, [NotNull] String query);

        /// <summary>
        /// Fills the destination object by using the supplied mappings in the <see cref="IParserContext"/>.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        void Parse([NotNull] Object destination, [NotNull] IParserContext parserContext);

        /// <summary>
        /// Fills the destination object by using the supplied mappings in the <see cref="IParserContext"/>.
        /// </summary>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        void Parse([NotNull] IParserContext parserContext);
    }
}