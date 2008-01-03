using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.ModelBuilder;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EngineTest
{
    public class City : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private ModelDirector modelDirector;
        private TextureDirector textureDirector;

        public City(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 20);
            camera.WorldState.MoveUp(2);

            modelDirector = new ModelDirector(ModelBuilder);
            textureDirector = new TextureDirector(TextureBuilder, TextureFactory);

            SetNoiseTexture();
            CreateGround();

            SetBrickTexture();
            CreateHouse();
        }

        private void CreateHouse()
        {
            modelDirector.CreateBox(10, 10, 10);
            modelDirector.Translate(0, 5, 0);
            IModel model = modelDirector.Generate("Default");
            ModelNode house = new ModelNode("Ground", model, GraphicsDevice);
            scene.AddNode(house);
        }

        private void CreateGround()
        {
            modelDirector.CreatePlane(50, 50, 1, 1);
            modelDirector.Rotate(Math.PI / 2, 0, 0);
            IModel model = modelDirector.Generate("Default");
            ModelNode ground = new ModelNode("Ground", model, GraphicsDevice);
            ground.CullMode = CullMode.None;
            scene.AddNode(ground);
        }

        private void SetNoiseTexture()
        {
            ModelBuilder.SetEffect("Default", "Content\\effects\\DefaultEffect");
            // Base circle
            textureDirector.CreateCircle(0.0f, 1.5f);
            textureDirector.Madd(0.3f, 0.0f);
            // Add brush noise
            textureDirector.CreateBrushNoise(3);
            textureDirector.Madd(0.05f, 0.0f);
            textureDirector.Add();
            // Add perlin noise
            textureDirector.CreatePerlinNoise(512, 6, 0.5f);
            textureDirector.Madd(0.04f, 0);
            textureDirector.Add();
            ModelBuilder.SetDiffuseTexture("Default", textureDirector.Generate(256, 256, 0, SurfaceFormat.Color));
        }

        private void SetBrickTexture()
        {
            ModelBuilder.SetEffect("Default", "Content\\effects\\DefaultEffect");
            // Base bricks
            textureDirector.CreateBricks(15, 40, 0.006f);
            //textureDirector.GaussianBlur();
            //textureDirector.Madd(0.3f, 0.0f);
            // Color it
            textureDirector.ColorBlend(Color.Gray.ToVector4(), Color.Tomato.ToVector4());
            // Add brush noise
            textureDirector.CreateBrushNoise(3);
            textureDirector.Madd(0.05f, 0.0f);
            textureDirector.Add();
            // Add perlin noise
            textureDirector.CreatePerlinNoise(64, 6, 0.5f);
            textureDirector.Madd(0.3f, 0);
            textureDirector.Add();
            // Subtract perlin noise
            textureDirector.CreatePerlinNoise(512, 6, 0.5f);
            textureDirector.Madd(0.3f, 0);
            textureDirector.Subtract();
            //textureDirector.Generate(256, 256, 0, SurfaceFormat.Color).Save("bricks.dds", ImageFileFormat.Dds);
            ModelBuilder.SetDiffuseTexture("Default", textureDirector.Generate(256, 256, 0, SurfaceFormat.Color));
        }

        public override void Step()
        {
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
