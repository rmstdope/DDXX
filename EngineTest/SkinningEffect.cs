using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Dope.DDXX.Animation;

namespace EngineTest
{
    public class SkinningEffect : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private ModelNode node;

        public SkinningEffect(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 100);
            IModel model = ModelFactory.FromFile("Content/models/dude", "Content/effects/Skinning");
            node = new ModelNode("", model, GraphicsDevice);
            node.Model.AnimationController.Speed = 1.0f;
            node.Model.AnimationController.PlayMode = PlayMode.Loop;
            node.WorldState.MoveDown(50);
            node.WorldState.Turn(MathHelper.PiOver2);
            scene.AddNode(node);
        }

        public override void Step()
        {
            //node.WorldState.Turn(Time.DeltaTime);
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
