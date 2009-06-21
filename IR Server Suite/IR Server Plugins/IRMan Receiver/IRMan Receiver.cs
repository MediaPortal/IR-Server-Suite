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
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using InputService.Plugin.Properties;

namespace InputService.Plugin
{
  /// <summary>
  /// IR Server Plugin for IRMan Receiver device.
  /// </summary>
  public class IRManReceiver : PluginBase, IConfigure, IRemoteReceiver
  {
    #region Constants

    private const int DeviceBufferSize = 6;
    private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "IRMan Receiver.xml");

    #endregion Constants

    #region Variables

    private byte[] _deviceBuffer;

    private bool _disposed;

    private string _lastCode = String.Empty;
    private DateTime _lastCodeTime = DateTime.Now;
    private RemoteHandler _remoteButtonHandler;

    private int _repeatDelay;
    private SerialPort _serialPort;
    private string _serialPortName;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "IRMan"; }
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
      get { return "Receiver support for the Serial IRMan device"; }
    }

    /// <summary>
    /// Gets the plugin icon.
    /// </summary>
    /// <value>The plugin icon.</value>
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

      Configure config = new Configure();

      config.RepeatDelay = _repeatDelay;
      config.CommPort = _serialPortName;

      if (config.ShowDialog(owner) == DialogResult.OK)
      {
        _repeatDelay = config.RepeatDelay;
        _serialPortName = config.CommPort;

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

      _deviceBuffer = new byte[DeviceBufferSize];

      _serialPort = new SerialPort(_serialPortName, 9600, Parity.None, 8, StopBits.One);
      _serialPort.Handshake = Handshake.None;
      _serialPort.DtrEnable = true;
      _serialPort.RtsEnable = true;
      _serialPort.ReadBufferSize = DeviceBufferSize;
      _serialPort.ReadTimeout = 1000;

      _serialPort.Open();
      Thread.Sleep(100);
      _serialPort.DiscardInBuffer();

      _serialPort.Write("I");
      Thread.Sleep(100);
      _serialPort.Write("R");
      Thread.Sleep(100);

      _serialPort.Read(_deviceBuffer, 0, 2);

      if (_deviceBuffer[0] == 'O' && _deviceBuffer[1] == 'K')
      {
        _serialPort.ReceivedBytesThreshold = DeviceBufferSize;
        _serialPort.DataReceived += SerialPort_DataReceived;
      }
      else
      {
        throw new IOException("Failed to initialize device");
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
      if (_serialPort == null)
        return;

      try
      {
        _serialPort.Close();
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
      finally
      {
        _serialPort = null;
      }
    }

    /// <summary>
    /// Handles the DataReceived event of the SerialPort control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.IO.Ports.SerialDataReceivedEventArgs"/> instance containing the event data.</param>
    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
      try
      {
        _serialPort.Read(_deviceBuffer, 0, DeviceBufferSize);

        TimeSpan timeSpan = DateTime.Now - _lastCodeTime;

        StringBuilder keyCode = new StringBuilder(2 * DeviceBufferSize);
        for (int index = 0; index < DeviceBufferSize; index++)
          keyCode.Append(_deviceBuffer[index].ToString("X2"));

        string thisCode = keyCode.ToString();

        if (thisCode.Equals(_lastCode, StringComparison.Ordinal)) // Repeated button
        {
          if (timeSpan.Milliseconds > _repeatDelay)
          {
            _remoteButtonHandler(Name, thisCode);
            _lastCodeTime = DateTime.Now;
          }
        }
        else
        {
          _remoteButtonHandler(Name, thisCode);
          _lastCodeTime = DateTime.Now;
        }

        _lastCode = thisCode;
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
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      // process only if mananged and unmanaged resources have
      // not been disposed of.
      if (!_disposed)
      {
        if (disposing)
        {
          // dispose managed resources
          Stop();
        }

        // dispose unmanaged resources
        _disposed = true;
      }
    }

    /// <summary>
    /// Loads the settings.
    /// </summary>
    private void LoadSettings()
    {
      try
      {
        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationFile);

        _repeatDelay = int.Parse(doc.DocumentElement.Attributes["RepeatDelay"].Value);
        _serialPortName = doc.DocumentElement.Attributes["SerialPortName"].Value;
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
        _serialPortName = "COM1";
      }
    }

    /// <summary>
    /// Saves the settings.
    /// </summary>
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

          writer.WriteAttributeString("RepeatDelay", _repeatDelay.ToString());
          writer.WriteAttributeString("SerialPortName", _serialPortName);

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
  }
}