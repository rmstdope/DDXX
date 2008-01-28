using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.Physics;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class ModelBuilder
    {
        private IGraphicsDevice device;
        private IGraphicsFactory graphicsFactory;
        private ITextureFactory textureFactory;
        private IEffectFactory effectFactory;
        private Dictionary<string, IMaterialHandler> materials = new Dictionary<string, IMaterialHandler>();

        public ModelBuilder(IGraphicsDevice device, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory, IEffectFactory effectFactory, IEffect defaultEffect)
        {
            this.device = device;
            this.graphicsFactory = graphicsFactory;
            this.textureFactory = textureFactory;
            this.effectFactory = effectFactory;
            materials["Default"] = new MaterialHandler(defaultEffect.Clone(defaultEffect.GraphicsDevice), new EffectConverter());
            materials["Default"].DiffuseColor = new Color(200, 200, 200);
            materials["Default"].AmbientColor = new Color(100, 100, 100);
            materials["Default"].SpecularColor = new Color(255, 255, 255);
            materials["Default"].ReflectiveFactor = 0;
            materials["Default"].Shininess = 1.0f;
            materials["Default"].SpecularPower = 32;
        }

        public IMaterialHandler GetMaterial(string name)
        {
            if (materials.ContainsKey(name))
                return materials[name];
            else
                throw new DDXXException("Can not find material " + name);
        }

        public void SetDiffuseTexture(string materialName, string fileName)
        {
            IMaterialHandler material = GetMaterial(materialName);
            if (fileName == null || fileName == "")
                material.DiffuseTexture = null;
            else
                material.DiffuseTexture = textureFactory.CreateFromFile(fileName);
        }

        public void SetDiffuseTexture(string materialName, ITexture2D texture)
        {
            IMaterialHandler material = GetMaterial(materialName);
            material.DiffuseTexture = texture;
        }

        public void SetNormalTexture(string materialName, string fileName)
        {
            IMaterialHandler material = GetMaterial(materialName);
            if (fileName == null || fileName == "")
                material.NormalTexture = null;
            else
                material.NormalTexture = textureFactory.CreateFromFile(fileName);
        }

        public void SetNormalTexture(string materialName, ITexture2D texture)
        {
            IMaterialHandler material = GetMaterial(materialName);
            material.NormalTexture = texture;
        }

        public void SetReflectiveTexture(string materialName, string fileName)
        {
            IMaterialHandler material = GetMaterial(materialName);
            if (fileName == null || fileName == "")
                material.ReflectiveTexture = null;
            else
                material.ReflectiveTexture = textureFactory.CreateCubeFromFile(fileName);
        }

        public void SetReflectiveFactor(string materialName, float factor)
        {
            IMaterialHandler material = GetMaterial(materialName);
            material.ReflectiveFactor = factor;
        }

        //public IModel CreateSkyBoxModel(string name, string textureName)
        //{
        //    Vector3[] vertices = new Vector3[4];
        //    float fHighW = -1.0f - (1.0f / graphicsFactory.GraphicsDeviceManager.GraphicsDevice.Viewport.Width);
        //    float fHighH = -1.0f - (1.0f / graphicsFactory.GraphicsDeviceManager.GraphicsDevice.Viewport.Height);
        //    float fLowW = 1.0f + (1.0f / graphicsFactory.GraphicsDeviceManager.GraphicsDevice.Viewport.Width);
        //    float fLowH = 1.0f + (1.0f / graphicsFactory.GraphicsDeviceManager.GraphicsDevice.Viewport.Height);
        //    vertices[0] = new Vector3(fLowW, fLowH, 1.0f);
        //    vertices[1] = new Vector3(fLowW, fHighH, 1.0f);
        //    vertices[2] = new Vector3(fHighW, fLowH, 1.0f);
        //    vertices[3] = new Vector3(fHighW, fHighH, 1.0f);
        //    short[] indices = new short[] { 0, 1, 2, 3, 2, 1 };
            
        //    VertexElementArray declaration = new VertexElementArray();
        //    declaration.AddPositions();

        //    IMesh mesh = graphicsFactory.CreateMesh(2, 4, MeshFlags.Managed, 
        //        declaration.VertexElements, device);
        //    mesh.SetVertexBufferData(vertices, LockFlags.None);
        //    mesh.SetIndexBufferData(indices, LockFlags.None);
        //    ModelMaterial material = new ModelMaterial(new Material());
        //    material.ReflectiveTexture = textureFactory.CreateCubeFromFile(textureName);
        //    IModel model = new Model(mesh, new ModelMaterial[] { material });
        //    return model;
        //}
        public IModel CreateModel(IModifier generator, string material)
        {
            return CreateModel(generator, GetMaterial(material));
        }

        public IModel CreateModel(IModifier generator, IMaterialHandler modelMaterial)
        {
            IModel model;

            IPrimitive primitive = generator.Generate();
            primitive.Calculate();
            IModelMesh mesh = CreateModelMesh(graphicsFactory, primitive.Vertices, primitive.Indices, modelMaterial);
            //if (body == null)
                model = new CustomModel(mesh);
            //else
            //    model = new PhysicalModel(mesh, body, new ModelMaterial[] { modelMaterial });
            return model;
        }

        private IModelMesh CreateModelMesh(IGraphicsFactory factory, Vertex[] vertices, short[] indices, IMaterialHandler material)
        {
            IIndexBuffer indexBuffer = CreateIndexBuffer(indices);
            IVertexBuffer vertexBuffer = CreateVertexBuffer(vertices);
            int vertexSize = GetVertexSize(vertices);
            IVertexDeclaration vertexDeclaration = GetVertexDeclaration(vertices);
            FillVertexBuffer(vertexBuffer, vertices);
            FillIndexBuffer(indexBuffer, indices);

            IModelMeshPart part = new CustomModelMeshPart(material.Effect, 0, vertices.Length, 0, indices.Length / 3);
            IModelMesh mesh = new CustomModelMesh(device, vertexBuffer, indexBuffer, vertexSize, 
                vertexDeclaration, PrimitiveType.TriangleList, new IModelMeshPart[] { part });
            
            return mesh;
        }

        private void FillIndexBuffer(IIndexBuffer indexBuffer, short[] indices)
        {
            indexBuffer.SetData<short>(indices);
        }

        private void FillVertexBuffer(IVertexBuffer vertexBuffer, Vertex[] vertices)
        {
            if (TextureCoordinatesUsed(vertices))
            {
                VertexPositionTangentTexture[] newVertices = new VertexPositionTangentTexture[vertices.Length];
                for (int i = 0; i < vertices.Length; i++)
                    newVertices[i] = new VertexPositionTangentTexture(vertices[i].Position, vertices[i].Normal, vertices[i].Tangent, vertices[i].BiNormal, new Vector2(vertices[i].U, vertices[i].V));
                vertexBuffer.SetData<VertexPositionTangentTexture>(newVertices);
            }
            else
            {
                VertexPositionNormal[] newVertices = new VertexPositionNormal[vertices.Length];
                for (int i = 0; i < vertices.Length; i++)
                    newVertices[i] = new VertexPositionNormal(vertices[i].Position, vertices[i].Normal);
                vertexBuffer.SetData<VertexPositionNormal>(newVertices);
            }
        }

        private IVertexDeclaration GetVertexDeclaration(Vertex[] vertices)
        {
            if (TextureCoordinatesUsed(vertices))
                return graphicsFactory.CreateVertexDeclaration(VertexPositionTangentTexture.VertexElements);
            return graphicsFactory.CreateVertexDeclaration(VertexPositionNormal.VertexElements);
        }

        private bool TextureCoordinatesUsed(Vertex[] vertices)
        {
            bool useTexCoords = false;
            foreach (Vertex vertex in vertices)
            {
                if (vertex.TextureCoordinatesUsed)
                    useTexCoords = true;
            }
            return useTexCoords;
        }

        private int GetVertexSize(Vertex[] vertices)
        {
            if (TextureCoordinatesUsed(vertices))
                return VertexPositionTangentTexture.SizeInBytes;
            return VertexPositionNormal.SizeInBytes;
        }

        private IVertexBuffer CreateVertexBuffer(Vertex[] vertices)
        {
            if (TextureCoordinatesUsed(vertices))
                return graphicsFactory.CreateVertexBuffer(typeof(VertexPositionTangentTexture), vertices.Length, BufferUsage.None);
            return graphicsFactory.CreateVertexBuffer(typeof(VertexPositionNormal), vertices.Length, BufferUsage.None);
        }

        private IIndexBuffer CreateIndexBuffer(short[] indices)
        {
            IIndexBuffer indexBuffer = graphicsFactory.CreateIndexBuffer(typeof(short), indices.Length, BufferUsage.None);
            return indexBuffer;
        }

        public void SetDiffuseColor(string materialName, Color color)
        {
            IMaterialHandler material = GetMaterial(materialName);
            material.DiffuseColor = color;
        }

        public void SetAmbientColor(string materialName, Color color)
        {
            IMaterialHandler material = GetMaterial(materialName);
            material.AmbientColor = color;
        }

        public void SetSpecularColor(string materialName, Color color)
        {
            IMaterialHandler material = GetMaterial(materialName);
            material.SpecularColor = color;
        }

        public void SetSpecularPower(string materialName, float power)
        {
            IMaterialHandler material = GetMaterial(materialName);
            material.SpecularPower = power;
        }

        public void SetShininess(string materialName, float shininess)
        {
            IMaterialHandler material = GetMaterial(materialName);
            material.Shininess = shininess;
        }

        public void SetEffect(string materialName, string effectName)
        {
            IMaterialHandler material = GetMaterial(materialName);
            material.Effect = effectFactory.CreateFromFile(effectName);
        }


        public void SetMaterial(string materialName, IMaterialHandler material)
        {
            materials[materialName] = material;
        }

    }
}
