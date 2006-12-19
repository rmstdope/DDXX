using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.ParticleSystems;

namespace EngineTest
{
    public class TestEffect : BaseDemoEffect
    {
        private FloaterSystem ps;
        private CameraNode camera;
        private ISprite sprite;
        private ITexture texture;
        private Color colorValue = Color.FromArgb(100, 20, 30, 40);
        private int intValue = 1;
        private Vector3 vector3Value = new Vector3(1, 1, 1);
        private Scene scene;
        private IFont font;
        private ModelNode mesh;

        public Vector3 Vector3Value
        {
            get { return vector3Value; }
            set { vector3Value = value; }
        }

        public int IntValue
        {
            get { return intValue; }
            set { intValue = value; }
        }

        public Color ColorValue
        {
            get { return colorValue; }
            set { colorValue = value; }
        }

        public TestEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
            scene = new Scene();
        }

        public override void Initialize()
        {
            base.Initialize();

            sprite = D3DDriver.Factory.CreateSprite(Device);
            texture = D3DDriver.TextureFactory.CreateFromFile("BlurBackground.jpg");
            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f);

            camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-400.0f);
            camera.WorldState.MoveRight(50.0f);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;

            ps = new FloaterSystem("System");
            ps.Initialize(50, 200.0f, null);//"BlurBackground.jpg");
            scene.AddNode(ps);

            // Render font
            FontDescription description = new FontDescription();
            description.FaceName = "Arial";
            description.Height = 12;
            font = D3DDriver.Factory.CreateFont(Device, description);
            font.DrawText(null, "Hejsan", new Point(100, 100), Color.FloralWhite);

            // Create mesh
            IEffect effect = D3DDriver.EffectFactory.CreateFromFile("Test.fxo");
            EffectHandler effectHandler = new EffectHandler(effect, "TransparentText");
            IModel model = D3DDriver.ModelFactory.FromFile("Wanting More.x", ModelFactory.Options.None);
            mesh = new ModelNode("Text1", model, effectHandler);
            scene.AddNode(mesh);
            //mesh.WorldState.Tilt(-(float)Math.PI / 2.0f);
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
            float scale = (Time.StepTime % 5.0f) / 5.0f;
            scale *= 2.0f;
            mesh.WorldState.Scaling = new Vector3(scale, scale, scale);
            mesh.WorldState.Position = new Vector3(0, scale * 200.0f, 0);

            mesh.WorldState.Roll(scale / 100.0f);
            mesh.WorldState.Turn(Time.DeltaTime);
            //mesh.WorldState.Tilt(Time.DeltaTime);
            scene.Step();
        }

        public override void Render()
        {
            IDevice device = D3DDriver.GetInstance().Device;
            scene.Render();

            //int sWidth = device.PresentationParameters.BackBufferWidth;
            //int sHeight = device.PresentationParameters.BackBufferHeight;
            //int tWidth = texture.GetSurfaceLevel(0).Description.Width;
            //int tHeight = texture.GetSurfaceLevel(0).Description.Height;
            //sprite.Begin(SpriteFlags.AlphaBlend);
            //sprite.Draw2D(texture, Rectangle.Empty, new SizeF(sWidth * 1.9f, sHeight * 1.9f), new PointF(tWidth / 2, tHeight / 2), Time.StepTime * 0.22f, new PointF(sWidth / 2, sHeight / 2), Color.FromArgb(30, 255, 150, 150));
            //sprite.Draw2D(texture, Rectangle.Empty, new SizeF(sWidth * 1.9f, sHeight * 1.9f), new PointF(tWidth / 2, tHeight / 2), -Time.StepTime * 0.15f, new PointF(sWidth / 2, sHeight / 2), Color.FromArgb(30, 150, 255, 150));
            //sprite.End();
        }

    }
}
