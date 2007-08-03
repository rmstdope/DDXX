using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.Utility;
using Dope.DDXX.ParticleSystems;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.MeshBuilder;

namespace TiVi
{
    public class ThinkTank : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        private ILine line;
        private ITexture screenTexture;
        private IDemoEffect subEffect;
        private ParticleSystemNode system;
        //private ITexture cd2Texture;
        //private ISprite sprite;
        private TiViMeshDirector meshDirector;
        private List<ModelNode> diamonds = new List<ModelNode>();
        private Interpolator<InterpolatedVector3> cameraInterpolator;
        private Interpolator<InterpolatedVector3> targetInterpolator;
        private TiVi tivi;
        private float turnSpeed;

        public float TurnSpeed
        {
            get { return turnSpeed; }
            set { turnSpeed = value; }
        }

        public ThinkTank(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            turnSpeed = 1.1f;
            SetStepSize(GetTweakableNumber("TurnSpeed"), 0.01f);
        }

        protected override void Initialize()
        {
            line = GraphicsFactory.CreateLine(Device);
            CreateStandardSceneAndCamera(out scene, out camera, 3);
            camera.WorldState.MoveUp(1.5f);

            XLoader.Load("Tivi-Sitting.X", EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(string name)
                {
                    if (name == "TiVi")
                    {
                        return delegate(int material)
                        {
                            if (material == 1)
                                return "TvScreen";
                            else
                                return "TiViReflective";
                        };
                    }
                    else
                        return TechniqueChooser.MaterialPrefix("Terrain");
                });
            XLoader.AddToScene(scene);
            (scene.GetNodeByName("Plane01") as ModelNode).Model.Materials[0].DiffuseTexture = TextureFactory.CreateFromFile("CrystalDreams2.jpg");
            ModelNode tiviNode = scene.GetNodeByName("TiVi") as ModelNode;
            tivi = new TiVi(tiviNode, camera, StartTime);
            tiviNode.Model.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
            tiviNode.Model.Materials[0].DiffuseTexture = TextureFactory.CreateFromFile("marble.jpg");
            tiviNode.Model.Materials[0].AmbientColor = new ColorValue(0.8f, 0.8f, 0.8f, 0.8f);
            tiviNode.Model.Materials[0].DiffuseColor = new ColorValue(0.5f, 0.5f, 0.5f, 0.5f);
            tiviNode.Model.Materials[0].ReflectiveFactor = 0.3f;
            tiviNode.Model.Materials[1].AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);

            cameraInterpolator = new Interpolator<InterpolatedVector3>();
            ClampedCubicSpline<InterpolatedVector3> spline = new ClampedCubicSpline<InterpolatedVector3>(
                new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(0.0f, 2.0f, -3.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(4, new InterpolatedVector3(new Vector3(1.0f, 1.5f, -2.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(7, new InterpolatedVector3(new Vector3(3.0f, 1.0f, 0.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(10, new InterpolatedVector3(tivi.DestinationPos)));
            spline.Calculate();
            cameraInterpolator.AddSpline(spline);

            targetInterpolator = new Interpolator<InterpolatedVector3>();
            spline = new ClampedCubicSpline<InterpolatedVector3>(
                new InterpolatedVector3(), new InterpolatedVector3());
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(0, new InterpolatedVector3(new Vector3(-6.0f, 1.0f, 0.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(5, new InterpolatedVector3(new Vector3(-1.0f, 1.0f, 5.0f))));
            spline.AddKeyFrame(new KeyFrame<InterpolatedVector3>(8, new InterpolatedVector3(tivi.Center)));
            spline.Calculate();
            targetInterpolator.AddSpline(spline);

            subEffect = new ChessScene("screeneffect", EndTime, EndTime);
            subEffect.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            screenTexture = TextureFactory.CreateFullsizeRenderTarget();
            (scene.GetNodeByName("TiVi") as ModelNode).Model.Materials[1].DiffuseTexture = screenTexture;

            Time.Pause();
            Time.CurrentTime = EndTime;
            subEffect.Step();
            using (ISurface original = Device.GetRenderTarget(0))
            {
                using (ISurface surface = screenTexture.GetSurfaceLevel(0))
                {
                    Device.SetRenderTarget(0, surface);
                    Device.BeginScene();
                    Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer | ClearFlags.Stencil, Color.Black, 1, 0);
                    subEffect.Render();
                    Device.EndScene();
                    Device.SetRenderTarget(0, original);
                }
            }
            Time.Resume();

            TextureDirector director = new TextureDirector(TextureBuilder);
            director.CreateCircle(0.1f, 0.4f);
            ITexture texture = director.Generate(64, 64, 0, Format.A8R8G8B8);
            SphereParticleSpawner spawner = new SphereParticleSpawner(GraphicsFactory, Device, 1000, 3);
            spawner.Color = Color.FromArgb(180, 180, 220);
            spawner.ColorDistortion = 35;
            spawner.Size = 0.2f;
            spawner.SizeDistortion = 0.1f;
            system = new ParticleSystemNode("System");
            system.Initialize(spawner, Device, GraphicsFactory, EffectFactory, TextureFactory.CreateFromFile("flare.dds"));// texture);
            scene.AddNode(system);

            meshDirector = new TiViMeshDirector(MeshBuilder, new MeshDirector(MeshBuilder), EffectFactory, Device);
            ModelNode baseDiamond = meshDirector.CreateDiamondNode(0.4f);
            for (int y = -15; y < 16; y++)
            {
                for (int x = -10; x < 11; x++)
                {
                    ModelNode diamond = new ModelNode(baseDiamond.Name + "Clone", baseDiamond.Model, baseDiamond.EffectHandler, Device);
                    diamonds.Add(diamond);
                    scene.AddNode(diamond);
                    diamond.Position = new Vector3(x * 0.4f, y * 0.6f, 4);
                    diamond.WorldState.Turn(y / 5.0f + x / 4.0f + Rand.Float(0, Math.PI / 4));
                }
            }
            for (int y = -15; y < 16; y++)
            {
                for (int x = -10; x < 11; x++)
                {
                    ModelNode diamond = new ModelNode(baseDiamond.Name + "Clone", baseDiamond.Model, baseDiamond.EffectHandler, Device);
                    diamonds.Add(diamond);
                    scene.AddNode(diamond);
                    diamond.Position = new Vector3(4, y * 0.6f, x * 0.4f);
                    diamond.WorldState.Turn(y / 5.0f + x / 4.0f + Rand.Float(0, Math.PI / 4));
                }
            }
            for (int y = -15; y < 16; y++)
            {
                for (int x = -10; x < 11; x++)
                {
                    ModelNode diamond = new ModelNode(baseDiamond.Name + "Clone", baseDiamond.Model, baseDiamond.EffectHandler, Device);
                    diamonds.Add(diamond);
                    scene.AddNode(diamond);
                    diamond.Position = new Vector3(-4, y * 0.6f, x * 0.4f);
                    diamond.WorldState.Turn(y / 5.0f + x / 4.0f + Rand.Float(0, Math.PI / 4));
                }
            }
            //for (int y = -10; y < 11; y++)
            //{
            //    for (int x = -10; x < 11; x++)
            //    {
            //        ModelNode diamond = new ModelNode(baseDiamond.Name + "Clone", baseDiamond.Model, baseDiamond.EffectHandler, Device);
            //        diamonds.Add(diamond);
            //        scene.AddNode(diamond);
            //        diamond.Position = new Vector3(x * 0.4f, 4, y * 0.4f);
            //        diamond.WorldState.Turn(Rand.Float(0, 2 * Math.PI));
            //    }
            //}
            //for (int y = -10; y < 11; y++)
            //{
            //    for (int x = -10; x < 11; x++)
            //    {
            //        ModelNode diamond = new ModelNode(baseDiamond.Name + "Clone", baseDiamond.Model, baseDiamond.EffectHandler, Device);
            //        diamonds.Add(diamond);
            //        scene.AddNode(diamond);
            //        diamond.Position = new Vector3(x * 0.4f, -4, y * 0.4f);
            //        diamond.WorldState.Turn(Rand.Float(0, 2 * Math.PI));
            //    }
            //}
        }

        public override void Step()
        {
            camera.Position = cameraInterpolator.GetValue(Time.StepTime - StartTime);
            camera.LookAt(targetInterpolator.GetValue(Time.StepTime - StartTime), tivi.DestinationUp);

            scene.Step();
        }

        public override void Render()
        {
            //system.WorldState.Position = scene.ActiveCamera.WorldState.Position;
            //system.WorldState.Rotation = scene.ActiveCamera.WorldState.Rotation;
            foreach (ModelNode diamond in diamonds)
            {
                diamond.WorldState.Turn(turnSpeed * Time.DeltaTime);
            }
            scene.Render();
            //if (Rand.Int(0, 20) == 5 && Time.StepTime > StartTime + 1 && Time.StepTime < EndTime - 1)
            //{
            //    sprite.Begin(SpriteFlags.AlphaBlend);
            //    sprite.Draw2D(cd2Texture, Rectangle.Empty, new SizeF(Device.Viewport.Width, Device.Viewport.Height), new PointF(), Color.White);
            //    sprite.End();
            //}
        }
    }
}
