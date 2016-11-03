namespace JanHafner.Queryfy.Inspection
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;
    using Processing;
    using Properties;

    /// <summary>
    /// The CannotCreateInstanceOfTypeException class type.
    /// </summary>
    [Serializable]
    public sealed class CannotCreateInstanceOfTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CannotCreateInstanceOfTypeException"/> class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of which no instance could be created.</param>
        /// <param name="instanceFactory">The instance of the <see cref="IInstanceFactory"/> that was not able to create the instance.</param>
        public CannotCreateInstanceOfTypeException([NotNull] Type type, [NotNull] IInstanceFactory instanceFactory)
            : base(String.Format(ExceptionMessages.CannotCreateInstanceOfTypeExceptionMessage, type.Name, instanceFactory.GetType().Name))
        {
            this.Type = type;
            this.InstanceFactory = instanceFactory;
        }

        /// <summary>
        /// Used to deserialize the <see cref="SerializationInfo"/>.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/>.</param>
        /// <param name="context">The <see cref="StreamingContext"/>.</param>
        [UsedImplicitly]
        // ReSharper disable once NotNullMemberIsNotInitialized
        protected CannotCreateInstanceOfTypeException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the <see cref="Type"/> that could not be created.
        /// </summary>
        [NotNull]
        public Type Type { get; private set; }

        /// <summary>
        /// Gets the instance of the <see cref="IInstanceFactory"/> that was not able to create the instance.
        /// </summary>
        [NotNull]
        public IInstanceFactory InstanceFactory { get; private set; }
    }
}