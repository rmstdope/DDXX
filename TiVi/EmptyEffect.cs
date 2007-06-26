using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace TiVi
{
    public class EmptyEffect : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private MeshDirector meshDirector;
        private ModelNode box;

        public EmptyEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 10);

            MeshBuilder.SetDiffuseTexture("Default1", "FLOWER6P.jpg");
            meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreateBox(2, 2, 2);
            IModel model = meshDirector.Generate("Default1");
            box = CreateSimpleModelNode(model, "TiVi.fxo", "Simple");
            scene.AddNode(box);
           
        }

        public override void Step()
        {
            box.WorldState.Roll(Time.DeltaTime);
            box.WorldState.Turn(Time.DeltaTime);
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
