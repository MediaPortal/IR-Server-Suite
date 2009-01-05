@ECHO OFF


REM detect if BUILD_TYPE should be release or debug
if not %1!==Debug! goto RELEASE
:DEBUG
set BUILD_TYPE=Debug
goto END_BUILD_TYPE
:RELEASE
set BUILD_TYPE=Release
:END_BUILD_TYPE


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
echo Building IR Server Suite...
"%progpath%\Microsoft Visual Studio 8\Common7\IDE\devenv.com" /rebuild %BUILD_TYPE% "IR Server Suite.sln" >> %LOG%

echo.
echo Building Help file...
"%ProgramFiles%\HTML Help Workshop\hhc.exe" "Documentation\IR Server Suite.hhp" >> %LOG%

echo.
echo Building Installer...
"%progpath%\NSIS\makensis.exe" setup\setup.nsi >> %LOG%