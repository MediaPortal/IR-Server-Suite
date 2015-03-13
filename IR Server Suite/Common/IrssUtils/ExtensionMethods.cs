
namespace IrssUtils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    public static class ExtensionMethods
    {
        public delegate void InvokeHandler();
        public static void SafeInvoke(this System.Windows.Forms.Control control, InvokeHandler handler)
        {
            if (control.InvokeRequired) control.Invoke(handler);
            else handler();
        }
 
    }
}
