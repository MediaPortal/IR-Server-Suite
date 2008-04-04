"%ProgramFiles%\Microsoft Visual Studio 8\Common7\IDE\devenv.com" /rebuild Release "IR Server Suite.sln"
"%ProgramFiles%\HTML Help Workshop\hhc.exe" "Documentation\IR Server Suite.hhp"
"%ProgramFiles%\NSIS\makensis.exe" setup\setup.nsi
