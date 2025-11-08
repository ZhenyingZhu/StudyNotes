param(
    [string]$BaseDir = $PSScriptRoot,
    [string]$OutputFile = (Join-Path $BaseDir "DllTargetFrameworks.csv")
)

$ErrorActionPreference = 'Continue'

# Function to get target framework from a DLL
function Get-TargetFramework {
    param([string]$DllPath)
    
    try {
        # Try using reflection to load the assembly
        $assembly = [System.Reflection.Assembly]::ReflectionOnlyLoadFrom($DllPath)
        
        # Get TargetFrameworkAttribute
        $targetFrameworkAttr = $assembly.CustomAttributes | 
            Where-Object { $_.AttributeType.Name -eq 'TargetFrameworkAttribute' } | 
            Select-Object -First 1
        
        if ($targetFrameworkAttr) {
            $framework = $targetFrameworkAttr.ConstructorArguments[0].Value
            return $framework
        }
        
        # Fallback: Check referenced assemblies to infer framework
        $refs = $assembly.GetReferencedAssemblies()
        
        # Check for mscorlib (indicates .NET Framework)
        $mscorlib = $refs | Where-Object { $_.Name -eq 'mscorlib' } | Select-Object -First 1
        if ($mscorlib) {
            $version = $mscorlib.Version
            if ($version.Major -eq 4) {
                return ".NETFramework,Version=v$($version.Major).$($version.Minor)"
            }
            return ".NETFramework,Version=v4.0"
        }
        
        # Check for netstandard
        $netstandard = $refs | Where-Object { $_.Name -eq 'netstandard' } | Select-Object -First 1
        if ($netstandard) {
            $version = $netstandard.Version
            return ".NETStandard,Version=v$($version.Major).$($version.Minor)"
        }
        
        # Check for System.Runtime (indicates .NET Core/.NET Standard)
        $systemRuntime = $refs | Where-Object { $_.Name -eq 'System.Runtime' } | Select-Object -First 1
        if ($systemRuntime) {
            return ".NETCore or .NETStandard"
        }
        
        return "Unknown"
        
    } catch {
        # If reflection fails, try to read PE metadata directly
        try {
            Add-Type -AssemblyName System.Reflection.Metadata
            
            $stream = [System.IO.File]::OpenRead($DllPath)
            $peReader = New-Object System.Reflection.PortableExecutable.PEReader($stream)
            
            if (-not $peReader.HasMetadata) {
                $stream.Dispose()
                $peReader.Dispose()
                return "No metadata"
            }
            
            $metadataReader = $peReader.GetMetadataReader()
            
            # Look for TargetFrameworkAttribute
            foreach ($attrHandle in $metadataReader.CustomAttributes) {
                $attr = $metadataReader.GetCustomAttribute($attrHandle)
                
                # This is complex to parse, so we'll return a generic message
                # Full implementation would require parsing the attribute constructor
            }
            
            $stream.Dispose()
            $peReader.Dispose()
            
            return "Could not determine (PE read)"
            
        } catch {
            return "ERROR: $($_.Exception.Message)"
        }
    }
}

Write-Host "Scanning for DLL files in: $BaseDir" -ForegroundColor Cyan
Write-Host ""

# Get all DLL files recursively
$dllFiles = Get-ChildItem -Path $BaseDir -Filter "*.dll" -Recurse -File | Sort-Object FullName

Write-Host "Found $($dllFiles.Count) DLL files. Analyzing..." -ForegroundColor Yellow
Write-Host ""

$results = @()

foreach ($dll in $dllFiles) {
    $relativePath = $dll.FullName.Substring($BaseDir.Length + 1)
    
    Write-Host "Analyzing: $relativePath" -NoNewline
    
    $framework = Get-TargetFramework -DllPath $dll.FullName
    
    Write-Host " -> $framework" -ForegroundColor Green
    
    $results += [PSCustomObject]@{
        DLLPath = $relativePath
        TargetFrameworks = $framework
    }
}

# Export to CSV
Write-Host ""
Write-Host "Writing results to: $OutputFile" -ForegroundColor Cyan

$results | Export-Csv -Path $OutputFile -NoTypeInformation -Encoding UTF8

Write-Host ""
Write-Host "Done! Total DLLs analyzed: $($results.Count)" -ForegroundColor Green
Write-Host ""
Write-Host "Sample results:" -ForegroundColor Cyan
$results | Select-Object -First 10 | Format-Table -AutoSize
