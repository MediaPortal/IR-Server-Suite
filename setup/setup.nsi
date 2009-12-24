#region Copyright (C) 2005-2009 Team MediaPortal

/*
 *  Copyright (C) 2005-2009 Team MediaPortal
 *  http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

#endregion

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
!define PRODUCT_PUBLISHER     "Team MediaPortal"
!define PRODUCT_WEB_SITE      "http://forum.team-mediaportal.com/ir-server-suite-irss-165/"

!define REG_UNINSTALL         "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define MEMENTO_REGISTRY_ROOT HKLM
!define MEMENTO_REGISTRY_KEY  "${REG_UNINSTALL}"
!define COMMON_APPDATA        "$APPDATA\${PRODUCT_NAME}"
!define STARTMENU_GROUP       "$SMPROGRAMS\${PRODUCT_NAME}"

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

!ifdef MPplugins
  !define MIN_VERSION_MP "1.0.2.0"
  !define MIN_VERSION_TVSERVER "1.0.4.24253"
!endif


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
!include "include\SystemRegistry.nsh"
!include "include\LanguageMacros.nsh"
!define INSTALL_LOG_DIR "${COMMON_APPDATA}\logs"
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
Name          "${PRODUCT_NAME}"
BrandingText  "${PRODUCT_NAME} ${VERSION} by ${PRODUCT_PUBLISHER}"
OutFile       "..\${PRODUCT_NAME} - ${VERSION}.exe"
InstallDir    ""

XPStyle on
RequestExecutionLevel admin
;ShowInstDetails show
;ShowUninstDetails show
CRCCheck on


#---------------------------------------------------------------------------
# INSTALLER INTERFACE settings
#---------------------------------------------------------------------------
!define MUI_ABORTWARNING
!define MUI_ICON    "Icons\iconGreen.ico"
!define MUI_UNICON  "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP            "images\header.bmp"
#!if ${VER_BUILD} == 0
  !define MUI_WELCOMEFINISHPAGE_BITMAP    "images\wizard.bmp"
  !define MUI_UNWELCOMEFINISHPAGE_BITMAP  "images\wizard.bmp"
#!else
#  !define MUI_WELCOMEFINISHPAGE_BITMAP    "images\wizard-svn.bmp"
#  !define MUI_UNWELCOMEFINISHPAGE_BITMAP  "images\wizard-svn.bmp"
#!endif
!define MUI_HEADERIMAGE_RIGHT

!define MUI_COMPONENTSPAGE_SMALLDESC
!define MUI_FINISHPAGE_NOAUTOCLOSE

!define MUI_FINISHPAGE_RUN      "$DIR_INSTALL\IR Server Configuration.exe"
!define MUI_FINISHPAGE_RUN_TEXT "Run IR Server Configuration"

!define MUI_UNFINISHPAGE_NOAUTOCLOSE


#---------------------------------------------------------------------------
# INSTALLER INTERFACE
#---------------------------------------------------------------------------
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "..\IR Server Suite\Documentation\LICENSE.GPL"

!define MUI_PAGE_CUSTOMFUNCTION_LEAVE PageReinstallLeave
!insertmacro MUI_PAGE_REINSTALL

!define MUI_PAGE_CUSTOMFUNCTION_PRE PageComponentsPre
!define MUI_PAGE_CUSTOMFUNCTION_LEAVE PageComponentsLeave
!insertmacro MUI_PAGE_COMPONENTS

Page custom PageServerServiceMode PageLeaveServerServiceMode

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

  !insertmacro "${MacroName}" "SectionTV3Common"
    !insertmacro "${MacroName}" "SectionTV3BlasterPlugin"
!endif

  #SectionGroupTools
  !insertmacro "${MacroName}" "SectionAbstractor"
  !insertmacro "${MacroName}" "SectionDebugClient"
  !insertmacro "${MacroName}" "SectionIRFileTool"
  !insertmacro "${MacroName}" "SectionKeyboardInputRelay"
  !insertmacro "${MacroName}" "SectionTranslator"
  !insertmacro "${MacroName}" "SectionVirtualRemote"
  
  #SectionGroupCmdLineTools
  !insertmacro "${MacroName}" "SectionIRBlast"
  !insertmacro "${MacroName}" "SectionDboxTuner"
  !insertmacro "${MacroName}" "SectionHcwPvrTuner"
!macroend

!macro ShutdownRunningMediaPortalApplications
  ${LOG_TEXT} "INFO" "Terminating processes..."

  ${StopService} "IRServer"
  ${KILLPROCESS} "IR Server.exe"
  ${KILLPROCESS} "IR Server Configuration.exe"
  ${KILLPROCESS} "IR Server Tray.exe"

  ${KILLPROCESS} "Abstractor.exe"
  ${KILLPROCESS} "DebugClient.exe"
  ${KILLPROCESS} "IRFileTool.exe"
  ${KILLPROCESS} "KeyboardInputRelay.exe"
  ${KILLPROCESS} "Translator.exe"

  ${KILLPROCESS} "VirtualRemote.exe"
  ${KILLPROCESS} "VirtualRemoteSkinEditor.exe"
  ${KILLPROCESS} "WebRemote.exe"

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

  ; close all IRSS related apps
  !insertmacro ShutdownRunningMediaPortalApplications

  ; uninstall old version if necessary
  ${If} ${Silent}

    !insertmacro RunUninstaller "silent"

  ${ElseIf} $EXPRESS_UPDATE != ""

    Call RunUninstaller
    BringToFront

  ${EndIf}

SectionEnd

;======================================

Section "-Core"
  ${LOG_TEXT} "INFO" "Setting up paths and installing core files ..."

  ; use the all users context
  SetShellVarContext all
  SetOverwrite on

  ; common files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Common\IrssComms\bin\${Build_Type}\IrssComms.*"
  File "..\IR Server Suite\Common\IrssScheduler\bin\${Build_Type}\IrssScheduler.*"
  File "..\IR Server Suite\Common\IrssUtils\bin\${Build_Type}\IrssUtils.*"
  File "..\IR Server Suite\Common\ShellLink\bin\${Build_Type}\ShellLink.*"

  ; startmenu needs to be created before creating shortcuts
  CreateDirectory "${STARTMENU_GROUP}"

  ; create app data directories
  CreateDirectory "${COMMON_APPDATA}"
  CreateDirectory "${COMMON_APPDATA}\Logs"
  CreateDirectory "${COMMON_APPDATA}\IR Commands"

  ; Copy known set top boxes
  CreateDirectory "${COMMON_APPDATA}\Set Top Boxes"
  SetOutPath "${COMMON_APPDATA}\Set Top Boxes"
  SetOverwrite ifnewer
  File /r /x .svn "..\IR Server Suite\Set Top Boxes\*.*"
  SetOverwrite on
SectionEnd

;======================================

${MementoSection} "IR Server" SectionIRServer
  ${LOG_TEXT} "INFO" "Installing IR Server..."

  ; use the all users context
  SetShellVarContext all

  SetOutPath "$DIR_INSTALL"
  ${LOG_TEXT} "INFO" "Installing IR Server..."
  File "..\IR Server Suite\IR Server\IR Server\bin\${Build_Type}\IR Server.*"
  File "..\IR Server Suite\IR Server\IR Server\Install.cmd"
  File "..\IR Server Suite\IR Server\IR Server\Uninstall.cmd"

  ${LOG_TEXT} "INFO" "Installing IR Server Configuration..."
  File "..\IR Server Suite\IR Server\IR Server Configuration\bin\${Build_Type}\IR Server Configuration.*"
  File "..\IR Server Suite\IR Server\SourceGrid\DevAge*"
  File "..\IR Server Suite\IR Server\SourceGrid\SourceGrid*"

  ${LOG_TEXT} "INFO" "Installing IR Server Tray..."
  File "..\IR Server Suite\IR Server\IR Server Tray\bin\${Build_Type}\IR Server Tray.*"

  File "..\IR Server Suite\IR Server\IRServer.Shared\bin\${Build_Type}\IRServer.Shared.*"
  File "..\IR Server Suite\IR Server Plugins\IR Server Plugin Interface\bin\${Build_Type}\IRServerPluginInterface.*"

  ${LOG_TEXT} "INFO" "Installing IR Server Plugins..."
  SetOutPath "$DIR_INSTALL\IR Server Plugins"

  File "..\IR Server Suite\IR Server Plugins\Ads Tech PTV-335 Receiver\bin\${Build_Type}\Ads Tech PTV-335 Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\CoolCommand Receiver\bin\${Build_Type}\CoolCommand Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Custom HID Receiver\bin\${Build_Type}\Custom HID Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Direct Input Receiver\bin\${Build_Type}\Direct Input Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\FireDTV Receiver\bin\${Build_Type}\FireDTV Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\FusionRemote Receiver\bin\${Build_Type}\FusionRemote Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Girder Plugin\bin\${Build_Type}\Girder Plugin.*"
  File "..\IR Server Suite\IR Server Plugins\HCW Receiver\bin\${Build_Type}\HCW Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\IgorPlug Receiver\bin\${Build_Type}\IgorPlug Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Imon USB Receivers\bin\${Build_Type}\Imon USB Receivers.*"
  File "..\IR Server Suite\IR Server Plugins\IR501 Receiver\bin\${Build_Type}\IR501 Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\IR507 Receiver\bin\${Build_Type}\IR507 Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\IRMan Receiver\bin\${Build_Type}\IRMan Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\IRTrans Transceiver\bin\${Build_Type}\IRTrans Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\Keyboard Input\bin\${Build_Type}\Keyboard Input.*"
  File "..\IR Server Suite\IR Server Plugins\LiveDrive Receiver\bin\${Build_Type}\LiveDrive Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\MacMini Receiver\bin\${Build_Type}\MacMini Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Microsoft MCE Transceiver\bin\${Build_Type}\Microsoft MCE Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\Pinnacle Serial Receiver\bin\${Build_Type}\Pinnacle Serial Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\RC102 Receiver\bin\${Build_Type}\RC102 Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\RedEye Blaster\bin\${Build_Type}\RedEye Blaster.*"
  File "..\IR Server Suite\IR Server Plugins\Serial IR Blaster\bin\${Build_Type}\Serial IR Blaster.*"
  File "..\IR Server Suite\IR Server Plugins\Speech Receiver\bin\${Build_Type}\Speech Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Technotrend Receiver\bin\${Build_Type}\Technotrend Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\Technotrend Receiver\bin\${Build_Type}\ttBdaDrvApi_Dll.dll"
  File "..\IR Server Suite\IR Server Plugins\USB-UIRT Transceiver\bin\${Build_Type}\USB-UIRT Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\Wii Remote Receiver\bin\${Build_Type}\Wii Remote Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\WiimoteLib\bin\${Build_Type}\WiimoteLib.*"
  File "..\IR Server Suite\IR Server Plugins\Windows Message Receiver\bin\${Build_Type}\Windows Message Receiver.*"
  File "..\IR Server Suite\IR Server Plugins\WinLirc Transceiver\bin\${Build_Type}\WinLirc Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\X10 Transceiver\bin\${Build_Type}\X10 Transceiver.*"
  File "..\IR Server Suite\IR Server Plugins\X10 Transceiver\bin\${Build_Type}\Interop.X10.dll"
  File "..\IR Server Suite\IR Server Plugins\XBCDRC Receiver\bin\${Build_Type}\XBCDRC Receiver.*"
  ; Ira project is not compiling currently
  ; File "..\IR Server Suite\IR Server Plugins\Ira Transceiver\bin\${Build_Type}\Ira Transceiver.*"
  ; File "..\IR Server Suite\IR Server Plugins\Tira Transceiver\bin\${Build_Type}\Tira Transceiver.*"
  
  ; Create App Data Folder for IR Server configuration files
  CreateDirectory "${COMMON_APPDATA}\IR Server"
  
  ; Copy Abstract Remote maps
  SetOutPath "${COMMON_APPDATA}\IR Server\Abstract Remote Maps"
  SetOverwrite ifnewer
  File /r /x .svn "..\IR Server Suite\IR Server\IR Server\Abstract Remote Maps\*.*"
  File "..\IR Server Suite\IR Server\IR Server\RemoteTable.xsd"
  SetOverwrite on

  ; Create start menu shortcut
  CreateShortCut "${STARTMENU_GROUP}\IR Server Configuration.lnk" "$DIR_INSTALL\IR Server Configuration.exe" "" "$DIR_INSTALL\IR Server Configuration.exe" 0
  CreateShortCut "${STARTMENU_GROUP}\IR Server Tray.lnk"          "$DIR_INSTALL\IR Server Tray.exe"          "" "$DIR_INSTALL\IR Server Tray.exe"          0

  ; install Server/Service
  ${If} $ServerServiceMode == "IRServerAsApplication"
    ${LOG_TEXT} "INFO" "Adding IR Server to Autostart..."
    !insertmacro SetAutoRun "IR Server" '"$DIR_INSTALL\IR Server.exe"'
  ${Else}
    ${LOG_TEXT} "INFO" "Installing IR Server as Service..."
    nsExec::ExecToLog '"$DIR_INSTALL\IR Server.exe" /install'
  ${EndIf}

  ${LOG_TEXT} "INFO" "Adding IR Server Tray to Autostart..."
  !insertmacro SetAutoRun "IR Server Tray" '"$DIR_INSTALL\IR Server Tray.exe"'

${MementoSectionEnd}
!macro Remove_${SectionIRServer}
  ${LOG_TEXT} "INFO" "Removing IR Server..."

  ${LOG_TEXT} "INFO" "Removing IR Server from Autostart..."
  !insertmacro RemoveAutoRun "IR Server"
  ${LOG_TEXT} "INFO" "Uninstalling IR Server as Service..."
  nsExec::ExecToLog '"$DIR_INSTALL\IR Server.exe" /uninstall'
  ${LOG_TEXT} "INFO" "Removing IR Server Tray from Autostart..."
  !insertmacro RemoveAutoRun "IR Server Tray"

  ; remove start menu shortcuts
  Delete "${STARTMENU_GROUP}\IR Server Configuration.lnk"
  Delete "${STARTMENU_GROUP}\IR Server Tray.lnk"

  ; remove files
  Delete "$DIR_INSTALL\IR Server.*"
  Delete "$DIR_INSTALL\Install.cmd"
  Delete "$DIR_INSTALL\Uninstall.cmd"

  Delete "$DIR_INSTALL\IR Server Configuration.*"
  Delete "$DIR_INSTALL\DevAge*"
  Delete "$DIR_INSTALL\SourceGrid*"

  Delete "$DIR_INSTALL\IR Server Tray.*"

  Delete "$DIR_INSTALL\IRServer.Shared.*"
  Delete "$DIR_INSTALL\IRServerPluginInterface.*"
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
  Delete "$MPdir.Plugins\Process\MPUtils.*"
  Delete "$MPdir.Plugins\Process\IrssComms.*"
  Delete "$MPdir.Plugins\Process\IrssUtils.*"
  Delete "$MPdir.Plugins\Windows\MPUtils.*"
  Delete "$MPdir.Plugins\Windows\IrssComms.*"
  Delete "$MPdir.Plugins\Windows\IrssUtils.*"
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
  CreateDirectory "${COMMON_APPDATA}\MP Control Plugin"
  SetOutPath "${COMMON_APPDATA}\MP Control Plugin"
  SetOverwrite ifnewer
  File /r /x .svn "..\MediaPortal Plugins\MediaPortal Plugins\MP Control Plugin\AppData\*.*"
  SetOverwrite on

  ; Create Macro folder
  CreateDirectory "${COMMON_APPDATA}\MP Control Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionMPControlPlugin}
  ${LOG_TEXT} "INFO" "MP Control Plugin..."

  Delete "$MPdir.Plugins\Process\MPControlPlugin.*"
!macroend

;======================================

${MementoUnselectedSection} "MP Blast Zone Plugin" SectionMPBlastZonePlugin
  ${LOG_TEXT} "INFO" "Installing MP Blast Zone Plugin..."

  ; use the all users context
  SetShellVarContext all

  ; Write plugin dll
  SetOutPath "$MPdir.Plugins\Windows"
  File "..\MediaPortal Plugins\MediaPortal Plugins\MP Blast Zone Plugin\bin\${Build_Type}\MPBlastZonePlugin.*"

  ; Write app data
  CreateDirectory "${COMMON_APPDATA}\MP Blast Zone Plugin"
  SetOutPath "${COMMON_APPDATA}\MP Blast Zone Plugin"
  SetOverwrite off
  File "..\MediaPortal Plugins\MediaPortal Plugins\MP Blast Zone Plugin\AppData\Menu.xml"
  SetOverwrite on

  ; Write skin files
  SetOutPath "$MPdir.Skin\Blue3"
  File /r /x .svn "..\MediaPortal Plugins\MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  SetOutPath "$MPdir.Skin\Blue3wide"
  File /r /x .svn "..\MediaPortal Plugins\MediaPortal Plugins\MP Blast Zone Plugin\Skin\*.*"

  ; Create Macro folder
  CreateDirectory "${COMMON_APPDATA}\MP Blast Zone Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionMPBlastZonePlugin}
  ${LOG_TEXT} "INFO" "Removing MP Blast Zone Plugin..."

  Delete "$MPdir.Plugins\Windows\MPBlastZonePlugin.*"
!macroend

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

;======================================

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
    Delete "$DIR_TVSERVER\Plugins\MPUtils.*"
    Delete "$DIR_TVSERVER\Plugins\IrssComms.*"
    Delete "$DIR_TVSERVER\Plugins\IrssUtils.*"

  ${EndIf}
!macroend

;======================================

${MementoUnselectedSection} "TV Server Blaster Plugin" SectionTV3BlasterPlugin
  ${LOG_TEXT} "INFO" "Installing TV Server Blaster Plugin..."

  ; Write plugin dll
  SetOutPath "$DIR_TVSERVER\Plugins"
  File "..\MediaPortal Plugins\TVServer plugins\TV3 Blaster Plugin\bin\${Build_Type}\TV3BlasterPlugin.*"

  ; create folders
  CreateDirectory "${COMMON_APPDATA}\TV3 Blaster Plugin"
  CreateDirectory "${COMMON_APPDATA}\TV3 Blaster Plugin\Macro"

${MementoSectionEnd}
!macro Remove_${SectionTV3BlasterPlugin}
  ${If} ${FileExists} "$DIR_TVSERVER\Plugins\TV3BlasterPlugin.*"

    ${LOG_TEXT} "INFO" "Removing TV Server Blaster Plugin..."
    !insertmacro StopTVService

    Delete "$DIR_TVSERVER\Plugins\TV3BlasterPlugin.*"

  ${EndIf}
!macroend

SectionGroupEnd

!endif

;======================================

SectionGroup "Tools" SectionGroupTools

${MementoSection} "Abstractor" SectionAbstractor
  ${LOG_TEXT} "INFO" "Installing Abstractor..."

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Abstractor\bin\${Build_Type}\Abstractor.*"

  ; create start menu shortcuts
  CreateShortCut "${STARTMENU_GROUP}\Abstractor.lnk" "$DIR_INSTALL\Abstractor.exe" "" "$DIR_INSTALL\Abstractor.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionAbstractor}
  ${LOG_TEXT} "INFO" "Removing Abstractor..."

  ; remove start menu shortcuts
  Delete "${STARTMENU_GROUP}\Abstractor.lnk"

  ; remove files
  Delete "$DIR_INSTALL\Abstractor.*"
!macroend

;======================================

${MementoSection} "Debug Client" SectionDebugClient
  ${LOG_TEXT} "INFO" "Installing Debug Client..."

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Debug Client\bin\${Build_Type}\DebugClient.*"

  ; create start menu shortcuts
  CreateShortCut "${STARTMENU_GROUP}\Debug Client.lnk" "$DIR_INSTALL\DebugClient.exe" "" "$DIR_INSTALL\DebugClient.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionDebugClient}
  ${LOG_TEXT} "INFO" "Removing Debug Client..."

  ; remove start menu shortcuts
  Delete "${STARTMENU_GROUP}\Debug Client.lnk"

  ; remove files
  Delete "$DIR_INSTALL\DebugClient.*"
!macroend

;======================================

${MementoUnselectedSection} "IR File Tool" SectionIRFileTool
  ${LOG_TEXT} "INFO" "Installing IR File Tool..."

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\IR File Tool\bin\${Build_Type}\IRFileTool.*"

  ; create start menu shortcuts
  CreateShortCut "${STARTMENU_GROUP}\IR File Tool.lnk" "$DIR_INSTALL\IRFileTool.exe" "" "$DIR_INSTALL\IRFileTool.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionIRFileTool}
  ${LOG_TEXT} "INFO" "Removing IR File Tool..."

  ; remove start menu shortcuts
  Delete "${STARTMENU_GROUP}\IR File Tool.lnk"

  ; remove files
  Delete "$DIR_INSTALL\IRFileTool.*"
!macroend

;======================================

${MementoUnselectedSection} "Keyboard Input Relay" SectionKeyboardInputRelay
  ${LOG_TEXT} "INFO" "Installing Keyboard Input Relay..."

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Keyboard Input Relay\bin\${Build_Type}\KeyboardInputRelay.*"

  ; create start menu shortcuts
  CreateShortCut "${STARTMENU_GROUP}\Keyboard Input Relay.lnk" "$DIR_INSTALL\KeyboardInputRelay.exe" "" "$DIR_INSTALL\KeyboardInputRelay.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionKeyboardInputRelay}
  ${LOG_TEXT} "INFO" "Removing Keyboard Input Relay..."

  ; remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Keyboard Input Relay"

  ; remove start menu shortcuts
  Delete "${STARTMENU_GROUP}\Keyboard Input Relay.lnk"

  ; remove files
  Delete "$DIR_INSTALL\KeyboardInputRelay.*"
!macroend

;======================================

${MementoSection} "Translator" SectionTranslator
  ${LOG_TEXT} "INFO" "Installing Translator..."

  ; install files
  SetOutPath "$DIR_INSTALL"
  File "..\IR Server Suite\Applications\Translator\bin\${Build_Type}\Translator.*"

  ; create folders
  CreateDirectory "${COMMON_APPDATA}\Translator"
  CreateDirectory "${COMMON_APPDATA}\Translator\Macro"
  CreateDirectory "${COMMON_APPDATA}\Translator\Default Settings"

  ; Copy in default settings files  
  SetOutPath "${COMMON_APPDATA}\Translator\Default Settings"
  File "..\IR Server Suite\Applications\Translator\Default Settings\*.xml"

  ; create start menu shortcuts
  CreateShortCut "${STARTMENU_GROUP}\Translator.lnk" "$DIR_INSTALL\Translator.exe" "" "$DIR_INSTALL\Translator.exe" 0

  ; check if Translator is an autorun app
  ${If} $AutoRunTranslator == 1
    !insertmacro SetAutoRun "Translator" "$DIR_INSTALL\Translator.exe"
  ${EndIf}
${MementoSectionEnd}
!macro Remove_${SectionTranslator}
  ${LOG_TEXT} "INFO" "Removing Translator..."

  ; remove auto-run
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Run" "Translator"

  ; remove start menu shortcuts
  Delete "${STARTMENU_GROUP}\Translator.lnk"

  ; remove files
  Delete "$DIR_INSTALL\Translator.*"
!macroend

;======================================

${MementoUnselectedSection} "Virtual Remote" SectionVirtualRemote
  ${LOG_TEXT} "INFO" "Installing Virtual Remote, Skin Editor, Smart Device versions, and Web Remote..."

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

  ; create start menu shortcuts
  CreateShortCut "${STARTMENU_GROUP}\Virtual Remote.lnk" "$DIR_INSTALL\VirtualRemote.exe" "" "$DIR_INSTALL\VirtualRemote.exe" 0
  CreateShortCut "${STARTMENU_GROUP}\Virtual Remote Skin Editor.lnk" "$DIR_INSTALL\VirtualRemoteSkinEditor.exe" "" "$DIR_INSTALL\VirtualRemoteSkinEditor.exe" 0
  CreateShortCut "${STARTMENU_GROUP}\Virtual Remote for Smart Devices.lnk" "$DIR_INSTALL\Virtual Remote\Smart Devices"
  CreateShortCut "${STARTMENU_GROUP}\Web Remote.lnk" "$DIR_INSTALL\WebRemote.exe" "" "$DIR_INSTALL\WebRemote.exe" 0

${MementoSectionEnd}
!macro Remove_${SectionVirtualRemote}
  ${LOG_TEXT} "INFO" "Removing Virtual Remote, Skin Editor, Smart Device versions, and Web Remote..."

  ; remove start menu shortcuts
  Delete "${STARTMENU_GROUP}\Virtual Remote.lnk"
  Delete "${STARTMENU_GROUP}\Virtual Remote Skin Editor.lnk"
  Delete "${STARTMENU_GROUP}\Virtual Remote for Smart Devices.lnk"
  Delete "${STARTMENU_GROUP}\Web Remote.lnk"

  ; remove files
  Delete "$DIR_INSTALL\VirtualRemote.*"
  Delete "$DIR_INSTALL\VirtualRemoteSkinEditor.*"
  Delete "$DIR_INSTALL\WebRemote.*"
  RMDir /R "$DIR_INSTALL\Virtual Remote"
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
  CreateDirectory "${COMMON_APPDATA}\Dbox Tuner"

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
  ${LOG_TEXT} "INFO" "Completing installation..."

  ; removes unselected components
  !insertmacro SectionList "FinishSection"
  ; writes component status to registry
  ${MementoSectionSave}

!ifdef MPplugins
  ; start tvservice, if it was closed before
  !insertmacro StartTVService

  ; removing tve2 blaster
  Delete "$MPdir.Plugins\Process\TV2BlasterPlugin.dll"
!endif

  ; use the all users context
  SetShellVarContext all

  ; Create start menu shortcuts
  WriteINIStr "${STARTMENU_GROUP}\Documentation.url"  "InternetShortcut" "URL" "http://www.team-mediaportal.com/manual/IRServerSuite"
  WriteINIStr "${STARTMENU_GROUP}\Website.url"  "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "${STARTMENU_GROUP}\Log Files.lnk" "${COMMON_APPDATA}\Logs"
  CreateShortCut "${STARTMENU_GROUP}\Uninstall.lnk" "$DIR_INSTALL\uninstall-irss.exe" "" "$DIR_INSTALL\uninstall-irss.exe"

  
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
  WriteRegStr HKLM "${REG_UNINSTALL}" "DisplayIcon"     "$DIR_INSTALL\IR Server.exe"
  WriteRegStr HKLM "${REG_UNINSTALL}" "UninstallString" "$DIR_INSTALL\uninstall-irss.exe"
  WriteRegDWORD HKLM "${REG_UNINSTALL}" "NoModify" 1
  WriteRegDWORD HKLM "${REG_UNINSTALL}" "NoRepair" 1

  ; Write the uninstaller
  WriteUninstaller "$DIR_INSTALL\uninstall-irss.exe"

  ; set rights to programmdata directory and reg keys
  Call SetRightsIRSS

  ; Store the install log
  StrCpy $0 "${COMMON_APPDATA}\Logs\Install.log"
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

  ; use the all users context
  SetShellVarContext all

  ; if uninstaller is launched from installer, closing the apps is already done
  ${IfNot} $frominstall == 1
    ; close all IRSS related apps
    !insertmacro ShutdownRunningMediaPortalApplications
  ${EndIf}

  ; First removes all optional components
  !insertmacro SectionList "RemoveSection"

!ifdef MPplugins
  ; start tvservice, if it was closed before
  !insertmacro StartTVService
!endif

  ; Remove files and uninstaller
  DetailPrint "Removing program files ..."
  RMDir /R "$DIR_INSTALL"

  DetailPrint "Removing start menu shortcuts ..."
  RMDir /R "${STARTMENU_GROUP}"
  
  ; Remove registry keys
  DetailPrint "Removing registry keys ..."
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
  DeleteRegKey HKLM "Software\${PRODUCT_NAME}"

  ${If} $UnInstallMode == 1

    ${LOG_TEXT} "INFO" "Removing User Settings"
    RMDir /r "${COMMON_APPDATA}"
    RMDir /r "$LOCALAPPDATA\VirtualStore\Program Files\${PRODUCT_NAME}"
    RMDir /r "$LOCALAPPDATA\VirtualStore\ProgramData\${PRODUCT_NAME}"

  ${EndIf}

  ; close uninstaller, if it is called from installer
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
!macro MediaPortalVersionCheck
  ${If} ${SectionIsSelected} ${SectionMPControlPlugin}
  ${OrIf} ${SectionIsSelected} ${SectionMPBlastZonePlugin}

    !insertmacro MP_GET_VERSION $0
    ${VersionCompare} $0 ${MIN_VERSION_MP} $R0
    ${If} $R0 == 2 ;MIN_VERSION_MP is higher than current installed
      MessageBox MB_OK|MB_ICONEXCLAMATION "$(ERROR_MIN_VERSION_MP)"
      Abort
    ${EndIf}

  ${EndIf}


  ${If} ${SectionIsSelected} ${SectionTV3BlasterPlugin}

    !insertmacro TVSERVER_GET_VERSION $0
    ${VersionCompare} $0 ${MIN_VERSION_TVSERVER} $R0
    ${If} $R0 == 2 ;MIN_VERSION_TVSERVER is higher than current installed
      MessageBox MB_OK|MB_ICONEXCLAMATION "$(ERROR_MIN_VERSION_TVSERVER)"
      Abort
    ${EndIf}

  ${EndIf}
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
    !insertmacro EnableSection "${SectionGroupMP}" "MediaPortal plugins"
  ${else}
    !insertmacro DisableSection "${SectionMPControlPlugin}" "MP Control Plugin" " "
    !insertmacro DisableSection "${SectionMPBlastZonePlugin}" "MP Blast Zone Plugin" " "
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
    Exec '"$DIR_INSTALL\IR Server.exe"'
  ${Else}
    ${LOG_TEXT} "INFO" "Starting IR Server service..."
    Exec '"$DIR_INSTALL\IR Server.exe" /start'
  ${EndIf}

  ; start tray tool
  ${LOG_TEXT} "INFO" "Starting IR Server Tray..."
  Exec '"$DIR_INSTALL\IR Server Tray.exe"'

${EndIf}

${If} ${SectionIsSelected} ${SectionTranslator}

  ; automatically start translator only if it was already and autostart app
  ${If} $AutoRunTranslator == 1
    ; if translator is an autorun app, start is not so user doesn't need to do it manually
    Exec '"$DIR_INSTALL\Translator.exe"'
  ${EndIf}

${EndIf}

${LOG_CLOSE}
FunctionEnd

Function .onSelChange

!ifdef MPplugins
  ; disable/remove common files for MediaPortal plugins if all MediaPortal plugins are unselected
  ${IfNot} ${SectionIsSelected} ${SectionMPControlPlugin}
  ${AndIfNot} ${SectionIsSelected} ${SectionMPBlastZonePlugin}
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

Function PageReinstallLeave
  !ifdef MPplugins
    ; ExpressUpdate is selected
    ${If} $PREVIOUS_VERSION_STATE != "same"
    ${AndIf} $ReinstallMode == 1

      ; check if mp components are selected and if correct versions are installed
      !insertmacro MediaPortalVersionCheck

    ${EndIf}
  !endif
FunctionEnd

Function PageComponentsPre
  ; skip page if previous settings are used for update
  ${If} $EXPRESS_UPDATE == 1
    Abort
  ${EndIf}
FunctionEnd

Function PageComponentsLeave
  !ifdef MPplugins
    ; check if mp components are selected and if correct versions are installed
    !insertmacro MediaPortalVersionCheck
  !endif
FunctionEnd

Function PageDirectoryPre
  ; skip page if previous settings are used for update
  ${If} $EXPRESS_UPDATE == 1
    Abort
  ${EndIf}
FunctionEnd


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
  ; skip page if uninstaller is called from installer
  ${If} $frominstall == 1
    Abort
  ${EndIf}
FunctionEnd

;Function un.ConfirmPagePre
;  ; skip page if uninstaller is called from installer
;  ${If} $frominstall == 1
;    Abort
;  ${EndIf}
;FunctionEnd

Function un.FinishPagePre
  ; skip page if uninstaller is called from installer
  ${If} $frominstall == 1
    SetRebootFlag false
    Abort
  ${EndIf}
FunctionEnd


Function SetRightsIRSS
  ${LOG_TEXT} "INFO" "Setting AccessRights to ProgramData dir and reg keys"

  SetOverwrite on
  SetOutPath "$PLUGINSDIR"
  File "Resources\SetRights.exe"

  ; use the all users context
  SetShellVarContext all
  nsExec::ExecToLog '"$PLUGINSDIR\SetRights.exe" FOLDER "${COMMON_APPDATA}"'
  nsExec::ExecToLog '"$PLUGINSDIR\SetRights.exe" HKLM "Software\${PRODUCT_NAME}"'
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
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionGroupTV3}            "$(DESC_SectionGroupTV3)"
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionTV3BlasterPlugin}    "$(DESC_SectionTV3BlasterPlugin)"
!endif

  !insertmacro MUI_DESCRIPTION_TEXT ${SectionTranslator}          "$(DESC_SectionTranslator)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionVirtualRemote}       "$(DESC_SectionVirtualRemote)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionIRBlast}             "$(DESC_SectionIRBlast)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionIRFileTool}          "$(DESC_SectionIRFileTool)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionKeyboardInputRelay}  "$(DESC_SectionKeyboardInputRelay)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionDboxTuner}           "$(DESC_SectionDboxTuner)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionHcwPvrTuner}         "$(DESC_SectionHcwPvrTuner)"
  !insertmacro MUI_DESCRIPTION_TEXT ${SectionDebugClient}         "$(DESC_SectionDebugClient)"
!insertmacro MUI_FUNCTION_DESCRIPTION_END
