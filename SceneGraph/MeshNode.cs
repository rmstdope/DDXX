using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public class MeshNode : NodeBase
    {
        private Model model;
        private EffectContainer effect;

        public MeshNode(string name, Model model) 
            : base(name)
        {
            this.model = model;
        }

        public EffectContainer EffectTechnique
        {
            set { effect = value; }
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(CameraNode camera)
        {
            if (effect == null)
                throw new DDXXException("MeshNode \"" + Name + "\" does not have an effect set.");

            for (int j = 0; j < model.GetMaterials().Length; j++)
            {
                int passes = effect.Effect.Begin(FX.None);

                for (int i = 0; i < passes; i++)
                {
                    effect.Effect.BeginPass(i);

                    model.IMesh.DrawSubset(j);

                    effect.Effect.EndPass();
                }

                effect.Effect.End();
            }
        }
    }
}
