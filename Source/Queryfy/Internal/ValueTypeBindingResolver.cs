namespace JanHafner.Queryfy
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Ninject;
    using Ninject.Activation;
    using Ninject.Infrastructure;
    using Ninject.Planning.Bindings;
    using Ninject.Planning.Bindings.Resolvers;

    /// <summary>
    /// The <see cref="ValueTypeBindingResolver"/> class provides a service that resolves not bound instances of <see cref="ValueType"/>`s.
    /// </summary>
    [UsedImplicitly]
    internal sealed class ValueTypeBindingResolver : IMissingBindingResolver
    {
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        [CanBeNull]
        public INinjectSettings Settings { get; set; }

        /// <summary>
        /// Returns any bindings from the specified collection that match the specified request.
        /// </summary>
        /// <param name="bindings">The multimap of all registered bindings.</param>
        /// <param name="request">The request in question.</param>
        /// <returns>
        /// The series of matching bindings.
        /// </returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="request"/>' cannot be null. </exception>
        [LinqTunnel]
        [NotNull]
        public IEnumerable<IBinding> Resolve([CanBeNull] Multimap<Type, IBinding> bindings, [NotNull] IRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Service == typeof (String) || request.Service.IsValueType)
            {
                yield return new Binding(request.Service, new BindingConfiguration
                                                          {
                                                              Target = BindingTarget.Provider,
                                                              ProviderCallback = c => new ValueTypeProvider()
                                                          });
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }
    }
}