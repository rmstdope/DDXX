using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;

namespace TiVi
{
    public class TunnelFlight1 : TunnelFlightBase
    {
        private const int NUM_RINGS = 32;
        private const int NUM_BLOCKS = 12;

        private List<Brick> fallingBricks = new List<Brick>();

        public TunnelFlight1(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void InitializeSpecific()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 6);
            camera.WorldState.MoveUp(1.5f);
            camera.SetFOV((float)Math.PI / 4);

            CreateSplines();
            CreateFallingBricks();
        }

        private void CreateFallingBricks()
        {
            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            MeshBuilder.SetDiffuseColor("Default1", new ColorValue(0.8f, 0.8f, 0.8f, 0.8f));
            director.CreateChamferBox(0.6f, 0.6f, 0.1f, 0.05f, 4);
            director.UvMapPlane(1, 1, 1);
            IModel model = director.Generate("Default1");
            EffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model);
            float t = 0;
            for (int j = 0; j < NUM_RINGS; j++)
            {
                for (int i = 0; i < NUM_BLOCKS; i++)
                {
                    ModelNode modelNode = new ModelNode("Brick", model, effectHandler, Device);
                    scene.AddNode(modelNode);
                    fallingBricks.Add(new Brick(modelNode, t));
                    t += 0.03f;
                }
                t += 0.08f;
            }
        }

        protected override void StepSpecific()
        {
            StepFallingBricks();
            StepCamera();
            scene.Step();
        }

        private void StepCamera()
        {
            float t = Time.StepTime - StartTime;
            camera.WorldState.Position = cameraInterpolator.GetValue(t);
            camera.LookAt(tiviNode.Position + new Vector3(0, 0.8f, 0), new Vector3(0, 1, 0));
        }

        protected override void RenderSpecific()
        {
            scene.Render();
        }

        private void CreateSplines()
        {
            discInterpolator = new Interpolator<InterpolatedVector3>();
            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(10,  new InterpolatedVector3(new Vector3(0.0f, -0.0f, 0.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(15,  new InterpolatedVector3(new Vector3(0.0f, 3.0f, 0.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(20, new InterpolatedVector3(new Vector3(0.0f, 10.0f, 0.0f))));
            spline.Calculate();
            discInterpolator.AddSpline(spline);

            cameraInterpolator = new Interpolator<InterpolatedVector3>();
            spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(3, new InterpolatedVector3(new Vector3(0.0f, 0.8f, -7.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(6, new InterpolatedVector3(new Vector3(0.0f, 0.8f, -4.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(9, new InterpolatedVector3(new Vector3(-1.0f, 1.0f, -1.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(10, new InterpolatedVector3(new Vector3(1.0f, 0.8f, -1.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(15, new InterpolatedVector3(new Vector3(-1.0f, 3.6f, 1.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(20, new InterpolatedVector3(new Vector3(1.0f, 10.0f, -1.0f))));
            spline.Calculate();
            cameraInterpolator.AddSpline(spline);
        }

        private void StepFallingBricks()
        {
            if (fallingBricks.Count <= 0)
                return;
            for (int j = 0; j < NUM_RINGS; j++)
            {
                for (int i = 0; i < NUM_BLOCKS; i++)
                {
                    ModelNode model = fallingBricks[j * NUM_BLOCKS + i].Model;
                    float startTime = fallingBricks[j * NUM_BLOCKS + i].StartTime;
                    const float FALL_TIME = 1.3f;
                    const float FALL_HEIGHT = 10.0f;
                    float time = Time.StepTime - StartTime - startTime;
                    float upAdd = 0;
                    if (time < FALL_TIME)
                    {
                        if (time < 0)
                            time = 0;
                        float zeroToOne = (float)Math.Sin(time / FALL_TIME * Math.PI / 2);
                        upAdd = (1 - zeroToOne) * FALL_HEIGHT;
                    }
                    model.WorldState.Reset();
                    //model.WorldState.MoveForward(2);
                    model.WorldState.MoveUp(0 + 0.8f * j + upAdd);
                    model.WorldState.Tilt((float)Math.PI / 2);
                    model.WorldState.Roll((Time.StepTime - StartTime) / 2 + (float)Math.PI * 2 * i / NUM_BLOCKS);
                    model.WorldState.MoveUp(-3.0f);
                }
            }
        }

    }
}
