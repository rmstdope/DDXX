using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.ParticleSystems;
using Dope.DDXX.Graphics.Skinning;

namespace EngineTest
{
    public class TestEffect : BaseDemoEffect
    {
        private FloaterSystem ps;
        private CameraNode camera;
        private Scene scene;
        private ModelNode mesh;

        private ModelNode modelSkinning;
        private ModelNode modelNoSkinning;

        public TestEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
            scene = new Scene();
        }

        public override void Initialize()
        {
            base.Initialize();

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f);

            camera = new CameraNode("Camera");
            //camera.WorldState.Tilt(2.0f);
            camera.WorldState.MoveForward(-300.0f);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;

            ps = new FloaterSystem("System");
            ps.Initialize(50, 200.0f, null);//"BlurBackground.jpg");
            scene.AddNode(ps);

            // Create mesh
            IEffect effect = D3DDriver.EffectFactory.CreateFromFile("Test.fxo");
            IModel model = D3DDriver.ModelFactory.FromFile("Wanting More.x", ModelFactory.Options.None);
            EffectHandler effectHandler = new EffectHandler(effect, "TransparentText", model);
            mesh = new ModelNode("Text1", model, effectHandler);
            scene.AddNode(mesh);
            //mesh.WorldState.Tilt(-(float)Math.PI / 2.0f);

            model = ModelFactory.FromFile("TiVi.x", ModelFactory.Options.None);
            modelNoSkinning = new ModelNode("No Skinning",
                model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"), "Skinning", model));
            modelNoSkinning.WorldState.Scale(100.0f);
            modelNoSkinning.WorldState.MoveRight(-50);
            modelNoSkinning.WorldState.Roll((float)Math.PI);
            modelNoSkinning.WorldState.Tilt((float)Math.PI / 2);
            //scene.AddNode(modelNoSkinning);

            model = ModelFactory.FromFile("TiVi.x", ModelFactory.Options.SkinnedModel);
            modelSkinning = new ModelNode("Skinning",
                model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"), "Skinning", model));
            modelSkinning.WorldState.Scale(100.0f);
            modelSkinning.WorldState.MoveRight(50);
            modelSkinning.WorldState.MoveUp(-75);
            modelSkinning.WorldState.Roll((float)Math.PI);
            modelSkinning.WorldState.Tilt((float)Math.PI / 2);
            scene.AddNode(modelSkinning);

            //CameraNodeX cameraX = new CameraNodeX((SkinnedModel)model, "Camera");
            //scene.AddNode(cameraX);
            //scene.ActiveCamera = cameraX;
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
            float scale = (Time.StepTime % 5.0f) / 5.0f;
            scale *= 2.0f;
            mesh.WorldState.Scaling = new Vector3(scale, scale, scale);
            mesh.WorldState.Position = new Vector3(0, scale * 200.0f, 0);

            mesh.WorldState.Roll(scale / 100.0f);
            mesh.WorldState.Turn(Time.DeltaTime);
            //mesh.WorldState.Tilt(Time.DeltaTime);
            scene.Step();
        }

        public override void Render()
        {
            IDevice device = D3DDriver.GetInstance().Device;
            scene.Render();
        }

    }
}
