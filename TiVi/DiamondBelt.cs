using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.MeshBuilder;

namespace TiVi
{
    public class DiamondBelt : BaseDemoEffect
    {
        private const int NUM_DIAMONDS = 100;
        private class Diamond
        {
            public Vector3 DeltaPosition;
            public ModelNode Node;
            public float TimeDelay;
            public float RotationSpeed;
            public float SineSpeed;
            public float SineAmplitude;
        }
        private List<Diamond> diamonds = new List<Diamond>();
        private Interpolator<InterpolatedVector3> diamondInterpolator;
        private INode dummyNode;
        private TiViMeshDirector tiviMeshDirector;
        private CameraNode camera;
        private IScene scene;

        public DiamondBelt(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 1000);
            camera.SetClippingPlanes(1, 10000);
            tiviMeshDirector = new TiViMeshDirector(MeshBuilder, new MeshDirector(MeshBuilder), EffectFactory, Device);

            CreateDiamondBelt();

            scene.Validate();
        }

        private void CreateDiamondBelt()
        {
            dummyNode = new DummyNode("Dummy");
            for (int i = 0; i < NUM_DIAMONDS; i++)
            {
                ModelNode node = tiviMeshDirector.CreateDiamondNode(50);
                Diamond diamond = new Diamond();
                diamond.DeltaPosition = new Vector3(Rand.Float(-40, 40), Rand.Float(-40, 40), Rand.Float(-40, 40));
                diamond.SineSpeed = Rand.Float(1, 3);
                diamond.SineAmplitude = Rand.Float(10, 20);
                diamond.RotationSpeed = Rand.Float(2, 3);
                diamond.TimeDelay = Rand.Float(-1, 0) - i * 0.1f;
                diamond.Node = node;
                diamonds.Add(diamond);
                dummyNode.AddChild(node);
            }
            scene.AddNode(dummyNode);

            CreateBeltInterpolator();
        }

        private void CreateBeltInterpolator()
        {
            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(-1500, 100, 800))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(2, new InterpolatedVector3(new Vector3(-100, 100, 800))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(5, new InterpolatedVector3(new Vector3(-200, 150, 500))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(8, new InterpolatedVector3(new Vector3(0, 50, 100))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(11, new InterpolatedVector3(new Vector3(30, -50, -500))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(14, new InterpolatedVector3(new Vector3(-100, 0, -500))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(17, new InterpolatedVector3(new Vector3(50, -100, -1100))));
            spline.Calculate();
            diamondInterpolator = new Interpolator<InterpolatedVector3>();
            diamondInterpolator.AddSpline(spline);
        }

        public override void Step()
        {
            StepCamera();
            StepDiamondBelt();
            scene.Step();
        }

        private void StepDiamondBelt()
        {
            foreach (Diamond diamond in diamonds)
            {
                diamond.Node.Position = diamondInterpolator.GetValue(Time.StepTime + diamond.TimeDelay - StartTime) +
                    diamond.DeltaPosition +
                    new Vector3((float)Math.Sin(diamond.SineSpeed * Time.StepTime),
                                (float)Math.Cos(diamond.SineSpeed * Time.StepTime),
                                (float)Math.Sin(diamond.SineSpeed * Time.StepTime)) * diamond.SineAmplitude;
                diamond.Node.WorldState.Turn(Time.DeltaTime * diamond.RotationSpeed);
            }
        }

        private void StepCamera()
        {
            float t = Time.StepTime / 4;
            camera.LookAt(new Vector3(), new Vector3(0, 1, 0));
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
