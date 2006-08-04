using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFramework
{
    public interface IEffect
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
