namespace JanHafner.Queryfy.Processing
{
    using System;
    using System.Globalization;
    using System.Linq;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// Converts a <see cref="CultureInfo"/> instance to the ISO 689-1 representation.
    /// </summary>
    public sealed class ISO6391ValueProcessor : IValueProcessor
    {
        /// <summary>
        /// Converts the object to the query parameter value.
        /// </summary>
        /// <param name="processingContext">The <see cref="QueryficationProcessingContext"/> which holds information about the current value.</param>
        /// <returns>The <see cref="string"/> used in the query.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="processingContext"/>' cannot be null. </exception>
        public String Process(QueryficationProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            var cultureInfo = (CultureInfo) processingContext.SourceValue;
            return cultureInfo.TwoLetterISOLanguageName;
        }

        /// <summary>
        /// Converts the query parameter value to an object.
        /// </summary>
        /// <param name="processingContext">The <see cref="processingContext"/> which holds information about the current value.</param>
        /// <returns>The newly created object.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="processingContext"/>' cannot be null. </exception>
        public Object Resolve(ParserProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            return CultureInfo.GetCultures(CultureTypes.AllCultures).First(cultureInfo => cultureInfo.TwoLetterISOLanguageName == processingContext.SourceValue);
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
            return type == typeof (CultureInfo);
        }
    }
}