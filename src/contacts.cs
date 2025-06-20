//
// Contacts bindings
//
// Authors:
//	Alex Soto  <alex.soto@xamarin.com>
//
// Copyright 2015 Xamarin Inc. All rights reserved.
//

using System;
using System.ComponentModel;
using ObjCRuntime;
using Foundation;

namespace Contacts {

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="Contacts.ICNKeyDescriptor" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="Contacts.ICNKeyDescriptor" />.</para>
	///       <para>If you create objects that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="Contacts.ICNKeyDescriptor" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="Contacts.CNKeyDescriptor_Extensions" /> class as extension methods to the interface, allowing you to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface ICNKeyDescriptor { }

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="Contacts.ICNKeyDescriptor" />.</summary>
	/// <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="Contacts.ICNKeyDescriptor" />.</para>
	///       <para>If you create objects that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="Contacts.ICNKeyDescriptor" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="Contacts.CNKeyDescriptor_Extensions" /> class as extension methods to the interface, allowing you to invoke any optional methods on the protocol.</para>
	///     </remarks>
	[MacCatalyst (13, 1)]
	[Protocol]
	// Headers say "This protocol is reserved for Contacts framework usage.", so don't create a model
	interface CNKeyDescriptor : NSObjectProtocol, NSSecureCoding, NSCopying {
	}

	/// <summary>Represents a contact such as a person or business and holds their data, such as name, phone numbers, etc.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNContact_Class/index.html">Apple documentation for <c>CNContact</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNContact : NSCopying, NSMutableCopying, NSSecureCoding, NSItemProviderReading, NSItemProviderWriting {

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("contactType")]
		CNContactType ContactType { get; }

		[Export ("namePrefix")]
		string NamePrefix { get; }

		[Export ("givenName")]
		string GivenName { get; }

		[Export ("middleName")]
		string MiddleName { get; }

		[Export ("familyName")]
		string FamilyName { get; }

		[Export ("previousFamilyName")]
		string PreviousFamilyName { get; }

		[Export ("nameSuffix")]
		string NameSuffix { get; }

		[Export ("nickname")]
		string Nickname { get; }

		[Export ("phoneticGivenName")]
		string PhoneticGivenName { get; }

		[Export ("phoneticMiddleName")]
		string PhoneticMiddleName { get; }

		[Export ("phoneticFamilyName")]
		string PhoneticFamilyName { get; }

		[MacCatalyst (13, 1)]
		[Export ("phoneticOrganizationName")]
		string PhoneticOrganizationName { get; }

		[Export ("organizationName")]
		string OrganizationName { get; }

		[Export ("departmentName")]
		string DepartmentName { get; }

		[Export ("jobTitle")]
		string JobTitle { get; }

		[Export ("note")]
		string Note { get; }

		[NullAllowed]
		[Export ("imageData", ArgumentSemantic.Copy)]
		NSData ImageData { get; }

		[NullAllowed]
		[Export ("thumbnailImageData", ArgumentSemantic.Copy)]
		NSData ThumbnailImageData { get; }

		[MacCatalyst (13, 1)]
		[Export ("imageDataAvailable")]
		bool ImageDataAvailable { get; }

		[Export ("phoneNumbers", ArgumentSemantic.Copy)]
		CNLabeledValue<CNPhoneNumber> [] PhoneNumbers { get; }

		[Export ("emailAddresses", ArgumentSemantic.Copy)]
		CNLabeledValue<NSString> [] EmailAddresses { get; }

		[Export ("postalAddresses", ArgumentSemantic.Copy)]
		CNLabeledValue<CNPostalAddress> [] PostalAddresses { get; }

		[Export ("urlAddresses", ArgumentSemantic.Copy)]
		CNLabeledValue<NSString> [] UrlAddresses { get; }

		[Export ("contactRelations", ArgumentSemantic.Copy)]
		CNLabeledValue<CNContactRelation> [] ContactRelations { get; }

		[Export ("socialProfiles", ArgumentSemantic.Copy)]
		CNLabeledValue<CNSocialProfile> [] SocialProfiles { get; }

		[Export ("instantMessageAddresses", ArgumentSemantic.Copy)]
		CNLabeledValue<CNInstantMessageAddress> [] InstantMessageAddresses { get; }

		[NullAllowed]
		[Export ("birthday", ArgumentSemantic.Copy)]
		NSDateComponents Birthday { get; }

		[NullAllowed]
		[Export ("nonGregorianBirthday", ArgumentSemantic.Copy)]
		NSDateComponents NonGregorianBirthday { get; }

		[Export ("dates", ArgumentSemantic.Copy)]
		CNLabeledValue<NSDateComponents> [] Dates { get; }

		[Export ("isKeyAvailable:")]
		bool IsKeyAvailable (NSString contactKey);

		// - (BOOL)areKeysAvailable:(NSArray <id<CNKeyDescriptor>>*)keyDescriptors;
		[Protected] // we cannot use ICNKeyDescriptor as Apple (and others) can adopt it from categories
		[Export ("areKeysAvailable:")]
		bool AreKeysAvailable (NSArray keyDescriptors);

		[Static]
		[Export ("localizedStringForKey:")]
		string LocalizeProperty (NSString contactKey);

		[Static]
		[Export ("comparatorForNameSortOrder:")] // Using func because names in ObjC block are obj1, obj2
		Func<NSObject, NSObject, NSComparisonResult> ComparatorForName (CNContactSortOrder sortOrder);

		[Static]
		[Export ("descriptorForAllComparatorKeys")]
		ICNKeyDescriptor GetDescriptorForAllComparatorKeys ();

		[Export ("isUnifiedWithContactWithIdentifier:")]
		bool IsUnifiedWithContact (string contactIdentifier);

		/// <summary>Used as the name of an exception thrown when attempting to read a property that has not been fetched.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactPropertyNotFetchedExceptionName")]
		NSString PropertyNotFetchedExceptionName { get; }

		// CNContact_PredicatesExtension - they should be in a [Category] but it makes
		// [Static] API hard (and ugly) to use since they become extension methods (and
		// do not look static anymore.
		// ref: https://trello.com/c/2z8FHb95/522-generator-static-members-in-category

		[Static]
		[Export ("predicateForContactsMatchingName:")]
		NSPredicate GetPredicateForContacts (string matchingName);

		[MacCatalyst (13, 1)]
		[Static]
		[Export ("predicateForContactsMatchingEmailAddress:")]
		NSPredicate GetPredicateForContactsMatchingEmailAddress (string emailAddress);

		[MacCatalyst (13, 1)]
		[Static]
		[Export ("predicateForContactsMatchingPhoneNumber:")]
		NSPredicate GetPredicateForContacts (CNPhoneNumber phoneNumber);

		[Static]
		[Export ("predicateForContactsWithIdentifiers:")]
		NSPredicate GetPredicateForContacts (string [] identifiers);

		[Static]
		[Export ("predicateForContactsInGroupWithIdentifier:")]
		NSPredicate GetPredicateForContactsInGroup (string groupIdentifier);

		[Static]
		[Export ("predicateForContactsInContainerWithIdentifier:")]
		NSPredicate GetPredicateForContactsInContainer (string containerIdentifier);
	}

	/// <summary>Provides string constants whose values are the names of the possibly-available keys for <see cref="Contacts.CNContact" /> objects.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNContactKey {

		/// <summary>Key identifying the <see cref="Contacts.CNContact.Identifier" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactIdentifierKey")]
		NSString Identifier { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.NamePrefix" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactNamePrefixKey")]
		NSString NamePrefix { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.GivenName" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactGivenNameKey")]
		NSString GivenName { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.MiddleName" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactMiddleNameKey")]
		NSString MiddleName { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.FamilyName" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactFamilyNameKey")]
		NSString FamilyName { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.PreviousFamilyName" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactPreviousFamilyNameKey")]
		NSString PreviousFamilyName { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.NameSuffix" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactNameSuffixKey")]
		NSString NameSuffix { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.Nickname" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactNicknameKey")]
		NSString Nickname { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.PhoneticGivenName" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactPhoneticGivenNameKey")]
		NSString PhoneticGivenName { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.PhoneticMiddleName" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactPhoneticMiddleNameKey")]
		NSString PhoneticMiddleName { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.PhoneticFamilyName" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactPhoneticFamilyNameKey")]
		NSString PhoneticFamilyName { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.PhoneticOrganizationName" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CNContactPhoneticOrganizationNameKey")]
		NSString PhoneticOrganizationName { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.OrganizationName" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactOrganizationNameKey")]
		NSString OrganizationName { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.DepartmentName" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactDepartmentNameKey")]
		NSString DepartmentName { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.JobTitle" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactJobTitleKey")]
		NSString JobTitle { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.Birthday" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactBirthdayKey")]
		NSString Birthday { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.NonGregorianBirthday" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactNonGregorianBirthdayKey")]
		NSString NonGregorianBirthday { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.Note" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactNoteKey")]
		NSString Note { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.ImageData" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactImageDataKey")]
		NSString ImageData { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.ImageDataAvailable" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CNContactImageDataAvailableKey")]
		NSString ImageDataAvailable { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.ThumbnailImageData" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactThumbnailImageDataKey")]
		NSString ThumbnailImageData { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.ContactType" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactTypeKey")]
		NSString Type { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.PhoneNumbers" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactPhoneNumbersKey")]
		NSString PhoneNumbers { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.EmailAddresses" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactEmailAddressesKey")]
		NSString EmailAddresses { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.PostalAddresses" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactPostalAddressesKey")]
		NSString PostalAddresses { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.Dates" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactDatesKey")]
		NSString Dates { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.UrlAddresses" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactUrlAddressesKey")]
		NSString UrlAddresses { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.Relations" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactRelationsKey")]
		NSString Relations { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.SocialProfiles" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactSocialProfilesKey")]
		NSString SocialProfiles { get; }

		/// <summary>Key identifying the <see cref="Contacts.CNContact.InstantMessageAddresses" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactInstantMessageAddressesKey")]
		NSString InstantMessageAddresses { get; }
	}

	/// <summary>Holds the parameters for a search request of the <see cref="Contacts.CNContactStore" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNContactFetchRequest_Class/index.html">Apple documentation for <c>CNContactFetchRequest</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNFetchRequest))]
	[DisableDefaultCtor] // using init raises an exception according to docs
	interface CNContactFetchRequest : NSSecureCoding {

		[DesignatedInitializer]
		[Export ("initWithKeysToFetch:")]
		[Protected] // we cannot use ICNKeyDescriptor as Apple (and others) can adopt it from categories
		NativeHandle Constructor (NSArray keysToFetch);

		[NullAllowed]
		[Export ("predicate", ArgumentSemantic.Copy)]
		NSPredicate Predicate { get; set; }

		[Export ("keysToFetch", ArgumentSemantic.Copy)]
		// we cannot use ICNKeyDescriptor as Apple (and others) can adopt it from categories
		// cannot be exposed as NSString since they could be internalized types, like CNAggregateKeyDescriptor
		NSArray KeysToFetch { get; set; }

		[MacCatalyst (13, 1)]
		[Export ("mutableObjects")]
		bool MutableObjects { get; set; }

		[Export ("unifyResults")]
		bool UnifyResults { get; set; }

		[Export ("sortOrder")]
		CNContactSortOrder SortOrder { get; set; }
	}

	/// <summary>A custom formatter for <see cref="Contacts.CNContact" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNContactFormatter_Class/index.html">Apple documentation for <c>CNContactFormatter</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSFormatter))]
	interface CNContactFormatter : NSSecureCoding {

		[Static]
		[Export ("descriptorForRequiredKeysForStyle:")]
		ICNKeyDescriptor GetDescriptorForRequiredKeys (CNContactFormatterStyle style);

		[Static]
		[return: NullAllowed]
		[Export ("stringFromContact:style:")]
		string GetStringFrom (CNContact contact, CNContactFormatterStyle style);

		[Static]
		[return: NullAllowed]
		[Export ("attributedStringFromContact:style:defaultAttributes:")]
		NSAttributedString GetAttributedStringFrom (CNContact contact, CNContactFormatterStyle style, [NullAllowed] NSDictionary attributes);

		[Static]
		[Export ("nameOrderForContact:")]
		CNContactDisplayNameOrder GetNameOrderFor (CNContact contact);

		[Static]
		[Export ("delimiterForContact:")]
		string GetDelimiterFor (CNContact contact);

		[Export ("style")]
		CNContactFormatterStyle Style { get; set; }

		[return: NullAllowed]
		[Export ("stringFromContact:")]
		string GetString (CNContact contact);

		[return: NullAllowed]
		[Export ("attributedStringFromContact:defaultAttributes:")]
		NSAttributedString GetAttributedString (CNContact contact, [NullAllowed] NSDictionary attributes);

		/// <summary>A string constant whose value is returned in calls to <see cref="Contacts.CNContactFormatter.GetAttributedString(Contacts.CNContact,Foundation.NSDictionary)" /> and <see cref="Contacts.CNContactFormatter.GetAttributedStringFrom(Contacts.CNContact,Contacts.CNContactFormatterStyle,Foundation.NSDictionary)" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContactPropertyAttribute")]
		NSString ContactPropertyAttribute { get; }

		[Static]
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("descriptorForRequiredKeysForDelimiter")]
		ICNKeyDescriptor RequiredKeysForDelimiter { get; }

		[Static]
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Export ("descriptorForRequiredKeysForNameOrder")]
		ICNKeyDescriptor RequiredKeysForNameOrder { get; }
	}

	/// <summary>A tuple of values for a contact property, including the contact, the property's key and value and, for labeled values, the identifier and label.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNContactProperty_Class/index.html">Apple documentation for <c>CNContactProperty</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNContactProperty : NSCopying, NSSecureCoding {

		[Export ("contact", ArgumentSemantic.Copy)]
		CNContact Contact { get; }

		[Export ("key")]
		string Key { get; }

		[NullAllowed]
		[Export ("value")]
		NSObject Value { get; }

		[NullAllowed]
		[Export ("identifier")]
		string Identifier { get; }

		[NullAllowed]
		[Export ("label")]
		string Label { get; }
	}

	/// <summary>Defines a relationship between two <see cref="Contacts.CNContact" /> objects, as specified by a <see cref="Contacts.CNLabelContactRelationKey" />.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNContactRelation_Class/index.html">Apple documentation for <c>CNContactRelation</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNContactRelation : NSCopying, NSSecureCoding, INSCopying, INSSecureCoding {

		[Static]
		[Export ("contactRelationWithName:")]
		CNContactRelation FromName (string name);

		[Export ("initWithName:")]
		NativeHandle Constructor (string name);

		[Export ("name")]
		string Name { get; }
	}

	/// <summary>Defines string constants whose values define various interpersonal relationships.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNLabelContactRelationKey {

		/// <summary>Father.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationFather")]
		NSString Father { get; }

		/// <summary>Mother.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationMother")]
		NSString Mother { get; }

		/// <summary>Parent.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationParent")]
		NSString Parent { get; }

		/// <summary>Brother.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationBrother")]
		NSString Brother { get; }

		/// <summary>Sister.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationSister")]
		NSString Sister { get; }

		/// <summary>Child.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationChild")]
		NSString Child { get; }

		/// <summary>Just friends.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationFriend")]
		NSString Friend { get; }

		/// <summary>Spouse.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationSpouse")]
		NSString Spouse { get; }

		/// <summary>Partner. (see also <see cref="Contacts.CNLabelContactRelationKey.Spouse" />)</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationPartner")]
		NSString Partner { get; }

		/// <summary>Professional assistant.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationAssistant")]
		NSString Assistant { get; }

		/// <summary>Manager.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationManager")]
		NSString Manager { get; }

		/// <summary>Son.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelContactRelationSon")]
		[MacCatalyst (13, 1)]
		NSString Son { get; }

		/// <summary>Daughter.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationDaughter")]
		NSString Daughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationColleague")]
		NSString Colleague { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationTeacher")]
		NSString Teacher { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSibling")]
		NSString Sibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerSibling")]
		NSString YoungerSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderSibling")]
		NSString ElderSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerSister")]
		NSString YoungerSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungestSister")]
		NSString YoungestSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderSister")]
		NSString ElderSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationEldestSister")]
		NSString EldestSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerBrother")]
		NSString YoungerBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungestBrother")]
		NSString YoungestBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderBrother")]
		NSString ElderBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationEldestBrother")]
		NSString EldestBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationMaleFriend")]
		NSString MaleFriend { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationFemaleFriend")]
		NSString FemaleFriend { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationWife")]
		NSString Wife { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationHusband")]
		NSString Husband { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationMalePartner")]
		NSString MalePartner { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationFemalePartner")]
		NSString FemalePartner { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGirlfriendOrBoyfriend")]
		NSString GirlfriendOrBoyfriend { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGirlfriend")]
		NSString Girlfriend { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationBoyfriend")]
		NSString Boyfriend { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandparent")]
		NSString Grandparent { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandmother")]
		NSString Grandmother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandmotherMothersMother")]
		NSString GrandmotherMothersMother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandmotherFathersMother")]
		NSString GrandmotherFathersMother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandfather")]
		NSString Grandfather { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandfatherMothersFather")]
		NSString GrandfatherMothersFather { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandfatherFathersFather")]
		NSString GrandfatherFathersFather { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGreatGrandparent")]
		NSString GreatGrandparent { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGreatGrandmother")]
		NSString GreatGrandmother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGreatGrandfather")]
		NSString GreatGrandfather { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandchild")]
		NSString Grandchild { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGranddaughter")]
		NSString Granddaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGranddaughterDaughtersDaughter")]
		NSString GranddaughterDaughtersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGranddaughterSonsDaughter")]
		NSString GranddaughterSonsDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandson")]
		NSString Grandson { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandsonDaughtersSon")]
		NSString GrandsonDaughtersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandsonSonsSon")]
		NSString GrandsonSonsSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGreatGrandchild")]
		NSString GreatGrandchild { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGreatGranddaughter")]
		NSString GreatGranddaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGreatGrandson")]
		NSString GreatGrandson { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationParentInLaw")]
		NSString ParentInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationMotherInLaw")]
		NSString MotherInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationMotherInLawWifesMother")]
		NSString MotherInLawWifesMother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationMotherInLawHusbandsMother")]
		NSString MotherInLawHusbandsMother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationFatherInLaw")]
		NSString FatherInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationFatherInLawWifesFather")]
		NSString FatherInLawWifesFather { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationFatherInLawHusbandsFather")]
		NSString FatherInLawHusbandsFather { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCoParentInLaw")]
		NSString CoParentInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCoMotherInLaw")]
		NSString CoMotherInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCoFatherInLaw")]
		NSString CoFatherInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSiblingInLaw")]
		NSString SiblingInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerSiblingInLaw")]
		NSString YoungerSiblingInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderSiblingInLaw")]
		NSString ElderSiblingInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSisterInLaw")]
		NSString SisterInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerSisterInLaw")]
		NSString YoungerSisterInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderSisterInLaw")]
		NSString ElderSisterInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSisterInLawSpousesSister")]
		NSString SisterInLawSpousesSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSisterInLawWifesSister")]
		NSString SisterInLawWifesSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSisterInLawHusbandsSister")]
		NSString SisterInLawHusbandsSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSisterInLawBrothersWife")]
		NSString SisterInLawBrothersWife { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSisterInLawYoungerBrothersWife")]
		NSString SisterInLawYoungerBrothersWife { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSisterInLawElderBrothersWife")]
		NSString SisterInLawElderBrothersWife { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationBrotherInLaw")]
		NSString BrotherInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerBrotherInLaw")]
		NSString YoungerBrotherInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderBrotherInLaw")]
		NSString ElderBrotherInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationBrotherInLawSpousesBrother")]
		NSString BrotherInLawSpousesBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationBrotherInLawHusbandsBrother")]
		NSString BrotherInLawHusbandsBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationBrotherInLawWifesBrother")]
		NSString BrotherInLawWifesBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationBrotherInLawSistersHusband")]
		NSString BrotherInLawSistersHusband { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationBrotherInLawYoungerSistersHusband")]
		NSString BrotherInLawYoungerSistersHusband { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationBrotherInLawElderSistersHusband")]
		NSString BrotherInLawElderSistersHusband { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSisterInLawWifesBrothersWife")]
		NSString SisterInLawWifesBrothersWife { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSisterInLawHusbandsBrothersWife")]
		NSString SisterInLawHusbandsBrothersWife { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationBrotherInLawWifesSistersHusband")]
		NSString BrotherInLawWifesSistersHusband { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationBrotherInLawHusbandsSistersHusband")]
		NSString BrotherInLawHusbandsSistersHusband { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCoSiblingInLaw")]
		NSString CoSiblingInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCoSisterInLaw")]
		NSString CoSisterInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCoBrotherInLaw")]
		NSString CoBrotherInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationChildInLaw")]
		NSString ChildInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationDaughterInLaw")]
		NSString DaughterInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSonInLaw")]
		NSString SonInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousin")]
		NSString Cousin { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousin")]
		NSString YoungerCousin { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousin")]
		NSString ElderCousin { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationMaleCousin")]
		NSString MaleCousin { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationFemaleCousin")]
		NSString FemaleCousin { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinParentsSiblingsChild")]
		NSString CousinParentsSiblingsChild { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinParentsSiblingsSon")]
		NSString CousinParentsSiblingsSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinParentsSiblingsSon")]
		NSString YoungerCousinParentsSiblingsSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinParentsSiblingsSon")]
		NSString ElderCousinParentsSiblingsSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinParentsSiblingsDaughter")]
		NSString CousinParentsSiblingsDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinParentsSiblingsDaughter")]
		NSString YoungerCousinParentsSiblingsDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinParentsSiblingsDaughter")]
		NSString ElderCousinParentsSiblingsDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinMothersSistersDaughter")]
		NSString CousinMothersSistersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinMothersSistersDaughter")]
		NSString YoungerCousinMothersSistersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinMothersSistersDaughter")]
		NSString ElderCousinMothersSistersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinMothersSistersSon")]
		NSString CousinMothersSistersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinMothersSistersSon")]
		NSString YoungerCousinMothersSistersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinMothersSistersSon")]
		NSString ElderCousinMothersSistersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinMothersBrothersDaughter")]
		NSString CousinMothersBrothersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinMothersBrothersDaughter")]
		NSString YoungerCousinMothersBrothersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinMothersBrothersDaughter")]
		NSString ElderCousinMothersBrothersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinMothersBrothersSon")]
		NSString CousinMothersBrothersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinMothersBrothersSon")]
		NSString YoungerCousinMothersBrothersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinMothersBrothersSon")]
		NSString ElderCousinMothersBrothersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinFathersSistersDaughter")]
		NSString CousinFathersSistersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinFathersSistersDaughter")]
		NSString YoungerCousinFathersSistersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinFathersSistersDaughter")]
		NSString ElderCousinFathersSistersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinFathersSistersSon")]
		NSString CousinFathersSistersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinFathersSistersSon")]
		NSString YoungerCousinFathersSistersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinFathersSistersSon")]
		NSString ElderCousinFathersSistersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinFathersBrothersDaughter")]
		NSString CousinFathersBrothersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinFathersBrothersDaughter")]
		NSString YoungerCousinFathersBrothersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinFathersBrothersDaughter")]
		NSString ElderCousinFathersBrothersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinFathersBrothersSon")]
		NSString CousinFathersBrothersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinFathersBrothersSon")]
		NSString YoungerCousinFathersBrothersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinFathersBrothersSon")]
		NSString ElderCousinFathersBrothersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinGrandparentsSiblingsChild")]
		NSString CousinGrandparentsSiblingsChild { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinGrandparentsSiblingsDaughter")]
		NSString CousinGrandparentsSiblingsDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinGrandparentsSiblingsSon")]
		NSString CousinGrandparentsSiblingsSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinMothersSiblingsSonOrFathersSistersSon")]
		NSString YoungerCousinMothersSiblingsSonOrFathersSistersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinMothersSiblingsSonOrFathersSistersSon")]
		NSString ElderCousinMothersSiblingsSonOrFathersSistersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationYoungerCousinMothersSiblingsDaughterOrFathersSistersDaughter")]
		NSString YoungerCousinMothersSiblingsDaughterOrFathersSistersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationElderCousinMothersSiblingsDaughterOrFathersSistersDaughter")]
		NSString ElderCousinMothersSiblingsDaughterOrFathersSistersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationParentsSibling")]
		NSString ParentsSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationParentsYoungerSibling")]
		NSString ParentsYoungerSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationParentsElderSibling")]
		NSString ParentsElderSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationParentsSiblingMothersSibling")]
		NSString ParentsSiblingMothersSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationParentsSiblingMothersYoungerSibling")]
		NSString ParentsSiblingMothersYoungerSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationParentsSiblingMothersElderSibling")]
		NSString ParentsSiblingMothersElderSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationParentsSiblingFathersSibling")]
		NSString ParentsSiblingFathersSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationParentsSiblingFathersYoungerSibling")]
		NSString ParentsSiblingFathersYoungerSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationParentsSiblingFathersElderSibling")]
		NSString ParentsSiblingFathersElderSibling { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAunt")]
		NSString Aunt { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntParentsSister")]
		NSString AuntParentsSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntParentsYoungerSister")]
		NSString AuntParentsYoungerSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntParentsElderSister")]
		NSString AuntParentsElderSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntFathersSister")]
		NSString AuntFathersSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntFathersYoungerSister")]
		NSString AuntFathersYoungerSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntFathersElderSister")]
		NSString AuntFathersElderSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntFathersBrothersWife")]
		NSString AuntFathersBrothersWife { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntFathersYoungerBrothersWife")]
		NSString AuntFathersYoungerBrothersWife { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntFathersElderBrothersWife")]
		NSString AuntFathersElderBrothersWife { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntMothersSister")]
		NSString AuntMothersSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntMothersYoungerSister")]
		NSString AuntMothersYoungerSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntMothersElderSister")]
		NSString AuntMothersElderSister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationAuntMothersBrothersWife")]
		NSString AuntMothersBrothersWife { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandaunt")]
		NSString Grandaunt { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncle")]
		NSString Uncle { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleParentsBrother")]
		NSString UncleParentsBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleParentsYoungerBrother")]
		NSString UncleParentsYoungerBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleParentsElderBrother")]
		NSString UncleParentsElderBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleMothersBrother")]
		NSString UncleMothersBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleMothersYoungerBrother")]
		NSString UncleMothersYoungerBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleMothersElderBrother")]
		NSString UncleMothersElderBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleMothersSistersHusband")]
		NSString UncleMothersSistersHusband { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleFathersBrother")]
		NSString UncleFathersBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleFathersYoungerBrother")]
		NSString UncleFathersYoungerBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleFathersElderBrother")]
		NSString UncleFathersElderBrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleFathersSistersHusband")]
		NSString UncleFathersSistersHusband { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleFathersYoungerSistersHusband")]
		NSString UncleFathersYoungerSistersHusband { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationUncleFathersElderSistersHusband")]
		NSString UncleFathersElderSistersHusband { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGranduncle")]
		NSString Granduncle { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSiblingsChild")]
		NSString SiblingsChild { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNiece")]
		NSString Niece { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNieceSistersDaughter")]
		NSString NieceSistersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNieceBrothersDaughter")]
		NSString NieceBrothersDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNieceSistersDaughterOrWifesSiblingsDaughter")]
		NSString NieceSistersDaughterOrWifesSiblingsDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNieceBrothersDaughterOrHusbandsSiblingsDaughter")]
		NSString NieceBrothersDaughterOrHusbandsSiblingsDaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNephew")]
		NSString Nephew { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNephewSistersSon")]
		NSString NephewSistersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNephewBrothersSon")]
		NSString NephewBrothersSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNephewBrothersSonOrHusbandsSiblingsSon")]
		NSString NephewBrothersSonOrHusbandsSiblingsSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNephewSistersSonOrWifesSiblingsSon")]
		NSString NephewSistersSonOrWifesSiblingsSon { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandniece")]
		NSString Grandniece { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandnieceSistersGranddaughter")]
		NSString GrandnieceSistersGranddaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandnieceBrothersGranddaughter")]
		NSString GrandnieceBrothersGranddaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandnephew")]
		NSString Grandnephew { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandnephewSistersGrandson")]
		NSString GrandnephewSistersGrandson { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandnephewBrothersGrandson")]
		NSString GrandnephewBrothersGrandson { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationStepparent")]
		NSString Stepparent { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationStepfather")]
		NSString Stepfather { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationStepmother")]
		NSString Stepmother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationStepchild")]
		NSString Stepchild { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationStepson")]
		NSString Stepson { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationStepdaughter")]
		NSString Stepdaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationStepbrother")]
		NSString Stepbrother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationStepsister")]
		NSString Stepsister { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationMotherInLawOrStepmother")]
		NSString MotherInLawOrStepmother { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationFatherInLawOrStepfather")]
		NSString FatherInLawOrStepfather { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationDaughterInLawOrStepdaughter")]
		NSString DaughterInLawOrStepdaughter { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSonInLawOrStepson")]
		NSString SonInLawOrStepson { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationCousinOrSiblingsChild")]
		NSString CousinOrSiblingsChild { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNieceOrCousin")]
		NSString NieceOrCousin { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationNephewOrCousin")]
		NSString NephewOrCousin { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGrandchildOrSiblingsChild")]
		NSString GrandchildOrSiblingsChild { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationGreatGrandchildOrSiblingsGrandchild")]
		NSString GreatGrandchildOrSiblingsGrandchild { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationDaughterInLawOrSisterInLaw")]
		NSString DaughterInLawOrSisterInLaw { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelContactRelationSonInLawOrBrotherInLaw")]
		NSString SonInLawOrBrotherInLaw { get; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("CNLabelContactRelationGranddaughterOrNiece")]
		NSString GranddaughterOrNiece { get; }

		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("CNLabelContactRelationGrandsonOrNephew")]
		NSString GrandsonOrNephew { get; }

	}

	/// <summary>Completion handler for calls to <see cref="Contacts.CNContactStore.RequestAccess(Contacts.CNEntityType,Contacts.CNContactStoreRequestAccessHandler)" /></summary>
	delegate void CNContactStoreRequestAccessHandler (bool granted, [NullAllowed] NSError error);
	delegate void CNContactStoreListContactsHandler (CNContact contact, ref bool stop);

	interface ICNChangeHistoryEventVisitor { }

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[Protocol]
	interface CNChangeHistoryEventVisitor {
		[Abstract]
		[Export ("visitDropEverythingEvent:")]
		void DropEverything (CNChangeHistoryDropEverythingEvent @event);

		[Abstract]
		[Export ("visitAddContactEvent:")]
		void AddContact (CNChangeHistoryAddContactEvent @event);

		[Abstract]
		[Export ("visitUpdateContactEvent:")]
		void UpdateContact (CNChangeHistoryUpdateContactEvent @event);

		[Abstract]
		[Export ("visitDeleteContactEvent:")]
		void DeleteContact (CNChangeHistoryDeleteContactEvent @event);

		[Export ("visitAddGroupEvent:")]
		void AddGroup (CNChangeHistoryAddGroupEvent @event);

		[Export ("visitUpdateGroupEvent:")]
		void UpdateGroup (CNChangeHistoryUpdateGroupEvent @event);

		[Export ("visitDeleteGroupEvent:")]
		void DeleteGroup (CNChangeHistoryDeleteGroupEvent @event);

		[Export ("visitAddMemberToGroupEvent:")]
		void AddMemberToGroup (CNChangeHistoryAddMemberToGroupEvent @event);

		[Export ("visitRemoveMemberFromGroupEvent:")]
		void RemoveMemberFromGroup (CNChangeHistoryRemoveMemberFromGroupEvent @event);

		[Export ("visitAddSubgroupToGroupEvent:")]
		void AddSubgroupToGroup (CNChangeHistoryAddSubgroupToGroupEvent @event);

		[Export ("visitRemoveSubgroupFromGroupEvent:")]
		void RemoveSubgroupFromGroup (CNChangeHistoryRemoveSubgroupFromGroupEvent @event);
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNChangeHistoryEvent : NSCopying, NSSecureCoding {
		[Export ("acceptEventVisitor:")]
		void AcceptEventVisitor (ICNChangeHistoryEventVisitor visitor);
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	interface CNChangeHistoryDropEverythingEvent { }

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	[DisableDefaultCtor]
	interface CNChangeHistoryAddContactEvent {
		[Export ("contact", ArgumentSemantic.Strong)]
		CNContact Contact { get; }

		[NullAllowed, Export ("containerIdentifier", ArgumentSemantic.Strong)]
		string ContainerIdentifier { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	[DisableDefaultCtor]
	interface CNChangeHistoryUpdateContactEvent {
		[Export ("contact", ArgumentSemantic.Strong)]
		CNContact Contact { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	[DisableDefaultCtor]
	interface CNChangeHistoryDeleteContactEvent {
		[Export ("contactIdentifier", ArgumentSemantic.Strong)]
		string ContactIdentifier { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	[DisableDefaultCtor]
	interface CNChangeHistoryAddGroupEvent {
		[Export ("group", ArgumentSemantic.Strong)]
		CNGroup Group { get; }

		[Export ("containerIdentifier", ArgumentSemantic.Strong)]
		string ContainerIdentifier { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	[DisableDefaultCtor]
	interface CNChangeHistoryUpdateGroupEvent {
		[Export ("group", ArgumentSemantic.Strong)]
		CNGroup Group { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	[DisableDefaultCtor]
	interface CNChangeHistoryDeleteGroupEvent {
		[Export ("groupIdentifier", ArgumentSemantic.Strong)]
		string GroupIdentifier { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	[DisableDefaultCtor]
	interface CNChangeHistoryAddMemberToGroupEvent {
		[Export ("member", ArgumentSemantic.Strong)]
		CNContact Member { get; }

		[Export ("group", ArgumentSemantic.Strong)]
		CNGroup Group { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	[DisableDefaultCtor]
	interface CNChangeHistoryRemoveMemberFromGroupEvent {
		[Export ("member", ArgumentSemantic.Strong)]
		CNContact Member { get; }

		[Export ("group", ArgumentSemantic.Strong)]
		CNGroup Group { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	[DisableDefaultCtor]
	interface CNChangeHistoryAddSubgroupToGroupEvent {
		[Export ("subgroup", ArgumentSemantic.Strong)]
		CNGroup Subgroup { get; }

		[Export ("group", ArgumentSemantic.Strong)]
		CNGroup Group { get; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNChangeHistoryEvent))]
	[DisableDefaultCtor]
	interface CNChangeHistoryRemoveSubgroupFromGroupEvent {
		[Export ("subgroup", ArgumentSemantic.Strong)]
		CNGroup Subgroup { get; }

		[Export ("group", ArgumentSemantic.Strong)]
		CNGroup Group { get; }
	}

	// this type is new in Xcode11 but is decorated with earlier versions since it's used as a
	// base type for older types (and that confuse the generator for 32bits availability)
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNFetchRequest { }

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNFetchRequest))]
	interface CNChangeHistoryFetchRequest : NSSecureCoding {
		[NullAllowed, Export ("startingToken", ArgumentSemantic.Copy)]
		NSData StartingToken { get; set; }

		[NullAllowed, Export ("additionalContactKeyDescriptors", ArgumentSemantic.Copy)]
		ICNKeyDescriptor [] AdditionalContactKeyDescriptors { get; set; }

		[Export ("shouldUnifyResults")]
		bool ShouldUnifyResults { get; set; }

		[Export ("mutableObjects")]
		bool MutableObjects { get; set; }

		[Export ("includeGroupChanges")]
		bool IncludeGroupChanges { get; set; }

		[NullAllowed]
		[Export ("excludedTransactionAuthors", ArgumentSemantic.Copy)]
		string [] ExcludedTransactionAuthors { get; set; }
	}

	[iOS (13, 0)]
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface CNFetchResult<T> {
		[Export ("value", ArgumentSemantic.Strong)]
		NSObject Value { get; }

		[Export ("currentHistoryToken", ArgumentSemantic.Copy)]
		NSData CurrentHistoryToken { get; }
	}

	/// <summary>The system's contact database.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNContactStore_Class/index.html">Apple documentation for <c>CNContactStore</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNContactStore {

		[Static]
		[Export ("authorizationStatusForEntityType:")]
		CNAuthorizationStatus GetAuthorizationStatus (CNEntityType entityType);

		[Async (XmlDocs = """
			<param name="entityType">To be added.</param>
			<summary>Requests access to the user's contacts.</summary>
			<returns>
			          <para>A task that represents the asynchronous RequestAccess operation.   The value of the TResult parameter is a Contacts.CNContactStoreRequestAccessHandler.</para>
			        </returns>
			<remarks>
			          <para copied="true">The RequestAccessAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		[Export ("requestAccessForEntityType:completionHandler:")]
		void RequestAccess (CNEntityType entityType, CNContactStoreRequestAccessHandler completionHandler);

		[Export ("unifiedContactsMatchingPredicate:keysToFetch:error:")]
		[Protected] // we cannot use ICNKeyDescriptor as Apple (and others) can adopt it from categories
		[return: NullAllowed]
		CNContact [] GetUnifiedContacts (NSPredicate predicate, NSArray keys, [NullAllowed] out NSError error);

		[Export ("unifiedContactWithIdentifier:keysToFetch:error:")]
		[Protected] // we cannot use ICNKeyDescriptor as Apple (and others) can adopt it from categories
		[return: NullAllowed]
		CNContact GetUnifiedContact (string identifier, NSArray keys, [NullAllowed] out NSError error);

		[NoiOS]
		[NoMacCatalyst]
		[Export ("unifiedMeContactWithKeysToFetch:error:")]
		[Protected] // we cannot use ICNKeyDescriptor as Apple (and others) can adopt it from categories
		[return: NullAllowed]
		NSObject GetUnifiedMeContact (NSArray keys, [NullAllowed] out NSError error);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("enumeratorForContactFetchRequest:error:")]
		[return: NullAllowed]
		CNFetchResult<NSEnumerator<CNContact>> GetEnumeratorForContact (CNContactFetchRequest request, [NullAllowed] out NSError error);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Export ("enumeratorForChangeHistoryFetchRequest:error:")]
		[return: NullAllowed]
		CNFetchResult<NSEnumerator<CNChangeHistoryEvent>> GetEnumeratorForChangeHistory (CNChangeHistoryFetchRequest request, [NullAllowed] out NSError error);

		[Export ("enumerateContactsWithFetchRequest:error:usingBlock:")]
		bool EnumerateContacts (CNContactFetchRequest fetchRequest, [NullAllowed] out NSError error, CNContactStoreListContactsHandler handler);

		[Export ("groupsMatchingPredicate:error:")]
		[return: NullAllowed]
		CNGroup [] GetGroups ([NullAllowed] NSPredicate predicate, [NullAllowed] out NSError error);

		[Export ("containersMatchingPredicate:error:")]
		[return: NullAllowed]
		CNContainer [] GetContainers ([NullAllowed] NSPredicate predicate, [NullAllowed] out NSError error);

		[Export ("executeSaveRequest:error:")]
		[return: NullAllowed]
		bool ExecuteSaveRequest (CNSaveRequest saveRequest, [NullAllowed] out NSError error);

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[NullAllowed]
		[Export ("currentHistoryToken", ArgumentSemantic.Copy)]
		NSData CurrentHistoryToken { get; }

		[Export ("defaultContainerIdentifier")]
		[NullAllowed]
		string DefaultContainerIdentifier { get; }

		/// <include file="../docs/api/Contacts/CNContactStore.xml" path="/Documentation/Docs[@DocId='P:Contacts.CNContactStore.NotificationDidChange']/*" />
		[Notification]
		[Field ("CNContactStoreDidChangeNotification")]
		NSString NotificationDidChange { get; }
	}

	/// <summary>Holds default values, such as <see cref="Contacts.CNContactsUserDefaults.CountryCode" />, for <see cref="Contacts.CNContact" /> objects.</summary>
	///     <remarks>
	///       <para>(More documentation for this node is coming)</para>
	///       <para tool="threads">The members of this class can be used from a background thread.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/reference/Contacts/CNContactsUserDefaults">Apple documentation for <c>CNContactsUserDefaults</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[ThreadSafe (false)]
	interface CNContactsUserDefaults {

		[Static]
		[Export ("sharedDefaults")]
		CNContactsUserDefaults GetSharedDefaults ();

		[Export ("sortOrder")]
		CNContactSortOrder SortOrder { get; }

		[Export ("countryCode")]
		string CountryCode { get; }
	}

	/// <summary>Provides vCard serialization for <see cref="Contacts.CNContact" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNContactVCardSerialization_Class/index.html">Apple documentation for <c>CNContactVCardSerialization</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNContactVCardSerialization {

		[Static]
		[Export ("descriptorForRequiredKeys")]
		ICNKeyDescriptor GetDescriptorFromRequiredKeys ();

		[Static]
		[return: NullAllowed]
		[Export ("dataWithContacts:error:")]
		NSData GetDataFromContacts (CNContact [] contacts, out NSError error);

		[Static]
		[return: NullAllowed]
		[Export ("contactsWithData:error:")]
		CNContact [] GetContactsFromData (NSData data, out NSError error);
	}

	/// <summary>An object such as an Exchange or CalDAV account that contains zero or more <see cref="Contacts.CNContact" /> objects.</summary>
	///     <remarks>
	///       <para> A <see cref="Contacts.CNContact" /> may be a member of only one <see cref="Contacts.CNContainer" />. This is in contrast to <see cref="Contacts.CNGroup" /> objects.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNContainer_Class/index.html">Apple documentation for <c>CNContainer</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNContainer : NSCopying, NSSecureCoding {

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("name")]
		string Name { get; }

		[Export ("type", ArgumentSemantic.Assign)]
		CNContainerType ContainerType { get; }

		#region comes from CNContainer (Predicates) Category
		[Static]
		[Export ("predicateForContainersWithIdentifiers:")]
		NSPredicate CreatePredicateForContainers (string [] identifiers);

		[Static]
		[Export ("predicateForContainerOfContactWithIdentifier:")]
		NSPredicate CreatePredicateForContainerOfContact (string contactIdentifier);

		[Static]
		[Export ("predicateForContainerOfGroupWithIdentifier:")]
		NSPredicate CreatePredicateForContainerOfGroup (string groupIdentifier);
		#endregion
	}

	/// <summary>Provides string constants whose values should be used as keys when referencing properties of <see cref="Contacts.CNContainer" /> objects.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNContainerKey { // Can be used in KVO

		/// <summary>The key for the <see cref="Contacts.CNContainer.Identifier" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContainerIdentifierKey")]
		NSString Identifier { get; }

		/// <summary>The key for the <see cref="Contacts.CNContainer.Name" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContainerNameKey")]
		NSString Name { get; }

		/// <summary>The key for the <see cref="Contacts.CNContainer.ContainerType" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNContainerTypeKey")]
		NSString Type { get; }
	}

	/// <summary>Provides string constants whose values identify the form of an error.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNErrorUserInfoKey {

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNErrorUserInfoAffectedRecordsKey")]
		NSString AffectedRecords { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNErrorUserInfoAffectedRecordIdentifiersKey")]
		NSString AffectedRecordIdentifiers { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNErrorUserInfoValidationErrorsKey")]
		NSString ValidationErrors { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNErrorUserInfoKeyPathsKey")]
		NSString KeyPaths { get; }
	}

	/// <summary>A group that contains <see cref="Contacts.CNContact" /> objects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNGroup_Class/index.html">Apple documentation for <c>CNGroup</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNGroup : NSCopying, NSMutableCopying, NSSecureCoding {

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("name")]
		string Name { get; }

		#region comes from CNGroup (Predicates) Category
		[Static]
		[Export ("predicateForGroupsWithIdentifiers:")]
		NSPredicate CreatePredicateForGroups (string [] identifiers);

		[NoiOS]
		[NoMacCatalyst]
		[Static]
		[Export ("predicateForSubgroupsInGroupWithIdentifier:")]
		NSPredicate CreatePredicateForSubgroupsInGroup (string parentGroupIdentifier);

		[Static]
		[Export ("predicateForGroupsInContainerWithIdentifier:")]
		NSPredicate CreatePredicateForGroupsInContainer (string containerIdentifier);
		#endregion
	}

	/// <summary>Provides string constants whose values are the names of properties common to all <see cref="Contacts.CNGroup" /> objects.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNGroupKey { // Can be used in KVO

		/// <summary>Key for the <see cref="Contacts.CNGroup.Identifier" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNGroupIdentifierKey")]
		NSString Identifier { get; }

		/// <summary>Key for the <see cref="Contacts.CNGroup.Name" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNGroupNameKey")]
		NSString Name { get; }
	}

	/// <summary>Defines an address for an instant-message service.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNInstantMessageAddress_Class/index.html">Apple documentation for <c>CNInstantMessageAddress</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNInstantMessageAddress : NSCopying, NSSecureCoding, INSCopying, INSSecureCoding {

		[Export ("initWithUsername:service:")]
		NativeHandle Constructor (string username, string service);

		[Export ("username")]
		string Username { get; }

		[Export ("service")]
		string Service { get; }

		[Static]
		[Export ("localizedStringForKey:")]
		string LocalizeProperty (NSString propertyKey);

		[Static]
		[Export ("localizedStringForService:")]
		string LocalizeService (NSString service);
	}

	/// <summary>Provides string constants whose values are the common properties of all instant-message providers.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNInstantMessageAddressKey { // Can be used in KVO

		/// <summary>A key identifying the <see cref="Contacts.CNInstantMessageAddress.Username" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageAddressUsernameKey")]
		NSString Username { get; }

		/// <summary>A key identifying the <see cref="Contacts.CNInstantMessageAddress.Service" /> property.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageAddressServiceKey")]
		NSString Service { get; }
	}

	/// <summary>Provides string constants whose values are the names of common providers of instant messaging services.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNInstantMessageServiceKey {

		/// <summary>AOL Instant Messenger.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageServiceAIM")]
		NSString Aim { get; }

		/// <summary>Facebook Messenger.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageServiceFacebook")]
		NSString Facebook { get; }

		/// <summary>Gadu-Gadu (Polish).</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageServiceGaduGadu")]
		NSString GaduGadu { get; }

		/// <summary>Google Talk.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageServiceGoogleTalk")]
		NSString GoogleTalk { get; }

		/// <summary>ICQ.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageServiceICQ")]
		NSString Icq { get; }

		/// <summary>Jabber.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageServiceJabber")]
		NSString Jabber { get; }

		/// <summary>MSN Messenger.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageServiceMSN")]
		NSString Msn { get; }

		/// <summary>Tencent QQ.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageServiceQQ")]
		NSString QQ { get; }

		/// <summary>Microsoft Skype.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageServiceSkype")]
		NSString Skype { get; }

		/// <summary>Yahoo Messenger.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNInstantMessageServiceYahoo")]
		NSString Yahoo { get; }
	}

	/// <summary>An object that holds a value and the label for that value.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNLabeledValue_Class/index.html">Apple documentation for <c>CNLabeledValue</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNLabeledValue<ValueType> : NSCopying, NSSecureCoding
		where ValueType : INSCopying, INSSecureCoding {

		[Export ("identifier")]
		string Identifier { get; }

		[NullAllowed]
		[Export ("label")]
		string Label { get; }

		[Export ("value", ArgumentSemantic.Copy)]
		ValueType Value { get; }

		[Static]
		[Export ("labeledValueWithLabel:value:")]
		ValueType FromLabel ([NullAllowed] string label, ValueType value);

		[Export ("initWithLabel:value:")]
		NativeHandle Constructor ([NullAllowed] string label, ValueType value);

		[Export ("labeledValueBySettingLabel:")]
		ValueType GetLabeledValue ([NullAllowed] string label);

		[Export ("labeledValueBySettingValue:")]
		ValueType GetLabeledValue (ValueType value);

		[Export ("labeledValueBySettingLabel:value:")]
		ValueType GetLabeledValue ([NullAllowed] string label, ValueType value);

		// TODO: Enumify this method, it seems to accept CNLabelKey, CNLabelContactRelationKey and CNLabelPhoneNumberKey unsure if it takes random user values
		[Static]
		[Export ("localizedStringForLabel:")]
		string LocalizeLabel (NSString labelKey);
	}

	/// <summary>Defines string constants whose values define the names of various <see cref="Contacts.CNLabeledValue{ValueType}" /> objects.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNLabelKey {

		/// <summary>The home of the user (as defined in the contact store).</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelHome")]
		NSString Home { get; }

		/// <summary>The user's work address (as defined in the contacts store).</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelWork")]
		NSString Work { get; }

		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		[Field ("CNLabelSchool")]
		NSString School { get; }

		/// <summary>A <see cref="Contacts.CNLabeledValue{ValueType}" /> with an unknown label.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelOther")]
		NSString Other { get; }

		/// <summary>An iCloud email.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelEmailiCloud")]
		NSString EmailiCloud { get; }

		/// <summary>The user's home page.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelURLAddressHomePage")]
		NSString UrlAddressHomePage { get; }

		/// <summary>A labeled anniversary.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelDateAnniversary")]
		NSString DateAnniversary { get; }
	}

	/// <summary>A <see cref="Contacts.CNContact" /> that can be modified after creation.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNMutableContact_Class/index.html">Apple documentation for <c>CNMutableContact</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNContact))]
	interface CNMutableContact {

		[New]
		[Export ("contactType")]
		CNContactType ContactType { get; set; }

		[New]
		[Export ("namePrefix")]
		string NamePrefix { get; set; }

		[New]
		[Export ("givenName")]
		string GivenName { get; set; }

		[New]
		[Export ("middleName")]
		string MiddleName { get; set; }

		[New]
		[Export ("familyName")]
		string FamilyName { get; set; }

		[New]
		[Export ("previousFamilyName")]
		string PreviousFamilyName { get; set; }

		[New]
		[Export ("nameSuffix")]
		string NameSuffix { get; set; }

		[New]
		[Export ("nickname")]
		string Nickname { get; set; }

		[New]
		[Export ("phoneticGivenName")]
		string PhoneticGivenName { get; set; }

		[New]
		[Export ("phoneticMiddleName")]
		string PhoneticMiddleName { get; set; }

		[New]
		[Export ("phoneticFamilyName")]
		string PhoneticFamilyName { get; set; }

		[MacCatalyst (13, 1)]
		[New]
		[Export ("phoneticOrganizationName")]
		string PhoneticOrganizationName { get; set; }

		[New]
		[Export ("organizationName")]
		string OrganizationName { get; set; }

		[New]
		[Export ("departmentName")]
		string DepartmentName { get; set; }

		[New]
		[Export ("jobTitle")]
		string JobTitle { get; set; }

		[New]
		[Export ("note")]
		string Note { get; set; }

		[New]
		[NullAllowed]
		[Export ("imageData", ArgumentSemantic.Copy)]
		NSData ImageData { get; set; }

		[New]
		[Export ("phoneNumbers", ArgumentSemantic.Copy)]
		CNLabeledValue<CNPhoneNumber> [] PhoneNumbers { get; set; }

		[New]
		[Export ("emailAddresses", ArgumentSemantic.Copy)]
		CNLabeledValue<NSString> [] EmailAddresses { get; set; }

		[New]
		[Export ("postalAddresses", ArgumentSemantic.Copy)]
		CNLabeledValue<CNPostalAddress> [] PostalAddresses { get; set; }

		[New]
		[Export ("urlAddresses", ArgumentSemantic.Copy)]
		CNLabeledValue<NSString> [] UrlAddresses { get; set; }

		[New]
		[Export ("contactRelations", ArgumentSemantic.Copy)]
		CNLabeledValue<CNContactRelation> [] ContactRelations { get; set; }

		[New]
		[Export ("socialProfiles", ArgumentSemantic.Copy)]
		CNLabeledValue<CNSocialProfile> [] SocialProfiles { get; set; }

		[New]
		[Export ("instantMessageAddresses", ArgumentSemantic.Copy)]
		CNLabeledValue<CNInstantMessageAddress> [] InstantMessageAddresses { get; set; }

		[New]
		[NullAllowed]
		[Export ("birthday", ArgumentSemantic.Copy)]
		NSDateComponents Birthday { get; set; }

		[New]
		[NullAllowed]
		[Export ("nonGregorianBirthday", ArgumentSemantic.Copy)]
		NSDateComponents NonGregorianBirthday { get; set; }

		[New]
		[Export ("dates", ArgumentSemantic.Copy)]
		CNLabeledValue<NSDateComponents> [] Dates { get; set; }
	}

	/// <summary>A <see cref="Contacts.CNGroup" /> whose values can change after initialization.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNMutableGroup_Class/index.html">Apple documentation for <c>CNMutableGroup</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNGroup))]
	interface CNMutableGroup {

		[New]
		[Export ("name")]
		string Name { get; set; }
	}

	/// <summary>A <see cref="Contacts.CNPostalAddress" /> whose values can be modified after initialization.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNMutablePostalAddress_Class/index.html">Apple documentation for <c>CNMutablePostalAddress</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (CNPostalAddress))]
	interface CNMutablePostalAddress {

		[New]
		[Export ("street")]
		string Street { get; set; }

		[MacCatalyst (13, 1)]
		[New]
		[Export ("subLocality")]
		string SubLocality { get; set; }

		[New]
		[Export ("city")]
		string City { get; set; }

		[MacCatalyst (13, 1)]
		[New]
		[Export ("subAdministrativeArea")]
		string SubAdministrativeArea { get; set; }

		[New]
		[Export ("state")]
		string State { get; set; }

		[New]
		[Export ("postalCode")]
		string PostalCode { get; set; }

		[New]
		[Export ("country")]
		string Country { get; set; }

		[New]
		[Export ("ISOCountryCode")]
		string IsoCountryCode { get; set; }
	}

	/// <summary>An immutable phone number.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNPhoneNumber_Class/index.html">Apple documentation for <c>CNPhoneNumber</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // Apple doc: no handle (nil) if no string (or nil string) is given
	interface CNPhoneNumber : NSCopying, NSSecureCoding, INSCopying, INSSecureCoding {

		[Static, Export ("phoneNumberWithStringValue:")]
		[return: NullAllowed]
		CNPhoneNumber PhoneNumberWithStringValue (string stringValue);

		[Export ("initWithStringValue:")]
		NativeHandle Constructor (string stringValue);

		[Export ("stringValue")]
		string StringValue { get; }
	}

	/// <summary>Defines string constants whose values are labels for various types of phones.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNLabelPhoneNumberKey {

		/// <summary>Not to be confused with a mere <see cref="Contacts.CNLabelPhoneNumberKey.Mobile" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelPhoneNumberiPhone")]
		NSString iPhone { get; }

		[iOS (14, 3)]
		[MacCatalyst (14, 3)]
		[Field ("CNLabelPhoneNumberAppleWatch")]
		NSString AppleWatch { get; }

		/// <summary>A mobile phone number (see <see cref="Contacts.CNLabelPhoneNumberKey.iPhone" />).</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelPhoneNumberMobile")]
		NSString Mobile { get; }

		/// <summary>The main phone number for a <see cref="Contacts.CNContact" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelPhoneNumberMain")]
		NSString Main { get; }

		/// <summary>The user's home fax number.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelPhoneNumberHomeFax")]
		NSString HomeFax { get; }

		/// <summary>Number for a fax located at work.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelPhoneNumberWorkFax")]
		NSString WorkFax { get; }

		/// <summary>A fax number (see <see cref="Contacts.CNLabelPhoneNumberKey.HomeFax" /> and <see cref="Contacts.CNLabelPhoneNumberKey.WorkFax" />).</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelPhoneNumberOtherFax")]
		NSString OtherFax { get; }

		/// <summary>A pager number.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNLabelPhoneNumberPager")]
		NSString Pager { get; }
	}

	/// <summary>A mailing address for a contact.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNPostalAddress_Class/index.html">Apple documentation for <c>CNPostalAddress</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNPostalAddress : NSCopying, NSMutableCopying, NSSecureCoding, INSCopying, INSSecureCoding {

		[Export ("street")]
		string Street { get; }

		[MacCatalyst (13, 1)]
		[Export ("subLocality")]
		string SubLocality { get; }

		[Export ("city")]
		string City { get; }

		[MacCatalyst (13, 1)]
		[Export ("subAdministrativeArea")]
		string SubAdministrativeArea { get; }

		[Export ("state")]
		string State { get; }

		[Export ("postalCode")]
		string PostalCode { get; }

		[Export ("country")]
		string Country { get; }

		[Export ("ISOCountryCode")]
		string IsoCountryCode { get; }

		[Static]
		[Export ("localizedStringForKey:")]
		string LocalizeProperty (NSString property);

		/// <param name="option">To be added.</param>
		///         <summary>The localized name for the property as modified by the option that is specified in <paramref name="option" />.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("LocalizeProperty (option.GetConstant ()!)")]
		string LocalizeProperty (CNPostalAddressKeyOption option);
	}

	/// <summary>Enumeration of properties of a <see cref="Contacts.CNPostalAddress" />.</summary>
	[MacCatalyst (13, 1)]
	public enum CNPostalAddressKeyOption {
		/// <summary>Corresponds to the <see cref="Contacts.CNPostalAddress.Street" /> property.</summary>
		[Field ("CNPostalAddressStreetKey")]
		Street,
		/// <summary>Corresponds to the <see cref="Contacts.CNPostalAddress.City" /> property.</summary>
		[Field ("CNPostalAddressCityKey")]
		City,
		/// <summary>Corresponds to the <see cref="Contacts.CNPostalAddress.State" /> property.</summary>
		[Field ("CNPostalAddressStateKey")]
		State,
		/// <summary>Corresponds to the <see cref="Contacts.CNPostalAddress.PostalCode" /> property.</summary>
		[Field ("CNPostalAddressPostalCodeKey")]
		PostalCode,
		/// <summary>Corresponds to the <see cref="Contacts.CNPostalAddress.Country" /> property.</summary>
		[Field ("CNPostalAddressCountryKey")]
		Country,
		/// <summary>Corresponds to the <see cref="Contacts.CNPostalAddress.IsoCountryCode" /> property.</summary>
		[Field ("CNPostalAddressISOCountryCodeKey")]
		IsoCountryCode,

		/// <summary>Corresponds to the <see cref="Contacts.CNPostalAddress.SubLocality" /> property.</summary>
		[MacCatalyst (13, 1)]
		[Field ("CNPostalAddressSubLocalityKey")]
		SubLocality,

		/// <summary>Corresponds to the <see cref="Contacts.CNPostalAddress.SubAdministrativeArea" /> property.</summary>
		[MacCatalyst (13, 1)]
		[Field ("CNPostalAddressSubAdministrativeAreaKey")]
		SubAdministrativeArea,
	}

	/// <summary>Formats postal addresses in the manner appropriate to the addresses.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNPostalAddressFormatter_Class/index.html">Apple documentation for <c>CNPostalAddressFormatter</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSFormatter))]
	interface CNPostalAddressFormatter {

		[Static]
		[Export ("stringFromPostalAddress:style:")]
		string GetStringFrom (CNPostalAddress postalAddress, CNPostalAddressFormatterStyle style);

		[Static]
		[Export ("attributedStringFromPostalAddress:style:withDefaultAttributes:")]
		NSAttributedString GetAttributedStringFrom (CNPostalAddress postalAddress, CNPostalAddressFormatterStyle style, NSDictionary attributes);

		[Export ("stringFromPostalAddress:")]
		string GetStringFromPostalAddress (CNPostalAddress postalAddress);

		[Export ("attributedStringFromPostalAddress:withDefaultAttributes:")]
		NSAttributedString GetAttributedStringFromPostalAddress (CNPostalAddress postalAddress, NSDictionary attributes);

		[Export ("style")]
		CNPostalAddressFormatterStyle Style { get; set; }

		/// <summary>Get the <see cref="Contacts.CNPostalAddress" /> property key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNPostalAddressPropertyAttribute")]
		NSString PropertyAttribute { get; }

		/// <summary>Returns the localized <see cref="Contacts.CNPostalAddress" /> property key.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNPostalAddressLocalizedPropertyNameAttribute")]
		NSString LocalizedPropertyNameAttribute { get; }
	}

	/// <summary>A request that performs a save operation for contacts.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNSaveRequest_Class/index.html">Apple documentation for <c>CNSaveRequest</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNSaveRequest {

		[Export ("addContact:toContainerWithIdentifier:")]
		void AddContact (CNMutableContact contact, [NullAllowed] string identifier);

		[Export ("updateContact:")]
		void UpdateContact (CNMutableContact contact);

		[Export ("deleteContact:")]
		void DeleteContact (CNMutableContact contact);

		[Export ("addGroup:toContainerWithIdentifier:")]
		void AddGroup (CNMutableGroup group, [NullAllowed] string identifier);

		[Export ("updateGroup:")]
		void UpdateGroup (CNMutableGroup group);

		[Export ("deleteGroup:")]
		void DeleteGroup (CNMutableGroup group);

		[NoiOS]
		[NoMacCatalyst]
		[Export ("addSubgroup:toGroup:")]
		void AddSubgroup (CNGroup subgroup, CNGroup group);

		[NoiOS]
		[NoMacCatalyst]
		[Export ("removeSubgroup:fromGroup:")]
		void RemoveSubgroup (CNGroup subgroup, CNGroup group);

		[Export ("addMember:toGroup:")]
		void AddMember (CNContact contact, CNGroup group);

		[Export ("removeMember:fromGroup:")]
		void RemoveMember (CNContact contact, CNGroup group);

		[iOS (15, 0), MacCatalyst (15, 0)]
		[NullAllowed, Export ("transactionAuthor")]
		string TransactionAuthor { get; set; }

		[Mac (12, 3), iOS (15, 4), MacCatalyst (15, 4)]
		[Export ("shouldRefetchContacts")]
		bool ShouldRefetchContacts { get; set; }
	}

	/// <summary>A profile for a social network, such as Facebook or Twitter.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/Contacts/Reference/CNSocialProfile_Class/index.html">Apple documentation for <c>CNSocialProfile</c></related>
	[MacCatalyst (13, 1)]
	[BaseType (typeof (NSObject))]
	interface CNSocialProfile : NSCopying, NSSecureCoding, INSCopying, INSSecureCoding {

		[Export ("urlString")]
		string UrlString { get; }

		[Export ("username")]
		string Username { get; }

		[Export ("userIdentifier")]
		string UserIdentifier { get; }

		[Export ("service")]
		string Service { get; }

		[Export ("initWithUrlString:username:userIdentifier:service:")]
		NativeHandle Constructor ([NullAllowed] string url, [NullAllowed] string username, [NullAllowed] string userIdentifier, [NullAllowed] string service);

		[Static]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		[Export ("localizedStringForKey:")]
		string LocalizeProperty (NSString key);

		/// <param name="key">To be added.</param>
		///         <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Wrap ("LocalizeProperty (key.GetConstant ()!)")]
		string LocalizeProperty (CNPostalAddressKeyOption key);

		[Static]
		[Export ("localizedStringForService:")]
		string LocalizeService (NSString service);
	}

	/// <summary>Provides string constants whose values specify the properties of social services that are always fetched.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNSocialProfileKey { // Can be used in KVO

		/// <summary>The key associated with <see cref="Contacts.CNSocialService.UrlString" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileURLStringKey")]
		NSString UrlString { get; }

		/// <summary>The key associated with <see cref="Contacts.CNSocialService.Username" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileUsernameKey")]
		NSString Username { get; }

		/// <summary>The key associated with <see cref="Contacts.CNSocialService.UserIdentifier" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileUserIdentifierKey")]
		NSString UserIdentifier { get; }

		/// <summary>The key associated with <see cref="Contacts.CNSocialProfile.Service" />.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileServiceKey")]
		NSString Service { get; }
	}

	/// <summary>Provides string constants naming known social networks.</summary>
	[MacCatalyst (13, 1)]
	[Static]
	[EditorBrowsable (EditorBrowsableState.Advanced)]
	interface CNSocialProfileServiceKey {

		/// <summary>Facebook.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileServiceFacebook")]
		NSString Facebook { get; }

		/// <summary>Flickr.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileServiceFlickr")]
		NSString Flickr { get; }

		/// <summary>LinkedIn.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileServiceLinkedIn")]
		NSString LinkedIn { get; }

		/// <summary>MySpace.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileServiceMySpace")]
		NSString MySpace { get; }

		/// <summary>SinaWeibo.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileServiceSinaWeibo")]
		NSString SinaWeibo { get; }

		/// <summary>TencentWeibo.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileServiceTencentWeibo")]
		NSString TencentWeibo { get; }

		/// <summary>Twitter.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileServiceTwitter")]
		NSString Twitter { get; }

		/// <summary>Yelp.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileServiceYelp")]
		NSString Yelp { get; }

		/// <summary>Apple GameCenter.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("CNSocialProfileServiceGameCenter")]
		NSString GameCenter { get; }
	}
}
