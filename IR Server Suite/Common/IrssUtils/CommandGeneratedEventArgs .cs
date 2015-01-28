using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IrssUtils
{
    public class CommandGeneratedEventArgs : EventArgs
    {
        /// <summary>
        /// If true, this is a test-command request (command should be executed);
        /// If false, this is a insert-command request (command to inserted to the macro/script)
        /// </summary>
        public bool test;

        /// <summary>
        /// IRSS command and its arguments
        /// </summary>
        public string command;
    }
}
