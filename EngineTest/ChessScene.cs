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

namespace EngineTest
{
    public class ChessScene : BaseDemoEffect
    {
        Scene scene;
        CameraNode camera;
        LightNode[] lightNodes = new LightNode[2];

        public ChessScene(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            scene = new Scene();
            IModel whiteModel = ModelFactory.FromFile("ChamferBox.X", ModelOptions.None);
            IModel blackModel = whiteModel.Clone();
            whiteModel.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
            whiteModel.Materials[0].ReflectiveFactor = 0.02f;
            whiteModel.Materials[0].DiffuseColor = ColorValue.FromColor(Color.White);
            blackModel.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
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
                        new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"), "Glass", model));
                    node.WorldState.Tilt((float)Math.PI / 2);
                    node.WorldState.Position = new Vector3((x - 4) * 1.90f, 0, (y - 4) * 1.90f);
                    scene.AddNode(node);
                }
            }

            camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-10);
            camera.WorldState.MoveUp(2);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;

            lightNodes[0] = new PointLightNode("Light0");
            lightNodes[0].DiffuseColor = new ColorValue(0.8f, 0.6f, 0.2f);
            scene.AddNode(lightNodes[0]);
            lightNodes[1] = new PointLightNode("Light1");
            lightNodes[1].DiffuseColor = new ColorValue(0.8f, 0.8f, 0.4f);
            scene.AddNode(lightNodes[1]);
        }

        public override void Step()
        {
            float t = Time.CurrentTime;
            camera.WorldState.Position =
                new Vector3(20 * (float)Math.Sin(t),
                3, 20 * (float)Math.Cos(t));
            camera.WorldState.Rotation = Quaternion.RotationYawPitchRoll((float)Math.PI + t, 0, 0);

            lightNodes[0].Position = new Vector3(
                7 * (float)Math.Cos(Time.CurrentTime / 2), 2.0f, 
                7 * (float)Math.Sin(Time.CurrentTime / 2));

            lightNodes[1].Position = new Vector3(
                3 * (float)Math.Cos(Time.CurrentTime / 1.2f), 2.0f,
                3 * (float)Math.Sin(Time.CurrentTime / 1.2f));

            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
