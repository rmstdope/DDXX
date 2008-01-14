using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakableValue : ITweakable
    {
        int Dimension { get; }
        void IncreaseValue(int index);
        void DecreaseValue(int index);
        void SetFromInputString(TweakerStatus status);
    }
}
