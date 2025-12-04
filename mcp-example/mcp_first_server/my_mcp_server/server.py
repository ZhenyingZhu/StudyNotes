# server.py
from mcp.server.fastmcp import FastMCP

# Initialize a FastMCP server instance with a unique name
mcp = FastMCP("my_first_server")

@mcp.tool()
def add(a: int, b: int) -> int:
    """Adds two integer numbers together."""
    return a + b

@mcp.resource("config://app")
def get_config() -> dict:
    """Provides the application's configuration."""
    return {"version": "1.0", "author": "MyTeam"}

@mcp.prompt()
def review_code(code: str) -> str:
    """Provides a prompt for reviewing code."""
    return f"Please review this code:\n\n{code}"

if __name__ == "__main__":
    # Run the server using the standard input/output transport
    mcp.run(transport="stdio")