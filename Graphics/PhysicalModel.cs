using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public class PhysicalModel : Model 
    {
        private IBody body;
        private Vector3[] normals;
        private short[] indices;

        public IBody Body
        {
            get 
            { 
                return body; 
            }
            set
            {
                if (value.Particles.Count != Mesh.NumberVertices)
                    throw new DDXXException("Mesh and Body must have equal number of vertices/particles.");
                body = value;
            }
        }

        public PhysicalModel(IMesh mesh, IBody body)
            : base(mesh)
        {
            Body = body;
            PrepareNormalCalculation();
        }

        public PhysicalModel(IMesh mesh, IBody body, ModelMaterial[] materials)
            : base(mesh, materials)
        {
            Body = body;
            PrepareNormalCalculation();
        }

        private void PrepareNormalCalculation()
        {
            normals = new Vector3[Mesh.NumberVertices];
            indices = new short[Mesh.NumberFaces * 3];
            using (IGraphicsStream stream = Mesh.LockIndexBuffer(LockFlags.None))
            {
                indices = (short[])stream.Read(typeof(short), new int[] { Mesh.NumberFaces * 3 });
                Mesh.UnlockIndexBuffer();
            }
        }

        public override void Step()
        {
            body.Step();

            for (int i = 0; i < Mesh.NumberVertices; i++)
            {
                normals[i] = new Vector3(0, 0, 0);
            }
            for (int i = 0; i < Mesh.NumberFaces; i++)
            {
                int i1 = indices[i * 3 + 0];
                int i2 = indices[i * 3 + 1];
                int i3 = indices[i * 3 + 2];
                // Faces are counter clockwise

                Vector3 vec = Vector3.Cross(body.Particles[i2].Position - body.Particles[i1].Position,
                    body.Particles[i3].Position - body.Particles[i1].Position);
                vec.Normalize();
                normals[i1] += vec;
                normals[i2] += vec;
                normals[i3] += vec;
            }
            for (int i = 0; i < Mesh.NumberVertices; i++)
            {
                normals[i].Normalize();
            }


            VertexElementArray array = new VertexElementArray(Mesh.Declaration);
            bool texCoords = array.HasTexCoords(0);

            using (IGraphicsStream stream = Mesh.LockVertexBuffer(LockFlags.None))
            {
                for (int i = 0; i < body.Particles.Count; i++)
                {
                    stream.Write(body.Particles[i].Position);
                    stream.Write(normals[i]);
                    if (texCoords)
                        stream.Seek(sizeof(float) * 2, System.IO.SeekOrigin.Current);
                }
                Mesh.UnlockVertexBuffer();
            }
        }
    }
}
