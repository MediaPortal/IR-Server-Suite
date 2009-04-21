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
; $ServerServiceMode 0 = InputService
; $ServerServiceMode 1 = IRServer
Var ServerServiceMode
Var ServerServiceModePage.optBtn0
Var ServerServiceModePage.optBtn0.state
Var ServerServiceModePage.optBtn1
Var ServerServiceModePage.optBtn1.state


Function PageServerServiceMode

  ; if input service is unselected, skip page
  ${IfNot} ${SectionIsSelected} SectionInputService
    Abort
  ${EndIf}

  !insertmacro MUI_HEADER_TEXT "$(ServerServiceModePage_HEADER)" "$(ServerServiceModePage_HEADER2)"

  nsDialogs::Create /NOUNLOAD 1018

  ${NSD_CreateLabel} 0 0 300u 24u "$(ServerServiceModePage_INFO)"
  Pop $R1


  ${NSD_CreateRadioButton} 10u 30u -10u 8u "$(ServerServiceModePage_OPT0)"
  Pop $ServerServiceModePage.optBtn0
  ${NSD_OnClick} $ServerServiceModePage.optBtn0 PageServerServiceModeUpdateSelection

  ${NSD_CreateLabel} 20u 45u -20u 24u "$(ServerServiceModePage_OPT0_DESC)"


  ${NSD_CreateRadioButton} 10u 70u -10u 8u "$(ServerServiceModePage_OPT1)"
  Pop $ServerServiceModePage.optBtn1
  ${NSD_OnClick} $ServerServiceModePage.optBtn1 PageServerServiceModeUpdateSelection

  ${NSD_CreateLabel} 20u 85u -20u 24u "$(ServerServiceModePage_OPT1_DESC)"

  ; set current ServerServiceMode to option buttons
  ${If} $ServerServiceMode == 1
    ${NSD_Check} $ServerServiceModePage.optBtn1
  ${Else}
    ${NSD_Check} $ServerServiceModePage.optBtn0
  ${EndIf}

  nsDialogs::Show
FunctionEnd

Function PageServerServiceModeUpdateSelection

  ${NSD_GetState} $ServerServiceModePage.optBtn0 $ServerServiceModePage.optBtn0.state
  ${NSD_GetState} $ServerServiceModePage.optBtn1 $ServerServiceModePage.optBtn1.state

  ${If} $ServerServiceModePage.optBtn1.state == ${BST_CHECKED}
    StrCpy $ServerServiceMode 1
  ${Else}
    StrCpy $ServerServiceMode 0
  ${EndIf}

FunctionEnd

!endif # !___SERVER_SERVICE_MODE_PAGE__NSH___
