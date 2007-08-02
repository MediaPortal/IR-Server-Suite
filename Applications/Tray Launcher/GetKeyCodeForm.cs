using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using NamedPipes;
using IrssUtils;

namespace TrayLauncher
{

  public partial class GetKeyCodeForm : Form
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

      Tray.HandleMessage -= new Common.MessageHandler(MessageReceiver);

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

      Tray.HandleMessage += new Common.MessageHandler(MessageReceiver);

      timer.Start();
    }
    
    void MessageReceiver(string message)
    {
      PipeMessage received = PipeMessage.FromString(message);

      if (received.Name == "Remote Event")
      {
        _keyCode = Encoding.ASCII.GetString(received.Data);

        this.Invoke(_keyCodeSet);
      }
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      KeyCodeSet();
    }
    
  }

}
