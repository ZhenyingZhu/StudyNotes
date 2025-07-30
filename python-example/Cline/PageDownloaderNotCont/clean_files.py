import os
import sys
import re
from rename_file import rename_files

def clean_file(file_path):
    """
    Process a file with the following steps:
    1. Remove all text before the first occurrence of '【'
    2. Remove all text after the occurrence of 'footer'
    3. Remove all line breaks
    4. Replace 'linebreak' with two line breaks
    
    Args:
        file_path (str): Path to the file to clean
        
    Returns:
        bool: True if file was modified, False otherwise
    """
    try:
        # Read the file content
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        # Find the first occurrence of '【'
        index = content.find('【')
        
        # Find the occurrence of 'footer'
        adult_site_index = content.find('footer')
        
        modified = False
        
        # If '【' is found, remove all text before it
        if index != -1:
            content = content[index:]
            modified = True
        
        # If 'footer' is found, remove all text after it
        if adult_site_index != -1:
            content = content[:adult_site_index]
            modified = True
        
        if modified:
            # Remove all line breaks
            content = content.replace('\n', ' ').replace('\r', ' ')
            
            # Replace 'linebreak' with two line breaks
            content = content.replace('linebreak', '\n\n')
            
            # Write the modified content back to the file
            with open(file_path, 'w', encoding='utf-8') as f:
                f.write(content)
            
            return True
        else:
            # No changes made, file remains unchanged
            return False
    
    except Exception as e:
        print(f"Error processing {file_path}: {e}")
        return False


def main():
    # Path to the downloaded_content folder
    folder_path = 'downloaded_content'
    
    # Check if the folder exists
    if not os.path.exists(folder_path):
        print(f"Error: Folder '{folder_path}' does not exist.")
        sys.exit(1)
    
    # Ask user what operation to perform
    print("Select operation:")
    print("1. Clean file contents")
    print("2. Rename files (add braces to numbers and remove text after braces)")
    print("3. Perform both operations")
    
    choice = input("Enter your choice (1/2/3): ").strip()
    
    if choice == '1' or choice == '3':
        # Count variables for summary of cleaning
        total_files = 0
        modified_files = 0
        
        # Process all files in the folder for cleaning
        for filename in os.listdir(folder_path):
            file_path = os.path.join(folder_path, filename)
            
            # Skip directories
            if os.path.isdir(file_path):
                continue
            
            total_files += 1
            
            # Clean the file and update count if modified
            if clean_file(file_path):
                modified_files += 1
                print(f"Cleaned: {filename}")
        
        # Print summary of cleaning
        print(f"\nCleaning Summary:")
        print(f"Total files processed: {total_files}")
        print(f"Files modified: {modified_files}")
    
    if choice == '2' or choice == '3':
        # Rename files using the new functionality
        total_files, renamed_files = rename_files()
        
        # Print summary of renaming
        print(f"\nRenaming Summary:")
        print(f"Total files processed: {total_files}")
        print(f"Files renamed: {renamed_files}")
    
    if choice not in ['1', '2', '3']:
        print("Invalid choice. Exiting.")

if __name__ == "__main__":
    main()
