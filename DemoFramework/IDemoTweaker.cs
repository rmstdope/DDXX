using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Input;

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
        void HandleInput(InputDriver inputDriver);
    }
}
