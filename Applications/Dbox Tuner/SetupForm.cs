#region Copyright (C) 2005-2007 Team MediaPortal

/* 
 *	Copyright (C) 2005-2007 Team MediaPortal
 *	http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace DboxTuner
{

  partial class SetupForm : Form
  {

    #region Variables

    string _url = "";
    string _userName = "";
    string _password = "";
    string _boxType = "";
    static DataTable _bouquets = null;

    #endregion Variables

    #region Constructor

    public SetupForm()
    {
      InitializeComponent();
    }

    #endregion Constructor

    private void InitializeComponent()
    {
      this.buttonGetData = new System.Windows.Forms.Button();
      this.textBoxPassword = new System.Windows.Forms.TextBox();
      this.labelPassword = new System.Windows.Forms.Label();
      this.textBoxUserName = new System.Windows.Forms.TextBox();
      this.labelUserName = new System.Windows.Forms.Label();
      this.textBoxIpAddress = new System.Windows.Forms.TextBox();
      this.labelIpAddress = new System.Windows.Forms.Label();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.statusStrip = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.statusStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // buttonGetData
      // 
      this.buttonGetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonGetData.Location = new System.Drawing.Point(8, 88);
      this.buttonGetData.Name = "buttonGetData";
      this.buttonGetData.Size = new System.Drawing.Size(104, 24);
      this.buttonGetData.TabIndex = 6;
      this.buttonGetData.Text = "Get channel list";
      this.buttonGetData.UseVisualStyleBackColor = true;
      this.buttonGetData.Click += new System.EventHandler(this.buttonGetData_Click);
      // 
      // textBoxPassword
      // 
      this.textBoxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPassword.Location = new System.Drawing.Point(88, 56);
      this.textBoxPassword.Name = "textBoxPassword";
      this.textBoxPassword.Size = new System.Drawing.Size(176, 20);
      this.textBoxPassword.TabIndex = 5;
      // 
      // labelPassword
      // 
      this.labelPassword.Location = new System.Drawing.Point(8, 56);
      this.labelPassword.Name = "labelPassword";
      this.labelPassword.Size = new System.Drawing.Size(80, 20);
      this.labelPassword.TabIndex = 4;
      this.labelPassword.Text = "Password:";
      this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxUserName
      // 
      this.textBoxUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxUserName.Location = new System.Drawing.Point(88, 32);
      this.textBoxUserName.Name = "textBoxUserName";
      this.textBoxUserName.Size = new System.Drawing.Size(176, 20);
      this.textBoxUserName.TabIndex = 3;
      // 
      // labelUserName
      // 
      this.labelUserName.Location = new System.Drawing.Point(8, 32);
      this.labelUserName.Name = "labelUserName";
      this.labelUserName.Size = new System.Drawing.Size(80, 20);
      this.labelUserName.TabIndex = 2;
      this.labelUserName.Text = "User name:";
      this.labelUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxIpAddress
      // 
      this.textBoxIpAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxIpAddress.Location = new System.Drawing.Point(88, 8);
      this.textBoxIpAddress.Name = "textBoxIpAddress";
      this.textBoxIpAddress.Size = new System.Drawing.Size(176, 20);
      this.textBoxIpAddress.TabIndex = 1;
      // 
      // labelIpAddress
      // 
      this.labelIpAddress.Location = new System.Drawing.Point(8, 8);
      this.labelIpAddress.Name = "labelIpAddress";
      this.labelIpAddress.Size = new System.Drawing.Size(80, 20);
      this.labelIpAddress.TabIndex = 0;
      this.labelIpAddress.Text = "IP address:";
      this.labelIpAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.Location = new System.Drawing.Point(128, 88);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 7;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(200, 88);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 8;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // statusStrip
      // 
      this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
      this.statusStrip.Location = new System.Drawing.Point(0, 123);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new System.Drawing.Size(272, 22);
      this.statusStrip.TabIndex = 9;
      // 
      // toolStripStatusLabel
      // 
      this.toolStripStatusLabel.Name = "toolStripStatusLabel";
      this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
      // 
      // SetupForm
      // 
      this.AcceptButton = this.buttonOK;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(272, 145);
      this.Controls.Add(this.statusStrip);
      this.Controls.Add(this.buttonGetData);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.labelIpAddress);
      this.Controls.Add(this.textBoxIpAddress);
      this.Controls.Add(this.labelUserName);
      this.Controls.Add(this.textBoxUserName);
      this.Controls.Add(this.textBoxPassword);
      this.Controls.Add(this.labelPassword);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(278, 170);
      this.Name = "SetupForm";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Dbox Tuner Setup";
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    private void buttonGetData_Click(object sender, EventArgs e)
    {
      StatusMessage("Attempting to read channel list ...");

      try
      {
        _url = "http://" + textBoxIpAddress.Text;
        _userName = textBoxUserName.Text;
        _password = textBoxPassword.Text;


        //detect boxtype

        // test if the value is one of the valid boxtypes, if it's not run detection routine
        if (_boxType != "Neutrino" && _boxType != "Enigma v1" && _boxType != "Enigma v2")
        {
          Request request = new Request(_url, _userName, _password);
          _boxType = "unknown";

          string str1 = request.PostData("/control/getmode").ToLower(); // neutrino
          if (str1.Contains("tv") || str1.Contains("radio") || str1.Contains("unknown"))
            _boxType = "Neutrino";

          if (_boxType != "Neutrino")
          {
            string str2 = request.PostData("/cgi-bin/status").ToLower(); // enigma v1
            if (str2.Contains("enigma"))
              _boxType = "Enigma v1";
          }

          if ((_boxType != "Neutrino") && (_boxType != "Enigma v1"))
          {
            string str3 = request.PostData("/web/stream.m3u"); // enigma v2
            if (str3.Contains("#EXTM3U"))
              _boxType = "Enigma v2";
          }

          StatusMessage("Detected: {0}", _boxType);
        }

        if (_boxType == "Neutrino" || _boxType == "Enigma v1" || _boxType == "Enigma v2")
        {
          //get bouquets
          Data _DBox = new Data(_url, _userName, _password, _boxType);
          DboxFunctions dboxfunc = new DboxFunctions(_url, _userName, _password, _boxType);
          
          _bouquets = _DBox.UserTVBouquets.Tables[0];
          
          if (_bouquets.Rows.Count != 0)
            StatusMessage("{0} channels found", _bouquets.Rows.Count);
          else
            StatusMessage("ERROR - No channels found!");

          if (File.Exists(Program.DataFile))
            File.Delete(Program.DataFile);

          _bouquets.WriteXml(Program.DataFile, XmlWriteMode.WriteSchema);
        }
        else
        {
          StatusMessage("ERROR - No STB detected!");
        }
      }
      catch
      {
        StatusMessage("ERROR - Cannot read bouquet!");
      }
    }
    
    void StatusMessage(string format, params object[] args)
    {
      toolStripStatusLabel.Text = String.Format(format, args);
      statusStrip.Update();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

  }

}
