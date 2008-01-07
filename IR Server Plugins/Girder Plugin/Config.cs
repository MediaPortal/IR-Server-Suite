using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GirderPlugin
{

  /// <summary>
  /// Config form for selecting and configuring a Girder plugin.
  /// </summary>
  public partial class Config : Form
  {

    /// <summary>
    /// Gets or sets the name of the Girder plugin file in use.
    /// </summary>
    /// <value>The name of the Girder plugin file.</value>
    public string FileName
    {
      get { return textBoxPluginFile.Text; }
      set { textBoxPluginFile.Text = value; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Config"/> class.
    /// </summary>
    public Config()
    {
      InitializeComponent();
    }

    private void buttonConfigureGirderPlugin_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxPluginFile.Text))
        return;

      try
      {
        GirderPluginWrapper pluginWrapper = new GirderPluginWrapper(textBoxPluginFile.Text);

        pluginWrapper.GirOpen();

        if (!pluginWrapper.CanConfigure)
        {
          MessageBox.Show(this, "No configuration available", "Girder Plugin Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
          pluginWrapper.GirCommandGui();

          MessageBox.Show(this, "Press OK after the Girder plugin configuration is complete", "Girder Plugin Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        pluginWrapper.GirClose();

        pluginWrapper.Dispose();
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonFind_Click(object sender, EventArgs e)
    {
      openFileDialog.FileName = textBoxPluginFile.Text;

      if (openFileDialog.ShowDialog(this) == DialogResult.OK)
      {
        try
        {
          GirderPluginWrapper pluginWrapper = new GirderPluginWrapper(openFileDialog.FileName);

          pluginWrapper.GirOpen();

          string message = String.Format("{0}\n{1}\n\nUse this plugin?", pluginWrapper.GirName, pluginWrapper.GirDescription);

          if (MessageBox.Show(this, message, openFileDialog.FileName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            textBoxPluginFile.Text = openFileDialog.FileName;

          pluginWrapper.GirClose();

          pluginWrapper.Dispose();
        }
        catch (Exception ex)
        {
          MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

  }

}
