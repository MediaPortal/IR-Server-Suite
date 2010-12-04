@ECHO OFF

REM set paths
set DeployVersionSVN="..\..\..\..\..\MediaPortal\trunk\Tools\Script & Batch tools\DeployVersionSVN\DeployVersionSVN\bin\Release\DeployVersionSVN.exe"

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
set ProgramDir=%ProgramFiles%
if not "%ProgramFiles(x86)%".=="". set ProgramDir=%ProgramFiles(x86)%


REM set logfile where the infos are written to, and clear that file
set LOG=build_%BUILD_TYPE%.log
echo. > %LOG%


echo.
echo -= IR Server Suite =-
echo -= build mode: %BUILD_TYPE% =-
echo.


echo.
echo Writing SVN revision assemblies...
%DeployVersionSVN% /svn=".."  >> %LOG%


echo.
echo Building IR Server Suite...
rem "%ProgramDir%\Microsoft Visual Studio 9.0\Common7\IDE\devenv.com" /rebuild %BUILD_TYPE% "..\IR Server Suite\IR Server Suite.sln" >> %LOG%
"%WINDIR%\Microsoft.NET\Framework\v3.5\MSBUILD.exe" /target:Rebuild /property:Configuration=%BUILD_TYPE%;Platform=x86;AllowUnsafeBlocks=true "..\IR Server Suite\IR Server Suite.sln" >> %LOG%
if not %ERRORLEVEL%==0 EXIT

if not %2!==MPplugins! goto NoMPplugins
echo.
echo Building MediaPortal plugins...
"%ProgramDir%\Microsoft Visual Studio 9.0\Common7\IDE\devenv.com" /rebuild %BUILD_TYPE% "..\MediaPortal Plugins\MediaPortal plugins.sln" >> %LOG%
rem "%WINDIR%\Microsoft.NET\Framework\v3.5\MSBUILD.exe" /target:Rebuild /property:Configuration=%BUILD_TYPE%;Platform=x86 "..\MediaPortal Plugins\MediaPortal plugins.sln" >> %LOG%
if not %ERRORLEVEL%==0 EXIT
:NoMPplugins


echo.
echo Reverting assemblies...
%DeployVersionSVN% /svn=".." /revert >> %LOG%

echo.
echo Reading the svn revision...
%DeployVersionSVN% /svn=".." /GetVersion >> %LOG%
rem SET /p version=<version.txt >> build.log
SET version=%errorlevel%
DEL version.txt >> %LOG%


echo.
if not %2!==MPplugins! goto NoMPplugins

echo Building Installer with MPplugins...
"%ProgramDir%\NSIS\makensis.exe" /DBUILD_TYPE=%BUILD_TYPE% /DVER_BUILD=%version% /DMPplugins ..\setup\setup.nsi >> %LOG%
GOTO END

:NoMPplugins
echo Building Installer...
"%ProgramDir%\NSIS\makensis.exe" /DBUILD_TYPE=%BUILD_TYPE% /DVER_BUILD=%version% ..\setup\setup.nsi >> %LOG%

:END

