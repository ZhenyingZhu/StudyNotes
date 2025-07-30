# Web Content Downloader

This Python script downloads text content from a specified URL and related pages, preserving line breaks and saving the content to text files.

## Features

- Downloads text content from a specified URL
- Finds and downloads content from related links containing "人间仙境" in their title
- Preserves line breaks in the downloaded text
- Saves content to files named after the page titles
- Uses UTF-8 encoding to properly handle Chinese characters

## Requirements

- Python 3.6 or higher
- Required packages: requests, beautifulsoup4

## Installation

1. Clone or download this repository
2. Install the required packages:

```
pip install -r requirements.txt
```

## Usage

Run the script with Python:

```
python downloader.py
```

The script will:
1. Download content from the initial URL
2. Save the content to a file named after the page title
3. Find links on the page that contain "keyword" in their title
4. Download content from those links and save them to separate files

All files will be saved in a `downloaded_content` directory with UTF-8 encoding.

## Notes

- The script includes a 2-second delay between requests to avoid overloading the server
- If the script fails to extract content using the default selectors, it will attempt to extract content from the entire body
- Error handling is included to ensure the script continues running even if some pages fail to download
