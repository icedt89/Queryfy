namespace JanHafner.Queryfy.Processing
{
    using System;
    using System.Reflection;
    using Extensions;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// Creates a dummy <see cref="Func{TResult}"/> and supplies the converted value from the query parameter as constructor parameter for the <see cref="Lazy{T}"/>.
    /// </summary>
    public sealed class LazyValueProcessor : IValueProcessor
    {
        [NotNull]
        private readonly IValueProcessor innerValueProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyValueProcessor"/> class.
        /// </summary>
        /// <param name="innerValueProcessor">An implementation of the <see cref="IValueProcessor"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="innerValueProcessor"/>' cannot be null. </exception>
        public LazyValueProcessor([NotNull] IValueProcessor innerValueProcessor)
        {
            if (innerValueProcessor == null)
            {
                throw new ArgumentNullException(nameof(innerValueProcessor));
            }

            this.innerValueProcessor = innerValueProcessor;
        }

        /// <summary>
        /// Converts the object to the query parameter value.
        /// </summary>
        /// <param name="processingContext">The <see cref="QueryficationProcessingContext"/> which holds information about the current value.</param>
        /// <returns>The <see cref="String"/> used in the query.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="processingContext"/>' cannot be null. </exception>
        public String Process(QueryficationProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            dynamic dynamicLazy = processingContext.SourceValue;
            return this.innerValueProcessor.Process(dynamicLazy.Value);
        }

        /// <summary>
        /// Converts the query parameter value to an object.
        /// </summary>
        /// <param name="processingContext">The <see cref="processingContext"/> which holds information about the current value.</param>
        /// <returns>The newly created object.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="processingContext"/>' cannot be null. </exception>
        public Object Resolve(ParserProcessingContext processingContext)
        {
            if (processingContext == null)
            {
                throw new ArgumentNullException(nameof(processingContext));
            }

            var lazyType = typeof (Lazy<>);
            var lazyGenericType = processingContext.DestinationType.GetSingleGenericParameter();
            var genericLazy = lazyType.MakeGenericType(lazyGenericType);
            var resolvedValue = this.innerValueProcessor.Resolve(processingContext.CreateProcessingContext(processingContext.SourceValue, lazyGenericType));
            var lazy = Activator.CreateInstance(genericLazy, this.CreateActivatorFunction(resolvedValue, lazyGenericType));
            return lazy;
        }

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' cannot be null. </exception>
        public Boolean CanHandle(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsLazy();
        }

        /// <summary>
        /// Calls the <see cref="CreateActivatorFunctionCore{T}"/>-Method with reflection to create the constructor argument for the <see cref="Lazy{T}"/>.
        /// </summary>
        /// <param name="returnValue">The value that will be used as return value for the <see cref="Func{T}"/>.</param>
        /// <param name="lazyGenericType">The generic <see cref="Type"/> argument of the <see cref="Lazy{T}"/>.</param>
        /// <returns>A <see cref="Func{T}"/> that will be passed to the constructor of the <see cref="Lazy{T}"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="lazyGenericType"/>' cannot be null. </exception>
        [NotNull]
        private Object CreateActivatorFunction([CanBeNull] Object returnValue, [NotNull] Type lazyGenericType)
        {
            if (lazyGenericType == null)
            {
                throw new ArgumentNullException(nameof(lazyGenericType));
            }

            var method = this.GetType().GetMethod("CreateActivatorFunctionCore", BindingFlags.NonPublic | BindingFlags.Static);
            return method.MakeGenericMethod(lazyGenericType).Invoke(null, new[]
                                                                          {
                                                                              returnValue
                                                                          });
        }

        /// <summary>
        /// Returns a <see cref="Func{T}"/> that returns the supplied value.
        /// </summary>
        /// <typeparam name="T">The return <see cref="Type"/> of the <see cref="Func{T}"/>.</typeparam>
        /// <param name="returnValue">The value that will be used as return value for the <see cref="Func{T}"/>.</param>
        /// <returns>A new <see cref="Func{T}"/> that returns the supplied value.</returns>
        [NotNull]
        [UsedImplicitly]
        private static Func<T> CreateActivatorFunctionCore<T>([CanBeNull] T returnValue)
        {
            return () => returnValue;
        }
    }
}