using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public class DDXXException : Exception
    {
        public DDXXException(string message)
            : base(message)
        {
        }
    }
}
