using System;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public interface IXLoader
    {
        void Load(string filename, IEffect effect, string techniquePrefix);
        void AddToScene(IScene scene);
    }
}
