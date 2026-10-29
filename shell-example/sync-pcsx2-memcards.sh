#!/usr/bin/env bash
set -euo pipefail

pause=false
work=
if [[ ${1:-} == --pause ]]; then
    pause=true
    shift
fi

finish() {
    local status=$?
    trap - EXIT
    if [[ -n $work ]]; then
        rm -r -- "$work"
    fi
    if (( status != 0 )); then
        printf 'Sync failed (exit code %s). See the error above.\n' "$status" >&2
    fi
    if [[ $pause == true && -t 0 ]]; then
        read -r -p 'Press Enter to close...' || true
    fi
    exit "$status"
}
trap finish EXIT

if (( $# == 0 )); then
    first=/home/zhu91/HotmailOneDrive/Documents/PCSX2/memcards
    second=/home/zhu91/.config/PCSX2/memcards
elif (( $# == 2 )); then
    first=$1
    second=$2
else
    printf 'Usage: %s [--pause] [DIRECTORY_A DIRECTORY_B]\n' "$0" >&2
    exit 2
fi

if ! command -v rsync >/dev/null 2>&1; then
    printf 'rsync is required. Install it with: sudo apt install rsync\n' >&2
    exit 1
fi

for directory in "$first" "$second"; do
    if [[ ! -d $directory ]]; then
        printf 'Directory does not exist: %s\n' "$directory" >&2
        exit 1
    fi
done

work=$(mktemp -d)

sync_one_way() {
    local source=$1 destination=$2 file
    printf 'Syncing %s -> %s\n' "$source" "$destination"
    (cd -- "$source" && find . -type f -print0) > "$work/files"
    : > "$work/newer"
    while IFS= read -r -d '' file; do
        if [[ ! -e $destination/$file || $source/$file -nt $destination/$file ]]; then
            printf '%s\0' "$file" >> "$work/newer"
        fi
    done < "$work/files"
    # Bash compares subsecond timestamps; rsync --update can miss these.
    rsync --times --relative --from0 --files-from="$work/newer" \
        --modify-window=-1 --itemize-changes -- "$source/" "$destination/"
}

sync_one_way "$first" "$second"
sync_one_way "$second" "$first"
printf 'Memory card sync completed successfully.\n'
