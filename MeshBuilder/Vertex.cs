using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public struct Vertex
    {
        private Vector3 position;
        private Vector3 normal;
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

    }
}
