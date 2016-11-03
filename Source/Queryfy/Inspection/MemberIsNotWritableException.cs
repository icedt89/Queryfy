namespace JanHafner.Queryfy.Inspection
{
    using System;
    using System.Reflection;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;
    using Properties;

    /// <summary>
    /// The MemberIsNotWritableException type.
    /// </summary>
    [Serializable]
    public sealed class MemberIsNotWritableException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberIsNotWritableException"/> class.
        /// </summary>
        /// <param name="memberInfo">The <see cref="MemberInfo"/>.</param>
        public MemberIsNotWritableException([NotNull] MemberInfo memberInfo) : base(String.Format(ExceptionMessages.MemberInfoIsNotWritableExceptionMessage, memberInfo.Name, memberInfo.DeclaringType.Name))
        {
            this.MemberInfo = memberInfo;
        }

       /// <summary>
        /// Initializes a new instance of the <see cref="MemberIsNotWritableException"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
       // ReSharper disable once NotNullMemberIsNotInitialized
        protected MemberIsNotWritableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// The <see cref="MemberInfo"/> which was not writable.
        /// </summary>
        [NotNull]
        public MemberInfo MemberInfo { get; private set; }
    }
}