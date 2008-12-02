@ECHO OFF

REM Select program path based on current machine environment

set progpath=%ProgramFiles%
if not "%ProgramFiles(x86)%".=="". set progpath=%ProgramFiles(x86)%

echo.
echo -= IR Server Suite : Build Deploy Release.bat =-

echo.
echo Building IR Server Suite...
"%progpath%\Microsoft Visual Studio 8\Common7\IDE\devenv.com" /rebuild Release "IR Server Suite.sln" > build_release.log

echo.
echo Building Help file...
"%ProgramFiles%\HTML Help Workshop\hhc.exe" "Documentation\IR Server Suite.hhp" >> build_release.log

echo.
echo Building Installer...
"%progpath%\NSIS\makensis.exe" setup\setup.nsi >> build_release.log