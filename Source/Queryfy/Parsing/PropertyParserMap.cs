namespace JanHafner.Queryfy.Parsing
{
    using System;
    using System.Reflection;
    using Inspection;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="PropertyParserMap"/> defines the mapping between a <see cref="PropertyInfo"/> and a <see cref="QueryParameter"/>.
    /// </summary>
    public sealed class PropertyParserMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyParserMap"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <param name="parameter">The parameter.</param>
        /// <exception cref="MemberIsNotWritableException">The CanWrite property of the propertyInfo parameter returned <c>false</c>.</exception>
        /// <exception cref="PropertyNotFoundException">The propertyInfo parameter is not declared on the source.</exception>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>', '<paramref name="propertyInfo"/>' and '<paramref name="parameter"/>' cannot be null. </exception>
        public PropertyParserMap([NotNull] Object source, [NotNull] PropertyInfo propertyInfo, [NotNull] QueryParameter parameter)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (!propertyInfo.CanWrite)
            {
                throw new MemberIsNotWritableException(this.Property);
            }

            if (propertyInfo.DeclaringType != source.GetType())
            {
                throw new PropertyNotFoundException(propertyInfo.Name, source.GetType());
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            this.Source = source;
            this.Property = propertyInfo;
            this.Parameter = parameter;
            this.Parameter.TypeOfValue = propertyInfo.PropertyType;
        }

        /// <summary>
        /// Gets the source value.
        /// </summary>
        [NotNull]
        public Object Source { get; private set; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/>.
        /// </summary>
        [NotNull]
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Gets the <see cref="QueryParameter"/>.
        /// </summary>
        [NotNull]
        public QueryParameter Parameter { get; private set; }
    }
}