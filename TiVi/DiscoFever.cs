using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace TiVi
{
    class DiscoFever : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        
        public DiscoFever(string name, float startTime, float endTime)
        : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 3);
            MeshDirector director = new MeshDirector(MeshBuilder);
            director.CreatePlane(1, 1, 1, 1, true);
            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            IModel model = director.Generate("Default1");
            ModelNode modelNode;

            model.Materials[0].AmbientColor = ColorValue.FromColor(Color.Red);
            modelNode = CreateSimpleModelNode(model.Clone(), "TiVi.fxo", "Simple");
            modelNode.WorldState.MoveRight(-0.5f);
            scene.AddNode(modelNode);
            model.Materials[0].AmbientColor = ColorValue.FromColor(Color.Green);
            modelNode = CreateSimpleModelNode(model.Clone(), "TiVi.fxo", "Simple");
            modelNode.WorldState.MoveRight(+0.5f);
            scene.AddNode(modelNode);

            Mixer.ClearColor = Color.Blue;
        }

        public override void Step()
        {
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
