ECHO parameter1=%1
CD %1
IF %2 == Release SET nodebug=/ndebug

md tmp
..\..\..\..\..\External\ILMerge.exe %nodebug% /out:tmp\TV3BlasterPlugin.dll TV3BlasterPlugin.dll MPUtils.dll IrssComms.dll IrssUtils.dll
IF NOT %errorlevel% == 0 exit

IF EXIST TV3BlasterPlugin_UNMERGED.dll del TV3BlasterPlugin_UNMERGED.dll
ren TV3BlasterPlugin.dll TV3BlasterPlugin_UNMERGED.dll
IF EXIST TV3BlasterPlugin_UNMERGED.pdb del TV3BlasterPlugin_UNMERGED.pdb
ren TV3BlasterPlugin.pdb TV3BlasterPlugin_UNMERGED.pdb

move tmp\*.* .
rd tmp
