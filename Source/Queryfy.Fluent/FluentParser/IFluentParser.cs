namespace JanHafner.Queryfy.Fluent.FluentParser
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    /// <summary>
    /// Provides an entry point for the fluent syntax.
    /// </summary>
    public interface IFluentParser
    {
        /// <summary>
        /// Provides a fluent syntax for mapping a property.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        IFluentParserMap Property();

        /// <summary>
        /// Binds the source object and <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="lambdaExpression">The <see cref="LambdaExpression"/>.</param>
        /// <returns>Returns self.</returns>
        [NotNull]
        IFluentParserMap Property([NotNull] Object source, [NotNull] LambdaExpression lambdaExpression);

        /// <summary>
        /// Processes the context.
        /// </summary>
        void Parse();
    }
}