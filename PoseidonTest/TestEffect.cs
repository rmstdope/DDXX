using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;

namespace PoseidonTest
{
    public class TestEffect : BaseDemoEffect
    {
        private MeshNode mesh;
        private Scene scene;

        public TestEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
            scene = new Scene();
        }

        public override void Initialize()
        {
            base.Initialize();

            Model model = ModelFactory.FromFile("../../Data/airplane 2.x", ModelFactory.Options.EnsureTangents);
            //Model model = ModelFactory.CreateBox(10, 10, 10);
            IEffect effect = EffectFactory.CreateFromFile("../../Effects/BlinnPhongShaders.fxo");
            EffectHandler effectHandler = new EffectHandler(effect);
            mesh = new MeshNode("Mesh", model, effectHandler);
            scene.AddNode(mesh);

            //Light dxLight = new Light();
            //dxLight.DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            //dxLight.Type = LightType.Point;
            //light = new LightNode("Point Light", dxLight);
            //scene.AddNode(light);

            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-20.0f);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
            scene.Step();
            //mesh.WorldState.Roll(0.01f);
            //mesh.WorldState.Turn(0.021f);
            //light.WorldState.Position = new Vector3(500.0f * (float)Math.Sin(Time.StepTime),
            //                                        0.0f,
            //                                        500.0f * (float)Math.Cos(Time.StepTime));
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
