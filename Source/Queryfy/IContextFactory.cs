namespace JanHafner.Queryfy
{
    using System;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// Provides methods for creating <see cref="IQueryficationContext" /> and <see cref="IParserContext"/> instances at runtime.
    /// </summary>
    public interface IContextFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="IQueryficationContext"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="IQueryficationContext"/> class.</returns>
        [NotNull]
        IQueryficationContext CreateQueryficationContext();

        /// <summary>
        /// Creates a new instance of the <see cref="IParserContext"/> class.
        /// </summary>
        /// <param name="query">The query that is used to build this instance.</param>
        /// <returns>A new instance of the <see cref="IParserContext"/> class.</returns>
        [NotNull]
        IParserContext CreateParserContext([NotNull] String query);
    }
}