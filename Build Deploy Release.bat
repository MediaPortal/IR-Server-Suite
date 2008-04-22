"%ProgramFiles%\Microsoft Visual Studio 8\Common7\IDE\devenv.com" /rebuild Release "IR Server Suite.sln" > build_release.log
"%ProgramFiles%\HTML Help Workshop\hhc.exe" "Documentation\IR Server Suite.hhp" >> build_release.log
"%ProgramFiles%\NSIS\makensis.exe" setup\setup.nsi >> build_release.log