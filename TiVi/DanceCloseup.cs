using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Microsoft.DirectX;

namespace TiVi
{
    public class DanceCloseup : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;

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
                            return "Solid";
                    };
                });
            XLoader.AddToScene(scene);
            scene.GetNodeByName("TiVi").WorldState.Turn((float)Math.PI * 1.2f);
            scene.GetNodeByName("TiVi").WorldState.Position = new Vector3(0, 0.25f, 0);
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
