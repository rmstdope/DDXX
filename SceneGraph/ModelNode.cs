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
        private LightState lightState;
        private GraphicsDevice device;
        public CullMode CullMode { get { return RasterizerState.CullMode; } }
        public CustomModel Model { get; set; }
        public RasterizerState RasterizerState { private get; set; }
        public DepthStencilState DepthStencilState { get; set; }

        public ModelNode(string name, CustomModel model, GraphicsDevice device) 
            : base(name)
        {
            Model = model;
            this.device = device;
            RasterizerState = RasterizerState.CullCounterClockwise;
            DepthStencilState = DepthStencilState.Default;
        }

        protected override void SetLightStateNode(LightState state)
        {
            lightState = state;
        }

        protected override void StepNode()
        {
            if (Model.AnimationController != null)
                Model.AnimationController.Step(WorldMatrix);
        }

        protected override void RenderNode(IScene scene)
        {
            foreach (CustomModelMesh mesh in Model.Meshes)
            {
                foreach (CustomModelMeshPart part in mesh.MeshParts)
                {
                    if (Model.AnimationController == null)
                        part.MaterialHandler.SetupRendering(new Matrix[] { WorldMatrix }, scene.ActiveCamera.ViewMatrix,
                            scene.ActiveCamera.ProjectionMatrix, scene.AmbientColor, lightState);
                    else
                        part.MaterialHandler.SetupRendering(Model.AnimationController.WorldMatrices, scene.ActiveCamera.ViewMatrix,
                            scene.ActiveCamera.ProjectionMatrix, scene.AmbientColor, lightState);
                }
                device.RasterizerState = RasterizerState;
                device.DepthStencilState = DepthStencilState;
                mesh.Draw();
            }
        }

    }
}
