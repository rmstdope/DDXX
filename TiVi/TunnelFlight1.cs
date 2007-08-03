using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;

namespace TiVi
{
    public class TunnelFlight1 : TunnelFlightBase
    {
        private class Diamond
        {
            public ModelNode Node;
            public Vector3 Velocity;
        };

        private const int NUM_RINGS = 24;
        private const int NUM_BLOCKS = 16;

        private List<Brick> fallingBricks = new List<Brick>();
        private TiVi tivi;
        //private TiVi tivi2;
        private List<Diamond> diamonds = new List<Diamond>();

        public TunnelFlight1(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void InitializeSpecific()
        {
            camera.SetFOV((float)Math.PI / 3);

            tivi = new TiVi(tiviNode, camera, StartTime);
            //tivi2 = new TiVi(tiviNode, camera, EndTime);

            CreateSplines();
            CreateFallingBricks();
            CreateDiamonds();
        }

        private void CreateDiamonds()
        {
            TiViMeshDirector director = 
                new TiViMeshDirector(MeshBuilder, new MeshDirector(MeshBuilder), EffectFactory, Device);
            ModelNode node = director.CreateDiamondNode(0.2f);
            for (int i = 0; i < 5; i++)
            {
                Diamond diamond = new Diamond();
                diamond.Node = new ModelNode(i.ToString(), node.Model, node.EffectHandler, Device);
                RestartDiamond(diamond);
                scene.AddNode(diamond.Node);
                diamonds.Add(diamond);
            }
        }

        private void CreateFallingBricks()
        {
            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            MeshBuilder.SetDiffuseColor("Default1", new ColorValue(0.5f, 0.5f, 0.5f, 0.8f));
            director.CreateChamferBox(0.6f, 0.6f, 0.1f, 0.05f, 4);
            director.UvMapPlane(1, 1, 1);
            IModel model = director.Generate("Default1");
            EffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model);
            float t = 2;
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
            StepDiamonds();
            StepFallingBricks();
            StepCamera();
            scene.Step();
        }

        private void StepDiamonds()
        {
            foreach (Diamond diamond in diamonds)
            {
                diamond.Node.WorldState.Turn(Time.DeltaTime);
                diamond.Node.Position += diamond.Velocity * Time.DeltaTime * 1.5f;
                // Ignore Y
                if (diamond.Node.Position.X * diamond.Node.Position.X + diamond.Node.Position.Z * diamond.Node.Position.Z > 9)
                    RestartDiamond(diamond);
            }
        }

        private void RestartDiamond(Diamond diamond)
        {
            diamond.Node.Position = new Vector3((float)Math.Sin(Rand.Float(0, Math.PI * 2)), 0, (float)Math.Cos(Rand.Float(0, Math.PI * 2))) * 3;
            diamond.Velocity = new Vector3(Rand.Float(-1, 1), Rand.Float(-0.2, 0.2), Rand.Float(-1, 1));
            //diamond.Velocity = -diamond.Node.Position;
            diamond.Velocity.Normalize();
            diamond.Node.Position = new Vector3(diamond.Node.Position.X, 1, diamond.Node.Position.Z) + discInterpolator.GetValue(Time.StepTime - StartTime);
        }

        private void StepCamera()
        {
            float t = Time.StepTime - StartTime;
            camera.WorldState.Position = cameraInterpolator.GetValue(t) + tiviNode.Position;
            camera.LookAt(cameraTargetInterpolator.GetValue(t) + tiviNode.Position, cameraUpInterpolator.GetValue(t));
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
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(tivi.DestinationPos)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(2, new InterpolatedVector3(new Vector3(tivi.DestinationPos.X + 1, 1, 4))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(6, new InterpolatedVector3(new Vector3(-1.9f, 0.8f, 1.5f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(10, new InterpolatedVector3(new Vector3(-1.8f, 2.0f, 1.3f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(15, new InterpolatedVector3(new Vector3(-1.5f, 1.2f, 1.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(20, new InterpolatedVector3(new Vector3(-1.6f, 1.3f, 1.2f))));
            //spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(EndTime - StartTime, new InterpolatedVector3(tivi2.DestinationPos)));
            spline.Calculate();
            cameraInterpolator.AddSpline(spline);

            cameraUpInterpolator = new Interpolator<InterpolatedVector3>();
            spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(tivi.DestinationUp)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(2, new InterpolatedVector3(new Vector3(0, 1, 0))));
            //spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(EndTime - StartTime, new InterpolatedVector3(tivi2.DestinationUp)));
            spline.Calculate();
            cameraUpInterpolator.AddSpline(spline);

            cameraTargetInterpolator = new Interpolator<InterpolatedVector3>();
            spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(6, new InterpolatedVector3(tivi.Center)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(12, new InterpolatedVector3(tivi.Center + new Vector3(0.3f, -1.0f, 0.2f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(16, new InterpolatedVector3(tivi.Center + new Vector3(0.1f, -0.2f, -0.3f))));
            //spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(EndTime - StartTime, new InterpolatedVector3(tivi2.Center)));
            spline.Calculate();
            cameraTargetInterpolator.AddSpline(spline);
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
                    const float FALL_TIME = 1.5f;
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
                    model.WorldState.MoveUp(0 + 0.8f * j + upAdd);
                    model.WorldState.Tilt((float)Math.PI / 2);
                    model.WorldState.Roll((Time.StepTime - StartTime) / 1.0f + (float)Math.PI * 2 * i / NUM_BLOCKS);
                    model.WorldState.MoveUp(-2.5f);
                }
            }
        }

        protected override string GetScreenTechnique()
        {
            return "WhiteScreen";
        }
    }
}
