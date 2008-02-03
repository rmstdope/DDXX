using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.ModelBuilder
{
    public class ModelDirector
    {
        private ModelBuilder builder;
        private IModifier primitive;

        public ModelDirector(ModelBuilder builder)
        {
            this.builder = builder;
        }

        public void CreateBox(float width, float length, float height)
        {
            BoxPrimitive box = new BoxPrimitive();
            box.Width = width;
            box.Height = height;
            box.Length = length;
            primitive = box;
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

        public void CreateTube(float innerRadius, float outerRadius, float height, int segments, int heightSegments)
        {
            TubePrimitive tube = new TubePrimitive();
            tube.InnerRadius = innerRadius;
            tube.OuterRadius = outerRadius;
            tube.Height = height;
            tube.Segments = segments;
            tube.HeightSegments = heightSegments;
            primitive = tube;
        }

        public void CreateTerrain(ITextureGenerator generator, float heightScale, float width, float height, int widthSegments, int heightSegments, bool textured)
        {
            TerrainPrimitive terrain = new TerrainPrimitive();
            terrain.HeightMapGenerator = generator;
            terrain.Height = height;
            terrain.Width = width;
            terrain.WidthSegments = widthSegments;
            terrain.HeightSegments = heightSegments;
            terrain.HeightScale = heightScale;
            terrain.Textured = textured;
            primitive = terrain;
        }

        public void CreatePlane(float width, float height, int widthSegments, int heightSegments)
        {
            PlanePrimitive plane = new PlanePrimitive();
            plane.Width = width;
            plane.Height = height;
            plane.WidthSegments = widthSegments;
            plane.HeightSegments = heightSegments;
            plane.Textured = true;
            primitive = plane;
        }

        public void CreateSphere(float radius, int rings)
        {
            SpherePrimitive sphere = new SpherePrimitive();
            sphere.Radius = radius;
            sphere.Rings = rings;
            primitive = sphere;
        }

        public void CreateCylinder(float radius, int segments, float height, int heightSegments, bool lid, int wrapU, int wrapV)
        {
            CylinderPrimitive cylinder = new CylinderPrimitive();
            cylinder.Radius = radius;
            cylinder.Segments = segments;
            cylinder.Height = height;
            cylinder.HeightSegments = heightSegments;
            cylinder.Lid = lid;
            cylinder.WrapU = wrapU;
            cylinder.WrapV = wrapV;
            primitive = cylinder;
        }

        public void CreateTunnel(float radius, int segments, float height, int heightSegments, int wrapU, int wrapV)
        {
            TunnelPrimitive tunnel = new TunnelPrimitive();
            tunnel.Radius = radius;
            tunnel.Segments = segments;
            tunnel.Height = height;
            tunnel.HeightSegments = heightSegments;
            tunnel.WrapU = wrapU;
            tunnel.WrapV = wrapV;
            primitive = tunnel;
        }

        public void UvMapPlane(int alignToAxis, int tileU, int tileV)
        {
            UvMapPlane map = new UvMapPlane();
            map.AlignToAxis = alignToAxis;
            map.ConnectToInput(0, primitive);
            map.TileU = tileU;
            map.TileV = tileV;
            primitive = map;
        }

        public void UvMapBox()
        {
            UvMapBox map = new UvMapBox();
            map.ConnectToInput(0, primitive);
            primitive = map;
        }

        /// <summary>
        /// Rotates around: 1)z-axis 2)x-axis 3)y-axis
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Rotate(double x, double y, double z)
        {
            Rotate rotate = new Rotate();
            rotate.X = (float)x;
            rotate.Y = (float)y;
            rotate.Z = (float)z;
            rotate.ConnectToInput(0, primitive);
            primitive = rotate;
        }

        public void Translate(float x, float y, float z)
        {
            Translate translate = new Translate();
            translate.X = x;
            translate.Y = y;
            translate.Z = z;
            translate.ConnectToInput(0, primitive);
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
            scale.ConnectToInput(0, primitive);
            primitive = scale;
        }

        public void UvMapSphere()
        {
            UvMapSphere map = new UvMapSphere();
            map.ConnectToInput(0, primitive);
            primitive = map;
        }

        public void Amplitude(AmplitudeFunction function)
        {
            Amplitude amplitude = new Amplitude();
            amplitude.Function = function;
            amplitude.ConnectToInput(0, primitive);
            primitive = amplitude;
        }

        public void HeightMap(ITextureGenerator generator)
        {
            HeightMap heightMap = new HeightMap();
            heightMap.HeightMapGenerator = generator;
            heightMap.ConnectToInput(0, primitive);
            primitive = heightMap;
        }

        public IModel Generate(string materialName)
        {
            return builder.CreateModel(primitive, materialName);
        }

        public IModel Generate(IMaterialHandler material)
        {
            return builder.CreateModel(primitive, material);
        }

    }
}
