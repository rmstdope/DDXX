using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class Swirl : Generator
    {
        private float swirlPower;
        private float swirlFactor;

        public float SwirlFactor
        {
            get { return swirlFactor; }
            set { swirlFactor = value; }
        }

        public float SwirlPower
        {
            get { return swirlPower; }
            set { swirlPower = value; }
        }

        public Swirl()
            : base(1)
        {
            swirlPower = 5;
            swirlFactor = 10;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector2 coordinates = textureCoordinate - new Vector2(0.5f, 0.5f);
            float radius = coordinates.Length();
            float swirl = (float)(Math.Pow(Math.Max(0, 0.5f - radius), swirlPower) * swirlFactor);
            Vector2 newCoordinates = new Vector2((float)(coordinates.X * Math.Cos(swirl) + coordinates.Y * Math.Sin(swirl)),
                (float)(coordinates.Y * Math.Cos(swirl) - coordinates.X * Math.Sin(swirl)));
            //Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, 0);//swirl);
            //Vector2 newCoordinates = Vector2.Transform(coordinates, rotation);
            newCoordinates += new Vector2(0.5f, 0.5f);
            return GetInputPixel(0, newCoordinates, texelSize);
        }
    }
}
