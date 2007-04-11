using System;
using Dope.DDXX.Graphics.Skinning;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public delegate MaterialTechniqueChooser MeshTechniqueChooser(string meshName);
    public interface INodeFactory
    {
        CameraNode CreateCameraNode(IFrame frame);
        DummyNode CreateDummyNode(IFrame frame);
        ModelNode CreateModelNode(IFrame frame, IEffect effect, string prefix);
        ModelNode CreateSkinnedModelNode(IAnimationRootFrame animationRootFrame, IFrame frame, IEffect effect, string prefix);
    }
}
