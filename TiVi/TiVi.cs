using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.Utility;

namespace TiVi
{
    public class TiVi
    {
        private ModelNode node;
        private SkinnedModel model;
        private Vector3 upperLeft;
        private Vector3 upperRight;
        private Vector3 lowerLeft;
        public Vector3 Center;
        private Vector3 normal;
        public Vector3 DestinationPos;
        public Vector3 DestinationUp;

        public TiVi(ModelNode node, CameraNode camera, float time)
        {
            this.node = node;
            this.model = node.Model as SkinnedModel;
            ExtractTiViInfo(camera.GetFOV(), time);

        }

        private void ExtractTiViInfo(float fov, float time)
        {
            IMesh mesh = node.Model.Mesh;
            VertexElement[] elements = node.Model.Mesh.Declaration;
            VertexFormats format = node.Model.Mesh.VertexFormat;
            AttributeRange range = node.Model.Mesh.GetAttributeTable()[1];
            short[] indices;// = new short[range.FaceCount];
            indices = mesh.IndexBuffer.Lock(range.FaceStart * 3 * sizeof(short), typeof(short), LockFlags.ReadOnly, new int[] { range.FaceCount * 3 }) as short[];
            mesh.IndexBuffer.Unlock();
            TiViVertex[] vertices;
            vertices = mesh.VertexBuffer.Lock(0, typeof(TiViVertex), LockFlags.ReadOnly, new int[] { mesh.NumberVertices }) as TiViVertex[];
            mesh.VertexBuffer.Unlock();

            int i1 = 0;
            int i2 = 1;
            int i3 = 2;
            Time.Pause();
            Time.CurrentTime = time;
            node.Step(null);
            Time.Resume();
            upperLeft = GetScreenPosition(new Vector2(0, 0), vertices[indices[i1]], vertices[indices[i2]], vertices[indices[i3]], model.GetBoneMatrices(0)[0]);
            upperRight = GetScreenPosition(new Vector2(1, 0), vertices[indices[i1]], vertices[indices[i2]], vertices[indices[i3]], model.GetBoneMatrices(0)[0]);
            lowerLeft = GetScreenPosition(new Vector2(0, 1), vertices[indices[i1]], vertices[indices[i2]], vertices[indices[i3]], model.GetBoneMatrices(0)[0]);

            Center = (lowerLeft + upperRight) * 0.5f;// new Vector3((upperLeft.mX + upperRight.mX) / 2.0f, (upperLeft.mY + lowerLeft.mY) / 2.0f, lowerLeft.mZ);
            normal = Vector3.Cross(upperRight - upperLeft, lowerLeft - upperLeft);
            normal.Normalize();

            float oppositeLength = (upperLeft - lowerLeft).Length() / 2;
            float closeLength = oppositeLength / (float)Math.Tan(fov / 2);
            DestinationPos = Center + normal * closeLength * 1.0f;

            Vector3 right = (upperRight - upperLeft);
            right.Normalize();
            DestinationUp = (upperLeft - lowerLeft);
            DestinationUp.Normalize();
        }

        private Vector3 GetScreenPosition(Vector2 destUV, TiViVertex v1, TiViVertex v2, TiViVertex v3, Matrix headMatrix)
        {
            Vector3 p1 = v1.Position;
            p1.TransformCoordinate(headMatrix);
            Vector2 t1 = new Vector2(v1.U, v1.V);
            Vector3 p2 = v2.Position;
            p2.TransformCoordinate(headMatrix);
            Vector2 t2 = new Vector2(v2.U, v2.V);
            Vector3 p3 = v3.Position;
            p3.TransformCoordinate(headMatrix);
            Vector2 t3 = new Vector2(v3.U, v3.V);

            Vector2 tv1 = t2 - t1;
            Vector2 tv2 = t3 - t1;

            float c2 = (((t1.Y - destUV.Y) * tv1.X / tv1.Y) - (t1.X - destUV.X)) / (tv2.X - (tv2.Y * tv1.X) / tv1.Y);
            float c1 = (-(t1.X - destUV.X) - c2 * tv2.X) / tv1.X;

            Vector3 pv1 = p2 - p1;
            Vector3 pv2 = p3 - p1;

            return p1 + c1 * pv1 + c2 * pv2;
        }

    }
}
