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

namespace EngineTest {
    public class FilmRoll : BaseDemoEffect {
        private DummyNode film;
        private CameraNode camera;
        private ModelNode filmNode;
        private ModelNode stillFilmNode;
        private IScene scene;
        private ITexture filmTexture;
        private ITexture filmRenderTarget;
        private IDemoEffect subEffect;
        private ISprite sprite;
        private ITexture filmSquareTexture;
        private ITexture filmPerforationTexture;
        private float rounding = 0.05f;
        private float filmWidth = 0.6f;
        private float squareWidth = 1.37f;
        private float squareHeight = 1.0f;
        private float perforationWidth = 0.05f;
        private float perforationHeight = 0.03f;
        private float perforationDistance = 0.03f;
        private const int squareCount = 8;
        private const int textureCount = squareCount + 2;
        private const int heightSegments = 5;
        float[] yValues = new float[squareCount * heightSegments + 1];
        float[] zValues = new float[squareCount * heightSegments + 1];
        private float filmOffset;
        private const float zDelta = 0.01f;
        private int currentFilmTexture;
        private int slowDown = 8;
        private float curveOffset;
        const int textureWidth = 128;
        const int textureHeight = 128;
        IAnimationRootFrame animationRootFrame;
        List<INode> animationNodes;

        public FilmRoll(string name, float start, float end)
            : base(name, start, end) {
            filmTexture = TextureFactory.CreateRenderTarget(textureWidth,
                textureHeight * textureCount, Format.A8R8G8B8);
            filmRenderTarget = TextureFactory.CreateRenderTarget(textureWidth,
                textureHeight, Format.A8R8G8B8);
            currentFilmTexture = textureCount - 1;
            SetStepSize(GetTweakableNumber("FilmWidth"), 0.01f);
            SetStepSize(GetTweakableNumber("FilmRounding"), 0.01f);
            SetStepSize(GetTweakableNumber("PerforationWidth"), 0.01f);
            SetStepSize(GetTweakableNumber("PerforationHeight"), 0.01f);
            SetStepSize(GetTweakableNumber("PerforationDistance"), 0.01f);
        }

        protected override void Initialize() {
            InitializeSubEffects();
            sprite = GraphicsFactory.CreateSprite(Device);
            CreateFilmSquareTexture();
            CreateFilmPerforationTexture();

            XLoader.Load("filmanimation.X", EffectFactory.CreateFromFile("TiVi.fxo"),
    delegate(string name)
    {
                return TechniqueChooser.MaterialPrefix("FilmRoll");
    });
            animationRootFrame = XLoader.RootFrame;
            animationNodes = XLoader.GetNodeHierarchy();
            
            CreateStandardSceneAndCamera(out scene, out camera, 15);
            film = new DummyNode("Film roll");

            //MeshBuilder.SetDiffuseTexture("Default1", "FLOWER6P.jpg");

            MeshDirector meshDirector = new MeshDirector(MeshBuilder);
            meshDirector.CreatePlane(squareWidth, squareCount * squareHeight, 1,
                squareCount * (heightSegments + 1) - 1, true);
            IModel filmModel = meshDirector.Generate("Default1");
            filmModel.Materials[0].AmbientColor = ColorValue.FromColor(Color.White);
            filmModel.Materials[0].DiffuseTexture = filmTexture;
            filmNode = CreateSimpleModelNode(filmModel, "TiVi.fxo", "FilmRoll");
            film.AddChild(filmNode);

            meshDirector.CreatePlane(squareWidth, squareHeight, 1, 1, true);
            IModel stillFilmModel = meshDirector.Generate("Default1");
            stillFilmModel.Materials[0].AmbientColor = ColorValue.FromColor(Color.White);
            stillFilmModel.Materials[0].DiffuseTexture = filmTexture;
            stillFilmNode = CreateSimpleModelNode(stillFilmModel, "TiVi.fxo", "FilmRoll");
            stillFilmNode.WorldState.MoveUp(-(squareHeight*squareCount)/2-squareHeight/2);
            stillFilmNode.WorldState.MoveForward(-zDelta);
            film.AddChild(stillFilmNode);

            film.WorldState.Turn(-(float)(-Math.PI / 4));
            film.WorldState.Tilt(-(float)(-Math.PI / 3));
            //film.WorldState.MoveUp(-5);
            //camera.WorldState.Tilt(-(float)(Math.PI / 4));
            scene.AddNode(film);

            SetVertices();
            SetFilmTextureCoordinates();
            SetStillFilmTextureCoordinates();

            scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            //Mixer.ClearColor = Color.Blue;

            // TODO: why doesn't this draw the blue film frames and perforations?
            DrawEmptyFilmTextures();
        }

        private void SetStillFilmTextureCoordinates() {
            int textureIndex = NextFilmTexture(currentFilmTexture);
            IModel model = stillFilmNode.Model;
            using (IVertexBuffer vb = model.Mesh.VertexBuffer) {
                CustomVertex.PositionNormalTextured[] verts =
                    (CustomVertex.PositionNormalTextured[])
                    vb.Lock(0, typeof(CustomVertex.PositionNormalTextured), LockFlags.None, model.Mesh.NumberVertices);
                try {
                    float yOffset = (float)textureIndex / (float)textureCount;
                    verts[0].Tv = yOffset;
                    verts[1].Tv = yOffset;
                    verts[2].Tv = yOffset + (float)1 / textureCount;
                    verts[3].Tv = yOffset + (float)1 / textureCount;
                } finally {
                    vb.Unlock();
                }
            }
        }

        private void CalculateCoordinates() {
            for (int i = 0; i <= squareCount * heightSegments; i++) {
                //yValues[i] = i * squareHeight / heightSegments - (squareCount * squareHeight / 2);
                //zValues[i] = (float)(0.5 * Math.Sin(curveOffset + ((float)i / zValues.Length) * Math.PI * 2 * 4));
            }
        }

        private void SetVertices() {
            CalculateCoordinates();
            IModel model = filmNode.Model;
            using (IVertexBuffer vb = model.Mesh.VertexBuffer) {
                CustomVertex.PositionNormalTextured[] verts =
                    (CustomVertex.PositionNormalTextured[])
                    vb.Lock(0, typeof(CustomVertex.PositionNormalTextured), LockFlags.None, model.Mesh.NumberVertices);
                try {
                    int offset = 0;
                    for (int squareIndex = squareCount-1; squareIndex >= 0; squareIndex--) {
                        int vertexOffset = squareIndex * (heightSegments + 1) * 2;
                        for (int i = heightSegments; i >= 0; i--) {
                            float y = yValues[offset];
                            float z = zValues[offset];
                            verts[vertexOffset + i * 2 + 0].Y = y;
                            verts[vertexOffset + i * 2 + 0].Z = z;
                            verts[vertexOffset + i * 2 + 1].Y = y;
                            verts[vertexOffset + i * 2 + 1].Z = z;
                            // last two y positions same, unless last square
                            if (i > 0 || squareIndex == 0)
                                offset++;
                        }
                    }
                } finally {
                    vb.Unlock();
                }
            }
        }

        private void MoveFilm() {
            filmOffset -= squareHeight / slowDown;
            if (filmOffset < 0) {
                filmOffset += squareHeight;
                currentFilmTexture = NextFilmTexture(currentFilmTexture);
                StepSubEffect();
            }
            SetFilmTextureCoordinates();
        }

        private void SetFilmTextureCoordinates() {
            int textureIndex = NextFilmTexture(currentFilmTexture);
            textureIndex = NextFilmTexture(textureIndex);
            for (int squareIndex = squareCount - 1; squareIndex >= 0; squareIndex--) {
                //squareModelNodes[i].Model.Materials[0].DiffuseTexture =
                //    filmTextures[t];
                SetTextureCoordinates(squareIndex, textureIndex);
                textureIndex = NextFilmTexture(textureIndex);
            }
        }

        private int NextFilmTexture(int index) {
            index -= 1;
            if (index < 0)
                index += (textureCount-1);
            return index;
        }

        private void SetTextureCoordinates(int squareIndex, int textureIndex) {
            IModel model = filmNode.Model;
            using (IVertexBuffer vb = model.Mesh.VertexBuffer) {
                CustomVertex.PositionNormalTextured[] verts =
                    (CustomVertex.PositionNormalTextured[])
                    vb.Lock(0, typeof(CustomVertex.PositionNormalTextured), LockFlags.None, model.Mesh.NumberVertices);
                try {
                    //float yOffset = (filmOffset / squareHeight) * heightSegments + textureIndex * heightSegments;
                    float yOffset = (float)textureIndex / (float)textureCount;
                    int vertexOffset = squareIndex * (heightSegments + 1) * 2;
                    for (int i = 0; i <= heightSegments; i++) {
                        float tv = yOffset + (float)i / (heightSegments*textureCount);
                        //tv += filmOffset / (squareHeight*textureCount);
                        verts[vertexOffset + i * 2].Tv = tv;
                        verts[vertexOffset + i * 2 + 1].Tv = tv;
                    }
                } finally {
                    vb.Unlock();
                }
            }
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
            const int width = textureWidth;
            const int height = textureHeight;
            if (Device != null) {
                float ratio = 1.37f * squareHeight / squareWidth;
                Vector2 size = new Vector2(FilmWidth, FilmWidth / ratio);
                IGenerator rect = new RoundedRectangle(size, new Vector2(0.5f, 0.5f), FilmRounding);
                filmSquareTexture = TextureBuilder.Generate(rect, width, height, 1, Format.A8R8G8B8);
            }
        }

        private void CreateFilmPerforationTexture() {
            if (Device != null) {
                int w = (int)(textureWidth * PerforationWidth / squareWidth);
                int h = (int)(textureHeight * PerforationHeight / squareHeight);
                filmPerforationTexture = TextureFactory.CreateFromFunction(w, h, 1, Usage.None,
                    Format.A8R8G8B8, Pool.Managed,
                    delegate(Vector2 texCoord, Vector2 texelSize) {
                        return new Vector4(1, 1, 1, 0);
                    });
            }
        }

        //private void CreateFadeTexture() {
        //    const int w = 256;
        //    const int h = 256;
        //    if (Device != null) {
        //        float ratio = 1.37f * squareHeight / squareWidth;
        //        Vector2 size = new Vector2(FilmWidth, FilmWidth / ratio);
        //        fadeTexture = TextureFactory.CreateFromFunction(w, h, 1, Usage.None,
        //            Format.A8R8G8B8, Pool.Managed,
        //            delegate(Vector2 texCoord, Vector2 texelSize) {
        //                return new Vector4(0, 0, 0, 0);
        //            });
        //    }
        //}

        private void DrawPerforations() {

            float width = filmPerforationTexture.GetLevelDescription(0).Width;
            float height = filmPerforationTexture.GetLevelDescription(0).Height;
            float xPos = textureWidth * PerforationDistance / squareWidth;
            float yDistance = textureHeight * squareHeight / 4;
            float yOffset = yDistance / 2 - height / 2;
            sprite.Begin(SpriteFlags.None);
            for (int i = 0; i < 4; i++) {
                sprite.Draw2D(filmPerforationTexture, Rectangle.Empty, SizeF.Empty,
                    new PointF(xPos, yOffset + (yDistance * i)),
                    Color.Transparent);
            }
            sprite.End();
            sprite.Begin(SpriteFlags.None);
            xPos = textureWidth - xPos - width;
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
            MoveFilm();
            SetStillFilmTextureCoordinates();
            SetVertices();
            scene.Step();
            StepAnimation();
        }

        private void StepAnimation()
        {
            StepAnimation2();
            UpdateFrameMatrices(animationRootFrame.FrameHierarchy, Matrix.Identity);

            //List<Matrix> matrices = new List<Matrix>();
            //IFrame frame = animationRootFrame.FrameHierarchy;
            //matrices.Add(frame.TransformationMatrix);
            //while (frame.FrameFirstChild != null)
            //{
            //    frame = frame.FrameFirstChild;
            //    matrices.Add(frame.TransformationMatrix);
            //}
            //for (int i = 0; i <= squareCount * heightSegments; i++) {
            //    //yValues[i] = i * squareHeight / heightSegments - (squareCount * squareHeight / 2);
            //    int s = i / heightSegments;
            //    if (s < matrices.Count) {
            //        zValues[i] = (matrices[s/heightSegments].M23-0.7f) * 10.0f;
            //    }
            //}
            List<Vector3> positions = new List<Vector3>();
            List<string> names = new List<string>();
            INode node = animationNodes[0].Children[3];
            if (node != null)
            {
                positions.Add(node.Position);
                names.Add(node.Name);
                while (node.Children.Count >= 1)
                {
                    positions.Add(node.Children[0].Position);
                    names.Add(node.Children[0].Name);
                    node = node.Children[0];
                }
            }
            for (int i = 0; i < squareCount; i++)
            {
                for (int j = 0; j <= heightSegments; j++)
                {
                    //yValues[i] = i * squareHeight / heightSegments - (squareCount * squareHeight / 2);
                    Vector3 pos = new Vector3();
                    if (i < positions.Count)
                    {
                        pos = positions[i];
                    }
                    yValues[i * heightSegments + j] = (pos.Z * 2.0f*squareHeight + j * squareHeight / heightSegments);// -(squareCount * squareHeight / 2);
                    zValues[i * heightSegments + j] = -(pos.Y*squareHeight);
                }
            }
        }

        private void UpdateFrameMatrices(IFrame frame, Matrix parentMatrix)
        {
            frame.CombinedTransformationMatrix = frame.TransformationMatrix * parentMatrix;

            if (frame.FrameSibling != null)
            {
                UpdateFrameMatrices(frame.FrameSibling, parentMatrix);
            }

            if (frame.FrameFirstChild != null)
            {
                UpdateFrameMatrices(frame.FrameFirstChild, frame.CombinedTransformationMatrix);
            }
        }

        private void StepAnimation2()
        {
            IAnimationController controller = animationRootFrame.AnimationController;
            float animationStartTime = 0;
            float animationSpeed = 7;
            if (controller != null)
            {
                IAnimationSet animationSet = animationRootFrame.AnimationController.GetAnimationSet(0);
                double rewindTime = animationSet.Period - (controller.Time % animationSet.Period);
                if (rewindTime < 0)
                    rewindTime += animationSet.Period;
                double forwardTime = (Time.StepTime - animationStartTime) * animationSpeed;
                int num = (int)(forwardTime / animationSet.Period);
                forwardTime = forwardTime % animationSet.Period;
                double time = rewindTime + forwardTime;
                if (time < 0)
                    time += animationSet.Period;
                controller.AdvanceTime(time);
            }
        }

        public int SlowDown {
            get { return slowDown; }
            set { slowDown = value; }
        }

        private void StepSubEffect() {
            subEffect.Step();
            DrawIntoFilmTexture();
        }

        private void DrawIntoFilmTexture() {
            using (ISurface original = Device.GetRenderTarget(0)) {
                using (ISurface surface = filmRenderTarget.GetSurfaceLevel(0)) {
                    Device.SetRenderTarget(0, surface);
                    DrawSubEffect();
                    DrawFilmFrame();
                    DrawPerforations();
                    Device.SetRenderTarget(0, original);
                    CopySurfaceToFilmTexture(surface, currentFilmTexture);
                    if (currentFilmTexture == 0)
                        CopySurfaceToFilmTexture(surface, textureCount - 1);
                }
            }
        }

        private void DrawEmptyFilmTextures() {
            using (ISurface original = Device.GetRenderTarget(0)) {
                using (ISurface surface = filmRenderTarget.GetSurfaceLevel(0)) {
                    Device.SetRenderTarget(0, surface);
                    for (int textureIndex = 0; textureIndex < textureCount; textureIndex++) {
                        DrawBlackScreen();
                        DrawFilmFrame();
                        DrawPerforations();
                        CopySurfaceToFilmTexture(surface, textureIndex);
                    }
                    Device.SetRenderTarget(0, original);
                }
            }
        }

        private void DrawSubEffect() {
            Device.BeginScene();
            Device.Clear(ClearFlags.Target, Color.FromArgb(0xff, Color.Black), 0, 0);
            subEffect.Render();
            Device.EndScene();
        }

        private void DrawBlackScreen() {
            Device.BeginScene();
            Device.Clear(ClearFlags.Target, Color.FromArgb(0xff, Color.Black), 0, 0);
            Device.EndScene();
        }


        private void CopySurfaceToFilmTexture(ISurface surface, int textureIndex) {
            Rectangle destRectangle = new Rectangle(0, textureIndex * textureHeight,
                textureWidth, textureHeight);
            using (ISurface destSurface = filmTexture.GetSurfaceLevel(0)) {
                Device.StretchRectangle(surface, new Rectangle(0, 0, textureWidth, textureHeight),
                    destSurface, destRectangle, TextureFilter.None);
            }
        }

        public override void Render() {
            scene.Render();
        }
    }
}
