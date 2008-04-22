@ECHO OFF

echo.
echo -= IR Server Suite : Build Deploy Release.bat =-

echo.
echo Building IR Server Suite...
"%ProgramFiles%\Microsoft Visual Studio 8\Common7\IDE\devenv.com" /rebuild Release "IR Server Suite.sln" > build_release.log

echo.
echo Building Help file...
"%ProgramFiles%\HTML Help Workshop\hhc.exe" "Documentation\IR Server Suite.hhp" >> build_release.log

echo.
echo Building Installer...
"%ProgramFiles%\NSIS\makensis.exe" setup\setup.nsi >> build_release.log