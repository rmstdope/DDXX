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
    public class TunnelFlight2 : TunnelFlightBase
    {
        private const int NUM_RINGS = 32;
        private const int NUM_BLOCKS = 12;
        private List<TunnleRing> rings = new List<TunnleRing>();
        private ModelNode terrainModel;
        private List<ModelNode> diamonds = new List<ModelNode>();
        private IModel brickModel;
        private float diffuseLuminance;

        public float DiffuseLuminance
        {
            get { return diffuseLuminance; }
            set 
            { 
                diffuseLuminance = value;
                if (brickModel != null)
                    brickModel.Materials[0].DiffuseColor = new ColorValue(diffuseLuminance, diffuseLuminance, diffuseLuminance, diffuseLuminance);
            }
        }

        public TunnelFlight2(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            diffuseLuminance = 0.3f;
            SetStepSize(GetTweakableNumber("DiffuseLuminance"), 0.01f);
        }

        protected override void InitializeSpecific()
        {
            camera.SetFOV((float)Math.PI * 0.62f);// * 0.7f);

            CreateSplines();
            //CreateNoiseTexture();
            CreateBricks();
            //CreateTerrain();
            //CreateDiamonds();

            //GlitterParticleSpawner spawner = new GlitterParticleSpawner(GraphicsFactory, Device, 500);
            //ParticleSystemNode system = new ParticleSystemNode("");
            //system.Initialize(spawner, Device, GraphicsFactory, EffectFactory, 
            //    TextureFactory.CreateFromFunction(128, 128, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback));//.CreateFromFile("noise"));
            //scene.AddNode(system);

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
        }

        private void CreateSplines()
        {
            discInterpolator = new Interpolator<InterpolatedVector3>();
            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(0.0f, 0.0f, 0.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(5, new InterpolatedVector3(new Vector3(2.0f, 0.0f, 10.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(10, new InterpolatedVector3(new Vector3(-2.0f, 0.0f, 20.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(15, new InterpolatedVector3(new Vector3(2.0f, 0.0f, 30.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(20, new InterpolatedVector3(new Vector3(-2.0f, 0.0f, 40.0f))));
            spline.Calculate();
            discInterpolator.AddSpline(spline);
        }

        //private ModelNode CreateMirrorNode(ModelNode originalNode)
        //{
        //    IModel mirrorModel = originalNode.Model.Clone();
        //    mirrorModel.CullMode = Cull.Clockwise;
        //    ModelNode mirrorNode = new ModelNode("Box", mirrorModel, originalNode.EffectHandler, Device);
        //    mirrorNode.WorldState.Scale(new Vector3(1, -1, 1));
        //    return mirrorNode;
        //}

        //private void CreateNoiseTexture()
        //{
        //    PerlinTurbulence noise = new PerlinTurbulence();
        //    noise.BaseFrequency = 30;
        //    Madd madd = new Madd();
        //    madd.Mul = 0.6f;
        //    madd.Add = 0.4f;
        //    madd.ConnectToInput(0, noise);
        //    TextureFactory.RegisterTexture("noise", TextureBuilder.Generate(madd, 32, 32, 1, Format.A8R8G8B8));
        //}

        //private void CreateDiamonds()
        //{
        //    const int NUM_DIAMONDS = 50;
        //    MeshBuilder.SetAmbientColor("Default3", new ColorValue(0.3f, 0.3f, 0.3f, 1.0f));
        //    MeshBuilder.SetDiffuseColor("Default3", new ColorValue(0.9f, 0.9f, 0.9f, 1.0f));
        //    MeshBuilder.SetDiffuseTexture("Default3", "square.tga");
        //    director.CreateChamferBox(1, 1, 0.4f, 0.2f, 4);
        //    director.UvMapPlane(1, 1, 1);
        //    director.Rotate((float)Math.PI / 2, 0, 0);
        //    director.Rotate(0, 0, (float)Math.PI / 4);
        //    director.Scale(0.1f, 0.2f, 0.2f);
        //    IModel model = director.Generate("Default3");
        //    model.Mesh.ComputeNormals();
        //    EffectHandler handler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
        //        delegate(int material) { return "Diamond"; }, model);
        //    for (int i = 0; i < NUM_DIAMONDS; i++)
        //    {
        //        ModelNode node1 = new ModelNode("", model, handler, Device);
        //        node1.WorldState.MoveUp(Rand.Float(-1, 5));
        //        node1.WorldState.MoveRight(Rand.Float(-3, 3));
        //        node1.WorldState.MoveForward(Rand.Float(-3, 3));
        //        float t = Rand.Float(Math.PI);
        //        node1.WorldState.Turn(t);
        //        //scene.AddNode(node1);
        //        diamonds.Add(node1);
        //    }
        //}

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

        private void CreateBricks()
        {
            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            MeshBuilder.SetAmbientColor("Default1", new ColorValue(0, 0, 0, 0));
            MeshBuilder.SetDiffuseColor("Default1", new ColorValue(diffuseLuminance, diffuseLuminance, diffuseLuminance, diffuseLuminance));
            director.CreateChamferBox(1.0f, 1.0f, 0.1f, 0.03f, 4);
            director.UvMapPlane(1, 1, 1);
            //director.UvRemap(0.1f, 0.8f, 0.1f, 0.8f);
            brickModel = director.Generate("Default1");
            EffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Bricks"; }, brickModel);
            Vector3 right = new Vector3(1, 0, 0);
            float nextTime = 0;
            for (int j = 0; j < NUM_RINGS; j++)
            {
                Vector3 up = discInterpolator.GetValue(nextTime + 0.05f) - (Vector3)discInterpolator.GetValue(nextTime);
                Vector3 forward = Vector3.Cross(right, up);
                right = Vector3.Cross(up, forward);
                up.Normalize();
                right.Normalize();
                forward.Normalize();
                DummyNode ringCenter = new DummyNode("");
                ringCenter.WorldState.Position = discInterpolator.GetValue(nextTime);
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
                    ModelNode modelNode = new ModelNode("Brick", brickModel, effectHandler, Device);
                    modelNode.WorldState.Tilt((float)Math.PI / 2);
                    modelNode.WorldState.Roll((float)(i * Math.PI * 2 / NUM_BLOCKS));
                    modelNode.WorldState.MoveUp(-2);
                    ringCenter.AddChild(modelNode);
                }
                scene.AddNode(ringCenter);
                Vector3 oldPos = discInterpolator.GetValue(nextTime);
                while ((oldPos - discInterpolator.GetValue(nextTime)).Length() < 2.0f && nextTime < 100)
                    nextTime += 0.01f;
                //nextTime += 1;
            }
        }

        protected override void StepSpecific()
        {
            //Mixer.ClearColor = Color.Gray;
            //camera.WorldState.Reset();
            float t = Time.StepTime - StartTime;
            camera.WorldState.Position = discInterpolator.GetValue(t);
            float sine = 0.5f * (float)Math.PI + 0.5f * (float)Math.PI * t * 136 / 60.0f;
            float dist = 0.9f + (float)Math.Sin(sine) * 0.1f;
            camera.WorldState.Position += new Vector3((float)Math.Sin(1 + t / 5), 1.0f, (float)Math.Cos(1 + t / 5)) * dist;
            camera.LookAt(discInterpolator.GetValue(t) + new Vector3(0, 0.8f, 0), new Vector3(0, 1, 0));
            //camera.WorldState.Position = new Vector3((float)Math.Sin(Time.StepTime / 4), 0.6f, (float)Math.Cos(Time.StepTime / 4)) * 3;
            //camera.LookAt(new Vector3(0, 0.5f, 0), new Vector3(0, 1, 0));

            StepDiamonds();
            scene.Step();
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

        protected override void RenderSpecific()
        {
            foreach (TunnleRing ring in rings)
                ring.RingCenter.WorldState.Turn(Time.DeltaTime * ring.RotDirection * 0.5f);
            foreach (ModelNode node in diamonds)
                node.Render(scene);
            scene.Render();
        }

        protected override string GetScreenTechnique()
        {
            return "TvScreen";
        }
    }
}
