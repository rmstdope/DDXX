using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX;

namespace TiVi
{
    public class GiantDiamond : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private TiViMeshDirector tiviMeshDirector;
        private ModelNode giantDiamond;

        public GiantDiamond(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 0);
            camera.SetClippingPlanes(0.1f, 10000.0f);
            tiviMeshDirector = new TiViMeshDirector(MeshBuilder, new MeshDirector(MeshBuilder), EffectFactory, Device);
            giantDiamond = tiviMeshDirector.CreateDiamondNode(10);
            scene.AddNode(giantDiamond);
        }

        public override void Step()
        {
            float modifiedTime = Time.StepTime - StartTime;
            giantDiamond.WorldState.Turn(Time.DeltaTime * 1.5f);
            giantDiamond.WorldState.Position = new Vector3(0, 0, 1000 - modifiedTime * 50);

            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
