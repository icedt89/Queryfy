namespace JanHafner.Queryfy.Parsing
{
    using Inspection;
    using JetBrains.Annotations;

    /// <summary>
    /// Instructs the <see cref="IQueryficationEngine"/> to execute user defined code after the call to <see cref="IQueryficationEngine.CreateMaps"/>.
    /// </summary>
    public interface INeedExtendedParsing
    {
        /// <summary>
        /// Executes user defined code to manipulate the <see cref="IParserContext"/>.
        /// </summary>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        void Parse([NotNull] IParserContext parserContext);
    }
}