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
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using IRServer.Plugin.Properties;

namespace IRServer.Plugin
{
  /// <summary>
  /// IR Server Plugin for using Girder 3.x plugins.
  /// </summary>
  public class GirderPlugin : PluginBase, IRemoteReceiver, ITransmitIR, IConfigure
  {
    #region Constants

    private const int GIRINFO_POWERBROADCAST = 2;
    private const int PBT_APMRESUMEAUTOMATIC = 18;
    private const int PBT_APMSUSPEND = 4;
    private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Girder Plugin.xml");

    private static readonly string DefaultPluginFolder =
      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "girder\\plugins");

    private static readonly string[] Ports = new string[] {"Plugin"};

    #endregion Constants

    #region Variables

    private string _pluginFile;
    private string _pluginFolder;

    private GirderPluginWrapper _pluginWrapper;
    private RemoteHandler _remoteButtonHandler;

    #endregion Variables

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "Girder Plugin"; }
    }

    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version
    {
      get { return "1.4.2.0"; }
    }

    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author
    {
      get { return "and-81"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Supports using Girder 3.x plugins"; }
    }

    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon
    {
      get { return Resources.Icon; }
    }

    #region IConfigure Members

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Config config = new Config();
      config.PluginFolder = _pluginFolder;
      config.PluginFile = _pluginFile;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _pluginFolder = config.PluginFolder;
        _pluginFile = config.PluginFile;

        SaveSettings();
      }
    }

    #endregion

    #region Implementation

    private void PluginCallback(string eventstring, IntPtr payload, int len, int device)
    {
      if (_remoteButtonHandler != null)
        _remoteButtonHandler(Name, eventstring);
    }

    private void LoadSettings()
    {
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
        _pluginFolder = doc.DocumentElement.Attributes["PluginFolder"].Value;
      }
      catch
      {
        _pluginFolder = DefaultPluginFolder;
      }

      try
      {
        _pluginFile = doc.DocumentElement.Attributes["PluginFile"].Value;
      }
      catch
      {
      }
    }

    private void SaveSettings()
    {
      try
      {
        using (XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8))
        {
          writer.Formatting = Formatting.Indented;
          writer.Indentation = 1;
          writer.IndentChar = (char) 9;
          writer.WriteStartDocument(true);
          writer.WriteStartElement("settings"); // <settings>

          writer.WriteAttributeString("PluginFolder", _pluginFolder);
          writer.WriteAttributeString("PluginFile", _pluginFile);

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

    #endregion Implementation

    #region IRemoteReceiver Members

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    /// <value>The remote callback.</value>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }

    #endregion

    #region ITransmitIR Members

    /// <summary>
    /// Lists the available blaster ports.
    /// </summary>
    /// <value>The available ports.</value>
    public string[] AvailablePorts
    {
      get { return Ports; }
    }

    /// <summary>
    /// Transmit an infrared command.
    /// </summary>
    /// <param name="port">Port to transmit on.</param>
    /// <param name="data">Data to transmit.</param>
    /// <returns><c>true</c> if successful, otherwise <c>false</c>.</returns>
    public bool Transmit(string port, byte[] data)
    {
      string xmlData = Encoding.ASCII.GetString(data);

      GirCommand blastCommand = GirCommand.FromXml(xmlData);

      return _pluginWrapper.GirEvent(blastCommand, String.Empty, IntPtr.Zero, 0, String.Empty, 0);
    }

    #endregion

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      if (String.IsNullOrEmpty(_pluginFile))
        throw new InvalidOperationException("No girder plugin file selected");

      string file = Path.Combine(_pluginFolder, _pluginFile);

      _pluginWrapper = new GirderPluginWrapper(file);

      _pluginWrapper.EventCallback += PluginCallback;

      if (!_pluginWrapper.GirOpen())
      {
        _pluginWrapper.Dispose();
        _pluginWrapper = null;

        throw new InvalidOperationException("Failed to initiate girder plugin");
      }

      if (!_pluginWrapper.GirStart())
      {
        _pluginWrapper.GirClose();
        _pluginWrapper.Dispose();
        _pluginWrapper = null;

        throw new InvalidOperationException("Failed to start girder plugin");
      }
    }

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      if (_pluginWrapper == null)
        return;

      _pluginWrapper.GirInfo(GIRINFO_POWERBROADCAST, PBT_APMSUSPEND, 0);
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      if (_pluginWrapper == null)
        return;

      _pluginWrapper.GirInfo(GIRINFO_POWERBROADCAST, PBT_APMRESUMEAUTOMATIC, 0);
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      if (_pluginWrapper == null)
        return;

      _pluginWrapper.GirStop();

      _pluginWrapper.GirClose();

      _pluginWrapper.Dispose();

      _pluginWrapper = null;
    }
  }
}