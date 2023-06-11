echo 'Press a Key to build'
pause
dotnet publish -f:net7.0-android -c:Release /p:AndroidSigningKeyPass=bl10pwd-UVHBG /p:AndroidSigningStorePass=bl10pwd-UVHBG
pause
pause