using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public class MeshNode : NodeBase, IRenderableMesh
    {
        private Model model;
        private IEffectHandler effectHandler;

        public MeshNode(string name, Model model, IEffectHandler effectHandler) 
            : base(name)
        {
            this.model = model;
            this.effectHandler = effectHandler;
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(IRenderableScene scene)
        {
            effectHandler.SetMeshConstants(scene, this);

            for (int j = 0; j < model.Materials.Length; j++)
            {
                effectHandler.SetMaterialConstants(scene, model.Materials[j]);

                int passes = effectHandler.Effect.Begin(FX.None);

                for (int i = 0; i < passes; i++)
                {
                    effectHandler.Effect.BeginPass(i);

                    model.IMesh.DrawSubset(j);

                    effectHandler.Effect.EndPass();
                }

                effectHandler.Effect.End();
            }
        }
    }
}
