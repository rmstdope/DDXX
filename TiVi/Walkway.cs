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
        private CameraNode camera;
        private List<PointLightNode> lights = new List<PointLightNode>();
        private ModelNode tiviNode;
        private List<MirrorNode> mirrors = new List<MirrorNode>();
        private MeshDirector director;
        private List<ModelNode> diamonds = new List<ModelNode>();
        private DummyNode diamondBase;
        private ModelNode plane;
        private ModelNode cylinder;
        private ModelNode blackPlane;

        public Walkway(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            director = new MeshDirector(MeshBuilder);

            CreateStandardSceneAndCamera(out scene, out camera, 10);
            camera.WorldState.MoveUp(1.5f);

            CreateLights();
            CreateDiamonds();
            CreateTiVi();
            //CreateWalkway();

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
        }

        private void CreateWalkway()
        {
            director.CreatePlane(2, 20, 1, 1, true);
            director.UvMapPlane(2, 0.2f, 2);
            director.Rotate((float)Math.PI / 2, 0, 0);
            //director.Rotate(0, 0, (float)Math.PI / 2);
            IModel model = director.Generate("Default1");
            IEffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Atmosphere"; }, model);
            plane = new ModelNode("", model, effectHandler, Device);
            plane.WorldState.MoveForward(-2);
            //plane.WorldState.MoveUp(-0.1f);
            GraphicsStream stream = ShaderLoader.CompileShaderFromFile("Imaginations.psh", "CreateCloudTexture", null, "tx_1_0", ShaderFlags.None);
            ITexture tex = GraphicsFactory.CreateTexture(Device, 256, 256, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            TextureLoader.FillTexture((Texture)((tex as TextureAdapter).BaseTextureDX), new TextureShader(stream));
            tex.Save("clouds.dds", ImageFileFormat.Dds);
            plane.Model.Materials[0].DiffuseTexture = tex;

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
                                    return "Solid";
                            };
                        case "Plane01":
                            return TechniqueChooser.MaterialPrefix("Atmosphere");
                        default:
                        return TechniqueChooser.MaterialPrefix("Terrain");
                    }
                });
            XLoader.AddToScene(scene);
            plane = scene.GetNodeByName("Plane01") as ModelNode;
            GraphicsStream stream = ShaderLoader.CompileShaderFromFile("Imaginations.psh", "CreateCloudTexture", null, "tx_1_0", ShaderFlags.None);
            ITexture tex = GraphicsFactory.CreateTexture(Device, 256, 256, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            TextureLoader.FillTexture((Texture)((tex as TextureAdapter).BaseTextureDX), new TextureShader(stream));
            plane.Model.Materials[0].DiffuseTexture = tex;
            scene.RemoveNode(scene.GetNodeByName("Plane01"));
            tiviNode = (ModelNode)scene.GetNodeByName("TiVi");
            /// TODO: Framehandling is buggy!
            tiviNode.WorldState.MoveForward(0);
            (tiviNode.Model as SkinnedModel).SetAnimationSet(0, StartTime, 1.03f);
            scene.ActiveCamera = scene.GetNodeByName("Camera01") as CameraNode;

            CreateMirrorOfNode(scene.GetNodeByName("Cylinder01"));
            CreateMirrorOfNode(scene.GetNodeByName("Cylinder02"));
            CreateMirrorOfNode(scene.GetNodeByName("ChamferBox01"));
            CreateMirrorOfNode(scene.GetNodeByName("Text02"));
            CreateMirrorOfNode(scene.GetNodeByName("TiVi"));

            CreateBlackPlane();
        }

        private void CreateBlackPlane()
        {
            MeshBuilder.SetAmbientColor("Default1", ColorValue.FromArgb(0));
            MeshBuilder.SetDiffuseColor("Default1", ColorValue.FromArgb(0));
            director.CreatePlane(50, 50, 1, 1, true);
            director.Rotate((float)Math.PI / 2, 0, 0);
            director.Translate(0, -0.01f, 0);
            IModel model = director.Generate("Default1");
            IEffectHandler effectHandler = new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(int material) { return "Terrain"; }, model);
            blackPlane = new ModelNode("", model, effectHandler, Device);
        }

        private void CreateMirrorOfNode(INode node)
        {
            MirrorNode mirrorNode = new MirrorNode(node);
            mirrorNode.Brightness = 0.2f;
            mirrors.Add(mirrorNode);
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
                ModelNode node1 = new ModelNode("", model, handler, Device);
                ModelNode node2 = new ModelNode("", model, handler, Device);
                node1.WorldState.MoveUp(Rand.Float(0, 4));
                node1.WorldState.MoveRight(Rand.Float(-5, 5));
                node1.WorldState.MoveForward(Rand.Float(-15, 15));
                node2.WorldState.Position =
                    new Vector3(node1.WorldState.Position.X, -node1.WorldState.Position.Y, node1.WorldState.Position.Z);
                float t = Rand.Float(Math.PI);
                node1.WorldState.Turn(t);
                node2.WorldState.Turn(t);
                diamonds.Add(node1);
                //diamonds.Add(node2);
                diamondBase.AddChild(node1);
                //diamondBase.AddChild(node2);
            }
            CreateMirrorOfNode(diamondBase);
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
            //camera.WorldState.Reset();
            //camera.WorldState.Position = new Vector3((float)Math.Sin(Time.StepTime / 4) * 10,
            //    2.0f,
            //    (float)Math.Cos(Time.StepTime / 4) * 10);
            //camera.LookAt(new Vector3(0, 1.0f, -5), new Vector3(0, 1, 0));

            StepDiamonds();
            StepWalkway();
            scene.Step();
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
            }
        }

        public override void Render()
        {
            scene.SetEffectParameters();
            foreach (MirrorNode node in mirrors)
                node.Render(scene);
            diamondBase.Render(scene);
            plane.Render(scene);
            blackPlane.Render(scene);
            scene.Render();
        }
    }
}
