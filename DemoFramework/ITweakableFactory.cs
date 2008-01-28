using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakableFactory
    {
        ITweakable CreateTweakableValue(PropertyInfo property, object target);
        ITweakable CreateTweakableObject(object target);
        ITextureFactory TextureFactory { get; }
    }
}
