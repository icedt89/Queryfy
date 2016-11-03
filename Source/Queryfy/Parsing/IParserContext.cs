namespace JanHafner.Queryfy.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Configuration;
    using Inspection;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// Provides methods to create maps for parsing query strings to complex objects and holds the data for the final process.
    /// </summary>
    public interface IParserContext
    {
        /// <summary>
        /// Gets a list of created maps.
        /// </summary>
        [NotNull]
        IEnumerable<PropertyParserMap> Maps { get; }

        /// <summary>
        /// Gets a list of mappings where each parameter from the query has a <see cref="QueryParameter"/>.
        /// </summary>
        [NotNull]
        IEnumerable<KeyValuePair<string, QueryParameter>> QueryValuePairs { get; }

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
        /// Creates a <see cref="ParserProcessingContext"/> which is supplied to the <see cref="IValueProcessor"/>.
        /// </summary>
        /// <param name="sourceValue">The source value.</param>
        /// <param name="destinationType">The type of the result.</param>
        /// <returns>Returns a new instance of the <see cref="ParserProcessingContext"/> class which is supplied to the <see cref="IValueProcessor"/>.</returns>
        [NotNull]
        ParserProcessingContext CreateProcessingContext([NotNull] String sourceValue, [NotNull] Type destinationType);

        /// <summary>
        /// Adds a new mapping for the supplied <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="source">The source object on which the <see cref="LambdaExpression"/> operates.</param>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property which is used for the mapping.</param>
        /// <param name="parameterName">The parameter name which identifies the parameter in the query string.</param>
        /// <param name="valueProcessor">An instance of the <see cref="IValueProcessor"/> class.</param>
        void AddPropertyMapping([NotNull] Object source, [NotNull] LambdaExpression propertySelector, [NotNull] String parameterName, [NotNull] IValueProcessor valueProcessor);

        /// <summary>
        /// Adds a new mapping for the supplied <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="source">The source object on which the <see cref="LambdaExpression"/> operates.</param>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property which is used for the mapping.</param>
        void AddPropertyMapping([NotNull] Object source, [NotNull] LambdaExpression propertySelector);

        /// <summary>
        /// Search for the mappable query parameter with the supplied name and checks if no mapping already exist. If these conditions are met the <see cref="PropertyParserMap"/> is added.
        /// </summary>
        /// <param name="propertyParserMap">The <see cref="PropertyParserMap"/>.</param>
        /// <returns><c>true</c> if the mapping was added; otherwise, <c>false</c>.</returns>
        Boolean TryAddPropertyMapping([NotNull] PropertyParserMap propertyParserMap);

        /// <summary>
        /// Checks if a mapping for the supplied parameter exists.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><c>true</c> if the mapping was found; otherwise, <c>false</c>.</returns>
        Boolean Exists([NotNull] String parameterName);

        /// <summary>
        /// Creates maps by inspecting the properties of the supplied object recursively.
        /// </summary>
        /// <param name="source">The source object for which to create maps.</param>
        void CreateMaps([NotNull] Object source);

        /// <summary>
        /// Checks if a mapping for the supplied parameter exists. Uses a <see cref="LambdaExpression"/> to identify the parameter.
        /// </summary>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        /// <returns><c>true</c> if the mapping was found; otherwise, <c>false</c>.</returns>
        Boolean Exists([NotNull] LambdaExpression propertySelector);

        /// <summary>
        /// Removes the mapping for the specified parameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        void Remove([NotNull] String parameterName);

        /// <summary>
        /// Checks if a mapping for the supplied parameter exists. Uses a <see cref="LambdaExpression"/> to identify the parameter.
        /// </summary>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        void Remove([NotNull] LambdaExpression propertySelector);
    }
}