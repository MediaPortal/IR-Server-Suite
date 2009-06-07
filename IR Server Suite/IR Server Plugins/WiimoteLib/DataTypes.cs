//////////////////////////////////////////////////////////////////////////////////
//	DataTypes.cs
//	Managed Wiimote Library
//	Written by Brian Peek (http://www.brianpeek.com/)
//	for MSDN's Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//	Visit http://blogs.msdn.com/coding4fun/archive/2007/03/14/1879033.aspx
//	for more information
//////////////////////////////////////////////////////////////////////////////////

using System;

// if we're building the MSRS version, we need to bring in the MSRS Attributes
// if we're not doing the MSRS build then define some fake attribute classes for DataMember/DataContract
#if MSRS
	using Microsoft.Dss.Core.Attributes;
#else
internal sealed class DataContract : Attribute
{
}

internal sealed class DataMember : Attribute
{
}
#endif

namespace WiimoteLib
{
#if MSRS
    [DataContract]
    public struct RumbleRequest
    {
        [DataMember]
        public bool Rumble;
    }
#endif

  /// <summary>
  /// Current overall state of the Wiimote and all attachments
  /// </summary>
  [DataContract]
  public class WiimoteState
  {
    /// <summary>
    /// Current calibration information
    /// </summary>
    [DataMember] public AccelCalibrationInfo AccelCalibrationInfo;

    /// <summary>
    /// Current state of accelerometers
    /// </summary>
    [DataMember] public AccelState AccelState;

    /// <summary>
    /// Current battery level
    /// </summary>
    [DataMember] public byte Battery;

    /// <summary>
    /// Current state of buttons
    /// </summary>
    [DataMember] public ButtonState ButtonState;

    /// <summary>
    /// Current state of Classic Controller extension
    /// </summary>
    [DataMember] public ClassicControllerState ClassicControllerState;

    /// <summary>
    /// Is an extension controller inserted?
    /// </summary>
    [DataMember] public bool Extension;

    /// <summary>
    /// Extension controller currently inserted, if any
    /// </summary>
    [DataMember] public ExtensionType ExtensionType;

    /// <summary>
    /// Current state of IR sensors
    /// </summary>
    [DataMember] public IRState IRState;

    /// <summary>
    /// Current state of LEDs
    /// </summary>
    [DataMember] public LEDState LEDState;

    /// <summary>
    /// Current state of Nunchuk extension
    /// </summary>
    [DataMember] public NunchukState NunchukState;

    /// <summary>
    /// Current state of rumble
    /// </summary>
    [DataMember] public bool Rumble;
  }

  /// <summary>
  /// Current state of LEDs
  /// </summary>
  [DataContract]
  public struct LEDState
  {
    /// <summary>
    /// LED on the Wiimote
    /// </summary>
    [DataMember] public bool LED1, LED2, LED3, LED4;
  }

  /// <summary>
  /// Calibration information stored on the Nunchuk
  /// </summary>
  [DataContract]
  public struct NunchukCalibrationInfo
  {
    /// <summary>
    /// Accelerometer calibration data
    /// </summary>
    public AccelCalibrationInfo AccelCalibration;

    /// <summary>
    /// Joystick X-axis calibration
    /// </summary>
    [DataMember] public byte MaxX;

    /// <summary>
    /// Joystick Y-axis calibration
    /// </summary>
    [DataMember] public byte MaxY;

    /// <summary>
    /// Joystick X-axis calibration
    /// </summary>
    [DataMember] public byte MidX;

    /// <summary>
    /// Joystick Y-axis calibration
    /// </summary>
    [DataMember] public byte MidY;

    /// <summary>
    /// Joystick X-axis calibration
    /// </summary>
    [DataMember] public byte MinX;

    /// <summary>
    /// Joystick Y-axis calibration
    /// </summary>
    [DataMember] public byte MinY;
  }

  /// <summary>
  /// Calibration information stored on the Classic Controller
  /// </summary>
  [DataContract]
  public struct ClassicControllerCalibrationInfo
  {
    /// <summary>
    /// Left analog trigger
    /// </summary>
    [DataMember] public byte MaxTriggerL;

    /// <summary>
    /// Right analog trigger
    /// </summary>
    [DataMember] public byte MaxTriggerR;

    /// <summary>
    /// Left joystick X-axis 
    /// </summary>
    [DataMember] public byte MaxXL;

    /// <summary>
    /// Right joystick X-axis
    /// </summary>
    [DataMember] public byte MaxXR;

    /// <summary>
    /// Left joystick Y-axis
    /// </summary>
    [DataMember] public byte MaxYL;

    /// <summary>
    /// Right joystick Y-axis
    /// </summary>
    [DataMember] public byte MaxYR;

    /// <summary>
    /// Left joystick X-axis 
    /// </summary>
    [DataMember] public byte MidXL;

    /// <summary>
    /// Right joystick X-axis
    /// </summary>
    [DataMember] public byte MidXR;

    /// <summary>
    /// Left joystick Y-axis
    /// </summary>
    [DataMember] public byte MidYL;

    /// <summary>
    /// Right joystick Y-axis
    /// </summary>
    [DataMember] public byte MidYR;

    /// <summary>
    /// Left analog trigger
    /// </summary>
    [DataMember] public byte MinTriggerL;

    /// <summary>
    /// Right analog trigger
    /// </summary>
    [DataMember] public byte MinTriggerR;

    /// <summary>
    /// Left joystick X-axis 
    /// </summary>
    [DataMember] public byte MinXL;

    /// <summary>
    /// Right joystick X-axis
    /// </summary>
    [DataMember] public byte MinXR;

    /// <summary>
    /// Left joystick Y-axis
    /// </summary>
    [DataMember] public byte MinYL;

    /// <summary>
    /// Right joystick Y-axis
    /// </summary>
    [DataMember] public byte MinYR;
  }

  /// <summary>
  /// Current state of the Nunchuk extension
  /// </summary>
  [DataContract]
  public struct NunchukState
  {
    /// <summary>
    /// State of accelerometers
    /// </summary>
    [DataMember] public AccelState AccelState;

    /// <summary>
    /// Digital button on Nunchuk extension
    /// </summary>
    [DataMember] public bool C;

    /// <summary>
    /// Calibration data for Nunchuk extension
    /// </summary>
    [DataMember] public NunchukCalibrationInfo CalibrationInfo;

    /// <summary>
    /// Raw joystick position before normalization.  Values range between 0 and 255.
    /// </summary>
    [DataMember] public byte RawX, RawY;

    /// <summary>
    /// Normalized joystick position.  Values range between -0.5 and 0.5
    /// </summary>
    [DataMember] public float X, Y;

    /// <summary>
    /// Digital button on Nunchuk extension
    /// </summary>
    [DataMember] public bool Z;
  }

  /// <summary>
  /// Curernt button state of the Classic Controller
  /// </summary>
  [DataContract]
  public struct ClassicControllerButtonState
  {
    /// <summary>
    /// Digital button on the Classic Controller extension
    /// </summary>
    [DataMember] public bool A, B;

    /// <summary>
    /// Digital button on the Classic Controller extension
    /// </summary>
    [DataMember] public bool Down;

    /// <summary>
    /// Digital button on the Classic Controller extension
    /// </summary>
    [DataMember] public bool Home;

    /// <summary>
    /// Digital button on the Classic Controller extension
    /// </summary>
    [DataMember] public bool Left;

    /// <summary>
    /// Digital button on the Classic Controller extension
    /// </summary>
    [DataMember] public bool Minus;

    /// <summary>
    /// Digital button on the Classic Controller extension
    /// </summary>
    [DataMember] public bool Plus;

    /// <summary>
    /// Digital button on the Classic Controller extension
    /// </summary>
    [DataMember] public bool Right;

    /// <summary>
    /// Analog trigger - false if released, true for any pressure applied
    /// </summary>
    [DataMember] public bool TriggerL, TriggerR;

    /// <summary>
    /// Digital button on the Classic Controller extension
    /// </summary>
    [DataMember] public bool Up;

    /// <summary>
    /// Digital button on the Classic Controller extension
    /// </summary>
    [DataMember] public bool X, Y, ZL, ZR;
  }

  /// <summary>
  /// Current state of the Classic Controller
  /// </summary>
  [DataContract]
  public struct ClassicControllerState
  {
    /// <summary>
    /// Current button state
    /// </summary>
    [DataMember] public ClassicControllerButtonState ButtonState;

    /// <summary>
    /// Calibration data for Classic Controller extension
    /// </summary>
    [DataMember] public ClassicControllerCalibrationInfo CalibrationInfo;

    /// <summary>
    /// Raw value of analog trigger.  Values range between 0 - 255.
    /// </summary>
    [DataMember] public byte RawTriggerL, RawTriggerR;

    /// <summary>
    /// Raw value of left joystick.  Values range between 0 - 255.
    /// </summary>
    [DataMember] public byte RawXL;

    /// <summary>
    /// Raw value of right joystick.  Values range between 0 - 255.
    /// </summary>
    [DataMember] public byte RawXR;

    /// <summary>
    /// Raw value of left joystick.  Values range between 0 - 255.
    /// </summary>
    [DataMember] public byte RawYL;

    /// <summary>
    /// Raw value of right joystick.  Values range between 0 - 255.
    /// </summary>
    [DataMember] public byte RawYR;

    /// <summary>
    /// Normalized value of analog trigger.  Values range between 0.0 - 1.0
    /// </summary>
    [DataMember] public float TriggerL, TriggerR;

    /// <summary>
    /// Normalized value of left joystick.  Values range between -0.5 - 0.5
    /// </summary>
    [DataMember] public float XL;

    /// <summary>
    /// Normalized value of right joystick.  Values range between -0.5 - 0.5
    /// </summary>
    [DataMember] public float XR;

    /// <summary>
    /// Normalized value of left joystick.  Values range between -0.5 - 0.5
    /// </summary>
    [DataMember] public float YL;

    /// <summary>
    /// Normalized value of right joystick.  Values range between -0.5 - 0.5
    /// </summary>
    [DataMember] public float YR;
  }

  /// <summary>
  /// Current state of the IR camera
  /// </summary>
  [DataContract]
  public struct IRState
  {
    /// <summary>
    /// IR sensor seen
    /// </summary>
    [DataMember] public bool Found1, Found2;

    /// <summary>
    /// Normalized midpoint of IR sensors.  Values range between 0.0 - 1.0
    /// </summary>
    [DataMember] public float MidX, MidY;

    /// <summary>
    /// Current mode of IR sensor data
    /// </summary>
    [DataMember] public IRMode Mode;

    /// <summary>
    /// Raw midpoint of IR sensors.  Values range between 0 - 1023, 0 - 767
    /// </summary>
    [DataMember] public int RawMidX, RawMidY;

    /// <summary>
    /// Raw value of X-axis on individual sensor.  Values range between 0 - 1023
    /// </summary>
    [DataMember] public int RawX1, RawX2;

    /// <summary>
    /// Raw value of Y-axis on individual sensor.  Values range between 0 - 767
    /// </summary>
    [DataMember] public int RawY1, RawY2;

    /// <summary>
    /// Size of IR Sensor.  Values range from 0 - 15
    /// </summary>
    [DataMember] public int Size1, Size2;

    /// <summary>
    /// Normalized value of X-axis on individual sensor.  Values range between 0.0 - 1.0
    /// </summary>
    [DataMember] public float X1, X2;

    /// <summary>
    /// Normalized value of Y-axis on individual sensor.  Values range between 0.0 - 1.0
    /// </summary>
    [DataMember] public float Y1, Y2;
  }

  /// <summary>
  /// Current state of the accelerometers
  /// </summary>
  [DataContract]
  public struct AccelState
  {
    /// <summary>
    /// Raw accelerometer data.
    /// <remarks>Values range between 0 - 255</remarks>
    /// </summary>
    [DataMember] public byte RawX, RawY, RawZ;

    /// <summary>
    /// Normalized acceerometer data.  Values range between 0 - ?
    /// </summary>
    [DataMember] public float X, Y, Z;
  }

  /// <summary>
  /// Accelerometer calibration information
  /// </summary>
  [DataContract]
  public struct AccelCalibrationInfo
  {
    /// <summary>
    /// Zero point of accelerometer
    /// </summary>
    [DataMember] public byte X0;

    /// <summary>
    /// Gravity at rest of accelerometer
    /// </summary>
    [DataMember] public byte XG;

    /// <summary>
    /// Zero point of accelerometer
    /// </summary>
    [DataMember] public byte Y0;

    /// <summary>
    /// Gravity at rest of accelerometer
    /// </summary>
    [DataMember] public byte YG;

    /// <summary>
    /// Zero point of accelerometer
    /// </summary>
    [DataMember] public byte Z0;

    /// <summary>
    /// Gravity at rest of accelerometer
    /// </summary>
    [DataMember] public byte ZG;
  }

  /// <summary>
  /// Current button state
  /// </summary>
  [DataContract]
  public struct ButtonState
  {
    /// <summary>
    /// Digital button on the Wiimote
    /// </summary>
    [DataMember] public bool A, B;

    /// <summary>
    /// Digital button on the Wiimote
    /// </summary>
    [DataMember] public bool Down;

    /// <summary>
    /// Digital button on the Wiimote
    /// </summary>
    [DataMember] public bool Home;

    /// <summary>
    /// Digital button on the Wiimote
    /// </summary>
    [DataMember] public bool Left;

    /// <summary>
    /// Digital button on the Wiimote
    /// </summary>
    [DataMember] public bool Minus, One;

    /// <summary>
    /// Digital button on the Wiimote
    /// </summary>
    [DataMember] public bool Plus;

    /// <summary>
    /// Digital button on the Wiimote
    /// </summary>
    [DataMember] public bool Right;

    /// <summary>
    /// Digital button on the Wiimote
    /// </summary>
    [DataMember] public bool Two, Up;
  }

  /// <summary>
  /// The extension plugged into the Wiimote
  /// </summary>
  [DataContract]
  public enum ExtensionType : byte
  {
    /// <summary>
    /// No extension
    /// </summary>
    None = 0x00,
    /// <summary>
    /// Nunchuk extension
    /// </summary>
    Nunchuk = 0xfe,
    /// <summary>
    /// Classic Controller extension
    /// </summary>
    ClassicController = 0xfd
  } ;

  /// <summary>
  /// The mode of data reported for the IR sensor
  /// </summary>
  [DataContract]
  public enum IRMode : byte
  {
    /// <summary>
    /// IR sensor off
    /// </summary>
    Off = 0x00,
    /// <summary>
    /// Basic mode
    /// </summary>
    Basic = 0x01, // 10 bytes
    /// <summary>
    /// Extended mode
    /// </summary>
    Extended = 0x03, // 12 bytes
    /// <summary>
    /// Full mode (unsupported)
    /// </summary>
    Full = 0x05, // 16 bytes * 2 (format unknown)
  } ;
}