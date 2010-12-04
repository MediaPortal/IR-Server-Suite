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

#if TRACE
using System;
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using IRServer.Plugin.Properties;
using WiimoteLib;

namespace IRServer.Plugin
{
  /// <summary>
  /// IR Server Plugin for the Wii Remote.
  /// </summary>
  public class WiiRemoteReceiver : PluginBase, IRemoteReceiver, IMouseReceiver, IConfigure
  {
    #region Constants

    private static readonly string ConfigurationFile = Path.Combine(ConfigurationPath, "Wii Remote Receiver.xml");

    private readonly int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;
    private readonly int ScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;

    #endregion Constants

    #region Variables

    private bool _handleMouseLocally = true;

    private bool _led1;
    private bool _led2 = true;
    private bool _led3 = true;
    private bool _led4;
    private MouseHandler _mouseHandler;
    private double _mouseSensitivity = 1.5;

    private WiimoteState _previousState;
    private RemoteHandler _remoteButtonHandler;
    private bool _useNunchukAsMouse;
    private Wiimote _wiimote;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name
    {
      get { return "Wii Remote"; }
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
      get { return "and-81, using the WiimoteLib by Brian Peek"; }
    }

    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description
    {
      get { return "Supports the Nintendo Wii Remote"; }
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

      Setup setup = new Setup();

      setup.HandleMouseLocal = _handleMouseLocally;
      setup.UseNunchukAsMouse = _useNunchukAsMouse;
      setup.MouseSensitivity = _mouseSensitivity;
      setup.LED1 = _led1;
      setup.LED2 = _led2;
      setup.LED3 = _led3;
      setup.LED4 = _led4;

      if (setup.ShowDialog(owner) == DialogResult.OK)
      {
        _handleMouseLocally = setup.HandleMouseLocal;
        _useNunchukAsMouse = setup.UseNunchukAsMouse;
        _mouseSensitivity = setup.MouseSensitivity;
        _led1 = setup.LED1;
        _led2 = setup.LED2;
        _led3 = setup.LED3;
        _led4 = setup.LED4;

        SaveSettings();
      }
    }

    #endregion

    #region IMouseReceiver Members

    /// <summary>
    /// Callback for mouse events.
    /// </summary>
    /// <value>The mouse callback.</value>
    public MouseHandler MouseCallback
    {
      get { return _mouseHandler; }
      set { _mouseHandler = value; }
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
    /// Detect the presence of this device.
    /// </summary>
    public override DetectionResult Detect()
    {
      // TODO: Add Wii Remote detection

      return DetectionResult.DeviceNotFound;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    public override void Start()
    {
      LoadSettings();

      _wiimote = new Wiimote();

      try
      {
        _wiimote.WiimoteChanged += WiimoteChanged;
        _wiimote.WiimoteExtensionChanged += WiimoteExtensionChanged;

        _wiimote.Connect();
        _wiimote.SetReportType(InputReport.IRAccel, true);
        _wiimote.SetLEDs(_led1, _led2, _led3, _led4);
        _wiimote.SetRumble(false);
      }
      catch
      {
        throw;
      }
      finally
      {
        _wiimote.Dispose();
        _wiimote = null;
      }
    }

    /// <summary>
    /// Suspend the IR Server plugin when computer enters standby.
    /// </summary>
    public override void Suspend()
    {
    }

    /// <summary>
    /// Resume the IR Server plugin when the computer returns from standby.
    /// </summary>
    public override void Resume()
    {
    }

    /// <summary>
    /// Stop the IR Server plugin.
    /// </summary>
    public override void Stop()
    {
      if (_wiimote == null)
        return;

      _wiimote.SetLEDs(false, false, false, false);
      _wiimote.SetRumble(false);
      _wiimote.Disconnect();
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
        _handleMouseLocally = bool.Parse(doc.DocumentElement.Attributes["HandleMouseLocally"].Value);
      }
      catch
      {
      }

      try
      {
        _useNunchukAsMouse = bool.Parse(doc.DocumentElement.Attributes["UseNunchukAsMouse"].Value);
      }
      catch
      {
      }

      try
      {
        _mouseSensitivity = double.Parse(doc.DocumentElement.Attributes["MouseSensitivity"].Value);
      }
      catch
      {
      }

      try
      {
        _led1 = bool.Parse(doc.DocumentElement.Attributes["LED1"].Value);
      }
      catch
      {
      }
      try
      {
        _led2 = bool.Parse(doc.DocumentElement.Attributes["LED2"].Value);
      }
      catch
      {
      }
      try
      {
        _led3 = bool.Parse(doc.DocumentElement.Attributes["LED3"].Value);
      }
      catch
      {
      }
      try
      {
        _led4 = bool.Parse(doc.DocumentElement.Attributes["LED4"].Value);
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

          writer.WriteAttributeString("HandleMouseLocally", _handleMouseLocally.ToString());
          writer.WriteAttributeString("UseNunchukAsMouse", _useNunchukAsMouse.ToString());
          writer.WriteAttributeString("MouseSensitivity", _mouseSensitivity.ToString());

          writer.WriteAttributeString("LED1", _led1.ToString());
          writer.WriteAttributeString("LED2", _led2.ToString());
          writer.WriteAttributeString("LED3", _led3.ToString());
          writer.WriteAttributeString("LED4", _led4.ToString());

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

    private void WiimoteChanged(object sender, WiimoteChangedEventArgs args)
    {
      WiimoteState ws = args.WiimoteState;

      if (_previousState != null)
      {
        if (ws.ButtonState.A && !_previousState.ButtonState.A) RemoteCallback(Name, "Wiimote_Button:A");
        if (ws.ButtonState.B && !_previousState.ButtonState.B) RemoteCallback(Name, "Wiimote_Button:B");
        if (ws.ButtonState.Home && !_previousState.ButtonState.Home) RemoteCallback(Name, "Wiimote_Button:Home");
        if (ws.ButtonState.Minus && !_previousState.ButtonState.Minus) RemoteCallback(Name, "Wiimote_Button:Minus");
        if (ws.ButtonState.One && !_previousState.ButtonState.One) RemoteCallback(Name, "Wiimote_Button:One");
        if (ws.ButtonState.Plus && !_previousState.ButtonState.Plus) RemoteCallback(Name, "Wiimote_Button:Plus");
        if (ws.ButtonState.Two && !_previousState.ButtonState.Two) RemoteCallback(Name, "Wiimote_Button:Two");

        if (ws.ButtonState.Down && !_previousState.ButtonState.Down) RemoteCallback(Name, "Wiimote_Pad:Down");
        if (ws.ButtonState.Left && !_previousState.ButtonState.Left) RemoteCallback(Name, "Wiimote_Pad:Left");
        if (ws.ButtonState.Right && !_previousState.ButtonState.Right) RemoteCallback(Name, "Wiimote_Pad:Right");
        if (ws.ButtonState.Up && !_previousState.ButtonState.Up) RemoteCallback(Name, "Wiimote_Pad:Up");

        if (ws.ExtensionType == ExtensionType.Nunchuk)
        {
          if (_useNunchukAsMouse) // Use nunchuk as mouse?
          {
            Mouse.MouseEvents mouseButtons = Mouse.MouseEvents.None;

            if (ws.NunchukState.C != _previousState.NunchukState.C)
              mouseButtons |= ws.NunchukState.C ? Mouse.MouseEvents.RightDown : Mouse.MouseEvents.RightUp;

            if (ws.NunchukState.Z != _previousState.NunchukState.Z)
              mouseButtons |= ws.NunchukState.Z ? Mouse.MouseEvents.LeftDown : Mouse.MouseEvents.LeftUp;

            int deltaX = (int) (ws.NunchukState.Joystick.X * 10 * _mouseSensitivity);
            int deltaY = (int) (ws.NunchukState.Joystick.Y * -10 * _mouseSensitivity);

            if (_handleMouseLocally)
            {
              if (deltaX != 0 || deltaY != 0)
                Mouse.Move(deltaX, deltaY, false);

              if (mouseButtons != Mouse.MouseEvents.None)
                Mouse.Button(mouseButtons);
            }
            else
            {
              if (deltaX != 0 || deltaY != 0 || mouseButtons != Mouse.MouseEvents.None)
                MouseCallback(Name, deltaX, deltaY, (int) mouseButtons);
            }
          }
          else
          {
            if (ws.NunchukState.C && !_previousState.NunchukState.C) RemoteCallback(Name, "WiimoteNunchuk_Button:C");
            if (ws.NunchukState.Z && !_previousState.NunchukState.Z) RemoteCallback(Name, "WiimoteNunchuk_Button:Z");
          }
        }

        if (ws.ExtensionType == ExtensionType.ClassicController)
        {
          if (ws.ClassicControllerState.ButtonState.A && !_previousState.ClassicControllerState.ButtonState.A)
            RemoteCallback(Name, "WiimoteClassic_Button:A");
          if (ws.ClassicControllerState.ButtonState.B && !_previousState.ClassicControllerState.ButtonState.B)
            RemoteCallback(Name, "WiimoteClassic_Button:B");
          if (ws.ClassicControllerState.ButtonState.Home && !_previousState.ClassicControllerState.ButtonState.Home)
            RemoteCallback(Name, "WiimoteClassic_Button:Home");
          if (ws.ClassicControllerState.ButtonState.Minus && !_previousState.ClassicControllerState.ButtonState.Minus)
            RemoteCallback(Name, "WiimoteClassic_Button:Minus");
          if (ws.ClassicControllerState.ButtonState.Plus && !_previousState.ClassicControllerState.ButtonState.Plus)
            RemoteCallback(Name, "WiimoteClassic_Button:Plus");
          if (ws.ClassicControllerState.ButtonState.X && !_previousState.ClassicControllerState.ButtonState.X)
            RemoteCallback(Name, "WiimoteClassic_Button:X");
          if (ws.ClassicControllerState.ButtonState.Y && !_previousState.ClassicControllerState.ButtonState.Y)
            RemoteCallback(Name, "WiimoteClassic_Button:Y");
          if (ws.ClassicControllerState.ButtonState.TriggerL &&
              !_previousState.ClassicControllerState.ButtonState.TriggerL)
            RemoteCallback(Name, "WiimoteClassic_Button:TriggerL");
          if (ws.ClassicControllerState.ButtonState.TriggerR &&
              !_previousState.ClassicControllerState.ButtonState.TriggerR)
            RemoteCallback(Name, "WiimoteClassic_Button:TriggerR");
          if (ws.ClassicControllerState.ButtonState.ZL && !_previousState.ClassicControllerState.ButtonState.ZL)
            RemoteCallback(Name, "WiimoteClassic_Button:ZL");
          if (ws.ClassicControllerState.ButtonState.ZR && !_previousState.ClassicControllerState.ButtonState.ZR)
            RemoteCallback(Name, "WiimoteClassic_Button:ZR");

          if (ws.ClassicControllerState.ButtonState.Down && !_previousState.ClassicControllerState.ButtonState.Down)
            RemoteCallback(Name, "WiimoteClassic_Pad:Down");
          if (ws.ClassicControllerState.ButtonState.Left && !_previousState.ClassicControllerState.ButtonState.Left)
            RemoteCallback(Name, "WiimoteClassic_Pad:Left");
          if (ws.ClassicControllerState.ButtonState.Right && !_previousState.ClassicControllerState.ButtonState.Right)
            RemoteCallback(Name, "WiimoteClassic_Pad:Right");
          if (ws.ClassicControllerState.ButtonState.Up && !_previousState.ClassicControllerState.ButtonState.Up)
            RemoteCallback(Name, "WiimoteClassic_Pad:Up");
        }

        if (ws.IRState.IRSensors[0].Found && ws.IRState.IRSensors[1].Found)
        {
          int x = (int)(ScreenWidth - (ws.IRState.IRSensors[0].Position.X + ws.IRState.IRSensors[1].Position.X) / 2 * ScreenWidth);
          int y = (int)((ws.IRState.IRSensors[0].Position.Y + ws.IRState.IRSensors[1].Position.Y) / 2 * ScreenHeight);

          if (_handleMouseLocally)
          {
            Mouse.Move(x, y, true);
          }
          else
          {
            int prevX = (int)(ScreenWidth - (_previousState.IRState.IRSensors[0].Position.X + _previousState.IRState.IRSensors[1].Position.X) / 2 * ScreenWidth);
            int prevY = (int)((_previousState.IRState.IRSensors[0].Position.Y + _previousState.IRState.IRSensors[1].Position.Y) / 2 * ScreenHeight);

            int deltaX = x - prevX;
            int deltaY = y - prevY;

            MouseCallback(Name, deltaX, deltaY, 0);
          }
        }
      }
      else
        _previousState = new WiimoteState();

      //_previousState.AccelCalibrationInfo.X0 = ws.AccelCalibrationInfo.X0;
      //_previousState.AccelCalibrationInfo.XG = ws.AccelCalibrationInfo.XG;
      //_previousState.AccelCalibrationInfo.Y0 = ws.AccelCalibrationInfo.Y0;
      //_previousState.AccelCalibrationInfo.YG = ws.AccelCalibrationInfo.YG;
      //_previousState.AccelCalibrationInfo.Z0 = ws.AccelCalibrationInfo.Z0;
      //_previousState.AccelCalibrationInfo.ZG = ws.AccelCalibrationInfo.ZG;

      //_previousState.AccelState.RawX = ws.AccelState.RawX;
      //_previousState.AccelState.RawY = ws.AccelState.RawY;
      //_previousState.AccelState.RawZ = ws.AccelState.RawZ;
      //_previousState.AccelState.X = ws.AccelState.X;
      //_previousState.AccelState.Y = ws.AccelState.Y;
      //_previousState.AccelState.Z = ws.AccelState.Z;

      //_previousState.Battery = ws.Battery;

      _previousState.ButtonState.A = ws.ButtonState.A;
      _previousState.ButtonState.B = ws.ButtonState.B;
      _previousState.ButtonState.Down = ws.ButtonState.Down;
      _previousState.ButtonState.Home = ws.ButtonState.Home;
      _previousState.ButtonState.Left = ws.ButtonState.Left;
      _previousState.ButtonState.Minus = ws.ButtonState.Minus;
      _previousState.ButtonState.One = ws.ButtonState.One;
      _previousState.ButtonState.Plus = ws.ButtonState.Plus;
      _previousState.ButtonState.Right = ws.ButtonState.Right;
      _previousState.ButtonState.Two = ws.ButtonState.Two;
      _previousState.ButtonState.Up = ws.ButtonState.Up;

      _previousState.Extension = ws.Extension;
      _previousState.ExtensionType = ws.ExtensionType;

      _previousState.IRState.IRSensors = ws.IRState.IRSensors;
      _previousState.IRState.Midpoint = ws.IRState.Midpoint;
      _previousState.IRState.Mode = ws.IRState.Mode;
      _previousState.IRState.RawMidpoint = ws.IRState.RawMidpoint;

      _previousState.NunchukState.C = ws.NunchukState.C;
      _previousState.NunchukState.Z = ws.NunchukState.Z;
    }

    private void WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs args)
    {
      if (args.Inserted)
        _wiimote.SetReportType(InputReport.IRExtensionAccel, true);
      else
        _wiimote.SetReportType(InputReport.IRAccel, true);
    }

    #endregion Implementation
  }
}