namespace JanHafner.Queryfy.Inspection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// The <see cref="TypeValueProcessorRegistry"/> class type.
    /// </summary>
    public sealed class TypeValueProcessorRegistry : ITypeValueProcessorRegistry
    {
        [NotNull]
        private readonly IDictionary<Type, IValueProcessor> registrations;

        [NotNull]
        private static volatile ITypeValueProcessorRegistry global;

        /// <summary>
        /// Initializes the <see cref="TypeValueProcessorRegistry"/> class.
        /// </summary>
        static TypeValueProcessorRegistry()
        {
            global = new TypeValueProcessorRegistry();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeValueProcessorRegistry"/> class.
        /// </summary>
        private TypeValueProcessorRegistry()
        {
            this.registrations = new Dictionary<Type, IValueProcessor>();
        }

        /// <summary>
        /// Gets the global instance of this class.
        /// </summary>
        public static ITypeValueProcessorRegistry Global
        {
            get
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse Expression is only false on first call.
                if (global == null)
                {
                    lock (typeof (TypeValueProcessorRegistry))
                    {
                        if (global == null)
                        {
                            global = new TypeValueProcessorRegistry();
                        }
                    }
                }

                return global;
            }
        }

        /// <summary>
        /// Lookup the mapped <see cref="IValueProcessor"/> for the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> for which the mapped instance should be returned.</param>
        /// <param name="result"></param>
        /// <returns><c>true</c> if the mapping was found; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' cannot be null. </exception>
        public Boolean TryLookup(Type type, out IValueProcessor result)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));    
            }

            foreach (var registeredType in this.registrations.OrderBy(p => p.Key.IsGenericType).ThenBy(p => p.Key.IsGenericTypeDefinition))
            {
                if (this.CanHandle(registeredType.Key, type))
                {
                    result = registeredType.Value;
                    return true;
                }
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Maps the <see cref="IValueProcessor"/> to the <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <param name="valueProcessor">The <see cref="IValueProcessor"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' and '<paramref name="valueProcessor"/>' cannot be null. </exception>
        public void SetRegistration(Type type, IValueProcessor valueProcessor)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(valueProcessor));
            }

            this.registrations[type] = valueProcessor;
        }

        /// <summary>
        /// Removes the mapping for the suppied <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' cannot be null. </exception>
        public void RemoveRegistration(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.registrations.Remove(type);
        }

        /// <summary>
        /// Determines whether <see langword="this"/> instance can handle the specified type handler.
        /// </summary>
        /// <param name="typeHandler">The type handler.</param>
        /// <param name="type">The <see cref="Type"/>.</param>
        /// <returns>
        ///   <c>true</c> if <see langword="this"/> instance can handle the specified type handler; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' and '<paramref name="typeHandler"/>' cannot be null. </exception>
        private Boolean CanHandle([NotNull] Type typeHandler, [NotNull] Type type)
        {
            if (typeHandler == null)
            {
                throw new ArgumentNullException(nameof(typeHandler));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (typeHandler.IsAssignableFrom(type))
            {
                return true;
            }

            if (typeHandler.IsGenericTypeDefinition && type.IsGenericType)
            {
                return typeHandler == type.GetGenericTypeDefinition();
            }

            return false;
        }
    }
}