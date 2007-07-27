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
        private ITexture filmPerforationTexture;
        private float rounding = 0.05f;
        private float filmWidth = 0.6f;
        private float squareWidth = 1.37f;
        private float squareHeight = 1.0f;
        private int squareCount = 10;
        private float perforationWidth = 0.05f;
        private float perforationHeight = 0.03f;
        private float perforationDistance = 0.03f;

        public FilmRoll(string name, float start, float end)
            : base(name, start, end)
        {
            filmTexture = TextureFactory.CreateRenderTarget(256, 256, Format.A8R8G8B8);
            SetStepSize(GetTweakableNumber("FilmWidth"), 0.01f);
            SetStepSize(GetTweakableNumber("FilmRounding"), 0.01f);
            SetStepSize(GetTweakableNumber("PerforationWidth"), 0.01f);
            SetStepSize(GetTweakableNumber("PerforationHeight"), 0.01f);
            SetStepSize(GetTweakableNumber("PerforationDistance"), 0.01f);
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
                ModelNode plane = CreateSimpleModelNode(model.Clone(), "TiVi.fxo", "SimpleAlphaBlend");
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
            CreateFilmPerforationTexture();
        }
        public float FilmRounding {
            get { return rounding; }
            set { rounding = value; CreateFilmSquareTexture(); }
        }

        public float FilmWidth {
            get { return filmWidth; }
            set { filmWidth = value; CreateFilmSquareTexture(); }
        }

        public float PerforationWidth {
            get { return perforationWidth; }
            set { perforationWidth = value; CreateFilmPerforationTexture(); }
        }

        public float PerforationHeight {
            get { return perforationHeight; }
            set { perforationHeight = value; CreateFilmPerforationTexture(); }
        }

        public float PerforationDistance {
            get { return perforationDistance; }
            set { perforationDistance = value; CreateFilmPerforationTexture(); }
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

        private void CreateFilmPerforationTexture() {
            if (Device != null) {
                int w = (int)(256 * PerforationWidth / squareWidth);
                int h = (int)(256 * PerforationHeight / squareHeight);
                filmPerforationTexture = TextureFactory.CreateFromFunction(w, h, 1, Usage.None,
                    Format.A8R8G8B8, Pool.Managed,
                    delegate(Vector2 texCoord, Vector2 texelSize) {
                        return new Vector4(1, 1, 1, 0);
                    });
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
                    DrawSubEffect(surface);
                    //PostProcessor.StartFrame(filmTexture);
                    //subPostEffect.Render();
                    filmTexture.Save("c:/film1.dds", ImageFileFormat.Dds);
                    DrawFilmFrame();
                    DrawPerforations();
                    filmTexture.Save("c:/film3.dds", ImageFileFormat.Dds);
                }
                Device.SetRenderTarget(0, original);
            }
        }

        private void DrawSubEffect(ISurface surface) {
            Device.SetRenderTarget(0, surface);
            Device.BeginScene();
            Device.Clear(ClearFlags.Target, Color.FromArgb(0xff, Color.Black), 0, 0);
            subEffect.Render();
            Device.EndScene();
        }

        private void DrawPerforations() {

            float width = filmPerforationTexture.GetLevelDescription(0).Width;
            float height = filmPerforationTexture.GetLevelDescription(0).Height;
            float xPos = 256 * PerforationDistance / squareWidth;
            float yDistance = 256 * squareHeight / 4;
            float yOffset = yDistance / 2 - height / 2;
            sprite.Begin(SpriteFlags.None);
            for (int i = 0; i < 4; i++) {
                sprite.Draw2D(filmPerforationTexture, Rectangle.Empty, SizeF.Empty,
                    new PointF(xPos, yOffset + (yDistance * i)),
                    Color.Transparent);
            }
            sprite.End();
            sprite.Begin(SpriteFlags.None);
            xPos = 256 - xPos - width;
            for (int i = 0; i < 4; i++) {
                sprite.Draw2D(filmPerforationTexture, Rectangle.Empty, SizeF.Empty,
                    new PointF(xPos, yOffset + (yDistance * i)),
                    Color.Transparent);
            }
            sprite.End();
        }

        private void DrawFilmFrame() {
            sprite.Begin(SpriteFlags.AlphaBlend);
            sprite.Draw2D(filmSquareTexture, Rectangle.Empty, SizeF.Empty,
                new PointF(0, 0), Color.Black);
            sprite.End();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
