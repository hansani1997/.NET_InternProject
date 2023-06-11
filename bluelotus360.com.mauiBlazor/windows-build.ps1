$FilePath = "Platforms/Windows/Package.appxmanifest"


#Load XML Content
$xml = [xml](Get-Content $FilePath)



# Get identity Element
$IdentityElement = $xml.Package.Identity

echo "Your Current Version"
$IdentityElement.Version
echo "________________________"

# Get the current version Number
$version = $IdentityElement.Version.Split(".") | ForEach-Object { [int]$_ }

# Increment values

$response = Read-Host "Do you Update Major Version ? (Y / N) "

if($response -eq "y" -or $response -eq "Y"){
    $version[0]++
}

$response = Read-Host "Do you Update Minor Version ? (Y / N) "

if($response -eq "y" -or $response -eq "Y"){
    $version[1]++
}
$response = Read-Host "Do you Update Build Number ? (Y / N) "

if($response -eq "y" -or $response -eq "Y"){
    $version[2]++
}
$response = Read-Host "Do you Update Revision Number ? (Y / N) "

if($response -eq "y" -or $response -eq "Y"){
    $version[3]++
}

# Update version with new one
$IdentityElement.Version = "$($version[0]).$($version[1]).$($version[2]).$($version[3])"

$CLoc = Get-Location


$combined = Join-Path $CLoc $FilePath


# Save the updated xml files
$xml.Save($combined)

$resp = Read-Host "Do you want to build now? (Y/N)"

if($resp -eq "Y" -or $resp -eq "y"){
    dotnet publish -f net7.0-windows10.0.19041.0 -c Release
}

Pause