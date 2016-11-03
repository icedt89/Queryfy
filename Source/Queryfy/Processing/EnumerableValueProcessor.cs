namespace JanHafner.Queryfy.Processing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Extensions;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// Processes <see cref="IEnumerable{T}"/> implementations.
    /// </summary>
    public class EnumerableValueProcessor : IValueProcessor
    {
        [NotNull]
        private readonly IValueProcessor itemValueProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumerableValueProcessor"/> class.
        /// </summary>
        /// <param name="itemValueProcessor">An implementation of the <see cref="IValueProcessor"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of 'itemValueProcessor' cannot be null. </exception>
        public EnumerableValueProcessor([NotNull] IValueProcessor itemValueProcessor)
        {
            if (itemValueProcessor == null)
            {
                throw new ArgumentNullException(nameof(itemValueProcessor));    
            }

            this.itemValueProcessor = itemValueProcessor;
        }

        /// <summary>
        /// Converts the object to the query parameter value.
        /// </summary>
        /// <param name="processingContext">The <see cref="QueryficationProcessingContext"/> which holds information about the current value.</param>
        /// <returns>The <see cref="string"/> used in the query.</returns>
        /// <exception cref="ArgumentNullException">The value of 'processingContext' cannot be null. </exception>
        public virtual String Process(QueryficationProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            var enumerable = ((IEnumerable) processingContext.SourceValue).Cast<Object>().Select(oldValue => this.itemValueProcessor.Process(processingContext.CreateProcessingContext(oldValue)));

            return String.Join(processingContext.Configuration.QueryParameterValueToValueSeparatorChar.ToString(), enumerable);
        }

        /// <summary>
        /// Converts the query parameter value to an object.
        /// </summary>
        /// <param name="processingContext">The <see cref="processingContext"/> which holds information about the current value.</param>
        /// <returns>The newly created object.</returns>
        /// <exception cref="ArgumentNullException">The value of 'processingContext' cannot be null. </exception>
        /// <exception cref="ValueProcessorCannotHandleTypeException">The itemValueProcessor can not handle the specified <see cref="Type"/> if elements.</exception>
        public virtual Object Resolve(ParserProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            var splittedValues = Regex.Split(processingContext.SourceValue, processingContext.Configuration.QueryParameterSplitValuesRegexPattern, RegexOptions.Singleline);
            if (!processingContext.DestinationType.IsGenericIEnumerable())
            {
                return splittedValues.AsEnumerable();
            }

            return this.MakeGenericEnumerable(splittedValues, processingContext.DestinationType, processingContext);
        }

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The value of 'type' cannot be null. </exception>
        public virtual Boolean CanHandle(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsIEnumerable() && !type.IsPrimitive();
        }

        /// <summary>
        /// Creates a generic list, converts each item to the destination value and fills the list.
        /// </summary>
        /// <param name="values">The values to convert and add.</param>
        /// <param name="enumerableType">The <see cref="Type"/> of the list.</param>
        /// <param name="processingContext">The supplied <see cref="ParserProcessingContext"/> from the caller.</param>
        /// <exception cref="ValueProcessorCannotHandleTypeException">The itemValueProcessor can not handle the specified <see cref="Type"/> if elements.</exception>
        /// <returns>A newly created list filled with the converted values.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="values"/>' and <paramref name="enumerableType"/> cannot be null. </exception>
        private IEnumerable MakeGenericEnumerable([NotNull] IEnumerable<String> values, [NotNull] Type enumerableType, [NotNull] ParserProcessingContext processingContext)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (enumerableType == null)
            {
                throw new ArgumentNullException(nameof(enumerableType));
            }

            var genericType = enumerableType.GetSingleGenericParameter();
            var checkType = genericType;
            if (genericType.IsNullable())
            {
                checkType = genericType.GetSingleGenericParameter();
            }

            if (!this.itemValueProcessor.CanHandle(checkType))
            {
                throw new ValueProcessorCannotHandleTypeException(this.itemValueProcessor, genericType);
            }

            var resultTypeDefinition = typeof (Collection<>).MakeGenericType(genericType);
            var result = Activator.CreateInstance(resultTypeDefinition);
            var addMethod = resultTypeDefinition.GetMethod("Add", new[] {genericType});
            foreach (var value in values)
            {
                var innerProcessingContext = processingContext.CreateProcessingContext(value, genericType);
                var resolvedValue = this.itemValueProcessor.Resolve(innerProcessingContext);
                addMethod.Invoke(result, new[] {resolvedValue});
            }

            return result as IEnumerable;
        }
    }
}