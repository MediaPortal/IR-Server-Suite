@ECHO OFF

REM set paths
set GIT_ROOT=..
set MP_ROOT=..\..\MediaPortal-1

set DeployVersionGIT="%GIT_ROOT%\External\DeployVersionGIT.exe"

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
echo %ERRORLEVEL%
if %ERRORLEVEL% GTR 1 GOTO END


echo.
echo Writing GIT revision assemblies...
%DeployVersionGIT% /git="%GIT_ROOT%" /path="%GIT_ROOT%\IR Server Suite" >> %log%

echo.
echo Copying BuildReport resources...
xcopy /I /Y .\BuildReport\_BuildReport_Files .\_BuildReport_Files >> %log%

echo.
echo Building IR Server Suite...

set xml=Build_Report_%BUILD_TYPE%_IRServer.xml
set html=Build_Report_%BUILD_TYPE%_IRServer.html
set logger=/l:XmlFileLogger,"BuildReport\MSBuild.ExtensionPack.Loggers.dll";logfile=%xml%

"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBUILD.exe" %logger% /target:Rebuild /property:Configuration=%BUILD_TYPE%;Platform=x86;AllowUnsafeBlocks=true "..\IR Server Suite\IR Server Suite.sln"
BuildReport\msxsl %xml% _BuildReport_Files\BuildReport.xslt -o %html%

if not %2!==MPplugins! goto NoMPplugins
echo.
echo Building MediaPortal plugins...
RmDir "..\IR Server Suite\Common\IrssCommands\obj" /s /q
RmDir "..\IR Server Suite\Common\IrssComms\obj" /s /q
RmDir "..\IR Server Suite\Common\IrssUtils\obj" /s /q

set xml=Build_Report_%BUILD_TYPE%_MP1plugins.xml
set html=Build_Report_%BUILD_TYPE%_MP1plugins.html
set logger=/l:XmlFileLogger,"BuildReport\MSBuild.ExtensionPack.Loggers.dll";logfile=%xml%

"%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBUILD.exe" %logger% /target:Rebuild /property:Configuration=%BUILD_TYPE%;Platform=x86 "..\MediaPortal Plugins\MediaPortal plugins.sln"
BuildReport\msxsl %xml% _BuildReport_Files\BuildReport.xslt -o %html%

:NoMPplugins


echo.
echo Reverting assemblies...
%DeployVersionGIT% /git="%GIT_ROOT%" /path="%GIT_ROOT%\IR Server Suite" /revert >> %log%

echo.
echo Reading the git revision...
%DeployVersionGIT% /git="%GIT_ROOT%" /path="%GIT_ROOT%\IR Server Suite" /GetVersion >> %log%
rem SET /p version=<version.txt >> build.log
SET version=%errorlevel%
DEL version.txt >> %LOG%
SET version=91

echo.
if not %2!==MPplugins! goto NoMPplugins

echo Building Installer with MPplugins...
"%ProgramDir%\NSIS\makensis.exe" /DBUILD_TYPE=%BUILD_TYPE% /DVER_BUILD=%version% /DMPplugins ..\setup\setup.nsi >> %LOG%
GOTO END

:NoMPplugins
echo Building Installer...
"%ProgramDir%\NSIS\makensis.exe" /DBUILD_TYPE=%BUILD_TYPE% /DVER_BUILD=%version% ..\setup\setup.nsi >> %LOG%

:END

