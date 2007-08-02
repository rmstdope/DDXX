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
    public class Walkway : BaseDemoEffect
    {
        private const int NUM_RINGS = 6;
        private const int NUM_LIGHTS = 2;
        private const int NUM_BLOCKS = 24;
        private IScene scene;
        private List<PointLightNode> lights = new List<PointLightNode>();
        private ModelNode tiviNode;
        private List<MirrorNode> mirrors = new List<MirrorNode>();
        private MeshDirector director;
        private List<ModelNode> diamonds = new List<ModelNode>();
        private DummyNode diamondBase;
        private ModelNode walkwayPlane;
        private ModelNode walkwayStencilPlane;
        //private ModelNode cylinder;
        private ITexture screenTexture;
        private IDemoEffect subEffect;
        private float camera02Start;
        private float camera03Start;
        private float camera04Start;

        public float Camera04Start
        {
            get { return camera04Start; }
            set { camera04Start = value; }
        }

        public float Camera03Start
        {
            get { return camera03Start; }
            set { camera03Start = value; }
        }

        public float Camera02Start
        {
            get { return camera02Start; }
            set { camera02Start = value; }
        }

        public Walkway(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            camera02Start = 14;
            camera03Start = 21;
        }

        protected override void Initialize()
        {
            director = new MeshDirector(MeshBuilder);

            scene = new Scene(EffectFactory);

            CreateScreenTexture();
            CreateLights();
            CreateDiamonds();
            CreateTiVi();

            subEffect = new DiscoFever("screeneffect", StartTime, EndTime);
            subEffect.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);

            // To make sure stuff is on GPU memory
            Device.BeginScene();
            Render();
            Device.EndScene();
        }

        private void CreateScreenTexture()
        {
            screenTexture = TextureFactory.CreateFullsizeRenderTarget();
        }

        private void CreateTiVi()
        {
            XLoader.Load("Tivi-Walkway.X", EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(string name)
                {
                    switch (name)
                    {
                        case "TiVi":
                            return delegate(int material)
                            {
                                if (material == 1)
                                    return "TvScreen";
                                else
                                    return "TiViReflective";
                            };
                        case "Walkway":
                            return TechniqueChooser.MaterialPrefix("TiViWalkwayMirror");
                        default:
                            return TechniqueChooser.MaterialPrefix("Terrain");
                    }
                });
            XLoader.AddToScene(scene);
            walkwayPlane = scene.GetNodeByName("Walkway") as ModelNode;
            walkwayStencilPlane = CreateStencilNodeOfNode(walkwayPlane);
            GraphicsStream stream = ShaderLoader.CompileShaderFromFile("Imaginations.psh", "CreateCloudTexture", null, "tx_1_0", ShaderFlags.None);
            ITexture tex = GraphicsFactory.CreateTexture(Device, 256, 256, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            TextureLoader.FillTexture((Texture)((tex as TextureAdapter).BaseTextureDX), new TextureShader(stream));
            walkwayPlane.Model.Materials[0].DiffuseTexture = tex;
            scene.RemoveNode(scene.GetNodeByName("Walkway"));
            tiviNode = (ModelNode)scene.GetNodeByName("TiVi");
            tiviNode.Model.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
            tiviNode.Model.Materials[0].DiffuseTexture = TextureFactory.CreateFromFile("marble.jpg");
            tiviNode.Model.Materials[0].AmbientColor = new ColorValue(0.3f, 0.3f, 0.3f, 0.5f);
            tiviNode.Model.Materials[0].DiffuseColor = new ColorValue(0.5f, 0.5f, 0.5f, 0.5f);
            tiviNode.Model.Materials[0].ReflectiveFactor = 0.1f;
            float timeScale = 1.045f;
            (tiviNode.Model as SkinnedModel).SetAnimationSet(0, StartTime, timeScale);
            scene.ActiveCamera = scene.GetNodeByName("Camera01") as CameraNode;
            tiviNode.Model.Materials[1].DiffuseTexture = screenTexture;

            CreateMirrorOfNode(scene.GetNodeByName("Cylinder01"), 0.2f);
            CreateMirrorOfNode(scene.GetNodeByName("Cylinder02"), 0.2f);
            CreateMirrorOfNode(scene.GetNodeByName("ChamferBox01"), 0.2f);
            CreateMirrorOfNode(scene.GetNodeByName("Text02"), 0.2f);
            CreateMirrorOfNode(scene.GetNodeByName("TiVi"), 0.2f);
        }

        private ModelNode CreateStencilNodeOfNode(ModelNode originalNode)
        {
            IModel newModel = originalNode.Model.Clone();
            ModelNode node = new ModelNode("Stencil" + originalNode.Name, newModel,
                new EffectHandler(originalNode.EffectHandler.Effect, TechniqueChooser.MaterialPrefix("StencilOnly"), newModel), Device);
            return node;
        }

        private void CreateMirrorOfNode(INode node, float brightness)
        {
            MirrorNode mirrorNode = new MirrorNode(node);
            mirrorNode.Brightness = brightness;
            mirrors.Add(mirrorNode);
        }

        private void CreateDiamonds()
        {
            const int NUM_DIAMONDS = 100;
            MeshBuilder.SetDiffuseTexture("Default3", "square.tga");
            director.CreateChamferBox(1, 1, 0.4f, 0.2f, 4);
            director.UvMapPlane(1, 1, 1);
            director.Rotate((float)Math.PI / 2, 0, 0);
            director.Rotate(0, 0, (float)Math.PI / 4);
            director.Scale(0.15f, 0.3f, 0.3f);
            IModel model = director.Generate("Default3");
            model.Mesh.ComputeNormals();
            EffectHandler handler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Diamond"; }, model);
            diamondBase = new DummyNode("DiamondBase");
            diamondBase.WorldState.Turn(-(float)Math.PI / 2);
            diamondBase.WorldState.MoveForward(-10);
            for (int i = 0; i < NUM_DIAMONDS; i++)
            {
                ModelNode node1 = new ModelNode("", model.Clone(), handler, Device);
                node1.WorldState.MoveUp(Rand.Float(0, 4));
                node1.WorldState.MoveRight(Rand.Float(-5, 5));
                node1.WorldState.MoveForward(Rand.Float(-15, 15));
                float t = Rand.Float(Math.PI);
                node1.WorldState.Turn(t);
                diamonds.Add(node1);
                diamondBase.AddChild(node1);
            }
            CreateMirrorOfNode(diamondBase, 0.05f);
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

        public override void Step()
        {
            float t = Time.StepTime - StartTime;
            if (t < camera02Start)
                scene.ActiveCamera = scene.GetNodeByName("Camera01") as IRenderableCamera;
            else if (t < camera03Start)
                scene.ActiveCamera = scene.GetNodeByName("Camera02") as IRenderableCamera;
            else if (t < camera04Start)
                scene.ActiveCamera = scene.GetNodeByName("Camera03") as IRenderableCamera;
            else
                scene.ActiveCamera = scene.GetNodeByName("Camera01") as IRenderableCamera;
            scene.ActiveCamera.SetClippingPlanes(0.01f, 1000);
            //scene.ActiveCamera.SetFOV((float)Math.PI / 3);
            StepScreen();
            StepDiamonds();
            StepWalkway();
            scene.Step();
        }

        private void StepScreen()
        {
            subEffect.Step();
            using (ISurface original = Device.GetRenderTarget(0))
            {
                using (ISurface surface = screenTexture.GetSurfaceLevel(0))
                {
                    Device.SetRenderTarget(0, surface);
                    Device.BeginScene();
                    Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1, 0);
                    subEffect.Render();
                    Device.EndScene();
                    Device.SetRenderTarget(0, original);
                }
            }
        }

        private void StepWalkway()
        {
            float[] f = new float[] { Time.StepTime * 0.075f, Time.StepTime * 0.063f };
            EffectFactory.CreateFromFile("TiVi.fxo").SetValue(EffectHandle.FromString("AtmosphereTime"), f);

            //cylinder.WorldState.Roll(Time.DeltaTime);
            //cylinder.WorldState.Turn(Time.DeltaTime);
        }

        private void StepDiamonds()
        {
            foreach (ModelNode node in diamonds)
            {
                node.WorldState.Turn(Time.DeltaTime * 4);
                node.WorldState.Position += new Vector3(0, 0, Time.DeltaTime * 2.5f);
                if (node.WorldState.Position.Z > 15)
                    node.WorldState.Position += new Vector3(0, 0, -30);
                float d = 1;
                if (node.WorldState.Position.Z > 8)
                    d = (15 - node.WorldState.Position.Z) / 7;
                if (node.WorldState.Position.Z < -8)
                    d = (node.WorldState.Position.Z + 15) / 7;
                node.Model.Materials[0].AmbientColor = new ColorValue(0.2f, 0.2f, 0.2f, 1.0f * d);
                node.Model.Materials[0].DiffuseColor = new ColorValue(0.5f, 0.5f, 1.0f, 1.0f * d);
                node.Model.Materials[0].SpecularColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f * d);
            }
        }

        public override void Render()
        {
            scene.SetEffectParameters();

            walkwayPlane.AddChild(walkwayStencilPlane);
            walkwayStencilPlane.Render(scene);
            walkwayPlane.RemoveChild(walkwayStencilPlane);
            foreach (MirrorNode node in mirrors)
                node.Render(scene);
            walkwayPlane.Render(scene);

            diamondBase.Render(scene);
            scene.Render();
        }
    }
}
