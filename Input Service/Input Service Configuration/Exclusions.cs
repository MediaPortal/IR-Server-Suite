using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using IrssUtils;

namespace InputService.Configuration
{
  internal partial class Exclusions : Form
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Exclusions"/> class.
    /// </summary>
    public Exclusions()
    {
      InitializeComponent();

      InitTree();
    }

    /// <summary>
    /// Gets or sets the exclusion list.
    /// </summary>
    /// <value>The exclusion list.</value>
    public string[] ExclusionList
    {
      get
      {
        List<string> exclusions = new List<string>();
        foreach (TreeNode node in treeViewExclusions.Nodes)
          if (node.Checked)
            exclusions.Add(node.Tag as string);

        return exclusions.ToArray();
      }
      set
      {
        foreach (TreeNode node in treeViewExclusions.Nodes)
        {
          node.Checked = false;

          foreach (string exclusion in value)
            if (exclusion.Equals(node.Tag as string, StringComparison.OrdinalIgnoreCase))
              node.Checked = true;
        }
      }
    }

    private void InitTree()
    {
      treeViewExclusions.Nodes.Clear();

      string AbstractRemoteMapFolder = Path.Combine(Common.FolderAppData, "Input Service\\Abstract Remote Maps");

      string[] folders = Directory.GetDirectories(AbstractRemoteMapFolder, "*", SearchOption.TopDirectoryOnly);
      foreach (string folder in folders)
      {
        if (!Directory.Exists(folder))
          continue;

        string device = Path.GetFileName(folder);

        TreeNode deviceNode = new TreeNode(device);
        deviceNode.Tag = String.Format("{0}:", device);

        string[] files = Directory.GetFiles(folder, "*.xml", SearchOption.TopDirectoryOnly);
        foreach (string file in files)
        {
          if (!File.Exists(file))
            continue;

          string remote = Path.GetFileNameWithoutExtension(file);

          TreeNode remoteNode = new TreeNode(remote);
          remoteNode.Tag = String.Format("{0}:{1}", device, remote);

          deviceNode.Nodes.Add(remoteNode);
        }

        treeViewExclusions.Nodes.Add(deviceNode);
      }
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

    private void labelExpandAll_Click(object sender, EventArgs e)
    {
      treeViewExclusions.ExpandAll();
    }

    private void labelCollapseAll_Click(object sender, EventArgs e)
    {
      treeViewExclusions.CollapseAll();
    }
  }
}