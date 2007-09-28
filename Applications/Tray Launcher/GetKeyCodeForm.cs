using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using IrssComms;
using IrssUtils;

namespace TrayLauncher
{

  partial class GetKeyCodeForm : Form
  {

    #region Variables

    string _keyCode = String.Empty;

    #endregion Variables

    #region Properties

    public string KeyCode
    {
      get { return _keyCode; }
    }

    #endregion Properties

    delegate void DelegateKeyCodeSet();
    DelegateKeyCodeSet _keyCodeSet = null;
    void KeyCodeSet()
    {
      timer.Stop();

      Tray.HandleMessage -= new ClientMessageSink(MessageReceiver);

      this.Close();
    }

    public GetKeyCodeForm()
    {
      InitializeComponent();
    }

    private void GetKeyCodeForm_Load(object sender, EventArgs e)
    {
      labelStatus.Text = "Press the remote button to map";

      _keyCodeSet = new DelegateKeyCodeSet(KeyCodeSet);

      Tray.HandleMessage += new ClientMessageSink(MessageReceiver);

      timer.Start();
    }
    
    void MessageReceiver(IrssMessage received)
    {
      if (received.Type == MessageType.RemoteEvent)
      {
        _keyCode = received.GetDataAsString();

        this.Invoke(_keyCodeSet);
      }
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      KeyCodeSet();
    }
    
  }

}
