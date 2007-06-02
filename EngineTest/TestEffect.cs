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
        private float[] notesAndTimes = new float[] {
            3.896984f, 78f, 4.108377f, 80f, 4.816084f, 76f, 5.137769f, 80f, 5.670847f, 78f, 5.873049f, 76f, 6.534801f, 73f, 6.957587f, 73f, 
            7.426328f, 81f, 7.849114f, 81f, 8.253518f, 81f, 8.611967f, 81f, 9.191f, 81f, 9.384011f, 80f, 10.01819f, 78f, 10.50531f, 76f, 
            11.04758f, 78f, 11.22221f, 80f, 11.88396f, 76f, 12.30675f, 80f, 12.72954f, 78f, 12.83983f, 76f, 13.54753f, 73f, 14.08061f, 73f, 
            14.54935f, 76f, 14.93538f, 73f, 15.4225f, 76f, 15.78095f, 73f, 16.13021f, 76f, 16.47027f, 73f, 17.15041f, 76f, 17.63753f, 78f, 
            18.23494f, 78f, 18.46472f, 80f, 19.14485f, 76f, 19.56764f, 80f, 19.97204f, 78f, 20.17424f, 76f, 20.81762f, 73f, 21.18526f, 73f, 
            21.61723f, 81f, 21.98487f, 81f, 22.52714f, 81f, 22.75692f, 81f, 23.26242f, 81f, 23.47381f, 80f, 24.08042f, 78f, 24.5124f, 76f, 
            25.0179f, 78f, 25.20172f, 80f, 25.94619f, 76f, 26.31383f, 80f, 26.76419f, 78f, 26.92044f, 76f, 27.52704f, 73f, 28.05093f, 73f, 
            28.40938f, 76f, 28.80459f, 73f, 29.27334f, 76f, 29.68693f, 73f, 30.09133f, 78f, 30.37626f, 78f, 32.27188f, 78f, 74.67802f, 78f, 
            74.95795f, 80f, 75.25741f, 76f, 75.53734f, 80f, 75.88237f, 78f, 75.98002f, 76f, 76.44874f, 73f, 76.6831f, 73f, 77.06067f, 76f, 
            77.46429f, 73f, 77.70517f, 76f, 77.96557f, 81f, 78.34315f, 81f, 78.49938f, 80f, 78.9681f, 78f, 79.13737f, 78f, 79.52797f, 78f, 
            79.85346f, 80f, 80.14642f, 76f, 80.49796f, 80f, 80.68674f, 78f, 80.96017f, 76f, 81.40285f, 73f, 81.65674f, 73f, 82.00177f, 76f, 
            82.3468f, 73f, 82.6137f, 76f, 82.88062f, 76f, 83.29074f, 76f, 83.3884f, 78f, 83.88315f, 78f, 84.11752f, 78f, 84.48859f, 78f, 
            84.81409f, 80f, 85.12657f, 76f, 85.45206f, 80f, 85.74502f, 78f, 85.91427f, 76f, 86.4481f, 73f, 86.72152f, 73f, 86.98192f, 81f, 
            87.33997f, 81f, 87.64594f, 81f, 87.9519f, 81f, 88.30345f, 81f, 88.45318f, 80f, 88.93491f, 78f, 89.14323f, 78f, 89.53384f, 78f, 
            89.75517f, 80f, 90.16531f, 76f, 90.49731f, 80f, 90.78376f, 78f, 90.94f, 76f, 91.42825f, 73f, 91.6561f, 73f, 92.00763f, 81f, 
            92.30058f, 81f, 92.63911f, 81f, 92.95158f, 81f, 93.34219f, 81f, 93.44634f, 80f, 94.03225f, 78f, 104.5784f, 78f, 104.7607f, 80f, 
            105.2034f, 76f, 105.4182f, 80f, 105.7503f, 78f, 105.887f, 76f, 106.3296f, 73f, 106.5249f, 73f, 107.0132f, 76f, 107.3322f, 73f, 
            107.6642f, 76f, 107.9571f, 73f, 108.2501f, 76f, 108.3673f, 73f, 108.7579f, 76f, 109.0248f, 78f, 109.5911f, 78f, 109.8581f, 80f, 110.151f, 76f, 110.4179f, 80f, 110.7564f, 78f, 110.9062f, 76f, 111.3944f, 73f, 111.6158f, 73f, 111.9738f, 81f, 112.3058f, 81f, 112.6574f, 81f, 112.9568f, 81f, 113.3279f, 81f, 113.4711f, 80f, 113.9659f, 78f, 114.1481f, 76f, 114.4736f, 76f, 114.8187f, 76f, 115.0791f, 76f, 115.3916f, 76f, 115.7105f, 76f, 115.9189f, 73f, 116.4071f, 73f, 116.6154f, 73f, 116.9474f, 76f, 117.2339f, 76f, 117.5984f, 76f, 117.8588f, 76f, 118.2495f, 76f, 118.3927f, 78f, 118.907f, 78f, 119.1478f, 78f, 119.5514f, 78f, 119.8314f, 80f, 120.1959f, 76f, 120.4433f, 80f, 120.7753f, 78f, 120.9055f, 76f, 121.3873f, 73f, 121.6477f, 73f, 121.9992f, 81f, 122.3182f, 81f, 122.6502f, 81f, 122.9562f, 81f, 123.3142f, 81f, 123.4575f, 80f, 124.0043f, 78f, 145.93f, 78f, 146.2034f, 80f, 146.4898f, 76f, 146.7698f, 80f, 147.0562f, 78f, 147.1734f, 76f, 147.5444f, 73f, 147.9155f, 73f, 148.3322f, 76f, 148.6772f, 73f, 148.9832f, 76f, 149.2045f, 81f, 149.5625f, 81f, 149.7774f, 80f, 150.168f, 78f, 150.3438f, 76f, 150.845f, 78f, 150.9882f, 80f, 151.4439f, 76f, 151.7434f, 80f, 152.0363f, 78f, 152.2837f, 76f, 152.5246f, 73f, 152.9998f, 73f, 153.2537f, 81f, 153.5727f, 81f, 153.9633f, 81f, 154.1716f, 81f, 154.5492f, 81f, 154.6989f, 80f, 155.0635f, 78f, 155.2458f, 76f, 155.8121f, 78f, 155.9033f, 80f, 156.5217f, 76f, 156.6845f, 76f, 157.0295f, 76f, 157.1467f, 73f, 157.687f, 73f, 158.1688f, 76f, 158.5203f, 76f, 158.8914f, 76f, 159.1908f, 76f, 159.5163f, 76f, 159.6335f, 78f, 160.1608f, 78f, 160.7923f, 78f, 160.929f, 80f, 161.4238f, 76f, 161.6972f, 80f, 162.0097f, 78f, 162.1399f, 76f, 162.5891f, 73f, 162.8364f, 81f, 163.188f, 81f, 163.507f, 81f, 163.826f, 81f, 164.158f, 81f, 164.4965f, 81f, 164.6657f, 80f, 165.1019f, 78f, 167.8557f, 81f, 168.2202f, 81f, 168.5132f, 81f, 168.8321f, 81f, 169.1186f, 81f, 169.4571f, 81f, 169.6524f, 80f, 170.056f, 78f, 177.9722f, 83f, 178.2977f, 83f, 178.5711f, 83f, 178.8576f, 83f, 179.1375f, 83f, 179.4955f, 83f, 179.6648f, 80f, 180.2181f, 78f, 188.4545f, 78f, 188.5004f, 78f, 188.721f, 80f, 189.3f, 76f, 189.7044f, 80f, 190.164f, 78f, 190.4121f, 76f, 191.1107f, 73f, 192.0114f, 76f, 192.4709f, 73f, 192.8478f, 76f, 193.0592f, 73f, 193.5738f, 76f, 193.9139f, 73f, 194.5205f, 76f, 194.6676f, 78f, 195.4856f, 78f, 195.8165f, 80f, 196.3495f, 76f, 196.5425f, 80f, 197.2043f, 78f, 197.4433f, 76f, 198.105f, 73f, 198.8771f, 81f, 199.2998f, 81f, 199.7778f, 81f, 200.0259f, 81f, 200.7428f, 81f, 201.1013f, 80f, 201.5332f, 76f, 201.8549f, 76f, 202.5443f, 78f, 202.7465f, 80f, 203.445f, 76f, 203.7391f, 80f, 204.263f, 78f, 204.4836f, 76f, 204.9615f, 73f, 205.2832f, 73f, 206.0001f, 76f, 206.285f, 73f, 206.7997f, 76f, 207.1673f, 73f, 207.5993f, 76f, 207.8934f, 73f, 208.6379f, 76f, 208.8125f, 78f, 209.6305f, 78f, 209.8327f, 80f, 210.5312f, 76f, 210.7518f, 80f, 211.3676f, 78f, 211.5239f, 76f, 211.9467f, 73f, 212.3694f, 73f, 212.9117f, 76f, 213.408f, 73f, 213.7849f, 76f, 214.0146f, 73f, 214.5385f, 78f, 214.8602f, 78f, 215.6506f, 78f, 217.5807f, 76f, };
        private float timeAdd = 0.2f;

        public float TimeAdd
        {
            get { return timeAdd; }
            set { timeAdd = value; }
        }

        private const int NUM_CIRCLES = 60;
        private struct BlitCircle
        {
            public Vector2 Offset;
            public Vector2 Period;
            public float Size;
            public Vector2 Scale;
        }

        private FloaterSystem ps;
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
            if (distance < 0.5f)
                return new Vector4(1, 1, 1, 1);
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

            ps = new FloaterSystem("System");
            ps.Initialize(50, 200.0f, null);//"BlurBackground.jpg");
            scene.AddNode(ps);

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

            Viewport viewport = Device.Viewport;
            Vector2 point;
            Vector2 center = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Vector2 size = new Vector2(200, 200);
            circleSprite.Begin(SpriteFlags.AlphaBlend);
            point = center - size * 0.5f;
            circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(size.X, size.Y),
                new PointF(point.X, point.Y), Color.White);
            size = new Vector2(800, 800);
            point = center - size * 0.5f;
            point.Y += size.Y / 1.9f;
            circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(size.X, size.Y),
                new PointF(point.X, point.Y), Color.Black);
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

            circleSprite.Begin(SpriteFlags.AlphaBlend);
            for (int i = 0; i < notesAndTimes.Length; i += 2)
            {
                if (Math.Abs(Time.StepTime + timeAdd - notesAndTimes[i]) < 0.1)
                {
                    int relNote = (int)notesAndTimes[i + 1] - 70;
                    circleSprite.Draw2D(circleTexture, Rectangle.Empty, new SizeF(40, 40),
                        new PointF(40 + 40 * relNote, 40), Color.Red);
                }
            }
            circleSprite.End();
            scene.Render();
        }

    }
}
