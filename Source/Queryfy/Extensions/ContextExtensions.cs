namespace JanHafner.Queryfy.Extensions
{
    using System;
    using System.Linq.Expressions;
    using Inspection;
    using JetBrains.Annotations;
    using Parsing;
    using Processing;
    using Queryfication;

    /// <summary>
    /// Provides extensions for <see cref="IParserContext"/> and <see cref="IQueryficationContext"/>.
    /// </summary>
    public static class ContextExtensions
    {
        /// <summary>
        /// Adds a new mapping for the supplied <see cref="Expression{Func{T, TProperty}}"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/>.</typeparam>
        /// <typeparam name="TProperty">The <see cref="Type"/> of the property.</typeparam>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <param name="source">The source object on which the <see cref="Expression{Func{T, TProperty}}"/> operates.</param>
        /// <param name="propertySelector">A <see cref="Expression"/> which selects the property which is used for the mapping.</param>
        /// <param name="parameterName">The parameter name which identifies the parameter in the query string.</param>
        /// <param name="valueProcessor">An instance of the <see cref="IValueProcessor"/> class.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parserContext"/>', '<paramref name="source"/>', '<paramref name="propertySelector"/>', '<paramref name="parameterName"/>' and '<paramref name="valueProcessor"/>' cannot be null. </exception>
        public static void AddPropertyMapping<T, TProperty>([NotNull] this IParserContext parserContext, T source, [NotNull] Expression<Func<T, TProperty>> propertySelector, [NotNull] String parameterName, [NotNull] IValueProcessor valueProcessor)
        {
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));    
            }

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

            parserContext.AddPropertyMapping(source, propertySelector, parameterName, valueProcessor);
        }

        /// <summary>
        /// Adds a new mapping for the supplied <see cref="Expression{Func{T, TProperty}}"/>.
        /// </summary>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/>.</typeparam>
        /// <typeparam name="TProperty">The <see cref="Type"/> of the property.</typeparam>
        /// <param name="source">The source object on which the <see cref="Expression{Func{T, TProperty}}"/> operates.</param>
        /// <param name="propertySelector">A <see cref="Expression{Func{T, TProperty}}"/> which selects the property which is used for the mapping.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parserContext"/>', '<paramref name="source"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        public static void AddPropertyMapping<T, TProperty>([NotNull] this IParserContext parserContext, [NotNull] T source, [NotNull] Expression<Func<T, TProperty>> propertySelector)
        {
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            parserContext.AddPropertyMapping(source, propertySelector);
        }

        /// <summary>
        /// Adds a new <see cref="QueryParameter"/>.
        /// </summary>
        /// <param name="queryficationContext">The <see cref="IQueryficationContext"/>.</param>
        /// <param name="source">The source object.</param>
        /// <param name="propertySelector">A <see cref="Expression{Func{T, TProperty}}"/> which selects the property.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryficationContext"/>', '<paramref name="source"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        public static void AddParameter<T, TProperty>([NotNull] this IQueryficationContext queryficationContext, [NotNull] T source, [NotNull] Expression<Func<T, TProperty>> propertySelector)
        {
            if (queryficationContext == null)
            {
                throw new ArgumentNullException(nameof(queryficationContext));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            queryficationContext.AddParameter(source, propertySelector);
        }

        /// <summary>
        /// Checks if the supplied parameter exists. Uses a <see cref="Expression{Func{T, TProperty}}"/> to identify the parameter.
        /// </summary>
        /// <param name="queryficationContext">The <see cref="IQueryficationContext"/>.</param>
        /// <param name="propertySelector">A <see cref="Expression{Func{T, TProperty}}"/> which selects the property.</param>
        /// <returns><c>true</c> if the parameter was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryficationContext"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        public static Boolean Exists<T, TProperty>([NotNull]  this IQueryficationContext queryficationContext, [NotNull] Expression<Func<T, TProperty>> propertySelector)
        {
            if(queryficationContext == null)
            {
                throw new ArgumentNullException(nameof(queryficationContext));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            return queryficationContext.Exists(propertySelector);
        }

        /// <summary>
        /// Checks if the supplied parameter exists. Uses a <see cref="Expression{Func{T, TProperty}}"/> to identify the parameter.
        /// </summary>
        /// <param name="queryficationContext">The <see cref="IQueryficationContext"/>.</param>
        /// <param name="propertySelector">A <see cref="Expression{Func{T, TProperty}}"/> which selects the property.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryficationContext"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        public static void Remove<T, TProperty>([NotNull] this IQueryficationContext queryficationContext, [NotNull] Expression<Func<T, TProperty>> propertySelector)
        {
            if (queryficationContext == null)
            {
                throw new ArgumentNullException(nameof(queryficationContext));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            queryficationContext.Remove(propertySelector);
        }

        /// <summary>
        /// Checks if a mapping for the supplied parameter exists. Uses a <see cref="LambdaExpression"/> to identify the parameter.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/>.</typeparam>
        /// <typeparam name="TProperty">The <see cref="Type"/> of the property.</typeparam>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <param name="propertySelector">A <see cref="LambdaExpression"/> which selects the property.</param>
        /// <returns><c>true</c> if the mapping was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parserContext"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        public static Boolean Exists<T, TProperty>([NotNull] this IParserContext parserContext, [NotNull] Expression<Func<T, TProperty>> propertySelector)
        {
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }
        
            return parserContext.Exists(propertySelector);
        }

        /// <summary>
        /// Checks if a mapping for the supplied parameter exists. Uses a <see cref="Expression{Func{T, TProperty}}"/> to identify the parameter.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/>.</typeparam>
        /// <typeparam name="TProperty">The <see cref="Type"/> of the property.</typeparam>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <param name="propertySelector">A <see cref="Expression{Func{T, TProperty}}"/> which selects the property.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parserContext"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        public static void Remove<T, TProperty>(this IParserContext parserContext, Expression<Func<T, TProperty>> propertySelector)
        {
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            parserContext.Remove(propertySelector);
        }
    }
}