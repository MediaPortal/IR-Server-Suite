#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Collections.Generic;


namespace IrssUtils
{
    /// <summary>
    /// Log file recording class.
    /// </summary>
    public static class IrssHelp
    {
        #region attributes and settings

        /// <summary>
        /// Root URL where the documentqtion is
        /// </summary>
        const string _mainUrl = @"http://wiki.team-mediaportal.com/3_IRSS_(IR_Server_Suite)/";

        /// <summary>
        /// Look-up table associating the different object to there associated help (sub-)page
        /// </summary>
        private static readonly Dictionary<string, string> _docMap = new Dictionary<string, string> 
        {   { "Translator"                    ,  "Apps/Translator"}
          , { "IrssUtils.Forms.MacroEditor"   ,  "Apps/Macro_Editor"}
          , { "IrssUtils.Forms.IREditor"      ,  "Apps/Translator"}
          , { "IrssUtils.Forms.CommandManager",  "Commands"}
          , { "IrssUtils.Forms.MouseCommand"  ,  "Commands/General_Commands/Mouse"}
        };

        #endregion attributes and settings

        #region class-functions

        /// <summary>
        /// Indicate whether an object has a dedicated Help-entry.
        /// </summary>
        /// <param name="obj">the name of the component</param>
        /// <returns>true if there is  a registered help-entry</returns>
        public static bool HasEntry(string obj)
        {
            return _docMap.ContainsKey(obj);
        }

        /// <summary>
        /// Indicate whether an object has a dedicated Help-entry.
        /// </summary>
        /// <param name="obj">the name of the component, as string</param>
        /// <returns>true if there is  a registered help-entry</returns>
        public static bool HasEntry(object obj)
        {
            return HasEntry(obj.GetType().ToString());
        }


        /// <summary>
        /// open the help entry for the given component.
        /// </summary>
        /// <param name="obj">the name of the component, as string</param>
        public static void Open(string obj)
        {
            // Find closest exact or parent entry
            string key = obj;
            while (key != "" && !_docMap.ContainsKey(key) )
            {
                int idx = key.IndexOf('.');
                if (idx<1) key = "";
                else key = key.Substring(0, idx);
            }

            string suburl = "";
            if (key != "") suburl = _docMap[key];

            // Open the help URL
            string url = _mainUrl + suburl; 
            IrssLog.Info("open help: " + url);
            Process.Start(url);
        }


        /// <summary>
        /// open the help entry for the given component.
        /// </summary>
        /// <param name="obj">the obejct (component) from which the help is called</param>
        public static void Open(object obj)
        {
            Open(obj.GetType().ToString());
        }

        #endregion class-functions
    }
}