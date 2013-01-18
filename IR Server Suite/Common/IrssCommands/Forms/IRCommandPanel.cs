using System;
using System.IO;
using System.Windows.Forms;
using IrssUtils;

namespace IrssCommands.Forms
{
  public partial class IRCommandPanel : UserControl
  {
    public delegate void AddIrCommandHandler();
    public delegate void EditIrCommandHandler(string filename);

    public AddIrCommandHandler DoAddIrCommand;
    public EditIrCommandHandler DoEditIrCommand;

    #region Constructor

    public IRCommandPanel()
    {
      InitializeComponent();

      // add images
      newIRToolStripButton.Image = IrssUtils.Properties.Resources.Plus;
      editIRToolStripButton.Image = IrssUtils.Properties.Resources.Edit;
      deleteIRToolStripButton.Image = IrssUtils.Properties.Resources.Delete;

      addIRToolStripMenuItem.Image = IrssUtils.Properties.Resources.Plus;
      editIRToolStripMenuItem.Image = IrssUtils.Properties.Resources.Edit;
      deleteIRToolStripMenuItem.Image = IrssUtils.Properties.Resources.Delete;
    }

    public IRCommandPanel(AddIrCommandHandler addIrCommandHandler, EditIrCommandHandler editIrCommandHandler)
      : this()
    {
      DoAddIrCommand += addIrCommandHandler;
      DoEditIrCommand += editIrCommandHandler;
    }

    #endregion Constructor

    #region Public methods
    
    public void RefreshList()
    {
      listViewIR.Items.Clear();

      //string[] files = Processor.GetListMacro(_macroFolder);
      //foreach (string file in files)
      //{
      //  ListViewItem item = new ListViewItem();
      //  item.Text = Path.GetFileNameWithoutExtension(file);
      //  item.Tag = file;

      //  listViewMacro.Items.Add(item);
      //}

      string[] irList = Processor.GetListIR();
      if (irList != null && irList.Length > 0)
        foreach (string irFile in irList)
          listViewIR.Items.Add(irFile);

      listViewIR_SelectedIndexChanged(null, null);

      //if (!ReferenceEquals(DoMacoListRefreshed, null))
      //  DoMacoListRefreshed();
    }

    #endregion Public methods

    private void NewIRCommand(object sender, EventArgs e)
    {
      DoAddIrCommand();
      RefreshList();

      //EditMacro macroEditor = new EditMacro(_commandProcessor, _macroFolder, _macroCategories);
      //macroEditor.ShowDialog(this);

      //RefreshMacroList();
    }

    private void EditIRCommand(object sender, EventArgs e)
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      string command = listViewIR.SelectedItems[0].Text;
      string fileName = Path.Combine(Common.FolderIRCommands, command + Common.FileExtensionIR);

      if (File.Exists(fileName))
      {
        DoEditIrCommand(fileName);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        RefreshList();
      }
    }
    //private void EditMacro(object sender, EventArgs e)
    //{
    //  if (listViewMacro.SelectedItems.Count != 1) return;
    //  if (ReferenceEquals(listViewMacro.SelectedItems[0].Tag, null)) return;

    //  string file = listViewMacro.SelectedItems[0].Tag as string;
    //  if (ReferenceEquals(file, null)) return;

    //  EditMacro macroEditor = new EditMacro(_commandProcessor, _macroFolder, _macroCategories, file);
    //  macroEditor.ShowDialog(this);

    //  RefreshMacroList();
    //}

    private void DeleteIRCommand(object sender, EventArgs e)
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      string file = listViewIR.SelectedItems[0].Text;
      string fileName = Path.Combine(Common.FolderIRCommands, file + Common.FileExtensionIR);
      if (File.Exists(fileName))
      {
        if (
          MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          File.Delete(fileName);
          listViewIR.Items.Remove(listViewIR.SelectedItems[0]);
        }
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
      }
    }
    //private void DeleteMacro(object sender, EventArgs e)
    //{
    //  if (listViewMacro.SelectedItems.Count != 1) return;
    //  if (ReferenceEquals(listViewMacro.SelectedItems[0].Tag, null)) return;

    //  string file = listViewMacro.SelectedItems[0].Tag as string;
    //  if (ReferenceEquals(file, null)) return;

    //  if (File.Exists(file))
    //  {
    //    if (
    //      MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete",
    //                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
    //    {
    //      File.Delete(file);
    //      listViewMacro.Items.Remove(listViewMacro.SelectedItems[0]);
    //    }
    //  }
    //  else
    //  {
    //    MessageBox.Show(this, "File not found: " + file, "Macro file missing", MessageBoxButtons.OK,
    //                    MessageBoxIcon.Exclamation);
    //  }
    //}


    private void listViewIR_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (listViewIR.SelectedIndices.Count != 1)
      {
        editIRToolStripButton.Enabled = false;
        editIRToolStripMenuItem.Enabled = false;
        deleteIRToolStripButton.Enabled = false;
        deleteIRToolStripMenuItem.Enabled = false;
      }
      else
      {
        editIRToolStripButton.Enabled = true;
        editIRToolStripMenuItem.Enabled = true;
        deleteIRToolStripButton.Enabled = true;
        deleteIRToolStripMenuItem.Enabled = true;
      }
    }

    private void listViewIR_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      ListView origin = sender as ListView;
      if (origin == null)
      {
        e.CancelEdit = true;
        return;
      }
      //  ListView origin = sender as ListView;
      //  if (origin == null)
      //  {
      //    e.CancelEdit = true;
      //    return;
      //  }

      if (String.IsNullOrEmpty(e.Label))
      {
        e.CancelEdit = true;
        return;
      }
      //  if (String.IsNullOrEmpty(e.Label))
      //  {
      //    e.CancelEdit = true;
      //    return;
      //  }

      ListViewItem originItem = origin.Items[e.Item];
      //  ListViewItem originItem = origin.Items[e.Item];

      string oldFileName = Path.Combine(Common.FolderIRCommands, originItem.Text + Common.FileExtensionIR);
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }
      //  string oldFileName = Path.Combine(_macroFolder, originItem.Text + Common.FileExtensionMacro);
      //  if (!File.Exists(oldFileName))
      //  {
      //    MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK,
      //                    MessageBoxIcon.Error);
      //    e.CancelEdit = true;
      //    return;
      //  }

      string name = e.Label.Trim();
      //  string name = e.Label.Trim();

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }
      //  if (!Common.IsValidFileName(name))
      //  {
      //    MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK,
      //                    MessageBoxIcon.Error);
      //    e.CancelEdit = true;
      //    return;
      //  }

      try
      {
        string newFileName = Path.Combine(Common.FolderIRCommands, name + Common.FileExtensionIR);

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      //  try
      //  {
      //    string newFileName = Path.Combine(_macroFolder, name + Common.FileExtensionMacro);
      //    File.Move(oldFileName, newFileName);
      //  }
      //  catch (Exception ex)
      //  {
      //    IrssLog.Error(ex);
      //    MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      //  }
    }
  }
}
