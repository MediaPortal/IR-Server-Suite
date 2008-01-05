using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LogViewer
{

  public partial class FormMain : Form
  {

    List<string> _files;

    public FormMain()
    {
      InitializeComponent();

      _files = new List<string>();

      AddFile("C:\\1.log");
      AddFile("C:\\2.log");
    }

    private void listViewLines_DragDrop(object sender, DragEventArgs e)
    {
      //AddFile();
    }


    void AddFile(string filename)
    {
      _files.Add(filename);

      RefreshListViewLines();
    }

    void RefreshListViewLines()
    {
      listViewLines.Clear();

      List<StreamReader> streams = new List<StreamReader>(_files.Count);

      List<DateTime> fileTimes = new List<DateTime>(_files.Count);

      foreach (string file in _files)
        streams.Add(new StreamReader(file));

      int eofCount = 0;

      DateTime currentTime = null;
      
      while (eofCount < _files.Count)
      {

        eofCount = 0;
        foreach (StreamReader sr in streams)
        {
          if (sr.EndOfStream)
          {
            eofCount++;
          }
          else
          {
            fileTimes[ = DateTime.TryParse(
          }
        }
      }

      LogLine line;

      foreach (DateTime dateTime in hash.Keys)
      {


        ListViewItem item = new ListViewItem(line.Line);
        item.BackColor = GetColour(line.FileID);

        listViewLines.Items.Add(item);
      }
    }

    Color GetColour(int id)
    {
      switch (id)
      {
        case 0:   return Color.White;
        case 1:   return Color.LightYellow;
        case 2:   return Color.LightBlue;
        case 3:   return Color.LightGreen;
        case 4:   return Color.LightSalmon;
        case 5:   return Color.LightCyan;
        default:  return Color.Indigo;

      }
    }

  }

}
