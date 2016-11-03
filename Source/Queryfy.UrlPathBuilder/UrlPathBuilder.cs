namespace JanHafner.Queryfy.UrlPathBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="UrlPathBuilder"/> class.
    /// </summary>
    public sealed class UrlPathBuilder : IUrlPathBuilder
    {
        [NotNull]
        private static readonly Type StopAtType;

        /// <summary>
        /// Initializes the <see cref="UrlPathBuilder"/> class.
        /// </summary>
        static UrlPathBuilder()
        {
            StopAtType = typeof (Object);
        }

        /// <summary>
        /// Uses the supplied <see cref="Object"/> for the process.
        /// </summary>
        /// <param name="source">The <see cref="Object"/>.</param>
        /// <returns>A <see cref="String"/> that represents the <see cref="Uri"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' cannot be null. </exception>
        public String Build(Object source)
        {
            return String.Join(@"/", this.ResolveUrlParts(source).OrderByDescending(uriPart => uriPart.Order).Select(uriPart => uriPart.PartName));
        }

        /// <summary>
        /// Uses reflection on the inheritance hirarchy to build the <see cref="Uri"/>.
        /// </summary>
        /// <param name="source">The <see cref="Object"/>.</param>
        /// <returns>A <see cref="IEnumerable{UrlPart}"/></returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' cannot be null. </exception>
        [LinqTunnel]
        private IEnumerable<UrlPart> ResolveUrlParts([NotNull] Object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var resolvedUriParts = new Stack<UrlPart>();

            this.ResolveUrlPartsCore(resolvedUriParts, source, source.GetType(), 0);

            return resolvedUriParts.Where(uriPart => !uriPart.IsIgnored);
        }

        /// <summary>
        /// Executes the main functionality to resolve the <see cref="UrlPart"/>`s.
        /// </summary>
        /// <param name="resolvedParts">A <see cref="Stack{UriPart}"/> which contains the resolved parts.</param>
        /// <param name="source">The source object.</param>
        /// <param name="currentInheritedType">The current <see cref="Type"/> in the inheritance hirarchy.</param>
        /// <param name="deep">The current deep.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="resolvedParts"/>', '<paramref name="source"/>' and '<paramref name="currentInheritedType"/>' cannot be null. </exception>
        private void ResolveUrlPartsCore([NotNull] Stack<UrlPart> resolvedParts, [NotNull] Object source, [NotNull] Type currentInheritedType, Int32 deep)
        {
            if (resolvedParts == null)
            {
                throw new ArgumentNullException(nameof(resolvedParts));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (currentInheritedType == null)
            {
                throw new ArgumentNullException(nameof(currentInheritedType));
            }

            if (currentInheritedType == StopAtType)
            {
                return;
            }

            var urlPartProvider = UrlPartFactory.GetProvider(currentInheritedType);
            if (urlPartProvider != null)
            {
                var newPart = urlPartProvider.ConstructPart(source, currentInheritedType, deep);
                resolvedParts.Push(newPart);
            }

            this.ResolveUrlPartsCore(resolvedParts, source, currentInheritedType.BaseType, deep + 1);
        }
    }
}