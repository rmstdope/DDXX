using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IModelFactory
    {
        CustomModel CreateFromName(string file, string effect);
        List<ModelParameters> ModelParameters { get; }
        void Update(ModelParameters Target);
    }
}
