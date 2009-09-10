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
using IrssUtils;

namespace IRServer.Plugin
{
  /// <summary>
  /// IR Server Plugin for X10 Transceiver devices.
  /// </summary>
  public class FireDTVReceiver : PluginBase, IRemoteReceiver, IConfigure
  {
    #region Constants

    private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "FireDTV Receiver.xml");

    #endregion Constants

    #region Variables

    private static RemoteHandler _remoteButtonHandler;
    private string _deviceName;
    private FireDTVControl _fireDTV = null;
    private ReceiverWindow _receiverWindow;
    private bool _running;
    #endregion Variables

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "FireDTV"; }
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
      get { return "MisterD, with original MediaPortal code"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "FireDTV Receiver"; }
    }

    /// <summary>
    /// Gets a display icon for the plugin.
    /// </summary>
    /// <value>The icon.</value>
    public override Icon DeviceIcon
    {
      get { return Resources.Icon; }
    }

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
    /// Detect the presence of this device.
    /// </summary>
    public override DetectionResult Detect()
    {
      if (IrssUtils.Win32.Check64Bit())
      {
        IrssLog.Warn("Plugin {0}: not available on current OS architecture (x64)", Name);
        return DetectionResult.DeviceDisabled;
      }
      try
      {
        new FireDTVControl((IntPtr)0);
        return DetectionResult.DevicePresent;
      }
      catch (FileNotFoundException)
      {
        return DetectionResult.DeviceNotFound;
      }
      catch (Exception ex)
      {
        IrssLog.Error("{0} exception: {1} type: {2}", Name, ex.Message, ex.GetType());
        return DetectionResult.DeviceException;
      }
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      if (!_running)
      {
        LoadSettings();

        _receiverWindow = new ReceiverWindow("FireDTV Receiver");
        _receiverWindow.ProcMsg += WndProc;

        _fireDTV = new FireDTVControl(_receiverWindow.Handle);
        if (!_fireDTV.OpenDrivers())
        {
          throw new InvalidOperationException("Failed to start FireDTV interface");
        }

        // Search for the named sourcefilter

        FireDTVSourceFilterInfo sourceFilter;
        if (string.IsNullOrEmpty(_deviceName))
        {
          sourceFilter = _fireDTV.SourceFilters.Item(0);
        }
        else
        {
          sourceFilter = _fireDTV.SourceFilters.ItemByName(_deviceName);
        }

        if (sourceFilter != null)
        {
          sourceFilter.StartFireDTVRemoteControlSupport();
          _running = true;
        }
        else
        {
          throw new InvalidOperationException("Failed to start FireDTV interface");
        }
      }
    }

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
      Stop();
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
      Start();
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      if (_running)
      {
        if (_fireDTV != null)
        {
          _fireDTV.CloseDrivers();
        }
        if (_receiverWindow != null)
        {
          _receiverWindow.DestroyHandle();
          _receiverWindow = null;
        }
        _running = false;
      }
    }

    private void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _deviceName = doc.DocumentElement.Attributes["deviceName"].Value;
      }
#if TRACE
      catch (Exception ex)
      {
        Trace.WriteLine(ex.ToString());
#else
      catch
      {
#endif
        _deviceName = null;
      }
    }

    private void SaveSettings()
    {
      try
      {
        XmlTextWriter writer = new XmlTextWriter(ConfigurationFile, Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 1;
        writer.IndentChar = (char)9;
        writer.WriteStartDocument(true);
        writer.WriteStartElement("settings"); // <settings>

        writer.WriteAttributeString("deviceName", string.IsNullOrEmpty(_deviceName) ? "" : _deviceName);

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

    /// <summary>
    /// Configure the IR Server plugin.
    /// </summary>
    /// <param name="owner">The owner window to use for creating modal dialogs.</param>
    public void Configure(IWin32Window owner)
    {
      LoadSettings();

      Configure config = new Configure { DeviceName = _deviceName };
      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _deviceName = config.DeviceName;
        SaveSettings();
        Stop();
        Start();
      }

    }

    private void WndProc(ref Message msg)
    {
      switch ((FireDTVConstants.FireDTVWindowMessages)msg.Msg)
      {
        case FireDTVConstants.FireDTVWindowMessages.DeviceAttached:
          Start();
          break;

        case FireDTVConstants.FireDTVWindowMessages.DeviceDetached:
          _fireDTV.SourceFilters.RemoveByHandle((uint)msg.WParam);
          break;

        case FireDTVConstants.FireDTVWindowMessages.DeviceChanged:
          Start();
          break;

        case FireDTVConstants.FireDTVWindowMessages.RemoteControlEvent:
          int remoteKeyCode = msg.LParam.ToInt32();
          if (RemoteCallback != null)
            RemoteCallback(Name, remoteKeyCode.ToString());
          break;
      }
    }

  }
}