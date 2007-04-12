using System;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public interface IXLoader
    {
        void Load(string filename, IEffect effect, MeshTechniqueChooser techniquePrefix);
        void AddToScene(IScene scene);
    }
}
