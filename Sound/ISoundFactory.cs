using System;
using System.Collections.Generic;
using System.Text;

namespace Sound
{
    public interface ISoundFactory
    {
        ISoundSystem CreateSystem();
    }
}
