namespace JanHafner.Queryfy.Processing
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;
    using Properties;

    /// <summary>
    /// The ValueProcessorCannotHandleTypeException type.
    /// </summary>
    [Serializable]
    public sealed class ValueProcessorCannotHandleTypeException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueProcessorCannotHandleTypeException"/> class.
        /// </summary>
        /// <param name="valueProcessor">The <see cref="IValueProcessor"/> which was not able to process the object.</param>
        /// <param name="type">The type which was not handled.</param>
        public ValueProcessorCannotHandleTypeException([NotNull] IValueProcessor valueProcessor, [NotNull] Type type)
            : base(String.Format(ExceptionMessages.ValueProcessorCannotHandleTypeExceptionMessage, type.Name, valueProcessor.GetType().Name))
        {
            this.ValueProcessor = valueProcessor;
            this.Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueProcessorCannotHandleTypeException"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        // ReSharper disable once NotNullMemberIsNotInitialized
        protected ValueProcessorCannotHandleTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// The <see cref="IValueProcessor"/> which was not able to process the object.
        /// </summary>
        [NotNull]
        public IValueProcessor ValueProcessor { get; private set; }

        /// <summary>
        /// The <see cref="Type"/> which was not handled.
        /// </summary>
        [NotNull]
        public Type Type { get; private set; }
    }
}