namespace JanHafner.Queryfy.Inspection
{
    using System;
    using System.Reflection;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// The <see cref="IQueryficationEngine"/> interface provides methods for stepping of the properties of the supplied <see cref="object"/> and fill fully automatic the <see cref="IQueryficationContext"/> or <see cref="IParserContext"/>.
    /// </summary>
    public interface IQueryficationEngine
    {
        /// <summary>
        /// Fills the <see cref="IQueryficationContext"/> with the properties from the supplied <see cref="object"/>.
        /// </summary>
        /// <param name="source">The <see cref="object"/> that gets analyzed.</param>
        /// <param name="queryficationContext">The <see cref="IQueryficationContext"/>.</param>
        void Fill([NotNull] Object source, [NotNull] IQueryficationContext queryficationContext);

        /// <summary>
        /// Fills the <see cref="IParserContext"/> with maps based on properties of the supplied <see cref="Object"/>.
        /// </summary>
        /// <param name="source">The <see cref="Object"/> that gets analyzed.</param>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        void CreateMaps([NotNull] Object source, [NotNull] IParserContext parserContext);

        /// <summary>
        /// Returns a new <see cref="QueryParameter"/> with metadata from the supplied <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <returns>The <see cref="QueryParameter"/>.</returns>
        [NotNull]
        QueryParameter GetParameterFromProperty([NotNull] PropertyInfo propertyInfo);
    }
}