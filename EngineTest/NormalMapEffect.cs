using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace EngineTest
{
    public class NormalMapEffect : BaseDemoEffect
    {
        private Scene scene;
        private ModelNode node;

        public NormalMapEffect(float start, float end)
            : base(start, end)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            scene = new Scene();

            // Create model
            IModel model = ModelFactory.FromFile("airplane 2.x", ModelFactory.Options.EnsureTangents);
            IEffect effect = EffectFactory.CreateFromFile("Test.fxo");
            EffectHandler effectHandler = new EffectHandler(effect, "NormalMa", model);
            node = new ModelNode("Test Model", model, effectHandler);
            scene.AddNode(node);

            // Create camera
            CameraNode camera = new CameraNode("Test Camera");
            camera.WorldState.MoveForward(-10);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;

            // Add normal map
            for (int i = 0; i < model.Materials.Length; i++)
            {
                model.Materials[i].NormalTexture = TextureFactory.CreateFromFile("NormalMap.dds");
            }
        }


        public override void Step()
        {
            node.WorldState.Roll(Time.DeltaTime * 0.7f);
            node.WorldState.Turn(Time.DeltaTime * 1.2f);

            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
