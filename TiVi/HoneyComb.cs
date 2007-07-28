using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace TiVi
{
    public class HoneyComb : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private float brightness;
        private IModel model;
        private ModelNode cylinder;

        public float Brightness
        {
            get { return brightness; }
            set 
            { 
                brightness = value;
                if (model != null)
                {
                    model.Materials[0].AmbientColor = new ColorValue(brightness, brightness, brightness);
                }
            }
        }

        public HoneyComb(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            brightness = 0.5f;
            SetStepSize(GetTweakableNumber("Brightness"), 0.02f);
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 0);
            camera.SetFOV((float)Math.PI / 6);
            MeshBuilder.SetDiffuseTexture("Default1", "honeycomb.jpg");
            MeshBuilder.SetAmbientColor("Default1", new ColorValue(0.6f, 0.6f, 0.6f));
            MeshDirector director = new MeshDirector(MeshBuilder);
            //director.CreatePlane(10, 10, 1, 1, true);
            //director.UvRemap(0, 10, 0, 10);
            //model = director.Generate("Default1");
            //ModelNode node = CreateSimpleModelNode(model, "TiVi.fxo", "Simple");
            //scene.AddNode(node);

            director.CreateCylinder(10, 10 * (float)Math.PI, 64, 8);
            director.NormalFlip();
            director.UvRemap(0, 20, 0, 20);
            model = director.Generate("Default1");
            cylinder = CreateSimpleModelNode(model, "TiVi.fxo", "SimpleMirroredTexture");
            scene.AddNode(cylinder);
        }

        public override void Step()
        {
            cylinder.WorldState.Turn(Time.DeltaTime * 0.04f);
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
