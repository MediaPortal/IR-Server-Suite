using System;
using System.Collections.Generic;
using System.Text;
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
        StreamReader sr = new StreamReader(ConfigurationFile);

        try
        {
          config = (Config) ser.Deserialize(sr);
        }
        catch (Exception ex)
        {
          Debug.WriteLine(ex.ToString());
          config = new Config();
        }

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