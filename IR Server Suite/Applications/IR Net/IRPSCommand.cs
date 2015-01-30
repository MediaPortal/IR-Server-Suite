using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;  // Windows PowerShell assembly.


namespace IRNet
{
    // Declare the class as a cmdlet and specify an 
    // appropriate verb and noun for the cmdlet name.
    [Cmdlet(VerbsCommunications.Send, "Greeting")]
    public class SendGreetingCommand : Cmdlet
    {
        // Declare the parameters for the cmdlet.
        [Parameter(Mandatory = true)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string name;

        // Overide the ProcessRecord method to process
        // the supplied user name and write out a 
        // greeting to the user by calling the WriteObject
        // method.
        protected override void ProcessRecord()
        {
            WriteObject("Hello " + name + "!");
        }
    }
}
