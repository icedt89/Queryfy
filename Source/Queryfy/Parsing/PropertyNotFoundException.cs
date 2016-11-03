namespace JanHafner.Queryfy.Parsing
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;
    using Properties;

    /// <summary>
    /// The PropertyNotFoundException type.
    /// </summary>
    [Serializable]
    public sealed class PropertyNotFoundException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyNotFoundException"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="type">The type.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyName"/>' cannot be null. </exception>
        public PropertyNotFoundException([NotNull] String propertyName, [NotNull] Type type)
            : base(String.Format(ExceptionMessages.PropertyNotFoundExceptionMessage, propertyName, type.Name))
        {
            if (String.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));    
            }

            this.PropertyName = propertyName;
            this.Type = type;
        }

          /// <summary>
        /// Initializes a new instance of the <see cref="PropertyNotFoundException"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
          // ReSharper disable once NotNullMemberIsNotInitialized
        protected PropertyNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the name of the property which was not found.
        /// </summary>
        [NotNull]
        public String PropertyName { get; private set; }

        /// <summary>
        /// Gets the <see cref="Type"/> which should hold the property.
        /// </summary>
        [NotNull]
        public Type Type { get; private set; }
    }
}