namespace JanHafner.Queryfy.Processing
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="IInstanceFactory"/> interface provides a common way of creating instances.
    /// </summary>
    public interface IInstanceFactory
    {
        /// <summary>
        /// Tries to create an instance of the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> from which an istance must be created.</param>
        /// <param name="result"><c>Null</c> or an instance of the expected <see cref="Type"/>.</param>
        /// <returns><c>true</c> if the creation of the instance was successful; otherwise, <c>false</c>.</returns>
        Boolean TryCreateInstance([NotNull] Type type, [CanBeNull] out Object result);
    }
}