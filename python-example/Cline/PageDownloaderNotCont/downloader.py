import requests
from bs4 import BeautifulSoup
import os
import re
import time

def download_page(url):
    """
    Download content from a URL and return the HTML
    """
    headers = {
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36'
    }
    try:
        response = requests.get(url, headers=headers)
        response.encoding = 'utf-8'  # Ensure proper Chinese character encoding
        return response.text
    except Exception as e:
        print(f"Error downloading {url}: {str(e)}")
        return None

def extract_title_and_content(html):
    """
    Extract the main text content and title from HTML
    """
    if not html:
        return None, None, None
    
    soup = BeautifulSoup(html, 'html.parser')
    
    try:
        # Look specifically for h1 with class "title py-1" as requested
        title_element = soup.find('h1', class_='title py-1')
        if title_element and title_element.text.strip():
            title = title_element.text.strip()
        else:
            # Fallback to other title extraction methods
            title = "Untitled"
            breadcrumb = soup.select('.breadcrumb li:last-child, nav li:last-child')
            if breadcrumb and len(breadcrumb) > 0:
                title = breadcrumb[-1].text.strip()
            else:
                h1_element = soup.find('h1')
                if h1_element:
                    title = h1_element.text.strip()
                else:
                    # Try to find title in the URL or meta tags
                    meta_title = soup.find('meta', property='og:title')
                    if meta_title and meta_title.get('content'):
                        title = meta_title['content'].strip()
        
        # Clean up navigation and footer elements that we don't want in the content
        for element in soup.select('header, footer, nav, .breadcrumb, script, style, iframe, .ad, .ads, .advertisement'):
            if element:
                element.decompose()
        
        # Try to find the main content
        content = ""
        
        # Look for common content containers
        content_candidates = [
            soup.find('article'),
            soup.find('div', class_='content'),
            soup.find('div', class_='article'),
            soup.find('div', class_='post'),
            soup.find('div', class_='entry'),
            soup.find('div', id='content'),
            soup.find('div', id='main'),
            soup.find('main')
        ]
        
        # Filter out None values
        content_candidates = [c for c in content_candidates if c]
        
        if content_candidates:
            # Use the first candidate that has substantial text
            for candidate in content_candidates:
                text = candidate.get_text('\n', strip=True)
                if len(text) > 200:  # Assuming real content has some minimum length
                    content = text
                    break
        
        # If no suitable container found, try to extract paragraphs directly
        if not content:
            paragraphs = []
            for p in soup.find_all('p'):
                if len(p.text.strip()) > 50:  # Filter out short paragraphs that might be UI elements
                    paragraphs.append(p.text.strip())
            
            if paragraphs:
                content = '\n\n'.join(paragraphs)
        
        # Last resort: get text from body but try to clean it up
        if not content:
            body = soup.find('body')
            if body:
                # Remove very short lines that are likely navigation or UI elements
                lines = [line.strip() for line in body.get_text('\n').split('\n') if len(line.strip()) > 0]
                content = '\n'.join([line for line in lines if len(line) > 20 or any(c.isalpha() for c in line)])
        
        return title, content, None
    except Exception as e:
        print(f"Error extracting content: {str(e)}")
        return None, None

def find_related_links(html, keyword="keyword"):
    """
    Find links that contain the specified keyword in their title or text
    """
    if not html:
        return []
    
    soup = BeautifulSoup(html, 'html.parser')
    links = []
    base_url = ""
    
    try:
        # Find all links that contain the keyword
        for a_tag in soup.find_all('a'):
            # Check if the keyword is in the link text or title attribute
            link_text = a_tag.text.strip()
            link_title = a_tag.get('title', '')
            
            if (keyword in link_text or keyword in link_title) and a_tag.has_attr('href'):
                # Get the full URL
                href = a_tag['href']
                if not href.startswith('http'):
                    # Handle relative URLs
                    if href.startswith('/'):
                        href = f"{base_url}{href}"
                    else:
                        href = f"{base_url}/{href}"
                
                # Avoid duplicates and fragment identifiers
                clean_href = href.split('#')[0]
                if clean_href not in links:
                    links.append(clean_href)
        
        # If no links found with the keyword, try to find links that might be chapter links
        if not links:
            # Look for common chapter navigation patterns
            chapter_links = []
            
            # Look for links in a navigation element
            nav_elements = soup.select('nav, .navigation, .pagination, .chapters, .chapter-list')
            for nav in nav_elements:
                for a_tag in nav.find_all('a'):
                    if a_tag.has_attr('href'):
                        href = a_tag['href']
                        if not href.startswith('http'):
                            if href.startswith('/'):
                                href = f"{base_url}{href}"
                            else:
                                href = f"{base_url}/{href}"
                        
                        # Avoid duplicates and fragment identifiers
                        clean_href = href.split('#')[0]
                        if clean_href not in chapter_links:
                            chapter_links.append(clean_href)
            
            # If we found chapter links, add them to our links list
            if chapter_links:
                links.extend(chapter_links)
    
    except Exception as e:
        print(f"Error finding related links: {str(e)}")
    
    return links

def save_content(title, content, directory="downloaded_content"):
    """
    Save content to a file named after the title
    """
    if not content:
        return None
    
    # Create directory if it doesn't exist
    if not os.path.exists(directory):
        os.makedirs(directory)
    
    # Use title as filename
    filename_base = title if title else "untitled"
    
    # Clean filename for use as filename
    clean_filename = re.sub(r'[\\/*?:"<>|]', "", filename_base)
    if not clean_filename:
        clean_filename = "untitled"
    
    # Save file with UTF-8 encoding
    file_path = os.path.join(directory, f"{clean_filename}.txt")
    try:
        with open(file_path, 'w', encoding='utf-8') as f:
            f.write(content)
        return file_path
    except Exception as e:
        print(f"Error saving content: {str(e)}")
        return None

def main():
    # Initial URL
    start_url = ""
    
    # Download initial page
    print(f"Downloading initial page: {start_url}")
    html = download_page(start_url)
    
    if not html:
        print("Failed to download the initial page. Exiting.")
        return
    
    # Extract and save content from initial page
    title, content, _ = extract_title_and_content(html)
    if content:
        file_path = save_content(title, content)
        if file_path:
            print(f"Saved content to: {file_path}")
    else:
        print("Failed to extract content from the initial page.")
    
    # Find related links
    related_links = find_related_links(html)
    print(f"Found {len(related_links)} related links")
    
    # Filter links to only include those with "人间仙境" in the URL or that are likely to be chapter links
    filtered_links = []
    for link in related_links:
        if "人间仙境" in link or any(pattern in link for pattern in ['/zh-hans/', 'chapter', 'page']):
            filtered_links.append(link)
    
    if filtered_links:
        print(f"Filtered to {len(filtered_links)} relevant links")
        related_links = filtered_links
    
    # Download content from related links
    for i, link in enumerate(related_links):
        print(f"Processing link {i+1}/{len(related_links)}: {link}")
        
        # Add delay to avoid overloading the server
        time.sleep(2)
        
        # Download page
        link_html = download_page(link)
        
        if not link_html:
            print(f"Failed to download content from {link}. Skipping.")
            continue
        
        # Extract and save content
        link_title, link_content, _ = extract_title_and_content(link_html)
        if link_content:
            link_file_path = save_content(link_title, link_content)
            if link_file_path:
                print(f"Saved content to: {link_file_path}")
        else:
            print(f"Failed to extract content from {link}.")

if __name__ == "__main__":
    main()
