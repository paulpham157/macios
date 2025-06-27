// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
#pragma warning disable APL0003
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Macios.Generator.Attributes;
using ObjCBindings;
using ObjCRuntime;
using Xunit;

namespace Microsoft.Macios.Generator.Tests.Attributes;

public class ExportDataTests {

	[Fact]
	public void TestExportDataEqualsDiffSelector ()
	{
		var x = new ExportData<Method> ("field1");
		var y = new ExportData<Method> ("field2");
		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

	[Fact]
	public void TestExportDataEqualsDiffArgumentSemantic ()
	{
		var x = new ExportData<Method> ("property", ArgumentSemantic.None);
		var y = new ExportData<Method> ("property", ArgumentSemantic.Retain);
		Assert.False (x.Equals (y));
		Assert.False (y.Equals (x));
		Assert.False (x == y);
		Assert.True (x != y);
	}

	[Theory]
	[InlineData (Property.Default, Property.Default, true)]
	[InlineData (Property.Notification, Property.Default, false)]
	public void TestExportDataEqualsDiffFlag (Property xFlag, Property yFlag, bool expected)
	{
		var x = new ExportData<Property> ("property", ArgumentSemantic.None, xFlag);
		var y = new ExportData<Property> ("property", ArgumentSemantic.None, yFlag);
		Assert.Equal (expected, x.Equals (y));
		Assert.Equal (expected, y.Equals (x));
		Assert.Equal (expected, x == y);
		Assert.Equal (!expected, x != y);
	}

	class TestDataToString : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			yield return [
				Method.Default,
				new ExportData<Method> ("symbol", ArgumentSemantic.None, Method.Default),
				"{ Type: 'ObjCBindings.Method', Selector: 'symbol', ArgumentSemantic: 'None', Flags: 'Default', NativePrefix: 'null', NativeSuffix: 'null', Library: 'null', ResultType: 'null', MethodName: 'null', ResultTypeName: 'null', PostNonResultSnippet: 'null' }",
			];
			yield return [
				Method.Default,
				new ExportData<Method> ("symbol"),
				"{ Type: 'ObjCBindings.Method', Selector: 'symbol', ArgumentSemantic: 'None', Flags: 'Default', NativePrefix: 'null', NativeSuffix: 'null', Library: 'null', ResultType: 'null', MethodName: 'null', ResultTypeName: 'null', PostNonResultSnippet: 'null' }",
			];
			yield return [
				Property.Default,
				new ExportData<Property> ("symbol", ArgumentSemantic.Retain, Property.Default),
				"{ Type: 'ObjCBindings.Property', Selector: 'symbol', ArgumentSemantic: 'Retain', Flags: 'Default', NativePrefix: 'null', NativeSuffix: 'null', Library: 'null', ResultType: 'null', MethodName: 'null', ResultTypeName: 'null', PostNonResultSnippet: 'null' }"
			];
			yield return [
				Property.Default,
				new ExportData<Property> ("symbol"),
				"{ Type: 'ObjCBindings.Property', Selector: 'symbol', ArgumentSemantic: 'None', Flags: 'Default', NativePrefix: 'null', NativeSuffix: 'null', Library: 'null', ResultType: 'null', MethodName: 'null', ResultTypeName: 'null', PostNonResultSnippet: 'null' }"
			];
			yield return [
				Method.Default,
				new ExportData<Method> ("symbol", ArgumentSemantic.None,
				Method.Default | Method.CustomMarshalDirective) {
					NativePrefix = "xamarin_",
					Library = "__Internal"
				},
				"{ Type: 'ObjCBindings.Method', Selector: 'symbol', ArgumentSemantic: 'None', Flags: 'CustomMarshalDirective', NativePrefix: 'xamarin_', NativeSuffix: 'null', Library: '__Internal', ResultType: 'null', MethodName: 'null', ResultTypeName: 'null', PostNonResultSnippet: 'null' }"
			];
		}

		IEnumerator IEnumerable.GetEnumerator ()
			=> GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof (TestDataToString))]
	void TestFieldDataToString<T> (T @enum, ExportData<T> x, string expected) where T : Enum
	{
		var str = x.ToString ();
		Assert.NotNull (@enum);
		Assert.Equal (expected, x.ToString ());
	}
}
