namespace JanHafner.Queryfy.Attributes
{
    using System;
    using Processing;

    /// <summary>
    /// Instructs the <see cref="IValueProcessor"/> to use the <see cref="EnumMemberAsNameValueProcessor"/> for the processing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class UseEnumValueNameAttribute : Attribute
    {
    }
}