//
// Test the generated API `init` selectors are usable by the binding consumers
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012-2015 Xamarin Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Reflection;
using System.Linq;
using System.Text;

using NUnit.Framework;
#if HAS_ARKIT
using ARKit;
#endif

using Foundation;
using ObjCRuntime;

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Introspection {

	public abstract class ApiCtorInitTest : ApiBaseTest {

		string instance_type_name;

		/// <summary>
		/// Gets or sets a value indicating whether this test fixture will log untested types.
		/// </summary>
		/// <value><c>true</c> if log untested types; otherwise, <c>false</c>.</value>
		public bool LogUntestedTypes { get; set; }

		/// <summary>
		/// Override this method if you want the test to skip some specific types.
		/// By default types decorated with [Model] will be skipped.
		/// </summary>
		/// <param name="type">The Type to be tested</param>
		protected virtual bool Skip (Type type)
		{
			if (type.ContainsGenericParameters)
				return true;

			switch (type.Name) {
			case "JSExport":
				// This is interesting: Apple defines a private JSExport class - if you try to define your own in an Objective-C project you get this warning at startup:
				//     objc[334]: Class JSExport is implemented in both /Applications/Xcode91.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer/Library/CoreSimulator/Profiles/Runtimes/iOS.simruntime/Contents/Resources/RuntimeRoot/System/Library/Frameworks/JavaScriptCore.framework/JavaScriptCore (0x112c1e430) and /Users/rolf/Library/Developer/CoreSimulator/Devices/AC5323CF-225F-44D9-AA18-A37B7C28CA68/data/Containers/Bundle/Application/DEF9EAFC-CB5C-454F-97F5-669BBD00A609/jsexporttest.app/jsexporttest (0x105b49df0). One of the two will be used. Which one is undefined.
				// Due to how we treat models, we'll always look the Objective-C type up at runtime (even with the static registrar),
				// see that there's an existing JSExport type, and use that one instead of creating a new type.
				// This is problematic, because Apple's JSExport is completely unusable, and will crash if you try to do anything.
				return true;
			// on iOS 8.2 (beta 1) we get:  NSInvalidArgumentException Caller did not provide an activityType, and this process does not have a NSUserActivityTypes in its Info.plist.
			// even if we specify an NSUserActivityTypes in the Info.plist - might be a bug or there's a new (undocumented) requirement
			case "NSUserActivity":
				return true;
			case "NEPacketTunnelProvider":
				return true;
			// On iOS 14 (beta 4) we get: [NISimulator] To simulate Nearby Interaction distance and direction, launch two or more simulators and
			// move the simulator windows around the screen.
			// The same error occurs when trying to default init NISession in Xcode.
			// It seems that it is only possible to create a NISession when there are two devices or sims running, which makes sense given the description of
			// NISession from Apple API docs: "An object that identifies a unique connection between two peer devices"
			case "NISession":
				return true;
			case "NSUnitDispersion": // -init should never be called on NSUnit!
			case "NSUnitVolume": // -init should never be called on NSUnit!
			case "NSUnitDuration": // -init should never be called on NSUnit!
			case "NSUnitElectricCharge": // -init should never be called on NSUnit!
			case "NSUnitElectricCurrent": // -init should never be called on NSUnit!
			case "NSUnitElectricPotentialDifference": // -init should never be called on NSUnit!
			case "NSUnitElectricResistance": // -init should never be called on NSUnit!
			case "NSUnit": // -init should never be called on NSUnit!
			case "NSUnitEnergy": // -init should never be called on NSUnit!
			case "NSUnitAcceleration": // -init should never be called on NSUnit!
			case "NSUnitFrequency": // -init should never be called on NSUnit!
			case "NSUnitAngle": // -init should never be called on NSUnit!
			case "NSUnitFuelEfficiency": // -init should never be called on NSUnit!
			case "NSUnitArea": // -init should never be called on NSUnit!
			case "NSUnitIlluminance": // -init should never be called on NSUnit!
			case "NSUnitConcentrationMass": // -init should never be called on NSUnit!
			case "NSUnitLength": // -init should never be called on NSUnit!
			case "NSUnitMass": // -init should never be called on NSUnit!
			case "NSUnitPower": // -init should never be called on NSUnit!
			case "NSUnitPressure": // -init should never be called on NSUnit!
			case "NSUnitSpeed": // -init should never be called on NSUnit!
				return true;
			case "MPSCnnNeuron": // Cannot directly initialize MPSCNNNeuron. Use one of the sub-classes of MPSCNNNeuron
			case "MPSCnnNeuronPReLU":
			case "MPSCnnNeuronHardSigmoid":
			case "MPSCnnNeuronSoftPlus":
				return true;
			case "MPSCnnBinaryConvolution": // [MPSCNNBinaryConvolution initWithDevice:] is not allowed. Please use initializers that are not marked NS_UNAVAILABLE.
			case "MPSCnnDilatedPoolingMax": // [MPSCNNDilatedPoolingMax initWithDevice:] is not allowed. Please use initializers that are not marked NS_UNAVAILABLE.
			case "MPSCnnPoolingL2Norm": // [MPSCNNPoolingL2Norm initWithDevice:] is not allowed. Please use initializers that are not marked NS_UNAVAILABLE.
				return true;
			case "MPSCnnBinaryFullyConnected": // Please initialize the MPSCNNBinaryFullyConnected class with initWithDevice:convolutionDescriptor:kernelWeights:biasTerms
				return true;
			case "MPSCnnUpsampling": // Cannot directly initialize MPSCNNUpsampling. Use one of the sub-classes of MPSCNNUpsampling
			case "MPSCnnUpsamplingBilinear":
			case "MPSCnnUpsamplingNearest":
				return true;
			case "MPSImageArithmetic": // Cannot directly initialize MPSImageArithmetic. Use one of the sub-classes of MPSImageArithmetic.
				return true;
			case "CKModifyBadgeOperation":
			case "CKDiscoverUserInfosOperation": // deprecated, throws exception
			case "CKSubscription":
			case "MPSCnnConvolutionState":
				return true;
			case "MPSGraphExecutableSerializationDescriptor":
#if __MACCATALYST__
				// failed assertion `Error: unhandled platform for MPSGraph serialization'
				return true;
#elif __IOS__
				// crashes in the simulator
				return TestRuntime.IsSimulator;
#endif
			case "AVSpeechSynthesisVoice": // Calling description crashes the test
				return TestRuntime.CheckExactXcodeVersion (12, 2, beta: 3);
			case "AVRouteDetector": // only seems to work on device.
				return TestRuntime.IsSimulator;
			case "SKView":
				// Causes a crash later. Filed as radar://18440271.
				// Apple said they won't fix this ('init' isn't a designated initializer)
				return true;
			case "HMMatterRequestHandler": // got removed and the current API throws an exception at run time.
				return true;
#if __MACCATALYST__
			case "PKIdentityButton":
				return true;
#endif
#if !XAMCORE_5_0
			case "GKHybridStrategist":
				return true; // GKHybridStrategist has been removed from our bindings
#endif
			case "THClient":
				// The default initializer is documented to work, but it takes a long time before it eventually fails on macOS Sequoia
				// Looking at the stack trace in Xcode, it seems it hits the network and times out waiting for something?
				// So just skip the testing, it's likely the constructor is bound correctly, but that it only works in some circumstances.
				return true;
			case "ASAccountAuthenticationModificationController":
				return true; // started failing in Xcode 16.3 beta 1 for unknown reasons (it works in an Xcode project).
#if __TVOS__
			case "MTLAccelerationStructureDescriptor":
			case "MTLAccelerationStructureGeometryDescriptor":
			case "MTLAccelerationStructureMotionBoundingBoxGeometryDescriptor":
			case "MTLAccelerationStructureMotionTriangleGeometryDescriptor":
			case "MTLAccelerationStructurePassDescriptor":
			case "MTLAccelerationStructurePassSampleBufferAttachmentDescriptor":
			case "MTLAccelerationStructurePassSampleBufferAttachmentDescriptorArray":
			case "MTLAccelerationStructureTriangleGeometryDescriptor":
			case "MTLMeshRenderPipelineDescriptor":
			case "MTLMotionKeyframeData":
			case "MTLRasterizationRateLayerArray":
			case "MTLRasterizationRateMapDescriptor":
			case "MTLRasterizationRateSampleArray":
			case "MTLRenderPipelineFunctionsDescriptor":
			case "MTLResourceStatePassSampleBufferAttachmentDescriptor":
			case "MTLResourceStatePassSampleBufferAttachmentDescriptorArray":
				// The initial tvOS 16.0 simulator doesn't have these classes, but the tvOS 16.1 simulator doess
				if (TestRuntime.IsSimulator && !TestRuntime.CheckXcodeVersion (14, 1))
					return true;
				break;
#endif
			case "PhaseConeDirectivityModelParameters":
				return !TestRuntime.IsSimulator; // fails on device
			}

			switch (type.Namespace) {
			case "SensorKit": // SensorKit doesn't exist on iPads
				if (TestRuntime.IsDevice && TestRuntime.IsiPad)
					return true;
				break;
			case "SafetyKit":
				return true; // SafetyKit requires a custom entitlement, and will throw exceptions if it's not present.
			}

			// skip types that we renamed / rewrite since they won't behave correctly (by design)
			if (SkipDueToRejectedTypes (type))
				return true;

			return SkipDueToAttribute (type);
		}

		/// <summary>
		/// Checks that the Handle property of the specified NSObject instance is not null (not IntPtr.Zero).
		/// </summary>
		/// <param name="obj">NSObject instance to validate</param>
		protected virtual void CheckHandle (NSObject obj)
		{
			if (obj.Handle == IntPtr.Zero)
				ReportError ("{0} : Handle", instance_type_name);
		}

		/// <summary>
		/// Checks that ToString does not return null (not helpful for debugging) and that it does not crash.
		/// </summary>
		/// <param name="obj">NSObject instance to validate</param>
		protected virtual void CheckToString (NSObject obj)
		{
			if (obj.ToString () is null)
				ReportError ("{0} : ToString", instance_type_name);
		}

		bool GetIsDirectBinding (NSObject obj)
		{
			var flags = TestRuntime.GetFlags (obj);
			return (flags & 4) == 4;
		}

		/// <summary>
		/// Checks that the IsDirectBinding property is identical to the IsWrapper property of the Register attribute.
		/// </summary>
		/// <param name="obj">Object.</param>
		protected virtual void CheckIsDirectBinding (NSObject obj)
		{
			var attrib = obj.GetType ().GetCustomAttribute<RegisterAttribute> (false);
			// only check types that we register - that way we avoid the 118 MonoTouch.CoreImagge.CI* "special" types
			if (attrib is null)
				return;
			var is_wrapper = attrib is not null && attrib.IsWrapper;
			var is_direct_binding = GetIsDirectBinding (obj);
			if (is_direct_binding != is_wrapper)
				ReportError ("{0} : IsDirectBinding (expected {1}, got {2})", instance_type_name, is_wrapper, is_direct_binding);
		}

		/// <summary>
		/// Skip, or not, the specified pproperty from being verified.
		/// </summary>
		/// <param name="pi">PropertyInfo candidate</param>
		protected virtual bool Skip (PropertyInfo pi)
		{
			// manually bound API can have the attributes only on the property (and not on the getter/setter)
			return SkipDueToAttribute (pi);
		}

		/// <summary>
		/// Dispose the specified NSObject instance. In some cases objects cannot be disposed safely.
		/// Override this method to keep them alive while the remaining tests execute.
		/// </summary>
		/// <param name="obj">NSObject instance to dispose</param>
		/// <param name="type">Type of the object, to be used if special logic is required.</param>
		protected virtual void Dispose (NSObject obj, Type type)
		{
			//***** ApiCtorInitTest.DefaultCtorAllowed
			//2017-01-23 15:52:09.762 introspection[4084:16658258] *** -[NSKeyedArchiver dealloc]: warning: NSKeyedArchiver deallocated without having had -finishEncoding called on it.
			(obj as NSKeyedArchiver)?.FinishEncoding ();

			obj.Dispose ();
		}

		protected virtual void CheckNSObjectProtocol (NSObject obj)
		{
			// not documented to allow null, but commonly used this way. OTOH it's not clear what code answer this
			// (it might be different implementations) but we can make sure that Apple allows null with this test
			// ref: https://bugzilla.xamarin.com/show_bug.cgi?id=35924
			var kind_of_null = obj.IsKindOfClass (null);
			if (kind_of_null)
				ReportError ("{0} : IsKindOfClass(null) failed", instance_type_name);
			var is_member_of_null = obj.IsMemberOfClass (null);
			if (is_member_of_null)
				ReportError ("{0} : IsMemberOfClass(null) failed", instance_type_name);
			var respond_to_null = obj.RespondsToSelector (null);
			if (respond_to_null)
				ReportError ("{0} : RespondToSelector(null) failed", instance_type_name);
			var conforms_to_null = obj.ConformsToProtocol (IntPtr.Zero);
			if (conforms_to_null)
				ReportError ("{0} : ConformsToProtocol(null) failed", instance_type_name);
		}

		// if a .ctor is obsolete then it's because it was not usable (nor testable)
		protected override bool SkipDueToAttribute (MemberInfo member)
		{
			if (member is null)
				return false;
			return MemberHasObsolete (member) || base.SkipDueToAttribute (member);
		}

		[Test]
		public void DefaultCtorAllowed ()
		{
			Errors = 0;
			ErrorData.Clear ();
			int n = 0;

			foreach (Type t in Assembly.GetTypes ()) {
				if (t.IsAbstract || !NSObjectType.IsAssignableFrom (t))
					continue;

				if (Skip (t))
					continue;

				var ctor = t.GetConstructor (Type.EmptyTypes);
				if (SkipDueToAttribute (ctor))
					continue;

				// Don't test methods that have [UnsupportedOSPlatform] + [EditorBrowsable (Never)]
				if (SkipDueToInvisibleAndUnsupported (ctor))
					continue;

				if ((ctor is null) || ctor.IsAbstract) {
					if (LogUntestedTypes)
						Console.WriteLine ("[WARNING] {0} was skipped because it had no default constructor", t);
					continue;
				}

				instance_type_name = t.FullName;
				if (LogProgress)
					Console.WriteLine ("{0}. {1}", n, instance_type_name);

				NSObject obj = null;
				try {
					obj = ctor.Invoke (null) as NSObject;
					CheckHandle (obj);
					CheckToString (obj);
					CheckIsDirectBinding (obj);
					CheckNSObjectProtocol (obj);
					Dispose (obj, t);
				} catch (Exception e) {
					// Objective-C exception thrown
					if (!ContinueOnFailure)
						throw;

					TargetInvocationException tie = (e as TargetInvocationException);
					if (tie is not null)
						e = tie.InnerException;
					ReportError ("Default constructor not allowed for {0} : {1}", instance_type_name, e.Message);
				}
				n++;
			}
			Assert.AreEqual (0, Errors, "{0} potential errors found in {1} default ctor validated{2}", Errors, n, Errors == 0 ? string.Empty : ":\n" + ErrorData.ToString () + "\n");
		}

		// .NET constructors are not virtual, so we need to re-expose the base class .ctor when a subclass is created.
		// That's very important for designated initializer since we can end up with no correct/safe way to create
		// subclasses of an existing type
		[Test]
		public void DesignatedInitializer ()
		{
			Errors = 0;
			int n = 0;

			foreach (Type t in Assembly.GetTypes ()) {

				if (SkipCheckShouldReExposeBaseCtor (t))
					continue;

				// we only care for NSObject subclasses that we expose publicly
				if (!t.IsPublic || !NSObjectType.IsAssignableFrom (t))
					continue;

				// we only care about wrapper types (types with a native counterpart), and they all have a Register attribute.
				var typeRegisterAttribute = t.GetCustomAttribute<RegisterAttribute> (false);
				if (typeRegisterAttribute is null)
					continue;

				int designated = 0;
				foreach (var ctor in t.GetConstructors ()) {
					if (ctor.GetCustomAttribute<DesignatedInitializerAttribute> () is null)
						continue;
					designated++;
				}
				// that does not mean that inlining is not required, i.e. it might be useful, even needed
				// but it's not a showstopper for subclassing so we'll start with those cases
				if (designated > 0)
					continue;

				var base_class = t.BaseType;
				// NSObject ctor requirements are handled by the generator
				if (base_class == NSObjectType)
					continue;
				foreach (var ctor in base_class.GetConstructors ()) {
					// if the base ctor is a designated (not a convenience) initializer then we should re-expose it
					if (ctor.GetCustomAttribute<DesignatedInitializerAttribute> () is null)
						continue;

					// check if this ctor (from base type) is exposed in the current (subclass) type
					if (!Match (ctor, t))
						ReportError ("{0} should re-expose {1}::{2}", t, base_class.Name, ctor.ToString ().Replace ("Void ", String.Empty));
					n++;
				}
			}
			Assert.AreEqual (0, Errors, "{0} potential errors found in {1} designated initializer validated", Errors, n);
		}

		protected virtual bool Match (ConstructorInfo ctor, Type type)
		{
			var cstr = ctor.ToString ();

			switch (type.Name) {
			case "MKTileOverlayRenderer":
				// NSInvalidArgumentEception Expected a MKTileOverlay
				// looks like Apple has not yet added a DI for this type, but it should be `initWithTileOverlay:`
				if (cstr == $"Void .ctor(MapKit.IMKOverlay)")
					return true;
				break;
			case "MPSMatrixMultiplication":
			// marked as NS_UNAVAILABLE - Use the above initialization method instead.
			case "MPSImageHistogram":
				// Could not initialize an instance of the type 'MetalPerformanceShaders.MPSImageHistogram': the native 'initWithDevice:' method returned nil.
				// make sense: there's a `initWithDevice:histogramInfo:` DI
				if (cstr == $"Void .ctor(Metal.IMTLDevice)")
					return true;
				break;
			case "NSDataDetector":
				// -[NSDataDetector initWithPattern:options:error:]: Not valid for NSDataDetector
				if (cstr == $"Void .ctor(Foundation.NSString, Foundation.NSRegularExpressionOptions, Foundation.NSError ByRef)")
					return true;
				break;
			case "SKStoreProductViewController":
			case "SKCloudServiceSetupViewController":
				// SKStoreProductViewController and SKCloudServiceSetupViewController are OS View Controllers which can't be customized. Therefore they shouldn't re-expose initWithNibName:bundle:
				if (cstr == $"Void .ctor(System.String, Foundation.NSBundle)")
					return true;
				break;
			case "MKCompassButton":
			case "MKScaleView":
			case "MKUserTrackingButton":
				// Xcode9 added types that are created only from static methods (no init)
				return true;
#if __TVOS__
			case "UISearchBar":
				// - (nullable instancetype)initWithCoder:(NSCoder *)aDecoder NS_DESIGNATED_INITIALIZER __TVOS_PROHIBITED;
				return true;
			case "TVDigitEntryViewController":
				// full screen, no customization w/NIB
				return true;
			case "TVDocumentViewController":
				// as documented
				return true;
#endif
			case "PdfAnnotationButtonWidget":
			case "PdfAnnotationChoiceWidget":
			case "PdfAnnotationCircle":
			case "PdfAnnotationFreeText":
			case "PdfAnnotationInk":
			case "PdfAnnotationLine":
			case "PdfAnnotationLink":
			case "PdfAnnotationMarkup":
			case "PdfAnnotationPopup":
			case "PdfAnnotationSquare":
			case "PdfAnnotationStamp":
			case "PdfAnnotationText":
			case "PdfAnnotationTextWidget":
				// This ctor was introduced in 10,13 but all of the above objects are deprecated in 10,12
				// so it does not make much sense to expose this ctor in all the deprecated subclasses
				if (cstr == $"Void .ctor(CoreGraphics.CGRect, Foundation.NSString, Foundation.NSDictionary)")
					return true;
				break;
			case "VNTargetedImageRequest": // Explicitly disabled
				if (cstr == $"Void .ctor(Vision.VNRequestCompletionHandler)")
					return true;
				break;
			case "PKPaymentRequestShippingContactUpdate":
				// a more precise designated initializer is provided
				if (cstr == $"Void .ctor(PassKit.PKPaymentSummaryItem[])")
					return true;
				break;
			case "NSApplication": // Does not make sense, also it crashes
			case "NSBitmapImageRep": // exception raised
			case "NSCachedImageRep": // exception raised
			case "NSCIImageRep": // exception raised
			case "NSCustomImageRep": // exception raised
			case "NSEPSImageRep": // exception raised
			case "NSPdfImageRep": // exception raised
				if (cstr == $"Void .ctor()")
					return true;
				break;
			case "AUPannerView": // Do not make sense without the AudioUnit
			case "AUGenericView": // Do not make sense without the AudioUnit
				if (cstr == $"Void .ctor(CoreGraphics.CGRect)")
					return true;
				break;
			case "MDLNoiseTexture":
			case "MDLSkyCubeTexture":
			case "MDLNormalMapTexture":
			case "MDLUrlTexture":
			case "MDLCheckerboardTexture":
			case "MDLColorSwatchTexture":
				// they don't make sense without extra arguments
				return true;
			case "ASCredentialProviderViewController": // goal is to "provides a standard interface for creating a credential provider extension", not a custom one
			case "ASAccountAuthenticationModificationViewController":
			case "INUIAddVoiceShortcutViewController": // Doesn't make sense without INVoiceShortcut and there is no other way to set this unless you use the other only .ctor
			case "INUIEditVoiceShortcutViewController": // Doesn't make sense without INVoiceShortcut and there is no other way to set this unless you use the other only .ctor
			case "ILClassificationUIExtensionViewController": // Meant to be an extension
				if (cstr == $"Void .ctor(System.String, Foundation.NSBundle)")
					return true;
				break;
			case "MPSImageReduceUnary": // Not meant to be used, only subclasses
			case "MPSCnnArithmetic": // Not meant to be used, only subclasses
			case "MPSCnnArithmeticGradient": // Not meant to be used, only subclasses
			case "MPSNNOptimizer": // Not meant to be used, only subclasses
			case "MPSNNReduceBinary": // Not meant to be used, only subclasses
			case "MPSNNReduceUnary": // Not meant to be used, only subclasses
			case "MPSMatrixRandom": // Not meant to be used, only subclasses
				if (cstr == "Void .ctor(Metal.IMTLDevice)" || cstr == $"Void .ctor(Foundation.NSCoder, Metal.IMTLDevice)")
					return true;
				break;
			case "MPSTemporaryNDArray": // NS_UNAVAILABLE
				if (ctor.ToString () == $"Void .ctor(Metal.IMTLDevice, MetalPerformanceShaders.MPSNDArrayDescriptor)")
					return true;
				break;
			case "MFMailComposeViewController": // You are meant to use the system provided one
			case "MFMessageComposeViewController": // You are meant to use the system provided one
			case "GKFriendRequestComposeViewController": // You are meant to use the system provided one
			case "GKGameCenterViewController": // You are meant to use the system provided one
			case "GKMatchmakerViewController": // You are meant to use the system provided one
			case "GKTurnBasedMatchmakerViewController": // You are meant to use the system provided one
			case "UIImagePickerController": // You are meant to use the system provided one
			case "UIVideoEditorController": // You are meant to use the system provided one
			case "VNDocumentCameraViewController": // Explicitly disabled on the headers
				if (cstr == $"Void .ctor(System.String, Foundation.NSBundle)")
					return true;
				if (cstr == $"Void .ctor(UIKit.UIViewController)")
					return true;
				break;
			case "UICollectionViewCompositionalLayout":
				// Explicitly disabled ctors - (instancetype)init NS_UNAVAILABLE;
				return true;
			case "NSPickerTouchBarItem": // You are meant to use the static factory methods
				if (cstr == $"Void .ctor(System.String)")
					return true;
				break;
			case "NSMenuToolbarItem": // No ctor specified
				if (cstr == $"Void .ctor(System.String)")
					return true;
				break;
			case "NSStepperTouchBarItem": // You are meant to use the static factory methods
				if (cstr == $"Void .ctor(System.String)")
					return true;
				break;
			case "NSSharingServicePickerToolbarItem": // This type doesn't have .ctors
				if (cstr == $"Void .ctor(System.String)")
					return true;
				break;
			case "UIRefreshControl": // init should be used instead.
				if (cstr == $"Void .ctor(CoreGraphics.CGRect)")
					return true;
				break;
			case "PKAddSecureElementPassViewController":
				// no overview available yet... unlikely that it can be customized
				if (cstr == "Void .ctor(System.String, Foundation.NSBundle)")
					return true;
				break;
			case "PKPayLaterView":
				// headers have: (instancetype)initWithFrame:(CGRect)frame NS_UNAVAILABLE;
				if (cstr == "Void .ctor(CoreGraphics.CGRect)")
					return true;
				break;
			case "VNDetectedPoint":
				// This class is not meant to be instantiated
				if (cstr == "Void .ctor(Double, Double)")
					return true;
				break;
			case "VNStatefulRequest":
				// This class uses another overload to get instantiated
				if (cstr == "Void .ctor(Vision.VNRequestCompletionHandler)")
					return true;
				break;
			case "AVSpeechSynthesisProviderAudioUnit":
				if (cstr == "Void .ctor(AudioUnit.AudioComponentDescription, AudioUnit.AudioComponentInstantiationOptions, Foundation.NSError ByRef)") {
					// This constructor is exposed using a factory method.
					return true;
				}
				break;
			}

			var ep = ctor.GetParameters ();
			// NonPublic to get `protected` which can be autogenerated for abstract types (subclassing a non-abstract type)
			foreach (var candidate in type.GetConstructors (BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)) {
				var cp = candidate.GetParameters ();
				if (ep.Length != cp.Length)
					continue;
				var result = true;
				for (int i = 0; i < ep.Length; i++) {
					var cpt = cp [i].ParameterType;
					var ept = ep [i].ParameterType;
					if (cpt == ept)
						continue;
					if (!cp [i].ParameterType.IsSubclassOf (ep [i].ParameterType))
						result = false;
				}
				if (result)
					return true;
			}
			return false;
		}

		[Test]
		public void ShouldNotExposeDefaultCtorTest ()
		{
			Errors = 0;
			int n = 0;

			// Set to 'true' to generate alloc/init ObjC code of types that fail this test.
			bool genObjCTestCode = false;
			var objCCode = genObjCTestCode ? new StringBuilder () : null;

			var types = Assembly.GetTypes ();
			var cifiltertype = types.FirstOrDefault (c => c.Name == "CIFilter");
			foreach (Type t in types) {
				// TODO: Remove this MPS check in the future, at the time of writing this we currently only care about MPS.
				if (!t.Name.StartsWith ("MPS", StringComparison.OrdinalIgnoreCase))
					continue;

				if (!t.IsPublic || !NSObjectType.IsAssignableFrom (t))
					continue;

				// ignore CIFilter derived subclasses since they are specially generated
				if (cifiltertype is not null && t.IsSubclassOf (cifiltertype))
					continue;

				if (SkipCheckShouldNotExposeDefaultCtor (t))
					continue;

				var ctor = t.GetConstructor (Type.EmptyTypes);
				if (SkipDueToAttribute (ctor))
					continue;

				if (ctor is null || ctor.IsAbstract) {
					if (LogUntestedTypes)
						Console.WriteLine ("[WARNING] {0} was skipped because it had no default constructor", t);
					continue;
				}

				if (LogProgress)
					Console.WriteLine ($"{n}: {t.FullName}");

				var parentType = t.BaseType;
				var parentCtor = parentType.GetConstructor (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

				if (parentCtor is null) {
					ReportError ($"Type '{t.FullName}' is a possible candidate for [DisableDefaultCtor] because its BaseType '{parentType.FullName}' does not have one.");

					// Useful to test in Xcode
					if (genObjCTestCode) {
						var export = t.GetCustomAttribute<RegisterAttribute> ();
						var typeName = export?.Name ?? t.Name;
						objCCode.AppendLine ($"{typeName}* test{n} = [[{typeName} alloc] init];");
					}
				}
				n++;
			}
			Assert.AreEqual (0, Errors, $"{Errors} potential errors found in {n} BaseType empty ctor validated: \n{ErrorData}\n{(genObjCTestCode ? $"\n\n{objCCode}\n" : string.Empty)}");
		}

		protected virtual bool SkipCheckShouldNotExposeDefaultCtor (Type type)
		{
			if (type.ContainsGenericParameters)
				return true;

			foreach (object ca in type.GetCustomAttributes (false)) {
				if (ca is ProtocolAttribute || ca is ModelAttribute)
					return true;
			}

			return SkipDueToAttribute (type);
		}

		protected virtual bool SkipCheckShouldReExposeBaseCtor (Type type)
		{
			return SkipDueToAttribute (type);
		}

#if HAS_ARKIT
		/// <summary>
		/// Ensures that all subclasses of a base class that conforms to IARAnchorCopying re-expose its constructor.
		/// Note: we cannot have constructors in protocols so we have to inline them in every subclass.
		/// </summary>
		[Test]
		public void ARAnchorCopyingCtorTest ()
		{
			Errors = 0;

			foreach (Type t in Assembly.GetTypes ()) {
				if (t.Name == "IARAnchorCopying" || t.Name == "ARAnchorCopyingWrapper")
					continue;

				if (!typeof (IARAnchorCopying).IsAssignableFrom (t))
					continue;

				if (t.GetConstructor (new Type [] { typeof (ARAnchor) }) is null)
					ReportError ("{0} should re-expose IARAnchorCopying::.ctor(ARAnchor)", t);
			}

			Assert.AreEqual (0, Errors, "{0} potential errors found when validating if subclasses of 'ARAnchor' re-expose 'IARAnchorCopying' constructor", Errors);
		}
#endif
	}
}
