using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public interface IRegisterable
    {
        string Name
        {
            get;
        }

        float StartTime
        {
            get;
            set;
        }

        float EndTime
        {
            get;
            set;
        }
    }
}
