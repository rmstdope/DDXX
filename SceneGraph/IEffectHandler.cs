using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public interface IEffectHandler
    {
        IEffect Effect { get; }
        EffectHandle Technique { get; }
        void SetNodeConstants(IRenderableScene scene, INode node);
        void SetMaterialConstants(IRenderableScene scene, ModelMaterial mesh);
    }
}
