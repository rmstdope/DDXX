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
        private const int NUM_SIGNS_X = 8;
        private const int NUM_SIGNS_Y = 8;
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
            CreateBoard();
            CreatePieces();
            CreateCameraInterpolator();

            PointLightNode light = new PointLightNode("");
            light.Position = new Vector3(0, 4, 0);
            light.DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            scene.AddNode(light);
        }

        enum Pieces
        {
            Pawn = 0,
            Rook,
            Bishop,
            Queen,
            King,
            Knight,
            NumPieces
        }

        private void CreatePieces()
        {
            XLoader.Load("ChessPieces2.X", EffectFactory.CreateFromFile("TiVi.fxo"), TechniqueChooser.MeshPrefix("Reflective"));
            XLoader.AddToScene(scene);
            ModelNode[] models = new ModelNode[(int)Pieces.NumPieces];
            models[(int)Pieces.Pawn] = scene.GetNodeByName("Pawn") as ModelNode;
            models[(int)Pieces.Rook] = scene.GetNodeByName("Rook") as ModelNode;
            models[(int)Pieces.Bishop] = scene.GetNodeByName("Bishop") as ModelNode;
            models[(int)Pieces.Queen] = scene.GetNodeByName("Queen") as ModelNode;
            models[(int)Pieces.King] = scene.GetNodeByName("King") as ModelNode;
            models[(int)Pieces.Knight] = scene.GetNodeByName("Knight") as ModelNode;
            for (int i = 0; i < (int)Pieces.NumPieces; i++)
            {
                if (i == (int)Pieces.Knight)
                    models[(int)Pieces.Knight].WorldState.Scale(0.00007f);
                else
                    models[i].WorldState.Scale(0.0003f);
                models[i].WorldState.Tilt((float)Math.PI / 2);
                models[i].WorldState.MoveRight(-2 + i);
                models[i].Model.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
                models[i].Model.Materials[0].ReflectiveFactor = 0.015f;
                models[i].Model.Materials[0].AmbientColor = new ColorValue(0.05f, 0.05f, 0.05f, 0.05f);
                models[i].Model.Materials[0].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f, 0.6f);
            }
        }

        private void CreateCameraInterpolator()
        {
            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(0, 2, -6))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(2, new InterpolatedVector3(new Vector3(3, 3, -8))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(4, new InterpolatedVector3(new Vector3(-2, 3, 8))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(6, new InterpolatedVector3(new Vector3(2, 4, 12))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(9, new InterpolatedVector3(new Vector3(0, 2, -6))));
            spline.Calculate();
            interpolator = new Interpolator<InterpolatedVector3>();
            interpolator.AddSpline(spline);
        }

        private void CreateBoard()
        {
            MeshBuilder.SetDiffuseTexture("Default1", "Square.tga");
            MeshBuilder.SetReflectiveTexture("Default1", "rnl_cross.dds");
            MeshBuilder.SetReflectiveFactor("Default1", 0.4f);
            meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreatePlane(1, 1, 1, 1, true);
            meshDirector.Rotate((float)Math.PI / 2, 0, 0);
            float c = 0;
            for (int y = 0; y < NUM_SIGNS_Y; y++)
            {
                for (int x = 0; x < NUM_SIGNS_X; x++)
                {
                    IModel model = meshDirector.Generate("Default1");
                    ModelNode node = CreateSimpleModelNode(model, "TiVi.fxo", "Reflective");
                    node.WorldState.MoveForward(-1.05f * (y - NUM_SIGNS_Y / 2));
                    node.WorldState.MoveRight(-1.05f * (x - NUM_SIGNS_X / 2));
                    float color = 0.1f + c * 0.2f;
                    node.Model.Materials[0].AmbientColor = new ColorValue(0.05f, 0.05f, 0.05f, 0.05f);
                    node.Model.Materials[0].DiffuseColor = new ColorValue(color, color, color, color);
                    if (c == 0)
                        node.Model.Materials[0].ReflectiveFactor = 0.1f;
                    else
                        node.Model.Materials[0].ReflectiveFactor = 0.04f;
                    c = 1 - c;
                    signs[y * NUM_SIGNS_X + x] = node;
                    scene.AddNode(node);
                }
                c = 1 - c;
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
                    //ModelNode node = signs[y * NUM_SIGNS_X + x];
                    //node.WorldState.ResetRotation();
                    //float t = (Time.StepTime + (x - y) / 20.0f) % 3;
                    //const float period = 0.5f;
                    //if (t < period)
                    //{
                    //    float d = (float)Math.Sin(t / period * Math.PI);
                        //node.WorldState.MoveForward(-d * 0.5f);
                        //node.WorldState.Turn(d * 1.0f);
                        //node.WorldState.Tilt(d * 1.0f);
                        //node.WorldState.Roll(d * 0.5f);
                        //node.Model.Materials[0].AmbientColor = new ColorValue(0.8f, 0.6f, d);
                    //}
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
