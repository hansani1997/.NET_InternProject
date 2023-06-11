$FilePath = "Platforms/Android/AndroidManifest.xml"

$xml = [xml](Get-Content $FilePath)

$currentVersion = $xml.manifest.'versionCode'

$re = Read-Host "Do you wish to increment version number ? (Y / N)";

if($re -eq "Y" -or $re -eq "y"){
    $newVersion = [int]$currentVersion + 1
    $versionName = Read-Host "Give Version Name : "
    $xml.manifest.'versionCode' = $newVersion.ToString()
    $xml.manifest.versionName = $versionName.ToString()
}

$CLoc = Get-Location


$combined = Join-Path $CLoc $FilePath


# Save the updated xml files
$xml.Save($combined)

$resp = Read-Host "Do you want to build now? (Y/N)"

if($resp -eq "Y" -or $resp -eq "y"){
    dotnet publish -f:net7.0-android -c:Release /p:AndroidSigningKeyPass=bl10pwd-UVHBG /p:AndroidSigningStorePass=bl10pwd-UVHBG
}

Pause