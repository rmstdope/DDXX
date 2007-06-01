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
using Dope.DDXX.Graphics.Skinning;
using Dope.DDXX.MeshBuilder;

namespace EngineTest
{
    public class TestEffect : BaseDemoEffect
    {
        private const int NUM_CIRCLES = 60;
        private struct BlitCircle
        {
            public Vector2 Offset;
            public Vector2 Period;
            public float Size;
            public Vector2 Scale;
        }

        //private FloaterSystem ps;
        private CameraNode camera;
        private IScene scene;
        private ModelNode modelNode;
        private ModelNode clothModel;
        private IBoundingObject sphere;
        private float reflectiveFactor;
        private PointLightNode light;
        private ITexture circleTexture;
        private ISprite circleSprite;
        private BlitCircle[] circles;

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
            if (distance < 0.3f)
                return new Vector4(1, 1, 1, 1);
            else if (distance < 0.5f)
            {
                float scaled = (0.5f - distance) / 0.2f;
                return new Vector4(scaled, scaled, scaled, scaled);
            }
            return new Vector4(0, 0, 0, 0);
        }

        protected override void Initialize()
        {
            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f);

            camera = new CameraNode("MyCamera");
            //camera.WorldState.Tilt(2.0f);
            camera.WorldState.MoveForward(-3.0f);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;

            circleTexture = TextureFactory.CreateFromFunction(512, 512, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback);
            circleSprite = GraphicsFactory.CreateSprite(Device);
            circles = new BlitCircle[NUM_CIRCLES];
            Random rand = new Random();
            for (int i = 0; i < NUM_CIRCLES; i++)
            {
                circles[i] = new BlitCircle();
                circles[i].Offset = new Vector2((float)(2 * Math.PI * rand.NextDouble()),
                    (float)(2 * Math.PI * rand.NextDouble()));
                circles[i].Period = new Vector2((float)(0.5f + 3 * rand.NextDouble()),
                    (float)(0.5f + 3 * rand.NextDouble()));
                circles[i].Size = 10 + rand.Next(20);
                circles[i].Scale = new Vector2(50 + 80 * (float)(rand.NextDouble()),
                    50 + 80 * (float)(rand.NextDouble()));
            }

            //circleTexture.Save("test.jpg", ImageFileFormat.Jpg);

            //ps = new FloaterSystem("System");
            //ps.Initialize(50, 200.0f, null);//"BlurBackground.jpg");
            //scene.AddNode(ps);

            //AddWantingMoreModel();

            //AddUnskinnedModel();

            //LoadFlyScene();

            //TestMeshBuilder();

            //scene.DebugPrintGraph();
            scene.Validate();
        }

        private void TestMeshBuilder()
        {
            MeshBuilder builder = new MeshBuilder(D3DDriver.GraphicsFactory, D3DDriver.TextureFactory,
                D3DDriver.GetInstance().Device);
            const int numSides = 10;
            Body body = new Body();
            //body.Gravity = new Vector3(0, -0.2f, 0);

            int[] pinned = new int[numSides + 1];
            for (int i = 0; i < numSides + 1; i++)
                pinned[i] = i;
            builder.CreateCloth("Cloth", body, 2, 2, numSides, numSides,
                pinned, true);
            builder.AssignMaterial("Cloth", "Default1");
            builder.SetDiffuseTexture("Default1", "red glass.jpg");
            builder.SetReflectiveTexture("Default1", "rnl_cross.dds");
            builder.SetReflectiveFactor("Default1", 0.2f);

            IModel model = builder.CreateModel("Cloth");
            model.Materials[0].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f);
            clothModel = new ModelNode("Cloth", model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"),
                delegate(int material) { return "Glass"; }, model));
            scene.AddNode(clothModel);

            // Fix sphere
            sphere = new BoundingSphere(0.5f);
            for (int i = 0; i < body.Particles.Count; i++)
                body.AddConstraint(new BoundingConstraint(body.Particles[i], sphere));

            model = builder.CreateSkyBoxModel("SkyBox", "rnl_cross.dds");
            ModelNode skyBoxModel = new ModelNode("SkyBox", model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"),
                delegate(int material) { return "SkyBox"; }, model));
            scene.AddNode(skyBoxModel);

            light = new PointLightNode("");
            light.DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f);
            scene.AddNode(light);
        }

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
            modelNode = new ModelNode("Text1", model, effectHandler);
            scene.AddNode(modelNode);
        }

        private void AddUnskinnedModel()
        {
            IModel model = ModelFactory.FromFile("TiVi.x", ModelOptions.None);
            modelNoSkinning = new ModelNode("No Skinning",
                model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"),
                delegate(int material) { return "Skinning"; }, model));
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

            scene.Step();
        }

        public override void Render()
        {
            IDevice device = D3DDriver.GetInstance().Device;
            scene.Render();

            Viewport viewport = Device.Viewport;
            Vector2 point;
            Vector2 center = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Vector2 size = new Vector2(200, 200);
            circleSprite.Begin(SpriteFlags.AlphaBlend);
            point = center - size * 0.5f;
            circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(size.X, size.Y),
                new PointF(point.X, point.Y), Color.White);
            //size = new Vector2(800, 800);
            //point = center - size * 0.5f;
            //point.Y += size.Y / 1.9f;
            //circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(size.X, size.Y),
            //    new PointF(point.X, point.Y), Color.Black);
            for (int i = 0; i < NUM_CIRCLES; i++)
            {
                float x = circles[i].Offset.X + Time.CurrentTime / circles[i].Period.X;
                float y = circles[i].Offset.Y + Time.CurrentTime / circles[i].Period.Y;
                Vector2 distortion =
                    new Vector2(circles[i].Scale.X * (float)Math.Sin(x),
                                circles[i].Scale.Y * (float)Math.Cos(y));
                size = new Vector2(circles[i].Size, circles[i].Size);
                point = center - size * 0.5f + distortion;
                circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(size.X, size.Y),
                    new PointF(point.X, point.Y), Color.Black);
            }
            circleSprite.End();
        }

    }
}
