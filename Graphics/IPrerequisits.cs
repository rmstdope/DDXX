using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface IPrerequisits
    {
        void CheckPrerequisits(int adapter, DeviceType deviceType);
    }
}
