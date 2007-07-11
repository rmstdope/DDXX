using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using Dope.DDXX.Physics;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.MeshBuilder
{
    public class MeshBuilder
    {
        private IGraphicsFactory graphicsFactory;
        private ITextureFactory textureFactory;
        private IDevice device;
        private Dictionary<string, ModelMaterial> materials = new Dictionary<string, ModelMaterial>();

        public MeshBuilder(IGraphicsFactory graphicsFactory, ITextureFactory textureFactory, 
            IDevice device)
        {
            this.graphicsFactory = graphicsFactory;
            this.textureFactory = textureFactory;
            this.device = device;
            for (int i = 0; i < 6; i++)
            {
                materials["Default" + (i + 1)] = new ModelMaterial(new Material());
                materials["Default" + (i + 1)].DiffuseColor = new ColorValue(0.6f, 0.6f, 0.6f, 0.6f);
                materials["Default" + (i + 1)].AmbientColor = new ColorValue(0.3f, 0.3f, 0.3f, 0.3f);
                materials["Default" + (i + 1)].SpecularColor = new ColorValue(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        public ModelMaterial GetMaterial(string name)
        {
            if (materials.ContainsKey(name))
                return materials[name];
            else
                throw new DDXXException("Can not find material " + name);
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

        public IModel CreateModel(IModifier primitive, string material)
        {
            IModel model;
            Primitive outputPrimitive = primitive.Generate();
            IMesh mesh = CreateMesh(graphicsFactory, device, outputPrimitive.Vertices, outputPrimitive.Indices);
            ModelMaterial modelMaterial = null;
            if (material != "")
                modelMaterial = GetMaterial(material).Clone();
            if (modelMaterial == null)
                modelMaterial = new ModelMaterial(new Material());
            if (outputPrimitive.Body == null)
                model = new Model(mesh, new ModelMaterial[] { modelMaterial });
            else
                model = new PhysicalModel(mesh, outputPrimitive.Body, new ModelMaterial[] { modelMaterial });
            return model;
        }

        private IMesh CreateMesh(IGraphicsFactory factory, IDevice device, Vertex[] vertices, short[] indices)
        {
            bool useTexCoords = false;
            foreach (Vertex vertex in vertices)
            {
                if (vertex.TextureCoordinatesUsed)
                    useTexCoords = true;
            }
            VertexElementArray declaration = new VertexElementArray();
            declaration.AddPositions();
            declaration.AddNormals();
            if (useTexCoords)
                declaration.AddTexCoords(0, 2);
            IMesh mesh = factory.CreateMesh(indices.Length / 3, vertices.Length, MeshFlags.Managed,
                declaration.VertexElements, device);
            using (IGraphicsStream stream = mesh.LockVertexBuffer(LockFlags.None))
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    stream.Write(vertices[i].Position);
                    stream.Write(vertices[i].Normal);
                    if (useTexCoords)
                    {
                        stream.Write(vertices[i].U);
                        stream.Write(vertices[i].V);
                    }
                }
                mesh.UnlockVertexBuffer();
            }
            using (IGraphicsStream stream = mesh.LockIndexBuffer(LockFlags.None))
            {
                stream.Write(indices);
                mesh.UnlockIndexBuffer();
            }
            return mesh;
        }

        public void SetDiffuseColor(string materialName, ColorValue colorValue)
        {
            ModelMaterial material = GetMaterial(materialName);
            material.DiffuseColor = colorValue;
        }

        public void SetAmbientColor(string materialName, ColorValue colorValue)
        {
            ModelMaterial material = GetMaterial(materialName);
            material.AmbientColor = colorValue;
        }

        public void SetSpecularColor(string materialName, ColorValue colorValue)
        {
            ModelMaterial material = GetMaterial(materialName);
            material.SpecularColor = colorValue;
        }

    }
}
