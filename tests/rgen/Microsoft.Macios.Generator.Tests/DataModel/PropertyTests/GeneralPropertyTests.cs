// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Macios.Generator.Attributes;
using Microsoft.Macios.Generator.DataModel;
using ObjCRuntime;
using Xunit;
using Property = Microsoft.Macios.Generator.DataModel.Property;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Microsoft.Macios.Generator.Tests.DataModel.PropertyTests;

public class GeneralPropertyTests {

	[Theory]
	[InlineData ("Name")]
	[InlineData ("Surname")]
	[InlineData ("Date")]
	public void BackingFieldStaticFieldTests (string propertyName)
	{
		var property = new Property (
			name: propertyName,
			returnType: ReturnTypeForString (),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [Token (SyntaxKind.StaticKeyword)],
			accessors: []
		) {
			ExportFieldData = new ()
		};
		Assert.Equal ($"_{propertyName}", property.BackingField);
	}

	[Theory]
	[InlineData ("Name", false)]
	[InlineData ("Surname", false)]
	[InlineData ("Date", true)]
	public void BackingFieldTests (string propertyName, bool isStatic)
	{
		var property = new Property (
			name: propertyName,
			returnType: ReturnTypeForString (),
			symbolAvailability: new (),
			attributes: [],
			modifiers: isStatic ? [Token (SyntaxKind.StaticKeyword)] : [],
			accessors: []
		) {
			ExportFieldData = null,
		};
		Assert.Equal (Nomenclator.GetPropertyBackingFieldName (propertyName, isStatic), property.BackingField);
	}

	[Fact]
	public void IsThreadSafeProperty ()
	{
		var property = new Property (
			name: "MyProperty",
			returnType: ReturnTypeForString (),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		) {
			ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.IsThreadSafe)
		};
		Assert.True (property.IsThreadSafe);

		property = new Property (
			name: "MyProperty",
			returnType: ReturnTypeForString (),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		) {
			ExportPropertyData = new ("myProperty", ArgumentSemantic.None, ObjCBindings.Property.Default)
		};
		Assert.False (property.IsThreadSafe);
	}

	[Fact]
	public void CompareDiffName ()
	{
		var x = new Property (
			name: "First",
			returnType: ReturnTypeForString (),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);
		var y = new Property (
			name: "Second",
			returnType: ReturnTypeForString (),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);

		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

	[Fact]
	public void CompareDiffType ()
	{
		var x = new Property (
			name: "First",
			returnType: ReturnTypeForString (),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);
		var y = new Property (
			name: "First",
			returnType: ReturnTypeForInt (),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);

		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

	[Fact]
	public void CompareDiffIsReferenceType ()
	{
		var x = new Property (
			name: "First",
			returnType: new (
				name: "string",
				isBlittable: false,
				isSmartEnum: false,
				isNullable: false,
				isArray: false,
				isReferenceType: false
			),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);
		var y = new Property (
			name: "First",
			returnType: new (
				name: "string",
				isBlittable: false,
				isSmartEnum: false,
				isNullable: false,
				isArray: false,
				isReferenceType: true
			),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);

		Assert.False (condition: x.Equals (other: y));
		Assert.False (condition: y.Equals (other: x));
		Assert.False (condition: x == y);
		Assert.True (condition: x != y);
	}

	[Fact]
	public void CompareDiffIsBlittableType ()
	{
		var x = new Property (
			name: "First",
			returnType: new (
				name: "string",
				isBlittable: true,
				isSmartEnum: false,
				isNullable: false,
				isArray: false,
				isReferenceType: false
			),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);
		var y = new Property (
			name: "First",
			returnType: new (
				name: "string",
				isBlittable: false,
				isSmartEnum: false,
				isNullable: false,
				isArray: false,
				isReferenceType: false
			),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);

		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

	[Fact]
	public void CompareDiffIsSmartEnum ()
	{
		var x = new Property (
			name: "First",
			returnType: new (
				name: "string",
				isBlittable: false,
				isSmartEnum: true,
				isNullable: false,
				isArray: false,
				isReferenceType: false
			),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);
		var y = new Property (
			name: "First",
			returnType: new (
				name: "string",
				isBlittable: false,
				isSmartEnum: false,
				isNullable: false,
				isArray: false,
				isReferenceType: false
			),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);

		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

	[Fact]
	public void CompareDiffAttrs ()
	{
		var x = new Property ("First", ReturnTypeForString (), new (), [
			new ("Attr1"),
			new ("Attr2"),
		], [], []);
		var y = new Property ("First", ReturnTypeForString (), new (), [
			new ("Attr2"),
		], [], []);

		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

	[Fact]
	public void CompareDiffModifiers ()
	{
		var x = new Property ("First", ReturnTypeForString (), new (), [
			new ("Attr1"),
			new ("Attr2"),
		], [
			SyntaxFactory.Token (SyntaxKind.AbstractKeyword)
		], []);
		var y = new Property ("First", ReturnTypeForString (), new (), [
			new ("Attr1"),
			new ("Attr2"),
		], [
			SyntaxFactory.Token (SyntaxKind.PublicKeyword)
		], []);

		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

	[Fact]
	public void CompareDiffAccessors ()
	{
		var x = new Property ("First", ReturnTypeForString (), new (), [
			new ("Attr1"),
			new ("Attr2"),
		], [
			SyntaxFactory.Token (SyntaxKind.PublicKeyword)
		], [
			new (
				accessorKind: AccessorKind.Getter,
				symbolAvailability: new (),
				exportPropertyData: null,
				attributes: [],
				modifiers: []
			),
			new (
				accessorKind: AccessorKind.Setter,
				symbolAvailability: new (),
				exportPropertyData: null,
				attributes: [],
				modifiers: []
			),
		]);
		var y = new Property ("First", ReturnTypeForString (), new (), [
			new ("Attr1"),
			new ("Attr2"),
		], [
			SyntaxFactory.Token (SyntaxKind.PublicKeyword)
		], [
			new (
				accessorKind: AccessorKind.Getter,
				symbolAvailability: new (),
				exportPropertyData: null,
				attributes: [],
				modifiers: []
			),
		]);

		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

	[Fact]
	public void CompareDiffAccessorsExportData ()
	{
		var x = new Property ("First", ReturnTypeForString (), new (), [
			new ("Attr1"),
			new ("Attr2"),
		], [
			SyntaxFactory.Token (SyntaxKind.PublicKeyword)
		], [
			new (
				accessorKind: AccessorKind.Getter,
				symbolAvailability: new (),
				exportPropertyData: new ("name"),
				attributes: [],
				modifiers: []
			),
		]);
		var y = new Property ("First", ReturnTypeForString (), new (), [
			new ("Attr1"),
			new ("Attr2"),
		], [
			SyntaxFactory.Token (SyntaxKind.PublicKeyword)
		], [
			new (
				accessorKind: AccessorKind.Getter,
				symbolAvailability: new (),
				exportPropertyData: new ("surname"),
				attributes: [],
				modifiers: []
			),
		]);

		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

	[Fact]
	public void CompareEquals ()
	{
		var x = new Property ("First", ReturnTypeForString (), new (), [
			new ("Attr1"),
			new ("Attr2"),
		], [
			SyntaxFactory.Token (SyntaxKind.PublicKeyword)
		], [
			new (
				accessorKind: AccessorKind.Getter,
				symbolAvailability: new (),
				exportPropertyData: null,
				attributes: [],
				modifiers: []
			),
			new (
				accessorKind: AccessorKind.Setter,
				symbolAvailability: new (),
				exportPropertyData: null,
				attributes: [],
				modifiers: []
			),
		]);
		var y = new Property ("First", ReturnTypeForString (), new (), [
			new ("Attr1"),
			new ("Attr2"),
		], [
			SyntaxFactory.Token (SyntaxKind.PublicKeyword)
		], [
			new (
				accessorKind: AccessorKind.Getter,
				symbolAvailability: new (),
				exportPropertyData: null,
				attributes: [],
				modifiers: []
			),
			new (
				accessorKind: AccessorKind.Setter,
				symbolAvailability: new (),
				exportPropertyData: null,
				attributes: [],
				modifiers: []
			),
		]);

		Assert.True (x.Equals (y));
		Assert.True (y.Equals (x));
		Assert.True (x == y);
		Assert.False (x != y);
	}

	[Theory]
	[InlineData (ObjCBindings.Property.Default, false)]
	[InlineData (ObjCBindings.Property.Notification, true)]
#pragma warning disable xUnit1025
	[InlineData (ObjCBindings.Property.Notification | ObjCBindings.Property.Default, true)]
#pragma warning restore xUnit1025
	public void IsNotification (ObjCBindings.Property flag, bool expectedResult)
	{
		var property = new Property (
			name: "Test",
			returnType: new TypeInfo ("string"),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		) {
			ExportFieldData = new (new FieldData<ObjCBindings.Property> ("name", flag), ""),
		};
		Assert.Equal (expectedResult, property.IsNotification);
	}

	[Theory]
	[InlineData (ObjCBindings.Property.Default, false)]
	[InlineData (ObjCBindings.Property.WeakDelegate, true)]
#pragma warning disable xUnit1025
	[InlineData (ObjCBindings.Property.WeakDelegate | ObjCBindings.Property.Default, true)]
#pragma warning restore xUnit1025
	public void IsWeakDelegate (ObjCBindings.Property flag, bool expectedResult)
	{
		var property = new Property (
			name: "Test",
			returnType: new TypeInfo ("string"),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		) {
			ExportPropertyData = new ("name", ArgumentSemantic.None, flag),
		};
		Assert.Equal (expectedResult, property.IsWeakDelegate);
	}

	[Theory]
	[InlineData (true, true, true, true)] // isProperty, isWeakDelegate, hasStrongDelegateType, shouldChange
	[InlineData (false, true, true, false)]
	[InlineData (true, false, true, false)]
	[InlineData (true, true, false, false)]
	public void ToStrongDelegate (bool isProperty, bool isWeakDelegate, bool hasStrongDelegateType, bool shouldChange)
	{
		TypeInfo? strongDelegateType = hasStrongDelegateType ? ReturnTypeForNSObject ("StrongDelegate") : null;
		var flags = isWeakDelegate ? ObjCBindings.Property.WeakDelegate : ObjCBindings.Property.Default;
		var property = new Property (
			name: "Test",
			returnType: ReturnTypeForNSObject (),
			symbolAvailability: new (),
			attributes: [],
			modifiers: [],
			accessors: []
		);

		if (isProperty) {
			property = property with {
				ExportPropertyData = new ExportData<ObjCBindings.Property> (
					"name",
					ArgumentSemantic.None,
					flags
				) {
					StrongDelegateType = strongDelegateType
				},
			};
		} else {
			property = property with {
				ExportFieldData = new (new FieldData<ObjCBindings.Property> ("name", flags), ""),
			};
		}

		var newProperty = property.ToStrongDelegate ();

		if (shouldChange) {
			Assert.NotEqual (property, newProperty);
			Assert.Equal (strongDelegateType!.Value.WithNullable (true), newProperty.ReturnType);
		} else {
			Assert.Equal (property, newProperty);
		}
	}
}
