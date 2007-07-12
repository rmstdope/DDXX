using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.ParticleSystems;

namespace TiVi
{
    public class TestEffect : BaseDemoEffect
    {
        private ISprite sprite;
        private ITexture texture;
        private ParticleSystemNode system;
        private IScene scene;
        private CameraNode camera;

        public TestEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 10);
            sprite = GraphicsFactory.CreateSprite(Device);

            TextureDirector director = new TextureDirector(TextureBuilder);
            director.CreateCircle(0.2f, 0.5f);
            director.CreatePerlinNoise(16, 6, 0.5f);
            director.Modulate();
            texture = director.Generate(256, 256, 1, Format.A8R8G8B8);

            system = new ParticleSystemNode("");
            CloudParticleSpawner spawner = new CloudParticleSpawner(GraphicsFactory, Device, 10);
            system.Initialize(spawner, Device, GraphicsFactory, EffectFactory, texture);
            scene.AddNode(system);
        }

        public override void Step()
        {
            camera.WorldState.MoveForward(Time.DeltaTime * 3.0f);
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();

            //sprite.Begin(SpriteFlags.None);
            //sprite.Draw2D(texture, Rectangle.Empty, SizeF.Empty, new PointF(512, 0), Color.White);
            //sprite.End();
        }
    }
}
