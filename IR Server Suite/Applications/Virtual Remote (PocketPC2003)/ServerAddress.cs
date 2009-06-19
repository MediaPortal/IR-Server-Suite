#region Copyright (C) 2005-2009 Team MediaPortal

// Copyright (C) 2005-2009 Team MediaPortal
// http://www.team-mediaportal.com
// 
// This Program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2, or (at your option)
// any later version.
// 
// This Program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with GNU Make; see the file COPYING.  If not, write to
// the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
// http://www.gnu.org/copyleft/gpl.html

#endregion

using System;
using System.Windows.Forms;

namespace VirtualRemote
{
  /// <summary>
  /// Server Address form.
  /// </summary>
  internal partial class ServerAddress : Form
  {
    #region Properties

    /// <summary>
    /// Gets the server host.
    /// </summary>
    /// <value>The server host.</value>
    public string ServerHost
    {
      get { return textBoxComputer.Text; }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerAddress"/> class.
    /// </summary>
    public ServerAddress()
    {
      InitializeComponent();

      inputPanel.Enabled = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerAddress"/> class.
    /// </summary>
    /// <param name="serverHost">The server host.</param>
    public ServerAddress(string serverHost) : this()
    {
      if (!String.IsNullOrEmpty(serverHost))
        textBoxComputer.Text = serverHost;
    }

    #endregion Constructor

    #region Implementation

    private void buttonOK_Click(object sender, EventArgs e)
    {
      inputPanel.Enabled = false;

      DialogResult = DialogResult.OK;
      Close();
    }

    #endregion Implementation
  }
}