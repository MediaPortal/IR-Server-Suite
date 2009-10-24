#region Copyright (C) 2005-2009 Team MediaPortal

/* 
 *	Copyright (C) 2005-2009 Team MediaPortal
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


!ifndef ___MediaPortalDirectories__NSH___
!define ___MediaPortalDirectories__NSH___

!include Util.nsh

!verbose push
!verbose 3
!ifndef _MediaPortalDirectories_VERBOSE
  !define _MediaPortalDirectories_VERBOSE 3
!endif
!verbose ${_MediaPortalDirectories_VERBOSE}
!define MediaPortalDirectories_VERBOSE `!insertmacro MediaPortalDirectories_VERBOSE`
!verbose pop

!macro MediaPortalDirectories_VERBOSE _VERBOSE
  !verbose push
  !verbose 3
  !undef _MediaPortalDirectories_VERBOSE
  !define _MediaPortalDirectories_VERBOSE ${_VERBOSE}
  !verbose pop
!macroend




!include LogicLib.nsh
!include FileFunc.nsh
!include WordFunc.nsh

!ifndef NO_INSTALL_LOG
  !include "${svn_InstallScripts}\include\LoggingMacros.nsh"
!else

  !ifndef LOG_TEXT
    !define prefixERROR "[ERROR     !!!]   "
    !define prefixDEBUG "[    DEBUG    ]   "
    !define prefixINFO  "[         INFO]   "

    !define LOG_TEXT `!insertmacro LOG_TEXT`
    !macro LOG_TEXT LEVEL TEXT
        DetailPrint "${prefix${LEVEL}}${TEXT}"
    !macroend
  !endif

!endif


!AddPluginDir "${svn_InstallScripts}\XML-plugin\Plugin"
!include "${svn_InstallScripts}\XML-plugin\Include\XML.nsh"

#---------------------------------------------------------------------------
#   Read      Special MediaPortal directories from  xml
#
#           enable it by defining         USE_READ_MP_DIRS      in parent script
#---------------------------------------------------------------------------

Var MyDocs
Var UserAppData
Var CommonAppData

Var MPdir.Base

Var MPdir.Config
Var MPdir.Plugins
Var MPdir.Log
Var MPdir.CustomInputDevice
Var MPdir.CustomInputDefault
Var MPdir.Skin
Var MPdir.Language
Var MPdir.Database
Var MPdir.Thumbs
Var MPdir.Weather
Var MPdir.Cache
Var MPdir.BurnerSupport

#***************************
#***************************

!macro GetPathTextCall _DIR _RESULT
  !verbose push
  !verbose ${_MediaPortalDirectories_VERBOSE}
  Push `${_DIR}`
  ${CallArtificialFunction} GetPathText_
  Pop ${_RESULT}
  !verbose pop
!macroend

!define GetPathText `!insertmacro GetPathTextCall`

!macro GetPathText_
  !verbose push
  !verbose ${_MediaPortalDirectories_VERBOSE}

  Exch $0 ;_DIR
  Push $1
  Push $2

  ${xml::GotoPath} "/Config" $1
  ${If} $1 != 0
    ${LOG_TEXT} "ERROR" "xml::GotoPath /Config"
    Goto error
  ${EndIf}

  loop:

  ${xml::FindNextElement} "Dir" $2 $1
  ${If} $1 != 0
    ${LOG_TEXT} "ERROR" "xml::FindNextElement >/Dir< >$2<"
    Goto error
  ${EndIf}

  ${xml::ElementPath} $2
  ${xml::GetAttribute} "id" $2 $1
  ${If} $1 != 0
    ${LOG_TEXT} "ERROR" "xml::GetAttribute >id< >$2<"
    Goto error
  ${EndIf}
  ${IfThen} $2 == $0  ${|} Goto foundDir ${|}

  Goto loop


  foundDir:
  ${xml::ElementPath} $2
  ${xml::GotoPath} "$2/Path" $1
  ${If} $1 != 0
    ${LOG_TEXT} "ERROR" "xml::GotoPath >$2/Path<"
    Goto error
  ${EndIf}

  ${xml::GetText} $0 $1
  ${If} $1 != 0
    ; maybe the path is only empty, which means MPdir.Base
    #MessageBox MB_OK "error: xml::GetText"
    #Goto error
    StrCpy $0 ""
  ${EndIf}


  Goto end

  error:
  StrCpy $0 "-1"

  end:
  Pop $2
  Pop $1
  Exch $0

  !verbose pop
!macroend

#***************************
#***************************

!define ReadMPdir `!insertmacro ReadMPdirCall`

!macro ReadMPdirCall DIR
  !verbose push
  !verbose ${_MediaPortalDirectories_VERBOSE}

  Push $0
  Push $1
  Push $2
  Push $3



  ${GetPathText} "${DIR}" $0
  ${IfThen} $0 == -1 ${|} Goto error1 ${|}

  ;${LOG_TEXT} "DEBUG" "macro: ReadMPdir | text found in xml: '$0'"
  ${WordReplace} "$0" "%APPDATA%" "$UserAppData" "+" $0
  ${WordReplace} "$0" "%PROGRAMDATA%" "$CommonAppData" "+" $0

  ${GetRoot} "$0" $1
  ${IfThen} $1 == "" ${|} StrCpy $0 "$MPdir.Base\$0" ${|}

  ; TRIM    \    AT THE END
  StrLen $1 "$0"
    #${DEBUG_MSG} "1 $1$\r$\n2 $2$\r$\n3 $3"
  IntOp $2 $1 - 1
    #${DEBUG_MSG} "1 $1$\r$\n2 $2$\r$\n3 $3"
  StrCpy $3 $0 1 $2
    #${DEBUG_MSG} "1 $1$\r$\n2 $2$\r$\n3 $3"

  ${If} $3 == "\"
    StrCpy $MPdir.${DIR} $0 $2
  ${Else}
    StrCpy $MPdir.${DIR} $0
  ${EndIf}


  Pop $3
  Pop $2
  Pop $1
  Pop $0

  !verbose pop
!macroend

#***************************
#***************************

!macro ReadConfigCall _PATH_TO_XML _RESULT
  !verbose push
  !verbose ${_MediaPortalDirectories_VERBOSE}
  Push `${_PATH_TO_XML}`
  ${CallArtificialFunction2} ReadConfig_
  Pop ${_RESULT}
  !verbose pop
!macroend

!define ReadConfig `!insertmacro ReadConfigCall`

!macro ReadConfig_
  !verbose push
  !verbose ${_MediaPortalDirectories_VERBOSE}

  Exch $0 ;_PATH_TO_XML
  Push $1


  IfFileExists "$0\MediaPortalDirs.xml" 0 error1

  ${xml::LoadFile} "$0\MediaPortalDirs.xml" $1
  ${IfThen} $1 != 0 ${|} Goto error1 ${|}

  #</Dir>  Log CustomInputDevice CustomInputDefault Skin Language Database Thumbs Weather Cache BurnerSupport

  ${ReadMPdir} Config
  ${ReadMPdir} Plugins
  ${ReadMPdir} Log
  ${ReadMPdir} CustomInputDevice
  ${ReadMPdir} CustomInputDefault
  ${ReadMPdir} Skin
  ${ReadMPdir} Language
  ${ReadMPdir} Database
  ${ReadMPdir} Thumbs
  ${ReadMPdir} Weather
  ${ReadMPdir} Cache
  ${ReadMPdir} BurnerSupport


  StrCpy $0 "0"
  Goto end1

  error1:
  StrCpy $0 "-1"

  end1:
  Pop $1
  Exch $0

  !verbose pop
!macroend

#***************************
#***************************

!macro LoadDefaultDirs

  StrCpy $MPdir.Config              "$CommonAppData\Team MediaPortal\MediaPortal"

  StrCpy $MPdir.Plugins             "$MPdir.Base\plugins"
  StrCpy $MPdir.Log                 "$MPdir.Config\log"
  StrCpy $MPdir.CustomInputDevice   "$MPdir.Config\InputDeviceMappings"
  StrCpy $MPdir.CustomInputDefault  "$MPdir.Base\InputDeviceMappings\defaults"
  StrCpy $MPdir.Skin                "$MPdir.Config\skin"
  StrCpy $MPdir.Language            "$MPdir.Config\language"
  StrCpy $MPdir.Database            "$MPdir.Config\database"
  StrCpy $MPdir.Thumbs              "$MPdir.Config\thumbs"
  StrCpy $MPdir.Weather             "$MPdir.Base\weather"
  StrCpy $MPdir.Cache               "$MPdir.Config\cache"
  StrCpy $MPdir.BurnerSupport       "$MPdir.Base\Burner"

!macroend

#***************************
#***************************

!define ReadMediaPortalDirs `!insertmacro ReadMediaPortalDirsCall`
!define un.ReadMediaPortalDirs `!insertmacro ReadMediaPortalDirsCall`
!macro ReadMediaPortalDirsCall INSTDIR
  !verbose push
  !verbose ${_MediaPortalDirectories_VERBOSE}
  Push $0


  StrCpy $MPdir.Base "${INSTDIR}"
  SetShellVarContext current
  StrCpy $MyDocs "$DOCUMENTS"
  StrCpy $UserAppData "$APPDATA"
  SetShellVarContext all
  StrCpy $CommonAppData "$APPDATA"

  !insertmacro LoadDefaultDirs

  ${ReadConfig} "$MyDocs\Team MediaPortal" $0
  ${If} $0 != 0   ; an error occured
    ${LOG_TEXT} "ERROR" "Loading MediaPortalDirectories from MyDocs failed. ('$MyDocs\Team MediaPortal\MediaPortalDirs.xml')"
    ${LOG_TEXT} "INFO"  "Trying to load from installation directory now."

    ${ReadConfig} "$MPdir.Base" $0
    ${If} $0 != 0   ; an error occured
      ${LOG_TEXT} "ERROR" "Loading MediaPortalDirectories from InstallDir failed. ('$MPdir.Base\MediaPortalDirs.xml')"
      ${LOG_TEXT} "INFO"  "Using default paths for MediaPortalDirectories now."
      !insertmacro LoadDefaultDirs

    ${Else}
      ${LOG_TEXT} "INFO" "Loaded MediaPortalDirectories from InstallDir successfully. ('$MPdir.Base\MediaPortalDirs.xml')"
    ${EndIf}

  ${Else}
    ${LOG_TEXT} "INFO" "Loaded MediaPortalDirectories from MyDocs successfully. ('$MyDocs\Team MediaPortal\MediaPortalDirs.xml')"
  ${EndIf}

  ${LOG_TEXT} "INFO" "Installer will use the following directories:"
  ${LOG_TEXT} "INFO" "          Base:  $MPdir.Base"
  ${LOG_TEXT} "INFO" "          Config:  $MPdir.Config"
  ${LOG_TEXT} "INFO" "          Plugins: $MPdir.Plugins"
  ${LOG_TEXT} "INFO" "          Log: $MPdir.Log"
  ${LOG_TEXT} "INFO" "          CustomInputDevice: $MPdir.CustomInputDevice"
  ${LOG_TEXT} "INFO" "          CustomInputDefault: $MPdir.CustomInputDefault"
  ${LOG_TEXT} "INFO" "          Skin: $MPdir.Skin"
  ${LOG_TEXT} "INFO" "          Language: $MPdir.Language"
  ${LOG_TEXT} "INFO" "          Database: $MPdir.Database"
  ${LOG_TEXT} "INFO" "          Thumbs: $MPdir.Thumbs"
  ${LOG_TEXT} "INFO" "          Weather: $MPdir.Weather"
  ${LOG_TEXT} "INFO" "          Cache: $MPdir.Cache"
  ${LOG_TEXT} "INFO" "          BurnerSupport: $MPdir.BurnerSupport"


  Pop $0
  !verbose pop
!macroend

!endif # !___MediaPortalDirectories__NSH___

