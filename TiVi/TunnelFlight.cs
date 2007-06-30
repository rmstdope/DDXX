using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.MeshBuilder;
using System.Drawing;
using Dope.DDXX.ParticleSystems;

namespace TiVi
{
    public class TunnelFlight : BaseDemoEffect
    {
        private struct Brick
        {
            public ModelNode Model;
            public float StartTime;
            public Brick(ModelNode model, float startTime)
            {
                Model = model;
                StartTime = startTime;
            }
        }

        private const int NUM_RINGS = 6;
        private const int NUM_LIGHTS = 2;
        private const int NUM_BLOCKS = 24;
        private IScene scene;
        private CameraNode camera;
        private List<PointLightNode> lights = new List<PointLightNode>();
        private List<Brick> bricks = new List<Brick>();
        private List<Brick> walkway = new List<Brick>();
        private ModelNode discModel;
        private ModelNode discModel2;
        private ModelNode tiviNode;
        private ModelNode mirrorNode;
        private MeshDirector director;
        private List<ModelNode> diamonds = new List<ModelNode>();
        private ModelNode plane;
        private ModelNode cylinder;
        private ModelNode blackPlane;

        public TunnelFlight(float startTime, float endTime)
            : base(startTime, endTime)
        {
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
                return new Vector4(scaled, scaled, scaled, 1);
            }
            return new Vector4(0, 0, 0, 0);
        }

        protected override void Initialize()
        {
            director = new MeshDirector(MeshBuilder);

            CreateStandardSceneAndCamera(out scene, out camera, 10);
            camera.WorldState.MoveUp(1.5f);

            CreateNoiseTexture();
            //CreateBricks();
            CreateLights();
            //CreateTerrain();
            CreateDiamonds();
            //CreateDiscs();
            CreateTiVi();
            CreateWalkway();

            //GlitterParticleSpawner spawner = new GlitterParticleSpawner(GraphicsFactory, Device, 500);
            //ParticleSystemNode system = new ParticleSystemNode("");
            //system.Initialize(spawner, Device, GraphicsFactory, EffectFactory, 
            //    TextureFactory.CreateFromFunction(128, 128, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback));//.CreateFromFile("noise"));
            //scene.AddNode(system);

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
        }

        private void CreateWalkway()
        {
            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            MeshBuilder.SetDiffuseColor("Default1", new ColorValue(0.8f, 0.8f, 0.8f, 0.8f));
            MeshBuilder.SetAmbientColor("Default1", new ColorValue(1.0f, 1.0f, 1.0f, 1.0f));
            director.CreateChamferBox(1, 1, 0.4f, 0.1f, 4);
            director.UvMapPlane(1, 1, 1);
            director.Scale(0.5f);
            IModel model = director.Generate("Default1");
            EffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model);
            float t = 0;
            ModelNode modelNode;
            for (int j = 0; j < 30; j++)
            {
                modelNode = new ModelNode("", model, effectHandler, Device);
                //scene.AddNode(modelNode);
                walkway.Add(new Brick(modelNode, t));
                modelNode.WorldState.MoveForward(-0.6f * j);
                modelNode.WorldState.MoveRight(-0.05f * j);
                modelNode.WorldState.MoveUp(-0.17f);
            }

            director.CreatePlane(2, 20, 1, 1, true);
            director.UvMapPlane(2, 0.2f, 2);
            director.Rotate((float)Math.PI / 2, 0, 0);
            //director.Rotate(0, 0, (float)Math.PI / 2);
            model = director.Generate("Default1");
            effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Atmosphere"; }, model);
            plane = new ModelNode("", model, effectHandler, Device);
            plane.WorldState.MoveForward(-2);
            plane.WorldState.MoveUp(-0.1f);
            GraphicsStream stream = ShaderLoader.CompileShaderFromFile("Imaginations.psh", "CreateCloudTexture", null, "tx_1_0", ShaderFlags.None);
            ITexture tex = GraphicsFactory.CreateTexture(Device, 256, 256, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            TextureLoader.FillTexture((Texture)((tex as TextureAdapter).BaseTextureDX), new TextureShader(stream));
            tex.Save("clouds.dds", ImageFileFormat.Dds);
            plane.Model.Materials[0].DiffuseTexture = tex;
            //scene.AddNode(plane);

            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            MeshBuilder.SetDiffuseColor("Default1", ColorValue.FromColor(Color.DarkGoldenrod));
            MeshBuilder.SetAmbientColor("Default1", ColorValue.FromColor(Color.DarkGoldenrod));
            director.CreateCylinder(0.1f, 20, 8, 1, true);
            director.UvRemap(0, 1, 0, 20);
            director.Rotate((float)Math.PI / 2, 0, 0);
            model = director.Generate("Default1");
            effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model);
            cylinder = new ModelNode("", model, effectHandler, Device);
            plane.AddChild(cylinder);
            cylinder.WorldState.MoveRight(1);
            ModelNode cylinder2 = new ModelNode("", model, effectHandler, Device);
            plane.AddChild(cylinder2);
            cylinder2.WorldState.MoveRight(-1);
        }

        private void CreateTiVi()
        {
            XLoader.Load("Tivi-WalkSlow.X", EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(string name)
                {
                    return delegate(int material)
                    {
                        if (material == 1)
                            return "TvScreen";
                        else
                            return "Solid";
                    };
                });
            XLoader.AddToScene(scene);
            tiviNode = (ModelNode)scene.GetNodeByName("TiVi");
            /// TODO: Framehandling is buggy!
            Vector3 pos = tiviNode.WorldState.Position;
            tiviNode.Model.Materials[0].Ambient = Color.White;

            mirrorNode = CreateMirrorNode(tiviNode);

            MeshBuilder.SetAmbientColor("Default1", ColorValue.FromArgb(0));
            MeshBuilder.SetDiffuseColor("Default1", ColorValue.FromArgb(0));
            director.CreatePlane(50, 50, 1, 1, true);
            director.Rotate((float)Math.PI / 2, 0, 0);
            director.Translate(0, -0.11f, 0);
            IModel model = director.Generate("Default1");
            IEffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model);
            blackPlane = new ModelNode("", model, effectHandler, Device);

        }

        private ModelNode CreateMirrorNode(ModelNode originalNode)
        {
            IModel mirrorModel = originalNode.Model.Clone();
            mirrorModel.CullMode = Cull.Clockwise;
            ModelNode mirrorNode = new ModelNode("Box", mirrorModel, originalNode.EffectHandler, Device);
            mirrorNode.WorldState.Scale(new Vector3(1, -1, 1));
            return mirrorNode;
        }

        private void CreateDiscs()
        {
            const float outerRadius = 2.5f;
            const float innerRadius = 2.0f;
            const float torusRadius = 0.07f;
            discModel = CreateDisc(outerRadius, innerRadius, "AlphaTest");
            discModel.AddChild(CreateTorus(outerRadius, torusRadius, "Terrain"));
            discModel.AddChild(CreateTorus(innerRadius, torusRadius, "Terrain"));
            discModel2 = CreateDisc(outerRadius * 0.95f, 0, "AlphaTest");
            discModel2.AddChild(CreateTorus(outerRadius * 0.95f, torusRadius, "Terrain"));
        }

        private void CreateNoiseTexture()
        {
            PerlinTurbulence noise = new PerlinTurbulence();
            noise.BaseFrequency = 30;
            Madd madd = new Madd(0.6f, 0.4f);
            madd.ConnectToInput(0, noise);
            TextureFactory.RegisterTexture("noise", TextureBuilder.Generate(madd, 32, 32, 1, Format.A8R8G8B8));
        }

        private void CreateDiamonds()
        {
            const int NUM_DIAMONDS = 50;
            MeshBuilder.SetAmbientColor("Default3", new ColorValue(0.3f, 0.3f, 0.3f, 1.0f));
            MeshBuilder.SetDiffuseColor("Default3", new ColorValue(0.3f, 0.3f, 0.3f, 1.0f));
            MeshBuilder.SetDiffuseTexture("Default3", "square.tga");
            director.CreateChamferBox(1, 1, 0.4f, 0.2f, 4);
            director.UvMapPlane(1, 1, 1);
            director.Rotate((float)Math.PI / 2, 0, 0);
            director.Rotate(0, 0, (float)Math.PI / 4);
            director.Scale(0.2f, 0.3f, 0.3f);
            IModel model = director.Generate("Default3");
            model.Mesh.ComputeNormals();
            EffectHandler handler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Diamond"; }, model);
            for (int i = 0; i < NUM_DIAMONDS; i++)
            {
                ModelNode node1 = new ModelNode("", model, handler, Device);
                ModelNode node2 = new ModelNode("", model, handler, Device);
                node1.WorldState.MoveUp(Rand.Float(0, 4));
                node1.WorldState.MoveRight(Rand.Float(-5, 5));
                node1.WorldState.MoveForward(Rand.Float(-10, 10));
                node2.WorldState.Position = 
                    new Vector3(node1.WorldState.Position.X, -node1.WorldState.Position.Y, node1.WorldState.Position.Z);
                float t = Rand.Float(Math.PI);
                node1.WorldState.Turn(t);
                node1.WorldState.Turn(t);
                //scene.AddNode(node1);
                //scene.AddNode(node2);
                diamonds.Add(node1);
                diamonds.Add(node2);
            }
        }

        private ModelNode CreateDisc(float outerRadius, float innerRadius, string technique)
        {
            MeshBuilder.SetDiffuseTexture("Default2", "noise");
            director.CreateDisc(outerRadius, innerRadius, 32);
            IModel model = director.Generate("Default2");
            model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.1f);
            model.Materials[0].DiffuseColor = new ColorValue(0.9f, 0.9f, 0.9f);
            return new ModelNode("Terrain", model,
                new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return technique; }, model), Device);
        }

        private ModelNode CreateTorus(float outerRadius, float torusRadius, string technique)
        {
            MeshBuilder.SetDiffuseTexture("Default2", "noise");
            director.CreateTorus(torusRadius, outerRadius, 32, 32);
            IModel model = director.Generate("Default2");
            model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.1f);
            model.Materials[0].DiffuseColor = new ColorValue(0.9f, 0.9f, 0.9f);
            return new ModelNode("Torus", model,
                new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return technique; }, model), Device);
        }

        private void CreateTerrain()
        {
            PerlinNoise generator = new PerlinNoise();
            generator.NumOctaves = 10;
            generator.BaseFrequency = 8;
            generator.Persistence = 0.5f;
            TerrainPrimitive terrain = new TerrainPrimitive();
            terrain.HeightMapGenerator = generator;
            terrain.HeightScale = 10.0f;
            terrain.Width = 50.0f;
            terrain.Height = 50.0f;
            terrain.HeightSegments = 25;
            terrain.WidthSegments = 25;
            terrain.Textured = true;
            MeshBuilder.SetDiffuseTexture("Default2", "BENEDETI.JPG");
            UvMapPlane uvMap = new UvMapPlane();
            uvMap.Input = terrain;
            uvMap.AlignToAxis = 1;
            uvMap.TileV = 4;
            uvMap.TileU = 4;
            IModel model = MeshBuilder.CreateModel(uvMap, "Default2");
            model.Mesh.ComputeNormals();
            model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.1f);
            model.Materials[0].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f);
            discModel = new ModelNode("Terrain", model,
                new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model), Device);
            scene.AddNode(discModel);
            discModel.WorldState.MoveUp(-8);
            discModel.WorldState.MoveForward(25);
        }

        private void CreateLights()
        {
            for (int i = 0; i < NUM_LIGHTS; i++)
            {
                PointLightNode light = new PointLightNode("");
                light.DiffuseColor = new ColorValue(0.3f + 0.7f * (1 - i), 0.3f + 0.7f * i, 1.0f, 1.0f);
                light.Position = new Vector3(0, 0, 0);
                scene.AddNode(light);
                lights.Add(light);
            }
        }

        private void CreateBricks()
        {
            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            MeshBuilder.SetDiffuseColor("Default1", new ColorValue(0.8f, 0.8f, 0.8f, 0.8f));
            director.CreateChamferBox(0.6f, 0.6f, 0.1f, 0.05f, 4);
            director.UvMapPlane(1, 1, 1);
            IModel model = director.Generate("Default1");
            EffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model);
            float t = 0;
            for (int j = 0; j < NUM_RINGS; j++)
            {
                for (int i = 0; i < NUM_BLOCKS; i++)
                {
                    ModelNode modelNode = new ModelNode("Brick", model, effectHandler, Device);
                    scene.AddNode(modelNode);
                    bricks.Add(new Brick(modelNode, t));
                    t += 0.03f;
                }
                t += 0.08f;
            }
        }


        public override void Step()
        {
            camera.WorldState.Reset();
            camera.WorldState.Position = new Vector3((float)Math.Sin(Time.StepTime / 4) * 10,
                2.0f,
                (float)Math.Cos(Time.StepTime / 4) * 10);
            camera.LookAt(new Vector3(0, 1.0f, -5), new Vector3(0, 1, 0));

            StepDiamonds();
            StepDiscs();
            StepBricks();
            StepWalkway();
            scene.Step();
        }

        private void StepWalkway()
        {
            float[] f = new float[] { Time.StepTime * 0.075f, Time.StepTime * 0.063f};
            EffectFactory.CreateFromFile("TiVi.fxo").SetValue(EffectHandle.FromString("AtmosphereTime"), f);

            //cylinder.WorldState.Roll(Time.DeltaTime);
            //cylinder.WorldState.Turn(Time.DeltaTime);
        }

        private void StepDiscs()
        {
            if (discModel == null)
                return;
            discModel.WorldState.Reset();
            discModel2.WorldState.Reset();
            discModel.WorldState.MoveUp(0.85f);
            //discModel2.WorldState.MoveUp(-1.35f);

            discModel.WorldState.Turn(Time.StepTime * 2);
            discModel.WorldState.Tilt(0.3f);
            //Mixer.ClearColor = Color.FromArgb(0, Color.White);
            for (int i = 0; i < NUM_LIGHTS; i++)
                lights[i].Position = new Vector3(
                    (float)Math.Sin(Time.StepTime * 0.5f + i) * 20,
                    (float)Math.Cos(Time.StepTime * 1.23f + i) * 10 + 10,
                    (float)Math.Sin(Time.StepTime + i) * 10 + 10);
        }

        private void StepBricks()
        {
            if (bricks.Count <= 0)
                return;
            for (int j = 0; j < NUM_RINGS; j++)
            {
                for (int i = 0; i < NUM_BLOCKS; i++)
                {
                    ModelNode model = bricks[j * NUM_BLOCKS + i].Model;
                    float startTime = bricks[j * NUM_BLOCKS + i].StartTime;
                    const float FALL_TIME = 1.3f;
                    const float FALL_HEIGHT = 10.0f;
                    float time = Time.StepTime - StartTime - startTime;
                    float upAdd = 0;
                    if (time < FALL_TIME)
                    {
                        if (time < 0)
                            time = 0;
                        float zeroToOne = (float)Math.Sin(time / FALL_TIME * Math.PI / 2);
                        upAdd = (1 - zeroToOne) * FALL_HEIGHT;
                    }
                    model.WorldState.Reset();
                    //model.WorldState.MoveForward(2);
                    model.WorldState.MoveUp(-2 + 0.8f * j + upAdd);
                    model.WorldState.Tilt((float)Math.PI / 2);
                    model.WorldState.Roll(Time.StepTime / 2 + (float)Math.PI * 2 * i / NUM_BLOCKS);
                    model.WorldState.MoveUp(-3.0f);
                }
            }
        }

        private void StepDiamonds()
        {
            foreach (ModelNode node in diamonds)
            {
                node.WorldState.Turn(Time.DeltaTime * 3);
                node.WorldState.Position += new Vector3(0, 0, Time.DeltaTime * 4.5f);
                if (node.WorldState.Position.Z > 10)
                    node.WorldState.Position += new Vector3(0, 0, -20);
            }
        }

        public override void Render()
        {
            mirrorNode.Render(scene);
            foreach (ModelNode node in diamonds)
                node.Render(scene);
            plane.Render(scene);
            blackPlane.Render(scene);
            scene.Render();
            if (discModel != null)
            {
                discModel.Render(scene);
                discModel2.Render(scene);
            }
        }
    }
}
