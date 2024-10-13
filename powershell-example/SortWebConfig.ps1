$doc = New-Object xml
$doc.Load("WebDLLBinding.xml")
$items = $doc.assemblyBinding.dependentAssembly
$sortedItems = $items | Sort-Object -Property @{Expression={$_.assemblyIdentity.name}}
$sortedItems | ForEach-Object {
    Write-Host $_.outerXml
}
