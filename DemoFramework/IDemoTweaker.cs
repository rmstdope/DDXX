using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoTweaker
    {
        bool Enabled
        {
            get;
            set;
        }
        void Initialize(IDemoRegistrator registrator);
        void Draw();
    }
}
