#region Copyright (C) 2005-2012 Team MediaPortal

// Copyright (C) 2005-2012 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Windows.Forms;

namespace IRServer.Plugin
{

  #region Delegates

  /// <summary>
  /// Windows message processing delegate.
  /// </summary>
  /// <param name="m">Windows message.</param>
  internal delegate void ProcessMessage(ref Message m);

  #endregion Delegates

  /// <summary>
  /// Use this class to receive windows messages.
  /// </summary>
  internal class ReceiverWindow : NativeWindow
  {
    #region Variables

    private ProcessMessage _processMessage;

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
      createParams.Style = unchecked((int) 0x80000000);

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
      if (_processMessage != null)
        _processMessage(ref m);

      base.WndProc(ref m);
    }

    #endregion Implementation
  }
}