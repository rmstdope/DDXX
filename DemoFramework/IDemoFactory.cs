using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoFactory
    {
        ITrack CreateTrack();
    }
}
