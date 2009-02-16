using System;
using System.Collections.Generic;

namespace Dope.DDXX.Graphics
{
    public interface IModelFactory
    {
        IModel CreateFromName(string file, string effect);
        List<ModelParameters> ModelParameters { get; }
        void Update(ModelParameters Target);
    }
}
