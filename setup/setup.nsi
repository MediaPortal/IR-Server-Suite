;======================================
; IR Server Suite.nsi
;
; (C) Copyright Aaron Dinnage, 2008
;======================================


#---------------------------------------------------------------------------
# SPECIAL BUILDS
#---------------------------------------------------------------------------
##### BUILD_TYPE
# Uncomment the following line to create a setup in debug mode
;!define BUILD_TYPE "Debug"
# parameter for command line execution: /DBUILD_TYPE=Debug
# by default BUILD_TYPE is set to "Release"
!ifndef BUILD_TYPE
  !define BUILD_TYPE "Release"
!endif


#---------------------------------------------------------------------------
# DEVELOPMENT ENVIRONMENT
#---------------------------------------------------------------------------
# path definitions
;!define svn_ROOT_IRSS ".."
;!define svn_InstallScripts "${svn_ROOT_IRSS}\setup\CommonNSIS"
!define svn_InstallScripts "."


#---------------------------------------------------------------------------
# DEFINES
#---------------------------------------------------------------------------
!define PRODUCT_NAME          "IR Server Suite"
!define PRODUCT_PUBLISHER     "Aaron Dinnage (and-81)"
!define PRODUCT_WEB_SITE      "http://forum.team-mediaportal.com/mce_replacement_plugin-f165.html"

!define REG_UNINSTALL         "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define MEMENTO_REGISTRY_ROOT HKLM
!define MEMENTO_REGISTRY_KEY  "${REG_UNINSTALL}"
!define COMMON_APPDATA        "$APPDATA\${PRODUCT_NAME}"

; VER_BUILD is set to zero for Release builds
!define VER_MAJOR       1
!define VER_MINOR       4
!define VER_REVISION    2

!ifndef VER_BUILD
  !define VER_BUILD     0
!endif

!if ${VER_BUILD} == 0       # it's a stable release
  !define VERSION "${VER_MAJOR}.${VER_MINOR}.${VER_REVISION}"
!else                       # it's an svn release
  !define VERSION "Test Build ${VER_MAJOR}.${VER_MINOR}.${VER_REVISION}.${VER_BUILD}"
!endif

BrandingText "${PRODUCT_NAME} - ${VERSION} by ${PRODUCT_PUBLISHER}"
SetCompressor /SOLID /FINAL lzma

; enable logging
!define INSTALL_LOG

; to use default path to logfile, COMMON_APPDATA has to be defined
; default logfile is: "${COMMON_APPDATA}\Logs\install_${VER_MAJOR}.${VER_MINOR}.${VER_REVISION}.${VER_BUILD}.log"
; if you want to set custom path to logfile, uncomment the following line
#!define INSTALL_LOG_FILE "$DESKTOP\install_$(^Name).log"

;======================================

!include x64.nsh
!include MUI2.nsh
!include Sections.nsh
!include LogicLib.nsh
!include Library.nsh
!include FileFunc.nsh
!include Memento.nsh
!include WinVer.nsh

!include "include\*"

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
Var DIR_INSTALL
Var DIR_MEDIAPORTAL
Var DIR_TVSERVER

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
!insertmacro MUI_PAGE_LICENSE "..\Documentation\LICENSE.GPL"
Page custom PageReinstall
!insertmacro MUI_PAGE_COMPONENTS
Page custom PageServerServiceMode

; MediaPortal install path
!define MUI_PAGE_HEADER_TEXT "Choose MediaPortal Location"
!define MUI_PAGE_HEADER_SUBTEXT "Choose the folder in which to install MediaPortal plugins."
!define MUI_DIRECTORYPAGE_TEXT_TOP "Setup will install MediaPortal plugins in the following folder.$\r$\n$\r$\nTo install in a different folder, click Browse and select another folder. Click Next to continue."
!define MUI_DIRECTORYPAGE_TEXT_DESTINATION "MediaPortal Folder"
!define MUI_DIRECTORYPAGE_VARIABLE "$DIR_MEDIAPORTAL"
!define MUI_PAGE_CUSTOMFUNCTION_PRE DirectoryPreMP
!define MUI_PAGE_CUSTOMFUNCTION_LEAVE DirectoryLeaveMP
!insertmacro MUI_PAGE_DIRECTORY

; TV Server install path
!define MUI_PAGE_HEADER_TEXT "Choose TV Server Location"
!define MUI_PAGE_HEADER_SUBTEXT "Choose the folder in which to install TV Server plugins."
!define MUI_DIRECTORYPAGE_TEXT_TOP "Setup will install TV Server plugins in the following folder.$\r$\n$\r$\nTo install in a different folder, click Browse and select another folder. Click Next to continue."
!define MUI_DIRECTORYPAGE_TEXT_DESTINATION "TV Server Folder"
!define MUI_DIRECTORYPAGE_VARIABLE "$DIR_TVSERVER"
!define MUI_PAGE_CUSTOMFUNCTION_PRE DirectoryPreTV
!define MUI_PAGE_CUSTOMFUNCTION_LEAVE DirectoryLeaveTV
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

# Installer languages
!insertmacro LANG_LOAD "English"

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

;  !insertmacro "${MacroName}" "SectionMCEBlaster"

  #SectionGroupTools
  !insertmacro "${MacroName}" "SectionAbstractor"
  !insertmacro "${MacroName}" "SectionDebugClient"
  !insertmacro "${MacroName}" "SectionIRFileTool"
  !insertmacro "${MacroName}" "SectionKeyboardInputRelay"
  !insertmacro "${MacroName}" "SectionTranslator"
  !insertmacro "${MacroName}" "SectionTrayLauncher"
  !insertmacro "${MacroName}" "SectionVirtualRemote"
  
  #SectionGroupCmdLineTools
  !insertmacro "${MacroName}" "SectionIRBlast"
  !insertmacro "${MacroName}" "SectionDboxTuner"
  !insertmacro "${MacroName}" "SectionHcwPvrTuner"
!macroend

;======================================

!macro Initialize

  ${If} ${RunningX64}
    SetRegView 64
    ${DisableX64FSRedirection}
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


  ; Create app data directories
  SetOutPath "$DIR_INSTALL"
  File "..\Documentation\${PRODUCT_NAME}.chm"


  ; Create app data directories
  CreateDirectory "$APPDATA\${PRODUCT_NAME}"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Logs"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\IR Commands"

  ; Copy known set top boxes
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Set Top Boxes"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Set Top Boxes"
  SetOverwrite ifnewer
  File /r /x .svn "..\Set Top Boxes\*.*"
  SetOverwrite on

  ; Create a start menu shortcut folder
  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"

SectionEnd

;======================================

${MementoSection} "Input Service" SectionInputService
  ${LOG_TEXT} "INFO" "Installing Input Service..."
  ${KILLPROCESS} "Input Service Configuration.exe"

  ; Use the all users context
  SetShellVarContext all



  ${LOG_TEXT} "INFO" "Removing current IRServer from Autostart..."
  !insertmacro RemoveAutoRun "IR Server"
  ${LOG_TEXT} "INFO" "Uninstalling current InputService..."
  ExecWait '"$DIR_INSTALL\Input Service\Input Service.exe" /uninstall'



  ${LOG_TEXT} "INFO" "Installing Input Service..."
  SetOutPath "$DIR_INSTALL\Input Service"
  File "..\Input Service\Input Service\bin\${Build_Type}\*.*"

  ${LOG_TEXT} "INFO" "Installing Input Service Configuration..."
  SetOutPath "$DIR_INSTALL\Input Service Configuration"
  File "..\Input Service\Input Service Configuration\bin\${Build_Type}\*.*"

  ${LOG_TEXT} "INFO" "Installing IR Server..."
  SetOutPath "$DIR_INSTALL\Input Service"
  File "..\Applications\IR Server\bin\${Build_Type}\*.*"


  ${LOG_TEXT} "INFO" "Installing IR Server Plugins..."
  SetOutPath "$DIR_INSTALL\IR Server Plugins"

  File "..\IR Server Plugins\Ads Tech PTV-335 Receiver\bin\${Build_Type}\Ads Tech PTV-335 Receiver.*"
  File "..\IR Server Plugins\CoolCommand Receiver\bin\${Build_Type}\CoolCommand Receiver.*"
  File "..\IR Server Plugins\Custom HID Receiver\bin\${Build_Type}\Custom HID Receiver.*"
  File "..\IR Server Plugins\Direct Input Receiver\bin\${Build_Type}\Direct Input Receiver.*"
  File "..\IR Server Plugins\Direct Input Receiver\bin\${Build_Type}\Microsoft.DirectX.DirectInput.dll"
  File "..\IR Server Plugins\Direct Input Receiver\bin\${Build_Type}\Microsoft.DirectX.dll"
  File "..\IR Server Plugins\FusionRemote Receiver\bin\${Build_Type}\FusionRemote Receiver.*"
  File "..\IR Server Plugins\Girder Plugin\bin\${Build_Type}\Girder Plugin.*"
  File "..\IR Server Plugins\HCW Receiver\bin\${Build_Type}\HCW Receiver.*"
  File "..\IR Server Plugins\IgorPlug Receiver\bin\${Build_Type}\IgorPlug Receiver.*"
  ;File "..\IR Server Plugins\Imon Receiver\bin\${Build_Type}\Imon Receiver.*"
  File "..\IR Server Plugins\Imon USB Receivers\bin\${Build_Type}\Imon USB Receivers.*"
  ;File "..\IR Server Plugins\IR501 Receiver\bin\${Build_Type}\IR501 Receiver.*"
  File "..\IR Server Plugins\IR507 Receiver\bin\${Build_Type}\IR507 Receiver.*"
  ;File "..\IR Server Plugins\Ira Transceiver\bin\${Build_Type}\Ira Transceiver.*"
  File "..\IR Server Plugins\IRMan Receiver\bin\${Build_Type}\IRMan Receiver.*"
  File "..\IR Server Plugins\IRTrans Transceiver\bin\${Build_Type}\IRTrans Transceiver.*"
  ;File "..\IR Server Plugins\Keyboard Input\bin\${Build_Type}\Keyboard Input.*"
  File "..\IR Server Plugins\LiveDrive Receiver\bin\${Build_Type}\LiveDrive Receiver.*"
  File "..\IR Server Plugins\MacMini Receiver\bin\${Build_Type}\MacMini Receiver.*"
  File "..\IR Server Plugins\Microsoft MCE Transceiver\bin\${Build_Type}\Microsoft MCE Transceiver.*"
  File "..\IR Server Plugins\Pinnacle Serial Receiver\bin\${Build_Type}\Pinnacle Serial Receiver.*"
  ;File "..\IR Server Plugins\RC102 Receiver\bin\${Build_Type}\RC102 Receiver.*"
  File "..\IR Server Plugins\RedEye Blaster\bin\${Build_Type}\RedEye Blaster.*"
  File "..\IR Server Plugins\Serial IR Blaster\bin\${Build_Type}\Serial IR Blaster.*"
  ;File "..\IR Server Plugins\Speech Receiver\bin\${Build_Type}\Speech Receiver.*"
  File "..\IR Server Plugins\Technotrend Receiver\bin\${Build_Type}\Technotrend Receiver.*"
  File "..\IR Server Plugins\Technotrend Receiver\bin\${Build_Type}\ttBdaDrvApi_Dll.dll"
  ;File "..\IR Server Plugins\Tira Transceiver\bin\${Build_Type}\Tira Transceiver.*"
  File "..\IR Server Plugins\USB-UIRT Transceiver\bin\${Build_Type}\USB-UIRT Transceiver.*"
  File "..\IR Server Plugins\Wii Remote Receiver\bin\${Build_Type}\Wii Remote Receiver.*"
  File "..\IR Server Plugins\WiimoteLib\bin\${Build_Type}\WiimoteLib.*"
  File "..\IR Server Plugins\Windows Message Receiver\bin\${Build_Type}\Windows Message Receiver.*"
  File "..\IR Server Plugins\WinLirc Transceiver\bin\${Build_Type}\WinLirc Transceiver.*"
  File "..\IR Server Plugins\X10 Transceiver\bin\${Build_Type}\X10 Transceiver.*"
  File "..\IR Server Plugins\X10 Transceiver\bin\${Build_Type}\Interop.X10.dll"
  File "..\IR Server Plugins\XBCDRC Receiver\bin\${Build_Type}\XBCDRC Receiver.*"

  ; Create App Data Folder for IR Server configuration files
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Input Service"
  
  ; Copy Abstract Remote maps
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Input Service\Abstract Remote Maps"
  SetOverwrite ifnewer
  File /r /x .svn "..\Input Service\Input Service\Abstract Remote Maps\*.*"
  File "..\Input Service\Input Service\RemoteTable.xsd"
  SetOverwrite on

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Input Service Configuration.lnk" "$DIR_INSTALL\Input Service Configuration\Input Service Configuration.exe" "" "$DIR_INSTALL\Input Service Configuration\Input Service Configuration.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionInputService}
  ${LOG_TEXT} "INFO" "Removing Input Service..."
  ${KILLPROCESS} "Input Service Configuration.exe"

  ${LOG_TEXT} "INFO" "Removing IRServer from Autostart..."
  !insertmacro RemoveAutoRun "IR Server"
  ${LOG_TEXT} "INFO" "Uninstalling InputService..."
  ExecWait '"$DIR_INSTALL\Input Service\Input Service.exe" /uninstall'

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Input Service Configuration.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Input Service"
  RMDir /R /REBOOTOK "$DIR_INSTALL\Input Service Configuration"
  RMDir /R /REBOOTOK "$DIR_INSTALL\IR Server"
  RMDir /R /REBOOTOK "$DIR_INSTALL\IR Server Plugins"
!macroend

;======================================

SectionGroup "MediaPortal plugins" SectionGroupMP

Section "-commonMP" SectionMPCommon
  ${LOG_TEXT} "INFO" "Installing common files for MediaPortal plugins..."
  ${KILLPROCESS} "MediaPortal.exe"
  ${KILLPROCESS} "configuration.exe"

  ; Write plugin dll
  SetOutPath "$MPdir.Plugins\Process"
  File "..\Common\MPUtils\bin\${Build_Type}\MPUtils.*"
  File "..\Common\IrssComms\bin\${Build_Type}\IrssComms.*"
  File "..\Common\IrssUtils\bin\${Build_Type}\IrssUtils.*"

  ; Write plugin dll
  SetOutPath "$MPdir.Plugins\Windows"
  File "..\Common\MPUtils\bin\${Build_Type}\MPUtils.*"
  File "..\Common\IrssComms\bin\${Build_Type}\IrssComms.*"
  File "..\Common\IrssUtils\bin\${Build_Type}\IrssUtils.*"
SectionEnd
!macro Remove_${SectionMPCommon}
  ${LOG_TEXT} "INFO" "Removing common files for MediaPortal plugins..."
  ${KILLPROCESS} "MediaPortal.exe"
  ${KILLPROCESS} "configuration.exe"

  ; remove files
  Delete /REBOOTOK "$MPdir.Plugins\Process\MPUtils.*"
  Delete /REBOOTOK "$MPdir.Plugins\Process\IrssComms.*"
  Delete /REBOOTOK "$MPdir.Plugins\Process\IrssUtils.*"
  Delete /REBOOTOK "$MPdir.Plugins\Windows\MPUtils.*"
  Delete /REBOOTOK "$MPdir.Plugins\Windows\IrssComms.*"
  Delete /REBOOTOK "$MPdir.Plugins\Windows\IrssUtils.*"
!macroend

;======================================

${MementoSection} "MP Control Plugin" SectionMPControlPlugin
  ${LOG_TEXT} "INFO" "Installing MP Control Plugin..."

  ; Write plugin dll
  SetOutPath "$MPdir.Plugins\Process"
  File "..\MediaPortal Plugins\MP Control Plugin\bin\${Build_Type}\MPControlPlugin.*"

  ; Write input mapping
  SetOutPath "$MPdir.CustomInputDefault"
  File "..\MediaPortal Plugins\MP Control Plugin\InputMapping\MPControlPlugin.xml"

  ; Write app data
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Control Plugin"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\MP Control Plugin"
  SetOverwrite ifnewer
  File /r /x .svn "..\MediaPortal Plugins\MP Control Plugin\AppData\*.*"
  SetOverwrite on

  ; Create Macro folder
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Control Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionMPControlPlugin}
  ${LOG_TEXT} "INFO" "MP Control Plugin..."

  Delete /REBOOTOK "$MPdir.Plugins\Process\MPControlPlugin.*"
!macroend

;======================================

${MementoUnselectedSection} "MP Blast Zone Plugin" SectionMPBlastZonePlugin
  ${LOG_TEXT} "INFO" "Installing MP Blast Zone Plugin..."

  ; Use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$MPdir.Plugins\Windows"
  File "..\MediaPortal Plugins\MP Blast Zone Plugin\bin\${Build_Type}\MPBlastZonePlugin.*"

  ; Write app data
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin"
  SetOverwrite off
  File "..\MediaPortal Plugins\MP Blast Zone Plugin\AppData\Menu.xml"
  SetOverwrite on

  ; Write skin files
  SetOutPath "$MPdir.Skin\Blue3"
  File /r /x .svn "..\MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  SetOutPath "$MPdir.Skin\Blue3wide"
  File /r /x .svn "..\MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  ; Create Macro folder
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionMPBlastZonePlugin}
  ${LOG_TEXT} "INFO" "Removing MP Blast Zone Plugin..."

  Delete /REBOOTOK "$MPdir.Plugins\Windows\MPBlastZonePlugin.*"
!macroend

;======================================

${MementoUnselectedSection} "TV2 Blaster Plugin" SectionTV2BlasterPlugin
  ${LOG_TEXT} "INFO" "Installing TV2 Blaster Plugin..."

  ; Write plugin dll
  SetOutPath "$MPdir.Plugins\Process"
  File "..\MediaPortal Plugins\TV2 Blaster Plugin\bin\${Build_Type}\TV2BlasterPlugin.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV2 Blaster Plugin"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV2 Blaster Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionTV2BlasterPlugin}
  ${LOG_TEXT} "INFO" "Removing TV2 Blaster Plugin..."

  Delete /REBOOTOK "$MPdir.Plugins\Process\TV2BlasterPlugin.*"
!macroend

;======================================

SectionGroupEnd

;======================================
Var RestartTvService
!macro StopTVService
  ${If} $RestartTvService != 0

  ; stopping TV Service
  ; if TV service was Running, and has been stopped correctly   $RestartTvService = 0 , otherwise 1 or 2 ............
  ${LOG_TEXT} "INFO" "Stopping TV Service..."
  nsExec::ExecToLog 'net stop TVservice'
  Pop $RestartTvService

  ${KILLPROCESS} "TVService.exe"
  ${KILLPROCESS} "SetupTv.exe"

  ${EndIf}
!macroend
!macro StartTVService
  ; only if TVService was stopped by the installer before, correctly, start it now
  ${If} $RestartTvService == 0
    ${LOG_TEXT} "INFO" "Starting TV Service..."
    nsExec::ExecToLog 'net start TVservice'
    StrCpy $RestartTvService 1
  ${EndIf}
!macroend

SectionGroup "TV Server plugins" SectionGroupTV3

Section "-commonTV3" SectionTV3Common
  ${LOG_TEXT} "INFO" "Installing common files for TV Server plugins..."
  !insertmacro StopTVService

  ; Write plugin dll
  SetOutPath "$DIR_TVSERVER\Plugins"
  File "..\Common\MPUtils\bin\${Build_Type}\MPUtils.*"
  File "..\Common\IrssComms\bin\${Build_Type}\IrssComms.*"
  File "..\Common\IrssUtils\bin\${Build_Type}\IrssUtils.*"
SectionEnd
!macro Remove_${SectionTV3Common}
  ${If} ${FileExists} "$DIR_TVSERVER\Plugins\MPUtils.*"
  ${OrIf} ${FileExists} "$DIR_TVSERVER\Plugins\IrssComms.*"
  ${OrIf} ${FileExists} "$DIR_TVSERVER\Plugins\IrssUtils.*"

    ${LOG_TEXT} "INFO" "Removing common files for TV Server plugins..."
    !insertmacro StopTVService

    ; remove files
    Delete /REBOOTOK "$DIR_TVSERVER\Plugins\MPUtils.*"
    Delete /REBOOTOK "$DIR_TVSERVER\Plugins\IrssComms.*"
    Delete /REBOOTOK "$DIR_TVSERVER\Plugins\IrssUtils.*"

  ${EndIf}
!macroend

;======================================

${MementoSection} "TV Server Blaster Plugin" SectionTV3BlasterPlugin
  ${LOG_TEXT} "INFO" "Installing TV Server Blaster Plugin..."

  ; Write plugin dll
  SetOutPath "$DIR_TVSERVER\Plugins"
  File "..\MediaPortal Plugins\TV3 Blaster Plugin\bin\${Build_Type}\TV3BlasterPlugin.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV3 Blaster Plugin"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV3 Blaster Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionTV3BlasterPlugin}
  ${If} ${FileExists} "$DIR_TVSERVER\Plugins\MPUtils.*"

    ${LOG_TEXT} "INFO" "Removing TV Server Blaster Plugin..."
    !insertmacro StopTVService

    Delete /REBOOTOK "$DIR_TVSERVER\Plugins\TV3BlasterPlugin.*"

  ${EndIf}
!macroend

;======================================

SectionGroupEnd

;======================================
/*
SectionGroup /e "Media Center add-ons" SectionGroupMCE

${MementoUnselectedSection} "Media Center Blaster (experimental)" SectionMCEBlaster
  ${LOG_TEXT} "INFO" "Installing MediaCenterBlaster..."
  ${KILLPROCESS} "MediaCenterBlaster.exe"

  ; Use the all users context
  SetShellVarContext all

  ; Installing Translator
  CreateDirectory "$DIR_INSTALL\Media Center Blaster"
  SetOutPath "$DIR_INSTALL\Media Center Blaster"
  File "..\Applications\Media Center Blaster\bin\${Build_Type}\*.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Media Center Blaster"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Media Center Blaster\Macro"

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Media Center Blaster.lnk" "$DIR_INSTALL\Media Center Blaster\MediaCenterBlaster.exe" "" "$DIR_INSTALL\Media Center Blaster\MediaCenterBlaster.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionMCEBlaster}
  ${LOG_TEXT} "INFO" "Removing MediaCenterBlaster..."
  ${KILLPROCESS} "MediaCenterBlaster.exe"

  ; Remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Media Center Blaster"

  ; remove Start Menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Media Center Blaster.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Media Center Blaster"
!macroend

;======================================

SectionGroupEnd
*/
;======================================

SectionGroup "Tools" SectionGroupTools

${MementoSection} "Abstractor" SectionAbstractor
  ${LOG_TEXT} "INFO" "Installing Abstractor..."
  ${KILLPROCESS} "Abstractor.exe"

  ; install files
  SetOutPath "$DIR_INSTALL\Abstractor"
  File "..\Applications\Abstractor\bin\${Build_Type}\*.*"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Abstractor.lnk" "$DIR_INSTALL\Abstractor\Abstractor.exe" "" "$DIR_INSTALL\Abstractor\Abstractor.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionAbstractor}
  ${LOG_TEXT} "INFO" "Removing Abstractor..."
  ${KILLPROCESS} "Abstractor.exe"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Abstractor.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Abstractor"
!macroend

;======================================

${MementoSection} "Debug Client" SectionDebugClient
  ${LOG_TEXT} "INFO" "Installing Debug Client..."
  ${KILLPROCESS} "DebugClient.exe"

  ; install files
  SetOutPath "$DIR_INSTALL\Debug Client"
  File "..\Applications\Debug Client\bin\${Build_Type}\*.*"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Debug Client"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Debug Client.lnk" "$DIR_INSTALL\Debug Client\DebugClient.exe" "" "$DIR_INSTALL\Debug Client\DebugClient.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionDebugClient}
  ${LOG_TEXT} "INFO" "Removing Debug Client..."
  ${KILLPROCESS} "DebugClient.exe"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Debug Client.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Debug Client"
!macroend

;======================================

${MementoSection} "IR File Tool" SectionIRFileTool
  ${LOG_TEXT} "INFO" "Installing IR File Tool..."
  ${KILLPROCESS} "IRFileTool.exe"

  ; install files
  SetOutPath "$DIR_INSTALL\IR File Tool"
  File "..\Applications\IR File Tool\bin\${Build_Type}\*.*"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\IR File Tool"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\IR File Tool.lnk" "$DIR_INSTALL\IR File Tool\IRFileTool.exe" "" "$DIR_INSTALL\IR File Tool\IRFileTool.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionIRFileTool}
  ${LOG_TEXT} "INFO" "Removing IR File Tool..."
  ${KILLPROCESS} "IRFileTool.exe"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\IR File Tool.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\IR File Tool"
!macroend

;======================================

${MementoSection} "Keyboard Input Relay" SectionKeyboardInputRelay
  ${LOG_TEXT} "INFO" "Installing Keyboard Input Relay..."
  ${KILLPROCESS} "KeyboardInputRelay.exe"

  ; install files
  SetOutPath "$DIR_INSTALL\Keyboard Input Relay"
  File "..\Applications\Keyboard Input Relay\bin\${Build_Type}\*.*"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Keyboard Input Relay"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Keyboard Input Relay.lnk" "$DIR_INSTALL\Keyboard Input Relay\KeyboardInputRelay.exe" "" "$DIR_INSTALL\Keyboard Input Relay\KeyboardInputRelay.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionKeyboardInputRelay}
  ${LOG_TEXT} "INFO" "Removing Keyboard Input Relay..."
  ${KILLPROCESS} "KeyboardInputRelay.exe"

  ; remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Keyboard Input Relay"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Keyboard Input Relay.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Keyboard Input Relay"
!macroend

;======================================

${MementoSection} "Translator" SectionTranslator
  ${LOG_TEXT} "INFO" "Installing Translator..."
  ${KILLPROCESS} "Translator.exe"

  ; install files
  SetOutPath "$DIR_INSTALL\Translator"
  File "..\Applications\Translator\bin\${Build_Type}\*.*"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator\Macro"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator\Default Settings"

  ; Copy in default settings files  
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Translator\Default Settings"
  File "..\Applications\Translator\Default Settings\*.xml"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Translator.lnk" "$DIR_INSTALL\Translator\Translator.exe" "" "$DIR_INSTALL\Translator\Translator.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionTranslator}
  ${LOG_TEXT} "INFO" "Removing Translator..."
  ${KILLPROCESS} "Translator.exe"

  ; remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Translator"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Translator.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Translator"
!macroend

;======================================

${MementoSection} "Tray Launcher" SectionTrayLauncher
  ${LOG_TEXT} "INFO" "Installing Tray Launcher..."
  ${KILLPROCESS} "TrayLauncher.exe"

  ; install files
  SetOutPath "$DIR_INSTALL\Tray Launcher"
  File "..\Applications\Tray Launcher\bin\${Build_Type}\*.*"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Tray Launcher.lnk" "$DIR_INSTALL\Tray Launcher\TrayLauncher.exe" "" "$DIR_INSTALL\Tray Launcher\TrayLauncher.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionTrayLauncher}
  ${LOG_TEXT} "INFO" "Removing Tray Launcher..."
  ${KILLPROCESS} "TrayLauncher.exe"

  ; remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Tray Launcher"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Tray Launcher.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Tray Launcher"
!macroend

;======================================

${MementoSection} "Virtual Remote" SectionVirtualRemote
  ${LOG_TEXT} "INFO" "Installing Virtual Remote, Skin Editor, Smart Device versions, and Web Remote..."
  ${KILLPROCESS} "WebRemote.exe"
  ${KILLPROCESS} "VirtualRemote.exe"
  ${KILLPROCESS} "VirtualRemoteSkinEditor.exe"

  ; Installing Virtual Remote and Web Remote
  SetOutPath "$DIR_INSTALL\Virtual Remote"
  File "..\Applications\Virtual Remote\bin\${Build_Type}\*.*"
  File "..\Applications\Web Remote\bin\${Build_Type}\WebRemote.*"
  File "..\Applications\Virtual Remote Skin Editor\bin\${Build_Type}\VirtualRemoteSkinEditor.*"

  ; Installing skins
  SetOutPath "$DIR_INSTALL\Virtual Remote\Skins"
  File "..\Applications\Virtual Remote\Skins\*.*"

  ; Installing Virtual Remote for Smart Devices
  SetOutPath "$DIR_INSTALL\Virtual Remote\Smart Devices"
  File "..\Applications\Virtual Remote (PocketPC2003) Installer\${Build_Type}\*.cab"
  File "..\Applications\Virtual Remote (Smartphone2003) Installer\${Build_Type}\*.cab"
  File "..\Applications\Virtual Remote (WinCE5) Installer\${Build_Type}\*.cab"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Virtual Remote"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote.lnk" "$DIR_INSTALL\Virtual Remote\VirtualRemote.exe" "" "$DIR_INSTALL\Virtual Remote\VirtualRemote.exe" 0
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote Skin Editor.lnk" "$DIR_INSTALL\Virtual Remote\VirtualRemoteSkinEditor.exe" "" "$DIR_INSTALL\Virtual Remote\VirtualRemoteSkinEditor.exe" 0
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote for Smart Devices.lnk" "$DIR_INSTALL\Virtual Remote\Smart Devices"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Web Remote.lnk" "$DIR_INSTALL\Virtual Remote\WebRemote.exe" "" "$DIR_INSTALL\Virtual Remote\WebRemote.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionVirtualRemote}
  ${LOG_TEXT} "INFO" "Removing Virtual Remote, Skin Editor, Smart Device versions, and Web Remote..."
  ${KILLPROCESS} "WebRemote.exe"
  ${KILLPROCESS} "VirtualRemote.exe"
  ${KILLPROCESS} "VirtualRemoteSkinEditor.exe"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote Skin Editor.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote for Smart Devices.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Web Remote.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Virtual Remote"
!macroend

SectionGroupEnd

;======================================

SectionGroup "CommandLine Tools" SectionGroupCmdLineTools

${MementoSection} "IR Blast" SectionIRBlast
  ${LOG_TEXT} "INFO" "Installing IR Blast..."

  ; install files
  SetOutPath "$DIR_INSTALL\IR Blast"
  File "..\Applications\IR Blast (No Window)\bin\${Build_Type}\*.*"
  File "..\Applications\IR Blast\bin\${Build_Type}\IRBlast.exe"

${MementoSectionEnd}
!macro Remove_${SectionIRBlast}
  ${LOG_TEXT} "INFO" "Removing IR Blast..."

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\IR Blast"
!macroend

;======================================

${MementoSection} "Dreambox Tuner" SectionDboxTuner
  ${LOG_TEXT} "INFO" "Installing Dreambox Tuner..."

  ; install files
  SetOutPath "$DIR_INSTALL\Dbox Tuner"
  File "..\Applications\Dbox Tuner\bin\${Build_Type}\*.*"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Dbox Tuner"

${MementoSectionEnd}
!macro Remove_${SectionDboxTuner}
  ${LOG_TEXT} "INFO" "Removing Dreambox Tuner..."

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\Dbox Tuner"
!macroend

;======================================

${MementoSection} "Hauppauge PVR Tuner" SectionHcwPvrTuner
  ${LOG_TEXT} "INFO" "Installing Hauppauge PVR Tuner..."

  ; install files
  SetOutPath "$DIR_INSTALL\HCW PVR Tuner"
  File "..\Applications\HCW PVR Tuner\bin\${Build_Type}\*.*"

${MementoSectionEnd}
!macro Remove_${SectionHcwPvrTuner}
  ${LOG_TEXT} "INFO" "Removing Hauppauge PVR Tuner..."

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\HCW PVR Tuner"
!macroend

SectionGroupEnd

;======================================

${MementoSectionDone}

;======================================

Section "-Complete"

  DetailPrint "Completing install ..."

  ;Removes unselected components
  !insertmacro SectionList "FinishSection"
  ;writes component status to registry
  ${MementoSectionSave}

  ; start tvservice, if it was closed before
  !insertmacro StartTVService

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

SectionEnd

;======================================
;======================================

Section "Uninstall"

  ; Use the all users context
  SetShellVarContext all

  ;First removes all optional components
  !insertmacro SectionList "RemoveSection"

  ; start tvservice, if it was closed before
  !insertmacro StartTVService

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

SectionEnd

;======================================
;======================================
;=======   Additional Pages   =========
;======================================
;======================================


!include pages\AddRemovePage.nsh
!insertmacro AddRemovePage "${REG_UNINSTALL}"

!include pages\ServerServiceMode.nsh


;======================================
;======================================

Function .onInit
  ${LOG_OPEN}

  !insertmacro Initialize
  ${ReadMediaPortalDirs} $DIR_MEDIAPORTAL

  ; first read the old installation status
  ${MementoSectionRestore}
  Call .onSelChange
  
  ; on default set mode to service
  StrCpy $ServerServiceMode 0

FunctionEnd

;======================================

Function .onInstFailed
  ${LOG_CLOSE}
FunctionEnd

;======================================

Function .onInstSuccess

${IfNot} ${SectionIsSelected} ${SectionInputService}

  ; Install Server/Service
  ; $ServerServiceMode 0 = InputService
  ; $ServerServiceMode 1 = IRServer
  ${If} $ServerServiceMode == 1
    ${LOG_TEXT} "INFO" "Adding IRServer to Autostart..."
    !insertmacro SetAutoRun "IR Server" "$DIR_INSTALL\Input Service\IRServer.exe"
  ${Else}
    ${LOG_TEXT} "INFO" "Installing InputService..."
    ExecWait '"$DIR_INSTALL\Input Service\Input Service.exe" /install'
  ${EndIf}

  ; start Server/Service
  ; $ServerServiceMode 0 = InputService
  ; $ServerServiceMode 1 = IRServer
  ${If} $ServerServiceMode == 1
    ${LOG_TEXT} "INFO" "Starting IRServer..."
    Exec "$DIR_INSTALL\Input Service\IRServer.exe"
  ${Else}
    ${LOG_TEXT} "INFO" "Starting InputService..."
    Exec '"$DIR_INSTALL\Input Service\Input Service.exe" /start'
  ${EndIf}

${EndIf}


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

  ; disable/remove common files for TVServer plugins if all TVServer plugins are unselected
  ${IfNot} ${SectionIsSelected} ${SectionTV3BlasterPlugin}
    !insertmacro UnselectSection ${SectionTV3Common}
  ${Else}
    !insertmacro SelectSection ${SectionTV3Common}
  ${EndIf}

FunctionEnd

;======================================

Function DirectoryPreMP
  ${IfNot} ${SectionIsSelected} ${SectionGroupMP}
    Abort
  ${EndIf}
FunctionEnd

Function DirectoryLeaveMP
  /*
  ; verify if the dir is valid
  ${IfNot} ${FileExists} "$DIR_MEDIAPORTAL\MediaPortal.exe"
    MessageBox MB_OK|MB_ICONEXCLAMATION "MediaPortal is not found in this directory. Please specify the correct path to MediaPortal."
    Abort
  ${EndIf}
  */

  ; refresh MP subdirs, if it user has changed the path again
  ${ReadMediaPortalDirs} $DIR_MEDIAPORTAL
FunctionEnd

;======================================

Function DirectoryPreTV
  ${IfNot} ${SectionIsSelected} ${SectionGroupTV3}
    Abort
  ${EndIf}
FunctionEnd

Function DirectoryLeaveTV
  /*
  ; verify if the dir is valid
  ${IfNot} ${FileExists} "$DIR_MEDIAPORTAL\MediaPortal.exe"
    MessageBox MB_OK|MB_ICONEXCLAMATION "MediaPortal is not found in this directory. Please specify the correct path to MediaPortal."
    Abort
  ${EndIf}
  */
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

  !insertmacro Initialize
  ${un.ReadMediaPortalDirs} $DIR_MEDIAPORTAL

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
;  !insertmacro MUI_DESCRIPTION_TEXT ${SectionGroupMCE}            "$(DESC_SectionGroupMCE)"
;    !insertmacro MUI_DESCRIPTION_TEXT ${SectionMCEBlaster}          "$(DESC_SectionMCEBlaster)"
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