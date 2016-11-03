namespace JanHafner.Queryfy.Processing
{
    using System;
    using JetBrains.Annotations;
    using Ninject;
    using Ninject.Syntax;

    /// <summary>
    /// The <see cref="InstanceFactory"/> class.
    /// </summary>
    public sealed class InstanceFactory : IInstanceFactory
    {
        [NotNull]
        private readonly IResolutionRoot resolutionRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceFactory"/> class.
        /// </summary>
        /// <param name="resolutionRoot">The <see cref="IResolutionRoot"/> supplied from the Ninject <see cref="IKernel"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="resolutionRoot"/>' cannot be null. </exception>
        public InstanceFactory([NotNull] IResolutionRoot resolutionRoot)
        {
            if (resolutionRoot == null)
            {
                throw new ArgumentNullException(nameof(resolutionRoot));    
            }

            this.resolutionRoot = resolutionRoot;
        }

        /// <summary>
        /// Tries to create an instance of the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> from which an istance must be created.</param>
        /// <param name="result"><c>Null</c> or an instance of the expected <see cref="Type"/>.</param>
        /// <returns><c>true</c> if the creation of the instance was successful; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="type"/>' cannot be null. </exception>
        public Boolean TryCreateInstance(Type type, out Object result)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));    
            }

            result = this.resolutionRoot.TryGet(type);
            return result != null;
        }
    }
}