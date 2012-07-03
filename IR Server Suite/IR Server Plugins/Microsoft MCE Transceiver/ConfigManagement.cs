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
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using IrssUtils;

namespace IRServer.Plugin
{
  public partial class MicrosoftMceTransceiver
  {
    private static class ConfigManagement
    {
      /// <summary>
      /// Loads the settings. Still using old code to prevent breaking existing setting-files
      /// </summary>
      public static void LoadSettings(ref Config config)
      {
        IrssLog.Info("LoadSettings()");
        //XmlSerializer ser = new XmlSerializer(typeof (Config));
        //StreamReader sr = new StreamReader(ConfigurationFile);

        //try
        //{
        //  config = (Config) ser.Deserialize(sr);
        //}
        //catch (Exception ex)
        //{
        //  IrssLog.Error(ex.ToString());
        //  config = new Config();
        //}

        //sr.Close();
        XmlDocument doc = new XmlDocument();

        try
        {
          doc.Load(ConfigurationFile);
        }
        catch
        {
          return;
        }

        try
        {
          config.LearnTimeout = int.Parse(doc.DocumentElement.Attributes["LearnTimeout"].Value, CultureInfo.InvariantCulture);
        }
        catch
        {
        }
        try
        {
          config._disableMceServices = bool.Parse(doc.DocumentElement.Attributes["DisableMceServices"].Value);
        }
        catch
        {
        }

        try
        {
          config.EnableRemoteInput = bool.Parse(doc.DocumentElement.Attributes["EnableRemoteInput"].Value);
        }
        catch
        {
        }
        try
        {
          config.UseSystemRatesRemote = bool.Parse(doc.DocumentElement.Attributes["UseSystemRatesRemote"].Value);
        }
        catch
        {
        }
        try
        {
          config.RemoteFirstRepeat = int.Parse(doc.DocumentElement.Attributes["RemoteFirstRepeat"].Value,
                                         CultureInfo.InvariantCulture);
        }
        catch
        {
        }
        try
        {
          config.RemoteHeldRepeats = int.Parse(doc.DocumentElement.Attributes["RemoteHeldRepeats"].Value,
                                         CultureInfo.InvariantCulture);
        }
        catch
        {
        }
        try
        {
          config._disableAutomaticButtons = bool.Parse(doc.DocumentElement.Attributes["DisableAutomaticButtons"].Value);
        }
        catch
        {
        }

        try
        {
          config.EnableKeyboardInput = bool.Parse(doc.DocumentElement.Attributes["EnableKeyboardInput"].Value);
        }
        catch
        {
        }
        try
        {
          config.UseSystemRatesKeyboard = bool.Parse(doc.DocumentElement.Attributes["UseSystemRatesKeyboard"].Value);
        }
        catch
        {
        }
        try
        {
          config.KeyboardFirstRepeat = int.Parse(doc.DocumentElement.Attributes["KeyboardFirstRepeat"].Value,
                                           CultureInfo.InvariantCulture);
        }
        catch
        {
        }
        try
        {
          config.KeyboardHeldRepeats = int.Parse(doc.DocumentElement.Attributes["KeyboardHeldRepeats"].Value,
                                           CultureInfo.InvariantCulture);
        }
        catch
        {
        }
        try
        {
          config.HandleKeyboardLocally = bool.Parse(doc.DocumentElement.Attributes["HandleKeyboardLocally"].Value);
        }
        catch
        {
        }
        try
        {
          config.UseQwertzLayout = bool.Parse(doc.DocumentElement.Attributes["UseQwertzLayout"].Value);
        }
        catch
        {
        }

        try
        {
          config.EnableMouseInput = bool.Parse(doc.DocumentElement.Attributes["EnableMouseInput"].Value);
        }
        catch
        {
        }
        try
        {
          config.HandleMouseLocally = bool.Parse(doc.DocumentElement.Attributes["HandleMouseLocally"].Value);
        }
        catch
        {
        }
        try
        {
          config.MouseSensitivity = double.Parse(doc.DocumentElement.Attributes["MouseSensitivity"].Value,
                                           CultureInfo.InvariantCulture);
        }
        catch
        {
        }
      }

      /// <summary>
      /// Saves the settings. Still using old code to prevent breaking existing setting-files
      /// </summary>
      public static void SaveSettings(Config config)
      {
        IrssLog.Info("SaveSettings()");
        //XmlSerializer ser = new XmlSerializer(typeof(Config));
        //FileStream str = new FileStream(ConfigurationFile, FileMode.Create);

        //try
        //{
        //  ser.Serialize(str, config);
        //}
        //catch (Exception ex)
        //{
        //  IrssLog.Error(ex.ToString());
        //}

        //str.Close();
        try
        {
          using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
          {
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 1;
            writer.IndentChar = (char)9;
            writer.WriteStartDocument(true);
            writer.WriteStartElement("settings"); // <settings>

            writer.WriteAttributeString("LearnTimeout", config.LearnTimeout.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("DisableMceServices", config._disableMceServices.ToString());

            writer.WriteAttributeString("EnableRemoteInput", config.EnableRemoteInput.ToString());
            writer.WriteAttributeString("UseSystemRatesRemote", config.UseSystemRatesRemote.ToString());
            writer.WriteAttributeString("RemoteFirstRepeat", config.RemoteFirstRepeat.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("RemoteHeldRepeats", config.RemoteHeldRepeats.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("DisableAutomaticButtons", config._disableAutomaticButtons.ToString());

            writer.WriteAttributeString("EnableKeyboardInput", config.EnableKeyboardInput.ToString());
            writer.WriteAttributeString("UseSystemRatesKeyboard", config.UseSystemRatesKeyboard.ToString());
            writer.WriteAttributeString("KeyboardFirstRepeat", config.KeyboardFirstRepeat.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("KeyboardHeldRepeats", config.KeyboardHeldRepeats.ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("HandleKeyboardLocally", config.HandleKeyboardLocally.ToString());
            writer.WriteAttributeString("UseQwertzLayout", config.UseQwertzLayout.ToString());

            writer.WriteAttributeString("EnableMouseInput", config.EnableMouseInput.ToString());
            writer.WriteAttributeString("HandleMouseLocally", config.HandleMouseLocally.ToString());
            writer.WriteAttributeString("MouseSensitivity", config.MouseSensitivity.ToString(CultureInfo.InvariantCulture));

            writer.WriteEndElement(); // </settings>
            writer.WriteEndDocument();
          }
        }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
      }
#else
        catch
        {
        }
#endif
      }
    }
  }
}