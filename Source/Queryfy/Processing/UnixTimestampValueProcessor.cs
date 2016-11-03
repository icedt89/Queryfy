namespace JanHafner.Queryfy.Processing
{
    using System;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Converts a <see cref="DateTime"/> to a <see cref="long"/> that represents an Unix-timestamp and vice-versa.
    /// </summary>
    public sealed class UnixTimestampValueProcessor : IValueProcessor
    {
        /// <summary>
        /// Converts the object to the query parameter value.
        /// </summary>
        /// <param name="processingContext">The <see cref="QueryficationProcessingContext"/> which holds information about the current value.</param>
        /// <returns>The <see cref="String"/> used in the query.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="processingContext"/>' cannot be null. </exception>
        public String Process(QueryficationProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            var sourceDateTime = (DateTime)processingContext.SourceValue;
            return sourceDateTime.ToUnixTimestamp().ToString();
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

            var seconds = Int64.Parse(processingContext.SourceValue);
            return seconds.FromUnixTimestamp();
        }

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.
        /// </returns>
        public Boolean CanHandle([CanBeNull] Type type)
        {
            return type == typeof (DateTime);
        }
    }
}