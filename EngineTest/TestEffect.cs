using System;
using System.Collections.Generic;
using System.Text;
using DemoFramework;
using Graphics;
using Physics;
using SceneGraph;

namespace EngineTest
{
    public class TestEffect : BaseEffect
    {
        public TestEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            MeshContainer container = MeshFactory.FromFile("../../Data/airplane 2.x");
            IEffect effect = EffectFactory.CreateFromFile("Effects.fx");
            MeshNode node = new MeshNode("Mesh", container);
            node.EffectTechnique = new EffectContainer(effect, effect.FindNextValidTechnique(null));
            Scene.AddNode(node);
            
            //IMesh mesh = MeshFactory.CreateBox(10.0f, 10.0f, 10.0f, MeshFactory.Usage.Static);
            //IMesh mesh = MeshFactory.FromFile("tiger.x", out EffectInstance[] );
            //Direct3D.IEffect effect = EffectFactory.CreateFromFile("Effects.fx");
            //MeshNode node = new MeshNode("Mesh", mesh);
            //node.EffectTechnique = new EffectTechnique(effect, effect.FindNextValidTechnique(null));
            //Scene.AddNode(node);

            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-100.0f);
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
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
