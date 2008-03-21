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

#**********************************************************************************************************#
#
# This header contains multilanguage strings for the setup routine
#
#**********************************************************************************************************#

# ENGLISH
LangString DESC_SectionInputService       ${LANG_ENGLISH} "A windows service that provides access to your IR devices."
LangString DESC_SectionMPControlPlugin    ${LANG_ENGLISH} "Connects to the Input Service to control MediaPortal."
LangString DESC_SectionMPBlastZonePlugin  ${LANG_ENGLISH} "Lets you control your IR devices from within the MediaPortal GUI."
LangString DESC_SectionTV2BlasterPlugin   ${LANG_ENGLISH} "For tuning external channels (on Set Top Boxes) with the default MediaPortal TV engine."
LangString DESC_SectionTV3BlasterPlugin   ${LANG_ENGLISH} "For tuning external channels (on Set Top Boxes) with the MediaPortal TV server."
LangString DESC_SectionTranslator         ${LANG_ENGLISH} "Control your whole PC."
LangString DESC_SectionTrayLauncher       ${LANG_ENGLISH} "Simple tray application to launch an application of your choosing when a particular button is pressed."
LangString DESC_SectionVirtualRemote      ${LANG_ENGLISH} "Simulated remote control, works as an application or as a web hosted remote control (with included Web Remote).  Also includes a Skin Editor and Smart Device versions of Virtual Remote."
LangString DESC_SectionIRBlast            ${LANG_ENGLISH} "Command line tools for blasting IR codes."
LangString DESC_SectionIRFileTool         ${LANG_ENGLISH} "Tool for learning, modifying, testing, correcting and converting IR command files."
LangString DESC_SectionKeyboardInputRelay ${LANG_ENGLISH} "Relays keyboard input to the Input Service to act on keypresses like remote buttons."
LangString DESC_SectionDboxTuner          ${LANG_ENGLISH} "Command line tuner for Dreambox devices."
LangString DESC_SectionHcwPvrTuner        ${LANG_ENGLISH} "Command line tuner for Hauppauge PVR devices."
LangString DESC_SectionDebugClient        ${LANG_ENGLISH} "Very simple testing tool for troubleshooting input and communications problems."


LangString ^UninstallLink                         ${LANG_ENGLISH                        ${LANG_ENGLISH} "Uninstall $(^Name)"

LangString TEXT_MSGBOX_REMOVE_ALL                 ${LANG_ENGLISH                        ${LANG_ENGLISH} "Do you want to remove your User settings?$\r$\nAttention: This will remove all your customised settings including Skins and Databases."

LangString TEXT_MSGBOX_ERROR_DOTNET               ${LANG_ENGLISH                        ${LANG_ENGLISH} "Microsoft .Net Framework Runtime is a prerequisite. Please install first."
LangString TEXT_MSGBOX_ERROR_WIN                  ${LANG_ENGLISH                        ${LANG_ENGLISH} "MediaPortal requires at least Windows XP. Your Windows is not supported. Installation aborted"
LangString TEXT_MSGBOX_ERROR_IS_INSTALLED         ${LANG_ENGLISH                        ${LANG_ENGLISH} "MediaPortal is already installed. You need to uninstall it, before you continue with the installation.$\r$\nUninstall will be lunched when pressing OK."
LangString TEXT_MSGBOX_ERROR_ON_UNINSTALL         ${LANG_ENGLISH                        ${LANG_ENGLISH} "An error occured while trying to uninstall old version!$\r$\nDo you still want to continue the installation?"
LangString TEXT_MSGBOX_ERROR_REBOOT_REQUIRED      ${LANG_ENGLISH                        ${LANG_ENGLISH} "REBOOT IS REQUIRED. DO THAT!!!!"
