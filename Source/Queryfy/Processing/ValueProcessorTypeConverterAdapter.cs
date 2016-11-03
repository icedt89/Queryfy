namespace JanHafner.Queryfy.Processing
{
    using System;
    using System.ComponentModel;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// The <see cref="ValueProcessorTypeConverterAdapter"/> class provides a bridge between the .NET Framework infrastructure and the <see cref="IValueProcessor"/>.
    /// </summary>
    public sealed class ValueProcessorTypeConverterAdapter : IValueProcessor
    {
        [NotNull]
        private readonly TypeConverter typeConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueProcessorTypeConverterAdapter"/> class.
        /// </summary>
        /// <param name="typeConverter">The <see cref="TypeConverter"/> to use during the conversion.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="typeConverter"/>' cannot be null. </exception>
        public ValueProcessorTypeConverterAdapter([NotNull] TypeConverter typeConverter)
        {
            if (typeConverter == null)
            {
                throw new ArgumentNullException(nameof(typeConverter));
            }

            this.typeConverter = typeConverter;
        }

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

            return this.typeConverter.ConvertToString(processingContext.SourceValue);
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

            return this.typeConverter.ConvertFromString(processingContext.SourceValue);
        }

        /// <summary>
        /// Determines whether <see langword="this"/> instance can handle the specified type.
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

            return this.typeConverter.CanConvertTo(type) && this.typeConverter.CanConvertFrom(typeof (String));
        }
    }
}