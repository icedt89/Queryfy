namespace JanHafner.Queryfy.UrlPathBuilder
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="UrlPart"/> class provides information about an <see cref="Uri"/>-part.
    /// </summary>
    public sealed class UrlPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlPart"/> class.
        /// </summary>
        /// <param name="partName">Name of the part.</param>
        /// <param name="isIgnored">If set to <c>true</c> the part will not display in the final <see cref="Uri"/>.</param>
        /// <param name="order">The order of the <see cref="UrlPart"/> in the final <see cref="Uri"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="partName"/>' cannot be null. </exception>
        public UrlPart([NotNull] String partName, Boolean isIgnored, Int32 order)
        {
            if (String.IsNullOrWhiteSpace(partName))
            {
                throw new ArgumentNullException(nameof(partName));    
            }

            this.PartName = partName;
            this.Order = order;
            this.IsIgnored = isIgnored;
        }

        /// <summary>
        /// Gets the name of the part.
        /// </summary>
        [NotNull]
        public String PartName { get; private set; }

        /// <summary>
        /// If set to <c>true</c> the part will not display in the final <see cref="Uri"/>.
        /// </summary>
        public Boolean IsIgnored { get; private set; }
        
        /// <summary>
        /// Gets the order of the <see cref="UrlPart"/> in the final <see cref="Uri"/>.
        /// </summary>
        public Int32 Order { get; private set; }
    }
}