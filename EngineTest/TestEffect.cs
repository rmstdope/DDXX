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

namespace EngineTest
{
    public class TestEffect : BaseDemoEffect
    {
        private MeshNode mesh;
        private LightNode light;
        private ISprite sprite;
        private ITexture texture;

        public TestEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Scene.AmbientColor = new ColorValue(0.3f, 0.3f, 0.3f);

            Model model = ModelFactory.FromFile("../../Data/airplane 2.x", ModelFactory.Options.EnsureTangents);
            IEffect effect = EffectFactory.CreateFromFile("../../../Effects/BlinnPhongShaders.fxo");
            EffectHandler handler = new EffectHandler(effect);
            mesh = new MeshNode("Mesh", model, handler);
            Scene.AddNode(mesh);

            Light dxLight = new Light();
            dxLight.DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            dxLight.Type = LightType.Point;
            light = new LightNode("Point Light", dxLight);
            Scene.AddNode(light);

            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-20.0f);
            Scene.AddNode(camera);
            Scene.ActiveCamera = camera;

            texture = D3DDriver.TextureFactory.CreateFromFile("../../Data/wings.bmp");
            sprite = D3DDriver.Factory.CreateSprite(Device);
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
            //mesh.WorldState.Roll(0.01f);
            //mesh.WorldState.Turn(0.021f);
            light.WorldState.Position = new Vector3(500.0f * (float)Math.Sin(Time.StepTime),
                                                    0.0f,
                                                    500.0f * (float)Math.Cos(Time.StepTime));
        }

        public override void Render()
        {
            IDevice device = D3DDriver.GetInstance().GetDevice();
            Scene.Render();
            sprite.Begin(SpriteFlags.AlphaBlend);
            //device.RenderState.AlphaBlendEnable = true;
            //device.RenderState.AlphaBlendOperation = BlendOperation.Add;
            //device.RenderState.AlphaSourceBlend = Blend.BlendFactor;
            //device.RenderState.AlphaDestinationBlend = Blend.InvBlendFactor;
            //device.RenderState.BlendFactor = Color.FromArgb(128, 128, 128, 128);
            sprite.Draw2D(texture, Rectangle.Empty, SizeF.Empty, new PointF(400.0f, 300.0f), Color.FromArgb(80,255,255,255));
            sprite.End();
        }
    }
}
