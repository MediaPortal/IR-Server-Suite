using System;
using System.Windows.Forms;

namespace InputService.Plugin
{

  #region Delegates

  /// <summary>
  /// Windows message processing delegate.
  /// </summary>
  /// <param name="m">Windows message.</param>
  delegate void ProcessMessage(ref Message m);

  #endregion Delegates

  /// <summary>
  /// Use this class to receive windows messages.
  /// </summary>
  class ReceiverWindow : NativeWindow
  {

    #region Variables

    ProcessMessage _processMessage;

    #endregion Variables

    #region Properties

    /// <summary>
    /// Gets or Sets the Windows Message processing delegate.
    /// </summary>
    public ProcessMessage ProcMsg
    {
      get { return _processMessage; }
      set { _processMessage = value; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Create a Windows Message receiving window object.
    /// </summary>
    public ReceiverWindow()
    {
      CreateParams createParams = new CreateParams();
      createParams.ExStyle = 0x80;
      createParams.Style = unchecked((int)0x80000000);
      
      base.CreateHandle(createParams);
    }

    #endregion Constructor

    #region Implementation

    protected override void WndProc(ref Message m)
    {
      if (_processMessage != null)
        _processMessage(ref m);

      base.WndProc(ref m);
    }

    #endregion Implementation

  }

}
