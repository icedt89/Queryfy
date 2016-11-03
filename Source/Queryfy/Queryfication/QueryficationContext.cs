namespace JanHafner.Queryfy.Queryfication
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using Configuration;
    using Extensions;
    using Inspection;
    using JetBrains.Annotations;
    using Processing;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// The default implementation of the <see cref="IQueryficationContext"/> interface.
    /// </summary>
    public sealed class QueryficationContext : IQueryficationContext
    {
        [NotNull]
        private readonly QueryficationDictionary queryficationDictionary;

        [NotNull]
        private readonly IQueryficationEngine queryficationEngine;

        [NotNull]
        private readonly ILambdaExpressionInitializer lambdaExpressionInitializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryficationContext"/> class.
        /// </summary>
        /// <param name="configuration">An implementation of the <see cref="IQueryfyConfiguration"/> interface.</param>
        /// <param name="queryficationEngine">An implementation of the <see cref="IQueryficationEngine"/> interface.</param>
        /// <param name="urlValueProcessorFactory">An implementation of the <see cref="IValueProcessorFactory"/> interface.</param>
        /// <param name="lambdaExpressionInitializer">An implementation of the <see cref="ILambdaExpressionInitializer"/> interface.</param>
        /// <param name="instanceFactory">An implementation of the <see cref="IInstanceFactory"/> interface</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="configuration"/>', '<paramref name="instanceFactory"/>', '<paramref name="lambdaExpressionInitializer"/>', '<paramref name="queryficationEngine"/>' and '<paramref name="urlValueProcessorFactory"/>' cannot be null. </exception>
        public QueryficationContext([NotNull] IQueryfyConfiguration configuration, [NotNull] IQueryficationEngine queryficationEngine, [NotNull] IValueProcessorFactory urlValueProcessorFactory, [NotNull] ILambdaExpressionInitializer lambdaExpressionInitializer, [NotNull] IInstanceFactory instanceFactory)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (queryficationEngine == null)
            {
                throw new ArgumentNullException(nameof(queryficationEngine));
            }

            if (urlValueProcessorFactory == null)
            {
                throw new ArgumentNullException(nameof(urlValueProcessorFactory));
            }

            if (lambdaExpressionInitializer == null)
            {
                throw new ArgumentNullException(nameof(lambdaExpressionInitializer));
            }

            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            this.queryficationDictionary = new QueryficationDictionary();
            this.Configuration = configuration;
            this.ValueProcessorFactory = urlValueProcessorFactory;
            this.lambdaExpressionInitializer = lambdaExpressionInitializer;
            this.queryficationEngine = queryficationEngine;
            this.InstanceFactory = instanceFactory;
        }

        /// <summary>
        /// Gets the <see cref="IEnumerable{T}"/> of constructed parameters.
        /// </summary>
        public IEnumerable<QueryParameter> Parameters
        {
            get { return this.queryficationDictionary.Values; }
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
        /// Adds a new <see cref="QueryParameter"/>.
        /// </summary>
        /// <param name="parameter">The <see cref="QueryParameter"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parameter"/>' cannot be null. </exception>
        /// <exception cref="PropertyMustBeSpecifiedException">The property 'TypeOfValue', 'ParameterName' and 'ValueProcessor' cannot be null.</exception>
        /// <exception cref="ValueProcessorCannotHandleTypeException">The <see cref="IValueProcessor"/> can not handle the <see cref="Type"/> specified in the TypeOfValue property.</exception>
        public void AddParameter(QueryParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameter.TypeOfValue != null && parameter.ValueProcessor == null)
            {
                parameter.ValueProcessor = this.ValueProcessorFactory.GetUrlValueProcessorForType(parameter.TypeOfValue);
            }

            ValidateQueryParameter(parameter);
            this.queryficationDictionary.Add(parameter.ParameterName, parameter);
        }

        /// <summary>
        /// Validates the supplied <see cref="QueryParameter"/>.
        /// </summary>
        /// <exception cref="PropertyMustBeSpecifiedException">The property 'TypeOfValue', 'ParameterName' and 'ValueProcessor' cannot be null.</exception>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parameter"/>' cannot be null. </exception>
        /// <exception cref="ValueProcessorCannotHandleTypeException">The <see cref="IValueProcessor"/> can not handle the <see cref="Type"/> specified in the TypeOfValue property.</exception>
        private static void ValidateQueryParameter([NotNull] QueryParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameter.TypeOfValue == null)
            {
                throw new PropertyMustBeSpecifiedException("TypeOfValue", parameter);
            }

            if (parameter.ValueProcessor == null)
            {
                throw new PropertyMustBeSpecifiedException("ValueProcessor", parameter);
            }

            var checkType = parameter.TypeOfValue;
            if (checkType.IsNullable())
            {
                checkType = checkType.GetSingleGenericParameter();
            }

            if (!parameter.ValueProcessor.CanHandle(checkType))
            {
                throw new ValueProcessorCannotHandleTypeException(parameter.ValueProcessor, parameter.TypeOfValue);
            }
        }

        /// <summary>
        /// Adds a new <see cref="QueryParameter"/>.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        /// <exception cref="ValueProcessorCannotHandleTypeException">The <see cref="IValueProcessor"/> can not handle the <see cref="Type"/> specified in the TypeOfValue property.</exception>
        /// <exception cref="PropertyMustBeSpecifiedException">The property 'TypeOfValue', 'ParameterName' and 'ValueProcessor' cannot be null.</exception>
        public void AddParameter(Object source, LambdaExpression propertySelector)
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
            var urlParameter = this.queryficationEngine.GetParameterFromProperty(propertyInfo);
            urlParameter.Value = propertyInfoSource;
            this.AddParameter(urlParameter);
        }

        /// <summary>
        /// Adds a new <see cref="QueryParameter"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="valueProcessor">The used <see cref="IValueProcessor"/></param>
        /// <exception cref="PropertyMustBeSpecifiedException">The property 'TypeOfValue', 'ParameterName' and 'ValueProcessor' cannot be null.</exception>
        /// <exception cref="ValueProcessorCannotHandleTypeException">The <see cref="IValueProcessor"/> can not handle the <see cref="Type"/> specified in the TypeOfValue property.</exception>
        public void AddParameter(String parameterName, Object value, IValueProcessor valueProcessor)
        {
            this.AddParameter(new QueryParameter(parameterName, value, valueProcessor));
        }

        /// <summary>
        /// Adds a new <see cref="QueryParameter"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <exception cref="PropertyMustBeSpecifiedException">The property 'TypeOfValue', 'ParameterName' and 'ValueProcessor' cannot be null.</exception>
        /// <exception cref="ValueProcessorCannotHandleTypeException">The <see cref="IValueProcessor"/> can not handle the <see cref="Type"/> specified in the TypeOfValue property.</exception>
        public void AddParameter(String parameterName, Object value)
        {
            this.AddParameter(parameterName, value, null);
        }

        /// <summary>
        /// Creates a <see cref="QueryficationProcessingContext"/> which is supplied to the <see cref="IValueProcessor"/>.
        /// </summary>
        /// <param name="sourceValue">The source value.</param>
        /// <returns>Returns a new instance of the <see cref="QueryficationProcessingContext"/> class which is supplied to the <see cref="IValueProcessor"/>.</returns>
        public QueryficationProcessingContext CreateProcessingContext(Object sourceValue)
        {
            return new QueryficationProcessingContext(sourceValue, this.Configuration);
        }

        /// <summary>
        /// Checks if the supplied parameter exists.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns><c>true</c> if the parameter was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="parameterName" /> is null.</exception>
        public Boolean Exists(String parameterName)
        {
            return this.queryficationDictionary.ContainsKey(parameterName);
        }

        /// <summary>
        /// Checks if the supplied parameter exists. Uses a <see cref="LambdaExpression"/> to identify the parameter.
        /// </summary>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        /// <returns><c>true</c> if the parameter was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of 'propertySelector' cannot be null. </exception>
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
        /// Removes the specified parameter.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameterName" /> is null.</exception>
        public void Remove(String parameterName)
        {
            this.queryficationDictionary.Remove(parameterName);
        }

        /// <summary>
        /// Checks if the supplied parameter exists. Uses a <see cref="LambdaExpression"/> to identify the parameter.
        /// </summary>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        /// <exception cref="ArgumentNullException">The value of 'propertySelector' cannot be null. </exception>
        public void Remove(LambdaExpression propertySelector)
        {
            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));    
            }

            var urlParameter = this.queryficationEngine.GetParameterFromProperty(propertySelector.GetPropertyInfo());
            this.Remove(urlParameter.ParameterName);
        }

        /// <summary>
        /// Imports the specified object and creates <see cref="QueryParameter"/> instances based on the metadata.
        /// </summary>
        /// <param name="source"></param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' cannot be null. </exception>
        public void Import(Object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));    
            }

            this.queryficationEngine.Fill(source, this);
        }
    }
}