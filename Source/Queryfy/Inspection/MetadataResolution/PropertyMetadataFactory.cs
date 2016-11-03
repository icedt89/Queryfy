namespace JanHafner.Queryfy.Inspection.MetadataResolution
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// The <see cref="PropertyMetadataFactory"/> class.
    /// </summary>
    public class PropertyMetadataFactory : IPropertyMetadataFactory
    {
        /// <summary>
        /// Gets the <see cref="IPropertyMetadata"/>
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
        /// <returns>The associated <see cref="IPropertyMetadata"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyInfo"/>' cannot be null. </exception>
        /// <exception cref="MissingMemberException">The delaring <see cref="Type"/> of the supplied <see cref="PropertyInfo"/> is annotated with the <see cref="MetadataTypeAttribute"/> but the </exception>
        public virtual IPropertyMetadata GetPropertyMetadata(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));    
            }

            var queryParameterAttribute = propertyInfo.GetAttribute<QueryParameterAttribute>();

            var metadataTypeAttribute = propertyInfo.DeclaringType.GetAttribute<MetadataTypeAttribute>();
            if (metadataTypeAttribute != null)
            {
                var metadataPropertyType = metadataTypeAttribute.MetadataClassType.GetProperty(propertyInfo.Name, propertyInfo.PropertyType, propertyInfo.GetIndexParameters().Select(p => p.ParameterType).ToArray());
                if (metadataPropertyType == null)
                {
                    throw new MissingMemberException(metadataTypeAttribute.MetadataClassType.Name, propertyInfo.Name);    
                }

                return new DataAnnotationsPropertyMetadata(queryParameterAttribute, metadataPropertyType, propertyInfo);
            }

            return new PropertyMetadata(queryParameterAttribute, propertyInfo);
        }
    }
}