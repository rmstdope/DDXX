using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics.Skinning;
using System.Drawing;
using Dope.DDXX.ParticleSystems;

namespace EngineTest
{
    public class CreationScene : BaseDemoEffect
    {
        private const int MAX_NUM_DRAW_LINES = 100;
        private string baseMesh;
        private Scene scene;
        private UnindexedMesh lineTiViMesh;
        //private UnindexedMesh drawLineMesh;
        private IMesh originalTiViMesh;
        private IModel modelTiVi;
        private ModelNode nodeTiVi;
        private Vertex[] lineVertices;
        private List<TiViEdge> edges;
        private Vertex[] allVertices;
        private short[] indices;

        public string BaseMesh
        {
            get { return baseMesh; }
            set { baseMesh = value; }
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
                        VertexFormats.Normal;
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

        public CreationScene(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            scene = new Scene();

            GenerateModel();

            SetUpCamera();

            //FallingStarSystem system = new FallingStarSystem("FS");
            //system.Initialize(100, Device, GraphicsFactory, EffectFactory, TextureFactory.CreateFromFile("red glass.jpg"));
            //scene.AddNode(system);
        }

        private void SetUpCamera()
        {
            //CameraNode camera = (CameraNode)scene.GetNodeByName("Camera01");
            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-3);
            camera.WorldState.MoveUp(1);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
        }

        private class TiViEdge
        {
            public short V1;
            public short V2;
            public float StartTime;
            public float Length;
            private static Random random = new Random();

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
                Length = 0.3f + (float)(random.NextDouble() * 0.1f);
            }
        }

        private void GenerateModel()
        {
            LoadAnimation();
            IMesh clonedMesh = modelTiVi.Mesh.Clone(MeshFlags.Managed, modelTiVi.Mesh.Declaration, Device);
            WeldVertices(clonedMesh);
            ExtractMeshData(clonedMesh);
            CreateEdges(indices, 0);
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
            nodeTiVi = (ModelNode)scene.GetNodeByName("TiVi");
            modelTiVi = nodeTiVi.Model;
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
            if (nodeTiVi != null)
            {
                int i;
                for (i = 0; i < edges.Count; i++)
                {
                    if (edges[i].StartTime > Time.CurrentTime)
                        break;
                    lineVertices[i * 2 + 0] = allVertices[edges[i].V1];
                    lineVertices[i * 2 + 1] = allVertices[edges[i].V2];
                    float delta = (Time.CurrentTime - edges[i].StartTime) / edges[i].Length;
                    if (delta > 1.0f)
                        delta = 1.0f;
                    Vector3 v = lineVertices[i * 2 + 1].Position - lineVertices[i * 2 + 0].Position;
                    lineVertices[i * 2 + 1].Position = lineVertices[i * 2 + 0].Position + v * delta;
                }
                lineTiViMesh.SetVertexBufferData(lineVertices, LockFlags.Discard);
                lineTiViMesh.NumberActiveVertices = i * 2;
            }

            scene.Step();
        }

        public override void Render()
        {
            if (nodeTiVi != null)
            {
                if (Time.CurrentTime > 25.0f)
                {
                    modelTiVi.Materials[0].AmbientColor = new ColorValue(0.2f, 0.2f, 0.2f);
                    modelTiVi.Materials[0].DiffuseColor = new ColorValue(1.0f, 1.0f, 1.0f);
                    modelTiVi.Materials[0].SpecularColor = new ColorValue(1.0f, 1.0f, 1.0f);
                    modelTiVi.Materials[0].Shininess = 64;
                    modelTiVi.Mesh = originalTiViMesh;
                }
                else
                {
                    modelTiVi.Materials[0].AmbientColor = new ColorValue(0.4f, 0.4f, 0.4f);
                    modelTiVi.Materials[0].DiffuseColor = new ColorValue(0.7f, 0.7f, 0.3f);
                    modelTiVi.Materials[0].SpecularColor = new ColorValue(0.5f, 0.5f, 0.5f);
                    modelTiVi.Materials[0].Shininess = 32;
                    modelTiVi.Mesh = lineTiViMesh;
                }
            }
            scene.Render();
        }
    }
}
