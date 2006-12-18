using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

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

        public string Callstack()
        {
            return base.ToString();
        }

        public override string ToString()
        {
            return specificMessage +
                   "\n\nWould you like to see the call stack?";
        }

        public void PresentInMessageBox()
        {
            if (DialogResult.Yes == MessageBox.Show(ToString(), "It seems you are having problems...", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2))
            {
                MessageBox.Show(Callstack(), "Callstack");
            }
        }
    }
}
