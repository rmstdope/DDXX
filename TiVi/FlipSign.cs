using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace TiVi
{
    public class FlipSign : BaseDemoEffect
    {
        private const int NUM_SIGNS_X = 32;
        private const int NUM_SIGNS_Y = 18;
        private IScene scene;
        private CameraNode camera;
        private MeshDirector meshDirector;
        private ModelNode[] signs = new ModelNode[NUM_SIGNS_X * NUM_SIGNS_Y];
        private Interpolator<InterpolatedVector3> interpolator;

        public FlipSign(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 10);
            CreateSigns();

            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(0, 0, -10))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(2, new InterpolatedVector3(new Vector3(5, 3, -8))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(4, new InterpolatedVector3(new Vector3(-3, 5, -8))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(6, new InterpolatedVector3(new Vector3(2, -3, -12))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(9, new InterpolatedVector3(new Vector3(0, 0, -10))));
            spline.Calculate();
            interpolator = new Interpolator<InterpolatedVector3>();
            interpolator.AddSpline(spline);
        }

        private void CreateSigns()
        {
            MeshBuilder.SetDiffuseTexture("Default1", "Square.tga");
            MeshBuilder.SetReflectiveTexture("Default1", "rnl_cross.dds");
            MeshBuilder.SetReflectiveFactor("Default1", 0.2f);
            meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreatePlane(1, 1, 1, 1, true);
            IModel model = meshDirector.Generate("Default1");
            for (int y = 0; y < NUM_SIGNS_Y; y++)
            {
                for (int x = 0; x < NUM_SIGNS_X; x++)
                {
                    ModelNode node = CreateSimpleModelNode(model, "TiVi.fxo", "Reflective");
                    signs[y * NUM_SIGNS_X + x] = node;
                    scene.AddNode(node);
                }
            }
        }

        public override void Step()
        {
            camera.WorldState.Position = interpolator.GetValue(Time.StepTime % 10);
            camera.LookAt(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            for (int y = 0; y < NUM_SIGNS_Y; y++)
            {
                for (int x = 0; x < NUM_SIGNS_X; x++)
                {
                    ModelNode node = signs[y * NUM_SIGNS_X + x];
                    node.WorldState.Reset();
                    node.WorldState.MoveUp(-1.05f * (y - NUM_SIGNS_Y / 2));
                    node.WorldState.MoveRight(-1.05f * (x - NUM_SIGNS_X / 2));
                    float t = (Time.StepTime + (x - y) / 40.0f) % 3;
                    const float period = 1.2f;
                    if (t < period)
                    {
                        float d = (float)Math.Sin(t / period * Math.PI);
                        node.WorldState.MoveForward(-d * 0.5f);
                        //node.WorldState.Turn(d * 1.0f);
                        //node.WorldState.Tilt(d * 1.0f);
                        node.WorldState.Roll(d * 2);
                        node.Model.Materials[0].AmbientColor = new ColorValue(0.8f, 0.6f, d);
                    }
                }
            }
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
