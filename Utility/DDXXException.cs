using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
