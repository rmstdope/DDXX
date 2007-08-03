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
        private Color tiviDiffuse;
        private Color tiviAmbient;
        private float tiviReflective;
        private Color tubeDiffuse;
        private Color tubeAmbient;
        private float tubeReflective;
        private ModelNode tube1;
        private ModelNode tube2;

        // film roll stuff
        private TextureViewer amigaViewer;
        private TextureViewer c64Viewer;
        private TextureViewer atariViewer;
        private TextureViewer pcViewer;
        private const int squareCount = 14;
        private const int heightSegments = 6;
        private FilmRoller c64Roller;
        private FilmRoller atariRoller;
        private FilmRoller amigaRoller;
        private FilmRoller pcRoller;


        public float TubeReflective
        {
            get { return tubeReflective; }
            set
            {
                tubeReflective = value;
                if (tube1 != null)
                {
                    tube1.Model.Materials[0].ReflectiveFactor = tubeReflective;
                    tube2.Model.Materials[0].ReflectiveFactor = tubeReflective;
                }
            }
        }

        public Color TubeAmbient
        {
            get { return tubeAmbient; }
            set
            {
                tubeAmbient = value;
                if (tube1 != null)
                {
                    tube1.Model.Materials[0].Ambient = tubeAmbient;
                    tube2.Model.Materials[0].Ambient = tubeAmbient;
                }
            }
        }

        public Color TubeDiffuse
        {
            get { return tubeDiffuse; }
            set
            {
                tubeDiffuse = value;
                if (tube1 != null)
                {
                    tube1.Model.Materials[0].Diffuse = tubeDiffuse;
                    tube2.Model.Materials[0].Diffuse = tubeDiffuse;
                }
            }
        }

        public float TiviReflective
        {
            get { return tiviReflective; }
            set
            {
                tiviReflective = value;
                if (tiviNode != null)
                    tiviNode.Model.Materials[0].ReflectiveFactor = tiviReflective;
            }
        }

        public Color TiviAmbient
        {
            get { return tiviAmbient; }
            set 
            { 
                tiviAmbient = value;
                if (tiviNode != null)
                    tiviNode.Model.Materials[0].Ambient = tiviAmbient;
            }
        }

        public Color TiviDiffuse
        {
            get { return tiviDiffuse; }
            set 
            { 
                tiviDiffuse = value;
                if (tiviNode != null)
                    tiviNode.Model.Materials[0].Diffuse = tiviDiffuse;
            }
        }

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
            tiviAmbient = Color.FromArgb(255, 75, 75, 128);
            tiviDiffuse = Color.FromArgb(255, 128, 128, 128);
            tiviReflective = 0.1f;
            CreateFilmRolls();
        }

        private void CreateFilmRolls()
        {
            amigaViewer = new TextureViewer("texture viewer", StartTime, EndTime);
            c64Viewer = new TextureViewer("texture viewer", StartTime, EndTime);
            atariViewer = new TextureViewer("texture viewer", StartTime, EndTime);
            pcViewer = new TextureViewer("texture viewer", StartTime, EndTime);
            amigaViewer.AddTexture("amiga_1_red_sector_mega_demo.jpg");
            amigaViewer.AddTexture("amiga_2_enigma.jpg");
            amigaViewer.AddTexture("amiga_3_hardwired.jpg");
            amigaViewer.AddTexture("amiga_4_state_of_the_art.jpg");
            amigaViewer.AddTexture("amiga_5_silkcut.jpg");
            c64Viewer.AddTexture("c64_1_en_rutig_banan.png");
            c64Viewer.AddTexture("c64_2_ko_opperation.png");
            c64Viewer.AddTexture("c64_3_brutality.png");
            c64Viewer.AddTexture("c64_4_coma_light_eight.png");
            c64Viewer.AddTexture("c64_5_wonderland_x.jpg");
            atariViewer.AddTexture("atari_1_big_demo.jpg");
            atariViewer.AddTexture("atari_2_union_demo.png");
            atariViewer.AddTexture("atari_3_cuddly_demos.jpg");
            atariViewer.AddTexture("atari_4_mindbomb.jpg");
            atariViewer.AddTexture("atari_5_virtual_escape.jpg");
            pcViewer.AddTexture("pc_1_crystal_dreams_2.jpg");
            pcViewer.AddTexture("pc_2_second_reality.png");
            pcViewer.AddTexture("pc_3_popular_demo.jpg");
            pcViewer.AddTexture("pc_4_we_cell.jpg");
            pcViewer.AddTexture("pc_5_planet_risk.jpg");
            c64Viewer.SetTargetWidthHeight(FilmRoller.TextureWidth, FilmRoller.TextureHeight);
            atariViewer.SetTargetWidthHeight(FilmRoller.TextureWidth, FilmRoller.TextureHeight);
            amigaViewer.SetTargetWidthHeight(FilmRoller.TextureWidth, FilmRoller.TextureHeight);
            pcViewer.SetTargetWidthHeight(FilmRoller.TextureWidth, FilmRoller.TextureHeight);
            c64Roller = new FilmRoller(squareCount, heightSegments, c64Viewer, TextureFactory, TextureBuilder);
            atariRoller = new FilmRoller(squareCount, heightSegments, atariViewer, TextureFactory, TextureBuilder);
            amigaRoller = new FilmRoller(squareCount, heightSegments, amigaViewer, TextureFactory, TextureBuilder);
            pcRoller = new FilmRoller(squareCount, heightSegments, pcViewer, TextureFactory, TextureBuilder);
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
    		(subEffect as DiscoFever).ColorAdd = 21;
    		(subEffect as DiscoFever).ColorDiv = 2;

            subEffect.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);

            InitializeFilmRolls();

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);

            // To make sure stuff is on GPU memory
            Device.BeginScene();
            Render();
            Device.EndScene();
        }

        private void InitializeFilmRolls()
        {
            c64Viewer.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            atariViewer.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            amigaViewer.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            pcViewer.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);

            c64Roller.Initialize(MeshBuilder, GraphicsFactory, EffectFactory, Device);
            atariRoller.Initialize(MeshBuilder, GraphicsFactory, EffectFactory, Device);
            amigaRoller.Initialize(MeshBuilder, GraphicsFactory, EffectFactory, Device);
            pcRoller.Initialize(MeshBuilder, GraphicsFactory, EffectFactory, Device);

            CreateSpline(c64Roller);
            CreateSpline(atariRoller);
            CreateSpline(amigaRoller);
            CreateSpline(pcRoller);

            INode c64Node = c64Roller.FilmNode;
            INode atariNode = atariRoller.FilmNode;
            INode amigaNode = amigaRoller.FilmNode;
            INode pcNode = pcRoller.FilmNode;
            c64Node.WorldState.MoveRight(1.5f);
            c64Node.WorldState.MoveForward(-3.5f);
            c64Node.WorldState.MoveUp(-c64Roller.Height / 2);
            atariNode.WorldState.MoveRight(9.0f);
            atariNode.WorldState.MoveForward(3.5f);
            atariNode.WorldState.MoveUp(-atariRoller.Height / 2);
            amigaNode.WorldState.MoveRight(17.0f);
            amigaNode.WorldState.MoveForward(-3.5f);
            amigaNode.WorldState.MoveUp(-amigaRoller.Height / 2);
            pcNode.WorldState.MoveRight(23.0f);
            pcNode.WorldState.MoveForward(3.5f);
            pcNode.WorldState.MoveUp(-pcRoller.Height / 2);
            scene.AddNode(c64Node);
            scene.AddNode(atariNode);
            scene.AddNode(amigaNode);
            scene.AddNode(pcNode);
        }

        private void CreateSpline(FilmRoller roller)
        {
            ISpline<InterpolatedVector3> spline = new NaturalCubicSpline<InterpolatedVector3>();
            for (double a = 0; a < 6 * Math.PI; a += Math.PI / 8)
            {
                float u = (float)(a / (6 * Math.PI));

                float x = (float)(1.0f * Math.Sin(a));
                float y = u;
                y *= roller.Height;
                float z = (float)(1.0f * Math.Cos(a));
                spline.AddKeyFrame(roller.GetKeyFrame(u, x, y, z));
            }
            spline.Calculate();
            roller.FilmSpline = spline;
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
            tube1 = scene.GetNodeByName("Cylinder01") as ModelNode;
            tube2 = scene.GetNodeByName("Cylinder02") as ModelNode;

            TextureDirector textureDirector = new TextureDirector(TextureBuilder);
            textureDirector.CreatePerlinNoise(10, 12, 0.5f);
            textureDirector.Madd(1.0f, -0.4f);
            ITexture texture = textureDirector.Generate(256, 256, 1, Format.A8R8G8B8);
            //GraphicsStream stream = ShaderLoader.CompileShaderFromFile("Imaginations.psh", "CreateCloudTexture", null, "tx_1_0", ShaderFlags.None);
            //ITexture tex = GraphicsFactory.CreateTexture(Device, 256, 256, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
            //TextureLoader.FillTexture((Texture)((tex as TextureAdapter).BaseTextureDX), new TextureShader(stream));

            walkwayPlane.Model.Materials[0].DiffuseTexture = texture;
            scene.RemoveNode(scene.GetNodeByName("Walkway"));
            tiviNode = (ModelNode)scene.GetNodeByName("TiVi");
            tiviNode.Model.Materials[0].ReflectiveTexture = TextureFactory.CreateCubeFromFile("rnl_cross.dds");
            tiviNode.Model.Materials[0].DiffuseTexture = TextureFactory.CreateFromFile("marble.jpg");
            tiviNode.Model.Materials[0].Ambient = tiviAmbient;
            tiviNode.Model.Materials[0].Diffuse = tiviDiffuse;
            tiviNode.Model.Materials[0].ReflectiveFactor = tiviReflective;
            tiviNode.Model.Materials[1].AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            //scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
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
                // Remove diamonds from the path where TiVi walks
                float up = 1.0f;
                float right = -0.5f;
                while (right > -1.1f && right < 0.2f && up < 2.0f)
                {
                    up = Rand.Float(0, 4);
                    right = Rand.Float(-5, 5);
                }
                node1.WorldState.MoveUp(up);
                node1.WorldState.MoveRight(right);
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
                scene.AddNode(light);
                lights.Add(light);
                //light.SetRenderParameters(Device, GraphicsFactory.CreateSprite(Device), TextureFactory.CreateFromFile("flare.dds"));
            }
            lights[0].Position = new Vector3(0, 0, 0);
            lights[1].Position = new Vector3(20, 0, 0);
        }

        public override void Step()
        {
            float t = Time.StepTime - StartTime;
            float sceneProgress = t / (EndTime - StartTime);
            if (t < camera02Start)
                scene.ActiveCamera = scene.GetNodeByName("Camera01") as IRenderableCamera;
            else if (t < camera03Start)
                scene.ActiveCamera = scene.GetNodeByName("Camera02") as IRenderableCamera;
            else if (t < camera04Start)
                scene.ActiveCamera = scene.GetNodeByName("Camera03") as IRenderableCamera;
            else
                scene.ActiveCamera = scene.GetNodeByName("Camera01") as IRenderableCamera;
            lights[0].Position = new Vector3(18.0f*sceneProgress, 1.5f+(float)Math.Sin(Time.StepTime*0.7f), -5.0f);
            lights[1].Position = new Vector3(18.0f * sceneProgress + 2.0f + (float)Math.Sin(Time.StepTime * 0.4f), 
                                             1.5f,
                                             3.0f*(float)Math.Cos(Time.StepTime * 0.4f));
            scene.ActiveCamera.SetClippingPlanes(0.01f, 1000);
            //scene.ActiveCamera.SetFOV((float)Math.PI / 3);
            StepScreen();
            StepDiamonds();
            StepWalkway();
            StepFilmRolls();
            scene.Step();
        }

        private void StepFilmRolls()
        {
            c64Roller.Step();
            atariRoller.Step();
            amigaRoller.Step();
            pcRoller.Step();
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
