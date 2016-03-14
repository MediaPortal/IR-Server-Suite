﻿#region Copyright (C) 2005-2012 Team MediaPortal

// Copyright (C) 2005-2012 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Xml.Serialization;
using System.IO;

namespace IRServer.Plugin
{
  public partial class PhilipsMceUsbIrReceiverSpinelPlus
  {
    private static class ConfigManagement
    {
      /// <summary>
      /// Loads the settings.
      /// </summary>
      public static void LoadSettings(ref Config config)
      {
        Debug.WriteLine("LoadSettings()");
        XmlSerializer ser = new XmlSerializer(typeof (Config));
        StreamReader sr = null;

        try
        {
          sr = new StreamReader(ConfigurationFile);
          config = (Config)ser.Deserialize(sr);
        }
        catch (Exception ex)
        {
          Debug.WriteLine(ex.ToString());
          config = new Config();
        }
        if (sr != null)
          sr.Close();
      }

      /// <summary>
      /// Saves the settings.
      /// </summary>
      public static void SaveSettings(Config config)
      {
        Debug.WriteLine("SaveSettings()");
        XmlSerializer ser = new XmlSerializer(typeof (Config));
        FileStream str = new FileStream(ConfigurationFile, FileMode.Create);

        try
        {
          ser.Serialize(str, config);
        }
        catch (Exception ex)
        {
          Debug.WriteLine(ex.ToString());
        }

        str.Close();
      }
    }
  }
}