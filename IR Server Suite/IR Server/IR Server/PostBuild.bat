
REM %1 = $(Solutiondir)
REM %2 = $(ConfigurationName)
REM %3 = $(TargetDir)
REM Check for Microsoft Antispyware .BAT bug
if exist .\kernel32.dll exit 1


rem
rem         Place holder for postbuild actions (usefull for debugging)
rem
