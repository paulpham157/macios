#if __MACOS__
#define MONOMAC
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

using AVFoundation;
using CoreBluetooth;
using CoreFoundation;
using Foundation;
#if !__TVOS__
using Contacts;
#endif
#if MONOMAC || __MACCATALYST__
using EventKit;
#endif
#if MONOMAC
using AppKit;
#else
#if !__TVOS__
using AddressBook;
#endif
using MediaPlayer;
using UIKit;
#endif
using ObjCRuntime;

using Xamarin.Utils;

using NUnit.Framework;

#nullable enable

partial class TestRuntime {

	[DllImport (Constants.CoreFoundationLibrary)]
	public extern static nint CFGetRetainCount (IntPtr handle);

	[DllImport (Constants.CoreFoundationLibrary)]
	public extern static nint CFRetain (IntPtr handle);

	[DllImport (Constants.CoreFoundationLibrary)]
	public extern static void CFRelease (IntPtr handle);

	[DllImport ("/usr/lib/system/libdyld.dylib")]
	static extern int dyld_get_program_sdk_version ();

	[DllImport ("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
	static extern IntPtr IntPtr_objc_msgSend (IntPtr receiver, IntPtr selector);

	public const string BuildVersion_iOS9_GM = "13A340";

	// Xcode 12.0 removed macOS 11.0 SDK and moved it up to Xcode 12.2
	// we use this constant to make up for that difference when using
	// AssertXcodeVersion and CheckXcodeVersion
#if __MACOS__ || __MACCATALYST__
	public const int MinorXcode12APIMismatch = 2;
#else
	public const int MinorXcode12APIMismatch = 0;
#endif

	public static string GetiOSBuildVersion ()
	{
#if MONOMAC
		throw new Exception ("Can't get iOS Build version on OSX.");
#else
		return CFString.FromHandle (IntPtr_objc_msgSend (UIDevice.CurrentDevice.Handle, Selector.GetHandle ("buildVersion")))!;
#endif
	}

#if MONOMAC
	const int sys1 = 1937339185;
	const int sys2 = 1937339186;
	const int sys3 = 1937339187;

	// Deprecated in OSX 10.8 - but no good alternative is (yet) available
	[System.Runtime.InteropServices.DllImport ("/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon")]
	static extern int Gestalt (int selector, out int result);

	static Version? version;

	public static Version OSXVersion {
		get {
			if (version is null) {
				int major, minor, build;
				Gestalt (sys1, out major);
				Gestalt (sys2, out minor);
				Gestalt (sys3, out build);
				version = new Version (major, minor, build);
			}
			return version;
		}
	}
#elif __MACCATALYST__
	public static Version OSXVersion {
		get {
			return Version.Parse (UIDevice.CurrentDevice.SystemVersion);
		}
	}
#endif

	public static ApplePlatform CurrentPlatform {
		get {
#if __MACCATALYST__
			return ApplePlatform.MacCatalyst;
#elif __IOS__
			return ApplePlatform.iOS;
#elif __TVOS__
			return ApplePlatform.TVOS;
#elif __MACOS__
			return ApplePlatform.MacOSX;
#else
#error Unknown platform
#endif
		}
	}

	// This returns the string for the platform as used in the OSPlatformAttribute's PlatformName property.
	public static string GetOSPlatformName (ApplePlatform platform)
	{
		switch (platform) {
		case ApplePlatform.iOS:
			return "ios";
		case ApplePlatform.MacOSX:
			return "macos";
		case ApplePlatform.TVOS:
			return "tvos";
		case ApplePlatform.MacCatalyst:
			return "maccatalyst";
		default:
			throw new Exception ($"Unknown platform: {platform}");
		}
	}

	public static bool HasOSPlatformAttributeForCurrentPlatform<T> (ICustomAttributeProvider provider) where T : OSPlatformAttribute
	{
		return HasOSPlatformAttribute<T> (provider, CurrentPlatform);
	}

	public static bool HasOSPlatformAttribute<T> (ICustomAttributeProvider provider, ApplePlatform platform) where T : OSPlatformAttribute
	{
		var attribs = provider.GetCustomAttributes (false);
		var platformName = GetOSPlatformName (platform);
		foreach (var attrib in attribs) {
			if (attrib is T platformAttribute) {
				if (platformAttribute.PlatformName.StartsWith (platformName, StringComparison.OrdinalIgnoreCase))
					return true;
			}
		}
		return false;
	}

	public static Version GetSDKVersion ()
	{
		var v = dyld_get_program_sdk_version ();
		var major = v >> 16;
		var minor = (v >> 8) & 0xFF;
		var build = v & 0xFF;
		return new Version (major, minor, build);
	}

	static bool? is_in_ci;
	public static bool IsInCI {
		get {
			if (!is_in_ci.HasValue) {
				var in_ci = !string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("BUILD_REVISION"));
				in_ci |= !string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("BUILD_SOURCEVERSION")); // set by Azure DevOps
				is_in_ci = in_ci;
			}
			return is_in_ci.Value;
		}
	}

	static bool? is_pull_request;
	public static bool IsPullRequest {
		get {
			if (!is_pull_request.HasValue) {
				var pr = string.Equals (Environment.GetEnvironmentVariable ("BUILD_REASON"), "PullRequest", StringComparison.Ordinal);
				pr |= string.Equals (Environment.GetEnvironmentVariable ("IS_PR"), "true", StringComparison.OrdinalIgnoreCase);
				is_pull_request = pr;
			}
			return is_pull_request.Value;
		}
	}

	public static void IgnoreInCI (string message)
	{
		if (!IsInCI) {
			Console.WriteLine ($"Not ignoring test, because not running in CI: {message}");
			return;
		}
		Console.WriteLine ($"Ignoring test, because not running in CI: {message}");
		NUnit.Framework.Assert.Ignore (message);
	}

	public static void AssertNotInterpreter (string message = "This test does not run when using the interpreter")
	{
		if (IsCoreCLR)
			return;

		if (RuntimeFeature.IsDynamicCodeSupported)
			NUnit.Framework.Assert.Ignore (message);
	}

	public static void AssertXcodeVersion (int major, int minor, int build = 0)
	{
		if (CheckXcodeVersion (major, minor, build))
			return;

		NUnit.Framework.Assert.Ignore ("Requires the platform version shipped with Xcode {0}.{1}", major, minor);
	}

	public static void AssertDevice (string message = "This test only runs on device.")
	{
#if !MONOMAC && !__MACCATALYST__
		if (ObjCRuntime.Runtime.Arch == Arch.DEVICE)
			return;
#endif
		NUnit.Framework.Assert.Ignore (message);
	}

	public static void AssertNotDevice (string message = "This test does not run on device.")
	{
#if !MONOMAC && !__MACCATALYST__
		if (ObjCRuntime.Runtime.Arch == Arch.DEVICE)
			NUnit.Framework.Assert.Ignore (message);
#endif
	}

	public static void AssertDesktop (string message = "This test only runs on Desktops (macOS or MacCatalyst).")
	{
#if !(__MACOS__ || __MACCATALYST__)
		NUnit.Framework.Assert.Ignore (message);
#endif
	}

	public static void AssertNotDesktop (string message = "This test does not run on Desktops (macOS or MacCatalyst).")
	{
#if __MACOS__ || __MACCATALYST__
		NUnit.Framework.Assert.Ignore (message);
#endif
	}

	public static void AssertNotX64Desktop (string message = "This test does not run on x64 desktops.")
	{
#if __MACOS__ || __MACCATALYST__
		if (!IsARM64)
			NUnit.Framework.Assert.Ignore (message);
#endif
	}

	public static void AssertNotARM64Desktop (string message = "This test does not run on an ARM64 desktop.")
	{
#if __MACOS__ || __MACCATALYST__
		if (IsARM64)
			NUnit.Framework.Assert.Ignore (message);
#endif
	}

	public static void AssertIfSimulatorThenARM64 ()
	{
#if !__MACOS__ && !__MACCATALYST__
		if (ObjCRuntime.Runtime.Arch != Arch.SIMULATOR)
			return;
		if (!IsARM64)
			NUnit.Framework.Assert.Ignore ("This test does not run simulators that aren't ARM64 simulators.");
#endif
	}

	public static void AssertOnlyARM64 ()
	{
		if (!IsARM64)
			NUnit.Framework.Assert.Ignore ("This test only runs on ARM64 simulators.");
	}

	public static void AssertNotSimulator (string message = "This test does not work in the simulator.")
	{
		if (IsSimulator)
			NUnit.Framework.Assert.Ignore (message);
	}

	public static void AssertSimulator (string message = "This test only works in the simulator.")
	{
		if (!IsSimulator)
			NUnit.Framework.Assert.Ignore (message);
	}

	public static void AssertSimulatorOrDesktop (string message = "This test only works in the simulator or on the desktop.")
	{
		if (!IsSimulatorOrDesktop)
			NUnit.Framework.Assert.Ignore (message);
	}

	public static bool IsVM =>
		!string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("VM_VENDOR"));

	public static void AssertNotVirtualMachine ()
	{
#if MONOMAC || __MACCATALYST__
		// enviroment variable set by the CI when running on a VM
		var vmVendor = Environment.GetEnvironmentVariable ("VM_VENDOR");
		if (!string.IsNullOrEmpty (vmVendor))
			NUnit.Framework.Assert.Ignore ($"This test only runs on device. Found vm vendor: {vmVendor}");
#endif
	}

	public static bool IsVSTS =>
		!string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("BUILD_BUILDID"));  // Env var set by vsts
																					   //
	public static void AssertNotVSTS ()
	{
		if (IsVSTS)
			NUnit.Framework.Assert.Ignore ("This test only runs on developer desktops and not on VSTS.");
	}

	// This function checks if the current Xcode version is exactly (neither higher nor lower) the requested one.
	public static bool CheckExactXcodeVersion (int major, int minor, int beta = 0)
	{
		// Add the Build number minus the one last character, sometimes Apple releases
		// different builds from the same Beta, for example in Xcode 9 Beta 3 we have
		// 15A5318g on device and 15A5318e on the simulator
		var nineb1 = new {
			Xcode = new { Major = 9, Minor = 0, Beta = 1 },
			iOS = new { Major = 11, Minor = 0, Build = "15A5278" },
			tvOS = new { Major = 11, Minor = 0, Build = "?" },
			macOS = new { Major = 10, Minor = 13, Build = "?" },
		};
		var nineb2 = new {
			Xcode = new { Major = 9, Minor = 0, Beta = 2 },
			iOS = new { Major = 11, Minor = 0, Build = "15A5304" },
			tvOS = new { Major = 11, Minor = 0, Build = "?" },
			macOS = new { Major = 10, Minor = 13, Build = "?" },
		};
		var nineb3 = new {
			Xcode = new { Major = 9, Minor = 0, Beta = 3 },
			iOS = new { Major = 11, Minor = 0, Build = "15A5318" },
			tvOS = new { Major = 11, Minor = 0, Build = "?" },
			macOS = new { Major = 10, Minor = 13, Build = "?" },
		};
		var elevenb5 = new {
			Xcode = new { Major = 11, Minor = 0, Beta = 5 },
			iOS = new { Major = 13, Minor = 0, Build = "17A5547" },
			tvOS = new { Major = 13, Minor = 0, Build = "?" },
			macOS = new { Major = 10, Minor = 15, Build = "?" },
		};
		var elevenb6 = new {
			Xcode = new { Major = 11, Minor = 0, Beta = 6 },
			iOS = new { Major = 13, Minor = 0, Build = "17A5565b" },
			tvOS = new { Major = 13, Minor = 0, Build = "?" },
			macOS = new { Major = 10, Minor = 15, Build = "?" },
		};

		var twelvedot2b2 = new {
			Xcode = new { Major = 12, Minor = 2, Beta = 2 },
			iOS = new { Major = 14, Minor = 2, Build = "18B5061" },
			tvOS = new { Major = 14, Minor = 2, Build = "18K5036" },
			macOS = new { Major = 11, Minor = 0, Build = "?" },
		};

		var twelvedot2b3 = new {
			Xcode = new { Major = 12, Minor = 2, Beta = 3 },
			iOS = new { Major = 14, Minor = 2, Build = "18B5072" },
			tvOS = new { Major = 14, Minor = 2, Build = "18K5047" },
			macOS = new { Major = 11, Minor = 0, Build = "20A5395" },
		};

		var versions = new [] {
			nineb1,
			nineb2,
			nineb3,
			elevenb5,
			elevenb6,
			twelvedot2b2,
			twelvedot2b3,
		};

		foreach (var v in versions) {
			if (v.Xcode.Major != major)
				continue;
			if (v.Xcode.Minor != minor)
				continue;
			if (v.Xcode.Beta != beta)
				continue;

#if __IOS__
			if (!CheckExactiOSSystemVersion (v.iOS.Major, v.iOS.Minor))
				return false;
			if (v.iOS.Build == "?")
				throw new NotImplementedException ($"Build number for iOS {v.iOS.Major}.{v.iOS.Minor} beta {beta} (candidate: {GetiOSBuildVersion ()})");
			var actual = GetiOSBuildVersion ();
			Console.WriteLine (actual);
			return actual.StartsWith (v.iOS.Build, StringComparison.Ordinal);
#elif __TVOS__
			if (!CheckExacttvOSSystemVersion (v.tvOS.Major, v.tvOS.Minor))
				return false;
			if (v.tvOS.Build == "?")
				throw new NotImplementedException ($"Build number for tvOS {v.tvOS.Major}.{v.tvOS.Minor} beta {beta} (candidate: {GetiOSBuildVersion ()})");
			var actual = GetiOSBuildVersion ();
			Console.WriteLine (actual);
			return actual.StartsWith (v.tvOS.Build, StringComparison.Ordinal);
#elif __MACOS__
			if (!CheckExactmacOSSystemVersion (v.macOS.Major, v.macOS.Minor))
				return false;
			if (v.macOS.Build == "?")
				throw new NotImplementedException ($"Build number for macOS {v.macOS.Major}.{v.macOS.Minor} beta {beta}.");
			/*
			 * I could be parsing the string but docs says it is not suitable for parsing and this is ugly enough so
			 * an apology in advance (I'm very sorry =]) to my future self or whoever is dealing with this if it broke
			 * but there are no better solutions at this time. That said this is good enough for the current use case.
			 * Example: Version 10.16 (Build 20A5395g)
			 *
			 * The above statement also applies to 'CheckExactmacOSSystemVersion' =S
			 */
			return NSProcessInfo.ProcessInfo.OperatingSystemVersionString.Contains (v.macOS.Build, StringComparison.Ordinal);
#else
			throw new NotImplementedException ();
#endif
		}

		throw new NotImplementedException ($"Build information for Xcode version {major}.{minor} beta {beta} not found");
	}

	public static bool CheckXcodeVersion (int major, int minor, int build = 0)
	{
		switch (major) {
		case 16:
			switch (minor) {
			case 0:
#if __TVOS__
				return ChecktvOSSystemVersion (18, 0);
#elif __IOS__
				return CheckiOSSystemVersion (18, 0);
#elif MONOMAC
				return CheckMacSystemVersion (15, 0);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			case 1:
#if __TVOS__
				return ChecktvOSSystemVersion (18, 1);
#elif __IOS__
				return CheckiOSSystemVersion (18, 1);
#elif MONOMAC
				return CheckMacSystemVersion (15, 1);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			case 2:
#if __TVOS__
				return ChecktvOSSystemVersion (18, 2);
#elif __IOS__
				return CheckiOSSystemVersion (18, 2);
#elif MONOMAC
				return CheckMacSystemVersion (15, 2);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			case 3:
#if __TVOS__
				return ChecktvOSSystemVersion (18, 4);
#elif __IOS__
				return CheckiOSSystemVersion (18, 4);
#elif MONOMAC
				return CheckMacSystemVersion (15, 4);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			default:
				throw new NotImplementedException ($"Missing version logic for checking for Xcode {major}.{minor}");
			}
		case 15:
			switch (minor) {
			case 0:
#if __TVOS__
				return ChecktvOSSystemVersion (17, 0);
#elif __IOS__
				return CheckiOSSystemVersion (17, 0);
#elif MONOMAC
				return CheckMacSystemVersion (14, 0);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			case 1:
			case 2:
#if __TVOS__
				return ChecktvOSSystemVersion (17, 2);
#elif __IOS__
				return CheckiOSSystemVersion (17, 2);
#elif MONOMAC
				return CheckMacSystemVersion (14, 2);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			case 3:
#if __TVOS__
				return ChecktvOSSystemVersion (17, 4);
#elif __IOS__
				return CheckiOSSystemVersion (17, 4);
#elif MONOMAC
				return CheckMacSystemVersion (14, 4);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			case 4:
#if __TVOS__
				return ChecktvOSSystemVersion (17, 5);
#elif __IOS__
				return CheckiOSSystemVersion (17, 5);
#elif MONOMAC
				return CheckMacSystemVersion (14, 5);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			default:
				throw new NotImplementedException ($"Missing version logic for checking for Xcode {major}.{minor}");
			}
		case 14:
			switch (minor) {
			case 0:
#if __TVOS__
				return ChecktvOSSystemVersion (16, 0);
#elif __IOS__
				return CheckiOSSystemVersion (16, 0);
#elif MONOMAC
				return CheckMacSystemVersion (13, 0);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			case 1:
#if __TVOS__
				return ChecktvOSSystemVersion (16, 1);
#elif __IOS__
				return CheckiOSSystemVersion (16, 1);
#elif MONOMAC
				return CheckMacSystemVersion (13, 0);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			case 2:
#if __TVOS__
				return ChecktvOSSystemVersion (16, 1);
#elif __IOS__
				return CheckiOSSystemVersion (16, 2);
#elif MONOMAC
				return CheckMacSystemVersion (13, 1);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			case 3:
#if __TVOS__
				return ChecktvOSSystemVersion (16, 4);
#elif __IOS__
				return CheckiOSSystemVersion (16, 4);
#elif MONOMAC
				return CheckMacSystemVersion (13, 3);
#else
				throw new NotImplementedException ($"Missing platform case for Xcode {major}.{minor}");
#endif
			default:
				throw new NotImplementedException ($"Missing version logic for checking for Xcode {major}.{minor}");
			}
		case 13:
			switch (minor) {
			case 0:
#if __TVOS__
				return ChecktvOSSystemVersion (15, 0);
#elif __IOS__
				return CheckiOSSystemVersion (15, 0);
#elif MONOMAC
				return CheckMacSystemVersion (12, 0);
#else
				throw new NotImplementedException ();
#endif
			case 1:
#if __TVOS__
				return ChecktvOSSystemVersion (15, 1);
#elif __IOS__
				return CheckiOSSystemVersion (15, 1);
#elif MONOMAC
				return CheckMacSystemVersion (12, 0);
#else
				throw new NotImplementedException ();
#endif
			case 2:
#if __TVOS__
				return ChecktvOSSystemVersion (15, 2);
#elif __IOS__
				return CheckiOSSystemVersion (15, 2);
#elif MONOMAC
				return CheckMacSystemVersion (12, 1);
#else
				throw new NotImplementedException ();
#endif
			case 3:
#if __TVOS__
				return ChecktvOSSystemVersion (15, 4);
#elif __IOS__
				return CheckiOSSystemVersion (15, 4);
#elif MONOMAC
				return CheckMacSystemVersion (12, 3);
#else
				throw new NotImplementedException ();
#endif
			default:
				throw new NotImplementedException ();
			}
		case 12:
			switch (minor) {
			case 0:
#if __TVOS__
				return ChecktvOSSystemVersion (14, 0);
#elif __IOS__
				return CheckiOSSystemVersion (14, 0);
#elif MONOMAC
				return CheckMacSystemVersion (10, 15, 6);
#else
				throw new NotImplementedException ();
#endif
			case 1:
#if __TVOS__
				return ChecktvOSSystemVersion (14, 0);
#elif __IOS__
				return CheckiOSSystemVersion (14, 1);
#elif MONOMAC
				return CheckMacSystemVersion (10, 15, 6);
#else
				throw new NotImplementedException ();
#endif
			case 2:
#if __TVOS__
				return ChecktvOSSystemVersion (14, 2);
#elif __IOS__
				return CheckiOSSystemVersion (14, 2);
#elif MONOMAC
				return CheckMacSystemVersion (11, 0, 0);
#else
				throw new NotImplementedException ();
#endif
			case 3:
#if __TVOS__
				return ChecktvOSSystemVersion (14, 3);
#elif __IOS__
				return CheckiOSSystemVersion (14, 3);
#elif MONOMAC
				return CheckMacSystemVersion (11, 1, 0);
#endif
			case 5:
#if __TVOS__
				return ChecktvOSSystemVersion (14, 5);
#elif __IOS__
				return CheckiOSSystemVersion (14, 5);
#elif MONOMAC
				return CheckMacSystemVersion (11, 3, 0);
#else
				throw new NotImplementedException ();
#endif
			default:
				throw new NotImplementedException ();
			}
		case 11:
			switch (minor) {
			case 0:
#if __TVOS__
				return ChecktvOSSystemVersion (13, 0);
#elif __IOS__
				return CheckiOSSystemVersion (13, 0);
#elif MONOMAC
				return CheckMacSystemVersion (10, 15, 0);
#else
				throw new NotImplementedException ();
#endif
			case 1:
#if __TVOS__
				return ChecktvOSSystemVersion (13, 0);
#elif __IOS__
				return CheckiOSSystemVersion (13, 1);
#elif MONOMAC
				return CheckMacSystemVersion (10, 15, 0);
#else
				throw new NotImplementedException ();
#endif
			case 2:
#if __TVOS__
				return ChecktvOSSystemVersion (13, 2);
#elif MONOMAC
				return CheckMacSystemVersion (10, 15, 1);
#elif __IOS__
				return CheckiOSSystemVersion (13, 2);
#else
				throw new NotImplementedException ();
#endif
			case 3:
#if __TVOS__
				return ChecktvOSSystemVersion (13, 3);
#elif __IOS__
				return CheckiOSSystemVersion (13, 3);
#elif MONOMAC
				return CheckMacSystemVersion (10, 15, 2);
#else
				throw new NotImplementedException ();
#endif
			case 4:
#if __TVOS__
				return ChecktvOSSystemVersion (13, 4);
#elif __IOS__
				return CheckiOSSystemVersion (13, 4);
#elif MONOMAC
				return CheckMacSystemVersion (10, 15, 4);
#else
				throw new NotImplementedException ();
#endif
			case 5:
#if __TVOS__
				return ChecktvOSSystemVersion (13, 4);
#elif __IOS__
				return CheckiOSSystemVersion (13, 5);
#elif MONOMAC
				return CheckMacSystemVersion (10, 15, 5);
#else
				throw new NotImplementedException ();
#endif
			case 6:
#if __TVOS__
				return ChecktvOSSystemVersion (13, 4);
#elif __IOS__
				return CheckiOSSystemVersion (13, 6);
#elif MONOMAC
				return CheckMacSystemVersion (10, 15, 6);
#else
				throw new NotImplementedException ();
#endif
			default:
				throw new NotImplementedException ();
			}
		case 10:
			switch (minor) {
			case 0:
#if __TVOS__
				return ChecktvOSSystemVersion (12, 0);
#elif __IOS__
				return CheckiOSSystemVersion (12, 0);
#elif MONOMAC
				return CheckMacSystemVersion (10, 14, 0);
#else
				throw new NotImplementedException ();
#endif
			case 1:
#if __TVOS__
				return ChecktvOSSystemVersion (12, 1);
#elif __IOS__
				return CheckiOSSystemVersion (12, 1);
#elif MONOMAC
				return CheckMacSystemVersion (10, 14, 3);
#else
				throw new NotImplementedException ();
#endif
			case 2:
#if __TVOS__
				return ChecktvOSSystemVersion (12, 2);
#elif __IOS__
				return CheckiOSSystemVersion (12, 2);
#elif MONOMAC
				return CheckMacSystemVersion (10, 14, 4);
#else
				throw new NotImplementedException ();
#endif
			default:
				throw new NotImplementedException ();
			}
		case 9:
			switch (minor) {
			case 0:
#if __TVOS__
				return ChecktvOSSystemVersion (11, 0);
#elif __IOS__
				return CheckiOSSystemVersion (11, 0);
#elif MONOMAC
				return CheckMacSystemVersion (10, 13, 0);
#else
				throw new NotImplementedException ();
#endif
			case 2:
#if __TVOS__
				return ChecktvOSSystemVersion (11, 2);
#elif __IOS__
				return CheckiOSSystemVersion (11, 2);
#elif MONOMAC
				return CheckMacSystemVersion (10, 13, 2);
#else
				throw new NotImplementedException ();
#endif
			case 3:
#if __TVOS__
				return ChecktvOSSystemVersion (11, 3);
#elif __IOS__
				return CheckiOSSystemVersion (11, 3);
#elif MONOMAC
				return CheckMacSystemVersion (10, 13, 4);
#else
				throw new NotImplementedException ();
#endif
			default:
				throw new NotImplementedException ();
			}
		case 8:
			switch (minor) {
			case 0:
#if __TVOS__
				return ChecktvOSSystemVersion (10, 0);
#elif __IOS__
				return CheckiOSSystemVersion (10, 0);
#elif MONOMAC
				return CheckMacSystemVersion (10, 12, 0);
#else
				throw new NotImplementedException ();
#endif
			case 1:
#if __TVOS__
				return ChecktvOSSystemVersion (10, 0);
#elif __IOS__
				return CheckiOSSystemVersion (10, 1);
#elif MONOMAC
				return CheckMacSystemVersion (10, 12, 1);
#else
				throw new NotImplementedException ();
#endif
			case 2:
#if __TVOS__
				return ChecktvOSSystemVersion (10, 1);
#elif __IOS__
				return CheckiOSSystemVersion (10, 2);
#elif MONOMAC
				return CheckMacSystemVersion (10, 12, 2);
#else
				throw new NotImplementedException ();
#endif
			case 3:
#if __TVOS__
				return ChecktvOSSystemVersion (10, 2);
#elif __IOS__
				return CheckiOSSystemVersion (10, 3);
#elif MONOMAC
				return CheckMacSystemVersion (10, 12, 4);
#else
				throw new NotImplementedException ();
#endif
			default:
				throw new NotImplementedException ();
			}
		case 7:
			switch (minor) {
			case 0:
#if __TVOS__
				return ChecktvOSSystemVersion (9, 0);
#elif __IOS__
				return CheckiOSSystemVersion (9, 0);
#elif MONOMAC
				return CheckMacSystemVersion (10, 11, 0);
#else
				throw new NotImplementedException ();
#endif
			case 1:
#if __TVOS__
				return ChecktvOSSystemVersion (9, 0);
#elif __IOS__
				return CheckiOSSystemVersion (9, 1);
#elif MONOMAC
				return CheckMacSystemVersion (10, 11, 0 /* yep */);
#else
				throw new NotImplementedException ();
#endif
			case 2:
#if __TVOS__
				return ChecktvOSSystemVersion (9, 1);
#elif __IOS__
				return CheckiOSSystemVersion (9, 2);
#elif MONOMAC
				return CheckMacSystemVersion (10, 11, 2);
#else
				throw new NotImplementedException ();
#endif
			case 3:
#if __TVOS__
				return ChecktvOSSystemVersion (9, 2);
#elif __IOS__
				return CheckiOSSystemVersion (9, 3);
#elif MONOMAC
				return CheckMacSystemVersion (10, 11, 4);
#else
				throw new NotImplementedException ();
#endif
			default:
				throw new NotImplementedException ();
			}
		case 6:
#if __IOS__
			switch (minor) {
			case 0:
				return CheckiOSSystemVersion (8, 0);
			case 1:
				return CheckiOSSystemVersion (8, 1);
			case 2:
				return CheckiOSSystemVersion (8, 2);
			case 3:
				return CheckiOSSystemVersion (8, 3);
			default:
				throw new NotImplementedException ();
			}
#elif __TVOS__
			return true;
#elif MONOMAC
			switch (minor) {
			case 0:
				return CheckMacSystemVersion (10, 9, 0);
			case 1:
				return CheckMacSystemVersion (10, 10, 0);
			case 2:
				return CheckMacSystemVersion (10, 10, 0);
			case 3:
				return CheckMacSystemVersion (10, 10, 0);
			default:
				throw new NotImplementedException ();
			}
#else
			throw new NotImplementedException ();
#endif
		case 5:
#if __IOS__
			switch (minor) {
			case 0:
				return CheckiOSSystemVersion (7, 0);
			case 1:
				return CheckiOSSystemVersion (7, 1);
			default:
				throw new NotImplementedException ();
			}
#elif __TVOS__
			return true;
#elif MONOMAC
			switch (minor) {
			case 0:
				// Xcode 5.0.1 ships OSX 10.9 SDK
				return CheckMacSystemVersion (10, build > 0 ? 9 : 8, 0);
			case 1:
				return CheckMacSystemVersion (10, 9, 0);
			default:
				throw new NotImplementedException ();
			}
#else
			throw new NotImplementedException ();
#endif
		case 4:
#if __IOS__
			switch (minor) {
			case 1:
				return true; // iOS 4.3.2
			case 5:
				return CheckiOSSystemVersion (6, 0);
			case 6:
				return CheckiOSSystemVersion (6, 1);
			default:
				throw new NotImplementedException ();
			}
#elif __TVOS__
			return true;
#elif MONOMAC
			switch (minor) {
			case 1:
				return CheckMacSystemVersion (10, 7, 0);
			case 5:
			case 6:
				return CheckMacSystemVersion (10, 8, 0);
			default:
				throw new NotImplementedException ();
			}
#else
			throw new NotImplementedException ();
#endif
		default:
			throw new NotImplementedException ($"Missing version logic for checking for Xcode {major}.{minor}");
		}
	}

	public static bool CheckSystemVersion (ApplePlatform platform, int major, int minor, int build = 0, bool throwIfOtherPlatform = true)
	{
		switch (platform) {
		case ApplePlatform.iOS:
			return CheckiOSSystemVersion (major, minor, throwIfOtherPlatform);
		case ApplePlatform.MacOSX:
			return CheckMacSystemVersion (major, minor, build, throwIfOtherPlatform);
		case ApplePlatform.TVOS:
			return ChecktvOSSystemVersion (major, minor, throwIfOtherPlatform);
		case ApplePlatform.MacCatalyst:
			return CheckMacCatalystSystemVersion (major, minor, throwIfOtherPlatform);
		default:
			throw new Exception ($"Unknown platform: {platform}");
		}
	}

	public static void AssertSystemVersion (ApplePlatform platform, int major, int minor, int build = 0, bool throwIfOtherPlatform = true)
	{
		switch (platform) {
		case ApplePlatform.iOS:
			AssertiOSSystemVersion (major, minor, throwIfOtherPlatform);
			break;
		case ApplePlatform.MacOSX:
			AssertMacSystemVersion (major, minor, build, throwIfOtherPlatform);
			break;
		case ApplePlatform.TVOS:
			AsserttvOSSystemVersion (major, minor, throwIfOtherPlatform);
			break;
		case ApplePlatform.MacCatalyst:
			AssertMacCatalystSystemVersion (major, minor, build, throwIfOtherPlatform);
			break;
		default:
			throw new Exception ($"Unknown platform: {platform}");
		}
	}

	/// <summary>Calls Assert.Ignore if we're running on an earlier OS version than the highest we support.</summary>
	public static void AssertMatchingOSVersionAndSdkVersion ()
	{
		var sdk = new Version (Constants.SdkVersion);
		if (CheckSystemVersion (CurrentPlatform, sdk.Major, sdk.Minor, sdk.Build == -1 ? 0 : sdk.Build))
			return;
		Assert.Ignore ($"This test only executes using the latest OS version ({sdk.Major}.{sdk.Minor})");
	}

	// This method returns true if:
	// system version >= specified version
	// AND
	// sdk version >= specified version
	static bool CheckiOSSystemVersion (int major, int minor, bool throwIfOtherPlatform = true)
	{
#if __IOS__
		return UIDevice.CurrentDevice.CheckSystemVersion (major, minor);
#else
		if (throwIfOtherPlatform)
			throw new Exception ("Can't get iOS System version on other platforms.");
		return true;
#endif
	}

	static void AssertiOSSystemVersion (int major, int minor, bool throwIfOtherPlatform = true)
	{
		if (!CheckiOSSystemVersion (major, minor, throwIfOtherPlatform))
			NUnit.Framework.Assert.Ignore ($"This test requires iOS {major}.{minor}");
	}

	static bool CheckExactiOSSystemVersion (int major, int minor)
	{
#if __IOS__
		var version = Version.Parse (UIDevice.CurrentDevice.SystemVersion);
		return version.Major == major && version.Minor == minor;
#else
		throw new Exception ("Can't get iOS System version on other platforms.");
#endif
	}

	static bool CheckExacttvOSSystemVersion (int major, int minor)
	{
#if __TVOS__
		var version = Version.Parse (UIDevice.CurrentDevice.SystemVersion);
		return version.Major == major && version.Minor == minor;
#else
		throw new Exception ("Can't get tvOS System version on other platforms.");
#endif
	}

	static bool CheckExactmacOSSystemVersion (int major, int minor, int build = 0)
	{
#if __MACOS__
		var v = NSProcessInfo.ProcessInfo.OperatingSystemVersion;
		var currentVersion = new Version ((int) v.Major, (int) v.Minor, (int) v.PatchVersion);
		if (currentVersion == new Version (10, 16, 0))
			currentVersion = new Version (11, 0, 0);
		return currentVersion == new Version (major, minor, build);
#else
		throw new Exception ("Can't get macOS System version on other platforms.");
#endif
	}

	// This method returns true if:
	// system version >= specified version
	// AND
	// sdk version >= specified version
	static bool ChecktvOSSystemVersion (int major, int minor, bool throwIfOtherPlatform = true)
	{
#if __TVOS__
		return UIDevice.CurrentDevice.CheckSystemVersion (major, minor);
#else
		if (throwIfOtherPlatform)
			throw new Exception ("Can't get tvOS System version on other platforms.");
		return true;
#endif
	}

	static void AsserttvOSSystemVersion (int major, int minor, bool throwIfOtherPlatform = true)
	{
		if (!ChecktvOSSystemVersion (major, minor, throwIfOtherPlatform))
			NUnit.Framework.Assert.Ignore ($"This test requires tvOS {major}.{minor}");
	}

	static bool CheckMacSystemVersion (int major, int minor, int build = 0, bool throwIfOtherPlatform = true)
	{
#if MONOMAC
		return OSXVersion >= new Version (major, minor, build);
#else
		if (throwIfOtherPlatform)
			throw new Exception ("Can't get iOS System version on other platforms.");
		return true;
#endif
	}

	static bool CheckMacCatalystSystemVersion (int major, int minor, bool throwIfOtherPlatform = true)
	{
#if __MACCATALYST__
		return UIDevice.CurrentDevice.CheckSystemVersion (major, minor);
#else
		if (throwIfOtherPlatform)
			throw new Exception ("Can't get Mac Catalyst System version on other platforms.");
		return true;
#endif
	}

	static void AssertMacSystemVersion (int major, int minor, int build = 0, bool throwIfOtherPlatform = true)
	{
		if (!CheckMacSystemVersion (major, minor, build, throwIfOtherPlatform))
			NUnit.Framework.Assert.Ignore ($"This test requires macOS {major}.{minor}.{build}");
	}

	static void AssertMacCatalystSystemVersion (int major, int minor, int build = 0, bool throwIfOtherPlatform = true)
	{
		if (!CheckMacCatalystSystemVersion (major, minor, throwIfOtherPlatform))
			NUnit.Framework.Assert.Ignore ($"This test requires macOS {major}.{minor}.{build}");
	}

	public static bool CheckSDKVersion (int major, int minor)
	{
#if !MONOMAC && !__MACCATALYST__
		if (Runtime.Arch == Arch.SIMULATOR || !UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
			// dyld_get_program_sdk_version was introduced with iOS 6.0, so don't do the SDK check on older deviecs.
			return true; // dyld_get_program_sdk_version doesn't return what we're looking for on the mac.
		}
#endif

		var sdk = GetSDKVersion ();
		if (sdk.Major > major)
			return true;
		if (sdk.Major == major && sdk.Minor >= minor)
			return true;
		return false;
	}

	public static void IgnoreOnTVOS ()
	{
#if __TVOS__
		NUnit.Framework.Assert.Ignore ("This test is disabled on TVOS.");
#endif
	}

	public static bool IsTVOS {
		get {
#if __TVOS__
			return true;
#else
			return false;
#endif
		}
	}

	public static bool IsDevice {
		get {
#if __MACOS__ || __MACCATALYST__
			return false;
#else
			return Runtime.Arch == Arch.DEVICE;
#endif
		}
	}

	public static bool IsSimulator {
		get {
#if __MACOS__ || __MACCATALYST__
			return false;
#else
			return Runtime.Arch == Arch.SIMULATOR;
#endif
		}
	}

	public static bool IsSimulatorOrDesktop {
		get {
#if __MACOS__ || __MACCATALYST__
			return true;
#else
			return Runtime.Arch == Arch.SIMULATOR;
#endif
		}
	}

	public static bool IsiPad {
		get {
#if __MACOS__
			return false;
#else
			return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
#endif
		}
	}

	public static bool IsiPhone {
		get {
#if __MACOS__
			return false;
#else
			return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
#endif
		}
	}

	public static void IgnoreOnMacCatalyst (string message = "")
	{
#if __MACCATALYST__
		NUnit.Framework.Assert.Ignore ($"This test is disabled on Mac Catalyst. {message}");
#endif
	}

	public static bool IgnoreTestThatRequiresSystemPermissions ()
	{
		return !string.IsNullOrEmpty (Environment.GetEnvironmentVariable ("DISABLE_SYSTEM_PERMISSION_TESTS"));
	}

	public static void CheckBluetoothPermission (bool assert_granted = false)
	{
		// New in Xcode11
		switch (CBManager.Authorization) {
		case CBManagerAuthorization.NotDetermined:
			if (IgnoreTestThatRequiresSystemPermissions ())
				NUnit.Framework.Assert.Ignore ("This test would show a dialog to ask for permission to use bluetooth.");
			break;
		case CBManagerAuthorization.Denied:
		case CBManagerAuthorization.Restricted:
			if (assert_granted)
				NUnit.Framework.Assert.Fail ("This test requires permission to use bluetooth.");
			break;
		}
	}

#if !MONOMAC && !__TVOS__
	public static void RequestCameraPermission (NSString mediaTypeToken, bool assert_granted = false)
	{
#if __MACCATALYST__
		// Microphone requires a hardened runtime entitlement for Mac Catalyst: com.apple.security.device.microphonee,
		// so just ignore these tests for now.
		NUnit.Framework.Assert.Ignore ("Requires a hardened runtime entitlement: com.apple.security.device.microphone");
#endif // __MACCATALYST__

		if (AVCaptureDevice.GetAuthorizationStatus (mediaTypeToken) == AVAuthorizationStatus.NotDetermined) {
			if (IgnoreTestThatRequiresSystemPermissions ())
				NUnit.Framework.Assert.Ignore ("This test would show a dialog to ask for permission to access the camera.");

			AVCaptureDevice.RequestAccessForMediaType (mediaTypeToken, (accessGranted) => {
				Console.WriteLine ("Camera permission {0}", accessGranted ? "granted" : "denied");
			});
		}

		switch (AVCaptureDevice.GetAuthorizationStatus (AVMediaTypes.Video.GetConstant ()!)) {
		case AVAuthorizationStatus.Restricted:
		case AVAuthorizationStatus.Denied:
			if (assert_granted)
				NUnit.Framework.Assert.Fail ("This test requires permission to access the camera.");
			break;
		}
	}
#endif // !!MONOMAC && !__TVOS__

#if !__TVOS__
	public static void CheckContactsPermission (bool assert_granted = false)
	{
		switch (CNContactStore.GetAuthorizationStatus (CNEntityType.Contacts)) {
		case CNAuthorizationStatus.NotDetermined:
			if (IgnoreTestThatRequiresSystemPermissions ())
				NUnit.Framework.Assert.Ignore ("This test would show a dialog to ask for permission to access the contacts.");
			// We don't request access here, because there's no global method to request access (an contact store instance is required).
			// Interestingly there is a global method to determine if access has been granted...
			break;
		case CNAuthorizationStatus.Restricted:
		case CNAuthorizationStatus.Denied:
			if (assert_granted)
				NUnit.Framework.Assert.Fail ("This test requires permission to access the contacts.");
			break;
		}
	}

	public static void RequestContactsPermission (bool assert_granted = false)
	{
		switch (CNContactStore.GetAuthorizationStatus (CNEntityType.Contacts)) {
		case CNAuthorizationStatus.NotDetermined:
			if (IgnoreTestThatRequiresSystemPermissions ())
				NUnit.Framework.Assert.Ignore ("This test would show a dialog to ask for permission to access the contacts.");

			// There's a static method to check for permission, but an instance method to ask for permission
			using (var store = new CNContactStore ()) {
				store.RequestAccess (CNEntityType.Contacts, (granted, error) => {
					Console.WriteLine ("Contacts permission {0} (error: {1})", granted ? "granted" : "denied", error);
				});
			}
			break;
		}

		CheckContactsPermission (assert_granted);
	}

#endif // !__TVOS__

#if !MONOMAC && !__TVOS__
	public static void CheckAddressBookPermission (bool assert_granted = false)
	{
#if __MACCATALYST__
		NUnit.Framework.Assert.Ignore ("CheckAddressBookPermission -> Ignore in MacCat, it hangs since our TCC hack does not work on BS.");
#endif
#pragma warning disable CA1422 // warning CA1422: This call site is reachable on: 'MacCatalyst' 13.3 and later. 'ABAuthorizationStatus.*' is obsoleted on: 'maccatalyst' 9.0 and later (Use the 'Contacts' API instead.).
		switch (ABAddressBook.GetAuthorizationStatus ()) {
		case ABAuthorizationStatus.NotDetermined:
			if (IgnoreTestThatRequiresSystemPermissions ())
				NUnit.Framework.Assert.Ignore ("This test would show a dialog to ask for permission to access the address book.");
			// We don't request access here, because there's no global method to request access (an addressbook instance is required).
			// Interestingly there is a global method to determine if access has been granted...
			break;
		case ABAuthorizationStatus.Restricted:
		case ABAuthorizationStatus.Denied:
			if (assert_granted)
				NUnit.Framework.Assert.Fail ("This test requires permission to access the address book.");
			break;
		}
	}
#pragma warning restore CA1422
#endif // !MONOMAC && !__TVOS__

	public static void RequestMicrophonePermission (bool assert_granted = false)
	{
#if MONOMAC
		// It looks like macOS does not restrict access to the microphone.
#elif __TVOS__
		// tvOS doesn't have a (developer-accessible) microphone, but it seems to have API that requires developers 
		// to request microphone access on other platforms (which means that it makes sense to both run those tests
		// on tvOS (because the API's there) and to request microphone access (because that's required on other platforms).
#elif __MACCATALYST__
		// Microphone requires a hardened runtime entitlement for Mac Catalyst: com.apple.security.device.microphonee,
		// so just ignore these tests for now.
		NUnit.Framework.Assert.Ignore ("Requires a hardened runtime entitlement: com.apple.security.device.microphone");
#else
		if (!CheckXcodeVersion (6, 0))
			return; // The API to check/request permission isn't available in earlier versions, the dialog will just pop up.

		if (!CheckXcodeVersion (15, 0)) {
#pragma warning disable CA1422 // warning CA1422: This call site is reachable on: 'iOS' 12.2 and later, 'maccatalyst' 12.2 and later. 'AVAudioSession.RecordPermission' is obsoleted on: 'ios' 17.0 and later (Please use 'AVAudioApplication.RecordPermission' instead.), 'maccatalyst' 17.0 and later (Please use 'AVAudioApplication.RecordPermission' instead.).
			if (AVAudioSession.SharedInstance ().RecordPermission == AVAudioSessionRecordPermission.Undetermined) {
				if (IgnoreTestThatRequiresSystemPermissions ())
					NUnit.Framework.Assert.Ignore ("This test would show a dialog to ask for permission to access the microphone.");

				AVAudioSession.SharedInstance ().RequestRecordPermission ((bool granted) => {
					Console.WriteLine ("Microphone permission {0}", granted ? "granted" : "denied");
				});
			}

			switch (AVAudioSession.SharedInstance ().RecordPermission) { // iOS 8+
			case AVAudioSessionRecordPermission.Denied:
				if (assert_granted)
					NUnit.Framework.Assert.Fail ("This test requires permission to access the microphone.");
				break;
			}
#pragma warning restore CA1422
			return;
		}

		if (AVAudioApplication.SharedInstance.RecordPermission == AVAudioApplicationRecordPermission.Undetermined) {
			if (IgnoreTestThatRequiresSystemPermissions ())
				NUnit.Framework.Assert.Ignore ("This test would show a dialog to ask for permission to access the microphone.");

			AVAudioApplication.RequestRecordPermission ((bool granted) => {
				Console.WriteLine ("Microphone permission {0}", granted ? "granted" : "denied");
			});
		}

		switch (AVAudioApplication.SharedInstance.RecordPermission) {
		case AVAudioApplicationRecordPermission.Denied:
			if (assert_granted)
				NUnit.Framework.Assert.Fail ("This test requires permission to access the microphone.");
			break;
		}

#endif // !MONOMAC && !__TVOS__
	}

#if !MONOMAC && !__TVOS__
	public static void RequestMediaLibraryPermission (bool assert_granted = false)
	{
		if (!CheckXcodeVersion (7, 3)) {
			if (IgnoreTestThatRequiresSystemPermissions ())
				NUnit.Framework.Assert.Ignore ("This test might show a dialog to ask for permission to access the media library, but the API to check if a dialog is required (or to request permission) is not available in this OS version.");
			return;
		}

		if (MPMediaLibrary.AuthorizationStatus == MPMediaLibraryAuthorizationStatus.NotDetermined) {
			if (IgnoreTestThatRequiresSystemPermissions ())
				NUnit.Framework.Assert.Ignore ("This test would show a dialog to ask for permission to access the media library.");

			MPMediaLibrary.RequestAuthorization ((access) => {
				Console.WriteLine ("Media library permission: {0}", access);
			});
		}

		switch (MPMediaLibrary.AuthorizationStatus) {
		case MPMediaLibraryAuthorizationStatus.Denied:
		case MPMediaLibraryAuthorizationStatus.Restricted:
			if (assert_granted)
				NUnit.Framework.Assert.Fail ("This test requires permission to access the media library.");
			break;
		}
	}
#endif // !MONOMAC && !__TVOS__

#if __MACOS__ || __MACCATALYST__
	public static void RequestEventStorePermission (EKEntityType entityType, bool assert_granted = false)
	{
		TestRuntime.AssertMacSystemVersion (10, 9, throwIfOtherPlatform: false);

		var status = EKEventStore.GetAuthorizationStatus (entityType);
		Console.WriteLine ("EKEventStore.GetAuthorizationStatus ({1}): {0}", status, entityType);
		switch (status) {
		case EKAuthorizationStatus.Authorized:
		case EKAuthorizationStatus.Restricted:
			return;
		case EKAuthorizationStatus.NotDetermined:
			// There's an instance method on EKEventStore to request permission,
			// but creating the instance can end up blocking the app showing a permission dialog...
			// (on Mavericks at least)
#if __MACCATALYST__
			return; // Crossing fingers that this won't hang.
#else
			if (TestRuntime.CheckMacSystemVersion (10, 10))
				return; // Crossing fingers that this won't hang.
			NUnit.Framework.Assert.Ignore ("This test requires permission to access events, but there's no API to request access without potentially showing dialogs.");
			break;
#endif
		case EKAuthorizationStatus.Denied:
			if (assert_granted)
				NUnit.Framework.Assert.Ignore ("This test requires permission to access events.");
			break;
		}
	}
#endif

#if __MACOS__
	public static global::CoreGraphics.CGColor GetCGColor (NSColor color)
#else
	public static global::CoreGraphics.CGColor GetCGColor (UIColor color)
#endif
	{
#if __MACOS__
		var components = new nfloat [color.ComponentCount];
		color.GetComponents (out components);
		NSApplication.CheckForIllegalCrossThreadCalls = false;
		var cs = color.ColorSpace.ColorSpace;
		NSApplication.CheckForIllegalCrossThreadCalls = true;
		return new global::CoreGraphics.CGColor (cs, components);
#else
		return color.CGColor;
#endif
	}

	public static uint GetFlags (NSObject obj)
	{
		const string fieldName = "actual_flags";
		return (uint) typeof (NSObject).GetField (fieldName, BindingFlags.Instance | BindingFlags.GetField | BindingFlags.NonPublic)!.GetValue (obj)!;
	}

	// Determine if linkall was enabled by checking if an unused class in this assembly is still here.
	static bool? link_all;
	[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "This property checks whether the trimmer is enabled by checking if a type survived trimming; it's thus trimmer safe in that the any behavioral difference when the trimmer is enabled is exactly what it's looking for.")]
	public static bool IsLinkAll {
		get {
			if (!link_all.HasValue)
				link_all = typeof (TestRuntime).Assembly.GetType (typeof (TestRuntime).FullName + "+LinkerSentinel") is null;
			return link_all.Value;
		}
	}
	class LinkerSentinel { }

	// Determine if any assemblies were linked by checking if a few uncommon classes in corlib are still here.
	static bool? link_any;
	[UnconditionalSuppressMessage ("Trimming", "IL2026", Justification = "This property checks whether the trimmer is enabled by checking if a type survived trimming; it's thus trimmer safe in that the any behavioral difference when the trimmer is enabled is exactly what it's looking for.")]
	public static bool IsLinkAny {
		get {
			if (!link_any.HasValue) {
				var uncommonTypes = new string [] {
					"System.Action`14",
					"System.DBNull",
					"System.Diagnostics.Debugger",
					"System.Func`15",
				};
				link_any = false;
				foreach (var uncommonType in uncommonTypes) {
					link_any = typeof (int).Assembly.GetType (uncommonType) is null;
					if (link_any == true)
						break;
				}
			}
			return link_any.Value;
		}
	}

	public static bool IsOptimizeAll {
		get {
#if OPTIMIZEALL
			return true;
#else
			return false;
#endif
		}
	}

	// There's no official API yet for distinguishing between CoreCLR and MonoVM (https://github.com/dotnet/runtime/issues/49481)
	// (checking for the Mono.Runtime type doesn't work, because the BCL is the same, so there's never a Mono.Runtime type).
	// However, the System.__Canon type seems to be CoreCLR-only.
	public static bool IsCoreCLR {
		get {
			return !(Type.GetType ("System.__Canon") is null);
		}
	}

	public static void IgnoreInCIIfBadNetwork (Exception? ex)
	{
		if (ex is null)
			return;

		IgnoreInCIfHttpStatusCodes (ex, HttpStatusCode.BadGateway, HttpStatusCode.GatewayTimeout, HttpStatusCode.ServiceUnavailable);
		IgnoreInCIIfNetworkConnectionLost (ex);
		IgnoreInCIIfDnsResolutionFailed (ex);
	}

	public static void IgnoreInCIIfBadNetwork (NSError? error)
	{
		if (error is null)
			return;

		IgnoreInCIIfNetworkConnectionLost (error);
		IgnoreInCIIfNoNetworkConnection (error);
		IgnoreInCIIfDnsResolutionFailed (error);
		IgnoreInCIIfTimedOut (error);
	}

	public static void IgnoreInCIIfDnsResolutionFailed (Exception ex)
	{
		var se = FindInner<System.Net.Sockets.SocketException> (ex);
		if (se is null)
			return;

		var isDnsResolutionFailed = false;
		if (se.ErrorCode == 8 /* EAI_NONAME: 'hostname or servname not provided, or not known' */) {
			isDnsResolutionFailed = true;
		} else if (se.Message.Contains ("hostname or servname not provided, or not known")) {
			isDnsResolutionFailed = true;
		}
		if (!isDnsResolutionFailed)
			return;

		IgnoreInCI ($"Ignored due to DNS resolution failure '{se.Message}'");
	}

	public static void IgnoreInCIIfDnsResolutionFailed (NSError error)
	{
		IgnoreNetworkError (error, CFNetworkErrors.CannotFindHost);
	}

	public static void IgnoreInCIIfTimedOut (NSError error)
	{
		IgnoreNetworkError (error, CFNetworkErrors.TimedOut);
	}

	public static void IgnoreInCIIfForbidden (Exception ex)
	{
		IgnoreInCIfHttpStatusCodes (ex, HttpStatusCode.BadGateway, HttpStatusCode.GatewayTimeout, HttpStatusCode.ServiceUnavailable, HttpStatusCode.Forbidden);
	}

	public static void IgnoreInCIIfBadNetwork (HttpStatusCode status)
	{
		IgnoreInCIfHttpStatusCodes (status, HttpStatusCode.BadGateway, HttpStatusCode.GatewayTimeout, HttpStatusCode.ServiceUnavailable);
	}

	public static void IgnoreInCIfHttpStatusCodes (HttpStatusCode status, params HttpStatusCode [] statusesToIgnore)
	{
		if (Array.IndexOf (statusesToIgnore, status) < 0)
			return;

		IgnoreInCI ($"Ignored due to http status code '{status}'");
	}

	public static void IgnoreInCIfHttpStatusCodes (Exception ex, params HttpStatusCode [] statusesToIgnore)
	{
		if (!TryGetHttpStatusCode (ex, out var status))
			return;

		if (Array.IndexOf (statusesToIgnore, status) < 0)
			return;

		IgnoreInCI ($"Ignored due to http status code '{status}': {ex.Message}");
	}

	public static void IgnoreInCIIfNetworkConnectionLost (Exception ex)
	{
		if (!(ex is NSErrorException nex))
			return;

		IgnoreInCIIfNetworkConnectionLost (nex.Error);
	}

	public static void IgnoreInCIIfNetworkConnectionLost (NSError error)
	{
		// <Foundation.NSErrorException: Error Domain=NSURLErrorDomain Code=-1005 "The network connection was lost." UserInfo ...
		IgnoreNetworkError (error, CFNetworkErrors.NetworkConnectionLost);
	}

	public static void IgnoreInCIIfNoNetworkConnection (NSError error)
	{
		// Error Domain=NSURLErrorDomain Code=-1009 "The Internet connection appears to be offline."
		IgnoreNetworkError (error, CFNetworkErrors.NotConnectedToInternet);
	}

	static void IgnoreNetworkError (NSError error, params CFNetworkErrors [] errors)
	{
		if (error is null)
			return;

		if (error.Domain != NSError.NSUrlErrorDomain && error.Domain != NSError.CFNetworkErrorDomain)
			return;

		foreach (var e in errors) {
			if (error.Code == (nint) (long) e)
				IgnoreInCI ($"Ignored due to network error: {error}");
		}
	}

	static T? FindInner<T> (Exception? ex) where T : Exception
	{
		while (ex is not null) {
			if (ex is T target)
				return target;
			ex = ex.InnerException;
		}
		return null;
	}

	static bool TryGetHttpStatusCode (Exception ex, out HttpStatusCode status)
	{
		status = (HttpStatusCode) 0;

		if (ex is HttpRequestException hre) {
			if (hre.StatusCode.HasValue) {
				status = hre.StatusCode.Value;
				return true;
			}
			return false;
		}

		var we = ex as WebException;
		if (we is null)
			return false;

		var repsonseStatus = (we.Response as HttpWebResponse)?.StatusCode;
		if (repsonseStatus.HasValue) {
			status = repsonseStatus.Value;
			return true;
		}

		var message = we.Message;
		if (we.Message.Contains ("(502)")) {
			status = (HttpStatusCode) 502;
			return true;
		}

		if (we.Message.Contains ("(503)")) {
			status = (HttpStatusCode) 503;
			return true;
		}

		return false;
	}

	public static void NotifyLaunchCompleted ()
	{
		var env = Environment.GetEnvironmentVariable ("LAUNCH_SENTINEL_FILE");
		if (!string.IsNullOrEmpty (env))
			File.WriteAllText (env, "Launched!"); // content doesn't matter, the file just has to exist.
	}

	enum NXByteOrder /* unspecified in header, means most likely int */ {
		Unknown,
		LittleEndian,
		BigEndian,
	}

	[StructLayout (LayoutKind.Sequential)]
	struct NXArchInfo {
		IntPtr name; // const char *
		public int CpuType; // cpu_type_t -> integer_t -> int
		public int CpuSubType; // cpu_subtype_t -> integer_t -> int
		public NXByteOrder ByteOrder;
		IntPtr description; // const char *

		public string Name {
			get { return Marshal.PtrToStringAuto (name)!; }
		}

		public string Description {
			get { return Marshal.PtrToStringAuto (description)!; }
		}
	}

	[DllImport (Constants.libSystemLibrary)]
	static unsafe extern NXArchInfo* NXGetLocalArchInfo ();

	public unsafe static bool IsARM64 {
		get { return NXGetLocalArchInfo ()->Name.StartsWith ("arm64"); }
	}

	[DllImport ("__Internal")]
	extern static void xamarin_log (IntPtr s);

	// Calling Console.WriteLine from inside a test is rather annoying, because NUnit captures stdout and only
	// shows it at the end of the test. That's not very helpful when the test crashes, or while debugging
	// a test (in a debugger).
	internal static void NSLog (string value)
	{
		var valuePtr = Marshal.StringToHGlobalUni (value);
		xamarin_log (valuePtr);
		Marshal.FreeHGlobal (valuePtr);
	}

	public static void AssertNoNonNUnitException (Exception ex, string message)
	{
		switch (ex) {
		case SuccessException: throw new SuccessException (ex.Message, ex);
		case IgnoreException: throw new IgnoreException (ex.Message, ex);
		case AssertionException: throw new AssertionException (ex.Message, ex);
		case InconclusiveException: throw new InconclusiveException (ex.Message, ex);
		case ResultStateException: throw ex;
		default:
			Assert.IsNull (ex, message);
			break;
		}
	}

	// this only applies to macOS, we can't determine it for other platforms.
	public static void IgnoreIfLockedScreen ()
	{
#if __MACOS__
		var props = global::CoreGraphics.CGSession.GetProperties ();
		var value = props?.Dictionary [(NSString) "CGSSessionScreenIsLocked"]; // This key isn't documented, so no binding for it.
		var isLocked = (value as NSNumber)?.BoolValue;
		if (isLocked == true)
			Assert.Ignore ("The screen is locked.");
#endif
	}
}

internal static class NativeHandleExtensions {
	public static string ToString (this NativeHandle @this, string format)
	{
		return ((IntPtr) @this).ToString (format);
	}
}
