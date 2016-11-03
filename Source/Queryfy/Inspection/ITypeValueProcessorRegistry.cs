namespace JanHafner.Queryfy.Inspection
{
    using System;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// The <see cref="ITypeValueProcessorRegistry"/> provides functionality to map a <see cref="Type"/> to an <see cref="IValueProcessor"/>.
    /// </summary>
    public interface ITypeValueProcessorRegistry
    {
        /// <summary>
        /// Lookup the mapped <see cref="IValueProcessor"/> for the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> for which the mapped instance should be returned.</param>
        /// <param name="result">The <see cref="IValueProcessor"/></param>
        /// <returns><c>true</c> if the mapping was found; otherwise, <c>false</c>.</returns>
        Boolean TryLookup([NotNull] Type type, [CanBeNull] out IValueProcessor result);

        /// <summary>
        /// Maps the <see cref="IValueProcessor"/> to the <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <param name="valueProcessor">The <see cref="IValueProcessor"/>.</param>
        void SetRegistration([NotNull] Type type, [NotNull] IValueProcessor valueProcessor);

        /// <summary>
        /// Removes the mapping for the suppied <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        void RemoveRegistration([NotNull] Type type);
    }
}