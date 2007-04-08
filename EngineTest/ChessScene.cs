using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.DirectX;
using Dope.DDXX.Utility;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.MeshBuilder;

namespace EngineTest
{
    public class ChessScene : BaseDemoEffect
    {
        Scene scene;
        CameraNode camera;
        LightNode[] lightNodes = new LightNode[2];
        MeshBuilder builder;
        List<ModelNode> chessNodes = new List<ModelNode>();
        ModelNode planeNode;
        ModelNode mirrorNode;

        public ChessScene(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            builder = new MeshBuilder(D3DDriver.GraphicsFactory, D3DDriver.TextureFactory,
                D3DDriver.GetInstance().Device);
            scene = new Scene();

            CreatePlane();
            CreateChessBoard();
            CreateCamera();
            CreateLights();
        }

        private void CreatePlane()
        {
            builder.CreatePlane("Box", 3, 3, 6, 6, true);
            builder.AssignMaterial("Box", "Default1");
            builder.SetDiffuseTexture("Default1", "red glass.jpg");
            builder.SetReflectiveTexture("Default1", "rnl_cross.dds");
            builder.SetReflectiveFactor("Default1", 0.02f);
            IModel boxModel = builder.CreateModel("Box");
            IModel mirrorModel = boxModel.Clone();
            boxModel.Materials[0].Diffuse = Color.DarkGray;
            mirrorModel.Materials[0].Diffuse = Color.DarkGray;
            mirrorModel.Materials[0].ReflectiveFactor = 0.0f;
            planeNode = new ModelNode("Box", boxModel,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"), "Glass", boxModel));
            mirrorNode = new ModelNode("Box", mirrorModel,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"), "Glass", mirrorModel));
            planeNode.WorldState.MoveUp(1.5f);
            mirrorNode.WorldState.MoveUp(-1.5f);
            scene.AddNode(planeNode);
            scene.AddNode(mirrorNode);
        }

        private void CreateLights()
        {
            lightNodes[0] = new PointLightNode("Light0");
            lightNodes[0].DiffuseColor = new ColorValue(0.8f, 0.6f, 0.2f, 1.0f);
            scene.AddNode(lightNodes[0]);
            lightNodes[1] = new PointLightNode("Light1");
            lightNodes[1].DiffuseColor = new ColorValue(0.8f, 0.8f, 0.4f, 1.0f);
            scene.AddNode(lightNodes[1]);
        }

        private void CreateCamera()
        {
            camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-10);
            camera.WorldState.MoveUp(2);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
        }

        private void CreateChessBoard()
        {
            IModel whiteModel = ModelFactory.FromFile("ChamferBox.X", ModelOptions.None);
            whiteModel.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
            IModel blackModel = whiteModel.Clone();
            whiteModel.Materials[0].ReflectiveFactor = 0.02f;
            whiteModel.Materials[0].DiffuseColor = ColorValue.FromColor(Color.White);
            blackModel.Materials[0].ReflectiveFactor = 0.002f;
            blackModel.Materials[0].DiffuseColor = ColorValue.FromColor(Color.Black);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    IModel model = blackModel;
                    if ((y & 1) == 1 && (x & 1) == 0)
                        model = whiteModel;
                    if ((y & 1) == 0 && (x & 1) == 1)
                        model = whiteModel;
                    ModelNode node = new ModelNode("FloorTile" + x + y, model,
                        new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"), "Chess", model));
                    node.WorldState.Tilt((float)Math.PI / 2);
                    node.WorldState.Position = new Vector3((x - 4) * 1.90f, 0, (y - 4) * 1.90f);
                    chessNodes.Add(node);
                    //scene.AddNode(node);
                }
            }
        }

        public override void Step()
        {
            MoveCamera();
            MoveLights();

            planeNode.WorldState.Tilt(Time.DeltaTime);
            mirrorNode.WorldState.Tilt(Time.DeltaTime);

            scene.Step();
        }

        private void MoveLights()
        {
            lightNodes[0].Position = new Vector3(
                7 * (float)Math.Cos(Time.CurrentTime / 2), 2.0f,
                7 * (float)Math.Sin(Time.CurrentTime / 2));

            lightNodes[1].Position = new Vector3(
                3 * (float)Math.Cos(Time.CurrentTime / 1.2f), 2.0f,
                3 * (float)Math.Sin(Time.CurrentTime / 1.2f));
        }

        private void MoveCamera()
        {
            float t = Time.CurrentTime;
            camera.WorldState.Position =
                new Vector3(15 * (float)Math.Sin(t),
                3, 15 * (float)Math.Cos(t));
            camera.WorldState.Rotation = Quaternion.RotationYawPitchRoll((float)Math.PI + t, 0, 0);
        }

        public override void Render()
        {
            scene.Render();
            Vector3 cameraPosition = scene.ActiveCamera.Position;
            chessNodes.Sort(delegate(ModelNode node1, ModelNode node2)
            {
                float length1 = (node1.Position - cameraPosition).Length();
                float length2 = (node2.Position - cameraPosition).Length();
                if (length1 < length2)
                    return -1;
                if (length1 > length2)
                    return 1;
                return 0;
            });
            chessNodes.ForEach(delegate (ModelNode node) { node.Render(scene); });
        }
    }
}
