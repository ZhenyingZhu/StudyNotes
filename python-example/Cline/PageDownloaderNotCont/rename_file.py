import os
import re

def rename_files():
    """
    Rename files in the downloaded_content folder by:
    1. Adding braces to number and 上/中/下 in the file name if not added
    2. Removing anything after the braces
    
    Returns:
        tuple: (total_files_processed, renamed_files)
    """
    # Path to the downloaded_content folder
    folder_path = 'downloaded_content'
    
    # Check if the folder exists
    if not os.path.exists(folder_path):
        print(f"Error: Folder '{folder_path}' does not exist.")
        return 0, 0
    
    # Count variables for summary
    total_files = 0
    renamed_files = 0
    
    # Process all files in the folder
    for filename in os.listdir(folder_path):
        file_path = os.path.join(folder_path, filename)
        
        # Skip directories
        if os.path.isdir(file_path):
            continue
        
        total_files += 1
        
        # Get the base name without extension
        base_name, ext = os.path.splitext(filename)
        
        # Check if the filename already has properly formatted braces
        if re.match(r'.*\(\d+(?:[-\s]?\d+)?(?:\s?[上中下])?\).*', base_name):
            # Extract the content inside the braces
            match = re.search(r'(.*?)(\(\d+(?:[-\s]?\d+)?(?:\s?[上中下])?\))(.*)', base_name)
            if match:
                prefix = match.group(1).strip()
                braced_part = match.group(2)
                # Create new filename with only content up to and including the braces
                new_filename = f"{prefix} {braced_part}{ext}"
                if new_filename != filename:
                    new_file_path = os.path.join(folder_path, new_filename)
                    try:
                        os.rename(file_path, new_file_path)
                        renamed_files += 1
                        print(f"Renamed: {filename} -> {new_filename}")
                    except Exception as e:
                        print(f"Error renaming {filename}: {e}")
        else:
            # Try to find number and 上/中/下 pattern
            match1 = re.search(r'(.*?)\s+(\d+(?:[-\s]?\d+)?)\s+([上中下])(.*)', base_name)
            match2 = re.search(r'(.*?)\s+\((\d+)\s*-\s*(\d+)\)(.*)', base_name)
            match3 = re.search(r'(.*?)\s+(\d+(?:[-\s]?\d+)?)(.*)', base_name)
            
            if match1:
                prefix = match1.group(1).strip()
                number = match1.group(2).replace(' ', '')
                position = match1.group(3)
                new_filename = f"{prefix} ({number}{position}){ext}"
                new_file_path = os.path.join(folder_path, new_filename)
                try:
                    os.rename(file_path, new_file_path)
                    renamed_files += 1
                    print(f"Renamed: {filename} -> {new_filename}")
                except Exception as e:
                    print(f"Error renaming {filename}: {e}")
            elif match2:
                prefix = match2.group(1).strip()
                number = match2.group(2)
                sub_number = match2.group(3)
                new_filename = f"{prefix} ({number}-{sub_number}){ext}"
                new_file_path = os.path.join(folder_path, new_filename)
                try:
                    os.rename(file_path, new_file_path)
                    renamed_files += 1
                    print(f"Renamed: {filename} -> {new_filename}")
                except Exception as e:
                    print(f"Error renaming {filename}: {e}")
            elif match3:
                prefix = match3.group(1).strip()
                number = match3.group(2).replace(' ', '')
                new_filename = f"{prefix} ({number}){ext}"
                new_file_path = os.path.join(folder_path, new_filename)
                try:
                    os.rename(file_path, new_file_path)
                    renamed_files += 1
                    print(f"Renamed: {filename} -> {new_filename}")
                except Exception as e:
                    print(f"Error renaming {filename}: {e}")
    
    return total_files, renamed_files

def main():
    print("Renaming files in downloaded_content folder...")
    print("1. Adding braces to number and 上/中/下 if not added")
    print("2. Removing anything after the braces")
    
    # Rename files
    total_files, renamed_files = rename_files()
    
    # Print summary of renaming
    print(f"\nSummary:")
    print(f"Total files processed: {total_files}")
    print(f"Files renamed: {renamed_files}")

if __name__ == "__main__":
    main()
