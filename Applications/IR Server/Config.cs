using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

using InputService.Plugin;
using IrssUtils;

namespace IRServer
{

  partial class Config : Form
  {

    #region Constants

    const int ColIcon       = 0;
    const int ColName       = 1;
    const int ColReceive    = 2;
    const int ColTransmit   = 3;
    const int ColConfigure  = 4;

    #endregion Constants

    #region Variables

    PluginBase[] _transceivers;

    bool _abstractRemoteMode  = false;
    IRServerMode _mode        = IRServerMode.ServerMode;
    string _hostComputer      = String.Empty;
    string _processPriority   = String.Empty;

    #endregion Variables

    #region Properties

    public bool AbstractRemoteMode
    {
      get { return _abstractRemoteMode; }
      set { _abstractRemoteMode = value; }
    }
    public IRServerMode Mode
    {
      get { return _mode; }
      set { _mode = value; }
    }
    public string HostComputer
    {
      get { return _hostComputer; }
      set { _hostComputer = value; }
    }
    public string ProcessPriority
    {
      get { return _processPriority; }
      set { _processPriority = value; }
    }

    public string[] PluginReceive
    {
      get
      {
        List<string> receivers = new List<string>();

        SourceGrid.Cells.CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, ColReceive] as SourceGrid.Cells.CheckBox;
          if (checkBox != null && checkBox.Checked)
            receivers.Add(gridPlugins[row, ColName].DisplayText);
        }

        if (receivers.Count == 0)
          return null;
        else
          return receivers.ToArray();
      }
      set
      {
        SourceGrid.Cells.CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, ColReceive] as SourceGrid.Cells.CheckBox;
          if (checkBox == null)
            continue;

          if (value == null)
            checkBox.Checked = false;
          else if (Array.IndexOf<string>(value, gridPlugins[row, ColName].DisplayText) != -1)
            checkBox.Checked = true;
          else
            checkBox.Checked = false;
        }
      }
    }
    public string PluginTransmit
    {
      get
      {
        SourceGrid.Cells.CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, ColTransmit] as SourceGrid.Cells.CheckBox;
          if (checkBox != null && checkBox.Checked)
            return gridPlugins[row, ColName].DisplayText;
        }

        return String.Empty;
      }
      set
      {
        if (String.IsNullOrEmpty(value))
          return;

        SourceGrid.Cells.CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, ColTransmit] as SourceGrid.Cells.CheckBox;
          if (checkBox == null)
            continue;

          if (gridPlugins[row, ColName].DisplayText.Equals(value, StringComparison.OrdinalIgnoreCase))
            checkBox.Checked = true;
          else
            checkBox.Checked = false;
        }
      }
    }

    #endregion Properties

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Config"/> class.
    /// </summary>
    public Config()
    {
      InitializeComponent();

      // Add transceivers to list ...
      _transceivers = Program.AvailablePlugins();
      if (_transceivers == null || _transceivers.Length == 0)
        MessageBox.Show(this, "No IR Server Plugins found!", "IR Server Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
      else
        CreateGrid();

      try
      {
        checkBoxRunAtBoot.Checked = SystemRegistry.GetAutoRun("IR Server");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        checkBoxRunAtBoot.Checked = false;
      }
    }

    #endregion Constructor

    #region Controls

    private void buttonOK_Click(object sender, EventArgs e)
    {
      try
      {
        if (checkBoxRunAtBoot.Checked)
          SystemRegistry.SetAutoRun("IR Server", Application.ExecutablePath);
        else
          SystemRegistry.RemoveAutoRun("IR Server");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
      }

      this.DialogResult = DialogResult.OK;
      this.Close();
    }
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void buttonClickEvent_Executed(object sender, EventArgs e)
    {
      SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
      SourceGrid.Cells.Button cell = (SourceGrid.Cells.Button)context.Cell;

      try
      {
        IConfigure plugin = cell.Row.Tag as IConfigure;
        if (plugin != null)
          plugin.Configure(this);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    private void TransmitChanged(object sender, EventArgs e)
    {
      SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
      SourceGrid.Cells.CheckBox cell = (SourceGrid.Cells.CheckBox)context.Cell;

      if (!cell.Checked)
        return;

      PluginBase plugin = cell.Row.Tag as PluginBase;

      for (int row = 1; row < gridPlugins.RowsCount; row++)
      {
        SourceGrid.Cells.CheckBox checkBox = gridPlugins[row, ColTransmit] as SourceGrid.Cells.CheckBox;

        if (checkBox != null && checkBox.Checked && !gridPlugins[row, ColName].DisplayText.Equals(plugin.Name, StringComparison.OrdinalIgnoreCase))
          checkBox.Checked = false;
      }
    }
    private void PluginDoubleClick(object sender, EventArgs e)
    {
      SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
      SourceGrid.Cells.Cell cell = (SourceGrid.Cells.Cell)context.Cell;

      SourceGrid.Cells.CheckBox checkBoxReceive   = gridPlugins[cell.Row.Index, ColReceive] as SourceGrid.Cells.CheckBox;
      if (checkBoxReceive != null)
        checkBoxReceive.Checked = true;

      SourceGrid.Cells.CheckBox checkBoxTransmit  = gridPlugins[cell.Row.Index, ColTransmit] as SourceGrid.Cells.CheckBox;
      if (checkBoxTransmit != null)
        checkBoxTransmit.Checked = true;
    }

    private void toolStripButtonDetect_Click(object sender, EventArgs e)
    {
      // TODO: Place on a seperate thread?
      Detect();
    }
    private void toolStripButtonAdvancedSettings_Click(object sender, EventArgs e)
    {
      Advanced();
    }
    private void toolStripButtonHelp_Click(object sender, EventArgs e)
    {
      ShowHelp();
    }

    #endregion Controls

    void CreateGrid()
    {
      IrssLog.Info("Creating configuration grid ...");

      try
      {
        int row = 0;

        gridPlugins.Rows.Clear();
        gridPlugins.Columns.SetCount(5);

        // Setup Column Headers
        gridPlugins.Rows.Insert(row);

        SourceGrid.Cells.ColumnHeader header = new SourceGrid.Cells.ColumnHeader(" ");
        header.AutomaticSortEnabled = false;
        gridPlugins[row, ColIcon] = header;

        gridPlugins[row, ColName]       = new SourceGrid.Cells.ColumnHeader("Name");
        gridPlugins[row, ColReceive]    = new SourceGrid.Cells.ColumnHeader("Receive");
        gridPlugins[row, ColTransmit]   = new SourceGrid.Cells.ColumnHeader("Transmit");
        gridPlugins[row, ColConfigure]  = new SourceGrid.Cells.ColumnHeader("Configure");
        gridPlugins.FixedRows = 1;

        foreach (PluginBase transceiver in _transceivers)
        {
          gridPlugins.Rows.Insert(++row);
          gridPlugins.Rows[row].Tag = transceiver;

          // Icon Cell
          if (transceiver.DeviceIcon != null)
          {
            SourceGrid.Cells.Image iconCell = new SourceGrid.Cells.Image(transceiver.DeviceIcon);
            iconCell.Editor.EnableEdit = false;

            gridPlugins[row, ColIcon] = iconCell;
          }
          else
          {
            gridPlugins[row, ColIcon] = new SourceGrid.Cells.Cell();
          }

          // Name Cell
          SourceGrid.Cells.Cell nameCell = new SourceGrid.Cells.Cell(transceiver.Name);

          SourceGrid.Cells.Controllers.CustomEvents nameCellController = new SourceGrid.Cells.Controllers.CustomEvents();
          nameCellController.DoubleClick += new EventHandler(PluginDoubleClick);
          nameCell.AddController(nameCellController);

          nameCell.AddController(new SourceGrid.Cells.Controllers.ToolTipText());
          nameCell.ToolTipText = String.Format("{0}\nVersion: {1}\nAuthor: {2}\n{3}", transceiver.Name, transceiver.Version, transceiver.Author, transceiver.Description);

          gridPlugins[row, ColName] = nameCell;

          // Receive Cell
          if (transceiver is IRemoteReceiver || transceiver is IMouseReceiver || transceiver is IKeyboardReceiver)
          {
            gridPlugins[row, ColReceive] = new SourceGrid.Cells.CheckBox();
          }
          else
          {
            gridPlugins[row, ColReceive] = new SourceGrid.Cells.Cell();
          }

          // Transmit Cell
          if (transceiver is ITransmitIR)
          {
            SourceGrid.Cells.CheckBox checkbox = new SourceGrid.Cells.CheckBox();

            SourceGrid.Cells.Controllers.CustomEvents checkboxcontroller = new SourceGrid.Cells.Controllers.CustomEvents();
            checkboxcontroller.ValueChanged += new EventHandler(TransmitChanged);
            checkbox.Controller.AddController(checkboxcontroller);

            gridPlugins[row, ColTransmit] = checkbox;
          }
          else
          {
            gridPlugins[row, ColTransmit] = new SourceGrid.Cells.Cell();
          }

          // Configure Cell
          if (transceiver is IConfigure)
          {
            SourceGrid.Cells.Button button = new SourceGrid.Cells.Button("Configure");

            SourceGrid.Cells.Controllers.Button buttonClickEvent = new SourceGrid.Cells.Controllers.Button();
            buttonClickEvent.Executed += new EventHandler(buttonClickEvent_Executed);
            button.Controller.AddController(buttonClickEvent);

            gridPlugins[row, ColConfigure] = button;
          }
          else
          {
            gridPlugins[row, ColConfigure] = new SourceGrid.Cells.Cell();
          }
        }

        gridPlugins.Columns[ColIcon].AutoSizeMode       = SourceGrid.AutoSizeMode.EnableAutoSize;
        gridPlugins.Columns[ColName].AutoSizeMode       = SourceGrid.AutoSizeMode.Default;
        gridPlugins.Columns[ColReceive].AutoSizeMode    = SourceGrid.AutoSizeMode.EnableAutoSize;
        gridPlugins.Columns[ColTransmit].AutoSizeMode   = SourceGrid.AutoSizeMode.EnableAutoSize;
        gridPlugins.Columns[ColConfigure].AutoSizeMode  = SourceGrid.AutoSizeMode.EnableAutoSize;
        gridPlugins.AutoStretchColumnsToFitWidth        = true;
        gridPlugins.AutoSizeCells();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.ToString(), "Error setting up plugin grid", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    void Detect()
    {
      IrssLog.Info("Attempting to detect Input Plugins ...");

      SourceGrid.Cells.CheckBox checkBox;
      for (int row = 1; row < gridPlugins.RowsCount; row++)
      {
        try
        {
          PluginBase plugin = gridPlugins.Rows[row].Tag as PluginBase;

          bool detected = plugin.Detect();

          if (detected)
            IrssLog.Info("Found: {0}", plugin.Name);

          // Receive
          checkBox = gridPlugins[row, ColReceive] as SourceGrid.Cells.CheckBox;
          if (checkBox != null)
            checkBox.Checked = detected;

          // Transmit
          checkBox = gridPlugins[row, ColTransmit] as SourceGrid.Cells.CheckBox;
          if (checkBox != null)
            checkBox.Checked = detected;
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }
      }
    }
    void Advanced()
    {
      IrssLog.Info("Entering advanced configuration ...");

      Advanced advanced = new Advanced();

      advanced.AbstractRemoteMode = _abstractRemoteMode;
      advanced.Mode               = _mode;
      advanced.HostComputer       = _hostComputer;
      advanced.ProcessPriority    = _processPriority;

      if (advanced.ShowDialog(this) == DialogResult.OK)
      {
        _abstractRemoteMode = advanced.AbstractRemoteMode;
        _mode               = advanced.Mode;
        _hostComputer       = advanced.HostComputer;
        _processPriority    = advanced.ProcessPriority;
      }
    }
    void ShowHelp()
    {
      try
      {
        string file = Path.Combine(SystemRegistry.GetInstallFolder(), "IR Server Suite.chm");
        Help.ShowHelp(this, file, HelpNavigator.Topic, "Input Service\\index.html");
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }

}
