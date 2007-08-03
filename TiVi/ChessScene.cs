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
using System.Drawing;

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
        // Kasparov är vit och vinner partiet

        private IScene scene;
        private CameraNode camera;
        private Interpolator<InterpolatedVector3> interpolator;
        private List<ChessPiece> chessPieces = new List<ChessPiece>();
        private ChessBoard chessBoard;
        private float winnerTime;
        private ModelNode textNode;
        private ITexture winnerTexture;
        private ITexture startTexture;

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
            startTexture = TextureFactory.CreateFromFile("ChessText1997.jpg");
            winnerTexture = TextureFactory.CreateFromFile("ChessTextWinner.jpg");
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

            MeshDirector director = new MeshDirector(MeshBuilder);
            director.CreatePlane(1, 1, 1, 1, true);
            IModel model = director.Generate("Default1");
            model.Materials[0].Ambient = Color.FromArgb(128, 255, 255, 255);
            textNode = CreateSimpleModelNode(model, "TiVi.fxo", "SimpleWithAlpha");
            foreach (PieceInfo info in pieceInfo)
            {
                piece = new ChessPiece(tempScene, GraphicsFactory, TextureFactory, Device,
                    info.Type, info.Color, info.Position, textNode);
                info.Piece = piece;
                chessPieces.Add(piece);
            }

            float time = 5;
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
            winnerTime = time;
            foreach (PieceInfo info in pieceInfo)
            {
                if (info.Position != "--")// && info.Type != ChessPiece.PieceType.King)
                    info.Piece.AddPosition(time, 4, info.Position + "-", 1);
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
            foreach (ChessPiece piece in chessPieces)
                piece.RenderText(scene, Time.StepTime - StartTime);
            if (Time.StepTime - StartTime < 4)
            {
                textNode.Model.Materials[0].DiffuseTexture = startTexture;
                int alpha = GetAlpha(StartTime);
                textNode.Model.Materials[0].Ambient = Color.FromArgb(0, (alpha * 200) / 255, (alpha * 200) / 255, (alpha * 200) / 255);
                textNode.WorldState.Reset();
                textNode.WorldState.Scale(new Vector3(0.45f, 0.45f * startTexture.GetLevelDescription(0).Height / startTexture.GetLevelDescription(0).Width, 0));
                textNode.WorldState.Rotation = scene.ActiveCamera.WorldState.Rotation;
                textNode.WorldState.Position = scene.ActiveCamera.WorldState.Position + scene.ActiveCamera.WorldState.Forward;
                textNode.WorldState.Position += scene.ActiveCamera.WorldState.Up * (Time.StepTime - StartTime) * 0.03f;
                textNode.WorldState.Position -= scene.ActiveCamera.WorldState.Forward * (Time.StepTime - StartTime) * 0.04f;
                textNode.Render(scene);
            }
            if (Time.StepTime - StartTime > winnerTime)
            {
                textNode.Model.Materials[0].DiffuseTexture = winnerTexture;
                int alpha = GetAlpha(winnerTime + StartTime);
                textNode.Model.Materials[0].Ambient = Color.FromArgb(0, (alpha * 255) / 255, (alpha * 200) / 255, (alpha * 200) / 255);
                textNode.WorldState.Reset();
                textNode.WorldState.Scale(new Vector3(0.45f, 0.45f * winnerTexture.GetLevelDescription(0).Height / winnerTexture.GetLevelDescription(0).Width, 0));
                textNode.WorldState.Rotation = scene.ActiveCamera.WorldState.Rotation;
                textNode.WorldState.Position = scene.ActiveCamera.WorldState.Position + scene.ActiveCamera.WorldState.Forward;
                textNode.WorldState.Position += scene.ActiveCamera.WorldState.Up * (Time.StepTime - (winnerTime + StartTime) - 10) * 0.03f;
                textNode.WorldState.Position -= scene.ActiveCamera.WorldState.Forward * (Time.StepTime - (winnerTime + StartTime)) * 0.04f;
                textNode.Render(scene);
            }
        }

        private int GetAlpha(float startTime)
        {
            int alpha = 255;
            float t = Time.StepTime - startTime;
            if (t < 1.0f)
                alpha = (int)(255 * (t / 1.0f));
            t = Time.StepTime - (startTime + 3);
            if (t > 0)
                alpha = 254 - (int)(255 * (t / 1.0));
            alpha = Math.Min(255, Math.Max(0, alpha));
            return alpha;
        }
    }
}
