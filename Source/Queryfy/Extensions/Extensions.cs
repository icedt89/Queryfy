namespace JanHafner.Queryfy.Extensions
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Inspection;
    using Configuration;
    using JetBrains.Annotations;
    using Parsing;
    using Processing;
    using Toolkit.Common;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Provides uncategorized extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Lookup the mapped <see cref="IValueProcessor"/> for the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="registry">The <see cref="ITypeValueProcessorRegistry"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/> for which the mapped instance should be returned.</typeparam>
        /// <param name="result">The <see cref="IValueProcessor"/></param>
        /// <returns><c>true</c> if the mapping was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="registry"/>' cannot be null. </exception>
        public static Boolean TryLookup<T>([NotNull] this ITypeValueProcessorRegistry registry, [CanBeNull] out IValueProcessor result)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            return registry.TryLookup(typeof (T), out result);
        }

        /// <summary>
        /// Initializes the specified <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="lambdaExpressionInitializer">The <see cref="ILambdaExpressionInitializer"/></param>
        /// <param name="source">The source parameter for the <see cref="LambdaExpression"/>.</param>
        /// <param name="propertySelector">The <see cref="LambdaExpression"/>.</param>
        /// <param name="propertyInfo">Returns the last initialized <see cref="PropertyInfo"/>.</param>
        /// <returns>The last created object on which last retrieved <see cref="PropertyInfo"/> resides.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="lambdaExpressionInitializer"/>', '<paramref name="source"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        [NotNull]
        public static Object Initialize<T, TProperty>([NotNull] this ILambdaExpressionInitializer lambdaExpressionInitializer, [NotNull] T source, [NotNull] Expression<Func<T, TProperty>> propertySelector, [NotNull] out PropertyInfo propertyInfo)
        {
            if (lambdaExpressionInitializer == null)
            {
                throw new ArgumentNullException(nameof(lambdaExpressionInitializer));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            return lambdaExpressionInitializer.Initialize(source, propertySelector, out propertyInfo);
        }

        /// <summary>
        /// Removes the mapping for the suppied <see cref="Type"/>.
        /// </summary>
        /// <param name="registry">The <see cref="ITypeValueProcessorRegistry"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/>.</typeparam>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="registry"/>' cannot be null. </exception>
        public static void RemoveRegistration<T>([NotNull] this ITypeValueProcessorRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            registry.RemoveRegistration(typeof (T));
        }

        /// <summary>
        /// Maps the <see cref="IValueProcessor"/> to the <see cref="Type"/>.
        /// </summary>
        /// <param name="registry">The <see cref="ITypeValueProcessorRegistry"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/>.</typeparam>
        /// <param name="valueProcessor">The <see cref="IValueProcessor"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="registry"/>' and '<paramref name="valueProcessor"/>' cannot be null. </exception>
        public static void SetRegistration<T>([NotNull] this ITypeValueProcessorRegistry registry, [NotNull] IValueProcessor valueProcessor)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            if (valueProcessor == null)
            {
                throw new ArgumentNullException(nameof(valueProcessor));
            }

            registry.SetRegistration(typeof (T), valueProcessor);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Type"/> and fills it with the supplied mappings in the <see cref="IParserContext"/>.
        /// </summary>
        /// <param name="queryfyDotNet">The <see cref="IQueryfyDotNet"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the destination.</typeparam>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryfyDotNet"/>' and '<paramref name="parserContext"/>' cannot be null. </exception>
        public static T Parse<T>([NotNull] this IQueryfyDotNet queryfyDotNet, [NotNull] IParserContext parserContext)
        {
            if (queryfyDotNet == null)
            {
                throw new ArgumentNullException(nameof(queryfyDotNet));
            }

            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            return (T) queryfyDotNet.Parse(typeof (T), parserContext);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Type"/> and fills it by using the query string.
        /// </summary>
        /// <param name="queryfyDotNet">The <see cref="IQueryfyDotNet"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/> of the destination.</typeparam>
        /// <param name="query">The query string.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryfyDotNet"/>' and '<paramref name="query"/>' cannot be null. </exception>
        public static T Parse<T>([NotNull] this IQueryfyDotNet queryfyDotNet, [NotNull] String query)
        {
            if (queryfyDotNet == null)
            {
                throw new ArgumentNullException(nameof(queryfyDotNet));
            }
            
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            return (T) queryfyDotNet.Parse(typeof (T), query);
        }

        /// <summary>
        /// Parses the supplied query into a new object of Type <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>The parsed object.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="query"/>' cannot be null. </exception>
        public static T Parse<T>([NotNull] this String query)
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));    
            }

            return Mapper.Parse<T>(query);
        }

        /// <summary>
        /// Queryfies the supplied source object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns>The query.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' cannot be null. </exception>
        public static String Queryfy([NotNull] this Object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Mapper.Queryfy(source).QueryString;
        }


        /// <summary>
        /// Imports the <see cref="IParserContext"/>-Mappings for the <see cref="Type"/> of the supplied sources.
        /// </summary>
        /// <param name="parserContext">The <see cref="IParserContext"/> to fill with mappings.</param>
        /// <param name="sourceObjects">A list of source objects which <see cref="Type"/>`s are used to construct the <see cref="LambdaExpression"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parserContext"/>' and '<paramref name="sourceObjects"/>' cannot be null. </exception>
        /// <exception cref="InvalidOperationException">No element satisfies the condition in <paramref name="predicate" />.-or-More than one element satisfies the condition in <paramref name="predicate" />.-or-The source sequence is empty.</exception>
        /// <exception cref="ArgumentException">The supplied property path does not contain any parts.</exception>
        /// <exception cref="ConstructedPropertyPathDoesNotContainAnyPartsException">Thee supplied property path '<paramref name="propertyPath"/>' does not contain any constructible parts for Type '<paramref name="sourceType"/>'. </exception>
        public static void ImportConfiguration([NotNull] this IParserContext parserContext, [NotNull] params Object[] sourceObjects)
        {
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            if (sourceObjects == null)
            {
                throw new ArgumentNullException(nameof(sourceObjects));
            }
            
            var configuration = QueryfyConfigurationSection.GetXmlConfiguration();
            if (configuration == null)
            {
                throw new ConfigurationErrorsException();
            }

            if (configuration.ParserConfiguration != null && configuration.ParserConfiguration.TypeMappings != null)
            {
                foreach (var source in sourceObjects)
                {
                    var closuredSource = source;
                    var parserTypeMappings = configuration.ParserConfiguration.TypeMappings.Single(m => m.SourceType == closuredSource.GetType());
                    foreach (var propertyMapping in parserTypeMappings.PropertyMappings)
                    {
                        var lambda = LambdaExpressionDeserializer.Deserialize(closuredSource.GetType(), propertyMapping.PropertyPath);
                        var propertyType = lambda.GetPropertyInfo();
                        var valueProcessor = parserContext.ValueProcessorFactory.GetUrlValueProcessor(propertyType.PropertyType, propertyMapping.ValueProcessorType);
                        parserContext.AddPropertyMapping(closuredSource, lambda, propertyMapping.ParameterName, valueProcessor);
                    }
                }
            }
        }
    }
}