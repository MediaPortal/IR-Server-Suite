; IR Server Suite.nsi
;
; (C) Copyright Aaron Dinnage, 2007
;
; Install script for IR Server Suite
;
;--------------------------------

; The name of the installer
Name "IR Server Suite"

; The file to write
OutFile "IR Server Suite.exe"

; The default installation directory
InstallDir "$PROGRAMFILES\IR Server Suite"

; Registry key to check for directory (so if you install again, it will overwrite the old one automatically)
InstallDirRegKey HKLM "Software\IR Server Suite" "Install_Dir"

; Show the installation/uninstallation steps to the user
ShowInstDetails show
ShowUninstDetails show

; Set the compression method
SetCompressor /SOLID /FINAL lzma

!include "x64.nsh"

;--------------------------------

; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

Function .onInit

  ${If} ${RunningX64}

	${DisableX64FSRedirection}
	StrCpy '$INSTDIR' '$PROGRAMFILES\IR Server Suite'
	${EnableX64FSRedirection}

	SetRegView 64

  ${Else}

	SetRegView 32

  ${Endif}

FunctionEnd

;--------------------------------

Section "-Core"

  DetailPrint "Installing Core files ..."

  ; Use the all users context
  SetShellVarContext all

  ; Kill running Programs
  DetailPrint "Terminating processes ..."
  ExecWait '"taskkill" /F /IM "Input Service.exe"'
  ExecWait '"taskkill" /F /IM Translator.exe'
  ExecWait '"taskkill" /F /IM TrayLauncher.exe'
  ExecWait '"taskkill" /F /IM WebRemote.exe'
  ExecWait '"taskkill" /F /IM VirtualRemote.exe'
  ExecWait '"taskkill" /F /IM VirtualRemoteSkinEditor.exe'
  ExecWait '"taskkill" /F /IM DebugClient.exe'
  Sleep 100

  CreateDirectory "$APPDATA\IR Server Suite"
  CreateDirectory "$APPDATA\IR Server Suite\Logs"
  CreateDirectory "$APPDATA\IR Server Suite\IR Commands"

  ; Copy known set top boxes
  CreateDirectory "$APPDATA\IR Server Suite\Set Top Boxes"
  SetOutPath "$APPDATA\IR Server Suite\Set Top Boxes"
  SetOverwrite ifnewer
  File /r /x .svn "Set Top Boxes\*.*"

  ; Set output path to install dir
  SetOutPath "$INSTDIR"

  ; Write documentation
  SetOverwrite ifnewer
  File "Documentation\IR Server Suite.chm"

  ; Write the uninstaller
  WriteUninstaller "Uninstall IR Server Suite.exe"

  ; Create a shortcut to the uninstaller & documentation
  CreateDirectory "$SMPROGRAMS\IR Server Suite"
  CreateShortCut "$SMPROGRAMS\IR Server Suite\Log Files.lnk" "$APPDATA\IR Server Suite\Logs" "" "" 0
  CreateShortCut "$SMPROGRAMS\IR Server Suite\Uninstall.lnk" "$INSTDIR\Uninstall IR Server Suite.exe" "" "$INSTDIR\Uninstall IR Server Suite.exe" 0
  CreateShortCut "$SMPROGRAMS\IR Server Suite\Documentation.lnk" "$INSTDIR\IR Server Suite.chm" "" "" 0
  
  ; Write the installation path into the registry
  WriteRegStr HKLM "Software\IR Server Suite" "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\IR Server Suite" "DisplayName" "IR Server Suite"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\IR Server Suite" "UninstallString" '"$INSTDIR\Uninstall IR Server Suite.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\IR Server Suite" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\IR Server Suite" "NoRepair" 1
    
SectionEnd

;--------------------------------

Section "Input Service"

  DetailPrint "Installing Input Service ..."

  ; Use the all users context
  SetShellVarContext all

  ; Uninstall current Input Service ...
  Exec '"$INSTDIR\Input Service\Input Service.exe" /uninstall'
  Sleep 100

  ; Installing Input Service
  CreateDirectory "$INSTDIR\Input Service"
  SetOutPath "$INSTDIR\Input Service"
  SetOverwrite ifnewer
  File "Input Service\Input Service\bin\Debug\*.*"

  ; Installing Input Service Configuration
  CreateDirectory "$INSTDIR\Input Service Configuration"
  SetOutPath "$INSTDIR\Input Service Configuration"
  SetOverwrite ifnewer
  File "Input Service\Input Service Configuration\bin\Debug\*.*"

  ; Install IR Server Plugins ...
  DetailPrint "Installing IR Server Plugins ..."
  CreateDirectory "$INSTDIR\IR Server Plugins"
  SetOutPath "$INSTDIR\IR Server Plugins"
  SetOverwrite ifnewer
  
  File "IR Server Plugins\Custom HID Receiver\bin\Debug\*.*"
  File "IR Server Plugins\FusionRemote Receiver\bin\Debug\*.*"
  File "IR Server Plugins\Girder Plugin\bin\Debug\*.*"
  ;File "IR Server Plugins\HCW Transceiver\bin\Debug\*.*"
  File "IR Server Plugins\IgorPlug Receiver\bin\Debug\*.*"
  File "IR Server Plugins\IRMan Receiver\bin\Debug\*.*"
  File "IR Server Plugins\IRTrans Transceiver\bin\Debug\*.*"
  File "IR Server Plugins\Microsoft MCE Transceiver\bin\Debug\*.*"
  File "IR Server Plugins\RedEye Blaster\bin\Debug\*.*"
  File "IR Server Plugins\Serial IR Blaster\bin\Debug\*.*"
  File "IR Server Plugins\USB-UIRT Transceiver\bin\Debug\*.*"
  File "IR Server Plugins\Wii Remote Receiver\bin\Debug\*.*"
  File "IR Server Plugins\WiimoteLib\bin\Debug\*.*"
  File "IR Server Plugins\Windows Message Receiver\bin\Debug\*.*"
  File "IR Server Plugins\WinLirc Transceiver\bin\Debug\*.*"
  File "IR Server Plugins\X10 Transceiver\bin\Debug\*.*"
  File "IR Server Plugins\XBCDRC Receiver\bin\Debug\*.*"

  ; Create App Data Folder for IR Server configuration files.
  CreateDirectory "$APPDATA\IR Server Suite\Input Service"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\IR Server Suite\Input Service Configuration.lnk" "$INSTDIR\Input Service Configuration\Input Service Configuration.exe" "" "$INSTDIR\Input Service Configuration\Input Service Configuration.exe" 0

  ; Launch Input Service
  Exec '"$INSTDIR\Input Service\Input Service.exe" /install'

SectionEnd

;--------------------------------

Section "MP Control Plugin"

  DetailPrint "Installing MP Control Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Process"
  SetOverwrite ifnewer
  File "MediaPortal Plugins\MP Control Plugin\bin\Debug\MPUtils.dll"
  File "MediaPortal Plugins\MP Control Plugin\bin\Debug\IrssComms.dll"
  File "MediaPortal Plugins\MP Control Plugin\bin\Debug\IrssUtils.dll"
  File "MediaPortal Plugins\MP Control Plugin\bin\Debug\MPControlPlugin.dll"

  ; Write input mapping
  SetOutPath "$PROGRAMFILES\Team MediaPortal\MediaPortal\InputDeviceMappings\defaults"
  SetOverwrite ifnewer
  File "MediaPortal Plugins\MP Control Plugin\InputMapping\MPControlPlugin.xml"

  ; Write app data
  CreateDirectory "$APPDATA\IR Server Suite\MP Control Plugin"
  SetOutPath "$APPDATA\IR Server Suite\MP Control Plugin"
  SetOverwrite ifnewer
  File /r /x .svn "MediaPortal Plugins\MP Control Plugin\AppData\*.*"

  ; Create Macro folder
  CreateDirectory "$APPDATA\IR Server Suite\MP Control Plugin\Macro"

SectionEnd

;--------------------------------

Section "MP Blast Zone Plugin"

  DetailPrint "Installing MP Blast Zone Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Windows"
  SetOverwrite ifnewer
  File "MediaPortal Plugins\MP Blast Zone Plugin\bin\Debug\MPUtils.dll"
  File "MediaPortal Plugins\MP Blast Zone Plugin\bin\Debug\IrssComms.dll"
  File "MediaPortal Plugins\MP Blast Zone Plugin\bin\Debug\IrssUtils.dll"
  File "MediaPortal Plugins\MP Blast Zone Plugin\bin\Debug\MPBlastZonePlugin.dll"

  ; Write app data
  CreateDirectory "$APPDATA\IR Server Suite\MP Blast Zone Plugin"
  SetOutPath "$APPDATA\IR Server Suite\MP Blast Zone Plugin"
  SetOverwrite off
  File "MediaPortal Plugins\MP Blast Zone Plugin\AppData\Menu.xml"

  ; Write skin files
  SetOutPath "$PROGRAMFILES\Team MediaPortal\MediaPortal\Skin\BlueTwo"
  SetOverwrite on
  File /r /x .svn "MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  SetOutPath "$PROGRAMFILES\Team MediaPortal\MediaPortal\Skin\BlueTwo wide"
  SetOverwrite on
  File /r /x .svn "MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  ; Create Macro folder
  CreateDirectory "$APPDATA\IR Server Suite\MP Blast Zone Plugin\Macro"

SectionEnd

;--------------------------------

Section /o "TV2 Blaster Plugin"

  DetailPrint "Installing TV2 Blaster Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Process"
  SetOverwrite ifnewer
  File "MediaPortal Plugins\TV2 Blaster Plugin\bin\Debug\MPUtils.dll"
  File "MediaPortal Plugins\TV2 Blaster Plugin\bin\Debug\IrssComms.dll"
  File "MediaPortal Plugins\TV2 Blaster Plugin\bin\Debug\IrssUtils.dll"
  File "MediaPortal Plugins\TV2 Blaster Plugin\bin\Debug\TV2BlasterPlugin.dll"

  ; Create folders
  CreateDirectory "$APPDATA\IR Server Suite\TV2 Blaster Plugin"
  CreateDirectory "$APPDATA\IR Server Suite\TV2 Blaster Plugin\Macro"

SectionEnd

;--------------------------------

Section /o "TV3 Blaster Plugin"

  DetailPrint "Installing TV3 Blaster Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$PROGRAMFILES\Team MediaPortal\MediaPortal TV Server\Plugins"
  SetOverwrite ifnewer
  File "MediaPortal Plugins\TV3 Blaster Plugin\bin\Debug\MPUtils.dll"
  File "MediaPortal Plugins\TV3 Blaster Plugin\bin\Debug\IrssComms.dll"
  File "MediaPortal Plugins\TV3 Blaster Plugin\bin\Debug\IrssUtils.dll"
  File "MediaPortal Plugins\TV3 Blaster Plugin\bin\Debug\TV3BlasterPlugin.dll"

  ; Create folders
  CreateDirectory "$APPDATA\IR Server Suite\TV3 Blaster Plugin"
  CreateDirectory "$APPDATA\IR Server Suite\TV3 Blaster Plugin\Macro"

SectionEnd

;--------------------------------

Section "Translator"

  DetailPrint "Installing Translator ..."

  ; Installing Translator
  CreateDirectory "$INSTDIR\Translator"
  SetOutPath "$INSTDIR\Translator"
  SetOverwrite ifnewer
  File "Applications\Translator\bin\Debug\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\IR Server Suite\Translator"
  CreateDirectory "$APPDATA\IR Server Suite\Translator\Macro"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\IR Server Suite\Translator.lnk" "$INSTDIR\Translator\Translator.exe" "" "$INSTDIR\Translator\Translator.exe" 0

SectionEnd

;--------------------------------

Section /o "Tray Launcher"

  DetailPrint "Installing Tray Launcher ..."

  ; Installing Translator
  CreateDirectory "$INSTDIR\Tray Launcher"
  SetOutPath "$INSTDIR\Tray Launcher"
  SetOverwrite ifnewer
  File "Applications\Tray Launcher\bin\Debug\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\IR Server Suite\Tray Launcher"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\IR Server Suite\Tray Launcher.lnk" "$INSTDIR\Tray Launcher\TrayLauncher.exe" "" "$INSTDIR\Tray Launcher\TrayLauncher.exe" 0

SectionEnd

;--------------------------------

Section "Virtual Remote and Web Remote"

  DetailPrint "Installing Virtual Remote and Web Remote..."

  ; Installing Virtual Remote and Web Remote
  CreateDirectory "$INSTDIR\Virtual Remote"
  SetOutPath "$INSTDIR\Virtual Remote"
  SetOverwrite ifnewer
  File "Applications\Virtual Remote\bin\Debug\*.*"
  File "Applications\Web Remote\bin\Debug\WebRemote.exe"

  CreateDirectory "$INSTDIR\Virtual Remote\Skins"
  SetOutPath "$INSTDIR\Virtual Remote\Skins"
  SetOverwrite ifnewer
  File "Applications\Virtual Remote\Skins\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\IR Server Suite\Virtual Remote"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\IR Server Suite\Virtual Remote.lnk" "$INSTDIR\Virtual Remote\VirtualRemote.exe" "" "$INSTDIR\Virtual Remote\VirtualRemote.exe" 0
  CreateShortCut "$SMPROGRAMS\IR Server Suite\Web Remote.lnk" "$INSTDIR\Virtual Remote\WebRemote.exe" "" "$INSTDIR\Virtual Remote\WebRemote.exe" 0

SectionEnd

;--------------------------------

Section /o "Virtual Remote Skin Editor"

  DetailPrint "Installing Virtual Remote Skin Editor ..."

  ; Installing Virtual Remote
  CreateDirectory "$INSTDIR\Virtual Remote Skin Editor"
  SetOutPath "$INSTDIR\Virtual Remote Skin Editor"
  SetOverwrite ifnewer
  File "Applications\Virtual Remote Skin Editor\bin\Debug\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\IR Server Suite\Virtual Remote Skin Editor"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\IR Server Suite\Virtual Remote Skin Editor.lnk" "$INSTDIR\Virtual Remote Skin Editor\VirtualRemoteSkinEditor.exe" "" "$INSTDIR\Virtual Remote Skin Editor\VirtualRemoteSkinEditor.exe" 0

SectionEnd

;--------------------------------

Section "IR Blast (Command line tool)"

  DetailPrint "Installing IR Blast ..."

  ; Installing IR Server
  CreateDirectory "$INSTDIR\IR Blast"
  SetOutPath "$INSTDIR\IR Blast"
  SetOverwrite ifnewer
  File "Applications\IR Blast\bin\Debug\IRBlast.exe"
  File "Applications\IR Blast (No Window)\bin\Debug\*.*"

SectionEnd

;--------------------------------

Section /o "Debug Client"

  DetailPrint "Installing Debug Client ..."

  ; Installing Debug Client
  CreateDirectory "$INSTDIR\Debug Client"
  SetOutPath "$INSTDIR\Debug Client"
  SetOverwrite ifnewer
  File "Applications\Debug Client\bin\Debug\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\IR Server Suite\Debug Client"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\IR Server Suite\Debug Client.lnk" "$INSTDIR\Debug Client\DebugClient.exe" "" "$INSTDIR\Debug Client\DebugClient.exe" 0

SectionEnd

;--------------------------------
;--------------------------------

Section "Uninstall"

  ; Use the all users context
  SetShellVarContext all

  ; Uninstall current Input Service ...
  Exec '"$INSTDIR\Input Service\Input Service.exe" /uninstall'
  Sleep 100

  ; Kill running Programs
  DetailPrint "Terminating processes ..."
  ExecWait '"taskkill" /F /IM "Input Service.exe"'
  ExecWait '"taskkill" /F /IM Translator.exe'
  ExecWait '"taskkill" /F /IM TrayLauncher.exe'
  ExecWait '"taskkill" /F /IM WebRemote.exe'
  ExecWait '"taskkill" /F /IM VirtualRemote.exe'
  ExecWait '"taskkill" /F /IM VirtualRemoteSkinEditor.exe'
  ExecWait '"taskkill" /F /IM DebugClient.exe'
  Sleep 100

  ; Remove files and uninstaller
  DetailPrint "Attempting to remove MediaPortal Blast Zone Plugin ..."
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Windows\MPUtils.dll"
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Windows\IrssComms.dll"
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Windows\IrssUtils.dll"
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Windows\MPBlastZonePlugin.dll"

  DetailPrint "Attempting to remove MediaPortal Process Plugin Common Files ..."
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Process\MPUtils.dll"
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Process\IrssComms.dll"
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Process\IrssUtils.dll"

  DetailPrint "Attempting to remove MediaPortal Control Plugin ..."
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Process\MPControlPlugin.dll"

  DetailPrint "Attempting to remove MediaPortal TV2 Plugin ..."
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal\Plugins\Process\TV2BlasterPlugin.dll"

  DetailPrint "Attempting to remove MediaPortal TV3 Plugin ..."
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal TV Server\Plugins\MPUtils.dll"
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal TV Server\Plugins\IrssComms.dll"
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal TV Server\Plugins\IrssUtils.dll"
  Delete /REBOOTOK "$PROGRAMFILES\Team MediaPortal\MediaPortal TV Server\Plugins\TV3BlasterPlugin.dll"

  DetailPrint "Removing Set Top Box presets ..."
  RMDir /R "$APPDATA\IR Server Suite\Set Top Boxes"

  DetailPrint "Removing program files ..."
  RMDir /R /REBOOTOK "$INSTDIR"
  
  DetailPrint "Removing start menu shortcuts ..."
  RMDir /R "$SMPROGRAMS\IR Server Suite"
  
  ; Remove registry keys
  DetailPrint "Removing registry keys ..."
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\IR Server Suite"
  DeleteRegKey HKLM "SOFTWARE\IR Server Suite"
  
  ; Remove auto-runs
  DetailPrint "Removing application auto-runs ..."
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Tray Launcher"
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Translator"

SectionEnd

;--------------------------------
