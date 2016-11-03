namespace JanHafner.Queryfy.Extensions
{
    using System;
    using Inspection;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// Provides extensions for the <see cref="IInstanceFactory"/> interface.
    /// </summary>
    public static class InstanceFactoryExtensions
    {
        /// <summary>
        /// Creates an instance of the supplied <see cref="Type"/>.
        /// </summary>
        /// <exception cref="CannotCreateInstanceOfTypeException">If no instance could be created.</exception>
        /// <typeparam name="T">The <see cref="Type"/> from which an istance must be created.</typeparam>
        /// <param name="instanceFactory">The <see cref="IInstanceFactory"/>.</param>
        /// <returns>The instance of the <see cref="Type"/>.</returns>
        /// <exception cref="CannotCreateInstanceOfTypeException">If no instance could be created.</exception>
        /// <exception cref="ArgumentNullException">The value of 'instanceFactory' cannot be null. </exception>
        [NotNull]
        public static T CreateInstance<T>([NotNull] this IInstanceFactory instanceFactory)
        {
            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            return instanceFactory.CreateInstance<T>(typeof (T));
        }

        /// <summary>
        /// Creates an instance of the supplied <see cref="Type"/>.
        /// </summary>
        /// <exception cref="CannotCreateInstanceOfTypeException">If no instance could be created.</exception>
        /// <typeparam name="T">The <see cref="Type"/> or an inherited version.</typeparam>
        /// <param name="type">The <see cref="Type"/> from which an istance must be created.</param>
        /// <param name="instanceFactory">The <see cref="IInstanceFactory"/>.</param>
        /// <returns>The instance of the <see cref="Type"/>.</returns>
        /// <exception cref="CannotCreateInstanceOfTypeException">If no instance could be created.</exception>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="instanceFactory"/>' and '<paramref name="type"/>' cannot be null. </exception>
        [NotNull]
        public static T CreateInstance<T>(this IInstanceFactory instanceFactory, [NotNull] Type type)
        {
            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return (T)instanceFactory.CreateInstance(type);
        }

        /// <summary>
        /// Creates an instance of the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="instanceFactory">The <see cref="IInstanceFactory"/>.</param>
        /// <param name="type">The <see cref="Type"/> from which an istance must be created.</param>
        /// <exception cref="CannotCreateInstanceOfTypeException">If no instance could be created.</exception>
        /// <returns>The instance of the <see cref="Type"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="instanceFactory"/>' and '<paramref name="type"/>' cannot be null. </exception>
        [NotNull]
        public static Object CreateInstance([NotNull] this IInstanceFactory instanceFactory, [NotNull] Type type)
        {
            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Object instance;
            if (!instanceFactory.TryCreateInstance(type, out instance) || instance == null)
            {
                throw new CannotCreateInstanceOfTypeException(type, instanceFactory);
            }

            return instance;
        }

        /// <summary>
        /// Tries to create an instance of the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="instanceFactory">The <see cref="IInstanceFactory"/>.</param>
        /// <typeparam name="T">The <see cref="Type"/> from which an istance must be created.</typeparam>
        /// <param name="result"><c>Null</c> or an instance of the expected <see cref="Type"/>.</param>
        /// <returns><c>true</c> if the creation of the instance was successful; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="instanceFactory"/>' cannot be null. </exception>
        public static Boolean TryCreateInstance<T>([NotNull] this IInstanceFactory instanceFactory, [CanBeNull] out T result)
        {
            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            return instanceFactory.TryCreateInstance(typeof (T), out result);
        }

        /// <summary>
        /// Tries to create an instance of the supplied <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> or an inherited version.</typeparam>
        /// <param name="instanceFactory">The <see cref="IInstanceFactory"/>.</param>
        /// <param name="type">The <see cref="Type"/> from which an istance must be created.</param>
        /// <param name="result"><c>Null</c> or an instance of the expected <see cref="Type"/>.</param>
        /// <returns><c>true</c> if the creation of the instance was successful; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="instanceFactory"/>' and '<paramref name="type"/>' cannot be null. </exception>
        public static Boolean TryCreateInstance<T>([NotNull] this IInstanceFactory instanceFactory, [NotNull] Type type, [CanBeNull] out T result)
        {
            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));    
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Object instance;
            var returnValue = instanceFactory.TryCreateInstance(type, out instance);
            if (!returnValue)
            {
                result = default(T);
                return false;
            }

            result = (T) instance;
            return true;
        }
    }
}