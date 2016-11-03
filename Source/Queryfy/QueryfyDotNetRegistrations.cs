namespace JanHafner.Queryfy
{
    using Configuration;
    using Inspection;
    using Inspection.MetadataResolution;
    using JanHafner.Toolkit.Common.Reflection;
    using Ninject.Modules;
    using Ninject.Planning.Bindings.Resolvers;
    using Parsing;
    using Processing;
    using Queryfication;

    /// <summary>
    /// The <see cref="QueryfyDotNetRegistrations"/> class provides initializations for the <see cref="Mapper"/> facade class.
    /// </summary>
    public sealed class QueryfyDotNetRegistrations : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Bind<IPropertyMetadataFactory>().To<PropertyMetadataFactory>().InSingletonScope();
            this.Bind<IQueryfyConfiguration>().ToConstant(QueryfyConfigurationSection.GetXmlConfiguration()).InSingletonScope();
            this.Bind<IPropertyReflectorSelector>().To<PropertyReflectorSelector>().InSingletonScope();
            this.Bind<IPropertyReflector>().To<ExpandoObjectPropertyReflector>().InSingletonScope();
            this.Bind<IPropertyReflector>().To<DynamicObjectPropertyReflector>().InSingletonScope();
            this.Bind<IPropertyReflector>().To<PropertyReflector>().InSingletonScope();
            this.Bind<IQueryficationBuilder>().To<QueryficationBuilder>().InSingletonScope();
            this.Bind<IQueryfyDotNet>().To<QueryfyDotNet>().InSingletonScope();
            this.Bind<IInstanceFactory>().To<InstanceFactory>().InSingletonScope();
            this.Bind<ITypeValueProcessorRegistry>().ToConstant(TypeValueProcessorRegistry.Global);
            this.Bind<IContextFactory>().To<ContextFactory>().InSingletonScope();
            this.Bind<IQueryficationEngine>().To<QueryficationEngine>().InSingletonScope();
            this.Bind<IValueProcessorFactory>().To<ValueProcessorFactory>().InSingletonScope();
            this.Bind<IQueryficationContext>().To<QueryficationContext>();
            this.Bind<IParserContext>().To<ParserContext>();
            this.Bind<ILambdaExpressionInitializer>().To<LambdaExpressionInitializer>().InSingletonScope();
            this.Kernel.Components.Add<IMissingBindingResolver, SelfBindingResolver>();
            this.Kernel.Components.Add<IMissingBindingResolver, ValueTypeBindingResolver>();
        }
    }
}