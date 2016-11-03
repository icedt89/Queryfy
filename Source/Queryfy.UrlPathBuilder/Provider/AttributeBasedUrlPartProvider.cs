namespace JanHafner.Queryfy.UrlPathBuilder.Provider
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Toolkit.Common.ExtensionMethods;

    /// <summary>
    /// Constructs an <see cref="UrlPart"/> from the information provided by the <see cref="UrlPartAttribute"/> attribute.
    /// </summary>
    public sealed class AttributeBasedUrlPartProvider : IUrlPartProvider
    {
        /// <summary>
        /// Construct the part based on the specified <see cref="Object"/>.
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

            var customAttributes = currentInheritedType.GetCustomAttributes(typeof (UrlPartAttribute), false);
            if (customAttributes.Length == 0)
            {
                throw new InvalidOperationException();
            }

            var uriPartAttribute = (UrlPartAttribute) customAttributes[0];
            var partName = uriPartAttribute.PartName;
            if (String.IsNullOrWhiteSpace(partName))
            {
                throw new InvalidOperationException();
            }

            var replacementMatches = Regex.Matches(partName, "{(.*?)}", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase).Cast<Match>().ToList();
            if (replacementMatches.Any())
            {
                foreach (var match in replacementMatches)
                {
                    var memberName = match.Value.Replace("{", String.Empty).Replace("}", String.Empty);
                    var memberInfo = currentInheritedType.GetMember(memberName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (!memberInfo.Any() || memberInfo.Count() > 1)
                    {
                        throw new InvalidOperationException();
                    }

                    var memberValue = memberInfo[0].GetValue(source);
                    if (memberValue == null)
                    {
                        throw new InvalidOperationException();    
                    }

                    partName = Regex.Replace(partName, String.Format("{{{0}}}", memberName), memberValue.ToString());
                }
            }

            return new UrlPart(partName, uriPartAttribute.IsIgnored, deep);
        }
    }
}