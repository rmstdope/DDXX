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
        private DummyNode filmWithCover;
        private float xAngle;
        private float yAngle;
        private float zAngle;
        private CameraNode camera;
        private IScene scene;
        private ITexture[] filmTextures;
        private IDemoEffect subEffect;
        private ISprite sprite;
        private ITexture filmSquareTexture;
        private ITexture filmPerforationTexture;
        private ITexture fadeTexture;
        private float rounding = 0.05f;
        private float filmWidth = 0.6f;
        private float squareWidth = 1.37f;
        private float squareHeight = 1.0f;
        private float perforationWidth = 0.05f;
        private float perforationHeight = 0.03f;
        private float perforationDistance = 0.03f;
        private ModelNode[] squareModelNodes;
        private const int squareCount = 10;
        private const int heightSegments = 5;
        float[] zValues = new float[squareCount * heightSegments + 1];
        private float filmOffset;
        private const float zDelta = 0.01f;
        private int currentFilmTexture;
        IModel coverModel1;
        IModel coverModel2;
        private int slowDown = 8;
        private float curveOffset;

        public FilmRoll(string name, float start, float end)
            : base(name, start, end)
        {
            filmTextures = new ITexture[squareCount];
            for (int i = 0; i < squareCount; i++) {
                filmTextures[i] = TextureFactory.CreateRenderTarget(256, 256, Format.A8R8G8B8);
            }
            currentFilmTexture = squareCount - 1;
            SetStepSize(GetTweakableNumber("FilmWidth"), 0.01f);
            SetStepSize(GetTweakableNumber("FilmRounding"), 0.01f);
            SetStepSize(GetTweakableNumber("PerforationWidth"), 0.01f);
            SetStepSize(GetTweakableNumber("PerforationHeight"), 0.01f);
            SetStepSize(GetTweakableNumber("PerforationDistance"), 0.01f);
        }

        protected override void Initialize()
        {
            InitializeSubEffects();
            sprite = GraphicsFactory.CreateSprite(Device);
            CreateFilmSquareTexture();
            CreateFilmPerforationTexture();
            CreateFadeTexture();

            CreateStandardSceneAndCamera(out scene, out camera, 12);
            film = new DummyNode("Film roll");
            filmWithCover = new DummyNode("Film with cover");

            squareModelNodes = new ModelNode[squareCount];

            //MeshBuilder.SetDiffuseTexture("Default1", "FLOWER6P.jpg");
     
            MeshDirector meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreatePlane(squareWidth, squareHeight, 1, heightSegments, true);
            for (int i = 0; i < squareCount; i++) {
                IModel model = meshDirector.Generate("Default1");
                model.Materials[0].AmbientColor = ColorValue.FromColor(Color.White);
                model.Materials[0].DiffuseTexture = filmTextures[i];
                ModelNode plane = CreateSimpleModelNode(model, "TiVi.fxo", "SimpleAlphaBlend");
                plane.WorldState.MoveUp(i * squareHeight - (squareCount / 2) * squareHeight);
                film.AddChild(plane);
                squareModelNodes[i] = plane;
            }
            coverModel1 = meshDirector.Generate("Default1");
            coverModel2 = meshDirector.Generate("Default1");
            coverModel1.Materials[0].AmbientColor = ColorValue.FromColor(Color.White);
            coverModel1.Materials[0].DiffuseTexture = fadeTexture;
            coverModel2.Materials[0].AmbientColor = ColorValue.FromColor(Color.White);
            coverModel2.Materials[0].DiffuseTexture = fadeTexture;
            ModelNode cover1 = CreateSimpleModelNode(coverModel1, "TiVi.fxo", "SimpleAlphaBlend");
            ModelNode cover2 = CreateSimpleModelNode(coverModel2, "TiVi.fxo", "SimpleAlphaBlend");
            Vector3 coverPos = cover1.WorldState.Position;
            coverPos.X -= 0.5f;
            coverPos.Y = squareCount * squareHeight - (squareCount / 2) * squareHeight;
            coverPos.Z -= zDelta;
            cover1.WorldState.Position = coverPos;
            cover1.WorldState.Scale(2.0f);
            coverPos = cover2.WorldState.Position;
            //coverPos.X -= 0.5f;
            coverPos.Y = 0 * squareHeight - (squareCount / 2) * squareHeight;
            coverPos.Z -= zDelta;
            cover2.WorldState.Position = coverPos;
            //cover2.WorldState.Scale(2.0f);
            //film.WorldState.Turn(-(float)(Math.PI / 3));
            //film.WorldState.MoveUp(-5);
            //camera.WorldState.Tilt(-(float)(Math.PI / 4));
            filmWithCover.AddChild(cover1);
            filmWithCover.AddChild(cover2);
            filmWithCover.AddChild(film);

            filmWithCover.WorldState.Turn((float)(Math.PI / 4));
            filmWithCover.WorldState.Tilt((float)(Math.PI / 4));
            filmWithCover.WorldState.MoveRight(2);
            filmWithCover.WorldState.MoveUp(2);
            scene.AddNode(filmWithCover);

            //SetVertices();
            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            //Mixer.ClearColor = Color.Blue;
        }

        private void SetVertices() {
            for (int i = 0; i < zValues.Length; i++) {
                zValues[i] = (float)(Math.Sin(curveOffset + ((float)i / zValues.Length) * Math.PI * 2 * 4));
            }
            int offset = 0;
            for (int k = 0; k < squareCount; k++) {
                IModel model = squareModelNodes[k].Model;
                offset = SetModelVertices(model, offset);
            }
        }

        private void MoveFilm() {
            filmOffset -= squareHeight / slowDown;
            if (filmOffset < 0) {
                filmOffset += squareHeight;
                StepSubEffect();
                MoveFilmTextures();
            }
            Vector3 pos = film.WorldState.Position;
            pos.Y = filmOffset;
            film.WorldState.Position = pos;
        }

        private void MoveFilmTextures() {
            int t = currentFilmTexture;
            for (int i = squareCount - 1; i >= 0; i--) {
                squareModelNodes[i].Model.Materials[0].DiffuseTexture =
                    filmTextures[t];
                t -= 1;
                if (t < 0)
                    t += squareCount;
            }
            coverModel2.Materials[0].DiffuseTexture = 
                squareModelNodes[0].Model.Materials[0].DiffuseTexture;
            currentFilmTexture += 1;
            if (currentFilmTexture >= squareCount)
                currentFilmTexture -= squareCount;
        }

        private int SetModelVertices(IModel model, int offset) {
            using (IVertexBuffer vb = model.Mesh.VertexBuffer) {
                CustomVertex.PositionNormalTextured[] verts =
                    (CustomVertex.PositionNormalTextured[])
                    vb.Lock(0, typeof(CustomVertex.PositionNormalTextured), LockFlags.None, model.Mesh.NumberVertices);
                try {
                    for (int i = verts.Length-2; i >= 0; i -= 2) {
                        float z = zValues[offset];
                        verts[i + 0].Z = z;
                        verts[i + 1].Z = z;
                        if (i > 0)
                            offset++;
                    }
                } finally {
                    vb.Unlock();
                }
            }
            return offset;
        }

        private void InitializeSubEffects() {
            subEffect = new DiscoFever("film effect", StartTime, EndTime);
            subEffect.Initialize(GraphicsFactory, EffectFactory, Device, Mixer, PostProcessor);
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

        private void CreateFadeTexture() {
            const int w = 256;
            const int h = 256;
            if (Device != null) {
                float ratio = 1.37f * squareHeight / squareWidth;
                Vector2 size = new Vector2(FilmWidth, FilmWidth / ratio);
                fadeTexture = TextureFactory.CreateFromFunction(w, h, 1, Usage.None,
                    Format.A8R8G8B8, Pool.Managed,
                    delegate(Vector2 texCoord, Vector2 texelSize) {
                        return new Vector4(0,0,0,0);
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
                new PointF(0, 0), Color.Blue);
            sprite.End();
        }

        public override void Step() {
            //film.WorldState.ResetRotation();
            //film.WorldState.Roll(ZAngle);
            //film.WorldState.Turn(YAngle);
            //film.WorldState.Tilt(XAngle);
            MoveFilm();
            //SetVertices();
            scene.Step();
        }

        public int SlowDown {
            get { return slowDown; }
            set { slowDown = value; }
        }

        private void StepSubEffect() {
            subEffect.Step();
            using (ISurface original = Device.GetRenderTarget(0)) {
                using (ISurface surface = filmTextures[currentFilmTexture].GetSurfaceLevel(0)) {
                    DrawSubEffect(surface);
                    //PostProcessor.StartFrame(filmTexture);
                    //subPostEffect.Render();
                    DrawFilmFrame();
                    DrawPerforations();
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
