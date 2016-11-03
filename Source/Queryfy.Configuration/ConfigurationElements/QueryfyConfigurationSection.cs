namespace JanHafner.Queryfy.Configuration
{
    using System;
    using System.Configuration;
    using JetBrains.Annotations;

    /// <summary>
    /// The <see cref="QueryfyConfigurationSection"/> class type.
    /// </summary>
    public sealed class QueryfyConfigurationSection : ConfigurationSection, IQueryfyConfiguration
    {
        /// <summary>
        /// Used to separate values. E.g. value1+value2+value3.
        /// </summary>
        [ConfigurationProperty("queryParameterValueToValueSeparatorChar", DefaultValue = '+')]
        public Char QueryParameterValueToValueSeparatorChar
        {
            get { return (Char) this["queryParameterValueToValueSeparatorChar"]; }
        }

        /// <summary>
        /// Used to separate the name of the parameter from the value(s). E.g. List=value1.
        /// </summary>
        [ConfigurationProperty("queryParameterValueSeparatorChar", DefaultValue = '=')]
        public Char QueryParameterValueSeparatorChar
        {
            get { return (Char) this["queryParameterValueSeparatorChar"]; }
        }

        /// <summary>
        /// Used to separate parameters. E.g. List=value1+value2&Some=property.
        /// </summary>
        [ConfigurationProperty("queryParametersSeparatorChar", DefaultValue = '&')]
        public Char QueryParametersSeparatorChar
        {
            get { return (Char) this["queryParametersSeparatorChar"]; }
        }

        /// <summary>
        /// Gets the pattern which splits query parameters from the query string.
        /// </summary>
        [ConfigurationProperty("queryParameterRegexPattern", DefaultValue = "([^?=&]+)(=([^&]*))?")]
        public String QueryParameterRegexPattern
        {
            get { return this["queryParameterRegexPattern"] as String; }
        }

        /// <summary>
        /// Gets the pattern which splits array-like values from the query string.
        /// </summary>
        [ConfigurationProperty("queryParameterSplitValuesRegexPattern", DefaultValue = "[+]")]
        public String QueryParameterSplitValuesRegexPattern
        {
            get { return this["queryParameterSplitValuesRegexPattern"] as String; }
        }

        /// <summary>
        /// Gets the <see cref="IParserConfiguration"/>.
        /// </summary>
        public IParserConfiguration ParserConfiguration
        {
            get { return this.ParserConfigurationElement; }
        }

        /// <summary>
        /// Gets the parser configuration.
        /// </summary>
        [ConfigurationProperty("Parser", DefaultValue = null)]
        internal ParserConfigurationElement ParserConfigurationElement
        {
            get { return (ParserConfigurationElement) this["Parser"]; }
        }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        public static IQueryfyConfiguration GetXmlConfiguration()
        {
            return QueryfyConfigurationSection.GetConfigurationSection();
        }

        /// <summary>
        /// Gets the configuration section.
        /// </summary>
        /// <returns></returns>
        [CanBeNull]
        internal static QueryfyConfigurationSection GetConfigurationSection()
        {
            return (QueryfyConfigurationSection)ConfigurationManager.GetSection("Queryfy");
        }
    }
}