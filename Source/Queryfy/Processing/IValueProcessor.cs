namespace JanHafner.Queryfy.Processing
{
    using System;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// The <see cref="IValueProcessor"/> interface provides the way of converting query parameter values to objects and vice-versa.
    /// </summary>
    public interface IValueProcessor
    {
        /// <summary>
        /// Converts the object to the query parameter value.
        /// </summary>
        /// <param name="processingContext">The <see cref="QueryficationProcessingContext"/> which holds information about the current value.</param>
        /// <returns>The <see cref="string"/> used in the query.</returns>
        [CanBeNull]
        String Process([NotNull] QueryficationProcessingContext processingContext);

        /// <summary>
        /// Converts the query parameter value to an object.
        /// </summary>
        /// <param name="processingContext">The <see cref="processingContext"/> which holds information about the current value.</param>
        /// <returns>The newly created object.</returns>
        [CanBeNull]
        Object Resolve([NotNull] ParserProcessingContext processingContext);

        /// <summary>
        /// Determines whether <see langword="this"/> instance can handle the specified type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.
        /// </returns>
        Boolean CanHandle([NotNull] Type type);
    }
}