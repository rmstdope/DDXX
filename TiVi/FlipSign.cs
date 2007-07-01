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
        private List<ChessPiece> chessPieces = new List<ChessPiece>();

        public FlipSign(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            IDevice device = Device;
            CreateStandardSceneAndCamera(out scene, out camera, 15);
            CreateBoard();
            CreatePieces();
            CreateCameraInterpolator();

            PointLightNode[] lights = new PointLightNode[2];
            lights[0] = new PointLightNode("");
            lights[0].Position = new Vector3(-5, 4, 0);
            lights[0].DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            scene.AddNode(lights[0]);
            lights[1] = new PointLightNode("");
            lights[1].Position = new Vector3(5, 4, 0);
            lights[1].DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            scene.AddNode(lights[1]);
        }

        private class PieceInfo
        {
            public ChessPiece.PieceColor Color;
            public ChessPiece.PieceType Type;
            public string Position;
            public ChessPiece Piece;
            public PieceInfo(ChessPiece.PieceColor color, ChessPiece.PieceType type, string position)
            {
                Color = color;
                Type = type;
                Position = position;
            }
        }

        private class PieceMovement
        {
            public string StartPos;
            public string EndPos;
            public PieceMovement(string startPos, string endPos)
            {
                StartPos = startPos;
                EndPos = endPos;
            }
        }

        PieceInfo[] pieceInfo = new PieceInfo[] {
            new PieceInfo(ChessPiece.PieceColor.White, ChessPiece.PieceType.Pawn, "g6"),
            new PieceInfo(ChessPiece.PieceColor.White, ChessPiece.PieceType.Pawn, "f5"),
            new PieceInfo(ChessPiece.PieceColor.White, ChessPiece.PieceType.Pawn, "b4"),
            new PieceInfo(ChessPiece.PieceColor.White, ChessPiece.PieceType.Pawn, "a3"),
            new PieceInfo(ChessPiece.PieceColor.White, ChessPiece.PieceType.Pawn, "c2"),
            new PieceInfo(ChessPiece.PieceColor.White, ChessPiece.PieceType.Bishop, "c3"),
            new PieceInfo(ChessPiece.PieceColor.White, ChessPiece.PieceType.Bishop, "h3"),
            new PieceInfo(ChessPiece.PieceColor.White, ChessPiece.PieceType.King, "f2"),
            new PieceInfo(ChessPiece.PieceColor.White, ChessPiece.PieceType.Rook, "g1"),
            new PieceInfo(ChessPiece.PieceColor.White, ChessPiece.PieceType.Knight, "e3"),
            new PieceInfo(ChessPiece.PieceColor.Black, ChessPiece.PieceType.Rook, "d8"),
            new PieceInfo(ChessPiece.PieceColor.Black, ChessPiece.PieceType.Rook, "e8"),
            new PieceInfo(ChessPiece.PieceColor.Black, ChessPiece.PieceType.Pawn, "a4"),
            new PieceInfo(ChessPiece.PieceColor.Black, ChessPiece.PieceType.Pawn, "b5"),
            new PieceInfo(ChessPiece.PieceColor.Black, ChessPiece.PieceType.Pawn, "c6"),
            new PieceInfo(ChessPiece.PieceColor.Black, ChessPiece.PieceType.Pawn, "e4"),
            new PieceInfo(ChessPiece.PieceColor.Black, ChessPiece.PieceType.Pawn, "g4"),
            new PieceInfo(ChessPiece.PieceColor.Black, ChessPiece.PieceType.Knight, "e5"),
            new PieceInfo(ChessPiece.PieceColor.Black, ChessPiece.PieceType.Bishop, "f3"),
            new PieceInfo(ChessPiece.PieceColor.Black, ChessPiece.PieceType.King, "h6"),
        };

        PieceMovement[] pieceMovement = new PieceMovement[] {
            new PieceMovement("h3", "g4"),
            new PieceMovement("f3", "g4"),
            new PieceMovement("e3", "g4"),
            new PieceMovement("e5", "g4"),
            new PieceMovement("g1", "g4"),
            new PieceMovement("d8", "d5"),
            new PieceMovement("f5", "f6"),
            new PieceMovement("d5", "d1"),
            new PieceMovement("g6", "g7"),
        };

        private void CreatePieces()
        {
            IScene tempScene = new Scene();
            XLoader.Load("ChessPieces2.X", EffectFactory.CreateFromFile("TiVi.fxo"), TechniqueChooser.MeshPrefix("Reflective"));
            XLoader.AddToScene(tempScene);
            ChessPiece piece;

            foreach (PieceInfo info in pieceInfo)
            {
                piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
                    info.Type, info.Color, info.Position);
                info.Piece = piece;
                chessPieces.Add(piece);
            }

            float time = 1;
            foreach (PieceMovement movement in pieceMovement)
            {
                PieceInfo movePiece = null;
                PieceInfo removePiece = null;
                foreach (PieceInfo info in pieceInfo)
                {
                    if (info.Position == movement.StartPos)
                        movePiece = info;
                    if (info.Position == movement.EndPos)
                        removePiece = info;
                }
                movePiece.Piece.AddPosition(time, 3, movement.EndPos, 1);
                movePiece.Position = movement.EndPos;
                if (removePiece != null)
                {
                    removePiece.Piece.AddPosition(time, 4, movement.EndPos + "-", 1);
                    removePiece.Position = "--";
                }
                time += 4;
            }

            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device, 
            //    ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.White, "g6");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device, 
            //    ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.White, "f5");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device, 
            //    ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.White, "b4");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device, 
            //    ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.White, "a3");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device, 
            //    ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.White, "c2");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Bishop, ChessPiece.PieceColor.White, "c3");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Bishop, ChessPiece.PieceColor.White, "h3");
            //piece.AddPosition(1, 3, "g4", 1);
            //piece.AddPosition(5.2f, 6, "g4-", 1);
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.King, ChessPiece.PieceColor.White, "f2");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Rook, ChessPiece.PieceColor.White, "g1");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Knight, ChessPiece.PieceColor.White, "e3");
            //chessPieces.Add(piece);

            // Black
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Rook, ChessPiece.PieceColor.Black, "d8");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Rook, ChessPiece.PieceColor.Black, "e8");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.Black, "a4");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.Black, "b5");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.Black, "c6");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.Black, "e4");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Pawn, ChessPiece.PieceColor.Black, "g4");
            //piece.AddPosition(1.5F, 4, "g4-", 1);
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Knight, ChessPiece.PieceColor.Black, "e5");
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.Bishop, ChessPiece.PieceColor.Black, "f3");
            //piece.AddPosition(5.0f, 3, "g4", 0.2f);
            //chessPieces.Add(piece);
            //piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
            //    ChessPiece.PieceType.King, ChessPiece.PieceColor.Black, "h6");
            //chessPieces.Add(piece);
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
                    node.WorldState.MoveForward(-1.0f * (y - NUM_SIGNS_Y / 2));
                    node.WorldState.MoveRight(-1.0f * (x - NUM_SIGNS_X / 2));
                    float color = 0.1f + c * 0.2f;
                    node.Model.Materials[0].AmbientColor = new ColorValue(0.05f, 0.05f, 0.05f, 0.05f);
                    node.Model.Materials[0].DiffuseColor = new ColorValue(color, color, color, color);
                    if (c == 0)
                        node.Model.Materials[0].ReflectiveFactor = 0.2f;
                    else
                        node.Model.Materials[0].ReflectiveFactor = 0.1f;
                    c = 1 - c;
                    signs[y * NUM_SIGNS_X + x] = node;
                    scene.AddNode(node);
                }
                c = 1 - c;
            }
        }

        public override void Step()
        {
            //camera.WorldState.Position = new Vector3(0, 3, -10);
            //camera.WorldState.Position = interpolator.GetValue(Time.StepTime % 10);
            camera.WorldState.Position = new Vector3((float)Math.Sin(Time.StepTime * 0.2f), 0.2f, (float)Math.Cos(Time.StepTime * 0.2f)) * 10;
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
            foreach (ChessPiece piece in chessPieces)
                piece.Step(Time.StepTime - StartTime);
            scene.Step();
        }

        public override void Render()
        {
            foreach (ChessPiece piece in chessPieces)
                piece.Render(scene);
            scene.Render();
        }
    }
}
