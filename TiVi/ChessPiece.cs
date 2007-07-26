using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;

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

        public ChessPiece(IScene scene, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory, 
            IDevice device, PieceType type, PieceColor color, string startPosition)
        {
            ModelNode originalNode = GetModel(scene, type);
            IModel model = originalNode.Model.Clone();
            ModifyMaterial(textureFactory, color, model);
            modelNode = new ModelNode("", model, originalNode.EffectHandler, device);
            SetRotationAndScaling(type);
            AddPosition(0, 0, startPosition, 0);
            CreateLineNode(graphicsFactory, device);
            CreateMirrorNode();
        }

        private void CreateLineNode(IGraphicsFactory graphicsFactory, IDevice device)
        {
            ILine line = graphicsFactory.CreateLine(device);
            line.Antialias = true;
            lineNode = new LineNode("", line, new Vector3(0, 0, 0), new Vector3(0, -ROPE_LENGTH, 0), Color.Gray);
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
            }
            lineNode.Position += new Vector3(0, ROPE_LENGTH, 0);
        }

        public void RenderMirror(IScene scene)
        {
            mirrorNode.Render(scene);
        }

        public void Render(IScene scene)
        {
            lineNode.Render(scene);
        }
    }
}
