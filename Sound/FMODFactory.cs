using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Sound
{
    class FMODFactory : ISoundFactory
    {
        #region IFactory Members

        public ISoundSystem CreateSystem()
        {
            return new FMODSystem();
        }

        #endregion
    }
}
