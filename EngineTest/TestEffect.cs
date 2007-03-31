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
        private FloaterSystem ps;
        private CameraNode camera;
        private IScene scene;
        private ModelNode modelNode;
        private ModelNode clothModel;

        //private ModelNode modelSkinning;
        private ModelNode modelNoSkinning;

        public TestEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
            scene = new Scene();
        }

        public override void Initialize()
        {
            base.Initialize();

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f);

            camera = new CameraNode("MyCamera");
            //camera.WorldState.Tilt(2.0f);
            camera.WorldState.MoveForward(-30.0f);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;

            ps = new FloaterSystem("System");
            ps.Initialize(50, 200.0f, null);//"BlurBackground.jpg");
            scene.AddNode(ps);

            // Create mesh
            IEffect effect = D3DDriver.EffectFactory.CreateFromFile("Test.fxo");
            IModel model = D3DDriver.ModelFactory.FromFile("Wanting More.x", ModelOptions.None);
            EffectHandler effectHandler = new EffectHandler(effect, "TransparentText", model);
            modelNode = new ModelNode("Text1", model, effectHandler);
            scene.AddNode(modelNode);
            //mesh.WorldState.Tilt(-(float)Math.PI / 2.0f);

            model = ModelFactory.FromFile("TiVi.x", ModelOptions.None);
            modelNoSkinning = new ModelNode("No Skinning",
                model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"), "Skinning", model));
            modelNoSkinning.WorldState.Scale(100.0f);
            modelNoSkinning.WorldState.MoveRight(-50);
            modelNoSkinning.WorldState.Roll((float)Math.PI);
            modelNoSkinning.WorldState.Tilt((float)Math.PI / 2);
            //scene.AddNode(modelNoSkinning);

            XLoader.Load("Flyscene.x", EffectFactory.CreateFromFile("Test.fxo"), "Skinning");
            XLoader.AddToScene(scene);
            //scene.ActiveCamera = scene.GetNodeByName("Camera") as CameraNode;

            MeshBuilder builder = new MeshBuilder(D3DDriver.GraphicsFactory, D3DDriver.TextureFactory, 
                D3DDriver.GetInstance().Device);
            const int numSides = 20;
            Body body = new Body();
            body.Gravity = new Vector3(0, -3, 0);

            Primitive primitive = Primitive.ClothPrimitive(body, 20, 20, numSides, numSides,
                new int[] { 0, /*(numSides + 1) */ numSides }, true);
            Material material = new Material();
            material.Ambient = Color.White;
            material.Diffuse = Color.Red;
            ExtendedMaterial extendedMaterial = new ExtendedMaterial();
            extendedMaterial.Material3D = material;
            extendedMaterial.TextureFilename = "ABALONE.JPG";
            primitive.Material = extendedMaterial;
            builder.AddPrimitive(primitive, "Box");
            model = builder.CreateModel("Box");
            clothModel = new ModelNode("Box", model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"), "Test", model));
            scene.AddNode(clothModel);

            scene.DebugPrintGraph();
            scene.Validate();
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
            // Long period
            Vector3 direction = new Vector3(
                (float)Math.Sin(Time.CurrentTime / 5), 
                0, 
                (float)Math.Cos(Time.CurrentTime / 5));
            Vector3 force = new Vector3(0, 0, -(float)Math.Abs(Math.Cos(Time.CurrentTime)) * 3);
            ((PhysicalModel)clothModel.Model).Body.ApplyForce(force);

            float scale = (Time.StepTime % 5.0f) / 5.0f;
            scale *= 2.0f;
            modelNode.WorldState.Scaling = new Vector3(scale, scale, scale);
            modelNode.WorldState.Position = new Vector3(0, scale * 200.0f, 0);

            modelNode.WorldState.Roll(scale / 100.0f);
            modelNode.WorldState.Turn(Time.DeltaTime);
            //clothModel.WorldState.Tilt(Time.DeltaTime);
            scene.Step();
        }

        public override void Render()
        {
            IDevice device = D3DDriver.GetInstance().Device;
            scene.Render();
        }

    }
}
