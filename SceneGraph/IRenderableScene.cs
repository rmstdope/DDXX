using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.SceneGraph
{
    public interface IRenderableScene
    {
        ICamera ActiveCamera
        { 
            get;
            set;
        }
    }
}
