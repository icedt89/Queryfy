namespace JanHafner.Queryfy.Attributes
{
    using System;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// Instructs the <see cref="EnumMemberAsNameValueProcessor"/> to use the supplied value instead of the enum value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumValueNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValueNameAttribute"/> class.
        /// </summary>
        /// <param name="parameterValue">The parameter value.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parameterValue"/>' cannot be null. </exception>
        public EnumValueNameAttribute([NotNull] String parameterValue)
        {
            if (String.IsNullOrEmpty(parameterValue))
            {
                throw new ArgumentNullException(nameof(parameterValue));
            }

            this.ParameterValue = parameterValue;
        }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        /// <value>The parameter value.</value>
        [NotNull]
        public String ParameterValue { get; private set; }
    }
}