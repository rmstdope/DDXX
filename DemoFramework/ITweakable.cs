using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakable
    {
        void CreateVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings);
    }
}
