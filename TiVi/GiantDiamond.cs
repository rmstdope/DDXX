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
        private float positionDelta;

        public float PositionDelta
        {
            get { return positionDelta; }
            set { positionDelta = value; }
        }

        public GiantDiamond(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            positionDelta = 1000;
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
            giantDiamond.WorldState.ResetRotation();
            giantDiamond.WorldState.Turn((Time.StepTime - StartTime) * 1.5f + (float)Math.PI / 2);
            giantDiamond.WorldState.Position = new Vector3(0, 0, positionDelta - modifiedTime * 50);

            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
