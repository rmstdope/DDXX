using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.MeshBuilder;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using TiVi;
using Dope.DDXX.DemoEffects;
using Dope.DDXX.TextureBuilder;
using Microsoft.DirectX;

namespace TiVi
{
    public class FilmRoll : BaseDemoEffect
    {
        private CameraNode camera;
        private IScene scene;
        private TextureViewer amigaViewer;
        private TextureViewer c64Viewer;
        private TextureViewer atariViewer;
        private TextureViewer pcViewer;
        private const int squareCount = 10;
        private const int heightSegments = 6;
        private FilmRoller c64Roller;
        private FilmRoller atariRoller;
        private FilmRoller amigaRoller;
        private FilmRoller pcRoller;

        public FilmRoll(string name, float start, float end)
            : base(name, start, end)
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
            c64Viewer.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            atariViewer.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            amigaViewer.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            pcViewer.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            
            c64Roller.Initialize(MeshBuilder, GraphicsFactory, EffectFactory, Device);
            atariRoller.Initialize(MeshBuilder, GraphicsFactory, EffectFactory, Device);
            amigaRoller.Initialize(MeshBuilder, GraphicsFactory, EffectFactory, Device);
            pcRoller.Initialize(MeshBuilder, GraphicsFactory, EffectFactory, Device);

            CreateSpiralSpline(c64Roller);
            CreateSpiralSpline(atariRoller);
            CreateSpiralSpline(amigaRoller);
            CreateSpiralSpline(pcRoller);

            CreateStandardSceneAndCamera(out scene, out camera, 15);

            //MeshBuilder.SetDiffuseTexture("Default1", "FLOWER6P.jpg");

            INode c64Node = c64Roller.FilmNode;
            INode atariNode = atariRoller.FilmNode;
            INode amigaNode = amigaRoller.FilmNode;
            INode pcNode = pcRoller.FilmNode;
            c64Node.WorldState.MoveRight(6.0f);
            atariNode.WorldState.MoveRight(6.0f);
            atariNode.WorldState.MoveForward(6.0f);
            amigaNode.WorldState.MoveRight(-6.0f);
            amigaNode.WorldState.MoveForward(6.0f);
            pcNode.WorldState.MoveRight(-6.0f);
            //film.WorldState.Turn(-(float)(-Math.PI / 4));
            //film.WorldState.Tilt(-(float)(-Math.PI / 3));
            //film.WorldState.MoveUp(-5);
            //camera.WorldState.Tilt(-(float)(Math.PI / 4));
            camera.WorldState.MoveUp(4);
            scene.AddNode(c64Node);
            scene.AddNode(atariNode);
            scene.AddNode(amigaNode);
            scene.AddNode(pcNode);

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            //Mixer.ClearColor = Color.Blue;

        }

        private void CreateSpiralSpline(FilmRoller roller)
        {
            ISpline<InterpolatedVector3> path = new NaturalCubicSpline<InterpolatedVector3>();
            path.AddKeyFrame(roller.GetKeyFrame(0, 0, 0, 0));
            path.AddKeyFrame(roller.GetKeyFrame(5.0f, 0, 4, 0));
            path.AddKeyFrame(roller.GetKeyFrame(10.0f, 0, 8, 0));
            path.Calculate();
            ISpline<InterpolatedVector3> spline = new NaturalCubicSpline<InterpolatedVector3>();
            Vector3 lastUp = new Vector3(0, 0, 1);
            for (double b = 0; b < 10.0f; b += 1.0f)
            {
                for (double a = 0; a < 4 * Math.PI; a += Math.PI / 8)
                {
                    float u = (float)(a / (4 * Math.PI));

                    Vector3 forward = path.GetDerivative((float)(b + u));
                    forward.Normalize();
                    Vector3 right = Vector3.Cross(lastUp, forward);
                    right.Normalize();
                    Vector3 up = Vector3.Cross(forward, right);
                    up.Normalize();
                    lastUp = up;

                    float x = (float)(1.5f * Math.Sin(a));
                    float y = u;
                    //y *= roller.Height;
                    float z = (float)(1.5f * Math.Cos(a));
                    forward.Multiply(z);
                    right.Multiply(x);
                    up.Multiply(y);
                    Vector3 pos = forward + right + up;
                    pos += path.GetValue((float)(b+u));
                    spline.AddKeyFrame(roller.GetKeyFrame((float)(b + u), pos.X, pos.Y, pos.Z));
                }
            }
            spline.Calculate();
            roller.FilmSpline = spline;
        }

        public override void Step()
        {
            //camera.WorldState.Reset();
            //camera.WorldState.MoveForward(-10+(float)(5 * Math.Sin(Time.StepTime)));
            //camera.WorldState.MoveRight((float)(5 * Math.Cos(Time.StepTime)));
            c64Roller.Step();
            atariRoller.Step();
            amigaRoller.Step();
            pcRoller.Step();
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
