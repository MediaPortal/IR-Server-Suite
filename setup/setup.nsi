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

##### CPU_TYPE
# Uncomment the following line to create a setup in AnyCPU mode
;!define CPU_TYPE "AnyCPU"
# parameter for command line execution: /DCPU_TYPE=AnyCPU
# by default CPU_TYPE is set to "x86"
!ifndef CPU_TYPE
  !define CPU_TYPE "x86"
!endif


#---------------------------------------------------------------------------
# DEVELOPMENT ENVIRONMENT
#---------------------------------------------------------------------------
# path definitions
;!define svn_ROOT_IRSS ".."
;!define svn_InstallScripts "${svn_ROOT_IRSS}\setup\CommonNSIS"
!define svn_InstallScripts "."

!define svn_MPplugins "..\MediaPortal Plugins"


#---------------------------------------------------------------------------
# DEFINES
#---------------------------------------------------------------------------
!define PRODUCT_NAME          "IR Server Suite"
!define PRODUCT_PUBLISHER     "Aaron Dinnage (and-81)"
!define PRODUCT_WEB_SITE      "http://forum.team-mediaportal.com/ir-server-suite-irss-165/"

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

SetCompressor /SOLID /FINAL lzma

; enable logging
!define INSTALL_LOG

; to use default path to logfile, COMMON_APPDATA has to be defined
; default logfile is: "${COMMON_APPDATA}\Logs\install_${VER_MAJOR}.${VER_MINOR}.${VER_REVISION}.${VER_BUILD}.log"
; if you want to set custom path to logfile, uncomment the following line
#!define INSTALL_LOG_FILE "$DESKTOP\install_$(^Name).log"


#---------------------------------------------------------------------------
# VARIABLES
#---------------------------------------------------------------------------
Var DIR_INSTALL
!ifdef MPplugins
Var DIR_MEDIAPORTAL
Var DIR_TVSERVER
!endif

Var PREVIOUS_INSTALLDIR
Var PREVIOUS_VERSION
Var PREVIOUS_VERSION_STATE
Var EXPRESS_UPDATE

Var AutoRunTranslator
Var AutoRunTrayLauncher

Var frominstall


#---------------------------------------------------------------------------
# INCLUDE FILES
#---------------------------------------------------------------------------
!if ${CPU_TYPE} != "x86"
!include x64.nsh
!endif
!include MUI2.nsh
!include Sections.nsh
!include LogicLib.nsh
!include Library.nsh
!include FileFunc.nsh
!include Memento.nsh
!include WinVer.nsh



!include "include\DumpLog.nsh"
;!include "include\FileAssociation.nsh"
!include "include\IrssSystemRegistry.nsh"
!include "include\LanguageMacros.nsh"
!include "include\LoggingMacros.nsh"
!ifdef MPplugins
!include "include\MediaPortalDirectories.nsh"
!endif
!include "include\MediaPortalMacros.nsh"
!include "include\ProcessMacros.nsh"
!include "include\WinVerEx.nsh"

!include "pages\AddRemovePage.nsh"
!include "pages\ServerServiceMode.nsh"
!include "pages\UninstallModePage.nsh"


#---------------------------------------------------------------------------
# INSTALLER ATTRIBUTES
#---------------------------------------------------------------------------
Name "${PRODUCT_NAME}"
OutFile "..\${PRODUCT_NAME} - ${VERSION}.exe"
InstallDir ""

ShowInstDetails show
ShowUninstDetails show
CRCCheck On

BrandingText "${PRODUCT_NAME} - ${VERSION} by ${PRODUCT_PUBLISHER}"


#---------------------------------------------------------------------------
# INSTALLER INTERFACE settings
#---------------------------------------------------------------------------
!define MUI_ABORTWARNING
!define MUI_ICON "Icons\iconGreen.ico"
!define MUI_UNICON "Icons\iconGreen.ico"

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
;!define MUI_FINISHPAGE_RUN      "$DIR_INSTALL\IR Server Configuration\IR Server Configuration.exe"
;!define MUI_FINISHPAGE_RUN_TEXT "Run IR Server Configuration"

!define MUI_UNFINISHPAGE_NOAUTOCLOSE


#---------------------------------------------------------------------------
# INSTALLER INTERFACE
#---------------------------------------------------------------------------
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "..\IR Server Suite\Documentation\LICENSE.GPL"

Page custom PageReinstallMode PageLeaveReinstallMode

!define MUI_PAGE_CUSTOMFUNCTION_PRE PageComponentsPre
!insertmacro MUI_PAGE_COMPONENTS

Page custom PageServerServiceMode PageLeaveServerServiceMode

/*
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
*/

; Main app install path
!define MUI_DIRECTORYPAGE_VARIABLE "$DIR_INSTALL"
!define MUI_PAGE_CUSTOMFUNCTION_PRE PageDirectoryPre
!insertmacro MUI_PAGE_DIRECTORY

!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH


; UnInstaller Interface
!define MUI_PAGE_CUSTOMFUNCTION_PRE un.WelcomePagePre
!insertmacro MUI_UNPAGE_WELCOME
UninstPage custom un.UninstallModePage un.UninstallModePageLeave
;!define MUI_PAGE_CUSTOMFUNCTION_PRE un.ConfirmPagePre
;!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!define MUI_PAGE_CUSTOMFUNCTION_PRE un.FinishPagePre
!insertmacro MUI_UNPAGE_FINISH


#---------------------------------------------------------------------------
# INSTALLER LANGUAGES
#---------------------------------------------------------------------------
!insertmacro LANG_LOAD "English"


#---------------------------------------------------------------------------
# USEFUL MACROS
#---------------------------------------------------------------------------
!macro SectionList MacroName
  ; This macro used to perform operation on multiple sections.
  ; List all of your components in following manner here.
  !insertmacro "${MacroName}" "SectionIRServer"

!ifdef MPplugins
  !insertmacro "${MacroName}" "SectionMPCommon"
    !insertmacro "${MacroName}" "SectionMPControlPlugin"
    !insertmacro "${MacroName}" "SectionMPBlastZonePlugin"
;    !insertmacro "${MacroName}" "SectionTV2BlasterPlugin"

  !insertmacro "${MacroName}" "SectionTV3Common"
    !insertmacro "${MacroName}" "SectionTV3BlasterPlugin"

;  !insertmacro "${MacroName}" "SectionMCEBlaster"
!endif

  #SectionGroupTools
  ;!insertmacro "${MacroName}" "SectionAbstractor"
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

Function RunUninstaller

  ; old (un)installers should be called silently
  ${If} $PREVIOUS_VERSION == ""
    !insertmacro RunUninstaller "silent"
  ${Else}
    !insertmacro RunUninstaller "NoSilent"
  ${EndIf}

FunctionEnd


#---------------------------------------------------------------------------
# SECTIONS and REMOVEMACROS
#---------------------------------------------------------------------------
Section "-prepare"
  DetailPrint "Uninstalling old version ..."

  ; uninstall old version if necessary
  ${If} ${Silent}

    !insertmacro RunUninstaller "silent"

  ${ElseIf} $EXPRESS_UPDATE != ""

    Call RunUninstaller
    BringToFront

  ${EndIf}
  
  
/* OBSOLETE since irss is uninstalled before
  DetailPrint "Preparing to install ..."

  IfFileExists "$DIR_INSTALL\IR Server\IR Server.exe" StopIRServer SkipStopIRServer

StopIRServer:
  ExecWait '"$DIR_INSTALL\IR Server\IR Server.exe" /stop'

SkipStopIRServer:
  Sleep 100
*/
SectionEnd

;======================================

Section "-Core"

  DetailPrint "Setting up paths and installing core files ..."

  ; Use the all users context
  SetShellVarContext all
  SetOverwrite on


  ; Create app data directories
  SetOutPath "$DIR_INSTALL"
  ;File "..\IR Server Suite\Documentation\${PRODUCT_NAME}.chm"


  ; common files
  File "..\IR Server Suite\Common\IrssComms\bin\${Build_Type}\IrssComms.*"
  File "..\IR Server Suite\Common\IrssScheduler\bin\${Build_Type}\IrssScheduler.*"
  File "..\IR Server Suite\Common\IrssUtils\bin\${Build_Type}\IrssUtils.*"
  File "..\IR Server Suite\Common\ShellLink\bin\${Build_Type}\ShellLink.*"


  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"

  ; Create app data directories
  CreateDirectory "$APPDATA\${PRODUCT_NAME}"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Logs"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\IR Commands"

  ; Copy known set top boxes
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Set Top Boxes"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Set Top Boxes"
  SetOverwrite ifnewer
  File /r /x .svn "..\IR Server Suite\Set Top Boxes\*.*"
  SetOverwrite on


SectionEnd

;======================================

${MementoSection} "IR Server" SectionIRServer
  ${LOG_TEXT} "INFO" "Installing IR Server..."
  ${StopService} "IRServer"
  ${KILLPROCESS} "IR Server.exe"
  ${KILLPROCESS} "IR Server Configuration.exe"

  ; Use the all users context
  SetShellVarContext all


  ;not needed anymore since uninstall is launched before
  ;${LOG_TEXT} "INFO" "Removing current IRServer from Autostart..."
  ;!insertmacro RemoveAutoRun "IR Server"
  ;${LOG_TEXT} "INFO" "Uninstalling current IRServer..."
  ;ExecWait '"$DIR_INSTALL\IR Server\IR Server.exe" /uninstall'


  ${LOG_TEXT} "INFO" "Installing IR Server..."
  SetOutPath "$DIR_INSTALL\IR Server"
  File "..\IR Server Suite\IR Server\IR Server\bin\${Build_Type}\*.*"

  ${LOG_TEXT} "INFO" "Installing IR Server Configuration..."
  SetOutPath "$DIR_INSTALL\IR Server Configuration"
  File "..\IR Server Suite\IR Server\IR Server Configuration\bin\${Build_Type}\*.*"

  ${LOG_TEXT} "INFO" "Installing IR Server Plugins..."
  SetOutPath "$DIR_INSTALL\IR Server Plugins"

  File "..\IR Server Suite\IR Server Plugins\Ads Tech PTV-335 Receiver\bin\${Build_Type}\Ads Tech PTV-335 Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\CoolCommand Receiver\bin\${Build_Type}\CoolCommand Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Custom HID Receiver\bin\${Build_Type}\Custom HID Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Direct Input Receiver\bin\${Build_Type}\Direct Input Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\FusionRemote Receiver\bin\${Build_Type}\FusionRemote Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Girder Plugin\bin\${Build_Type}\Girder Plugin.*"
  File "..\IR Server Suite\IR Server Plugins\HCW Receiver\bin\${Build_Type}\HCW Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\IgorPlug Receiver\bin\${Build_Type}\IgorPlug Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Imon USB Receivers\bin\${Build_Type}\Imon USB Receivers.*"
  ;File "..\IR Server Suite\IR Server Plugins\IR501 Receiver\bin\${Build_Type}\IR501 Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\IR507 Receiver\bin\${Build_Type}\IR507 Receiver.*"
  ;File "..\IR Server Suite\IR Server Plugins\Ira Transceiver\bin\${Build_Type}\Ira Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\IRMan Receiver\bin\${Build_Type}\IRMan Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\IRTrans Transceiver\bin\${Build_Type}\IRTrans Transceiver.*"
  ;File "..\IR Server Suite\IR Server Plugins\Keyboard Input\bin\${Build_Type}\Keyboard Input.*"
  File "..\IR Server Suite\IR Server Plugins\LiveDrive Receiver\bin\${Build_Type}\LiveDrive Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\MacMini Receiver\bin\${Build_Type}\MacMini Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Microsoft MCE Transceiver\bin\${Build_Type}\Microsoft MCE Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\Pinnacle Serial Receiver\bin\${Build_Type}\Pinnacle Serial Receiver.*"
  ;File "..\IR Server Suite\IR Server Plugins\RC102 Receiver\bin\${Build_Type}\RC102 Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\RedEye Blaster\bin\${Build_Type}\RedEye Blaster.*"
  File "..\IR Server Suite\IR Server Plugins\Serial IR Blaster\bin\${Build_Type}\Serial IR Blaster.*"
  ;File "..\IR Server Suite\IR Server Plugins\Speech Receiver\bin\${Build_Type}\Speech Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Technotrend Receiver\bin\${Build_Type}\Technotrend Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Technotrend Receiver\bin\${Build_Type}\ttBdaDrvApi_Dll.dll"
  ;File "..\IR Server Suite\IR Server Plugins\Tira Transceiver\bin\${Build_Type}\Tira Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\USB-UIRT Transceiver\bin\${Build_Type}\USB-UIRT Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\Wii Remote Receiver\bin\${Build_Type}\Wii Remote Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\WiimoteLib\bin\${Build_Type}\WiimoteLib.*"
  File "..\IR Server Suite\IR Server Plugins\Windows Message Receiver\bin\${Build_Type}\Windows Message Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\WinLirc Transceiver\bin\${Build_Type}\WinLirc Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\X10 Transceiver\bin\${Build_Type}\X10 Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\X10 Transceiver\bin\${Build_Type}\Interop.X10.dll"
  File "..\IR Server Suite\IR Server Plugins\XBCDRC Receiver\bin\${Build_Type}\XBCDRC Receiver.*"

  ; Create App Data Folder for IR Server configuration files
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\IR Server"
  
  ; Copy Abstract Remote maps
  SetOutPath "$APPDATA\${PRODUCT_NAME}\IR Server\Abstract Remote Maps"
  SetOverwrite ifnewer
  File /r /x .svn "..\IR Server Suite\IR Server\IR Server\Abstract Remote Maps\*.*"
  File "..\IR Server Suite\IR Server\IR Server\RemoteTable.xsd"
  SetOverwrite on

  ; Create start menu shortcut
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\IR Server Configuration.lnk" "$DIR_INSTALL\IR Server Configuration\IR Server Configuration.exe" "" "$DIR_INSTALL\IR Server Configuration\IR Server Configuration.exe" 0

  ; Install Server/Service
  ${If} $ServerServiceMode == "IRServerAsApplication"
    ${LOG_TEXT} "INFO" "Adding IR Server to Autostart..."
    !insertmacro SetAutoRun "IR Server" '"$DIR_INSTALL\IR Server\IR Server.exe"'
  ${Else}
    ${LOG_TEXT} "INFO" "Installing IR Server as Service..."
    ExecWait '"$DIR_INSTALL\IR Server\IR Server.exe" /install'
  ${EndIf}
  !insertmacro SetAutoRun "IR Server Configuration" '"$DIR_INSTALL\IR Server Configuration\IR Server Configuration.exe"'

${MementoSectionEnd}
!macro Remove_${SectionIRServer}
  ${LOG_TEXT} "INFO" "Removing IR Server..."
  ${StopService} "IRServer"
  ${KILLPROCESS} "IR Server.exe"
  ${KILLPROCESS} "IR Server Configuration.exe"

  ${LOG_TEXT} "INFO" "Removing IR Server from Autostart..."
  !insertmacro RemoveAutoRun "IR Server"
  ${LOG_TEXT} "INFO" "Uninstalling IR Server as Service..."
  ExecWait '"$DIR_INSTALL\IR Server\IR Server.exe" /uninstall'
  !insertmacro RemoveAutoRun "IR Server Configuration"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\IR Server Configuration.lnk"

  ; remove files
  RMDir /R /REBOOTOK "$DIR_INSTALL\IR Server"
  RMDir /R /REBOOTOK "$DIR_INSTALL\IR Server Configuration"
  RMDir /R /REBOOTOK "$DIR_INSTALL\IR Server Plugins"
!macroend

;======================================

!ifdef MPplugins

SectionGroup "MediaPortal plugins" SectionGroupMP

Section "-commonMP" SectionMPCommon
  ${LOG_TEXT} "INFO" "Installing common files for MediaPortal plugins..."
  ${KILLPROCESS} "MediaPortal.exe"
  ${KILLPROCESS} "configuration.exe"

  ; Write plugin dll
  SetOutPath "$MPdir.Plugins\Process"
  File "..\MediaPortal Plugins\Common\MPUtils\bin\${Build_Type}\MPUtils.*"
  File "..\IR Server Suite\Common\IrssComms\bin\${Build_Type}\IrssComms.*"
  File "..\IR Server Suite\Common\IrssUtils\bin\${Build_Type}\IrssUtils.*"

  ; Write plugin dll
  SetOutPath "$MPdir.Plugins\Windows"
  File "..\MediaPortal Plugins\Common\MPUtils\bin\${Build_Type}\MPUtils.*"
  File "..\IR Server Suite\Common\IrssComms\bin\${Build_Type}\IrssComms.*"
  File "..\IR Server Suite\Common\IrssUtils\bin\${Build_Type}\IrssUtils.*"
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
  File "..\MediaPortal Plugins\MediaPortal Plugins\MP Control Plugin\bin\${Build_Type}\MPControlPlugin.*"

  ; Write input mapping
  SetOutPath "$MPdir.CustomInputDefault"
  File "..\MediaPortal Plugins\MediaPortal Plugins\MP Control Plugin\InputMapping\MPControlPlugin.xml"

  ; Write app data
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Control Plugin"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\MP Control Plugin"
  SetOverwrite ifnewer
  File /r /x .svn "..\MediaPortal Plugins\MediaPortal Plugins\MP Control Plugin\AppData\*.*"
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
  File "..\MediaPortal Plugins\MediaPortal Plugins\MP Blast Zone Plugin\bin\${Build_Type}\MPBlastZonePlugin.*"

  ; Write app data
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin"
  SetOutPath "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin"
  SetOverwrite off
  File "..\MediaPortal Plugins\MediaPortal Plugins\MP Blast Zone Plugin\AppData\Menu.xml"
  SetOverwrite on

  ; Write skin files
  SetOutPath "$MPdir.Skin\Blue3"
  File /r /x .svn "..\MediaPortal Plugins\MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  SetOutPath "$MPdir.Skin\Blue3wide"
  File /r /x .svn "..\MediaPortal Plugins\MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  ; Create Macro folder
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\MP Blast Zone Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionMPBlastZonePlugin}
  ${LOG_TEXT} "INFO" "Removing MP Blast Zone Plugin..."

  Delete /REBOOTOK "$MPdir.Plugins\Windows\MPBlastZonePlugin.*"
!macroend

;======================================
/*
${MementoUnselectedSection} "TV2 Blaster Plugin" SectionTV2BlasterPlugin
  ${LOG_TEXT} "INFO" "Installing TV2 Blaster Plugin..."

  ; Write plugin dll
  SetOutPath "$MPdir.Plugins\Process"
  File "..\MediaPortal Plugins\MediaPortal Plugins\TV2 Blaster Plugin\bin\${Build_Type}\TV2BlasterPlugin.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV2 Blaster Plugin"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV2 Blaster Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionTV2BlasterPlugin}
  ${LOG_TEXT} "INFO" "Removing TV2 Blaster Plugin..."

  Delete /REBOOTOK "$MPdir.Plugins\Process\TV2BlasterPlugin.*"
!macroend
;======================================
*/

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
  File "..\MediaPortal Plugins\Common\MPUtils\bin\${Build_Type}\MPUtils.*"
  File "..\IR Server Suite\Common\IrssComms\bin\${Build_Type}\IrssComms.*"
  File "..\IR Server Suite\Common\IrssUtils\bin\${Build_Type}\IrssUtils.*"
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

${MementoUnselectedSection} "TV Server Blaster Plugin" SectionTV3BlasterPlugin
  ${LOG_TEXT} "INFO" "Installing TV Server Blaster Plugin..."

  ; Write plugin dll
  SetOutPath "$DIR_TVSERVER\Plugins"
  File "..\MediaPortal Plugins\TVServer plugins\TV3 Blaster Plugin\bin\${Build_Type}\TV3BlasterPlugin.*"

  ; Create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV3 Blaster Plugin"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\TV3 Blaster Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionTV3BlasterPlugin}
  ${If} ${FileExists} "$DIR_TVSERVER\Plugins\TV3BlasterPlugin.*"

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
  File "..\IR Server Suite\Applications\Media Center Blaster\bin\${Build_Type}\*.*"

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

;======================================
*/

!endif

SectionGroup "Tools" SectionGroupTools

${MementoSection} "Abstractor" SectionAbstractor
  ${LOG_TEXT} "INFO" "Installing Abstractor..."
  ${KILLPROCESS} "Abstractor.exe"

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Abstractor\bin\${Build_Type}\Abstractor.*"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Abstractor.lnk" "$DIR_INSTALL\Abstractor.exe" "" "$DIR_INSTALL\Abstractor.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionAbstractor}
  ${LOG_TEXT} "INFO" "Removing Abstractor..."
  ${KILLPROCESS} "Abstractor.exe"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Abstractor.lnk"

  ; remove files
  Delete "$DIR_INSTALL\Abstractor.*"
!macroend

;======================================

${MementoSection} "Debug Client" SectionDebugClient
  ${LOG_TEXT} "INFO" "Installing Debug Client..."
  ${KILLPROCESS} "DebugClient.exe"

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Debug Client\bin\${Build_Type}\DebugClient.*"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Debug Client"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Debug Client.lnk" "$DIR_INSTALL\DebugClient.exe" "" "$DIR_INSTALL\DebugClient.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionDebugClient}
  ${LOG_TEXT} "INFO" "Removing Debug Client..."
  ${KILLPROCESS} "DebugClient.exe"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Debug Client.lnk"

  ; remove files
  Delete "$DIR_INSTALL\DebugClient.*"
!macroend

;======================================

${MementoSection} "IR File Tool" SectionIRFileTool
  ${LOG_TEXT} "INFO" "Installing IR File Tool..."
  ${KILLPROCESS} "IRFileTool.exe"

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\IR File Tool\bin\${Build_Type}\IRFileTool.*"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\IR File Tool"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\IR File Tool.lnk" "$DIR_INSTALL\IRFileTool.exe" "" "$DIR_INSTALL\IRFileTool.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionIRFileTool}
  ${LOG_TEXT} "INFO" "Removing IR File Tool..."
  ${KILLPROCESS} "IRFileTool.exe"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\IR File Tool.lnk"

  ; remove files
  Delete "$DIR_INSTALL\IRFileTool.*"
!macroend

;======================================

${MementoSection} "Keyboard Input Relay" SectionKeyboardInputRelay
  ${LOG_TEXT} "INFO" "Installing Keyboard Input Relay..."
  ${KILLPROCESS} "KeyboardInputRelay.exe"

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Keyboard Input Relay\bin\${Build_Type}\KeyboardInputRelay.*"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Keyboard Input Relay"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Keyboard Input Relay.lnk" "$DIR_INSTALL\KeyboardInputRelay.exe" "" "$DIR_INSTALL\KeyboardInputRelay.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionKeyboardInputRelay}
  ${LOG_TEXT} "INFO" "Removing Keyboard Input Relay..."
  ${KILLPROCESS} "KeyboardInputRelay.exe"

  ; remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Keyboard Input Relay"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Keyboard Input Relay.lnk"

  ; remove files
  Delete "$DIR_INSTALL\KeyboardInputRelay.*"
!macroend

;======================================

${MementoSection} "Translator" SectionTranslator
  ${LOG_TEXT} "INFO" "Installing Translator..."
  ${KILLPROCESS} "Translator.exe"

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Translator\bin\${Build_Type}\Translator.*"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator\Macro"
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Translator\Default Settings"

  ; Copy in default settings files  
  SetOutPath "$APPDATA\${PRODUCT_NAME}\Translator\Default Settings"
  File "..\IR Server Suite\Applications\Translator\Default Settings\*.xml"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Translator.lnk" "$DIR_INSTALL\Translator.exe" "" "$DIR_INSTALL\Translator.exe" 0


  ; check if Translator is an autorun app
  ${If} $AutoRunTranslator == 1
    !insertmacro SetAutoRun "Translator" "$DIR_INSTALL\Translator.exe"
  ${EndIf}
${MementoSectionEnd}
!macro Remove_${SectionTranslator}
  ${LOG_TEXT} "INFO" "Removing Translator..."
  ${KILLPROCESS} "Translator.exe"

  ; remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Translator"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Translator.lnk"

  ; remove files
  Delete "$DIR_INSTALL\Translator.*"
!macroend

;======================================

${MementoSection} "Tray Launcher" SectionTrayLauncher
  ${LOG_TEXT} "INFO" "Installing Tray Launcher..."
  ${KILLPROCESS} "TrayLauncher.exe"

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Tray Launcher\bin\${Build_Type}\TrayLauncher.*"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Tray Launcher.lnk" "$DIR_INSTALL\TrayLauncher.exe" "" "$DIR_INSTALL\TrayLauncher.exe" 0


  ; check if TrayLauncher is an autorun app
  ${If} $AutoRunTrayLauncher == 1
    !insertmacro SetAutoRun "Tray Launcher" "$DIR_INSTALL\TrayLauncher.exe"
  ${EndIf}
${MementoSectionEnd}
!macro Remove_${SectionTrayLauncher}
  ${LOG_TEXT} "INFO" "Removing Tray Launcher..."
  ${KILLPROCESS} "TrayLauncher.exe"

  ; remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Tray Launcher"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Tray Launcher.lnk"

  ; remove files
  Delete "$DIR_INSTALL\TrayLauncher.*"
!macroend

;======================================

${MementoSection} "Virtual Remote" SectionVirtualRemote
  ${LOG_TEXT} "INFO" "Installing Virtual Remote, Skin Editor, Smart Device versions, and Web Remote..."
  ${KILLPROCESS} "VirtualRemote.exe"
  ${KILLPROCESS} "VirtualRemoteSkinEditor.exe"
  ${KILLPROCESS} "WebRemote.exe"

  ; Installing Virtual Remote and Web Remote
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Virtual Remote\bin\${Build_Type}\VirtualRemote.*"
  File "..\IR Server Suite\Applications\Virtual Remote Skin Editor\bin\${Build_Type}\VirtualRemoteSkinEditor.*"
  File "..\IR Server Suite\Applications\Web Remote\bin\${Build_Type}\WebRemote.*"

  ; Installing skins
  SetOutPath "$DIR_INSTALL\Virtual Remote\Skins"
  File "..\IR Server Suite\Applications\Virtual Remote\Skins\*.*"

  ; Installing Virtual Remote for Smart Devices
  SetOutPath "$DIR_INSTALL\Virtual Remote\Smart Devices"
  File "..\Virtual Remote\Applications\Virtual Remote (PocketPC2003) Installer\${Build_Type}\*.cab"
  File "..\Virtual Remote\Applications\Virtual Remote (Smartphone2003) Installer\${Build_Type}\*.cab"
  File "..\Virtual Remote\Applications\Virtual Remote (WinCE5) Installer\${Build_Type}\*.cab"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Virtual Remote"

  ; create start menu shortcuts
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote.lnk" "$DIR_INSTALL\VirtualRemote.exe" "" "$DIR_INSTALL\VirtualRemote.exe" 0
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote Skin Editor.lnk" "$DIR_INSTALL\VirtualRemoteSkinEditor.exe" "" "$DIR_INSTALL\VirtualRemoteSkinEditor.exe" 0
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote for Smart Devices.lnk" "$DIR_INSTALL\Virtual Remote\Smart Devices"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Web Remote.lnk" "$DIR_INSTALL\WebRemote.exe" "" "$DIR_INSTALL\WebRemote.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionVirtualRemote}
  ${LOG_TEXT} "INFO" "Removing Virtual Remote, Skin Editor, Smart Device versions, and Web Remote..."
  ${KILLPROCESS} "VirtualRemote.exe"
  ${KILLPROCESS} "VirtualRemoteSkinEditor.exe"
  ${KILLPROCESS} "WebRemote.exe"

  ; remove start menu shortcuts
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote Skin Editor.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Virtual Remote for Smart Devices.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\Web Remote.lnk"

  ; remove files
  Delete "$DIR_INSTALL\VirtualRemote.*"
  Delete "$DIR_INSTALL\VirtualRemoteSkinEditor.*"
  Delete "$DIR_INSTALL\WebRemote.*"
  RMDir /R /REBOOTOK "$DIR_INSTALL\Virtual Remote"
!macroend

SectionGroupEnd

;======================================

SectionGroup "CommandLine Tools" SectionGroupCmdLineTools

${MementoUnselectedSection} "IR Blast" SectionIRBlast
  ${LOG_TEXT} "INFO" "Installing IR Blast..."

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\IR Blast\bin\${Build_Type}\IRBlast.*"
  File "..\IR Server Suite\Applications\IR Blast (No Window)\bin\${Build_Type}\IRBlast-NoWindow.*"

${MementoSectionEnd}
!macro Remove_${SectionIRBlast}
  ${LOG_TEXT} "INFO" "Removing IR Blast..."

  ; remove files
  Delete "$DIR_INSTALL\IRBlast.*"
  Delete "$DIR_INSTALL\IRBlast-NoWindow.*"
!macroend

;======================================

${MementoUnselectedSection} "Dreambox Tuner" SectionDboxTuner
  ${LOG_TEXT} "INFO" "Installing Dreambox Tuner..."

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Dbox Tuner\bin\${Build_Type}\DboxTuner.*"

  ; create folders
  CreateDirectory "$APPDATA\${PRODUCT_NAME}\Dbox Tuner"

${MementoSectionEnd}
!macro Remove_${SectionDboxTuner}
  ${LOG_TEXT} "INFO" "Removing Dreambox Tuner..."

  ; remove files
  Delete "$DIR_INSTALL\DboxTuner.*"
!macroend

;======================================

${MementoUnselectedSection} "Hauppauge PVR Tuner" SectionHcwPvrTuner
  ${LOG_TEXT} "INFO" "Installing Hauppauge PVR Tuner..."

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\HCW PVR Tuner\bin\${Build_Type}\HcwPvrTuner.*"

${MementoSectionEnd}
!macro Remove_${SectionHcwPvrTuner}
  ${LOG_TEXT} "INFO" "Removing Hauppauge PVR Tuner..."

  ; remove files
  Delete "$DIR_INSTALL\HcwPvrTuner.*"
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

!ifdef MPplugins
  ; start tvservice, if it was closed before
  !insertmacro StartTVService
!endif

  ; removing tve2 blaster
  Delete "$MPdir.Plugins\Process\TV2BlasterPlugin.dll"

  ; Use the all users context
  SetShellVarContext all

  ; Create start menu shortcuts
  WriteINIStr "$SMPROGRAMS\${PRODUCT_NAME}\Documentation.url"  "InternetShortcut" "URL" "http://www.team-mediaportal.com/manual/IRServerSuite"
  WriteINIStr "$SMPROGRAMS\${PRODUCT_NAME}\Website.url"  "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Log Files.lnk" "$APPDATA\${PRODUCT_NAME}\Logs"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe" "" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"

  
  ; Write registry settings
  WriteRegStr HKLM "${REG_UNINSTALL}" "ServerServiceMode" "$ServerServiceMode"

  ; Write the installation paths into the registry
  WriteRegStr HKLM "Software\${PRODUCT_NAME}" "Install_Dir" "$DIR_INSTALL"
!ifdef MPplugins
  WriteRegStr HKLM "Software\${PRODUCT_NAME}" "MediaPortal_Dir" "$DIR_MEDIAPORTAL"
  WriteRegStr HKLM "Software\${PRODUCT_NAME}" "TVServer_Dir" "$DIR_TVSERVER"
!endif

  ; Write the product version into the registry
  WriteRegDWORD HKLM "${REG_UNINSTALL}" "VersionMajor"    "${VER_MAJOR}"
  WriteRegDWORD HKLM "${REG_UNINSTALL}" "VersionMinor"    "${VER_MINOR}"
  WriteRegDWORD HKLM "${REG_UNINSTALL}" "VersionRevision" "${VER_REVISION}"
  WriteRegDWORD HKLM "${REG_UNINSTALL}" "VersionBuild"    "${VER_BUILD}"

  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "${REG_UNINSTALL}" "DisplayName"     "${PRODUCT_NAME}"
  WriteRegStr HKLM "${REG_UNINSTALL}" "DisplayVersion"  "${VERSION}"
  WriteRegStr HKLM "${REG_UNINSTALL}" "Publisher"       "${PRODUCT_PUBLISHER}"
  WriteRegStr HKLM "${REG_UNINSTALL}" "URLInfoAbout"    "${PRODUCT_WEB_SITE}"
  WriteRegStr HKLM "${REG_UNINSTALL}" "DisplayIcon"     "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"
  WriteRegStr HKLM "${REG_UNINSTALL}" "UninstallString" "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"
  WriteRegDWORD HKLM "${REG_UNINSTALL}" "NoModify" 1
  WriteRegDWORD HKLM "${REG_UNINSTALL}" "NoRepair" 1

  ; Write the uninstaller
  WriteUninstaller "$DIR_INSTALL\Uninstall ${PRODUCT_NAME}.exe"

  ; Store the install log
  StrCpy $0 "$APPDATA\${PRODUCT_NAME}\Logs\Install.log"
  Push $0
  Call DumpLog

SectionEnd

;======================================

Section "Uninstall"
  DetailPrint "DIR_INSTALL: $DIR_INSTALL"
!ifdef MPplugins
  DetailPrint "DIR_MEDIAPORTAL: $DIR_MEDIAPORTAL"
  DetailPrint "DIR_TVSERVER: $DIR_TVSERVER"
!endif


  ; Use the all users context
  SetShellVarContext all

  ; First removes all optional components
  !insertmacro SectionList "RemoveSection"

!ifdef MPplugins
  ; start tvservice, if it was closed before
  !insertmacro StartTVService
!endif

  ; do not remove anything in appdata
  ;DetailPrint "Removing Set Top Box presets ..."
  ;RMDir /R "$APPDATA\${PRODUCT_NAME}\Set Top Boxes"

  ; Remove files and uninstaller
  DetailPrint "Removing program files ..."
  RMDir /R /REBOOTOK "$DIR_INSTALL"

  DetailPrint "Removing start menu shortcuts ..."
  RMDir /R "$SMPROGRAMS\${PRODUCT_NAME}"
  
  ; Remove registry keys
  DetailPrint "Removing registry keys ..."
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  DeleteRegKey HKLM "Software\${PRODUCT_NAME}"

  ${If} $UnInstallMode == 1

    ${LOG_TEXT} "INFO" "Removing User Settings"
    RMDir /r "$APPDATA\${PRODUCT_NAME}"

  ${EndIf}

  ${If} $frominstall == 1
    Quit
  ${EndIf}
SectionEnd


#---------------------------------------------------------------------------
# SOME MACROS AND FUNCTIONS
#---------------------------------------------------------------------------
!ifdef MPplugins
!macro GetMediaPortalPaths

!if ${CPU_TYPE} != "x86"
  ${If} ${RunningX64}
    SetRegView 32
    ${EnableX64FSRedirection}
  ${Endif}
!endif

  ; Get MediaPortal installation directory ...
  !insertmacro MP_GET_INSTALL_DIR $DIR_MEDIAPORTAL
  ${If} $DIR_MEDIAPORTAL != ""
    ${ReadMediaPortalDirs} $DIR_MEDIAPORTAL
  ${Endif}

  ; Get MediaPortal TV Server installation directory ...
  !insertmacro TVSERVER_GET_INSTALL_DIR $DIR_TVSERVER

!if ${CPU_TYPE} != "x86"
  ${If} ${RunningX64}
    SetRegView 64
    ${DisableX64FSRedirection}
  ${Endif}
!endif

  !macroend
!endif

Function ReadPreviousSettings
!if ${CPU_TYPE} != "x86"
  ${If} ${RunningX64}
    SetRegView 64
    ${DisableX64FSRedirection}
  ${Endif}
!endif

  ; read and analyze previous version
  !insertmacro ReadPreviousVersion

  ; read previous used directories
  ReadRegStr $PREVIOUS_INSTALLDIR HKLM "Software\${PRODUCT_NAME}" "Install_Dir"
!ifdef MPplugins
  #ReadRegStr $DIR_MEDIAPORTAL HKLM "Software\${PRODUCT_NAME}" "MediaPortal_Dir"
  #ReadRegStr $DIR_TVSERVER HKLM "Software\${PRODUCT_NAME}" "TVServer_Dir"
  !insertmacro GetMediaPortalPaths    ; if not installed, path == ""
!endif
  
  ; read previous settings
  ReadRegStr $PREVIOUS_ServerServiceMode HKLM "${REG_UNINSTALL}" "ServerServiceMode"


  ; check if Translator is an autorun app
  ${If} ${IsAutoRun} "Translator"
    StrCpy $AutoRunTranslator 1
  ${Else}
    StrCpy $AutoRunTranslator 0
  ${EndIf}

  ; check if TrayLauncher is an autorun app
  ${If} ${IsAutoRun} "Tray Launcher"
    StrCpy $AutoRunTrayLauncher 1
  ${Else}
    StrCpy $AutoRunTrayLauncher 0
  ${EndIf}
FunctionEnd

Function LoadPreviousSettings
  ; reset DIR_INSTALL
  ${If} $PREVIOUS_INSTALLDIR != ""
    StrCpy $DIR_INSTALL "$PREVIOUS_INSTALLDIR"
  ${Else}
    StrCpy $DIR_INSTALL "$PROGRAMFILES\${PRODUCT_NAME}"
  ${EndIf}

  ; reset ServerServiceMode
  ${If} $PREVIOUS_ServerServiceMode == "IRServerAsApplication"
  ${OrIf} $PREVIOUS_ServerServiceMode == "IRServerAsService"
    StrCpy $ServerServiceMode $PREVIOUS_ServerServiceMode
  ${Else}
    StrCpy $ServerServiceMode "IRServerAsService"
  ${EndIf}


  ; reset previous component selection from registry
  ${MementoSectionRestore}

!ifdef MPplugins
  ; set sections, according to possible selections
  ${If} "$DIR_MEDIAPORTAL" != ""
    !insertmacro EnableSection "${SectionMPControlPlugin}" "MP Control Plugin"
    !insertmacro EnableSection "${SectionMPBlastZonePlugin}" "MP Blast Zone Plugin"
    ;!insertmacro EnableSection "${SectionTV2BlasterPlugin}" "TV2 Blaster Plugin"
    !insertmacro EnableSection "${SectionGroupMP}" "MediaPortal plugins"
  ${else}
    !insertmacro DisableSection "${SectionMPControlPlugin}" "MP Control Plugin" " "
    !insertmacro DisableSection "${SectionMPBlastZonePlugin}" "MP Blast Zone Plugin" " "
    ;!insertmacro DisableSection "${SectionTV2BlasterPlugin}" "TV2 Blaster Plugin" " "
    !insertmacro DisableSection "${SectionGroupMP}" "MediaPortal plugins" " ($(TEXT_MP_NOT_INSTALLED))"
  ${Endif}

  ${If} "$DIR_TVSERVER" != ""
    !insertmacro EnableSection "${SectionTV3BlasterPlugin}" "TV Server Blaster Plugin"
    !insertmacro EnableSection "${SectionGroupTV3}" "TV Server plugins"
  ${else}
    !insertmacro DisableSection "${SectionTV3BlasterPlugin}" "TV Server Blaster Plugin" " "
    !insertmacro DisableSection "${SectionGroupTV3}" "TV Server plugins" " ($(TEXT_TVSERVER_NOT_INSTALLED))"
  ${Endif}
!endif

  ; update component selection
  Call .onSelChange
FunctionEnd


#---------------------------------------------------------------------------
# INSTALLER CALLBACKS
#---------------------------------------------------------------------------
Function .onInit
  ${LOG_OPEN}

  Call ReadPreviousSettings
  Call LoadPreviousSettings

FunctionEnd

Function .onInstFailed
  ${LOG_CLOSE}
FunctionEnd

Function .onInstSuccess

${If} ${SectionIsSelected} ${SectionIRServer}

  ; start Server/Service
  ${If} $ServerServiceMode == "IRServerAsApplication"
    ${LOG_TEXT} "INFO" "Starting IR Server..."
    Exec "$DIR_INSTALL\IR Server\IR Server.exe"
  ${Else}
    ${LOG_TEXT} "INFO" "Starting IR Server..."
    Exec '"$DIR_INSTALL\IR Server\IR Server.exe" /start'
  ${EndIf}
  Exec "$DIR_INSTALL\IR Server Configuration\IR Server Configuration.exe"

${EndIf}


  ${LOG_CLOSE}
FunctionEnd

Function .onSelChange

!ifdef MPplugins
  ; disable/remove common files for MediaPortal plugins if all MediaPortal plugins are unselected
  ${IfNot} ${SectionIsSelected} ${SectionMPControlPlugin}
  ${AndIfNot} ${SectionIsSelected} ${SectionMPBlastZonePlugin}
;  ${AndIfNot} ${SectionIsSelected} ${SectionTV2BlasterPlugin}
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
!endif

FunctionEnd

;======================================

Function PageComponentsPre
  ; skip page if previous settings are used for update
  ${If} $EXPRESS_UPDATE == 1
    Abort
  ${EndIf}
FunctionEnd

/*
Function DirectoryPreMP
  ; skip page if no MediaPortal plugins are selected
  ${IfNot} ${SectionIsSelected} ${SectionGroupMP}
    Abort
  ${EndIf}

  ; skip page if previous settings are used for update and DIR_MEDIAPORTAL is valid
  ${If} $EXPRESS_UPDATE == 1
  ${AndIf} $DIR_MEDIAPORTAL != ""
    Abort
  ${EndIf}
FunctionEnd

Function DirectoryLeaveMP
  ; verify if the dir is valid
  ${IfNot} ${FileExists} "$DIR_MEDIAPORTAL\MediaPortal.exe"
    MessageBox MB_OK|MB_ICONEXCLAMATION "MediaPortal is not found in this directory. Please specify the correct path to MediaPortal."
    Abort
  ${EndIf}

  ; refresh MP subdirs, if user has changed the path again
  ${ReadMediaPortalDirs} $DIR_MEDIAPORTAL
FunctionEnd

Function DirectoryPreTV
  ; skip page if no TvServer plugins are selected
  ${IfNot} ${SectionIsSelected} ${SectionGroupTV3}
    Abort
  ${EndIf}

  ; skip page if previous settings are used for update and DIR_TVSERVER is valid
  ${If} $EXPRESS_UPDATE == 1
  ${AndIf} $DIR_TVSERVER != ""
    Abort
  ${EndIf}
FunctionEnd

Function DirectoryLeaveTV
  ; verify if the dir is valid
  ${IfNot} ${FileExists} "$DIR_TVSERVER\TvService.exe"
    MessageBox MB_OK|MB_ICONEXCLAMATION "TvServer is not found in this directory. Please specify the correct path to TVServer."
    Abort
  ${EndIf}
FunctionEnd
*/

Function PageDirectoryPre
  ; skip page if previous settings are used for update
  ${If} $EXPRESS_UPDATE == 1
    Abort
  ${EndIf}
FunctionEnd

/*
Function FinishShow
  ; This function is called, after the Finish Page creation is finished

  ; It checks, if the Server has been selected and only displays the run checkbox in this case
  ${IfNot} ${SectionIsSelected} SectionIRServer
      SendMessage $mui.FinishPage.Run ${BM_CLICK} 0 0
      ShowWindow  $mui.FinishPage.Run ${SW_HIDE}
  ${EndIf}
FunctionEnd
*/


#---------------------------------------------------------------------------
# UNINSTALLER CALLBACKS
#---------------------------------------------------------------------------
Function un.onInit
!if ${CPU_TYPE} != "x86"
  ${If} ${RunningX64}
    SetRegView 64
    ${DisableX64FSRedirection}
  ${Endif}
!endif

  ReadRegStr $DIR_INSTALL HKLM "Software\${PRODUCT_NAME}" "Install_Dir"
!ifdef MPplugins
  ReadRegStr $DIR_MEDIAPORTAL HKLM "Software\${PRODUCT_NAME}" "MediaPortal_Dir"
  ReadRegStr $DIR_TVSERVER HKLM "Software\${PRODUCT_NAME}" "TVServer_Dir"
  ${un.ReadMediaPortalDirs} $DIR_MEDIAPORTAL
!endif

  ${un.InitCommandlineParameter}
  ${un.ReadCommandlineParameter} "frominstall"

FunctionEnd

;======================================

Function un.WelcomePagePre

  ${If} $frominstall == 1
    Abort
  ${EndIf}

FunctionEnd

Function un.ConfirmPagePre

  ${If} $frominstall == 1
    Abort
  ${EndIf}

FunctionEnd

Function un.FinishPagePre

  ${If} $frominstall == 1
    SetRebootFlag false
    Abort
  ${EndIf}

FunctionEnd


#---------------------------------------------------------------------------
# SECTION DESCRIPTIONS
#---------------------------------------------------------------------------
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionIRServer}        "$(DESC_SectionIRServer)"

!ifdef MPplugins
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionGroupMP}             "$(DESC_SectionGroupMP)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionMPControlPlugin}     "$(DESC_SectionMPControlPlugin)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionMPBlastZonePlugin}   "$(DESC_SectionMPBlastZonePlugin)"
;    !insertmacro MUI_DESCRIPTION_TEXT ${SectionTV2BlasterPlugin}    "$(DESC_SectionTV2BlasterPlugin)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionGroupTV3}            "$(DESC_SectionGroupTV3)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionTV3BlasterPlugin}    "$(DESC_SectionTV3BlasterPlugin)"
;  !insertmacro MUI_DESCRIPTION_TEXT ${SectionGroupMCE}            "$(DESC_SectionGroupMCE)"
;    !insertmacro MUI_DESCRIPTION_TEXT ${SectionMCEBlaster}          "$(DESC_SectionMCEBlaster)"
!endif

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
