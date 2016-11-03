namespace JanHafner.Queryfy.Processing
{
    using System;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// Calls on the supplied object <see cref="object.ToString"/> or uses UniversalCast (Toolkit.Common) to convert the <see cref="string"/> back to the object.
    /// </summary>
    public sealed class DefaultValueProcessor : IValueProcessor
    {
        /// <summary>
        /// Converts the object to the query parameter value.
        /// </summary>
        /// <param name="processingContext">The <see cref="QueryficationProcessingContext"/> which holds information about the current value.</param>
        /// <returns>The <see cref="string"/> used in the query.</returns>
        /// <exception cref="ArgumentNullException">The value of 'processingContext' cannot be null. </exception>
        public String Process(QueryficationProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            return processingContext.SourceValue.ToString();
        }

        /// <summary>
        /// Converts the query parameter value to an object.
        /// </summary>
        /// <param name="processingContext">The <see cref="processingContext"/> which holds information about the current value.</param>
        /// <returns>The newly created object.</returns>
        /// <exception cref="ArgumentNullException">The value of 'processingContext' cannot be null. </exception>
        public Object Resolve(ParserProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            return Convert.ChangeType(processingContext.SourceValue, processingContext.DestinationType);
        }

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.
        /// </returns>
        public Boolean CanHandle([CanBeNull] Type type)
        {
            return true;
        }
    }
}