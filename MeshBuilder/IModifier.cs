using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.MeshBuilder
{
    public interface IModifier
    {
        Primitive Generate();
    }
}
