using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public class ModelNode : NodeBase, IModelNode
    {
        private CustomModel model;
        private LightState lightState;
        private GraphicsDevice device;
        private RasterizerState rasterizerState;
        private DepthStencilState depthStencilState;

        public CustomModel Model
        {
            get { return model; }
            set { model = value; }
        }

        public RasterizerState RasterizerState
        {
            get { return rasterizerState; }
            set { rasterizerState = value; }
        }

        public DepthStencilState DepthStencilState
        {
            get { return depthStencilState; }
            set { depthStencilState = value; }
        }

        public ModelNode(string name, CustomModel model, GraphicsDevice device) 
            : base(name)
        {
            this.model = model;
            this.device = device;
            this.rasterizerState = new RasterizerState();
            this.depthStencilState = new DepthStencilState();

            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            depthStencilState.DepthBufferEnable = false;
        }

        protected override void SetLightStateNode(LightState state)
        {
            lightState = state;
        }

        protected override void StepNode()
        {
            if (model.AnimationController != null)
                model.AnimationController.Step(WorldMatrix);
        }

        protected override void RenderNode(IScene scene)
        {
            foreach (CustomModelMesh mesh in model.Meshes)
            {
                foreach (CustomModelMeshPart part in mesh.MeshParts)
                {
                    if (model.AnimationController == null)
                        part.MaterialHandler.SetupRendering(new Matrix[] { WorldMatrix }, scene.ActiveCamera.ViewMatrix,
                            scene.ActiveCamera.ProjectionMatrix, scene.AmbientColor, lightState);
                    else
                        part.MaterialHandler.SetupRendering(model.AnimationController.WorldMatrices, scene.ActiveCamera.ViewMatrix,
                            scene.ActiveCamera.ProjectionMatrix, scene.AmbientColor, lightState);
                }
                device.RasterizerState = rasterizerState;
                device.DepthStencilState = depthStencilState;
                mesh.Draw();
            }
        }

    }
}
