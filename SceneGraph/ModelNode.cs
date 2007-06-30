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
        private IDevice device;

        public IModel Model
        {
            get { return model; }
            set { model = value; }
        }

        public IEffectHandler EffectHandler
        {
            get { return effectHandler; }
        }

        public ModelNode(string name, IModel model, IEffectHandler effectHandler, IDevice device) 
            : base(name)
        {
            this.model = model;
            this.effectHandler = effectHandler;
            this.device = device;
        }

        protected override void StepNode()
        {
            model.Step();
        }

        protected override void RenderNode(IScene scene)
        {
            model.Render(device, effectHandler, scene.AmbientColor, WorldMatrix,
                scene.ActiveCamera.ViewMatrix, scene.ActiveCamera.ProjectionMatrix);
        }
    }
}
