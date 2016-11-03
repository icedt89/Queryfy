namespace JanHafner.Queryfy.Inspection.MetadataResolution
{
    using System;
    using System.Reflection;
    using Attributes;
    using JetBrains.Annotations;

    /// <summary>
    /// Provides its meta data from the DataAnnotations defined <see cref="Type"/>.
    /// </summary>
    public sealed class DataAnnotationsPropertyMetadata : PropertyMetadata
    {
        [NotNull]
        private readonly PropertyInfo realPropertyInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAnnotationsPropertyMetadata"/> class.
        /// </summary>
        /// <param name="queryParameterAttribute">The <see cref="QueryParameterAttribute"/>.</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> from which the base class should retrieve its information about the property.</param>
        /// <param name="realPropertyInfo">The real <see cref="PropertyInfo"/> for further processing.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyInfo" />' and '<paramref name="realPropertyInfo" />' cannot be null. </exception>
        public DataAnnotationsPropertyMetadata(QueryParameterAttribute queryParameterAttribute, [NotNull] PropertyInfo propertyInfo, [NotNull] PropertyInfo realPropertyInfo)
            : base(queryParameterAttribute, propertyInfo)
        {
            if (realPropertyInfo == null)
            {
                throw new ArgumentNullException(nameof(realPropertyInfo));    
            }

            this.realPropertyInfo = realPropertyInfo;
        }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/>.
        /// </summary>
        public override PropertyInfo Property
        {
            get { return this.realPropertyInfo; }
        }
    }
}