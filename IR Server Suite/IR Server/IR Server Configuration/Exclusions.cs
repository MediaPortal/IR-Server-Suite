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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using IrssUtils;

namespace IRServer.Configuration
{
  internal partial class Exclusions : Form
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Exclusions"/> class.
    /// </summary>
    public Exclusions()
    {
      InitializeComponent();
      Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

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

      string AbstractRemoteMapFolder = Path.Combine(Common.FolderAppData, "IR Server\\Abstract Remote Maps");

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