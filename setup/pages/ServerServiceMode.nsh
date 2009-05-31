#region Copyright (C) 2005-2008 Team MediaPortal

/* 
 *	Copyright (C) 2005-2008 Team MediaPortal
 *	http://www.team-mediaportal.com
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

!ifndef ___SERVER_SERVICE_MODE_PAGE__NSH___
!define ___SERVER_SERVICE_MODE_PAGE__NSH___

!include WordFunc.nsh
!include FileFunc.nsh

!insertmacro VersionCompare
!insertmacro GetParent

#####    Server/Service Mode page
; $ServerServiceMode "InputService" = InputService
; $ServerServiceMode "IRServer" = IRServer
Var PREVIOUS_ServerServiceMode
Var ServerServiceMode
Var ServerServiceModePage.optBtnInputService
Var ServerServiceModePage.optBtnInputService.state
Var ServerServiceModePage.optBtnIRServer
Var ServerServiceModePage.optBtnIRServer.state


Function PageServerServiceMode
  Push $R0

  ; skip page if InputService/IRServer is unselected
  ${IfNot} ${SectionIsSelected} SectionInputService
    Abort
  ${EndIf}

  ; skip page if previous settings are used for update
  ${If} $EXPRESS_UPDATE == 1
    Abort
  ${EndIf}

  !insertmacro MUI_HEADER_TEXT "$(ServerServiceModePage_HEADER)" "$(ServerServiceModePage_HEADER2)"

  nsDialogs::Create /NOUNLOAD 1018
  Pop $R0

  ${NSD_CreateLabel} 0 0 300u 24u "$(ServerServiceModePage_INFO)"
  Pop $R0


  ${NSD_CreateRadioButton} 10u 30u -10u 8u "$(ServerServiceModePage_OPT0)"
  Pop $ServerServiceModePage.optBtnInputService
  ${NSD_OnClick} $ServerServiceModePage.optBtnInputService PageServerServiceModeUpdateSelection

  ${NSD_CreateLabel} 20u 45u -20u 24u "$(ServerServiceModePage_OPT0_DESC)"
  Pop $R0


  ${NSD_CreateRadioButton} 10u 70u -10u 8u "$(ServerServiceModePage_OPT1)"
  Pop $ServerServiceModePage.optBtnIRServer
  ${NSD_OnClick} $ServerServiceModePage.optBtnIRServer PageServerServiceModeUpdateSelection

  ${NSD_CreateLabel} 20u 85u -20u 24u "$(ServerServiceModePage_OPT1_DESC)"
  Pop $R0


  ; set current ServerServiceMode to option buttons
  ${If} $ServerServiceMode == "IRServer"
    ${NSD_Check} $ServerServiceModePage.optBtnIRServer
  ${Else}
    ${NSD_Check} $ServerServiceModePage.optBtnInputService
  ${EndIf}

  nsDialogs::Show

  Pop $R0
FunctionEnd

Function PageServerServiceModeUpdateSelection

  ${NSD_GetState} $ServerServiceModePage.optBtnInputService $ServerServiceModePage.optBtnInputService.state
  ${NSD_GetState} $ServerServiceModePage.optBtnIRServer     $ServerServiceModePage.optBtnIRServer.state

  ${If} $ServerServiceModePage.optBtnIRServer.state == ${BST_CHECKED}
    StrCpy $ServerServiceMode "IRServer"
  ${Else}
    StrCpy $ServerServiceMode "InputService"
  ${EndIf}

FunctionEnd

Function PageLeaveServerServiceMode

FunctionEnd

!endif # !___SERVER_SERVICE_MODE_PAGE__NSH___
