namespace JanHafner.Queryfy.Parsing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Defines the <see cref="ParserMapCollection"/> type.
    /// </summary>
    public sealed class ParserMapCollection : ICollection<PropertyParserMap>
    {
        [NotNull]
        private readonly ICollection<PropertyParserMap> internalList = new List<PropertyParserMap>();

        /// <inheritdoc />
        public Int32 Count
        {
            get { return this.internalList.Count; }
        }

        /// <inheritdoc />
        public Boolean IsReadOnly
        {
            get { return this.internalList.IsReadOnly; }
        }

        /// <inheritdoc />
        [LinqTunnel]
        public IEnumerator<PropertyParserMap> GetEnumerator()
        {
            return this.internalList.GetEnumerator();
        }

        /// <inheritdoc />
        [LinqTunnel]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">The value of '<paramref name="item"/>' cannot be null. </exception>
        public void Add([NotNull] PropertyParserMap item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (this.internalList.Any(map => map.Property == item.Property))
            {
                this.internalList.Remove(map => map.Property == item.Property);
            }

            this.internalList.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.internalList.Clear();
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">The value of '<paramref name="item"/>' cannot be null. </exception>
        public Boolean Contains([NotNull] PropertyParserMap item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return this.internalList.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(PropertyParserMap[] array, Int32 arrayIndex)
        {
            this.internalList.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">The value of '<paramref name="item"/>' cannot be null. </exception>
        public Boolean Remove([NotNull] PropertyParserMap item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return this.internalList.Remove(item);
        }
    }
}