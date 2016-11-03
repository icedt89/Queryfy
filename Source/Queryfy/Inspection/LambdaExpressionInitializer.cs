namespace JanHafner.Queryfy.Inspection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using JetBrains.Annotations;
    using Processing;
    using Toolkit.Common.Properties;

    /// <summary>
    /// The LambdaExpressionInitializer class type.
    /// </summary>
    public sealed class LambdaExpressionInitializer : ExpressionVisitor, ILambdaExpressionInitializer
    {
        [NotNull]
        private readonly IInstanceFactory instanceFactory;

        [NotNull]
        private readonly ICollection<VisitedPropertyNode> visitedPropertyNodes = new List<VisitedPropertyNode>();

        [CanBeNull]
        private Object currentSourceInstance;

        [CanBeNull]
        private LambdaExpression lambdaExpressionToInitialize;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaExpressionInitializer"/> class.
        /// </summary>
        /// <param name="instanceFactory">The <see cref="IInstanceFactory"/>.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="instanceFactory"/>' cannot be null. </exception>
        public LambdaExpressionInitializer([NotNull] IInstanceFactory instanceFactory)
        {
            if (instanceFactory == null)
            {
                throw new ArgumentNullException(nameof(instanceFactory));
            }

            this.instanceFactory = instanceFactory;
        }

        /// <inheritdoc />
        /// <exception cref="MemberIsNotWritableException">The current part of the <see cref="LambdaExpression"/> is not writable.</exception>
        /// <exception cref="LambdaExpressionPartCouldNotBeInitializedException">The current part of the <see cref="LambdaExpression"/> can not be null.</exception>
        /// <exception cref="ArgumentException">The MemberInfo can not be converted to a PropertyInfo.</exception>
        protected override Expression VisitMember(MemberExpression node)
        {
            var propertyInfo = node.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException(ExceptionMessages.MemberInfoIsNotPropertyInfoExceptionMessage);    
            }

            var baseNode = base.VisitMember(node);

            // ReSharper disable once AssignNullToNotNullAttribute Can not be null at the time this method is called from the base class!
            this.visitedPropertyNodes.Add(new VisitedPropertyNode(this.currentSourceInstance, propertyInfo));

            Object result = null;
            if (propertyInfo.CanRead)
            {
                result = propertyInfo.GetValue(this.currentSourceInstance);
            }

            if (result == null || !propertyInfo.CanRead)
            {
                if (!propertyInfo.CanWrite)
                {
                    throw new MemberIsNotWritableException(propertyInfo);
                }

                if (!this.instanceFactory.TryCreateInstance(propertyInfo.PropertyType, out result))
                {
                    var diagnosticString = this.GetDiagnosticStringForBrokenPart(propertyInfo);
                    // ReSharper disable once AssignNullToNotNullAttribute Can not be null at the time this method is called from the base class!
                    throw new LambdaExpressionPartCouldNotBeInitializedException(propertyInfo, this.lambdaExpressionToInitialize, diagnosticString);
                }

                propertyInfo.SetValue(this.currentSourceInstance, result);
            }
        
            this.currentSourceInstance = result;
            return baseNode;
        }

        /// <summary>
        /// Returns the diagnostic string which describes which part in the <see cref="LambdaExpression"/> is broken.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="propertyInfo"/>' cannot be null. </exception>
        [NotNull]
        private String GetDiagnosticStringForBrokenPart([NotNull] PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            return String.Join(".", this.visitedPropertyNodes.Select(visitedNode => visitedNode.PropertyInfo == propertyInfo ? String.Format("--->>>{0}<<<---", propertyInfo.Name) : propertyInfo.Name));
        }

        /// <summary>
        /// Initializes the specified <see cref="LambdaExpression"/>.
        /// </summary>
        /// <param name="source">The source parameter for the <see cref="LambdaExpression"/>.</param>
        /// <param name="propertyPath">The <see cref="LambdaExpression"/>.</param>
        /// <param name="lastPropertyInfo">Returns the last initialized <see cref="PropertyInfo"/>.</param>
        /// <returns>The last created instance of the <see cref="PropertyInfo"/>.</returns>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="propertyPath"/>' cannot be null. </exception>
        public Object Initialize(Object source, LambdaExpression propertyPath, out PropertyInfo lastPropertyInfo)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));   
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(propertyPath));
            }

            this.lambdaExpressionToInitialize = propertyPath;
            this.currentSourceInstance = source;

            VisitedPropertyNode lastNode;
            try
            {
                this.Visit(propertyPath);
                lastNode = this.visitedPropertyNodes.Last();
            }
            finally 
            {
                this.currentSourceInstance = null;
                this.lambdaExpressionToInitialize = null;
                this.visitedPropertyNodes.Clear();
            }
            
            lastPropertyInfo = lastNode.PropertyInfo;
            return lastNode.Source;
        }

        /// <summary>
        /// The <see cref="VisitedPropertyNode"/> class holds information about a single processed <see cref="PropertyInfo"/> of the <see cref="LambdaExpression"/>.
        /// </summary>
        private sealed class VisitedPropertyNode
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="VisitedPropertyNode"/> class.
            /// </summary>
            /// <param name="source">The source object in which the <see cref="PropertyInfo"/> resides.</param>
            /// <param name="propertyInfo">The <see cref="PropertyInfo"/>.</param>
            /// <exception cref="ArgumentNullException">The value of '<paramref name="source"/>' and '<paramref name="propertyInfo"/>' cannot be null. </exception>
            public VisitedPropertyNode([NotNull] Object source, [NotNull] PropertyInfo propertyInfo)
            {
                if (source == null)
                {
                    throw new ArgumentNullException(nameof(source));
                }

                if (propertyInfo == null)
                {
                    throw new ArgumentNullException(nameof(propertyInfo));
                }

                this.Source = source;
                this.PropertyInfo = propertyInfo;
            }

            /// <summary>
            /// Gets the source <see langword="object"/> in which the <see cref="PropertyInfo"/> resides.
            /// </summary>
            [NotNull]
            public Object Source { get; private set; }

            /// <summary>
            /// Gets the <see cref="PropertyInfo"/>.
            /// </summary>
            [NotNull]
            public PropertyInfo PropertyInfo { get; private set; }
        }
    }
}