namespace JanHafner.Queryfy.Fluent
{
    using System;
    using System.Linq.Expressions;
    using FluentParser;
    using FluentQueryfy;
    using JetBrains.Annotations;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Applies fluent syntax to the supplied <see cref="IQueryfyDotNet"/>.
        /// </summary>
        /// <param name="queryfyDotNet">The <see cref="IQueryfyDotNet"/>.</param>
        /// <returns>A fluent syntax provide.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryfyDotNet"/>' cannot be null. </exception>
        [NotNull]
        public static IFluentQueryfy FluentQueryfy([NotNull] this IQueryfyDotNet queryfyDotNet)
        {
            if (queryfyDotNet == null)
            {
                throw new ArgumentNullException(nameof(queryfyDotNet));
            }

            return new FluentQueryfy.FluentQueryfy(queryfyDotNet);
        }

        /// <summary>
        /// Applies fluent syntax to the supplied <see cref="IQueryfyDotNet"/>.
        /// </summary>
        /// <param name="queryfyDotNet">The <see cref="IQueryfyDotNet"/>.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>A fluent syntax provide.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryfyDotNet"/>' and '<paramref name="queryString"/>' cannot be null. </exception>
        [NotNull]
        public static IFluentParser FluentParser([NotNull] this IQueryfyDotNet queryfyDotNet, [NotNull] String queryString)
        {
            if (queryfyDotNet == null)
            {
                throw new ArgumentNullException(nameof(queryfyDotNet));
            }
            
            if(String.IsNullOrEmpty(queryString))
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            return new FluentParser.FluentParser(queryfyDotNet, queryString);
        }

        /// <summary>
        /// Binds the specified property path.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the source object.</typeparam>
        /// <typeparam name="TProperty">The <see cref="Type"/> of the selected property.</typeparam>
        /// <param name="fluentParserMap">The <see cref="IFluentParserMap"/>.</param>
        /// <param name="source">The source.</param>
        /// <param name="propertySelector">The property selector.</param>
        /// <returns>The same <see cref="IFluentParserMap"/> from the input.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="fluentParserMap"/>', '<paramref name="source"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        [NotNull]
        public static IFluentParserMap Property<T, TProperty>([NotNull] this IFluentParserMap fluentParserMap, [NotNull] T source, [NotNull] Expression<Func<T, TProperty>> propertySelector)
        {
            if (fluentParserMap == null)
            {
                throw new ArgumentNullException(nameof(fluentParserMap));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            return fluentParserMap.BindTo(source, propertySelector);
        }

        /// <summary>
        /// Binds the specified property path.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the source object.</typeparam>
        /// <typeparam name="TProperty">The <see cref="Type"/> of the selected property.</typeparam>
        /// <param name="fluentParser">The <see cref="IFluentParser"/>.</param>
        /// <param name="source">The source.</param>
        /// <param name="propertySelector">The property selector.</param>
        /// <returns>The same <see cref="IFluentParserMap"/> from the input.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="fluentParser"/>', '<paramref name="source"/>' and '<paramref name="propertySelector"/>' cannot be null. </exception>
        [NotNull]
        public static IFluentParserMap Property<T, TProperty>([NotNull] this IFluentParser fluentParser, [NotNull] T source, [NotNull] Expression<Func<T, TProperty>> propertySelector)
        {
            if (fluentParser == null)
            {
                throw new ArgumentNullException(nameof(fluentParser));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            return fluentParser.Property(source, propertySelector);
        }
    }
}