@ECHO OFF


REM set build type to debug
set BUILD_TYPE=Debug


REM set logfile where the infos are written to, and clear that file
set LOG=build_%BUILD_TYPE%.log
echo. > %LOG%


REM Select program path based on current machine environment
set progpath=%ProgramFiles%
if not "%ProgramFiles(x86)%".=="". set progpath=%ProgramFiles(x86)%


echo.
echo -= IR Server Suite =-
echo -= build mode: %BUILD_TYPE% =-
echo.

echo.
echo Writing SVN revision assemblies...
setup\DeployVersionSVN.exe /svn="%CD%"  >> %LOG%

echo.
echo Building IR Server Suite...
"%progpath%\Microsoft Visual Studio 8\Common7\IDE\devenv.com" /rebuild %BUILD_TYPE% "IR Server Suite.sln" >> %LOG%

echo.
echo Reverting assemblies...
setup\DeployVersionSVN.exe /svn="%CD%" /revert >> %LOG%

echo.
echo Building Installer...
setup\DeployVersionSVN.exe /svn="%CD%" /GetVersion >> %LOG%
IF NOT EXIST version.txt EXIT
SET /p version=<version.txt
DEL version.txt
"%progpath%\NSIS\makensis.exe" /DVER_BUILD=%version% setup\setup.nsi >> %LOG%