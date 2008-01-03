using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public class ModelNode : NodeBase, IRenderableMesh
    {
        private IModel model;
        private LightState lightState;
        private IGraphicsDevice device;
        private CullMode cullMode = CullMode.CullCounterClockwiseFace;

        public IModel Model
        {
            get { return model; }
            set { model = value; }
        }

        public CullMode CullMode
        {
            get { return cullMode; }
            set { cullMode = value; }
        }

        public ModelNode(string name, IModel model, IGraphicsDevice device) 
            : base(name)
        {
            this.model = model;
            this.device = device;
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
            foreach (IModelMesh mesh in model.Meshes)
            {
                foreach (IModelMeshPart part in mesh.MeshParts)
                {
                    if (model.AnimationController == null)
                        part.MaterialHandler.SetupRendering(new Matrix[] { WorldMatrix }, scene.ActiveCamera.ViewMatrix,
                            scene.ActiveCamera.ProjectionMatrix, scene.AmbientColor, lightState);
                    else
                        part.MaterialHandler.SetupRendering(model.AnimationController.WorldMatrices, scene.ActiveCamera.ViewMatrix,
                            scene.ActiveCamera.ProjectionMatrix, scene.AmbientColor, lightState);
                }
                device.RenderState.CullMode = cullMode;
                mesh.Draw();
            }
        }

    }
}
