using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public delegate Vector3 AmplitudeFunction(Vector3 pos);

    public class Amplitude : ModifierBase
    {
        private AmplitudeFunction function;

        public AmplitudeFunction Function
        {
            get { return function; }
            set { function = value; }
        }

        public Amplitude()
            : base(1)
        {
            function = delegate(Vector3 pos) { return Vector3.One; };
        }

        public override IPrimitive Generate()
        {
            IPrimitive primitive = GetInput(0);
            foreach (Vertex vertex in primitive.Vertices)
            {
                Vector3 amplitude = function(vertex.Position);
                vertex.Position = new Vector3(vertex.Position.X * amplitude.X,
                                              vertex.Position.Y * amplitude.Y,
                                              vertex.Position.Z * amplitude.Z);
            }
            ComputeNormals(primitive);
            return primitive;
        }

        //private float Function(Vector3 position)
        //{
        //    return 1;
            //const float minY = 0.1f;
            //const float maxY = 0.6f;
            //const float xLimit = 0.4f;
            //Vector2 vec = new Vector2(position.X, position.Z);
            //float x = vec.Length();
            //if (x < xLimit)
            //    return minY;
            //x -= xLimit;
            //return (maxY - minY) * (float)Math.Sin(x * Math.PI / 2) + minY;
        //}
    }
}
