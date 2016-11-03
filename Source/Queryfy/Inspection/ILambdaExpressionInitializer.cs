namespace JanHafner.Queryfy.Inspection
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="ILambdaExpressionInitializer"/> interface provides methods for initializing each property of the supplied <see cref="LambdaExpression"/>.
    /// </summary>
    public interface ILambdaExpressionInitializer
    {
        /// <summary>
        /// Initializes the specified <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="source">The source parameter for the <see cref="LambdaExpression"/>.</param>
        /// <param name="propertyPath">The <see cref="LambdaExpression"/>.</param>
        /// <param name="lastPropertyInfo">Returns the last initialized <see cref="PropertyInfo"/>.</param>
        /// <returns>The last created object on which last retrieved <see cref="PropertyInfo"/> resides.</returns>
        [NotNull]
        Object Initialize([NotNull] Object source, [NotNull] LambdaExpression propertyPath, [NotNull] out PropertyInfo lastPropertyInfo);
    }
}