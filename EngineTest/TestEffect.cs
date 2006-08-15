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

            IMesh mesh = Factory.CreateBoxMesh(Device, 10.0f, 10.0f, 10.0f);
            MeshNode node = new MeshNode("Mesh", mesh);
            Scene.AddNode(node);

            Camera camera = new Camera("Camera");
            camera.WorldState.MoveForward(-100.0f);
            Scene.AddNode(camera);
            Scene.ActiveCamera = camera;
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
