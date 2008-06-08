using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.ModelBuilder;

namespace TiVi
{
    public class EmptyEffect : BaseDemoEffect
    {
        private CameraNode camera;
        private ModelDirector modelDirector;
        private ModelNode box;

        public EmptyEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, 3);

            ModelBuilder.SetDiffuseTexture("Default1", "FLOWER6P.jpg");
            modelDirector = new ModelDirector(ModelBuilder);
            modelDirector.CreateBox(2, 2, 2);
            IModel model = modelDirector.Generate("Default1");
            //box = CreateSimpleModelNode(model, "TiVi.fxo", "Simple");
            Scene.AddNode(box);

            Scene.Validate();
        }

        public override void Step()
        {
            box.WorldState.Roll(Time.DeltaTime);
            box.WorldState.Turn(Time.DeltaTime);
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
