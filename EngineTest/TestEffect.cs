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

namespace EngineTest
{
    public class TestEffect : BaseDemoEffect
    {
        private MeshNode mesh;
        private LightNode light;

        public TestEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Scene.AmbientColor = new ColorValue(0.3f, 0.3f, 0.3f);

            Model model = ModelFactory.FromFile("../../Data/airplane 2.x", ModelFactory.Options.EnsureTangents);
            IEffect effect = EffectFactory.CreateFromFile("../../../Effects/BlinnPhongShaders.fxo");
            EffectHandler handler = new EffectHandler(effect);
            mesh = new MeshNode("Mesh", model, handler);
            Scene.AddNode(mesh);

            Light dxLight = new Light();
            dxLight.DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            dxLight.Type = LightType.Point;
            light = new LightNode("Point Light", dxLight);
            Scene.AddNode(light);

            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-20.0f);
            Scene.AddNode(camera);
            Scene.ActiveCamera = camera;
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
            Scene.Step();
            //mesh.WorldState.Roll(0.01f);
            //mesh.WorldState.Turn(0.021f);
            light.WorldState.Position = new Vector3(500.0f * (float)Math.Sin(Time.StepTime),
                                                    0.0f,
                                                    500.0f * (float)Math.Cos(Time.StepTime));
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
