namespace JanHafner.Queryfy.Fluent.FluentQueryfy
{
    using System;
    using Inspection;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// Provides fluent syntax for configuring a <see cref="QueryParameter"/>.
    /// </summary>
    public interface IFluentQueryParameter
    {
        /// <summary>
        /// Sets the supplied value.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>Returns self.</returns>
        [NotNull]
        IFluentQueryParameter Value([NotNull] Object value);

        /// <summary>
        /// Sets the supplied name.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>Returns self.</returns>
        [NotNull]
        IFluentQueryParameter Named([NotNull] String name);

        /// <summary>
        /// Sets the supplied <see cref="IValueProcessor"/>.
        /// </summary>
        /// <param name="valueProcessor">The <see cref="IValueProcessor"/>.</param>
        /// <returns>Returns self.</returns>
        [NotNull]
        IFluentQueryParameter ProcessUsing([NotNull] IValueProcessor valueProcessor);

        /// <summary>
        /// Adds the parameter to the context.
        /// </summary>
        /// <returns>Returns the parent.</returns>
        [NotNull]
        IFluentQueryfy Add();
    }
}