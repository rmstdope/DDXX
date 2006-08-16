using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public class MeshNode : NodeBase
    {
        private Dope.DDXX.Graphics.MeshContainer mesh;
        private EffectContainer effect;

        public MeshNode(string name, Dope.DDXX.Graphics.MeshContainer mesh) 
            : base(name)
        {
            this.mesh = mesh;
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

            for (int j = 0; j < mesh.EffectInstance.Length; j++)
            {
                int passes = effect.Effect.Begin(FX.None);

                for (int i = 0; i < passes; i++)
                {
                    effect.Effect.BeginPass(i);

                    mesh.Mesh.DrawSubset(j);

                    effect.Effect.EndPass();
                }

                effect.Effect.End();
            }
        }
    }
}
