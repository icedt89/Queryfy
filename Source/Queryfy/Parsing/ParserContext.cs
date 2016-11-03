namespace JanHafner.Queryfy.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Configuration;
    using Inspection;
    using JetBrains.Annotations;
    using Processing;
    using Queryfication;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Provides methods to create maps for parsing query strings to complex objects.
    /// </summary>
    public sealed class ParserContext : IParserContext
    {
        [NotNull]
        private readonly ICollection<PropertyParserMap> propertyParserMaps;

        [NotNull]
        private readonly IQueryficationEngine queryficationEngine;

        [NotNull]
        private readonly ILambdaExpressionInitializer lambdaExpressionInitializer;

        [NotNull]
        private readonly QueryficationDictionary queryficationDictionary;

        /// <summary>
        /// Gets a list of created maps.
        /// </summary>
        public IEnumerable<PropertyParserMap> Maps
        {
            get { return this.propertyParserMaps; }
        }

        /// <summary>
        /// Gets a list of mappings where each parameter from the query has a <see cref="QueryParameter"/>.
        /// </summary>
        public IEnumerable<KeyValuePair<String, QueryParameter>> QueryValuePairs
        {
            get { return this.queryficationDictionary; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryfyConfiguration"/>.
        /// </summary>
        public IQueryfyConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets the <see cref="IValueProcessorFactory"/>.
        /// </summary>
        public IValueProcessorFactory ValueProcessorFactory { get; private set; }

        /// <summary>
        /// Gets the <see cref="IInstanceFactory"/>.
        /// </summary>
        public IInstanceFactory InstanceFactory { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserContext"/> class.
        /// </summary>
        /// <param name="query">The query to parse.</param>
        /// <param name="configuration">An implementation of the <see cref="IQueryfyConfiguration"/> interface.</param>
        /// <param name="queryficationEngine">An implementation of the <see cref="IQueryficationEngine"/> interface.</param>
        /// <param name="valueProcessorFactory">An implementation of the <see cref="IValueProcessorFactory"/> interface.</param>
        /// <param name="lambdaExpressionInitializer">An implementation of the <see cref="ILambdaExpressionInitializer"/> interface.</param>
        /// <param name="instanceFactory">An implementation of the <see cref="IInstanceFactory"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="query"/>', '<paramref name="configuration"/>', '<paramref name="queryficationEngine"/>', '<paramref name="valueProcessorFactory"/>', '<paramref name="lambdaExpressionInitializer"/>' and '<paramref name="instanceFactory"/>' cannot be null. </exception>
        public ParserContext(String query, IQueryfyConfiguration configuration, IQueryficationEngine queryficationEngine, IValueProcessorFactory valueProcessorFactory, ILambdaExpressionInitializer lambdaExpressionInitializer, IInstanceFactory instanceFactory)
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (queryficationEngine == null)
            {
                throw new ArgumentNullException(nameof(queryficationEngine));
            }

            if (valueProcessorFactory == null)
            {
                throw new ArgumentNullException(nameof(valueProcessorFactory));
            }

            if (lambdaExpressionInitializer == null)
            {
                throw new ArgumentNullException(nameof(lambdaExpressionInitializer));
            }

            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            this.queryficationDictionary = QueryficationDictionary.FromQueryString(query, configuration);
            this.propertyParserMaps = new ParserMapCollection();
            this.Configuration = configuration;
            this.queryficationEngine = queryficationEngine;
            this.InstanceFactory = instanceFactory;
            this.ValueProcessorFactory = valueProcessorFactory;
            this.lambdaExpressionInitializer = lambdaExpressionInitializer;
        }

        /// <summary>
        /// Creates a <see cref="ParserProcessingContext"/> which is supplied to the <see cref="IValueProcessor"/>.
        /// </summary>
        /// <param name="sourceValue">The source value.</param>
        /// <param name="destinationType">The type of the result.</param>
        /// <returns>Returns a new instance of the <see cref="ParserProcessingContext"/> class which is supplied to the <see cref="IValueProcessor"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="sourceValue" />' and '<paramref name="destinationType" />' cannot be null. </exception>
        public ParserProcessingContext CreateProcessingContext(String sourceValue, Type destinationType)
        {
            return new ParserProcessingContext(sourceValue, destinationType, this.Configuration);
        }

        /// <summary>
        /// Adds a new mapping for the supplied <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="source">The source object on which the <see cref="LambdaExpression"/> operates.</param>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property which is used for the mapping.</param>
        /// <param name="parameterName">The parameter name which identifies the parameter in the query string.</param>
        /// <param name="valueProcessor">An instance of the <see cref="IValueProcessor"/> class.</param>
        /// <exception cref="MemberIsNotWritableException">The CanWrite property of the propertyInfo parameter returned <c>false</c>.</exception>
        /// <exception cref="PropertyNotFoundException">The propertyInfo parameter is not declared on the source.</exception>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>', '<paramref name="propertySelector"/>', '<paramref name="parameterName"/>' and '<paramref name="valueProcessor"/>' cannot be null. </exception>
        public void AddPropertyMapping(Object source, LambdaExpression propertySelector, String parameterName, IValueProcessor valueProcessor)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            if (String.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            if (valueProcessor == null)
            {
                throw new ArgumentNullException(nameof(valueProcessor));
            }

            PropertyInfo propertyInfo;
            var propertyInfoSource = this.lambdaExpressionInitializer.Initialize(source, propertySelector, out propertyInfo);
            this.AddPropertyMapping(new PropertyParserMap(propertyInfoSource, propertyInfo, new QueryParameter(parameterName, null, valueProcessor)));
        }

        /// <summary>
        /// Adds a new mapping for the supplied <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="source">The source object on which the <see cref="LambdaExpression"/> operates.</param>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property which is used for the mapping.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        /// <exception cref="MemberIsNotWritableException">The CanWrite property of the propertyInfo parameter returned <c>false</c>.</exception>
        /// <exception cref="PropertyNotFoundException">The propertyInfo parameter is not declared on the source.</exception>
        public void AddPropertyMapping(Object source, LambdaExpression propertySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            PropertyInfo propertyInfo;
            var propertyInfoSource = this.lambdaExpressionInitializer.Initialize(source, propertySelector, out propertyInfo);
            var urlParameterPrototype = this.queryficationEngine.GetParameterFromProperty(propertyInfo);
            this.AddPropertyMapping(new PropertyParserMap(propertyInfoSource, propertyInfo, new QueryParameter(urlParameterPrototype.ParameterName, null, urlParameterPrototype.ValueProcessor)));
        }

        /// <summary>
        /// Adds a new property mapping.
        /// </summary>
        /// <param name="propertyParserMap">The <see cref="PropertyParserMap"/> to add.</param>
        /// <exception cref="ArgumentException">"A parameter with the specified name already exists."</exception>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyParserMap"/>' cannot be null. </exception>
        private void AddPropertyMapping(PropertyParserMap propertyParserMap)
        {
            if (!this.TryAddPropertyMapping(propertyParserMap))
            {
                throw new InvalidOperationException(String.Format("The QueryString does not contain the key '{0}'.", propertyParserMap.Parameter.ParameterName));
            }
        }

        /// <summary>
        /// Search for the mappable query parameter with the supplied name and checks if no mapping already exist. If these conditions are met the <see cref="PropertyParserMap"/> is added.
        /// </summary>
        /// <param name="propertyParserMap">The <see cref="PropertyParserMap"/>.</param>
        /// <returns><c>true</c> if the mapping was added; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyParserMap"/>' cannot be null. </exception>
        public Boolean TryAddPropertyMapping(PropertyParserMap propertyParserMap)
        {
            if (propertyParserMap == null)
            {
                throw new ArgumentNullException(nameof(propertyParserMap));    
            }

            if (propertyParserMap.Parameter.TypeOfValue != null && propertyParserMap.Parameter.ValueProcessor == null)
            {
                propertyParserMap.Parameter.ValueProcessor = this.ValueProcessorFactory.GetUrlValueProcessorForType(propertyParserMap.Parameter.TypeOfValue);
            }

            if (this.queryficationDictionary.ContainsKey(propertyParserMap.Parameter.ParameterName) && !this.Exists(propertyParserMap.Parameter.ParameterName))
            {
                this.propertyParserMaps.Add(propertyParserMap);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a mapping for the supplied parameter exists.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><c>true</c> if the mapping was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parameterName"/>' cannot be null. </exception>
        public Boolean Exists(String parameterName)
        {
            if (String.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            return this.propertyParserMaps.Any(map => map.Parameter.ParameterName == parameterName);
        }

        /// <summary>
        /// Checks if a mapping for the supplied parameter exists. Uses a <see cref="LambdaExpression"/> to identify the parameter.
        /// </summary>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        /// <returns><c>true</c> if the mapping was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertySelector"/>' cannot be null. </exception>
        public Boolean Exists(LambdaExpression propertySelector)
        {
            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            var urlParameter = this.queryficationEngine.GetParameterFromProperty(propertySelector.GetPropertyInfo());
            return this.Exists(urlParameter.ParameterName);
        }

        /// <summary>
        /// Removes the mapping for the specified parameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parameterName"/>' cannot be null. </exception>
        public void Remove(String parameterName)
        {
            if (String.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            this.propertyParserMaps.Remove(map => map.Parameter.ParameterName == parameterName);
        }

        /// <summary>
        /// Checks if a mapping for the supplied parameter exists. Uses a <see cref="LambdaExpression"/> to identify the parameter.
        /// </summary>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertySelector"/>' cannot be null. </exception>
        public void Remove(LambdaExpression propertySelector)
        {
            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            var queryParameter = this.queryficationEngine.GetParameterFromProperty(propertySelector.GetPropertyInfo());
            this.Remove(queryParameter.ParameterName);
        }

        /// <summary>
        /// Creates maps by inspecting the properties of the supplied object recursively.
        /// </summary>
        /// <param name="source">The source object for which to create maps.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' cannot be null. </exception>
        public void CreateMaps(Object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            this.queryficationEngine.CreateMaps(source, this);
        }
    }
}