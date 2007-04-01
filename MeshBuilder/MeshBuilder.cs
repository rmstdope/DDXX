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
        private IPrimitiveFactory primitiveFactory;
        private IDevice device;
        private Dictionary<string, IPrimitive> primitives = new Dictionary<string, IPrimitive>();
        private Dictionary<string, ModelMaterial> materials = new Dictionary<string, ModelMaterial>();

        public MeshBuilder(IGraphicsFactory graphicsFactory, ITextureFactory textureFactory, 
            IDevice device, IPrimitiveFactory primitiveFactory)
        {
            this.graphicsFactory = graphicsFactory;
            this.textureFactory = textureFactory;
            this.primitiveFactory = primitiveFactory;
            this.device = device;
            materials["Default1"] = new ModelMaterial(new Material());
            materials["Default2"] = new ModelMaterial(new Material());
            materials["Default3"] = new ModelMaterial(new Material());
            materials["Default4"] = new ModelMaterial(new Material());
            materials["Default5"] = new ModelMaterial(new Material());
            materials["Default6"] = new ModelMaterial(new Material());
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
    }
}
