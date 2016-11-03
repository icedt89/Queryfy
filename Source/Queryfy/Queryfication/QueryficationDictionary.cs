namespace JanHafner.Queryfy.Queryfication
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Configuration;
    using Extensions;
    using Inspection;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="QueryficationDictionary"/> class provides special methods for working with query strings.
    /// </summary>
    public sealed class QueryficationDictionary : IDictionary<String, QueryParameter>
    {
        [NotNull]
        private readonly IDictionary<String, QueryParameter> internalDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryficationDictionary"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The value of 'sourceQuery' cannot be null. </exception>
        private QueryficationDictionary(String sourceQuery)
            : this()
        {
            if (String.IsNullOrEmpty(sourceQuery))
            {
                throw new ArgumentNullException(nameof(sourceQuery));
            }

            this.SourceQuery = sourceQuery;
        }

         /// <summary>
        /// Initializes a new instance of the <see cref="QueryficationDictionary"/> class.
        /// </summary>
        public QueryficationDictionary()
        {
            this.internalDictionary = new Dictionary<String, QueryParameter>();
        }

        /// <summary>
        /// Gets the source query.
        /// </summary>
        [CanBeNull]
        public String SourceQuery { get; private set; }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<String, QueryParameter>> GetEnumerator()
        {
            return this.internalDictionary.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.internalDictionary.GetEnumerator();
        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">The key of the item is not equal the name of the parameter.</exception>
        public void Add(KeyValuePair<String, QueryParameter> item)
        {
            if (item.Key != item.Value.ParameterName)
            {
                throw new InvalidOperationException(String.Format("The key '{0}' must be equal to the parametername '{1}'.", item.Key, item.Value.ParameterName));
            }

            if (this.ContainsKey(item.Key))
            {
                this[item.Key] = item.Value;
            }
            else
            {
                this.internalDictionary.Add(item);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.internalDictionary.Clear();
        }

        /// <inheritdoc />
        public Boolean Contains(KeyValuePair<String, QueryParameter> item)
        {
            return this.internalDictionary.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<String, QueryParameter>[] array, Int32 arrayIndex)
        {
            this.internalDictionary.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public Boolean Remove(KeyValuePair<String, QueryParameter> item)
        {
            return this.internalDictionary.Remove(item);
        }

        /// <inheritdoc />
        public Int32 Count
        {
            get { return this.internalDictionary.Count; }
        }

        /// <inheritdoc />
        public Boolean IsReadOnly
        {
            get { return this.internalDictionary.IsReadOnly; }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        public Boolean ContainsKey(String key)
        {
            return this.internalDictionary.ContainsKey(key);
        }

        /// <inheritdoc />
        public void Add([NotNull] String key, [NotNull] QueryParameter value)
        {
            this.Add(new KeyValuePair<String, QueryParameter>(key, value));
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        public Boolean Remove(String key)
        {
            return this.internalDictionary.Remove(key);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        public Boolean TryGetValue(String key, [CanBeNull] out QueryParameter value)
        {
            return this.internalDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <value></value>
        /// <exception cref="ArgumentNullException" accessor="set"><paramref name="key" /> is null.</exception>
        /// <exception cref="KeyNotFoundException" accessor="set">The property is retrieved and <paramref name="key" /> is not found.</exception>
        public QueryParameter this[String key]
        {
            get { return this.internalDictionary[key]; }
            set { this.internalDictionary[key] = value; }
        }

        /// <inheritdoc />
        public ICollection<String> Keys
        {
            get { return this.internalDictionary.Keys; }
        }

        /// <inheritdoc />
        public ICollection<QueryParameter> Values
        {
            get { return this.internalDictionary.Values; }
        }

        /// <summary>
        /// Factory method for creating <see cref="QueryficationDictionary"/> instances from a query string.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="configuration">The <see cref="IQueryfyConfiguration"/></param>
        /// <returns>An filled instance of the <see cref="QueryficationDictionary"/> class.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="configuration"/>' cannot be null. </exception>
        [NotNull]
        public static QueryficationDictionary FromQueryString([NotNull] String query, [NotNull] IQueryfyConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));    
            }

            var result = new QueryficationDictionary(query);
            var matches = Regex.Matches(query, configuration.QueryParameterRegexPattern, RegexOptions.Singleline);
            foreach (var parameterGroup in matches.AsEnumerable().GroupBy(match => match.Groups[1].Value))
            {
                var concatenatedValues = parameterGroup.Aggregate(new StringBuilder(), (seed, current) => seed.Append(current.Groups[3].Value + "+"), seed => seed.ToString());
                concatenatedValues = concatenatedValues.Remove(concatenatedValues.Length - 1, 1);
                result.Add(parameterGroup.Key, new QueryParameter(parameterGroup.Key, concatenatedValues));
            }

            return result;
        }
    }
}