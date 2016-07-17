rem HomeServerSettings.json 
robocopy .\ t:\HomeServer\ Install HomeServer.exe.config HomeServer.exe *.pdb *.dll /E
robocopy .\Install t:\HomeServer\Install installService home639server.service
exit 0
