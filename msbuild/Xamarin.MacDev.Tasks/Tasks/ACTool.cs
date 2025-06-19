using System;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Xamarin.Localization.MSBuild;
using Xamarin.Messaging.Build.Client;
using Xamarin.Utils;

namespace Xamarin.MacDev.Tasks {
	public class ACTool : XcodeCompilerToolTask, ICancelableTask {
		string? outputSpecs;
		string? partialAppManifestPath;

		#region Inputs

		public string AccentColor { get; set; } = string.Empty;

		public ITaskItem [] AlternateAppIcons { get; set; } = Array.Empty<ITaskItem> ();

		// The name of an app icon
		public string AppIcon { get; set; } = string.Empty;

		public string DeviceModel { get; set; } = string.Empty;

		public string DeviceOSVersion { get; set; } = string.Empty;

		public bool EnableOnDemandResources { get; set; }

		[Required]
		public ITaskItem [] ImageAssets { get; set; } = Array.Empty<ITaskItem> ();

		public bool IncludeAllAppIcons { get; set; }

		public bool IsWatchApp { get; set; }

		[Required]
		public bool OptimizePNGs { get; set; }

		[Required]
		public string OutputPath { get; set; } = string.Empty;

		#endregion

		#region Outputs

		[Output]
		public ITaskItem? PartialAppManifest { get; set; }

		#endregion

		// All the icons among the 'ImageAssets'.
		HashSet<string> appIconsInAssets = new (); // iOS, macOS and Mac Catalyst
		HashSet<string> brandAssetsInAssets = new (); // tvOS
		HashSet<string> imageStacksInAssets = new (); // tvOS

		protected override string DefaultBinDir {
			get { return DeveloperRootBinDir; }
		}

		protected override string ToolName {
			get { return "actool"; }
		}

		bool IsMessagesExtension {
			get {
				return NSExtensionPointIdentifier == "com.apple.message-payload-provider";
			}
		}

		protected override void AppendCommandLineArguments (IDictionary<string, string?> environment, List<string> args, ITaskItem [] items)
		{
			var assetDirs = new HashSet<string> (items.Select (x => BundleResource.GetVirtualProjectPath (this, x)));

			if (!string.IsNullOrEmpty (XSAppIconAssets) && !string.IsNullOrEmpty (AppIcon)) {
				Log.LogError (MSBStrings.E7129 /* Can't specify both 'XSAppIconAssets' in the Info.plist and 'AppIcon' in the project file. Please select one or the other. */);
				return;
			}

			if (!string.IsNullOrEmpty (XSAppIconAssets)) {
				int index = XSAppIconAssets.IndexOf (".xcassets" + Path.DirectorySeparatorChar, StringComparison.Ordinal);
				string? assetDir = null;
				var rpath = XSAppIconAssets;

				if (index != -1)
					assetDir = rpath.Substring (0, index + ".xcassets".Length);

				if (assetDirs is not null && assetDir is not null && assetDirs.Contains (assetDir)) {
					var assetName = Path.GetFileNameWithoutExtension (rpath);

					args.Add ("--app-icon");
					args.Add (assetName);

					if (IsMessagesExtension) {
						args.Add ("--product-type");
						args.Add ("com.apple.product-type.app-extension.messages");
					}
				}
			}

			if (!string.IsNullOrEmpty (AccentColor)) {
				args.Add ("--accent-color");
				args.Add (AccentColor);
			}

			if (!string.IsNullOrEmpty (XSLaunchImageAssets)) {
				int index = XSLaunchImageAssets.IndexOf (".xcassets" + Path.DirectorySeparatorChar, StringComparison.Ordinal);
				string? assetDir = null;
				var rpath = XSLaunchImageAssets;

				if (index != -1)
					assetDir = rpath.Substring (0, index + ".xcassets".Length);

				if (assetDirs is not null && assetDir is not null && assetDirs.Contains (assetDir)) {
					var assetName = Path.GetFileNameWithoutExtension (rpath);
					args.Add ("--launch-image");
					args.Add (assetName);
				}
			}

			if (!string.IsNullOrEmpty (CLKComplicationGroup)) {
				args.Add ("--complication");
				args.Add (CLKComplicationGroup);
			}

			if (OptimizePNGs)
				args.Add ("--compress-pngs");

			if (AppleSdkSettings.XcodeVersion.Major >= 7) {
				if (!string.IsNullOrEmpty (outputSpecs)) {
					args.Add ("--enable-on-demand-resources");
					args.Add (EnableOnDemandResources ? "YES" : "NO");
				}

				if (!string.IsNullOrEmpty (DeviceModel)) {
					args.Add ("--filter-for-device-model");
					args.Add (DeviceModel);
				}

				if (!string.IsNullOrEmpty (DeviceOSVersion)) {
					args.Add ("--filter-for-device-os-version");
					args.Add (DeviceOSVersion);
				}

				if (!string.IsNullOrEmpty (outputSpecs)) {
					args.Add ("--asset-pack-output-specifications");
					args.Add (Path.GetFullPath (outputSpecs));
				}
			}

			if (Platform == ApplePlatform.MacCatalyst) {
				args.Add ("--ui-framework-family");
				args.Add ("uikit");
			}

			foreach (var targetDevice in GetTargetDevices ()) {
				args.Add ("--target-device");
				args.Add (targetDevice);
			}

			if (!string.IsNullOrEmpty (MinimumOSVersion)) {
				args.Add ("--minimum-deployment-target");
				args.Add (MinimumOSVersion);
			}

			var platform = PlatformUtils.GetTargetPlatform (SdkPlatform, IsWatchApp);

			if (platform is not null) {
				args.Add ("--platform");
				args.Add (platform);
			}

			if (!string.IsNullOrEmpty (AppIcon)) {
				if (Platform == ApplePlatform.TVOS) {
					if (!brandAssetsInAssets.Contains (AppIcon)) {
						Log.LogError (MSBStrings.E7130 /* Can't find the AppIcon '{0}' among the image resources. There are {1} app icons in the image resources: {2} */, AppIcon, brandAssetsInAssets.Count, string.Join (", ", brandAssetsInAssets.OrderBy (v => v)));
						return;
					}
				} else {
					if (!appIconsInAssets.Contains (AppIcon)) {
						Log.LogError (MSBStrings.E7130 /* Can't find the AppIcon '{0}' among the image resources. There are {1} app icons in the image resources: {2} */, AppIcon, appIconsInAssets.Count, string.Join (", ", appIconsInAssets.OrderBy (v => v)));
						return;
					}
				}
				args.Add ("--app-icon");
				args.Add (AppIcon);
			}

			foreach (var alternate in AlternateAppIcons) {
				var alternateAppIcon = alternate.ItemSpec!;
				if (Platform == ApplePlatform.TVOS) {
					if (!imageStacksInAssets.Contains (alternateAppIcon)) {
						Log.LogError (MSBStrings.E7127 /* Can't find the AlternateAppIcon '{0}' among the image resources. There are {1} app icons in the image resources: {2}. */, alternateAppIcon, imageStacksInAssets.Count, string.Join (", ", imageStacksInAssets.OrderBy (v => v)));
						return;
					}
				} else {
					if (!appIconsInAssets.Contains (alternateAppIcon)) {
						Log.LogError (MSBStrings.E7127 /* Can't find the AlternateAppIcon '{0}' among the image resources. There are {1} app icons in the image resources: {2}. */, alternateAppIcon, appIconsInAssets.Count, string.Join (", ", appIconsInAssets.OrderBy (v => v)));
						return;
					}
				}
				if (string.Equals (alternateAppIcon, AppIcon, StringComparison.OrdinalIgnoreCase)) {
					Log.LogError (MSBStrings.E7128 /* The image resource '{0}' is specified as both 'AppIcon' and 'AlternateAppIcon'. */, AppIcon);
					return;
				}
				// This doesn't seem to be necessary/applicable for tvOS (it also triggers a warning from actool)
				args.Add ("--alternate-app-icon");
				args.Add (alternateAppIcon);
			}

			if (IncludeAllAppIcons)
				args.Add ("--include-all-app-icons");

			args.Add ("--output-partial-info-plist");
			args.Add (Path.GetFullPath (partialAppManifestPath));
		}

		IEnumerable<ITaskItem> GetCompiledBundleResources (PDictionary output, string intermediateBundleDir)
		{
			var pwd = PathUtils.ResolveSymbolicLinks (Environment.CurrentDirectory);

			if (output.TryGetValue (string.Format ("com.apple.{0}.compilation-results", ToolName), out PDictionary? dict) && dict.TryGetValue ("output-files", out PArray? array)) {
				foreach (var path in array.OfType<PString> ().Select (x => x.Value)) {
					// don't include the generated plist files as BundleResources
					if (path.EndsWith ("partial-info.plist", StringComparison.Ordinal))
						continue;

					var resolvedPath = PathUtils.ResolveSymbolicLinks (path);
					var vpath = PathUtils.AbsoluteToRelative (pwd, resolvedPath);
					var item = new TaskItem (vpath);

					// Note: the intermediate bundle dir functions as a top-level bundle dir
					var logicalName = PathUtils.AbsoluteToRelative (intermediateBundleDir, resolvedPath);

					if (logicalName.StartsWith ("../OnDemandResources/", StringComparison.Ordinal)) {
						logicalName = logicalName.Substring (3);

						var outputPath = Path.Combine (OutputPath, logicalName);

						item.SetMetadata ("OutputPath", outputPath);
					}

					item.SetMetadata ("LogicalName", logicalName);
					item.SetMetadata ("Optimize", "false");

					yield return item;
				}
			}

			yield break;
		}

		void FindXCAssetsDirectory (string main, string secondary, out string mainResult, out string secondaryResult)
		{
			mainResult = main;
			secondaryResult = secondary;

			while (!string.IsNullOrEmpty (mainResult) && !mainResult.EndsWith (".xcassets", StringComparison.OrdinalIgnoreCase)) {
				mainResult = Path.GetDirectoryName (mainResult)!;
				if (!string.IsNullOrEmpty (secondaryResult))
					secondaryResult = Path.GetDirectoryName (secondaryResult)!;
			}
		}

		public override bool Execute ()
		{
			if (ShouldExecuteRemotely ())
				return new TaskRunner (SessionId, BuildEngine4).RunAsync (this).Result;

			var intermediate = Path.Combine (IntermediateOutputPath, ToolName);
			var intermediateBundleDir = Path.Combine (intermediate, "bundle");
			var intermediateCloneDir = Path.Combine (intermediate, "cloned-assets");
			var manifest = new TaskItem (Path.Combine (intermediate, "asset-manifest.plist"));
			var bundleResources = new List<ITaskItem> ();
			var outputManifests = new List<ITaskItem> ();
			var catalogs = new List<ITaskItem> ();
			var unique = new HashSet<string> ();

			var knownSpecs = new HashSet<string> ();
			var clones = new HashSet<string> ();
			var items = new List<AssetInfo> ();
			var specs = new PArray ();

			var filteredImageAssets = ImageAssets
				.Where (item => {
					// Ignore MacOS .DS_Store files...
					return !Path.GetFileName (item.ItemSpec).Equals (".DS_Store", StringComparison.OrdinalIgnoreCase);
				});

			filteredImageAssets = CollectBundleResources.ComputeLogicalNameAndDetectDuplicates (this, filteredImageAssets, ProjectDir, string.Empty, "ImageAsset").ToArray ();

			var imageAssets = filteredImageAssets
				.Select (imageAsset => {
					var vpath = BundleResource.GetVirtualProjectPath (this, imageAsset);
					var catalogFullPath = imageAsset.GetMetadata ("FullPath");

					// get the parent (which will typically be .appiconset, .launchimage, .imageset, .iconset, etc)
					var catalog = Path.GetDirectoryName (vpath);
					catalogFullPath = Path.GetDirectoryName (catalogFullPath);

					var assetType = Path.GetExtension (catalog).TrimStart ('.');

					// keep walking up the directory structure until we get to the .xcassets directory
					FindXCAssetsDirectory (catalog, catalogFullPath, out var catalog2, out var catalogFullPath2);
					catalog = catalog2;
					catalogFullPath = catalogFullPath2;

					return new AssetInfo (imageAsset, vpath, catalog, catalogFullPath, assetType);
				})
				.Where (asset => {
					if (string.IsNullOrEmpty (asset.Catalog)) {
						Log.LogWarning (null, null, null, asset.Item.ItemSpec, 0, 0, 0, 0, MSBStrings.W0090, asset.Item.ItemSpec);
						return false;
					}
					return true;
				})
				.ToArray ();

			for (int i = 0; i < imageAssets.Length; i++) {
				var asset = imageAssets [i];
				var vpath = asset.VirtualProjectPath;
				var assetItem = asset.Item;

				if (!string.IsNullOrEmpty (assetItem.GetMetadata ("Link"))) {
					// Note: if any of the files within a catalog are linked, we'll have to clone the *entire* catalog
					clones.Add (asset.Catalog);
					continue;
				}

				// filter out everything except paths containing a Contents.json file since our main processing loop only cares about these
				if (Path.GetFileName (vpath) != "Contents.json")
					continue;

				items.Add (asset);
			}

			// clone any *.xcassets dirs that need cloning
			if (clones.Count > 0) {
				if (Directory.Exists (intermediateCloneDir))
					Directory.Delete (intermediateCloneDir, true);

				Directory.CreateDirectory (intermediateCloneDir);

				items.Clear ();

				for (int i = 0; i < imageAssets.Length; i++) {
					var asset = imageAssets [i];
					var vpath = asset.VirtualProjectPath;
					var assetItem = asset.Item;
					var clone = false;
					ITaskItem item;

					foreach (var catalog in clones) {
						if (vpath.Length > catalog.Length && vpath [catalog.Length] == '/' && vpath.StartsWith (catalog, StringComparison.Ordinal)) {
							clone = true;
							break;
						}
					}

					if (clone) {
						var src = assetItem.GetMetadata ("FullPath");

						if (!File.Exists (src)) {
							Log.LogError (null, null, null, src, 0, 0, 0, 0, MSBStrings.E0091, src);
							return false;
						}

						var dest = Path.Combine (intermediateCloneDir, vpath);
						var dir = Path.GetDirectoryName (dest);

						Directory.CreateDirectory (dir);

						File.Copy (src, dest, true);

						// filter out everything except paths containing a Contents.json file since our main processing loop only cares about these
						if (Path.GetFileName (vpath) != "Contents.json")
							continue;

						item = new TaskItem (dest);
						assetItem.CopyMetadataTo (item);
						item.SetMetadata ("Link", vpath);
						FindXCAssetsDirectory (Path.GetFullPath (dest), "", out var catalogFullPath, out var _);
						items.Add (new AssetInfo (item, vpath, asset.Catalog, catalogFullPath, asset.AssetType));
					} else {
						// filter out everything except paths containing a Contents.json file since our main processing loop only cares about these
						if (Path.GetFileName (vpath) != "Contents.json")
							continue;

						items.Add (asset);
					}
				}
			}

			// Note: `items` contains only the Contents.json files at this point
			for (int i = 0; i < items.Count; i++) {
				var asset = items [i];
				var assetItem = asset.Item;
				var vpath = asset.VirtualProjectPath;
				var catalog = asset.Catalog;
				var path = assetItem.GetMetadata ("FullPath");
				var assetType = asset.AssetType;

				if (Platform == ApplePlatform.TVOS) {
					if (assetType.Equals ("imagestack", StringComparison.OrdinalIgnoreCase)) {
						imageStacksInAssets.Add (Path.GetFileNameWithoutExtension (Path.GetDirectoryName (vpath)));
					} else if (assetType.Equals ("brandassets", StringComparison.OrdinalIgnoreCase)) {
						brandAssetsInAssets.Add (Path.GetFileNameWithoutExtension (Path.GetDirectoryName (vpath)));
					}
				} else {
					if (assetType.Equals ("appiconset", StringComparison.OrdinalIgnoreCase))
						appIconsInAssets.Add (Path.GetFileNameWithoutExtension (Path.GetDirectoryName (vpath)));
				}

				if (unique.Add (catalog)) {
					var item = new TaskItem (asset.CatalogFullPath);
					item.SetMetadata ("Link", catalog);

					catalogs.Add (item);
				}

				if (AppleSdkSettings.XcodeVersion.Major >= 7 && SdkPlatform != "WatchSimulator") {
					var text = File.ReadAllText (assetItem.ItemSpec);

					if (string.IsNullOrEmpty (text))
						continue;

					JsonDocument json;
					JsonElement value;

					try {
						var options = new JsonDocumentOptions () {
							AllowTrailingCommas = true,
						};
						json = JsonDocument.Parse (text, options);
					} catch (JsonException je) {
						var line = (int) (je.LineNumber + 1 ?? 0);
						var col = (int) (je.BytePositionInLine + 1 ?? 0);
						Log.LogError (null, null, null, assetItem.ItemSpec, line, col, line, col, "{0}", je.Message);
						return false;
					} catch (Exception e) {
						Log.LogError (null, null, null, assetItem.ItemSpec, 0, 0, 0, 0, MSBStrings.E0092, e.Message);
						return false;

					}

					if (!json.RootElement.TryGetProperty ("properties", out value) || value.ValueKind != JsonValueKind.Object)
						continue;

					var properties = value;

					if (!properties.TryGetProperty ("on-demand-resource-tags", out value) || value.ValueKind != JsonValueKind.Array)
						continue;

					var resourceTags = value;
					var tags = new HashSet<string> ();
					string hash;

					foreach (var tag in resourceTags.EnumerateArray ()) {
						if (tag.ValueKind == JsonValueKind.String)
							tags.Add (tag.GetString ()!);
					}

					var tagList = tags.ToList ();
					tagList.Sort ();

					var assetDir = AssetPackUtils.GetAssetPackDirectory (intermediate, BundleIdentifier, tagList, out hash);

					if (knownSpecs.Add (hash)) {
						var assetpack = new PDictionary ();
						var ptags = new PArray ();

						Directory.CreateDirectory (assetDir);

						for (int j = 0; j < tagList.Count; j++)
							ptags.Add (new PString (tagList [j]));

						assetpack.Add ("bundle-id", new PString (string.Format ("{0}.asset-pack-{1}", BundleIdentifier, hash)));
						assetpack.Add ("bundle-path", new PString (Path.GetFullPath (assetDir)));
						assetpack.Add ("tags", ptags);
						specs.Add (assetpack);
					}
				}
			}

			if (catalogs.Count == 0) {
				// There are no (supported?) asset catalogs
				return !Log.HasLoggedErrors;
			}

			partialAppManifestPath = Path.Combine (intermediate, "partial-info.plist");
			PartialAppManifest = new TaskItem (partialAppManifestPath);

			if (specs.Count > 0) {
				outputSpecs = Path.Combine (intermediate, "output-specifications.plist");
				specs.Save (outputSpecs, true);
			}

			Directory.CreateDirectory (intermediateBundleDir);

			if ((Compile (catalogs.ToArray (), intermediateBundleDir, manifest)) != 0)
				return false;

			if (Log.HasLoggedErrors)
				return false;

			if (!File.Exists (Path.GetFullPath (partialAppManifestPath)))
				Log.LogError (MSBStrings.E0093, Path.GetFullPath (partialAppManifestPath));

			try {
				var manifestOutput = PDictionary.FromFile (manifest.ItemSpec)!;

				LogWarningsAndErrors (manifestOutput, catalogs [0]);

				bundleResources.AddRange (GetCompiledBundleResources (manifestOutput, intermediateBundleDir));
				outputManifests.Add (manifest);
			} catch (Exception ex) {
				Log.LogError (MSBStrings.E0094, ToolName, manifest.ItemSpec, ex.Message);
			}

			foreach (var assetpack in specs.OfType<PDictionary> ()) {
				var path = Path.Combine (assetpack.GetString ("bundle-path").Value, "Info.plist");
				var bundlePath = PathUtils.AbsoluteToRelative (intermediate, path);
				var outputPath = Path.Combine (OutputPath, bundlePath);
				var rpath = Path.Combine (intermediate, bundlePath);
				var dict = new PDictionary ();

				dict.SetCFBundleIdentifier (assetpack.GetString ("bundle-id").Value);
				dict.Add ("Tags", assetpack.GetArray ("tags").Clone ());

				dict.Save (path, true, true);

				var item = new TaskItem (rpath);
				item.SetMetadata ("LogicalName", bundlePath);
				item.SetMetadata ("OutputPath", outputPath);
				item.SetMetadata ("Optimize", "false");

				bundleResources.Add (item);
			}

			BundleResources = bundleResources.ToArray ();
			OutputManifests = outputManifests.ToArray ();

			return !Log.HasLoggedErrors;
		}

		public void Cancel ()
		{
			if (ShouldExecuteRemotely ())
				BuildConnection.CancelAsync (BuildEngine4).Wait ();
		}
	}

	class AssetInfo {
		public ITaskItem Item;
		public string VirtualProjectPath;
		public string Catalog;
		public string CatalogFullPath;
		public string AssetType;
		public AssetInfo (ITaskItem item, string vpath, string catalog, string catalogFullPath, string assetType)
		{
			this.Item = item;
			this.VirtualProjectPath = vpath;
			this.Catalog = catalog;
			this.CatalogFullPath = catalogFullPath;
			this.AssetType = assetType;
		}
	}
}
