using System;
using ObjCRuntime;
using Foundation;
using Security;

namespace LocalAuthentication {

	/// <summary>Enumerates supported biometric authentication types.</summary>
	[NoTV]
	[MacCatalyst (13, 1)]
	[Native]
	public enum LABiometryType : long {
		/// <summary>Indicates that biometric authentication is not supported.</summary>
		None,
		/// <summary>Indicates that Touch ID is supported.</summary>
		TouchId,
		/// <summary>Indicates that Face ID is supported.</summary>
		[MacCatalyst (13, 1)]
		FaceId,
		[iOS (17, 0), Mac (14, 0), MacCatalyst (17, 0)]
		OpticId = 1L << 2,
	}

	/// <summary>Signature for a function to be invoked in response to a <see cref="LocalAuthentication.LAContext.EvaluatePolicy(LocalAuthentication.LAPolicy,System.String,LocalAuthentication.LAContextReplyHandler)" /> invocation.</summary>
	///     <remarks>The method when invoked returns a boolean indicating if the policy evaluation was successful, and on failure a detailed description of the error in the error parameter.</remarks>
	[MacCatalyst (13, 1)]
	delegate void LAContextReplyHandler (bool success, [NullAllowed] NSError error);

	/// <summary>The context in which authentication policies are evaluated.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/LocalAuthentication/Reference/LAContext_Class/index.html">Apple documentation for <c>LAContext</c></related>
	[NoTV] // ".objc_class_name_LAContext", referenced from: '' not found
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface LAContext {
		/// <summary>Gets or sets the localized title of the fallback dialog.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed] // by default this property is null
		[Export ("localizedFallbackTitle")]
		string LocalizedFallbackTitle { get; set; }

		/// <param name="policy">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>Preflights <paramref name="policy" />, and reports any errors in the <paramref name="error" /><see langword="out" /> parameter.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("canEvaluatePolicy:error:")]
		bool CanEvaluatePolicy (LAPolicy policy, out NSError error);

		/// <param name="policy">To be added.</param>
		///         <param name="localizedReason">To be added.</param>
		///         <param name="reply">To be added.</param>
		///         <summary>Evaluates the specified access control <paramref name="policy" />.</summary>
		///         <remarks>To be added.</remarks>
		[Async (XmlDocs = """
			<param name="policy">To be added.</param>
			<param name="localizedReason">To be added.</param>
			<summary>Evaluates the specified access control <paramref name="policy" />.</summary>
			<returns>
			          <para>A task that represents the asynchronous EvaluatePolicy operation.   The value of the TResult parameter is a LocalAuthentication.LAContextReplyHandler.</para>
			        </returns>
			<remarks>
			          <para copied="true">The EvaluatePolicyAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("evaluatePolicy:localizedReason:reply:")]
		void EvaluatePolicy (LAPolicy policy, string localizedReason, LAContextReplyHandler reply);

		/// <summary>Stops all pending policy evaluations and renders the context unusable for further policy evaluation.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("invalidate")]
		void Invalidate ();

		/// <param name="credential">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="type">To be added.</param>
		///         <summary>Attempts to set the specified credential <paramref name="type" /> to the specified <paramref name="credential" />, and returns <see langword="true" /> if it succeeds. </summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("setCredential:type:")]
		bool SetCredentialType ([NullAllowed] NSData credential, LACredentialType type);


		/// <param name="type">To be added.</param>
		///         <summary>Returns <see langword="true" /> if the specified credential <paramref name="type" /> is set.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("isCredentialSet:")]
		bool IsCredentialSet (LACredentialType type);


		/// <param name="accessControl">To be added.</param>
		///         <param name="operation">To be added.</param>
		///         <param name="localizedReason">To be added.</param>
		///         <param name="reply">To be added.</param>
		///         <summary>Evaluates <paramref name="accessControl" /> for the specified access control <paramref name="operation" />.</summary>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("evaluateAccessControl:operation:localizedReason:reply:")]
		void EvaluateAccessControl (SecAccessControl accessControl, LAAccessControlOperation operation, string localizedReason, Action<bool, NSError> reply);

		/// <summary>Gets the state of the policy domain after a biometric authentication success or after the policy has been evaluated.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'LADomainStateBiometry.StateHash' instead.")]
		[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'LADomainStateBiometry.StateHash' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'LADomainStateBiometry.StateHash' instead.")]
		[MacCatalyst (13, 1)]
		[Export ("evaluatedPolicyDomainState")]
		[NullAllowed]
		NSData EvaluatedPolicyDomainState { get; }

		/// <summary>Gets or sets the localized title of the Cancel button in the fallback dialog.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[NullAllowed, Export ("localizedCancelTitle")]
		string LocalizedCancelTitle { get; set; }

		/// <summary>Represents the value that is associated with the LATouchIDAuthenticationMaximumAllowableReuseDuration constant.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("LATouchIDAuthenticationMaximumAllowableReuseDuration")]
		double /* NSTimeInterval */ TouchIdAuthenticationMaximumAllowableReuseDuration { get; }

		/// <summary>Gets or sets the time, in seconds, after a successful Touch ID authentication for which a user will not be challenged for another.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("touchIDAuthenticationAllowableReuseDuration")]
		double /* NSTimeInterval */ TouchIdAuthenticationAllowableReuseDuration { get; set; }

		/// <summary>Developers should not use this deprecated property. </summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 9, 0)]
		[Deprecated (PlatformName.MacOSX, 10, 11)]
		[MacCatalyst (13, 1)]
		[Deprecated (PlatformName.MacCatalyst, 13, 1)]
		[NullAllowed]
		[Export ("maxBiometryFailures")]
		NSNumber MaxBiometryFailures { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("localizedReason")]
		string LocalizedReason { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoTV]
		[MacCatalyst (13, 1)]
		[Export ("interactionNotAllowed")]
		bool InteractionNotAllowed { get; set; }

		/// <summary>Gets a value that tells what kind of biometric authentication is supported by the device.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Export ("biometryType")]
		LABiometryType BiometryType { get; }

		[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
		[Export ("domainState")]
		LADomainState DomainState { get; }
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (LARight))]
	[DisableDefaultCtor]
	interface LAPersistedRight {
		[Export ("key")]
		LAPrivateKey Key { get; }

		[Export ("secret")]
		LASecret Secret { get; }
	}

	delegate void LAPrivateKeyCompletionHandler ([NullAllowed] NSData data, [NullAllowed] NSError error);

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface LAPrivateKey {
		[Export ("publicKey")]
		LAPublicKey PublicKey { get; }

		[Async]
		[Export ("signData:secKeyAlgorithm:completion:")]
		void Sign (NSData data, SecKeyAlgorithm algorithm, LAPrivateKeyCompletionHandler handler);

		[Export ("canSignUsingSecKeyAlgorithm:")]
		bool CanSign (SecKeyAlgorithm algorithm);

		[Async]
		[Export ("decryptData:secKeyAlgorithm:completion:")]
		void Decrypt (NSData data, SecKeyAlgorithm algorithm, LAPrivateKeyCompletionHandler handler);

		[Export ("canDecryptUsingSecKeyAlgorithm:")]
		bool CanDecrypt (SecKeyAlgorithm algorithm);

		[Async]
		[Export ("exchangeKeysWithPublicKey:secKeyAlgorithm:secKeyParameters:completion:")]
		void ExchangeKeys (NSData publicKey, SecKeyAlgorithm algorithm, NSDictionary parameters, LAPrivateKeyCompletionHandler handler);

		[Export ("canExchangeKeysUsingSecKeyAlgorithm:")]
		bool CanExchangeKeys (SecKeyAlgorithm algorithm);
	}

	delegate void LAPublicKeyCompletionHandler ([NullAllowed] NSData data, [NullAllowed] NSError error);
	delegate void LAPublicKeyVerifyDataCompletionHandler ([NullAllowed] NSError error);

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface LAPublicKey {
		[Async]
		[Export ("exportBytesWithCompletion:")]
		void ExportBytes (LAPublicKeyCompletionHandler handler);

		[Async]
		[Export ("encryptData:secKeyAlgorithm:completion:")]
		void Encrypt (NSData data, SecKeyAlgorithm algorithm, LAPublicKeyCompletionHandler handler);

		[Export ("canEncryptUsingSecKeyAlgorithm:")]
		bool CanEncrypt (SecKeyAlgorithm algorithm);

		[Async]
		[Export ("verifyData:signature:secKeyAlgorithm:completion:")]
		void Verify (NSData signedData, NSData signature, SecKeyAlgorithm algorithm, LAPublicKeyVerifyDataCompletionHandler handler);

		[Export ("canVerifyUsingSecKeyAlgorithm:")]
		bool CanVerify (SecKeyAlgorithm algorithm);
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	interface LAAuthenticationRequirement {
		[Static]
		[Export ("defaultRequirement")]
		LAAuthenticationRequirement DefaultRequirement { get; }

		[Static]
		[Export ("biometryRequirement")]
		LAAuthenticationRequirement BiometryRequirement { get; }

		[Static]
		[Export ("biometryCurrentSetRequirement")]
		LAAuthenticationRequirement BiometryCurrentSetRequirement { get; }

		[Static]
		[Export ("biometryRequirementWithFallback:")]
		LAAuthenticationRequirement GetBiometryRequirement (LABiometryFallbackRequirement fallback);
	}

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	interface LABiometryFallbackRequirement {
		[Static]
		[Export ("defaultRequirement")]
		LABiometryFallbackRequirement DefaultRequirement { get; }

		[Static]
		[Export ("devicePasscodeRequirement")]
		LABiometryFallbackRequirement DevicePasscodeRequirement { get; }
	}

	delegate void LARightAuthorizeCompletionHandler ([NullAllowed] NSError error);

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	interface LARight {
		[Export ("state")]
		LARightState State { get; }

		[Export ("tag")]
		nint Tag { get; set; }

		[Export ("initWithRequirement:")]
		NativeHandle Constructor (LAAuthenticationRequirement requirement);

		[Async]
		[Export ("authorizeWithLocalizedReason:completion:")]
		void Authorize (string localizedReason, LARightAuthorizeCompletionHandler handler);

		[Async]
		[Export ("checkCanAuthorizeWithCompletion:")]
		void CheckCanAuthorize (LARightAuthorizeCompletionHandler handler);

		[Async]
		[Export ("deauthorizeWithCompletion:")]
		void Deauthorize (Action handler);
	}

	delegate void LARightStoreCompletionHandler ([NullAllowed] LAPersistedRight right, [NullAllowed] NSError error);
	delegate void LARightStoreRemoveRightCompletionHandler ([NullAllowed] NSError error);

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface LARightStore {
		[Static]
		[Export ("sharedStore")]
		LARightStore SharedStore { get; }

		[Async]
		[Export ("rightForIdentifier:completion:")]
		void Get (string identifier, LARightStoreCompletionHandler handler);

		[Async]
		[Export ("saveRight:identifier:completion:")]
		void Save (LARight right, string identifier, LARightStoreCompletionHandler handler);

		[Async]
		[Export ("saveRight:identifier:secret:completion:")]
		void Save (LARight right, string identifier, NSData secret, LARightStoreCompletionHandler handler);

		[Async]
		[Export ("removeRight:completion:")]
		void Remove (LAPersistedRight right, LARightStoreRemoveRightCompletionHandler handler);

		[Async]
		[Export ("removeRightForIdentifier:completion:")]
		void Remove (string identifier, LARightStoreRemoveRightCompletionHandler handler);

		[Async]
		[Export ("removeAllRightsWithCompletion:")]
		void RemoveAll (LARightStoreRemoveRightCompletionHandler handler);
	}

	delegate void LASecretCompletionHandler ([NullAllowed] NSData data, [NullAllowed] NSError error);

	[Mac (13, 0), iOS (16, 0), MacCatalyst (16, 0), NoTV]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface LASecret {
		[Async]
		[Export ("loadDataWithCompletion:")]
		void LoadData (LASecretCompletionHandler handler);
	}

	[Flags]
	[Native]
	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0), NoTV]
	enum LACompanionType : long {
		None = 0,
		[NoiOS, NoTV, NoMacCatalyst]
		Watch = 1 << 0,
		[NoMac, NoTV]
		Mac = 1 << 1,
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface LADomainStateBiometry {
		[Export ("biometryType")]
		LABiometryType BiometryType { get; }

		[Export ("stateHash"), NullAllowed]
		NSData StateHash { get; }
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface LADomainStateCompanion {
		[Export ("availableCompanionTypes")]
		NSSet<NSNumber> WeakAvailableCompanionTypes { get; }

		[Export ("stateHash"), NullAllowed]
		NSData StateHash { get; }

		[Export ("stateHashForCompanionType:")]
		[return: NullAllowed]
		NSData GetStateHash (LACompanionType companionType);
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface LADomainState {
		[Export ("biometry")]
		LADomainStateBiometry Biometry { get; }

		[Export ("companion")]
		LADomainStateCompanion Companion { get; }

		[Export ("stateHash"), NullAllowed]
		NSData StateHash { get; }
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface LAEnvironment {
		[Export ("addObserver:")]
		void AddObserver (ILAEnvironmentObserver observer);

		[Export ("removeObserver:")]
		void RemoveObserver (ILAEnvironmentObserver observer);

		[Static]
		[Export ("currentUser")]
		LAEnvironment CurrentUser { get; }

		[Export ("state")]
		LAEnvironmentState State { get; }
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false), Model]
	[BaseType (typeof (NSObject))]
	interface LAEnvironmentObserver {
		[Export ("environment:stateDidChangeFromOldState:")]
		void StateDidChangeFromOldState (LAEnvironment environment, LAEnvironmentState oldState);
	}

	interface ILAEnvironmentObserver { }

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface LAEnvironmentMechanism {
		[Export ("isUsable")]
		bool IsUsable { get; }

		[Export ("localizedName")]
		string LocalizedName { get; }

		[Export ("iconSystemName")]
		string IconSystemName { get; }
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (LAEnvironmentMechanism))]
	[DisableDefaultCtor]
	interface LAEnvironmentMechanismBiometry {
		[Export ("biometryType")]
		LABiometryType BiometryType { get; }

		[Export ("isEnrolled")]
		bool IsEnrolled { get; }

		[Export ("isLockedOut")]
		bool IsLockedOut { get; }

		[Export ("stateHash")]
		NSData StateHash { get; }

		[Export ("builtInSensorInaccessible")]
		bool BuiltInSensorInaccessible { get; }
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (LAEnvironmentMechanism))]
	[DisableDefaultCtor]
	interface LAEnvironmentMechanismCompanion {
		[Export ("type")]
		LACompanionType Type { get; }

		[Export ("stateHash"), NullAllowed]
		NSData StateHash { get; }
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (LAEnvironmentMechanism))]
	[DisableDefaultCtor]
	interface LAEnvironmentMechanismUserPassword {
		[Export ("isSet")]
		bool IsSet { get; }
	}

	[Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface LAEnvironmentState : NSCopying {
		[Export ("biometry"), NullAllowed]
		LAEnvironmentMechanismBiometry Biometry { get; }

		[Export ("userPassword"), NullAllowed]
		LAEnvironmentMechanismUserPassword UserPassword { get; }

		[Export ("companions")]
		LAEnvironmentMechanismCompanion [] Companions { get; }

		[Export ("allMechanisms")]
		LAEnvironmentMechanism [] AllMechanisms { get; }
	}
}
