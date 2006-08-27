using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Dope.DDXX.SceneGraph;

namespace EngineTest
{
    public class TestEffect : BaseDemoEffect
    {
        private MeshNode mesh;

        public TestEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Model model = ModelFactory.FromFile("../../Data/airplane 2.x", ModelFactory.Options.EnsureTangents);
            IEffect effect = EffectFactory.CreateFromFile("../../../Effects/BlinnPhongShaders.fxo");
            EffectHandler handler = new EffectHandler(effect);
            mesh = new MeshNode("Mesh", model, handler);
            Scene.AddNode(mesh);

            //IMesh mesh = ModelFactory.CreateBox(10.0f, 10.0f, 10.0f, ModelFactory.Usage.Static);
            //IMesh mesh = ModelFactory.FromFile("tiger.x", out EffectInstance[] );
            //Direct3D.IEffect effect = EffectFactory.CreateFromFile("Effects.fx");
            //MeshNode mesh = new MeshNode("Mesh", mesh);
            //mesh.EffectTechnique = new EffectTechnique(effect, effect.FindNextValidTechnique(null));
            //Scene.AddNode(mesh);

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
            mesh.WorldState.Roll(0.01f);
            mesh.WorldState.Turn(0.021f);
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
