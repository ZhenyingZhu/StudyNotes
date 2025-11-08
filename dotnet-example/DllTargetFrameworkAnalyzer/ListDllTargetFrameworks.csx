#!/usr/bin/env dotnet-script
#r "nuget: System.Reflection.Metadata, 8.0.0"

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;

var baseDir = @"$(baseDir)";
var outputFile = Path.Combine(baseDir, "DllTargetFrameworks.csv");

var results = new List<(string path, string frameworks)>();

// Get all DLL files recursively
var dllFiles = Directory.GetFiles(baseDir, "*.dll", SearchOption.AllDirectories)
    .OrderBy(f => f)
    .ToList();

Console.WriteLine($"Found {dllFiles.Count} DLL files. Analyzing...\n");

foreach (var dllPath in dllFiles)
{
    try
    {
        var frameworks = GetTargetFrameworks(dllPath);
        var relativePath = Path.GetRelativePath(baseDir, dllPath);
        results.Add((relativePath, frameworks));
        Console.WriteLine($"{relativePath}, {frameworks}");
    }
    catch (Exception ex)
    {
        var relativePath = Path.GetRelativePath(baseDir, dllPath);
        results.Add((relativePath, $"ERROR: {ex.Message}"));
        Console.WriteLine($"{relativePath}, ERROR: {ex.Message}");
    }
}

// Write to CSV file
using (var writer = new StreamWriter(outputFile, false, Encoding.UTF8))
{
    writer.WriteLine("DLLPath,TargetFrameworks");
    foreach (var (path, frameworks) in results)
    {
        writer.WriteLine($"{path},{frameworks}");
    }
}

Console.WriteLine($"\nResults written to: {outputFile}");
Console.WriteLine($"Total DLLs analyzed: {results.Count}");

string GetTargetFrameworks(string assemblyPath)
{
    var frameworks = new HashSet<string>();
    
    using (var stream = File.OpenRead(assemblyPath))
    using (var peReader = new PEReader(stream))
    {
        if (!peReader.HasMetadata)
        {
            return "No metadata";
        }

        var metadataReader = peReader.GetMetadataReader();
        
        // Check for TargetFrameworkAttribute
        foreach (var attributeHandle in metadataReader.CustomAttributes)
        {
            var attribute = metadataReader.GetCustomAttribute(attributeHandle);
            
            if (attribute.Parent.Kind == HandleKind.AssemblyDefinition)
            {
                var attributeType = GetAttributeTypeName(metadataReader, attribute);
                
                if (attributeType == "System.Runtime.Versioning.TargetFrameworkAttribute")
                {
                    var framework = GetAttributeValue(metadataReader, attribute);
                    if (!string.IsNullOrEmpty(framework))
                    {
                        frameworks.Add(framework);
                    }
                }
            }
        }
        
        // If no TargetFrameworkAttribute found, check assembly references to infer framework
        if (frameworks.Count == 0)
        {
            var targetFramework = InferTargetFramework(metadataReader);
            if (!string.IsNullOrEmpty(targetFramework))
            {
                frameworks.Add(targetFramework);
            }
        }
    }
    
    return frameworks.Count > 0 ? string.Join(";", frameworks) : "Unknown";
}

string GetAttributeTypeName(MetadataReader reader, CustomAttribute attribute)
{
    try
    {
        if (attribute.Constructor.Kind == HandleKind.MemberReference)
        {
            var ctor = reader.GetMemberReference((MemberReferenceHandle)attribute.Constructor);
            if (ctor.Parent.Kind == HandleKind.TypeReference)
            {
                var typeRef = reader.GetTypeReference((TypeReferenceHandle)ctor.Parent);
                var typeName = reader.GetString(typeRef.Name);
                var typeNamespace = reader.GetString(typeRef.Namespace);
                return $"{typeNamespace}.{typeName}";
            }
        }
        else if (attribute.Constructor.Kind == HandleKind.MethodDefinition)
        {
            var method = reader.GetMethodDefinition((MethodDefinitionHandle)attribute.Constructor);
            var typeDef = reader.GetTypeDefinition(method.GetDeclaringType());
            var typeName = reader.GetString(typeDef.Name);
            var typeNamespace = reader.GetString(typeDef.Namespace);
            return $"{typeNamespace}.{typeName}";
        }
    }
    catch { }
    
    return string.Empty;
}

string GetAttributeValue(MetadataReader reader, CustomAttribute attribute)
{
    try
    {
        var valueBlob = reader.GetBlobReader(attribute.Value);
        
        // Skip prolog (0x0001)
        if (valueBlob.ReadUInt16() != 0x0001)
            return string.Empty;
        
        // Read the string value
        var value = valueBlob.ReadSerializedString();
        return value;
    }
    catch { }
    
    return string.Empty;
}

string InferTargetFramework(MetadataReader reader)
{
    var hasSystemRuntime = false;
    var hasNetStandard = false;
    var hasMscorlib = false;
    var frameworkVersion = "";
    
    foreach (var assemblyRefHandle in reader.AssemblyReferences)
    {
        var assemblyRef = reader.GetAssemblyReference(assemblyRefHandle);
        var name = reader.GetString(assemblyRef.Name);
        var version = assemblyRef.Version;
        
        if (name == "System.Runtime")
        {
            hasSystemRuntime = true;
        }
        else if (name == "netstandard")
        {
            hasNetStandard = true;
            frameworkVersion = $".NETStandard,Version=v{version.Major}.{version.Minor}";
        }
        else if (name == "mscorlib")
        {
            hasMscorlib = true;
            if (version.Major == 4)
            {
                frameworkVersion = $".NETFramework,Version=v{version.Major}.{version.Minor}.{version.Build}";
            }
            else if (version.Major == 2)
            {
                frameworkVersion = $".NETFramework,Version=v3.5";
            }
        }
    }
    
    // Prioritize explicit framework references
    if (!string.IsNullOrEmpty(frameworkVersion))
    {
        return frameworkVersion;
    }
    
    // Infer based on references
    if (hasNetStandard)
    {
        return ".NETStandard";
    }
    else if (hasSystemRuntime && !hasMscorlib)
    {
        return ".NETCore or .NETStandard";
    }
    else if (hasMscorlib)
    {
        return ".NETFramework,Version=v4.0";
    }
    
    return string.Empty;
}
