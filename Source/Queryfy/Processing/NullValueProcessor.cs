namespace JanHafner.Queryfy.Processing
{
    using System;
    using JetBrains.Annotations;
    using Parsing;
    using Queryfication;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Does nothing. ONLY purpose is to provide query parameters that have no value. They are expressed like this: ?valueless=&amp;withValue=1.
    /// This class is special handled by the default implementation of <see cref="IQueryficationBuilder"/>
    /// </summary>
    public sealed class NullValueProcessor : IValueProcessor
    {
        /// <summary>
        /// Converts the object to the query parameter value.
        /// Always returns <c>null</c>.
        /// </summary>
        /// <param name="processingContext">The <see cref="QueryficationProcessingContext"/> which holds information about the current value.</param>
        /// <returns>The <see cref="string"/> used in the query.</returns>
        public String Process([CanBeNull] QueryficationProcessingContext processingContext)
        {
            return null;
        }

        /// <summary>
        /// Converts the query parameter value to an object.
        /// Always returns the default value of the <see cref="Type"/>.
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

            return processingContext.DestinationType.GetDefault();
        }

        /// <summary>
        /// Determines whether this instance can handle the specified type. Returns always <c>true</c>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.
        /// </returns>
        public Boolean CanHandle([CanBeNull] Type type)
        {
            return true;
        }
    }
}