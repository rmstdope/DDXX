using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace EngineTest
{
    /*public class NormalMapEffect : BaseDemoEffect
    {
        private Scene scene;
        private ModelNode node;

        public NormalMapEffect(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            scene = new Scene();

            // Create modelTivi
            IModel model = ModelFactory.FromFile("airplane 2.x", "Test.fx");

            // Add normal map
            for (int i = 0; i < model.Materials.Length; i++)
            {
                model.Materials[i].NormalTexture = TextureFactory.CreateFromFile("NormalMap.dds");
            }

            // create effect and nodeTiVi
            IEffect effect = EffectFactory.CreateFromFile("Test.fxo");
            EffectHandler effectHandler = new EffectHandler(effect,
                delegate(int material) { return ""; }, model);
            node = new ModelNode("Test Model", model, effectHandler, Device);
            scene.AddNode(node);

            // Create camera
            CameraNode camera = new CameraNode("Test Camera");
            camera.WorldState.MoveForward(-10);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
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
    }*/
}
