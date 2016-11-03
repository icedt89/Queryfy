namespace JanHafner.Queryfy
{
    using System;
    using System.Runtime.Serialization;
    using Attributes;
    using JetBrains.Annotations;
    using Properties;

    /// <summary>
    /// This exception is thrown if the the member of the <see langword="enum"/> does not contain the <see cref="EnumValueNameAttribute"/>
    /// </summary>
    [Serializable]
    public sealed class EnumValueNameAttributeNotFoundException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryfyDotNetException"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="valueName"/>' and '<paramref name="enumType"/>' cannot be null. </exception>
        public EnumValueNameAttributeNotFoundException([NotNull] String valueName, [NotNull] Type enumType) 
            : base(String.Format(ExceptionMessages.EnumValueNameAttributeNotFoundExceptionMessage, valueName, enumType.Name))
        {
            if (String.IsNullOrEmpty(valueName))
            {
                throw new ArgumentNullException(nameof(valueName));
            }

            this.ValueName = valueName;
            this.EnumType = enumType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValueNameAttributeNotFoundException"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public EnumValueNameAttributeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// The name of the member of the <see langword="enum"/>.
        /// </summary>
        [NotNull]
        public string ValueName { get; private set; }

        /// <summary>
        /// The type of the <see langword="enum"/>.
        /// </summary>
        [NotNull]
        public Type EnumType { get; private set; }
    }
}