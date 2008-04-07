using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
#if TRACE
using System.Diagnostics;
#endif
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using IrssUtils;

namespace MediaCenterBlaster
{

  partial class ExternalChannels : Form
  {

    #region Variables

    TabPage _tvCardTab;
    StbSetup _tvCardStbSetup;
    
    #endregion Variables

    #region Constructor

    public ExternalChannels()
    {
      InitializeComponent();
    }

    #endregion Constructor

    private void ExternalChannels_Load(object sender, EventArgs e)
    {
      _tvCardStbSetup = new StbSetup(0);
      _tvCardStbSetup.Name = "StbSetup";
      _tvCardStbSetup.Dock = DockStyle.Fill;

      _tvCardTab = new TabPage("STB");
      _tvCardTab.Controls.Add(_tvCardStbSetup);

      this.tabControlTVCards.TabPages.Add(_tvCardTab);

      // Setup quick setup combo box
      string[] quickSetupFiles = Directory.GetFiles(Common.FolderSTB, "*.xml", SearchOption.TopDirectoryOnly);
      foreach (string file in quickSetupFiles)
        comboBoxQuickSetup.Items.Add(Path.GetFileNameWithoutExtension(file));

      comboBoxQuickSetup.Items.Add("Clear all");
    }

    static void ProcessExternalChannelProgram(string runCommand, int currentChannelDigit, string fullChannelString)
    {
      string[] commands = Common.SplitRunCommand(runCommand);

      commands[2] = commands[2].Replace("%1", currentChannelDigit.ToString());
      commands[2] = commands[2].Replace("%2", fullChannelString);

      Common.ProcessRunCommand(commands);
    }

    static void ProcessSerialCommand(string serialCommand, int currentChannelDigit, string fullChannelString)
    {
      string[] commands = Common.SplitSerialCommand(serialCommand);

      commands[0] = commands[0].Replace("%1", currentChannelDigit.ToString());
      commands[0] = commands[0].Replace("%2", fullChannelString);

      Common.ProcessSerialCommand(commands);
    }

    #region Buttons

    private void buttonOK_Click(object sender, EventArgs e)
    {
      try
      {
        _tvCardStbSetup.Save();
        Tray.ExtChannelConfig.Save();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "Failed to save external channel setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      try
      {
        StbSetup setup = _tvCardStbSetup;

        int channelTest = Decimal.ToInt32(numericUpDownTest.Value);
        string channel;
        switch (setup.ChannelDigits)
        {
          case 2:
            channel = channelTest.ToString("00");
            break;

          case 3:
            channel = channelTest.ToString("000");
            break;

          case 4:
            channel = channelTest.ToString("0000");
            break;

          default:
            channel = channelTest.ToString();
            break;
        }

        int charVal;
        string command;

        for (int repeatCount = 0; repeatCount <= setup.RepeatChannelCommands; repeatCount++)
        {
          if (repeatCount > 0 && setup.RepeatPauseTime > 0)
            Thread.Sleep(setup.RepeatPauseTime);

          if (setup.UsePreChangeCommand && !String.IsNullOrEmpty(setup.PreChangeCommand))
          {
            Tray.ProcessExternalCommand(setup.PreChangeCommand, -1, channel);

            if (setup.PauseTime > 0)
              Thread.Sleep(setup.PauseTime);
          }
          
          foreach (char digit in channel)
          {
            charVal = digit - 48;

            command = setup.Digits[charVal];
            if (!String.IsNullOrEmpty(command))
            {
              Tray.ProcessExternalCommand(command, charVal, channel);

              if (setup.PauseTime > 0)
                Thread.Sleep(setup.PauseTime);
            }
          }

          if (setup.SendSelect && !String.IsNullOrEmpty(setup.SelectCommand))
          {
            Tray.ProcessExternalCommand(setup.SelectCommand, -1, channel);

            if (setup.DoubleChannelSelect)
            {
              if (setup.PauseTime > 0)
                Thread.Sleep(setup.PauseTime);

              Tray.ProcessExternalCommand(setup.SelectCommand, -1, channel);
            }
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "Failed to test external channel", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonQuickSet_Click(object sender, EventArgs e)
    {
      string quickSetup = comboBoxQuickSetup.Text;

      if (String.IsNullOrEmpty(quickSetup))
        return;

      try
      {
        _tvCardStbSetup.SetToXml(quickSetup);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "Failed to quick-set external channel setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    #endregion Buttons

  }

}
