//
// FileProvider C# bindings
//
// Authors:
//	Alex Soto  <alexsoto@microsoft.com>
//	Sebastien Pouliot  <sebastien.pouliot@microsoft.com>
//
// Copyright 2017 Xamarin Inc. All rights reserved.
// Copyright 2019 Microsoft Corporation
//

using System;
using ObjCRuntime;
using CoreGraphics;
using Foundation;
using UniformTypeIdentifiers;

namespace FileProvider {
	/// <summary>Delegate for handling a thumbnail fetch operation.</summary>
	delegate void NSFileProviderExtensionFetchThumbnailsHandler (NSString identifier, [NullAllowed] NSData imageData, [NullAllowed] NSError error);

	/// <summary>Subclasses of <see cref="UIKit.NSFileProviderExtension" /> implement the move and open functionality for extensions of type <see cref="UIKit.UIDocumentPickerViewController" />.</summary>
	///     <remarks>
	///       <para>(More documentation for this node is coming)</para>
	///       <para tool="threads">The members of this class can be used from a background thread.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/UIKit/Reference/NSFileProviderExtension_Class/index.html">Apple documentation for <c>NSFileProviderExtension</c></related>
	[NoTV]
	[NoMac]
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[ThreadSafe]
	[BaseType (typeof (NSObject))]
	partial interface NSFileProviderExtension {

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'NSFileProviderManager' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'NSFileProviderManager' instead.")]
		[Static, Export ("writePlaceholderAtURL:withMetadata:error:")]
		bool WritePlaceholder (NSUrl placeholderUrl, NSDictionary metadata, ref NSError error);

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'NSFileProviderManager.GetPlaceholderUrl (NSUrl)' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'NSFileProviderManager.GetPlaceholderUrl (NSUrl)' instead.")]
		[Static, Export ("placeholderURLForURL:")]
		NSUrl GetPlaceholderUrl (NSUrl url);

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'NSFileProviderManager.ProviderIdentifier' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'NSFileProviderManager.ProviderIdentifier' instead.")]
		[Export ("providerIdentifier")]
		string ProviderIdentifier { get; }

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'NSFileProviderManager.DocumentStorageUrl' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'NSFileProviderManager.DocumentStorageUrl' instead.")]
		[Export ("documentStorageURL")]
		NSUrl DocumentStorageUrl { get; }

		[return: NullAllowed]
		[Export ("URLForItemWithPersistentIdentifier:")]
		NSUrl GetUrlForItem (string persistentIdentifier);

		[return: NullAllowed]
		[Export ("persistentIdentifierForItemAtURL:")]
		string GetPersistentIdentifier (NSUrl itemUrl);

		[Export ("providePlaceholderAtURL:completionHandler:")]
		[Async]
		void ProvidePlaceholderAtUrl (NSUrl url, Action<NSError> completionHandler);

		[Export ("startProvidingItemAtURL:completionHandler:")]
		[Async]
		void StartProvidingItemAtUrl (NSUrl url, Action<NSError> completionHandler);

		[Export ("itemChangedAtURL:")]
		void ItemChangedAtUrl (NSUrl url);

		[Export ("stopProvidingItemAtURL:")]
		void StopProvidingItemAtUrl (NSUrl url);

		[Export ("itemForIdentifier:error:")]
		[return: NullAllowed]
		INSFileProviderItem GetItem (NSString identifier, out NSError error);

		// Inlining NSFileProviderExtension (NSFileProviderActions) so we get asyncs

		[Async]
		[Export ("importDocumentAtURL:toParentItemIdentifier:completionHandler:")]
		void ImportDocument (NSUrl fileUrl, string parentItemIdentifier, Action<INSFileProviderItem, NSError> completionHandler);

		[Async]
		[Export ("createDirectoryWithName:inParentItemIdentifier:completionHandler:")]
		void CreateDirectory (string directoryName, string parentItemIdentifier, Action<INSFileProviderItem, NSError> completionHandler);

		[Async]
		[Export ("renameItemWithIdentifier:toName:completionHandler:")]
		void RenameItem (string itemIdentifier, string itemName, Action<INSFileProviderItem, NSError> completionHandler);

		[Async]
		[Export ("reparentItemWithIdentifier:toParentItemWithIdentifier:newName:completionHandler:")]
		void ReparentItem (string itemIdentifier, string parentItemIdentifier, [NullAllowed] string newName, Action<INSFileProviderItem, NSError> completionHandler);

		[Async]
		[Export ("trashItemWithIdentifier:completionHandler:")]
		void TrashItem (string itemIdentifier, Action<INSFileProviderItem, NSError> completionHandler);

		[Async]
		[Export ("untrashItemWithIdentifier:toParentItemIdentifier:completionHandler:")]
		void UntrashItem (string itemIdentifier, [NullAllowed] string parentItemIdentifier, Action<INSFileProviderItem, NSError> completionHandler);

		[Async]
		[Export ("deleteItemWithIdentifier:completionHandler:")]
		void DeleteItem (string itemIdentifier, Action<NSError> completionHandler);

		[Async]
		[Export ("setLastUsedDate:forItemIdentifier:completionHandler:")]
		void SetLastUsedDate ([NullAllowed] NSDate lastUsedDate, string itemIdentifier, Action<INSFileProviderItem, NSError> completionHandler);

		[Async]
		[Export ("setTagData:forItemIdentifier:completionHandler:")]
		void SetTagData ([NullAllowed] NSData tagData, string itemIdentifier, Action<INSFileProviderItem, NSError> completionHandler);

		[Async]
		[Export ("setFavoriteRank:forItemIdentifier:completionHandler:")]
		void SetFavoriteRank ([NullAllowed] NSNumber favoriteRank, string itemIdentifier, Action<INSFileProviderItem, NSError> completionHandler);

		#region NSFileProviderEnumeration (NSFileProviderExtension)
		[Export ("enumeratorForContainerItemIdentifier:error:")]
		[return: NullAllowed]
		INSFileProviderEnumerator GetEnumerator (string containerItemIdentifier, out NSError error);
		#endregion

		// From NSFileProviderExtension (NSFileProviderThumbnailing)

		[Export ("fetchThumbnailsForItemIdentifiers:requestedSize:perThumbnailCompletionHandler:completionHandler:")]
		[Async]
		NSProgress FetchThumbnails (NSString [] itemIdentifiers, CGSize size, NSFileProviderExtensionFetchThumbnailsHandler perThumbnailCompletionHandler, Action<NSError> completionHandler);

		// From NSFileProviderExtension (NSFileProviderService)

		[Export ("supportedServiceSourcesForItemIdentifier:error:")]
		[return: NullAllowed]
		INSFileProviderServiceSource [] GetSupportedServiceSources (string itemIdentifier, out NSError error);

		// From NSFileProviderExtension (NSFileProviderDomain)

		[NullAllowed, Export ("domain")]
		NSFileProviderDomain Domain { get; }
	}
}

namespace FileProvider {

	/// <summary>Enumerates errors relating to providing files.</summary>
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[ErrorDomain ("NSFileProviderErrorDomain")]
	[Native ("NSFileProviderErrorCode")]
	enum NSFileProviderError : long {
		/// <summary>To be added.</summary>
		NotAuthenticated = -1000,
		/// <summary>To be added.</summary>
		FilenameCollision = -1001,
		/// <summary>To be added.</summary>
		SyncAnchorExpired = -1002,
		/// <summary>To be added.</summary>
		PageExpired = SyncAnchorExpired,
		/// <summary>To be added.</summary>
		InsufficientQuota = -1003,
		/// <summary>To be added.</summary>
		ServerUnreachable = -1004,
		/// <summary>To be added.</summary>
		NoSuchItem = -1005,
		VersionOutOfDate = -1006,
		DirectoryNotEmpty = -1007,
		ProviderNotFound = -2001,
		ProviderTranslocated = -2002,
		OlderExtensionVersionRunning = -2003,
		NewerExtensionVersionFound = -2004,
		CannotSynchronize = -2005,
		NonEvictableChildren = -2006,
		UnsyncedEdits = -2007,
		NonEvictable = -2008,
		VersionNoLongerAvailable = -2009,
		ExcludedFromSync = -2010,
		DomainDisabled = -2011,
		ProviderDomainTemporarilyUnavailable = -2012,
		ProviderDomainNotFound = -2013,
		ApplicationExtensionNotFound = -2014,
	}

	[iOS (16, 0), NoMacCatalyst]
	[Native]
	public enum NSFileProviderDomainRemovalMode : long {
		RemoveAll = 0,
		[NoiOS, NoMacCatalyst]
		PreserveDirtyUserData = 1,
		[NoiOS, NoMacCatalyst]
		PreserveDownloadedUserData = 2,
	}

	/// <summary>Defines constants regarding errors regarding keys of the file provider enumeration.</summary>
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[Static]
	interface NSFileProviderErrorKeys {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'NSFileProviderErrorItemKey' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'NSFileProviderErrorItemKey' instead.")]
		[Field ("NSFileProviderErrorCollidingItemKey")]
		NSString CollidingItemKey { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NSFileProviderErrorNonExistentItemIdentifierKey")]
		NSString NonExistentItemIdentifierKey { get; }

		[iOS (15, 0)]
		[Field ("NSFileProviderErrorItemKey")]
		NSString ItemKey { get; }
	}

	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[Static]
	interface NSFileProviderFavoriteRank {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NSFileProviderFavoriteRankUnranked")]
		ulong Unranked { get; }
	}

	/// <summary>Uniquely identifies a File Provider-managed item.</summary>
	[NoMacCatalyst]
	[Static]
	interface NSFileProviderItemIdentifier {

		/// <summary>Gets the persistent name of the root directory in the shared hierarchy.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NSFileProviderRootContainerItemIdentifier")]
		NSString RootContainer { get; }

		/// <summary>Gets the persistent name of the documents and directories.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("NSFileProviderWorkingSetContainerItemIdentifier")]
		NSString WorkingSetContainer { get; }

		[iOS (16, 0)]
		[Field ("NSFileProviderTrashContainerItemIdentifier")]
		NSString TrashContainer { get; }
	}

	/// <summary>Defines the actions the user can perform within the document browser.</summary>
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	enum NSFileProviderItemCapabilities : ulong {
		/// <summary>To be added.</summary>
		Reading = 1 << 0,
		/// <summary>To be added.</summary>
		Writing = 1 << 1,
		/// <summary>To be added.</summary>
		Reparenting = 1 << 2,
		/// <summary>To be added.</summary>
		Renaming = 1 << 3,
		/// <summary>To be added.</summary>
		Trashing = 1 << 4,
		/// <summary>To be added.</summary>
		Deleting = 1 << 5,
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		Evicting = 1 << 6,
		[NoiOS]
		[NoTV]
		[NoMacCatalyst]
		ExcludingFromSync = 1 << 7,
		/// <summary>To be added.</summary>
		AddingSubItems = Writing,
		/// <summary>To be added.</summary>
		ContentEnumerating = Reading,
	}

	[Flags, NoTV, NoMacCatalyst, NoiOS, Mac (12, 3)]
	[Native]
	public enum NSFileProviderMaterializationFlags : ulong {
		KnownSparseRanges = 1uL << 0,
	}

	[Flags, NoTV, NoMacCatalyst, NoiOS, Mac (12, 3)]
	[Native]
	public enum NSFileProviderFetchContentsOptions : ulong {
		StrictVersioning = 1uL << 0,
	}

	[Native]
	[iOS (16, 0), Mac (13, 0), NoTV, NoMacCatalyst]
	public enum NSFileProviderContentPolicy : long {
		Inherited,
		[NoiOS, NoMacCatalyst]
		DownloadLazily,
		DownloadLazilyAndEvictOnRemoteUpdate,
		[NoiOS, NoMacCatalyst]
		DownloadEagerlyAndKeepDownloaded,
	}

	/// <summary>A batch of data to return from an enumerator.</summary>
	[NoMacCatalyst]
	[Static]
	interface NSFileProviderPage {

		[Internal]
		[Field ("NSFileProviderInitialPageSortedByName")]
		IntPtr _InitialPageSortedByName { get; }

		/// <summary>Gets the first page in name order.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("Runtime.GetNSObject<NSData> (_InitialPageSortedByName)")]
		NSData InitialPageSortedByName { get; }

		[Internal]
		[Field ("NSFileProviderInitialPageSortedByDate")]
		IntPtr _InitialPageSortedByDate { get; }

		/// <summary>Gets the first page in date order.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("Runtime.GetNSObject<NSData> (_InitialPageSortedByDate)")]
		NSData InitialPageSortedByDate { get; }
	}

	/// <summary>Partitions the file provider's data along user-meaningful lines, such as accounts or locations.</summary>
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[DisableDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface NSFileProviderDomain {

		/// <param name="identifier">To be added.</param>
		/// <param name="displayName">To be added.</param>
		/// <param name="pathRelativeToDocumentStorage">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[NoMac]
		[Export ("initWithIdentifier:displayName:pathRelativeToDocumentStorage:")]
		NativeHandle Constructor (string identifier, string displayName, string pathRelativeToDocumentStorage);

		[iOS (16, 0)]
		[Export ("initWithIdentifier:displayName:")]
		NativeHandle Constructor (string identifier, string displayName);

		[Mac (15, 0), NoiOS]
		[Export ("initWithDisplayName:userInfo:volumeURL:")]
		NativeHandle Constructor (string displayName, NSDictionary userInfo, [NullAllowed] NSUrl volumeUrl);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("identifier")]
		string Identifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("displayName")]
		string DisplayName { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Export ("pathRelativeToDocumentStorage")]
		string PathRelativeToDocumentStorage { get; }

		[NoiOS]
		[Export ("disconnected")]
		bool Disconnected { [Bind ("isDisconnected")] get; }

		[iOS (16, 0)]
		[Export ("userEnabled")]
		bool UserEnabled { get; }

		[NoiOS]
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")] get; set; }

		[NoMacCatalyst]
		[NoTV, iOS (16, 0)]
		[Export ("testingModes", ArgumentSemantic.Assign)]
		NSFileProviderDomainTestingModes TestingModes { get; set; }

		[iOS (16, 0)]
		[Notification]
		[Field ("NSFileProviderDomainDidChange")]
		NSString DidChange { get; }

		[NoTV, iOS (16, 0), NoMacCatalyst]
		[NullAllowed, Export ("backingStoreIdentity")]
		NSData BackingStoreIdentity { get; }

		[NoTV, NoMacCatalyst, Mac (13, 0), iOS (16, 0)]
		[Export ("replicated")]
		bool Replicated { [Bind ("isReplicated")] get; }

		[NoTV, NoMacCatalyst, iOS (18, 0), Mac (13, 0)]
		[Export ("supportsSyncingTrash")]
		bool SupportsSyncingTrash { get; set; }

		[NoTV, NoMacCatalyst, Mac (13, 3), iOS (16, 4)]
		[NullAllowed, Export ("volumeUUID")]
		NSUuid VolumeUuid { get; }

		[Mac (15, 0), NoiOS]
		[Export ("userInfo", ArgumentSemantic.Copy), NullAllowed]
		NSDictionary<NSString, NSObject> UserInfo { get; set; }

		[Mac (15, 0), NoiOS]
		[Export ("replicatedKnownFolders", ArgumentSemantic.Assign)]
		NSFileProviderKnownFolders ReplicatedKnownFolders { get; }

		[Mac (15, 0), NoiOS]
		[Export ("supportedKnownFolders", ArgumentSemantic.Assign)]
		NSFileProviderKnownFolders SupportedKnownFolders { get; set; }
	}

	interface INSFileProviderEnumerationObserver { }

	[NoMacCatalyst]
	[Protocol]
	interface NSFileProviderEnumerationObserver {

		/// <param name="updatedItems">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("didEnumerateItems:")]
		void DidEnumerateItems (INSFileProviderItem [] updatedItems);

		/// <param name="upToPage">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("finishEnumeratingUpToPage:")]
		void FinishEnumerating ([NullAllowed] NSData upToPage);

		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("finishEnumeratingWithError:")]
		void FinishEnumerating (NSError error);

		[iOS (16, 0)]
		[Export ("suggestedPageSize")]
		nint GetSuggestedPageSize ();
	}

	interface INSFileProviderChangeObserver { }

	/// <summary>Observes changes and deletions of the enumerated files from a <see cref="FileProvider.INSFileProviderEnumerator" />.</summary>
	[NoMacCatalyst]
	[Protocol]
	interface NSFileProviderChangeObserver {

		/// <param name="updatedItems">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("didUpdateItems:")]
		void DidUpdateItems (INSFileProviderItem [] updatedItems);

		/// <param name="deletedItemIdentifiers">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("didDeleteItemsWithIdentifiers:")]
		void DidDeleteItems (string [] deletedItemIdentifiers);

		/// <param name="anchor">To be added.</param>
		/// <param name="moreComing">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("finishEnumeratingChangesUpToSyncAnchor:moreComing:")]
		void FinishEnumeratingChanges (NSData anchor, bool moreComing);

		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("finishEnumeratingWithError:")]
		void FinishEnumerating (NSError error);

		[iOS (16, 0)]
		[Export ("suggestedBatchSize")]
		nint GetSuggestedBatchSize ();
	}

	interface INSFileProviderEnumerator { }

	/// <summary>Enumerates items from an <see cref="FileProvider.INSFileProvider" />.</summary>
	[NoMacCatalyst]
	[Protocol]
	interface NSFileProviderEnumerator {

		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("invalidate")]
		void Invalidate ();

		/// <param name="observer">To be added.</param>
		/// <param name="startPage">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("enumerateItemsForObserver:startingAtPage:")]
		void EnumerateItems (INSFileProviderEnumerationObserver observer, NSData startPage);

		/// <param name="observer">To be added.</param>
		/// <param name="syncAnchor">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("enumerateChangesForObserver:fromSyncAnchor:")]
		void EnumerateChanges (INSFileProviderChangeObserver observer, NSData syncAnchor);

		/// <param name="completionHandler">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("currentSyncAnchorWithCompletionHandler:")]
		void CurrentSyncAnchor (Action<NSData> completionHandler);
	}

	/// <summary>An item provided by an <see cref="FileProvider.INSFileProviderItem" />. (A type alias for <see cref="FileProvider.NSFileProviderItemProtocol" />.)</summary>
	interface INSFileProviderItem { }

	/// <summary>An item provided by an <see cref="FileProvider.INSFileProviderItem" />. (A type alias for <see cref="FileProvider.NSFileProviderItemProtocol" />.)</summary>
	/// <remarks>To be added.</remarks>
	[NoMacCatalyst]
	[Protocol]
	interface NSFileProviderItem {

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("itemIdentifier")]
		string Identifier { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("parentItemIdentifier")]
		string ParentIdentifier { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("filename")]
		string Filename { get; }

		/// <summary>To be added.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Deprecated (PlatformName.iOS, 14, 0, message: "Use 'GetContentType' instead.")]
		[Deprecated (PlatformName.MacOSX, 11, 0, message: "Use 'GetContentType' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 14, 0, message: "Use 'GetContentType' instead.")]
		[Export ("typeIdentifier")]
		string TypeIdentifier { get; }

		[iOS (14, 0)]
		[Export ("contentType", ArgumentSemantic.Copy)]
		UTType GetContentType ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("capabilities")]
		NSFileProviderItemCapabilities GetCapabilities ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("documentSize", ArgumentSemantic.Copy)]
		NSNumber GetDocumentSize ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("childItemCount", ArgumentSemantic.Copy)]
		NSNumber GetChildItemCount ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("creationDate", ArgumentSemantic.Copy)]
		NSDate GetCreationDate ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("contentModificationDate", ArgumentSemantic.Copy)]
		NSDate GetContentModificationDate ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("lastUsedDate", ArgumentSemantic.Copy)]
		NSDate GetLastUsedDate ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("tagData", ArgumentSemantic.Copy)]
		NSData GetTagData ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[NoMac]
		[return: NullAllowed]
		[Export ("favoriteRank", ArgumentSemantic.Copy)]
		NSNumber GetFavoriteRank ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[NoMac]
		[Export ("isTrashed")]
		bool IsTrashed ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isUploaded")]
		bool IsUploaded ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isUploading")]
		bool IsUploading ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("uploadingError", ArgumentSemantic.Copy)]
		NSError GetUploadingError ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isDownloaded")]
		bool IsDownloaded ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isDownloading")]
		bool IsDownloading ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("downloadingError", ArgumentSemantic.Copy)]
		NSError GetDownloadingError ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isMostRecentVersionDownloaded")]
		bool IsMostRecentVersionDownloaded ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isShared")]
		bool IsShared ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Export ("isSharedByCurrentUser")]
		bool IsSharedByCurrentUser ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("ownerNameComponents")]
		NSPersonNameComponents GetOwnerNameComponents ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("mostRecentEditorNameComponents")]
		NSPersonNameComponents GetMostRecentEditorNameComponents ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[NoMac]
		[return: NullAllowed]
		[Export ("versionIdentifier")]
		NSData GetVersionIdentifier ();

		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[return: NullAllowed]
		[Export ("userInfo")]
		NSDictionary GetUserInfo ();

		[iOS (16, 0), NoMacCatalyst]
		[Export ("fileSystemFlags")]
		NSFileProviderFileSystemFlags FileSystemFlags { get; }

		[iOS (16, 0)]
		[Export ("extendedAttributes", ArgumentSemantic.Strong)]
		NSDictionary<NSString, NSData> ExtendedAttributes { get; }

		[iOS (16, 0)]
		[NullAllowed, Export ("itemVersion", ArgumentSemantic.Strong)]
		NSFileProviderItemVersion ItemVersion { get; }

		[iOS (16, 0)]
		[NullAllowed, Export ("symlinkTargetPath")]
		string SymlinkTargetPath { get; }

		[iOS (16, 0), NoMacCatalyst]
		[Export ("typeAndCreator")]
		NSFileProviderTypeAndCreator TypeAndCreator { get; }

		[NoTV, NoMacCatalyst, Mac (13, 0), iOS (16, 0)]
		[Export ("contentPolicy")]
		NSFileProviderContentPolicy ContentPolicy { get; }
	}

	/// <summary>A shared object that is accessible from both the containing app and the extension.</summary>
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSFileProviderManager {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Static]
		[Export ("defaultManager", ArgumentSemantic.Strong)]
		NSFileProviderManager DefaultManager { get; }

		/// <param name="containerItemIdentifier">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("signalEnumeratorForContainerItemIdentifier:completionHandler:")]
		// Not Async'ified on purpose, because this can switch from app to extension.
		void SignalEnumerator (string containerItemIdentifier, Action<NSError> completion);

		// Not Async'ified on purpose, because the task must be accesed while the completion action is performing...
		/// <param name="task">To be added.</param>
		///         <param name="identifier">To be added.</param>
		///         <param name="completion">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("registerURLSessionTask:forItemWithIdentifier:completionHandler:")]
		void Register (NSUrlSessionTask task, string identifier, Action<NSError> completion);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Export ("providerIdentifier")]
		string ProviderIdentifier { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Export ("documentStorageURL")]
		NSUrl DocumentStorageUrl { get; }

		/// <param name="placeholderUrl">To be added.</param>
		///         <param name="metadata">To be added.</param>
		///         <param name="error">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Static]
		[Export ("writePlaceholderAtURL:withMetadata:error:")]
		bool WritePlaceholder (NSUrl placeholderUrl, INSFileProviderItem metadata, out NSError error);

		/// <param name="url">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[NoMac]
		[Static]
		[Export ("placeholderURLForURL:")]
		NSUrl GetPlaceholderUrl (NSUrl url);

		/// <param name="domain">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Async (XmlDocs = """
			<param name="domain">To be added.</param>
			<summary>To be added.</summary>
			<returns>A task that represents the asynchronous AddDomain operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("addDomain:completionHandler:")]
		void AddDomain (NSFileProviderDomain domain, Action<NSError> completionHandler);

		/// <param name="domain">To be added.</param>
		///         <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Async (XmlDocs = """
			<param name="domain">To be added.</param>
			<summary>To be added.</summary>
			<returns>A task that represents the asynchronous RemoveDomain operation</returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("removeDomain:completionHandler:")]
		void RemoveDomain (NSFileProviderDomain domain, Action<NSError> completionHandler);

		/// <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Async (XmlDocs = """
			<summary>To be added.</summary>
			<returns>
			          <para class="improve-task-t-return-type-description">A task that represents the asynchronous GetDomains operation.  The value of the TResult parameter is of type System.Action&lt;FileProvider.NSFileProviderDomain[],Foundation.NSError&gt;.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		[Export ("getDomainsWithCompletionHandler:")]
		void GetDomains (Action<NSFileProviderDomain [], NSError> completionHandler);

		/// <param name="completionHandler">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Static]
		[Async (XmlDocs = """
			<summary>To be added.</summary>
			<returns>A task that represents the asynchronous RemoveAllDomains operation</returns>
			<remarks>
			          <para copied="true">The RemoveAllDomainsAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("removeAllDomainsWithCompletionHandler:")]
		void RemoveAllDomains (Action<NSError> completionHandler);

		/// <param name="domain">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("managerForDomain:")]
		[return: NullAllowed]
		NSFileProviderManager FromDomain (NSFileProviderDomain domain);

		[iOS (16, 0)]
		[Static]
		[Async (ResultTypeName = "NSFileProviderGetIdentifierResult")]
		[Export ("getIdentifierForUserVisibleFileAtURL:completionHandler:")]
		void GetIdentifierForUserVisibleFile (NSUrl url, NSFileProviderGetIdentifierHandler completionHandler);

		[iOS (16, 0)]
		[Async]
		[Export ("getUserVisibleURLForItemIdentifier:completionHandler:")]
		void GetUserVisibleUrl (NSString itemIdentifier, Action<NSUrl, NSError> completionHandler);

		[iOS (16, 0)]
		[Export ("temporaryDirectoryURLWithError:")]
		[return: NullAllowed]
		NSUrl GetTemporaryDirectoryUrl ([NullAllowed] out NSError error);

		[iOS (16, 0)]
		[Async]
		[Export ("signalErrorResolved:completionHandler:")]
		void SignalErrorResolved (NSError error, Action<NSError> completionHandler);

		[Unavailable (PlatformName.MacCatalyst)]
		[NoTV, iOS (16, 0)]
		[Export ("globalProgressForKind:")]
		NSProgress GetGlobalProgress (NSString kind); // NSString intended.

		[Unavailable (PlatformName.MacCatalyst)]
		[NoTV, iOS (16, 0)]
		[Field ("NSFileProviderMaterializedSetDidChange")]
		[Notification]
		NSString MaterializedSetDidChange { get; }

		[Unavailable (PlatformName.MacCatalyst)]
		[NoTV, iOS (16, 0)]
		[Field ("NSFileProviderPendingSetDidChange")]
		[Notification]
		NSString PendingSetDidChange { get; }

		#region Import (NSFileProviderManager)
		[iOS (16, 0)]
		[Static]
		[Async]
		[Export ("importDomain:fromDirectoryAtURL:completionHandler:")]
		void Import (NSFileProviderDomain domain, NSUrl url, Action<NSError> completionHandler);

		[iOS (16, 0)]
		[Async]
		[Export ("reimportItemsBelowItemWithIdentifier:completionHandler:")]
		void ReimportItemsBelowItem (NSString itemIdentifier, Action<NSError> completionHandler);
		#endregion

		#region MaterializedSet (NSFileProviderManager)
		[iOS (16, 0)]
		[Export ("enumeratorForMaterializedItems")]
		INSFileProviderEnumerator GetMaterializedItemsEnumerator ();
		#endregion

		#region Eviction (NSFileProviderManager)
		[iOS (16, 0)]
		[Export ("evictItemWithIdentifier:completionHandler:")]
		[Async]
		void EvictItem (NSString itemIdentifier, Action<NSError> completionHandler);
		#endregion

		#region Disconnection (NSFileProviderManager)
		[NoiOS]
		[Async]
		[Export ("disconnectWithReason:options:completionHandler:")]
		void Disconnect (string localizedReason, NSFileProviderManagerDisconnectionOptions options, Action<NSError> completionHandler);

		[NoiOS]
		[Async]
		[Export ("reconnectWithCompletionHandler:")]
		void Reconnect (Action<NSError> completionHandler);
		#endregion

		#region Barrier (NSFileProviderManager)
		[iOS (16, 0)]
		[Async]
		[Export ("waitForChangesOnItemsBelowItemWithIdentifier:completionHandler:")]
		void WaitForChangesOnItemsBelowItem (string itemIdentifier, Action<NSError> completionHandler);
		#endregion

		#region Stabilization (NSFileProviderManager)
		[NoiOS]
		[Async]
		[Export ("waitForStabilizationWithCompletionHandler:")]
		void WaitForStabilization (Action<NSError> completionHandler);
		#endregion

		[Unavailable (PlatformName.MacCatalyst)]
		[NoTV, iOS (16, 0)]
		[Export ("enumeratorForPendingItems")]
		INSFileProviderPendingSetEnumerator GetEnumeratorForPendingItems ();

		// From NSFileProviderManager (TestingModeInteractive) Category

		[Unavailable (PlatformName.MacCatalyst)]
		[NoTV, iOS (16, 0)]
		[Export ("listAvailableTestingOperationsWithError:")]
		[return: NullAllowed]
		INSFileProviderTestingOperation [] ListAvailableTestingOperations ([NullAllowed] out NSError error);

		[Unavailable (PlatformName.MacCatalyst)]
		[NoTV, iOS (16, 0)]
		[Export ("runTestingOperations:error:")]
		[return: NullAllowed]
		NSDictionary<INSFileProviderTestingOperation, NSError> GetRunTestingOperations (INSFileProviderTestingOperation [] operations, [NullAllowed] out NSError error);

		[iOS (16, 0), NoMacCatalyst]
		[Async (ResultTypeName = "NSFileProviderRemoveDomainResult")]
		[Static]
		[Export ("removeDomain:mode:completionHandler:")]
		void RemoveDomain (NSFileProviderDomain domain, NSFileProviderDomainRemovalMode mode, Action<NSUrl, NSError> completionHandler);

		[Async]
		[iOS (16, 0), Mac (13, 0), NoTV, NoMacCatalyst]
		[Export ("getServiceWithName:itemIdentifier:completionHandler:")]
		void GetService (string serviceName, string itemIdentifier, Action<NSFileProviderService, NSError> completionHandler);

		[Async]
		[NoTV, NoMacCatalyst, Mac (13, 0), iOS (16, 0)]
		[Export ("requestModificationOfFields:forItemWithIdentifier:options:completionHandler:")]
		void RequestModification (NSFileProviderItemFields fields, string itemIdentifier, NSFileProviderModifyItemOptions options, Action<NSError> completionHandler);

		[Async]
		[NoTV, NoMacCatalyst, NoiOS, Mac (13, 0)]
		[Export ("requestDownloadForItemWithIdentifier:requestedRange:completionHandler:")]
		void RequestDownload (string itemIdentifier, NSRange rangeToMaterialize, Action<NSError> completionHandler);

		// From NSFileProviderManager (ExternalDomain) Category
		[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
		[Export ("checkDomainsCanBeStored:onVolumeAtURL:unsupportedReason:error:")]
		[Static]
		bool CheckDomainsCanBeStored (out bool eligible, NSUrl volumeAtUrl, out NSFileProviderVolumeUnsupportedReason unsupportedReason, [NullAllowed] out NSError error);
	}

	interface INSFileProviderPendingSetEnumerator { }

	[Category]
	[BaseType (typeof (NSFileProviderManager))]
	[NoTV, NoMacCatalyst, NoiOS, Mac (15, 4)]
	interface NSFileProviderManager_Diagnostics {
		[Export ("requestDiagnosticCollectionForItemWithIdentifier:errorReason:completionHandler:")]
		void RequestDiagnosticCollection (string itemIdentifier, NSError errorReason, NSFileProviderManagerRequestDiagnosticCollectionCallback completionHandler);
	}

	delegate void NSFileProviderManagerRequestDiagnosticCollectionCallback ([NullAllowed] NSError error);

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Protocol]
	interface NSFileProviderPendingSetEnumerator : NSFileProviderEnumerator {

		[Abstract]
		[NullAllowed, Export ("domainVersion")]
		NSFileProviderDomainVersion DomainVersion { get; }

		[Abstract]
		[Export ("refreshInterval")]
		double RefreshInterval { get; }

#if XAMCORE_5_0
		[Abstract]
#endif
		[NoTV, NoMacCatalyst, Mac (13, 0), iOS (16, 0)]
		[Export ("maximumSizeReached")]
		bool MaximumSizeReached { [Bind ("isMaximumSizeReached")] get; }
	}

	// typedef NSString *NSFileProviderDomainIdentifier NS_EXTENSIBLE_STRING_ENUM
	delegate void NSFileProviderGetIdentifierHandler (/* /NSFileProviderItemIdentifier */ [NullAllowed] NSString itemIdentifier, /* NSFileProviderDomainIdentifier */ [NullAllowed] NSString domainIdentifier, [NullAllowed] NSError error);

	interface INSFileProviderServiceSource { }

	/// <summary>Provides a communication channel between host applications and file provider extensions.</summary>
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[Protocol]
	interface NSFileProviderServiceSource {

		/// <summary>Gets the unique service name.</summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("serviceName")]
		string ServiceName { get; }

		/// <param name="error">On failure, contains the error that occurred.</param>
		/// <summary>Creates and returns an endpoint for communicating with the file provider extension.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("makeListenerEndpointAndReturnError:")]
		[return: NullAllowed]
		NSXpcListenerEndpoint MakeListenerEndpoint (out NSError error);

		[NoTV, NoMacCatalyst, Mac (13, 0), iOS (16, 0)]
		[Export ("restricted")]
		bool Restricted { [Bind ("isRestricted")] get; }
	}

	[iOS (16, 0)]
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[BaseType (typeof (NSObject))]
	interface NSFileProviderItemVersion {

		[Export ("initWithContentVersion:metadataVersion:")]
		NativeHandle Constructor (NSData contentVersion, NSData metadataVersion);

		[Export ("contentVersion")]
		NSData ContentVersion { get; }

		[Export ("metadataVersion")]
		NSData MetadataVersion { get; }

		[NoTV, NoMacCatalyst, iOS (16, 0)]
		[Static]
		[Export ("beforeFirstSyncComponent")]
		NSData BeforeFirstSyncComponent { get; }
	}

	[iOS (16, 0)]
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[Native]
	[Flags]
	enum NSFileProviderCreateItemOptions : ulong {
		None = 0,
		MayAlreadyExist = 1,
		DeletionConflicted = 2,
	}

	[iOS (16, 0)]
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[Native]
	[Flags]
	enum NSFileProviderDeleteItemOptions : ulong {
		None = 0,
		Recursive = 1,
	}

	[iOS (16, 0)]
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[Native]
	[Flags]
	enum NSFileProviderModifyItemOptions : ulong {
		None = 0,
		MayAlreadyExist = 1,
	}

	[iOS (16, 0)]
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[Native]
	[Flags]
	enum NSFileProviderItemFields : ulong {
		Contents = 1uL << 0,
		Filename = 1uL << 1,
		ParentItemIdentifier = 1uL << 2,
		LastUsedDate = 1uL << 3,
		TagData = 1uL << 4,
		FavoriteRank = 1uL << 5,
		CreationDate = 1uL << 6,
		ContentModificationDate = 1uL << 7,
		FileSystemFlags = 1uL << 8,
		ExtendedAttributes = 1uL << 9,
		TypeAndCreator = 1uL << 10,
	}

	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[NoiOS]
	[Native]
	[Flags]
	enum NSFileProviderManagerDisconnectionOptions : ulong {
		None = 0,
		Temporary = 1,
	}

	[iOS (15, 0)]
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[Native]
	[Flags]
	enum NSFileProviderFileSystemFlags : ulong {
		UserExecutable = 1uL << 0,
		UserReadable = 1uL << 1,
		UserWritable = 1uL << 2,
		Hidden = 1uL << 3,
		PathExtensionHidden = 1uL << 4,
	}

	[iOS (16, 0)]
	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[BaseType (typeof (NSObject))]
	interface NSFileProviderRequest {

		[Export ("isSystemRequest")]
		bool IsSystemRequest { get; }

		[Export ("isFileViewerRequest")]
		bool IsFileViewerRequest { get; }

		[NullAllowed]
		[Export ("requestingExecutable", ArgumentSemantic.Copy)]
		NSUrl RequestingExecutable { get; }

		[NoTV, iOS (16, 0)]
		[NoMacCatalyst]
		[NullAllowed, Export ("domainVersion")]
		NSFileProviderDomainVersion DomainVersion { get; }
	}

	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[iOS (16, 0)]
	[Protocol]
	interface NSFileProviderCustomAction {

		[Abstract]
		[Export ("performActionWithIdentifier:onItemsWithIdentifiers:completionHandler:")]
		NSProgress PerformAction (string actionIdentifier, string [] itemIdentifiers, Action<NSError> completionHandler);
	}

	interface INSFileProviderEnumerating { }

	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[iOS (16, 0)]
	[Protocol]
	interface NSFileProviderEnumerating {

		[Abstract]
		[Export ("enumeratorForContainerItemIdentifier:request:error:")]
		[return: NullAllowed]
		INSFileProviderEnumerator GetEnumerator (string containerItemIdentifier, NSFileProviderRequest request, [NullAllowed] out NSError error);
	}

	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[iOS (16, 0)]
	[Protocol]
	interface NSFileProviderIncrementalContentFetching {

		[Abstract]
		[Export ("fetchContentsForItemWithIdentifier:version:usingExistingContentsAtURL:existingVersion:request:completionHandler:")]
		NSProgress FetchContents (string itemIdentifier, [NullAllowed] NSFileProviderItemVersion requestedVersion, NSUrl existingContents, NSFileProviderItemVersion existingVersion, NSFileProviderRequest request, NSFileProviderFetchContentsCompletionHandler completionHandler);
	}

	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[iOS (16, 0)]
	[Protocol]
	interface NSFileProviderServicing {

		[Abstract]
		[Export ("supportedServiceSourcesForItemIdentifier:completionHandler:")]
		NSProgress GetSupportedServiceSources (string itemIdentifier, Action<INSFileProviderServiceSource [], NSError> completionHandler);
	}

	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[iOS (16, 0)]
	[Protocol]
	interface NSFileProviderThumbnailing {

		[Abstract]
		[Export ("fetchThumbnailsForItemIdentifiers:requestedSize:perThumbnailCompletionHandler:completionHandler:")]
		NSProgress FetchThumbnails (string [] itemIdentifiers, CGSize size, NSFileProviderPerThumbnailCompletionHandler perThumbnailCompletionHandler, Action<NSError> completionHandler);
	}

	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[NoiOS]
	delegate void NSFileProviderPerThumbnailCompletionHandler (NSString identifier, [NullAllowed] NSData imageData, [NullAllowed] NSError error);

	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[NoiOS]
	delegate void NSFileProviderFetchContentsCompletionHandler ([NullAllowed] NSUrl fileContents, [NullAllowed] INSFileProviderItem item, [NullAllowed] NSError error);

	[Unavailable (PlatformName.MacCatalyst)]
	[Advice ("This API is not available when using Catalyst on macOS.")]
	[NoiOS]
	delegate void NSFileProviderCreateOrModifyItemCompletionHandler ([NullAllowed] INSFileProviderItem item, NSFileProviderItemFields stillPendingFields, bool shouldFetchContent, [NullAllowed] NSError error);

	[Unavailable (PlatformName.MacCatalyst)]
	[iOS (16, 0)]
	[Protocol]
	[Advice ("Implementation must expose selector 'initWithDomain:' with '.ctor (NSFileProviderDomain)'.")]
	interface NSFileProviderReplicatedExtension : NSFileProviderEnumerating {

		/* see Advice above
		[Abstract]
		[Export ("initWithDomain:")]
		NativeHandle Constructor (NSFileProviderDomain domain);
		*/

		[Abstract]
		[Export ("invalidate")]
		void Invalidate ();

		[Abstract]
		[Export ("itemForIdentifier:request:completionHandler:")]
		NSProgress GetItem (string identifier, NSFileProviderRequest request, Action<INSFileProviderItem, NSError> completionHandler);

		[Abstract]
		[Export ("createItemBasedOnTemplate:fields:contents:options:request:completionHandler:")]
		NSProgress CreateItem (INSFileProviderItem itemTemplate, NSFileProviderItemFields fields, [NullAllowed] NSUrl url, NSFileProviderCreateItemOptions options, NSFileProviderRequest request, NSFileProviderCreateOrModifyItemCompletionHandler completionHandler);

		[Abstract]
		[Export ("fetchContentsForItemWithIdentifier:version:request:completionHandler:")]
		NSProgress FetchContents (string itemIdentifier, [NullAllowed] NSFileProviderItemVersion requestedVersion, NSFileProviderRequest request, NSFileProviderFetchContentsCompletionHandler completionHandler);

		[Abstract]
		[Export ("modifyItem:baseVersion:changedFields:contents:options:request:completionHandler:")]
		NSProgress ModifyItem (INSFileProviderItem item, NSFileProviderItemVersion version, NSFileProviderItemFields changedFields, [NullAllowed] NSUrl newContents, NSFileProviderModifyItemOptions options, NSFileProviderRequest request, NSFileProviderCreateOrModifyItemCompletionHandler completionHandler);

		[Abstract]
		[Export ("deleteItemWithIdentifier:baseVersion:options:request:completionHandler:")]
		NSProgress DeleteItem (string identifier, NSFileProviderItemVersion version, NSFileProviderDeleteItemOptions options, NSFileProviderRequest request, Action<NSError> completionHandler);

		[Export ("importDidFinishWithCompletionHandler:")]
		void ImportDidFinish (Action completionHandler);

		[Export ("materializedItemsDidChangeWithCompletionHandler:")]
		void MaterializedItemsDidChange (Action completionHandler);

		[NoMacCatalyst]
		[NoTV, iOS (16, 0)]
		[Export ("pendingItemsDidChangeWithCompletionHandler:")]
		void PendingItemsDidChange (Action completionHandler);
	}

	interface INSFileProviderDomainState { }

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Protocol]
	interface NSFileProviderDomainState {

		[Abstract]
		[Export ("domainVersion")]
		NSFileProviderDomainVersion DomainVersion { get; }

		[Abstract]
		[Export ("userInfo", ArgumentSemantic.Strong)]
		NSDictionary UserInfo { get; }
	}

	[NoTV, iOS (15, 0), NoMacCatalyst]
	[Flags]
	[Native]
	public enum NSFileProviderDomainTestingModes : ulong {
		AlwaysEnabled = 1uL << 0,
		Interactive = 1uL << 1,
	}

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSFileProviderDomainVersion : NSSecureCoding {

		[Export ("next")]
		NSFileProviderDomainVersion Next { get; }

		[Export ("compare:")]
		NSComparisonResult Compare (NSFileProviderDomainVersion otherVersion);
	}

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Native]
	public enum NSFileProviderTestingOperationType : long {
		Ingestion = 0,
		Lookup = 1,
		Creation = 2,
		Modification = 3,
		Deletion = 4,
		ContentFetch = 5,
		ChildrenEnumeration = 6,
		CollisionResolution = 7,
	}

	interface INSFileProviderTestingOperation : global::ObjCRuntime.INativeObject { }

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Protocol]
	interface NSFileProviderTestingOperation {

		[Abstract]
		[Export ("type")]
		NSFileProviderTestingOperationType Type { get; }

		[Abstract]
		[return: NullAllowed]
		[Export ("asIngestion")]
		INSFileProviderTestingIngestion GetAsIngestion ();

		[Abstract]
		[return: NullAllowed]
		[Export ("asLookup")]
		INSFileProviderTestingLookup GetAsLookup ();

		[Abstract]
		[return: NullAllowed]
		[Export ("asCreation")]
		INSFileProviderTestingCreation GetAsCreation ();

		[Abstract]
		[return: NullAllowed]
		[Export ("asModification")]
		INSFileProviderTestingModification GetAsModification ();

		[Abstract]
		[return: NullAllowed]
		[Export ("asDeletion")]
		INSFileProviderTestingDeletion GetAsDeletion ();

		[Abstract]
		[return: NullAllowed]
		[Export ("asContentFetch")]
		INSFileProviderTestingContentFetch GetAsContentFetch ();

		[Abstract]
		[return: NullAllowed]
		[Export ("asChildrenEnumeration")]
		INSFileProviderTestingChildrenEnumeration GetAsChildrenEnumeration ();

		[Abstract]
		[return: NullAllowed]
		[Export ("asCollisionResolution")]
		INSFileProviderTestingCollisionResolution GetAsCollisionResolution ();
	}

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Native]
	public enum NSFileProviderTestingOperationSide : ulong {
		Disk = 0,
		FileProvider = 1,
	}

	interface INSFileProviderTestingIngestion { }

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Protocol]
	interface NSFileProviderTestingIngestion : NSFileProviderTestingOperation {

		[Abstract]
		[Export ("side")]
		NSFileProviderTestingOperationSide Side { get; }

		[Abstract]
		[Export ("itemIdentifier")]
		string ItemIdentifier { get; }

		[Abstract]
		[NullAllowed, Export ("item")]
		INSFileProviderItem Item { get; }
	}

	interface INSFileProviderTestingLookup { }

	[NoTV, iOS (16, 0), NoMacCatalyst]
	[Protocol]
	interface NSFileProviderTestingLookup : NSFileProviderTestingOperation {

		[Abstract]
		[Export ("side")]
		NSFileProviderTestingOperationSide Side { get; }

		[Abstract]
		[Export ("itemIdentifier")]
		string ItemIdentifier { get; }
	}

	interface INSFileProviderTestingCreation { }

	[NoTV, iOS (16, 0), NoMacCatalyst]
	[Protocol]
	interface NSFileProviderTestingCreation : NSFileProviderTestingOperation {

		[Abstract]
		[Export ("targetSide")]
		NSFileProviderTestingOperationSide TargetSide { get; }

		[Abstract]
		[Export ("sourceItem")]
		INSFileProviderItem SourceItem { get; }

		[Abstract]
		[NullAllowed, Export ("domainVersion")]
		NSFileProviderDomainVersion DomainVersion { get; }
	}

	interface INSFileProviderTestingModification { }

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Protocol]
	interface NSFileProviderTestingModification : NSFileProviderTestingOperation {

		[Abstract]
		[Export ("targetSide")]
		NSFileProviderTestingOperationSide TargetSide { get; }

		[Abstract]
		[Export ("sourceItem")]
		INSFileProviderItem SourceItem { get; }

		[Abstract]
		[Export ("targetItemIdentifier")]
		string TargetItemIdentifier { get; }

		[Abstract]
		[Export ("targetItemBaseVersion")]
		NSFileProviderItemVersion TargetItemBaseVersion { get; }

		[Abstract]
		[Export ("changedFields")]
		NSFileProviderItemFields ChangedFields { get; }

		[Abstract]
		[NullAllowed, Export ("domainVersion")]
		NSFileProviderDomainVersion DomainVersion { get; }
	}

	interface INSFileProviderTestingDeletion { }

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Protocol]
	interface NSFileProviderTestingDeletion : NSFileProviderTestingOperation {

		[Abstract]
		[Export ("targetSide")]
		NSFileProviderTestingOperationSide TargetSide { get; }

		[Abstract]
		[Export ("sourceItemIdentifier")]
		string SourceItemIdentifier { get; }

		[Abstract]
		[Export ("targetItemIdentifier")]
		string TargetItemIdentifier { get; }

		[Abstract]
		[Export ("targetItemBaseVersion")]
		NSFileProviderItemVersion TargetItemBaseVersion { get; }

		[Abstract]
		[NullAllowed, Export ("domainVersion")]
		NSFileProviderDomainVersion DomainVersion { get; }
	}

	interface INSFileProviderTestingContentFetch { }

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Protocol]
	interface NSFileProviderTestingContentFetch : NSFileProviderTestingOperation {

		[Abstract]
		[Export ("side")]
		NSFileProviderTestingOperationSide Side { get; }

		[Abstract]
		[Export ("itemIdentifier")]
		string ItemIdentifier { get; }
	}

	interface INSFileProviderTestingChildrenEnumeration { }

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Protocol]
	interface NSFileProviderTestingChildrenEnumeration : NSFileProviderTestingOperation {

		[Abstract]
		[Export ("side")]
		NSFileProviderTestingOperationSide Side { get; }

		[Abstract]
		[Export ("itemIdentifier")]
		string ItemIdentifier { get; }
	}

	interface INSFileProviderTestingCollisionResolution { }

	[NoMacCatalyst]
	[NoTV, iOS (16, 0)]
	[Protocol]
	interface NSFileProviderTestingCollisionResolution : NSFileProviderTestingOperation {

		[Abstract]
		[Export ("side")]
		NSFileProviderTestingOperationSide Side { get; }

		[Abstract]
		[Export ("renamedItem")]
		INSFileProviderItem RenamedItem { get; }
	}

	[NoTV, NoiOS, NoMacCatalyst]
	[Protocol]
	interface NSFileProviderUserInteractionSuppressing {
		[Abstract]
		[Export ("setInteractionSuppressed:forIdentifier:")]
		void SetInteractionSuppressed (bool suppression, string suppressionIdentifier);

		[Abstract]
		[Export ("isInteractionSuppressedForIdentifier:")]
		bool IsInteractionSuppressed (string suppressionIdentifier);
	}

	interface INSFileProviderPartialContentFetching { }
	delegate void NSFileProviderPartialContentFetchingCompletionHandler (NSUrl fileContents, INSFileProviderItem item, NSRange retrievedRange, NSFileProviderMaterializationFlags flags, NSError error);

	[NoTV, NoMacCatalyst, NoiOS, Mac (12, 3)]
	[Protocol]
	interface NSFileProviderPartialContentFetching {

		[Abstract]
		[Export ("fetchPartialContentsForItemWithIdentifier:version:request:minimalRange:aligningTo:options:completionHandler:")]
		NSProgress FetchPartialContents (string itemIdentifier, NSFileProviderItemVersion requestedVersion, NSFileProviderRequest request, NSRange requestedRange, nuint alignment, NSFileProviderFetchContentsOptions options, NSFileProviderPartialContentFetchingCompletionHandler completionHandler);
	}

	[NoTV, iOS (18, 0), MacCatalyst (18, 0), Mac (15, 0)]
	[Native]
	public enum NSFileProviderKnownFolders : ulong {
		Desktop = 1 << 0,
		Documents = 1 << 1,
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[BaseType (typeof (NSObject))]
	interface NSFileProviderKnownFolderLocation {
		[Export ("initWithParentItemIdentifier:filename:")]
		NativeHandle Constructor (string parentItemIdentifier, string filename);

		[Export ("initWithExistingItemIdentifier:")]
		NativeHandle Constructor (string existing);
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[BaseType (typeof (NSObject))]
	interface NSFileProviderKnownFolderLocations {
		[Export ("shouldCreateBinaryCompatibilitySymlink", ArgumentSemantic.Assign)]
		bool ShouldCreateBinaryCompatibilitySymlink { get; set; }

		[Export ("desktopLocation", ArgumentSemantic.Strong), NullAllowed]
		NSFileProviderKnownFolderLocation DesktopLocation { get; set; }

		[Export ("documentsLocation", ArgumentSemantic.Strong), NullAllowed]
		NSFileProviderKnownFolderLocation DocumentsLocation { get; set; }
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	delegate void NSFileProviderManagerKnownFoldersCallback ([NullAllowed] NSError error);

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[Category]
	[BaseType (typeof (NSFileProviderManager))]
	interface NSFileProviderManager_KnownFolders {
		[Export ("claimKnownFolders:localizedReason:completionHandler:")]
		void ClaimKnownFolders (NSFileProviderKnownFolderLocations knownFolders, string localizedReason, NSFileProviderManagerKnownFoldersCallback completionHandler);

		[Export ("releaseKnownFolders:localizedReason:completionHandler:")]
		void ReleaseKnownFolders (NSFileProviderKnownFolderLocations knownFolders, string localizedReason, NSFileProviderManagerKnownFoldersCallback completionHandler);
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	delegate void NSFileProviderKnownFolderLocationCallback (INSFileProviderKnownFolderSupporting result, [NullAllowed] NSError error);

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface NSFileProviderKnownFolderSupporting {
		[Abstract]
		[Export ("getKnownFolderLocations:completionHandler:")]
		void GetKnownFolderLocations (NSFileProviderKnownFolders knownFolders, NSFileProviderKnownFolderLocationCallback completionHandler);
	}

	interface INSFileProviderKnownFolderSupporting { }

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[Category]
	[BaseType (typeof (NSFileProviderManager))]
	interface NSFileProviderManager_StateDirectory {
		[Export ("stateDirectoryURLWithError:")]
		[return: NullAllowed]
		NSUrl GetStateDirectoryUrl (out NSError error);
	}

	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[Native]
	public enum NSFileProviderVolumeUnsupportedReason : ulong {
		None = 0,
		Unknown = 1 << 0,
		NonAPFS = 1 << 1,
		NonEncrypted = 1 << 2,
		ReadOnly = 1 << 3,
		Network = 1 << 4,
		Quarantined = 1 << 5,
	}

#if !XAMCORE_5_0
	[NoTV, NoiOS, NoMacCatalyst, Mac (15, 0)]
	[Category]
	[BaseType (typeof (NSFileProviderManager))]
	interface NSFileProviderManager_ExternalDomain {
		[Obsolete ("Call 'NSFileProviderManager.CheckDomainsCanBeStored' instead.")]
		[Export ("checkDomainsCanBeStored:onVolumeAtURL:unsupportedReason:error:")]
		unsafe bool CheckDomainsCanBeStored (out bool eligible, NSUrl volumeAtUrl, NSFileProviderVolumeUnsupportedReason* unsupportedReason, [NullAllowed] out NSError error);
	}
#endif

	[NoTV, NoMacCatalyst, NoiOS, Mac (15, 0)]
	[Protocol (BackwardsCompatibleCodeGeneration = false)]
	interface NSFileProviderExternalVolumeHandling {
		[Abstract]
		[Export ("shouldConnectExternalDomainWithCompletionHandler:")]
		void ShouldConnectExternalDomain (NSFileProviderExternalVolumeHandlingShouldConnectExternalDomainCallback completionHandler);
	}

	delegate void NSFileProviderExternalVolumeHandlingShouldConnectExternalDomainCallback ([NullAllowed] NSError connectionError);
}
