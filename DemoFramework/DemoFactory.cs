using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public class DemoFactory : IDemoFactory
    {
        public ITrack CreateTrack()
        {
            return new Track();
        }
    }
}
