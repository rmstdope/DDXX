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

namespace EngineTest
{
    public class FilmRoll : BaseDemoEffect
    {
        private DummyNode film;
        private float xAngle;
        private float yAngle;
        private float zAngle;
        private CameraNode camera;
        private IScene scene;
        private ITexture filmTexture;
        private IDemoEffect subEffect;
        private IDemoPostEffect subPostEffect;
        private ISprite sprite;
        private ITexture filmSquareTexture;
        private float rounding = 0.05f;
        private float filmWidth = 0.6f;
        private float squareWidth = 1.37f;
        private float squareHeight = 1.0f;
        private int squareCount = 10;

        public FilmRoll(string name, float start, float end)
            : base(name, start, end)
        {
            filmTexture = TextureFactory.CreateRenderTarget(256, 256, Format.A8R8G8B8);
            SetStepSize(GetTweakableNumber("FilmWidth"), 0.01f);
            SetStepSize(GetTweakableNumber("FilmRounding"), 0.01f);
        }

        protected override void Initialize()
        {
            InitializeSubEffects();

            CreateStandardSceneAndCamera(out scene, out camera, 12);
            film = new DummyNode("Film roll");

            //MeshBuilder.SetDiffuseTexture("Default1", "FLOWER6P.jpg");
            MeshDirector meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreatePlane(squareWidth, squareHeight, 1, 1, true);
            IModel model = meshDirector.Generate("Default1");
            model.Materials[0].AmbientColor = ColorValue.FromColor(Color.White);
            model.Materials[0].DiffuseTexture = filmTexture;

            for (int i = 0; i < squareCount; i++) {
                ModelNode plane = CreateSimpleModelNode(model.Clone(), "TiVi.fxo", "Simple");
                plane.WorldState.MoveUp(i * squareHeight - (squareCount / 2) * squareHeight);
                film.AddChild(plane);
            }
            film.WorldState.Tilt((float)(Math.PI / 4));
            //film.WorldState.MoveUp(-5);
            //camera.WorldState.Tilt(-(float)(Math.PI / 4));
            scene.AddNode(film);
            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            Mixer.ClearColor = Color.Blue;
        }

        private void InitializeSubEffects() {
            subEffect = new DiscoFever("film effect", StartTime, EndTime);
            subEffect.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
            subPostEffect = new AspectilizerPostEffect("aspectilizer", StartTime, EndTime);
            sprite = GraphicsFactory.CreateSprite(Device);
            CreateFilmSquareTexture();
        }
        public float FilmRounding {
            get { return rounding; }
            set { rounding = value; CreateFilmSquareTexture(); }
        }

        public float FilmWidth {
            get { return filmWidth; }
            set { filmWidth = value; CreateFilmSquareTexture(); }
        }

        private void CreateFilmSquareTexture() {
            const int width = 256;
            const int height = 256;
            if (Device != null) {
                float ratio = 1.37f * squareHeight / squareWidth;
                Vector2 size = new Vector2(FilmWidth, FilmWidth / ratio);
                IGenerator rect = new RoundedRectangle(size, new Vector2(0.5f, 0.5f), FilmRounding);
                filmSquareTexture = TextureBuilder.Generate(rect, width, height, 1, Format.A8R8G8B8);
            }
        }


        public float XAngle
        {
            set { xAngle = value; }
            get { return xAngle; }
        }

        public float YAngle
        {
            set { yAngle = value; }
            get { return yAngle; }
        }

        public float ZAngle
        {
            set { zAngle = value; }
            get { return zAngle; }
        }

        public override void Step()
        {
            //film.WorldState.ResetRotation();
            //film.WorldState.Roll(ZAngle);
            //film.WorldState.Turn(YAngle);
            //film.WorldState.Tilt(XAngle);
            StepSubEffect();
            scene.Step();
        }

        private void StepSubEffect() {
            subEffect.Step();
            using (ISurface original = Device.GetRenderTarget(0)) {
                using (ISurface surface = filmTexture.GetSurfaceLevel(0)) {
                    Device.SetRenderTarget(0, surface);
                    Device.BeginScene();
                    Device.Clear(ClearFlags.Target, Color.Black, 0, 0);
                    subEffect.Render();
                    Device.EndScene();

                    //PostProcessor.StartFrame(filmTexture);
                    //subPostEffect.Render();

                    sprite.Begin(SpriteFlags.AlphaBlend);
                    sprite.Draw2D(filmSquareTexture, Rectangle.Empty, SizeF.Empty, 
                        new PointF(0, 0), Color.Black);
                    sprite.End();
                }
                Device.SetRenderTarget(0, original);
            }
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
