using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.UserInterface;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakableFactory
    {
        ITweakableProperty CreateTweakableValue(PropertyInfo property, object target);
        ITweakable CreateTweakableObject(object target);
        IGraphicsFactory GraphicsFactory { get; }
        IDemoEffectTypes EffectTypes { get; }
        IMenuControl<T> CreateMenuControl<T>();
    }
}
