!define LANG "ENGLISH" ; Must be the lang name define my NSIS

!insertmacro LANG_STRING ^UninstallLink                      "Uninstall $(^Name)"

!insertmacro LANG_STRING TEXT_MP_NOT_INSTALLED        "MediaPortal not installed"
!insertmacro LANG_STRING TEXT_TVSERVER_NOT_INSTALLED  "TVServer not installed"

!insertmacro LANG_STRING TEXT_MSGBOX_REMOVE_ALL              "Do you want to remove your User settings?$\r$\nAttention: This will remove all your customised settings including Skins and Databases."

!insertmacro LANG_STRING TEXT_MSGBOX_ERROR_ON_UNINSTALL               "An error occured while trying to uninstall old version!$\r$\nDo you still want to continue the installation?"
!insertmacro LANG_STRING TEXT_MSGBOX_ERROR_REBOOT_REQUIRED            "A reboot is required after a previous action. Reboot you system and try it again."


# Descriptions for components (sections)
!insertmacro LANG_STRING DESC_SectionIRServer        "A windows service that provides access to your IR devices."

!insertmacro LANG_STRING DESC_SectionGroupMP             "MediaPortal plugins."
!insertmacro LANG_STRING DESC_SectionMPControlPlugin     "Connects to the IR Server to control MediaPortal."
!insertmacro LANG_STRING DESC_SectionMPBlastZonePlugin   "Lets you control your IR devices from within the MediaPortal GUI."
!insertmacro LANG_STRING DESC_SectionTV2BlasterPlugin    "For tuning external channels (on Set Top Boxes) with the default MediaPortal TV engine."

!insertmacro LANG_STRING DESC_SectionGroupTV3            "MediaPortal TV Server plugins."
!insertmacro LANG_STRING DESC_SectionTV3BlasterPlugin    "For tuning external channels (on Set Top Boxes) with the MediaPortal TV server."
!insertmacro LANG_STRING DESC_SectionGroupMCE            "Windows Media Center add-ons."

!insertmacro LANG_STRING DESC_SectionDebugClient         "Simple testing tool for troubleshooting input and communications problems."
!insertmacro LANG_STRING DESC_SectionIRFileTool          "Tool for learning, modifying, testing, correcting and converting IR command files."
!insertmacro LANG_STRING DESC_SectionKeyboardInputRelay  "Relays keyboard input to the IR Server to act on keypresses like remote buttons."
!insertmacro LANG_STRING DESC_SectionTranslator          "Control your whole PC."
!insertmacro LANG_STRING DESC_SectionTrayLauncher        "Simple program to launch an application of your choosing when a particular button is pressed."
!insertmacro LANG_STRING DESC_SectionVirtualRemote       "Simulated remote control. Includes PC application, web, and Smart Devices versions."

!insertmacro LANG_STRING DESC_SectionIRBlast             "Command line tools for blasting IR codes."
!insertmacro LANG_STRING DESC_SectionDboxTuner           "Command line tuner for Dreambox devices."
!insertmacro LANG_STRING DESC_SectionHcwPvrTuner         "Command line tuner for Hauppauge PVR devices."
!insertmacro LANG_STRING DESC_SectionMCEBlaster          "For tuning external channels (on Set Top Boxes) with Windows Media Center."

# Strings for AddRemove-Page
!insertmacro LANG_STRING TEXT_ADDREMOVE_HEADER          "Already Installed"
!insertmacro LANG_STRING TEXT_ADDREMOVE_HEADER2_REPAIR  "Choose the maintenance option to perform."
!insertmacro LANG_STRING TEXT_ADDREMOVE_HEADER2_UPDOWN  "Choose how you want to install $(^Name)."
!insertmacro LANG_STRING TEXT_ADDREMOVE_INFO_SELECT     "Select the operation you want to perform and click Next to continue."
!insertmacro LANG_STRING TEXT_ADDREMOVE_INFO_REPAIR     "$(^Name) ${VERSION} is already installed. $(TEXT_ADDREMOVE_INFO_SELECT)"
!insertmacro LANG_STRING TEXT_ADDREMOVE_INFO_UPGRADE    "An older version of $(^Name) is installed on your system. $(TEXT_ADDREMOVE_INFO_SELECT)"
!insertmacro LANG_STRING TEXT_ADDREMOVE_INFO_DOWNGRADE  "A newer version of $(^Name) is already installed! It is not recommended that you install an older version. $(TEXT_ADDREMOVE_INFO_SELECT)"
!insertmacro LANG_STRING TEXT_ADDREMOVE_REPAIR_OPT1     "Add/Remove/Reinstall components"
!insertmacro LANG_STRING TEXT_ADDREMOVE_REPAIR_OPT2     "Uninstall $(^Name)"
!insertmacro LANG_STRING TEXT_ADDREMOVE_UPDOWN_OPT1     "Upgrade $(^Name) using previous settings (recommended)"
!insertmacro LANG_STRING TEXT_ADDREMOVE_UPDOWN_OPT2     "Change settings (advanced)"


# Strings for UninstallMode-Page
!insertmacro LANG_STRING TEXT_UNMODE_HEADER       "Uninstallation Mode"
!insertmacro LANG_STRING TEXT_UNMODE_HEADER2      "Please choose the mode, you want to do the uninstallation."
!insertmacro LANG_STRING TEXT_UNMODE_OPT0         "Standard Uninstall (recommended)"
!insertmacro LANG_STRING TEXT_UNMODE_OPT1         "Complete Uninstallation for ${PRODUCT_NAME}"
;!insertmacro LANG_STRING TEXT_UNMODE_OPT2         "Full MediaPortal Products cleanup"
!insertmacro LANG_STRING TEXT_UNMODE_OPT0_DESC    "Only the main application will be uninstalled. Userfiles and settings won't be deleted (recommended)"
!insertmacro LANG_STRING TEXT_UNMODE_OPT1_DESC    "This will uninstall ${PRODUCT_NAME} and remove all userfiles"
;!insertmacro LANG_STRING TEXT_UNMODE_OPT2_DESC    "This will also remove all files, folders, databases, settings and registry keys which might be leftovers from older MediaPortal versions."
!insertmacro LANG_STRING TEXT_UNMODE_OPT1_MSGBOX  "Are you sure that you want to do a Complete Uninstallation? This can not be undone!"
;!insertmacro LANG_STRING TEXT_UNMODE_OPT2_MSGBOX  "Are you sure that you want to do a Full MediaPortal Products cleanup? This can not be undone!"



# Strings for ServerServiceMode-Page
!insertmacro LANG_STRING ServerServiceModePage_HEADER    "Choose the Server execution Mode"
!insertmacro LANG_STRING ServerServiceModePage_HEADER2   "Choose whether you want to install IRServer as Service or Application"
!insertmacro LANG_STRING ServerServiceModePage_INFO      "Some Plugins doesn't work together with a windows service. If you have such a remote listed below, choose IRServer."

!insertmacro LANG_STRING ServerServiceModePage_OPT0      "IR Server as Service (recommended)"
!insertmacro LANG_STRING ServerServiceModePage_OPT0_DESC "Install IR Server as a windows service. If you don't know what to choose and you don't have a remote listed below, choose this option."

!insertmacro LANG_STRING ServerServiceModePage_OPT1      "IR Server as Application"
!insertmacro LANG_STRING ServerServiceModePage_OPT1_DESC "Install IR Server as a windows application. Choose this if you have one of the following remotes:$\r$\n     - HID, FireDTV, FloppyDTV and some iMON devices"

