using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;

using IRServerPluginInterface;
using IrssUtils;

namespace IRServer
{

  public partial class Config : Form
  {

    #region Variables

    IRServerPlugin[] _transceivers;

    #endregion Variables

    #region Properties

    public IRServerMode Mode
    {
      get
      {
        if (radioButtonRelay.Checked)
          return IRServerMode.RelayMode;
        else if (radioButtonRepeater.Checked)
          return IRServerMode.RepeaterMode;
        else
          return IRServerMode.ServerMode;
      }
      set
      {
        switch (value)
        {
          case IRServerMode.ServerMode:
            radioButtonServer.Checked = true;
            break;

          case IRServerMode.RelayMode:
            radioButtonRelay.Checked = true;
            break;

          case IRServerMode.RepeaterMode:
            radioButtonRepeater.Checked = true;
            break;
        }
      }
    }
    public string HostComputer
    {
      get { return comboBoxComputer.Text; }
      set { comboBoxComputer.Text = value; }
    }
    public string PluginReceive
    {
      get
      {
        SourceGrid.Cells.CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, 1] as SourceGrid.Cells.CheckBox;
          if (checkBox != null && checkBox.Checked)
            return gridPlugins[row, 0].DisplayText;
        }

        return String.Empty;
      }
      set
      {
        SourceGrid.Cells.CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, 1] as SourceGrid.Cells.CheckBox;
          if (checkBox == null)
            continue;

          if (gridPlugins[row, 0].DisplayText.Equals(value, StringComparison.InvariantCultureIgnoreCase))
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
          checkBox = gridPlugins[row, 2] as SourceGrid.Cells.CheckBox;
          if (checkBox != null && checkBox.Checked)
            return gridPlugins[row, 0].DisplayText;
        }

        return String.Empty;
      }
      set
      {
        SourceGrid.Cells.CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, 2] as SourceGrid.Cells.CheckBox;
          if (checkBox == null)
            continue;

          if (gridPlugins[row, 0].DisplayText.Equals(value, StringComparison.InvariantCultureIgnoreCase))
            checkBox.Checked = true;
          else
            checkBox.Checked = false;
        }
      }
    }

    #endregion Properties

    #region Constructor

    public Config()
    {
      InitializeComponent();

      // Add transceivers to list ...
      _transceivers = Program.AvailablePlugins();
      if (_transceivers == null || _transceivers.Length == 0)
      {
        MessageBox.Show(this, "No IR Server Plugins found!", "IR Server Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      else
      {
        CreateGrid();
      }

      try
      {
        checkBoxRunAtBoot.Checked = SystemRegistry.GetAutoRun("IR Server");
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex.ToString());
      }

      ArrayList networkPCs = IrssUtils.Win32.GetNetworkComputers();
      if (networkPCs != null)
      {
        foreach (string computer in networkPCs.ToArray(typeof(string)))
          if (computer != Environment.MachineName)
            comboBoxComputer.Items.Add(computer);
      }
    }

    #endregion Constructor

    void CreateGrid()
    {
      int row = 0;
      gridPlugins.Rows.Clear();

      gridPlugins.Columns.SetCount(4);

      gridPlugins.Rows.Insert(row);
      SourceGrid.Cells.ColumnHeader headerCell;

      headerCell = new SourceGrid.Cells.ColumnHeader("Name");
      gridPlugins[row, 0] = headerCell;

      headerCell = new SourceGrid.Cells.ColumnHeader("Receive");
      gridPlugins[row, 1] = headerCell;

      headerCell = new SourceGrid.Cells.ColumnHeader("Transmit");
      gridPlugins[row, 2] = headerCell;

      headerCell = new SourceGrid.Cells.ColumnHeader("Configure");
      gridPlugins[row, 3] = headerCell;

      gridPlugins.FixedRows = 1;

      row++;

      foreach (IRServerPlugin transceiver in _transceivers)
      {
        gridPlugins.Rows.Insert(row);

        SourceGrid.Cells.Cell nameCell = new SourceGrid.Cells.Cell(transceiver.Name);

        SourceGrid.Cells.Controllers.CustomEvents nameCellController = new SourceGrid.Cells.Controllers.CustomEvents();
        nameCellController.DoubleClick += new EventHandler(PluginDoubleClick);
        nameCell.AddController(nameCellController);

        nameCell.AddController(new SourceGrid.Cells.Controllers.ToolTipText());
        nameCell.ToolTipText = string.Format("{0}\nVersion: {1}\nAuthor: {2}\n{3}", transceiver.Name, transceiver.Version, transceiver.Author, transceiver.Description);

        gridPlugins[row, 0] = nameCell;

        if (transceiver is IRemoteReceiver)
        {
          SourceGrid.Cells.CheckBox checkbox = new SourceGrid.Cells.CheckBox();

          SourceGrid.Cells.Controllers.CustomEvents checkboxcontroller = new SourceGrid.Cells.Controllers.CustomEvents();
          checkboxcontroller.ValueChanged += new EventHandler(ReceiveChanged);
          checkbox.Controller.AddController(checkboxcontroller);

          gridPlugins[row, 1] = checkbox;
        }
        else
          gridPlugins[row, 1] = new SourceGrid.Cells.Cell();

        if (transceiver is ITransmitIR)
        {
          SourceGrid.Cells.CheckBox checkbox = new SourceGrid.Cells.CheckBox();

          SourceGrid.Cells.Controllers.CustomEvents checkboxcontroller = new SourceGrid.Cells.Controllers.CustomEvents();
          checkboxcontroller.ValueChanged += new EventHandler(TransmitChanged);
          checkbox.Controller.AddController(checkboxcontroller);

          gridPlugins[row, 2] = checkbox;
        }
        else
          gridPlugins[row, 2] = new SourceGrid.Cells.Cell();

        if (transceiver is IConfigure)
        {
          SourceGrid.Cells.Button button = new SourceGrid.Cells.Button("Configure");

          SourceGrid.Cells.Controllers.Button buttonClickEvent = new SourceGrid.Cells.Controllers.Button();
          buttonClickEvent.Executed += new EventHandler(buttonClickEvent_Executed);
          button.Controller.AddController(buttonClickEvent);

          gridPlugins[row, 3] = button;
        }
        else
          gridPlugins[row, 3] = new SourceGrid.Cells.Cell();

        row++;
      }

      gridPlugins.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.Default;
      gridPlugins.Columns[1].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
      gridPlugins.Columns[2].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
      gridPlugins.Columns[3].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
      gridPlugins.AutoStretchColumnsToFitWidth = true;
      gridPlugins.AutoSizeCells();
    }

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
        IrssLog.Error(ex.ToString());
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

      string plugin = gridPlugins[cell.Row.Index, 0].DisplayText;

      foreach (IRServerPlugin transceiver in _transceivers)
        if (transceiver.Name == plugin)
          (transceiver as IConfigure).Configure();
    }

    private void ReceiveChanged(object sender, EventArgs e)
    {
      SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
      SourceGrid.Cells.CheckBox cell = (SourceGrid.Cells.CheckBox)context.Cell;

      if (!cell.Checked)
        return;

      string plugin = gridPlugins[cell.Row.Index, 0].DisplayText;

      for (int row = 1; row < gridPlugins.RowsCount; row++)
      {
        SourceGrid.Cells.CheckBox checkBox = gridPlugins[row, 1] as SourceGrid.Cells.CheckBox;
        if (checkBox != null && checkBox.Checked && !gridPlugins[row, 0].DisplayText.Equals(plugin, StringComparison.InvariantCultureIgnoreCase))
          checkBox.Checked = false;
      }
    }
    private void TransmitChanged(object sender, EventArgs e)
    {
      SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
      SourceGrid.Cells.CheckBox cell = (SourceGrid.Cells.CheckBox)context.Cell;

      if (!cell.Checked)
        return;

      string plugin = gridPlugins[cell.Row.Index, 0].DisplayText;

      for (int row = 1; row < gridPlugins.RowsCount; row++)
      {
        SourceGrid.Cells.CheckBox checkBox = gridPlugins[row, 2] as SourceGrid.Cells.CheckBox;
        if (checkBox != null && checkBox.Checked && !gridPlugins[row, 0].DisplayText.Equals(plugin, StringComparison.InvariantCultureIgnoreCase))
          checkBox.Checked = false;
      }
    }
    private void PluginDoubleClick(object sender, EventArgs e)
    {
      SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
      SourceGrid.Cells.Cell cell = (SourceGrid.Cells.Cell)context.Cell;

      SourceGrid.Cells.CheckBox checkBoxReceive   = gridPlugins[cell.Row.Index, 1] as SourceGrid.Cells.CheckBox;
      if (checkBoxReceive != null)
        checkBoxReceive.Checked = true;

      SourceGrid.Cells.CheckBox checkBoxTransmit  = gridPlugins[cell.Row.Index, 2] as SourceGrid.Cells.CheckBox;
      if (checkBoxTransmit != null)
        checkBoxTransmit.Checked = true;
    }

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      try
      {
        Help.ShowHelp(this, SystemRegistry.GetInstallFolder() + "\\IR Server Suite.chm", HelpNavigator.Topic, "IR Server\\index.html");
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void radioButtonServer_CheckedChanged(object sender, EventArgs e)
    {
      comboBoxComputer.Enabled = false;
    }
    private void radioButtonRelay_CheckedChanged(object sender, EventArgs e)
    {
      comboBoxComputer.Enabled = true;
    }
    private void radioButtonRepeater_CheckedChanged(object sender, EventArgs e)
    {
      comboBoxComputer.Enabled = true;
    }

    #endregion Controls

  }

}
