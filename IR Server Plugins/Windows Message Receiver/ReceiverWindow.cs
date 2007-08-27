using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindowsMessageReceiver
{

  public delegate void ProcessMessage(ref Message m);

  public class ReceiverWindow : NativeWindow
  {

    ProcessMessage _processMessage = null;

    public ProcessMessage ProcMsg
    {
      get { return _processMessage; }
      set { _processMessage = value; }
    }

    public ReceiverWindow(string windowTitle)
    {
      CreateParams createParams = new CreateParams();
      createParams.Caption = windowTitle;
      createParams.ExStyle = 0x80;
      createParams.Style = unchecked((int)0x80000000);
      
      CreateHandle(createParams);
    }

    protected override void WndProc(ref Message m)
    {
      if (_processMessage != null)
        _processMessage(ref m);

      base.WndProc(ref m);
    }

  }

}
