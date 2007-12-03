using System;
using System.Diagnostics;
using System.Drawing;

using IRServerPluginInterface;

using WiimoteLib;

namespace WiiRemoteReceiver
{

  /// <summary>
  /// IR Server Plugin for the Wii Remote.
  /// </summary>
  public class WiiRemoteReceiver : IRServerPluginBase, IRemoteReceiver
  {

    #region Delegates

    delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
    delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);

    #endregion Delegates

    #region Variables

    RemoteHandler _remoteButtonHandler;
    
    Wiimote wm;
    Bitmap b;
    Graphics g;

    #endregion Variables

    #region Implementation

    /// <summary>
    /// Name of the IR Server plugin.
    /// </summary>
    /// <value>The name.</value>
    public override string Name         { get { return "Wii Remote"; } }
    /// <summary>
    /// IR Server plugin version.
    /// </summary>
    /// <value>The version.</value>
    public override string Version      { get { return "1.0.3.5"; } }
    /// <summary>
    /// The IR Server plugin's author.
    /// </summary>
    /// <value>The author.</value>
    public override string Author       { get { return "and-81"; } }
    /// <summary>
    /// A description of the IR Server plugin.
    /// </summary>
    /// <value>The description.</value>
    public override string Description  { get { return "Supports the Wii Remote"; } }

    /// <summary>
    /// Detect the presence of this device.  Devices that cannot be detected will always return false.
    /// </summary>
    /// <returns>
    /// true if the device is present, otherwise false.
    /// </returns>
    public override bool Detect()
    {
      return false;
    }

    /// <summary>
    /// Start the IR Server plugin.
    /// </summary>
    /// <returns>true if successful, otherwise false.</returns>
    public override bool Start()
    {
      wm = new Wiimote();

      wm.WiimoteChanged += new WiimoteChangedEventHandler(wm_WiimoteChanged);
      wm.WiimoteExtensionChanged += new WiimoteExtensionChangedEventHandler(wm_WiimoteExtensionChanged);

      b = new Bitmap(256, 192, PixelFormat.Format24bppRgb);

      g = Graphics.FromImage(b);

      wm.Connect();
      wm.SetReportType(Wiimote.InputReport.IRAccel, true);
      wm.SetLEDs(false, true, true, false);

      return true;
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
      if (wm == null)
        return;

      wm.Disconnect();
    }

    /// <summary>
    /// Callback for remote button presses.
    /// </summary>
    /// <value>The remote callback.</value>
    public RemoteHandler RemoteCallback
    {
      get { return _remoteButtonHandler; }
      set { _remoteButtonHandler = value; }
    }




    private void UpdateExtensionChanged(WiimoteExtensionChangedEventArgs args)
    {
      chkExtension.Text = args.ExtensionType.ToString();
      chkExtension.Checked = args.Inserted;

      if (args.Inserted)
        wm.SetReportType(Wiimote.InputReport.IRExtensionAccel, true);
      else
        wm.SetReportType(Wiimote.InputReport.IRAccel, true);
    }

    private void UpdateWiimoteState(WiimoteChangedEventArgs args)
    {
      WiimoteState ws = args.WiimoteState;

      clbButtons.SetItemChecked(0, ws.ButtonState.A);
      clbButtons.SetItemChecked(1, ws.ButtonState.B);
      clbButtons.SetItemChecked(2, ws.ButtonState.Minus);
      clbButtons.SetItemChecked(3, ws.ButtonState.Home);
      clbButtons.SetItemChecked(4, ws.ButtonState.Plus);
      clbButtons.SetItemChecked(5, ws.ButtonState.One);
      clbButtons.SetItemChecked(6, ws.ButtonState.Two);
      clbButtons.SetItemChecked(7, ws.ButtonState.Up);
      clbButtons.SetItemChecked(8, ws.ButtonState.Down);
      clbButtons.SetItemChecked(9, ws.ButtonState.Left);
      clbButtons.SetItemChecked(10, ws.ButtonState.Right);
      clbButtons.SetItemChecked(11, ws.NunchukState.C);
      clbButtons.SetItemChecked(12, ws.NunchukState.Z);

      lblX.Text = ws.AccelState.X.ToString();
      lblY.Text = ws.AccelState.Y.ToString();
      lblZ.Text = ws.AccelState.Z.ToString();

      if (ws.ExtensionType == ExtensionType.Nunchuk)
      {
        lblChukX.Text = ws.NunchukState.AccelState.X.ToString();
        lblChukY.Text = ws.NunchukState.AccelState.Y.ToString();
        lblChukZ.Text = ws.NunchukState.AccelState.Z.ToString();

        lblChukJoyX.Text = ws.NunchukState.X.ToString();
        lblChukJoyY.Text = ws.NunchukState.Y.ToString();
      }

      if (ws.ExtensionType == ExtensionType.ClassicController)
      {
        clbCCButtons.SetItemChecked(0, ws.ClassicControllerState.ButtonState.A);
        clbCCButtons.SetItemChecked(1, ws.ClassicControllerState.ButtonState.B);
        clbCCButtons.SetItemChecked(2, ws.ClassicControllerState.ButtonState.X);
        clbCCButtons.SetItemChecked(3, ws.ClassicControllerState.ButtonState.Y);
        clbCCButtons.SetItemChecked(4, ws.ClassicControllerState.ButtonState.Minus);
        clbCCButtons.SetItemChecked(5, ws.ClassicControllerState.ButtonState.Home);
        clbCCButtons.SetItemChecked(6, ws.ClassicControllerState.ButtonState.Plus);
        clbCCButtons.SetItemChecked(7, ws.ClassicControllerState.ButtonState.Up);
        clbCCButtons.SetItemChecked(8, ws.ClassicControllerState.ButtonState.Down);
        clbCCButtons.SetItemChecked(9, ws.ClassicControllerState.ButtonState.Left);
        clbCCButtons.SetItemChecked(10, ws.ClassicControllerState.ButtonState.Right);
        clbCCButtons.SetItemChecked(11, ws.ClassicControllerState.ButtonState.ZL);
        clbCCButtons.SetItemChecked(12, ws.ClassicControllerState.ButtonState.ZR);
        clbCCButtons.SetItemChecked(13, ws.ClassicControllerState.ButtonState.TriggerL);
        clbCCButtons.SetItemChecked(14, ws.ClassicControllerState.ButtonState.TriggerR);

        lblXL.Text = ws.ClassicControllerState.XL.ToString();
        lblYL.Text = ws.ClassicControllerState.YL.ToString();
        lblXR.Text = ws.ClassicControllerState.XR.ToString();
        lblYR.Text = ws.ClassicControllerState.YR.ToString();

        lblTriggerL.Text = ws.ClassicControllerState.TriggerL.ToString();
        lblTriggerR.Text = ws.ClassicControllerState.TriggerR.ToString();
      }

      if (ws.IRState.Found1)
      {
        lblIR1.Text = ws.IRState.X1.ToString() + ", " + ws.IRState.Y1.ToString() + ", " + ws.IRState.Size1;
        lblIR1Raw.Text = ws.IRState.RawX1.ToString() + ", " + ws.IRState.RawY1.ToString();
      }
      if (ws.IRState.Found2)
      {
        lblIR2.Text = ws.IRState.X2.ToString() + ", " + ws.IRState.Y2.ToString() + ", " + ws.IRState.Size2;
        lblIR2Raw.Text = ws.IRState.RawX2.ToString() + ", " + ws.IRState.RawY2.ToString();
      }

      chkFound1.Checked = ws.IRState.Found1;
      chkFound2.Checked = ws.IRState.Found2;

      pbBattery.Value = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery);
      float f = (((100.0f * 48.0f * (float)(ws.Battery / 48.0f))) / 192.0f);
      lblBattery.Text = f.ToString("F");

      g.Clear(Color.Black);
      if (ws.IRState.Found1)
        g.DrawEllipse(new Pen(Color.Red), (int)(ws.IRState.RawX1 / 4), (int)(ws.IRState.RawY1 / 4), ws.IRState.Size1 + 1, ws.IRState.Size1 + 1);
      if (ws.IRState.Found2)
        g.DrawEllipse(new Pen(Color.Blue), (int)(ws.IRState.RawX2 / 4), (int)(ws.IRState.RawY2 / 4), ws.IRState.Size2 + 1, ws.IRState.Size2 + 1);
      if (ws.IRState.Found1 && ws.IRState.Found2)
        g.DrawEllipse(new Pen(Color.Green), (int)(ws.IRState.RawMidX / 4), (int)(ws.IRState.RawMidY / 4), 2, 2);
      pbIR.Image = b;
    }

    private void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
    {
      BeginInvoke(new UpdateWiimoteStateDelegate(UpdateWiimoteState), args);
    }

    private void wm_WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs args)
    {
      BeginInvoke(new UpdateExtensionChangedDelegate(UpdateExtensionChanged), args);
    }

    

    #endregion Implementation

  }

}
