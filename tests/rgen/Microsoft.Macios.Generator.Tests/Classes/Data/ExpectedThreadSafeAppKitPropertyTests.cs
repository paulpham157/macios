// <auto-generated />

#nullable enable

using Foundation;
using ObjCBindings;
using ObjCRuntime;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace AppKit;

[Register ("ThreadSafeAppKitPropertyTests", true)]
public partial class ThreadSafeAppKitPropertyTests
{
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	const string selCountX = "count";
	static readonly global::ObjCRuntime.NativeHandle selCountXHandle = global::ObjCRuntime.Selector.GetHandle ("count");

	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	static readonly global::ObjCRuntime.NativeHandle class_ptr = global::ObjCRuntime.Class.GetHandle ("ThreadSafeAppKitPropertyTests");

	/// <summary>The Objective-C class handle for this class.</summary>
	/// <value>The pointer to the Objective-C class.</value>
	/// <remarks>
	///     Each managed class mirrors an unmanaged Objective-C class.
	///     This value contains the pointer to the Objective-C class.
	///     It is similar to calling the managed <see cref=\"ObjCRuntime.Class.GetHandle(string)\" /> or the native <see href=\"https://developer.apple.com/documentation/objectivec/1418952-objc_getclass\">objc_getClass</see> method with the type name.
	/// </remarks>
	public override global::ObjCRuntime.NativeHandle ClassHandle => class_ptr;

	/// <summary>Creates a new <see cref="ThreadSafeAppKitPropertyTests" /> with default values.</summary>
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	[DesignatedInitializer]
	[Export ("init")]
	public ThreadSafeAppKitPropertyTests () : base (global::Foundation.NSObjectFlag.Empty)
	{
		if (IsDirectBinding)
			InitializeHandle (global::ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle ("init")), "init");
		else
			InitializeHandle (global::ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, global::ObjCRuntime.Selector.GetHandle ("init")), "init");
	}

	/// <summary>Constructor to call on derived classes to skip initialization and merely allocate the object.</summary>
	/// <param name="t">Unused sentinel value, pass NSObjectFlag.Empty.</param>
	/// <remarks>
	///     <para>
	///         This constructor should be called by derived classes when they completely construct the object in managed code and merely want the runtime to allocate and initialize the <see cref="Foundation.NSObject" />.
	///         This is required to implement the two-step initialization process that Objective-C uses, the first step is to perform the object allocation, the second step is to initialize the object.
	///         When developers invoke this constructor, they take advantage of a direct path that goes all the way up to <see cref="Foundation.NSObject" /> to merely allocate the object's memory and bind the Objective-C and C# objects together.
	///         The actual initialization of the object is up to the developer.
	///     </para>
	///     <para>
	///         This constructor is typically used by the binding generator to allocate the object, but prevent the actual initialization to take place.
	///         Once the allocation has taken place, the constructor has to initialize the object.
	///         With constructors generated by the binding generator this means that it manually invokes one of the "init" methods to initialize the object.
	///     </para>
	///     <para>It is the developer's responsibility to completely initialize the object if they chain up using this constructor chain.</para>
	///     <para>
	///         In general, if the developer's constructor invokes the corresponding base implementation, then it should also call an Objective-C init method.
	///         If this is not the case, developers should instead chain to the proper constructor in their class.
	///     </para>
	///     <para>
	///         The argument value is ignored and merely ensures that the only code that is executed is the construction phase is the basic <see cref="Foundation.NSObject" /> allocation and runtime type registration.
	///         Typically the chaining would look like this:
	///     </para>
	///     <example>
	///             <code lang="csharp lang-csharp"><![CDATA[
	/// //
	/// // The NSObjectFlag constructor merely allocates the object and registers the C# class with the Objective-C runtime if necessary.
	/// // No actual initXxx method is invoked, that is done later in the constructor
	/// //
	/// // This is taken from the iOS SDK's source code for the UIView class:
	/// //
	/// [Export ("initWithFrame:")]
	/// public UIView (System.Drawing.RectangleF frame) : base (NSObjectFlag.Empty)
	/// {
	///     // Invoke the init method now.
	///     var initWithFrame = new Selector ("initWithFrame:").Handle;
	///     if (IsDirectBinding) {
	///         Handle = ObjCRuntime.Messaging.IntPtr_objc_msgSend_CGRect (this.Handle, initWithFrame, frame);
	///     } else {
	///         Handle = ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_CGRect (this.SuperHandle, initWithFrame, frame);
	///     }
	/// }
	/// ]]></code>
	///     </example>
	/// </remarks>
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	protected ThreadSafeAppKitPropertyTests (global::Foundation.NSObjectFlag t) : base (t) {}

	/// <summary>A constructor used when creating managed representations of unmanaged objects. Called by the runtime.</summary>
	/// <param name="handle">Pointer (handle) to the unmanaged object.</param>
	/// <remarks>
	///     <para>
	///         This constructor is invoked by the runtime infrastructure (<see cref="ObjCRuntime.Runtime.GetNSObject(System.IntPtr)" />) to create a new managed representation for a pointer to an unmanaged Objective-C object.
	///         Developers should not invoke this method directly, instead they should call <see cref="ObjCRuntime.Runtime.GetNSObject(System.IntPtr)" /> as it will prevent two instances of a managed object pointing to the same native object.
	///     </para>
	/// </remarks>
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	protected internal ThreadSafeAppKitPropertyTests (global::ObjCRuntime.NativeHandle handle) : base (handle) {}

	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[BindingImpl (BindingImplOptions.GeneratedCode | BindingImplOptions.Optimizable)]
	public virtual partial global::System.UIntPtr Count
	{
		[SupportedOSPlatform ("macos")]
		[SupportedOSPlatform ("ios")]
		[SupportedOSPlatform ("tvos")]
		[SupportedOSPlatform ("maccatalyst13.1")]
		get
		{
			global::System.UIntPtr ret;
			if (IsDirectBinding) {
				ret = global::ObjCRuntime.Messaging.UIntPtr_objc_msgSend (this.Handle, global::ObjCRuntime.Selector.GetHandle ("count"));
			} else {
				ret = global::ObjCRuntime.Messaging.UIntPtr_objc_msgSendSuper (this.Handle, global::ObjCRuntime.Selector.GetHandle ("count"));
			}
			global::System.GC.KeepAlive (this);
			return ret;
		}
	}
	// TODO: add binding code here
}
