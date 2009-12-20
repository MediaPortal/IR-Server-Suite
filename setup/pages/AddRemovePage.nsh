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

/*
_____________________________________________________________________________

                       ReinstallPage

    If same version is already installed, this page offers to uninstall or reinstall.
    If different version, this page offers simple and advanced mode for installation.
    
    Needs:     $PREVIOUS_VERSION_STATE { "newer" , "older", "same" }
               $EXPRESS_UPDATE { 0 , 1 }
               Function: RunUninstaller            to start uninstaller of previous version
               Function: LoadPreviousSettings      to re-load settings of previous installation
_____________________________________________________________________________
*/

!ifndef ___REINSTALLPAGE__NSH___
!define ___REINSTALLPAGE__NSH___

;--------------------------------
;Page interface settings and variables

!macro MUI_REINSTALLPAGE_INTERFACE

  !ifndef MUI_REINSTALLPAGE_INTERFACE
    !define MUI_REINSTALLPAGE_INTERFACE
    Var mui.ReinstallPage
    Var ReinstallMode

    Var mui.ReinstallPage.Text

    Var mui.ReinstallPage.Option1
    Var mui.ReinstallPage.Option1.State
    Var mui.ReinstallPage.Option2
    Var mui.ReinstallPage.Option2.State


    Function ReinstallPage.UpdateSelection

      ${NSD_GetState} $mui.ReinstallPage.Option1 $mui.ReinstallPage.Option1.State
      ${NSD_GetState} $mui.ReinstallPage.Option2 $mui.ReinstallPage.Option2.State

      ${If} $mui.ReinstallPage.Option2.State == ${BST_CHECKED}
        StrCpy $ReinstallMode 2
      ${Else}
        StrCpy $ReinstallMode 1
      ${EndIf}

    FunctionEnd

  !endif
  
;  !insertmacro MUI_DEFAULT MUI_${MUI_PAGE_UNINSTALLER_PREFIX}WELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Wizard\win.bmp"  

!macroend


;--------------------------------
;Interface initialization

!macro MUI_REINSTALLPAGE_GUIINIT

  !ifndef MUI_${MUI_PAGE_UNINSTALLER_PREFIX}REINSTALLPAGE_GUINIT
    !define MUI_${MUI_PAGE_UNINSTALLER_PREFIX}REINSTALLPAGE_GUINIT

    Function ${MUI_PAGE_UNINSTALLER_FUNCPREFIX}mui.ReinstallPage.GUIInit

;      InitPluginsDir
;      File "/oname=$PLUGINSDIR\modern-wizard.bmp" "${MUI_${MUI_PAGE_UNINSTALLER_PREFIX}WELCOMEFINISHPAGE_BITMAP}"

      !ifdef MUI_${MUI_PAGE_UNINSTALLER_PREFIX}PAGE_FUNCTION_GUIINIT
        Call "${MUI_${MUI_PAGE_UNINSTALLER_PREFIX}PAGE_FUNCTION_GUIINIT}"
      !endif

    FunctionEnd
  
    !insertmacro MUI_SET MUI_${MUI_PAGE_UNINSTALLER_PREFIX}PAGE_FUNCTION_GUIINIT ${MUI_PAGE_UNINSTALLER_FUNCPREFIX}mui.ReinstallPage.GUIInit

  !endif

!macroend


;--------------------------------
;Page declaration

!macro MUI_PAGEDECLARATION_REINSTALL

  !insertmacro MUI_SET MUI_${MUI_PAGE_UNINSTALLER_PREFIX}REINSTALLPAGE ""
  !insertmacro MUI_REINSTALLPAGE_INTERFACE
  
  !insertmacro MUI_REINSTALLPAGE_GUIINIT

;  !insertmacro MUI_DEFAULT MUI_WELCOMEPAGE_TITLE "$(MUI_${MUI_PAGE_UNINSTALLER_PREFIX}TEXT_WELCOME_INFO_TITLE)"
;  !insertmacro MUI_DEFAULT MUI_WELCOMEPAGE_TEXT "$(MUI_${MUI_PAGE_UNINSTALLER_PREFIX}TEXT_WELCOME_INFO_TEXT)"
  
;  !insertmacro MUI_PAGE_FUNCTION_FULLWINDOW

  PageEx ${MUI_PAGE_UNINSTALLER_FUNCPREFIX}custom

    PageCallbacks ${MUI_PAGE_UNINSTALLER_FUNCPREFIX}mui.ReinstallPre_${MUI_UNIQUEID} ${MUI_PAGE_UNINSTALLER_FUNCPREFIX}mui.ReinstallLeave_${MUI_UNIQUEID}

  PageExEnd

  !insertmacro MUI_FUNCTION_REINSTALLPAGE ${MUI_PAGE_UNINSTALLER_FUNCPREFIX}mui.ReinstallPre_${MUI_UNIQUEID} ${MUI_PAGE_UNINSTALLER_FUNCPREFIX}mui.ReinstallLeave_${MUI_UNIQUEID}

;  !insertmacro MUI_UNSET MUI_WELCOMEPAGE_TITLE
;  !insertmacro MUI_UNSET MUI_WELCOMEPAGE_TITLE_3LINES
;  !insertmacro MUI_UNSET MUI_WELCOMEPAGE_TEXT

!macroend

!macro MUI_PAGE_REINSTALL

  !verbose push
;  !verbose ${MUI_VERBOSE}
  !verbose 4

  !insertmacro MUI_PAGE_INIT
  !insertmacro MUI_PAGEDECLARATION_REINSTALL

  !verbose pop

!macroend


;--------------------------------
;Page functions

!macro MUI_FUNCTION_REINSTALLPAGE PRE LEAVE

  Function "${PRE}"

    !insertmacro MUI_PAGE_FUNCTION_CUSTOM PRE

    ; check if software is already installed
    ${If} $PREVIOUS_VERSION == ""
      Abort
    ${EndIf}


    ; set string for control texts
    ${If} $PREVIOUS_VERSION_STATE == "newer"
      StrCpy $R1 "$(TEXT_ADDREMOVE_INFO_UPGRADE)"
      StrCpy $R2 "$(TEXT_ADDREMOVE_UPDOWN_OPT1)"
      StrCpy $R3 "$(TEXT_ADDREMOVE_UPDOWN_OPT2)"
      !insertmacro MUI_HEADER_TEXT "$(TEXT_ADDREMOVE_HEADER)" "$(TEXT_ADDREMOVE_HEADER2_UPDOWN)"

    ${ElseIf} $PREVIOUS_VERSION_STATE == "older"
      StrCpy $R1 "$(TEXT_ADDREMOVE_INFO_DOWNGRADE)"
      StrCpy $R2 "$(TEXT_ADDREMOVE_UPDOWN_OPT1)"
      StrCpy $R3 "$(TEXT_ADDREMOVE_UPDOWN_OPT2)"
      !insertmacro MUI_HEADER_TEXT "$(TEXT_ADDREMOVE_HEADER)" "$(TEXT_ADDREMOVE_HEADER2_UPDOWN)"

    ${ElseIf} $PREVIOUS_VERSION_STATE == "same"
      StrCpy $R1 "$(TEXT_ADDREMOVE_INFO_REPAIR)"
      StrCpy $R2 "$(TEXT_ADDREMOVE_REPAIR_OPT1)"
      StrCpy $R3 "$(TEXT_ADDREMOVE_REPAIR_OPT2)"
      !insertmacro MUI_HEADER_TEXT "$(TEXT_ADDREMOVE_HEADER)" "$(TEXT_ADDREMOVE_HEADER2_REPAIR)"

    ${Else}
      MessageBox MB_ICONSTOP "Unknown value of PREVIOUS_VERSION_STATE, aborting" /SD IDOK
      Abort
    ${EndIf}


    ;create dialog
    nsDialogs::Create 1018
    Pop $mui.ReinstallPage
;    nsDialogs::SetRTL $(^RTL)
;    SetCtlColors $mui.ReinstallPage "" "${MUI_BGCOLOR}"

    ;create text
    ${NSD_CreateLabel} 0 0 300u 24u "$R1"
    Pop $mui.ReinstallPage.Text

    ;create options
    ${NSD_CreateRadioButton} 30u 50u -30u 8u "$R2"
    Pop $mui.ReinstallPage.Option1
    ${NSD_OnClick} $mui.ReinstallPage.Option1 ReinstallPage.UpdateSelection
    ${NSD_CreateRadioButton} 30u 70u -30u 8u "$R3"
    Pop $mui.ReinstallPage.Option2
    ${NSD_OnClick} $mui.ReinstallPage.Option2 ReinstallPage.UpdateSelection


    ; set current ReinstallMode to option buttons
    ${If} $ReinstallMode == 2
      ${NSD_Check} $mui.ReinstallPage.Option2
    ${Else}
      ; if not 2, set to 1
      ${NSD_Check} $mui.ReinstallPage.Option1
      ; set reinstallmode to 1, if reinstallmode = ""
      StrCpy $ReinstallMode 1
    ${EndIf}


    ;Show page
;    Call ${MUI_PAGE_UNINSTALLER_FUNCPREFIX}muiPageLoadFullWindow
    !insertmacro MUI_PAGE_FUNCTION_CUSTOM SHOW
    nsDialogs::Show
;    Call ${MUI_PAGE_UNINSTALLER_FUNCPREFIX}muiPageUnloadFullWindow

;    !insertmacro MUI_UNSET MUI_WELCOMEPAGE_TITLE_HEIGHT
;    !insertmacro MUI_UNSET MUI_WELCOMEPAGE_TEXT_TOP

  FunctionEnd

  Function "${LEAVE}"

    !insertmacro MUI_PAGE_FUNCTION_CUSTOM LEAVE


    StrCpy $EXPRESS_UPDATE 0

    ; Uninstall is selected
    ${If} $PREVIOUS_VERSION_STATE == "same"
    ${AndIf} $ReinstallMode == 2

      StrCpy $EXPRESS_UPDATE 1
      Call RunUninstaller
      Quit

    ${EndIf}

    ; ExpressUpdate is selected
    ${If} $PREVIOUS_VERSION_STATE != "same"
    ${AndIf} $ReinstallMode == 1

      StrCpy $EXPRESS_UPDATE 1
      Call LoadPreviousSettings

    ${EndIf}

  FunctionEnd

!macroend

!endif # !___REINSTALL_PAGE__NSH___
