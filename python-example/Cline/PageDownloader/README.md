# Web Crawler

A Python web crawler that downloads text content from web pages and follows links to the next page. This project includes two versions of the crawler:

1. **Basic Crawler** (`web_crawler.py`): Uses requests and BeautifulSoup to download and parse static HTML content.
2. **Selenium Crawler** (`selenium_crawler.py`): Uses Selenium WebDriver to handle JavaScript-rendered content.

## Features

- Starts crawling from a specified URL
- Extracts text content from web pages
- Identifies and follows links to the next page
- Saves content to text files named by page number
- Limits crawling to a specified number of pages
- Handles JavaScript-rendered content (Selenium version)

## Requirements

- Python 3.6 or higher
- Required packages:
  - requests
  - beautifulsoup4
  - selenium (for the Selenium-based crawler)
- Chrome browser (for the Selenium-based crawler)
- ChromeDriver (for the Selenium-based crawler)

## Installation

1. Clone this repository or download the files.
2. Install the required packages:

```bash
pip install -r requirements.txt
```

3. For the Selenium-based crawler, you'll need to install Chrome browser and ChromeDriver:
   - Download Chrome browser from [here](https://www.google.com/chrome/)
   - Download ChromeDriver from [here](https://sites.google.com/chromium.org/driver/) (make sure to download the version that matches your Chrome browser version)
   - Add ChromeDriver to your system PATH

## Usage

### Basic Crawler

1. Open `web_crawler.py` and modify the `start_url` if needed:

```python
start_url = ""
```

2. You can also change the maximum number of pages to crawl (default is 2):

```python
crawler = WebCrawler(start_url, max_pages=2)
```

3. Run the script:

```bash
python web_crawler.py
```

### Selenium-based Crawler

1. Open `selenium_crawler.py` and modify the `start_url` if needed:

```python
start_url = ""
```

2. You can also change the maximum number of pages to crawl (default is 2):

```python
crawler = WebCrawler(start_url, max_pages=2)
```

3. Run the script:

```bash
python selenium_crawler.py
```

4. The crawler will download the content and save it to text files in the `downloaded_pages` directory.

### Choosing the Right Crawler

- Use the basic crawler (`web_crawler.py`) for static websites that don't rely on JavaScript to load content.
- Use the Selenium-based crawler (`selenium_crawler.py`) for dynamic websites that use JavaScript to load content.

## How It Works

1. The crawler starts at the specified URL.
2. It downloads the HTML content of the page.
3. It extracts the text content from the HTML.
4. It saves the text content to a file named `page_X.txt` where X is the page number.
5. It looks for a link to the next page.
6. If it finds a next page link and hasn't reached the maximum number of pages, it repeats the process.

## Customization

You can customize the crawler by modifying the following:

- `max_pages`: Change the maximum number of pages to crawl.
- `output_dir`: Change the directory where the text files are saved.
- `find_next_page_link`: Modify the logic for finding the next page link.
- `extract_text`: Customize how text is extracted from HTML.

## Notes

- The crawler includes a 1-second delay between requests to avoid overloading the server.
- It uses a User-Agent header to mimic a web browser.
- It skips pages that have already been visited to avoid infinite loops.
