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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using IrssUtils;

namespace DboxTuner
{
  internal partial class SetupForm : Form
  {
    #region Variables

    private StbBoxType _boxType = StbBoxType.Unknown;

    #endregion Variables

    #region Properties

    public StbBoxType BoxType
    {
      get { return _boxType; }
      set { _boxType = value; }
    }

    public string Address
    {
      get { return textBoxIpAddress.Text; }
      set { textBoxIpAddress.Text = value; }
    }

    public string UserName
    {
      get { return textBoxUserName.Text; }
      set { textBoxUserName.Text = value; }
    }

    public string Password
    {
      get { return textBoxPassword.Text; }
      set { textBoxPassword.Text = value; }
    }

    public int Timeout
    {
      get { return decimal.ToInt32(numericUpDownTimeout.Value); }
      set { numericUpDownTimeout.Value = new decimal(value); }
    }

    #endregion Properties

    #region Constructor

    public SetupForm()
    {
      InitializeComponent();
    }

    #endregion Constructor

    private void InitializeComponent()
    {
      buttonGetData = new Button();
      textBoxPassword = new TextBox();
      labelPassword = new Label();
      textBoxUserName = new TextBox();
      labelUserName = new Label();
      textBoxIpAddress = new TextBox();
      labelIpAddress = new Label();
      buttonOK = new Button();
      buttonCancel = new Button();
      statusStrip = new StatusStrip();
      toolStripStatusLabel = new ToolStripStatusLabel();
      buttonDetectBoxType = new Button();
      labelTimeout = new Label();
      numericUpDownTimeout = new NumericUpDown();
      labelMilliseconds = new Label();
      statusStrip.SuspendLayout();
      ((ISupportInitialize) (numericUpDownTimeout)).BeginInit();
      SuspendLayout();
      // 
      // buttonGetData
      // 
      buttonGetData.Anchor = (((AnchorStyles.Bottom | AnchorStyles.Left)));
      buttonGetData.Location = new Point(8, 112);
      buttonGetData.Name = "buttonGetData";
      buttonGetData.Size = new Size(104, 24);
      buttonGetData.TabIndex = 6;
      buttonGetData.Text = "Get channel list";
      buttonGetData.UseVisualStyleBackColor = true;
      buttonGetData.Click += buttonGetData_Click;
      // 
      // textBoxPassword
      // 
      textBoxPassword.Anchor = ((((AnchorStyles.Top | AnchorStyles.Left)
                                  | AnchorStyles.Right)));
      textBoxPassword.Location = new Point(88, 56);
      textBoxPassword.Name = "textBoxPassword";
      textBoxPassword.Size = new Size(176, 20);
      textBoxPassword.TabIndex = 5;
      // 
      // labelPassword
      // 
      labelPassword.Location = new Point(8, 56);
      labelPassword.Name = "labelPassword";
      labelPassword.Size = new Size(80, 20);
      labelPassword.TabIndex = 4;
      labelPassword.Text = "Password:";
      labelPassword.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // textBoxUserName
      // 
      textBoxUserName.Anchor = ((((AnchorStyles.Top | AnchorStyles.Left)
                                  | AnchorStyles.Right)));
      textBoxUserName.Location = new Point(88, 32);
      textBoxUserName.Name = "textBoxUserName";
      textBoxUserName.Size = new Size(176, 20);
      textBoxUserName.TabIndex = 3;
      // 
      // labelUserName
      // 
      labelUserName.Location = new Point(8, 32);
      labelUserName.Name = "labelUserName";
      labelUserName.Size = new Size(80, 20);
      labelUserName.TabIndex = 2;
      labelUserName.Text = "User name:";
      labelUserName.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // textBoxIpAddress
      // 
      textBoxIpAddress.Anchor = ((((AnchorStyles.Top | AnchorStyles.Left)
                                   | AnchorStyles.Right)));
      textBoxIpAddress.Location = new Point(88, 8);
      textBoxIpAddress.Name = "textBoxIpAddress";
      textBoxIpAddress.Size = new Size(176, 20);
      textBoxIpAddress.TabIndex = 1;
      // 
      // labelIpAddress
      // 
      labelIpAddress.Location = new Point(8, 8);
      labelIpAddress.Name = "labelIpAddress";
      labelIpAddress.Size = new Size(80, 20);
      labelIpAddress.TabIndex = 0;
      labelIpAddress.Text = "IP address:";
      labelIpAddress.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // buttonOK
      // 
      buttonOK.Anchor = (((AnchorStyles.Bottom | AnchorStyles.Right)));
      buttonOK.Location = new Point(128, 144);
      buttonOK.Name = "buttonOK";
      buttonOK.Size = new Size(64, 24);
      buttonOK.TabIndex = 7;
      buttonOK.Text = "OK";
      buttonOK.UseVisualStyleBackColor = true;
      buttonOK.Click += buttonOK_Click;
      // 
      // buttonCancel
      // 
      buttonCancel.Anchor = (((AnchorStyles.Bottom | AnchorStyles.Right)));
      buttonCancel.DialogResult = DialogResult.Cancel;
      buttonCancel.Location = new Point(200, 144);
      buttonCancel.Name = "buttonCancel";
      buttonCancel.Size = new Size(64, 24);
      buttonCancel.TabIndex = 8;
      buttonCancel.Text = "Cancel";
      buttonCancel.UseVisualStyleBackColor = true;
      buttonCancel.Click += buttonCancel_Click;
      // 
      // statusStrip
      // 
      statusStrip.Items.AddRange(new ToolStripItem[]
                                   {
                                     toolStripStatusLabel
                                   });
      statusStrip.Location = new Point(0, 177);
      statusStrip.Name = "statusStrip";
      statusStrip.Size = new Size(272, 22);
      statusStrip.TabIndex = 9;
      // 
      // toolStripStatusLabel
      // 
      toolStripStatusLabel.Name = "toolStripStatusLabel";
      toolStripStatusLabel.Size = new Size(0, 17);
      // 
      // buttonDetectBoxType
      // 
      buttonDetectBoxType.Anchor = (((AnchorStyles.Bottom | AnchorStyles.Left)));
      buttonDetectBoxType.Location = new Point(8, 144);
      buttonDetectBoxType.Name = "buttonDetectBoxType";
      buttonDetectBoxType.Size = new Size(104, 24);
      buttonDetectBoxType.TabIndex = 10;
      buttonDetectBoxType.Text = "Redetect box";
      buttonDetectBoxType.UseVisualStyleBackColor = true;
      buttonDetectBoxType.Click += buttonDetectBoxType_Click;
      // 
      // labelTimeout
      // 
      labelTimeout.Location = new Point(8, 80);
      labelTimeout.Name = "labelTimeout";
      labelTimeout.Size = new Size(80, 20);
      labelTimeout.TabIndex = 11;
      labelTimeout.Text = "Timeout:";
      labelTimeout.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // numericUpDownTimeout
      // 
      numericUpDownTimeout.Anchor = ((((AnchorStyles.Top | AnchorStyles.Left)
                                       | AnchorStyles.Right)));
      numericUpDownTimeout.Increment = new decimal(new int[]
                                                     {
                                                       500,
                                                       0,
                                                       0,
                                                       0
                                                     });
      numericUpDownTimeout.Location = new Point(88, 80);
      numericUpDownTimeout.Maximum = new decimal(new int[]
                                                   {
                                                     60000,
                                                     0,
                                                     0,
                                                     0
                                                   });
      numericUpDownTimeout.Minimum = new decimal(new int[]
                                                   {
                                                     1000,
                                                     0,
                                                     0,
                                                     0
                                                   });
      numericUpDownTimeout.Name = "numericUpDownTimeout";
      numericUpDownTimeout.Size = new Size(144, 20);
      numericUpDownTimeout.TabIndex = 12;
      numericUpDownTimeout.TextAlign = HorizontalAlignment.Center;
      numericUpDownTimeout.Value = new decimal(new int[]
                                                 {
                                                   2000,
                                                   0,
                                                   0,
                                                   0
                                                 });
      // 
      // labelMilliseconds
      // 
      labelMilliseconds.Anchor = (((AnchorStyles.Top | AnchorStyles.Right)));
      labelMilliseconds.Location = new Point(232, 80);
      labelMilliseconds.Name = "labelMilliseconds";
      labelMilliseconds.Size = new Size(32, 20);
      labelMilliseconds.TabIndex = 13;
      labelMilliseconds.Text = "ms";
      labelMilliseconds.TextAlign = ContentAlignment.MiddleCenter;
      // 
      // SetupForm
      // 
      ClientSize = new Size(272, 199);
      Controls.Add(labelMilliseconds);
      Controls.Add(numericUpDownTimeout);
      Controls.Add(labelTimeout);
      Controls.Add(buttonDetectBoxType);
      Controls.Add(statusStrip);
      Controls.Add(buttonGetData);
      Controls.Add(buttonCancel);
      Controls.Add(buttonOK);
      Controls.Add(labelIpAddress);
      Controls.Add(textBoxIpAddress);
      Controls.Add(labelUserName);
      Controls.Add(textBoxUserName);
      Controls.Add(textBoxPassword);
      Controls.Add(labelPassword);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MinimizeBox = false;
      MinimumSize = new Size(278, 200);
      Name = "SetupForm";
      ShowIcon = false;
      StartPosition = FormStartPosition.CenterScreen;
      Text = "Dbox Tuner Setup";
      statusStrip.ResumeLayout(false);
      statusStrip.PerformLayout();
      ((ISupportInitialize) (numericUpDownTimeout)).EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    private void buttonGetData_Click(object sender, EventArgs e)
    {
      StatusMessage("Attempting to read channel list ...");

      try
      {
        string url = Program.UrlPrefix + Address;

        // Detect box type ...
        if (BoxType == StbBoxType.Unknown)
          BoxType = Program.DetectBoxType(url, UserName, Password, Timeout);

        if (BoxType == StbBoxType.Unknown)
        {
          StatusMessage("ERROR - No STB or unknown type detected!");
        }
        else
        {
          StatusMessage("Detected box type: {0}", _boxType);

          DataSet dataSet = Program.GetData(url, UserName, Password, BoxType, Timeout);
          DataTable dataTable = dataSet.Tables[0];

          if (dataTable.Rows.Count != 0)
            StatusMessage("{0} channels found", dataTable.Rows.Count);
          else
            StatusMessage("ERROR - No channels found!");

          if (File.Exists(Program.DataFile))
            File.Delete(Program.DataFile);

          dataTable.WriteXml(Program.DataFile, XmlWriteMode.WriteSchema);
        }
      }
      catch (Exception ex)
      {
        StatusMessage("ERROR - Cannot read bouquet!");
        IrssLog.Error(ex);

        MessageBox.Show(this, ex.ToString(), "Dbox Tuner - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void StatusMessage(string format, params object[] args)
    {
      string message = String.Format(format, args);
      toolStripStatusLabel.Text = message;
      statusStrip.Update();

      if (message.StartsWith("ERROR", StringComparison.OrdinalIgnoreCase))
        IrssLog.Error(message);
      else
        IrssLog.Info(message);
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void buttonDetectBoxType_Click(object sender, EventArgs e)
    {
      BoxType = Program.DetectBoxType(Program.UrlPrefix + Address, UserName, Password, Timeout);
    }
  }
}