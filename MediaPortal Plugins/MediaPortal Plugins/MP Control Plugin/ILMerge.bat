ECHO parameter1=%1
CD %1
IF %2 == Release SET nodebug=/ndebug

md tmp
..\..\..\..\..\External\ILMerge.exe %nodebug% /out:tmp\MPControlPlugin.dll MPControlPlugin.dll MPUtils.dll IrssComms.dll IrssUtils.dll
IF NOT %errorlevel% == 0 exit

IF EXIST MPControlPlugin_UNMERGED.dll del MPControlPlugin_UNMERGED.dll
ren MPControlPlugin.dll MPControlPlugin_UNMERGED.dll
IF EXIST MPControlPlugin_UNMERGED.pdb del MPControlPlugin_UNMERGED.pdb
ren MPControlPlugin.pdb MPControlPlugin_UNMERGED.pdb

move tmp\*.* .
rd tmp
