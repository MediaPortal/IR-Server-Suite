using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MceTransceiver
{

  public static class Program
  {

    [STAThread]
    //static void Main(string[] args)
    static void Main()
    {
      //try
      //{

      Console.WriteLine("Attempting to terminate ehtray process ...");
      try
      {
        Process[] processes = Process.GetProcesses();
        foreach (Process proc in processes)
          if (proc.ProcessName.Equals("ehtray", StringComparison.InvariantCultureIgnoreCase))
            proc.Kill();
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed to terminate ehtray process: {0}", ex.Message);
      }

      Console.WriteLine("Connecting to MCE Transceiver ...");

      MceTransceiver tx = new MceTransceiver();

      if (!tx.Start())
        throw new Exception("Failed to start MCE Transceiver.");

      if (!tx.Connected)
        throw new Exception("Connect MCE Transceiver and try again.");

      //Console.WriteLine("Press an MCE remote button to test receiver ...");
      //Console.WriteLine("Received button presses will be retransmitted to test blasting ability.");

      #region Learn IR
      /*
      while (true)
      {

        Console.WriteLine("Learn IR");
        byte[] nativeData = tx.LearnIR(15000);
        //byte[] nativeData = MceTransceiver.ReadIRFile("pronto.IR");

        if (nativeData != null)
        {
          MceTransceiver.WriteIRFile("learned.IR", nativeData);

          Console.WriteLine("IR Learned");

          MceTransceiver.Dump(nativeData);

          Console.WriteLine("Blasting IR");

          tx.Send(nativeData, BlasterPort.Both, BlasterSpeed.None, BlasterType.Microsoft);

          Console.WriteLine("IR Sent");
        }
        else
        {
          Console.WriteLine("Learn IR Failed");
        }

      }
      */
      #endregion Learn IR

      System.Windows.Forms.Application.Run();

      tx.Stop();
      //}
      //catch (Exception ex)
      //{
      //Console.WriteLine(ex.Message);
      //}

      //System.Windows.Forms.MessageBox.Show("Done", "MCE Transceiver");
    }

  }

}
