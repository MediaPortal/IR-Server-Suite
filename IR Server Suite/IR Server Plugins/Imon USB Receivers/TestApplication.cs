using System;
using System.Collections.Generic;
using System.Text;

namespace InputService.Plugin
{
    public partial class iMonUSBReceivers
    {
        #region Code to impliment the test application version of the plugin

        // #define TEST_APPLICATION in the project properties when creating the console test app ...
#if TEST_APPLICATION

        #region Test Application - Enumeration for KeyCode Translation
        /// <summary>
        /// Remote Key Mapping (for displaying the remote button names).
        /// </summary>
        internal enum iMonRemoteKeyMapping
        {
            // iMon PAD mappings
            IMON_PAD_BUTTON_APPEXIT = 1002,
            IMON_PAD_BUTTON_POWER = 1016,
            IMON_PAD_BUTTON_RECORD = 1064,
            IMON_PAD_BUTTON_PLAY = 1128,
            IMON_PAD_BUTTON_EJECT = 1114,
            IMON_PAD_BUTTON_REWIND = 1130,
            IMON_PAD_BUTTON_PAUSE = 1144,
            IMON_PAD_BUTTON_FORWARD = 1192,
            IMON_PAD_BUTTON_REPLAY = 1208,
            IMON_PAD_BUTTON_STOP = 1220,
            IMON_PAD_BUTTON_SKIP = 1066,

            IMON_PAD_BUTTON_BACKSPACE = 1032,
            IMON_PAD_BUTTON_MOUSE_KEYBD = 1080,
            IMON_PAD_BUTTON_SELECT_SPACE = 1148,
            IMON_PAD_BUTTON_WINKEY = 1194,
            IMON_PAD_BUTTON_MENUKEY = 1060,

            IMON_PAD_BUTTON_LEFTCLICK = 1226,
            IMON_PAD_BUTTON_RIGHTCLICK = 1228,

            IMON_PAD_BUTTON_ENTER = 1034,
            IMON_PAD_BUTTON_ESC = 1252,
            IMON_PAD_BUTTON_EJECT2 = 1086,
            IMON_PAD_BUTTON_APPLAUNCH = 1124,
            IMON_PAD_BUTTON_GREENBUTTON = 1178,
            IMON_PAD_BUTTON_TASKSWITCH = 1150,

            IMON_PAD_BUTTON_MUTE = 1218,
            IMON_PAD_BUTTON_VOLUME_UP = 1038,
            IMON_PAD_BUTTON_CHANNEL_UP = 1022,
            IMON_PAD_BUTTON_TIMER = 1198,
            IMON_PAD_BUTTON_VOLUME_DOWN = 1042,
            IMON_PAD_BUTTON_CHANNEL_DOWN = 1014,

            IMON_PAD_BUTTON_NUMPAD_1 = 1058,
            IMON_PAD_BUTTON_NUMPAD_2 = 1242,
            IMON_PAD_BUTTON_NUMPAD_3 = 1050,
            IMON_PAD_BUTTON_NUMPAD_4 = 1138,
            IMON_PAD_BUTTON_NUMPAD_5 = 1090,
            IMON_PAD_BUTTON_NUMPAD_6 = 1170,
            IMON_PAD_BUTTON_NUMPAD_7 = 1214,
            IMON_PAD_BUTTON_NUMPAD_8 = 1136,
            IMON_PAD_BUTTON_NUMPAD_9 = 1160,
            IMON_PAD_BUTTON_NUMPAD_STAR = 1056,
            IMON_PAD_BUTTON_NUMPAD_0 = 1234,
            IMON_PAD_BUTTON_NUMPAD_HASH = 1096,

            IMON_PAD_BUTTON_MY_MOVIE = 1200,
            IMON_PAD_BUTTON_MY_MUSIC = 1082,
            IMON_PAD_BUTTON_MY_PHOTO = 1224,
            IMON_PAD_BUTTON_MY_TV = 1040,
            IMON_PAD_BUTTON_BOOKMARK = 1008,
            IMON_PAD_BUTTON_THUMBNAIL = 1188,
            IMON_PAD_BUTTON_ASPECT_RATIO = 1106,
            IMON_PAD_BUTTON_FULLSCREEN = 1166,
            IMON_PAD_BUTTON_MY_DVD = 1102,
            IMON_PAD_BUTTON_MENU = 1230,
            IMON_PAD_BUTTON_CAPTION = 1074,
            IMON_PAD_BUTTON_LANGUAGE = 1202,


            IMON_PAD_BUTTON_RIGHT = 1244,
            IMON_PAD_BUTTON_LEFT = 1246,
            IMON_PAD_BUTTON_DOWN = 1248,
            IMON_PAD_BUTTON_UP = 1250,

            IMON_MCE_BUTTON_POWER_TV = 2101,
            IMON_MCE_BUTTON_RECORD = 2023,
            IMON_MCE_BUTTON_STOP = 2025,
            IMON_MCE_BUTTON_PAUSE = 2024,
            IMON_MCE_BUTTON_REWIND = 2021,
            IMON_MCE_BUTTON_PLAY = 2022,
            IMON_MCE_BUTTON_FORWARD = 2020,
            IMON_MCE_BUTTON_REPLAY = 2027,
            IMON_MCE_BUTTON_SKIP = 2026,
            IMON_MCE_BUTTON_BACK = 2035,
            IMON_MCE_BUTTON_UP = 2030,
            IMON_MCE_BUTTON_DOWN = 2031,
            IMON_MCE_BUTTON_LEFT = 2032,
            IMON_MCE_BUTTON_RIGHT = 2033,
            IMON_MCE_BUTTON_OK = 2034,
            IMON_MCE_BUTTON_INFO = 2015,
            IMON_MCE_BUTTON_VOLUME_UP = 2016,
            IMON_MCE_BUTTON_VOLUME_DOWN = 2017,
            IMON_MCE_BUTTON_START = 2013,
            IMON_MCE_BUTTON_CHANNEL_UP = 2018,
            IMON_MCE_BUTTON_CHANNEL_DOWN = 2019,
            IMON_MCE_BUTTON_MUTE = 2014,
            IMON_MCE_BUTTON_RECORDED_TV = 2072,
            IMON_MCE_BUTTON_GUIDE = 2038,
            IMON_MCE_BUTTON_LIVE_TV = 2037,
            IMON_MCE_BUTTON_DVD_MENU = 2036,
            IMON_MCE_BUTTON_NUMPAD_1 = 2001,
            IMON_MCE_BUTTON_NUMPAD_2 = 2002,
            IMON_MCE_BUTTON_NUMPAD_3 = 2003,
            IMON_MCE_BUTTON_NUMPAD_4 = 2004,
            IMON_MCE_BUTTON_NUMPAD_5 = 2005,
            IMON_MCE_BUTTON_NUMPAD_6 = 2006,
            IMON_MCE_BUTTON_NUMPAD_7 = 2007,
            IMON_MCE_BUTTON_NUMPAD_8 = 2008,
            IMON_MCE_BUTTON_NUMPAD_9 = 2009,
            IMON_MCE_BUTTON_NUMPAD_0 = 2000,
            IMON_MCE_BUTTON_NUMPAD_STAR = 2029,
            IMON_MCE_BUTTON_NUMPAD_HASH = 2028,
            IMON_MCE_BUTTON_CLEAR = 2010,
            IMON_MCE_BUTTON_ENTER = 2011,
            IMON_MCE_BUTTON_TELETEXT = 2090,
            IMON_MCE_BUTTON_RED = 2091,
            IMON_MCE_BUTTON_GREEN = 2092,
            IMON_MCE_BUTTON_YELLOW = 2093,
            IMON_MCE_BUTTON_BLUE = 2094,
            IMON_MCE_BUTTON_MY_TV = 2070,
            IMON_MCE_BUTTON_MY_MUSIC = 2071,
            IMON_MCE_BUTTON_MY_PICTURES = 2073,
            IMON_MCE_BUTTON_MY_VIDEOS = 2074,
            IMON_MCE_BUTTON_MY_RADIO = 2080,
            IMON_MCE_BUTTON_MESSENGER = 2105,
            IMON_MCE_BUTTON_ASPECT_RATIO = 2012,
            IMON_MCE_BUTTON_PRINT = 2078,

            IMON_PANEL_BUTTON = 3000,

            IMON_PANEL_BUTTON_VOLUME_KNOB = 3001,
            IMON_PANEL_BUTTON_MCE = 3015,
            IMON_PANEL_BUTTON_APPEXIT = 3043,
            IMON_PANEL_BUTTON_BACK = 3023,
            IMON_PANEL_BUTTON_UP = 3018,
            IMON_PANEL_BUTTON_ENTER = 3022,
            IMON_PANEL_BUTTON_START = 3044,
            IMON_PANEL_BUTTON_MENU = 3045,
            IMON_PANEL_BUTTON_LEFT = 3020,
            IMON_PANEL_BUTTON_DOWN = 3019,
            IMON_PANEL_BUTTON_RIGHT = 3021,

            IMON_VOLUME_UP = 4001,
            IMON_VOLUME_DOWN = 4002,
        }
        #endregion

        static void xRemote_HID(string deviceName, string code)
        {
            DebugWriteLine("iMon HID Remote: {0}     (button = {1})\n", code, Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), code)));
        }

        static void xRemote_DOS(string deviceName, string code)
        {
            DebugWriteLine("iMon DOS Remote: {0}     (button = {1})\n", code, Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), code)));
        }

        static void xKeyboard_HID(string deviceName, int button, bool up)
        {
            DebugWriteLine("iMon HID Keyboard: {0}, {1}     (key = {2})\n", button, (up ? "Released" : "Pressed"), Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), button.ToString())));
        }

        static void xKeyboard_DOS(string deviceName, int button, bool up)
        {
            DebugWriteLine("iMon DOS Keyboard: {0}, {1}     (key = {2})\n", button, (up ? "Released" : "Pressed"), Enum.GetName(typeof(iMonRemoteKeyMapping), Enum.Parse(typeof(iMonRemoteKeyMapping), button.ToString())));
        }

        static void xMouse_HID(string deviceName, int x, int y, int buttons)
        {
            DebugWriteLine("iMon HID Mouse: ({0}, {1}) - {2}\n", x, y, buttons);
        }

        static void xMouse_DOS(string deviceName, int x, int y, int buttons)
        {
            DebugWriteLine("iMon DOS Mouse: ({0}, {1}) - {2}\n", x, y, buttons);
        }

        [STAThread]
        static void Main()
        {
            DebugOpen("iMonTestApp.log");
            DebugWriteLine("Main()");

            DeviceType DevType;

            iMonUSBReceivers device = new iMonUSBReceiver();

            try
            {
                device.Configure(null);

                DevType = device.DeviceDriverMode;

                DebugWriteLine("Main(): Detected device type = {0}", Enum.GetName(typeof(DeviceType), DevType));

                if (DevType == DeviceType.DOS)
                {
                    DebugWriteLine("Found an iMon DOS Device\n");
                    device.RemoteCallback += new RemoteHandler(xRemote_DOS);
                    device.KeyboardCallback += new KeyboardHandler(xKeyboard_DOS);
                    device.MouseCallback += new MouseHandler(xMouse_DOS);
                }
                else if (DevType == DeviceType.HID)
                {
                    DebugWriteLine("Found an iMon HID Device\n");
                    device.RemoteCallback += new RemoteHandler(xRemote_HID);
                    device.KeyboardCallback += new KeyboardHandler(xKeyboard_HID);
                    device.MouseCallback += new MouseHandler(xMouse_HID);
                }

                if ((DevType == DeviceType.DOS) | (DevType == DeviceType.HID))
                {
                    device.Start();
                    Application.Run();
                    device.Stop();
                }
                else
                {
                    DebugWriteLine("Main(): NO SUPPORTED iMon DEVICE FOUND\n");
                    throw new Exception("NO SUPPORTED iMon DEVICE FOUND");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                device = null;
            }
            DebugWriteLine("Main(): completed");
            Console.ReadKey();
        }
        
#endif
        #endregion
    }
}
        