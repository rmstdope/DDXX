using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.MeshBuilder;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace TiVi
{
    public class FilmRoller
    {
        private DummyNode film;
        private ModelNode filmNode;
        private ModelNode stillFilmNode;
        private ITexture filmTexture;
        private ITexture filmRenderTarget;
        private ISprite sprite;
        private ITexture filmSquareTexture;
        private ITexture filmPerforationTexture;
        private float rounding = 0.02f;
        private float filmWidth = 0.65f;
        private float squareWidth = 1.37f;
        private float squareHeight = 1.0f;
        private float perforationWidth = 0.09f;
        private float perforationHeight = 0.07f;
        private float perforationDistance = 0.07f;
        private int squareCount = 8;
        private int heightSegments = 5;
        private int textureCount;
        private Vector3[] leftPositions;
        private Vector3[] rightPositions;
        private float curveOffset;
        public const int TextureWidth = 128;
        public const int TextureHeight = 128;
        private int slowDown = 40;
        private int currentFilmTexture;
        private float filmOffset;
        private const float zDelta = 0.01f;
        private IDevice device;
        private ITextureBuilder textureBuilder;
        private IEffectFactory effectFactory;
        private ITextureFactory textureFactory;
        IDemoEffect subEffect;
        private ISpline<InterpolatedVector3> filmSpline;
        private float splineOffsetSpeed = 0.2f;

        public float SplineOffsetSpeed
        {
            get { return splineOffsetSpeed; }
            set { splineOffsetSpeed = value; }
        }

        public FilmRoller(int squareCount, int heightSegments, IDemoEffect subEffect,
            ITextureFactory textureFactory, ITextureBuilder textureBuilder)
        {
            this.squareCount = squareCount;
            this.heightSegments = heightSegments;
            this.subEffect = subEffect;

            CreateDefaultSpline();

            this.textureCount = squareCount + 2;
            filmTexture = textureFactory.CreateRenderTarget(TextureWidth,
                TextureHeight * textureCount, Format.A8R8G8B8);
            filmRenderTarget = textureFactory.CreateRenderTarget(TextureWidth,
                TextureHeight, Format.A8R8G8B8);
            currentFilmTexture = textureCount - 1;
            this.leftPositions = new Vector3[squareCount * heightSegments + 1];
            this.rightPositions = new Vector3[squareCount * heightSegments + 1];
            this.textureBuilder = textureBuilder;
            this.textureFactory = textureFactory;
        }

        public float Height
        {
            get { return squareHeight * squareCount; }
        }

        public KeyFrame<InterpolatedVector3> GetKeyFrame(float time, float x, float y, float z)
        {
            return new KeyFrame<InterpolatedVector3>(time, new InterpolatedVector3(x, y, z));
        }

        private void CreateDefaultSpline()
        {
            this.filmSpline = new NaturalCubicSpline<InterpolatedVector3>();
            this.filmSpline.AddKeyFrame(GetKeyFrame(0, 0, 0, 0));
            this.filmSpline.AddKeyFrame(GetKeyFrame(1.0f, 0, Height, 0));
            this.filmSpline.Calculate();
        }

        private ModelNode CreateSimpleModelNode(IModel model, string effectFileName, string techniquePrefix)
        {
            return BaseDemoEffect.CreateSimpleModelNode(model, effectFileName, techniquePrefix, effectFactory, device);
        }

        public void Initialize(MeshBuilder meshBuilder, IGraphicsFactory graphicsFactory,
            IEffectFactory effectFactory, IDevice device)
        {
            this.device = device;
            this.effectFactory = effectFactory;
            sprite = graphicsFactory.CreateSprite(device);
            CreateFilmSquareTexture();
            CreateFilmPerforationTexture();
            film = new DummyNode("Film roll");
            MeshDirector meshDirector = new MeshDirector(meshBuilder);
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
            stillFilmNode.WorldState.MoveUp(-(squareHeight * squareCount) / 2 - squareHeight / 2);
            stillFilmNode.WorldState.MoveForward(-zDelta);
            film.AddChild(stillFilmNode);
            SetVertices();
            SetFilmTextureCoordinates();
            SetStillFilmTextureCoordinates();

            // TODO: why doesn't this draw the blue film frames and perforations?
            DrawAllFilmTextures();
        }

        public INode FilmNode
        {
            get { return film; }
        }

        public ISpline<InterpolatedVector3> FilmSpline
        {
            get { return filmSpline; }
            set { filmSpline = value; }
        }

        private void SetStillFilmTextureCoordinates()
        {
            int textureIndex = NextFilmTexture(currentFilmTexture);
            IModel model = stillFilmNode.Model;
            using (IVertexBuffer vb = model.Mesh.VertexBuffer)
            {
                CustomVertex.PositionNormalTextured[] verts =
                    (CustomVertex.PositionNormalTextured[])
                    vb.Lock(0, typeof(CustomVertex.PositionNormalTextured), LockFlags.None, model.Mesh.NumberVertices);
                try
                {
                    float yOffset = (float)textureIndex / (float)textureCount;
                    verts[0].Tv = yOffset;
                    verts[1].Tv = yOffset;
                    verts[2].Tv = yOffset + (float)1 / textureCount;
                    verts[3].Tv = yOffset + (float)1 / textureCount;
                }
                finally
                {
                    vb.Unlock();
                }
            }
        }

        private void CalculateCoordinates()
        {
            Vector3 lastUp = new Vector3(0, 1, 0);
            for (int i = 0; i <= squareCount * heightSegments; i++)
            {
                //yValues[i] = i * squareHeight / heightSegments - (squareCount * squareHeight / 2);
                //zValues[i] = (float)(0.5 * Math.Sin(curveOffset + ((float)i / zValues.Length) * Math.PI * 2 * 4));
                float u = i / (float)(squareCount * heightSegments);
                //u += curveOffset;
                Vector3 forward = filmSpline.GetDerivative(u);
                forward.Normalize();
                Vector3 right = Vector3.Cross(lastUp, forward);
                right.Normalize();
                Vector3 up = Vector3.Cross(forward, right);
                up.Normalize();
                lastUp = up;
                Vector3 pos = filmSpline.GetValue(u);
                Vector3 rightPos = pos;
                Vector3 halfRight = right;
                halfRight.Multiply(squareWidth / 2);
                pos -= halfRight;
                //pos.Y *= squareCount * squareHeight;
                leftPositions[i] = pos;
                rightPos += halfRight;
                //rightPos.Y *= squareCount * squareHeight;
                rightPositions[i] = rightPos;
            }
            Vector3 stillPos = stillFilmNode.Position;
            Vector3 newpos = filmSpline.GetValue(1.0f);
            stillPos.Z = newpos.Z;
            stillFilmNode.Position = stillPos;
        }

        private void SetVertices()
        {
            CalculateCoordinates();
            IModel model = filmNode.Model;
            using (IVertexBuffer vb = model.Mesh.VertexBuffer)
            {
                CustomVertex.PositionNormalTextured[] verts =
                    (CustomVertex.PositionNormalTextured[])
                    vb.Lock(0, typeof(CustomVertex.PositionNormalTextured), LockFlags.None, model.Mesh.NumberVertices);
                try
                {
                    int offset = 0;
                    for (int squareIndex = squareCount - 1; squareIndex >= 0; squareIndex--)
                    {
                        int vertexOffset = squareIndex * (heightSegments + 1) * 2;
                        for (int i = heightSegments; i >= 0; i--)
                        {
                            int index = vertexOffset + i * 2;
                            verts[index + 0].X = leftPositions[offset].X;
                            verts[index + 0].Y = leftPositions[offset].Y;
                            verts[index + 0].Z = leftPositions[offset].Z;
                            verts[index + 1].X = rightPositions[offset].X;
                            verts[index + 1].Y = rightPositions[offset].Y;
                            verts[index + 1].Z = rightPositions[offset].Z;
                            // last two y positions same, unless last square
                            if (i > 0 || squareIndex == 0)
                                offset++;
                        }
                    }
                }
                finally
                {
                    vb.Unlock();
                }
            }
        }

        private void MoveFilm()
        {
            filmOffset -= squareHeight / slowDown;
            if (filmOffset < 0)
            {
                filmOffset += squareHeight;
                currentFilmTexture = NextFilmTexture(currentFilmTexture);
                subEffect.Step();
                DrawIntoFilmTexture();
            }
            SetFilmTextureCoordinates();
        }

        private void SetFilmTextureCoordinates()
        {
            int textureIndex = NextFilmTexture(currentFilmTexture);
            textureIndex = NextFilmTexture(textureIndex);
            for (int squareIndex = squareCount - 1; squareIndex >= 0; squareIndex--)
            {
                //squareModelNodes[i].Model.Materials[0].DiffuseTexture =
                //    filmTextures[t];
                SetTextureCoordinates(squareIndex, textureIndex);
                textureIndex = NextFilmTexture(textureIndex);
            }
        }

        private int NextFilmTexture(int index)
        {
            index -= 1;
            if (index < 0)
                index += (textureCount - 1);
            return index;
        }

        private void SetTextureCoordinates(int squareIndex, int textureIndex)
        {
            IModel model = filmNode.Model;
            using (IVertexBuffer vb = model.Mesh.VertexBuffer)
            {
                CustomVertex.PositionNormalTextured[] verts =
                    (CustomVertex.PositionNormalTextured[])
                    vb.Lock(0, typeof(CustomVertex.PositionNormalTextured), LockFlags.None, model.Mesh.NumberVertices);
                try
                {
                    //float yOffset = (filmOffset / squareHeight) * heightSegments + textureIndex * heightSegments;
                    float yOffset = (float)textureIndex / (float)textureCount;
                    int vertexOffset = squareIndex * (heightSegments + 1) * 2;
                    for (int i = 0; i <= heightSegments; i++)
                    {
                        float tv = yOffset + (float)i / (heightSegments * textureCount);
                        tv += filmOffset / (squareHeight * textureCount);
                        verts[vertexOffset + i * 2].Tv = tv;
                        verts[vertexOffset + i * 2 + 1].Tv = tv;
                    }
                }
                finally
                {
                    vb.Unlock();
                }
            }
        }

        public float FilmRounding
        {
            get { return rounding; }
            set { rounding = value; CreateFilmSquareTexture(); }
        }

        public float FilmWidth
        {
            get { return filmWidth; }
            set { filmWidth = value; CreateFilmSquareTexture(); }
        }

        public float PerforationWidth
        {
            get { return perforationWidth; }
            set { perforationWidth = value; CreateFilmPerforationTexture(); }
        }

        public float PerforationHeight
        {
            get { return perforationHeight; }
            set { perforationHeight = value; CreateFilmPerforationTexture(); }
        }

        public float PerforationDistance
        {
            get { return perforationDistance; }
            set { perforationDistance = value; CreateFilmPerforationTexture(); }
        }

        public int SlowDown
        {
            get { return slowDown; }
            set { slowDown = value; }
        }

        private void CreateFilmSquareTexture()
        {
            const int width = TextureWidth;
            const int height = TextureHeight;
            if (device != null)
            {
                float ratio = 1.37f * squareHeight / squareWidth;
                Vector2 size = new Vector2(FilmWidth, FilmWidth / ratio);
                IGenerator rect = new RoundedRectangle(size, new Vector2(0.5f, 0.5f), FilmRounding);
                filmSquareTexture = textureBuilder.Generate(rect, width, height, 1, Format.A8R8G8B8);
            }
        }

        private void CreateFilmPerforationTexture()
        {
            if (device != null)
            {
                int w = (int)(TextureWidth * PerforationWidth / squareWidth);
                int h = (int)(TextureHeight * PerforationHeight / squareHeight);
                filmPerforationTexture = textureFactory.CreateFromFunction(w, h, 1, Usage.None,
                    Format.A8R8G8B8, Pool.Managed,
                    delegate(Vector2 texCoord, Vector2 texelSize)
                    {
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

        private void DrawPerforations()
        {

            float width = filmPerforationTexture.GetLevelDescription(0).Width;
            float height = filmPerforationTexture.GetLevelDescription(0).Height;
            float xPos = TextureWidth * PerforationDistance / squareWidth;
            float yDistance = TextureHeight * squareHeight / 4;
            float yOffset = yDistance / 2 - height / 2;
            sprite.Begin(SpriteFlags.None);
            for (int i = 0; i < 4; i++)
            {
                sprite.Draw2D(filmPerforationTexture, Rectangle.Empty, SizeF.Empty,
                    new PointF(xPos, yOffset + (yDistance * i)),
                    Color.Transparent);
            }
            sprite.End();
            sprite.Begin(SpriteFlags.None);
            xPos = TextureWidth - xPos - width;
            for (int i = 0; i < 4; i++)
            {
                sprite.Draw2D(filmPerforationTexture, Rectangle.Empty, SizeF.Empty,
                    new PointF(xPos, yOffset + (yDistance * i)),
                    Color.Transparent);
            }
            sprite.End();
        }

        private void DrawFilmFrame()
        {
            sprite.Begin(SpriteFlags.AlphaBlend);
            sprite.Draw2D(filmSquareTexture, Rectangle.Empty, SizeF.Empty,
                new PointF(0, 0), Color.Black);
            sprite.End();
        }

        public void Step()
        {
            MoveFilm();
            SetStillFilmTextureCoordinates();
            SetVertices();
            curveOffset += Time.DeltaTime * splineOffsetSpeed;
            if (curveOffset >= FilmSpline.EndTime)
                curveOffset = FilmSpline.StartTime;
        }

        private void DrawIntoFilmTexture()
        {
            using (ISurface original = device.GetRenderTarget(0))
            {
                using (ISurface surface = filmRenderTarget.GetSurfaceLevel(0))
                {
                    device.SetRenderTarget(0, surface);
                    DrawSubEffect();
                    //filmRenderTarget.Save("c:/filmrendertarget1.dds", ImageFileFormat.Dds);
                    DrawFilmFrame();
                    //filmRenderTarget.Save("c:/filmrendertarget2.dds", ImageFileFormat.Dds);
                    DrawPerforations();
                    //filmRenderTarget.Save("c:/filmrendertarget3.dds", ImageFileFormat.Dds);
                    device.SetRenderTarget(0, original);
                    CopySurfaceToFilmTexture(surface, currentFilmTexture);
                    if (currentFilmTexture == 0)
                        CopySurfaceToFilmTexture(surface, textureCount - 1);
                }
            }
            //filmRenderTarget.Save("c:/filmrendertarget.dds", ImageFileFormat.Dds);
        }

        private void DrawAllFilmTextures()
        {
            using (ISurface original = device.GetRenderTarget(0))
            {
                using (ISurface surface = filmRenderTarget.GetSurfaceLevel(0))
                {
                    device.SetRenderTarget(0, surface);
                    for (int textureIndex = 0; textureIndex < textureCount-1; textureIndex++)
                    {
                        subEffect.Step();
                        DrawSubEffect();
                        //filmRenderTarget.Save("c:/filmrendertarget1.dds", ImageFileFormat.Dds);
                        DrawFilmFrame();
                        //filmRenderTarget.Save("c:/filmrendertarget2.dds", ImageFileFormat.Dds);
                        DrawPerforations();
                        //filmRenderTarget.Save("c:/filmrendertarget3.dds", ImageFileFormat.Dds);
                        CopySurfaceToFilmTexture(surface, textureIndex);
                        if (textureIndex == 0)
                            CopySurfaceToFilmTexture(surface, textureCount - 1);
                    }
                    device.SetRenderTarget(0, original);
                }
            }
            //filmRenderTarget.Save("c:/filmrendertarget.dds", ImageFileFormat.Dds);
        }

        private void DrawEmptyFilmTextures()
        {
            using (ISurface original = device.GetRenderTarget(0))
            {
                using (ISurface surface = filmRenderTarget.GetSurfaceLevel(0))
                {
                    device.SetRenderTarget(0, surface);
                    for (int textureIndex = 0; textureIndex < textureCount; textureIndex++)
                    {
                        DrawBlackScreen();
                        DrawFilmFrame();
                        DrawPerforations();
                        CopySurfaceToFilmTexture(surface, textureIndex);
                    }
                    device.SetRenderTarget(0, original);
                }
            }
        }

        private void DrawSubEffect()
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.FromArgb(0xff, Color.Black), 1.0f, 0);
            device.BeginScene();
            subEffect.Render();
            device.EndScene();
        }

        private void DrawBlackScreen()
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.FromArgb(0xff, Color.Black), 1.0f, 0);
            //device.BeginScene();
            //device.EndScene();
        }


        private void CopySurfaceToFilmTexture(ISurface surface, int textureIndex)
        {
            Rectangle destRectangle = new Rectangle(0, textureIndex * TextureHeight,
                TextureWidth, TextureHeight);
            using (ISurface destSurface = filmTexture.GetSurfaceLevel(0))
            {
                device.StretchRectangle(surface, new Rectangle(0, 0, TextureWidth, TextureHeight),
                    destSurface, destRectangle, TextureFilter.None);
            }
            //filmTexture.Save("c:/filmtexture.dds", ImageFileFormat.Dds);
        }
    }
}
