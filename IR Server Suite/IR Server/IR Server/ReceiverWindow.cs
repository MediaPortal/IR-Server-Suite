using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IRServer
{
    internal delegate void ProcessMessage(ref Message m);

    class ReceiverWindow : NativeWindow
    {        

        #region Constructor/Destructor

        /// <summary>
        /// Create a Windows Message receiving window object.
        /// </summary>
        /// <param name="windowTitle">Window title for receiver object.</param>
        public ReceiverWindow(string windowTitle)
        {
            CreateParams createParams = new CreateParams();
            createParams.Caption = windowTitle;
            createParams.ExStyle = 0x80;
            createParams.Style = unchecked((int)0x80000000);

            CreateHandle(createParams);
        }

        ~ReceiverWindow()
        {
            if (Handle != IntPtr.Zero)
                DestroyHandle();
        }

        #endregion Constructor/Destructor

        #region Implementation

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 16)
                Application.Exit();

            base.WndProc(ref m);
        }

        #endregion Implementation
    }
}
