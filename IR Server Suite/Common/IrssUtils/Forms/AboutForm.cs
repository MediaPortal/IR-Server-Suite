using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IrssUtils.Forms
{
  public partial class AboutForm : Form
  {
    public AboutForm()
    {
      InitializeComponent();

      productNameLabel.Text = Application.ProductName;
      productVersionLabel.Text = Application.ProductVersion;
    }

    private void okButton_Click(object sender, EventArgs e)
    {
      Close();
    }
    
    private void manualLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://wiki.team-mediaportal.com/IRServerSuite");
    }

    private void forumLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://forum.team-mediaportal.com/ir-server-suite-irss-165/");
    }

    private void teamLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("http://www.team-mediaportal.com/");
    }
  }
}
