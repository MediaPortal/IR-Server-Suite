;======================================
; IR Server Suite.nsi
;
; (C) Copyright Aaron Dinnage, 2008
;======================================
!define DEBUG

!ifdef DEBUG
    !define BuildType "Debug"
!else
    !define BuildType "Release"
!endif

!include "x64.nsh"
!include "MUI.nsh"

!define PRODUCT_NAME "IR Server Suite"
!define PRODUCT_VERSION "1.0.4.2"
!define PRODUCT_PUBLISHER "and-81"
!define PRODUCT_WEB_SITE "http://forum.team-mediaportal.com/mce_replacement_plugin-f165.html"

Name "${PRODUCT_NAME}"
OutFile "${PRODUCT_NAME} - ${PRODUCT_VERSION}.exe"
InstallDir ""
!ifdef DEBUG
    ShowInstDetails show
    ShowUninstDetails show
!else
    ShowInstDetails hide
    ShowUninstDetails hide
!endif
BrandingText "${PRODUCT_NAME} by Aaron Dinnage"
SetCompressor /SOLID /FINAL lzma
CRCCheck On

; Variables
var DIR_INSTALL
var DIR_MEDIAPORTAL
var DIR_TVSERVER

!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\win-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\win-uninstall.ico"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "Documentation\LICENSE.GPL"
!insertmacro MUI_PAGE_COMPONENTS

; Main app install path
!define MUI_PAGE_CUSTOMFUNCTION_SHOW DirectoryShowApp
!define MUI_PAGE_CUSTOMFUNCTION_LEAVE DirectoryLeaveApp
!insertmacro MUI_PAGE_DIRECTORY

; MediaPortal install path
!define MUI_PAGE_CUSTOMFUNCTION_PRE DirectoryPreMP
!define MUI_PAGE_CUSTOMFUNCTION_SHOW DirectoryShowMP
!define MUI_PAGE_CUSTOMFUNCTION_LEAVE DirectoryLeaveMP
!insertmacro MUI_PAGE_DIRECTORY

; TV Server install path
!define MUI_PAGE_CUSTOMFUNCTION_PRE DirectoryPreTV
!define MUI_PAGE_CUSTOMFUNCTION_SHOW DirectoryShowTV
!define MUI_PAGE_CUSTOMFUNCTION_LEAVE DirectoryLeaveTV
!insertmacro MUI_PAGE_DIRECTORY

!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English"

;======================================
;======================================

!macro initRegKeys
  ${If} ${RunningX64}

    SetRegView 64

    ${DisableX64FSRedirection}

      ; Get IR Server Suite installation directory ...
      ReadRegStr $DIR_INSTALL HKLM "Software\${PRODUCT_NAME}" "Install_Dir"
      ${If} $DIR_INSTALL == ""
        StrCpy '$DIR_INSTALL' '$PROGRAMFILES\${PRODUCT_NAME}'
      ${Endif}

      ; Get MediaPortal installation directory ...
      ReadRegStr $DIR_MEDIAPORTAL HKLM "Software\${PRODUCT_NAME}" "MediaPortal_Dir"
      ${If} $DIR_MEDIAPORTAL == ""

        ReadRegStr $DIR_MEDIAPORTAL HKLM "Software\Team MediaPortal\MediaPortal" "ApplicationDir"

        ${If} $DIR_MEDIAPORTAL == ""
          StrCpy '$DIR_MEDIAPORTAL' '$PROGRAMFILES\Team MediaPortal\MediaPortal'
        ${Endif}

      ${Endif}

      ; Get MediaPortal TV Server installation directory ...
      ReadRegStr $DIR_TVSERVER HKLM "Software\${PRODUCT_NAME}" "TVServer_Dir"
      ${If} $DIR_TVSERVER == ""

        ReadRegStr $DIR_TVSERVER HKLM "Software\Team MediaPortal\MediaPortal TV Server" "InstallPath"

        ${If} $DIR_TVSERVER == ""
          StrCpy '$DIR_TVSERVER' '$PROGRAMFILES\Team MediaPortal\MediaPortal TV Server'
        ${Endif}

      ${Endif}

      ${EnableX64FSRedirection}

  ${Else}

    SetRegView 32

    ; Get IR Server Suite installation directory ...
    ReadRegStr $DIR_INSTALL HKLM "Software\${PRODUCT_NAME}" "Install_Dir"
    ${If} $DIR_INSTALL == ""
      StrCpy '$DIR_INSTALL' '$PROGRAMFILES\${PRODUCT_NAME}'
    ${Endif}

    ; Get MediaPortal installation directory ...
    ReadRegStr $DIR_MEDIAPORTAL HKLM "Software\${PRODUCT_NAME}" "MediaPortal_Dir"
    ${If} $DIR_MEDIAPORTAL == ""

      ReadRegStr $DIR_MEDIAPORTAL HKLM "Software\Team MediaPortal\MediaPortal" "ApplicationDir"

      ${If} $DIR_MEDIAPORTAL == ""
        StrCpy '$DIR_MEDIAPORTAL' '$PROGRAMFILES\Team MediaPortal\MediaPortal'
      ${Endif}

    ${Endif}

    ; Get MediaPortal TV Server installation directory ...
    ReadRegStr $DIR_TVSERVER HKLM "Software\${PRODUCT_NAME}" "TVServer_Dir"
    ${If} $DIR_TVSERVER == ""

      ReadRegStr $DIR_TVSERVER HKLM "Software\Team MediaPortal\MediaPortal TV Server" "InstallPath"

      ${If} $DIR_TVSERVER == ""
        StrCpy '$DIR_TVSERVER' '$PROGRAMFILES\Team MediaPortal\MediaPortal TV Server'
      ${Endif}

    ${Endif}

  ${Endif}
!macroend
 
;======================================
;======================================

Function .onInit

!insertmacro initRegKeys

FunctionEnd

;======================================

Function .onInstSuccess

  IfFileExists "$DIR_INSTALL\Input Service\Input Service.exe" StartInputService SkipStartInputService

StartInputService:
  Exec '"$DIR_INSTALL\Input Service\Input Service.exe" /start'

SkipStartInputService:

FunctionEnd

;======================================

Function DirectoryPreMP
  SectionGetFlags 3 $R0
  IntOp $R0 $R0 & ${SF_SELECTED}
  IntCmp $R0 ${SF_SELECTED} EndDirectoryPreMP

  SectionGetFlags 4 $R0
  IntOp $R0 $R0 & ${SF_SELECTED}
  IntCmp $R0 ${SF_SELECTED} EndDirectoryPreMP

  SectionGetFlags 5 $R0
  IntOp $R0 $R0 & ${SF_SELECTED}
  IntCmp $R0 ${SF_SELECTED} EndDirectoryPreMP

  Abort

EndDirectoryPreMP:
FunctionEnd

;======================================

Function DirectoryPreTV
  SectionGetFlags 6 $R0
  IntOp $R0 $R0 & ${SF_SELECTED}
  IntCmp $R0 ${SF_SELECTED} EndDirectoryPreTV

  Abort

EndDirectoryPreTV:
FunctionEnd

;======================================

Function DirectoryShowApp
  !insertmacro MUI_HEADER_TEXT "Choose ${PRODUCT_NAME} Location" "Choose the folder in which to install ${PRODUCT_NAME}."
  !insertmacro MUI_INNERDIALOG_TEXT 1041 "${PRODUCT_NAME} Folder"
  !insertmacro MUI_INNERDIALOG_TEXT 1019 "$DIR_INSTALL"
  !insertmacro MUI_INNERDIALOG_TEXT 1006 "Setup will install ${PRODUCT_NAME} in the following folder.$\r$\n$\r$\nTo install in a different folder, click Browse and select another folder. Click Next to continue."
FunctionEnd

;======================================

Function DirectoryShowMP
  !insertmacro MUI_HEADER_TEXT "Choose MediaPortal Location" "Choose the folder in which to install MediaPortal plugins."
  !insertmacro MUI_INNERDIALOG_TEXT 1041 "MediaPortal Folder"
  !insertmacro MUI_INNERDIALOG_TEXT 1019 "$DIR_MEDIAPORTAL"
  !insertmacro MUI_INNERDIALOG_TEXT 1006 "Setup will install MediaPortal plugins in the following folder.$\r$\n$\r$\nTo install in a different folder, click Browse and select another folder. Click Install to start the installation."
FunctionEnd

;======================================

Function DirectoryShowTV
  !insertmacro MUI_HEADER_TEXT "Choose TV Server Location" "Choose the folder in which to install TV Server plugins."
  !insertmacro MUI_INNERDIALOG_TEXT 1041 "TV Server Folder"
  !insertmacro MUI_INNERDIALOG_TEXT 1019 "$DIR_TVSERVER"
  !insertmacro MUI_INNERDIALOG_TEXT 1006 "Setup will install TV Server plugins in the following folder.$\r$\n$\r$\nTo install in a different folder, click Browse and select another folder. Click Install to start the installation."
FunctionEnd

;======================================

Function DirectoryLeaveApp
  StrCpy $DIR_INSTALL $INSTDIR
FunctionEnd

;======================================

Function DirectoryLeaveMP
  StrCpy $DIR_MEDIAPORTAL $INSTDIR
FunctionEnd

;======================================

Function DirectoryLeaveTV
  StrCpy $DIR_TVSERVER $INSTDIR
FunctionEnd

;======================================

!define LVM_GETITEMCOUNT 0x1004
!define LVM_GETITEMTEXT 0x102D
 
Function DumpLog
  Exch $5
  Push $0
  Push $1
  Push $2
  Push $3
  Push $4
  Push $6
 
  FindWindow $0 "#32770" "" $HWNDPARENT
  GetDlgItem $0 $0 1016
  StrCmp $0 0 exit
  FileOpen $5 $5 "w"
  StrCmp $5 "" exit
    SendMessage $0 ${LVM_GETITEMCOUNT} 0 0 $6
    System::Alloc ${NSIS_MAX_STRLEN}
    Pop $3
    StrCpy $2 0
    System::Call "*(i, i, i, i, i, i, i, i, i) i \
      (0, 0, 0, 0, 0, r3, ${NSIS_MAX_STRLEN}) .r1"
    loop: StrCmp $2 $6 done
      System::Call "User32::SendMessageA(i, i, i, i) i \
        ($0, ${LVM_GETITEMTEXT}, $2, r1)"
      System::Call "*$3(&t${NSIS_MAX_STRLEN} .r4)"
      FileWrite $5 "$4$\r$\n"
      IntOp $2 $2 + 1
      Goto loop
    done:
      FileClose $5
      System::Free $1
      System::Free $3
  exit:
    Pop $6
    Pop $4
    Pop $3
    Pop $2
    Pop $1
    Pop $0
    Exch $5
FunctionEnd

;======================================
;======================================

Section "-Prepare"

  DetailPrint "Preparing to install ..."

  ; Use the all users context
  SetShellVarContext all

  ; Kill running Programs
  DetailPrint "Terminating processes ..."
  ExecWait '"taskkill" /F /IM Translator.exe'
  ExecWait '"taskkill" /F /IM TrayLauncher.exe'
  ExecWait '"taskkill" /F /IM WebRemote.exe'
  ExecWait '"taskkill" /F /IM VirtualRemote.exe'
  ExecWait '"taskkill" /F /IM VirtualRemoteSkinEditor.exe'
  ExecWait '"taskkill" /F /IM IRFileTool.exe'
  ExecWait '"taskkill" /F /IM DebugClient.exe'
  ExecWait '"taskkill" /F /IM KeyboardInputRelay.exe'

  IfFileExists "$DIR_INSTALL\Input Service\Input Service.exe" StopInputService SkipStopInputService

StopInputService:
  ExecWait '"$DIR_INSTALL\Input Service\Input Service.exe" /stop'

SkipStopInputService:
  Sleep 100

SectionEnd

;======================================

Section "-Core"

  DetailPrint "Setting up paths and installing core files ..."

  ; Use the all users context
  SetShellVarContext all

  ; Create install directory
  CreateDirectory "$DIR_INSTALL"

  ; Write the installation paths into the registry
  WriteRegStr HKLM "Software\${PRODUCT_NAME}" "Install_Dir" "$DIR_INSTALL"
  WriteRegStr HKLM "Software\${PRODUCT_NAME}" "MediaPortal_Dir" "$DIR_MEDIAPORTAL"
  WriteRegStr HKLM "Software\${PRODUCT_NAME}" "TVServer_Dir" "$DIR_TVSERVER"

  ; Write documentation
!ifdef DEBUG
  DetailPrint "Warning: Documentation is not included in debug builds"
!else
  SetOutPath "$DIR_INSTALL"
  SetOverwrite ifnewer
  File "Documentation\${PRODUCT_NAME}.chm"
!endif

  ; Create app data directories
  CreateDirectory "$APPDATA\${PRODUCT_NAME}"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Logs"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\IR Commands"

  ; Copy known set top boxes
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Set Top Boxes"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Set Top Boxes"
  SetOverwrite ifnewer
  File /r /x .svn "Set Top Boxes\*.*"

  ; Create a start menu shortcut folder
  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"

SectionEnd

;======================================

Section "Input Service" SectionInputService

  DetailPrint "Installing Input Service ..."

  ; Use the all users context
  SetShellVarContext all

  ; Uninstall current Input Service ...
  IfFileExists "$DIR_INSTALL\Input Service\Input Service.exe" UninstallInputService SkipUninstallInputService

UninstallInputService:
  ExecWait '"$DIR_INSTALL\Input Service\Input Service.exe" /uninstall'

SkipUninstallInputService:
  Sleep 100

  ; Installing Input Service
  CreateDirectory "$DIR_INSTALL\Input Service"
  SetOutPath "$DIR_INSTALL\Input Service"
  SetOverwrite ifnewer
  File "Input Service\Input Service\bin\${BuildType}\*.*"

  ; Installing Input Service Configuration
  CreateDirectory "$DIR_INSTALL\Input Service Configuration"
  SetOutPath "$DIR_INSTALL\Input Service Configuration"
  SetOverwrite ifnewer
  File "Input Service\Input Service Configuration\bin\${BuildType}\*.*"

  ; Install IR Server Plugins ...
  DetailPrint "Installing IR Server Plugins ..."
  CreateDirectory "$DIR_INSTALL\IR Server Plugins"
  SetOutPath "$DIR_INSTALL\IR Server Plugins"
  SetOverwrite ifnewer

  File "IR Server Plugins\Ads Tech PTV-335 Receiver\bin\${BuildType}\Ads Tech PTV-335 Receiver.*"
  File "IR Server Plugins\CoolCommand Receiver\bin\${BuildType}\CoolCommand Receiver.*"
  File "IR Server Plugins\Custom HID Receiver\bin\${BuildType}\Custom HID Receiver.*"
  File "IR Server Plugins\Direct Input Receiver\bin\${BuildType}\Direct Input Receiver.*"
  File "IR Server Plugins\Direct Input Receiver\bin\${BuildType}\Microsoft.DirectX.DirectInput.dll"
  File "IR Server Plugins\Direct Input Receiver\bin\${BuildType}\Microsoft.DirectX.dll"
  File "IR Server Plugins\FusionRemote Receiver\bin\${BuildType}\FusionRemote Receiver.*"
  File "IR Server Plugins\Girder Plugin\bin\${BuildType}\Girder Plugin.*"
  File "IR Server Plugins\HCW Receiver\bin\${BuildType}\HCW Receiver.*"
  File "IR Server Plugins\IgorPlug Receiver\bin\${BuildType}\IgorPlug Receiver.*"
  ;File "IR Server Plugins\IR501 Receiver\bin\${BuildType}\IR501 Receiver.*"
  File "IR Server Plugins\IR507 Receiver\bin\${BuildType}\IR507 Receiver.*"
  ;File "IR Server Plugins\Ira Transceiver\bin\${BuildType}\Ira Transceiver.*"
  File "IR Server Plugins\IRMan Receiver\bin\${BuildType}\IRMan Receiver.*"
  File "IR Server Plugins\IRTrans Transceiver\bin\${BuildType}\IRTrans Transceiver.*"
  ;File "IR Server Plugins\Keyboard Input\bin\${BuildType}\Keyboard Input.*"
  File "IR Server Plugins\LiveDrive Receiver\bin\${BuildType}\LiveDrive Receiver.*"
  File "IR Server Plugins\MacMini Receiver\bin\${BuildType}\MacMini Receiver.*"
  File "IR Server Plugins\Microsoft MCE Transceiver\bin\${BuildType}\Microsoft MCE Transceiver.*"
  ;File "IR Server Plugins\RC102 Receiver\bin\${BuildType}\RC102 Receiver.*"
  File "IR Server Plugins\RedEye Blaster\bin\${BuildType}\RedEye Blaster.*"
  File "IR Server Plugins\Serial IR Blaster\bin\${BuildType}\Serial IR Blaster.*"
  ;File "IR Server Plugins\Speech Receiver\bin\${BuildType}\Speech Receiver.*"
  File "IR Server Plugins\Technotrend Receiver\bin\${BuildType}\Technotrend Receiver.*"
  ;File "IR Server Plugins\Tira Transceiver\bin\${BuildType}\Tira Transceiver.*"
  File "IR Server Plugins\USB-UIRT Transceiver\bin\${BuildType}\USB-UIRT Transceiver.*"
  File "IR Server Plugins\Wii Remote Receiver\bin\${BuildType}\Wii Remote Receiver.*"
  File "IR Server Plugins\WiimoteLib\bin\${BuildType}\WiimoteLib.*"
  File "IR Server Plugins\Windows Message Receiver\bin\${BuildType}\Windows Message Receiver.*"
  File "IR Server Plugins\WinLirc Transceiver\bin\${BuildType}\WinLirc Transceiver.*"
  File "IR Server Plugins\X10 Transceiver\bin\${BuildType}\X10 Transceiver.*"
  File "IR Server Plugins\X10 Transceiver\bin\${BuildType}\Interop.X10.dll"
  File "IR Server Plugins\XBCDRC Receiver\bin\${BuildType}\XBCDRC Receiver.*"

  ; Create App Data Folder for IR Server configuration files
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Input Service"
  
  ; Copy Abstract Remote maps
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Input Service\Abstract Remote Maps"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Input Service\Abstract Remote Maps"
  SetOverwrite ifnewer
  File /r /x .svn "Input Service\Input Service\Abstract Remote Maps\*.*"
  File "Input Service\Input Service\RemoteTable.xsd"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Input Service Configuration.lnk" "$DIR_INSTALL\Input Service Configuration\Input Service Configuration.exe" "" "$DIR_INSTALL\Input Service Configuration\Input Service Configuration.exe" 0

  ; Launch Input Service
  DetailPrint "Starting Input Service ..."
  ExecWait '"$DIR_INSTALL\Input Service\Input Service.exe" /install'

SectionEnd

;======================================

Section "MP Control Plugin" SectionMPControlPlugin

  DetailPrint "Installing MP Control Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$DIR_MEDIAPORTAL\Plugins\Process"
  SetOverwrite ifnewer
  File "MediaPortal Plugins\MP Control Plugin\bin\${BuildType}\MPUtils.dll"
  File "MediaPortal Plugins\MP Control Plugin\bin\${BuildType}\IrssComms.dll"
  File "MediaPortal Plugins\MP Control Plugin\bin\${BuildType}\IrssUtils.dll"
  File "MediaPortal Plugins\MP Control Plugin\bin\${BuildType}\MPControlPlugin.dll"

  ; Write input mapping
  SetOutPath "$DIR_MEDIAPORTAL\InputDeviceMappings\defaults"
  SetOverwrite ifnewer
  File "MediaPortal Plugins\MP Control Plugin\InputMapping\MPControlPlugin.xml"

  ; Write app data
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Control Plugin"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\MP Control Plugin"
  SetOverwrite ifnewer
  File /r /x .svn "MediaPortal Plugins\MP Control Plugin\AppData\*.*"

  ; Create Macro folder
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Control Plugin\Macro"

SectionEnd

;======================================

!ifdef DEBUG
Section "MP Blast Zone Plugin" SectionMPBlastZonePlugin
!else
Section /o "MP Blast Zone Plugin" SectionMPBlastZonePlugin
!endif

  DetailPrint "Installing MP Blast Zone Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$DIR_MEDIAPORTAL\Plugins\Windows"
  SetOverwrite ifnewer
  File "MediaPortal Plugins\MP Blast Zone Plugin\bin\${BuildType}\MPUtils.dll"
  File "MediaPortal Plugins\MP Blast Zone Plugin\bin\${BuildType}\IrssComms.dll"
  File "MediaPortal Plugins\MP Blast Zone Plugin\bin\${BuildType}\IrssUtils.dll"
  File "MediaPortal Plugins\MP Blast Zone Plugin\bin\${BuildType}\MPBlastZonePlugin.dll"

  ; Write app data
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin"
  SetOverwrite off
  File "MediaPortal Plugins\MP Blast Zone Plugin\AppData\Menu.xml"

  ; Write skin files
  SetOutPath "$DIR_MEDIAPORTAL\Skin\BlueTwo"
  SetOverwrite on
  File /r /x .svn "MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  SetOutPath "$DIR_MEDIAPORTAL\Skin\BlueTwo wide"
  SetOverwrite on
  File /r /x .svn "MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  ; Create Macro folder
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin\Macro"

SectionEnd

;======================================

!ifdef DEBUG
Section "TV2 Blaster Plugin" SectionTV2BlasterPlugin
!else
Section /o "TV2 Blaster Plugin" SectionTV2BlasterPlugin
!endif

  DetailPrint "Installing TV2 Blaster Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$DIR_MEDIAPORTAL\Plugins\Process"
  SetOverwrite ifnewer
  File "MediaPortal Plugins\TV2 Blaster Plugin\bin\${BuildType}\MPUtils.dll"
  File "MediaPortal Plugins\TV2 Blaster Plugin\bin\${BuildType}\IrssComms.dll"
  File "MediaPortal Plugins\TV2 Blaster Plugin\bin\${BuildType}\IrssUtils.dll"
  File "MediaPortal Plugins\TV2 Blaster Plugin\bin\${BuildType}\TV2BlasterPlugin.dll"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV2 Blaster Plugin"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV2 Blaster Plugin\Macro"

SectionEnd

;======================================

Section /o "TV3 Blaster Plugin" SectionTV3BlasterPlugin

  DetailPrint "Installing TV3 Blaster Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$DIR_TVSERVER\Plugins"
  SetOverwrite ifnewer
  File "MediaPortal Plugins\TV3 Blaster Plugin\bin\${BuildType}\MPUtils.dll"
  File "MediaPortal Plugins\TV3 Blaster Plugin\bin\${BuildType}\IrssComms.dll"
  File "MediaPortal Plugins\TV3 Blaster Plugin\bin\${BuildType}\IrssUtils.dll"
  File "MediaPortal Plugins\TV3 Blaster Plugin\bin\${BuildType}\TV3BlasterPlugin.dll"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV3 Blaster Plugin"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV3 Blaster Plugin\Macro"

SectionEnd

;======================================

Section "Translator" SectionTranslator

  DetailPrint "Installing Translator ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing Translator
  CreateDirectory "$DIR_INSTALL\Translator"
  SetOutPath "$DIR_INSTALL\Translator"
  SetOverwrite ifnewer
  File "Applications\Translator\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator\Macro"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator\Default Settings"

  ; Copy in default settings files  
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Translator\Default Settings"
  SetOverwrite ifnewer
  File "Applications\Translator\Default Settings\*.xml"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Translator.lnk" "$DIR_INSTALL\Translator\Translator.exe" "" "$DIR_INSTALL\Translator\Translator.exe" 0

SectionEnd

;======================================

!ifdef DEBUG
Section "Tray Launcher" SectionTrayLauncher
!else
Section /o "Tray Launcher" SectionTrayLauncher
!endif

  DetailPrint "Installing Tray Launcher ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing Translator
  CreateDirectory "$DIR_INSTALL\Tray Launcher"
  SetOutPath "$DIR_INSTALL\Tray Launcher"
  SetOverwrite ifnewer
  File "Applications\Tray Launcher\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Tray Launcher"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Tray Launcher.lnk" "$DIR_INSTALL\Tray Launcher\TrayLauncher.exe" "" "$DIR_INSTALL\Tray Launcher\TrayLauncher.exe" 0

SectionEnd

;======================================

Section "Virtual Remote" SectionVirtualRemote

  DetailPrint "Installing Virtual Remote, Skin Editor, Smart Device versions, and Web Remote..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing Virtual Remote and Web Remote
  CreateDirectory "$DIR_INSTALL\Virtual Remote"
  SetOutPath "$DIR_INSTALL\Virtual Remote"
  SetOverwrite ifnewer
  File "Applications\Virtual Remote\bin\${BuildType}\*.*"
  File "Applications\Web Remote\bin\${BuildType}\WebRemote.*"
  File "Applications\Virtual Remote Skin Editor\bin\${BuildType}\VirtualRemoteSkinEditor.*"

  ; Installing skins
  CreateDirectory "$DIR_INSTALL\Virtual Remote\Skins"
  SetOutPath "$DIR_INSTALL\Virtual Remote\Skins"
  SetOverwrite ifnewer
  File "Applications\Virtual Remote\Skins\*.*"

  ; Installing Virtual Remote for Smart Devices
  CreateDirectory "$DIR_INSTALL\Virtual Remote\Smart Devices"
  SetOutPath "$DIR_INSTALL\Virtual Remote\Smart Devices"
  SetOverwrite ifnewer
  File "Applications\Virtual Remote (PocketPC2003) Installer\${BuildType}\*.cab"
  File "Applications\Virtual Remote (Smartphone2003) Installer\${BuildType}\*.cab"
  File "Applications\Virtual Remote (WinCE5) Installer\${BuildType}\*.cab"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Virtual Remote"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote.lnk" "$DIR_INSTALL\Virtual Remote\VirtualRemote.exe" "" "$DIR_INSTALL\Virtual Remote\VirtualRemote.exe" 0
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote Skin Editor.lnk" "$DIR_INSTALL\Virtual Remote\VirtualRemoteSkinEditor.exe" "" "$DIR_INSTALL\Virtual Remote\VirtualRemoteSkinEditor.exe" 0
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote for Smart Devices.lnk" "$DIR_INSTALL\Virtual Remote\Smart Devices"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Web Remote.lnk" "$DIR_INSTALL\Virtual Remote\WebRemote.exe" "" "$DIR_INSTALL\Virtual Remote\WebRemote.exe" 0

SectionEnd

;======================================

Section "IR Blast" SectionIRBlast

  DetailPrint "Installing IR Blast ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing IR Server
  CreateDirectory "$DIR_INSTALL\IR Blast"
  SetOutPath "$DIR_INSTALL\IR Blast"
  SetOverwrite ifnewer
  File "Applications\IR Blast (No Window)\bin\${BuildType}\*.*"
  File "Applications\IR Blast\bin\${BuildType}\IRBlast.exe"

SectionEnd

;======================================

!ifdef DEBUG
Section "IR File Tool" SectionIRFileTool
!else
Section /o "IR File Tool" SectionIRFileTool
!endif

  DetailPrint "Installing IR File Tool ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing IR Server
  CreateDirectory "$DIR_INSTALL\IR File Tool"
  SetOutPath "$DIR_INSTALL\IR File Tool"
  SetOverwrite ifnewer
  File "Applications\IR File Tool\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\IR File Tool"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\IR File Tool.lnk" "$DIR_INSTALL\IR File Tool\IRFileTool.exe" "" "$DIR_INSTALL\IR File Tool\IRFileTool.exe" 0

SectionEnd

;======================================

!ifdef DEBUG
Section "Keyboard Relay" SectionKeyboardInputRelay
!else
Section /o "Keyboard Relay" SectionKeyboardInputRelay
!endif

  DetailPrint "Installing Keyboard Input Relay ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing IR Server
  CreateDirectory "$DIR_INSTALL\Keyboard Input Relay"
  SetOutPath "$DIR_INSTALL\Keyboard Input Relay"
  SetOverwrite ifnewer
  File "Applications\Keyboard Input Relay\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Keyboard Input Relay"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Keyboard Input Relay.lnk" "$DIR_INSTALL\Keyboard Input Relay\KeyboardInputRelay.exe" "" "$DIR_INSTALL\Keyboard Input Relay\KeyboardInputRelay.exe" 0

SectionEnd

;======================================

!ifdef DEBUG
Section "Dbox Tuner" SectionDboxTuner
!else
Section /o "Dbox Tuner" SectionDboxTuner
!endif

  DetailPrint "Installing Dbox Tuner ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing IR Server
  CreateDirectory "$DIR_INSTALL\Dbox Tuner"
  SetOutPath "$DIR_INSTALL\Dbox Tuner"
  SetOverwrite ifnewer
  File "Applications\Dbox Tuner\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Dbox Tuner"

SectionEnd

;======================================

!ifdef DEBUG
Section "HCW PVR Tuner" SectionHcwPvrTuner
!else
Section /o "HCW PVR Tuner" SectionHcwPvrTuner
!endif

  DetailPrint "Installing HCW PVR Tuner ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing IR Server
  CreateDirectory "$DIR_INSTALL\HCW PVR Tuner"
  SetOutPath "$DIR_INSTALL\HCW PVR Tuner"
  SetOverwrite ifnewer
  File "Applications\HCW PVR Tuner\bin\${BuildType}\*.*"

SectionEnd

;======================================

!ifdef DEBUG
Section "Debug Client" SectionDebugClient
!else
Section /o "Debug Client" SectionDebugClient
!endif

  DetailPrint "Installing Debug Client ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing Debug Client
  CreateDirectory "$DIR_INSTALL\Debug Client"
  SetOutPath "$DIR_INSTALL\Debug Client"
  SetOverwrite ifnewer
  File "Applications\Debug Client\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Debug Client"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Debug Client.lnk" "$DIR_INSTALL\Debug Client\DebugClient.exe" "" "$DIR_INSTALL\Debug Client\DebugClient.exe" 0

SectionEnd

;======================================

Section "-Complete"

  DetailPrint "Completing install ..."

  ; Use the all users context
  SetShellVarContext all

  ; Create website link file
  WriteIniStr "$DIR_INSTALL\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"

  ; Write the uninstaller
  WriteUninstaller "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"

  ; Create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Documentation.lnk" "$DIR_INSTALL\${PRODUCT_NAME}.chm"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Website.lnk" "$DIR_INSTALL\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Log Files.lnk" "$APPDATA\${PRODUCT_NAME}\Logs"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe" "" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"

  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayName" "${PRODUCT_NAME}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "UninstallString" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayIcon" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "Publisher" "${PRODUCT_PUBLISHER}"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "NoRepair" 1

  ; Store the install log
  StrCpy $0 "$APPDATA\${PRODUCT_NAME}\Logs\Install.log"
  Push $0
  Call DumpLog

  ; Finish
!ifdef DEBUG
  SetAutoClose false
!else
  SetAutoClose true
!endif

SectionEnd

;======================================
;======================================

; Section descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionInputService} "A windows service that provides access to your IR devices."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionMPControlPlugin} "Connects to the Input Service to control MediaPortal."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionMPBlastZonePlugin} "Lets you control your IR devices from within the MediaPortal GUI."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionTV2BlasterPlugin} "For tuning external channels (on Set Top Boxes) with the default MediaPortal TV engine."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionTV3BlasterPlugin} "For tuning external channels (on Set Top Boxes) with the MediaPortal TV server."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionTranslator} "Control your whole PC."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionTrayLauncher} "Simple tray application to launch an application of your choosing when a particular button is pressed."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionVirtualRemote} "Simulated remote control, works as an application or as a web hosted remote control (with included Web Remote).  Also includes a Skin Editor and Smart Device versions of Virtual Remote."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionIRBlast} "Command line tools for blasting IR codes."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionIRFileTool} "Tool for learning, modifying, testing, correcting and converting IR command files."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionKeyboardInputRelay} "Relays keyboard input to the Input Service to act on keypresses like remote buttons."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionDboxTuner} "Command line tuner for Dreambox devices."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionHcwPvrTuner} "Command line tuner for Hauppauge PVR devices."
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionDebugClient} "Very simple testing tool for troubleshooting input and communications problems."
!insertmacro MUI_FUNCTION_DESCRIPTION_END

;======================================
;======================================

!ifndef DEBUG
Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
FunctionEnd
!endif


;======================================

Function un.onInit

  !insertmacro initRegKeys

  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

;======================================
;======================================

Section "Uninstall"

  ; Use the all users context
  SetShellVarContext all

  ; Kill running Programs
  DetailPrint "Terminating processes ..."
  ExecWait '"taskkill" /F /IM Translator.exe'
  ExecWait '"taskkill" /F /IM TrayLauncher.exe'
  ExecWait '"taskkill" /F /IM WebRemote.exe'
  ExecWait '"taskkill" /F /IM VirtualRemote.exe'
  ExecWait '"taskkill" /F /IM VirtualRemoteSkinEditor.exe'
  ExecWait '"taskkill" /F /IM IRFileTool.exe'
  ExecWait '"taskkill" /F /IM DebugClient.exe'
  ExecWait '"taskkill" /F /IM KeyboardInputRelay.exe'
  Sleep 100

  ; Uninstall current Input Service ...
  IfFileExists "$DIR_INSTALL\Input Service\Input Service.exe" UninstallInputService SkipUninstallInputService

UninstallInputService:
  ExecWait '"$DIR_INSTALL\Input Service\Input Service.exe" /uninstall'

SkipUninstallInputService:
  Sleep 100

  ; Remove files and uninstaller
  DetailPrint "Attempting to remove MediaPortal Blast Zone Plugin ..."
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Windows\MPUtils.dll"
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Windows\IrssComms.dll"
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Windows\IrssUtils.dll"
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Windows\MPBlastZonePlugin.dll"

  DetailPrint "Attempting to remove MediaPortal Process Plugin Common Files ..."
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Process\MPUtils.dll"
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Process\IrssComms.dll"
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Process\IrssUtils.dll"

  DetailPrint "Attempting to remove MediaPortal Control Plugin ..."
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Process\MPControlPlugin.dll"

  DetailPrint "Attempting to remove MediaPortal TV2 Plugin ..."
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Process\TV2BlasterPlugin.dll"

  DetailPrint "Attempting to remove MediaPortal TV3 Plugin ..."
  Delete /REBOOTOK "$DIR_TVSERVER\Plugins\MPUtils.dll"
  Delete /REBOOTOK "$DIR_TVSERVER\Plugins\IrssComms.dll"
  Delete /REBOOTOK "$DIR_TVSERVER\Plugins\IrssUtils.dll"
  Delete /REBOOTOK "$DIR_TVSERVER\Plugins\TV3BlasterPlugin.dll"

  DetailPrint "Removing Set Top Box presets ..."
  RMDir /R "$APPDATA\${PRODUCT_NAME}\Set Top Boxes"

  DetailPrint "Removing program files ..."
  RMDir /R /REBOOTOK "$DIR_INSTALL"

  DetailPrint "Removing start menu shortcuts ..."
  RMDir /R "$SMPROGRAMS\${PRODUCT_NAME}"
  
  ; Remove registry keys
  DetailPrint "Removing registry keys ..."
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  DeleteRegKey HKLM "Software\${PRODUCT_NAME}"
  
  ; Remove auto-runs
  DetailPrint "Removing application auto-runs ..."
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Tray Launcher"
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Translator"
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Keyboard Input Relay"

!ifdef DEBUG
  SetAutoClose false
!else
  SetAutoClose true
!endif

SectionEnd

;======================================
;======================================