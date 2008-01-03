using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Sound
{
    public interface ISoundFactory
    {
        void Initialize(string resourceName);
        ICue CreateCue(string name);

        void Step();
    }
}
