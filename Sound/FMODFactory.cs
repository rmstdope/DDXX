using System;
using System.Collections.Generic;
using System.Text;

namespace Sound
{
    class FMODFactory : IFactory
    {
        #region IFactory Members

        public ISystem CreateSystem()
        {
            return new FMODSystem();
        }

        #endregion
    }
}
