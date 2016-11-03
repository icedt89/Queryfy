namespace JanHafner.Queryfy.Processing
{
    using System;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// Provides a conversion from the <see cref="Enum"/> to its underlying <see cref="Type"/> and vice-versa.
    /// </summary>
    public sealed class EnumMemberAsUnderlyingTypeValueProcessor : IValueProcessor
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

            var underlyingType = processingContext.SourceValue.GetType().GetEnumUnderlyingType();
            return Convert.ChangeType(processingContext.SourceValue, underlyingType).ToString();
        }

        /// <summary>
        /// Converts the query parameter value to an object.
        /// </summary>
        /// <param name="processingContext">The <see cref="processingContext"/> which holds information about the current value.</param>
        /// <returns>The newly created object.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="processingContext"/>' cannot be null. </exception>
        public Object Resolve(ParserProcessingContext processingContext)
        {
            if(processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            return Enum.Parse(processingContext.DestinationType, processingContext.SourceValue);
        }

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' cannot be null. </exception>
        public Boolean CanHandle(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsEnum;
        }
    }
}