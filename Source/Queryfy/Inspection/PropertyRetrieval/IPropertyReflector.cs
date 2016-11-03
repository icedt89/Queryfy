namespace JanHafner.Queryfy.Inspection.PropertyRetrieval
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IPropertyReflector"/> interface provides methods for reflecting the properties of the supplied <see cref="object"/>.
    /// </summary>
    [PublicAPI]
    public interface IPropertyReflector
    {
        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> from the supplied instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>An <see cref="IEnumerable{PropertyInfo}"/>.</returns>
        [PublicAPI]
        [NotNull]
        [LinqTunnel]
        IEnumerable<PropertyInfo> ReflectProperties([NotNull] Object instance);
    }
}