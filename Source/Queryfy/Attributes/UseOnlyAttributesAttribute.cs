namespace JanHafner.Queryfy.Attributes
{
    using System;
    using JanHafner.Toolkit.Common.Reflection;

    /// <summary>
    /// Instructs the <see cref="IPropertyReflector"/> to query only properties which have the <see cref="QueryParameterAttribute"/>-Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public sealed class UseOnlyAttributesAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UseOnlyAttributesAttribute"/> class and supplies <see langword="true"/>.
        /// </summary>
        public UseOnlyAttributesAttribute() : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UseOnlyAttributesAttribute"/> class.
        /// </summary>
        /// <param name="useOnlyAttributes">If set to <see langword="true"/> the <see cref="IPropertyReflector"/> should only query properties which have the <see cref="QueryParameterAttribute"/>.</param>
        public UseOnlyAttributesAttribute(Boolean useOnlyAttributes)
        {
            this.UseOnlyAttributes = useOnlyAttributes;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="IPropertyReflector"/> should only query properties which have the <see cref="QueryParameterAttribute"/>.
        /// </summary>
        /// <value>
        ///   <see langword="true"/> if the <see cref="IPropertyReflector"/> should only query properties which have the <see cref="QueryParameterAttribute"/>; otherwise, <see langword="false"/>.
        /// </value>
        public Boolean UseOnlyAttributes { get; private set; }
    }
}