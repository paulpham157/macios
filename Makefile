TOP=.
SUBDIRS=builds runtime src msbuild tools
include $(TOP)/Make.config
include $(TOP)/mk/versions.mk

SUBDIRS += dotnet

#
# Common
#

all-local:: check-system
install-local::

.PHONY: world
world: check-system
	@$(MAKE) reset-versions
	@$(MAKE) all -j8
	@$(MAKE) install -j8

.PHONY: check-system
check-system:
	@./system-dependencies.sh
	$(Q) $(MAKE) show-versions

show-versions:
	@echo "Building:"
	@echo "    The .NET NuGet(s):"
	@$(foreach platform,$(DOTNET_PLATFORMS),echo "        Microsoft.$(platform) $($(shell echo $(platform) | tr 'a-z' 'A-Z')_NUGET_VERSION_FULL)";)

all-local:: global.json

# This tells NuGet to use the exact same dotnet version we've configured in Make.config
global.json: $(TOP)/dotnet.config Makefile $(GIT_DIRECTORY)/HEAD $(GIT_DIRECTORY)/index
	$(Q_GEN) \
		printf "{\n" > $@; \
		printf "  \"sdk\": {\n    \"version\": \"$(DOTNET_VERSION)\"\n  },\n" >> $@; \
		printf "  \"tools\": {\n    \"dotnet\": \"$(DOTNET_VERSION)\"\n  },\n" >> $@; \
		printf "  \"msbuild-sdks\": {\n    \"Microsoft.DotNet.Arcade.Sdk\": \"$(ARCADE_VERSION)\"\n  }\n" >> $@; \
		printf "}\n" >> $@

install-hook::
	$(Q) if ! git diff --exit-code global.json; then \
		echo "Error: global.json has changed: please commit the changes."; \
		exit 1; \
	fi

all-hook install-hook::
	$(Q) $(MAKE) -C dotnet shutdown-build-server

.PHONY: package release
package release:
	$(Q) $(MAKE) -C $(TOP)/release release
	# copy .pkg, .zip and *updateinfo to the packages directory to be uploaded to storage
	$(Q) mkdir -p ../package
	$(Q) echo "Output from 'make release':"
	$(Q) ls -la $(TOP)/release | sed 's/^/    /'
	$(Q) if test -n "$$(shopt -s nullglob; echo $(TOP)/release/*.pkg)"; then $(CP) $(TOP)/release/*.pkg ../package; fi
	$(Q) if test -n "$$(shopt -s nullglob; echo $(TOP)/release/*.zip)"; then $(CP) $(TOP)/release/*.zip ../package; fi
	$(Q) if test -n "$$(shopt -s nullglob; echo $(TOP)/release/*updateinfo)"; then $(CP) $(TOP)/release/*updateinfo ../package; fi
	$(Q) echo "Packages:"
	$(Q) ls -la ../package | sed 's/^/    /'

dotnet-install-system:
	$(Q) $(MAKE) -C dotnet install-system

fix-xcode-select:
	sudo xcode-select -s $(XCODE_DEVELOPER_ROOT)

fix-xcode-first-run:
	$(XCODE_DEVELOPER_ROOT)/usr/bin/xcodebuild -runFirstLaunch

install-dotnet:
	@echo "Figuring out package link..."
	@export PKG=$$(make -C builds print-dotnet-pkg-urls); \
	echo "Downloading $$(basename $$PKG)..."; \
	curl -LO "$$PKG"; \
	echo "Installing $$(basename $$PKG)..."; \
	time sudo installer -pkg "$$(basename $$PKG)" -target / -verbose -dumplog

git-clean-all:
	@echo "$(COLOR_RED)Cleaning and resetting all dependencies. This is a destructive operation.$(COLOR_CLEAR)"
	@echo "$(COLOR_RED)You have 5 seconds to cancel (Ctrl-C) if you wish.$(COLOR_CLEAR)"
	@sleep 5
	@echo "Cleaning macios..."
	@git clean -xffdq
	@echo "Cleaning submodules..."
	@git submodule foreach -q --recursive 'git clean -xffdq && git reset --hard -q'
	@set -e; \
	for dir in $(DEPENDENCY_DIRECTORIES); do \
		if test -d $$dir; then \
			echo "Cleaning $$(basename $$dir)..."; \
			cd $$dir; \
			git clean -xffdq; \
			git reset --hard -q; \
			git submodule foreach -q --recursive 'git clean -xffdq'; \
		else \
			echo "Skipped $$dir (does not exist)"; \
		fi; \
	done

	@set -e;  \
	if [ -n "$(ENABLE_XAMARIN)" ]; then \
		CONFIGURE_FLAGS=""; \
		if [ -n "$(ENABLE_XAMARIN)" ]; then \
			echo "Xamarin-specific build has been re-enabled"; \
			CONFIGURE_FLAGS="$$CONFIGURE_FLAGS --enable-xamarin"; \
		fi; \
		./configure $$CONFIGURE_FLAGS; \
		$(MAKE) reset; \
		echo "Done"; \
	else \
		echo "Done"; \
	fi; \

SUBDIRS += tests

