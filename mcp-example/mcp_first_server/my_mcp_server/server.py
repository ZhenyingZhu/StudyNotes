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

def build_project(csproj_path: str) -> str:
    """Builds a .NET project using msbuild."""
    import subprocess
    try:
        result = subprocess.run(
            ["msbuild", csproj_path],
            capture_output=True,
            text=True,
            shell=True
        )
        return result.stdout + "\n" + result.stderr
    except Exception as e:
        return f"Error building project: {e}"

    # Build PowerShell command for admin msbuild
    ps_command = f'Start-Process powershell -Verb runAs -ArgumentList "msbuild \"{csproj_path}\""'
    try:
        result = subprocess.run([
            "powershell",
            "-Command",
            ps_command
        ], capture_output=True, text=True, shell=True)
        output = result.stdout + "\n" + result.stderr
    except Exception as e:
        output = f"Error running msbuild: {e}"
    return [
        types.TextContent(
            type="text",
            text=f"msbuild output for {csproj_path}:\n{output}",
        )
    ]


def main():
    """Entry point for the MCP server."""
    mcp.run(transport="stdio")


if __name__ == "__main__":
    # Run the server using the standard input/output transport
    main()