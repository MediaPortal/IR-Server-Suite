using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InputService.Configuration
{

  partial class Exclusions : Form
  {

    /// <summary>
    /// Gets or sets the exclusion list.
    /// </summary>
    /// <value>The exclusion list.</value>
    public string[] ExclusionList
    {
      get
      {
        List<string> exclude = new List<string>();
        foreach (ListViewItem item in listViewExclusions.Items)
        {
          if (item.Checked)
            exclude.Add(item.Text);
        }

        if (exclude.Count == 0)
          return null;
        else
          return exclude.ToArray();
      }
      set
      {
        foreach (ListViewItem item in listViewExclusions.Items)
        {
          foreach (string exclude in value)
          {
            if (item.Text.Equals(exclude, StringComparison.OrdinalIgnoreCase))
            {
              item.Checked = true;
              break;
            }
          }
        }          
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Exclusions"/> class.
    /// </summary>
    /// <param name="plugins">The list of plugins.</param>
    public Exclusions(string[] plugins)
    {
      InitializeComponent();

      foreach (string plugin in plugins)
        listViewExclusions.Items.Add(plugin);
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
