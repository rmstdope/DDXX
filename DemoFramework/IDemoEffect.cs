using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFramework
{
    public interface IDemoEffect
    {
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

        void Step();

        void Render();
    }
}
