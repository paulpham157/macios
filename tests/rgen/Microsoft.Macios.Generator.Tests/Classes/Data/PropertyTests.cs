// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Runtime.Versioning;
using AVFoundation;
using CoreGraphics;
using Foundation;
using ObjCBindings;
using ObjCRuntime;
using nfloat = System.Runtime.InteropServices.NFloat;

namespace TestNamespace;

[BindingType<Class>]
public partial class PropertyTests {

	// the following are a list of examples of all possible property definitions

	// simple value type
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("count")]
	public virtual partial nuint Count { get; set; }

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("lineSpacing")]
	public virtual partial nfloat LineSpacing { get; set; }

	// array
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("sizes")]
	public virtual partial nuint [] Sizes { get; set; }

	// boolean
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("containsAttachments")]
	public virtual partial bool ContainsAttachments { get; set; }

	// simple string
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("name")]
	public virtual partial string Name { get; set; }

	// nullable string
	[Export<Property> ("name")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	public virtual partial string? Name { get; set; }

	// array of strings
	[Export<Property> ("surnames")]
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	public virtual partial string [] Name { get; set; }

	// simple NSObject
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("attributedStringByInflectingString")]
	public virtual partial NSAttributedString AttributedStringByInflectingString { get; set; }

	// nullable NSObject
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("delegate", ArgumentSemantic.Weak, Flags = Property.WeakDelegate)]
	public virtual partial NSObject? WeakDelegate { get; set; }

	// array nsobject
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("results")]
	public virtual partial NSMetadataItem [] Results { get; set; }

	// struct
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("size")]
	public virtual partial CGSize Size { get; set; }

	// static property
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("alphanumericCharacterSet", ArgumentSemantic.Copy)]
	public static partial NSCharacterSet Alphanumerics { get; set; }

	// internal property
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("locale", ArgumentSemantic.Copy)]
	internal virtual partial NSLocale Locale { get; set; }

	// property with custom selector on getter
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("forPersonMassUse")]
	public virtual partial bool ForPersonMassUse {
		[Export<Property> ("isForPersonMassUse")]
		get;
		set;
	}

	// property with custom selector on setter
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("isLenient")]
	public virtual partial bool IsLenient {
		get;
		[Export<Property> ("setLenient:")]
		set;
	}

	// wrapper property example
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	public virtual INSMetadataQueryDelegate? Delegate {
		get => WeakDelegate as INSMetadataQueryDelegate;
		set => WeakDelegate = value;
	}

	// bindfrom
	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[BindFrom (typeof (NSNumber))]
	[Export<Property> ("canDraw")]
	public virtual partial bool CanDraw { get; set; }

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[BindFrom (typeof (NSString))]
	[Export<Property> ("canDraw")]
	public virtual partial AVCaptureReactionType ReactionType { get; set; }

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[Export<Property> ("Center")]
	public virtual partial CGPoint Center { get; set; }

	[SupportedOSPlatform ("ios")]
	[SupportedOSPlatform ("tvos")]
	[SupportedOSPlatform ("macos")]
	[SupportedOSPlatform ("maccatalyst13.1")]
	[BindFrom (typeof (NSValue))]
	[Export<Property> ("Center")]
	public virtual partial CGPoint [] Location { get; set; }
}
