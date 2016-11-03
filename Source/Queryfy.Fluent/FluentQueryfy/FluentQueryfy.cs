namespace JanHafner.Queryfy.Fluent.FluentQueryfy
{
    using System;
    using Inspection;
    using JetBrains.Annotations;
    using Queryfication;

    /// <summary>
    /// Implements <see cref="IFluentQueryfy"/>.
    /// </summary>
    public sealed class FluentQueryfy : IFluentQueryfy
    {
        [NotNull]
        private readonly IQueryfyDotNet queryfyDotNet;

        [NotNull]
        private readonly IQueryficationContext queryficationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentQueryfy"/> class.
        /// </summary>
        /// <param name="queryfyDotNet">An implementation of the <see cref="IQueryfyDotNet"/> interface.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="queryfyDotNet"/>' cannot be null. </exception>
        public FluentQueryfy([NotNull] IQueryfyDotNet queryfyDotNet)
        {
            if (queryfyDotNet == null)
            {
                throw new ArgumentNullException(nameof(queryfyDotNet));
            }

            this.queryfyDotNet = queryfyDotNet;
            this.queryficationContext = this.queryfyDotNet.CreateQueryficationContext();
        }

        /// <summary>
        /// Returns a new parameter with the supplied name.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>A new fluent syntax provider for the <see cref="QueryParameter"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="name"/>' cannot be null. </exception>
        public IFluentQueryParameter Parameter(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));    
            }

            return new FluentQueryParameter(this.queryficationContext, this).Named(name);
        }

        /// <summary>
        /// Returns a new parameter.
        /// </summary>
        /// <returns>A new fluent syntax provider for the <see cref="QueryParameter"/>.</returns>
        public IFluentQueryParameter Parameter()
        {
            return new FluentQueryParameter(this.queryficationContext, this);
        }

        /// <summary>
        /// Processes the parameters.
        /// </summary>
        /// <returns>The <see cref="QueryficationResult"/>.</returns>
        public QueryficationResult Queryfy()
        {
            return this.queryfyDotNet.Queryfy(this.queryficationContext);
        }
    }
}