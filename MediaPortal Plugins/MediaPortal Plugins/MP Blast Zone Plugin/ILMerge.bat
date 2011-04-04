ECHO parameter1=%1
CD %1
IF %2 == Release SET nodebug=/ndebug

md tmp
..\..\..\..\..\External\ILMerge.exe %nodebug% /out:tmp\MPBlastZonePlugin.dll MPBlastZonePlugin.dll MPUtils.dll IrssComms.dll IrssUtils.dll
IF NOT %errorlevel% == 0 exit

IF EXIST MPBlastZonePlugin_UNMERGED.dll del MPBlastZonePlugin_UNMERGED.dll
ren MPBlastZonePlugin.dll MPBlastZonePlugin_UNMERGED.dll
IF EXIST MPBlastZonePlugin_UNMERGED.pdb del MPBlastZonePlugin_UNMERGED.pdb
ren MPBlastZonePlugin.pdb MPBlastZonePlugin_UNMERGED.pdb

move tmp\*.* .
rd tmp
