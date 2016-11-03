namespace JanHafner.Queryfy.Fluent.FluentQueryfy
{
    using System;
    using Inspection;
    using JetBrains.Annotations;

    /// <summary>
    /// Provides an entry point for the fluent syntax.
    /// </summary>
    public interface IFluentQueryfy
    {
        /// <summary>
        /// Returns a new parameter with the supplied name.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>A new fluent syntax provider for the <see cref="QueryParameter"/>.</returns>
        [NotNull]
        IFluentQueryParameter Parameter([NotNull] String name);

        /// <summary>
        /// Returns a new parameter.
        /// </summary>
        /// <returns>A new fluent syntax provider for the <see cref="QueryParameter"/>.</returns>
        [NotNull]
        IFluentQueryParameter Parameter();

        /// <summary>
        /// Processes the parameters.
        /// </summary>
        /// <returns>The <see cref="QueryficationResult"/>.</returns>
        [NotNull]
        QueryficationResult Queryfy();
    }
}