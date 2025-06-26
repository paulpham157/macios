//
// PassKit bindings
//
// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012, 2015-2016 Xamarin Inc. All rights reserved.
// Copyright 2020 Microsoft Corp.
//

using System;
using System.ComponentModel;
using Contacts;
using CoreGraphics;
using ObjCRuntime;
using Foundation;
#if MONOMAC
using AppKit;
using ABRecord = Foundation.NSObject;
using UIButton = AppKit.NSButton;
using UIImage = AppKit.NSImage;
using UIViewController = AppKit.NSViewController;
using UIWindow = AppKit.NSWindow;
using UIControl = AppKit.NSControl;
using UIView = AppKit.NSView;
#else
using UIKit;
#if IOS
using AddressBook;
#else
using ABRecord = Foundation.NSObject;
using UIViewController = Foundation.NSObject;
using UIWindow = Foundation.NSObject;
using UIControl = Foundation.NSObject;
#endif // IOS
#endif // MONOMAC

namespace PassKit {

	/// <summary>Shipping and billing information for a single Apple Pay transaction.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKContact_Class/index.html">Apple documentation for <c>PKContact</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface PKContact : NSSecureCoding {
		/// <summary>The contact's name components.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("name", ArgumentSemantic.Strong)]
		NSPersonNameComponents Name { get; set; }

		/// <summary>Gets or sets the contact's address.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>The contact's address.</remarks>
		[NullAllowed, Export ("postalAddress", ArgumentSemantic.Retain)]
		CNPostalAddress PostalAddress { get; set; }

		/// <summary>The contact's email address.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("emailAddress", ArgumentSemantic.Strong)]
		string EmailAddress { get; set; }

		/// <summary>The contact's phone number.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("phoneNumber", ArgumentSemantic.Strong)]
		CNPhoneNumber PhoneNumber { get; set; }

		/// <summary>Developers should not use this deprecated property. Developers should use 'SubLocality' and 'SubAdministrativeArea' on 'PostalAddress' instead.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 10, 3, message: "Use 'SubLocality' and 'SubAdministrativeArea' on 'PostalAddress' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'SubLocality' and 'SubAdministrativeArea' on 'PostalAddress' instead.")]
		[NullAllowed, Export ("supplementarySubLocality", ArgumentSemantic.Strong)]
		string SupplementarySubLocality { get; set; }
	}

	[iOS (13, 4)]
	[MacCatalyst (13, 1)]
	delegate void PKPassLibrarySignDataCompletionHandler ([NullAllowed] NSData signedData, [NullAllowed] NSData signature, [NullAllowed] NSError error);

	/// <summary>Represents the user's library of passes.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPassLibrary_Ref/index.html">Apple documentation for <c>PKPassLibrary</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface PKPassLibrary {
		/// <summary>Whether the pass library is available.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("isPassLibraryAvailable")]
		bool IsAvailable { get; }

		/// <param name="pass">To be added.</param>
		///         <summary>Whether the specified <paramref name="pass" /> is available.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("containsPass:")]
		bool Contains (PKPass pass);

		/// <summary>The passes in the user's pass library.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("passes")]
		PKPass [] GetPasses ();

		/// <param name="identifier">To be added.</param>
		///         <param name="serialNumber">To be added.</param>
		///         <summary>Returns the  <see cref="PassKit.PKPass" /> whose <see cref="PassKit.PKPass.PassTypeIdentifier" /> and <see cref="PassKit.PKPass.SerialNumber" /> match the arguments.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("passWithPassTypeIdentifier:serialNumber:")]
		[return: NullAllowed]
		PKPass GetPass (string identifier, string serialNumber);

		[iOS (18, 2), MacCatalyst (18, 2), Mac (15, 2)]
		[Export ("passesWithReaderIdentifier:")]
		NSSet<PKSecureElementPass> GetPasses (string readerIdentifier);

		/// <param name="passType">To be added.</param>
		///         <summary>The passes in the user's pass library whose <see cref="PassKit.PKPassType.PassType" /> matches <paramref name="passType" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("passesOfType:")]
		PKPass [] GetPasses (PKPassType passType);

		/// <param name="pass">To be added.</param>
		///         <summary>Removes the specified <paramref name="pass" /> from the pass library.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("removePass:")]
		void Remove (PKPass pass);

		/// <param name="pass">To be added.</param>
		///         <summary>Replaces an existing pass with <paramref name="pass" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		///         <!-- Huh? How do it know which one to replace? -->
		[Export ("replacePassWithPass:")]
		bool Replace (PKPass pass);

		/// <param name="passes">To be added.</param>
		///         <param name="completion">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Presents a standard UX for adding multiple passes.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("addPasses:withCompletionHandler:")]
		[Async (XmlDocs = """
			<param name="passes">To be added.</param>
			<summary>Presents a standard UX for adding multiple passes.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous AddPasses operation.  The value of the TResult parameter is of type System.Action&lt;PassKit.PKPassLibraryAddPassesStatus&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void AddPasses (PKPass [] passes, [NullAllowed] Action<PKPassLibraryAddPassesStatus> completion);

		/// <include file="../docs/api/PassKit/PKPassLibrary.xml" path="/Documentation/Docs[@DocId='P:PassKit.PKPassLibrary.DidChangeNotification']/*" />
		[Field ("PKPassLibraryDidChangeNotification")]
		[Notification]
		NSString DidChangeNotification { get; }

		/// <include file="../docs/api/PassKit/PKPassLibrary.xml" path="/Documentation/Docs[@DocId='P:PassKit.PKPassLibrary.RemotePaymentPassesDidChangeNotification']/*" />
		[MacCatalyst (13, 1)]
		[Field ("PKPassLibraryRemotePaymentPassesDidChangeNotification")]
		[Notification]
		NSString RemotePaymentPassesDidChangeNotification { get; }

		/// <summary>Developers should not use this deprecated property. Developers should use the library's instance 'IsLibraryPaymentPassActivationAvailable' property instead.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Static, Export ("isPaymentPassActivationAvailable")]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use the library's instance 'IsLibraryPaymentPassActivationAvailable' property instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use the library's instance 'IsLibraryPaymentPassActivationAvailable' property instead.")]
		bool IsPaymentPassActivationAvailable { get; }

		/// <summary>Whether the device allows adding library payment passes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'SecureElementPassActivationAvailable' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'SecureElementPassActivationAvailable' instead.")]
		[Export ("isPaymentPassActivationAvailable")]
		bool IsLibraryPaymentPassActivationAvailable { get; }

		[iOS (13, 4)]
		[MacCatalyst (13, 1)]
		[Export ("secureElementPassActivationAvailable")]
		bool SecureElementPassActivationAvailable { [Bind ("isSecureElementPassActivationAvailable")] get; }

		/// <param name="paymentPass">To be added.</param>
		///         <param name="activationData">To be added.</param>
		///         <param name="completion">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Activates the specified <paramref name="paymentPass" /> with the activation code in <paramref name="activationData" />.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'ActivateSecureElementPass' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ActivateSecureElementPass' instead.")]
		[Async (XmlDocs = """
			<param name="paymentPass">To be added.</param>
			<param name="activationData">To be added.</param>
			<summary>Activates the specified <paramref name="paymentPass" /> with the activation code in <paramref name="activationData" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous ActivatePaymentPass operation.  The value of the TResult parameter is of type System.Action&lt;System.Boolean,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("activatePaymentPass:withActivationData:completion:")]
		void ActivatePaymentPass (PKPaymentPass paymentPass, NSData activationData, [NullAllowed] Action<bool, NSError> completion);

		[Async]
		[iOS (13, 4)]
		[MacCatalyst (13, 1)]
		[Export ("activateSecureElementPass:withActivationData:completion:")]
		void ActivateSecureElementPass (PKSecureElementPass secureElementPass, NSData activationData, [NullAllowed] Action<bool, NSError> completion);

		/// <param name="paymentPass">To be added.</param>
		///         <param name="activationCode">To be added.</param>
		///         <param name="completion">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Activates the specified <paramref name="paymentPass" /> with the activation code in <paramref name="activationCode" />.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'ActivatePaymentPass (PKPaymentPass, NSData, Action<bool, NSError> completion)' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ActivatePaymentPass (PKPaymentPass, NSData, Action<bool, NSError> completion)' instead.")]
		[Async (XmlDocs = """
			<param name="paymentPass">To be added.</param>
			<param name="activationCode">To be added.</param>
			<summary>Activates the specified <paramref name="paymentPass" /> with the activation code in <paramref name="activationCode" />.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous ActivatePaymentPass operation.  The value of the TResult parameter is of type System.Action&lt;System.Boolean,Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>
			          <para copied="true">The ActivatePaymentPassAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("activatePaymentPass:withActivationCode:completion:")]
		void ActivatePaymentPass (PKPaymentPass paymentPass, string activationCode, [NullAllowed] Action<bool, NSError> completion);

		/// <summary>Presents to the user the standard interface to set up credit cards for use with Apple Pay.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("openPaymentSetup")]
		void OpenPaymentSetup ();

		/// <param name="primaryAccountIdentifier">To be added.</param>
		///         <summary>Whether the app can add a card to Apple Pay for <paramref name="primaryAccountIdentifier" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'CanAddSecureElementPass' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CanAddSecureElementPass' instead.")]
		[Export ("canAddPaymentPassWithPrimaryAccountIdentifier:")]
		bool CanAddPaymentPass (string primaryAccountIdentifier);

		[iOS (13, 4)]
		[MacCatalyst (13, 1)]
		[Export ("canAddSecureElementPassWithPrimaryAccountIdentifier:")]
		bool CanAddSecureElementPass (string primaryAccountIdentifier);

		/// <summary>Gets a Boolean value that tells whether Felica passes can be added to the library.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("canAddFelicaPass")]
		bool CanAddFelicaPass { get; }

		/// <param name="requestToken">To be added.</param>
		/// <summary>Enables automatic display of the Apple Pay UI.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("endAutomaticPassPresentationSuppressionWithRequestToken:")]
		void EndAutomaticPassPresentationSuppression (nuint requestToken);

		/// <summary>Whether the system is suppressing automatic presentation of passes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("isSuppressingAutomaticPassPresentation")]
		bool IsSuppressingAutomaticPassPresentation { get; }

		/// <summary>The <see cref="PassKit.PKPaymentPass" /> objects stored on a remote device.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'RemoteSecureElementPasses' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'RemoteSecureElementPasses' instead.")]
		[Export ("remotePaymentPasses")]
		PKPaymentPass [] RemotePaymentPasses { get; }

		[iOS (13, 4)]
		[MacCatalyst (13, 1)]
		[Export ("remoteSecureElementPasses", ArgumentSemantic.Copy)]
		PKSecureElementPass [] RemoteSecureElementPasses { get; }

		/// <param name="responseHandler">To be added.</param>
		///         <summary>Stops the device from automatically presenting Apply Pay.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("requestAutomaticPassPresentationSuppressionWithResponseHandler:")]
		nuint RequestAutomaticPassPresentationSuppression (Action<PKAutomaticPassPresentationSuppressionResult> responseHandler);

		/// <param name="pass">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'PresentSecureElementPass' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PresentSecureElementPass' instead.")]
		[Export ("presentPaymentPass:")]
		void PresentPaymentPass (PKPaymentPass pass);

		[iOS (13, 4)]
		[MacCatalyst (13, 1)]
		[Export ("presentSecureElementPass:")]
		void PresentSecureElementPass (PKSecureElementPass pass);

		[Async (ResultTypeName = "PKSignDataCompletionResult")]
		[iOS (13, 4)]
		[MacCatalyst (13, 1)]
		[Export ("signData:withSecureElementPass:completion:")]
		void SignData (NSData signData, PKSecureElementPass secureElementPass, PKPassLibrarySignDataCompletionHandler completion);

		[Async (ResultTypeName = "PKServiceProviderDataCompletionResult")]
		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("serviceProviderDataForSecureElementPass:completion:")]
		void GetServiceProviderData (PKSecureElementPass secureElementPass, Action<NSData, NSError> completion);

		[Async]
		[iOS (16, 0), MacCatalyst (16, 0), Mac (13, 0), NoTV]
		[Export ("encryptedServiceProviderDataForSecureElementPass:completion:")]
		void GetEncryptedServiceProviderData (PKSecureElementPass secureElementPass, Action<NSDictionary, NSError> completion);
	}

	/// <summary>A class whose static members represent keys to be used with the <see cref="PassKit.PKPass.GetLocalizedValue(Foundation.NSString)" /> method.</summary>
	[Static]
	[MacCatalyst (13, 1)]
	interface PKPassLibraryUserInfoKey {
		/// <summary>Represents the value associated with the constant PKPassLibraryAddedPassesUserInfoKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("PKPassLibraryAddedPassesUserInfoKey")]
		NSString AddedPasses { get; }

		/// <summary>Represents the value associated with the constant PKPassLibraryReplacementPassesUserInfoKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("PKPassLibraryReplacementPassesUserInfoKey")]
		NSString ReplacementPasses { get; }

		/// <summary>Represents the value associated with the constant PKPassLibraryRemovedPassInfosUserInfoKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("PKPassLibraryRemovedPassInfosUserInfoKey")]
		NSString RemovedPassInfos { get; }

		/// <summary>Represents the value associated with the constant PKPassLibraryPassTypeIdentifierUserInfoKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("PKPassLibraryPassTypeIdentifierUserInfoKey")]
		NSString PassTypeIdentifier { get; }

		/// <summary>Represents the value associated with the constant PKPassLibrarySerialNumberUserInfoKey</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("PKPassLibrarySerialNumberUserInfoKey")]
		NSString SerialNumber { get; }

		[iOS (15, 2), Mac (12, 1), MacCatalyst (15, 2)]
		[Field ("PKPassLibraryRecoveredPassesUserInfoKey")]
		NSString RecoveredPasses { get; }
	}

	/// <summary>The result of an authorized payment request. Contains encrypted payment information.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPayment_Ref/index.html">Apple documentation for <c>PKPayment</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface PKPayment {
		/// <summary>The <see cref="PassKit.PKPaymentToken" /> for the <see cref="PassKit.PKPayment" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("token", ArgumentSemantic.Strong)]
		PKPaymentToken Token { get; }

		/// <summary>Developers should not use this deprecated property. Developers should use 'BillingContact' instead.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[NoMacCatalyst]
		[Export ("billingAddress", ArgumentSemantic.Assign)]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'BillingContact' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'BillingContact' instead.")]
		ABRecord BillingAddress { get; }

		/// <summary>The shipping address associated with the <see cref="PassKit.PKPayment" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[NoMacCatalyst]
		[Export ("shippingAddress", ArgumentSemantic.Assign)]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'ShippingContact' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ShippingContact' instead.")]
		ABRecord ShippingAddress { get; }

		/// <summary>The selected <see cref="PassKit.PKShippingMethod" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("shippingMethod", ArgumentSemantic.Strong)]
		PKShippingMethod ShippingMethod { get; }


		/// <summary>The shipping contact associated with the <see cref="PassKit.PKPayment" />.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("shippingContact", ArgumentSemantic.Strong)]
		PKContact ShippingContact { get; }

		/// <summary>The billing contact for the payment.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("billingContact", ArgumentSemantic.Strong)]
		PKContact BillingContact { get; }
	}

	/// <summary>Delegate called when the user has selected a shipping address.</summary>
	delegate void PKPaymentShippingAddressSelected (PKPaymentAuthorizationStatus status, PKShippingMethod [] shippingMethods, PKPaymentSummaryItem [] summaryItems);
	/// <summary>Delegate called when the user has selected a shipping method.</summary>
	delegate void PKPaymentShippingMethodSelected (PKPaymentAuthorizationStatus status, PKPaymentSummaryItem [] summaryItems);

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="PassKit.PKPaymentAuthorizationViewControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="PassKit.PKPaymentAuthorizationViewControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="PassKit.PKPaymentAuthorizationViewControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="PassKit.PKPaymentAuthorizationViewControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IPKPaymentAuthorizationViewControllerDelegate { }

	/// <summary>Delegate object providing events relating to a payment authorization request made with a <see cref="PassKit.PKPaymentAuthorizationViewController" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPaymentAuthorizationViewControllerDelegate_Ref/index.html">Apple documentation for <c>PKPaymentAuthorizationViewControllerDelegate</c></related>
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface PKPaymentAuthorizationViewControllerDelegate {

		/// <param name="controller">To be added.</param>
		///         <param name="payment">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>Developers should not use this deprecated method. Developers should use 'DidAuthorizePayment2' instead.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'DidAuthorizePayment2' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidAuthorizePayment2' instead.")]
		[Export ("paymentAuthorizationViewController:didAuthorizePayment:completion:")]
		[EventArgs ("PKPaymentAuthorization", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidAuthorizePayment (PKPaymentAuthorizationViewController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion);

		/// <param name="controller">To be added.</param>
		///         <param name="payment">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("paymentAuthorizationViewController:didAuthorizePayment:handler:")]
		[EventArgs ("PKPaymentAuthorizationResult", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidAuthorizePayment2 (PKPaymentAuthorizationViewController controller, PKPayment payment, Action<PKPaymentAuthorizationResult> completion);

		/// <param name="controller">To be added.</param>
		///         <summary>Indicates the payment authorization has completed.</summary>
		///         <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("paymentAuthorizationViewControllerDidFinish:")]
		[Abstract]
		void PaymentAuthorizationViewControllerDidFinish (PKPaymentAuthorizationViewController controller);

		/// <param name="controller">To be added.</param>
		///         <param name="shippingMethod">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>Indicates the user selected a shippingmethod.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'DidSelectShippingMethod2' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidSelectShippingMethod2' instead.")]
		[Export ("paymentAuthorizationViewController:didSelectShippingMethod:completion:")]
		[EventArgs ("PKPaymentShippingMethodSelected", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidSelectShippingMethod (PKPaymentAuthorizationViewController controller, PKShippingMethod shippingMethod, PKPaymentShippingMethodSelected completion);

		/// <param name="controller">To be added.</param>
		///         <param name="shippingMethod">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("paymentAuthorizationViewController:didSelectShippingMethod:handler:")]
		[EventArgs ("PKPaymentRequestShippingMethodUpdate", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidSelectShippingMethod2 (PKPaymentAuthorizationViewController controller, PKShippingMethod shippingMethod, Action<PKPaymentRequestShippingMethodUpdate> completion);

		/// <param name="controller">To be added.</param>
		///         <param name="address">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>Indicates the user selected a shipping address.</summary>
		///         <remarks>To be added.</remarks>
		[NoMacCatalyst]
		[Deprecated (PlatformName.iOS, 9, 0)]
		[NoMac]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[Export ("paymentAuthorizationViewController:didSelectShippingAddress:completion:")]
		[EventArgs ("PKPaymentShippingAddressSelected", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidSelectShippingAddress (PKPaymentAuthorizationViewController controller, ABRecord address, PKPaymentShippingAddressSelected completion);

		/// <param name="controller">To be added.</param>
		///         <summary>Indicates that payment authorization will shortly begin.</summary>
		///         <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[MacCatalyst (13, 1)]
		[Export ("paymentAuthorizationViewControllerWillAuthorizePayment:")]
		void WillAuthorizePayment (PKPaymentAuthorizationViewController controller);

		/// <param name="controller">To be added.</param>
		///         <param name="contact">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>Called after the user has selected a shipping contact.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'DidSelectShippingContact' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidSelectShippingContact' instead.")]
		[Export ("paymentAuthorizationViewController:didSelectShippingContact:completion:")]
		[EventArgs ("PKPaymentSelectedContact", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidSelectShippingContact (PKPaymentAuthorizationViewController controller, PKContact contact, PKPaymentShippingAddressSelected completion);

		/// <param name="controller">To be added.</param>
		///         <param name="contact">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("paymentAuthorizationViewController:didSelectShippingContact:handler:")]
		[EventArgs ("PKPaymentRequestShippingContactUpdate", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidSelectShippingContact2 (PKPaymentAuthorizationViewController controller, PKContact contact, Action<PKPaymentRequestShippingContactUpdate> completion);

		/// <param name="controller">To be added.</param>
		///         <param name="paymentMethod">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>Called after the user has selected a payment method.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'DidSelectPaymentMethod2' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidSelectPaymentMethod2' instead.")]
		[Export ("paymentAuthorizationViewController:didSelectPaymentMethod:completion:")]
		[EventArgs ("PKPaymentMethodSelected", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidSelectPaymentMethod (PKPaymentAuthorizationViewController controller, PKPaymentMethod paymentMethod, Action<PKPaymentSummaryItem []> completion);

		/// <param name="controller">To be added.</param>
		///         <param name="paymentMethod">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("paymentAuthorizationViewController:didSelectPaymentMethod:handler:")]
		[EventArgs ("PKPaymentRequestPaymentMethodUpdate", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the Delegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DidSelectPaymentMethod2 (PKPaymentAuthorizationViewController controller, PKPaymentMethod paymentMethod, Action<PKPaymentRequestPaymentMethodUpdate> completion);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("paymentAuthorizationViewController:didRequestMerchantSessionUpdate:")]
		[EventArgs ("PKPaymentRequestMerchantSessionUpdate")]
		void DidRequestMerchantSessionUpdate (PKPaymentAuthorizationViewController controller, Action<PKPaymentRequestMerchantSessionUpdate> updateHandler);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("paymentAuthorizationViewController:didChangeCouponCode:handler:")]
		[EventArgs ("PKPaymentRequestCouponCodeUpdate")]
		void DidChangeCouponCode (PKPaymentAuthorizationViewController controller, string couponCode, Action<PKPaymentRequestCouponCodeUpdate> completion);
	}

	/// <summary>Standard view controller that prompts the user to authorize a payment.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPaymentAuthorizationViewController_Ref/index.html">Apple documentation for <c>PKPaymentAuthorizationViewController</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIViewController), Delegates = new string [] { "Delegate" }, Events = new Type [] { typeof (PKPaymentAuthorizationViewControllerDelegate) })]
	[DisableDefaultCtor]
	interface PKPaymentAuthorizationViewController {
		/// <param name="request">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[DesignatedInitializer]
		[Export ("initWithPaymentRequest:")]
		NativeHandle Constructor (PKPaymentRequest request);

		[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("initWithDisbursementRequest:")]
		NativeHandle Constructor (PKDisbursementRequest request);

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.UnsafeUnretained)]
		[NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the PassKit.IPKPaymentAuthorizationViewControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the PassKit.IPKPaymentAuthorizationViewControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IPKPaymentAuthorizationViewControllerDelegate Delegate { get; set; }

		/// <summary>Whether the user can make payments.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static, Export ("canMakePayments")]
		bool CanMakePayments { get; }

		// These are the NSString constants
		/// <param name="paymentNetworks">To be added.</param>
		///         <summary>Whether the user can make payments in at least one of the specified <paramref name="paymentNetworks" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("canMakePaymentsUsingNetworks:")]
		bool CanMakePaymentsUsingNetworks (NSString [] paymentNetworks);

		/// <param name="supportedNetworks">To be added.</param>
		///         <param name="capabilties">To be added.</param>
		///         <summary>Whether the user can make payments in at least one of the specified networks with the specified capabilities.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("canMakePaymentsUsingNetworks:capabilities:")]
#if XAMCORE_5_0
		bool CanMakePaymentsUsingNetworks (string [] supportedNetworks, PKMerchantCapability capabilities);
#else
		bool CanMakePaymentsUsingNetworks (string [] supportedNetworks, PKMerchantCapability capabilties);
#endif

		[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Static]
		[Export ("supportsDisbursements")]
		bool SupportsDisbursements ();

		[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Static]
		[Export ("supportsDisbursementsUsingNetworks:")]
		bool SupportsDisbursements (string [] supportedNetworks);

		[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Static]
		[Export ("supportsDisbursementsUsingNetworks:capabilities:")]
		bool SupportsDisbursements (string [] supportedNetworks, PKMerchantCapability capabilities);
	}

	/// <summary>A summary item (such as grand total, tax, or discounts) within a payment request.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPaymentSummaryItem_Ref/index.html">Apple documentation for <c>PKPaymentSummaryItem</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface PKPaymentSummaryItem {
		/// <summary>A brief, localized description of the item.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("label")]
		string Label { get; set; }

		/// <summary>The amount of the transaction.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("amount", ArgumentSemantic.Copy)]
		NSDecimalNumber Amount { get; set; }

		/// <param name="label">To be added.</param>
		///         <param name="amount">To be added.</param>
		///         <summary>Factory method to create a new <see cref="PassKit.PKPaymentSummaryItem" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static, Export ("summaryItemWithLabel:amount:")]
		PKPaymentSummaryItem Create (string label, NSDecimalNumber amount);

		/// <summary>Gets a value that tells whether the payment is final or pending.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("type", ArgumentSemantic.Assign)]
		PKPaymentSummaryItemType Type { get; set; }

		/// <param name="label">To be added.</param>
		///         <param name="amount">To be added.</param>
		///         <param name="type">To be added.</param>
		///         <summary>Creates and returns a new payment summary item with the specified data.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("summaryItemWithLabel:amount:type:")]
		PKPaymentSummaryItem Create (string label, NSDecimalNumber amount, PKPaymentSummaryItemType type);
	}

	/// <summary>A shipping method for physical goods.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKShippingMethod_Ref/index.html">Apple documentation for <c>PKShippingMethod</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (PKPaymentSummaryItem))]
	interface PKShippingMethod {
		/// <summary>A unique identifier of the shipping method.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("identifier")]
		string Identifier { get; set; }

		/// <summary>An end-user meaningful description of the shipping method.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("detail")]
		string Detail { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed]
		[Export ("dateComponentsRange", ArgumentSemantic.Copy)]
		PKDateComponentsRange DateComponentsRange { get; set; }
	}

	/// <summary>The main class for a payment request, including processing capabilities, amount request, and shipping information.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPaymentRequest_Ref/index.html">Apple documentation for <c>PKPaymentRequest</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface PKPaymentRequest {
		/// <summary>The developer's merchant identifier.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("merchantIdentifier")]
		string MerchantIdentifier { get; set; }

		/// <summary>The ISO 3166 country code for the country defining the payment request.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("countryCode")]
		string CountryCode { get; set; }

		/// <summary>The set of payment networks supported (Use values from <see cref="PassKit.PKPaymentNetwork" />).</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("supportedNetworks", ArgumentSemantic.Copy)]
		NSString [] SupportedNetworks { get; set; }

		/// <summary>Defines the developer's payment-processing capabilities.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("merchantCapabilities", ArgumentSemantic.UnsafeUnretained)]
		PKMerchantCapability MerchantCapabilities { get; set; }

		/// <summary>An array of <see cref="PassKit.PKPaymentSummaryItem" /> objects that summarize the amount of payment.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("paymentSummaryItems", ArgumentSemantic.Copy)]
		PKPaymentSummaryItem [] PaymentSummaryItems { get; set; }

		/// <summary>The ISO 4217 currency code in which the payment request is being made.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("currencyCode")]
		string CurrencyCode { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("supportsCouponCode")]
		bool SupportsCouponCode { get; set; }

		[NullAllowed]
		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("couponCode")]
		string CouponCode { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("shippingContactEditingMode", ArgumentSemantic.Assign)]
		PKShippingContactEditingMode ShippingContactEditingMode { get; set; }

		/// <summary>The set of billing address fields that must be filled in.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'RequiredBillingContactFields' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'RequiredBillingContactFields' instead.")]
		[Export ("requiredBillingAddressFields", ArgumentSemantic.UnsafeUnretained)]
		PKAddressField RequiredBillingAddressFields { get; set; }

		/// <summary>Developers should not use this deprecated property. Developers should use 'BillingContact' instead.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[NoMacCatalyst]
		[NullAllowed] // by default this property is null
		[Export ("billingAddress", ArgumentSemantic.Assign)]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'BillingContact' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'BillingContact' instead.")]
		ABRecord BillingAddress { get; set; }

		/// <summary>The set of shipping address fields that must be filled in.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'RequiredShippingContactFields' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'RequiredShippingContactFields' instead.")]
		[Export ("requiredShippingAddressFields", ArgumentSemantic.UnsafeUnretained)]
		PKAddressField RequiredShippingAddressFields { get; set; }

		/// <summary>Prepopulated address field.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[NoMacCatalyst]
		[NullAllowed] // by default this property is null
		[Export ("shippingAddress", ArgumentSemantic.Assign)]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'ShippingContact' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'ShippingContact' instead.")]
		ABRecord ShippingAddress { get; set; }

		/// <summary>The set of supported shipping methods.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("shippingMethods", ArgumentSemantic.Copy)]
		PKShippingMethod [] ShippingMethods { get; set; }

		/// <summary>Developer-specified extra data or state.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("applicationData", ArgumentSemantic.Copy)]
		NSData ApplicationData { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("shippingType", ArgumentSemantic.Assign)]
		PKShippingType ShippingType { get; set; }

		/// <summary>Prepopulated shipping address.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("shippingContact", ArgumentSemantic.Strong)]
		PKContact ShippingContact { get; set; }

		/// <summary>Prepopulated billing address.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("billingContact", ArgumentSemantic.Strong)]
		PKContact BillingContact { get; set; }

		/// <summary>Gets the list of payment networks that are supported.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("availableNetworks")]
		NSString [] AvailableNetworks { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("requiredBillingContactFields", ArgumentSemantic.Strong)]
		NSSet WeakRequiredBillingContactFields { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("requiredShippingContactFields", ArgumentSemantic.Strong)]
		NSSet WeakRequiredShippingContactFields { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("supportedCountries", ArgumentSemantic.Copy)]
		NSSet<NSString> SupportedCountries { get; set; }

		/// <param name="field">To be added.</param>
		///         <param name="localizedDescription">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("paymentContactInvalidErrorWithContactField:localizedDescription:")]
		NSError CreatePaymentContactInvalidError (NSString field, [NullAllowed] string localizedDescription);

		/// <param name="contactField">To be added.</param>
		///         <param name="localizedDescription">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Wrap ("CreatePaymentContactInvalidError (contactField.GetConstant ()!, localizedDescription)")]
		NSError CreatePaymentContactInvalidError (PKContactFields contactField, [NullAllowed] string localizedDescription);

		/// <param name="postalAddressKey">To be added.</param>
		///         <param name="localizedDescription">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("paymentShippingAddressInvalidErrorWithKey:localizedDescription:")]
		NSError CreatePaymentShippingAddressInvalidError (NSString postalAddressKey, [NullAllowed] string localizedDescription);

		/// <param name="postalAddress">To be added.</param>
		///         <param name="localizedDescription">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Wrap ("CreatePaymentShippingAddressInvalidError (postalAddress.GetConstant ()!, localizedDescription)")]
		NSError CreatePaymentShippingAddressInvalidError (CNPostalAddressKeyOption postalAddress, [NullAllowed] string localizedDescription);

		/// <param name="postalAddressKey">To be added.</param>
		///         <param name="localizedDescription">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("paymentBillingAddressInvalidErrorWithKey:localizedDescription:")]
		NSError CreatePaymentBillingAddressInvalidError (NSString postalAddressKey, [NullAllowed] string localizedDescription);

		/// <param name="postalAddress">To be added.</param>
		///         <param name="localizedDescription">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Wrap ("CreatePaymentBillingAddressInvalidError (postalAddress.GetConstant ()!, localizedDescription)")]
		NSError CreatePaymentBillingAddressInvalidError (CNPostalAddressKeyOption postalAddress, [NullAllowed] string localizedDescription);

		/// <param name="localizedDescription">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("paymentShippingAddressUnserviceableErrorWithLocalizedDescription:")]
		NSError CreatePaymentShippingAddressUnserviceableError ([NullAllowed] string localizedDescription);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Static]
		[Export ("paymentCouponCodeInvalidErrorWithLocalizedDescription:")]
		NSError GetCouponCodeInvalidError ([NullAllowed] string localizedDescription);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Static]
		[Export ("paymentCouponCodeExpiredErrorWithLocalizedDescription:")]
		NSError GetCouponCodeExpiredError ([NullAllowed] string localizedDescription);

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[Export ("multiTokenContexts", ArgumentSemantic.Copy)]
		PKPaymentTokenContext [] MultiTokenContexts { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[NullAllowed, Export ("recurringPaymentRequest", ArgumentSemantic.Strong)]
		PKRecurringPaymentRequest RecurringPaymentRequest { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[NullAllowed, Export ("automaticReloadPaymentRequest", ArgumentSemantic.Strong)]
		PKAutomaticReloadPaymentRequest AutomaticReloadPaymentRequest { get; set; }

		[NullAllowed]
		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4), NoTV]
		[Export ("deferredPaymentRequest", ArgumentSemantic.Strong)]
		PKDeferredPaymentRequest DeferredPaymentRequest { get; set; }

		[iOS (17, 0), Mac (14, 0), NoTV, MacCatalyst (17, 0)]
		[Export ("applePayLaterAvailability", ArgumentSemantic.Assign)]
		PKApplePayLaterAvailability ApplePayLaterAvailability { get; set; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("merchantCategoryCode", ArgumentSemantic.Assign)]
		PKMerchantCategoryCode MerchantCategoryCode { get; set; }
	}

	/// <summary>Enumerates fields for a contact.</summary>
	[Mac (11, 0)]
	[MacCatalyst (13, 1)]
	[Flags]
	enum PKContactFields {
		/// <summary>Indicates a name field.</summary>
		None = 0,

		/// <summary>Indicates a postal address field.</summary>
		[Field ("PKContactFieldPostalAddress")]
		PostalAddress = 1 << 0,

		/// <summary>Indicates an email address field.</summary>
		[Field ("PKContactFieldEmailAddress")]
		EmailAddress = 1 << 1,

		/// <summary>Indicates a phone number field.</summary>
		[Field ("PKContactFieldPhoneNumber")]
		PhoneNumber = 1 << 2,

		/// <summary>To be added.</summary>
		[Field ("PKContactFieldName")]
		Name = 1 << 3,

		/// <summary>Indicates a phonetic name field.</summary>
		[Field ("PKContactFieldPhoneticName")]
		PhoneticName = 1 << 4,
	}

	/// <summary>The user's payment credentials. All fields are read-only.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPaymentToken_Ref/index.html">Apple documentation for <c>PKPaymentToken</c></related>
	[Mac (11, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface PKPaymentToken {

		/// <summary>Developers should not use this deprecated property. Developers should use 'PaymentMethod' instead.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Export ("paymentInstrumentName", ArgumentSemantic.Copy)]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'PaymentMethod' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PaymentMethod' instead.")]
		string PaymentInstrumentName { get; }

		/// <summary>The network that funded the transaction. (Read-only)</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Export ("paymentNetwork")]
		[Deprecated (PlatformName.iOS, 9, 0, message: "Use 'PaymentMethod' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PaymentMethod' instead.")]
		string PaymentNetwork { get; }

		/// <summary>A unique identifier for the payment. (Read-only)</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transactionIdentifier")]
		string TransactionIdentifier { get; }

		/// <summary>A UTF-8-encoded serialized JSON dictionary of the payment data. (Read-only)</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("paymentData", ArgumentSemantic.Copy)]
		NSData PaymentData { get; }

		/// <summary>Information about the payment card or account used in the transaction.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("paymentMethod", ArgumentSemantic.Strong)]
		PKPaymentMethod PaymentMethod { get; }
	}

	/// <summary>A <see cref="UIKit.UIViewController" /> that manages the user experience of viewing a <see cref="PassKit.PKPass" /> and prompting the user to add it to the <see cref="PassKit.PKPassLibrary" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKAddPassesViewController_Ref/index.html">Apple documentation for <c>PKAddPassesViewController</c></related>
	[NoMac] // under `TARGET_OS_IPHONE`
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIViewController), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (PKAddPassesViewControllerDelegate) })]
	// invalid null handle for default 'init'
	[DisableDefaultCtor]
	interface PKAddPassesViewController {

		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		/// <param name="pass">To be added.</param>
		/// <summary>Creates a new <see cref="PassKit.PKAddPassesViewController" /> that displays the specified <paramref name="pass" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithPass:")]
		NativeHandle Constructor (PKPass pass);

		/// <param name="pass">To be added.</param>
		/// <summary>Creates a new <see cref="PassKit.PKAddPassesViewController" /> for the specifies passes.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithPasses:")]
		NativeHandle Constructor (PKPass [] pass);

		[iOS (16, 4), MacCatalyst (16, 4)]
		[Export ("initWithIssuerData:signature:error:")]
		NativeHandle Constructor (NSData issuerData, NSData signature, [NullAllowed] out NSError error);

		/// <summary>Whether this device supports adding passes.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Static]
		[Export ("canAddPasses")]
		bool CanAddPasses { get; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the PassKit.IPKAddPassesViewControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the PassKit.IPKAddPassesViewControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IPKAddPassesViewControllerDelegate Delegate { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="PassKit.PKAddPassesViewControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="PassKit.PKAddPassesViewControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="PassKit.PKAddPassesViewControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="PassKit.PKAddPassesViewControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IPKAddPassesViewControllerDelegate { }

	/// <summary>A delegate object that gives the application developer fine-grained control over life-cycle events of a <see cref="PassKit.PKAddPassesViewController" /> object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKAddPassesViewControllerDelegate_Ref/index.html">Apple documentation for <c>PKAddPassesViewControllerDelegate</c></related>
	[NoMac] // under `TARGET_OS_IPHONE`
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface PKAddPassesViewControllerDelegate {
		/// <param name="controller">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[EventArgs ("", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		[Export ("addPassesViewControllerDidFinish:")]
		void Finished (PKAddPassesViewController controller);
	}

	/// <summary>Used to hold card data being inserted into Apple Pay.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKAddPaymentPassRequest_Class/index.html">Apple documentation for <c>PKAddPaymentPassRequest</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // designated
	interface PKAddPaymentPassRequest : NSSecureCoding {
		/// <summary>Default constructor, initializes a new instance of this class.</summary>
		/// <remarks>
		///         </remarks>
		[DesignatedInitializer]
		[Export ("init")]
		NativeHandle Constructor ();

		/// <summary>An encrypted JSON string.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("encryptedPassData", ArgumentSemantic.Copy)]
		NSData EncryptedPassData { get; set; }

		/// <summary>The request's activation data.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("activationData", ArgumentSemantic.Copy)]
		NSData ActivationData { get; set; }

		/// <summary>The temporary public key used by the ECC schema.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("ephemeralPublicKey", ArgumentSemantic.Copy)]
		NSData EphemeralPublicKey { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("wrappedKey", ArgumentSemantic.Copy)]
		NSData WrappedKey { get; set; }
	}

	/// <summary>Holds configuration data needed by a <see cref="PassKit.PKAddPaymentPassViewController" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKAddPaymentPassRequestConfiguration_Class/index.html">Apple documentation for <c>PKAddPaymentPassRequestConfiguration</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKAddPaymentPassRequestConfiguration : NSSecureCoding {
		/// <param name="encryptionScheme">To be added.</param>
		/// <summary>Creates a new <see cref="PassKit.PKAddPaymentPassRequestConfiguration" />. In iOS 9, the only valid <paramref name="encryptionScheme" /> is <see cref="PassKit.PKEncryptionScheme.Ecc_V2" />.</summary>
		/// <remarks>To be added.</remarks>
		[DesignatedInitializer]
		[Export ("initWithEncryptionScheme:")]
		NativeHandle Constructor (NSString encryptionScheme);

		/// <summary>The encryption scheme to be used.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("encryptionScheme")]
		NSString EncryptionScheme { get; }

		/// <summary>The name as shown on the card.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("cardholderName")]
		string CardholderName { get; set; }

		/// <summary>The last four or five digits of the card.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("primaryAccountSuffix")]
		string PrimaryAccountSuffix { get; set; }

		/// <summary>Gets or sets the array of <see cref="PassKit.PKLabeledValue" /> objects that describe the card.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("cardDetails", ArgumentSemantic.Copy)]
		PKLabeledValue [] CardDetails { get; set; }

		/// <summary>Describes the card, in a localized string.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("localizedDescription")]
		string LocalizedDescription { get; set; }

		/// <summary>The primary account identifier, used to filter pass libraries.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("primaryAccountIdentifier")]
		string PrimaryAccountIdentifier { get; set; }

		/// <summary>The payment network backing the card.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("paymentNetwork")]
		string PaymentNetwork { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the Felica Secure Element is required.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("requiresFelicaSecureElement")]
		bool RequiresFelicaSecureElement { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("style", ArgumentSemantic.Assign)]
		PKAddPaymentPassStyle Style { get; set; }

		[iOS (12, 3)]
		[MacCatalyst (13, 1)]
		[Export ("productIdentifiers", ArgumentSemantic.Copy)]
		NSSet<NSString> ProductIdentifiers { get; set; }
	}

	/// <summary>A standard <see cref="UIKit.UIViewController" /> for adding cards to Apple Pay.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKAddPaymentPassViewController_Class/index.html">Apple documentation for <c>PKAddPaymentPassViewController</c></related>
	[NoMac] // under `#if TARGET_OS_IPHONE`
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIViewController))]
	[DisableDefaultCtor]
	interface PKAddPaymentPassViewController {
		/// <summary>Whether the app can add cards to Apple Pay.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canAddPaymentPass")]
		bool CanAddPaymentPass { get; }

		/// <param name="configuration">To be added.</param>
		/// <param name="viewControllerDelegate">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[DesignatedInitializer]
		[Export ("initWithRequestConfiguration:delegate:")]
		NativeHandle Constructor (PKAddPaymentPassRequestConfiguration configuration, [NullAllowed] IPKAddPaymentPassViewControllerDelegate viewControllerDelegate);

		/// <summary>An instance of the PassKit.IPKAddPaymentPassViewControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the PassKit.IPKAddPaymentPassViewControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IPKAddPaymentPassViewControllerDelegate Delegate { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="PassKit.PKAddPaymentPassViewControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="PassKit.PKAddPaymentPassViewControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="PassKit.PKAddPaymentPassViewControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="PassKit.PKAddPaymentPassViewControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IPKAddPaymentPassViewControllerDelegate { }

	/// <summary>Delegate object for <see cref="PassKit.PKAddPaymentPassViewController" /> whose members are called when prompting for an add payment request and when an <see cref="PassKit.PKAddPaymentPassRequest" /> has failed.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKAddPaymentPassViewControllerDelegate_Protocol/index.html">Apple documentation for <c>PKAddPaymentPassViewControllerDelegate</c></related>
	[NoMac] // under `#if TARGET_OS_IPHONE`
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface PKAddPaymentPassViewControllerDelegate {
		/// <param name="controller">To be added.</param>
		///         <param name="certificates">To be added.</param>
		///         <param name="nonce">To be added.</param>
		///         <param name="nonceSignature">To be added.</param>
		///         <param name="handler">To be added.</param>
		///         <summary>Called to create an "add payment" request.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("addPaymentPassViewController:generateRequestWithCertificateChain:nonce:nonceSignature:completionHandler:")]
		void GenerateRequestWithCertificateChain (PKAddPaymentPassViewController controller, NSData [] certificates, NSData nonce, NSData nonceSignature, Action<PKAddPaymentPassRequest> handler);

		/// <param name="controller">To be added.</param>
		///         <param name="pass">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Called to prompt the user for an "add payment" request.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("addPaymentPassViewController:didFinishAddingPaymentPass:error:")]
		void DidFinishAddingPaymentPass (PKAddPaymentPassViewController controller, [NullAllowed] PKPaymentPass pass, [NullAllowed] NSError error);
	}

	/// <summary>A pass, which is an abstraction of such things as tickets, boarding passes, or gift or loyalty cards.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPass_Ref/index.html">Apple documentation for <c>PKPass</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (PKObject))]
	interface PKPass : NSSecureCoding, NSCopying {
		/// <param name="data">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>Creates a new <see cref="PassKit.PKPass" />, possibly returning an error.</summary>
		/// <remarks>
		///           <para>If <paramref name="error" /> is not <see langword="null" />, it will indicate an error in creation and the resulting <see cref="PassKit.PKPass" /> should not be used.</para>
		///         </remarks>
		[Export ("initWithData:error:")]
		NativeHandle Constructor (NSData data, out NSError error);

		/// <summary>Used to authenticate with the Web service.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("authenticationToken", ArgumentSemantic.Copy)]
		string AuthenticationToken { get; }

		/// <summary>The icon for the pass.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[NoMacCatalyst]
		[Export ("icon", ArgumentSemantic.Copy)]
		UIImage Icon { get; }

		/// <summary>A localized description of the pass.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("localizedDescription", ArgumentSemantic.Copy)]
		string LocalizedDescription { get; }

		/// <summary>A localized description of the pass's kind.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("localizedName", ArgumentSemantic.Copy)]
		string LocalizedName { get; }

		/// <summary>The organization that created the pass.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("organizationName", ArgumentSemantic.Copy)]
		string OrganizationName { get; }

		/// <summary>Identifies the <see cref="PassKit.PKPass.PKPassType" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("passTypeIdentifier", ArgumentSemantic.Copy)]
		string PassTypeIdentifier { get; }

		/// <summary>The URL that will open the pass in the Passbook app.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed]
		[Export ("passURL", ArgumentSemantic.Copy)]
		NSUrl PassUrl { get; }

		/// <summary>The date when the pass is most likely to be needed.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'RelevantDates' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'RelevantDates' instead.")]
		[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'RelevantDates' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'RelevantDates' instead.")]
		[NullAllowed, Export ("relevantDate", ArgumentSemantic.Copy)]
		NSDate RelevantDate { get; }

		/// <summary>A unique identifier for the pass.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("serialNumber", ArgumentSemantic.Copy)]
		string SerialNumber { get; }

		/// <summary>The URL of the developer's Web service.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("webServiceURL", ArgumentSemantic.Copy)]
		NSUrl WebServiceUrl { get; }

		/// <param name="key">A value from <see cref="PassKit.PKPassLibraryUserInfoKey" />.</param>
		///         <summary>Returns the localized value for the provided <paramref name="key" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("localizedValueForFieldKey:")]
		[return: NullAllowed]
		NSObject GetLocalizedValue (NSString key); // TODO: Should be enum for PKPassLibraryUserInfoKey

		/// <summary>Developer-specified extra data.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("userInfo", ArgumentSemantic.Copy)]
		NSDictionary UserInfo { get; }

		/// <summary>The <see cref="PassKit.PKPassType" /> of the pass.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("passType")]
		PKPassType PassType { get; }

		/// <summary>If not <see langword="null" />, the underlying <see cref="PassKit.PKPaymentPass" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'SecureElementPass' instead.")]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'SecureElementPass' instead.")]
		[Export ("paymentPass")]
		PKPaymentPass PaymentPass { get; }

		[iOS (13, 4)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("secureElementPass")]
		PKSecureElementPass SecureElementPass { get; }

		/// <summary>Whether the pass is stored on a peer device (e.g., an Apple Watch).</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("remotePass")]
		bool RemotePass { [Bind ("isRemotePass")] get; }

		/// <summary>The name of the device hosting the pass.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("deviceName")]
		string DeviceName { get; }

		[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("relevantDates", ArgumentSemantic.Copy)]
		PKPassRelevantDate [] RelevantDates { get; }
	}

	/// <summary>Information about Apple Pay cards.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPaymentMethod_Class/index.html">Apple documentation for <c>PKPaymentMethod</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface PKPaymentMethod : NSSecureCoding {
		/// <summary>A user-meaningful description of the card.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("displayName")]
		string DisplayName { get; }

		/// <summary>A user-meaningful name of the payment network backing the card.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("network")]
		string Network { get; }

		/// <summary>The <see cref="PassKit.PKPaymentMethodType" /> of the <see cref="PassKit.PKPaymentMethod" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("type")]
		PKPaymentMethodType Type { get; }

		/// <summary>Gets the payment pass for the method, if present.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'SecureElementPass' instead.")]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'SecureElementPass' instead.")]
		[NullAllowed, Export ("paymentPass", ArgumentSemantic.Copy)]
		PKPaymentPass PaymentPass { get; }

		[iOS (13, 4)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("secureElementPass", ArgumentSemantic.Copy)]
		PKSecureElementPass SecureElementPass { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("billingAddress", ArgumentSemantic.Copy)]
		CNContact BillingAddress { get; }
	}

	/// <summary>A provisioned payment card that may be used for in-app purchases. (All fields are read-only)</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPaymentPass_Ref/index.html">Apple documentation for <c>PKPaymentPass</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (PKSecureElementPass))]
	interface PKPaymentPass {

		/// <summary>The <see cref="PassKit.PKPaymentPassActivationState" /> of the pass. (Read-only)</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 13, 4, message: "Use 'PKSecureElementPass.PassActivationState' instead.")]
		[NoMac]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'PKSecureElementPass.PassActivationState' instead.")]
		[Export ("activationState")]
		PKPaymentPassActivationState ActivationState { get; }
	}

	/// <summary>Base class for <see cref="PassKit.PKPass" />. Defines copy and encoding methods.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKObject_Class/index.html">Apple documentation for <c>PKObject</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	partial interface PKObject : NSCoding, NSSecureCoding, NSCopying {
		//Empty class in header file
	}

	/// <summary>Standard values returned by <see cref="PassKit.PKPaymentToken.PaymentNetwork" />.</summary>
	[Static]
	[MacCatalyst (13, 1)]
	interface PKPaymentNetwork {
		/// <summary>Represents the value associated with the constant PKPaymentNetworkAmex</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("PKPaymentNetworkAmex")]
		NSString Amex { get; }

		/// <summary>Developers should not use this deprecated property. Developers should use 'CartesBancaires' instead.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'CartesBancaires' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CartesBancaires' instead.")]
		[Field ("PKPaymentNetworkCarteBancaire")]
		NSString CarteBancaire { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 2, message: "Use 'CartesBancaires' instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'CartesBancaires' instead.")]
		[Field ("PKPaymentNetworkCarteBancaires")]
		NSString CarteBancaires { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkCartesBancaires")]
		NSString CartesBancaires { get; }

		/// <summary>Represents the value associated with the constant PKPaymentNetworkChinaUnionPay.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkChinaUnionPay")]
		NSString ChinaUnionPay { get; }

		[Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Field ("PKPaymentNetworkDankort")]
		NSString Dankort { get; }

		/// <summary>Represents the value associated with the constant PKPaymentNetworkInterac.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkInterac")]
		NSString Interac { get; }

		/// <summary>Represents the value associated with the constant PKPaymentNetworkMasterCard</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("PKPaymentNetworkMasterCard")]
		NSString MasterCard { get; }

		/// <summary>Represents the value associated with the constant PKPaymentNetworkVisa</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("PKPaymentNetworkVisa")]
		NSString Visa { get; }

		/// <summary>The Discover payment network.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkDiscover")]
		NSString Discover { get; }

		/// <summary>Represents the value associated with the constant PKPaymentNetworkPrivateLabel.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkPrivateLabel")]
		NSString PrivateLabel { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkJCB")]
		NSString Jcb { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkSuica")]
		NSString Suica { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkQuicPay")]
		NSString QuicPay { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkIDCredit")]
		NSString IDCredit { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkElectron")]
		NSString Electron { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkMaestro")]
		NSString Maestro { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkVPay")]
		NSString VPay { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkEftpos")]
		NSString Eftpos { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkElo")]
		NSString Elo { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentNetworkMada")]
		NSString Mada { get; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("PKPaymentNetworkBarcode")]
		NSString Barcode { get; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("PKPaymentNetworkGirocard")]
		NSString Girocard { get; }

		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		[Field ("PKPaymentNetworkMir")]
		NSString Mir { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("PKPaymentNetworkNanaco")]
		NSString Nanaco { get; }

		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
		[Field ("PKPaymentNetworkPostFinance")]
		NSString PKPaymentNetworkPostFinance { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Field ("PKPaymentNetworkWaon")]
		NSString Waon { get; }

		[iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("PKPaymentNetworkBancomat")]
		NSString Bancomat { get; }

		[iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)]
		[Field ("PKPaymentNetworkBancontact")]
		NSString Bancontact { get; }

		[iOS (17, 0), Mac (14, 0), NoTV, MacCatalyst (17, 0)]
		[Field ("PKPaymentNetworkPagoBancomat")]
		NSString PagoBancomat { get; }

		[iOS (17, 0), Mac (14, 0), NoTV, MacCatalyst (17, 0)]
		[Field ("PKPaymentNetworkTmoney")]
		NSString Tmoney { get; }

		[Mac (14, 4), iOS (17, 4), NoTV, MacCatalyst (17, 4)]
		[Field ("PKPaymentNetworkMeeza")]
		NSString Meeza { get; }

		[Mac (14, 5), iOS (17, 5), NoTV, MacCatalyst (17, 5)]
		[Field ("PKPaymentNetworkBankAxept")]
		NSString BankAxept { get; }

		[Mac (14, 5), iOS (17, 5), NoTV, MacCatalyst (17, 5)]
		[Field ("PKPaymentNetworkNAPAS")]
		NSString Napas { get; }

		[Mac (15, 4), iOS (18, 4), NoTV, MacCatalyst (18, 4)]
		[Field ("PKPaymentNetworkHimyan")]
		NSString Himyan { get; }

		[Mac (15, 4), iOS (18, 4), NoTV, MacCatalyst (18, 4)]
		[Field ("PKPaymentNetworkJaywan")]
		NSString Jaywan { get; }
	}

	/// <summary>A button used to activate an Apple Pay payment. Available styles and types are defined by <see cref="PassKit.PKPaymentButtonStyle" /> and <see cref="PassKit.PKPaymentButtonType" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/PassKit/Reference/PKPaymentButton_Class/index.html">Apple documentation for <c>PKPaymentButton</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIButton))]
	[DisableDefaultCtor]
	interface PKPaymentButton {

		/// <param name="buttonType">To be added.</param>
		///         <param name="buttonStyle">To be added.</param>
		///         <summary>Factory method to create a new <see cref="PassKit.PKPaymentButton" /> with the specified <see cref="PassKit.PKPaymentButtonType" /> and <see cref="PassKit.PKPaymentButtonStyle" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("buttonWithType:style:")]
		// note: named like UIButton method
		PKPaymentButton FromType (PKPaymentButtonType buttonType, PKPaymentButtonStyle buttonStyle);

		/// <param name="type">To be added.</param>
		/// <param name="style">To be added.</param>
		/// <summary>Creates a new Pass Kit payment button with the specified <paramref name="type" /> and <paramref name="style" />.</summary>
		/// <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("initWithPaymentButtonType:paymentButtonStyle:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKPaymentButtonType type, PKPaymentButtonStyle style);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("cornerRadius")]
		nfloat CornerRadius { get; set; }
	}

	/// <summary>A button that adds passes to a Wallet.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Miscellaneous/Reference/PKAddPassButton_Class/index.html">Apple documentation for <c>PKAddPassButton</c></related>
	[NoMac] // under `#if TARGET_OS_IOS`
	[MacCatalyst (13, 1)]
	[BaseType (typeof (UIButton))]
	[DisableDefaultCtor]
	interface PKAddPassButton {
		/// <param name="addPassButtonStyle">To be added.</param>
		///         <summary>Creates and returns a new button, with the specified button <paramref name="addPassButtonStyle" />, for adding passes to the Wallet.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("addPassButtonWithStyle:")]
		PKAddPassButton Create (PKAddPassButtonStyle addPassButtonStyle);

		/// <param name="style">To be added.</param>
		/// <summary>Creates a new button, with the specified button <paramref name="style" />, for adding passes to the Wallet.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithAddPassButtonStyle:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKAddPassButtonStyle style);

		/// <summary>Gets the style of the button.</summary>
		///         <value>To be added.</value>
		///         <remarks>
		///           <para id="tool-remark">This member participates in the <see cref="UIKit.UIAppearance" /> styling system.  See the <see cref="PassKit.PKAddPassButton.Appearance" /> property and the <see cref="PassKit.PKAddPassButton.AppearanceWhenContainedIn(System.Type[])" /> method.</para>
		///         </remarks>
		[Appearance]
		[Export ("addPassButtonStyle", ArgumentSemantic.Assign)]
		PKAddPassButtonStyle Style { get; set; }
	}

	/// <summary>Defines the constant string <see cref="PassKit.PKEncryptionScheme.Ecc_V2" />.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	interface PKEncryptionScheme {
		/// <summary>Elliptical Curve Cryptography, version 2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("PKEncryptionSchemeECC_V2")]
		NSString Ecc_V2 { get; }

		/// <summary>RSA v2.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKEncryptionSchemeRSA_V2")]
		NSString Rsa_V2 { get; }
	}

	/// <summary>Presents a payment authorization user interface to the user and acts on the user's response.</summary>
	///     <remarks>
	///       <para>This class performs the same job as <see cref="PassKit.PKPaymentAuthorizationViewController" /> but does not rely on UIKit. Because of this, this view controller can be used in watchOS apps and in intents extensions.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/PassKit/PKPaymentAuthorizationController">Apple documentation for <c>PKPaymentAuthorizationController</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // providing DesignatedInitializer
	interface PKPaymentAuthorizationController {

		/// <summary>Gets a value that tells whether the user can make payments.</summary>
		///         <value>A value that tells whether the user can make payments.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canMakePayments")]
		bool CanMakePayments { get; }

		/// <param name="supportedNetworks">To be added.</param>
		///         <summary>Gets a value that tells wether the user can make payments in at least one of the specified <paramref name="supportedNetworks" />.</summary>
		///         <returns>A value that tells wether the user can make payments in at least one of the specified <paramref name="supportedNetworks" />.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canMakePaymentsUsingNetworks:")]
		bool CanMakePaymentsUsingNetworks (string [] supportedNetworks);

		/// <param name="supportedNetworks">To be added.</param>
		///         <param name="capabilties">To be added.</param>
		///         <summary>Gets a value that tells wether the user can make payments in at least one of the specified <paramref name="supportedNetworks" /> with the specified <paramref name="capabilties" />.</summary>
		///         <returns>A value that tells wether the user can make payments in at least one of the specified <paramref name="supportedNetworks" /> with the specified <paramref name="capabilties" />.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("canMakePaymentsUsingNetworks:capabilities:")]
#if XAMCORE_5_0
		bool CanMakePaymentsUsingNetworks (string [] supportedNetworks, PKMerchantCapability capabilities);
#else
		bool CanMakePaymentsUsingNetworks (string [] supportedNetworks, PKMerchantCapability capabilties);
#endif

		/// <summary>An instance of the PassKit.IPKPaymentAuthorizationControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the PassKit.IPKPaymentAuthorizationControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		IPKPaymentAuthorizationControllerDelegate Delegate { get; set; }

		/// <param name="request">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithPaymentRequest:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKPaymentRequest request);

		/// <param name="completion">
		///           <para>A handler that is called after the payment authorization UI is presented.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Presents the payment authorization UI and runs a handler after the sheet is displayed.</summary>
		///         <remarks>The developer must use the <see cref="PassKit.PKPaymentAuthorizationController.Dismiss(System.Action)" /> method to dismiss the payment authorization UI.</remarks>
		[Async (XmlDocs = """
			<summary>Presents the payment authorization UI and runs a handler after the sheet is displayed.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous Present operation.  The value of the TResult parameter is of type System.Action&lt;System.Boolean&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("presentWithCompletion:")]
		void Present ([NullAllowed] Action<bool> completion);

		/// <param name="completion">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <summary>Dismisses the payment authorization UI and runs the specified completion handler.</summary>
		///         <remarks>Developers call this method to dismiss the payment authorization UI, typically when they receive a call to the <see cref="PassKit.PKPaymentAuthorizationControllerDelegate.DidFinish(PassKit.PKPaymentAuthorizationController)" /> method.</remarks>
		[Async (XmlDocs = """
			<summary>Dismisses the payment authorization UI and runs the specified completion handler.</summary>
			<returns>A task that represents the asynchronous Dismiss operation</returns>
			<remarks>
			          <para>The DismissAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <see cref="PassKit.PKPaymentAuthorizationControllerDelegate.DidFinish(PassKit.PKPaymentAuthorizationController)" />
			          <para copied="true">The DismissAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <see cref="PassKit.PKPaymentAuthorizationControllerDelegate.DidFinish(PassKit.PKPaymentAuthorizationController)" copied="true" />
			        </remarks>
			""")]
		[Export ("dismissWithCompletion:")]
		void Dismiss ([NullAllowed] Action completion);

		[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Static]
		[Export ("supportsDisbursements")]
		bool SupportsDisbursements ();

		[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Static]
		[Export ("supportsDisbursementsUsingNetworks:")]
		bool SupportsDisbursements (string [] supportedNetworks);

		[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Static]
		[Export ("supportsDisbursementsUsingNetworks:capabilities:")]
		bool SupportsDisbursements (string [] supportedNetworks, PKMerchantCapability capabilities);

		[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
		[Export ("initWithDisbursementRequest:")]
		NativeHandle Constructor (PKDisbursementRequest request);
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="PassKit.PKPaymentAuthorizationControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="PassKit.PKPaymentAuthorizationControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="PassKit.PKPaymentAuthorizationControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="PassKit.PKPaymentAuthorizationControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IPKPaymentAuthorizationControllerDelegate { }

	/// <summary>Delegate object that responds to user interactions on behalf of a <see cref="PassKit.PKPaymentAuthorizationController" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/PassKit/PKPaymentAuthorizationControllerDelegate">Apple documentation for <c>PKPaymentAuthorizationControllerDelegate</c></related>
	[Mac (11, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	[Model]
	[BaseType (typeof (NSObject))]
	interface PKPaymentAuthorizationControllerDelegate {

		/// <param name="controller">To be added.</param>
		///         <param name="payment">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>Developers should not use this deprecated method. Developers should use 'DidAuthorizePayment' overload with the 'Action&lt;PKPaymentAuthorizationResult&gt;' parameter instead.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'DidAuthorizePayment' overload with the 'Action<PKPaymentAuthorizationResult>' parameter instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidAuthorizePayment' overload with the 'Action<PKPaymentAuthorizationResult>' parameter instead.")]
		[Export ("paymentAuthorizationController:didAuthorizePayment:completion:")]
		void DidAuthorizePayment (PKPaymentAuthorizationController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion);

		/// <param name="controller">To be added.</param>
		///         <param name="payment">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("paymentAuthorizationController:didAuthorizePayment:handler:")]
		void DidAuthorizePayment (PKPaymentAuthorizationController controller, PKPayment payment, Action<PKPaymentAuthorizationResult> completion);

		/// <param name="controller">The <see cref="PassKit.PKPaymentAuthorizationController" /> for which the payment authorization has finished.</param>
		///         <summary>Method that is called when payment authorization has finished.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("paymentAuthorizationControllerDidFinish:")]
		void DidFinish (PKPaymentAuthorizationController controller);

		/// <param name="controller">The controller that owns this delegate.</param>
		///         <summary>Method that is called when the user is authorizing a payment request.</summary>
		///         <remarks>This method is called after the user authenticates, but before the request is authorized.</remarks>
		[Export ("paymentAuthorizationControllerWillAuthorizePayment:")]
		void WillAuthorizePayment (PKPaymentAuthorizationController controller);

		/// <param name="controller">The controller that owns this delegate.</param>
		///         <param name="shippingMethod">The new shipping method.</param>
		///         <param name="completion">A handler that takes the authorization status for the payment and a list of updated payment summary items.</param>
		///         <summary>Method that is called when a user selects a new shipping method.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'DidSelectShippingMethod' overload with the 'Action<PKPaymentRequestPaymentMethodUpdate>' parameter instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidSelectShippingMethod' overload with the 'Action<PKPaymentRequestPaymentMethodUpdate>' parameter instead.")]
		[Export ("paymentAuthorizationController:didSelectShippingMethod:completion:")]
		void DidSelectShippingMethod (PKPaymentAuthorizationController controller, PKShippingMethod shippingMethod, Action<PKPaymentAuthorizationStatus, PKPaymentSummaryItem []> completion);

		/// <param name="controller">To be added.</param>
		///         <param name="paymentMethod">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("paymentAuthorizationController:didSelectShippingMethod:handler:")]
		void DidSelectShippingMethod (PKPaymentAuthorizationController controller, PKPaymentMethod paymentMethod, Action<PKPaymentRequestPaymentMethodUpdate> completion);

		/// <param name="controller">The controller that owns this delegate.</param>
		///         <param name="contact">The new shipping address.</param>
		///         <param name="completion">A handler that takes the payment authorization status, a list of updated shipping method objects, and a list of updated payment summary items.</param>
		///         <summary>Method that is called when a user selects a contact to ship to.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'DidSelectShippingContact' overload with the 'Action<PKPaymentRequestShippingContactUpdate>' parameter instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidSelectShippingContact' overload with the 'Action<PKPaymentRequestShippingContactUpdate>' parameter instead.")]
		[Export ("paymentAuthorizationController:didSelectShippingContact:completion:")]
		void DidSelectShippingContact (PKPaymentAuthorizationController controller, PKContact contact, Action<PKPaymentAuthorizationStatus, PKShippingMethod [], PKPaymentSummaryItem []> completion);

		/// <param name="controller">To be added.</param>
		///         <param name="contact">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("paymentAuthorizationController:didSelectShippingContact:handler:")]
		void DidSelectShippingContact (PKPaymentAuthorizationController controller, PKContact contact, Action<PKPaymentRequestShippingContactUpdate> completion);

		/// <param name="controller">The controller that owns this delegate.</param>
		///         <param name="paymentMethod">The payment method that was selected.</param>
		///         <param name="completion">A handler that takes a list of updated payment summary items.</param>
		///         <summary>Mehod that is called when the user selects a payment method.</summary>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'DidSelectPaymentMethod' overload with the 'Action<PKPaymentRequestPaymentMethodUpdate>' parameter instead.")]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'DidSelectPaymentMethod' overload with the 'Action<PKPaymentRequestPaymentMethodUpdate>' parameter instead.")]
		[Export ("paymentAuthorizationController:didSelectPaymentMethod:completion:")]
		void DidSelectPaymentMethod (PKPaymentAuthorizationController controller, PKPaymentMethod paymentMethod, Action<PKPaymentSummaryItem []> completion);

		/// <param name="controller">To be added.</param>
		///         <param name="paymentMethod">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("paymentAuthorizationController:didSelectPaymentMethod:handler:")]
		void DidSelectPaymentMethod (PKPaymentAuthorizationController controller, PKPaymentMethod paymentMethod, Action<PKPaymentRequestPaymentMethodUpdate> completion);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("paymentAuthorizationController:didRequestMerchantSessionUpdate:")]
		void DidRequestMerchantSessionUpdate (PKPaymentAuthorizationController controller, Action<PKPaymentRequestMerchantSessionUpdate> handler);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("paymentAuthorizationController:didChangeCouponCode:handler:")]
		void DidChangeCouponCode (PKPaymentAuthorizationController controller, string couponCode, Action<PKPaymentRequestCouponCodeUpdate> completion);

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("presentationWindowForPaymentAuthorizationController:")]
		[return: NullAllowed]
#if MONOMAC || __MACCATALYST__
		[Abstract]
#endif
		UIWindow GetPresentationWindow (PKPaymentAuthorizationController controller);
	}

	/// <summary>A labeled value for card details.</summary>
	[Mac (11, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // there's a designated initializer and it does not accept null
	interface PKLabeledValue {
		/// <param name="label">To be added.</param>
		/// <param name="value">To be added.</param>
		/// <summary>Creates a new <see cref="PassKit.PKLabeledValue" /> with the specified label and value.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithLabel:value:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string label, string value);

		/// <summary>Gets the label.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("label")]
		string Label { get; }

		/// <summary>Gets the value.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("value")]
		string Value { get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (PKStoredValuePassProperties))]
	[DisableDefaultCtor]
	interface PKTransitPassProperties {

		/// <param name="pass">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("passPropertiesForPass:")]
		[return: NullAllowed]
		PKTransitPassProperties GetPassProperties (PKPass pass);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 15, 0)]
		[Deprecated (PlatformName.MacOSX, 12, 0)]
		[Deprecated (PlatformName.MacCatalyst, 15, 0)]
		[Export ("transitBalance", ArgumentSemantic.Copy)]
		NSDecimalNumber TransitBalance { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 15, 0)]
		[Deprecated (PlatformName.MacOSX, 12, 0)]
		[Deprecated (PlatformName.MacCatalyst, 15, 0)]
		[Export ("transitBalanceCurrencyCode")]
		string TransitBalanceCurrencyCode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("inStation")]
		bool InStation { [Bind ("isInStation")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 14, 5, message: "Use 'Blocked' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 5, message: "Use 'Blocked' instead.")]
		[Deprecated (PlatformName.MacOSX, 11, 3, message: "Use 'Blocked' instead.")]
		[Export ("blacklisted")]
		bool Blacklisted { [Bind ("isBlacklisted")] get; }

		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		[Export ("blocked")]
		bool Blocked { [Bind ("isBlocked")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("expirationDate", ArgumentSemantic.Copy)]
		NSDate ExpirationDate { get; }
	}

	/// <summary>Contains Suica pass properties.</summary>
	[Mac (11, 0)]
	[MacCatalyst (13, 1)]
	[DisableDefaultCtor] // hint: getter only props and a factory method.
	[BaseType (typeof (PKTransitPassProperties))]
	interface PKSuicaPassProperties {
		/// <param name="pass">The pass for which to get properties.</param>
		///         <summary>Returns the properties for the specified pass.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("passPropertiesForPass:")]
		[return: NullAllowed]
		PKSuicaPassProperties GetPassProperties (PKPass pass);

		/// <summary>Gets the balance on the pass.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transitBalance", ArgumentSemantic.Copy)]
		NSDecimalNumber TransitBalance { get; }

		/// <summary>Gets the currency of the balance.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transitBalanceCurrencyCode")]
		string TransitBalanceCurrencyCode { get; }

		/// <summary>Gets a value that tells whether the pass works in a transit station.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("inStation")]
		bool InStation { [Bind ("isInStation")] get; }

		/// <summary>Gets a value that tells whether the pass works in the Shinkansen Station.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("inShinkansenStation")]
		bool InShinkansenStation { [Bind ("isInShinkansenStation")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("balanceAllowedForCommute")]
		bool BalanceAllowedForCommute { [Bind ("isBalanceAllowedForCommute")] get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("lowBalanceGateNotificationEnabled")]
		bool LowBalanceGateNotificationEnabled { [Bind ("isLowBalanceGateNotificationEnabled")] get; }

		/// <summary>Gets a value that tells whether the pass works with the Green Car service.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("greenCarTicketUsed")]
		bool GreenCarTicketUsed { [Bind ("isGreenCarTicketUsed")] get; }

		/// <summary>Gets a value that tells whether the pass is blacklisted.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("blacklisted")]
		[Deprecated (PlatformName.iOS, 14, 5, message: "Use 'Blocked' instead.")] // exists in base class
		[Deprecated (PlatformName.MacOSX, 11, 3, message: "Use 'Blocked' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 5, message: "Use 'Blocked' instead.")]
		bool Blacklisted { [Bind ("isBlacklisted")] get; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKPaymentAuthorizationResult {
		/// <param name="status">To be added.</param>
		/// <param name="errors">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithStatus:errors:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKPaymentAuthorizationStatus status, [NullAllowed] NSError [] errors);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("status", ArgumentSemantic.Assign)]
		PKPaymentAuthorizationStatus Status { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("errors", ArgumentSemantic.Copy)]
		NSError [] Errors { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[NullAllowed, Export ("orderDetails", ArgumentSemantic.Strong)]
		PKPaymentOrderDetails OrderDetails { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKPaymentRequestUpdate {

		/// <param name="paymentSummaryItems">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithPaymentSummaryItems:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKPaymentSummaryItem [] paymentSummaryItems);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("status", ArgumentSemantic.Assign)]
		PKPaymentAuthorizationStatus Status { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("paymentSummaryItems", ArgumentSemantic.Copy)]
		PKPaymentSummaryItem [] PaymentSummaryItems { get; set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("shippingMethods", ArgumentSemantic.Copy)]
		PKShippingMethod [] ShippingMethods { get; set; }

		[Mac (13, 0), iOS (16, 0), NoTV, MacCatalyst (16, 0)]
		[NullAllowed, Export ("multiTokenContexts", ArgumentSemantic.Copy)]
		PKPaymentTokenContext [] MultiTokenContexts { get; set; }

		[Mac (13, 0), iOS (16, 0), NoTV, MacCatalyst (16, 0)]
		[NullAllowed, Export ("recurringPaymentRequest", ArgumentSemantic.Strong)]
		PKRecurringPaymentRequest RecurringPaymentRequest { get; set; }

		[Mac (13, 0), iOS (16, 0), NoTV, MacCatalyst (16, 0)]
		[NullAllowed, Export ("automaticReloadPaymentRequest", ArgumentSemantic.Strong)]
		PKAutomaticReloadPaymentRequest AutomaticReloadPaymentRequest { get; set; }

		[NullAllowed]
		[Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4), NoTV]
		[Export ("deferredPaymentRequest", ArgumentSemantic.Strong)]
		PKDeferredPaymentRequest DeferredPaymentRequest { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (PKPaymentRequestUpdate))]
	[DisableDefaultCtor]
	interface PKPaymentRequestShippingContactUpdate {

		/// <param name="errors">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="paymentSummaryItems">To be added.</param>
		/// <param name="shippingMethods">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithErrors:paymentSummaryItems:shippingMethods:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] NSError [] errors, PKPaymentSummaryItem [] paymentSummaryItems, PKShippingMethod [] shippingMethods);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shippingMethods", ArgumentSemantic.Copy)]
		PKShippingMethod [] ShippingMethods { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("errors", ArgumentSemantic.Copy)]
		NSError [] Errors { get; set; }
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (PKPaymentRequestUpdate))]
	[DisableDefaultCtor]
	interface PKPaymentRequestShippingMethodUpdate {

		// inlined
		/// <param name="paymentSummaryItems">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithPaymentSummaryItems:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKPaymentSummaryItem [] paymentSummaryItems);
	}

	[MacCatalyst (13, 1)]
	[BaseType (typeof (PKPaymentRequestUpdate))]
	[DisableDefaultCtor]
	interface PKPaymentRequestPaymentMethodUpdate {

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("initWithErrors:paymentSummaryItems:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] NSError [] errors, PKPaymentSummaryItem [] paymentSummaryItems);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("errors", ArgumentSemantic.Copy)]
		NSError [] Errors { get; set; }

		// inlined
		/// <param name="paymentSummaryItems">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithPaymentSummaryItems:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKPaymentSummaryItem [] paymentSummaryItems);
	}

	/// <summary>Enumerates fields that caused payment errors.</summary>
	[Mac (11, 0)]
	[MacCatalyst (13, 1)]
	[Static] // not to enum'ify - exposed as NSString inside NSError
	interface PKPaymentErrorKeys {

		/// <summary>Gets a key that identifes the contact field as the source of the error.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentErrorContactFieldUserInfoKey")]
		NSString ContactFieldUserInfoKey { get; }

		/// <summary>Gets a key that identifes the postal address field as the source of the error.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("PKPaymentErrorPostalAddressUserInfoKey")]
		NSString PostalAddressUserInfoKey { get; }
	}

	interface IPKDisbursementAuthorizationControllerDelegate { }

#if !XAMCORE_5_0
	[NoMac] // only used in non-macOS API
	[MacCatalyst (13, 1)]
	[Obsoleted (PlatformName.iOS, 17, 0, message: "No longer used.")]
	[Obsoleted (PlatformName.MacCatalyst, 17, 0, message: "No longer used.")]
	[Native]
	public enum PKDisbursementRequestSchedule : long {
		/// <summary>To be added.</summary>
		OneTime,
		/// <summary>To be added.</summary>
		Future,
	}
#endif

	[iOS (17, 0)]
	[Mac (15, 0)]
	[MacCatalyst (17, 0)]
	[NoTV]
	[BaseType (typeof (NSObject))]
	interface PKDisbursementRequest {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("currencyCode")]
		string CurrencyCode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("summaryItems", ArgumentSemantic.Copy)]
		PKPaymentSummaryItem [] SummaryItems { get; set; }

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[Export ("merchantIdentifier")]
		string MerchantIdentifier { get; set; }

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[Export ("regionCode")]
		string RegionCode { get; set; }

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[Export ("supportedNetworks", ArgumentSemantic.Copy)]
		string [] SupportedNetworks { get; set; }

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[Export ("merchantCapabilities", ArgumentSemantic.Assign)]
		PKMerchantCapability MerchantCapabilities { get; set; }

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[Export ("requiredRecipientContactFields", ArgumentSemantic.Strong)]
		string [] RequiredRecipientContactFields { get; set; }

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[NullAllowed, Export ("recipientContact", ArgumentSemantic.Strong)]
		PKContact RecipientContact { get; set; }

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[NullAllowed, Export ("supportedRegions", ArgumentSemantic.Copy)]
		string [] SupportedRegions { get; set; }

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[NullAllowed, Export ("applicationData", ArgumentSemantic.Copy)]
		NSData ApplicationData { get; set; }

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[Export ("initWithMerchantIdentifier:currencyCode:regionCode:supportedNetworks:merchantCapabilities:summaryItems:")]
		NativeHandle Constructor (string merchantIdentifier, string currencyCode, string regionCode, string [] supportedNetworks, PKMerchantCapability merchantCapabilities, PKPaymentSummaryItem [] summaryItems);

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[Static]
		[Export ("disbursementContactInvalidErrorWithContactField:localizedDescription:")]
		NSError GetDisbursementContactInvalidError (string field, [NullAllowed] string localizedDescription);

		[iOS (17, 0), Mac (15, 0), NoTV, MacCatalyst (17, 0)]
		[Static]
		[Export ("disbursementCardUnsupportedError")]
		NSError DisbursementCardUnsupportedError { get; }
	}

	[iOS (13, 4)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (PKPass))]
	[DisableDefaultCtor]
	interface PKSecureElementPass {

		/// <summary>An obfuscated unique identifier for the account number of the payment card. (Read-only)</summary>
		[Export ("primaryAccountIdentifier")]
		string PrimaryAccountIdentifier { get; }

		/// <summary>A version of the <see cref="PrimaryAccountIdentifier" /> to be displayed to the user. (Read-only)</summary>
		[Export ("primaryAccountNumberSuffix")]
		string PrimaryAccountNumberSuffix { get; }

		/// <summary>The device-specific account number. (Read-only)</summary>
		[Export ("deviceAccountIdentifier", ArgumentSemantic.Strong)]
		string DeviceAccountIdentifier { get; }

		/// <summary>A version of the <see cref="DeviceAccountIdentifier" /> to be displayed to the user. (Read-only)</summary>
		[Export ("deviceAccountNumberSuffix", ArgumentSemantic.Strong)]
		string DeviceAccountNumberSuffix { get; }

		[Export ("passActivationState")]
		PKSecureElementPassActivationState PassActivationState { get; }

		[NullAllowed, Export ("devicePassIdentifier")]
		string DevicePassIdentifier { get; }

		[NullAllowed, Export ("pairedTerminalIdentifier")]
		string PairedTerminalIdentifier { get; }
	}

	[NoTV]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum PKAddShareablePassConfigurationPrimaryAction : ulong {
		Add,
		Share,
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[NoTV]
	[Native]
	public enum PKBarcodeEventConfigurationDataType : long {
		Unknown,
		SigningKeyMaterial,
		SigningCertificate,
	}

	[NoTV, NoMac]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum PKIssuerProvisioningExtensionAuthorizationResult : long {
		Canceled,
		Authorized,
	}

	[NoTV]
	[iOS (13, 4)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKAddSecureElementPassConfiguration {

		[NullAllowed, Export ("issuerIdentifier")]
		string IssuerIdentifier { get; set; }

		[NullAllowed, Export ("localizedDescription")]
		string LocalizedDescription { get; set; }
	}

	[NoTV]
	[iOS (13, 4)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (PKAddSecureElementPassConfiguration))]
	// note: `init` is present in headers
	interface PKAddCarKeyPassConfiguration {

		[Export ("password")]
		string Password { get; set; }

		// headers say but PKAddSecureElementPassConfiguration is not supported for watch
		[iOS (14, 5)]
		[MacCatalyst (14, 5)]
		[Export ("supportedRadioTechnologies", ArgumentSemantic.Assign)]
		PKRadioTechnology SupportedRadioTechnologies { get; set; }

		// headers say but PKAddSecureElementPassConfiguration is not supported for watch
		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0), NoTV]
		[Export ("manufacturerIdentifier")]
		string ManufacturerIdentifier { get; set; }

		// headers say but PKAddSecureElementPassConfiguration is not supported for watch
		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0), NoTV]
		[NullAllowed, Export ("provisioningTemplateIdentifier", ArgumentSemantic.Strong)]
		string ProvisioningTemplateIdentifier { get; set; }
	}

	interface IPKAddSecureElementPassViewControllerDelegate { }

	[NoTV, NoMac] // under `#if TARGET_OS_IOS`
	[iOS (13, 4)]
	[MacCatalyst (13, 1)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface PKAddSecureElementPassViewControllerDelegate {

		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'DidFinishAddingSecureElementPasses' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'DidFinishAddingSecureElementPasses' instead.")]
#if !XAMCORE_5_0
		[Abstract]
#endif
		[Export ("addSecureElementPassViewController:didFinishAddingSecureElementPass:error:")]
		void DidFinishAddingSecureElementPass (PKAddSecureElementPassViewController controller, [NullAllowed] PKSecureElementPass pass, [NullAllowed] NSError error);

		[Abstract]
		[Export ("addSecureElementPassViewController:didFinishAddingSecureElementPasses:error:")]
		void DidFinishAddingSecureElementPasses (PKAddSecureElementPassViewController controller, [NullAllowed] PKSecureElementPass [] passes, [NullAllowed] NSError error);
	}

	[NoTV, NoMac] // under `#if TARGET_OS_IOS`
	[iOS (13, 4)]
	[MacCatalyst (14, 0)] // doc mention 13.4 but we can't load the class
	[BaseType (typeof (UIViewController))]
	[DisableDefaultCtor]
	interface PKAddSecureElementPassViewController {

		[Static]
		[Export ("canAddSecureElementPassWithConfiguration:")]
		bool CanAddSecureElementPass (PKAddSecureElementPassConfiguration configuration);

		[Export ("initWithConfiguration:delegate:")]
		NativeHandle Constructor (PKAddSecureElementPassConfiguration configuration, [NullAllowed] IPKAddSecureElementPassViewControllerDelegate @delegate);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IPKAddSecureElementPassViewControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }
	}

	[NoTV]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKShareablePassMetadata {

		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[Export ("initWithProvisioningCredentialIdentifier:cardConfigurationIdentifier:sharingInstanceIdentifier:passThumbnailImage:ownerDisplayName:localizedDescription:")]
		NativeHandle Constructor (string credentialIdentifier, string cardConfigurationIdentifier, string sharingInstanceIdentifier, CGImage passThumbnailImage, string ownerDisplayName, string localizedDescription);

		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("initWithProvisioningCredentialIdentifier:sharingInstanceIdentifier:passThumbnailImage:ownerDisplayName:localizedDescription:accountHash:templateIdentifier:relyingPartyIdentifier:requiresUnifiedAccessCapableDevice:")]
		NativeHandle Constructor (string credentialIdentifier, string sharingInstanceIdentifier, CGImage passThumbnailImage, string ownerDisplayName, string localizedDescription, string accountHash, string templateIdentifier, string relyingPartyIdentifier, bool requiresUnifiedAccessCapableDevice);

		[Internal]
		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0), NoTV]
		[Export ("initWithProvisioningCredentialIdentifier:sharingInstanceIdentifier:cardTemplateIdentifier:preview:")]
		NativeHandle _InitWithCardTemplate (string credentialIdentifier, string sharingInstanceIdentifier, string templateIdentifier, PKShareablePassMetadataPreview preview);

		[Internal]
		[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0), NoTV]
		[Export ("initWithProvisioningCredentialIdentifier:sharingInstanceIdentifier:cardConfigurationIdentifier:preview:")]
		NativeHandle _InitWithCardConfiguration (string credentialIdentifier, string sharingInstanceIdentifier, string templateIdentifier, PKShareablePassMetadataPreview preview);

		[Export ("credentialIdentifier", ArgumentSemantic.Strong)]
		string CredentialIdentifier { get; }

		[Export ("sharingInstanceIdentifier", ArgumentSemantic.Strong)]
		string SharingInstanceIdentifier { get; }

		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[Export ("passThumbnailImage")]
		CGImage PassThumbnailImage { get; }

		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[Export ("localizedDescription", ArgumentSemantic.Strong)]
		string LocalizedDescription { get; }

		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[Export ("ownerDisplayName", ArgumentSemantic.Strong)]
		string OwnerDisplayName { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("accountHash", ArgumentSemantic.Strong)]
		string AccountHash { get; [iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)] set; }

		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("templateIdentifier", ArgumentSemantic.Strong)]
		string TemplateIdentifier { get; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("relyingPartyIdentifier", ArgumentSemantic.Strong)]
		string RelyingPartyIdentifier { get; [iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)] set; }

		[iOS (15, 0), MacCatalyst (15, 0)]
		[Export ("requiresUnifiedAccessCapableDevice")]
		bool RequiresUnifiedAccessCapableDevice { get; [iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)] set; }

		[NoTV, iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		[Export ("cardTemplateIdentifier", ArgumentSemantic.Strong)]
		string CardTemplateIdentifier { get; }

		[Export ("cardConfigurationIdentifier", ArgumentSemantic.Strong)]
		string CardConfigurationIdentifier { get; }

		[NoTV, iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		[Export ("preview", ArgumentSemantic.Strong)]
		PKShareablePassMetadataPreview Preview { get; }

		[NoTV, iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		[Export ("serverEnvironmentIdentifier", ArgumentSemantic.Strong)]
		string ServerEnvironmentIdentifier { get; set; }
	}

	[NoTV]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (PKAddSecureElementPassConfiguration))]
	[DisableDefaultCtor]
	interface PKAddShareablePassConfiguration {

		[Async]
		[Static]
		[Export ("configurationForPassMetadata:provisioningPolicyIdentifier:primaryAction:completion:")]
		void GetConfiguration (PKShareablePassMetadata [] passMetadata, string provisioningPolicyIdentifier, PKAddShareablePassConfigurationPrimaryAction action, Action<PKAddShareablePassConfiguration, NSError> completion);

		[Export ("primaryAction")]
		PKAddShareablePassConfigurationPrimaryAction PrimaryAction { get; }

		[Export ("credentialsMetadata", ArgumentSemantic.Strong)]
		PKShareablePassMetadata [] CredentialsMetadata { get; }

		[Deprecated (PlatformName.iOS, 16, 0)]
		[Deprecated (PlatformName.MacCatalyst, 16, 0)]
		[Export ("provisioningPolicyIdentifier", ArgumentSemantic.Strong)]
		string ProvisioningPolicyIdentifier { get; }

		[NoTV, iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
		[Static, Async]
		[Export ("configurationForPassMetadata:primaryAction:completion:")]
		void GetConfiguration (PKShareablePassMetadata [] passMetadata, PKAddShareablePassConfigurationPrimaryAction action, Action<PKAddShareablePassConfiguration, NSError> completion);
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKBarcodeEventConfigurationRequest {

		[Export ("deviceAccountIdentifier")]
		string DeviceAccountIdentifier { get; }

		[Export ("configurationData")]
		NSData ConfigurationData { get; }

		[Export ("configurationDataType")]
		PKBarcodeEventConfigurationDataType ConfigurationDataType { get; }
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKBarcodeEventMetadataRequest {

		[Export ("deviceAccountIdentifier")]
		string DeviceAccountIdentifier { get; }

		[Export ("lastUsedBarcodeIdentifier")]
		string LastUsedBarcodeIdentifier { get; }
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKBarcodeEventMetadataResponse {

		[Export ("initWithPaymentInformation:")]
		NativeHandle Constructor (NSData paymentInformation);

		[Export ("paymentInformation", ArgumentSemantic.Copy)]
		NSData PaymentInformation { get; set; }
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKBarcodeEventSignatureRequest {

		[Export ("deviceAccountIdentifier")]
		string DeviceAccountIdentifier { get; }

		[Export ("transactionIdentifier")]
		string TransactionIdentifier { get; }

		[Export ("barcodeIdentifier")]
		string BarcodeIdentifier { get; }

		[Export ("rawMerchantName")]
		string RawMerchantName { get; }

		[Export ("merchantName")]
		string MerchantName { get; }

		[Export ("transactionDate", ArgumentSemantic.Strong)]
		NSDate TransactionDate { get; }

		[Export ("currencyCode")]
		string CurrencyCode { get; }

		// NSDecimalNumber is used elsewhere (but it's a subclass for NSNumber and can't be used here)
		[Export ("amount", ArgumentSemantic.Strong)]
		NSNumber Amount { get; }

		[Export ("transactionStatus")]
		string TransactionStatus { get; }

		[Export ("partialSignature", ArgumentSemantic.Copy)]
		NSData PartialSignature { get; }
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKBarcodeEventSignatureResponse {

		[Export ("initWithSignedData:")]
		NativeHandle Constructor (NSData signedData);

		[Export ("signedData", ArgumentSemantic.Copy)]
		NSData SignedData { get; set; }
	}

	[NoTV]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DesignatedDefaultCtor]
	interface PKIssuerProvisioningExtensionStatus {

		[Export ("requiresAuthentication")]
		bool RequiresAuthentication { get; set; }

		[Export ("passEntriesAvailable")]
		bool PassEntriesAvailable { get; set; }

		[Export ("remotePassEntriesAvailable")]
		bool RemotePassEntriesAvailable { get; set; }
	}

	[NoTV]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKIssuerProvisioningExtensionPassEntry {

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("title")]
		string Title { get; }

		[Export ("art")]
		CGImage Art { get; }
	}

	[NoTV, NoMac]
	[iOS (14, 0)]
	[NoMacCatalyst] // type cannot be loaded, lack of documentation about usage
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKIssuerProvisioningExtensionHandler {

		[Async]
		[Export ("statusWithCompletion:")]
		void GetStatus (Action<PKIssuerProvisioningExtensionStatus> completion);

		[Async]
		[Export ("passEntriesWithCompletion:")]
		void PassEntries (Action<PKIssuerProvisioningExtensionPassEntry []> completion);

		[Async]
		[Export ("remotePassEntriesWithCompletion:")]
		void RemotePassEntries (Action<PKIssuerProvisioningExtensionPassEntry []> completion);

		[Async]
		[Export ("generateAddPaymentPassRequestForPassEntryWithIdentifier:configuration:certificateChain:nonce:nonceSignature:completionHandler:")]
		void GenerateAddPaymentPassRequest (string identifier, PKAddPaymentPassRequestConfiguration configuration, NSData [] certificates, NSData nonce, NSData nonceSignature, Action<PKAddPaymentPassRequest> completion);
	}

	[NoTV, NoMac]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface PKIssuerProvisioningExtensionAuthorizationProviding {

		[Abstract]
		[NullAllowed, Export ("completionHandler", ArgumentSemantic.Copy)]
		Action<PKIssuerProvisioningExtensionAuthorizationResult> CompletionHandler { get; set; }
	}

	[NoTV, NoMac]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	delegate void PKInformationRequestCompletionBlock (PKBarcodeEventMetadataResponse response);

	[NoTV, NoMac]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	delegate void PKSignatureRequestCompletionBlock (PKBarcodeEventSignatureResponse response);

	[NoTV]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[Protocol]
	interface PKPaymentInformationRequestHandling {

		[Abstract]
		[Export ("handleInformationRequest:completion:")]
		void HandleInformationRequest (PKBarcodeEventMetadataRequest infoRequest, PKInformationRequestCompletionBlock completion);

		[Abstract]
		[Export ("handleSignatureRequest:completion:")]
		void HandleSignatureRequest (PKBarcodeEventSignatureRequest signatureRequest, PKSignatureRequestCompletionBlock completion);

		[Abstract]
		[Export ("handleConfigurationRequest:completion:")]
		void HandleConfigurationRequest (PKBarcodeEventConfigurationRequest configurationRequest, Action completion);
	}

	[NoTV]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (PKIssuerProvisioningExtensionPassEntry))]
	[DisableDefaultCtor]
	interface PKIssuerProvisioningExtensionPaymentPassEntry {

		[Export ("initWithIdentifier:title:art:addRequestConfiguration:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string identifier, string title, CGImage art, PKAddPaymentPassRequestConfiguration configuration);

		[Export ("addRequestConfiguration")]
		PKAddPaymentPassRequestConfiguration AddRequestConfiguration { get; }
	}

	[NoTV]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKPaymentMerchantSession {

		[Export ("initWithDictionary:")]
		NativeHandle Constructor (NSDictionary dictionary);
	}

	[NoTV]
	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	interface PKPaymentRequestMerchantSessionUpdate {

		[Export ("initWithStatus:merchantSession:")]
		NativeHandle Constructor (PKPaymentAuthorizationStatus status, [NullAllowed] PKPaymentMerchantSession session);

		[Export ("status", ArgumentSemantic.Assign)]
		PKPaymentAuthorizationStatus Status { get; set; }

		[NullAllowed, Export ("session", ArgumentSemantic.Strong)]
		PKPaymentMerchantSession Session { get; set; }
	}

	[iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (PKPaymentRequestUpdate))]
	[DisableDefaultCtor]
	interface PKPaymentRequestCouponCodeUpdate {
		[Export ("initWithPaymentSummaryItems:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKPaymentSummaryItem [] paymentSummaryItems);

		[Export ("initWithErrors:paymentSummaryItems:shippingMethods:")]
		[DesignatedInitializer]
		NativeHandle Constructor ([NullAllowed] NSError [] errors, PKPaymentSummaryItem [] paymentSummaryItems, PKShippingMethod [] shippingMethods);

		[NullAllowed, Export ("errors", ArgumentSemantic.Copy)]
		NSError [] Errors { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[Export ("multiTokenContexts", ArgumentSemantic.Copy)]
		PKPaymentTokenContext [] MultiTokenContexts { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[Export ("recurringPaymentRequest", ArgumentSemantic.Strong)]
		PKRecurringPaymentRequest RecurringPaymentRequest { get; set; }

		[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
		[Export ("automaticReloadPaymentRequest", ArgumentSemantic.Strong)]
		PKAutomaticReloadPaymentRequest AutomaticReloadPaymentRequest { get; set; }
	}

	[iOS (14, 0)]
	[MacCatalyst (14, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKPaymentInformationEventExtension {
	}

	[iOS (14, 5)]
	[MacCatalyst (14, 5)]
	[Flags]
	[Native]
	enum PKRadioTechnology : ulong {
		None = 0,
		Nfc = 1 << 0,
		Bluetooth = 1 << 1,
	}

	[iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKDateComponentsRange : NSCopying, NSSecureCoding {
		[Export ("initWithStartDateComponents:endDateComponents:")]
		[return: NullAllowed]
		NativeHandle Constructor (NSDateComponents startDateComponents, NSDateComponents endDateComponents);

		[Export ("startDateComponents", ArgumentSemantic.Copy)]
		NSDateComponents StartDateComponents { get; }

		[Export ("endDateComponents", ArgumentSemantic.Copy)]
		NSDateComponents EndDateComponents { get; }
	}

	[iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (PKPaymentSummaryItem))]
	[DisableDefaultCtor]
	interface PKDeferredPaymentSummaryItem {
		[Export ("deferredDate", ArgumentSemantic.Copy)]
		NSDate DeferredDate { get; set; }
	}

	[iOS (15, 0), MacCatalyst (15, 0)]
	[Native]
	public enum PKShippingContactEditingMode : ulong {
		Enabled = 1,
		StorePickup,
	}

	[iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (PKPaymentSummaryItem))]
	[DisableDefaultCtor]
	interface PKRecurringPaymentSummaryItem {
		[NullAllowed, Export ("startDate", ArgumentSemantic.Copy)]
		NSDate StartDate { get; set; }

		[Export ("intervalUnit", ArgumentSemantic.Assign)]
		NSCalendarUnit IntervalUnit { get; set; }

		[Export ("intervalCount")]
		nint IntervalCount { get; set; }

		[NullAllowed, Export ("endDate", ArgumentSemantic.Copy)]
		NSDate EndDate { get; set; }
	}

	[iOS (15, 0), MacCatalyst (15, 0)]
	public enum PKStoredValuePassBalanceType {
		[Field ("PKStoredValuePassBalanceTypeCash")]
		Cash,
		[Field ("PKStoredValuePassBalanceTypeLoyaltyPoints")]
		LoyaltyPoints,
	}

	[iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKStoredValuePassBalance {
		[Export ("amount", ArgumentSemantic.Strong)]
		NSDecimalNumber Amount { get; }

		[NullAllowed, Export ("currencyCode")]
		string CurrencyCode { get; }

		[Export ("balanceType")]
		string BalanceType { get; }

		[NullAllowed, Export ("expiryDate", ArgumentSemantic.Strong)]
		NSDate ExpiryDate { get; }

		[Export ("isEqualToBalance:")]
		bool IsEqual (PKStoredValuePassBalance balance);
	}

	[iOS (15, 0), MacCatalyst (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKStoredValuePassProperties {
		[Static]
		[Export ("passPropertiesForPass:")]
		[return: NullAllowed]
		PKStoredValuePassProperties GetPassProperties (PKPass pass);

		[Export ("blocked")]
		bool Blocked { [Bind ("isBlocked")] get; }

		[NullAllowed, Export ("expirationDate", ArgumentSemantic.Copy)]
		NSDate ExpirationDate { get; }

		[Export ("balances", ArgumentSemantic.Copy)]
		PKStoredValuePassBalance [] Balances { get; }
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	interface IPKIdentityDocumentDescriptor { }

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[Protocol]
	interface PKIdentityDocumentDescriptor {
		[Abstract]
		[Export ("elements")]
		PKIdentityElement [] Elements { get; }

		[Abstract]
		[Export ("intentToStoreForElement:")]
		[return: NullAllowed]
		PKIdentityIntentToStore GetIntentToStore (PKIdentityElement element);

		[Abstract]
		[Export ("addElements:withIntentToStore:")]
		void AddElements (PKIdentityElement [] elements, PKIdentityIntentToStore intentToStore);
	}

	interface IPKShareSecureElementPassViewControllerDelegate { };

	[iOS (16, 0), MacCatalyst (16, 0), NoTV, NoMac]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface PKShareSecureElementPassViewControllerDelegate {
		[Abstract]
		[Export ("shareSecureElementPassViewController:didFinishWithResult:")]
		void DidFinish (PKShareSecureElementPassViewController controller, PKShareSecureElementPassResult result);

		[Export ("shareSecureElementPassViewController:didCreateShareURL:activationCode:")]
		void DidCreateShareUrl (PKShareSecureElementPassViewController controller, [NullAllowed] NSUrl universalShareUrl, [NullAllowed] string activationCode);
	}

	interface IPKVehicleConnectionDelegate { }

	[iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface PKVehicleConnectionDelegate {
		[Abstract]
		[Export ("sessionDidChangeConnectionState:")]
		void SessionDidChangeConnectionState (PKVehicleConnectionSessionConnectionState newState);

		[Abstract]
		[Export ("sessionDidReceiveData:")]
		void SessionDidReceiveData (NSData data);
	}

	[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKAutomaticReloadPaymentRequest // : NSCoding, NSCopying, NSSecureCoding // https://feedbackassistant.apple.com/feedback/11018799
	{
		[Export ("paymentDescription")]
		string PaymentDescription { get; set; }

		[Export ("automaticReloadBilling", ArgumentSemantic.Strong)]
		PKAutomaticReloadPaymentSummaryItem AutomaticReloadBilling { get; set; }

		[NullAllowed, Export ("billingAgreement")]
		string BillingAgreement { get; set; }

		[Export ("managementURL", ArgumentSemantic.Strong)]
		NSUrl ManagementUrl { get; set; }

		[NullAllowed, Export ("tokenNotificationURL", ArgumentSemantic.Strong)]
		NSUrl TokenNotificationUrl { get; set; }

		[Export ("initWithPaymentDescription:automaticReloadBilling:managementURL:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string paymentDescription, PKAutomaticReloadPaymentSummaryItem automaticReloadBilling, NSUrl managementUrl);
	}

	[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (PKPaymentSummaryItem))]
	interface PKAutomaticReloadPaymentSummaryItem // : NSCoding, NSCopying, NSSecureCoding // https://feedbackassistant.apple.com/feedback/11018799
	{
		[Export ("thresholdAmount", ArgumentSemantic.Strong)]
		NSDecimalNumber ThresholdAmount { get; set; }
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface PKIdentityAuthorizationController {
		[Async]
		[Export ("checkCanRequestDocument:completion:")]
		void CheckCanRequestDocument (IPKIdentityDocumentDescriptor descriptor, Action<bool> completion);

		[Async]
		[Export ("requestDocument:completion:")]
		void RequestDocument (PKIdentityRequest request, Action<PKIdentityDocument, NSError> completion);

		[Export ("cancelRequest")]
		void CancelRequest ();
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (UIControl))]
	interface PKIdentityButton {
		[Export ("initWithLabel:style:")]
		[DesignatedInitializer]
		NativeHandle Constructor (PKIdentityButtonLabel label, PKIdentityButtonStyle style);

		[Static]
		[Export ("buttonWithLabel:style:")]
		PKIdentityButton Create (PKIdentityButtonLabel label, PKIdentityButtonStyle style);

		[Export ("cornerRadius")]
		nfloat CornerRadius { get; set; }
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKIdentityDocument {
		[Export ("encryptedData")]
		NSData EncryptedData { get; }
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKIdentityDriversLicenseDescriptor : PKIdentityDocumentDescriptor {
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKIdentityElement : NSCopying {
		[Static]
		[Export ("givenNameElement")]
		PKIdentityElement GivenNameElement { get; }

		[Static]
		[Export ("familyNameElement")]
		PKIdentityElement FamilyNameElement { get; }

		[Static]
		[Export ("portraitElement")]
		PKIdentityElement PortraitElement { get; }

		[Static]
		[Export ("addressElement")]
		PKIdentityElement AddressElement { get; }

		[Static]
		[Export ("issuingAuthorityElement")]
		PKIdentityElement IssuingAuthorityElement { get; }

		[Static]
		[Export ("documentIssueDateElement")]
		PKIdentityElement DocumentIssueDateElement { get; }

		[Static]
		[Export ("documentExpirationDateElement")]
		PKIdentityElement DocumentExpirationDateElement { get; }

		[iOS (17, 2), MacCatalyst (17, 2)]
		[Static]
		[Export ("documentDHSComplianceStatusElement")]
		PKIdentityElement DocumentDhsComplianceStatusElement { get; }

		[Static]
		[Export ("documentNumberElement")]
		PKIdentityElement DocumentNumberElement { get; }

		[Static]
		[Export ("drivingPrivilegesElement")]
		PKIdentityElement DrivingPrivilegesElement { get; }

		[Static]
		[Export ("ageElement")]
		PKIdentityElement AgeElement { get; }

		[Static]
		[Export ("dateOfBirthElement")]
		PKIdentityElement DateOfBirthElement { get; }

		[iOS (17, 2), MacCatalyst (17, 2)]
		[Static]
		[Export ("sexElement")]
		PKIdentityElement SexElement { get; }

		[Static]
		[Export ("ageThresholdElementWithAge:")]
		PKIdentityElement AgeThresholdElementWithAge (nint age);
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKIdentityIntentToStore : NSCopying {
		[Static]
		[Export ("willNotStoreIntent")]
		PKIdentityIntentToStore WillNotStoreIntent { get; }

		[Static]
		[Export ("mayStoreIntent")]
		PKIdentityIntentToStore MayStoreIntent { get; }

		[Static]
		[Export ("mayStoreIntentForDays:")]
		PKIdentityIntentToStore MayStoreIntentForDays (nint days);
	}

	[NoTV, NoMac, iOS (16, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	interface PKIdentityRequest {
		[NullAllowed, Export ("descriptor", ArgumentSemantic.Strong)]
		IPKIdentityDocumentDescriptor Descriptor { get; set; }

		[NullAllowed, Export ("nonce", ArgumentSemantic.Copy)]
		NSData Nonce { get; set; }

		[NullAllowed, Export ("merchantIdentifier")]
		string MerchantIdentifier { get; set; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKPaymentOrderDetails // : NSCopying, NSSecureCoding // https://feedbackassistant.apple.com/feedback/11018799
	{
		[Export ("initWithOrderTypeIdentifier:orderIdentifier:webServiceURL:authenticationToken:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string orderTypeIdentifier, string orderIdentifier, NSUrl webServiceUrl, string authenticationToken);

		[Export ("orderTypeIdentifier")]
		string OrderTypeIdentifier { get; set; }

		[Export ("orderIdentifier")]
		string OrderIdentifier { get; set; }

		[Export ("webServiceURL", ArgumentSemantic.Copy)]
		NSUrl WebServiceUrl { get; set; }

		[Export ("authenticationToken")]
		string AuthenticationToken { get; set; }
	}

	[iOS (16, 0), MacCatalyst (16, 0), Mac (13, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKPaymentTokenContext // : NSCoding, NSCopying, NSSecureCoding // https://feedbackassistant.apple.com/feedback/11018799
	{
		[Export ("initWithMerchantIdentifier:externalIdentifier:merchantName:merchantDomain:amount:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string merchantIdentifier, string externalIdentifier, string merchantName, [NullAllowed] string merchantDomain, NSDecimalNumber amount);

		[Export ("merchantIdentifier")]
		string MerchantIdentifier { get; set; }

		[Export ("externalIdentifier")]
		string ExternalIdentifier { get; set; }

		[Export ("merchantName")]
		string MerchantName { get; set; }

		[NullAllowed, Export ("merchantDomain")]
		string MerchantDomain { get; set; }

		[Export ("amount", ArgumentSemantic.Copy)]
		NSDecimalNumber Amount { get; set; }
	}

	[iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKRecurringPaymentRequest // : NSCoding, NSCopying, NSSecureCoding // https://feedbackassistant.apple.com/feedback/11018799
	{
		[Export ("initWithPaymentDescription:regularBilling:managementURL:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string paymentDescription, PKRecurringPaymentSummaryItem regularBilling, NSUrl managementUrl);

		[Export ("paymentDescription")]
		string PaymentDescription { get; set; }

		[Export ("regularBilling", ArgumentSemantic.Strong)]
		PKRecurringPaymentSummaryItem RegularBilling { get; set; }

		[NullAllowed, Export ("trialBilling", ArgumentSemantic.Strong)]
		PKRecurringPaymentSummaryItem TrialBilling { get; set; }

		[NullAllowed, Export ("billingAgreement")]
		string BillingAgreement { get; set; }

		[Export ("managementURL", ArgumentSemantic.Strong)]
		NSUrl ManagementUrl { get; set; }

		[NullAllowed, Export ("tokenNotificationURL", ArgumentSemantic.Strong)]
		NSUrl TokenNotificationUrl { get; set; }
	}

	[NoTV, iOS (16, 0), Mac (13, 0), MacCatalyst (16, 0)]
	[BaseType (typeof (PKAddPassMetadataPreview))]
	[DisableDefaultCtor]
	interface PKShareablePassMetadataPreview // : NSCoding, NSCopying, NSSecureCoding // https://feedbackassistant.apple.com/feedback/11018799
	{
		[Export ("initWithPassThumbnail:localizedDescription:")]
		NativeHandle Constructor (CGImage passThumbnail, string description);

		[Export ("initWithTemplateIdentifier:")]
		NativeHandle Constructor (string templateIdentifier);

		[Static]
		[Export ("previewWithPassThumbnail:localizedDescription:")]
		PKShareablePassMetadataPreview PreviewWithPassThumbnail (CGImage passThumbnail, string description);

		[Static]
		[Export ("previewWithTemplateIdentifier:")]
		PKShareablePassMetadataPreview PreviewWithTemplateIdentifier (string templateIdentifier);

		[NullAllowed, Export ("passThumbnailImage", ArgumentSemantic.Assign)]
		CGImage PassThumbnailImage { get; }

		[NullAllowed, Export ("localizedDescription", ArgumentSemantic.Strong)]
		string LocalizedDescription { get; }

		[NullAllowed, Export ("ownerDisplayName", ArgumentSemantic.Strong)]
		string OwnerDisplayName { get; set; }

		[NullAllowed, Export ("provisioningTemplateIdentifier", ArgumentSemantic.Strong)]
		string ProvisioningTemplateIdentifier { get; }
	}

	[iOS (16, 0), MacCatalyst (16, 0), NoTV, NoMac]
	[BaseType (typeof (UIViewController))]
	[DisableDefaultCtor]
	interface PKShareSecureElementPassViewController {
		// from UIViewController
		[DesignatedInitializer]
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		[Export ("initWithSecureElementPass:delegate:")]
		NativeHandle Constructor (PKSecureElementPass pass, [NullAllowed] IPKShareSecureElementPassViewControllerDelegate @delegate);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IPKShareSecureElementPassViewControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("promptToShareURL")]
		bool PromptToShareUrl { get; set; }
	}

	[iOS (16, 0), Mac (13, 0), NoTV, MacCatalyst (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKVehicleConnectionSession {
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IPKVehicleConnectionDelegate Delegate { get; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; }

		[Export ("connectionStatus", ArgumentSemantic.Assign)]
		PKVehicleConnectionSessionConnectionState ConnectionStatus { get; }

		[Async]
		[Static]
		[Export ("sessionForPass:delegate:completion:")]
		void GetSession (PKSecureElementPass pass, IPKVehicleConnectionDelegate @delegate, Action<PKVehicleConnectionSession, NSError> completion);

		[Export ("sendData:error:")]
		bool SendData (NSData message, [NullAllowed] out NSError error);

		[Export ("invalidate")]
		void Invalidate ();
	}

	[NoTV, Mac (13, 3), iOS (16, 4), MacCatalyst (16, 4)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKDeferredPaymentRequest {

		[Export ("paymentDescription")]
		string PaymentDescription { get; set; }

		[Export ("deferredBilling", ArgumentSemantic.Strong)]
		PKDeferredPaymentSummaryItem DeferredBilling { get; set; }

		[NullAllowed, Export ("billingAgreement")]
		string BillingAgreement { get; set; }

		[Export ("managementURL", ArgumentSemantic.Strong)]
		NSUrl ManagementUrl { get; set; }

		[NullAllowed, Export ("tokenNotificationURL", ArgumentSemantic.Strong)]
		NSUrl TokenNotificationUrl { get; set; }

		[NullAllowed, Export ("freeCancellationDate", ArgumentSemantic.Strong)]
		NSDate FreeCancellationDate { get; set; }

		[NullAllowed, Export ("freeCancellationDateTimeZone", ArgumentSemantic.Strong)]
		NSTimeZone FreeCancellationDateTimeZone { get; set; }

		[Export ("initWithPaymentDescription:deferredBilling:managementURL:")]
		[DesignatedInitializer]
		NativeHandle Constructor (string paymentDescription, PKDeferredPaymentSummaryItem deferredBilling, NSUrl managementUrl);
	}

	[NoTV, NoMac, iOS (17, 0), NoMacCatalyst]
	[BaseType (typeof (UIView))]
	[DisableDefaultCtor]
	interface PKPayLaterView {

		[Export ("initWithAmount:currencyCode:")]
		NativeHandle Constructor (NSDecimalNumber amount, string currencyCode);

		[NullAllowed, Wrap ("WeakDelegate")]
		IPKPayLaterViewDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }

		[Export ("amount", ArgumentSemantic.Copy)]
		NSDecimalNumber Amount { get; set; }

		[Export ("currencyCode")]
		string CurrencyCode { get; set; }

		[Export ("displayStyle", ArgumentSemantic.Assign)]
		PKPayLaterDisplayStyle DisplayStyle { get; set; }

		[Export ("action", ArgumentSemantic.Assign)]
		PKPayLaterAction Action { get; set; }
	}

	interface IPKPayLaterViewDelegate { }

	[NoTV, NoMac, iOS (17, 0), NoMacCatalyst]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface PKPayLaterViewDelegate {
		[Abstract]
		[Export ("payLaterViewDidUpdateHeight:")]
		void PayLaterViewDidUpdateHeight (PKPayLaterView view);
	}

	[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[Static]
	interface PKDirbursementError {
		[Field ("PKDisbursementErrorContactFieldUserInfoKey")]
		NSString ContactFieldUserInfoKey { get; }
	}

	[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (PKPaymentSummaryItem))]
	[DisableDefaultCtor]
	interface PKInstantFundsOutFeeSummaryItem : NSCoding, NSCopying, NSSecureCoding {
	}

	[NoTV, Mac (15, 0), iOS (17, 0), MacCatalyst (17, 0)]
	[BaseType (typeof (PKPaymentSummaryItem))]
	[DisableDefaultCtor]
	interface PKDisbursementSummaryItem : NSCoding, NSCopying, NSSecureCoding {
	}

	[NoTV, iOS (18, 0), MacCatalyst (18, 0), Mac (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKIdentityDocumentMetadata {
		[Export ("credentialIdentifier", ArgumentSemantic.Strong)]
		string CredentialIdentifier { get; }

		[Export ("sharingInstanceIdentifier", ArgumentSemantic.Strong)]
		string SharingInstanceIdentifier { get; }

		[Export ("cardTemplateIdentifier", ArgumentSemantic.Strong)]
		string CardTemplateIdentifier { get; }

		[Export ("cardConfigurationIdentifier", ArgumentSemantic.Strong)]
		string CardConfigurationIdentifier { get; }

		[Export ("serverEnvironmentIdentifier", ArgumentSemantic.Strong)]
		string ServerEnvironmentIdentifier { get; set; }
	}

	[NoTV, iOS (18, 0), MacCatalyst (18, 0), Mac (15, 0)]
	[BaseType (typeof (PKIdentityDocumentMetadata))]
	[DisableDefaultCtor]
	interface PKJapanIndividualNumberCardMetadata {
		[Export ("initWithProvisioningCredentialIdentifier:sharingInstanceIdentifier:cardTemplateIdentifier:preview:")]
		[Internal]
		NativeHandle _InitWithProvisioningCredentialIdentifier_CardTemplateIdentifier (string credentialIdentifier, string sharingInstanceIdentifier, string cardTemplateIdentifier, PKAddPassMetadataPreview preview);

		[Export ("initWithProvisioningCredentialIdentifier:sharingInstanceIdentifier:cardConfigurationIdentifier:preview:")]
		[Internal]
		NativeHandle _InitWithProvisioningCredentialIdentifier_CardConfigurationIdentifier (string credentialIdentifier, string sharingInstanceIdentifier, string cardConfigurationIdentifier, PKAddPassMetadataPreview preview);

		[Export ("authenticationPassword", ArgumentSemantic.Copy), NullAllowed]
		string AuthenticationPassword { get; set; }

		[Export ("signingPassword", ArgumentSemantic.Copy), NullAllowed]
		string SigningPassword { get; set; }

		[Export ("preview", ArgumentSemantic.Strong)]
		PKAddPassMetadataPreview Preview { get; set; }
	}

	delegate void PKAddIdentityDocumentConfigurationGetConfigurationCompletionHandler ([NullAllowed] PKAddIdentityDocumentConfiguration credentialConfiguration, [NullAllowed] NSError error);

	[NoTV, iOS (18, 0), MacCatalyst (18, 0), Mac (15, 0)]
	[BaseType (typeof (PKAddSecureElementPassConfiguration))]
	[DisableDefaultCtor]
	interface PKAddIdentityDocumentConfiguration {
		[Async]
		[Static]
		[Export ("configurationForMetadata:completion:")]
		void GetConfiguration (PKIdentityDocumentMetadata metadata, PKAddIdentityDocumentConfigurationGetConfigurationCompletionHandler completionHandler);

		[Export ("metadata", ArgumentSemantic.Strong)]
		PKIdentityDocumentMetadata Metadata { get; }
	}

	[NoTV, iOS (18, 0), MacCatalyst (18, 0), Mac (15, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKAddPassMetadataPreview {
		[Export ("initWithPassThumbnail:localizedDescription:")]
		NativeHandle Constructor (CGImage passThumbnail, string description);

		[Static]
		[Export ("previewWithPassThumbnail:localizedDescription:")]
		PKAddPassMetadataPreview PreviewWithPassThumbnail (CGImage passThumbnail, string localizedDescription);

		[Export ("passThumbnailImage", ArgumentSemantic.Assign), NullAllowed]
		CGImage PassThumbnailImage { get; }

		[Export ("localizedDescription", ArgumentSemantic.Strong), NullAllowed]
		string LocalizedDescription { get; }
	}

	[NoTV, iOS (18, 0), MacCatalyst (18, 0), NoMac]
	[BaseType (typeof (NSObject), Name = "PKIdentityNationalIDCardDescriptor")]
	[DisableDefaultCtor]
	interface PKIdentityNationalIdCardDescriptor : PKIdentityDocumentDescriptor {
		[Export ("regionCode", ArgumentSemantic.Copy), NullAllowed]
		string RegionCode { get; set; }
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface PKPassRelevantDate {
		[Export ("interval", ArgumentSemantic.Copy), NullAllowed]
		NSDateInterval Interval { get; }

		[Export ("date", ArgumentSemantic.Copy), NullAllowed]
		NSDate Date { get; }
	}
}
