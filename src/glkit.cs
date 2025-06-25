//
// GLKit.cs: bindings to the iOS5/Lion GLKit
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011 Xamarin, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Numerics;
using Foundation;
using ObjCRuntime;
using CoreGraphics;
using CoreFoundation;
using ModelIO;

using Matrix3 = global::CoreGraphics.RMatrix3;
using Matrix4 = global::System.Numerics.Matrix4x4;

#if MONOMAC
using pfloat = System.Runtime.InteropServices.NFloat;
using AppKit;
using EAGLSharegroup = Foundation.NSObject;
using EAGLContext = Foundation.NSObject;
using UIView = AppKit.NSView;
using UIImage = AppKit.NSImage;
using UIViewController = AppKit.NSViewController;
#else
using OpenGLES;
using UIKit;
using pfloat = System.Single;
using NSOpenGLContext = Foundation.NSObject;
#endif

namespace GLKit {

	/// <summary>Defines values whose values represent constant values relating to errors.</summary>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[Static]
	interface GLKModelError {

		/// <summary>A value corresponding to the constant <c>kGLKModelErrorDomain</c>.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kGLKModelErrorDomain")]
		NSString Domain { get; }

		/// <summary>A value corresponding to the constant <c>kGLKModelErrorKey</c>.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Field ("kGLKModelErrorKey")]
		NSString Key { get; }
	}

	/// <summary>A class that provides a variety of shaders based on the OpenGL ES 1.1 lighting and shading model.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKBaseEffect_ClassRef/index.html">Apple documentation for <c>GLKBaseEffect</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	interface GLKBaseEffect : GLKNamedEffect {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("colorMaterialEnabled", ArgumentSemantic.Assign)]
		bool ColorMaterialEnabled { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("useConstantColor", ArgumentSemantic.Assign)]
		bool UseConstantColor { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transform")]
		GLKEffectPropertyTransform Transform { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("light0")]
		GLKEffectPropertyLight Light0 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("light1")]
		GLKEffectPropertyLight Light1 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("light2")]
		GLKEffectPropertyLight Light2 { get; }

		[Export ("lightingType", ArgumentSemantic.Assign)]
		GLKLightingType LightingType { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lightModelAmbientColor", ArgumentSemantic.Assign)]
		Vector4 LightModelAmbientColor { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("material")]
		GLKEffectPropertyMaterial Material { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("texture2d0")]
		GLKEffectPropertyTexture Texture2d0 { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("texture2d1")]
		GLKEffectPropertyTexture Texture2d1 { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("textureOrder", ArgumentSemantic.Copy)]
		GLKEffectPropertyTexture [] TextureOrder { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("constantColor", ArgumentSemantic.Assign)]
		Vector4 ConstantColor { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("fog")]
		GLKEffectPropertyFog Fog { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Export ("label", ArgumentSemantic.Copy)]
		[DisableZeroCopy]
		[NullAllowed] // default is null on iOS 5.1.1
		string Label { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("lightModelTwoSided", ArgumentSemantic.Assign)]
		bool LightModelTwoSided { get; set; }
	}

	/// <summary>A base class whose subtypes define properties for graphic effects.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKEffectProperty_ClassRef/index.html">Apple documentation for <c>GLKEffectProperty</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	interface GLKEffectProperty {
	}

	/// <summary>A class that holds properties that configure how fog is applied to an effect.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKEffectPropertyFog_ClassRef/index.html">Apple documentation for <c>GLKEffectPropertyFog</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (GLKEffectProperty))]
	interface GLKEffectPropertyFog {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mode", ArgumentSemantic.Assign)]
		GLKFogMode Mode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("color", ArgumentSemantic.Assign)]
		Vector4 Color { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("density", ArgumentSemantic.Assign)]
		float Density { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("start", ArgumentSemantic.Assign)]
		float Start { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("end", ArgumentSemantic.Assign)]
		float End { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled", ArgumentSemantic.Assign)]
		bool Enabled { get; set; }
	}

	/// <summary>A class that holds properties that configure how a single light is applied to an effect.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKEffectPropertyLight_ClassRef/index.html">Apple documentation for <c>GLKEffectPropertyLight</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (GLKEffectProperty))]
	interface GLKEffectPropertyLight {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("position", ArgumentSemantic.Assign)]
		Vector4 Position { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ambientColor", ArgumentSemantic.Assign)]
		Vector4 AmbientColor { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("diffuseColor", ArgumentSemantic.Assign)]
		Vector4 DiffuseColor { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("specularColor", ArgumentSemantic.Assign)]
		Vector4 SpecularColor { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("spotDirection", ArgumentSemantic.Assign)]
		Vector3 SpotDirection { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("spotExponent", ArgumentSemantic.Assign)]
		float SpotExponent { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("spotCutoff", ArgumentSemantic.Assign)]
		float SpotCutoff { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("constantAttenuation", ArgumentSemantic.Assign)]
		float ConstantAttenuation { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("linearAttenuation", ArgumentSemantic.Assign)]
		float LinearAttenuation { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("quadraticAttenuation", ArgumentSemantic.Assign)]
		float QuadraticAttenuation { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("transform", ArgumentSemantic.Retain)]
		GLKEffectPropertyTransform Transform { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled", ArgumentSemantic.Assign)]
		bool Enabled { get; set; }

	}

	/// <summary>A class that holds properties that configure the characteristics of a surface being lit.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKEffectPropertyMaterial_ClassRef/index.html">Apple documentation for <c>GLKEffectPropertyMaterial</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (GLKEffectProperty))]
	interface GLKEffectPropertyMaterial {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("diffuseColor", ArgumentSemantic.Assign)]
		Vector4 DiffuseColor { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("specularColor", ArgumentSemantic.Assign)]
		Vector4 SpecularColor { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("emissiveColor", ArgumentSemantic.Assign)]
		Vector4 EmissiveColor { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("shininess", ArgumentSemantic.Assign)]
		float Shininess { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ambientColor", ArgumentSemantic.Assign)]
		Vector4 AmbientColor { [Align (16)] get; set; }
	}

	/// <summary>A class that holds properties that configure an OpenGL texturing operation.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKEffectPropertyTexture_ClassRef/index.html">Apple documentation for <c>GLKEffectPropertyTexture</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (GLKEffectProperty))]
	interface GLKEffectPropertyTexture {
		[Export ("target", ArgumentSemantic.Assign)]
		GLKTextureTarget Target { get; set; }

		[Export ("envMode", ArgumentSemantic.Assign)]
		GLKTextureEnvMode EnvMode { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enabled", ArgumentSemantic.Assign)]
		bool Enabled { get; set; }

		[Export ("name", ArgumentSemantic.Assign)]
		uint GLName { get; set; } /* GLuint = uint32_t */

	}

	/// <summary>A class that holds properties that configure the coordinate transforms to be applied when rendering an effect.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKEffectPropertyTransform_ClassRef/index.html">Apple documentation for <c>GLKEffectPropertyTransform</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (GLKEffectProperty))]
	interface GLKEffectPropertyTransform {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("normalMatrix")]
		Matrix3 NormalMatrix { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("modelviewMatrix", ArgumentSemantic.Assign)]
		Matrix4 ModelViewMatrix { [Align (16)] get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("projectionMatrix", ArgumentSemantic.Assign)]
		Matrix4 ProjectionMatrix { [Align (16)] get; set; }
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/GLKit/GLKMesh">Apple documentation for <c>GLKMesh</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // - (nullable instancetype)init NS_UNAVAILABLE;
	interface GLKMesh {
		/// <param name="mesh">To be added.</param>
		/// <param name="error">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithMesh:error:")]
		NativeHandle Constructor (MDLMesh mesh, out NSError error);

		// generator does not like `out []` -> https://trello.com/c/sZYNalbB/524-generator-support-out
		[Internal] // there's another, manual, public API exposed
		[Static]
		[Export ("newMeshesFromAsset:sourceMeshes:error:")]
		[return: NullAllowed]
		GLKMesh [] FromAsset (MDLAsset asset, out NSArray sourceMeshes, out NSError error);

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("vertexCount")]
		nuint VertexCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("vertexBuffers")]
		GLKMeshBuffer [] VertexBuffers { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("vertexDescriptor")]
		MDLVertexDescriptor VertexDescriptor { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("submeshes")]
		GLKSubmesh [] Submeshes { get; }

		[Export ("name")]
		string Name { get; }
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/GLKit/GLKMeshBuffer">Apple documentation for <c>GLKMeshBuffer</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GLKMeshBuffer : MDLMeshBuffer {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("glBufferName")]
		uint GlBufferName { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("offset")]
		nuint Offset { get; }
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/GLKit/GLKMeshBufferAllocator">Apple documentation for <c>GLKMeshBufferAllocator</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface GLKMeshBufferAllocator : MDLMeshBufferAllocator {
	}

	/// <summary>A class that allows pre-drawing initialization for an effect.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKNamedEffect_ProtocolRef/index.html">Apple documentation for <c>GLKNamedEffect</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface GLKNamedEffect {
		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Abstract]
		[Export ("prepareToDraw")]
		void PrepareToDraw ();
	}

	/// <summary>A type of <see cref="GLKit.GLKBaseEffect" /> that has a reflection-mapping texturing stage.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKReflectionEffect_ClassRef/index.html">Apple documentation for <c>GLKReflectionMapEffect</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (GLKBaseEffect))]
	interface GLKReflectionMapEffect : GLKNamedEffect {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("textureCubeMap")]
		GLKEffectPropertyTexture TextureCubeMap { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("matrix", ArgumentSemantic.Assign)]
		Matrix3 Matrix { get; set; }
	}

	/// <summary>A skybox effect.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKSkyboxEffect_ClassRef/index.html">Apple documentation for <c>GLKSkyboxEffect</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	interface GLKSkyboxEffect : GLKNamedEffect {
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("center", ArgumentSemantic.Assign)]
		Vector3 Center { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("xSize", ArgumentSemantic.Assign)]
		float XSize { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("ySize", ArgumentSemantic.Assign)]
		float YSize { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("zSize", ArgumentSemantic.Assign)]
		float ZSize { get; set; } /* GLfloat = float */

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("textureCubeMap")]
		GLKEffectPropertyTexture TextureCubeMap { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("transform")]
		GLKEffectPropertyTransform Transform { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("label", ArgumentSemantic.Copy)]
		[DisableZeroCopy]
		string Label { get; set; }

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("draw")]
		void Draw ();
	}

	/// <related type="externalDocumentation" href="https://developer.apple.com/reference/GLKit/GLKSubmesh">Apple documentation for <c>GLKSubmesh</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // (nullable instancetype)init NS_UNAVAILABLE;
	interface GLKSubmesh {
		// Problematic, OpenTK has this define in 3 namespaces in:
		// OpenTK.Graphics.ES11.DataType
		// OpenTK.Graphics.ES20.DataType
		// OpenTK.Graphics.ES30.DataType
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("type")]
		uint Type { get; }

		//  Problematic, OpenTK has this define in 3 namespaces in:
		// OpenTK.Graphics.ES11.BeginMode
		// OpenTK.Graphics.ES20.BeginMode
		// OpenTK.Graphics.ES30.BeginMode
		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("mode")]
		uint Mode { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("elementCount")]
		int ElementCount { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("elementBuffer")]
		GLKMeshBuffer ElementBuffer { get; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed, Export ("mesh", ArgumentSemantic.Weak)]
		GLKMesh Mesh { get; }

		[Export ("name")]
		string Name { get; }
	}

	/// <summary>Encapsulates the information relating to a texture.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKTextureInfo_Ref/index.html">Apple documentation for <c>GLKTextureInfo</c></related>
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	interface GLKTextureInfo : NSCopying {
		[Export ("width")]
		int Width { get; } /* GLuint = uint32_t */

		[Export ("height")]
		int Height { get; } /* GLuint = uint32_t */

		[Export ("alphaState")]
		GLKTextureInfoAlphaState AlphaState { get; }

		[Export ("textureOrigin")]
		GLKTextureInfoOrigin TextureOrigin { get; }

		[Export ("containsMipmaps")]
		bool ContainsMipmaps { get; }

		[Export ("name")]
		uint Name { get; } /* GLuint = uint32_t */

		[Export ("target")]
		GLKTextureTarget Target { get; }

		[Export ("mimapLevelCount")]
		uint MimapLevelCount { get; }

		[Export ("arrayLength")]
		uint ArrayLength { get; }

		[Export ("depth")]
		uint Depth { get; }
	}

	/// <param name="textureInfo">The infromation about the texture loaded, or null on error.</param>
	///     <param name="error">On success, this value is null.   Otherwise it contains the error information.</param>
	///     <summary>Signature used by the asynchrous texture loading methods in <see cref="GLKit.GLKTextureLoader" />.</summary>
	delegate void GLKTextureLoaderCallback ([NullAllowed] GLKTextureInfo textureInfo, [NullAllowed] NSError error);

	/// <include file="../docs/api/GLKit/GLKTextureLoader.xml" path="/Documentation/Docs[@DocId='T:GLKit.GLKTextureLoader']/*" />
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.MacOSX, 10, 14, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	interface GLKTextureLoader {
		/// <param name="path">File name where the data will be loaded from.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a texture from a file synchronously.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("textureWithContentsOfFile:options:error:")]
		[return: NullAllowed]
		GLKTextureInfo FromFile (string path, [NullAllowed] NSDictionary textureOperations, out NSError error);

		/// <param name="url">URL pointing to the texture to load.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a texture from a file pointed to by the url.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("textureWithContentsOfURL:options:error:")]
		[return: NullAllowed]
		GLKTextureInfo FromUrl (NSUrl url, [NullAllowed] NSDictionary textureOperations, out NSError error);

		/// <param name="data">NSData object that contains the bitmap that will be loaded into the texture.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a texture from an NSData source.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("textureWithContentsOfData:options:error:")]
		[return: NullAllowed]
		GLKTextureInfo FromData (NSData data, [NullAllowed] NSDictionary textureOperations, out NSError error);

		/// <param name="cgImage">CGImage that contains the image to be loaded into the texture.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a texture from a CGImage.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo be added.</returns>
		///         <remarks>
		///         </remarks>
		[Static]
		[Export ("textureWithCGImage:options:error:")]
		[return: NullAllowed]
		GLKTextureInfo FromImage (CGImage cgImage, [NullAllowed] NSDictionary textureOperations, out NSError error);

		[Static]
		[Export ("cubeMapWithContentsOfFiles:options:error:"), Internal]
		[return: NullAllowed]
		GLKTextureInfo CubeMapFromFiles (NSArray paths, [NullAllowed] NSDictionary textureOperations, out NSError error);

		/// <param name="path">The file that contains the texture.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a cube map synchronously.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("cubeMapWithContentsOfFile:options:error:")]
		[return: NullAllowed]
		GLKTextureInfo CubeMapFromFile (string path, [NullAllowed] NSDictionary textureOperations, out NSError error);

		/// <param name="url">URL pointing to the texture to load.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="error">Error result.</param>
		///         <summary>Loads a cube map synchronously.</summary>
		///         <returns>On error, this will return null, the details of the error will be stored in the NSError parameter.   Otherwise the instance of the GLKTextureInfo.</returns>
		///         <remarks>To be added.</remarks>
		[Static]
		[Export ("cubeMapWithContentsOfURL:options:error:")]
		[return: NullAllowed]
		GLKTextureInfo CubeMapFromUrl (NSUrl url, [NullAllowed] NSDictionary textureOperations, out NSError error);

		/// <param name="name">To be added.</param>
		/// <param name="scaleFactor">To be added.</param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="outError">To be added.</param>
		/// <summary>To be added.</summary>
		/// <returns>To be added.</returns>
		/// <remarks>To be added.</remarks>
		[Static]
		[Export ("textureWithName:scaleFactor:bundle:options:error:")]
		[return: NullAllowed]
		GLKTextureInfo FromName (string name, nfloat scaleFactor, [NullAllowed] NSBundle bundle, [NullAllowed] NSDictionary<NSString, NSNumber> options, out NSError outError);

		/// <param name="context">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[NoiOS]
		[NoMacCatalyst]
		[NoTV]
		[Export ("initWithShareContext:")]
		NativeHandle Constructor (NSOpenGLContext context);

		/// <param name="sharegroup">Share context where the textures will be loaded.</param>
		/// <summary>Creates a GLKTextureLoader for an EAGLSharegroup, used for asynchronous texture loading.</summary>
		/// <remarks>
		///         </remarks>
		[NoMac]
		[Export ("initWithSharegroup:")]
		NativeHandle Constructor (EAGLSharegroup sharegroup);

		/// <param name="file">The file that contains the texture.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="queue">
		///           <para>The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a texture.</summary>
		///         <remarks>
		///         </remarks>
		[Export ("textureWithContentsOfFile:options:queue:completionHandler:")]
		[Async (XmlDocs = """
			<param name="file">The file that contains the texture.</param>
			<param name="textureOperations">An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.This parameter can be .</param>
			<param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.This parameter can be .</param>
			<summary>Asynchronously loads a texture.</summary>
			<returns>
			          <para>A task that represents the asynchronous BeginTextureLoad operation.   The value of the TResult parameter is a <see cref="GLKit.GLKTextureLoaderCallback" />.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void BeginTextureLoad (string file, [NullAllowed] NSDictionary textureOperations, [NullAllowed] DispatchQueue queue, GLKTextureLoaderCallback onComplete);

		/// <param name="filePath">The file that contains the texture.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="queue">
		///           <para>The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a texture.</summary>
		///         <remarks>
		///         </remarks>
		[Export ("textureWithContentsOfURL:options:queue:completionHandler:")]
		[Async (XmlDocs = """
			<param name="filePath">The file that contains the texture.</param>
			<param name="textureOperations">An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.This parameter can be .</param>
			<param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.This parameter can be .</param>
			<summary>Asynchronously loads a texture.</summary>
			<returns>
			          <para>A task that represents the asynchronous BeginTextureLoad operation.   The value of the TResult parameter is a <see cref="GLKit.GLKTextureLoaderCallback" />.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void BeginTextureLoad (NSUrl filePath, [NullAllowed] NSDictionary textureOperations, [NullAllowed] DispatchQueue queue, GLKTextureLoaderCallback onComplete);

		/// <param name="data">NSData object that contains the bitmap that will be loaded into the texture.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="queue">
		///           <para>The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a texture.</summary>
		///         <remarks>
		///         </remarks>
		[Export ("textureWithContentsOfData:options:queue:completionHandler:")]
		[Async (XmlDocs = """
			<param name="data">NSData object that contains the bitmap that will be loaded into the texture.</param>
			<param name="textureOperations">An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.This parameter can be .</param>
			<param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.This parameter can be .</param>
			<summary>Asynchronously loads a texture.</summary>
			<returns>
			          <para>A task that represents the asynchronous BeginTextureLoad operation.   The value of the TResult parameter is a <see cref="GLKit.GLKTextureLoaderCallback" />.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void BeginTextureLoad (NSData data, [NullAllowed] NSDictionary textureOperations, [NullAllowed] DispatchQueue queue, GLKTextureLoaderCallback onComplete);

		/// <param name="image">CGImage that contains the image to be loaded into the texture.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="queue">
		///           <para>The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a texture.</summary>
		///         <remarks>
		///         </remarks>
		[Export ("textureWithCGImage:options:queue:completionHandler:")]
		[Async (XmlDocs = """
			<param name="image">CGImage that contains the image to be loaded into the texture.</param>
			<param name="textureOperations">An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.This parameter can be .</param>
			<param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.This parameter can be .</param>
			<summary>Asynchronously loads a texture.</summary>
			<returns>
			          <para>A task that represents the asynchronous BeginTextureLoad operation.   The value of the TResult parameter is a <see cref="GLKit.GLKTextureLoaderCallback" />.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void BeginTextureLoad (CGImage image, [NullAllowed] NSDictionary textureOperations, [NullAllowed] DispatchQueue queue, GLKTextureLoaderCallback onComplete);

		[Export ("cubeMapWithContentsOfFiles:options:queue:completionHandler:"), Internal]
		[Async]
		void BeginLoadCubeMap (NSArray filePaths, [NullAllowed] NSDictionary textureOperations, [NullAllowed] DispatchQueue queue, GLKTextureLoaderCallback onComplete);

		/// <param name="fileName">File name where the data will be loaded from.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="queue">
		///           <para>The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a cube map.</summary>
		///         <remarks>
		///         </remarks>
		[Export ("cubeMapWithContentsOfFile:options:queue:completionHandler:")]
		[Async (XmlDocs = """
			<param name="fileName">File name where the data will be loaded from.</param>
			<param name="textureOperations">An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.This parameter can be .</param>
			<param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.This parameter can be .</param>
			<summary>Asynchronously loads a cube map.</summary>
			<returns>
			          <para>A task that represents the asynchronous BeginLoadCubeMap operation.   The value of the TResult parameter is a <see cref="GLKit.GLKTextureLoaderCallback" />.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void BeginLoadCubeMap (string fileName, [NullAllowed] NSDictionary textureOperations, [NullAllowed] DispatchQueue queue, GLKTextureLoaderCallback onComplete);

		/// <param name="filePath">The file that contains the texture.</param>
		///         <param name="textureOperations">
		///           <para>An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="queue">
		///           <para>The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		///         <param name="onComplete">Callback to invoke when the texture is loaded.   The callback receives a GLKTextureInfo and an NSError.</param>
		///         <summary>Asynchronously loads a cube map.</summary>
		///         <remarks>
		///         </remarks>
		[Export ("cubeMapWithContentsOfURL:options:queue:completionHandler:")]
		[Async (XmlDocs = """
			<param name="filePath">The file that contains the texture.</param>
			<param name="textureOperations">An NSDictionary populated with configuration options.   Alternatively, use the strongly-typed version of this method that takes a GLKTextureOperations object.This parameter can be .</param>
			<param name="queue">The queue on which the callback method will be invoked, or null to invoke the method on the main dispatch queue.This parameter can be .</param>
			<summary>Asynchronously loads a cube map.</summary>
			<returns>
			          <para>A task that represents the asynchronous BeginLoadCubeMap operation.   The value of the TResult parameter is a <see cref="GLKit.GLKTextureLoaderCallback" />.</para>
			        </returns>
			<remarks>To be added.</remarks>
			""")]
		void BeginLoadCubeMap (NSUrl filePath, [NullAllowed] NSDictionary textureOperations, [NullAllowed] DispatchQueue queue, GLKTextureLoaderCallback onComplete);

		/// <param name="name">To be added.</param>
		/// <param name="scaleFactor">To be added.</param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="options">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="queue">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="block">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("textureWithName:scaleFactor:bundle:options:queue:completionHandler:")]
		[Async (XmlDocs = """
			<param name="name">To be added.</param>
			<param name="scaleFactor">To be added.</param>
			<param name="bundle">To be added.</param>
			<param name="options">To be added.</param>
			<param name="queue">To be added.</param>
			<summary>To be added.</summary>
			<returns>
			          <para>A task that represents the asynchronous BeginTextureLoad operation.   The value of the TResult parameter is a <see cref="GLKit.GLKTextureLoaderCallback" />.</para>
			        </returns>
			<remarks>
			          <para copied="true">The BeginTextureLoadAsync method is suitable to be used with C# async by returning control to the caller with a Task representing the operation.</para>
			          <para copied="true">To be added.</para>
			        </remarks>
			""")]
		void BeginTextureLoad (string name, nfloat scaleFactor, [NullAllowed] NSBundle bundle, [NullAllowed] NSDictionary<NSString, NSNumber> options, [NullAllowed] DispatchQueue queue, GLKTextureLoaderCallback block);

		/// <summary>Represents the value associated with the constant GLKTextureLoaderApplyPremultiplication</summary>
		///         <value>
		///         </value>
		///         <remarks>Used as a key for textureOperations if you are using the NSDictionary overloads instead of the strongly typed GLKTextureOperations class.</remarks>
		[Field ("GLKTextureLoaderApplyPremultiplication")]
		NSString ApplyPremultiplication { get; }

		/// <summary>Represents the value associated with the constant GLKTextureLoaderGenerateMipmaps</summary>
		///         <value>
		///         </value>
		///         <remarks>Used as a key for textureOperations if you are using the NSDictionary overloads instead of the strongly typed GLKTextureOperations class.</remarks>
		[Field ("GLKTextureLoaderGenerateMipmaps")]
		NSString GenerateMipmaps { get; }

		/// <summary>Represents the value associated with the constant GLKTextureLoaderOriginBottomLeft</summary>
		///         <value>
		///         </value>
		///         <remarks>Used as a key for textureOperations if you are using the NSDictionary overloads instead of the strongly typed GLKTextureOperations class.</remarks>
		[Field ("GLKTextureLoaderOriginBottomLeft")]
		NSString OriginBottomLeft { get; }

		/// <summary>Represents the value associated with the constant GLKTextureLoaderGrayscaleAsAlpha</summary>
		///         <value>
		///         </value>
		///         <remarks>Used as a key for textureOperations if you are using the NSDictionary overloads instead of the strongly typed GLKTextureOperations class.</remarks>
		[Field ("GLKTextureLoaderGrayscaleAsAlpha")]
		NSString GrayscaleAsAlpha { get; }

		/// <summary>Represents the value associated with the constant GLKTextureLoaderSRGB</summary>
		///         <value>
		///         </value>
		///         <remarks>To be added.</remarks>
		[Field ("GLKTextureLoaderSRGB")]
		NSString SRGB { get; }

		/// <summary>Represents the value associated with the constant GLKTextureLoaderErrorDomain</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("GLKTextureLoaderErrorDomain")]
		NSString ErrorDomain { get; }

		/// <summary>Represents the value associated with the constant GLKTextureLoaderErrorKey</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("GLKTextureLoaderErrorKey")]
		NSString ErrorKey { get; }

		/// <summary>Represents the value associated with the constant GLKTextureLoaderGLErrorKey</summary>
		///         <value>
		///         </value>
		///         <remarks>
		///         </remarks>
		[Field ("GLKTextureLoaderGLErrorKey")]
		NSString GLErrorKey { get; }
	}

	/// <summary>A <see cref="UIKit.UIView" /> that supports OpenGL ES rendering.</summary>
	///     
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKView_ClassReference/index.html">Apple documentation for <c>GLKView</c></related>
	[NoMac]
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[BaseType (typeof (UIView), Delegates = new string [] { "WeakDelegate" }, Events = new Type [] { typeof (GLKViewDelegate) })]
	interface GLKView {
		/// <param name="frame">Frame used by the view, expressed in iOS points.</param>
		/// <summary>Initializes the GLKView with the specified frame.</summary>
		/// <remarks>
		///           <para>This constructor is used to programmatically create a new instance of GLKView with the specified dimension in the frame.   The object will only be displayed once it has been added to a view hierarchy by calling AddSubview in a containing view.</para>
		///           <para>This constructor is not invoked when deserializing objects from storyboards or XIB filesinstead the constructor that takes an NSCoder parameter is invoked.</para>
		///         </remarks>
		[Export ("initWithFrame:")]
		NativeHandle Constructor (CGRect frame);

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the GLKit.IGLKViewDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the GLKit.IGLKViewDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IGLKViewDelegate Delegate { get; set; }

		/// <summary>To be added.</summary>
		///         <value>
		///           <para>(More documentation for this node is coming)</para>
		///           <para tool="nullallowed">This value can be <see langword="null" />.</para>
		///         </value>
		///         <remarks>To be added.</remarks>
		[NullAllowed] // by default this property is null
		[Export ("context", ArgumentSemantic.Retain)]
		EAGLContext Context { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("drawableWidth")]
		nint DrawableWidth { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("drawableHeight")]
		nint DrawableHeight { get; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("drawableColorFormat")]
		GLKViewDrawableColorFormat DrawableColorFormat { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("drawableDepthFormat")]
		GLKViewDrawableDepthFormat DrawableDepthFormat { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("drawableStencilFormat")]
		GLKViewDrawableStencilFormat DrawableStencilFormat { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("drawableMultisample")]
		GLKViewDrawableMultisample DrawableMultisample { get; set; }

		/// <summary>To be added.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("enableSetNeedsDisplay")]
		bool EnableSetNeedsDisplay { get; set; }

		/// <param name="frame">To be added.</param>
		/// <param name="context">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithFrame:context:")]
		NativeHandle Constructor (CGRect frame, EAGLContext context);

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("bindDrawable")]
		void BindDrawable ();

		/// <summary>To be added.</summary>
		///         <returns>To be added.</returns>
		///         <remarks>To be added.</remarks>
		[Export ("snapshot")]
		UIImage Snapshot ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("display")]
		void Display ();

		/// <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		[Export ("deleteDrawable")]
		void DeleteDrawable ();
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="GLKit.GLKViewDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="GLKit.GLKViewDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="GLKit.GLKViewDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="GLKit.GLKViewDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IGLKViewDelegate { }

	/// <summary>A class that acts like a delegate object for instances of <see cref="GLKit.GLKView" />.</summary>
	///     <remarks>
	///       <para>The specific use-case supported by this class is to customize the <see cref="GLKit.IGLKViewDelegate.DrawInRect(GLKit.GLKView,CoreGraphics.CGRect)" /> method without subclassing <see cref="GLKit.GLKView" />.</para>
	///     </remarks>
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKViewDelegate_ProtocolRef/index.html">Apple documentation for <c>GLKViewDelegate</c></related>
	[NoMac]
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface GLKViewDelegate {
		/// <param name="view">To be added.</param>
		/// <param name="rect">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("glkView:drawInRect:"), EventArgs ("GLKViewDraw", XmlDocs = """
			<summary>Event raised by the object.</summary>
			<remarks>If developers do not assign a value to this event, this will reset the value for the WeakDelegate property to an internal handler that maps delegates to events.</remarks>
			""")]
		void DrawInRect (GLKView view, CGRect rect);
	}

	/// <include file="../docs/api/GLKit/GLKViewController.xml" path="/Documentation/Docs[@DocId='T:GLKit.GLKViewController']/*" />
	[NoMac]
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[BaseType (typeof (UIViewController))]
	interface GLKViewController : GLKViewDelegate {
		/// <param name="nibName">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <param name="bundle">
		///           <para>To be added.</para>
		///           <para tool="nullallowed">This parameter can be <see langword="null" />.</para>
		///         </param>
		/// <summary>Creates a new <see cref="GLKit.GLKViewController" /> from the specified Nib name in the specified <paramref name="bundle" />.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("initWithNibName:bundle:")]
		[PostGet ("NibBundle")]
		NativeHandle Constructor ([NullAllowed] string nibName, [NullAllowed] NSBundle bundle);

		/// <summary>The desired number of frames per second.   Controls the frequency that your Update and Draw methods will be called.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("preferredFramesPerSecond")]
		nint PreferredFramesPerSecond { get; set; }

		/// <summary>The actual frames per second that your application is getting.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("framesPerSecond")]
		nint FramesPerSecond { get; }

		/// <summary>When paused, the Update and Draw methods are not invoked.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("paused")]
		bool Paused { [Bind ("isPaused")] get; set; }

		/// <summary>Cumulative count of frames displayed.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("framesDisplayed")]
		nint FramesDisplayed { get; }

		/// <summary>Gets the time interval, in seconds, since the view controller first resumed sending updates.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("timeSinceFirstResume")]
		double TimeSinceFirstResume { get; }

		/// <summary>Gets the time interval, in seconds, since the view controller most recently resumed sending updates.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("timeSinceLastResume")]
		double TimeSinceLastResume { get; }

		[Export ("timeSinceLastUpdate")]
		double TimeSinceLastUpdate { get; }

		[Export ("timeSinceLastDraw")]
		double TimeSinceLastDraw { get; }

		/// <summary>Gets or sets a Boolean value that controls whether the rendering loop will pause when the application resigns from the active state.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("pauseOnWillResignActive")]
		bool PauseOnWillResignActive { get; set; }

		/// <summary>Gets or sets a Boolean value that controls whether the rendering loop will resume when the application enters the active state.</summary>
		///         <value>To be added.</value>
		///         <remarks>To be added.</remarks>
		[Export ("resumeOnDidBecomeActive")]
		bool ResumeOnDidBecomeActive { get; set; }

		/// <summary>An object that can respond to the delegate protocol for this type</summary>
		///         <value>The instance that will respond to events and data requests.</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>   Methods must be decorated with the [Export ("selectorName")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>
		///         </remarks>
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		/// <summary>An instance of the GLKit.IGLKViewControllerDelegate model class which acts as the class delegate.</summary>
		///         <value>The instance of the GLKit.IGLKViewControllerDelegate model class</value>
		///         <remarks>
		///           <para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>
		///           <para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>
		///           <para>This is the strongly typed version of the object, developers should use the WeakDelegate property instead if they want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>
		///         </remarks>
		[Wrap ("WeakDelegate")]
		IGLKViewControllerDelegate Delegate { get; set; }

		// Pseudo-documented, if the user overrides it, call this instead of the delegate method
		/// <include file="../docs/api/GLKit/GLKViewController.xml" path="/Documentation/Docs[@DocId='M:GLKit.GLKViewController.Update']/*" />
		[Export ("update")]
		void Update ();
	}

	/// <summary>Interface representing the required methods (if any) of the protocol <see cref="GLKit.GLKViewControllerDelegate" />.</summary>
	///     <remarks>
	///       <para>This interface contains the required methods (if any) from the protocol defined by <see cref="GLKit.GLKViewControllerDelegate" />.</para>
	///       <para>If developers create classes that implement this interface, the implementation methods will automatically be exported to Objective-C with the matching signature from the method defined in the <see cref="GLKit.GLKViewControllerDelegate" /> protocol.</para>
	///       <para>Optional methods (if any) are provided by the <see cref="GLKit.GLKViewControllerDelegate_Extensions" /> class as extension methods to the interface, allowing developers to invoke any optional methods on the protocol.</para>
	///     </remarks>
	interface IGLKViewControllerDelegate { }

	/// <summary>A delegate object that gives the application developer fine-grained control over events relating to the life-cycle of a <see cref="GLKit.GLKViewController" /> object.</summary>
	///     
	///     <related type="externalDocumentation" href="https://developer.apple.com/library/ios/documentation/GLkit/Reference/GLKViewControllerDelegate_ProtocolRef/index.html">Apple documentation for <c>GLKViewControllerDelegate</c></related>
	[NoMac]
	[Deprecated (PlatformName.iOS, 12, 0, message: "Use 'Metal' instead.")]
	[Deprecated (PlatformName.TvOS, 12, 0, message: "Use 'Metal' instead.")]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface GLKViewControllerDelegate {
		/// <param name="controller">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Abstract]
		[Export ("glkViewControllerUpdate:")]
		void Update (GLKViewController controller);

		/// <param name="controller">To be added.</param>
		/// <param name="pause">To be added.</param>
		/// <summary>To be added.</summary>
		/// <remarks>To be added.</remarks>
		[Export ("glkViewController:willPause:")]
		void WillPause (GLKViewController controller, bool pause);
	}
}
