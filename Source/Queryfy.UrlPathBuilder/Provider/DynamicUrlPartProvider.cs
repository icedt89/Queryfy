namespace JanHafner.Queryfy.UrlPathBuilder.Provider
{
    using System;

    /// <summary>
    /// Constructs an <see cref="UrlPart"/> from the information provided by the <see cref="IProvideDynamicPart"/> implementation.
    /// </summary>
    public sealed class DynamicUrlPartProvider : IUrlPartProvider
    {
        /// <summary>
        /// Constrcucts the part based on the specified <see cref="Object"/>.
        /// </summary>
        /// <param name="source">The current <see cref="Object"/>.</param>
        /// <param name="currentInheritedType">The base <see cref="Type"/> of the current source <see cref="object"/>.</param>
        /// <param name="deep">The current deep of the recursion.</param>
        /// <returns>The constructed <see cref="UrlPart"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="currentInheritedType"/>' cannot be null. </exception>
        public UrlPart ConstructPart(Object source, Type currentInheritedType, Int32 deep)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            if (currentInheritedType == null)
            {
                throw new ArgumentNullException(nameof(currentInheritedType));
            }

            var iProvideDynamicPartType = typeof (IProvideDynamicPart);
            if (!iProvideDynamicPartType.IsAssignableFrom(currentInheritedType))
            {
                throw new InvalidOperationException();
            }

            var interfaceMap = currentInheritedType.GetInterfaceMap(iProvideDynamicPartType);
            if (interfaceMap.TargetMethods[0].DeclaringType != currentInheritedType)
            {
                throw new InvalidOperationException();
            }

            var partName = (String) interfaceMap.TargetMethods[0].Invoke(source, null);
            return new UrlPart(partName, false, deep);
        }
    }
}