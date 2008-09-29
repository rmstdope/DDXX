using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NMock2.Actions;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class ModelBuilderTest : D3DMockTest
    {
        private ModelBuilder builder;
        private IModifier modifier;
        private IIndexBuffer indexBuffer;
        private IEffect cloneEffect;
        private IMaterialHandler material;
        private IPrimitive primitive;

        private Vertex[] vertices;
        private short[] indices;
        private int numFaces;
        private int numVertices;
        private bool useTextureCoordinates;

        [SetUp]
        public override void  SetUp()
        {
         	base.SetUp();

            numFaces = 1;
            numVertices = 1;
            vertices = new Vertex[numVertices];
            indices = new short[numFaces * 3];

            modifier = mockery.NewMock<IModifier>();
            indexBuffer = mockery.NewMock<IIndexBuffer>();
            cloneEffect = mockery.NewMock<IEffect>();
            material = mockery.NewMock<IMaterialHandler>();
            primitive = mockery.NewMock<IPrimitive>();

            Stub.On(modifier).Method("Generate").Will(Return.Value(primitive));
            Stub.On(primitive).GetProperty("Vertices").Will(Return.Value(vertices));
            Stub.On(primitive).GetProperty("Indices").Will(Return.Value(indices));
            Stub.On(effect).Method("Clone").With(device).Will(Return.Value(cloneEffect));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void Constructor()
        {
            // Setup
            IEffectParameter diffuseParameter = mockery.NewMock<IEffectParameter>();
            IEffectParameter ambientParameter = mockery.NewMock<IEffectParameter>();
            IEffectParameter specularParameter = mockery.NewMock<IEffectParameter>();
            IEffectParameter shininessParameter = mockery.NewMock<IEffectParameter>();
            IEffectParameter powerParameter = mockery.NewMock<IEffectParameter>();
            IEffectParameter reflectiveParameter = mockery.NewMock<IEffectParameter>();
            IEffectParameter transparencyParameter = mockery.NewMock<IEffectParameter>();
            ICollectionAdapter<IEffectParameter> parameterCollection = mockery.NewMock<ICollectionAdapter<IEffectParameter>>();
            Stub.On(cloneEffect).GetProperty("Parameters").Will(Return.Value(parameterCollection));
            Stub.On(parameterCollection).Method("get_Item").With("DiffuseColor").
                Will(Return.Value(diffuseParameter));
            Stub.On(parameterCollection).Method("get_Item").With("AmbientColor").
                Will(Return.Value(ambientParameter));
            Stub.On(parameterCollection).Method("get_Item").With("SpecularColor").
                Will(Return.Value(specularParameter));
            Stub.On(parameterCollection).Method("get_Item").With("SpecularPower").
                Will(Return.Value(powerParameter));
            Stub.On(parameterCollection).Method("get_Item").With("Shininess").
                Will(Return.Value(shininessParameter));
            Stub.On(parameterCollection).Method("get_Item").With("ReflectiveFactor").
                Will(Return.Value(reflectiveParameter));
            Stub.On(parameterCollection).Method("get_Item").With("Transparency").
                Will(Return.Value(transparencyParameter));
            Expect.Once.On(diffuseParameter).Method("SetValue").With(new Color(200, 200, 200).ToVector3());
            Expect.Once.On(ambientParameter).Method("SetValue").With(new Color(100, 100, 100).ToVector3());
            Expect.Once.On(specularParameter).Method("SetValue").With(new Color(255, 255, 255).ToVector3());
            Expect.Once.On(powerParameter).Method("SetValue").With(32.0f);
            Expect.Once.On(shininessParameter).Method("SetValue").With(1.0f);
            Expect.Once.On(reflectiveParameter).Method("SetValue").With(0.0f);
            Expect.Once.On(transparencyParameter).Method("SetValue").With(0.0f);
            // Exercise SUT
            builder = new ModelBuilder(graphicsFactory, textureFactory, effectFactory, effect);
        }

        [Test]
        public void Material()
        {
            // Setup
            Constructor();
            Assert.IsInstanceOfType(typeof(MaterialHandler), builder.GetMaterial("Default"));
            // Exercise SUT
            builder.SetMaterial("Default", material);
            // Verify
            Assert.IsNotInstanceOfType(typeof(MaterialHandler), builder.GetMaterial("Default"));
        }

        [Test]
        public void SetDiffuseTextureFromString()
        {
            // Setup
            Material();
            Expect.Once.On(textureFactory).Method("CreateFromName").
                With("DiffuseTexture").Will(Return.Value(texture2D));
            Expect.Once.On(material).SetProperty("DiffuseTexture").To(texture2D);
            // Exercise SUT
            builder.SetDiffuseTexture("Default", "DiffuseTexture");
        }

        [Test]
        public void SetDiffuseTextureFromReference()
        {
            // Setup
            Material();
            Expect.Once.On(material).SetProperty("DiffuseTexture").To(texture2D);
            // Exercise SUT
            builder.SetDiffuseTexture("Default", texture2D);
        }

        [Test]
        public void DiffuseTextureRemove1()
        {
            // Setup
            SetDiffuseTextureFromString();
            Expect.Once.On(material).SetProperty("DiffuseTexture").To(Is.Null);
            // Exercise SUT
            builder.SetDiffuseTexture("Default", (string)null);
        }

        [Test]
        public void DiffuseTextureRemove2()
        {
            // Setup
            SetDiffuseTextureFromString();
            Expect.Once.On(material).SetProperty("DiffuseTexture").To(Is.Null);
            // Exercise SUT
            builder.SetDiffuseTexture("Default", "");
        }

        [Test]
        public void SetNormalTexture()
        {
            // Setup
            Material();
            Expect.Once.On(textureFactory).Method("CreateFromName").
                With("NormalTexture").Will(Return.Value(texture2D));
            Expect.Once.On(material).SetProperty("NormalTexture").To(texture2D);
            // Exercise SUT
            builder.SetNormalTexture("Default", "NormalTexture");
        }

        [Test]
        public void SetNormalTextureFromReference()
        {
            // Setup
            Material();
            Expect.Once.On(material).SetProperty("NormalTexture").To(texture2D);
            // Exercise SUT
            builder.SetNormalTexture("Default", texture2D);
        }

        [Test]
        public void NormalTextureRemove1()
        {
            // Setup
            SetNormalTexture();
            Expect.Once.On(material).SetProperty("NormalTexture").To(Is.Null);
            // Exercise SUT
            builder.SetNormalTexture("Default", (string)null);
        }

        [Test]
        public void NormalTextureRemove2()
        {
            // Setup
            SetNormalTexture();
            Expect.Once.On(material).SetProperty("NormalTexture").To(Is.Null);
            // Exercise SUT
            builder.SetNormalTexture("Default", "");
        }

        [Test]
        public void ReflectiveTexture()
        {
            // Setup
            Material();
            Expect.Once.On(textureFactory).Method("CreateCubeFromFile").
                With("ReflectiveTexture").Will(Return.Value(textureCube));
            Expect.Once.On(material).SetProperty("ReflectiveTexture").To(textureCube);
            // Exercise SUT
            builder.SetReflectiveTexture("Default", "ReflectiveTexture");
        }

        [Test]
        public void TestReflectiveTextureRemove1()
        {
            // Setup
            ReflectiveTexture();
            Expect.Once.On(material).SetProperty("ReflectiveTexture").To(Is.Null);
            // Exercise SUT
            builder.SetReflectiveTexture("Default", null);
        }

        [Test]
        public void TestReflectiveTextureRemove2()
        {
            // Setup
            ReflectiveTexture();
            Expect.Once.On(material).SetProperty("ReflectiveTexture").To(Is.Null);
            // Exercise SUT
            builder.SetReflectiveTexture("Default", "");
        }

        //[Test]
        //public void TestCreateSkyBox()
        //{
        //    numFaces = 2;
        //    numVertices = 4;
        //    viewport.Width = 100;
        //    viewport.Height = 200;
        //    fileName = "SkyBoxTexture";
        //    IModel model = builder.CreateSkyBoxModel("SkyBoxName", fileName);
        //    Assert.AreSame(this, model.Mesh, "This instance should be returned as Mesh.");
        //    Assert.AreEqual(1, model.Materials.Length, "We should have one material."); 
        //    Assert.AreSame(this, model.Materials[0].ReflectiveTexture, 
        //        "This instance should be returned as reflective texture.");
        //    Assert.IsNull(model.Materials[0].DiffuseTexture, "Diffuse texture should be null.");
        //    Assert.IsNull(model.Materials[0].NormalTexture, "Normal texture should be null.");
        //}

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void CreateWithNoMaterial()
        {
            // Setup
            Material();
            // Exercise SUT
            IModel model = builder.CreateModel(modifier, "Invalid");
        }

        [Test]
        public void CreateModelWithTexCoords()
        {
            // Setup
            Material();
            useTextureCoordinates = true;
            CreatePrimitive();
            Expect.Once.On(primitive).Method("Calculate");
            Expect.Once.On(material).GetProperty("Effect").Will(Return.Value(cloneEffect));
            Expect.Once.On(graphicsFactory).Method("CreateIndexBuffer").
                With(typeof(short), numFaces * 3, BufferUsage.None).
                Will(Return.Value(indexBuffer));
            Expect.Once.On(graphicsFactory).Method("CreateVertexBuffer").
                With(typeof(VertexPositionTangentTexture), numVertices, BufferUsage.None).
                Will(Return.Value(vertexBuffer));
            Expect.Once.On(graphicsFactory).Method("CreateVertexDeclaration").
                With(VertexPositionTangentTexture.VertexElements).
                Will(Return.Value(vertexDeclaration));
            Expect.Once.On(vertexBuffer).Method("SetData").With(new VertexPositionTangentTexture[] {
                new VertexPositionTangentTexture(vertices[0].Position, vertices[0].Normal, vertices[0].Tangent, vertices[0].BiNormal, new Vector2(vertices[0].U, vertices[0].V))
            });
            Expect.Once.On(indexBuffer).Method("SetData").With(indices);
            // Exercise SUT
            IModel model = builder.CreateModel(modifier, "Default");
            // Verify
            Assert.AreEqual(1, model.Meshes.Count);
            Assert.AreEqual(vertexBuffer, model.Meshes[0].VertexBuffer);
            Assert.AreEqual(indexBuffer, model.Meshes[0].IndexBuffer);
            Assert.AreEqual(1, model.Meshes[0].MeshParts.Count);
            //Assert.AreEqual(vertexBuffer, model.Meshes[0].MeshParts[0].);
        }

        [Test]
        public void TestCreateModel()
        {
            // Setup
            Material();
            useTextureCoordinates = false;
            CreatePrimitive();
            Expect.Once.On(primitive).Method("Calculate");
            Expect.Once.On(material).GetProperty("Effect").Will(Return.Value(cloneEffect));
            Expect.Once.On(graphicsFactory).Method("CreateIndexBuffer").
                With(typeof(short), numFaces * 3, BufferUsage.None).
                Will(Return.Value(indexBuffer));
            Expect.Once.On(graphicsFactory).Method("CreateVertexBuffer").
                With(typeof(VertexPositionNormal), numVertices, BufferUsage.None).
                Will(Return.Value(vertexBuffer));
            Expect.Once.On(graphicsFactory).Method("CreateVertexDeclaration").
                With(VertexPositionNormal.VertexElements).
                Will(Return.Value(vertexDeclaration));
            Expect.Once.On(vertexBuffer).Method("SetData").With(new VertexPositionNormal[] {
                new VertexPositionNormal(vertices[0].Position, vertices[0].Normal)
            });
            Expect.Once.On(indexBuffer).Method("SetData").With(indices);
            // Exercise SUT
            IModel model = builder.CreateModel(modifier, "Default");
            // Verify
            Assert.AreEqual(1, model.Meshes.Count);
            Assert.AreEqual(vertexBuffer, model.Meshes[0].VertexBuffer);
            Assert.AreEqual(indexBuffer, model.Meshes[0].IndexBuffer);
            Assert.AreEqual(1, model.Meshes[0].MeshParts.Count);
        }

        //[Test]
        //public void TestCreatePhysicalModel()
        //{
        //    CreatePrimitive();
        //    body = true;
        //    IModel model = builder.CreateModel(this, "");
        //    CheckModel(model);
        //    Assert.IsInstanceOfType(typeof(PhysicalModel), model, "Model shall be PhysicalModel");
        //}

        [Test]
        public void SetDiffuseColor()
        {
            // Setup
            Material();
            Expect.Once.On(material).SetProperty("DiffuseColor").To(new Color(1, 2, 3, 4));
            // Exercise SUT
            builder.SetDiffuseColor("Default", new Color(1, 2, 3, 4));
        }

        [Test]
        public void SetAmbientColor()
        {
            // Setup
            Material();
            Expect.Once.On(material).SetProperty("AmbientColor").To(new Color(5, 6, 7, 8));
            // Exercise SUT
            builder.SetAmbientColor("Default", new Color(5, 6, 7, 8));
        }

        [Test]
        public void SetSpecularColor()
        {
            // Setup
            Material();
            Expect.Once.On(material).SetProperty("SpecularColor").To(new Color(3, 5, 7, 9));
            // Exercise SUT
            builder.SetSpecularColor("Default", new Color(3, 5, 7, 9));
        }

        [Test]
        public void SetSpecularPower()
        {
            // Setup
            Material();
            Expect.Once.On(material).SetProperty("SpecularPower").To(1.1f);
            // Exercise SUT
            builder.SetSpecularPower("Default", 1.1f);
        }

        [Test]
        public void SetShininess()
        {
            // Setup
            Material();
            Expect.Once.On(material).SetProperty("Shininess").To(1.1f);
            // Exercise SUT
            builder.SetShininess("Default", 1.1f);
        }

        [Test]
        public void SetReflectiveFactor()
        {
            // Setup
            Material();
            Expect.Once.On(material).SetProperty("ReflectiveFactor").To(1.1f);
            // Exercise SUT
            builder.SetReflectiveFactor("Default", 1.1f);
        }

        [Test]
        public void SetTransparency()
        {
            // Setup
            Material();
            Expect.Once.On(material).SetProperty("Transparency").To(1.1f);
            // Exercise SUT
            builder.SetTransparency("Default", 1.1f);
        }

        [Test]
        public void TestEffect()
        {
            // Setup
            IEffect newEffect = mockery.NewMock<IEffect>();
            Material();
            Expect.Once.On(effectFactory).Method("CreateFromFile").
                With("TheEffect").Will(Return.Value(newEffect));
            Expect.Once.On(material).SetProperty("Effect").To(newEffect);
            // Exercise SUT
            builder.SetEffect("Default", "TheEffect");
        }

        private void CreatePrimitive()
        {
            for (int i = 0; i < numVertices; i++)
            {
                vertices[i] = new Vertex();
                vertices[i].Position = new Vector3(i, i + 1, i + 2);
                vertices[i].Normal = new Vector3(i + 2, i + 3, i + 4);
                vertices[i].Tangent = new Vector3(i + 3, i + 4, i + 5);
                vertices[i].BiNormal = new Vector3(i + 4, i + 5, i + 6);
                if (useTextureCoordinates)
                {
                    vertices[i].U = i * 2;
                    vertices[i].V = i * 1;
                }
            }
            for (int i = 0; i < numFaces * 3; i++)
                indices[i] = (short)i;
        }

    }
}
