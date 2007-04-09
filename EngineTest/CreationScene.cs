using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;

namespace EngineTest
{
    public class CreationScene : BaseDemoEffect
    {
        private string baseMesh;
        private Scene scene;
        private UnindexedMesh lineMesh;
        private IModel model;
        private ModelNode node;
        private Vertex[] lineVertices;
        private List<Edge> edges;
        private Vertex[] allVertices;
        private Random random = new Random();

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
                Normal = new Vector3();
                U = 0;
                V = 0;
            }
            public Vector3 Position;
            public Vector3 Normal;
            public float U;
            public float V;
        }

        public CreationScene(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Vertex[] vertices = new Vertex[]
            {
                new Vertex(),
                new Vertex(),
                new Vertex(),
                new Vertex()
            };

            scene = new Scene();

            GenerateModel();

            model = new Model(lineMesh);
            node = new ModelNode("LineTiVi", model,
                new EffectHandler(EffectFactory.CreateFromFile("Test.fxo"), "Line", null));
            //node.WorldState.Tilt(-(float)Math.PI / 2);
            node.WorldState.MoveUp(-2);
            scene.AddNode(node);

            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.MoveForward(-6);
            scene.AddNode(camera);
            scene.ActiveCamera = camera;
        }

        private class Edge
        {
            public Edge(short v1, short v2)
            {
                V1 = v1;
                V2 = v2;
                StartTime = -1;
                Length = 0.0f;
            }
            public short V1;
            public short V2;
            public float StartTime;
            public float Length;
        }

        private void GenerateModel()
        {
            IModel model = ModelFactory.FromFile("king.X", ModelOptions.None);
            IMesh mesh = model.Mesh;
            int[] adia = new int[mesh.NumberFaces * 3];
            mesh.GenerateAdjacency(0.1f, adia);
            mesh.WeldVertices(WeldEpsilonsFlags.WeldAll, new WeldEpsilons(), adia);
            IGraphicsStream stream = mesh.LockIndexBuffer(LockFlags.ReadOnly);
            short[] indices = (short[])stream.Read(typeof(short), new int[] { mesh.NumberFaces * 3 });
            mesh.UnlockIndexBuffer();
            stream = mesh.LockVertexBuffer(LockFlags.ReadOnly);
            allVertices = (Vertex[])stream.Read(typeof(Vertex), new int[] { mesh.NumberVertices });
            mesh.UnlockVertexBuffer();
            edges = CreateEdges(indices, 0);
            lineVertices = new Vertex[edges.Count * 2];

            lineMesh = new UnindexedMesh(D3DDriver.GraphicsFactory, typeof(Vertex), lineVertices.Length, //undrawnEdges.Count * 2,
                D3DDriver.GetInstance().Device, Usage.WriteOnly | Usage.Dynamic, VertexFormats.Position, Pool.Default);

        }

        private List<Edge> CreateEdges(short[] indices, int startVertex)
        {
            List<Edge> undrawnEdges = new List<Edge>();
            List<Edge> drawingEdges = new List<Edge>();
            List<Edge> drawnEdges = new List<Edge>();
            for (int i = 0; i < indices.Length / 3; i++)
            {
                AddEdge(undrawnEdges, indices[i * 3 + 0], indices[i * 3 + 1]);
                AddEdge(undrawnEdges, indices[i * 3 + 1], indices[i * 3 + 2]);
                AddEdge(undrawnEdges, indices[i * 3 + 2], indices[i * 3 + 0]);
            }

            GetEdges(undrawnEdges, drawingEdges, startVertex, 0);
            while (drawingEdges.Count != 0)
            {
                SortEdges(drawingEdges);
                Edge e = drawingEdges[0];
                drawingEdges.RemoveAt(0);
                drawnEdges.Add(e);
                GetEdges(undrawnEdges, drawingEdges, e.V2, e.StartTime + e.Length);
            }
            SortEdges(drawnEdges);
            return drawnEdges;
        }

        private void SortEdges(List<Edge> edges)
        {
            edges.Sort(delegate(Edge e1, Edge e2)
            {
                return Comparer<float>.Default.Compare(e1.StartTime, e2.StartTime);
            });
        }

        private void GetEdges(List<Edge> srcList, List<Edge> dstList, int vertex, float t)
        {
            srcList.ForEach(delegate(Edge e)
            {
                if (e.V2 == vertex)
                {
                    short temp = e.V1;
                    e.V1 = e.V2;
                    e.V2 = temp;
                }
                if (e.V1 == vertex)
                {
                    e.StartTime = t;
                    e.Length = 0.1f + (float)(random.NextDouble() * 0.2f);
                    dstList.Add(e);
                    srcList.Remove(e);
                }
            });
        }


        private void AddEdge(List<Edge> edges, short v1, short v2)
        {
            if (!edges.Exists(delegate(Edge e1)
            {
                if ((e1.V1 == v1 && e1.V2 == v2) ||
                    (e1.V1 == v2 && e1.V2 == v1))
                    return true;
                return false;
            }))
                edges.Add(new Edge(v1, v2));
        }

        public override void Step()
        {
            for (int i = 0; i < edges.Count; i++)
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
            lineMesh.SetVertexBufferData(lineVertices, LockFlags.Discard);

            //node.WorldState.Turn(Time.DeltaTime);
            //node.WorldState.Tilt(Time.DeltaTime / 1.234f);
            //node.WorldState.Roll(Time.DeltaTime / 1.623f);

            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
