import os
import time
import re
from urllib.parse import urljoin
from selenium import webdriver
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from bs4 import BeautifulSoup

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
        self.page_count = 210
        
        # Create output directory if it doesn't exist
        self.output_dir = "downloaded_pages"
        if not os.path.exists(self.output_dir):
            os.makedirs(self.output_dir)
        
        # Set up Selenium
        self.setup_selenium()
    
    def setup_selenium(self):
        """
        Set up Selenium WebDriver.
        """
        chrome_options = Options()
        chrome_options.add_argument("--headless")  # Run in headless mode (no GUI)
        chrome_options.add_argument("--disable-gpu")
        chrome_options.add_argument("--no-sandbox")
        chrome_options.add_argument("--disable-dev-shm-usage")
        chrome_options.add_argument("--window-size=1920,1080")
        
        # Create a new instance of the Chrome driver
        self.driver = webdriver.Chrome(options=chrome_options)
    
    def fetch_page(self, url):
        """
        Fetch the HTML content of a page using Selenium.
        
        Args:
            url (str): The URL to fetch
            
        Returns:
            str: The HTML content of the page
        """
        try:
            print(f"Attempting to fetch URL: {url}")
            self.driver.get(url)
            
            print("Waiting for page to load...")
            # Wait for the page to load (adjust timeout as needed)
            WebDriverWait(self.driver, 10).until(
                EC.presence_of_element_located((By.TAG_NAME, "body"))
            )
            
            # Wait a bit more for any JavaScript to execute
            print("Waiting for JavaScript to execute...")
            time.sleep(3)
            
            # Get the page source after JavaScript execution
            page_source = self.driver.page_source
            print(f"Page source length: {len(page_source)} characters")
            return page_source
        except Exception as e:
            print(f"Error fetching {url}: {e}")
            return None
    
    def extract_text(self, html):
        """
        Extract text content from HTML.
        
        Args:
            html (str): The HTML content
            
        Returns:
            str: The extracted text with line breaks for each paragraph
        """
        if not html:
            return ""
        
        soup = BeautifulSoup(html, 'html.parser')
        
        # Remove script and style elements
        for script_or_style in soup(["script", "style"]):
            script_or_style.decompose()
        
        # Process paragraphs to add line breaks
        paragraphs = []
        
        # First, handle actual <p> tags
        p_tags = soup.find_all('p')
        if p_tags:
            for p in p_tags:
                # Get text from this paragraph and clean it
                p_text = p.get_text().strip()
                if p_text:  # Only add non-empty paragraphs
                    paragraphs.append(p_text)
        
        # If no <p> tags were found, fall back to the original method
        if not paragraphs:
            # Get all text
            text = soup.get_text()
            
            # Clean text
            lines = (line.strip() for line in text.splitlines())
            chunks = (phrase.strip() for line in lines for phrase in line.split("  "))
            text = '\n'.join(chunk for chunk in chunks if chunk)
            return text
        
        # Join paragraphs with double line breaks
        text = '\n\n'.join(paragraphs)
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
                    href = element.get('href')
                    # Check if the href is a JavaScript function call
                    if href.startswith('javascript:'):
                        print(f"Found JavaScript link: {href}")
                        return None  # Skip JavaScript links as they can't be directly navigated to
                    return urljoin(current_url, href)
        
        # If we couldn't find a clear "next" link, look for any link that might be a pagination link
        pagination_links = soup.find_all('a', href=True)
        for link in pagination_links:
            href = link.get('href')
            # Skip JavaScript links
            if href.startswith('javascript:'):
                continue
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
        
        try:
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
                
                # Try to find the next page link
                # First, check for JavaScript links that might be the next page
                try:
                    # Look for links with text that suggests "next page"
                    next_links = self.driver.find_elements(By.XPATH, "//a[contains(text(), '下一章') or contains(text(), '下一页') or contains(text(), 'next')]")
                    
                    if next_links:
                        # Found a next page link with Selenium
                        next_link = next_links[0]
                        next_text = next_link.text.strip()
                        next_href = next_link.get_attribute("href")
                        print(f"Found next page link with Selenium: Text='{next_text}', href='{next_href}'")
                        
                        # Check if it's a JavaScript link
                        if next_href and next_href.startswith("javascript:"):
                            print(f"Executing JavaScript: {next_href}")
                            try:
                                # Extract the JavaScript function name from the href
                                js_function = next_href.replace("javascript:", "")
                                print(f"Extracted JavaScript function: {js_function}")
                                
                                # Try to execute the JavaScript directly
                                print("Executing JavaScript directly...")
                                self.driver.execute_script(js_function)
                                
                                # Wait for the page to update
                                print("Waiting for page to update...")
                                time.sleep(3)
                                
                                # Get the new URL after JavaScript execution
                                new_url = self.driver.current_url
                                print(f"New URL after JavaScript execution: {new_url}")
                                
                                # If the URL changed, use it as the next URL
                                if new_url != current_url:
                                    next_url = new_url
                                else:
                                    # If URL didn't change, we're still on the same page
                                    # Try to find content changes that indicate we're on a new page
                                    print("URL didn't change, checking for content changes...")
                                    # Get the new page source
                                    new_html = self.driver.page_source
                                    # Extract text from the new page
                                    new_text = self.extract_text(new_html)
                                    # If the text is different, we're on a new page
                                    if new_text != text:
                                        print("Content changed, treating as a new page")
                                        html = new_html
                                        text = new_text
                                        # Save the new content
                                        self.page_count += 1
                                        self.save_text_to_file(text, self.page_count)
                                        continue  # Skip to the next iteration
                                    else:
                                        print("Content didn't change, no next page found")
                                        next_url = None
                            except Exception as e:
                                print(f"Error executing JavaScript: {e}")
                                # Try an alternative approach - scroll to the element and click
                                try:
                                    print("Trying alternative approach - scrolling to element...")
                                    # Scroll to the element
                                    self.driver.execute_script("arguments[0].scrollIntoView(true);", next_link)
                                    time.sleep(1)  # Wait for scroll to complete
                                    
                                    # Try to click again
                                    print("Attempting to click after scrolling...")
                                    next_link.click()
                                    
                                    # Wait for the page to update
                                    time.sleep(3)
                                    
                                    # Check for content changes
                                    new_html = self.driver.page_source
                                    new_text = self.extract_text(new_html)
                                    if new_text != text:
                                        print("Content changed after alternative approach")
                                        html = new_html
                                        text = new_text
                                        self.page_count += 1
                                        self.save_text_to_file(text, self.page_count)
                                        continue  # Skip to the next iteration
                                    else:
                                        print("Content didn't change after alternative approach")
                                        next_url = None
                                except Exception as e2:
                                    print(f"Alternative approach failed: {e2}")
                                    next_url = None
                        else:
                            # Regular URL, use it directly
                            next_url = next_href
                    else:
                        # Fall back to the original method
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
                except Exception as e:
                    print(f"Error finding next page link: {e}")
                    next_url = None
                
                # Add a small delay to avoid overloading the server
                time.sleep(1)
                
                # Update current URL
                current_url = next_url
            
            print(f"Crawling complete. Visited {self.page_count} pages.")
        finally:
            # Always close the browser when done
            self.driver.quit()


if __name__ == "__main__":
    # URL to start crawling from
    start_url = ""
    
    # Create and run the crawler
    crawler = WebCrawler(start_url, max_pages=214)
    crawler.crawl()
