# Compare daily quantity sums between MyCal.csv and SOT.csv

# Read MyCal.csv and sum quantities by date
$myCalData = Import-Csv -Path "MyCal.csv"
$myCalSums = @{}

foreach ($row in $myCalData) {
    $date = $row.'Date (Start track on 11/29/2020)'
    $quantity = [decimal]$row.Quantity
    
    if ($myCalSums.ContainsKey($date)) {
        $myCalSums[$date] += $quantity
    } else {
        $myCalSums[$date] = $quantity
    }
}

# Read SOT.csv and sum quantities by date
$sotData = Import-Csv -Path "SOT.csv"
$sotSums = @{}

foreach ($row in $sotData) {
    $date = $row.Date
    $quantity = [decimal]$row.Quantity
    
    if ($sotSums.ContainsKey($date)) {
        $sotSums[$date] += $quantity
    } else {
        $sotSums[$date] = $quantity
    }
}

# Get all unique dates from both files
$allDates = ($myCalSums.Keys + $sotSums.Keys) | Select-Object -Unique | Sort-Object

# Compare sums and find mismatches
Write-Host "`nComparing daily quantity sums:" -ForegroundColor Cyan
Write-Host "=" * 80

$mismatches = @()

foreach ($date in $allDates) {
    $myCalSum = if ($myCalSums.ContainsKey($date)) { $myCalSums[$date] } else { 0 }
    $sotSum = if ($sotSums.ContainsKey($date)) { $sotSums[$date] } else { 0 }
    
    $difference = [Math]::Abs($myCalSum - $sotSum)
    
    if ($difference -gt 0.001) {  # Allow for small floating point differences
        $mismatch = [PSCustomObject]@{
            Date = $date
            MyCal_Sum = [Math]::Round($myCalSum, 3)
            SOT_Sum = [Math]::Round($sotSum, 3)
            Difference = [Math]::Round($myCalSum - $sotSum, 3)
        }
        $mismatches += $mismatch
    }
}

# Display results
if ($mismatches.Count -eq 0) {
    Write-Host "`nAll dates match! No discrepancies found." -ForegroundColor Green
} else {
    Write-Host "`nFound $($mismatches.Count) date(s) with mismatched quantities:" -ForegroundColor Yellow
    Write-Host ""
    $mismatches | Format-Table -AutoSize
}

Write-Host "`nSummary:" -ForegroundColor Cyan
Write-Host "Total dates in MyCal.csv: $($myCalSums.Count)"
Write-Host "Total dates in SOT.csv: $($sotSums.Count)"
Write-Host "Dates with mismatches: $($mismatches.Count)"
