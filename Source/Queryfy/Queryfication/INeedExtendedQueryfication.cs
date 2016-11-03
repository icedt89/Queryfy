namespace JanHafner.Queryfy.Queryfication
{
    using Inspection;
    using JetBrains.Annotations;

    /// <summary>
    /// Instructs the <see cref="IQueryficationEngine"/> to execute user defined code after the call to <see cref="IQueryficationEngine.Fill"/>.
    /// </summary>
    public interface INeedExtendedQueryfication
    {
        /// <summary>
        /// Executes user defined code to manipulate the <see cref="IQueryficationContext"/>.
        /// </summary>
        /// <param name="queryficationContext">The <see cref="IQueryficationContext"/>.</param>
        void Queryfy([NotNull]IQueryficationContext queryficationContext);
    }
}