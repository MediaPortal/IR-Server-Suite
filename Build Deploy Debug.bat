rem @ECHO OFF

setup\DeployVersionSVN.exe /svn="%CD%"  > build_debug.log

"%ProgramFiles%\Microsoft Visual Studio 8\Common7\IDE\devenv.com" /rebuild Debug "IR Server Suite.sln" >> build_debug.log

setup\DeployVersionSVN.exe /svn="%CD%" /revert >> build_debug.log

rem these commands are necessary to get the svn revision, to enable them just remove the EXIT one line above
setup\DeployVersionSVN.exe /svn="%CD%" /GetVersion >> build_debug.log

IF NOT EXIST version.txt EXIT
SET /p version=<version.txt
DEL version.txt
"%ProgramFiles%\NSIS\makensis.exe" /DVER_BUILD=%version% setup\setup.nsi >> build_debug.log