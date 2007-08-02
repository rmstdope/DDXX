using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace TiVi
{
    public class DanceCloseup : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private PointLightNode[] lights = new PointLightNode[2];

        public DanceCloseup(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 3);
            camera.WorldState.MoveUp(1.5f);

            XLoader.Load("Tivi-Dance.X", EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(string name)
                {
                    return delegate(int material)
                    {
                        if (material == 1)
                            return "TvScreen";
                        else
                            return "TiViReflective";
                    };
                });
            XLoader.AddToScene(scene);
            ModelNode tivi = scene.GetNodeByName("TiVi") as ModelNode;
            tivi.WorldState.Turn((float)Math.PI * 1.2f);
            tivi.WorldState.Position = new Vector3(0, 0.25f, 0);
            tivi.Model.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
            tivi.Model.Materials[0].DiffuseTexture = TextureFactory.CreateFromFile("marble.jpg");
            tivi.Model.Materials[0].AmbientColor = new ColorValue(0.3f, 0.3f, 0.3f, 0.3f);
            tivi.Model.Materials[0].DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            tivi.Model.Materials[0].ReflectiveFactor = 0.1f;
            float timeScale = 1.348f;
            ((tivi as ModelNode).Model as SkinnedModel).SetAnimationSet(0, StartTime, timeScale);

            lights[0] = new PointLightNode("Light1");
            lights[0].Position = new Vector3(0, 0, -2);
            lights[0].DiffuseColor = new ColorValue(0.7f, 0.7f, 1.0f, 1.0f);
            lights[0].Range = 0.05f;
            scene.AddNode(lights[0]);
            lights[1] = new PointLightNode("Light1");
            lights[1].Position = new Vector3(0, 0, 2);
            lights[1].DiffuseColor = new ColorValue(1.0f, 0.7f, 1.0f, 1.0f);
            lights[1].Range = 0.05f;
            scene.AddNode(lights[1]);
        }

        public override void Step()
        {
            //camera.WorldState.Reset();
            //camera.WorldState.Turn(Time.StepTime * 0.3f);
            //camera.WorldState.MoveForward(-2);
            //camera.WorldState.MoveUp(1.5f + (float)Math.Sin(Time.StepTime * 0.2f));

            lights[0].Position = new Vector3(2 * (float)Math.Sin(Time.StepTime * 0.6f),
                2 * (float)Math.Cos(Time.StepTime * 0.6f), -1);
            lights[1].Position = new Vector3(2 * (float)Math.Sin(Time.StepTime * 0.9f),
                2 * (float)Math.Cos(Time.StepTime * 0.9f), -1);
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
