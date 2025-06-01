import os
import re
import glob

def main():
    # 1. Get all text files from the directory
    files = sorted(glob.glob("../downloaded_pages/page_*.txt"), 
                  key=lambda x: int(re.search(r'page_(\d+)\.txt', x).group(1)))
    
    # 2. Read and concatenate all files
    full_text = ""
    for file_path in files:
        try:
            with open(file_path, 'r', encoding='utf-8') as f:
                full_text += f.read() + "\n"
        except Exception as e:
            print(f"Error reading {file_path}: {e}")
    
    # 3. Split the text by part markers
    parts = re.split(r'(第[一二三四五六七八九十]+部.*?\n)', full_text)
    
    # 4. Process and save each part
    current_part = ""
    current_part_title = "前言"  # Default title for content before first part
    
    for i, section in enumerate(parts):
        if re.match(r'第[一二三四五六七八九十]+部', section):
            # This is a part title
            if current_part:
                # Save the previous part
                save_part(current_part_title, current_part)
            current_part_title = section.strip()
            current_part = ""
        else:
            # This is content
            current_part += section
    
    # Save the last part
    if current_part:
        save_part(current_part_title, current_part)

def save_part(title, content):
    # Create a clean filename from the title
    filename = title.replace(" ", "_").replace("：", "_").replace(":", "_")
    filename = re.sub(r'[^\w\s]', '', filename)
    
    # Save the content to a file
    with open(f"{filename}.txt", 'w', encoding='utf-8') as f:
        f.write(title + "\n\n")
        f.write(content)
    
    print(f"Saved: {filename}.txt")

if __name__ == "__main__":
    main()
