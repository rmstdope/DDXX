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
    public class ChessScene : BaseDemoEffect
    {
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

        private IScene scene;
        private CameraNode camera;
        private Interpolator<InterpolatedVector3> interpolator;
        private List<ChessPiece> chessPieces = new List<ChessPiece>();
        private ChessBoard chessBoard;

        public ChessScene(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            IDevice device = Device;
            CreateStandardSceneAndCamera(out scene, out camera, 15);
            CreatePieces();
            CreateCameraInterpolator();
            CreateLights();
            chessBoard = new ChessBoard(scene, MeshBuilder, EffectFactory.CreateFromFile("TiVi.fxo"), Device, 1, 0.4f);
        }

        private void CreateLights()
        {
            PointLightNode[] lights = new PointLightNode[2];
            lights[0] = new PointLightNode("");
            lights[0].Position = new Vector3(-5, 4, 0);
            lights[0].DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            lights[0].Range = 0.02f;
            scene.AddNode(lights[0]);
            lights[1] = new PointLightNode("");
            lights[1].Position = new Vector3(5, 4, 0);
            lights[1].DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            lights[1].Range = 0.02f;
            scene.AddNode(lights[1]);
        }

        private void CreatePieces()
        {
            IScene tempScene = new Scene();
            XLoader.Load("ChessPieces2.X", EffectFactory.CreateFromFile("TiVi.fxo"), TechniqueChooser.MeshPrefix("TiviChessPiece"));
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

        public override void Step()
        {
            camera.WorldState.Position = new Vector3((float)Math.Sin(Time.StepTime * 0.2f), 0.2f, (float)Math.Cos(Time.StepTime * 0.2f)) * 8;
            camera.LookAt(new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            foreach (ChessPiece piece in chessPieces)
                piece.Step(Time.StepTime - StartTime);
            scene.Step();
        }

        public override void Render()
        {
            scene.SetEffectParameters();
            chessBoard.Render(scene);
            foreach (ChessPiece piece in chessPieces)
                piece.RenderMirror(scene);
            scene.Render();
            foreach (ChessPiece piece in chessPieces)
                piece.Render(scene, Time.StepTime - StartTime);
        }
    }
}
