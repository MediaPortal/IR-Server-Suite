using System;

namespace LogViewer
{

  class LogLine
  {
    int _fileID;
    string _line;

    public int FileID
    {
      get { return _fileID; }
      set { _fileID = value; }
    }

    public string Line
    {
      get { return _line; }
      set { _line = value; }
    }

    public LogLine(int fileID, string line)
    {
      _fileID = fileID;
      _line = line;
    }

  }

}
