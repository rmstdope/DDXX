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
        private struct TunnleRing
        {
            public DummyNode RingCenter;
            public float RotDirection;
        }

        private const int NUM_RINGS = 32;
        private const int NUM_LIGHTS = 2;
        private const int NUM_BLOCKS = 12;
        private IScene scene;
        private CameraNode camera;
        private List<PointLightNode> lights = new List<PointLightNode>();
        private List<Brick> fallingBricks = new List<Brick>();
        private List<TunnleRing> rings = new List<TunnleRing>();
        private ModelNode discModel;
        private ModelNode discModel2;
        private ModelNode terrainModel;
        private ModelNode tiviNode;
        private ModelNode mirrorNode;
        private MeshDirector director;
        private List<ModelNode> diamonds = new List<ModelNode>();
        private Interpolator<InterpolatedVector3> interpolator;

        public TunnelFlight(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
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
            camera.SetFOV((float)Math.PI / 2 * 0.7f);

            CreateSplines();
            CreateNoiseTexture();
            CreateBricks();
            //CreateFallingBricks();
            CreateLights();
            //CreateTerrain();
            //CreateDiamonds();
            CreateDiscs();
            //CreateTiVi();

            //GlitterParticleSpawner spawner = new GlitterParticleSpawner(GraphicsFactory, Device, 500);
            //ParticleSystemNode system = new ParticleSystemNode("");
            //system.Initialize(spawner, Device, GraphicsFactory, EffectFactory, 
            //    TextureFactory.CreateFromFunction(128, 128, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback));//.CreateFromFile("noise"));
            //scene.AddNode(system);

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
        }

        private void CreateSplines()
        {
            interpolator = new Interpolator<InterpolatedVector3>();
            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(0.0f, 0.0f, 0.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(5, new InterpolatedVector3(new Vector3(2.0f, 0.0f, 10.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(10, new InterpolatedVector3(new Vector3(-2.0f, 0.0f, 20.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(15, new InterpolatedVector3(new Vector3(2.0f, 0.0f, 30.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(20, new InterpolatedVector3(new Vector3(-2.0f, 0.0f, 40.0f))));
            spline.Calculate();
            interpolator.AddSpline(spline);
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
            const float outerRadius = 1.0f;
            const float innerRadius = 0.9f;
            const float torusRadius = 0.01f;
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
            MeshBuilder.SetDiffuseColor("Default3", new ColorValue(0.9f, 0.9f, 0.9f, 1.0f));
            MeshBuilder.SetDiffuseTexture("Default3", "square.tga");
            director.CreateChamferBox(1, 1, 0.4f, 0.2f, 4);
            director.UvMapPlane(1, 1, 1);
            director.Rotate((float)Math.PI / 2, 0, 0);
            director.Rotate(0, 0, (float)Math.PI / 4);
            director.Scale(0.1f, 0.2f, 0.2f);
            IModel model = director.Generate("Default3");
            model.Mesh.ComputeNormals();
            EffectHandler handler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Diamond"; }, model);
            for (int i = 0; i < NUM_DIAMONDS; i++)
            {
                ModelNode node1 = new ModelNode("", model, handler, Device);
                node1.WorldState.MoveUp(Rand.Float(-1, 5));
                node1.WorldState.MoveRight(Rand.Float(-3, 3));
                node1.WorldState.MoveForward(Rand.Float(-3, 3));
                float t = Rand.Float(Math.PI);
                node1.WorldState.Turn(t);
                //scene.AddNode(node1);
                diamonds.Add(node1);
            }
        }

        private ModelNode CreateDisc(float outerRadius, float innerRadius, string technique)
        {
            MeshBuilder.SetDiffuseTexture("Default2", "noise");
            director.CreateDisc(outerRadius, innerRadius, 32);
            IModel model = director.Generate("Default2");
            model.Materials[0].AmbientColor = new ColorValue(0.1f, 0.1f, 0.1f);
            model.Materials[0].DiffuseColor = new ColorValue(0.5f, 0.5f, 0.5f);
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
            //MeshBuilder.SetDiffuseTexture("Default2", "BENEDETI.JPG");
            MeshBuilder.SetDiffuseTexture("Default2", "clouds.dds");
            UvMapPlane uvMap = new UvMapPlane();
            uvMap.Input = terrain;
            uvMap.AlignToAxis = 1;
            uvMap.TileV = 4;
            uvMap.TileU = 4;
            IModel model = MeshBuilder.CreateModel(uvMap, "Default2");
            model.Mesh.ComputeNormals();
            model.Materials[0].AmbientColor = new ColorValue(0.4f, 0.4f, 0.4f);
            model.Materials[0].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f);
            terrainModel = new ModelNode("Terrain", model,
                new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model), Device);
            scene.AddNode(terrainModel);
            terrainModel.WorldState.MoveUp(-5.5f);
            terrainModel.WorldState.MoveForward(0);
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
            MeshBuilder.SetAmbientColor("Default1", new ColorValue(0, 0, 0, 0));
            MeshBuilder.SetDiffuseColor("Default1", new ColorValue(0.6f, 0.6f, 0.6f, 1.0f));
            director.CreateChamferBox(1.0f, 1.0f, 0.1f, 0.03f, 4);
            director.UvMapPlane(1, 1, 1);
            //director.UvRemap(0.1f, 0.8f, 0.1f, 0.8f);
            IModel model = director.Generate("Default1");
            EffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Bricks"; }, model);
            Vector3 right = new Vector3(1, 0, 0);
            float nextTime = 0;
            for (int j = 0; j < NUM_RINGS; j++)
            {
                Vector3 up = interpolator.GetValue(nextTime + 0.05f) - (Vector3)interpolator.GetValue(nextTime);
                Vector3 forward = Vector3.Cross(right, up);
                right = Vector3.Cross(up, forward);
                up.Normalize();
                right.Normalize();
                forward.Normalize();
                DummyNode ringCenter = new DummyNode("");
                ringCenter.WorldState.Position = interpolator.GetValue(nextTime);
                Matrix rot = new Matrix();
                rot.M11 = right.X;
                rot.M12 = right.Y;
                rot.M13 = right.Z;
                rot.M21 = up.X;
                rot.M22 = up.Y;
                rot.M23 = up.Z;
                rot.M31 = forward.X;
                rot.M32 = forward.Y;
                rot.M33 = forward.Z;
                rot.M44 = 1;
                ringCenter.WorldState.Rotation = rot;
                TunnleRing ring = new TunnleRing();
                ring.RingCenter = ringCenter;
                ring.RotDirection = j % 2 == 1 ? 1 : -1;
                rings.Add(ring);
                for (int i = 0; i < NUM_BLOCKS; i++)
                {
                    ModelNode modelNode = new ModelNode("Brick", model, effectHandler, Device);
                    modelNode.WorldState.Tilt((float)Math.PI / 2);
                    modelNode.WorldState.Roll((float)(i * Math.PI * 2 / NUM_BLOCKS));
                    modelNode.WorldState.MoveUp(-2);
                    ringCenter.AddChild(modelNode);
                }
                scene.AddNode(ringCenter);
                Vector3 oldPos = interpolator.GetValue(nextTime);
                while ((oldPos - interpolator.GetValue(nextTime)).Length() < 2.0f && nextTime < 100)
                    nextTime += 0.01f;
                //nextTime += 1;
            }
        }

        private void CreateFallingBricks()
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
                    fallingBricks.Add(new Brick(modelNode, t));
                    t += 0.03f;
                }
                t += 0.08f;
            }
        }

        public override void Step()
        {
            //Mixer.ClearColor = Color.Gray;
            //camera.WorldState.Reset();
            camera.WorldState.Position = interpolator.GetValue(Time.StepTime);
            camera.WorldState.Position += new Vector3((float)Math.Sin(Time.StepTime / 4), 1.0f, (float)Math.Cos(Time.StepTime / 4)) * 1.3f;
            camera.LookAt(interpolator.GetValue(Time.StepTime) + new Vector3(0, 0.8f, 0), new Vector3(0, 1, 0));
            //camera.WorldState.Position = new Vector3((float)Math.Sin(Time.StepTime / 4), 0.6f, (float)Math.Cos(Time.StepTime / 4)) * 3;
            //camera.LookAt(new Vector3(0, 0.5f, 0), new Vector3(0, 1, 0));

            StepDiamonds();
            StepDiscs();
            StepFallingBricks();
            scene.Step();
        }

        private void StepDiscs()
        {
            if (discModel == null)
                return;
            discModel.WorldState.Reset();
            discModel2.WorldState.Reset();
            discModel.WorldState.Position = interpolator.GetValue(Time.StepTime);
            discModel2.WorldState.Position = interpolator.GetValue(Time.StepTime);
            discModel.WorldState.MoveUp(0.40f);
            //discModel2.WorldState.MoveUp(-1.35f);

            discModel.WorldState.Turn(Time.StepTime * 2);
            discModel.WorldState.Tilt(0.3f);
            //Mixer.ClearColor = Color.FromArgb(0, Color.White);
            for (int i = 0; i < NUM_LIGHTS; i++)
            {
                lights[i].Position = interpolator.GetValue(Time.StepTime) + new Vector3(
                    (float)Math.Sin(Time.StepTime * 0.5f + i) * 2,
                    (float)Math.Cos(Time.StepTime * 1.23f + i) * 1 + 1,
                    (float)Math.Sin(Time.StepTime + i) * 2);
                //lights[i].Position = new Vector3(
                //    (float)Math.Sin(Time.StepTime * 0.5f + i) * 20,
                //    (float)Math.Cos(Time.StepTime * 1.23f + i) * 10 + 10,
                //    (float)Math.Sin(Time.StepTime + i) * 10 + 10);
            }
        }

        private void StepFallingBricks()
        {
            if (fallingBricks.Count <= 0)
                return;
            for (int j = 0; j < NUM_RINGS; j++)
            {
                for (int i = 0; i < NUM_BLOCKS; i++)
                {
                    ModelNode model = fallingBricks[j * NUM_BLOCKS + i].Model;
                    float startTime = fallingBricks[j * NUM_BLOCKS + i].StartTime;
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
                    model.WorldState.MoveUp(0 + 0.8f * j + upAdd);
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
                node.WorldState.Turn(Time.DeltaTime * 8);
                node.WorldState.Position += new Vector3(0, Time.DeltaTime * 0.5f, 0);
                if (node.WorldState.Position.Y > 5)
                    node.WorldState.Position += new Vector3(0, -6, 0);
            }
        }

        public override void Render()
        {
            foreach (TunnleRing ring in rings)
                ring.RingCenter.WorldState.Turn(Time.DeltaTime * ring.RotDirection * 0.5f);
            //mirrorNode.Render(scene);
            foreach (ModelNode node in diamonds)
                node.Render(scene);
            scene.Render();
            if (discModel != null)
            {
                discModel2.Render(scene);
                discModel.Render(scene);
            }
        }
    }
}
