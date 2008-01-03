using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.ParticleSystems;

namespace EngineTest
{
    public class BloodCell : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private List<ModelNode> cells = new List<ModelNode>();

        public BloodCell(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 6);
            InitializeSperms();
            InitializeParticles();
            scene.AmbientColor = new Color(30, 30, 30);
        }

        private void InitializeParticles()
        {
            TextureDirector.CreateCircle(0.1f, 0.5f);
            FloaterSystemNode floaterSystem = new FloaterSystemNode("Floaters", 4, 0.1f, 1);
            floaterSystem.Initialize(GraphicsDevice, GraphicsFactory, 100);
            floaterSystem.Material.DiffuseTexture = TextureDirector.Generate(64, 64, 0, SurfaceFormat.Color);
            floaterSystem.Material.BlendFunction = BlendFunction.ReverseSubtract;
            floaterSystem.Material.SourceBlend = Blend.One;
            floaterSystem.Material.DestinationBlend = Blend.One;
            scene.AddNode(floaterSystem);
        }

        private void InitializeSperms()
        {
            TextureDirector.CreatePerlinNoise(1, 6, 0.5f);
            ModelBuilder.SetDiffuseTexture("Default", TextureDirector.Generate(64, 64, 0, SurfaceFormat.Color));
            //ModelBuilder.SetDiffuseTexture("Default", TextureFactory.CreateFromFile("Content\\textures\\CARPTBLU"));
            ModelBuilder.SetAmbientColor("Default", Color.Red);
            ModelBuilder.SetDiffuseColor("Default", Color.Red);
            ModelBuilder.SetSpecularColor("Default", Color.White);
            ModelBuilder.SetShininess("Default", 0.5f);
            ModelBuilder.SetSpecularPower("Default", 8);
            ModelBuilder.SetEffect("Default", "Content\\effects\\BloodCell");
            ModelDirector.CreateSphere(1, 32);
            ModelDirector.Amplitude(cellFunction);
            IModel model = ModelDirector.Generate("Default");
            for (int i = 0; i < 20; i++)
            {
                ModelNode cell = new ModelNode("Cell", model, GraphicsDevice);
                cell.WorldState.Position = Rand.Vector3(-6, 6);
                cells.Add(cell);
                scene.AddNode(cell);
            }
        }

        private Vector3 cellFunction(Vector3 pos)
        {
            const float minY = 0.1f;
            const float maxY = 0.6f;
            const float xLimit = 0.4f;
            Vector2 vec = new Vector2(pos.X, pos.Z);
            float x = vec.Length();
            if (x < xLimit)
                return new Vector3(1, minY, 1);
            x -= xLimit;
            return new Vector3(1, (maxY - minY) * (float)Math.Sin(x * Math.PI / 2) + minY, 1);
        }

        public override void Step()
        {
            Mixer.ClearColor = Color.Coral;
            foreach (ModelNode cell in cells)
            {
                cell.WorldState.Turn(Time.DeltaTime * 0.8f);
                cell.WorldState.Tilt(Time.DeltaTime * 0.94f);
                cell.WorldState.Position += new Vector3(Time.DeltaTime * 1.4f, 0, 0);
                if (cell.WorldState.Position.X > 8)
                    cell.WorldState.Position -= new Vector3(16, 0, 0);
            }
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
