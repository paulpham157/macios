// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#pragma warning disable APL0003
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Formatters;
using TypeInfo = Microsoft.Macios.Generator.DataModel.TypeInfo;

namespace Microsoft.Macios.Generator.Tests;

static class TestDataFactory {
	public static TypeInfo ReturnTypeForString (bool isNullable = false)
		=> new (
			name: "string",
			specialType: SpecialType.System_String,
			isNullable: isNullable,
			isBlittable: false,
			isSmartEnum: false,
			isArray: false,
			isReferenceType: true
		) {
			Interfaces = [
				"System.Collections.Generic.IEnumerable<char>",
				"System.Collections.IEnumerable",
				"System.ICloneable",
				"System.IComparable",
				"System.IComparable<string?>",
				"System.IConvertible",
				"System.IEquatable<string?>",
				"System.IParsable<string>",
				"System.ISpanParsable<string>"
			],
			MetadataName = "String",
			Parents = ["object"],
			IsNSObject = false,
			IsINativeObject = false,
		};

	public static TypeInfo ReturnTypeForInt (bool isNullable = false, bool keepInterfaces = false,
		bool isUnsigned = false)
	{
		var typeName = isUnsigned ? "uint" : "int";
		var metadataName = isUnsigned ? "UInt32" : "Int32";
		var type = new TypeInfo (
			name: typeName,
			specialType: isUnsigned ? SpecialType.System_UInt32 : SpecialType.System_Int32,
			isBlittable: !isNullable,
			isNullable: isNullable,
			isStruct: true
		) {
			Parents = ["System.ValueType", "object"],
			Interfaces = isNullable && !keepInterfaces
				? []
				: [
					"System.IComparable",
					$"System.IComparable<{typeName}>",
					"System.IConvertible",
					$"System.IEquatable<{typeName}>",
					"System.IFormattable",
					$"System.IParsable<{typeName}>",
					"System.ISpanFormattable",
					$"System.ISpanParsable<{typeName}>",
					"System.IUtf8SpanFormattable",
					$"System.IUtf8SpanParsable<{typeName}>",
					$"System.Numerics.IAdditionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IAdditiveIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IBinaryInteger<{typeName}>",
					$"System.Numerics.IBinaryNumber<{typeName}>",
					$"System.Numerics.IBitwiseOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IComparisonOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IEqualityOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IDecrementOperators<{typeName}>",
					$"System.Numerics.IDivisionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IIncrementOperators<{typeName}>",
					$"System.Numerics.IModulusOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMultiplicativeIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IMultiplyOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.INumber<{typeName}>",
					$"System.Numerics.INumberBase<{typeName}>",
					$"System.Numerics.ISubtractionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IUnaryNegationOperators<{typeName}, {typeName}>",
					$"System.Numerics.IUnaryPlusOperators<{typeName}, {typeName}>",
					$"System.Numerics.IShiftOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMinMaxValue<{typeName}>",
					$"System.Numerics.ISignedNumber<{typeName}>"
				],
			MetadataName = metadataName,
		};
		return type;
	}

	public static TypeInfo ReturnTypeForShort (bool isNullable = false, bool keepInterfaces = false,
		bool isUnsigned = false)
	{
		var typeName = isUnsigned ? "ushort" : "short";
		var metadataName = isUnsigned ? "UInt16" : "Int16";
		var type = new TypeInfo (
			name: typeName,
			specialType: isUnsigned ? SpecialType.System_UInt16 : SpecialType.System_Int16,
			isBlittable: !isNullable,
			isNullable: isNullable,
			isStruct: true
		) {
			Parents = ["System.ValueType", "object"],
			Interfaces = isNullable && !keepInterfaces
				? []
				: [
					"System.IComparable",
					$"System.IComparable<{typeName}>",
					"System.IConvertible",
					$"System.IEquatable<{typeName}>",
					"System.IFormattable",
					$"System.IParsable<{typeName}>",
					"System.ISpanFormattable",
					$"System.ISpanParsable<{typeName}>",
					"System.IUtf8SpanFormattable",
					$"System.IUtf8SpanParsable<{typeName}>",
					$"System.Numerics.IAdditionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IAdditiveIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IBinaryInteger<{typeName}>",
					$"System.Numerics.IBinaryNumber<{typeName}>",
					$"System.Numerics.IBitwiseOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IComparisonOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IEqualityOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IDecrementOperators<{typeName}>",
					$"System.Numerics.IDivisionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IIncrementOperators<{typeName}>",
					$"System.Numerics.IModulusOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMultiplicativeIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IMultiplyOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.INumber<{typeName}>",
					$"System.Numerics.INumberBase<{typeName}>",
					$"System.Numerics.ISubtractionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IUnaryNegationOperators<{typeName}, {typeName}>",
					$"System.Numerics.IUnaryPlusOperators<{typeName}, {typeName}>",
					$"System.Numerics.IShiftOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMinMaxValue<{typeName}>",
					$"System.Numerics.ISignedNumber<{typeName}>"
				],
			MetadataName = metadataName,
		};
		return type;
	}

	public static TypeInfo ReturnTypeForLong (bool isNullable = false, bool keepInterfaces = false,
		bool isUnsigned = false)
	{
		var typeName = isUnsigned ? "ulong" : "long";
		var metadataName = isUnsigned ? "UInt64" : "Int64";
		var type = new TypeInfo (
			name: typeName,
			specialType: isUnsigned ? SpecialType.System_UInt64 : SpecialType.System_Int64,
			isBlittable: !isNullable,
			isNullable: isNullable,
			isStruct: true
		) {
			Parents = ["System.ValueType", "object"],
			Interfaces = isNullable && !keepInterfaces
				? []
				: [
					"System.IComparable",
					$"System.IComparable<{typeName}>",
					"System.IConvertible",
					$"System.IEquatable<{typeName}>",
					"System.IFormattable",
					$"System.IParsable<{typeName}>",
					"System.ISpanFormattable",
					$"System.ISpanParsable<{typeName}>",
					"System.IUtf8SpanFormattable",
					$"System.IUtf8SpanParsable<{typeName}>",
					$"System.Numerics.IAdditionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IAdditiveIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IBinaryInteger<{typeName}>",
					$"System.Numerics.IBinaryNumber<{typeName}>",
					$"System.Numerics.IBitwiseOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IComparisonOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IEqualityOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IDecrementOperators<{typeName}>",
					$"System.Numerics.IDivisionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IIncrementOperators<{typeName}>",
					$"System.Numerics.IModulusOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMultiplicativeIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IMultiplyOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.INumber<{typeName}>",
					$"System.Numerics.INumberBase<{typeName}>",
					$"System.Numerics.ISubtractionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IUnaryNegationOperators<{typeName}, {typeName}>",
					$"System.Numerics.IUnaryPlusOperators<{typeName}, {typeName}>",
					$"System.Numerics.IShiftOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMinMaxValue<{typeName}>",
					$"System.Numerics.ISignedNumber<{typeName}>"
				],
			MetadataName = metadataName,
		};
		return type;
	}

	public static TypeInfo ReturnTypeForDouble (bool isNullable = false, bool keepInterfaces = false)
	{
		var typeName = "double";
		var type = new TypeInfo (
			name: typeName,
			specialType: SpecialType.System_Double,
			isBlittable: !isNullable,
			isNullable: isNullable,
			isStruct: true
		) {
			Parents = ["System.ValueType", "object"],
			Interfaces = isNullable && !keepInterfaces
				? []
				: [
					"System.IComparable",
					$"System.IComparable<{typeName}>",
					"System.IConvertible",
					$"System.IEquatable<{typeName}>",
					"System.IFormattable",
					$"System.IParsable<{typeName}>",
					"System.ISpanFormattable",
					$"System.ISpanParsable<{typeName}>",
					"System.IUtf8SpanFormattable",
					$"System.IUtf8SpanParsable<{typeName}>",
					$"System.Numerics.IAdditionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IAdditiveIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IBinaryInteger<{typeName}>",
					$"System.Numerics.IBinaryNumber<{typeName}>",
					$"System.Numerics.IBitwiseOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IComparisonOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IEqualityOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IDecrementOperators<{typeName}>",
					$"System.Numerics.IDivisionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IIncrementOperators<{typeName}>",
					$"System.Numerics.IModulusOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMultiplicativeIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IMultiplyOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.INumber<{typeName}>",
					$"System.Numerics.INumberBase<{typeName}>",
					$"System.Numerics.ISubtractionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IUnaryNegationOperators<{typeName}, {typeName}>",
					$"System.Numerics.IUnaryPlusOperators<{typeName}, {typeName}>",
					$"System.Numerics.IShiftOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMinMaxValue<{typeName}>",
					$"System.Numerics.ISignedNumber<{typeName}>"
				],
			MetadataName = "Double",
		};
		return type;
	}

	public static TypeInfo ReturnTypeForFloat (bool isNullable = false, bool keepInterfaces = false)
	{
		var typeName = "float";
		var type = new TypeInfo (
			name: typeName,
			specialType: SpecialType.System_Single,
			isBlittable: !isNullable,
			isNullable: isNullable,
			isStruct: true
		) {
			Parents = ["System.ValueType", "object"],
			Interfaces = isNullable && !keepInterfaces
				? []
				: [
					"System.IComparable",
					$"System.IComparable<{typeName}>",
					"System.IConvertible",
					$"System.IEquatable<{typeName}>",
					"System.IFormattable",
					$"System.IParsable<{typeName}>",
					"System.ISpanFormattable",
					$"System.ISpanParsable<{typeName}>",
					"System.IUtf8SpanFormattable",
					$"System.IUtf8SpanParsable<{typeName}>",
					$"System.Numerics.IAdditionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IAdditiveIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IBinaryInteger<{typeName}>",
					$"System.Numerics.IBinaryNumber<{typeName}>",
					$"System.Numerics.IBitwiseOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IComparisonOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IEqualityOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IDecrementOperators<{typeName}>",
					$"System.Numerics.IDivisionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IIncrementOperators<{typeName}>",
					$"System.Numerics.IModulusOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMultiplicativeIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IMultiplyOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.INumber<{typeName}>",
					$"System.Numerics.INumberBase<{typeName}>",
					$"System.Numerics.ISubtractionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IUnaryNegationOperators<{typeName}, {typeName}>",
					$"System.Numerics.IUnaryPlusOperators<{typeName}, {typeName}>",
					$"System.Numerics.IShiftOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMinMaxValue<{typeName}>",
					$"System.Numerics.ISignedNumber<{typeName}>"
				],
			MetadataName = "Float",
		};
		return type;
	}

	public static TypeInfo ReturnTypeForNInt (bool isNullable = false, bool keepInterfaces = false,
		bool isUnsigned = false)
	{
		var typeName = isUnsigned ? "nuint" : "nint";
		var metadataName = isUnsigned ? "UIntPtr" : "IntPtr";
		var type = new TypeInfo (
			name: typeName,
			specialType: isUnsigned ? SpecialType.System_UInt32 : SpecialType.System_Int32,
			isBlittable: !isNullable,
			isNullable: isNullable,
			isStruct: true
		) {
			Parents = ["System.ValueType", "object"],
			Interfaces = isNullable && !keepInterfaces
				? []
				: [
					"System.IComparable",
					$"System.IComparable<{typeName}>",
					"System.IConvertible",
					$"System.IEquatable<{typeName}>",
					"System.IFormattable",
					$"System.IParsable<{typeName}>",
					"System.ISpanFormattable",
					$"System.ISpanParsable<{typeName}>",
					"System.IUtf8SpanFormattable",
					$"System.IUtf8SpanParsable<{typeName}>",
					$"System.Numerics.IAdditionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IAdditiveIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IBinaryInteger<{typeName}>",
					$"System.Numerics.IBinaryNumber<{typeName}>",
					$"System.Numerics.IBitwiseOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IComparisonOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IEqualityOperators<{typeName}, {typeName}, bool>",
					$"System.Numerics.IDecrementOperators<{typeName}>",
					$"System.Numerics.IDivisionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IIncrementOperators<{typeName}>",
					$"System.Numerics.IModulusOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMultiplicativeIdentity<{typeName}, {typeName}>",
					$"System.Numerics.IMultiplyOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.INumber<{typeName}>",
					$"System.Numerics.INumberBase<{typeName}>",
					$"System.Numerics.ISubtractionOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IUnaryNegationOperators<{typeName}, {typeName}>",
					$"System.Numerics.IUnaryPlusOperators<{typeName}, {typeName}>",
					$"System.Numerics.IShiftOperators<{typeName}, {typeName}, {typeName}>",
					$"System.Numerics.IMinMaxValue<{typeName}>",
					$"System.Numerics.ISignedNumber<{typeName}>"
				],
			MetadataName = metadataName,
		};
		return type;
	}

	public static TypeInfo ReturnTypeForIntPtr (bool isNullable = false)
		=> new (
			name: "nint",
			specialType: SpecialType.System_IntPtr,
			isBlittable: !isNullable,
			isNullable: isNullable,
			isStruct: true
		) {
			Parents = ["System.ValueType", "object"],
			Interfaces = isNullable
				? []
				: [
					"System.IComparable",
					"System.IComparable<nint>",
					"System.IEquatable<nint>",
					"System.IFormattable",
					"System.IParsable<nint>",
					"System.ISpanFormattable",
					"System.ISpanParsable<nint>",
					"System.IUtf8SpanFormattable",
					"System.IUtf8SpanParsable<nint>",
					"System.Numerics.IAdditionOperators<nint, nint, nint>",
					"System.Numerics.IAdditiveIdentity<nint, nint>",
					"System.Numerics.IBinaryInteger<nint>",
					"System.Numerics.IBinaryNumber<nint>",
					"System.Numerics.IBitwiseOperators<nint, nint, nint>",
					"System.Numerics.IComparisonOperators<nint, nint, bool>",
					"System.Numerics.IEqualityOperators<nint, nint, bool>",
					"System.Numerics.IDecrementOperators<nint>",
					"System.Numerics.IDivisionOperators<nint, nint, nint>",
					"System.Numerics.IIncrementOperators<nint>",
					"System.Numerics.IModulusOperators<nint, nint, nint>",
					"System.Numerics.IMultiplicativeIdentity<nint, nint>",
					"System.Numerics.IMultiplyOperators<nint, nint, nint>",
					"System.Numerics.INumber<nint>",
					"System.Numerics.INumberBase<nint>",
					"System.Numerics.ISubtractionOperators<nint, nint, nint>",
					"System.Numerics.IUnaryNegationOperators<nint, nint>",
					"System.Numerics.IUnaryPlusOperators<nint, nint>",
					"System.Numerics.IShiftOperators<nint, int, nint>",
					"System.Numerics.IMinMaxValue<nint>",
					"System.Numerics.ISignedNumber<nint>",
					"System.Runtime.Serialization.ISerializable"
				],
			MetadataName = "IntPtr",
			IsNativeIntegerType = !isNullable,
		};

	public static TypeInfo ReturnTypeForBool ()
		=> new (
			name: "bool",
			specialType: SpecialType.System_Boolean,
			isBlittable: false,
			isStruct: true
		) {
			Parents = ["System.ValueType", "object"],
			Interfaces = [
				"System.IComparable",
				"System.IComparable<bool>",
				"System.IConvertible",
				"System.IEquatable<bool>",
				"System.IParsable<bool>",
				"System.ISpanParsable<bool>"
			],
			MetadataName = "Boolean",
		};

	public static TypeInfo ReturnTypeForVoid ()
		=> new ("void", SpecialType.System_Void, isStruct: true) { Parents = ["System.ValueType", "object"], };

	public static TypeInfo ReturnTypeForClass (string className, bool isNullable = false)
		=> new (
			name: className,
			isReferenceType: true,
			isNullable: isNullable
		) { Parents = ["object"] };

	public static TypeInfo ReturnTypeForGeneric (string genericName, bool isNullable = false)
		=> new (
			name: genericName,
			isReferenceType: false,
			isNullable: isNullable
		);

	public static TypeInfo ReturnTypeForInterface (string interfaceName, bool isNullable = false, bool isProtocol = false)
		=> new (
			name: interfaceName,
			isNullable: isNullable,
			isArray: false,
			isReferenceType: true
		) {
			IsInterface = true,
			IsProtocol = isProtocol,
			Parents = [],
			Interfaces = []
		};

	public static TypeInfo ReturnTypeForStruct (string structName, bool isBlittable = false)
		=> new (
			name: structName,
			isBlittable: isBlittable,
			isStruct: true
		) { Parents = ["System.ValueType", "object"] };

	public static TypeInfo ReturnTypeForEnum (string enumName, bool isSmartEnum = false, bool isNativeEnum = false,
		bool isNullable = false, bool isBlittable = true, SpecialType underlyingType = SpecialType.System_Int32)
		=> new (
			name: enumName,
			isNullable: isNullable,
			isBlittable: isBlittable,
			isSmartEnum: isSmartEnum
		) {
			Parents = [
				"System.Enum",
				"System.ValueType",
				"object"
			],
			Interfaces = [
				"System.IComparable",
				"System.IConvertible",
				"System.IFormattable",
				"System.ISpanFormattable"
			],
			IsNativeEnum = isNativeEnum,
			EnumUnderlyingType = underlyingType,
		};

	public static TypeInfo ReturnTypeForArray (string type,
		bool isNullable = false,
		bool isBlittable = false,
		bool isEnum = false,
		bool isSmartEnum = false,
		bool isStruct = false,
		bool isNSObject = false,
		SpecialType? underlyingType = null)
		=> new (
			name: type,
			isNullable: isNullable,
			isBlittable: isBlittable,
			isArray: true,
			isReferenceType: true,
			isSmartEnum: isSmartEnum,
			isStruct: isStruct
		) {
			EnumUnderlyingType = isEnum ? SpecialType.System_Int32 : null,
			ArrayElementType = underlyingType,
			ArrayElementTypeIsWrapped = isNSObject,
			Parents = ["System.Array", "object"],
			Interfaces = [
				$"System.Collections.Generic.IList<{type}>",
				$"System.Collections.Generic.IReadOnlyList<{type}>",
				"System.Collections.ICollection",
				"System.Collections.IEnumerable",
				"System.Collections.IList",
				"System.Collections.IStructuralComparable",
				"System.Collections.IStructuralEquatable",
				"System.ICloneable"
			]
		};

	public static TypeInfo ReturnTypeForAction (DelegateInfo? delegateInfo = null)
		=> new (
			name: "System.Action",
			isNullable: false,
			isBlittable: false,
			isArray: false,
			isReferenceType: true
		) {
			IsDelegate = true,
			Delegate = delegateInfo,
			Parents = [
				"System.MulticastDelegate",
				"System.Delegate",
				"object"
			],
			Interfaces = [
				"System.ICloneable",
				"System.Runtime.Serialization.ISerializable",
			]
		};

	public static TypeInfo ReturnTypeForAction (DelegateInfo? delegateInfo = null, params string [] parameters)
		=> new (
			name: $"System.Action<{string.Join (", ", parameters)}>",
			isNullable: false,
			isBlittable: false,
			isArray: false,
			isReferenceType: true
		) {
			IsDelegate = true,
			Delegate = delegateInfo,
			Parents = [
				"System.MulticastDelegate",
				"System.Delegate",
				"object"
			],
			Interfaces = [
				"System.ICloneable",
				"System.Runtime.Serialization.ISerializable",
			]
		};

	public static TypeInfo ReturnTypeForFunc (DelegateInfo? delegateInfo = null, params string [] parameters)
		=> new (
			name: $"System.Func<{string.Join (", ", parameters)}>",
			isNullable: false,
			isBlittable: false,
			isArray: false,
			isReferenceType: true
		) {
			IsDelegate = true,
			Delegate = delegateInfo,
			Parents = [
				"System.MulticastDelegate",
				"System.Delegate",
				"object"
			],
			Interfaces = [
				"System.ICloneable",
				"System.Runtime.Serialization.ISerializable",
			]
		};

	public static TypeInfo ReturnTypeForDelegate (string delegateName, DelegateInfo? delegateInfo = null)
		=> new (
			name: delegateName,
			isNullable: false,
			isBlittable: false,
			isArray: false,
			isReferenceType: true
		) {
			IsDelegate = true,
			Delegate = delegateInfo,
			Parents = [
				"System.MulticastDelegate",
				"System.Delegate",
				"object"
			],
			Interfaces = [
				"System.ICloneable",
				"System.Runtime.Serialization.ISerializable",
			]
		};

	public static TypeInfo ReturnTypeForNSObject (string? nsObjectName = null, bool isNullable = false, bool isApiDefinition = false)
		=> new (
			name: nsObjectName ?? "Foundation.NSObject",
			isNullable: isNullable,
			isArray: false,
			isReferenceType: true
		) {
			IsNSObject = true,
			IsWrapped = true,
			IsINativeObject = true,
			Parents = nsObjectName is null ? ["object"] : ["Foundation.NSObject", "object"],
			Interfaces = isApiDefinition
				? [
					"ObjCRuntime.INativeObject",
					"Foundation.INSObjectFactory",
				]
				: [
					"ObjCRuntime.INativeObject",
					$"System.IEquatable<Foundation.NSObject>",
					"System.IDisposable",
					"Foundation.INSObjectFactory",
					"Foundation.INSObjectProtocol"
				]
		};

	public static TypeInfo ReturnTypeForINativeObject (string nativeObjectName, bool isNullable = false)
		=> new (
			name: nativeObjectName,
			isNullable: isNullable,
			isArray: false
		) {
			IsNSObject = true,
			IsINativeObject = true,
			Parents = ["object"],
			Interfaces = ["ObjCRuntime.INativeObject"]
		};

	public static TypeInfo ReturnTypeForNSString (bool isNullable = false)
		=> new (
			name: "Foundation.NSString",
			isNullable: isNullable,
			isArray: false, isReferenceType: true) {
			IsNSObject = true,
			IsINativeObject = true,
			Parents = [
				"Foundation.NSObject",
				"object"
			],
			Interfaces = [
				"Foundation.INSCopying",
				"Foundation.INSSecureCoding",
				"ObjCRuntime.INativeObject",
				"Foundation.INSObjectFactory"
			]
		};

	public static TypeInfo ReturnTypeForNamedTuple (params KeyValuePair<string, TypeInfo> [] fields)
	{
		var dataMembers = string.Join (", ", fields.Select (x => $"{x.Value.GetIdentifierSyntax ()} {x.Key}"));
		var genericMembers = string.Join (", ", fields.Select (x => $"{x.Value.GetIdentifierSyntax ()}"));
		var tupleFields = ImmutableArray.CreateBuilder<KeyValuePair<string, string>> (fields.Length);
		foreach (var (name, typeInfo) in fields) {
			tupleFields.Add (new (name, typeInfo.GetIdentifierSyntax ().ToString ()));
		}
		var type = new TypeInfo (
			name: $"({dataMembers})",
			isNullable: false,
			isBlittable: false,
			isArray: false,
			isReferenceType: false,
			isStruct: true
		) {
			IsNamedTuple = true,
			NamedTupleFields = tupleFields.ToImmutable (),
			Parents = ["System.ValueType", "object"],
			Interfaces = [
				"System.Collections.IStructuralComparable",
				"System.Collections.IStructuralEquatable",
				"System.IComparable",
				$"System.IComparable<({genericMembers})>",
				$"System.IEquatable<({genericMembers})>",
				"System.Runtime.CompilerServices.ITuple"
			]
		};
		return type;
	}
}
