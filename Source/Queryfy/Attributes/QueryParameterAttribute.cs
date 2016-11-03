namespace JanHafner.Queryfy.Attributes
{
    using System;
    using JetBrains.Annotations;
    using Processing;

    /// <summary>
    /// Provides a declarative way of controlling the parsing or queryfication process.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class QueryParameterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        public QueryParameterAttribute()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        public QueryParameterAttribute([CanBeNull] String parameterName)
            : this(parameterName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        /// <param name="valueProcessorType"><see cref="Type"/> of the <see cref="IValueProcessor"/>.</param>
        /// <exception cref="ArgumentException">The <paramref name="valueProcessorType" /> parameter is not assignable from an <see cref="T:JanHafner.Queryfy.Processing.IValueProcessor" />.</exception>
        public QueryParameterAttribute([CanBeNull] Type valueProcessorType)
            : this(null, valueProcessorType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameterAttribute"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="ArgumentException">The <paramref name="valueProcessorType"/> parameter is not assignable from an <see cref="IValueProcessor"/>.</exception>
        /// <param name="valueProcessorType">The <see cref="Type"/> of the <see cref="IValueProcessor"/>.</param>
        public QueryParameterAttribute([CanBeNull] String parameterName, [CanBeNull] Type valueProcessorType)
        {
            this.ParameterName = parameterName;
            if (valueProcessorType == null)
            {
                return;
            }

            if (!typeof (IValueProcessor).IsAssignableFrom(valueProcessorType))
            {
                throw new ArgumentException(String.Format("The Type '{0}' does not implement the IValueProcessor interface.", valueProcessorType.Name));
            }

            this.ValueProcessorType = valueProcessorType;
        }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        [CanBeNull]
        public String ParameterName { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="IValueProcessor"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> of the <see cref="IValueProcessor"/>.
        /// </value>
        [CanBeNull]
        public Type ValueProcessorType { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this parameter should be ignored on a parsing process.
        /// </summary>
        /// <value><see langword="true"/> if the parameter is ignored on parse; otherwise, <see langword="false"/>.</value>
        public Boolean IgnoreOnParse { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether <see langword="this"/> parameter should be ignored on a queryfy process.
        /// </summary>
        /// <value><see langword="true"/> if the parameter is ignored on queryfy; otherwise, <see langword="false"/>.</value>
        public Boolean IgnoreOnQueryfy { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property should be instantiated if its skipped.
        /// </summary>
        /// <value><see langword="true"/> if it should be instantiated on skip; otherwise, <see langword="false"/>.</value>
        public Boolean InstantiateOnSkip { get; private set; }
    }
}