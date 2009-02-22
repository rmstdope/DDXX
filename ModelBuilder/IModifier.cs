using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;

namespace Dope.DDXX.ModelBuilder
{
    public interface IModifier
    {
        void ConnectToInput(int inputPin, IModifier outputGenerator);
        IPrimitive Generate();
        IModifier GetInputModifier(int index);
    }
}
