namespace JanHafner.Queryfy.Fluent.FluentParser
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// Provides method for the fluent parser syntax.
    /// </summary>
    public interface IFluentParserMap
    {
        /// <summary>
        /// Binds the map to the supplied parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>Returns self.</returns>
        [NotNull]
        IFluentParserMap Named([NotNull] String name);

        /// <summary>
        /// Binds the map to the supplied <see cref="IValueProcessor"/>.
        /// </summary>
        /// <param name="valueProcessor">The <see cref="IValueProcessor"/>.</param>
        /// <returns>Returns self.</returns>
        [NotNull]
        IFluentParserMap ProcessUsing([NotNull] IValueProcessor valueProcessor);

        /// <summary>
        /// Binds the source object and <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="lambdaExpression">The <see cref="LambdaExpression"/>.</param>
        /// <returns>Returns self.</returns>
        [NotNull]
        IFluentParserMap BindTo([NotNull] Object source, [NotNull] LambdaExpression lambdaExpression);

        /// <summary>
        /// Adds the map to the context.
        /// </summary>
        /// <returns>Returns the parent.</returns>
        [NotNull]
        IFluentParser Map();
    }
}