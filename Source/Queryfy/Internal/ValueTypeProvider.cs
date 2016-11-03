namespace JanHafner.Queryfy
{
    using System;
    using JetBrains.Annotations;
    using Ninject.Activation;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// The <see cref="ValueTypeProvider"/> class provides instances of <see cref="ValueType"/>`s.
    /// </summary>
    internal sealed class ValueTypeProvider : IProvider
    {
        /// <summary>
        /// Gets the type (or prototype) of instances the provider creates.
        /// </summary>
        [CanBeNull]
        public Type Type { get; private set; }

        /// <summary>
        /// Creates an instance within the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// The created instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="context"/>' cannot be null. </exception>
        [CanBeNull]
        public Object Create([NotNull] IContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Request.Service == typeof (String) ? String.Empty : context.Request.Service.GetDefault();
        }
    }
}