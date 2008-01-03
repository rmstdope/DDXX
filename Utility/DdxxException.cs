using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Dope.DDXX.Utility
{
    public class DDXXException : Exception
    {
        private string specificMessage;

        public DDXXException(string message)
            : base()
        {
            specificMessage = message;
        }

        public override string ToString()
        {
            return "A DDXX exception has occured:\n\"" + specificMessage + "\"\n\n" + base.ToString();
        }

        //public string Callstack()
        //{
        //    return base.ToString();
        //}

        //public override string ToString()
        //{
        //    return specificMessage +
        //           "\n\nWould you like to see the call stack?";
        //}

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);
        
        //public void PresentInMessageBox()
        //{
        //    MessageBox(new IntPtr(0), Callstack(), "Problem", 0);
        //    //if (DialogResult.Yes == MessageBox.Show(ToString(), "It seems you are having problems...", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2))
        //    //{
        //    //    MessageBox.Show(Callstack(), "Callstack");
        //    //}
        //}
    }
}
