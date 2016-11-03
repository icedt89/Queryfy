namespace JanHafner.Queryfy.UrlPathBuilder
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="UrlPartAttribute"/> class provides a declarative way of defining <see cref="Uri"/>-parts in the inheritance hirarchy.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class UrlPartAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlPartAttribute"/> class.
        /// </summary>
        /// <param name="partName">The name of the part.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="partName"/>' cannot be null. </exception>
        public UrlPartAttribute([NotNull] String partName)
        {
            if (String.IsNullOrWhiteSpace(partName))
            {
                throw new ArgumentNullException(nameof(partName));    
            }

            this.PartName = partName;
        }

        /// <summary>
        /// Gets the name of the part.
        /// </summary>
        [NotNull]
        public String PartName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to ignore this part in the final <see cref="Uri"/>.
        /// </summary>
        public Boolean IsIgnored { get; set; }
    }
}