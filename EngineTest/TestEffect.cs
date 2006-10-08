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

        public TestEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            sprite = D3DDriver.Factory.CreateSprite(Device);
            texture = D3DDriver.TextureFactory.CreateFromFile("BlurBackground.jpg");
            Scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f);

            camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-400.0f);
            camera.WorldState.MoveRight(50.0f);
            Scene.AddNode(camera);
            Scene.ActiveCamera = camera;

            ps = new FloaterSystem("System");
            ps.Initialize(10, 200.0f);
            Scene.AddNode(ps);
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
            Scene.Step();
        }

        public override void Render()
        {
            IDevice device = D3DDriver.GetInstance().GetDevice();
            Scene.Render();
            int sWidth = device.PresentationParameters.BackBufferWidth;
            int sHeight = device.PresentationParameters.BackBufferHeight;
            int tWidth = texture.GetSurfaceLevel(0).Description.Width;
            int tHeight = texture.GetSurfaceLevel(0).Description.Height;
            sprite.Begin(SpriteFlags.AlphaBlend);
            sprite.Draw2D(texture, Rectangle.Empty, new SizeF(sWidth * 1.9f, sHeight * 1.9f), new PointF(tWidth / 2, tHeight / 2), Time.StepTime * 0.22f, new PointF(sWidth / 2, sHeight / 2), Color.FromArgb(30, 255, 150, 150));
            sprite.Draw2D(texture, Rectangle.Empty, new SizeF(sWidth * 1.9f, sHeight * 1.9f), new PointF(tWidth / 2, tHeight / 2), -Time.StepTime * 0.15f, new PointF(sWidth / 2, sHeight / 2), Color.FromArgb(30, 150, 255, 150));
            sprite.End();
        }

    }
}
