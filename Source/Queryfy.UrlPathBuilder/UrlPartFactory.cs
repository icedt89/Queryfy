namespace JanHafner.Queryfy.UrlPathBuilder
{
    using System;
    using JetBrains.Annotations;
    using Provider;

    /// <summary>
    /// Provides static methods for resolving the right <see cref="IUrlPartProvider"/> from the supplied <see cref="Type"/>.
    /// </summary>
    internal static class UrlPartFactory
    {
        /// <summary>
        /// Gets the <see cref="IUrlPartProvider"/> for the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="currentType">The current <see cref="Type"/>.</param>
        /// <returns>An instance of the <see cref="IUrlPartProvider"/> interface.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="currentType"/>' cannot be null. </exception>
        [CanBeNull]
        public static IUrlPartProvider GetProvider([NotNull] Type currentType)
        {
            if (currentType == null)
            {
                throw new ArgumentNullException(nameof(currentType));
            }

            if (typeof (IProvideDynamicPart).IsAssignableFrom(currentType))
            {
                return new DynamicUrlPartProvider();
            }

            var customAttributes = currentType.GetCustomAttributes(typeof (UrlPartAttribute), false);
            if (customAttributes.Length > 0)
            {
                return new AttributeBasedUrlPartProvider();
            }

            return null;
        }
    }
}