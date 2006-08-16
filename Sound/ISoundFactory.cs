using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Sound
{
    public interface ISoundFactory
    {
        ISoundSystem CreateSystem();
    }
}
