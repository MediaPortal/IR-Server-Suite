using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace IrssScheduler
{

  /// <summary>
  /// 
  /// </summary>
  public class Scheduler
  {

    List<ScheduleEvent> _scheduleEvents;

    /// <summary>
    /// Initializes a new instance of the <see cref="Scheduler"/> class.
    /// </summary>
    public Scheduler()
    {

      _scheduleEvents = new List<ScheduleEvent>();

    }


    public void LoadSchedule(string fileName)
    {

    }

    public void SaveSchedule(string fileName)
    {

    }


  }

}
