using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakableFactory
    {
        ITweakableValue CreateTweakableValue(PropertyInfo property, object target);
        ITweakableObject CreateTweakableObject(object target);
    }
}
