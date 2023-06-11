$response = Read-Host "Do you try to build for which platform (a - android / w - windows)"

if($response -eq "a" -or $response -eq "A"){
    powershell -File android-build.ps1
}

if($response -eq "w" -or $response -eq "W"){
    powershell -File windows-build.ps1
}