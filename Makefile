# =============================================================================
# Makefile — RazorEnhanced cross-platform build
#
# Targets:
#   all    — build the solution in $(CONFIG) mode (default: Debug)
#   build  — same as all
#   run    — build then launch the binary
#   clean  — remove build output for the current config
#
# Overrides (pass on command line):
#   CONFIG=Release   make build
# =============================================================================

# Detect OS and pull in platform-specific variables.
# On Windows, the environment variable OS is set to "Windows_NT".
ifeq ($(OS),Windows_NT)
    include Makefile.windows
else
    include Makefile.linux
endif

# ---------------------------------------------------------------------------
# Common variables (defined after include so OS files can be overridden via
# recursive = assignment, which defers expansion to use time)
# ---------------------------------------------------------------------------
CONFIG   ?= Debug
EXE_NAME := RazorEnhanced.exe
EXE_PATH  = $(OUT_DIR)/$(EXE_NAME)

# msbuild flags shared by build and clean targets
MSBUILD_FLAGS = -p:Configuration=$(CONFIG) "-p:Platform=$(BUILD_PLATFORM)"

# ---------------------------------------------------------------------------
# Targets
# ---------------------------------------------------------------------------
.PHONY: all build run clean

all: build

build:
	$(PREP_CMD)
	$(MSBUILD) $(SLN) $(MSBUILD_FLAGS)

run: build
	$(RUN_CMD) $(EXE_PATH)

clean:
	$(MSBUILD) $(SLN) /t:Clean $(MSBUILD_FLAGS)
