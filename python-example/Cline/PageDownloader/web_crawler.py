import requests
from bs4 import BeautifulSoup
import os
import time
import re
from urllib.parse import urljoin, urlparse

class WebCrawler:
    def __init__(self, start_url, max_pages=2):
        """
        Initialize the web crawler with a starting URL and maximum number of pages to crawl.
        
        Args:
            start_url (str): The URL to start crawling from
            max_pages (int): Maximum number of pages to crawl
        """
        self.start_url = start_url
        self.max_pages = max_pages
        self.visited_urls = set()
        self.page_count = 0
        
        # Create output directory if it doesn't exist
        self.output_dir = "downloaded_pages"
        if not os.path.exists(self.output_dir):
            os.makedirs(self.output_dir)
    
    def fetch_page(self, url):
        """
        Fetch the HTML content of a page.
        
        Args:
            url (str): The URL to fetch
            
        Returns:
            str: The HTML content of the page
        """
        headers = {
            'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36'
        }
        
        try:
            response = requests.get(url, headers=headers)
            response.raise_for_status()
            return response.text
        except requests.exceptions.RequestException as e:
            print(f"Error fetching {url}: {e}")
            return None
    
    def extract_text(self, html):
        """
        Extract text content from HTML.
        
        Args:
            html (str): The HTML content
            
        Returns:
            str: The extracted text
        """
        if not html:
            return ""
        
        soup = BeautifulSoup(html, 'html.parser')
        
        # Remove script and style elements
        for script_or_style in soup(["script", "style"]):
            script_or_style.decompose()
        
        # Get text
        text = soup.get_text()
        
        # Clean text
        lines = (line.strip() for line in text.splitlines())
        chunks = (phrase.strip() for line in lines for phrase in line.split("  "))
        text = '\n'.join(chunk for chunk in chunks if chunk)
        
        return text
    
    def find_next_page_link(self, html, current_url):
        """
        Find the link to the next page.
        
        Args:
            html (str): The HTML content
            current_url (str): The current URL
            
        Returns:
            str: The URL of the next page, or None if not found
        """
        if not html:
            return None
        
        soup = BeautifulSoup(html, 'html.parser')
        
        # Look for common next page indicators
        next_link_patterns = [
            # Look for text that suggests "next page" (including Chinese characters for "Next Chapter")
            lambda tag: tag.name == 'a' and tag.text and re.search(r'next|forward|continue|→|>|»|下一章|下一页', tag.text.lower()),
            
            # Look for pagination links that might be the next page
            lambda tag: tag.name == 'a' and tag.get('class') and any('next' in c.lower() for c in tag.get('class')),
            
            # Look for links with rel="next"
            lambda tag: tag.name == 'a' and tag.get('rel') and 'next' in tag.get('rel'),
            
            # Look for buttons that might lead to the next page
            lambda tag: tag.name == 'button' and tag.text and re.search(r'next|forward|continue|→|>|»|下一章|下一页', tag.text.lower()),
        ]
        
        for pattern in next_link_patterns:
            next_elements = soup.find_all(pattern)
            for element in next_elements:
                if element.name == 'a' and element.get('href'):
                    return urljoin(current_url, element.get('href'))
        
        # If we couldn't find a clear "next" link, look for any link that might be a pagination link
        pagination_links = soup.find_all('a', href=True)
        for link in pagination_links:
            href = link.get('href')
            # Check if the link looks like a pagination link
            if re.search(r'page=\d+|p=\d+|/page/\d+|/p/\d+', href):
                return urljoin(current_url, href)
        
        return None
    
    def save_text_to_file(self, text, page_number):
        """
        Save text content to a file.
        
        Args:
            text (str): The text content to save
            page_number (int): The page number for the filename
        """
        filename = os.path.join(self.output_dir, f"page_{page_number}.txt")
        with open(filename, 'w', encoding='utf-8') as f:
            f.write(text)
        print(f"Saved content to {filename}")
    
    def crawl(self):
        """
        Start the crawling process.
        """
        current_url = self.start_url
        
        while self.page_count < self.max_pages and current_url:
            # Skip if we've already visited this URL
            if current_url in self.visited_urls:
                break
            
            print(f"Crawling: {current_url}")
            self.visited_urls.add(current_url)
            
            # Fetch the page
            html = self.fetch_page(current_url)
            if not html:
                break
            
            # Extract text
            text = self.extract_text(html)
            
            # Increment page count
            self.page_count += 1
            
            # Save text to file
            self.save_text_to_file(text, self.page_count)
            
            # Find the next page link
            next_url = self.find_next_page_link(html, current_url)
            
            if next_url:
                print(f"Found next page link: {next_url}")
            else:
                print("No next page link found. Trying to debug...")
                # Print some links for debugging
                soup = BeautifulSoup(html, 'html.parser')
                links = soup.find_all('a', href=True)
                print(f"Found {len(links)} links on the page")
                for i, link in enumerate(links[:5]):  # Print first 5 links for debugging
                    print(f"Link {i+1}: Text='{link.text.strip()}', href='{link.get('href')}'")
            
            # Add a small delay to avoid overloading the server
            time.sleep(1)
            
            # Update current URL
            current_url = next_url
        
        print(f"Crawling complete. Visited {self.page_count} pages.")


if __name__ == "__main__":
    # URL to start crawling from
    start_url = ""
    
    # Create and run the crawler
    crawler = WebCrawler(start_url, max_pages=2)
    crawler.crawl()
