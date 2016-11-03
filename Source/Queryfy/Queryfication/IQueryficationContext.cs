namespace JanHafner.Queryfy.Queryfication
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Configuration;
    using Inspection;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// Provides methods to fill the context for further processing.
    /// </summary>
    public interface IQueryficationContext
    {
        /// <summary>
        /// Gets the <see cref="IEnumerable{T}"/> of constructed parameters.
        /// </summary>
        [NotNull]
        IEnumerable<QueryParameter> Parameters { get; }

        /// <summary>
        /// Gets the <see cref="IQueryfyConfiguration"/>.
        /// </summary>
        [NotNull]
        IQueryfyConfiguration Configuration { get; }

        /// <summary>
        /// Gets the <see cref="IValueProcessorFactory"/>.
        /// </summary>
        [NotNull]
        IValueProcessorFactory ValueProcessorFactory { get; }

        /// <summary>
        /// Gets the <see cref="IInstanceFactory"/>.
        /// </summary>
        [NotNull]
        IInstanceFactory InstanceFactory { get; }

        /// <summary>
        /// Adds a new <see cref="QueryParameter"/>.
        /// </summary>
        /// <param name="parameter">The <see cref="QueryParameter"/>.</param>
        void AddParameter([NotNull] QueryParameter parameter);

        /// <summary>
        /// Adds a new <see cref="QueryParameter"/>.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        void AddParameter([NotNull] Object source, [NotNull] LambdaExpression propertySelector);

        /// <summary>
        /// Adds a new <see cref="QueryParameter"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="valueProcessor">The used <see cref="IValueProcessor"/></param>
        void AddParameter([NotNull] String parameterName, [NotNull] Object value, [CanBeNull] IValueProcessor valueProcessor);

        /// <summary>
        /// Adds a new <see cref="QueryParameter"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        void AddParameter([NotNull] String parameterName, [NotNull] Object value);

        /// <summary>
        /// Creates a <see cref="QueryficationProcessingContext"/> which is supplied to the <see cref="IValueProcessor"/>.
        /// </summary>
        /// <param name="sourceValue">The source value.</param>
        /// <returns>Returns a new instance of the <see cref="QueryficationProcessingContext"/> class which is supplied to the <see cref="IValueProcessor"/>.</returns>
        [NotNull]
        QueryficationProcessingContext CreateProcessingContext([NotNull] Object sourceValue);

        /// <summary>
        /// Imports the specified object and creates <see cref="QueryParameter"/> instances based on the metadata.
        /// </summary>
        /// <param name="source"></param>
        void Import([NotNull] Object source);

        /// <summary>
        /// Checks if the supplied parameter exists.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><c>true</c> if the parameter was found; otherwise, <c>false</c>.</returns>
        Boolean Exists([NotNull] String parameterName);

        /// <summary>
        /// Checks if the supplied parameter exists. Uses a <see cref="LambdaExpression"/> to identify the parameter.
        /// </summary>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        /// <returns><c>true</c> if the parameter was found; otherwise, <c>false</c>.</returns>
        Boolean Exists([NotNull] LambdaExpression propertySelector);

        /// <summary>
        /// Removes the specified parameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        void Remove([NotNull] String parameterName);

        /// <summary>
        /// Checks if the supplied parameter exists. Uses a <see cref="LambdaExpression"/> to identify the parameter.
        /// </summary>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        void Remove([NotNull] LambdaExpression propertySelector);
    }
}