@ECHO OFF

echo.
echo -= IR Server Suite : Build Deploy Debug.bat =-

echo.
echo Writing SVN revision assemblies...
setup\DeployVersionSVN.exe /svn="%CD%"  > build_debug.log

echo.
echo Building IR Server Suite...
"C:\Program Files (x86)\Microsoft Visual Studio 8\Common7\IDE\devenv.com" /rebuild Debug "IR Server Suite.sln" >> build_debug.log

echo.
echo Reverting assemblies...
setup\DeployVersionSVN.exe /svn="%CD%" /revert >> build_debug.log

echo.
echo Building Installer...
setup\DeployVersionSVN.exe /svn="%CD%" /GetVersion >> build_debug.log
IF NOT EXIST version.txt EXIT
SET /p version=<version.txt
DEL version.txt
"C:\Program Files (x86)\NSIS\makensis.exe" /DVER_BUILD=%version% setup\setup.nsi >> build_debug.log