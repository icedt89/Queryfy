namespace JanHafner.Queryfy.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// The <see cref="QueryficationBuilder"/> class.
    /// </summary>
    public sealed class QueryficationBuilder : IQueryficationBuilder
    {
        [NotNull]
        private readonly IQueryfyConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryficationBuilder"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IQueryfyConfiguration"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="configuration"/>' cannot be null. </exception>
        public QueryficationBuilder([NotNull] IQueryfyConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.configuration = configuration;
        }

        /// <summary>
        /// Uses the supplied <see cref="IQueryficationContext"/> to build the final query string.
        /// </summary>
        /// <param name="queryficationContext">The supplied <see cref="IQueryficationContext"/>.</param>
        /// <returns>An <see cref="QueryficationResult"/> that contains information about the output.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryficationContext"/>' cannot be null. </exception>
        /// <exception cref="PropertyMustBeSpecifiedException">The 'ValueProcessor' property of one of the parameters inside the <see cref="IQueryficationContext"/> is null.</exception>
        public QueryficationResult Queryfy(IQueryficationContext queryficationContext)
        {
            if (queryficationContext == null)
            {
                throw new ArgumentNullException(nameof(queryficationContext));
            }

            var processedValues = new Dictionary<String, String>();
            foreach (var parameter in queryficationContext.Parameters)
            {
                if (parameter.ValueProcessor == null)
                {
                    throw new PropertyMustBeSpecifiedException("ValueProcessor", parameter);
                }

                var value = parameter.Value;
                if (value == null)
                {
                    if (!(parameter.ValueProcessor is NullValueProcessor))
                    {
                        continue;
                    }
                }
                else
                {
                    value = parameter.ValueProcessor.Process(queryficationContext.CreateProcessingContext(value));
                }

                processedValues.Add(parameter.ParameterName, (String)value);
            }

            return new QueryficationResult(this.configuration, processedValues);
        }

        /// <summary>
        /// Uses the supplied <see cref="IParserContext"/> to write all values to properties.
        /// </summary>
        /// <param name="parserContext">The supplied <see cref="IParserContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parserContext"/>' cannot be null. </exception>
        /// <exception cref="PropertyMustBeSpecifiedException">The 'ValueProcessor' property of one of the parameters inside the <see cref="IParserContext"/> is null.</exception>
        public void Parse(IParserContext parserContext)
        {
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            foreach (var propertyMapping in parserContext.Maps.Join(parserContext.QueryValuePairs, map => map.Parameter.ParameterName, pair => pair.Key, (map, pair) =>
                                                                                                                                                         {
                                                                                                                                                             map.Parameter.Value = pair.Value.Value;
                                                                                                                                                             return map;
                                                                                                                                                         }))
            {
                if (propertyMapping.Parameter.ValueProcessor == null)
                {
                    throw new PropertyMustBeSpecifiedException("ValueProcessor", propertyMapping.Parameter);
                }

                if (propertyMapping.Parameter.Value == null)
                {
                    throw new PropertyMustBeSpecifiedException("Value", propertyMapping.Parameter);
                }

                if (propertyMapping.Parameter.TypeOfValue == null)
                {
                    throw new PropertyMustBeSpecifiedException("TypeOfValue", propertyMapping.Parameter);
                }

                var childProcessingContext = parserContext.CreateProcessingContext(propertyMapping.Parameter.Value.ToString(), propertyMapping.Parameter.TypeOfValue);
                var resolvedValue = propertyMapping.Parameter.ValueProcessor.Resolve(childProcessingContext);
             
                propertyMapping.Property.SetValue(propertyMapping.Source, resolvedValue);
            }
        }
    }
}