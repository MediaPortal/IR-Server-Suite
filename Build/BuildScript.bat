@ECHO OFF


REM detect if BUILD_TYPE should be release or debug
if not %1!==Debug! goto RELEASE
:DEBUG
set BUILD_TYPE=Debug
goto START
:RELEASE
set BUILD_TYPE=Release
goto START


:START
REM Select program path based on current machine environment
set progpath=%ProgramFiles%
if not "%ProgramFiles(x86)%".=="". set progpath=%ProgramFiles(x86)%


REM set logfile where the infos are written to, and clear that file
set LOG=build_%BUILD_TYPE%.log
echo. > %LOG%


echo.
echo -= IR Server Suite =-
echo -= build mode: %BUILD_TYPE% =-
echo.

echo.
echo Writing SVN revision assemblies...
setup\DeployVersionSVN.exe /svn="%CD%"  >> %LOG%

echo.
echo Building IR Server Suite...
"%progpath%\Microsoft Visual Studio 9.0\Common7\IDE\devenv.com" /rebuild %BUILD_TYPE% "IR Server Suite.sln" >> %LOG%
rem "%WINDIR%\Microsoft.NET\Framework\v3.5\MSBUILD.exe" /target:Rebuild /property:Configuration=%BUILD_TYPE% "IR Server Suite.sln" >> %LOG%

echo.
echo Reverting assemblies...
setup\DeployVersionSVN.exe /svn="%CD%" /revert >> %LOG%

echo.
echo Reading the svn revision...
echo $WCREV$>template.txt
"%ProgramFiles%\TortoiseSVN\bin\SubWCRev.exe" "." template.txt version.txt >> %LOG%
SET /p version=<version.txt >> %LOG%
DEL template.txt >> %LOG%
DEL version.txt >> %LOG%

echo.
echo Building Installer...
"%progpath%\NSIS\makensis.exe" /DBUILD_TYPE=%BUILD_TYPE% /DVER_BUILD=%version% setup\setup.nsi >> %LOG%
