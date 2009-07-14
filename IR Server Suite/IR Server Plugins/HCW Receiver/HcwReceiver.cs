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
  /// IR Server plugin supporting Hauppauge devices.
  /// </summary>
  [CLSCompliant(false)]
  public class HcwReceiver : PluginBase, IRemoteReceiver, IConfigure
  {
    #region Constants

    private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "HCW Receiver.xml");

    #endregion Constants

    #region Variables

    private IrRemoteWrapper _irRemoteWrapper;

    private int _lastCode;
    private DateTime _lastCodeTime = DateTime.Now;
    private RemoteHandler _remoteButtonHandler;
    private int _repeatDelay;
    private bool _startIrExe;
    private bool _stopIrExe;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "Hauppauge"; }
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
      get { return "and-81, original MediaPortal code by mPod"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Support for Hauppauge IR devices"; }
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
    /// <param name="owner">The owner window to use for creating modal dialogs.</param>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Configure config = new Configure();

      config.RepeatDelay = _repeatDelay;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _repeatDelay = config.RepeatDelay;

        SaveSettings();
      }
    }

    #endregion

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

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      _irRemoteWrapper = new IrRemoteWrapper();
      _irRemoteWrapper.ButtonCallback = ButtonCallback;

      if (_stopIrExe)
        _irRemoteWrapper.StopIrExe();

      _irRemoteWrapper.Start();
    }

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      //_irRemoteWrapper.Stop();
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      //_irRemoteWrapper.Start();
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      _irRemoteWrapper.Stop();

      if (_startIrExe)
        _irRemoteWrapper.StartIrExe();
    }


    private void ButtonCallback(int button)
    {
      DateTime now = DateTime.Now;
      TimeSpan timeSpan = now - _lastCodeTime;

      if (_lastCode != button || timeSpan.Milliseconds >= _repeatDelay)
      {
        if (_remoteButtonHandler != null)
          _remoteButtonHandler(Name, button.ToString());

        _lastCodeTime = now;
      }

      _lastCode = button;
    }


    private void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _repeatDelay = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
        _stopIrExe = bool.Parse(doc.DocumentElement.Attributes["StopIrExe"].Value);
        _startIrExe = bool.Parse(doc.DocumentElement.Attributes["StartIrExe"].Value);
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif
        _repeatDelay = 500;
        _stopIrExe = true;
        _startIrExe = true;
      }
    }

    private void SaveSettings()
    {
      try
      {
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char) 9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("RepeatDelay", _repeatDelay.ToString());
        writer.WriteAttributeString("StopIrExe", _stopIrExe.ToString());
        writer.WriteAttributeString("StartIrExe", _startIrExe.ToString());

        writer.WriteEndElement(); // </settings>
        writer.WriteEndDocument();
        writer.Close();
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
  }
}