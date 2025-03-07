cd %CD%
del Installer.7z
del MyHomeBills_Setup.exe
7z.exe a Installer.7z .
copy /b 7zS.sfx + config.txt + Installer.7z MyHomeBills_Setup.exe
pause