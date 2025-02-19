import requests
import json
import os

CLIENT_ID = "56dbe56c-b961-49b9-824b-204abc45bfe3"
CLIENT_SECRET = os.getenv("azureclientsecret") # doesn't work yet
TENANT_ID = "c74a24d8-2986-4739-aec1-36b4c9934ed3"
GRAPH_API_ENDPOINT = "https://graph.microsoft.com/v1.0/me/onenote/pages"

# Get Access Token
def get_access_token():
    token_url = f"https://login.microsoftonline.com/{TENANT_ID}/oauth2/v2.0/token"
    token_data = {
        "grant_type": "client_credentials",
        "client_id": CLIENT_ID,
        "client_secret": CLIENT_SECRET,
        "scope": "https://graph.microsoft.com/.default",
    }
    response = requests.post(token_url, data=token_data)
    response_json = response.json()
    return response_json.get("access_token")

# Get all OneNote pages
def get_onenote_pages():
    access_token = get_access_token()
    headers = {"Authorization": f"Bearer {access_token}"}
    response = requests.get(GRAPH_API_ENDPOINT, headers=headers)
    
    if response.status_code == 200:
        pages = response.json().get("value", [])
        return pages
    else:
        print("Error fetching pages:", response.text)
        return []

# Extract text content from each OneNote page
def extract_text_from_pages():
    pages = get_onenote_pages()
    access_token = get_access_token()
    headers = {"Authorization": f"Bearer {access_token}"}
    
    for page in pages:
        page_id = page["id"]
        page_title = page["title"]
        page_content_url = f"https://graph.microsoft.com/v1.0/me/onenote/pages/{page_id}/content"
        
        response = requests.get(page_content_url, headers=headers)
        if response.status_code == 200:
            page_html = response.text
            print(f"Page Title: {page_title}")
            print(f"Content (HTML): {page_html[:500]}...\n")  # Truncate for preview
        else:
            print(f"Error fetching content for {page_title}: {response.text}")

# Run the script
extract_text_from_pages()