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
using System.Threading;
using System.Windows.Forms;
using IRServer.Plugin;
using IrssUtils;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;
using Button = SourceGrid.Cells.Button;
using CheckBox = SourceGrid.Cells.CheckBox;
using ColumnHeader = SourceGrid.Cells.ColumnHeader;
using IRServer.Configuration.Properties;
using System.ComponentModel;

namespace IRServer.Configuration
{
  internal partial class Config : Form
  {
    #region Constants

    private const int ColConfigure = 4;
    private const int ColIcon = 0;
    private const int ColName = 1;
    private const int ColReceive = 2;
    private const int ColTransmit = 3;

    #endregion Constants

    #region Variables

    private readonly PluginBase[] _transceivers;

    private bool _abstractRemoteMode;
    private string _hostComputer = String.Empty;
    private IRServerMode _mode = IRServerMode.ServerMode;
    private string _processPriority = String.Empty;
    private BackgroundWorker monitorThread;

    #endregion Variables

    private void Config_Load(object sender, EventArgs e)
    {
      monitorThreadStart();
    }

    private void Config_FormClosing(object sender, FormClosingEventArgs e)
    {
      monitorThreadStop();
    }

    private void monitorThreadStart()
    {
      if (monitorThread == null)
        monitorThread = new BackgroundWorker();
      monitorThread.WorkerReportsProgress = true;
      monitorThread.WorkerSupportsCancellation = true;
      monitorThread.ProgressChanged += new ProgressChangedEventHandler(monitorThread_ProgressChanged);
      monitorThread.DoWork += new DoWorkEventHandler(monitorThread_DoWork);
      monitorThread.RunWorkerAsync();
    }

    private void monitorThreadStop()
    {
      monitorThread.CancelAsync();
    }

    private void monitorThread_DoWork(object sender, EventArgs e)
    {
      do
      {
        Shared.getStatus();
        monitorThread.ReportProgress(0);
        Thread.Sleep(1000);
      }
      while (!monitorThread.CancellationPending);
    }

    private void monitorThread_ProgressChanged(object sender, EventArgs e)
    {
      setButtons();
    }

    private void setButtons()
    {
      if (Shared._serviceInstalled == true)
      {
        toolStripServiceButton.Text = "Uninstall Service";
      }
      else
      {
        toolStripServiceButton.Text = "Install Service";
      }
      switch (Shared._irsStatus)
      {
        case IrsStatus.NotRunning:
          {
            toolStripButtonApplication.Image = IrssUtils.Properties.Resources.Start;
            toolStripButtonApplication.Enabled = true;
            toolStripButtonService.Image = IrssUtils.Properties.Resources.Start;
            toolStripButtonService.Enabled = Shared._serviceInstalled;
            break;
          }
        case IrsStatus.RunningApplication:
          {
            toolStripButtonApplication.Image = IrssUtils.Properties.Resources.Stop;
            toolStripButtonApplication.Enabled = true;
            toolStripButtonService.Image = IrssUtils.Properties.Resources.Start;
            toolStripButtonService.Enabled = false;
            break;
          }
        case IrsStatus.RunningService:
          {
            toolStripButtonApplication.Image = IrssUtils.Properties.Resources.Start;
            toolStripButtonApplication.Enabled = false;
            toolStripButtonService.Image = IrssUtils.Properties.Resources.Stop;
            toolStripButtonService.Enabled = true;
            break;
          }
      }
    }

    private void CreateGrid()
    {
      IrssLog.Info("Creating configuration grid ...");

      try
      {
        int row = 0;

        gridPlugins.Rows.Clear();
        gridPlugins.Columns.SetCount(5);

        // Setup Column Headers
        gridPlugins.Rows.Insert(row);

        ColumnHeader header = new ColumnHeader(" ");
        header.AutomaticSortEnabled = false;
        gridPlugins[row, ColIcon] = header;

        gridPlugins[row, ColName] = new ColumnHeader("Name");
        gridPlugins[row, ColReceive] = new ColumnHeader("Receive");
        gridPlugins[row, ColTransmit] = new ColumnHeader("Transmit");
        gridPlugins[row, ColConfigure] = new ColumnHeader("Configure");
        gridPlugins.FixedRows = 1;

        foreach (PluginBase transceiver in _transceivers)
        {
          gridPlugins.Rows.Insert(++row);
          gridPlugins.Rows[row].Tag = transceiver;

          // Icon Cell
          if (transceiver.DeviceIcon != null)
          {
            Image iconCell = new Image(transceiver.DeviceIcon);
            iconCell.Editor.EnableEdit = false;

            gridPlugins[row, ColIcon] = iconCell;
          }
          else
          {
            gridPlugins[row, ColIcon] = new Cell();
          }

          // Name Cell
          Cell nameCell = new Cell(transceiver.Name);

          CustomEvents nameCellController = new CustomEvents();
          nameCellController.DoubleClick += PluginDoubleClick;
          nameCell.AddController(nameCellController);

          nameCell.AddController(new ToolTipText());
          nameCell.ToolTipText = String.Format("{0}\nVersion: {1}\nAuthor: {2}\n{3}", transceiver.Name,
                                               transceiver.Version, transceiver.Author, transceiver.Description);

          gridPlugins[row, ColName] = nameCell;

          // Receive Cell
          if (transceiver is IRemoteReceiver || transceiver is IMouseReceiver || transceiver is IKeyboardReceiver)
          {
            gridPlugins[row, ColReceive] = new CheckBox();
          }
          else
          {
            gridPlugins[row, ColReceive] = new Cell();
          }

          // Transmit Cell
          if (transceiver is ITransmitIR)
          {
            CheckBox checkbox = new CheckBox();

            CustomEvents checkboxcontroller = new CustomEvents();
            checkboxcontroller.ValueChanged += TransmitChanged;
            checkbox.Controller.AddController(checkboxcontroller);

            gridPlugins[row, ColTransmit] = checkbox;
          }
          else
          {
            gridPlugins[row, ColTransmit] = new Cell();
          }

          // Configure Cell
          if (transceiver is IConfigure)
          {
            Button button = new Button("Configure");

            SourceGrid.Cells.Controllers.Button buttonClickEvent = new SourceGrid.Cells.Controllers.Button();
            buttonClickEvent.Executed += buttonClickEvent_Executed;
            button.Controller.AddController(buttonClickEvent);

            gridPlugins[row, ColConfigure] = button;
          }
          else
          {
            gridPlugins[row, ColConfigure] = new Cell();
          }
        }

        gridPlugins.Columns[ColIcon].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
        gridPlugins.Columns[ColName].AutoSizeMode = SourceGrid.AutoSizeMode.Default;
        gridPlugins.Columns[ColReceive].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
        gridPlugins.Columns[ColTransmit].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
        gridPlugins.Columns[ColConfigure].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize;
        gridPlugins.AutoStretchColumnsToFitWidth = true;
        gridPlugins.AutoSizeCells();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(this, ex.ToString(), "Error setting up plugin grid", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void Detect()
    {
      IrssLog.Info("Attempting to detect Input Plugins ...");

      CheckBox checkBox;
      for (int row = 1; row < gridPlugins.RowsCount; row++)
      {
        PluginBase plugin = gridPlugins.Rows[row].Tag as PluginBase;

        try
        {
          if (plugin == null)
            throw new InvalidOperationException(String.Format("Invalid grid data, row {0} contains no plugin in tag",
                                                              row));

          PluginBase.DetectionResult detected = plugin.Detect();

          if (detected == PluginBase.DetectionResult.DevicePresent)
          {
            IrssLog.Info("Plugin {0}: detected", plugin.Name);
          }
          if (detected == PluginBase.DetectionResult.DeviceException)
          {
            IrssLog.Warn("Plugin {0}: exception during Detect()", plugin.Name);
          }

          // Receive
          checkBox = gridPlugins[row, ColReceive] as CheckBox;
          if (checkBox != null)
            checkBox.Checked = (detected == PluginBase.DetectionResult.DevicePresent ? true : false);

          // Transmit
          checkBox = gridPlugins[row, ColTransmit] as CheckBox;
          if (checkBox != null)
            checkBox.Checked = (detected == PluginBase.DetectionResult.DevicePresent ? true : false);
        }
        catch (Exception ex)
        {
          IrssLog.Error(ex);
        }
      }

      IrssLog.Info("Input Plugins detection completed...");
    }

    private void Advanced()
    {
      IrssLog.Info("Entering advanced configuration ...");

      Advanced advanced = new Advanced();

      advanced.AbstractRemoteMode = _abstractRemoteMode;
      advanced.Mode = _mode;
      advanced.HostComputer = _hostComputer;
      advanced.ProcessPriority = _processPriority;

      if (advanced.ShowDialog(this) == DialogResult.OK)
      {
        _abstractRemoteMode = advanced.AbstractRemoteMode;
        _mode = advanced.Mode;
        _hostComputer = advanced.HostComputer;
        _processPriority = advanced.ProcessPriority;
      }
    }

    #region Properties

    public string[] PluginReceive
    {
      get
      {
        List<string> receivers = new List<string>();

        CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, ColReceive] as CheckBox;
          if (checkBox != null && checkBox.Checked == true)
            receivers.Add(gridPlugins[row, ColName].DisplayText);
        }

        if (receivers.Count == 0)
          return null;

        return receivers.ToArray();
      }
      set
      {
        CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, ColReceive] as CheckBox;
          if (checkBox == null)
            continue;

          if (value == null)
            checkBox.Checked = false;
          else if (Array.IndexOf(value, gridPlugins[row, ColName].DisplayText) != -1)
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
        CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, ColTransmit] as CheckBox;
          if (checkBox != null && checkBox.Checked == true)
            return gridPlugins[row, ColName].DisplayText;
        }

        return String.Empty;
      }
      set
      {
        if (String.IsNullOrEmpty(value))
          return;

        CheckBox checkBox;
        for (int row = 1; row < gridPlugins.RowsCount; row++)
        {
          checkBox = gridPlugins[row, ColTransmit] as CheckBox;
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

      try
      {
        _transceivers = BasicFunctions.AvailablePlugins();
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);

        _transceivers = null;
      }

      if (_transceivers == null || _transceivers.Length == 0)
        MessageBox.Show(this, "No IR Server Plugins found!", "IR Server Configuration", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
      else
        CreateGrid();


      LoadSettings();
    }

    #endregion Constructor

    #region Controls

    private void buttonOK_Click(object sender, EventArgs e)
    {
      SaveSettings();
      Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void buttonClickEvent_Executed(object sender, EventArgs e)
    {
      CellContext context = (CellContext)sender;
      Button cell = (Button)context.Cell;

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
      CellContext context = (CellContext)sender;
      CheckBox cell = (CheckBox)context.Cell;

      if (cell.Checked != true)
        return;

      PluginBase plugin = cell.Row.Tag as PluginBase;
      if (plugin == null)
        return;

      for (int row = 1; row < gridPlugins.RowsCount; row++)
      {
        CheckBox checkBox = gridPlugins[row, ColTransmit] as CheckBox;

        if (checkBox != null && checkBox.Checked == true &&
            !gridPlugins[row, ColName].DisplayText.Equals(plugin.Name, StringComparison.OrdinalIgnoreCase))
          checkBox.Checked = false;
      }
    }

    private void PluginDoubleClick(object sender, EventArgs e)
    {
      CellContext context = (CellContext)sender;
      Cell cell = (Cell)context.Cell;

      CheckBox checkBoxReceive = gridPlugins[cell.Row.Index, ColReceive] as CheckBox;
      if (checkBoxReceive != null)
        checkBoxReceive.Checked = true;

      CheckBox checkBoxTransmit = gridPlugins[cell.Row.Index, ColTransmit] as CheckBox;
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
      IrssHelp.Open(this);
    }

    private void Config_HelpRequested(object sender, HelpEventArgs hlpevent)
    {
      IrssHelp.Open(sender);
    }

    private void toolStripServiceButton_Click(object sender, EventArgs e)
    {
      toolStripServiceButton.Enabled = false;
      if (Shared._serviceInstalled == true)
        Program.ServiceUninstall();
      else
        Program.ServiceInstall();

      Shared.getStatus();
      setButtons();
      toolStripServiceButton.Enabled = true;
    }

    private void toolStripButtonService_Click(object sender, EventArgs e)
    {
      if (Shared._irsStatus == IrsStatus.RunningService)
        Program.ServiceStop();
      if (Shared._irsStatus == IrsStatus.NotRunning)
        Program.ServiceStart();
    }

    private void toolStripButtonApplication_Click(object sender, EventArgs e)
    {
      if (Shared._irsStatus == IrsStatus.RunningApplication)
        Program.ApplicationStop();
      if (Shared._irsStatus == IrsStatus.NotRunning)
        Program.ApplicationStart();
    }

    #endregion Controls

    #region Settings

    private void LoadSettings()
    {
      _abstractRemoteMode = Settings.AbstractRemoteMode;
      _mode = Settings.Mode;
      _hostComputer = Settings.HostComputer;
      _processPriority = Settings.ProcessPriority;
      PluginReceive = Settings.PluginNameReceive;
      PluginTransmit = Settings.PluginNameTransmit;
    }

    private void SaveSettings()
    {
      if ((Settings.AbstractRemoteMode != _abstractRemoteMode) ||
          (Settings.Mode != _mode) ||
          (Settings.HostComputer != _hostComputer) ||
          (Settings.ProcessPriority != _processPriority) ||
          (Settings.PluginNameReceive != PluginReceive) ||
          (Settings.PluginNameTransmit != PluginTransmit))
      {
        if (
          MessageBox.Show("IR Server will now be restarted for configuration changes to take effect",
                          "Restarting IR Server", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) ==
          DialogResult.OK)
        {
          // Change settings ...
          Settings.AbstractRemoteMode = _abstractRemoteMode;
          Settings.Mode = _mode;
          Settings.HostComputer = _hostComputer;
          Settings.ProcessPriority = _processPriority;
          Settings.PluginNameReceive = PluginReceive;
          Settings.PluginNameTransmit = PluginTransmit;

          Settings.SaveSettings();

          // Restart IR Server ...
          Program.RestartIRS();
        }
        else
        {
          IrssLog.Info("Canceled settings changes");
        }
      }
    }

    #endregion
  }
}