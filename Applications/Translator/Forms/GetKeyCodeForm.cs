using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using NamedPipes;
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

      Program.HandleMessage += new Common.MessageHandler(MessageReceiver);

      timer.Start();
    }

    void MessageReceiver(string message)
    {
      PipeMessage received = PipeMessage.FromString(message);

      if (received.Name == "Remote Button")
      {
        _keyCode = Encoding.ASCII.GetString(received.Data);

        this.Invoke(_keyCodeSet);
      }
    }

    void KeyCodeSet()
    {
      timer.Stop();

      Program.HandleMessage -= new Common.MessageHandler(MessageReceiver);

      this.Close();
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      KeyCodeSet();
    }

  }

}
