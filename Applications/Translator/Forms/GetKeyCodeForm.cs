using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using IrssComms;
using IrssUtils;

namespace Translator
{

  public partial class GetKeyCodeForm : Form
  {

    #region Delegates

    delegate void DelegateKeyCodeSet();

    #endregion Delegates

    #region Variables

    string _keyCode = String.Empty;

    DelegateKeyCodeSet _keyCodeSet;

    #endregion Variables

    #region Properties

    public string KeyCode
    {
      get { return _keyCode; }
    }

    #endregion Properties

    #region Constructor

    public GetKeyCodeForm()
    {
      InitializeComponent();
    }

    #endregion Constructor

    private void GetKeyCodeForm_Load(object sender, EventArgs e)
    {
      labelStatus.Text = "Press the remote button to map";

      _keyCodeSet = new DelegateKeyCodeSet(KeyCodeSet);

      Program.HandleMessage += new ClientMessageSink(MessageReceiver);

      timer.Start();
    }

    void MessageReceiver(IrssMessage received)
    {
      if (received.Type == MessageType.RemoteEvent)
      {
        _keyCode = received.DataAsString;

        this.Invoke(_keyCodeSet);
      }
    }

    void KeyCodeSet()
    {
      timer.Stop();

      Program.HandleMessage -= new ClientMessageSink(MessageReceiver);

      this.Close();
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      KeyCodeSet();
    }

  }

}
