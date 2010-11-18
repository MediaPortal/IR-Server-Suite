using System;
using System.Management;

namespace IrssUtils
{
  public class HardwareMonitor
  {
    public delegate void HardwareMonitorEvent();

    #region Enums

    private enum HardwareEventType
    {
      ConfigurationChanged = 1,
      DeviceConnected = 2,
      DeviceDisconnected = 3,
      DeviceDocking = 4
    }

    #endregion

    #region Events

    public event HardwareMonitorEvent DeviceConnected;
    public event HardwareMonitorEvent DeviceDisconnected;

    #endregion

    #region Variables

    private ManagementEventWatcher watcher;
    private HardwareEventType lastEvent;
    private DateTime lastHardwareEventTime;

    #endregion

    #region Constructor

    public HardwareMonitor()
    {
      lastEvent = HardwareEventType.ConfigurationChanged;
      lastHardwareEventTime = DateTime.MinValue;

      try
      {
        WqlEventQuery query = new WqlEventQuery(
          "SELECT * FROM Win32_DeviceChangeEvent");

        watcher = new ManagementEventWatcher(query);

        watcher.EventArrived +=
          new EventArrivedEventHandler(
            HandleEvent);
      }
      catch (ManagementException err)
      {
        IrssLog.Error("An error occurred while trying to receive an event: " + err.Message);
      }
    }

    #endregion

    #region Implementation

    public void Start()
    {
      try
      {
        watcher.Start();
        IrssLog.Info("Started Hardware Monitoring. Waiting for an event...");
      }
      catch (ManagementException err)
      {
        IrssLog.Error("An error occurred while trying to receive an event: " + err.Message);
      }
    }

    public void Stop()
    {
      try
      {
        watcher.Stop();
        IrssLog.Info("Stopped Hardware Monitoring.");
      }
      catch (ManagementException err)
      {
        IrssLog.Error("An error occurred while trying to stop hardware monitoring: " + err.Message);
      }
    }

    private void HandleEvent(object sender,
        EventArrivedEventArgs e)
    {
      HardwareEventType eventType = (HardwareEventType)int.Parse(e.NewEvent.Properties["EventType"].Value.ToString());
      IrssLog.Debug("HandleEvent: EvenType = {0}", eventType);

      // handle only (dis)connects
      if (eventType == HardwareEventType.ConfigurationChanged
          || eventType == HardwareEventType.DeviceDocking)
        return;

      // if eventtype is the same and timespan is < 1sec do nothing
      TimeSpan eventTimeout = DateTime.Now.Subtract(lastHardwareEventTime);
      if (lastEvent == eventType && eventTimeout.Milliseconds < 1000) return;

      // go on with event handling
      lastEvent = eventType;
      lastHardwareEventTime = DateTime.Now;

      if (eventType == HardwareEventType.DeviceConnected)
      {
        IrssLog.Info("HardwareMonitor: Device connected");
        if (DeviceConnected != null)
          DeviceConnected();
      }
      else
      {
        IrssLog.Info("HardwareMonitor: Device disconnected");
        if (DeviceDisconnected != null)
          DeviceDisconnected();
      }
    }

    #endregion
  }
}