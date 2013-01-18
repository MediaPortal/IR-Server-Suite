using System;
using System.IO;
using System.Windows.Forms;
using IrssUtils;

namespace IrssCommands.Forms
{
  public partial class MacroPanel : UserControl
  {
    public delegate void EditMacroDelegate(string filename);
    public EditMacroDelegate DoCreateShortcutForMacro;

    //public delegate void MacoListRefreshed();
    //public MacoListRefreshed DoMacoListRefreshed;

    private Processor _commandProcessor;
    private readonly string _macroFolder;
    private readonly string[] _macroCategories;

    #region Constructor

    private MacroPanel()
    {
      InitializeComponent();

      // add images
      newMacroToolStripButton.Image = IrssUtils.Properties.Resources.Plus;
      editMacroToolStripButton.Image = IrssUtils.Properties.Resources.Edit;
      deleteMacroToolStripButton.Image = IrssUtils.Properties.Resources.Delete;
      createShortcutForMacroToolStripButton.Image = IrssUtils.Properties.Resources.Shortcut;
      testMacroToolStripButton.Image = IrssUtils.Properties.Resources.MoveRight;

      addMacroToolStripMenuItem.Image = IrssUtils.Properties.Resources.Plus;
      editMacroToolStripMenuItem.Image = IrssUtils.Properties.Resources.Edit;
      deleteMacroToolStripMenuItem.Image = IrssUtils.Properties.Resources.Delete;
      createShortcutForMacroToolStripMenuItem.Image = IrssUtils.Properties.Resources.Shortcut;
      testMacroToolStripMenuItem.Image = IrssUtils.Properties.Resources.MoveRight;
    }

    public MacroPanel(Processor processor, string macroFolder, string[] macroCategories) : this()
    {
      _commandProcessor = processor;
      _macroFolder = macroFolder;
      _macroCategories = macroCategories;
    }

    private void MacroPanel_Load(object sender, EventArgs e)
    {
      createShortcutForMacroToolStripButton.Visible = !ReferenceEquals(DoCreateShortcutForMacro, null);
      createShortcutForMacroToolStripMenuItem.Visible = !ReferenceEquals(DoCreateShortcutForMacro, null);
      toolStripSeparator2.Visible = !ReferenceEquals(DoCreateShortcutForMacro, null);
    }

    #endregion Constructor

    #region Public methods

    public void RefreshList()
    {
      listViewMacro.Items.Clear();

      string[] files = Processor.GetListMacro(_macroFolder);
      foreach (string file in files)
      {
        ListViewItem item = new ListViewItem();
        item.Text = Path.GetFileNameWithoutExtension(file);
        item.Tag = file;

        listViewMacro.Items.Add(item);
      }

      listViewMacro_SelectedIndexChanged(null, null);

      //if (!ReferenceEquals(DoMacoListRefreshed, null))
      //  DoMacoListRefreshed();
    }

    public void SetCommandProcessor(Processor commandProcessor)
    {
      _commandProcessor = commandProcessor;
    }

    #endregion Public methods

    private void NewMacro(object sender, EventArgs e)
    {
      EditMacro macroEditor = new EditMacro(_commandProcessor, _macroFolder, _macroCategories);
      macroEditor.ShowDialog(this);

      RefreshList();
    }

    private void EditMacro(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1) return;
      if (ReferenceEquals(listViewMacro.SelectedItems[0].Tag, null)) return;

      string file = listViewMacro.SelectedItems[0].Tag as string;
      if (ReferenceEquals(file, null)) return;

      EditMacro macroEditor = new EditMacro(_commandProcessor, _macroFolder, _macroCategories, file);
      macroEditor.ShowDialog(this);

      RefreshList();
    }

    private void DeleteMacro(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1) return;
      if (ReferenceEquals(listViewMacro.SelectedItems[0].Tag, null)) return;

      string file = listViewMacro.SelectedItems[0].Tag as string;
      if (ReferenceEquals(file, null)) return;

      if (File.Exists(file))
      {
        if (
          MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          File.Delete(file);
          listViewMacro.Items.Remove(listViewMacro.SelectedItems[0]);
        }
      }
      else
      {
        MessageBox.Show(this, "File not found: " + file, "Macro file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
      }
    }

    private void TestMacro(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1) return;
      if (ReferenceEquals(listViewMacro.SelectedItems[0].Tag, null)) return;

      string file = listViewMacro.SelectedItems[0].Tag as string;
      if (ReferenceEquals(file, null)) return;

      if (!File.Exists(file))
      {
        MessageBox.Show(this, "File not found: " + file, "Macro file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
        return;
      }

      try
      {
        Macro macro = new Macro(file);
        macro.Execute(_commandProcessor);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void CreateShortcutForMacro(object sender, EventArgs e)
    {
      if (ReferenceEquals(DoCreateShortcutForMacro, null)) return;

      if (listViewMacro.SelectedItems.Count != 1) return;
      if (ReferenceEquals(listViewMacro.SelectedItems[0].Tag, null)) return;

      string file = listViewMacro.SelectedItems[0].Tag as string;
      if (ReferenceEquals(file, null)) return;

      DoCreateShortcutForMacro(file);
    }

    private void listViewMacro_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedIndices.Count != 1)
      {
        editMacroToolStripButton.Enabled = false;
        editMacroToolStripMenuItem.Enabled = false;
        deleteMacroToolStripButton.Enabled = false;
        deleteMacroToolStripMenuItem.Enabled = false;
        testMacroToolStripButton.Enabled = false;
        testMacroToolStripMenuItem.Enabled = false;
        createShortcutForMacroToolStripButton.Enabled = false;
        createShortcutForMacroToolStripMenuItem.Enabled = false;
      }
      else
      {
        editMacroToolStripButton.Enabled = true;
        editMacroToolStripMenuItem.Enabled = true;
        deleteMacroToolStripButton.Enabled = true;
        deleteMacroToolStripMenuItem.Enabled = true;
        testMacroToolStripButton.Enabled = true;
        testMacroToolStripMenuItem.Enabled = true;
        createShortcutForMacroToolStripButton.Enabled = true;
        createShortcutForMacroToolStripMenuItem.Enabled = true;
      }
    }

    private void listViewMacro_AfterLabelEdit(object sender, LabelEditEventArgs e)
    {
      ListView origin = sender as ListView;
      if (origin == null)
      {
        e.CancelEdit = true;
        return;
      }

      if (String.IsNullOrEmpty(e.Label))
      {
        e.CancelEdit = true;
        return;
      }

      ListViewItem originItem = origin.Items[e.Item];

      string oldFileName = Path.Combine(_macroFolder, originItem.Text + Common.FileExtensionMacro);
      if (!File.Exists(oldFileName))
      {
        MessageBox.Show("File not found: " + oldFileName, "Cannot rename, Original file not found", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      string name = e.Label.Trim();

      if (!Common.IsValidFileName(name))
      {
        MessageBox.Show("File name not valid: " + name, "Cannot rename, New file name not valid", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        e.CancelEdit = true;
        return;
      }

      try
      {
        string newFileName = Path.Combine(_macroFolder, name + Common.FileExtensionMacro);
        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
