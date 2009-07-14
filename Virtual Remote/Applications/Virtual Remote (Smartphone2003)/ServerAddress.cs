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

#region Using directives

using System;
using System.Windows.Forms;

#endregion

namespace VirtualRemote
{
  /// <summary>
  /// Summary description for ServerAddress.
  /// </summary>
  internal class ServerAddress : Form
  {
    private Label labelDescription;
    private MainMenu mainMenu;
    private MenuItem menuItemCancel;
    private MenuItem menuItemOK;
    private TextBox textBoxServerHost;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerAddress"/> class.
    /// </summary>
    public ServerAddress()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerAddress"/> class.
    /// </summary>
    /// <param name="serverHost">The server host.</param>
    public ServerAddress(string serverHost) : this()
    {
      if (serverHost != null)
        textBoxServerHost.Text = serverHost;
    }

    /// <summary>
    /// Gets or sets the server host.
    /// </summary>
    /// <value>The server host.</value>
    public string ServerHost
    {
      get { return textBoxServerHost.Text; }
      set { textBoxServerHost.Text = value; }
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
    }

    private void menuItemOK_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void menuItemCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void textBoxServerHost_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.KeyCode)
      {
        case Keys.Enter:
          DialogResult = DialogResult.OK;
          Close();
          break;

        case Keys.Escape:
          DialogResult = DialogResult.Cancel;
          Close();
          break;
      }
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuItemOK = new System.Windows.Forms.MenuItem();
      this.menuItemCancel = new System.Windows.Forms.MenuItem();
      this.textBoxServerHost = new System.Windows.Forms.TextBox();
      this.labelDescription = new System.Windows.Forms.Label();
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.Add(this.menuItemOK);
      this.mainMenu.MenuItems.Add(this.menuItemCancel);
      // 
      // menuItemOK
      // 
      this.menuItemOK.Text = "OK";
      this.menuItemOK.Click += new System.EventHandler(this.menuItemOK_Click);
      // 
      // menuItemCancel
      // 
      this.menuItemCancel.Text = "Cancel";
      this.menuItemCancel.Click += new System.EventHandler(this.menuItemCancel_Click);
      // 
      // textBoxServerHost
      // 
      this.textBoxServerHost.Location = new System.Drawing.Point(8, 64);
      this.textBoxServerHost.Size = new System.Drawing.Size(160, 24);
      this.textBoxServerHost.Text = "192.168.0.1";
      this.textBoxServerHost.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxServerHost_KeyDown);
      // 
      // labelDescription
      // 
      this.labelDescription.Location = new System.Drawing.Point(8, 8);
      this.labelDescription.Size = new System.Drawing.Size(160, 48);
      this.labelDescription.Text = "Please enter the IR Server host address:";
      // 
      // ServerAddress
      // 
      this.ClientSize = new System.Drawing.Size(176, 180);
      this.Controls.Add(this.textBoxServerHost);
      this.Controls.Add(this.labelDescription);
      this.Menu = this.mainMenu;
      this.Text = "Server Address";
    }

    #endregion
  }
}