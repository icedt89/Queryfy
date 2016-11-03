namespace JanHafner.Queryfy.Processing
{
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// Provides the core functionality of queryfying and parsing.
    /// </summary>
    public interface IQueryficationBuilder
    {
        /// <summary>
        /// Uses the supplied <see cref="IQueryficationContext"/> to build the final query string.
        /// </summary>
        /// <param name="queryficationContext">The supplied <see cref="IQueryficationContext"/>.</param>
        /// <returns>An <see cref="QueryficationResult"/> that contains information about the output.</returns>
        [NotNull]
        QueryficationResult Queryfy([NotNull] IQueryficationContext queryficationContext);

        /// <summary>
        /// Uses the supplied <see cref="IParserContext"/> to write all values to properties.
        /// </summary>
        /// <param name="parserContext">The supplied <see cref="IParserContext"/>.</param>
        void Parse([NotNull] IParserContext parserContext);
    }
}