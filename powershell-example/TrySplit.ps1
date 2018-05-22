#$EncodedCommaSepartedList = "0%2c1%2c2"
#UrlDecode($EncodedCommaSepartedList)

$CommaSepartedList = "0-0,1-1,2-2"

$nums = $CommaSepartedList.Split(",")

foreach($num in $nums)
{
    Write-Host($num)
}