robocopy .\ s:\HomeServer\ Install HomeServer.exe.config HomeServer.exe HomeSettings.xml *.pdb *.dll /E
robocopy .\Install s:\HomeServer\Install installService home639server.service
exit 0
