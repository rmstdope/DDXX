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
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.TextureBuilder;

namespace EngineTest
{
    public class TestEffect : BaseDemoEffect
    {
        private ParticleSystemNode floaterSystem;
        private ParticleSystemNode starSystem;
        private ParticleSystemNode spiralSystem1;
        private CameraNode camera;
        private IScene scene;
        private ModelNode modelNode;
        private ModelNode clothModel = null;
        private IBoundingObject sphere = null;
        private float reflectiveFactor;
        private PointLightNode light = null;
        private ISprite sprite;
        private ITexture generatedTexture1;
        private ITexture generatedTexture2;
        private ILine line;
        private List<float> yPos = new List<float>();
        //private ModelNode terrainModel = null;

        public float ReflectiveFactor
        {
            get { return reflectiveFactor; }
            set { reflectiveFactor = value; }
        }

        //private ModelNode modelSkinning;
        private ModelNode modelNoSkinning;

        public TestEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
            scene = new Scene();
        }

        private Vector4 circleCallback(Vector2 texCoord, Vector2 texelSize)
        {
            Vector2 centered = texCoord - new Vector2(0.5f, 0.5f);
            float distance = centered.Length();
            if (distance < 0.1f)
                return new Vector4(1, 1, 1, 1);
            else if (distance < 0.5f)
            {
                float scaled = (0.5f - distance) / 0.4f;
                return new Vector4(scaled, scaled, scaled, scaled);
            }
            return new Vector4(0, 0, 0, 0);
        }

        protected override void Initialize()
        {
            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f);

            camera = new CameraNode("MyCamera");
            camera.SetClippingPlanes(1, 10000);
            //camera.WorldState.Tilt(2.0f);
            camera.WorldState.MoveForward(-3.0f);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;

            ITexture circleTexture = TextureFactory.CreateFromFunction(64, 64, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback);

            floaterSystem = new ParticleSystemNode("FloaterSystem");
            ISystemParticleSpawner spawner = new FloaterParticleSpawner(GraphicsFactory, Device, 50, 100.0f);
            floaterSystem.Initialize(spawner, Device, GraphicsFactory, EffectFactory, circleTexture);
            floaterSystem.WorldState.MoveForward(500.0f);
            floaterSystem.WorldState.MoveRight(100.0f);
            //scene.AddNode(floaterSystem);

            starSystem = new ParticleSystemNode("StarSystem");
            spawner = new FallingStarParticleSpawner(GraphicsFactory, Device, 100);
            starSystem.Initialize(spawner, Device, GraphicsFactory, EffectFactory, circleTexture);
            starSystem.WorldState.MoveForward(500.0f);
            starSystem.WorldState.MoveRight(-100.0f);
            //scene.AddNode(starSystem);

            spiralSystem1 = new ParticleSystemNode("SpiralSystem1");
            //spiralSystem2 = new ParticleSystemNode("SpiralSystem2");
            spawner = new SpiralParticleSpawner(GraphicsFactory, Device, 50000);
            spiralSystem1.Initialize(spawner, Device, GraphicsFactory, EffectFactory, circleTexture);
            spawner = new SpiralParticleSpawner(GraphicsFactory, Device, 50000);
            //spiralSystem2.Initialize(spawner, Device, GraphicsFactory, EffectFactory, null);
            spiralSystem1.WorldState.MoveForward(500.0f);
            //spiralSystem2.WorldState.MoveForward(500.0f);
            spiralSystem1.WorldState.MoveUp(100.0f);
            //spiralSystem2.WorldState.MoveUp(100.0f);
            scene.AddNode(spiralSystem1);
            //scene.AddNode(spiralSystem2);

            //generator1.SetColors(Color.Black, Color.Blue);
            //IGenerator generator2 = new ColorModulationGenerator(generator1, Color.Red);

            InitializeTextures();

            //IGenerator generator = new PerlinNoise(5, 16, 0.5f);
            //MeshBuilder builder = new MeshBuilder(GraphicsFactory, TextureFactory, Device);
            //builder.CreateTerrain("Terrain", generator, 10.0f, 100.0f, 100.0f, 50, 50, true);
            //builder.AssignMaterial("Terrain", "Default1");
            //builder.SetDiffuseTexture("Default1", "square.tga");
            //IModel model = builder.CreateModel("Terrain");
            //model.Mesh.ComputeNormals();
            //model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.1f);
            //model.Materials[0].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f);
            //terrainModel = new ModelNode("Terrain", model,
            //    new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"),
            //    delegate(int material) { return "Terrain"; }, model));
            //scene.AddNode(terrainModel);
            //terrainModel.WorldState.MoveUp(-6);
            

            //AddWantingMoreModel();

            //AddUnskinnedModel();

            //LoadFlyScene();

            //TestMeshBuilder();

            line = GraphicsFactory.CreateLine(Device);
            for (int i = 0; i < 10; i++)
                yPos.Add(Rand.Int(0, 400));

            //scene.DebugPrintGraph();
            scene.Validate();
        }

        //private void TestMeshBuilder()
        //{
        //    MeshBuilder builder = new MeshBuilder(D3DDriver.GraphicsFactory, D3DDriver.TextureFactory,
        //        D3DDriver.GetInstance().Device);
        //    const int numSides = 10;
        //    Body body = new Body();
        //    //body.Gravity = new Vector3(0, -0.2f, 0);

        //    int[] pinned = new int[numSides + 1];
        //    for (int i = 0; i < numSides + 1; i++)
        //        pinned[i] = i;
        //    builder.CreateCloth("Cloth", body, 2, 2, numSides, numSides,
        //        pinned, true);
        //    builder.AssignMaterial("Cloth", "Default1");
        //    builder.SetDiffuseTexture("Default1", "red glass.jpg");
        //    builder.SetReflectiveTexture("Default1", "rnl_cross.dds");
        //    builder.SetReflectiveFactor("Default1", 0.2f);

        //    IModel model = builder.CreateModel("Cloth");
        //    model.Materials[0].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f);
        //    clothModel = new ModelNode("Cloth", model,
        //        new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"),
        //        delegate(int material) { return "Glass"; }, model));
        //    scene.AddNode(clothModel);

        //    // Fix sphere
        //    sphere = new BoundingSphere(0.5f);
        //    for (int i = 0; i < body.Particles.Count; i++)
        //        body.AddConstraint(new BoundingConstraint(body.Particles[i], sphere));

        //    model = builder.CreateSkyBoxModel("SkyBox", "rnl_cross.dds");
        //    ModelNode skyBoxModel = new ModelNode("SkyBox", model,
        //        new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"),
        //        delegate(int material) { return "SkyBox"; }, model));
        //    scene.AddNode(skyBoxModel);

        //    light = new PointLightNode("");
        //    light.DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f);
        //    scene.AddNode(light);
        //}

        private void LoadFlyScene()
        {
            XLoader.Load("Flyscene.x", EffectFactory.CreateFromFile("Test.fxo"),
                TechniqueChooser.MeshPrefix("Skinning"));
            XLoader.AddToScene(scene);
            scene.ActiveCamera = scene.GetNodeByName("Camera") as CameraNode;
        }

        private void AddWantingMoreModel()
        {
            IEffect effect = D3DDriver.EffectFactory.CreateFromFile("Test.fxo");
            IModel model = D3DDriver.ModelFactory.FromFile("Wanting More.x", ModelOptions.None);
            EffectHandler effectHandler = new EffectHandler(effect,
                delegate(int material) { return "TransparentText"; }, model);
            modelNode = new ModelNode("Text1", model, effectHandler, Device);
            scene.AddNode(modelNode);
        }

        private void AddUnskinnedModel()
        {
            IModel model = ModelFactory.FromFile("TiVi.x", ModelOptions.None);
            modelNoSkinning = new ModelNode("No Skinning",
                model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"),
                delegate(int material) { return "Skinning"; }, model), Device);
            modelNoSkinning.WorldState.Scale(100.0f);
            modelNoSkinning.WorldState.MoveRight(-50);
            modelNoSkinning.WorldState.Roll((float)Math.PI);
            modelNoSkinning.WorldState.Tilt((float)Math.PI / 2);
            scene.AddNode(modelNoSkinning);
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        private void InitializeTextures()
        {
            Dope.DDXX.TextureBuilder.TextureBuilder builder = new Dope.DDXX.TextureBuilder.TextureBuilder(TextureFactory);
            sprite = GraphicsFactory.CreateSprite(Device);

            //IGenerator generator1 = new MarbleGenerator(1, 2, 4.0f, 3.0f);
            //IGenerator generator1 = new PerlinNoise(6, 8, 0.5f);
            IGenerator bricks = new Bricks(4, 8, 0.025f);
            PerlinNoise noise = new PerlinNoise();
            noise.BaseFrequency = 32;
            PerlinNoise noise2 = new PerlinNoise();
            noise2.NumOctaves = 4;
            noise2.BaseFrequency = 128;
            IGenerator madd = new Madd(0.5f, 0.5f);
            IGenerator madd2 = new Madd(1.0f, 0.6f);
            IGenerator modulate = new Modulate();
            IGenerator factor = new FactorBlend(0.5f);
            IGenerator colorModulation = new ColorBlend(new Vector4(0.3f, 0.3f, 0.3f, 1), new Vector4(0.89f, 0.45f, 0.36f, 1));

            madd.ConnectToInput(0, noise);
            madd2.ConnectToInput(0, noise2);
            modulate.ConnectToInput(0, bricks);
            modulate.ConnectToInput(1, madd);
            factor.ConnectToInput(0, bricks);
            factor.ConnectToInput(1, madd2);

            //generator2 = new ColorModulation(new Vector4(0.8f, 0.1f, 0.1f, 1));
            colorModulation.ConnectToInput(0, factor);
            generatedTexture1 = builder.Generate(madd2, 256, 256, 1, Format.A8R8G8B8);

            IGenerator rect = new RoundedRectangle(new Vector2(1.0f, 0.9f), new Vector2(0.5f, 0.5f), 0.1f);
            colorModulation.ConnectToInput(0, rect);
            generatedTexture2 = builder.Generate(rect, 256, 256, 1, Format.A8R8G8B8);
        }

        public override void Step()
        {
            if (clothModel != null)
            {
                clothModel.Model.Materials[0].ReflectiveFactor = reflectiveFactor;
            }
            // Rotate camera
            //camera.WorldState.Tilt(Time.DeltaTime / 2);
            //camera.WorldState.Turn(Time.DeltaTime / 1.456f);

            if (light != null)
                light.Position = new Vector3(2 * (float)Math.Sin(Time.CurrentTime), 0,
                    2 * (float)Math.Cos(Time.CurrentTime));

            if (sphere != null)
            {
                // Move sphere
                Vector3 pos = new Vector3(0, 0.5f, 2 * (float)Math.Cos(Time.CurrentTime / 4));
                sphere.Center = pos;
            }

            // Long period
            Vector3 direction = new Vector3(
                (float)Math.Sin(Time.CurrentTime / 5), 
                0, 
                (float)Math.Cos(Time.CurrentTime / 5));
            Vector3 force = new Vector3(0, 0, -(float)Math.Abs(Math.Cos(Time.CurrentTime)) * 10);
            //((PhysicalModel)clothModel.Model).Body.ApplyForce(force);

            if (modelNode != null)
            {
                float scale = (Time.StepTime % 5.0f) / 5.0f;
                scale *= 2.0f;
                modelNode.WorldState.Scaling = new Vector3(scale, scale, scale);
                modelNode.WorldState.Position = new Vector3(0, scale * 200.0f, 0);

                modelNode.WorldState.Roll(scale / 100.0f);
                modelNode.WorldState.Turn(Time.DeltaTime);
            }

            spiralSystem1.WorldState.Turn(-Time.DeltaTime * 0.05f);

            scene.Step();
        }

        public override void Render()
        {
            //for (int y = 0; y < 2; y++)
            //{
            //    for (int x = 0; x < 2; x++)
            //    {
            //        Viewport viewport = Device.Viewport;
            //        viewport.Width = 400;
            //        viewport.Height = 300;
            //        viewport.Y = y * 300;
            //        viewport.X = x * 400;
            //        Device.Viewport = viewport;

            //        scene.Render();
            //    }
            //}
            scene.Render();
            //sprite.Begin(SpriteFlags.None);
            //Device.RenderState.AlphaBlendEnable = false;
            //if (generatedTexture1 != null)
            //{
            //    sprite.Draw2D(generatedTexture1, Rectangle.Empty, new SizeF(256, 256), new PointF(100, 44),
            //        Color.FromArgb(255, Color.White));
            //    sprite.Draw2D(generatedTexture1, Rectangle.Empty, new SizeF(256, 256), new PointF(100, 300),
            //        Color.FromArgb(255, Color.White));
            //}
            //if (generatedTexture2 != null)
            //{
            //    sprite.Draw2D(generatedTexture2, Rectangle.Empty, new SizeF(256, 256), new PointF(356, 44),
            //        Color.FromArgb(255, Color.White));
            //    sprite.Draw2D(generatedTexture2, Rectangle.Empty, new SizeF(256, 256), new PointF(356, 300),
            //        Color.FromArgb(255, Color.White));
            //}
            //sprite.End();

            Vector2[] points = new Vector2[9 * 10];
            float xPos = 0;
            int p = 0;
            for (int i = 0; i < 9; i++)
            {
                float v0;
                float v1;
                float v2;
                float v3;
                v0 = yPos[i == 0 ? i : i - 1];
                v1 = yPos[i];
                v2 = yPos[i + 1];
                v3 = yPos[i < 8 ? i + 2: i + 1];
                for (int x = 0; x < 10; x++)
                {
                    float d = x / 10.0f;
                    float P = (v3 - v2) - (v0 - v1);
                    float Q = (v0 - v1) - P;
                    float R = v2 - v0;
                    float S = v1;
                    xPos += 8.0f;
                    points[p++] = new Vector2(xPos, P * d * d * d + Q * d * d + R * d + S); 
                }
            }
            line.Begin();
            line.Draw(points, Color.White);
            line.End();
        }

    }
}
