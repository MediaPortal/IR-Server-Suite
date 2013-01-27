@ECHO OFF

REM set paths
set GIT_ROOT=..
set MP_ROOT=..\..\MediaPortal-1

set DeployVersionGIT="%MP_ROOT%\Tools\Script & Batch tools\DeployVersionGIT\DeployVersionGIT\bin\Release\DeployVersionGIT.exe"

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
echo Removing old binaries...
RMDir /S /Q ..\bin\%BUILD_TYPE% >> %LOG%
if not %ERRORLEVEL%==0 EXIT


echo.
echo Writing GIT revision assemblies...
%DeployVersionGIT% /git="%GIT_ROOT%" /path="%GIT_ROOT%\IR Server Suite" >> %log%


echo.
echo Building IR Server...
"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBUILD.exe" /target:Rebuild /property:Configuration=%BUILD_TYPE%;Platform=x86;AllowUnsafeBlocks=true "..\IR Server Suite\IR Server.sln" >> %LOG%
if not %ERRORLEVEL%==0 EXIT

echo.
echo Building Applications...
"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBUILD.exe" /target:Rebuild /property:Configuration=%BUILD_TYPE%;Platform=x86;AllowUnsafeBlocks=true "..\IR Server Suite\Applications.sln" >> %LOG%
if not %ERRORLEVEL%==0 EXIT

if not %2!==MPplugins! goto NoMPplugins
echo.
echo Building MediaPortal plugins...
"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBUILD.exe" /target:Rebuild /property:Configuration=%BUILD_TYPE%;Platform=x86 "..\MediaPortal Plugins\MediaPortal plugins.sln" >> %LOG%
if not %ERRORLEVEL%==0 EXIT
:NoMPplugins


echo.
echo Reverting assemblies...
%DeployVersionGIT% /git="%GIT_ROOT%" /path="%GIT_ROOT%\IR Server Suite" /revert >> %log%

echo.
echo Reading the svn revision...
%DeployVersionGIT% /git="%GIT_ROOT%" /path="%GIT_ROOT%\IR Server Suite" /GetVersion >> %log%
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

