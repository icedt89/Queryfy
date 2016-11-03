﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JanHafner.Queryfy.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("JanHafner.Queryfy.Properties.ExceptionMessages", typeof(ExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Type &apos;{0}&apos; can not be instantiated from the IInstanceFactory &apos;{1}&apos;..
        /// </summary>
        internal static string CannotCreateInstanceOfTypeExceptionMessage {
            get {
                return ResourceManager.GetString("CannotCreateInstanceOfTypeExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The enum &apos;{0}&apos; does not contain any members..
        /// </summary>
        internal static string EnumDoesNotContainMembersExceptionMessage {
            get {
                return ResourceManager.GetString("EnumDoesNotContainMembersExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The EnumValueNameAttribute-attribute was not found on the supplied member &apos;{0}&apos; of enum &apos;{1}&apos;..
        /// </summary>
        internal static string EnumValueNameAttributeNotFoundExceptionMessage {
            get {
                return ResourceManager.GetString("EnumValueNameAttributeNotFoundExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Indexed parameters are not supported..
        /// </summary>
        internal static string IndexedParametersAreNotSupportedExceptionMessage {
            get {
                return ResourceManager.GetString("IndexedParametersAreNotSupportedExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property &apos;{0}&apos; with type &apos;{1}&apos; on instance of type &apos;{2}&apos; on LambdaExpression &apos;{3}&apos; could not be created.\r\n\r\nLambdaExpression part: {4}..
        /// </summary>
        internal static string LambdaExpressionPartCouldNotBeInitializedExceptionMessage {
            get {
                return ResourceManager.GetString("LambdaExpressionPartCouldNotBeInitializedExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The member &apos;{0}&apos; on type &apos;{1}&apos; is not writable..
        /// </summary>
        internal static string MemberInfoIsNotWritableExceptionMessage {
            get {
                return ResourceManager.GetString("MemberInfoIsNotWritableExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The supplied type &apos;{0}&apos; provides more than one generic type parameters: &apos;{1}&apos;..
        /// </summary>
        internal static string MoreThanOneGenericTypeParameterFoundExceptionMessages {
            get {
                return ResourceManager.GetString("MoreThanOneGenericTypeParameterFoundExceptionMessages", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property &apos;{0}&apos; on &apos;{1}&apos; must be specified..
        /// </summary>
        internal static string PropertyMustBeSpecifiedExceptionMessage {
            get {
                return ResourceManager.GetString("PropertyMustBeSpecifiedExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property &apos;{0}&apos; was not found on Type &apos;{1}&apos;..
        /// </summary>
        internal static string PropertyNotFoundExceptionMessage {
            get {
                return ResourceManager.GetString("PropertyNotFoundExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The supplied type &apos;{0}&apos; is not a generic type..
        /// </summary>
        internal static string ProvidedTypeIsNotGenericExceptionMessage {
            get {
                return ResourceManager.GetString("ProvidedTypeIsNotGenericExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Type &apos;{0}&apos; can not be handled by the ValueProcessor &apos;{1}&apos;..
        /// </summary>
        internal static string ValueProcessorCannotHandleTypeExceptionMessage {
            get {
                return ResourceManager.GetString("ValueProcessorCannotHandleTypeExceptionMessage", resourceCulture);
            }
        }
    }
}