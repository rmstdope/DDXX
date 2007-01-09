using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public class ModelNode : NodeBase, IRenderableMesh
    {
        private IModel model;
        private IEffectHandler effectHandler;

        public IModel Model
        {
            get { return model; }
            set { model = value; }
        }

        public ModelNode(string name, IModel model, IEffectHandler effectHandler) 
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

            model.Draw(effectHandler, scene.AmbientColor, WorldMatrix,
                scene.ActiveCamera.ViewMatrix, scene.ActiveCamera.ProjectionMatrix);

            //for (int j = 0; j < model.Materials.Length; j++)
            //{
            //    effectHandler.SetMaterialConstants(scene.AmbientColor, model.Materials[j], j);

            //    int passes = effectHandler.Effect.Begin(FX.None);

            //    for (int i = 0; i < passes; i++)
            //    {
            //        effectHandler.Effect.BeginPass(i);

            //        model.DrawSubset(j);

            //        effectHandler.Effect.EndPass();
            //    }

            //    effectHandler.Effect.End();
            //}
        }
    }
}
