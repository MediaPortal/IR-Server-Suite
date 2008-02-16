using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InputService.Plugin
{

  /// <summary>
  /// Config form for selecting and configuring a Girder plugin.
  /// </summary>
  public partial class Config : Form
  {

    //List<string> _selectedPlugins;
    string _pluginFolder;


    /// <summary>
    /// Gets or sets the path of the Girder plugin folder.
    /// </summary>
    /// <value>The path of the Girder plugin folder.</value>
    public string PluginFolder
    {
      get { return _pluginFolder; }
      set { _pluginFolder = value; }
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
      if (listViewPlugins.SelectedIndices.Count != 1)
        return;

      string pluginFile = listViewPlugins.SelectedItems[0].Tag as string;

      try
      {
        GirderPluginWrapper pluginWrapper = new GirderPluginWrapper(pluginFile);

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
      if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
      {
        _pluginFolder = folderBrowserDialog.SelectedPath;

        UpdatePluginList();
      }
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    void UpdatePluginList()
    {

    }

  }

}
