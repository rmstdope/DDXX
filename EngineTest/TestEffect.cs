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
        private IBoundingObject sphere;

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
            camera.WorldState.MoveForward(-3.0f);
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
            //XLoader.AddToScene(scene);
            //scene.ActiveCamera = scene.GetNodeByName("Camera") as CameraNode;

            MeshBuilder builder = new MeshBuilder(D3DDriver.GraphicsFactory, D3DDriver.TextureFactory, 
                D3DDriver.GetInstance().Device);
            const int numSides = 10;
            Body body = new Body();

            int[] pinned = new int[numSides + 1];
            for (int i = 0; i < numSides + 1; i++)
                pinned[i] = i;
            builder.CreateCloth("Cloth", body, 2, 2, numSides, numSides,
                pinned, true);
            builder.AssignMaterial("Cloth", "Default1");
            builder.SetDiffuseTexture("Default1", "red glass.jpg");

            model = builder.CreateModel("Cloth");
            clothModel = new ModelNode("Cloth", model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"), "Glass", model));
            scene.AddNode(clothModel);

            // Fix sphere
            sphere = new BoundingSphere(0.5f);
            for (int i = 0; i < body.Particles.Count; i++)
                body.AddConstraint(new BoundingConstraint(body.Particles[i], sphere));

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
            // Move sphere
            Vector3 pos = new Vector3(0, 0.5f, 2 * (float)Math.Cos(Time.CurrentTime / 4));
            sphere.Center = pos;
            //sphere.

            // Long period
            Vector3 direction = new Vector3(
                (float)Math.Sin(Time.CurrentTime / 5), 
                0, 
                (float)Math.Cos(Time.CurrentTime / 5));
            Vector3 force = new Vector3(0, 0, -(float)Math.Abs(Math.Cos(Time.CurrentTime)) * 10);
            //((PhysicalModel)clothModel.Model).Body.ApplyForce(force);

            float scale = (Time.StepTime % 5.0f) / 5.0f;
            scale *= 2.0f;
            modelNode.WorldState.Scaling = new Vector3(scale, scale, scale);
            modelNode.WorldState.Position = new Vector3(0, scale * 200.0f, 0);

            modelNode.WorldState.Roll(scale / 100.0f);
            modelNode.WorldState.Turn(Time.DeltaTime);
            scene.Step();
        }

        public override void Render()
        {
            IDevice device = D3DDriver.GetInstance().Device;
            scene.Render();
        }

    }
}
