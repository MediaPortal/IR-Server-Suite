#region Copyright (C) 2005-2012 Team MediaPortal

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
using IrssUtils;

namespace IRServer.Plugin
{
  public partial class DirectInputReceiver
  {
    private static class ConfigManagement
    {
      /// <summary>
      /// Loads the settings.
      /// </summary>
      public static void LoadSettings(ref Config config)
      {
        IrssLog.Info("LoadSettings()");
        XmlSerializer ser = new XmlSerializer(typeof (Config));
        StreamReader sr = new StreamReader(ConfigurationFile);

        try
        {
          config = (Config) ser.Deserialize(sr);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex.ToString());
          config = new Config();
        }

        sr.Close();
      }

      /// <summary>
      /// Saves the settings.
      /// </summary>
      public static void SaveSettings(Config config)
      {
        IrssLog.Info("SaveSettings()");
        XmlSerializer ser = new XmlSerializer(typeof (Config));
        FileStream str = new FileStream(ConfigurationFile, FileMode.Create);

        try
        {
          ser.Serialize(str, config);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex.ToString());
        }

        str.Close();
      }
    }
  }
}