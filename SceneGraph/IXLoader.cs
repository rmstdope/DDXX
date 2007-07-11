using System;
using Dope.DDXX.Graphics;
using System.Collections.Generic;

namespace Dope.DDXX.SceneGraph
{
    public interface IXLoader
    {
        void Load(string filename, IEffect effect, MeshTechniqueChooser techniquePrefix);
        void AddToScene(IScene scene);
        List<INode> GetNodeHierarchy();
        IAnimationRootFrame RootFrame { get; }
    }
}
