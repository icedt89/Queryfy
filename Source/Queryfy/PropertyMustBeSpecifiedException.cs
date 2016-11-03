namespace JanHafner.Queryfy
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;
    using Properties;

    /// <summary>
    /// This exception is thrown when the property of an argument is null.
    /// </summary>
    [Serializable]
    public sealed class PropertyMustBeSpecifiedException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMustBeSpecifiedException"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyName"/>' and '<paramref name="instance"/>' cannot be null. </exception>
        public PropertyMustBeSpecifiedException([NotNull] String propertyName, [NotNull] Object instance) 
            : base(String.Format(ExceptionMessages.PropertyMustBeSpecifiedExceptionMessage, propertyName, instance.GetType()))
        {
            if (String.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            this.PropertyName = propertyName;
            this.Instance = instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMustBeSpecifiedException"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public PropertyMustBeSpecifiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        [NotNull]
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets the instance that caused this exception.
        /// </summary>
        [NotNull]
        public object Instance { get; private set; }
    }
}