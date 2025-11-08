# DLL Target Framework Analyzer

This tool analyzes all DLL files in a directory and extracts their target framework information.

## Output Format

The program generates a CSV file with the following format:
```
DLLPath,TargetFrameworks
MsodsRoleConfigurator\amd64\Microsoft.Azure.KeyVault.Core.dll,.NETFramework,Version=v4.0
```

## Available Options

### Option 1: PowerShell Script (Recommended - No compilation needed)

**Run:**
```powershell
.\ListDllTargetFrameworks.ps1
```

This will:
- Scan all DLL files in the current directory and subdirectories
- Display results in the console
- Save results to `DllTargetFrameworks.csv`

**Run with custom directory:**
```powershell
.\ListDllTargetFrameworks.ps1 -BaseDir "C:\path\to\dlls" -OutputFile "C:\output\results.csv"
```

### Option 2: C# Console Application

**Build:**
```powershell
dotnet build DllFrameworkAnalyzer.csproj
```

**Run:**
```powershell
dotnet run --project DllFrameworkAnalyzer.csproj
```

**Run with custom directory:**
```powershell
dotnet run --project DllFrameworkAnalyzer.csproj -- "C:\path\to\dlls"
```

### Option 3: C# Script (requires dotnet-script)

**Install dotnet-script** (if not already installed):
```powershell
dotnet tool install -g dotnet-script
```

**Run:**
```powershell
dotnet script ListDllTargetFrameworks.csx
```

## How It Works

The tool uses the following methods to determine target frameworks:

1. **Primary Method**: Reads the `TargetFrameworkAttribute` from the assembly metadata
2. **Fallback Method**: Infers the framework by analyzing assembly references:
   - `mscorlib` reference → .NET Framework
   - `netstandard` reference → .NET Standard
   - `System.Runtime` without `mscorlib` → .NET Core/.NET Standard

## Output Examples

```
DLLPath,TargetFrameworks
Microsoft.Azure.KeyVault.Core.dll,.NETFramework,Version=v4.0
Newtonsoft.Json.dll,.NETStandard,Version=v2.0
SomeApp.dll,.NETCoreApp,Version=v6.0
MsodsRoleConfigurator\amd64\Microsoft.Azure.KeyVault.Core.dll,.NETFramework,Version=v4.0
```

## Requirements

- **PowerShell Script**: PowerShell 5.1+ or PowerShell Core 7+
- **C# Console App**: .NET 6.0 SDK or later
- **C# Script**: dotnet-script tool

## Troubleshooting

If a DLL cannot be analyzed, the output will show:
- `ERROR: [error message]` - If the file cannot be read
- `No metadata` - If the DLL doesn't contain .NET metadata
- `Unknown` - If the framework cannot be determined
