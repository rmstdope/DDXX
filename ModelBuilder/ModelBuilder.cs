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
    public class ModelBuilder : IModelBuilder
    {
        private IGraphicsFactory graphicsFactory;
        private TextureFactory textureFactory;
        private EffectFactory effectFactory;
        private Dictionary<string, MaterialHandler> materials = new Dictionary<string, MaterialHandler>();
        private Effect defaultEffect;

        public TextureFactory TextureFactory { get { return textureFactory; } }
        public EffectFactory EffectFactory { get { return effectFactory; } }
        public GraphicsDevice GraphicsDevice { get { return textureFactory.GraphicsDevice; } }

        public ModelBuilder(IGraphicsFactory graphicsFactory, TextureFactory textureFactory, EffectFactory effectFactory, Effect defaultEffect)
        {
            this.defaultEffect = defaultEffect;
            this.graphicsFactory = graphicsFactory;
            this.textureFactory = textureFactory;
            this.effectFactory = effectFactory;
            CreateMaterial("Default");
        }

        public MaterialHandler GetMaterial(string name)
        {
            if (materials.ContainsKey(name))
                return materials[name];
            else
                throw new DDXXException("Can not find material " + name);
        }

        public void SetDiffuseTexture(string materialName, string fileName)
        {
            MaterialHandler material = GetMaterial(materialName);
            if (fileName == null || fileName == "")
                material.DiffuseTexture = null;
            else
                material.DiffuseTexture = textureFactory.CreateFromName(fileName);
        }

        public void SetDiffuseTexture(string materialName, Texture2D texture)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.DiffuseTexture = texture;
        }

        public void SetNormalTexture(string materialName, string fileName)
        {
            MaterialHandler material = GetMaterial(materialName);
            if (fileName == null || fileName == "")
                material.NormalTexture = null;
            else
                material.NormalTexture = textureFactory.CreateFromName(fileName);
        }

        public void SetNormalTexture(string materialName, Texture2D texture)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.NormalTexture = texture;
        }

        public void SetReflectiveTexture(string materialName, string fileName)
        {
            MaterialHandler material = GetMaterial(materialName);
            if (fileName == null || fileName == "")
                material.ReflectiveTexture = null;
            else
                material.ReflectiveTexture = textureFactory.CreateCubeFromFile(fileName);
        }

        public void SetReflectiveFactor(string materialName, float factor)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.ReflectiveFactor = factor;
        }

        public void SetTransparency(string materialName, float factor)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.Transparency = factor;
        }

        public CustomModel CreateModel(IModifier generator, string material)
        {
            return CreateModel(generator, GetMaterial(material));
        }

        public CustomModel CreateModel(IModifier generator, MaterialHandler modelMaterial)
        {
            CustomModel model;

            IPrimitive primitive = generator.Generate();
            primitive.Calculate();
            CustomModelMesh mesh = CreateModelMesh(graphicsFactory, primitive.Vertices, primitive.Indices, modelMaterial);
            //if (body == null)
                model = new CustomModel(mesh);
            //else
            //    model = new PhysicalModel(mesh, body, new ModelMaterial[] { modelMaterial });
            return model;
        }

        private CustomModelMesh CreateModelMesh(IGraphicsFactory factory, Vertex[] vertices, short[] indices, MaterialHandler material)
        {
            IndexBuffer indexBuffer = CreateIndexBuffer(indices);
            VertexBuffer vertexBuffer = CreateVertexBuffer(vertices);
            int vertexSize = GetVertexSize(vertices);
            FillVertexBuffer(vertexBuffer, vertices);
            FillIndexBuffer(indexBuffer, indices);

            CustomModelMeshPart part = new CustomModelMeshPart(vertexBuffer, 0, vertices.Length, indexBuffer, 0, indices.Length / 3, PrimitiveType.TriangleList, material);
            CustomModelMesh mesh = new CustomModelMesh(graphicsFactory.GraphicsDevice, new CustomModelMeshPart[] { part });
            
            return mesh;
        }

        private void FillIndexBuffer(IndexBuffer indexBuffer, short[] indices)
        {
            indexBuffer.SetData<short>(indices);
        }

        private void FillVertexBuffer(VertexBuffer vertexBuffer, Vertex[] vertices)
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

        private VertexBuffer CreateVertexBuffer(Vertex[] vertices)
        {
            if (TextureCoordinatesUsed(vertices))
                return new VertexBuffer(GraphicsDevice, new VertexDeclaration(VertexPositionTangentTexture.VertexElements), vertices.Length, BufferUsage.None);
            return new VertexBuffer(GraphicsDevice, new VertexDeclaration(VertexPositionNormal.VertexElements), vertices.Length, BufferUsage.None);
        }

        private IndexBuffer CreateIndexBuffer(short[] indices)
        {
            IndexBuffer indexBuffer = new IndexBuffer(GraphicsDevice, typeof(short), indices.Length, BufferUsage.None);
            return indexBuffer;
        }

        public void SetDiffuseColor(string materialName, Color color)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.DiffuseColor = color;
        }

        public void SetAmbientColor(string materialName, Color color)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.AmbientColor = color;
        }

        public void SetSpecularColor(string materialName, Color color)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.SpecularColor = color;
        }

        public void SetSpecularPower(string materialName, float power)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.SpecularPower = power;
        }

        public void SetShininess(string materialName, float shininess)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.Shininess = shininess;
        }

        public void SetEffect(string materialName, string effectName)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.Effect = effectFactory.CreateFromFile(effectName);
        }

        public void SetBlendMode(string materialName, BlendFunction blendFunction, Blend sourceBlend, Blend destinationBlend)
        {
            MaterialHandler material = GetMaterial(materialName);
            material.BlendState.ColorBlendFunction = blendFunction;
            material.BlendState.ColorSourceBlend = sourceBlend;
            material.BlendState.ColorDestinationBlend = destinationBlend;
        }

        public void SetMaterial(string materialName, MaterialHandler material)
        {
            materials[materialName] = material;
        }

        public void CreateMaterial(string materialName)
        {
            if (!materials.ContainsKey(materialName))
            {
                materials[materialName] = new MaterialHandler(defaultEffect.Clone(), new EffectConverter());
                materials[materialName].DiffuseColor = new Color(200, 200, 200);
                materials[materialName].SpecularColor = new Color(255, 255, 255);
                materials[materialName].SpecularPower = 32;
                if (!(defaultEffect is BasicEffect))
                {
                    materials[materialName].AmbientColor = new Color(100, 100, 100);
                    materials[materialName].ReflectiveFactor = 0;
                    materials[materialName].Shininess = 1.0f;
                    materials[materialName].Transparency = 0.0f;
                }
            }
        }
        
    }
}
