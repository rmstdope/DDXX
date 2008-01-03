using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class Vertex
    {
        private Vector3 position;
        private Vector3 normal;
        private Vector3 tangent;
        private Vector3 binormal;
        private float u;
        private float v;
        private bool textureCoordinatesUsed;

        public bool TextureCoordinatesUsed
        {
            get { return textureCoordinatesUsed; }
        }

        public float U
        {
            get { return u; }
            set { u = value; textureCoordinatesUsed = true; }
        }

        public float V
        {
            get { return v; }
            set { v = value; textureCoordinatesUsed = true; }
        }

        public Vector2 UV
        {
            get { return new Vector2(u, v); }
            set { u = value.X; v = value.Y; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Normal
        {
            get { return normal; }
            set { normal = value; }
        }

        public Vector3 BiNormal
        {
            get { return binormal; }
            set { binormal = value; }
        }

        public Vector3 Tangent
        {
            get { return tangent; }
            set { tangent = value; }
        }

        public Vertex()
        {
        }

        public Vertex(Vector3 position, Vector3 normal, Vector2 texCoords)
        {
            this.position = position;
            this.normal = normal;
            this.u = texCoords.X;
            this.v = texCoords.Y;
            this.textureCoordinatesUsed = true;
            this.tangent = new Vector3();
            this.binormal = new Vector3();
        }

        public Vertex(Vector3 position, Vector3 normal)
        {
            this.position = position;
            this.normal = normal;
        }

    }
}
