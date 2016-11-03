namespace JanHafner.Queryfy
{
    using System;
    using Configuration;
    using Extensions;
    using JetBrains.Annotations;
    using Ninject;
    using Parsing;
    using Queryfication;

    /// <summary>
    /// The <see cref="Mapper"/> class is a facade and provides all the functionality of this framework through static methods.
    /// All static methods in this class act exactly like the methods of the Default-instance.
    /// </summary>
    public static class Mapper
    {
        [NotNull]
        private static readonly IKernel Kernel;

        /// <summary>
        /// Initializes the <see cref="Mapper"/> class.
        /// </summary>
        static Mapper()
        {
            Kernel = new StandardKernel(new QueryfyDotNetRegistrations());
        }

        /// <summary>
        /// The default <see cref="IQueryfyDotNet"/>.
        /// </summary>
        [NotNull]
        public static IQueryfyDotNet Default
        {
            get { return Kernel.Get<IQueryfyDotNet>(); }
        }

        /// <summary>
        /// Configures the facade with the supplied <see cref="IQueryfyConfiguration"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="IQueryfyConfiguration"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="configuration"/>' cannot be null. </exception>
        public static void ConfigureWith([NotNull] IQueryfyConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            Kernel.Rebind<IQueryfyConfiguration>().ToConstant(configuration).InSingletonScope();
        }

        /// <summary>
        /// Configures the facade with the supplied <see cref="IQueryfyConfiguration"/>.
        /// </summary>
        /// <typeparam name="TConfiguration">The <see cref="Type"/> of the <see cref="IQueryfyConfiguration"/>.</typeparam>
        public static void ConfigureWith<TConfiguration>()
            where TConfiguration : IQueryfyConfiguration
        {
            Kernel.Rebind<IQueryfyConfiguration>().To<TConfiguration>().InSingletonScope();
        }

        /// <summary>
        /// Creates a new instance of the implementation of the <see cref="IQueryficationContext"/> interface.
        /// </summary>
        /// <returns>The newly created <see cref="IQueryficationContext"/>.</returns>
        [NotNull]
        public static IQueryficationContext CreateQueryficationContext()
        {
            return Default.CreateQueryficationContext();
        }

        /// <summary>
        /// Creates a new instance of the implementation of the <see cref="IParserContext"/> interface.
        /// </summary>
        /// <returns>The newly created <see cref="IParserContext"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="query"/>' cannot be null. </exception>
        [NotNull]
        public static IParserContext CreateParserContext([NotNull] String query)
        {
            if(String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));    
            }

            return Default.CreateParserContext(query);
        }

        /// <summary>
        /// Creates a query from the supplied source object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>An <see cref="QueryficationResult"/> instance with information about the result.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' cannot be null. </exception>
        [NotNull]
        public static QueryficationResult Queryfy([NotNull] Object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Default.Queryfy(source);
        }

        /// <summary>
        /// Creates a query fromt he supplied source <see cref="Object"/> and <see cref="IQueryficationContext"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="queryficationContext">The <see cref="IQueryficationContext"/>.</param>
        /// <returns>An <see cref="QueryficationResult"/> instance with information about the result.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="queryficationContext"/>' cannot be null. </exception>
        [NotNull]
        public static QueryficationResult Queryfy([NotNull] Object source, [NotNull] IQueryficationContext queryficationContext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            
            if (queryficationContext == null)
            {
                throw new ArgumentNullException(nameof(queryficationContext));
            }

            return Default.Queryfy(source, queryficationContext);
        }

        /// <summary>
        /// Fills the destination object by using the supplied query string.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="query">The query string.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="destination"/>' and '<paramref name="query"/>' cannot be null. </exception>
        public static void Parse([NotNull] Object destination, [NotNull] String query)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            Default.Parse(destination, query);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Type"/> and fills it with the supplied mappings in the <see cref="IParserContext"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the destination.</typeparam>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parserContext"/>' cannot be null. </exception>
        [NotNull]
        public static T Parse<T>([NotNull] IParserContext parserContext)
        {
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            return Default.Parse<T>(parserContext);
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Type"/> and fills it by using the query string.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the destination.</typeparam>
        /// <param name="query">The query string.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="query"/>' cannot be null. </exception>
        [NotNull]
        public static T Parse<T>(String query)
        {
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));    
            }

            return Default.Parse<T>(query);
        }

        /// <summary>
        /// Fills the destination object by using the supplied mappings in the <see cref="IParserContext"/>.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="destination"/>' and '<paramref name="parserContext"/>' cannot be null. </exception>
        public static void Parse([NotNull] Object destination, [NotNull] IParserContext parserContext)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            Default.Parse(destination, parserContext);
        }

        /// <summary>
        /// Fills the destination object by using the supplied mappings in the <see cref="IParserContext"/>.
        /// </summary>
        /// <param name="parserContext">The <see cref="IParserContext"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="parserContext"/>' cannot be null. </exception>
        public static void Parse(IParserContext parserContext)
        {
            if (parserContext == null)
            {
                throw new ArgumentNullException(nameof(parserContext));
            }

            Default.Parse(parserContext);
        }
    }
}