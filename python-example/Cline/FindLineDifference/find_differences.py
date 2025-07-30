import re
from pathlib import Path

def normalize_path(path):
    # Remove variable segments from paths (like hashes)
    parts = Path(path).parts
    normalized_parts = []
    for part in parts:
        # If part looks like a hash (alphanumeric and certain length), replace with *
        if re.match(r'^[a-f0-9]{6,}$', part):
            normalized_parts.append('*')
        else:
            normalized_parts.append(part)
    return str(Path(*normalized_parts))

def normalize_package(name):
    # Handle package version normalization
    # Example: package.1.0.1.nupkg -> package.*.nupkg
    return re.sub(r'\.\d+\.\d+\.\d+\.nupkg$', '.*.nupkg', name)

def is_random_filename(name):
    # Check if filename appears to be a random/temporary file
    # This is a simple heuristic and might need adjustment
    random_patterns = [
        r'^[a-z0-9]{8}\.[a-z]+$',  # like iaaq2kpl.cmdline
        r'^[a-z0-9]{32}$',          # long hex strings
        r'^tmp[0-9a-f]{6,}$'        # temporary files
    ]
    
    name = Path(name).name
    return any(re.match(pattern, name.lower()) for pattern in random_patterns)

def normalize_line(line):
    line = line.strip()
    if not line:
        return None
        
    # First check if it's a random filename
    if is_random_filename(line):
        return None
        
    # Check if it's a package
    if line.endswith('.nupkg'):
        return normalize_package(line)
    
    # Otherwise normalize the path
    return normalize_path(line)

def process_file(filename):
    normalized_paths = set()
    with open(filename, 'r', encoding='utf-8') as f:
        for line in f:
            normalized = normalize_line(line)
            if normalized:
                normalized_paths.add(normalized)
    return normalized_paths

def get_leaf_folder(path):
    """Extract the meaningful leaf folder from a path."""
    parts = Path(path).parts
    # Skip the filename by using parts[:-1]
    # Reverse the parts to find the first meaningful directory
    for part in reversed(parts[:-1]):
        # Skip hash-like directories
        if not re.match(r'^[a-f0-9]{6,}$', part):
            return part
    return "root"  # fallback if no meaningful directory found

def main():
    try:
        old_files = process_file('1 DropFilesList.txt')
        new_files = process_file('2 DropFilesList.txt')

        # Find additions (files in 2 but not in 1)
        additions = new_files - old_files
        
        # Group files by leaf folders
        folder_groups = {}
        for file in sorted(additions):
            leaf = get_leaf_folder(file)
            folder_groups.setdefault(leaf, []).append(file)
        
        # Write summary to result.txt
        with open('result.txt', 'w', encoding='utf-8') as f:
            f.write("New Files Summary by Folder\n")
            f.write("=" * 30 + "\n\n")
            for folder, files in sorted(folder_groups.items()):
                f.write(f"{folder}: {len(files)} files\n")
        
        # Write detailed log to result.log
        with open('result.log', 'w', encoding='utf-8') as f:
            f.write("Detailed List of New Files\n")
            f.write("=" * 30 + "\n")
            for folder, files in sorted(folder_groups.items()):
                f.write(f"\n[{folder}] ({len(files)} files)\n")
                f.write("-" * 50 + "\n")
                for file in sorted(files):
                    f.write(f"{file}\n")
        
        print("Analysis complete! Results have been written to:")
        print("- result.txt (folder summary)")
        print("- result.log (detailed file list)")
            
    except Exception as e:
        print(f"Error: {str(e)}")

if __name__ == "__main__":
    main()
