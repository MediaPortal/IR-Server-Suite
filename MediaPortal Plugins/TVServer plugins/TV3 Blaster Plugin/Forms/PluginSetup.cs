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
using System.IO;
using System.Net;
using System.Windows.Forms;
using IrssComms;
using IrssUtils;
using IrssUtils.Forms;
using SetupTv;
using TvDatabase;
using TvLibrary.Log;
using Server = IrssComms.Server;

namespace TvEngine.Plugins.IRSS.TV3BlasterPlugin.Forms
{
  internal partial class PluginSetup : SectionSettings
  {
    #region Variables

    private LearnIR _learnIR;

    #endregion Variables

    #region Constructor

    public PluginSetup()
    {
      InitializeComponent();
    }

    #endregion Constructor

    #region SetupTv.SectionSettings

    public override void OnSectionActivated()
    {
      Log.Info("TV3BlasterPlugin: Configuration activated");

      TV3BlasterPlugin.InConfiguration = true;

      TvBusinessLayer layer = new TvBusinessLayer();
      TV3BlasterPlugin.ServerHost = layer.GetSetting("TV3BlasterPlugin_ServerHost", "localhost").Value;
      IPAddress serverIP = Network.GetIPFromName(TV3BlasterPlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      if (!TV3BlasterPlugin.StartClient(endPoint))
        MessageBox.Show(this, "Failed to start local comms. IR functions temporarily disabled.",
                        "TV3 Blaster Plugin - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

      TV3BlasterPlugin.LoadExternalConfigs();

      RefreshIRList();
      RefreshMacroList();

      TV3BlasterPlugin.HandleMessage += ReceivedMessage;

      base.OnSectionActivated();
    }

    public override void OnSectionDeActivated()
    {
      Log.Info("TV3BlasterPlugin: Configuration deactivated");

      TV3BlasterPlugin.HandleMessage -= ReceivedMessage;

      TvBusinessLayer layer = new TvBusinessLayer();

      Setting setting = layer.GetSetting("TV3BlasterPlugin_ServerHost");
      setting.Value = TV3BlasterPlugin.ServerHost;
      setting.Persist();

      //TV3BlasterPlugin.LoadExternalConfigs();

      TV3BlasterPlugin.StopClient();

      TV3BlasterPlugin.InConfiguration = false;

      base.OnSectionDeActivated();
    }

    #endregion SetupTv.SectionSettings

    #region Implementation

    private void ReceivedMessage(IrssMessage received)
    {
      if (_learnIR != null && received.Type == MessageType.LearnIR)
      {
        if ((received.Flags & MessageFlags.Success) == MessageFlags.Success)
        {
          _learnIR.LearnStatus("Learned IR successfully", true);
        }
        else if ((received.Flags & MessageFlags.Timeout) == MessageFlags.Timeout)
        {
          _learnIR.LearnStatus("Learn IR timed out", false);
        }
        else if ((received.Flags & MessageFlags.Failure) == MessageFlags.Failure)
        {
          _learnIR.LearnStatus("Learn IR failed", false);
        }
      }
    }

    private void RefreshIRList()
    {
      listViewIR.Items.Clear();

      string[] irList = Common.GetIRList(false);
      if (irList != null && irList.Length > 0)
        foreach (string irFile in irList)
          listViewIR.Items.Add(irFile);
    }

    private void RefreshMacroList()
    {
      listViewMacro.Items.Clear();

      string[] macroList = TV3BlasterPlugin.GetMacroList(false);
      if (macroList != null && macroList.Length > 0)
        foreach (string macroFile in macroList)
          listViewMacro.Items.Add(macroFile);
    }

    private void EditIR()
    {
      if (listViewIR.SelectedItems.Count != 1)
        return;

      try
      {
        string command = listViewIR.SelectedItems[0].Text;
        string fileName = Path.Combine(Common.FolderIRCommands, command + Common.FileExtensionIR);

        if (File.Exists(fileName))
        {
          _learnIR = new LearnIR(
            TV3BlasterPlugin.LearnIR,
            TV3BlasterPlugin.BlastIR,
            TV3BlasterPlugin.TransceiverInformation.Ports,
            command);

          _learnIR.ShowDialog(this);

          _learnIR = null;
        }
        else
        {
          RefreshIRList();

          throw new FileNotFoundException("IR file missing", fileName);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
        MessageBox.Show(this, ex.Message, "Failed to edit IR file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void EditMacro()
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      try
      {
        string command = listViewMacro.SelectedItems[0].Text;
        string fileName = TV3BlasterPlugin.PathCombine(command);

        if (File.Exists(fileName))
        {
          MacroEditor macroEditor = new MacroEditor(command);
          macroEditor.ShowDialog(this);
        }
        else
        {
          RefreshMacroList();

          throw new FileNotFoundException("Macro file missing", fileName);
        }
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
        MessageBox.Show(this, ex.Message, "Failed to edit macro", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Implementation

    #region Buttons

    private void buttonHelp_Click(object sender, EventArgs e)
    {
      try
      {
        string file = Path.Combine(SystemRegistry.GetInstallFolder(), "IR Server Suite.chm");
        Help.ShowHelp(this, file, HelpNavigator.Topic, "Plugins\\TV3 Blaster Plugin\\index.html");
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
        MessageBox.Show(this, ex.Message, "Failed to load help", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonSTB_Click(object sender, EventArgs e)
    {
      if (Card.ListAll().Count == 0)
      {
        MessageBox.Show(this, "There are no capture cards installed in the TV server", "No TV Cards",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
      }
      else
      {
        ExternalChannels externalChannels = new ExternalChannels();
        externalChannels.ShowDialog(this);
      }
    }

    private void buttonNewIR_Click(object sender, EventArgs e)
    {
      _learnIR = new LearnIR(
        TV3BlasterPlugin.LearnIR,
        TV3BlasterPlugin.BlastIR,
        TV3BlasterPlugin.TransceiverInformation.Ports);

      _learnIR.ShowDialog(this);

      _learnIR = null;

      RefreshIRList();
    }

    private void buttonEditIR_Click(object sender, EventArgs e)
    {
      EditIR();
    }

    private void buttonDeleteIR_Click(object sender, EventArgs e)
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
          File.Delete(fileName);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "IR file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
      }

      RefreshIRList();
    }

    private void buttonNewMacro_Click(object sender, EventArgs e)
    {
      MacroEditor macroEditor = new MacroEditor();
      macroEditor.ShowDialog(this);

      RefreshMacroList();
    }

    private void buttonEditMacro_Click(object sender, EventArgs e)
    {
      EditMacro();
    }

    private void buttonDeleteMacro_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      string file = listViewMacro.SelectedItems[0].Text;
      string fileName = TV3BlasterPlugin.PathCombine(file);
      if (File.Exists(fileName))
      {
        if (
          MessageBox.Show(this, String.Format("Are you sure you want to delete \"{0}\"?", file), "Confirm delete",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          File.Delete(fileName);
      }
      else
      {
        MessageBox.Show(this, "File not found: " + fileName, "Macro file missing", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
      }

      RefreshMacroList();
    }

    private void buttonTestMacro_Click(object sender, EventArgs e)
    {
      if (listViewMacro.SelectedItems.Count != 1)
        return;

      try
      {
        TV3BlasterPlugin.ProcessCommand(Common.CmdPrefixMacro + listViewMacro.SelectedItems[0].Text, false);
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
        MessageBox.Show(this, ex.Message, "Test failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonHostSetup_Click(object sender, EventArgs e)
    {
      TV3BlasterPlugin.StopClient();

      ServerAddress serverAddress = new ServerAddress(TV3BlasterPlugin.ServerHost);
      serverAddress.ShowDialog(this);

      TV3BlasterPlugin.ServerHost = serverAddress.ServerHost;

      IPAddress serverIP = Network.GetIPFromName(TV3BlasterPlugin.ServerHost);
      IPEndPoint endPoint = new IPEndPoint(serverIP, Server.DefaultPort);

      TV3BlasterPlugin.StartClient(endPoint);
    }

    #endregion Buttons

    #region Other Controls

    private void listViewIR_DoubleClick(object sender, EventArgs e)
    {
      EditIR();
    }

    private void listViewMacro_DoubleClick(object sender, EventArgs e)
    {
      EditMacro();
    }

    private void listViewIR_AfterLabelEdit(object sender, LabelEditEventArgs e)
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

      string oldFileName = Path.Combine(Common.FolderIRCommands, originItem.Text + Common.FileExtensionIR);
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
        string newFileName = Path.Combine(Common.FolderIRCommands, name + Common.FileExtensionIR);

        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

      string oldFileName = TV3BlasterPlugin.PathCombine(originItem.Text);
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
        string newFileName = TV3BlasterPlugin.PathCombine(name);
        File.Move(oldFileName, newFileName);
      }
      catch (Exception ex)
      {
        IrssLog.Error(ex);
        MessageBox.Show(ex.Message, "Failed to rename file", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    #endregion Other Controls
  }
}