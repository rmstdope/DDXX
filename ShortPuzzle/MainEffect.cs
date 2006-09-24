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

namespace ShortPuzzle
{
    class MainEffect : BaseDemoEffect
    {
        private Cube[] cubes;
        private LightNode[] lights;
        private ISprite sprite;
        private ITexture texture;
        private ITexture dopeTexture;
        //private ICubeTexture cubeTexture;
        //private ICubeTexture lightProbe;
        private const int CubesInRow = 16;
        //private float angle = 0;
        private CameraNode camera;
        private const string PREFIX = "";//"../../Data/";
        private const string EPREFIX = "";//"../../../Effects/";

        private class Cube
        {
            public MeshNode[] meshes = new MeshNode[2];
            public Vector3 startPosition;
            public Vector3 endPosition;
            public float moveTime;
            public float timeOffset;
            static Random rand = new Random();
            float yAmplitude;
            float xAmplitude;
            int x;
            int y;
            private const int CubesInRow = 16;

            static string[] letters = new string[] 
            {
                " XYZW           ",
                " adgj           ",
                " behk     D     ",
                " cfil     O     ",
                "          P     ",
                "        O E     ",
                "   T A  R       ",
                "   H N  B B  T  ",
                "DOPE DOMINATION ",
                " FUCK OUTBREAK  ",
                "  S     E  N B  ",
                "  S     R  D R  ",
                "           I A  ",
                " 01 V98    N    ",
                " 23 6-7    G    ",
                " 45 JQL         "
            };
            // 89:;<=>?@AB
            static string[] letterGroups = new string[] 
            {
                " 4444           ",
                " 4444           ",
                " 4444     1     ",
                " 4444     1     ",
                "          1     ",
                "        8 1     ",
                "   6 7  8       ",
                "   6 7  8 2  3  ",
                "??56 7??8?22?3? ",
                " @56@ @@8@22@3  ",
                "  5     8  2 3  ",
                "  5     8  2 3  ",
                "           2 3  ",
                " 9: BCD    2    ",
                " ;< AHI    2    ",
                " => EFG         "
            };
            public static float[] times = new float[] 
            {
                5, 5.5f, 6.5f, // text
                12, // stugan
                18, 19, 19.3f, 19.6f, // text
                24.9f, 25.4f, 25.9f, 26.4f, 26.9f, 27.4f, // creds 
                32, 33, // text
                39, 41, 41.5f, 42, 43, 43.5f, 44, 45, 48, // greetz
                55 // text
            };

            public Cube(int x, int y, MeshNode mesh1, MeshNode mesh2, Vector3 startPos, Vector3 endPos)
            {
                meshes[0] = mesh1;
                meshes[1] = mesh2;
                if (letters[CubesInRow - y - 1][x] != ' ')
                {
                    Material material = new Material();
                    material.AmbientColor = new ColorValue(0.8f, 0.8f, 0.8f);
                    meshes[1].Model.Materials = new ModelMaterial[1];
                    if (letters[CubesInRow - y - 1][x] >= 'a' && letters[CubesInRow - y - 1][x] <= 'z')
                        meshes[1].Model.Materials[0] = new ModelMaterial(material, D3DDriver.TextureFactory.CreateFromFile(PREFIX + letters[CubesInRow - y - 1][x] + ".JPG"));
                    else
                       meshes[1].Model.Materials[0] = new ModelMaterial(material, D3DDriver.TextureFactory.CreateFromFile(PREFIX + letters[CubesInRow - y - 1][x] + ".PNG"));
                }
                endPosition = endPos;
                startPosition = startPos;
                timeOffset = 0.5f * x + 0.1f * y;// 2.0f * (float)Math.PI * (float)rand.NextDouble();
                yAmplitude = 1.0f;// (float)rand.NextDouble();
                xAmplitude = 1.5f;//(float)rand.NextDouble() * 0.5f;
                moveTime = 4;
                this.x = x;
                this.y = y;
            }

            public void AddToScene(Scene scene)
            {
                scene.AddNode(meshes[0]);
                scene.AddNode(meshes[1]);
            }

            public void Step()
            {
                float delta = moveTime - Time.StepTime;
                if (delta < 0.0f)
                {
                    delta = 0.0f;
                }
                delta /= moveTime;
                Vector3 pos = endPosition + (startPosition - endPosition) * delta;

                meshes[0].WorldState.Reset();
                meshes[1].WorldState.Reset();

                meshes[0].WorldState.Position = pos;
                meshes[1].WorldState.Position = pos;

                DoRandomMove();

                DoTurning();
            }

            private void DoTurning()
            {
                if (letters[CubesInRow - y - 1][x] == ' ')
                    return;
                int group = letterGroups[CubesInRow - y - 1][x] - '1';
                float startRotate = times[group];
                float endRotate = startRotate + 0.5f;

                float delta = (float)Math.PI;
                if (Time.StepTime > startRotate)
                {
                    delta = endRotate - Time.StepTime;
                    if (delta < 0.0f)
                        delta = 0.0f;
                    delta /= (endRotate - startRotate);
                    delta = delta * (float)Math.PI;
                }
                meshes[0].WorldState.Roll(-(float)Math.PI / 2);
                meshes[1].WorldState.Roll(-(float)Math.PI / 2);
                meshes[0].WorldState.Turn(delta);
                meshes[1].WorldState.Turn(delta);

                meshes[0].WorldState.MoveDelta(new Vector3(0, 0, -40 * (float)Math.Sin(delta)));
                meshes[1].WorldState.MoveDelta(new Vector3(0, 0, -40 * (float)Math.Sin(delta)));
            }

            private void DoRandomMove()
            {
                Vector3 pos = meshes[0].WorldState.Position;
                pos += new Vector3(xAmplitude * (float)Math.Sin(Time.StepTime * 2.0f + timeOffset),
                                   yAmplitude * (float)Math.Sin(Time.StepTime * 4.0f + timeOffset) * (float)Math.Sin(Time.StepTime * 4.0f + timeOffset), 
                                   0);
                meshes[0].WorldState.Position = pos;
                meshes[1].WorldState.Position = pos;
            }
        }

        public MainEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Scene.AmbientColor = new ColorValue(1.0f, 1.0f, 1.0f);

            Model model1 = ModelFactory.CreateBox(10, 10, 10);
            model1.IMesh = model1.IMesh.Clone(MeshFlags.Managed, VertexFormats.Position | VertexFormats.Texture1 | VertexFormats.Normal, Device);
            model1.IMesh.ComputeNormals();
            GraphicsStream stream = model1.IMesh.LockVertexBuffer(LockFlags.None);
            CustomVertex.PositionNormalTextured[] vertices = new CustomVertex.PositionNormalTextured[model1.IMesh.NumberVertices];
            for (int i = 0; i < model1.IMesh.NumberVertices; i++)
            {
                vertices[i] = (CustomVertex.PositionNormalTextured)stream.Read(typeof(CustomVertex.PositionNormalTextured));
            }
            for (int i = 0; i < model1.IMesh.NumberVertices / 4; i++)
            {
                vertices[i * 4 + 0].Tu = 0;
                vertices[i * 4 + 0].Tv = 0;
                vertices[i * 4 + 1].Tu = 1;
                vertices[i * 4 + 1].Tv = 0;
                vertices[i * 4 + 2].Tu = 1;
                vertices[i * 4 + 2].Tv = 1;
                vertices[i * 4 + 3].Tu = 0;
                vertices[i * 4 + 3].Tv = 1;
            }
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            stream.Write(vertices);
            model1.IMesh.UnlockVertexBuffer();
            int[] adj = new int[model1.IMesh.NumberFaces * 3];
            model1.IMesh.GenerateAdjacency(1e-6f, adj);
            model1.IMesh.OptimizeInPlace(MeshFlags.OptimizeAttributeSort, adj);
            Model model2 = new Model(model1.IMesh.Clone(MeshFlags.Managed, model1.IMesh.Declaration, Device), new ModelMaterial[1]);

            Material material = new Material();
            material.AmbientColor = new ColorValue(0.3f, 0.3f, 0.3f);
            model2.Materials[0] = new ModelMaterial(material, D3DDriver.TextureFactory.CreateFromFile(PREFIX + "whitesortof.jpg"));

            AttributeRange[] attributes = new AttributeRange[1];
            attributes[0] = new AttributeRange();
            attributes[0].AttributeId = 0;
            attributes[0].FaceCount = 10;
            attributes[0].FaceStart = 0;
            attributes[0].VertexCount = 24;
            attributes[0].VertexStart = 0;
            model1.IMesh.SetAttributeTable(attributes);
            material = new Material();
            material.AmbientColor = new ColorValue(0.3f, 0.3f, 0.3f);
            model1.Materials[0] = new ModelMaterial(material, D3DDriver.TextureFactory.CreateFromFile(PREFIX + "whitesortof.jpg"));

            attributes[0] = new AttributeRange();
            attributes[0].AttributeId = 0;
            attributes[0].FaceCount = 2;
            attributes[0].FaceStart = 10;
            attributes[0].VertexCount = 24;
            attributes[0].VertexStart = 0;
            model2.IMesh.SetAttributeTable(attributes);

            IEffect effect = EffectFactory.CreateFromFile(EPREFIX + "Test.fxo");
            EffectHandler handler = new EffectHandler(effect);

            const float distance = 14.0f;
            int c = 0;
            cubes = new Cube[CubesInRow * CubesInRow];
            for (int y = 0; y < CubesInRow; y++)
            {
                for (int x = 0; x < CubesInRow; x++)
                {
                    Vector3 start = new Vector3(0, 0, 0);
                    if (x < CubesInRow / 2)
                        start.X = -200;
                    else
                        start.X = 200;
                    if (y < CubesInRow / 2)
                        start.Y = -200;
                    else
                        start.Y = 200;
                    cubes[c] = new Cube(x, y,
                                        new MeshNode("Mesh", new Model(model1.IMesh, model1.Materials), handler),
                                        new MeshNode("Mesh", new Model(model2.IMesh, model2.Materials), handler), 
                                        start,
                                        new Vector3((x - CubesInRow / 2) * distance, (y - CubesInRow / 2) * distance, 0));
                    cubes[c].AddToScene(Scene);
                    c++;
                }
            }

            lights = new LightNode[2];
            for (int i = 0; i < 2; i++)
            {
                Light dxLight = new Light();
                dxLight.DiffuseColor = new ColorValue(0.8f, 0.3f, 0.9f, 1.0f);
                dxLight.Type = LightType.Point;
                lights[i] = new LightNode("Point Light", dxLight);
                Scene.AddNode(lights[i]);
            }

            camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-400.0f);
            camera.WorldState.MoveRight(50.0f);
            Scene.AddNode(camera);
            Scene.ActiveCamera = camera;

            texture = D3DDriver.TextureFactory.CreateFromFile(PREFIX + "BlurBackground.jpg");
            dopeTexture = D3DDriver.TextureFactory.CreateFromFile(PREFIX + "DopeLogo.dds");
            sprite = D3DDriver.Factory.CreateSprite(Device);

            //lightProbe = D3DDriver.Factory.CubeTextureFromFile("../../Data/stpeters_cross.dds");
            //cubeTexture = D3DDriver.Factory.CreateCubeTexture(Device, 256, 1, Usage.None, Format.A16B16G16R16F, Pool.Managed);

            //float[] red = new float[6 * 6];
            //float[] green = new float[6 * 6];
            //float[] blue = new float[6 * 6];
            //D3DDriver.Factory.SphericalHarmonics.ProjectCubeMap(6, cubeTexture, red, green, blue);

        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
            if (Time.StepTime > 65.0f)
                Time.CurrentTime = 0.0f;
            Vector3 p1 = new Vector3(50.0f, 0.0f, -400.0f);
            Vector3 p2 = new Vector3(-85.0f, 75.0f, -80.0f);
            Vector3 p3 = new Vector3(-90.0f, -100.0f, -80.0f);
            Vector3 p4 = new Vector3(-45.0f, -97.5f, -50.0f);
            // 1-3
            camera.WorldState.Reset();
            camera.WorldState.Position = p1;

            // Bild 4
            SetPos(Cube.times[3] - 1.5f, p1, p2);

            // 5-8
            SetPos(Cube.times[4] - 1.5f, p2, p1);

            // Creds 9-14
            SetPos(Cube.times[8] - 1.5f, p1, p3);

            // 15-16
            SetPos(Cube.times[14] - 1.5f, p3, p1);

            // greets 17-18
            SetPos(Cube.times[16] - 1.5f, p1, p4);

            SetPos(Cube.times[25] - 1.5f, p4, p1);

            Scene.Step();
            foreach (Cube cube in cubes)
            {
                cube.Step();
                //mesh.WorldState.Roll(0.01f);
                //mesh.WorldState.Turn(0.021f);
            }
            lights[0].WorldState.Position = new Vector3(100.0f * (float)Math.Sin(Time.StepTime * 1.475f),
                                                        100.0f * (float)Math.Cos(Time.StepTime * 0.9465f),
                                                        -15.0f);
            lights[1].WorldState.Position = new Vector3(100.0f * (float)Math.Sin(Time.StepTime * 1.475f + 2),
                                                        100.0f * (float)Math.Cos(Time.StepTime * 0.9465f + 2),
                                                        -15.0f);
        }

        private void SetPos(float time, Vector3 p1, Vector3 p2)
        {
            if (Time.StepTime > time)
            {
                float delta = Time.StepTime - time;
                if (delta > 1.0f)
                    delta = 1.0f;
                Vector3 pos = p1 + (p2 - p1) * delta;
                camera.WorldState.Reset();
                camera.WorldState.Position = pos;
            }
        }

        public override void Render()
        {
            float logoAlpha = 0.8f;

            logoAlpha = GetAlpha(Cube.times[3] - 1.5f, logoAlpha, 0.8f, 0.0f);
            logoAlpha = GetAlpha(Cube.times[4] - 1.5f, logoAlpha, 0.0f, 0.8f);
            logoAlpha = GetAlpha(Cube.times[8] - 1.5f, logoAlpha, 0.8f, 0.0f);
            logoAlpha = GetAlpha(Cube.times[14] - 1.5f, logoAlpha, 0.0f, 0.8f);
            logoAlpha = GetAlpha(Cube.times[16] - 1.5f, logoAlpha, 0.8f, 0.0f);
            logoAlpha = GetAlpha(Cube.times[25] - 1.5f, logoAlpha, 0.0f, 0.8f);

            IDevice device = D3DDriver.GetInstance().GetDevice();
            Scene.Render();
            int sWidth = device.PresentationParameters.BackBufferWidth;
            int sHeight = device.PresentationParameters.BackBufferHeight;
            int tWidth = texture.GetSurfaceLevel(0).Description.Width;
            int tHeight = texture.GetSurfaceLevel(0).Description.Height;
            int tWidth2 = dopeTexture.GetSurfaceLevel(0).Description.Width;
            int tHeight2 = dopeTexture.GetSurfaceLevel(0).Description.Height;
            sprite.Begin(SpriteFlags.AlphaBlend);
            sprite.Draw2D(texture, Rectangle.Empty, new SizeF(sWidth * 1.9f, sHeight * 1.9f), new PointF(tWidth / 2, tHeight / 2), Time.StepTime * 0.22f, new PointF(sWidth / 2, sHeight / 2), Color.FromArgb(30, 255, 150, 150));
            sprite.Draw2D(texture, Rectangle.Empty, new SizeF(sWidth * 1.9f, sHeight * 1.9f), new PointF(tWidth / 2, tHeight / 2), -Time.StepTime * 0.15f, new PointF(sWidth / 2, sHeight / 2), Color.FromArgb(30, 150, 255, 150));
            sprite.Draw2D(dopeTexture, Rectangle.Empty, new SizeF(tWidth2, tHeight2), new PointF(tWidth2 / 2, tHeight2 / 2), (float)Math.PI / 2, new PointF(sWidth - tHeight * 0.8f, sHeight / 2), Color.FromArgb((int)(logoAlpha * 255), 255, 255, 255));
            sprite.End();
            ////device.RenderState.AlphaBlendEnable = true;
            ////device.RenderState.AlphaBlendOperation = BlendOperation.Add;
            ////device.RenderState.AlphaSourceBlend = Blend.BlendFactor;
            ////device.RenderState.AlphaDestinationBlend = Blend.InvBlendFactor;
            ////device.RenderState.BlendFactor = Color.FromArgb(128, 128, 128, 128);
        }

        private float GetAlpha(float time, float alpha, float alpha1, float alpha2)
        {
            if (Time.StepTime > time)
            {
                float delta = Time.StepTime - time;
                if (delta > 1.0f)
                    delta = 1.0f;
                return alpha1 + (alpha2 - alpha1) * delta;
            }
            return alpha;
        }
    }
}
