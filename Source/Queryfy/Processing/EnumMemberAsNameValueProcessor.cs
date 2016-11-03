namespace JanHafner.Queryfy.Processing
{
    using System;
    using Attributes;
    using Extensions;
    using JanHafner.Queryfy.Properties;
    using Toolkit.Common.ExtensionMethods;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// Processes the <see cref="EnumValueNameAttribute"/> and uses the value it provides to read and write the value.
    /// </summary>
    public sealed class EnumMemberAsNameValueProcessor : IValueProcessor
    {
        /// <summary>
        /// Converts the object to the query parameter value.
        /// </summary>
        /// <param name="processingContext">The <see cref="QueryficationProcessingContext"/> which holds information about the current value.</param>
        /// <returns>The <see cref="String"/> used in the query.</returns>
        /// <exception cref="InvalidOperationException">The <see cref="EnumValueNameAttribute"/>-Attribute was not found on the supplied Enum value</exception>
        /// <exception cref="ArgumentNullException">The value of 'processingContext' cannot be null. </exception>
        public String Process(QueryficationProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            var enumMembers = processingContext.SourceValue.GetType().GetMember(processingContext.SourceValue.ToString());
            var methodTypeNameAttribute = enumMembers[0].GetAttribute<EnumValueNameAttribute>();
            if (methodTypeNameAttribute == null)
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.EnumValueNameAttributeNotFoundExceptionMessage, processingContext.SourceValue, processingContext.SourceValue.GetType().Name));
            }

            return methodTypeNameAttribute.ParameterValue;
        }

        /// <summary>
        /// Converts the query parameter value to an object.
        /// </summary>
        /// <param name="processingContext">The <see cref="processingContext"/> which holds information about the current value.</param>
        /// <returns>The newly created object.</returns>
        /// <exception cref="ArgumentNullException">The value of 'processingContext' cannot be null. </exception>
        /// <exception cref="InvalidOperationException">The <see cref="EnumValueNameAttribute"/>-Attribute was not found on the supplied Enum value. Or there is no <see cref="EnumValueNameAttribute"/> with the supplied value name.</exception>
        /// <exception cref="EnumValueNameAttributeNotFoundException">The EnumValueNameAttribute-attribute was not found on the supplied member '<paramref name="valueName"/>' of enum '<paramref name="enumType"/>'.</exception>
        /// <exception cref="MissingMemberException">There is no enum-member whith the specified name.</exception>
        public Object Resolve(ParserProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            return processingContext.SourceValue.GetEnumMemberByValueName(processingContext.DestinationType);
        }

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The value of 'type' cannot be null. </exception>
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