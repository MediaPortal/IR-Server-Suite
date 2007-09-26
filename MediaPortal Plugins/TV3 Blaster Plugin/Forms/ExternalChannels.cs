using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using IrssUtils;

namespace TvEngine
{

  partial class ExternalChannels : Form
  {

    #region Variables

    StbSetup[] _tvCardStbSetups;
    
    #endregion Variables

    #region Constructor

    public ExternalChannels()
    {
      InitializeComponent();
    }

    #endregion Constructor

    private void ExternalChannels_Load(object sender, EventArgs e)
    {
      IList cards = TvDatabase.Card.ListAll();

      _tvCardStbSetups = new StbSetup[cards.Count];

      comboBoxCopyFrom.Items.Clear();

      TabPage tempPage;
      int index = 0;

      foreach (TvDatabase.Card card in cards)
      {
        comboBoxCopyFrom.Items.Add(card.IdCard);

        _tvCardStbSetups[index] = new StbSetup(card.IdCard);
        _tvCardStbSetups[index].Name = String.Format("StbSetup{0}", index);
        _tvCardStbSetups[index].Dock = DockStyle.Fill;
        _tvCardStbSetups[index].TabIndex = 0;

        tempPage = new TabPage(String.Format("TV Card {0}", index + 1));
        tempPage.Controls.Add(_tvCardStbSetups[index]);

        this.tabControlTVCards.TabPages.Add(tempPage);

        index++;
      }

      comboBoxCopyFrom.SelectedIndex = 0;

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
      foreach (StbSetup setup in _tvCardStbSetups)
      {
        setup.Save();
        TV3BlasterPlugin.GetExternalChannelConfig(setup.CardId).Save();
      }

      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonTest_Click(object sender, EventArgs e)
    {
      StbSetup setup = _tvCardStbSetups[tabControlTVCards.SelectedIndex];

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

      try
      {
        int charVal;
        string command;

        for (int repeatCount = 0; repeatCount <= setup.RepeatChannelCommands; repeatCount++)
        {
          if (repeatCount > 0 && setup.RepeatPauseTime > 0)
            Thread.Sleep(setup.RepeatPauseTime);

          if (setup.UsePreChangeCommand && !String.IsNullOrEmpty(setup.PreChangeCommand))
          {
            if (setup.PreChangeCommand.StartsWith(Common.CmdPrefixRun))
              ProcessExternalChannelProgram(setup.PreChangeCommand.Substring(Common.CmdPrefixRun.Length), -1, channel);
            else if (setup.PreChangeCommand.StartsWith(Common.CmdPrefixSerial))
              ProcessSerialCommand(setup.PreChangeCommand.Substring(Common.CmdPrefixSerial.Length), -1, channel);
            else
              TV3BlasterPlugin.ProcessCommand(setup.PreChangeCommand);

            if (setup.PauseTime > 0)
              Thread.Sleep(setup.PauseTime);
          }
          
          foreach (char digit in channel)
          {
            charVal = digit - 48;

            command = setup.Digits[charVal];
            if (!String.IsNullOrEmpty(command))
            {
              if (command.StartsWith(Common.CmdPrefixRun))
                ProcessExternalChannelProgram(command.Substring(Common.CmdPrefixRun.Length), charVal, channel);
              else if (command.StartsWith(Common.CmdPrefixSerial))
                ProcessSerialCommand(command.Substring(Common.CmdPrefixSerial.Length), charVal, channel);
              else
                TV3BlasterPlugin.ProcessCommand(command);

              if (setup.PauseTime > 0)
                Thread.Sleep(setup.PauseTime);
            }
          }

          if (setup.SendSelect && !String.IsNullOrEmpty(setup.SelectCommand))
          {
            if (setup.SelectCommand.StartsWith(Common.CmdPrefixRun))
            {
              ProcessExternalChannelProgram(setup.SelectCommand.Substring(Common.CmdPrefixRun.Length), -1, channel);

              if (setup.DoubleChannelSelect)
              {
                if (setup.PauseTime > 0)
                  Thread.Sleep(setup.PauseTime);

                ProcessExternalChannelProgram(setup.SelectCommand.Substring(Common.CmdPrefixRun.Length), -1, channel);
              }
            }
            else if (setup.SelectCommand.StartsWith(Common.CmdPrefixSerial))
            {
              ProcessSerialCommand(setup.SelectCommand.Substring(Common.CmdPrefixSerial.Length), -1, channel);

              if (setup.DoubleChannelSelect)
              {
                if (setup.PauseTime > 0)
                  Thread.Sleep(setup.PauseTime);

                ProcessSerialCommand(setup.SelectCommand.Substring(Common.CmdPrefixSerial.Length), -1, channel);
              }
            }
            else
            {
              TV3BlasterPlugin.ProcessCommand(setup.SelectCommand);

              if (setup.DoubleChannelSelect)
              {
                if (setup.PauseTime > 0)
                  Thread.Sleep(setup.PauseTime);

                TV3BlasterPlugin.ProcessCommand(setup.SelectCommand);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Failed to test external channel", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void buttonQuickSet_Click(object sender, EventArgs e)
    {
      string quickSetup = comboBoxQuickSetup.Text;

      if (String.IsNullOrEmpty(quickSetup))
        return;

      _tvCardStbSetups[tabControlTVCards.SelectedIndex].SetToXml(quickSetup);
    }

    private void buttonCopyFrom_Click(object sender, EventArgs e)
    {
      _tvCardStbSetups[tabControlTVCards.SelectedIndex].SetToCard((int)comboBoxCopyFrom.SelectedItem);
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    #endregion Buttons

  }

}
