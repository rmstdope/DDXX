using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using Dope.DDXX.Physics;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.MeshBuilder
{
    public class MeshBuilder
    {
        private IGraphicsFactory graphicsFactory;
        private ITextureFactory textureFactory;
        private IPrimitiveFactory primitiveFactory = new PrimitiveFactory();
        private IDevice device;
        private Dictionary<string, IPrimitive> primitives = new Dictionary<string, IPrimitive>();
        private Dictionary<string, ModelMaterial> materials = new Dictionary<string, ModelMaterial>();

        public MeshBuilder(IGraphicsFactory graphicsFactory, ITextureFactory textureFactory, 
            IDevice device)
        {
            this.graphicsFactory = graphicsFactory;
            this.textureFactory = textureFactory;
            this.device = device;
            materials["Default1"] = new ModelMaterial(new Material());
            materials["Default2"] = new ModelMaterial(new Material());
            materials["Default3"] = new ModelMaterial(new Material());
            materials["Default4"] = new ModelMaterial(new Material());
            materials["Default5"] = new ModelMaterial(new Material());
            materials["Default6"] = new ModelMaterial(new Material());
        }

        internal IPrimitiveFactory PrimitiveFactory
        {
            set { primitiveFactory = value; }
        }

        internal IPrimitive GetPrimitive(string name)
        {
            if (primitives.ContainsKey(name))
                return primitives[name];
            else
                throw new DDXXException("Can not find primitive " + name);
        }

        internal ModelMaterial GetMaterial(string name)
        {
            if (materials.ContainsKey(name))
                return materials[name];
            else
                throw new DDXXException("Can not find material " + name);
        }

        internal void AddPrimitive(IPrimitive primitive, string name)
        {
            if (primitives.ContainsKey(name))
                throw new DDXXException("Can not add the two primitives with the same name.");
            primitives[name] = primitive;
        }

        public IModel CreateModel(string name)
        {
            if (!primitives.ContainsKey(name))
                throw new DDXXException("Can not create mesh from a primitive that does not exist.");
            return primitives[name].CreateModel(graphicsFactory, device);
        }

        public void CreateBox(string name, float length, float width, float height,
            int lengthSegments, int widthSegments, int heightSegments)
        {
            IPrimitive box = primitiveFactory.CreateBox(length, width, height, 
                lengthSegments, widthSegments, heightSegments);
            AddPrimitive(box, name);
        }

        public void CreatePlane(string name, float width, float height,
            int widthSegments, int heightSegments, bool textured)
        {
            IPrimitive plane = primitiveFactory.CreatePlane(width, height, 
                widthSegments, heightSegments, textured);
            AddPrimitive(plane, name);
        }

        public void CreateCloth(string name, IBody body, float width, float height,
           int widthSegments, int heightSegments, int[] pinnedParticles, bool textured)
        {
            IPrimitive plane = primitiveFactory.CreateCloth(body, width, height, 
                widthSegments, heightSegments, pinnedParticles, textured);
            AddPrimitive(plane, name);
        }

        public void CreateCloth(string name, IBody body, float width, float height,
            int widthSegments, int heightSegments, bool textured)
        {
            IPrimitive plane = primitiveFactory.CreateCloth(body, width, height, 
                widthSegments, heightSegments, textured);
            AddPrimitive(plane, name);
        }

        public void CreateSphere(string name, float radius, short rings)
        {
            IPrimitive sphere = primitiveFactory.CreateSphere2(radius, rings);
            AddPrimitive(sphere, name);
        }

        public void AssignMaterial(string primitiveName, string materialName)
        {
            IPrimitive primitive = GetPrimitive(primitiveName);
            ModelMaterial material = GetMaterial(materialName);
            primitive.ModelMaterial = material;
        }

        /// <summary>
        /// Set the diffuse texture of a material.
        /// </summary>
        /// <param name="materialName">The name of the material.</param>
        /// <param name="fileName">The name of the texture file. Set to null or "" to remove the texture.</param>
        public void SetDiffuseTexture(string materialName, string fileName)
        {
            ModelMaterial material = GetMaterial(materialName);
            if (fileName == null || fileName == "")
                material.DiffuseTexture = null;
            else
                material.DiffuseTexture = textureFactory.CreateFromFile(fileName);
        }

        /// <summary>
        /// Set the normal texture of a material.
        /// </summary>
        /// <param name="materialName">The name of the material.</param>
        /// <param name="fileName">The name of the texture file. Set to null or "" to remove the texture.</param>
        public void SetNormalTexture(string materialName, string fileName)
        {
            ModelMaterial material = GetMaterial(materialName);
            if (fileName == null || fileName == "")
                material.NormalTexture = null;
            else
                material.NormalTexture = textureFactory.CreateFromFile(fileName);
        }

        /// <summary>
        /// Set the Reflective texture of a material.
        /// </summary>
        /// <param name="materialName">The name of the material.</param>
        /// <param name="fileName">The name of the texture file. Set to null or "" to remove the texture.</param>
        public void SetReflectiveTexture(string materialName, string fileName)
        {
            ModelMaterial material = GetMaterial(materialName);
            if (fileName == null || fileName == "")
                material.ReflectiveTexture = null;
            else
                material.ReflectiveTexture = textureFactory.CreateCubeFromFile(fileName);
        }

        public void SetReflectiveFactor(string materialName, float factor)
        {
            ModelMaterial material = GetMaterial(materialName);
            material.ReflectiveFactor = factor;
        }

        /// <summary>
        /// Create a sky box model, ring.e. four transformed and screen
        /// aligned vertices and no texture coordinates.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="textureName"></param>
        /// <returns></returns>
        public IModel CreateSkyBoxModel(string name, string textureName)
        {
            Vector3[] vertices = new Vector3[4];
            float fHighW = -1.0f - (1.0f / device.Viewport.Width);
            float fHighH = -1.0f - (1.0f / device.Viewport.Height);
            float fLowW = 1.0f + (1.0f / device.Viewport.Width);
            float fLowH = 1.0f + (1.0f / device.Viewport.Height);
            vertices[0] = new Vector3(fLowW, fLowH, 1.0f);
            vertices[1] = new Vector3(fLowW, fHighH, 1.0f);
            vertices[2] = new Vector3(fHighW, fLowH, 1.0f);
            vertices[3] = new Vector3(fHighW, fHighH, 1.0f);
            short[] indices = new short[] { 0, 1, 2, 3, 2, 1 };
            
            VertexElementArray declaration = new VertexElementArray();
            declaration.AddPositions();

            IMesh mesh = graphicsFactory.CreateMesh(2, 4, MeshFlags.Managed, 
                declaration.VertexElements, device);
            mesh.SetVertexBufferData(vertices, LockFlags.None);
            mesh.SetIndexBufferData(indices, LockFlags.None);
            ModelMaterial material = new ModelMaterial(new Material());
            material.ReflectiveTexture = textureFactory.CreateCubeFromFile(textureName);
            IModel model = new Model(mesh, new ModelMaterial[] { material });
            return model;
        }

        public void Weld(string primitiveName, float distance)
        {
            IPrimitive primitive = GetPrimitive(primitiveName);
            primitive.Weld(distance);
        }
    }
}
