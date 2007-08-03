using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.DemoEffects;

namespace TiVi
{
    public class ChessPiece
    {
        public enum PieceType
        {
            Pawn = 0,
            Rook,
            Bishop,
            Queen,
            King,
            Knight,
            NumPieces
        }

        public enum PieceColor
        {
            Black,
            White
        }

        private class PositionInfo
        {
            public float StartTime;
            public float EndTime;
            public float Duration;
            public Vector3 Position;
            public float MaxY;
            public PositionInfo(float startTime, float duration, Vector3 position, float maxY)
            {
                StartTime = startTime;
                EndTime = startTime + duration;
                Duration = duration;
                Position = position;
                MaxY = maxY;
            }
        }

        private ModelNode modelNode;
        private MirrorNode mirrorNode;
        private LineNode lineNode;
        private List<PositionInfo> positions = new List<PositionInfo>();
        private const float ROPE_LENGTH = 2.0f;
        private PieceColor color;
        private Vector3 distortion;
        private static int direction = 0;
        private ITexture texture;
        private IDevice device;
        private Vector3 textPosition;
        private ModelNode textNode;
        private bool doText;
        private int alpha;

        public ChessPiece(IScene scene, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory, 
            IDevice device, PieceType type, PieceColor color, string startPosition, ModelNode textModel)
        {
            ModelNode originalNode = GetModel(scene, type);
            IModel model = originalNode.Model.Clone();
            ModifyMaterial(textureFactory, color, model);
            modelNode = new ModelNode("", model, originalNode.EffectHandler, device);
            SetRotationAndScaling(type);
            AddPosition(-2, 4, startPosition, 5);
            CreateLineNode(graphicsFactory, device);
            CreateMirrorNode();
            texture = (color == PieceColor.Black) ? 
                textureFactory.CreateFromFile("ChessTextDeepBlue.jpg") :
                textureFactory.CreateFromFile("ChessTextKasparov.jpg");
            this.device = device;

            this.color = color;
            distortion = new Vector3(0/*Rand.Float(0.1f, 0.3f)*/, (float)(direction * Math.PI / 2), 0);
            direction+=3;
            textNode = textModel;
        }

        private void CreateLineNode(IGraphicsFactory graphicsFactory, IDevice device)
        {
            ILine line = graphicsFactory.CreateLine(device);
            line.Antialias = true;
            lineNode = new LineNode("", line, new Vector3(0, 0, 0), new Vector3(0, 0/*-ROPE_LENGTH*/, 0), Color.Gray);
            modelNode.Position = new Vector3(0, -ROPE_LENGTH, 0);
            lineNode.AddChild(modelNode);
        }

        private void CreateMirrorNode()
        {
            mirrorNode = new MirrorNode(lineNode);
        }

        public void AddPosition(float startTime, float duration, string position, float maxY)
        {
            positions.Add(new PositionInfo(startTime, duration, TranslatePosition(position), maxY));
        }

        private Vector3 TranslatePosition(string position)
        {
            Vector3 vec = new Vector3((position[0] - 'a' + 1) - 4,
                0, int.Parse(position.Substring(1, 1)) - 4);
            if (position.Length == 3 && position[2] == '-')
                vec.Y = 10;
            return vec;
        }

        private void SetRotationAndScaling(PieceType type)
        {
            if (type == PieceType.Knight)
                modelNode.WorldState.Scale(0.00007f);
            else
                modelNode.WorldState.Scale(0.0003f);
            modelNode.WorldState.Tilt((float)Math.PI / 2);
        }

        private static void ModifyMaterial(ITextureFactory textureFactory, PieceColor color, IModel model)
        {
            model.Materials[0].ReflectiveTexture = textureFactory.CreateCubeFromFile("rnl_cross.dds");
            model.Materials[0].ReflectiveFactor = 0.3f;// 0.8f;
            if (color == PieceColor.Black)
            {
                model.Materials[0].DiffuseTexture = textureFactory.CreateFromFile("marble2.jpg");
                //model.Materials[0].AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
                model.Materials[0].AmbientColor = new ColorValue(0.3f, 0.3f, 0.3f, 0.3f);
                model.Materials[0].DiffuseColor = new ColorValue(0.8f, 0.8f, 0.8f, 0.8f);
            }
            else
            {
                model.Materials[0].DiffuseTexture = textureFactory.CreateFromFile("marble.jpg");
                //model.Materials[0].AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
                model.Materials[0].AmbientColor = new ColorValue(0.3f, 0.3f, 0.3f, 0.3f);
                model.Materials[0].DiffuseColor = new ColorValue(0.8f, 0.8f, 0.8f, 0.8f);
            }
        }

        private static ModelNode GetModel(IScene scene, PieceType type)
        {
            switch (type)
            {
                case PieceType.Bishop:
                    return scene.GetNodeByName("Bishop") as ModelNode;
                case PieceType.King:
                    return scene.GetNodeByName("King") as ModelNode;
                case PieceType.Knight:
                    return scene.GetNodeByName("Knight") as ModelNode;
                case PieceType.Pawn:
                    return scene.GetNodeByName("Pawn") as ModelNode;
                case PieceType.Queen:
                    return scene.GetNodeByName("Queen") as ModelNode;
                case PieceType.Rook:
                    return scene.GetNodeByName("Rook") as ModelNode;
            }
            throw new DDXXException("Unknown type.");
        }

        public void Step(float time)
        {
            doText = false;
            Vector3 oldPos = new Vector3();
            foreach (PositionInfo info in positions)
            {
                if (time >= info.EndTime)
                {
                    lineNode.Position = info.Position;
                }
                else if (time >= info.StartTime)
                {
                    Vector3 pos = SmoothInterval.SineInterpolation(info.StartTime, info.EndTime, time, oldPos, info.Position);
                    if (pos.Y == 0)
                        pos.Y = SmoothInterval.ScaledSine(info.StartTime, info.EndTime, time, info.MaxY);
                    lineNode.Position = pos;

                    float d = SmoothInterval.ScaledSine(info.StartTime, info.EndTime, time, 1);
                    lineNode.WorldState.ResetRotation();
                    lineNode.WorldState.Turn((Time.StepTime - info.StartTime) * 0.01f * d);
                    lineNode.WorldState.Roll((float)Math.Sin(Time.StepTime * 4.0f) * 0.2f * d);
                }
                oldPos = lineNode.Position;

                // Set text fading parameters
                if (time >= info.StartTime - 1 && time <= info.StartTime - 1 + 5)
                {
                    if (info.Position.Y != 10 && info.StartTime >= 0)
                    {
                        Vector3 posAdd = new Vector3((float)Math.Sin(distortion.X * time + distortion.Y),
                                                     (float)Math.Cos(distortion.X * time + distortion.Y), 
                                                     3);
                        posAdd *= (time - info.StartTime) * 0.01f;
                        textPosition = (color == PieceColor.Black) ? 
                            new Vector3(0.3f, 0.7f, 0) + posAdd:
                            new Vector3(0.7f, 0.7f, 0) + posAdd;
                        doText = true;
                        alpha = 255;
                        float t = time - (info.StartTime - 1);
                        if (t < 1.0f)
                            alpha = (int)(255 * (t / 1.0f));
                        t = time - (info.StartTime - 1 + 5 - 1);
                        if (t > 0)
                            alpha = 254 - (int)(255 * (t / 1.0));
                        alpha = Math.Min(255, Math.Max(0, alpha));
                    }
                }
            }
            lineNode.Position += new Vector3(0, ROPE_LENGTH, 0);
        }

        public void RenderMirror(IScene scene)
        {
            mirrorNode.Render(scene);
        }

        public void Render(IScene scene, float time)
        {
            lineNode.Render(scene);
        }
        public void RenderText(IScene scene, float time)
        {
            if (doText)
            {
                textNode.Model.Materials[0].DiffuseTexture = texture;
                textNode.Model.Materials[0].Ambient = (color == PieceColor.Black) ?
                    Color.FromArgb(0, (alpha * 200) / 255, (alpha * 200) / 255, (alpha * 255) / 255) :
                    Color.FromArgb(0, (alpha * 255) / 255, (alpha * 200) / 255, (alpha * 200) / 255);
                textNode.WorldState.Reset();
                textNode.WorldState.Scale(new Vector3(0.45f, 0.45f * texture.GetLevelDescription(0).Height / texture.GetLevelDescription(0).Width, 0));
                textNode.WorldState.Rotation = scene.ActiveCamera.WorldState.Rotation;
                textNode.WorldState.Position = scene.ActiveCamera.WorldState.Position + scene.ActiveCamera.WorldState.Forward;
                textNode.WorldState.Position += scene.ActiveCamera.WorldState.Right * (textPosition.X - 0.5f);
                textNode.WorldState.Position += scene.ActiveCamera.WorldState.Up * (0.5f - textPosition.Y);
                textNode.WorldState.Position -= scene.ActiveCamera.WorldState.Forward * (textPosition.Z);
                textNode.Render(scene);
            }
        }
    }
}
