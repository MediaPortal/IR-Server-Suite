using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Commands
{
  /// <summary>
  /// Edit Switch Command form.
  /// </summary>
  internal partial class EditSwitch : Form
  {
    #region Properties

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>The command parameters.</value>
    public string[] Parameters
    {
      get
      {
        string[] cases = new string[listViewCases.Items.Count*2];
        int index = 0;
        foreach (ListViewItem item in listViewCases.Items)
        {
          cases[index] = item.Text;
          cases[index + 1] = item.SubItems[0].Text;
          index += 2;
        }

        StringBuilder xml = new StringBuilder();
        using (StringWriter stringWriter = new StringWriter(xml))
        {
          XmlSerializer xmlSerializer = new XmlSerializer(typeof (string[]));
          xmlSerializer.Serialize(stringWriter, cases);
        }

        return new string[]
                 {
                   textBoxSwitchVar.Text.Trim(),
                   xml.ToString(),
                   textBoxDefaultCase.Text.Trim()
                 };
      }
    }

    #endregion Properties

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSwitch"/> class.
    /// </summary>
    public EditSwitch()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSwitch"/> class.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    public EditSwitch(string[] parameters)
      : this()
    {
      textBoxSwitchVar.Text = parameters[0];
      textBoxDefaultCase.Text = parameters[2];

      if (!String.IsNullOrEmpty(parameters[1]))
      {
        string[] cases;
        using (StringReader stringReader = new StringReader(parameters[1]))
        {
          XmlSerializer xmlSerializer = new XmlSerializer(typeof (string[]));
          cases = (string[]) xmlSerializer.Deserialize(stringReader);
        }

        for (int index = 0; index < cases.Length; index += 2)
        {
          ListViewItem item = new ListViewItem(cases[index]);
          item.SubItems.Add(cases[index + 1]);
          listViewCases.Items.Add(item);
        }
      }
    }

    #endregion Constructors

    #region Buttons

    private void toolStripButtonAdd_Click(object sender, EventArgs e)
    {
    }

    private void toolStripButtonEdit_Click(object sender, EventArgs e)
    {
    }

    private void toolStripButtonDelete_Click(object sender, EventArgs e)
    {
    }

    private void toolStripButtonDeleteAll_Click(object sender, EventArgs e)
    {
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (String.IsNullOrEmpty(textBoxSwitchVar.Text.Trim()))
      {
        MessageBox.Show(this, "You must include a variable to switch with", "Missing switch variable",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      if (listViewCases.Items.Count == 0)
      {
        MessageBox.Show(this, "You must include at least one case", "Missing cases", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
        return;
      }

      DialogResult = DialogResult.OK;
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    #endregion Buttons
  }
}