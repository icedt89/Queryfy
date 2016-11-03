namespace JanHafner.Queryfy.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Attributes;
    using JetBrains.Annotations;
    using Properties;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// The <see cref="ReflectionExtensions"/> class provides extension methods useful in reflection scenarios.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Checks if the supplied <see cref="Type"/> is a primitive <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>Returns <c>true</c> if the supplied <see cref="Type"/> is type.IsPrimitive or type == typeof(String) or type == typeof(Decimal) or type == typeof(Guid).</returns>
        /// <exception cref="ArgumentNullException">The value of '<param name="type"></param>' cannot be null. </exception>
        public static Boolean IsPrimitive([NotNull] this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsPrimitive || type == typeof (String) || type.IsEnum || type == typeof (Decimal) || type == typeof (Guid);
        }

        /// <summary>
        /// Casts the supplied <see cref="MatchCollection"/> the an <see cref="IEnumerable{Match}"/>.
        /// </summary>
        /// <param name="matches">The <see cref="MatchCollection"/>.</param>
        /// <returns>An <see cref="IEnumerable{Match}"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="matches"/>' cannot be null. </exception>
        /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type <see cref="Match"/>.</exception>
        [LinqTunnel]
        internal static IEnumerable<Match> AsEnumerable([NotNull] this MatchCollection matches)
        {
            if (matches == null)
            {
                throw new ArgumentNullException(nameof(matches));
            }

            return matches.Cast<Match>();
        }

        /// <summary>
        /// Checks if the supplied <see cref="Type"/> is an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns><c>true</c> if the <see cref="Type"/> is a generic <see cref="IEnumerable{T}"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' cannot be null. </exception>
        public static Boolean IsGenericIEnumerable([NotNull] this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsIEnumerable() && !type.IsPrimitive())
            {
                return type.IsGenericType;
            }

            return false;
        }

        /// <summary>
        /// Checks if the supplied <see cref="Type"/> is a <see cref="Lazy{T}"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns><c>true</c> if the supplied <see cref="Type"/> is a <see cref="Lazy{T}"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' cannot be null. </exception>
        public static Boolean IsLazy([NotNull] this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Lazy<>);
        }

        /// <summary>
        /// Checks if the supplied <see cref="Type"/> is a <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns><c>true</c> if the supplied <see cref="Type"/> is a <see cref="Nullable{T}"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' cannot be null. </exception>
        public static Boolean IsNullable([NotNull] this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
        }

        /// <summary>
        /// Checks if the supplied <see cref="Type"/> is assignable to <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns><c>true</c> if the supplied <see cref="Type"/> is assignable to <see cref="IEnumerable"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' cannot be null. </exception>
        public static Boolean IsIEnumerable([NotNull] this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return typeof (IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// Gets the value of the <see cref="Enum"/> based on the name of the value.
        /// </summary>
        /// <param name="valueName">The value name.</param>
        /// <typeparam name="TEnum">The <see cref="Type"/> of the <see cref="Enum"/>.</typeparam>
        /// <returns>The value of the <see cref="Enum"/> castet to the needed <see cref="Type"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="valueName"/>' cannot be null. </exception>
        /// <exception cref="EnumValueNameAttributeNotFoundException">The EnumValueNameAttribute-attribute was not found on the supplied member '<paramref name="valueName"/>' of enum '<typeparam name="TEnum" />'.</exception>
        /// <exception cref="MissingMemberException">There is no enum-member whith the specified name.</exception>
        /// <exception cref="ArgumentException"><typeparam name="TEnum" /> is not an <see cref="T:System.Enum" />.-or- <paramref name="valueName" /> is either an empty string or only contains white space.-or- <paramref name="valueName" /> is a name, but not one of the named constants defined for the enumeration. </exception>
        /// <exception cref="InvalidOperationException">The enum '<typeparam name="TEnum" />' does not contain any members. </exception>
        /// <exception cref="OverflowException"><paramref name="valueName" /> is outside the range of the underlying type of <typeparam name="TEnum" />.</exception>
        [NotNull]
        public static TEnum GetEnumMemberByValueName<TEnum>([NotNull] this String valueName)
        {
            return (TEnum)(object)valueName.GetEnumMemberByValueName(typeof(TEnum));
        }

        /// <summary>
        /// Gets the value of the <see cref="Enum"/> based on the name of the value.
        /// </summary>
        /// <param name="valueName">The value name.</param>
        /// <param name="enumType">The <see cref="Type"/> of the <see cref="Enum"/>.</param>
        /// <returns>The value of the <see cref="Enum"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="valueName"/>' and '<paramref name="enumType"/>' cannot be null. </exception>
        /// <exception cref="EnumValueNameAttributeNotFoundException">The EnumValueNameAttribute-attribute was not found on the supplied member '<paramref name="valueName"/>' of enum '<paramref name="enumType"/>'.</exception>
        /// <exception cref="MissingMemberException">There is no enum-member whith the specified name.</exception>
        /// <exception cref="ArgumentException"><paramref name="enumType" /> is not an <see cref="T:System.Enum" />.-or- <paramref name="valueName" /> is either an empty string or only contains white space.-or- <paramref name="valueName" /> is a name, but not one of the named constants defined for the enumeration. </exception>
        /// <exception cref="InvalidOperationException">The enum '<paramref name="enumType"/>' does not contain any members. </exception>
        /// <exception cref="OverflowException"><paramref name="valueName" /> is outside the range of the underlying type of <paramref name="enumType" />.</exception>
        [NotNull]
        public static Enum GetEnumMemberByValueName(this String valueName, [NotNull] Type enumType)
        {
            if (String.IsNullOrWhiteSpace(valueName))
            {
                throw new ArgumentNullException(nameof(valueName));
            }

            if (enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            var enumValues = Enum.GetValues(enumType);
            foreach (var enumValue in enumValues)
            {
                var typedEnumValue = (Enum) Enum.Parse(enumType, enumValue.ToString());
                var methodTypeNameAttribute = typedEnumValue.GetAttributeFromEnumMember<EnumValueNameAttribute>();
                if (methodTypeNameAttribute == null)
                {
                    throw new EnumValueNameAttributeNotFoundException(valueName, enumType);
                }

                if (methodTypeNameAttribute.ParameterValue == valueName)
                {
                    return typedEnumValue;
                }
            }

            throw new InvalidOperationException(String.Format(ExceptionMessages.EnumDoesNotContainMembersExceptionMessage, enumType.Name));
        }

        /// <summary>
        /// Gets the only one generic parameter the <see cref="Type"/> is supposed to have.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>The <see cref="Type"/> of the generic parameter.</returns>
        /// <exception cref="ArgumentNullException">The value of 'type' cannot be null. </exception>
        /// <exception cref="ArgumentException">The provided <see cref="Type"/> is not a generic <see cref="Type"/>. Or The provided <see cref="Type"/> provides more than one generic argument.</exception>
        /// 
        [NotNull]
        public static Type GetSingleGenericParameter([NotNull] this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!type.IsGenericType)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.ProvidedTypeIsNotGenericExceptionMessage, type.Name));
            }

            var genericArguments = type.GetGenericArguments();
            if (genericArguments.Length > 1)
            {
                throw new ArgumentException(String.Format(ExceptionMessages.MoreThanOneGenericTypeParameterFoundExceptionMessages, type.Name, String.Join(", ", genericArguments.Select(n => n.Name))));
            }

            return genericArguments[0];
        }

        /// <summary>
        /// Gets the supplied value from the supplied <see cref="Object"/> of the <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <exception cref="NotSupportedException">Indexed parameters are not supported.</exception>
        /// <param name="instance">The source object.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyInfo"/>' cannot be null. </exception>
        [UsedImplicitly]
        [NotNull]
        internal static Object GetValue([NotNull] this PropertyInfo propertyInfo, [CanBeNull] Object instance)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (propertyInfo.GetIndexParameters().Length > 0)
            {
                throw new NotSupportedException(ExceptionMessages.IndexedParametersAreNotSupportedExceptionMessage);
            }

            return propertyInfo.GetValue(instance, null);
        }

        /// <summary>
        /// Sets the supplied value on the supplied <see cref="Object"/> on the <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <param name="instance">The source object.</param>
        /// <exception cref="NotSupportedException">Indexed parameters are not supported.</exception>
        /// <param name="value">The value to set.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyInfo"/>' cannot be null. </exception>
        [UsedImplicitly]
        internal static void SetValue([NotNull] this PropertyInfo propertyInfo, [CanBeNull] Object instance, [CanBeNull] Object value)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (propertyInfo.GetIndexParameters().Length > 0)
            {
                throw new NotSupportedException(ExceptionMessages.IndexedParametersAreNotSupportedExceptionMessage);
            }

            propertyInfo.SetValue(instance, value, null);
        }
    }
}