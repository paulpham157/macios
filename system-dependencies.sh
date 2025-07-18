#!/bin/bash -e

set -o pipefail

cd $(dirname $0)

FAIL=
PROVISION_DOWNLOAD_DIR=/tmp/x-provisioning
SUDO=sudo
VERBOSE=

OPTIONAL_SHARPIE=1
OPTIONAL_SIMULATORS=1
OPTIONAL_OLD_SIMULATORS=1

# parse command-line arguments
while ! test -z $1; do
	case $1 in
		--no-sudo)
			SUDO=
			shift
			;;
		--provision-xcode)
			PROVISION_XCODE=1
			unset IGNORE_XCODE
			shift
			;;
		--provision)
			# historical reasons :(
			PROVISION_XCODE=1
			PROVISION_VS=1
			unset IGNORE_XCODE
			unset IGNORE_VISUAL_STUDIO
			shift
			;;
		--provision-*-studio)
			PROVISION_VS=1
			unset IGNORE_VISUAL_STUDIO
			shift
			;;
		--provision-mono)
			PROVISION_MONO=1
			unset IGNORE_MONO
			shift
			;;
		--provision-7z)
			PROVISION_7Z=1
			unset IGNORE_7Z
			shift
			;;
		--provision-autotools)
			# this is an old argument, just ignore it
			shift
			;;
		--provision-python3)
			# building mono from source requires having python3 installed
			PROVISION_PYTHON3=1
			unset IGNORE_PYTHON3
			shift
			;;
		--provision-sharpie)
			PROVISION_SHARPIE=1
			unset OPTIONAL_SHARPIE
			unset IGNORE_SHARPIE
			shift
			;;
		--provision-simulators)
			PROVISION_SIMULATORS=1
			unset OPTIONAL_SIMULATORS
			unset IGNORE_SIMULATORS
			shift
			;;
		--provision-old-simulators)
			PROVISION_OLD_SIMULATORS=1
			unset OPTIONAL_OLD_SIMULATORS
			unset IGNORE_OLD_SIMULATORS
			shift
			;;
		--provision-dotnet)
			PROVISION_DOTNET=1
			unset IGNORE_DOTNET
			shift
			;;
		--provision-shellcheck)
			PROVISION_SHELLCHECK=1
			unset IGNORE_SHELLCHECK
			shift
			;;
		--provision-yamllint)
			PROVISION_YAMLLINT=1
			unset IGNORE_YAMLLINT
			shift
			;;
		--provision-all)
			PROVISION_MONO=1
			unset IGNORE_MONO
			PROVISION_VS=1
			unset IGNORE_VISUAL_STUDIO
			PROVISION_XCODE=1
			unset IGNORE_XCODE
			PROVISION_7Z=1
			unset IGNORE_7Z
			PROVISION_HOMEBREW=1
			unset IGNORE_HOMEBREW
			PROVISION_SHARPIE=1
			unset IGNORE_SHARPIE
			PROVISION_SIMULATORS=1
			unset IGNORE_SIMULATORS
			PROVISION_OLD_SIMULATORS=1
			unset IGNORE_OLD_SIMULATORS
			PROVISION_PYTHON3=1
			unset IGNORE_PYTHON3
			PROVISION_DOTNET=1
			unset IGNORE_DOTNET
			PROVISION_SHELLCHECK=1
			unset IGNORE_SHELLCHECK
			PROVISION_YAMLLINT=1
			unset IGNORE_YAMLLINT
			shift
			;;
		--ignore-all)
			IGNORE_OSX=1
			IGNORE_MONO=1
			IGNORE_VISUAL_STUDIO=1
			IGNORE_XCODE=1
			IGNORE_7Z=1
			IGNORE_HOMEBREW=1
			IGNORE_SHARPIE=1
			IGNORE_SIMULATORS=1
			IGNORE_PYTHON3=1
			IGNORE_DOTNET=1
			IGNORE_SHELLCHECK=1
			IGNORE_YAMLLINT=1
			shift
			;;
		--ignore-osx)
			IGNORE_OSX=1
			shift
			;;
		--ignore-xcode)
			IGNORE_XCODE=1
			shift
			;;
		--ignore-*-studio)
			IGNORE_VISUAL_STUDIO=1
			shift
			;;
		--ignore-mono)
			IGNORE_MONO=1
			shift
			;;
		--ignore-autotools)
			# this is an old argument, just ignore it
			shift
			;;
		--ignore-python3)
			IGNORE_PYTHON3=1
			shift
			;;
		--ignore-7z)
			IGNORE_7Z=1
			shift
			;;
		--ignore-sharpie)
			IGNORE_SHARPIE=1
			shift
			;;
		--enforce-sharpie)
			unset IGNORE_SHARPIE
			unset OPTIONAL_SHARPIE
			shift
			;;
		--ignore-simulators)
			IGNORE_SIMULATORS=1
			shift
			;;
		--ignore-old-simulators)
			IGNORE_OLD_SIMULATORS=1
			shift
			;;
		--enforce-simulators)
			unset IGNORE_SIMULATORS
			unset OPTIONAL_SIMULATORS
			shift
			;;
		--ignore-dotnet)
			IGNORE_DOTNET=1
			shift
			;;
		--ignore-shellcheck)
			IGNORE_SHELLCHECK=1
			shift
			;;
		--ignore-yamllint)
			IGNORE_YAMLLINT=1
			shift
			;;
		-v | --verbose)
			set -x
			shift
			VERBOSE=1
			;;
		*)
			echo "Unknown argument: $1"
			exit 1
			;;
	esac
done

# reporting functions
COLOR_RED=$(tput setaf 1 2>/dev/null || true)
COLOR_ORANGE=$(tput setaf 3 2>/dev/null || true)
COLOR_MAGENTA=$(tput setaf 5 2>/dev/null || true)
COLOR_BLUE=$(tput setaf 6 2>/dev/null || true)
COLOR_CLEAR=$(tput sgr0 2>/dev/null || true)
COLOR_RESET=uniquesearchablestring
FAILURE_PREFIX=
if test -z "$COLOR_RED"; then FAILURE_PREFIX="** failure ** "; fi

function fail () {
	echo "    $FAILURE_PREFIX${COLOR_RED}${1//${COLOR_RESET}/${COLOR_RED}}${COLOR_CLEAR}"
	FAIL=1
}

function warn () {
	echo "    ${COLOR_ORANGE}${1//${COLOR_RESET}/${COLOR_ORANGE}}${COLOR_CLEAR}"
}

function ok () {
	echo "    ${1//${COLOR_RESET}/${COLOR_CLEAR}}"
}

function log () {
	echo "        ${1//${COLOR_RESET}/${COLOR_CLEAR}}"
}

# $1: the version to check
# $2: the minimum version to check against
function is_at_least_version () {
	ACT_V=$1
	MIN_V=$2

	if [[ "$ACT_V" == "$MIN_V" ]]; then
		return 0
	fi

	IFS=. read -a V_ACT <<< "$ACT_V"
	IFS=. read -a V_MIN <<< "$MIN_V"
	
	# get the minimum # of elements
	AC=${#V_ACT[@]}
	MC=${#V_MIN[@]}
	COUNT=$(($AC>$MC?$MC:$AC))

	C=0
	while (( $C < $COUNT )); do
		ACT=${V_ACT[$C]}
		MIN=${V_MIN[$C]}
		if (( $ACT > $MIN )); then
			return 0
		elif (( "$MIN" > "$ACT" )); then
			return 1
		fi
		let C++
	done

	if (( $AC == $MC )); then
		# identical?
		return 0
	fi

	if (( $AC > $MC )); then
		# more version fields in actual than min: OK
		return 0
	elif (( $AC == $MC )); then
		# entire strings aren't equal (first check in function), but each individual field is?
		return 0
	else
		# more version fields in min than actual (1.0 vs 1.0.1 for instance): not OK
		return 1
	fi
}

function install_mono () {
	local MONO_URL=`grep MIN_MONO_URL= Make.config | sed 's/.*=//'`
	local MIN_MONO_VERSION=`grep MIN_MONO_VERSION= Make.config | sed 's/.*=//'`

	if test -z $MONO_URL; then
		fail "No MIN_MONO_URL set in Make.config, cannot provision"
		return
	fi

	mkdir -p $PROVISION_DOWNLOAD_DIR
	log "Downloading Mono $MIN_MONO_VERSION from $MONO_URL to $PROVISION_DOWNLOAD_DIR..."
	local MONO_NAME=`basename $MONO_URL`
	local MONO_PKG=$PROVISION_DOWNLOAD_DIR/$MONO_NAME
	curl -L $MONO_URL > $MONO_PKG

	log "Installing Mono $MIN_MONO_VERSION from $MONO_URL..."
	$SUDO installer -pkg $MONO_PKG -target /

	rm -f $MONO_PKG
}

function xcodebuild_download_selected_platforms ()
{
	log "Executing '$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -downloadPlatform iOS' $1"
	"$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -downloadPlatform iOS
	log "Executing '$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -downloadPlatform tvOS' $1"
	"$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -downloadPlatform tvOS
}

function download_xcode_platforms ()
{
	if test -n "$IGNORE_SIMULATORS"; then return; fi

	local XCODE_VERSION
	local XCODE_DEVELOPER_ROOT="$1"

	XCODE_VERSION=$(grep ^XCODE_VERSION= Make.config | sed 's/.*=//')

	if ! is_at_least_version "$XCODE_VERSION" 14.0; then
		# Nothing to do here
		log "This version of Xcode ($XCODE_VERSION) does not have any additional platforms to download"
		return
	fi

	if test -z "$PROVISION_SIMULATORS"; then
		warn "    Xcode may have additional platforms that must be downloaded. Execute './system-dependencies.sh --provision-simulators' to install those platforms (or alternatively ${COLOR_MAGENTA}export IGNORE_SIMULATORS=1${COLOR_RESET} to skip this check)"
		return
	fi

	log "Xcode has additional platforms that must be downloaded ($MUST_INSTALL_RUNTIMES), so installing those."

	log "Executing '$SUDO pkill -9 -f CoreSimulator.framework'"
	$SUDO pkill -9 -f "CoreSimulator.framework" || true
	if ! xcodebuild_download_selected_platforms; then
		log "Executing '$XCODE_DEVELOPER_ROOT/usr/bin/simctl runtime list -v"
		"$XCODE_DEVELOPER_ROOT/usr/bin/simctl" runtime list -v
		# Don't exit here, just hope for the best instead.
		set +x
		echo "##vso[task.logissue type=warning;sourcepath=system-dependencies.sh]Failed to download all simulator platforms, this may result in problems executing tests in the simulator."
		set -x
	else
		log "Executing '$XCODE_DEVELOPER_ROOT/usr/bin/simctl runtime list -v"
		"$XCODE_DEVELOPER_ROOT/usr/bin/simctl" runtime list -v
		log "Executing '$XCODE_DEVELOPER_ROOT/usr/bin/simctl list -v"
		"$XCODE_DEVELOPER_ROOT/usr/bin/simctl" list -v
	fi

	xcodebuild_download_selected_platforms "(second time)" || true
	xcodebuild_download_selected_platforms "(third time)" || true
	xcodebuild_download_selected_platforms "(fourth time)" || true

	log "Executing '$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -runFirstLaunch'"
	$SUDO "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -runFirstLaunch
	log "Executed '$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -runFirstLaunch'"

	# This is a workaround for a bug in Xcode 15 where we need to open the platforms panel for it to register the simulators.
	log "Executing 'open xcpref://Xcode.PreferencePane.Platforms'"
	log "Killing Xcode"
	pkill -9 "Xcode" || log "Xcode was not running."
	log "Opening Xcode preferences panel"
	open xcpref://Xcode.PreferencePane.Platforms
	log "waiting 10 secs for Xcode to open the preferences panel"
	sleep 10
	log "Killing Xcode"
	pkill -9 "Xcode" || log "Xcode was not running."
	log "Executed 'open xcpref://Xcode.PreferencePane.Platforms'"

	log "Finished installing Xcode platforms"
}

function run_xcode_first_launch ()
{
	local XCODE_VERSION="$1"
	local XCODE_DEVELOPER_ROOT="$2"

	# xcodebuild -runFirstLaunch seems to have been introduced in Xcode 9
	if ! is_at_least_version "$XCODE_VERSION" 9.0; then
		return
	fi

	if ! "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -checkFirstLaunchStatus; then
		if ! test -z "$PROVISION_XCODE"; then
			# Run the first launch tasks
			log "Executing '$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -runFirstLaunch'"
			$SUDO "$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -runFirstLaunch
			log "Executed '$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -runFirstLaunch'"
		else
			fail "Xcode has pending first launch tasks. Execute 'make fix-xcode-first-run' to execute those tasks."
			return
		fi
	fi
}

function install_specific_xcode () {
	local XCODE_URL=`grep XCODE$1_URL= Make.config | sed 's/.*=//'`
	local XCODE_VERSION=`grep XCODE$1_VERSION= Make.config | sed 's/.*=//'`
	local XCODE_DEVELOPER_ROOT="$2"
	local XCODE_ROOT="$(dirname "$(dirname "$XCODE_DEVELOPER_ROOT")")"

	if test -z $XCODE_URL; then
		fail "No XCODE$1_URL set in Make.config, cannot provision"
		return
	fi

	mkdir -p $PROVISION_DOWNLOAD_DIR
	log "Downloading Xcode $XCODE_VERSION from $XCODE_URL to $PROVISION_DOWNLOAD_DIR..."
	local XCODE_NAME=`basename $XCODE_URL`
	local XCODE_DMG=$PROVISION_DOWNLOAD_DIR/$XCODE_NAME

	# To test this script with new Xcode versions, copy the downloaded file to $XCODE_DMG,
	# uncomment the following curl line, and run ./system-dependencies.sh --provision-xcode
	if test -f "$HOME/Downloads/$XCODE_NAME"; then
		log "Found $XCODE_NAME in your ~/Downloads folder, copying that version to $XCODE_DMG instead of re-downloading it."
		cp "$HOME/Downloads/$XCODE_NAME" "$XCODE_DMG"
	else
		curl -L $XCODE_URL > $XCODE_DMG
	fi

	if [[ ${XCODE_DMG: -4} == ".dmg" ]]; then
		local XCODE_MOUNTPOINT=$PROVISION_DOWNLOAD_DIR/$XCODE_NAME-mount
		log "Mounting $XCODE_DMG into $XCODE_MOUNTPOINT..."
		hdiutil attach $XCODE_DMG -mountpoint $XCODE_MOUNTPOINT -quiet -nobrowse
		log "Removing previous Xcode from $XCODE_ROOT"
		rm -Rf $XCODE_ROOT
		log "Installing Xcode $XCODE_VERSION to $XCODE_ROOT..."
		cp -R $XCODE_MOUNTPOINT/*.app $XCODE_ROOT
		log "Unmounting $XCODE_DMG..."
		hdiutil detach $XCODE_MOUNTPOINT -quiet
	elif [[ ${XCODE_DMG: -4} == ".xip" ]]; then
		log "Extracting $XCODE_DMG..."
		pushd . > /dev/null
		cd $PROVISION_DOWNLOAD_DIR
		# make sure there's nothing interfering
		rm -Rf *.app
		rm -Rf $XCODE_ROOT
		# extract
		xip --expand "$XCODE_DMG"
		log "Installing Xcode $XCODE_VERSION to $XCODE_ROOT..."
		mv *.app $XCODE_ROOT
		popd > /dev/null
	else
		fail "Don't know how to install $XCODE_DMG"
	fi
	rm -f $XCODE_DMG

	log "Removing any com.apple.quarantine attributes from the installed Xcode"
	$SUDO xattr -s -d -r com.apple.quarantine $XCODE_ROOT

	if is_at_least_version $XCODE_VERSION 5.0; then
		log "Accepting Xcode license"
		$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -license accept
	fi

	if is_at_least_version "$XCODE_VERSION" 9.0; then
		run_xcode_first_launch "$XCODE_VERSION" "$XCODE_DEVELOPER_ROOT"
	elif is_at_least_version $XCODE_VERSION 8.0; then
		PKGS="MobileDevice.pkg MobileDeviceDevelopment.pkg XcodeSystemResources.pkg"
		for pkg in $PKGS; do
			if test -f "$XCODE_DEVELOPER_ROOT/../Resources/Packages/$pkg"; then
				log "Installing $pkg"
				$SUDO /usr/sbin/installer -dumplog -verbose -pkg "$XCODE_DEVELOPER_ROOT/../Resources/Packages/$pkg" -target /
				log "Installed $pkg"
			else
				log "Not installing $pkg because it doesn't exist."
			fi
		done
	fi

	log "Clearing xcrun cache..."
	xcrun -k

	ok "Xcode $XCODE_VERSION provisioned"
}

function install_coresimulator ()
{
	local XCODE_DEVELOPER_ROOT
	local CORESIMULATOR_PKG
	local CORESIMULATOR_PKG_DIR
	local XCODE_ROOT
	local TARGET_CORESIMULATOR_VERSION
	local CURRENT_CORESIMULATOR_VERSION

	XCODE_DEVELOPER_ROOT=$(grep XCODE_DEVELOPER_ROOT= Make.config | sed 's/.*=//')
	XCODE_ROOT=$(dirname "$(dirname "$XCODE_DEVELOPER_ROOT")")
	CORESIMULATOR_PKG=$XCODE_ROOT/Contents/Resources/Packages/XcodeSystemResources.pkg

	if ! test -f "$CORESIMULATOR_PKG"; then
		warn "Could not find XcodeSystemResources.pkg (which contains CoreSimulator.framework) in $XCODE_DEVELOPER_ROOT ($CORESIMULATOR_PKG doesn't exist)."
		return
	fi

	# Get the CoreSimulator.framework version from our Xcode
	# Extract the .pkg to get the pkg's PackageInfo file, which contains the CoreSimulator.framework version.
	CORESIMULATOR_PKG_DIR=$(mktemp -d)
	pkgutil --expand "$CORESIMULATOR_PKG" "$CORESIMULATOR_PKG_DIR/extracted"

	if ! TARGET_CORESIMULATOR_VERSION=$(xmllint --xpath 'string(/pkg-info/bundle-version/bundle[@id="com.apple.CoreSimulator"]/@CFBundleShortVersionString)' "$CORESIMULATOR_PKG_DIR/extracted/PackageInfo"); then
		rm -rf "$CORESIMULATOR_PKG_DIR"
		warn "Failed to look up the CoreSimulator version of $XCODE_DEVELOPER_ROOT"
		return
	fi
	rm -rf "$CORESIMULATOR_PKG_DIR"

	# Get the CoreSimulator.framework currently installed
	local CURRENT_CORESIMULATOR_PATH=/Library/Developer/PrivateFrameworks/CoreSimulator.framework/Versions/A/CoreSimulator
	local CURRENT_CORESIMULATOR_VERSION=0.0
	if test -f "$CURRENT_CORESIMULATOR_PATH"; then
		CURRENT_CORESIMULATOR_VERSION=$(otool -L $CURRENT_CORESIMULATOR_PATH | grep "$CURRENT_CORESIMULATOR_PATH.*current version" | sed -e 's/.*current version//' -e 's/)//' -e 's/[[:space:]]//g' | uniq)
	fi

	# Either version may be composed of either 1, 2 or 3 numbers.
	# We only care about the first two, so strip off the 3rd number if it exists.
	# shellcheck disable=SC2001
	CURRENT_CORESIMULATOR_VERSION=$(echo "$CURRENT_CORESIMULATOR_VERSION" | sed 's/\([0-9]*[.][0-9]*\).*/\1/')
	# shellcheck disable=SC2001
	TARGET_CORESIMULATOR_VERSION=$(echo "$TARGET_CORESIMULATOR_VERSION" | sed 's/\([0-9]*[.][0-9]*\).*/\1/')
	# Add a .0 if we only got one number
	if [[ "${CURRENT_CORESIMULATOR_VERSION/./}" == "${CURRENT_CORESIMULATOR_VERSION}" ]]; then
		CURRENT_CORESIMULATOR_VERSION=$CURRENT_CORESIMULATOR_VERSION.0
	fi
	if [[ "${TARGET_CORESIMULATOR_VERSION/./}" == "${TARGET_CORESIMULATOR_VERSION}" ]]; then
		TARGET_CORESIMULATOR_VERSION=$TARGET_CORESIMULATOR_VERSION.0
	fi

	# Compare versions to see if we got what we need
	if [[ x"$TARGET_CORESIMULATOR_VERSION" == x"$CURRENT_CORESIMULATOR_VERSION" ]]; then
		log "Found CoreSimulator.framework $CURRENT_CORESIMULATOR_VERSION (exactly $TARGET_CORESIMULATOR_VERSION is recommended)"
		return
	fi

	if test -z $PROVISION_XCODE; then
		# This is not a failure for now, until this logic has been tested thoroughly
		warn "You should have exactly CoreSimulator.framework version $TARGET_CORESIMULATOR_VERSION (found $CURRENT_CORESIMULATOR_VERSION). Execute './system-dependencies.sh --provision-xcode' to install the expected version."
		return
	fi

	# Just installing the package won't work, because there's a version check somewhere
	# that prevents the macOS installer from downgrading, so remove the existing
	# CoreSimulator.framework manually first.
	log "Installing CoreSimulator.framework $CURRENT_CORESIMULATOR_VERSION..."
	$SUDO rm -Rf /Library/Developer/PrivateFrameworks/CoreSimulator.framework
	$SUDO installer -pkg "$CORESIMULATOR_PKG" -target /

	CURRENT_CORESIMULATOR_VERSION=$(otool -L $CURRENT_CORESIMULATOR_PATH | grep "$CURRENT_CORESIMULATOR_PATH.*current version" | sed -e 's/.*current version//' -e 's/)//' -e 's/[[:space:]]//g')
	log "Installed CoreSimulator.framework $CURRENT_CORESIMULATOR_VERSION successfully."
}

function check_specific_xcode () {
	local XCODE_DEVELOPER_ROOT=`grep XCODE$1_DEVELOPER_ROOT= Make.config | sed 's/.*=//'`
	local XCODE_VERSION=`grep XCODE$1_VERSION= Make.config | sed 's/.*=//'`
	local XCODE_ROOT=$(dirname `dirname $XCODE_DEVELOPER_ROOT`)
	
	if ! test -d $XCODE_DEVELOPER_ROOT; then
		if ! test -z $PROVISION_XCODE; then
			install_specific_xcode "$1" "$XCODE_DEVELOPER_ROOT"
		else
			fail "You must install Xcode ($XCODE_VERSION) in $XCODE_ROOT. You can download Xcode $XCODE_VERSION here: https://developer.apple.com/downloads/index.action?name=Xcode"
		fi
		return
	else
		if is_at_least_version $XCODE_VERSION 5.0; then
			if ! $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -license check >/dev/null 2>&1; then
				if ! test -z $PROVISION_XCODE; then
					$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild -license accept
				else
					fail "The license for Xcode $XCODE_VERSION has not been accepted. Execute '$SUDO $XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild' to review the license and accept it."
					return
				fi
			fi
		fi

		run_xcode_first_launch "$XCODE_VERSION" "$XCODE_DEVELOPER_ROOT"
	fi

	local XCODE_ACTUAL_VERSION=`/usr/libexec/PlistBuddy -c 'Print :CFBundleShortVersionString' "$XCODE_DEVELOPER_ROOT/../version.plist"`
	# this is a hard match, having 4.5 when requesting 4.4 is not OK (but 4.4.1 is OK)
	if [[ ! "x$XCODE_ACTUAL_VERSION" =~ "x$XCODE_VERSION" ]]; then
		fail "You must install Xcode $XCODE_VERSION in $XCODE_ROOT (found $XCODE_ACTUAL_VERSION).  You can download Xcode $XCODE_VERSION here: https://developer.apple.com/downloads/index.action?name=Xcode";
		return
	fi

	ok "Found Xcode $XCODE_ACTUAL_VERSION in $XCODE_ROOT"
}

function check_xcode () {
	if ! test -z $IGNORE_XCODE; then return; fi

	# must have latest Xcode in /Applications/Xcode<version>.app
	check_specific_xcode
	install_coresimulator

	local IOS_SDK_VERSION MACOS_SDK_VERSION TVOS_SDK_VERSION
	local XCODE_DEVELOPER_ROOT=`grep ^XCODE_DEVELOPER_ROOT= Make.config | sed 's/.*=//'`
	IOS_SDK_VERSION=$(grep ^IOS_NUGET_OS_VERSION= Make.versions | sed -e 's/.*=//')
	MACOS_SDK_VERSION=$(grep ^MACOS_NUGET_OS_VERSION= Make.versions | sed -e 's/.*=//')
	TVOS_SDK_VERSION=$(grep ^TVOS_NUGET_OS_VERSION= Make.versions | sed -e 's/.*=//')

	download_xcode_platforms "$XCODE_DEVELOPER_ROOT" "$TVOS_SDK_VERSION"

	local D=$XCODE_DEVELOPER_ROOT/Platforms/iPhoneSimulator.platform/Developer/SDKs/iPhoneSimulator${IOS_SDK_VERSION}.sdk
	if test ! -d $D -a -z "$FAIL"; then
		fail "The directory $D does not exist. If you've updated the Xcode location it means you also need to update IOS_SDK_VERSION in Make.config."
	fi

	local D=$XCODE_DEVELOPER_ROOT/Platforms/MacOSX.platform/Developer/SDKs/MacOSX${MACOS_SDK_VERSION}.sdk
	if test ! -d $D -a -z "$FAIL"; then
		fail "The directory $D does not exist. If you've updated the Xcode location it means you also need to update MACOS_SDK_VERSION in Make.config."
	fi

	local D=$XCODE_DEVELOPER_ROOT/Platforms/AppleTVOS.platform/Developer/SDKs/AppleTVOS${TVOS_SDK_VERSION}.sdk
	if test ! -d $D -a -z "$FAIL"; then
		fail "The directory $D does not exist. If you've updated the Xcode location it means you also need to update TVOS_SDK_VERSION in Make.config."
	fi
}

function check_mono () {
	if ! test -z $IGNORE_MONO; then return; fi

	MONO_VERSION_FILE=/Library/Frameworks/Mono.framework/Versions/Current/VERSION
	if ! /Library/Frameworks/Mono.framework/Commands/mono --version 2>/dev/null >/dev/null; then
		if ! test -z $PROVISION_MONO; then
			install_mono
		else
			fail "You must install the Mono MDK. Download URL: $MIN_MONO_URL"
			return
		fi
	elif ! test -e $MONO_VERSION_FILE; then
		if ! test -z $PROVISION_MONO; then
			install_mono
		else
			fail "Could not find Mono's VERSION file, you must install the Mono MDK. Download URL: $MIN_MONO_URL"
			return
		fi
	fi

	MIN_MONO_VERSION=`grep MIN_MONO_VERSION= Make.config | sed 's/.*=//'`
	MAX_MONO_VERSION=`grep MAX_MONO_VERSION= Make.config | sed 's/.*=//'`

	ACTUAL_MONO_VERSION=`cat $MONO_VERSION_FILE`
	if ! is_at_least_version $ACTUAL_MONO_VERSION $MIN_MONO_VERSION; then
		if ! test -z $PROVISION_MONO; then
			install_mono
			ACTUAL_MONO_VERSION=`cat $MONO_VERSION_FILE`
		else
			MIN_MONO_URL=$(grep ^MIN_MONO_URL= Make.config | sed 's/.*=//')
			fail "You must have at least Mono $MIN_MONO_VERSION, found $ACTUAL_MONO_VERSION. Download URL: $MIN_MONO_URL"
			return
		fi
	elif [[ "$ACTUAL_MONO_VERSION" == "$MAX_MONO_VERSION" ]]; then
		: # this is ok
	elif is_at_least_version $ACTUAL_MONO_VERSION $MAX_MONO_VERSION; then
		if ! test -z $PROVISION_MONO; then
			install_mono
			ACTUAL_MONO_VERSION=`cat $MONO_VERSION_FILE`
		else
			fail "Your mono version is too new, max version is $MAX_MONO_VERSION, found $ACTUAL_MONO_VERSION."
			fail "You may edit Make.config and change MAX_MONO_VERSION to your actual version to continue the"
			fail "build (unless you're on a release branch). Once the build completes successfully, please"
			fail "commit the new MAX_MONO_VERSION value."
			fail "Alternatively you can ${COLOR_MAGENTA}export IGNORE_MONO=1${COLOR_RED} to skip this check."
			return
		fi
	fi

	if ! which mono > /dev/null 2>&1; then
		fail "Mono is not in PATH. You must add '/Library/Frameworks/Mono.framework/Versions/Current/Commands' to PATH. Current PATH is: $PATH".
		return
	fi

	ok "Found Mono $ACTUAL_MONO_VERSION (at least $MIN_MONO_VERSION and not more than $MAX_MONO_VERSION is required)"
}

function install_shellcheck () {
	if ! brew --version >& /dev/null; then
		fail "Asked to install shellcheck, but brew is not installed."
		return
	fi

	ok "Installing ${COLOR_BLUE}shellcheck${COLOR_RESET}..."
	brew install shellcheck
}

function install_yamllint () {
	if ! brew --version >& /dev/null; then
		fail "Asked to install yamllint, but brew is not installed."
		return
	fi

	ok "Installing ${COLOR_BLUE}yamllint${COLOR_RESET}..."
	brew install yamllint
}

function install_python3 () {
	if ! brew --version >& /dev/null; then
		fail "Asked to install python3, but brew is not installed."
		return
	fi

	ok "Installing ${COLOR_BLUE}python3${COLOR_RESET}..."
	brew install python3
}

function check_shellcheck () {
	if ! test -z $IGNORE_SHELLCHECK; then return; fi

IFStmp=$IFS
IFS='
'
	if SHELLCHECK_VERSION=($(shellcheck --version 2>/dev/null)); then
		ok "Found shellcheck ${SHELLCHECK_VERSION[1]} (no specific version is required)"
	elif ! test -z $PROVISION_SHELLCHECK; then
		install_shellcheck
	else
		fail "You must install shellcheck. The easiest way is to use homebrew, and execute ${COLOR_MAGENTA}brew install shellcheck${COLOR_RESET}."
	fi

IFS=$IFS_tmp
}

function check_yamllint () {
	if ! test -z $IGNORE_YAMLLINT; then return; fi

IFStmp=$IFS
IFS='
'
	if YAMLLINT_VERSION=($(yamllint --version 2>/dev/null)); then
		ok "Found ${YAMLLINT_VERSION[0]} (no specific version is required)"
	elif ! test -z $PROVISION_YAMLLINT; then
		install_yamllint
	else
		fail "You must install yamllint. The easiest way is to use homebrew, and execute ${COLOR_MAGENTA}brew install yamllint${COLOR_RESET}."
	fi

IFS=$IFS_tmp
}

function check_python3 () {
	if ! test -z $IGNORE_PYTHON3; then return; fi

IFStmp=$IFS
IFS='
'
	if PYTHON3_VERSION=$(python3 --version 2>/dev/null); then
		ok "Found $PYTHON3_VERSION (no specific version is required)"
	elif ! test -z $PROVISION_PYTHON3; then
		install_python3
	else
		fail "You must install python3. The easiest way is to use homebrew, and execute ${COLOR_MAGENTA}brew install python3${COLOR_RESET}."
	fi

IFS=$IFS_tmp
}

function check_osx_version () {
	if ! test -z $IGNORE_OSX; then return; fi

	MIN_OSX_BUILD_VERSION=`grep MIN_OSX_BUILD_VERSION= Make.config | sed 's/.*=//'`

	ACTUAL_OSX_VERSION=$(sw_vers -productVersion)
	if ! is_at_least_version $ACTUAL_OSX_VERSION $MIN_OSX_BUILD_VERSION; then
		fail "You must have at least OSX $MIN_OSX_BUILD_VERSION (found $ACTUAL_OSX_VERSION)"
		return
	fi

	ok "Found OSX $ACTUAL_OSX_VERSION (at least $MIN_OSX_BUILD_VERSION is required)"
}

function check_checkout_dir () {
	# use apple script to get the possibly translated special folders and check that we are not a subdir
	for special in documents downloads desktop; do
		path=$(osascript -e "set result to POSIX path of (path to $special folder as string)")
		if [[ $PWD == $path* ]]; then
			fail "Your checkout is under $path which is a special path. This can result in problems running the tests."
		fi
	done
	ok "Checkout location will not result in test problems."
}

function install_7z () {
	if ! brew --version >& /dev/null; then
		fail "Asked to install 7z, but brew is not installed."
		return
	fi

	brew install p7zip
}

function check_7z () {
	if ! test -z $IGNORE_7Z; then return; fi


	if ! 7z &> /dev/null; then
		if ! test -z $PROVISION_7Z; then
			install_7z
		else
			fail "You must install 7z (no specific version is required)"
		fi
		return
	fi

	ok "Found 7z (no specific version is required)"
}

function check_homebrew ()
{
	if ! test -z $IGNORE_HOMEBREW; then return; fi

IFStmp=$IFS
IFS='
'
	if HOMEBREW_VERSION=($(brew --version 2>/dev/null)); then
		ok "Found Homebrew ($HOMEBREW_VERSION)"
	elif ! test -z $PROVISION_HOMEBREW; then
		log "Installing Homebrew..."
		/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install.sh)"	
		HOMEBREW_VERSION=($(brew --version 2>/dev/null))
		log "Installed Homebrew ($HOMEBREW_VERSION)"
	else
		warn "Could not find Homebrew. Homebrew is required to auto-provision some dependencies (p7zip), but not required otherwise."
	fi
IFS=$IFS_tmp
}

function install_objective_sharpie () {
	local SHARPIE_URL=$(grep MIN_SHARPIE_URL= Make.config | sed 's/.*=//')
	local MIN_SHARPIE_VERSION=$(grep MIN_SHARPIE_VERSION= Make.config | sed 's/.*=//')

	if test -z "$SHARPIE_URL"; then
		fail "No MIN_SHARPIE_URL set in Make.config, cannot provision Objective Sharpie"
		return
	fi

	mkdir -p "$PROVISION_DOWNLOAD_DIR"
	log "Downloading Objective Sharpie $MIN_SHARPIE_VERSION from $SHARPIE_URL to $PROVISION_DOWNLOAD_DIR..."
	local SHARPIE_NAME=$(basename "$SHARPIE_URL")
	local SHARPIE_PKG=$PROVISION_DOWNLOAD_DIR/$SHARPIE_NAME
	curl -L "$SHARPIE_URL" > "$SHARPIE_PKG"

	log "Installing Objective-Sharpie $MIN_SHARPIE_VERSION from $SHARPIE_URL..."
	sudo installer -pkg "$SHARPIE_PKG" -target /

	rm -f "$SHARPIE_PKG"
}

function check_objective_sharpie () {
	if ! test -z $IGNORE_SHARPIE; then return; fi

	SHARPIE_URL=$(grep MIN_SHARPIE_URL= Make.config | sed 's/.*=//')
	MIN_SHARPIE_VERSION=$(grep MIN_SHARPIE_VERSION= Make.config | sed 's/.*=//')
	MAX_SHARPIE_VERSION=$(grep MAX_SHARPIE_VERSION= Make.config | sed 's/.*=//')

	if ! test -f /Library/Frameworks/ObjectiveSharpie.framework/Versions/Current/Version; then
		if ! test -z "$PROVISION_SHARPIE"; then
			install_objective_sharpie
			ACTUAL_SHARPIE_VERSION=$(cat /Library/Frameworks/ObjectiveSharpie.framework/Versions/Current/Version)
		else
			if test -z $OPTIONAL_SHARPIE; then
				fail "You must install Objective Sharpie, at least $MIN_SHARPIE_VERSION (no Objective Sharpie found). You can download it from $SHARPIE_URL"
				fail "Alternatively you can ${COLOR_MAGENTA}export IGNORE_SHARPIE=1${COLOR_RED} to skip this check."
			else
				warn "You do not have Objective Sharpie installed (should be at least $MIN_SHARPIE_VERSION). You can download it from $SHARPIE_URL"
			fi
			return
		fi
	else
		ACTUAL_SHARPIE_VERSION=$(cat /Library/Frameworks/ObjectiveSharpie.framework/Versions/Current/Version)
		if ! is_at_least_version "$ACTUAL_SHARPIE_VERSION" "$MIN_SHARPIE_VERSION"; then
			if ! test -z "$PROVISION_SHARPIE"; then
				install_objective_sharpie
				ACTUAL_SHARPIE_VERSION=$(cat /Library/Frameworks/ObjectiveSharpie.framework/Versions/Current/Version)
			else
				if test -z $OPTIONAL_SHARPIE; then
					fail "You must have at least Objective Sharpie $MIN_SHARPIE_VERSION, found $ACTUAL_SHARPIE_VERSION. You can download it from $SHARPIE_URL"
					fail "Alternatively you can ${COLOR_MAGENTA}export IGNORE_SHARPIE=1${COLOR_RED} to skip this check."
				else
					warn "You do not have have at least Objective Sharpie $MIN_SHARPIE_VERSION (found $ACTUAL_SHARPIE_VERSION). You can download it from $SHARPIE_URL"
				fi
				return
			fi
		elif [[ "$ACTUAL_SHARPIE_VERSION" == "$MAX_SHARPIE_VERSION" ]]; then
			: # this is ok
		elif is_at_least_version "$ACTUAL_SHARPIE_VERSION" "$MAX_SHARPIE_VERSION"; then
			if ! test -z "$PROVISION_SHARPIE"; then
				install_objective_sharpie
				ACTUAL_SHARPIE_VERSION=$(cat /Library/Frameworks/ObjectiveSharpie.framework/Versions/Current/Version)
			else
				if test -z $OPTIONAL_SHARPIE; then
					fail "Your Objective Sharpie version is too new, max version is $MAX_SHARPIE_VERSION, found $ACTUAL_SHARPIE_VERSION. We recommend you download $SHARPIE_URL"
					fail "Alternatively you can ${COLOR_MAGENTA}export IGNORE_SHARPIE=1${COLOR_RED} to skip this check."
				else
					warn "You do not have have at most Objective Sharpie $MAX_SHARPIE_VERSION (found $ACTUAL_SHARPIE_VERSION). We recommend you download $SHARPIE_URL"
				fi
				return
			fi
		fi
	fi

	if test -z $OPTIONAL_SHARPIE; then
		ok "Found Objective Sharpie $ACTUAL_SHARPIE_VERSION (at least $MIN_SHARPIE_VERSION and not more than $MAX_SHARPIE_VERSION is required)"
	else
		ok "Found Objective Sharpie $ACTUAL_SHARPIE_VERSION (at least $MIN_SHARPIE_VERSION and not more than $MAX_SHARPIE_VERSION is recommended)"
	fi
}

function check_old_simulators ()
{
	if test -n "$IGNORE_OLD_SIMULATORS"; then return; fi

	local EXTRA_SIMULATORS
	local XCODE
	local XCODE_DEVELOPER_ROOT

	XCODE_DEVELOPER_ROOT=$(grep XCODE$1_DEVELOPER_ROOT= Make.config | sed 's/.*=//')

	IFS=' ' read -r -a EXTRA_SIMULATORS <<< "$(grep ^EXTRA_SIMULATORS= Make.config | sed 's/.*=//')"
	XCODE=$(dirname "$(dirname "$XCODE_DEVELOPER_ROOT")")

	if ! test -d "$XCODE"; then
		# can't test unless Xcode is present
		warn "Can't check if simulators are available unless Xcode is already installed."
		return
	fi

	SD_TMP_DIR=$(mktemp -d /tmp/system-dependencies.XXXXXX)
	trap 'rm -rf -- "$SD_TMP_DIR"' EXIT
	TMP_FILE=$SD_TMP_DIR/simulator-runtimes.json
	xcrun simctl list runtimes --json --json-output "$TMP_FILE"

	local action=warn
	if test -z $OPTIONAL_OLD_SIMULATORS; then
		action=fail
	fi

	local os
	local versionName
	local version

	for spec in "${EXTRA_SIMULATORS[@]}"; do
		os=${spec/:*/}
		versionName=${spec/*:/}
		version=$(grep "^${versionName}=" Make.config | sed 's/.*=//')

		OS_TMP_FILE=$SD_TMP_DIR/$os-$version.json
		jq ".runtimes[] | select(.platform == \"$os\" and .version == \"$version\" and .isAvailable == true and .isInternal == false) | [ { \"version\":.version, \"name\":.name, \"identifier\":.identifier } ]" < "$TMP_FILE" > "$OS_TMP_FILE"
		#echo $OS_TMP_FILE
		LENGTH=$(jq length < "$OS_TMP_FILE")
		if [[ "$LENGTH" != "" && "$LENGTH" -gt 0 ]]; then
			ok "Found the $os $version simulator."
		elif test -z "$PROVISION_OLD_SIMULATORS"; then
			$action "The $os $version simulator is not installed. Execute ${COLOR_MAGENTA}xcodebuild -downloadPlatform $os -buildVersion $version${COLOR_RESET} to install."
		else
			warn "The $os $version simulator is not installed. Now executing ${COLOR_BLUE}"$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -downloadPlatform $os -buildVersion $version${COLOR_RESET} to install..."
			"$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -downloadPlatform "$os" -buildVersion "$version"
			warn "Successfully executed ${COLOR_BLUE}"$XCODE_DEVELOPER_ROOT/usr/bin/xcodebuild" -downloadPlatform $os -buildVersion $version${COLOR_RESET}."
		fi
	done
}

echo "Checking system..."

check_osx_version
check_checkout_dir
check_xcode
check_homebrew
check_shellcheck
check_yamllint
check_python3
check_mono
check_7z
check_objective_sharpie
check_old_simulators
if test -z "$IGNORE_DOTNET"; then
	if test -f /usr/local/share/dotnet/dotnet; then
		ok "Installed .NET SDKs:"
		(IFS=$'\n'; for i in $(/usr/local/share/dotnet/dotnet --list-sdks); do log "$i"; done)
	else
		warn ".NET is not installed"
	fi
fi

if test -z $FAIL; then
	echo "System check succeeded"
else
	echo "System check failed"
	exit 1
fi

