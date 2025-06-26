// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Macios.Generator.DataModel;
using Microsoft.Macios.Generator.Formatters;
using Xamarin.Tests;
using Xamarin.Utils;
using Xunit;

using static Microsoft.Macios.Generator.Tests.TestDataFactory;
namespace Microsoft.Macios.Generator.Tests.DataModel;

public class TypeInfoTests : BaseGeneratorTestClass {

	class TestDataFromMethodDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			const string voidMethodNoParams = @"
using System;

namespace NS {
	public class MyClass {
		public void MyMethod () {}
	}
}
";

			yield return [
				voidMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForVoid (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string intMethodNoParams = @"
using System;

namespace NS {
	public class MyClass {
		public int MyMethod () {}
	}
}
";
			yield return [
				intMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForInt (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string nullableIntMethodNoParams = @"
using System;

namespace NS {
	public class MyClass {
		public int? MyMethod () {}
	}
}
";
			yield return [
				nullableIntMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForInt (isNullable: true),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string intArrayMethodNoParams = @"
using System;

namespace NS {
	public class MyClass {
		public int[] MyMethod () {}
	}
}
";
			yield return [
				intArrayMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForArray ("int", isBlittable: true),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string nullableIntArrayMethodNoParams = @"
using System;

namespace NS {
	public class MyClass {
		public int[]? MyMethod () {}
	}
}
";
			yield return [
				nullableIntArrayMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForArray ("int", isNullable: true),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string intNullArrayMethodNoParams = @"
using System;

namespace NS {
	public class MyClass {
		public int?[] MyMethod () {}
	}
}
";
			yield return [
				intNullArrayMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForArray ("int?", isBlittable: false),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string int2DArrayMethodNoParams = @"
using System;

namespace NS {
	public class MyClass {
		public int[][] MyMethod () {}
	}
}
";
			yield return [
				int2DArrayMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForArray ("int[]", isBlittable: true),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string stringMethodNoParams = @"
using System;

namespace NS {
	public class MyClass {
		public string MyMethod () {}
	}
}
";
			yield return [
				stringMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForString (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string customClassMethodNoParams = @"
using System;

namespace NS {
	public class ReturnClass {}

	public class MyClass {
		public ReturnClass MyMethod () {}
	}
}
";
			yield return [
				customClassMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForClass ("NS.ReturnClass"),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string customStructMethodNoParams = @"
using System;

namespace NS {
	public struct ReturnClass {}

	public class MyClass {
		public ReturnClass MyMethod () {}
	}
}
";
			yield return [
				customStructMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForStruct ("NS.ReturnClass"),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string intPtrMethodNoParams = @"
using System;

namespace NS {
	public class MyClass {
		public IntPtr MyMethod () {}
	}
}
";
			yield return [
				intPtrMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForIntPtr (),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string nullableIntPtrMethodNoParams = @"
using System;

namespace NS {
	public class MyClass {
		public IntPtr? MyMethod () {}
	}
}
";
			yield return [
				nullableIntPtrMethodNoParams,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForIntPtr (isNullable: true),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string interfaceMethod = @"
using System;
using System.Collections.Generic;

namespace NS {
	public interface IInterface {}

	public class MyClass {
		public IInterface MyMethod () {}
	}
}
";
			yield return [
				interfaceMethod,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForInterface ("NS.IInterface"),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];

			const string nativeEnumMethod = @"
using System;
using ObjCRuntime;
using System.Collections.Generic;

namespace NS {
	[Native]
	public enum MyEnum : int {
		One,
		Two
	}

	public class MyClass {
		public MyEnum MyMethod () {}
	}
}
";
			yield return [
				nativeEnumMethod,
				new Method (
					type: "NS.MyClass",
					name: "MyMethod",
					returnType: ReturnTypeForEnum ("NS.MyEnum", isNativeEnum: true),
					symbolAvailability: new (),
					exportMethodData: new (),
					attributes: [],
					modifiers: [
						SyntaxFactory.Token (SyntaxKind.PublicKeyword),
					],
					parameters: []
				)
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[AllSupportedPlatformsClassData<TestDataFromMethodDeclaration>]
	void FromMethodDeclaration (ApplePlatform platform, string inputText, Method expected)
	{
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		Assert.Equal (expected, changes);
	}

	[Theory]
	[AllSupportedPlatforms]
	void TypeInfoIsPointer (ApplePlatform platform)
	{
		var inputText = @"
using System;
using ObjCRuntime;
using System.Collections.Generic;

namespace NS {
	public class MyClass {
		public unsafe void ProcessPointer (int* pointer)
		{
			if (pointer is null)
			{
				return;
			}

			// Modify the value at the pointer
			*pointer += 10;
		}
	}
}
";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		// ensure that the method has a single parameter
		Assert.Single (changes.Value.Parameters);
		// ensure that the first parameter is a pointer
		Assert.True (changes.Value.Parameters [0].Type.IsPointer);
	}

	[Theory]
	[AllSupportedPlatforms]
	void TypeInfoGenericName (ApplePlatform platform)
	{
		var inputText = @"
using System;
using System.Collections.Generic;
using ObjCRuntime;
using System.Collections.Generic;

namespace NS {

	public class ExampleClass {
		public int Number => 0;
	}

	public class MyClass {
		public int ProcessPointer (List<ExampleClass> pointer)
		{
			return pointer.Count;
		}
	}
}
";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		// ensure that the method has a single parameter
		Assert.Single (changes.Value.Parameters);
		// ensure that the first parameter is a pointer
		Assert.True (changes.Value.Parameters [0].Type.IsGenericType);
		Assert.Single (changes.Value.Parameters [0].Type.TypeArguments);
		Assert.Equal (Global ("NS.ExampleClass"), changes.Value.Parameters [0].Type.TypeArguments [0]);
		Assert.Equal ("List", changes.Value.Parameters [0].Type.Name);
		Assert.Equal ("System.Collections.Generic.List<NS.ExampleClass>", changes.Value.Parameters [0].Type.FullyQualifiedName);
		Assert.Equal ("System.Collections.Generic", string.Join ('.', changes.Value.Parameters [0].Type.Namespace));
	}

	[Theory]
	[AllSupportedPlatforms]
	void TypeInfoArrayName (ApplePlatform platform)
	{
		var inputText = @"
using System;
using System.Collections.Generic;
using ObjCRuntime;
using System.Collections.Generic;

namespace NS {

	public class ExampleClass {
		public int Number => 0;
	}

	public class MyClass {
		public int ProcessPointer (ExampleClass[] pointer)
		{
			return pointer.Length;
		}
	}
}
";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		// ensure that the method has a single parameter
		Assert.Single (changes.Value.Parameters);
		// ensure that the first parameter is a pointer
		Assert.True (changes.Value.Parameters [0].Type.IsArray);
		Assert.Equal ("ExampleClass", changes.Value.Parameters [0].Type.Name);
		Assert.Equal ("NS.ExampleClass", changes.Value.Parameters [0].Type.FullyQualifiedName);
		Assert.Equal ("NS", string.Join ('.', changes.Value.Parameters [0].Type.Namespace));
	}

	[Theory]
	[AllSupportedPlatforms]
	void TypeInfoNonGenericNoNamespace (ApplePlatform platform)
	{
		var inputText = @"
using System;
using System.Collections.Generic;
using ObjCRuntime;
using System.Collections.Generic;

namespace NS {
	public class MyClass {
		public int ProcessPointer (int pointer)
		{
			return pointer;
		}
	}
}
";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		// ensure that the method has a single parameter
		Assert.Single (changes.Value.Parameters);
		// ensure that the first parameter is a pointer
		Assert.False (changes.Value.Parameters [0].Type.IsGenericType);
		Assert.Equal ("int", changes.Value.Parameters [0].Type.Name);
		Assert.Equal ("int", changes.Value.Parameters [0].Type.FullyQualifiedName);
		Assert.Empty (changes.Value.Parameters [0].Type.Namespace);
	}

	[Theory]
	[AllSupportedPlatforms]
	void TypeInfoNameStringArray (ApplePlatform platform)
	{
		var inputText = @"
using System;
using System.Collections.Generic;
using ObjCRuntime;
using System.Collections.Generic;

namespace NS {
	public class MyClass {
		public int ProcessPointer (string[] pointer)
		{
			return pointer.Length;
		}
	}
}
";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		// ensure that the method has a single parameter
		Assert.Single (changes.Value.Parameters);
		// ensure that the first parameter is a pointer
		Assert.False (changes.Value.Parameters [0].Type.IsGenericType);
		Assert.Equal ("string", changes.Value.Parameters [0].Type.Name);
		Assert.Equal ("string", changes.Value.Parameters [0].Type.FullyQualifiedName);
		Assert.Empty (changes.Value.Parameters [0].Type.Namespace);
	}

	[Theory]
	[AllSupportedPlatforms]
	void TypeInfoGeneralCase (ApplePlatform platform)
	{
		var inputText = @"
using System;
using System.Collections.Generic;
using ObjCRuntime;
using System.Collections.Generic;

namespace Example {
	namespace NS {
		public class ExampleClass {
			public int Number => 0;
		}

		public class MyClass {
			public int ProcessPointer (ExampleClass pointer)
			{
				return pointer.Number;
			}
		}
	}
}
";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		// ensure that the method has a single parameter
		Assert.Single (changes.Value.Parameters);
		// ensure that the first parameter is a pointer
		Assert.False (changes.Value.Parameters [0].Type.IsGenericType);
		Assert.Equal ("ExampleClass", changes.Value.Parameters [0].Type.Name);
		Assert.Equal ("Example.NS.ExampleClass", changes.Value.Parameters [0].Type.FullyQualifiedName);
		Assert.Equal ("Example.NS", string.Join ('.', changes.Value.Parameters [0].Type.Namespace));
	}

	[Theory]
	[AllSupportedPlatforms]
	void TypeInfoNestedCase (ApplePlatform platform)
	{
		var inputText = @"
using System;
using System.Collections.Generic;
using ObjCRuntime;
using System.Collections.Generic;

namespace Example {
	namespace NS {

		public class MyClass {

			public class ExampleClass {
				public int Number => 0;
			}

			public int ProcessPointer (ExampleClass pointer)
			{
				return pointer.Number;
			}
		}
	}
}
";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		// ensure that the method has a single parameter
		Assert.Single (changes.Value.Parameters);
		// ensure that the first parameter is a pointer
		Assert.False (changes.Value.Parameters [0].Type.IsGenericType);
		Assert.Equal ("MyClass.ExampleClass", changes.Value.Parameters [0].Type.Name);
		Assert.Equal ("Example.NS.MyClass.ExampleClass", changes.Value.Parameters [0].Type.FullyQualifiedName);
		Assert.Equal ("Example.NS", string.Join ('.', changes.Value.Parameters [0].Type.Namespace));
	}

	[Theory]
	[PlatformInlineData (ApplePlatform.iOS, "Action", "Task", "TaskCompletionSource")]
	[PlatformInlineData (ApplePlatform.TVOS, "Action", "Task", "TaskCompletionSource")]
	[PlatformInlineData (ApplePlatform.MacCatalyst, "Action", "Task", "TaskCompletionSource")]
	[PlatformInlineData (ApplePlatform.MacOSX, "Action", "Task", "TaskCompletionSource")]
	[PlatformInlineData (ApplePlatform.iOS, "Action<int>", "Task<int>", "TaskCompletionSource<int>")]
	[PlatformInlineData (ApplePlatform.TVOS, "Action<int>", "Task<int>", "TaskCompletionSource<int>")]
	[PlatformInlineData (ApplePlatform.MacCatalyst, "Action<int>", "Task<int>", "TaskCompletionSource<int>")]
	[PlatformInlineData (ApplePlatform.MacOSX, "Action<int>", "Task<int>", "TaskCompletionSource<int>")]
	[PlatformInlineData (ApplePlatform.iOS, "Action<int, NSError>", "Task<int>", "TaskCompletionSource<int>")]
	[PlatformInlineData (ApplePlatform.TVOS, "Action<int, NSError>", "Task<int>", "TaskCompletionSource<int>")]
	[PlatformInlineData (ApplePlatform.MacCatalyst, "Action<int, NSError>", "Task<int>", "TaskCompletionSource<int>")]
	[PlatformInlineData (ApplePlatform.MacOSX, "Action<int, NSError>", "Task<int>", "TaskCompletionSource<int>")]
	[PlatformInlineData (ApplePlatform.iOS, "Action<int, string>", "Task<int, string>", "TaskCompletionSource<int, string>")]
	[PlatformInlineData (ApplePlatform.TVOS, "Action<int, string>", "Task<int, string>", "TaskCompletionSource<int, string>")]
	[PlatformInlineData (ApplePlatform.MacCatalyst, "Action<int, string>", "Task<int, string>", "TaskCompletionSource<int, string>")]
	[PlatformInlineData (ApplePlatform.MacOSX, "Action<int, string>", "Task<int, string>", "TaskCompletionSource<int, string>")]
	void TypeInfoToTask (ApplePlatform platform, string action, string expectedTask, string expectedCompletionSource)
	{
		var inputText = $@"
using System;
using System.Threading.Tasks;
using Foundation;
using ObjCRuntime;
using System.Collections.Generic;

namespace NS {{
	public class MyClass {{
		public void ProcessPointer ({action} myTask)
		{{
			// do nothing
		}}
	}}
}}
";
		var (compilation, syntaxTrees) = CreateCompilation (platform, sources: inputText);
		Assert.Single (syntaxTrees);
		var semanticModel = compilation.GetSemanticModel (syntaxTrees [0]);
		var declaration = syntaxTrees [0].GetRoot ()
			.DescendantNodes ().OfType<MethodDeclarationSyntax> ()
			.FirstOrDefault ();
		Assert.NotNull (declaration);
		Assert.True (Method.TryCreate (declaration, semanticModel, out var changes));
		Assert.NotNull (changes);
		// ensure that the method has a single parameter
		Assert.Single (changes.Value.Parameters);
		var type = changes.Value.Parameters [0].Type;
		var task = type.ToTask ();
		Assert.NotEqual (type, task);
		Assert.Equal ($"{Global ("System.Threading")}.Tasks.{expectedTask}", task.GetIdentifierSyntax ().ToString ());
		var completionSource = task.ToTaskCompletionSource ();
		Assert.Equal ($"{Global ("System.Threading")}.Tasks.{expectedCompletionSource}", completionSource.GetIdentifierSyntax ().ToString ());
	}
}
