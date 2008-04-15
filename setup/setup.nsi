;======================================
; IR Server Suite.nsi
;
; (C) Copyright Aaron Dinnage, 2008
;======================================
#**********************************************************************************************************#
#
#   For building the installer on your own you need:
#       1. Lastest NSIS version from http://nsis.sourceforge.net/Download
#       2. The xml-plugin from http://nsis.sourceforge.net/XML_plug-in
#
#**********************************************************************************************************#

!define PRODUCT_NAME          "IR Server Suite"
!define PRODUCT_PUBLISHER     "Aaron Dinnage (and-81)"
!define PRODUCT_WEB_SITE      "http://forum.team-mediaportal.com/mce_replacement_plugin-f165.html"

!define REG_UNINSTALL         "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define MEMENTO_REGISTRY_ROOT HKLM
!define MEMENTO_REGISTRY_KEY  "${REG_UNINSTALL}"

; VER_BUILD is set to zero for Release builds
!define VER_MAJOR       1
!define VER_MINOR       4
!define VER_REVISION    2

!ifndef VER_BUILD
  !define VER_BUILD     0
!endif

!if ${VER_BUILD} == 0       # it's a stable release
  !define BuildType "Release"
  !define VERSION "${VER_MAJOR}.${VER_MINOR}.${VER_REVISION}"
!else                       # it's an svn release
  !define BuildType "Debug"
  !define VERSION "Test Build ${VER_MAJOR}.${VER_MINOR}.${VER_REVISION}.${VER_BUILD}"
!endif

BrandingText "${PRODUCT_NAME} - ${VERSION} by ${PRODUCT_PUBLISHER}"
SetCompressor /SOLID /FINAL lzma

#!define INSTALL_LOG_FILE "$DESKTOP\install_$(^Name).log"

;======================================

!include "x64.nsh"
!include MUI2.nsh
!include Sections.nsh
!include LogicLib.nsh
!include Library.nsh
!include FileFunc.nsh
!include WinVer.nsh
!include Memento.nsh

!include setup-AddRemovePage.nsh
!include setup-CommonMPMacros.nsh
!include setup-languages.nsh

; FileFunc macros
!insertmacro GetParent

;======================================

Name "${PRODUCT_NAME}"
OutFile "..\${PRODUCT_NAME} - ${VERSION}.exe"
InstallDir ""

ShowInstDetails show
ShowUninstDetails show
CRCCheck On

; Variables
var DIR_INSTALL
var DIR_MEDIAPORTAL
var DIR_TVSERVER

var MP_INSTALLED
var TVSERVER_INSTALLED

#---------------------------------------------------------------------------
# INSTALLER INTERFACE settings
#---------------------------------------------------------------------------
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\win-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\win-uninstall.ico"

!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP            "images\header.bmp"
!if ${VER_BUILD} == 0
  !define MUI_WELCOMEFINISHPAGE_BITMAP    "images\wizard.bmp"
  !define MUI_UNWELCOMEFINISHPAGE_BITMAP  "images\wizard.bmp"
!else
  !define MUI_WELCOMEFINISHPAGE_BITMAP    "images\wizard-svn.bmp"
  !define MUI_UNWELCOMEFINISHPAGE_BITMAP  "images\wizard-svn.bmp"
!endif
!define MUI_HEADERIMAGE_RIGHT

!define MUI_COMPONENTSPAGE_SMALLDESC
!define MUI_FINISHPAGE_NOAUTOCLOSE
;!define MUI_FINISHPAGE_RUN_NOTCHECKED
;!define MUI_FINISHPAGE_RUN      "$DIR_INSTALL\Input Service Configuration\Input Service Configuration.exe"
;!define MUI_FINISHPAGE_RUN_TEXT "Run Input Service Configuration"

!define MUI_UNFINISHPAGE_NOAUTOCLOSE

#---------------------------------------------------------------------------
# INSTALLER INTERFACE
#---------------------------------------------------------------------------
!insertmacro MUI_PAGE_WELCOME
Page custom PageReinstall PageLeaveReinstall
!insertmacro MUI_PAGE_LICENSE "..\Documentation\LICENSE.GPL"
#!define MUI_PAGE_CUSTOMFUNCTION_PRE ComponentsPre
!insertmacro MUI_PAGE_COMPONENTS

; MediaPortal install path
!define MUI_PAGE_HEADER_TEXT "Choose MediaPortal Location"
!define MUI_PAGE_HEADER_SUBTEXT "Choose the folder in which to install MediaPortal plugins."
!define MUI_DIRECTORYPAGE_TEXT_TOP "Setup will install MediaPortal plugins in the following folder.$\r$\n$\r$\nTo install in a different folder, click Browse and select another folder. Click Next to continue."
!define MUI_DIRECTORYPAGE_TEXT_DESTINATION "MediaPortal Folder"
!define MUI_DIRECTORYPAGE_VARIABLE "$DIR_MEDIAPORTAL"
!define MUI_PAGE_CUSTOMFUNCTION_PRE DirectoryPreMP
!insertmacro MUI_PAGE_DIRECTORY

; TV Server install path
!define MUI_PAGE_HEADER_TEXT "Choose TV Server Location"
!define MUI_PAGE_HEADER_SUBTEXT "Choose the folder in which to install TV Server plugins."
!define MUI_DIRECTORYPAGE_TEXT_TOP "Setup will install TV Server plugins in the following folder.$\r$\n$\r$\nTo install in a different folder, click Browse and select another folder. Click Next to continue."
!define MUI_DIRECTORYPAGE_TEXT_DESTINATION "TV Server Folder"
!define MUI_DIRECTORYPAGE_VARIABLE "$DIR_TVSERVER"
!define MUI_PAGE_CUSTOMFUNCTION_PRE DirectoryPreTV
!insertmacro MUI_PAGE_DIRECTORY

; Main app install path
!define MUI_PAGE_HEADER_TEXT "Choose ${PRODUCT_NAME} Location"
!define MUI_PAGE_HEADER_SUBTEXT "Choose the folder in which to install ${PRODUCT_NAME}."
!define MUI_DIRECTORYPAGE_TEXT_TOP "Setup will install ${PRODUCT_NAME} in the following folder.$\r$\n$\r$\nTo install in a different folder, click Browse and select another folder. Click Install to start the installation."
!define MUI_DIRECTORYPAGE_TEXT_DESTINATION "${PRODUCT_NAME} Folder"
!define MUI_DIRECTORYPAGE_VARIABLE "$DIR_INSTALL"
!insertmacro MUI_PAGE_DIRECTORY

!insertmacro MUI_PAGE_INSTFILES
!define MUI_PAGE_CUSTOMFUNCTION_SHOW FinishShow
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

;======================================
;======================================

!macro SectionList MacroName
  ; This macro used to perform operation on multiple sections.
  ; List all of your components in following manner here.
  !insertmacro "${MacroName}" "SectionInputService"

  !insertmacro "${MacroName}" "SectionMPCommon"
  !insertmacro "${MacroName}" "SectionMPControlPlugin"
  !insertmacro "${MacroName}" "SectionMPBlastZonePlugin"
  !insertmacro "${MacroName}" "SectionTV2BlasterPlugin"

  !insertmacro "${MacroName}" "SectionTV3Common"
  !insertmacro "${MacroName}" "SectionTV3BlasterPlugin"

  !insertmacro "${MacroName}" "SectionMCEBlaster"

  !insertmacro "${MacroName}" "SectionTranslator"
  !insertmacro "${MacroName}" "SectionTrayLauncher"
  !insertmacro "${MacroName}" "SectionVirtualRemote"
  !insertmacro "${MacroName}" "SectionIRBlast"
  !insertmacro "${MacroName}" "SectionIRFileTool"
  !insertmacro "${MacroName}" "SectionKeyboardInputRelay"
  !insertmacro "${MacroName}" "SectionDboxTuner"
  !insertmacro "${MacroName}" "SectionHcwPvrTuner"
  !insertmacro "${MacroName}" "SectionDebugClient"
!macroend

;======================================

!macro Initialize

  ${If} ${RunningX64}
    SetRegView 64
  ${Endif}

  ReadRegStr $DIR_INSTALL HKLM "Software\${PRODUCT_NAME}" "Install_Dir"
  ReadRegStr $DIR_MEDIAPORTAL HKLM "Software\${PRODUCT_NAME}" "MediaPortal_Dir"
  ReadRegStr $DIR_TVSERVER HKLM "Software\${PRODUCT_NAME}" "TVServer_Dir"

  ${If} ${RunningX64}
    SetRegView 32
    ${EnableX64FSRedirection}
  ${Endif}

  ; Get MediaPortal installation directory ...
  ${If} $DIR_MEDIAPORTAL == ""
    !insertmacro MP_GET_INSTALL_DIR "$DIR_MEDIAPORTAL"
  ${Endif}

  ; Get MediaPortal TV Server installation directory ...
  ${If} $DIR_TVSERVER == ""
    !insertmacro TVSERVER_GET_INSTALL_DIR "$DIR_TVSERVER"
  ${Endif}

  ${If} ${RunningX64}
    SetRegView 64
    ${DisableX64FSRedirection}
  ${Endif}

  ; Get IR Server Suite installation directory ...
  ${If} $DIR_INSTALL == ""
    StrCpy '$DIR_INSTALL' '$PROGRAMFILES\${PRODUCT_NAME}'
  ${Endif}

!macroend

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
  ExecWait '"taskkill" /F /IM MediaCenterBlaster.exe'
  ExecWait '"taskkill" /F /IM "Input Service Configuration.exe"'

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
!if ${VER_BUILD} != 0
  DetailPrint "Warning: Documentation is not included in debug builds"
!else
  SetOutPath "$DIR_INSTALL"
  SetOverwrite ifnewer
  File "..\Documentation\${PRODUCT_NAME}.chm"
!endif

  ; Create app data directories
  CreateDirectory "$APPDATA\${PRODUCT_NAME}"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Logs"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\IR Commands"

  ; Copy known set top boxes
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Set Top Boxes"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Set Top Boxes"
  SetOverwrite ifnewer
  File /r /x .svn "..\Set Top Boxes\*.*"

  ; Create a start menu shortcut folder
  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"

SectionEnd

;======================================

${MementoSection} "Input Service" SectionInputService

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
  File "..\Input Service\Input Service\bin\${BuildType}\*.*"

  ; Installing Input Service Configuration
  CreateDirectory "$DIR_INSTALL\Input Service Configuration"
  SetOutPath "$DIR_INSTALL\Input Service Configuration"
  SetOverwrite ifnewer
  File "..\Input Service\Input Service Configuration\bin\${BuildType}\*.*"

  ; Install IR Server Plugins ...
  DetailPrint "Installing IR Server Plugins ..."
  CreateDirectory "$DIR_INSTALL\IR Server Plugins"
  SetOutPath "$DIR_INSTALL\IR Server Plugins"
  SetOverwrite ifnewer

  File "..\IR Server Plugins\Ads Tech PTV-335 Receiver\bin\${BuildType}\Ads Tech PTV-335 Receiver.*"
  File "..\IR Server Plugins\CoolCommand Receiver\bin\${BuildType}\CoolCommand Receiver.*"
  File "..\IR Server Plugins\Custom HID Receiver\bin\${BuildType}\Custom HID Receiver.*"
  File "..\IR Server Plugins\Direct Input Receiver\bin\${BuildType}\Direct Input Receiver.*"
  File "..\IR Server Plugins\Direct Input Receiver\bin\${BuildType}\Microsoft.DirectX.DirectInput.dll"
  File "..\IR Server Plugins\Direct Input Receiver\bin\${BuildType}\Microsoft.DirectX.dll"
  File "..\IR Server Plugins\FusionRemote Receiver\bin\${BuildType}\FusionRemote Receiver.*"
  File "..\IR Server Plugins\Girder Plugin\bin\${BuildType}\Girder Plugin.*"
  File "..\IR Server Plugins\HCW Receiver\bin\${BuildType}\HCW Receiver.*"
  File "..\IR Server Plugins\IgorPlug Receiver\bin\${BuildType}\IgorPlug Receiver.*"
  File "..\IR Server Plugins\Imon Receiver\bin\${BuildType}\Imon Receiver.*"
  ;File "..\IR Server Plugins\IR501 Receiver\bin\${BuildType}\IR501 Receiver.*"
  File "..\IR Server Plugins\IR507 Receiver\bin\${BuildType}\IR507 Receiver.*"
  ;File "..\IR Server Plugins\Ira Transceiver\bin\${BuildType}\Ira Transceiver.*"
  File "..\IR Server Plugins\IRMan Receiver\bin\${BuildType}\IRMan Receiver.*"
  File "..\IR Server Plugins\IRTrans Transceiver\bin\${BuildType}\IRTrans Transceiver.*"
  ;File "..\IR Server Plugins\Keyboard Input\bin\${BuildType}\Keyboard Input.*"
  File "..\IR Server Plugins\LiveDrive Receiver\bin\${BuildType}\LiveDrive Receiver.*"
  File "..\IR Server Plugins\MacMini Receiver\bin\${BuildType}\MacMini Receiver.*"
  File "..\IR Server Plugins\Microsoft MCE Transceiver\bin\${BuildType}\Microsoft MCE Transceiver.*"
  ;File "..\IR Server Plugins\RC102 Receiver\bin\${BuildType}\RC102 Receiver.*"
  File "..\IR Server Plugins\RedEye Blaster\bin\${BuildType}\RedEye Blaster.*"
  File "..\IR Server Plugins\Serial IR Blaster\bin\${BuildType}\Serial IR Blaster.*"
  ;File "..\IR Server Plugins\Speech Receiver\bin\${BuildType}\Speech Receiver.*"
  File "..\IR Server Plugins\Technotrend Receiver\bin\${BuildType}\Technotrend Receiver.*"
  File "..\IR Server Plugins\Technotrend Receiver\bin\${BuildType}\ttBdaDrvApi_Dll.dll"
  ;File "..\IR Server Plugins\Tira Transceiver\bin\${BuildType}\Tira Transceiver.*"
  File "..\IR Server Plugins\USB-UIRT Transceiver\bin\${BuildType}\USB-UIRT Transceiver.*"
  File "..\IR Server Plugins\Wii Remote Receiver\bin\${BuildType}\Wii Remote Receiver.*"
  File "..\IR Server Plugins\WiimoteLib\bin\${BuildType}\WiimoteLib.*"
  File "..\IR Server Plugins\Windows Message Receiver\bin\${BuildType}\Windows Message Receiver.*"
  File "..\IR Server Plugins\WinLirc Transceiver\bin\${BuildType}\WinLirc Transceiver.*"
  File "..\IR Server Plugins\X10 Transceiver\bin\${BuildType}\X10 Transceiver.*"
  File "..\IR Server Plugins\X10 Transceiver\bin\${BuildType}\Interop.X10.dll"
  File "..\IR Server Plugins\XBCDRC Receiver\bin\${BuildType}\XBCDRC Receiver.*"

  ; Create App Data Folder for IR Server configuration files
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Input Service"
  
  ; Copy Abstract Remote maps
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Input Service\Abstract Remote Maps"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Input Service\Abstract Remote Maps"
  SetOverwrite ifnewer
  File /r /x .svn "..\Input Service\Input Service\Abstract Remote Maps\*.*"
  File "..\Input Service\Input Service\RemoteTable.xsd"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Input Service Configuration.lnk" "$DIR_INSTALL\Input Service Configuration\Input Service Configuration.exe" "" "$DIR_INSTALL\Input Service Configuration\Input Service Configuration.exe" 0

  ; Launch Input Service
  DetailPrint "Starting Input Service ..."
  ExecWait '"$DIR_INSTALL\Input Service\Input Service.exe" /install'

${MementoSectionEnd}
!macro Remove_${SectionInputService}
  DetailPrint "Attempting to remove Input Service ..."

  ; remove Input Service
  ExecWait '"$DIR_INSTALL\Input Service\Input Service.exe" /uninstall'

  ; remove Start Menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Input Service Configuration.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Input Service"
  RMDir /R /REBOOTOK "$DIR_INSTALL\Input Service Configuration"
  RMDir /R /REBOOTOK "$DIR_INSTALL\IR Server Plugins"
!macroend

;======================================

SectionGroup /e "MediaPortal plugins" SectionGroupMP

Section "-commonMP" SectionMPCommon

  DetailPrint "Installing common files for MediaPortal plugins ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$DIR_MEDIAPORTAL\Plugins\Process"
  SetOverwrite ifnewer
  File "..\Common\MPUtils\bin\${BuildType}\MPUtils.dll"
  File "..\Common\IrssComms\bin\${BuildType}\IrssComms.dll"
  File "..\Common\IrssUtils\bin\${BuildType}\IrssUtils.dll"

  ; Write plugin dll
  SetOutPath "$DIR_MEDIAPORTAL\Plugins\Windows"
  SetOverwrite ifnewer
  File "..\Common\MPUtils\bin\${BuildType}\MPUtils.dll"
  File "..\Common\IrssComms\bin\${BuildType}\IrssComms.dll"
  File "..\Common\IrssUtils\bin\${BuildType}\IrssUtils.dll"
SectionEnd
!macro Remove_${SectionMPCommon}
  DetailPrint "Attempting to remove common files for MediaPortal plugins ..."

  ; remove files
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Process\MPUtils.dll"
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Process\IrssComms.dll"
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Process\IrssUtils.dll"
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Windows\MPUtils.dll"
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Windows\IrssComms.dll"
  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Windows\IrssUtils.dll"
!macroend

;======================================

${MementoSection} "MP Control Plugin" SectionMPControlPlugin

  DetailPrint "Installing MP Control Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$DIR_MEDIAPORTAL\Plugins\Process"
  SetOverwrite ifnewer
  File "..\MediaPortal Plugins\MP Control Plugin\bin\${BuildType}\MPControlPlugin.dll"

  ; Write input mapping
  SetOutPath "$DIR_MEDIAPORTAL\InputDeviceMappings\defaults"
  SetOverwrite ifnewer
  File "..\MediaPortal Plugins\MP Control Plugin\InputMapping\MPControlPlugin.xml"

  ; Write app data
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Control Plugin"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\MP Control Plugin"
  SetOverwrite ifnewer
  File /r /x .svn "..\MediaPortal Plugins\MP Control Plugin\AppData\*.*"

  ; Create Macro folder
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Control Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionMPControlPlugin}
  DetailPrint "Attempting to remove MediaPortal Control Plugin ..."

  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Process\MPControlPlugin.dll"
!macroend

;======================================

!if ${VER_BUILD} != 0
${MementoSection} "MP Blast Zone Plugin" SectionMPBlastZonePlugin
!else
${MementoUnselectedSection} "MP Blast Zone Plugin" SectionMPBlastZonePlugin
!endif

  DetailPrint "Installing MP Blast Zone Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$DIR_MEDIAPORTAL\Plugins\Windows"
  SetOverwrite ifnewer
  File "..\MediaPortal Plugins\MP Blast Zone Plugin\bin\${BuildType}\MPBlastZonePlugin.dll"

  ; Write app data
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin"
  SetOverwrite off
  File "..\MediaPortal Plugins\MP Blast Zone Plugin\AppData\Menu.xml"

  ; Write skin files
  SetOutPath "$DIR_MEDIAPORTAL\Skin\BlueTwo"
  SetOverwrite on
  File /r /x .svn "..\MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  SetOutPath "$DIR_MEDIAPORTAL\Skin\BlueTwo wide"
  SetOverwrite on
  File /r /x .svn "..\MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  ; Create Macro folder
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionMPBlastZonePlugin}
  DetailPrint "Attempting to remove MediaPortal Blast Zone Plugin ..."

  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Windows\MPBlastZonePlugin.dll"
!macroend

;======================================

!if ${VER_BUILD} != 0
${MementoSection} "TV2 Blaster Plugin" SectionTV2BlasterPlugin
!else
${MementoUnselectedSection} "TV2 Blaster Plugin" SectionTV2BlasterPlugin
!endif

  DetailPrint "Installing TV2 Blaster Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$DIR_MEDIAPORTAL\Plugins\Process"
  SetOverwrite ifnewer
  File "..\MediaPortal Plugins\TV2 Blaster Plugin\bin\${BuildType}\TV2BlasterPlugin.dll"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV2 Blaster Plugin"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV2 Blaster Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionTV2BlasterPlugin}
  DetailPrint "Attempting to remove MediaPortal TV2 Plugin ..."

  Delete /REBOOTOK "$DIR_MEDIAPORTAL\Plugins\Process\TV2BlasterPlugin.dll"
!macroend

;======================================

SectionGroupEnd

;======================================

SectionGroup /e "TV Server plugins" SectionGroupTV3

Section "-commonTV3" SectionTV3Common

  DetailPrint "Installing common files for TV Server plugins ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$DIR_TVSERVER\Plugins"
  SetOverwrite ifnewer
  File "..\Common\MPUtils\bin\${BuildType}\MPUtils.dll"
  File "..\Common\IrssComms\bin\${BuildType}\IrssComms.dll"
  File "..\Common\IrssUtils\bin\${BuildType}\IrssUtils.dll"
SectionEnd
!macro Remove_${SectionTV3Common}
  DetailPrint "Attempting to remove common files for TV Server plugins ..."

  ; remove files
  Delete /REBOOTOK "$DIR_TVSERVER\Plugins\MPUtils.dll"
  Delete /REBOOTOK "$DIR_TVSERVER\Plugins\IrssComms.dll"
  Delete /REBOOTOK "$DIR_TVSERVER\Plugins\IrssUtils.dll"
!macroend

;======================================

${MementoUnselectedSection} "TV3 Blaster Plugin" SectionTV3BlasterPlugin

  DetailPrint "Installing TV3 Blaster Plugin ..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$DIR_TVSERVER\Plugins"
  SetOverwrite ifnewer
  File "..\MediaPortal Plugins\TV3 Blaster Plugin\bin\${BuildType}\TV3BlasterPlugin.dll"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV3 Blaster Plugin"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV3 Blaster Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionTV3BlasterPlugin}
  DetailPrint "Attempting to remove MediaPortal TV3 Plugin ..."

  Delete /REBOOTOK "$DIR_TVSERVER\Plugins\TV3BlasterPlugin.dll"
!macroend

;======================================

SectionGroupEnd

;======================================

SectionGroup /e "Media Center add-ons" SectionGroupMCE

!if ${VER_BUILD} != 0
${MementoSection} "Media Center Blaster (experimental)" SectionMCEBlaster
!else
${MementoUnselectedSection} "Media Center Blaster (experimental)" SectionMCEBlaster
!endif

  DetailPrint "Installing Media Center Blaster ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing Translator
  CreateDirectory "$DIR_INSTALL\Media Center Blaster"
  SetOutPath "$DIR_INSTALL\Media Center Blaster"
  SetOverwrite ifnewer
  File "..\Applications\Media Center Blaster\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Media Center Blaster"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Media Center Blaster\Macro"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Media Center Blaster.lnk" "$DIR_INSTALL\Media Center Blaster\MediaCenterBlaster.exe" "" "$DIR_INSTALL\Media Center Blaster\MediaCenterBlaster.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionMCEBlaster}
  DetailPrint "Attempting to remove Media Center Blaster ..."

  ; Remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Media Center Blaster"

  ; remove Start Menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Media Center Blaster.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Media Center Blaster"
!macroend

;======================================

SectionGroupEnd

;======================================

${MementoSection} "Translator" SectionTranslator

  DetailPrint "Installing Translator ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing Translator
  CreateDirectory "$DIR_INSTALL\Translator"
  SetOutPath "$DIR_INSTALL\Translator"
  SetOverwrite ifnewer
  File "..\Applications\Translator\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator\Macro"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator\Default Settings"

  ; Copy in default settings files  
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Translator\Default Settings"
  SetOverwrite ifnewer
  File "..\Applications\Translator\Default Settings\*.xml"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Translator.lnk" "$DIR_INSTALL\Translator\Translator.exe" "" "$DIR_INSTALL\Translator\Translator.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionTranslator}
  DetailPrint "Attempting to remove Translator ..."

  ; Remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Translator"

  ; remove Start Menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Translator.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Translator"
!macroend

;======================================

!if ${VER_BUILD} != 0
${MementoSection} "Tray Launcher" SectionTrayLauncher
!else
${MementoUnselectedSection} "Tray Launcher" SectionTrayLauncher
!endif

  DetailPrint "Installing Tray Launcher ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing Translator
  CreateDirectory "$DIR_INSTALL\Tray Launcher"
  SetOutPath "$DIR_INSTALL\Tray Launcher"
  SetOverwrite ifnewer
  File "..\Applications\Tray Launcher\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Tray Launcher"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Tray Launcher.lnk" "$DIR_INSTALL\Tray Launcher\TrayLauncher.exe" "" "$DIR_INSTALL\Tray Launcher\TrayLauncher.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionTrayLauncher}
  DetailPrint "Attempting to remove Tray Launcher ..."

  ; Remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Tray Launcher"

  ; remove Start Menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Tray Launcher.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Tray Launcher"
!macroend

;======================================

${MementoSection} "Virtual Remote" SectionVirtualRemote

  DetailPrint "Installing Virtual Remote, Skin Editor, Smart Device versions, and Web Remote..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing Virtual Remote and Web Remote
  CreateDirectory "$DIR_INSTALL\Virtual Remote"
  SetOutPath "$DIR_INSTALL\Virtual Remote"
  SetOverwrite ifnewer
  File "..\Applications\Virtual Remote\bin\${BuildType}\*.*"
  File "..\Applications\Web Remote\bin\${BuildType}\WebRemote.*"
  File "..\Applications\Virtual Remote Skin Editor\bin\${BuildType}\VirtualRemoteSkinEditor.*"

  ; Installing skins
  CreateDirectory "$DIR_INSTALL\Virtual Remote\Skins"
  SetOutPath "$DIR_INSTALL\Virtual Remote\Skins"
  SetOverwrite ifnewer
  File "..\Applications\Virtual Remote\Skins\*.*"

  ; Installing Virtual Remote for Smart Devices
  CreateDirectory "$DIR_INSTALL\Virtual Remote\Smart Devices"
  SetOutPath "$DIR_INSTALL\Virtual Remote\Smart Devices"
  SetOverwrite ifnewer
  File "..\Applications\Virtual Remote (PocketPC2003) Installer\${BuildType}\*.cab"
  File "..\Applications\Virtual Remote (Smartphone2003) Installer\${BuildType}\*.cab"
  File "..\Applications\Virtual Remote (WinCE5) Installer\${BuildType}\*.cab"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Virtual Remote"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote.lnk" "$DIR_INSTALL\Virtual Remote\VirtualRemote.exe" "" "$DIR_INSTALL\Virtual Remote\VirtualRemote.exe" 0
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote Skin Editor.lnk" "$DIR_INSTALL\Virtual Remote\VirtualRemoteSkinEditor.exe" "" "$DIR_INSTALL\Virtual Remote\VirtualRemoteSkinEditor.exe" 0
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote for Smart Devices.lnk" "$DIR_INSTALL\Virtual Remote\Smart Devices"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Web Remote.lnk" "$DIR_INSTALL\Virtual Remote\WebRemote.exe" "" "$DIR_INSTALL\Virtual Remote\WebRemote.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionVirtualRemote}
  DetailPrint "Attempting to remove Virtual Remote, Skin Editor, Smart Device versions, and Web Remote ..."

  ; remove Start Menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote Skin Editor.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote for Smart Devices.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Web Remote.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Virtual Remote"
!macroend

;======================================

${MementoSection} "IR Blast" SectionIRBlast

  DetailPrint "Installing IR Blast ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing IR Server
  CreateDirectory "$DIR_INSTALL\IR Blast"
  SetOutPath "$DIR_INSTALL\IR Blast"
  SetOverwrite ifnewer
  File "..\Applications\IR Blast (No Window)\bin\${BuildType}\*.*"
  File "..\Applications\IR Blast\bin\${BuildType}\IRBlast.exe"

${MementoSectionEnd}
!macro Remove_${SectionIRBlast}
  DetailPrint "Attempting to remove IR Blast ..."

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\IR Blast"
!macroend

;======================================

!if ${VER_BUILD} != 0
${MementoSection} "IR File Tool" SectionIRFileTool
!else
${MementoUnselectedSection} "IR File Tool" SectionIRFileTool
!endif

  DetailPrint "Installing IR File Tool ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing IR Server
  CreateDirectory "$DIR_INSTALL\IR File Tool"
  SetOutPath "$DIR_INSTALL\IR File Tool"
  SetOverwrite ifnewer
  File "..\Applications\IR File Tool\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\IR File Tool"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\IR File Tool.lnk" "$DIR_INSTALL\IR File Tool\IRFileTool.exe" "" "$DIR_INSTALL\IR File Tool\IRFileTool.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionIRFileTool}
  DetailPrint "Attempting to remove IR File Tool ..."

  ; remove Start Menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\IR File Tool.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\IR File Tool"
!macroend

;======================================

!if ${VER_BUILD} != 0
${MementoSection} "Keyboard Relay" SectionKeyboardInputRelay
!else
${MementoUnselectedSection} "Keyboard Relay" SectionKeyboardInputRelay
!endif

  DetailPrint "Installing Keyboard Input Relay ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing IR Server
  CreateDirectory "$DIR_INSTALL\Keyboard Input Relay"
  SetOutPath "$DIR_INSTALL\Keyboard Input Relay"
  SetOverwrite ifnewer
  File "..\Applications\Keyboard Input Relay\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Keyboard Input Relay"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Keyboard Input Relay.lnk" "$DIR_INSTALL\Keyboard Input Relay\KeyboardInputRelay.exe" "" "$DIR_INSTALL\Keyboard Input Relay\KeyboardInputRelay.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionKeyboardInputRelay}
  DetailPrint "Attempting to remove Keyboard Relay ..."

  ; Remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Keyboard Input Relay"

  ; remove Start Menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Keyboard Input Relay.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Keyboard Input Relay"
!macroend

;======================================

!if ${VER_BUILD} != 0
${MementoSection} "Dbox Tuner" SectionDboxTuner
!else
${MementoUnselectedSection} "Dbox Tuner" SectionDboxTuner
!endif

  DetailPrint "Installing Dbox Tuner ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing IR Server
  CreateDirectory "$DIR_INSTALL\Dbox Tuner"
  SetOutPath "$DIR_INSTALL\Dbox Tuner"
  SetOverwrite ifnewer
  File "..\Applications\Dbox Tuner\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Dbox Tuner"

${MementoSectionEnd}
!macro Remove_${SectionDboxTuner}
  DetailPrint "Attempting to remove Dbox Tuner ..."

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Dbox Tuner"
!macroend

;======================================

!if ${VER_BUILD} != 0
${MementoSection} "HCW PVR Tuner" SectionHcwPvrTuner
!else
${MementoUnselectedSection} "HCW PVR Tuner" SectionHcwPvrTuner
!endif

  DetailPrint "Installing HCW PVR Tuner ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing IR Server
  CreateDirectory "$DIR_INSTALL\HCW PVR Tuner"
  SetOutPath "$DIR_INSTALL\HCW PVR Tuner"
  SetOverwrite ifnewer
  File "..\Applications\HCW PVR Tuner\bin\${BuildType}\*.*"

${MementoSectionEnd}
!macro Remove_${SectionHcwPvrTuner}
  DetailPrint "Attempting to remove HCW PVR Tuner ..."

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\HCW PVR Tuner"
!macroend

;======================================

!if ${VER_BUILD} != 0
${MementoSection} "Debug Client" SectionDebugClient
!else
${MementoUnselectedSection} "Debug Client" SectionDebugClient
!endif

  DetailPrint "Installing Debug Client ..."

  ; Use the all users context
  SetShellVarContext all

  ; Installing Debug Client
  CreateDirectory "$DIR_INSTALL\Debug Client"
  SetOutPath "$DIR_INSTALL\Debug Client"
  SetOverwrite ifnewer
  File "..\Applications\Debug Client\bin\${BuildType}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Debug Client"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Debug Client.lnk" "$DIR_INSTALL\Debug Client\DebugClient.exe" "" "$DIR_INSTALL\Debug Client\DebugClient.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionDebugClient}
  DetailPrint "Attempting to remove Debug Client ..."

  ; remove Start Menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Debug Client.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Debug Client"
!macroend

;======================================

${MementoSectionDone}

;======================================

Section "-Complete"

  DetailPrint "Completing install ..."

  ;Removes unselected components
  !insertmacro SectionList "FinishSection"
  ;writes component status to registry
  ${MementoSectionSave}

  ; Use the all users context
  SetShellVarContext all

  ; Create website link file
  WriteIniStr "$DIR_INSTALL\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"

  ; Write the uninstaller
  WriteUninstaller "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"

  ; Create start menu shortcuts
!if ${VER_BUILD} == 0
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Documentation.lnk" "$DIR_INSTALL\${PRODUCT_NAME}.chm"
!endif
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Website.lnk" "$DIR_INSTALL\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Log Files.lnk" "$APPDATA\${PRODUCT_NAME}\Logs"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe" "" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"

  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayName" "${PRODUCT_NAME}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "UninstallString" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayIcon" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "DisplayVersion" "${VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "Publisher" "${PRODUCT_PUBLISHER}"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}" "NoRepair" 1

  ; Store the install log
  StrCpy $0 "$APPDATA\${PRODUCT_NAME}\Logs\Install.log"
  Push $0
  Call DumpLog

  ; Finish
!if ${VER_BUILD} != 0
  SetAutoClose false
!else
  SetAutoClose true
!endif

SectionEnd

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
  ExecWait '"taskkill" /F /IM MediaCenterBlaster.exe'
  ExecWait '"taskkill" /F /IM "Input Service Configuration.exe"'
  Sleep 100

  ;First removes all optional components
  !insertmacro SectionList "RemoveSection"

  ; Remove files and uninstaller
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

!if ${VER_BUILD} != 0
  SetAutoClose false
!else
  SetAutoClose true
!endif

SectionEnd

;======================================
;======================================

Function .onInit
  ${LOG_OPEN}


  #!insertmacro Initialize


  ${If} ${RunningX64}
    SetRegView 32
    ${EnableX64FSRedirection}
  ${Endif}

  ${IfNot} ${MP023IsInstalled}
  ${AndIfNot} ${MPIsInstalled}
    #StrCpy '$MP_INSTALLED' 'FALSE'
    !insertmacro DisableComponent "${SectionGroupMP}" " ($(TEXT_MP_NOT_INSTALLED))"
    !insertmacro DisableComponent "${SectionMPCommon}" ""
    !insertmacro DisableComponent "${SectionMPControlPlugin}" ""
    !insertmacro DisableComponent "${SectionMPBlastZonePlugin}" ""
    !insertmacro DisableComponent "${SectionTV2BlasterPlugin}" ""
  ${Else}
    #StrCpy '$MP_INSTALLED' 'TRUE'
    !insertmacro MP_GET_INSTALL_DIR $MPdir.Base
    ${ReadMediaPortalDirs} $MPdir.Base
  ${EndIf}

  ${IfNot} ${TVServerIsInstalled}
    #StrCpy '$TVSERVER_INSTALLED' 'FALSE'
    !insertmacro DisableComponent "${SectionGroupTV3}" " ($(TEXT_TVSERVER_NOT_INSTALLED))"
    !insertmacro DisableComponent "${SectionTV3Common}" ""
    !insertmacro DisableComponent "${SectionTV3BlasterPlugin}" ""
  ${Else}
    #StrCpy '$TVSERVER_INSTALLED' 'TRUE'
    !insertmacro TVSERVER_GET_INSTALL_DIR "$DIR_TVSERVER"
  ${EndIf}
  
  ${If} ${RunningX64}
    SetRegView 64
    ${DisableX64FSRedirection}
  ${Endif}


  ; reads components status for registry
  ${MementoSectionRestore}

FunctionEnd

;======================================

Function .onInstFailed
  ${LOG_CLOSE}
FunctionEnd

;======================================

Function .onInstSuccess

  IfFileExists "$DIR_INSTALL\Input Service\Input Service.exe" StartInputService SkipStartInputService

StartInputService:
  Exec '"$DIR_INSTALL\Input Service\Input Service.exe" /start'

SkipStartInputService:

  ${LOG_CLOSE}
FunctionEnd

;======================================

Function .onSelChange

  ; disable/remove common files for MediaPortal plugins if all MediaPortal plugins are unselected
  ${IfNot} ${SectionIsSelected} ${SectionMPControlPlugin}
  ${AndIfNot} ${SectionIsSelected} ${SectionMPBlastZonePlugin}
  ${AndIfNot} ${SectionIsSelected} ${SectionTV2BlasterPlugin}
    !insertmacro UnselectSection ${SectionMPCommon}
  ${Else}
    !insertmacro SelectSection ${SectionMPCommon}
  ${EndIf}

  ; disable/remove common files for MediaPortal plugins if all MediaPortal plugins are unselected
  ${IfNot} ${SectionIsSelected} ${SectionTV3BlasterPlugin}
    !insertmacro UnselectSection ${SectionTV3Common}
  ${Else}
    !insertmacro SelectSection ${SectionTV3Common}
  ${EndIf}

FunctionEnd

;======================================
/*
Function ComponentsPre

  ${If} $MP_INSTALLED == "FALSE"
    !insertmacro DisableComponent "${SectionGroupMP}" " ($(TEXT_MP_NOT_INSTALLED))"
    !insertmacro DisableComponent "${SectionMPCommon}" ""
    !insertmacro DisableComponent "${SectionMPControlPlugin}" ""
    !insertmacro DisableComponent "${SectionMPBlastZonePlugin}" ""
    !insertmacro DisableComponent "${SectionTV2BlasterPlugin}" ""
  ${Endif}

  ${If} $TVSERVER_INSTALLED == "FALSE"
    !insertmacro DisableComponent "${SectionGroupTV3}" " ($(TEXT_TVSERVER_NOT_INSTALLED))"
    !insertmacro DisableComponent "${SectionTV3Common}" ""
    !insertmacro DisableComponent "${SectionTV3BlasterPlugin}" ""
  ${Endif}

FunctionEnd
*/
;======================================

Function DirectoryPreMP
  ${IfNot} ${SectionIsSelected} ${SectionGroupMP}
    Abort
  ${EndIf}
FunctionEnd

;======================================

Function DirectoryPreTV
  ${IfNot} ${SectionIsSelected} ${SectionGroupTV3}
    Abort
  ${EndIf}
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

Function FinishShow
  ; This function is called, after the Finish Page creation is finished

  ; It checks, if the Server has been selected and only displays the run checkbox in this case
  ${IfNot} ${SectionIsSelected} SectionInputService
      SendMessage $mui.FinishPage.Run ${BM_CLICK} 0 0
      ShowWindow  $mui.FinishPage.Run ${SW_HIDE}
  ${EndIf}
FunctionEnd

;======================================
;======================================

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) was successfully removed from your computer."
FunctionEnd

;======================================

Function un.onInit

  #!insertmacro Initialize


  ${If} ${RunningX64}
    SetRegView 32
    ${EnableX64FSRedirection}
  ${Endif}

  ${IfNot} ${MP023IsInstalled}
  ${AndIfNot} ${MPIsInstalled}
    Sleep 1
  ${Else}
    !insertmacro MP_GET_INSTALL_DIR $MPdir.Base
    ${un.ReadMediaPortalDirs} $MPdir.Base
  ${EndIf}

  ${IfNot} ${TVServerIsInstalled}
    Sleep 1
  ${Else}
    !insertmacro TVSERVER_GET_INSTALL_DIR "$DIR_TVSERVER"
  ${EndIf}
  
  ${If} ${RunningX64}
    SetRegView 64
    ${DisableX64FSRedirection}
  ${Endif}


  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove $(^Name) and all of its components?" IDYES +2
  Abort
FunctionEnd

;======================================
;======================================

; Section descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionInputService}        "$(DESC_SectionInputService)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionGroupMP}             "$(DESC_SectionGroupMP)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionMPControlPlugin}     "$(DESC_SectionMPControlPlugin)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionMPBlastZonePlugin}   "$(DESC_SectionMPBlastZonePlugin)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionTV2BlasterPlugin}    "$(DESC_SectionTV2BlasterPlugin)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionGroupTV3}            "$(DESC_SectionGroupTV3)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionTV3BlasterPlugin}    "$(DESC_SectionTV3BlasterPlugin)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionGroupMCE}            "$(DESC_SectionGroupMCE)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionMCEBlaster}          "$(DESC_SectionMCEBlaster)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionTranslator}          "$(DESC_SectionTranslator)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionTrayLauncher}        "$(DESC_SectionTrayLauncher)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionVirtualRemote}       "$(DESC_SectionVirtualRemote)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionIRBlast}             "$(DESC_SectionIRBlast)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionIRFileTool}          "$(DESC_SectionIRFileTool)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionKeyboardInputRelay}  "$(DESC_SectionKeyboardInputRelay)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionDboxTuner}           "$(DESC_SectionDboxTuner)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionHcwPvrTuner}         "$(DESC_SectionHcwPvrTuner)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionDebugClient}         "$(DESC_SectionDebugClient)"
!insertmacro MUI_FUNCTION_DESCRIPTION_END

;======================================
;======================================