using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;

namespace EngineTest
{
    public class BloodCell : BaseDemoEffect
    {
        private CameraNode camera;
        private List<ModelNode> cells = new List<ModelNode>();
        private ModelNode artery;
        private float pulseSpeed;
        private float pulseFrequency;
        private float pulseAmplitude;
        private PointLightNode light;

        private MaterialHandler material;
        public MaterialHandler Material
        {
            get { return material; }
            set { material = value; }
        }

        public float PulseSpeed
        {
            get { return pulseSpeed; }
            set { pulseSpeed = value; }
        }
        public float PulseFrequency
        {
            get { return pulseFrequency; }
            set { pulseFrequency = value; }
        }
        [TweakStep(0.01f)]
        public float PulseAmplitude
        {
            get { return pulseAmplitude; }
            set { pulseAmplitude = value; }
        }

        public BloodCell(string name, float start, float end)
            : base(name, start, end)
        {
            PulseSpeed = 14;
            PulseFrequency = 100.0f;
            PulseAmplitude = 0.1f;
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, 0);
            InitializeCells();
            InitializeArtery();
            //InitializeParticles();
            light = new PointLightNode("Light");
            //light.Position = new Vector3(1000, 0, 0);
            light.Position = new Vector3(0, 0, 0);
            Scene.AddNode(light);
            Scene.AmbientColor = new Color(255, 255, 255);
        }

        private void InitializeArtery()
        {
            MaterialHandler material = new MaterialHandler(EffectFactory.CreateFromFile("Content\\effects\\Artery"), new EffectConverter());
            material.DiffuseTexture = TextureFactory.CreateFromName("Noise256");
            ModelDirector.CreateTunnel(2.0f, 32, 30, 60, 2, 10, true);
            PerlinNoise heightMap = new PerlinNoise();
            heightMap.BaseFrequency = 16;
            Madd madd = new Madd();
            madd.Mul = 0.2f;
            madd.ConnectToInput(0, heightMap);
            ModelDirector.HeightMap(madd);
            material.NormalTexture = TextureFactory.CreateFromName("NormalNoise256");
            material.AmbientColor = Color.Black;
            material.DiffuseColor = Color.Red;
            material.Shininess = 0.0f;
            CustomModel model = ModelDirector.Generate(material);
            artery = new ModelNode("Artery", model, GraphicsDevice);
            Scene.AddNode(artery);
            artery.WorldState.Tilt(MathHelper.PiOver2);
        }

        private void InitializeParticles()
        {
            TextureDirector.CreateCircle(0.1f, 0.3f, 0.5f, 0.5f, new Vector2(0.5f, 0.5f));
            //FloaterSystemNode floaterSystem = new FloaterSystemNode("Floaters", 4, 0.1f, 1);
            //floaterSystem.Initialize(GraphicsFactory, 100);
            //floaterSystem.Material.DiffuseTexture = TextureDirector.Generate("Circle64", 64, 64, 0, SurfaceFormat.Color);
            //floaterSystem.Material.BlendFunction = BlendFunction.ReverseSubtract;
            //floaterSystem.Material.SourceBlend = Blend.One;
            //floaterSystem.Material.DestinationBlend = Blend.One;
            //Scene.AddNode(floaterSystem);
        }

        private void InitializeCells()
        {
            TextureDirector.CreatePerlinNoise(1, 6, 0.5f);
            ModelBuilder.SetDiffuseTexture("Default", TextureDirector.Generate("Noise64", 64, 64, false, SurfaceFormat.Color));
            //ModelBuilder.SetDiffuseTexture("Default", TextureFactory.CreateFromFile("Content\\textures\\CARPTBLU"));
            ModelBuilder.SetAmbientColor("Default", Color.Red);
            ModelBuilder.SetDiffuseColor("Default", new Color(255, 100, 100));
            ModelBuilder.SetSpecularColor("Default", Color.White);
            ModelBuilder.SetShininess("Default", 0.5f);
            ModelBuilder.SetSpecularPower("Default", 8);
            ModelBuilder.SetEffect("Default", "Content\\effects\\BloodCell");
            ModelDirector.CreateSphere(1, 32);
            ModelDirector.Amplitude(cellFunction);
            ModelDirector.Scale(0.3f);
            CustomModel model = ModelDirector.Generate("Default");
            for (int i = 0; i < 20; i++)
            {
                ModelNode cell = new ModelNode("Cell", model, GraphicsDevice);
                cell.WorldState.Position = new Vector3(Rand.Float(-1, 1), Rand.Float(-1, 1), Rand.Float(-5, 5));
                cells.Add(cell);
                Scene.AddNode(cell);
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
            foreach (ModelNode cell in cells)
            {
                cell.WorldState.Turn(Time.DeltaTime * 0.8f);
                cell.WorldState.Tilt(Time.DeltaTime * 0.94f);
                cell.WorldState.Position += new Vector3(0, 0, Time.DeltaTime * 1.4f);
                if (cell.WorldState.Position.Z > 8)
                    cell.WorldState.Position -= new Vector3(0, 0, 16);
            }
            artery.Model.Meshes[0].MeshParts[0].MaterialHandler.Effect.Parameters["time"].SetValue(Time.CurrentTime);
            artery.Model.Meshes[0].MeshParts[0].MaterialHandler.Effect.Parameters["PulseSpeed"].SetValue(PulseSpeed);
            artery.Model.Meshes[0].MeshParts[0].MaterialHandler.Effect.Parameters["PulseFrequency"].SetValue(PulseFrequency / 60.0f);
            artery.Model.Meshes[0].MeshParts[0].MaterialHandler.Effect.Parameters["Amplitude"].SetValue(PulseAmplitude);

            //artery.WorldState.Tilt(Time.DeltaTime);
            camera.Position = new Vector3(0, 0, 10);
            //camera.WorldState.Turn(Time.DeltaTime);
            //light.Position = camera.Position; new Vector3(0, 0, (float)Math.Sin(Time.CurrentTime) * 5);

            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
