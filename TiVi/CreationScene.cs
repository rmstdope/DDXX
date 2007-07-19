using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using System.Drawing;
using Dope.DDXX.ParticleSystems;

namespace TiVi
{
    public class CreationScene : BaseDemoEffect
    {
        private const int MAX_NUM_DRAW_LINES = 100;
        private string baseMesh;
        private Scene scene;
        private UnindexedMesh lineTiViMesh;
        //private UnindexedMesh drawLineMesh;
        private IMesh originalTiViMesh;
        private SkinnedModel modelTiVi;
        private ModelNode nodeTiVi;
        private Vertex[] lineVertices;
        private List<TiViEdge> edges;
        private Vertex[] allVertices;
        private short[] indices;
        private ISprite sprite;
        private ITexture flareTexture;
        private ITexture whiteTexture;
        private List<int> flareIndices = new List<int>();
        private int startVertex = 8;
        private float flareSize = 20;
        //private ColorFader fader;
        private CameraNode headCamera;
        private CameraNode bodyCamera1;
        private CameraNode handCamera;
        private CameraNode bodyCamera2;
        private float bodyCameraStart1 = 7.0f;
        private float handCameraStart = 14.0f;
        private float bodyCameraStart2 = 21.0f;
        private float solidStart = 35.0f;
        private static float segmentLength = 0.45f;

        public float FlareSize
        {
            get { return flareSize; }
            set { flareSize = value; }
        }
        public float SegmentLength
        {
            get { return segmentLength; }
            set 
            { 
                segmentLength = value;
                if (lineTiViMesh != null)
                    CreateLineMesh(startVertex);
            }
        }
        public int StartVertex
        {
            get { return startVertex; }
            set 
            { 
                startVertex = value;
                if (lineTiViMesh != null)
                    CreateLineMesh(startVertex);
            }
        }
        public string BaseMesh
        {
            get { return baseMesh; }
            set { baseMesh = value; }
        }
        public float BodyCameraStart1
        {
            get { return bodyCameraStart1; }
            set { bodyCameraStart1 = value; }
        }
        public float HandCameraStart
        {
            get { return handCameraStart; }
            set { handCameraStart = value; }
        }
        public float BodyCameraStart2
        {
            get { return bodyCameraStart2; }
            set { bodyCameraStart2 = value; }
        }

        private struct Vertex
        {
            public Vertex(int i)
            {
                Position = new Vector3();
                BlendWeight1 = 0;
                BlendWeight2 = 0;
                BlendWeight3 = 0;
                BlendIndices = 0;
                Normal = new Vector3();
                U = 0;
                V = 0;
                //BiNormal = new Vector3();
                //Tangent = new Vector3();
            }
            public VertexFormats VertexFormats
            {
                get
                {
                    return VertexFormats.PositionBlend4 | VertexFormats.LastBetaUByte4 |
                        VertexFormats.Normal | VertexFormats.Texture0 | 
                        (VertexFormats)(1 << (int)VertexFormats.TextureCountShift);
                }
            }
            public Vector3 Position;
            public float BlendWeight1;
            public float BlendWeight2;
            public float BlendWeight3;
            public UInt32 BlendIndices;
            public Vector3 Normal;
            public float U;
            public float V;
            //public Vector3 BiNormal;
            //public Vector3 Tangent;
        }

        public CreationScene(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        private Vector4 circleCallback(Vector2 texCoord, Vector2 texelSize)
        {
            Vector2 centered = texCoord - new Vector2(0.5f, 0.5f);
            float distance = centered.Length();
            if (distance < 0.1f)
                return new Vector4(1, 1, 1, 1);
            else if (distance < 0.5f)
            {
                float scaled = (0.5f - distance) / 0.4f;
                return new Vector4(scaled, scaled, scaled, 1);
            }
            return new Vector4(0, 0, 0, 0);
        }

        protected override void Initialize()
        {
            scene = new Scene();

            GenerateModel();

            SetUpCamera();

            sprite = GraphicsFactory.CreateSprite(Device);
            flareTexture = TextureFactory.CreateFromFunction(64, 64, 0, Usage.None, Format.A8R8G8B8, Pool.Managed, circleCallback);
            //TextureFactory.CreateFromFile("Flare.dds");
            whiteTexture = TextureFactory.CreateFromFunction(64, 64, 1, Usage.None,
                Format.A8R8G8B8, Pool.Managed,
                delegate(Vector2 coord, Vector2 texel) { return new Vector4(1, 1, 1, 1); });

            //fader = new ColorFader(Device, sprite, whiteTexture);
        }

        private void SetUpCamera()
        {
            bodyCamera1 = new CameraNode("FullsizeCamera1");
            scene.AddNode(bodyCamera1);

            bodyCamera2 = new CameraNode("FullsizeCamera2");
            scene.AddNode(bodyCamera2);

            headCamera = new CameraNode("HeadCamera");
            scene.AddNode(headCamera);

            handCamera = new CameraNode("HandCamera");
            scene.AddNode(handCamera);

            scene.ActiveCamera = bodyCamera1;
        }

        private class TiViEdge
        {
            public short V1;
            public short V2;
            public float StartTime;
            public float Length;
            private static Random random = new Random(0xBEEF);

            public TiViEdge(short v1, short v2)
            {
                V1 = v1;
                V2 = v2;
                StartTime = -1;
                Length = 0.0f;
            }

            public void SwitchVertices()
            {
                short temp = V1;
                V1 = V2;
                V2 = temp;
            }
            
            public void StartVertex(float startTime)
            {
                StartTime = startTime;
                Length = segmentLength + (float)(random.NextDouble() * segmentLength);
                if (startTime < 7.0f)
                    Length += 0.5f;
                if (startTime > 21.0f)
                    Length -= 0.15f;
                //if (startTime == 0)
                //    Length += 3;
            }
        }

        private void GenerateModel()
        {
            LoadAnimation();
            IMesh clonedMesh = modelTiVi.Mesh.Clone(MeshFlags.Managed, modelTiVi.Mesh.Declaration, Device);
            WeldVertices(clonedMesh);
            ExtractMeshData(clonedMesh);
            CreateLineMesh(StartVertex);
        }

        private void CreateLineMesh(int start)
        {
            CreateEdges(indices, start);
            CreateLineMeshes();
        }

        private void CreateLineMeshes()
        {
            lineVertices = new Vertex[edges.Count * 2];
            lineTiViMesh = new UnindexedMesh(GraphicsFactory, typeof(Vertex), lineVertices.Length,
                Device, Usage.WriteOnly | Usage.Dynamic, lineVertices[0].VertexFormats, Pool.Default);

            //drawLineMesh = new UnindexedMesh(GraphicsFactory, typeof(Vertex), MAX_NUM_DRAW_LINES * 2,
            //    Device, Usage.WriteOnly | Usage.Dynamic, lineVertices[0].VertexFormats, Pool.Default);
            //IModel model = new Model(drawLineMesh);
            //ModelNode node = new ModelNode("Lines", modelTiVi,
            //    new EffectHandler(EffectFactory.CreateFromFile("TiVi.fxo"), 
            //    TechniqueChooser.MaterialPrefix("Line"), model));
            //scene.AddNode(node);
        }

        private void LoadAnimation()
        {
            XLoader.Load(BaseMesh, EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(string name)
                {
                    return delegate(int material)
                    {
                        if (material == 1)
                            return "TvScreen";
                        else
                            return "Solid";
                    };
                });
            XLoader.AddToScene(scene);
            //IAnimationSet set = XLoader.RootFrame.AnimationController.GetAnimationSet(0);
            //XLoader.RootFrame.AnimationController.SetTrackAnimationSet(0, set);
            nodeTiVi = (ModelNode)scene.GetNodeByName("TiVi");
            modelTiVi = nodeTiVi.Model as SkinnedModel;
            originalTiViMesh = modelTiVi.Mesh;
        }

        private void ExtractMeshData(IMesh mesh)
        {
            ExtractMeshIndices(mesh);
            ExtractMeshVertices(mesh);
        }

        private void ExtractMeshVertices(IMesh mesh)
        {
            using (IGraphicsStream stream = mesh.LockVertexBuffer(LockFlags.ReadOnly))
            {
                allVertices = (Vertex[])stream.Read(typeof(Vertex), new int[] { mesh.NumberVertices });
                mesh.UnlockVertexBuffer();
            }
        }

        private void ExtractMeshIndices(IMesh mesh)
        {
            using (IGraphicsStream stream = mesh.LockIndexBuffer(LockFlags.ReadOnly))
            {
                indices = (short[])stream.Read(typeof(short), new int[] { mesh.NumberFaces * 3 });
                mesh.UnlockIndexBuffer();
            }
        }

        private static void WeldVertices(IMesh mesh)
        {
            int[] adia = new int[mesh.NumberFaces * 3];
            mesh.GenerateAdjacency(0.01f, adia);
            mesh.WeldVertices(WeldEpsilonsFlags.WeldAll, new WeldEpsilons(), adia);
        }

        private void CreateEdges(short[] indices, int startVertex)
        {
            edges = new List<TiViEdge>();
            List<TiViEdge> undrawnEdges = new List<TiViEdge>();
            List<TiViEdge> drawingEdges = new List<TiViEdge>();
            for (int i = 0; i < indices.Length / 3; i++)
            {
                AddEdge(undrawnEdges, indices[i * 3 + 0], indices[i * 3 + 1]);
                AddEdge(undrawnEdges, indices[i * 3 + 1], indices[i * 3 + 2]);
                AddEdge(undrawnEdges, indices[i * 3 + 2], indices[i * 3 + 0]);
            }

            GetEdgesForVertex(undrawnEdges, drawingEdges, startVertex, 0);
            while (drawingEdges.Count != 0)
            {
                SortEdges(drawingEdges);
                TiViEdge e = drawingEdges[0];
                drawingEdges.RemoveAt(0);
                edges.Add(e);
                GetEdgesForVertex(undrawnEdges, drawingEdges, e.V2, e.StartTime + e.Length);
            }
            SortEdges(edges);
        }

        private void SortEdges(List<TiViEdge> edges)
        {
            edges.Sort(delegate(TiViEdge e1, TiViEdge e2)
            {
                return Comparer<float>.Default.Compare(e1.StartTime, e2.StartTime);
            });
        }

        private void GetEdgesForVertex(List<TiViEdge> srcList, List<TiViEdge> dstList, int vertex, float t)
        {
            srcList.ForEach(delegate(TiViEdge e)
            {
                if (e.V2 == vertex)
                    e.SwitchVertices();
                if (e.V1 == vertex)
                {
                    e.StartVertex(t);
                    dstList.Add(e);
                    srcList.Remove(e);
                }
            });
        }


        private void AddEdge(List<TiViEdge> edges, short v1, short v2)
        {
            if (!edges.Exists(delegate(TiViEdge e1)
            {
                if ((e1.V1 == v1 && e1.V2 == v2) ||
                    (e1.V1 == v2 && e1.V2 == v1))
                    return true;
                return false;
            }))
                edges.Add(new TiViEdge(v1, v2));
        }

        public override void Step()
        {
            float t = Time.StepTime - StartTime;

            Mixer.ClearColor = Color.FromArgb(0, 0, 0, 0);
            if (nodeTiVi != null)
            {
                int i;
                flareIndices.Clear();
                for (i = 0; i < edges.Count; i++)
                {
                    if (edges[i].StartTime > t)
                        break;
                    lineVertices[i * 2 + 0] = allVertices[edges[i].V1];
                    lineVertices[i * 2 + 1] = allVertices[edges[i].V2];
                    float delta = (t - edges[i].StartTime) / edges[i].Length;
                    if (delta > 1.0f)
                        delta = 1.0f;
                    else
                        flareIndices.Add(i * 2 + 1);
                    Vector3 v = lineVertices[i * 2 + 1].Position - lineVertices[i * 2 + 0].Position;
                    lineVertices[i * 2 + 1].Position = lineVertices[i * 2 + 0].Position + v * delta;
                }
                lineTiViMesh.SetVertexBufferData(lineVertices, LockFlags.Discard);
                lineTiViMesh.NumberActiveVertices = i * 2;
            }

            headCamera.WorldState.Reset();
            headCamera.WorldState.MoveForward(-(1.0f + t / 8.0f));
            headCamera.WorldState.MoveUp(1.5f);

            bodyCamera1.WorldState.Reset();
            bodyCamera1.WorldState.MoveForward(-(3.0f - t / 10.0f));
            bodyCamera1.WorldState.MoveUp(1);

            handCamera.WorldState.Reset();
            handCamera.WorldState.Turn(1.0f - t / 7.0f);
            handCamera.WorldState.MoveUp(1.0f);
            handCamera.WorldState.MoveForward(-1.0f);//.Position = new Vector3(2, 2, 0);
            handCamera.LookAt(new Vector3(0, 1.5f, 0), new Vector3(0, 1, 0));

            bodyCamera2.WorldState.Reset();
            bodyCamera2.WorldState.MoveForward(-(1.0f + t / 10.0f));
            bodyCamera2.WorldState.MoveUp(1);

            if (t < bodyCameraStart1)
            {
                //modelTiVi.SetAnimationSet("LookArm", 0);
                modelTiVi.SetAnimationSet("SkinPose", StartTime);
                scene.ActiveCamera = headCamera;
            }
            else if (t < handCameraStart)
            {
                modelTiVi.SetAnimationSet("ArmsCrossed", StartTime + bodyCameraStart1);
                scene.ActiveCamera = bodyCamera1;
            }
            else if (t < bodyCameraStart2)
            {
                modelTiVi.SetAnimationSet("LookArm", StartTime + handCameraStart);
                scene.ActiveCamera = handCamera;
            }
            else
            {
                modelTiVi.SetAnimationSet("ArmsCrossed", StartTime + bodyCameraStart2);
                scene.ActiveCamera = bodyCamera2;
            }

            scene.Step();
        }

        public override void Render()
        {
            if (nodeTiVi != null)
            {
                if (Time.CurrentTime > solidStart + StartTime)
                {
                    nodeTiVi.EffectHandler.Techniques[0] = EffectHandle.FromString("SolidSkinning");
                    nodeTiVi.EffectHandler.Techniques[1] = EffectHandle.FromString("TvScreenSkinning");
                    modelTiVi.Materials[0].AmbientColor = new ColorValue(0.2f, 0.2f, 0.2f);
                    modelTiVi.Materials[0].DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f);
                    modelTiVi.Materials[0].SpecularColor = new ColorValue(1.0f, 1.0f, 1.0f);
                    modelTiVi.Materials[0].Shininess = 64;
                    modelTiVi.Mesh = originalTiViMesh;
                }
                else
                {
                    nodeTiVi.EffectHandler.Techniques[0] = EffectHandle.FromString("LineDrawerSkinning");
                    nodeTiVi.EffectHandler.Techniques[1] = EffectHandle.FromString("LineDrawerSkinning");
                    modelTiVi.Materials[0].AmbientColor = new ColorValue(0.4f, 0.4f, 0.4f);
                    modelTiVi.Materials[0].DiffuseColor = new ColorValue(0.7f, 0.7f, 0.3f);
                    modelTiVi.Materials[0].SpecularColor = new ColorValue(1.0f, 1.0f, 1.0f);
                    //modelTiVi.Materials[0].AmbientColor = new ColorValue(0.4f, 0.4f, 0.4f);
                    //modelTiVi.Materials[0].DiffuseColor = new ColorValue(0.7f, 0.7f, 0.3f);
                    //modelTiVi.Materials[0].SpecularColor = new ColorValue(0.5f, 0.5f, 0.5f);
                    modelTiVi.Materials[0].Shininess = 16;
                    modelTiVi.Mesh = lineTiViMesh;
                }
            }
            scene.Render();
            DrawFlares();
        }

        private void DrawFlares()
        {
            sprite.Begin(SpriteFlags.AlphaBlend);
            Matrix[] matrices = ((SkinnedModel)modelTiVi).GetBoneMatrices(0);
            for (int i = 0; i < flareIndices.Count; i++)
            {
                Vertex v = lineVertices[flareIndices[i]];
                Matrix matrix = matrices[v.BlendIndices & 0xFF] * scene.ActiveCamera.ViewMatrix * scene.ActiveCamera.ProjectionMatrix;
                Vector3 pos = v.Position;
                pos.TransformCoordinate(matrix);
                pos *= 0.5f;
                pos += new Vector3(0.5f, 0.5f, 0);
                pos.X *= Device.Viewport.Width;
                pos.Y *= Device.Viewport.Height;
                pos.Y = Device.Viewport.Height - pos.Y;
                pos -= new Vector3(flareSize / 2.0f, flareSize / 2.0f, 0);
                Device.RenderState.BlendOperation = BlendOperation.Max;
                Device.RenderState.SourceBlend = Blend.One;
                Device.RenderState.DestinationBlend = Blend.One;
                sprite.Draw2D(flareTexture, Rectangle.Empty, new SizeF(flareSize, flareSize),
                    new PointF(pos.X, pos.Y), Color.White);
            }
            sprite.End();
        }
    }
}
