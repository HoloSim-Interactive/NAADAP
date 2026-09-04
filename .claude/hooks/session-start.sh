#!/bin/bash
# SessionStart hook for Claude Code on the web.
#
# Makes `dotnet build` / `dotnet test` work inside a fresh cloud sandbox:
# installs the .NET 9 SDK if it is missing, puts it on the session's PATH,
# and restores NuGet packages so the first prompt doesn't stall on a restore.
#
# Idempotent and non-interactive. Safe to run alongside an environment-level
# setup script that installs the same SDK: dotnet-install.sh is a no-op when
# the requested channel is already present in the install dir, and the PATH
# export is harmless if the entry is already there.
#
# Runs only in remote sessions (Claude Code on the web); a local checkout is
# expected to have its own SDK per README.md "Prerequisites".
set -euo pipefail

if [ "${CLAUDE_CODE_REMOTE:-}" != "true" ]; then
  exit 0
fi

DOTNET_CHANNEL="9.0"                       # matches <TargetFramework>net9.0 and Dockerfile sdk:9.0
DOTNET_ROOT="${DOTNET_ROOT:-$HOME/.dotnet}" # dotnet-install.sh's default install dir
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1

# --- 1. Install the SDK if it is not already present -----------------------
if ! "$DOTNET_ROOT/dotnet" --list-sdks 2>/dev/null | grep -q "^${DOTNET_CHANNEL}\."; then
  echo "session-start: installing .NET SDK ${DOTNET_CHANNEL} into ${DOTNET_ROOT}"
  installer="$(mktemp)"
  curl -sSL https://dot.net/v1/dotnet-install.sh -o "$installer"
  bash "$installer" --channel "$DOTNET_CHANNEL" --install-dir "$DOTNET_ROOT" --no-path
  rm -f "$installer"
else
  echo "session-start: .NET SDK ${DOTNET_CHANNEL} already present in ${DOTNET_ROOT}"
fi

# --- 2. Expose it to the session shell -------------------------------------
export PATH="$DOTNET_ROOT:$DOTNET_ROOT/tools:$PATH"
if [ -n "${CLAUDE_ENV_FILE:-}" ]; then
  {
    echo "export DOTNET_ROOT=\"$DOTNET_ROOT\""
    echo "export PATH=\"$DOTNET_ROOT:$DOTNET_ROOT/tools:\$PATH\""
    echo "export DOTNET_CLI_TELEMETRY_OPTOUT=1"
    echo "export DOTNET_NOLOGO=1"
  } >> "$CLAUDE_ENV_FILE"
fi

# --- 3. Restore packages so build/test are ready immediately ---------------
cd "${CLAUDE_PROJECT_DIR:-$(cd "$(dirname "$0")/../.." && pwd)}"
dotnet restore Naadap.sln --nologo --verbosity quiet
echo "session-start: $(dotnet --version) ready, packages restored"
