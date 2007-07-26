using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;

namespace TiVi
{
    public class ThinkTank : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;

        public ThinkTank(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 3);
            camera.WorldState.MoveUp(1.5f);

            XLoader.Load("Tivi-Sitting.X", EffectFactory.CreateFromFile("TiVi.fxo"),
                delegate(string name)
                {
                    if (name == "TiVi")
                    {
                        return delegate(int material)
                        {
                            if (material == 1)
                                return "TvScreen";
                            else
                                return "Solid";
                        };
                    }
                    else
                        return TechniqueChooser.MaterialPrefix("Terrain");
                });
            XLoader.AddToScene(scene);
            //scene.GetNodeByName("TiVi").WorldState.Turn((float)Math.PI * 1.2f);
            //scene.GetNodeByName("TiVi").WorldState.Position = new Vector3(0, 0.25f, 0);

            ExtractTiViInfo();
        }

        private void ExtractTiViInfo()
        {
            ModelNode tiviNode = scene.GetNodeByName("TiVi") as ModelNode;
            IMesh mesh = tiviNode.Model.Mesh;
            VertexElement[] elements = tiviNode.Model.Mesh.Declaration;
            VertexFormats format = tiviNode.Model.Mesh.VertexFormat;
            AttributeRange range = tiviNode.Model.Mesh.GetAttributeTable()[1];
            short[] indices;// = new short[range.FaceCount];
            indices = mesh.IndexBuffer.Lock(range.FaceStart * 3 * sizeof(short), typeof(short), LockFlags.ReadOnly, new int[] { range.FaceCount * 3 }) as short[];
            mesh.IndexBuffer.Unlock();
            TiViVertex[] vertices;
            vertices = mesh.VertexBuffer.Lock(0, typeof(TiViVertex), LockFlags.ReadOnly, new int[] { mesh.NumberVertices }) as TiViVertex[];
            mesh.VertexBuffer.Unlock();

            Vector3 pos = new Vector3();
            for (int i = 0; i < range.FaceCount * 3; i++)
            {
                TiViVertex vertex = vertices[indices[i]];
                pos += vertex.Position;
            }
            //pos /= range.FaceCount * 3;
        }

        public override void Step()
        {
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
