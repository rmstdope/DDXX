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
        }

        public override void Step()
        {
            body.Step();

            VertexElementArray array = new VertexElementArray(Mesh.Declaration);
            bool texCoords = array.HasTexCoords(0);

            IGraphicsStream stream = Mesh.LockVertexBuffer(LockFlags.None);
            for (int i = 0; i < body.Particles.Count; i++)
            {
                stream.Write(body.Particles[i].Position);
                stream.Write(new Vector3(0, 0, -1));
                if (texCoords)
                    stream.Seek(sizeof(float) * 2, System.IO.SeekOrigin.Current);
            }
            Mesh.UnlockVertexBuffer();
        }
    }
}
