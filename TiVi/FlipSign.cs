using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.MeshBuilder;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.ParticleSystems;

namespace TiVi
{
    public class FlipSign : BaseDemoEffect
    {
        private const int NUM_SIGNS_X = 20;
        private const int NUM_SIGNS_Y = 20;
        private IScene scene;
        private CameraNode camera;
        private MeshDirector meshDirector;
        private ModelNode[] signs = new ModelNode[NUM_SIGNS_X * NUM_SIGNS_Y];
        private Interpolator<InterpolatedVector3> interpolator;
        private Interpolator<InterpolatedVector3> platformInterpolator;
        private ModelNode platform;
        private IDemoEffect subEffect;
        private ITexture subEffectTexture;

        public FlipSign(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            subEffect = new DanceCloseup("Dance closeup", StartTime, EndTime);
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
            subEffect.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            subEffectTexture = GraphicsFactory.CreateTexture(Device, 256, 256, 1, Usage.RenderTarget, Format.A8R8G8B8, Pool.Default);

            IDevice device = Device;
            CreateStandardSceneAndCamera(out scene, out camera, 15);
            CreateBoard();
            CreateCameraInterpolator();
            CreateLights();
            CreatePlatform();
        }

        private void CreatePlatform()
        {
            MeshBuilder.SetAmbientColor("Default1", new ColorValue(0.2f, 0.3f, 0.5f));
            MeshBuilder.SetDiffuseTexture("Default1", "square.tga");
            MeshBuilder.SetReflectiveTexture("Default1", "rnl_cross.dds");
            MeshBuilder.SetReflectiveFactor("Default1", 0.15f);
            meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreateChamferBox(2, 2, 0.5f, 0.2f, 4);
            IModel model = meshDirector.Generate("Default1");
            platform = CreateSimpleModelNode(model, "TiVi.fxo", "ReflectiveTransparent");
            scene.AddNode(platform);

            ParticleSystemNode system = new ParticleSystemNode("xx");
            SwirlParticleSpawner spawner = new SwirlParticleSpawner(GraphicsFactory, Device, 1000);
            spawner.TimeBetweenSpawns = 0.02f;
            system.Initialize(spawner, Device, GraphicsFactory, EffectFactory,
                TextureFactory.CreateFromFunction(128, 128, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback));
            platform.AddChild(system);

            XLoader.Load("Tivi-Dance.X", EffectFactory.CreateFromFile("TiVi.fxo"),
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
            INode node = XLoader.GetNodeHierarchy()[0].Children[1];
            node.WorldState.Turn((float)Math.PI * 1.2f);
            node.WorldState.Position = new Vector3(0, 0.25f, 0);
            platform.AddChild(node);
            scene.HandleHierarchy(XLoader.RootFrame);

            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(2, -1, -2))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(4, new InterpolatedVector3(new Vector3(3, 1, -3))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(8, new InterpolatedVector3(new Vector3(-1, 1, -3))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(12, new InterpolatedVector3(new Vector3(-3, -1, -4))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(16, new InterpolatedVector3(new Vector3(-2, -1, -2))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(20, new InterpolatedVector3(new Vector3(2, 1, -3))));
            spline.Calculate();
            platformInterpolator = new Interpolator<InterpolatedVector3>();
            platformInterpolator.AddSpline(spline);
        }

        private void CreateLights()
        {
            PointLightNode[] lights = new PointLightNode[2];
            lights[0] = new PointLightNode("");
            lights[0].Position = new Vector3(-5, 4, 0);
            lights[0].DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            scene.AddNode(lights[0]);
            lights[1] = new PointLightNode("");
            lights[1].Position = new Vector3(5, 4, 0);
            lights[1].DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            scene.AddNode(lights[1]);
        }

        private void CreateCameraInterpolator()
        {
            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(0, -2, -16))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(2, new InterpolatedVector3(new Vector3(3, 3, -18))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(4, new InterpolatedVector3(new Vector3(-2, 3, -18))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(6, new InterpolatedVector3(new Vector3(2, -4, -22))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(8, new InterpolatedVector3(new Vector3(0, -2, -16))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(10, new InterpolatedVector3(new Vector3(-2, 3, -18))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(12, new InterpolatedVector3(new Vector3(2, -3, -18))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(14, new InterpolatedVector3(new Vector3(2, 4, -22))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(16, new InterpolatedVector3(new Vector3(0, -2, -16))));
            spline.Calculate();
            interpolator = new Interpolator<InterpolatedVector3>();
            interpolator.AddSpline(spline);
        }

        private void CreateBoard()
        {
            //MeshBuilder.SetAmbientColor("Default1", new ColorValue(1.0f, 1.0f, 1.0f));
            MeshBuilder.SetAmbientColor("Default1", new ColorValue(0.4f, 0.6f, 0.7f));
            MeshBuilder.SetDiffuseTexture("Default1", "screenshot0.jpg");
            MeshBuilder.SetReflectiveTexture("Default1", "rnl_cross.dds");
            MeshBuilder.SetReflectiveFactor("Default1", 0.15f);
            meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreatePlane(1, 1, 1, 1, true);
            float uAdd = 1.0f / NUM_SIGNS_X;
            float vAdd = 1.0f / NUM_SIGNS_Y;
            float uWidth = 1.0f / (NUM_SIGNS_X * 1.1f);
            float vWidth = 1.0f / (NUM_SIGNS_Y * 1.1f);
            float vStart = 0;
            for (int y = 0; y < NUM_SIGNS_Y; y++)
            {
                float uStart = 0;
                for (int x = 0; x < NUM_SIGNS_X; x++)
                {
                    float yPos = -1.1f * (y - NUM_SIGNS_Y / 2);
                    float xPos = -1.1f * (x - NUM_SIGNS_X / 2);
                    meshDirector.UvMapPlane(2, 1, 1);
                    meshDirector.UvRemap(1, -1, 1, -1);
                    meshDirector.UvRemap(uStart, uWidth, vStart, vWidth);
                    meshDirector.UvRemap(1, -1, 0, 1);
                    uStart += uAdd;
                    IModel model = meshDirector.Generate("Default1");
                    ModelNode node = CreateSimpleModelNode(model, "TiVi.fxo", "ReflectiveTransparent");
                    node.Model.Materials[0].DiffuseTexture = subEffectTexture;
                    node.WorldState.MoveUp(yPos);
                    node.WorldState.MoveRight(xPos);
                    signs[y * NUM_SIGNS_X + x] = node;
                    scene.AddNode(node);
                }
                vStart += vAdd;
            }
        }

        public override void Step()
        {
            RenderSubEffect();
            platform.WorldState.Position = platformInterpolator.GetValue(Time.StepTime);

            Mixer.ClearColor = Color.Black;
            //camera.WorldState.Position = new Vector3(0, 3, -10);
            camera.WorldState.Position = interpolator.GetValue(Time.StepTime % 16);
            //camera.WorldState.Position = new Vector3((float)Math.Sin(Time.StepTime * 0.2f), 0.2f, (float)Math.Cos(Time.StepTime * 0.2f)) * 8;
            Vector3 at = interpolator.GetValue((Time.StepTime + 3) % 8);
            at *= 0.5f;
            at.Z = 0;
            camera.LookAt(platform.Position + new Vector3(0, 1, 0), new Vector3(0, 1, 0));
            for (int y = 0; y < NUM_SIGNS_Y; y++)
            {
                for (int x = 0; x < NUM_SIGNS_X; x++)
                {
                    ModelNode node = signs[y * NUM_SIGNS_X + x];
                    node.WorldState.ResetRotation();
                    float t = (Time.StepTime + (x + y * 1.0f) / 30.0f) % 3;
                    const float period = 0.7f;
                    if (t < period)
                    {
                        float d = (float)Math.Sin(t / period * Math.PI);
                        //node.WorldState.MoveForward(-d * 0.5f);
                        node.WorldState.Turn(d * 0.2f);
                        node.WorldState.Tilt(d * 0.2f);
                        node.WorldState.Roll(d * 0.2f);
                        node.Model.Materials[0].AmbientColor = new ColorValue(0.4f + 0.6f * d, 0.6f + 0.4f * d, 0.7f);
                    }
                }
            }

            //float v = (Time.StepTime / 1.0f) % 1;
            //float g = (float)Math.Sin(v * Math.PI);
            //camera.SetFOV((float)Math.PI / 4 * (0.5f + g)); 
            scene.Step();
        }

        private void RenderSubEffect()
        {
            subEffect.Step();
            using (ISurface original = Device.GetRenderTarget(0))
            {
                using (ISurface surface = subEffectTexture.GetSurfaceLevel(0))
                {
                    Device.SetRenderTarget(0, surface);
                    Device.BeginScene();
                    Device.Clear(ClearFlags.Target, Color.Black, 0, 0);
                    subEffect.Render();
                    Device.EndScene();
                    Device.SetRenderTarget(0, original);
                }
            }
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
