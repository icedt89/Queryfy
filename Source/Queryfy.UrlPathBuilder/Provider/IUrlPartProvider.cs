namespace JanHafner.Queryfy.UrlPathBuilder.Provider
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Provides methods for constructing an <see cref="UrlPart"/> from an <see cref="object"/>.
    /// </summary>
    public interface IUrlPartProvider
    {
        /// <summary>
        /// Constrcucts the part based on the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="source">The current <see cref="Object"/>.</param>
        /// <param name="currentInheritedType">The base <see cref="Type"/> of the current source <see cref="object"/>.</param>
        /// <param name="deep">The current deep of the recursion.</param>
        /// <returns>The constructed <see cref="UrlPart"/>.</returns>
        [NotNull]
        UrlPart ConstructPart([NotNull] Object source, [NotNull] Type currentInheritedType, Int32 deep);
    }
}