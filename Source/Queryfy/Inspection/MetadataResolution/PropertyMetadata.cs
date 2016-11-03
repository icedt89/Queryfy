namespace JanHafner.Queryfy.Inspection.MetadataResolution
{
    using System;
    using System.Reflection;
    using Attributes;
    using Extensions;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// The <see cref="PropertyMetadata"/> class.
    /// </summary>
    public class PropertyMetadata : IPropertyMetadata
    {
        [CanBeNull]
        private readonly QueryParameterAttribute queryParameterAttribute;

        [NotNull]
        private readonly PropertyInfo propertyInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMetadata"/> class.
        /// </summary>
        /// <param name="queryParameterAttribute">The <see cref="QueryParameterAttribute"/>.</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyInfo" />' cannot be null. </exception>
        public PropertyMetadata([CanBeNull] QueryParameterAttribute queryParameterAttribute, [NotNull] PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));    
            }

            this.queryParameterAttribute = queryParameterAttribute;
            this.propertyInfo = propertyInfo;
        }

        /// <summary>
        /// Gets or sets a value indicating, that an attempt should be made to instantiate the property if a <see langword="null"/> value would be retrieved.
        /// </summary>
        /// <value><see langword="true"/> if it should be tried to instantiate the property if no <see langword="null"/> would be retrieved; otherwise, <see langword="false"/>.</value>
        public virtual Boolean TryInstantiateOnNull
        {
            get
            {
                if (this.queryParameterAttribute != null)
                {
                    return this.queryParameterAttribute.InstantiateOnSkip;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see langword="this"/> instance is a parameter group.
        /// </summary>
        /// <value>
        ///     <see langword="true"/> if this instance is a parameter group; otherwise, <see langword="false"/>.
        /// </value>
        public virtual Boolean IsParameterGroup
        {
            get
            {
                var result = !this.propertyInfo.PropertyType.IsPrimitive() && !this.propertyInfo.PropertyType.IsIEnumerable() && !this.propertyInfo.PropertyType.IsLazy();
                if (this.queryParameterAttribute != null)
                {
                    return this.queryParameterAttribute.ValueProcessorType == null && result;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public virtual String ParameterName
        {
            get
            {
                if (this.queryParameterAttribute != null && !string.IsNullOrWhiteSpace(this.queryParameterAttribute.ParameterName))
                {
                    return this.queryParameterAttribute.ParameterName;
                }

                return this.propertyInfo.Name;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see langword="this"/> instance is ignored on parsing.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if <see langword="this"/> instance is ignored on parsing; otherwise, <see langword="false"/>.
        /// </value>
        public virtual Boolean IsIgnoredOnParse
        {
            get
            {
                if (this.queryParameterAttribute != null)
                {
                    return this.queryParameterAttribute.IgnoreOnParse || !this.propertyInfo.CanWrite;
                }

                return !this.propertyInfo.CanWrite;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="IValueProcessor"/>.
        /// </summary>
        /// <value>The <see cref="Type"/> of the <see cref="IValueProcessor"/>.</value>
        public virtual Type ValueProcessorType
        {
            get
            {
                if (this.queryParameterAttribute != null)
                {
                    return this.queryParameterAttribute.ValueProcessorType;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see langword="this"/> instance is ignored on queryfy.
        /// </summary>
        /// <value>
        /// 	<see langword="true"/> if <see langword="this"/> instance is ignored on queryfy; otherwise, <see langword="falses"/>.
        /// </value>
        public virtual Boolean IsIgnoredOnQueryfy
        {
            get
            {
                if (this.queryParameterAttribute != null)
                {
                    return this.queryParameterAttribute.IgnoreOnQueryfy || !this.propertyInfo.CanRead;
                }

                return !this.propertyInfo.CanRead;
            }
        }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/>.
        /// </summary>
        public virtual PropertyInfo Property
        {
            get { return this.propertyInfo; }
        }
    }
}