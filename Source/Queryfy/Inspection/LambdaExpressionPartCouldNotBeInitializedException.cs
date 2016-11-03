namespace JanHafner.Queryfy.Inspection
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;
    using Properties;

    /// <summary>
    /// Defines the Exception that is thrown if the a single part of the <see cref="LambdaExpression"/> could not be initialized.
    /// </summary>
    [Serializable]
    public sealed class LambdaExpressionPartCouldNotBeInitializedException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaExpressionPartCouldNotBeInitializedException"/> class.
        /// </summary>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"/> which coould not be initialized.</param>
        /// <param name="lambdaExpression">The whole <see cref="LambdaExpression"/> that could not be initialized.</param>
        /// <param name="formattedPartDiagnostic">A formatted diagnostic string which part is defect.</param>
        /// <exception cref="ArgumentNullException">The value of '<paramref name="formattedPartDiagnostic"/>' cannot be null. </exception>
        public LambdaExpressionPartCouldNotBeInitializedException([NotNull] PropertyInfo propertyInfo, [NotNull] LambdaExpression lambdaExpression, [NotNull] String formattedPartDiagnostic)
            : base(String.Format(ExceptionMessages.LambdaExpressionPartCouldNotBeInitializedExceptionMessage, propertyInfo.Name, propertyInfo.PropertyType, propertyInfo.DeclaringType.Name, lambdaExpression, formattedPartDiagnostic))
        {
            if (String.IsNullOrEmpty(formattedPartDiagnostic))
            {
                throw new ArgumentNullException(nameof(formattedPartDiagnostic));
            }

            this.PropertyInfo = propertyInfo;
            this.LambdaExpression = lambdaExpression;
            this.FormattedPartDiagnostic = formattedPartDiagnostic;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaExpressionPartCouldNotBeInitializedException"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public LambdaExpressionPartCouldNotBeInitializedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/>.
        /// </summary>
        [NotNull]
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Gets the <see cref="LambdaExpression"/>.
        /// </summary>
        [NotNull]
        public LambdaExpression LambdaExpression { get; set; }
        
        /// <summary>
        /// Gets the diagnostics.
        /// </summary>
        [NotNull]
        public string FormattedPartDiagnostic { get; set; }
    }
}