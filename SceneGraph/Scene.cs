using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public class Scene : IRenderableScene
    {
        private IEffect effect;
        private EffectHandle lightDiffuseHandle;
        private EffectHandle lightSpecularHandle;
        private EffectHandle lightPositionHandle;
        private EffectHandle eyePositionHandle;

        private NodeBase rootNode;
        private IRenderableCamera activeCamera;
        private IDevice device;
        private ColorValue ambientColor;

        public Scene()
        {
            rootNode = new DummyNode("Scene Root Node");
            device = D3DDriver.GetInstance().GetDevice();
            ambientColor = new ColorValue(0.5f, 0.5f, 0.5f, 0.5f);
            EffectFactory effectFactory = D3DDriver.EffectFactory;
            effect = effectFactory.CreateFromFile(/*"../../../Effects/*/"PoolEffect.fxo");
            lightDiffuseHandle = effect.GetParameter(null, "LightDiffuseColor");
            lightSpecularHandle = effect.GetParameter(null, "LightSpecularColor");
            lightPositionHandle = effect.GetParameter(null, "LightPosition");
            eyePositionHandle = effect.GetParameter(null, "EyePosition");

            if (lightDiffuseHandle == null || lightSpecularHandle == null || lightPositionHandle == null || eyePositionHandle == null)
                throw new DDXXException("Can't find mandatory handles in PoolEffect");
        }

        public void AddNode(INode node1)
        {
            rootNode.AddChild(node1);
        }

        public void Step()
        {
            LightState state = new LightState();

            rootNode.Step();

            rootNode.SetLightState(state);

            effect.SetValue(lightDiffuseHandle, state.DiffuseColor);
            effect.SetValue(lightSpecularHandle, state.SpecularColor);
            effect.SetValue(lightPositionHandle, state.Positions);

            Vector3 eyePos = ActiveCamera.WorldState.Position;
            effect.SetValue(eyePositionHandle, new Vector4(eyePos.X, eyePos.Y, eyePos.Z, 1.0f));
        }

        public void Render()
        {
            if (ActiveCamera == null)
                throw new DDXXException("Must have an active camera set before a scene can be rendered.");

            rootNode.Render(this);
        }

        #region IRenderableScene Members

        public IRenderableCamera ActiveCamera
        {
            get { return activeCamera; }
            set
            {
                if (rootNode.HasChild(value))
                    activeCamera = value;
                else
                    throw new DDXXException("The active camera must be part of the scene graph.");
            }
        }

        public ColorValue AmbientColor
        {
            get { return ambientColor; }
            set { ambientColor = value; }
        }

        #endregion
    }
}
