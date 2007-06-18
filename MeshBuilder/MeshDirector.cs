using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.MeshBuilder
{
    public class MeshDirector
    {
        private MeshBuilder builder;
        private IPrimitive primitive;

        public MeshDirector(MeshBuilder builder)
        {
            this.builder = builder;
        }

        public void CreateChamferBox(float width, float length, float height, float fillet, int filletSegments)
        {
            ChamferBoxPrimitive chamferBox = new ChamferBoxPrimitive();
            chamferBox.Width = width;
            chamferBox.Height = height;
            chamferBox.Length = length;
            chamferBox.Fillet = fillet;
            chamferBox.FilletSegments = filletSegments;
            primitive = chamferBox;
        }

        public void CreateDisc(float outerRadius, float innerRadius, int segments)
        {
            DiscPrimitive disc = new DiscPrimitive();
            disc.Radius = outerRadius;
            disc.InnerRadius = innerRadius;
            disc.Segments = segments;
            primitive = disc;
        }

        public void CreateTorus(float smallRadius, float largeRadius, int side, int segments)
        {
            TorusPrimitive torus = new TorusPrimitive();
            torus.LargeRadius = largeRadius;
            torus.SmallRadius = smallRadius;
            torus.Sides = side;
            torus.Segments = segments;
            primitive = torus;
        }

        public void UvMapPlane(int alignToAxis, int tileU, int tileV)
        {
            UvMapPlane map = new UvMapPlane();
            map.AlignToAxis = alignToAxis;
            map.Input = primitive;
            map.TileU = tileU;
            map.TileV = tileV;
            primitive = map;
        }

        /// <summary>
        /// Rotates around: 1)z-axis 2)x-axis 3)y-axis
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Rotate(float x, float y, float z)
        {
            Rotate rotate = new Rotate();
            rotate.X = x;
            rotate.Y = y;
            rotate.Z = z;
            rotate.Input = primitive;
            primitive = rotate;
        }

        public void Translate(float x, float y, float z)
        {
            Translate translate = new Translate();
            translate.X = x;
            translate.Y = y;
            translate.Z = z;
            translate.Input = primitive;
            primitive = translate;
        }

        public void Scale(float factor)
        {
            Scale(factor, factor, factor);
        }

        public void Scale(float x, float y, float z)
        {
            Scale scale = new Scale();
            scale.X = x;
            scale.Y = y;
            scale.Z = z;
            scale.Input = primitive;
            primitive = scale;
        }

        public IModel Generate(string materialName)
        {
            return builder.CreateModel(primitive, materialName);
        }

    }
}
