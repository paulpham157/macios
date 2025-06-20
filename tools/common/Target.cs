// Copyright 2013--2014 Xamarin Inc. All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


using Mono.Cecil;
using Mono.Tuner;
using Mono.Linker;
using Xamarin.Linker;

using Xamarin.Utils;
using Registrar;
using ObjCRuntime;

#if MONOTOUCH
using MonoTouch;
using MonoTouch.Tuner;
using PlatformResolver = MonoTouch.Tuner.MonoTouchResolver;
using PlatformLinkContext = Xamarin.Tuner.DerivedLinkContext;
#elif MMP
using MonoMac.Tuner;
using PlatformResolver = Xamarin.Bundler.MonoMacResolver;
using PlatformLinkContext = Xamarin.Tuner.DerivedLinkContext;
#elif NET
using LinkerOptions = Xamarin.Linker.LinkerConfiguration;
using PlatformLinkContext = Xamarin.Tuner.DerivedLinkContext;
using PlatformResolver = Xamarin.Linker.DotNetResolver;
#else
#error Invalid defines
#endif

// Disable until we get around to enable + fix any issues.
#nullable disable

namespace Xamarin.Bundler {
	public partial class Target {
		public Application App;
		public AssemblyCollection Assemblies = new AssemblyCollection (); // The root assembly is not in this list.

		public PlatformLinkContext LinkContext;
		public LinkerOptions LinkerOptions;
		public PlatformResolver Resolver = new PlatformResolver ();

		public HashSet<string> Frameworks = new HashSet<string> ();
		public HashSet<string> WeakFrameworks = new HashSet<string> ();

		internal StaticRegistrar StaticRegistrar { get; set; }

		// If we didn't link because the existing (cached) assemblyes are up-to-date.
		bool cached_link = false;

#if !MMP
		Symbols dynamic_symbols;
#endif // MMP

		// Note that each 'Target' can have multiple abis: armv7+armv7s for instance.
		public List<Abi> Abis;

		// If we're targetting a 32 bit arch for this target.
		bool? is32bits;
		public bool Is32Build {
			get {
				if (!is32bits.HasValue)
					is32bits = Application.IsArchEnabled (Abis, Abi.Arch32Mask);
				return is32bits.Value;
			}
		}

		// If we're targetting a 64 bit arch for this target.
		bool? is64bits;
		public bool Is64Build {
			get {
				if (!is64bits.HasValue)
					is64bits = Application.IsArchEnabled (Abis, Abi.Arch64Mask);
				return is64bits.Value;
			}
		}

		public Target (Application app)
		{
			this.App = app;
			this.StaticRegistrar = new StaticRegistrar (this);
		}

		// If this is an app extension, this returns the equivalent (32/64bit) target for the container app.
		// This may be null (it's possible to build an extension for 32+64bit, and the main app only for 64-bit, for instance.
		public Target ContainerTarget {
			get {
				return App.ContainerApp.Targets.FirstOrDefault ((v) => v.Is32Build == Is32Build);
			}
		}

		public Assembly AddAssembly (AssemblyDefinition assembly)
		{
			var asm = new Assembly (this, assembly);
			Assemblies.Add (asm);
			return asm;
		}

		// This will find the link context, possibly looking in container targets.
		public PlatformLinkContext GetLinkContext ()
		{
			if (LinkContext is not null)
				return LinkContext;
			if (App.IsExtension && App.IsCodeShared)
				return ContainerTarget.GetLinkContext ();
			return null;
		}

		public bool CachedLink {
			get {
				return cached_link;
			}
		}

		public void ExtractNativeLinkInfo (List<Exception> exceptions)
		{
			foreach (var a in Assemblies) {
				try {
					a.ExtractNativeLinkInfo ();
				} catch (Exception e) {
					exceptions.Add (e);
				}
			}
		}

		[DllImport (Constants.libSystemLibrary, SetLastError = true)]
		static extern string realpath (string path, IntPtr zero);

		public static string GetRealPath (string path, bool warnIfNoSuchPathExists = true)
		{
			// For some reason realpath doesn't always like filenames only, and will randomly fail.
			// Prepend the current directory if there's no directory specified.
			if (string.IsNullOrEmpty (Path.GetDirectoryName (path)))
				path = Path.Combine (Environment.CurrentDirectory, path);

			var rv = realpath (path, IntPtr.Zero);
			if (rv is not null)
				return rv;

			var errno = Marshal.GetLastWin32Error ();
			if (warnIfNoSuchPathExists || (errno != 2))
				ErrorHelper.Warning (54, Errors.MT0054, path, FileCopier.strerror (errno), errno);
			return path;
		}

		public void ValidateAssembliesBeforeLink ()
		{
			if (App.AreAnyAssembliesTrimmed) {
				foreach (Assembly assembly in Assemblies) {
					if ((assembly.AssemblyDefinition.MainModule.Attributes & ModuleAttributes.ILOnly) == 0)
						throw ErrorHelper.CreateError (2014, Errors.MT2014, assembly.AssemblyDefinition.MainModule.FileName);
				}
			}
		}

		public void ComputeLinkerFlags ()
		{
			foreach (var a in Assemblies)
				a.ComputeLinkerFlags ();
		}

		public void GatherFrameworks ()
		{
			Assembly asm = null;

			foreach (var assembly in Assemblies) {
				if (assembly.AssemblyDefinition.Name.Name == Driver.GetProductAssembly (App)) {
					asm = assembly;
					break;
				}
			}

			if (asm is null)
				throw ErrorHelper.CreateError (99, Errors.MX0099, $"could not find the product assembly {Driver.GetProductAssembly (App)} in the list of assemblies referenced by the executable");

			AssemblyDefinition productAssembly = asm.AssemblyDefinition;

			// *** make sure any change in the above lists (or new list) are also reflected in 
			// *** Makefile so simlauncher-sgen does not miss any framework

			HashSet<string> processed = new HashSet<string> ();
			Version v80 = new Version (8, 0);

			foreach (ModuleDefinition md in productAssembly.Modules) {
				foreach (TypeDefinition td in md.Types) {
					// process only once each namespace (as we keep adding logic below)
					string nspace = td.Namespace;
#if !XAMCORE_5_0
					// AVCustomRoutingControllerDelegate was incorrectly placed in AVKit
					if (td.Is ("AVKit", "AVCustomRoutingControllerDelegate"))
						nspace = "AVRouting";
#endif

					if (processed.Contains (nspace))
						continue;
					processed.Add (nspace);

					Framework framework;
					if (Driver.GetFrameworks (App).TryGetValue (nspace, out framework)) {
						// framework specific processing
						switch (framework.Name) {
						case "CoreAudioKit":
							// CoreAudioKit seems to be functional in the iOS 9 simulator.
							if (App.IsSimulatorBuild && App.SdkVersion.Major < 9)
								continue;
							break;
						case "Metal":
						case "MetalKit":
						case "MetalPerformanceShaders":
						case "PHASE":
						case "ThreadNetwork":
							// some frameworks do not exists on simulators and will result in linker errors if we include them
							if (App.IsSimulatorBuild)
								continue;
							break;
						case "DeviceCheck":
							if (App.IsSimulatorBuild && App.SdkVersion.Major < 13)
								continue;
							break;
						case "PushKit":
							// in Xcode 6 beta 7 this became an (ld) error - it was a warning earlier :(
							// ld: embedded dylibs/frameworks are only supported on iOS 8.0 and later (@rpath/PushKit.framework/PushKit) for architecture armv7
							// this was fixed in Xcode 6.2 (6.1 was still buggy) see #29786
							if ((App.DeploymentTarget < v80) && (Driver.XcodeVersion < new Version (6, 2))) {
								ErrorHelper.Warning (49, Errors.MT0049, framework.Name);
								continue;
							}
							break;
						case "GameKit":
							if (Driver.XcodeVersion.Major >= 14 && Is32Build) {
								Driver.Log (3, "Not linking with the framework {0} because it's not available when using Xcode 14+ and building for a 32-bit simulator architecture.", framework.Name);
								continue;
							}
							break;
						case "NewsstandKit":
							if (Driver.XcodeVersion.Major >= 15) {
								Driver.Log (3, "Not linking with the framework {0} because it's not available when using Xcode 15+.", framework.Name);
								continue;
							}
							break;
						case "AssetsLibrary":
							if (Driver.XcodeVersion.Major >= 16 || (Driver.XcodeVersion.Major == 15 && Driver.XcodeVersion.Minor >= 3)) {
								Driver.Log (3, "Not linking with the framework {0} because it's not available when using Xcode 15.3+.", framework.Name);
								continue;
							}
							break;
						default:
							if (App.IsSimulatorBuild && !App.IsFrameworkAvailableInSimulator (framework.Name)) {
								if (App.AreAnyAssembliesTrimmed) {
									ErrorHelper.Warning (5223, Errors.MX5223, framework.Name, App.PlatformName);
								} else {
									Driver.Log (3, Errors.MX5223, framework.Name, App.PlatformName);
								}
								continue;
							}
							break;
						}

						if (framework.Unavailable) {
							ErrorHelper.Warning (181, Errors.MX0181 /* Not linking with the framework {0} (used by the type {1}) because it's not available on the current platform ({2}). */, framework.Name, td.FullName, App.PlatformName);
							continue;
						}

						if (App.SdkVersion >= framework.Version) {
							var add_to = framework.AlwaysWeakLinked || App.DeploymentTarget < framework.Version ? asm.WeakFrameworks : asm.Frameworks;
							add_to.Add (framework.Name);
							continue;
						} else {
							Driver.Log (3, "Not linking with the framework {0} (used by the type {1}) because it was introduced in {2} {3}, and we're using the {2} {4} SDK.", framework.Name, td.FullName, App.PlatformName, framework.Version, App.SdkVersion);
						}
					}
				}
			}

			// Make sure there are no duplicates between frameworks and weak frameworks.
			// Keep the weak ones.
			asm.Frameworks.ExceptWith (asm.WeakFrameworks);
		}

		internal static void PrintAssemblyReferences (AssemblyDefinition assembly)
		{
			if (Driver.Verbosity < 2)
				return;

			var main = assembly.MainModule;
			Driver.Log ($"Loaded assembly '{assembly.FullName}' from {StringUtils.Quote (assembly.MainModule.FileName)}");
			foreach (var ar in main.AssemblyReferences)
				Driver.Log ($"    References: '{ar.FullName}'");
		}

#if !MMP
		public Symbols GetAllSymbols ()
		{
			CollectAllSymbols ();
			return dynamic_symbols;
		}

		public void CollectAllSymbols ()
		{
			if (dynamic_symbols is not null)
				return;

			var dyn_msgSend_functions = new [] {
				new { Name = "xamarin_dyn_objc_msgSend", ValidAbis = Abi.SimulatorArchMask | Abi.ARM64 },
				new { Name = "xamarin_dyn_objc_msgSendSuper", ValidAbis = Abi.SimulatorArchMask | Abi.ARM64 },
				new { Name = "xamarin_dyn_objc_msgSend_stret", ValidAbis = Abi.SimulatorArchMask },
				new { Name = "xamarin_dyn_objc_msgSendSuper_stret", ValidAbis = Abi.SimulatorArchMask },
			};

			var cache_location = Path.Combine (App.Cache.Location, "entry-points.txt");
			if (cached_link) {
				dynamic_symbols = new Symbols ();
				dynamic_symbols.Load (cache_location, this);
			} else {
				if (LinkContext is null) {
					// This happens when using the simlauncher and the msbuild tasks asked for a list
					// of symbols (--symbollist). In that case just produce an empty list, since the
					// binary shouldn't end up stripped anyway.
					dynamic_symbols = new Symbols ();
				} else {
					dynamic_symbols = LinkContext.RequiredSymbols;
				}

				// keep the debugging helper in debugging binaries only
				var has_mono_pmip = App.EnableDebug;
#if MMP
				has_mono_pmip &= !Driver.IsUnifiedFullSystemFramework;
#endif
				if (has_mono_pmip)
					dynamic_symbols.AddFunction ("mono_pmip");

				bool has_dyn_msgSend;

				switch (App.Platform) {
				case ApplePlatform.iOS:
				case ApplePlatform.TVOS:
					has_dyn_msgSend = App.IsSimulatorBuild || App.MarshalObjectiveCExceptions == MarshalObjectiveCExceptionMode.UnwindManagedCode;
					break;
				case ApplePlatform.MacCatalyst:
				case ApplePlatform.MacOSX:
					has_dyn_msgSend = App.MarshalObjectiveCExceptions != MarshalObjectiveCExceptionMode.Disable && !App.RequiresPInvokeWrappers;
					break;
				default:
					throw ErrorHelper.CreateError (71, Errors.MX0071, App.Platform, App.ProductName);
				}

				if (has_dyn_msgSend) {
					foreach (var dyn_msgSend_function in dyn_msgSend_functions)
						dynamic_symbols.AddFunction (dyn_msgSend_function.Name);
				}

#if MONOTOUCH
				if (App.EnableDiagnostics && App.LibProfilerLinkMode == AssemblyBuildTarget.StaticObject)
					dynamic_symbols.AddFunction ("mono_profiler_init_log");
#endif

				dynamic_symbols.Save (cache_location);
			}

			foreach (var dynamicFunction in dyn_msgSend_functions) {
				var symbol = dynamic_symbols.Find (dynamicFunction.Name);
				if (symbol is not null) {
					symbol.ValidAbis = dynamicFunction.ValidAbis;
				}
			}

			foreach (var name in App.IgnoredSymbols) {
				var symbol = dynamic_symbols.Find (name);
				if (symbol is null) {
					ErrorHelper.Warning (5218, Errors.MT5218, StringUtils.Quote (name));
				} else {
					symbol.Ignore = true;
				}
			}
		}
#endif // MMP

		bool IsRequiredSymbol (Symbol symbol, Assembly single_assembly = null, Abi? target_abis = null)
		{
			if (symbol.Ignore)
				return false;

			// If this symbol is only defined for certain abis, verify if there is an abi match
			if (target_abis.HasValue && symbol.ValidAbis.HasValue && (target_abis.Value & symbol.ValidAbis.Value) == 0)
				return false;

			// Check if this symbol is used in the assembly we're filtering to
			if (single_assembly is not null && !symbol.Members.Any ((v) => v.Module.Assembly == single_assembly.AssemblyDefinition))
				return false; // nope, this symbol is not used in the assembly we're using as filter.

			// If we're code-sharing, the managed linker might have found symbols
			// that are not in any of the assemblies in the current app.
			// This occurs because the managed linker processes all the
			// assemblies for all the apps together, but when linking natively
			// we're only linking with the assemblies that actually go into the app.
			if (App.Platform != ApplePlatform.MacOSX && App.IsCodeShared && symbol.Assemblies.Count > 0) {
				// So if this is a symbol related to any assembly, make sure
				// at least one of those assemblies are in the current app.
				if (!symbol.Assemblies.Any ((v) => Assemblies.Contains (v)))
					return false;
			}

			switch (symbol.Type) {
			case SymbolType.Field:
				return true;
			case SymbolType.Function:
				return true;
			case SymbolType.ObjectiveCClass:
				// Objective-C classes are not required when we're using the static registrar and we're not compiling to shared libraries,
				// (because the registrar code is linked into the main app, but not each shared library, 
				// so the registrar code won't keep symbols in the shared libraries).
				if (single_assembly is not null)
					return true;
				return App.Registrar != RegistrarMode.Static;
			default:
				throw ErrorHelper.CreateError (99, Errors.MX0099, $"invalid symbol type {symbol.Type} for symbol {symbol.Name}");
			}
		}

#if !MMP
		public Symbols GetRequiredSymbols (Assembly assembly = null, Abi? target_abis = null)
		{
			CollectAllSymbols ();

			Symbols filtered = new Symbols ();
			foreach (var ep in dynamic_symbols) {
				if (IsRequiredSymbol (ep, assembly, target_abis)) {
					filtered.Add (ep);
				}
			}
			return filtered ?? dynamic_symbols;
		}
#endif // MMP

#if !MMP && !MTOUCH
		internal string GenerateReferencingSource (string reference_m, IEnumerable<Symbol> symbols)
		{
			if (!symbols.Any ()) {
				if (File.Exists (reference_m))
					File.Delete (reference_m);
				return null;
			}
			var sb = new StringBuilder ();
			sb.AppendLine ("#import <Foundation/Foundation.h>");
			foreach (var symbol in symbols) {
				switch (symbol.Type) {
				case SymbolType.Function:
				case SymbolType.Field:
					sb.Append ("extern void * ").Append (symbol.Name).AppendLine (";");
					break;
				case SymbolType.ObjectiveCClass:
					sb.AppendLine ($"@interface {symbol.ObjectiveCName} : NSObject @end");
					break;
				default:
					throw ErrorHelper.CreateError (99, Errors.MX0099, $"invalid symbol type {symbol.Type} for symbol {symbol.Name}");
				}
			}
			sb.AppendLine ("static void __xamarin_symbol_referencer () __attribute__ ((used)) __attribute__ ((optnone));");
			sb.AppendLine ("void __xamarin_symbol_referencer ()");
			sb.AppendLine ("{");
			sb.AppendLine ("\tvoid *value;");
			foreach (var symbol in symbols) {
				switch (symbol.Type) {
				case SymbolType.Function:
				case SymbolType.Field:
					sb.AppendLine ($"\tvalue = {symbol.Name};");
					break;
				case SymbolType.ObjectiveCClass:
					sb.AppendLine ($"\tvalue = [{symbol.ObjectiveCName} class];");
					break;
				default:
					throw ErrorHelper.CreateError (99, Errors.MX0099, $"invalid symbol type {symbol.Type} for symbol {symbol.Name}");
				}
			}
			sb.AppendLine ("}");
			sb.AppendLine ();

			Driver.WriteIfDifferent (reference_m, sb.ToString (), true);

			return reference_m;
		}
#endif // !MMP && !MTOUCH

		// This is to load the symbols for all assemblies, so that we can give better error messages
		// (with file name / line number information).
		public void LoadSymbols ()
		{
			foreach (var a in Assemblies)
				a.LoadSymbols ();
		}

#if !MMP
		public void GenerateMain (ApplePlatform platform, Abi abi, string main_source, IList<string> registration_methods)
		{
			var sb = new StringBuilder ();
			GenerateMain (sb, platform, abi, main_source, registration_methods);
		}

		public void GenerateMain (StringBuilder sb, ApplePlatform platform, Abi abi, string main_source, IList<string> registration_methods)
		{
			try {
				using (var sw = new StringWriter (sb)) {

					if (registration_methods is not null) {
						foreach (var method in registration_methods) {
							sw.Write ("extern \"C\" void ");
							sw.Write (method);
							sw.WriteLine ("();");
						}
						sw.WriteLine ();
					}

					sw.WriteLine ("static void xamarin_invoke_registration_methods ()");
					sw.WriteLine ("{");
					if (registration_methods is not null) {
						for (int i = 0; i < registration_methods.Count; i++) {
							sw.Write ("\t");
							sw.Write (registration_methods [i]);
							sw.WriteLine ("();");
						}
					}
					sw.WriteLine ("}");
					sw.WriteLine ();

					switch (platform) {
					case ApplePlatform.iOS:
					case ApplePlatform.TVOS:
					case ApplePlatform.MacCatalyst:
						GenerateIOSMain (sw, abi);
						break;
					case ApplePlatform.MacOSX:
#if NET && !LEGACY_TOOLS
						GenerateIOSMain (sw, abi);
#else
						GenerateMacMain (sw);
#endif
						break;
					default:
						throw ErrorHelper.CreateError (71, Errors.MX0071, platform, App.ProductName);
					}
				}
				Driver.WriteIfDifferent (main_source, sb.ToString (), true);
			} catch (ProductException) {
				throw;
			} catch (Exception e) {
				throw new ProductException (4001, true, e, Errors.MT4001, main_source);
			}
		}

		void GenerateMacMain (StringWriter sw)
		{
			sw.WriteLine ("#define MONOMAC 1");
			sw.WriteLine ("#include <xamarin/xamarin.h>");
#if !NET || LEGACY_TOOLS
			if (App.Registrar == RegistrarMode.PartialStatic)
				sw.WriteLine ($"extern \"C\" void {StaticRegistrar.GetInitializationMethodName ("Xamarin.Mac")} ();");
#endif
			sw.WriteLine ();
			sw.WriteLine ();
			sw.WriteLine ();
			sw.WriteLine ("extern \"C\" int xammac_setup ()");

			sw.WriteLine ("{");
			if (App.CustomBundleName is not null) {
				sw.WriteLine ("\textern NSString* xamarin_custom_bundle_name;");
				sw.WriteLine ("\txamarin_custom_bundle_name = @\"" + App.CustomBundleName + "\";");
			}
			sw.WriteLine ("\txamarin_executable_name = \"{0}\";", App.AssemblyName);
			if (!App.IsDefaultMarshalManagedExceptionMode)
				sw.WriteLine ("\txamarin_marshal_managed_exception_mode = MarshalManagedExceptionMode{0};", App.MarshalManagedExceptions);
			sw.WriteLine ("\txamarin_marshal_objectivec_exception_mode = MarshalObjectiveCExceptionMode{0};", App.MarshalObjectiveCExceptions);
			if (App.DisableLldbAttach.HasValue ? App.DisableLldbAttach.Value : !App.EnableDebug)
				sw.WriteLine ("\txamarin_disable_lldb_attach = true;");
			if (App.DisableOmitFramePointer ?? App.EnableDebug)
				sw.WriteLine ("\txamarin_disable_omit_fp = true;");
			sw.WriteLine ();

			if (App.EnableDebug)
				sw.WriteLine ("\txamarin_debug_mode = TRUE;");

			sw.WriteLine ($"\tsetenv (\"MONO_GC_PARAMS\", \"{App.MonoGCParams}\", 1);");

			sw.WriteLine ("\txamarin_supports_dynamic_registration = {0};", App.DynamicRegistrationSupported ? "TRUE" : "FALSE");

			sw.WriteLine ("\txamarin_invoke_registration_methods ();");

			sw.WriteLine ("\treturn 0;");
			sw.WriteLine ("}");
			sw.WriteLine ();
		}

		// note: this is executed under Parallel.ForEach
		void GenerateIOSMain (StringWriter sw, Abi abi)
		{
			var app = App;
			var assemblies = Assemblies;
			var assembly_name = App.AssemblyName;
			var assembly_externs = new StringBuilder ();
			var assembly_aot_modules = new StringBuilder ();
			var register_assemblies = new StringBuilder ();
			var assembly_location = new StringBuilder ();
			var assembly_location_count = 0;
			var enable_llvm = (abi & Abi.LLVM) != 0;

			if (app.XamarinRuntime != XamarinRuntime.NativeAOT) {
				register_assemblies.AppendLine ("\tGCHandle exception_gchandle = INVALID_GCHANDLE;");
				foreach (var s in assemblies) {
					if (!s.IsAOTCompiled)
						continue;

					var info = s.AssemblyDefinition.Name.Name;
					info = EncodeAotSymbol (info);
					assembly_externs.Append ("extern void *mono_aot_module_").Append (info).AppendLine ("_info;");
					assembly_aot_modules.Append ("\tmono_aot_register_module (mono_aot_module_").Append (info).AppendLine ("_info);");

					string sname = s.FileName;
					if (assembly_name != sname && IsBoundAssembly (s)) {
						register_assemblies.Append ("\txamarin_open_and_register (\"").Append (sname).Append ("\", &exception_gchandle);").AppendLine ();
						register_assemblies.AppendLine ("\txamarin_process_managed_exception_gchandle (exception_gchandle);");
					}
				}
			}

			var frameworks = assemblies.Where ((a) => a.BuildTarget == AssemblyBuildTarget.Framework)
										.OrderBy ((a) => a.Identity, StringComparer.Ordinal);
			foreach (var asm_fw in frameworks) {
				var asm_name = asm_fw.Identity;
				if (asm_fw.BuildTargetName == asm_name)
					continue; // this is deduceable
				var prefix = string.Empty;
				if (!app.HasFrameworksDirectory && asm_fw.IsCodeShared)
					prefix = "../../";
				var suffix = string.Empty;
				if (app.IsSimulatorBuild)
					suffix = "/simulator";
				assembly_location.AppendFormat ("\t{{ \"{0}\", \"{2}Frameworks/{1}.framework/MonoBundle{3}\" }},\n", asm_name, asm_fw.BuildTargetName, prefix, suffix);
				assembly_location_count++;
			}

			sw.WriteLine ("#include \"xamarin/xamarin.h\"");

			if (assembly_location.Length > 0) {
				sw.WriteLine ();
				sw.WriteLine ("struct AssemblyLocation assembly_location_entries [] = {");
				sw.WriteLine (assembly_location);
				sw.WriteLine ("};");

				sw.WriteLine ();
				sw.WriteLine ("struct AssemblyLocations assembly_locations = {{ {0}, assembly_location_entries }};", assembly_location_count);
			}

			sw.WriteLine ();
			sw.WriteLine (assembly_externs);

			sw.WriteLine ("void xamarin_register_modules_impl ()");
			sw.WriteLine ("{");
			sw.WriteLine (assembly_aot_modules);
			sw.WriteLine ("}");
			sw.WriteLine ();

			sw.WriteLine ("void xamarin_register_assemblies_impl ()");
			sw.WriteLine ("{");
			sw.WriteLine (register_assemblies);
			sw.WriteLine ("}");
			sw.WriteLine ();

			// Burn in a reference to the profiling symbol so that the native linker doesn't remove it
			// On iOS we can pass -u to the native linker, but that doesn't work on tvOS, where
			// we're building with bitcode (even when bitcode is disabled, we still build with the
			// bitcode marker, which makes the linker reject -u).
			if (app.EnableDiagnostics) {
				sw.WriteLine ("extern \"C\" { void mono_profiler_init_log (); }");
				sw.WriteLine ("typedef void (*xamarin_profiler_symbol_def)();");
				sw.WriteLine ("extern xamarin_profiler_symbol_def xamarin_profiler_symbol;");
				sw.WriteLine ("xamarin_profiler_symbol_def xamarin_profiler_symbol = NULL;");
			}

			if (app.UseInterpreter) {
				sw.WriteLine ("extern \"C\" { void mono_ee_interp_init (const char *); }");
				sw.WriteLine ("extern \"C\" { void mono_icall_table_init (void); }");
				sw.WriteLine ("extern \"C\" { void mono_marshal_ilgen_init (void); }");
				sw.WriteLine ("extern \"C\" { void mono_method_builder_ilgen_init (void); }");
				sw.WriteLine ("extern \"C\" { void mono_sgen_mono_ilgen_init (void); }");
			}

#if NET && !LEGACY_TOOLS
			if (app.MonoNativeMode != MonoNativeMode.None) {
				sw.WriteLine ("static const char *xamarin_runtime_libraries_array[] = {");
				foreach (var lib in app.MonoLibraries)
					sw.WriteLine ($"\t\"{Path.GetFileNameWithoutExtension (lib)}\",");
				sw.WriteLine ($"\tNULL");
				sw.WriteLine ("};");
			}
#endif

			sw.WriteLine ("void xamarin_setup_impl ()");
			sw.WriteLine ("{");

			if (app.EnableDiagnostics)
				sw.WriteLine ("\txamarin_profiler_symbol = mono_profiler_init_log;");

			if (app.UseInterpreter) {
				sw.WriteLine ("\tmono_icall_table_init ();");
				sw.WriteLine ("\tmono_marshal_ilgen_init ();");
				sw.WriteLine ("\tmono_method_builder_ilgen_init ();");
				sw.WriteLine ("\tmono_sgen_mono_ilgen_init ();");
#if !NET || LEGACY_TOOLS
				sw.WriteLine ("\tmono_ee_interp_init (NULL);");
#endif
				if ((abi & Abi.x86_64) == Abi.x86_64) {
					sw.WriteLine ("\tmono_jit_set_aot_mode (MONO_AOT_MODE_INTERP_ONLY);");
				} else {
					sw.WriteLine ("\tmono_jit_set_aot_mode (MONO_AOT_MODE_INTERP);");
				}
			} else if (app.XamarinRuntime == XamarinRuntime.NativeAOT) {
				// don't call mono_jit_set_aot_mode
			} else if (app.IsDeviceBuild) {
				sw.WriteLine ("\tmono_jit_set_aot_mode (MONO_AOT_MODE_FULL);");
			} else if (app.Platform == ApplePlatform.MacCatalyst && ((abi & Abi.ARM64) == Abi.ARM64)) {
				sw.WriteLine ("\tmono_jit_set_aot_mode (MONO_AOT_MODE_FULL);");
			} else if (app.IsSimulatorBuild && ((abi & Abi.ARM64) == Abi.ARM64)) {
				sw.WriteLine ("\tmono_jit_set_aot_mode (MONO_AOT_MODE_FULL);");
			}

			if (assembly_location.Length > 0)
				sw.WriteLine ("\txamarin_set_assembly_directories (&assembly_locations);");

			sw.WriteLine ("\txamarin_invoke_registration_methods ();");

			if (app.MonoNativeMode != MonoNativeMode.None) {
#if NET && !LEGACY_TOOLS
				// Mono doesn't support dllmaps for Mac Catalyst / macOS in .NET, so we're using an alternative:
				// the PINVOKE_OVERRIDE runtime option. Since we have to use it for Mac Catalyst + macOS, let's
				// just use it everywhere to simplify code. This means that at runtime we need to know how we
				// linked to mono, so store that in the xamarin_libmono_native_link_mode variable.
				// Ref: https://github.com/dotnet/runtime/issues/43204 (macOS) https://github.com/dotnet/runtime/issues/48110 (Mac Catalyst)
				sw.WriteLine ($"\txamarin_libmono_native_link_mode = XamarinNativeLinkMode{app.LibMonoNativeLinkMode};");
				sw.WriteLine ($"\txamarin_runtime_libraries = xamarin_runtime_libraries_array;");
#else
				string mono_native_lib;
				if (app.LibMonoNativeLinkMode == AssemblyBuildTarget.StaticObject) {
					mono_native_lib = "__Internal";
				} else {
					mono_native_lib = app.GetLibNativeName () + ".dylib";
				}
				sw.WriteLine ();
				sw.WriteLine ($"\tmono_dllmap_insert (NULL, \"System.Native\", NULL, \"{mono_native_lib}\", NULL);");
				sw.WriteLine ($"\tmono_dllmap_insert (NULL, \"System.Security.Cryptography.Native.Apple\", NULL, \"{mono_native_lib}\", NULL);");
				sw.WriteLine ($"\tmono_dllmap_insert (NULL, \"System.Net.Security.Native\", NULL, \"{mono_native_lib}\", NULL);");
				sw.WriteLine ();
#endif
			}

			if (app.EnableDebug)
				sw.WriteLine ("\txamarin_gc_pump = {0};", app.DebugTrack.Value ? "TRUE" : "FALSE");
			sw.WriteLine ("\txamarin_init_mono_debug = {0};", app.PackageManagedDebugSymbols ? "TRUE" : "FALSE");
			sw.WriteLine ("\txamarin_executable_name = \"{0}\";", assembly_name);
			if (app.XamarinRuntime == XamarinRuntime.MonoVM)
				sw.WriteLine ("\tmono_use_llvm = {0};", enable_llvm ? "TRUE" : "FALSE");
			sw.WriteLine ("\txamarin_log_level = {0};", Driver.Verbosity.ToString (CultureInfo.InvariantCulture));
			sw.WriteLine ("\txamarin_arch_name = \"{0}\";", abi.AsArchString ());
			if (!app.IsDefaultMarshalManagedExceptionMode)
				sw.WriteLine ("\txamarin_marshal_managed_exception_mode = MarshalManagedExceptionMode{0};", app.MarshalManagedExceptions);
			sw.WriteLine ("\txamarin_marshal_objectivec_exception_mode = MarshalObjectiveCExceptionMode{0};", app.MarshalObjectiveCExceptions);
			if (app.EnableDebug)
				sw.WriteLine ("\txamarin_debug_mode = TRUE;");
			if (!string.IsNullOrEmpty (app.MonoGCParams))
				sw.WriteLine ("\tsetenv (\"MONO_GC_PARAMS\", \"{0}\", 1);", app.MonoGCParams);
			// Do this last, so that the app developer can override any other environment variable we set.
			foreach (var kvp in app.EnvironmentVariables)
				sw.WriteLine ("\tsetenv (\"{0}\", \"{1}\", 1);", kvp.Key.Replace ("\"", "\\\""), kvp.Value.Replace ("\"", "\\\""));
			if (app.XamarinRuntime != XamarinRuntime.NativeAOT)
				sw.WriteLine ("\txamarin_supports_dynamic_registration = {0};", app.DynamicRegistrationSupported ? "TRUE" : "FALSE");
#if NET && !LEGACY_TOOLS
			sw.WriteLine ("\txamarin_runtime_configuration_name = {0};", string.IsNullOrEmpty (app.RuntimeConfigurationFile) ? "NULL" : $"\"{app.RuntimeConfigurationFile}\"");
#endif
			if (app.Registrar == RegistrarMode.ManagedStatic)
				sw.WriteLine ("\txamarin_set_is_managed_static_registrar (true);");
			sw.WriteLine ("}");
			sw.WriteLine ();
			sw.Write ("int main");
			sw.WriteLine (" (int argc, char **argv)");
			sw.WriteLine ("{");
			sw.WriteLine ("\tNSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];");
			if (app.IsExtension) {
				// the name of the executable must be the bundle id (reverse dns notation)
				// but we do not want to impose that (ugly) restriction to the managed .exe / project name / ...
				sw.WriteLine ("\targv [0] = (char *) \"{0}\";", Path.GetFileNameWithoutExtension (app.RootAssemblies [0]));
				sw.WriteLine ("\tint rv = xamarin_main (argc, argv, XamarinLaunchModeExtension);");
			} else {
				sw.WriteLine ("\tint rv = xamarin_main (argc, argv, XamarinLaunchModeApp);");
			}
			sw.WriteLine ("\t[pool drain];");
			sw.WriteLine ("\treturn rv;");
			sw.WriteLine ("}");

			// Add an empty __managed__Main function when building class lib app extensions with NativeAOT to workaround static reference to this symbol from nativeaot-bridge.m
			if (app.IsExtension && app.XamarinRuntime == XamarinRuntime.NativeAOT) {
				sw.WriteLine ();
				sw.Write ("extern \"C\" int __managed__Main (int argc, const char** argv) { return 0; } ");
				sw.WriteLine ();
			}

			sw.WriteLine ();
			sw.WriteLine ("void xamarin_initialize_callbacks () __attribute__ ((constructor));");
			sw.WriteLine ("void xamarin_initialize_callbacks ()");
			sw.WriteLine ("{");
			sw.WriteLine ("\txamarin_setup = xamarin_setup_impl;");
			sw.WriteLine ("\txamarin_register_assemblies = xamarin_register_assemblies_impl;");
			sw.WriteLine ("\txamarin_register_modules = xamarin_register_modules_impl;");
			sw.WriteLine ("}");
		}
#endif // MMP

#if NET && !LEGACY_TOOLS
		static readonly char [] charsToReplaceAot = new [] { '.', '-', '+', '<', '>' };
#endif
		static string EncodeAotSymbol (string symbol)
		{
			var sb = new StringBuilder ();
			/* This mimics what the aot-compiler does */
			// https://github.com/dotnet/runtime/blob/2f08fcbfece0c09319f237a6aee6f74c4a9e14e8/src/mono/mono/metadata/native-library.c#L1265-L1284
			// https://github.com/dotnet/runtime/blob/2f08fcbfece0c09319f237a6aee6f74c4a9e14e8/src/tasks/Common/Utils.cs#L419-L445
			foreach (var b in System.Text.Encoding.UTF8.GetBytes (symbol)) {
				char c = (char) b;
				if ((c >= '0' && c <= '9') ||
					(c >= 'a' && c <= 'z') ||
					(c >= 'A' && c <= 'Z') ||
					(c == '_')) {
					sb.Append (c);
					continue;
#if NET && !LEGACY_TOOLS
				} else if (charsToReplaceAot.Contains (c)) {
					sb.Append ('_');
				} else {
					// Append the hex representation of b between underscores
					sb.Append ($"_{b:X}_");
#endif
				}
#if !NET || LEGACY_TOOLS
				sb.Append ('_');
#endif
			}
			return sb.ToString ();
		}

#if !MMP
		static bool IsBoundAssembly (Assembly s)
		{
			if (s.IsFrameworkAssembly == true)
				return false;

			AssemblyDefinition ad = s.AssemblyDefinition;

			foreach (ModuleDefinition md in ad.Modules)
				foreach (TypeDefinition td in md.Types)
					if (td.IsNSObject (s.Target.LinkContext))
						return true;

			return false;
		}
#endif // MMP
	}
}
