namespace JanHafner.Queryfy.UrlPathBuilder
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IUrlPathBuilder"/> interface provides functionality for converting an inheritance based object to an <see cref="Uri"/>.
    /// </summary>
    public interface IUrlPathBuilder
    {
        /// <summary>
        /// Uses the supplied <see cref="Object"/> for the process.
        /// </summary>
        /// <param name="source">The <see cref="Object"/>.</param>
        /// <returns>A <see cref="String"/> that represents the <see cref="Uri"/>.</returns>
        [NotNull]
        String Build([NotNull] Object source);
    }
}