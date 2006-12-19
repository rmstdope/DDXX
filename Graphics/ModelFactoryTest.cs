using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class ModelFactoryTest
    {
        ModelFactory modelFactory;
        TextureFactory textureFactory;

        Mockery mockery;
        IGraphicsFactory graphicsFactory;
        IMesh mesh;

        VertexElement[] pntbxDeclaration = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
            new VertexElement(0, 24, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            new VertexElement(0, 32, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
            new VertexElement(0, 44, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.BiNormal, 0),
            VertexElement.VertexDeclarationEnd,
        };
        VertexElement[] pntxDeclaration = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
            new VertexElement(0, 24, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            new VertexElement(0, 32, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
            VertexElement.VertexDeclarationEnd,
        };
        VertexElement[] pnxDeclaration = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
            new VertexElement(0, 24, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            VertexElement.VertexDeclarationEnd,
        };
        VertexElement[] pnDeclaration = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
            VertexElement.VertexDeclarationEnd,
        };
        VertexElement[] pxDeclaration = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            VertexElement.VertexDeclarationEnd,
        };
        VertexElement[] pDeclaration = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            VertexElement.VertexDeclarationEnd,
        };

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            mesh = mockery.NewMock<IMesh>();
            textureFactory = new TextureFactory(null, graphicsFactory, new PresentParameters());
            modelFactory = new ModelFactory(null, graphicsFactory, textureFactory);
            Stub.On(mesh).
                GetProperty("NumberFaces").
                Will(Return.Value(1));
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        internal class SetMaterial : IAction
        {
            private IMesh mesh;

            public SetMaterial(IMesh mesh)
            {
                this.mesh = mesh;
            }

            #region IInvokable Members
            public void Invoke(NMock2.Monitoring.Invocation invocation)
            {
                ExtendedMaterial[] materials = new ExtendedMaterial[2];
                Material material = new Material();
                invocation.Result = mesh;
                material.AmbientColor = new ColorValue(1, 1, 1, 1);
                material.DiffuseColor = new ColorValue(2, 2, 2, 2);
                materials[0].Material3D = material;
                materials[1].Material3D = material;
                materials[0].TextureFilename = "TextureFileName";
                invocation.Parameters[2] = materials;
            }
            #endregion
            #region ISelfDescribing Members
            void  ISelfDescribing.DescribeTo(System.IO.TextWriter writer)
            {
                writer.Write("Setting Material.");
            }
            #endregion
        }

        internal class SetOutAdjaceny : IAction
        {
            private IMesh mesh;

            public SetOutAdjaceny(IMesh mesh)
            {
                this.mesh = mesh;
            }

            #region IInvokable Members
            public void Invoke(NMock2.Monitoring.Invocation invocation)
            {
                invocation.Result = mesh;
                invocation.Parameters[2] = null;
            }
            #endregion
            #region ISelfDescribing Members
            void ISelfDescribing.DescribeTo(System.IO.TextWriter writer)
            {
                writer.Write("Setting adjacencyOut.");
            }
            #endregion
        }

        [Test]
        public void CreateBoxTest()
        {
            Expect.Exactly(2).On(graphicsFactory).
                Method("CreateBoxMesh").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            IModel mesh1 = modelFactory.CreateBox(10.0f, 20.0f, 30.0f);
            IModel mesh2 = modelFactory.CreateBox(10.0f, 20.0f, 30.0f);
            IModel mesh3 = modelFactory.CreateBox(10.0f, 22.0f, 30.0f);

            Assert.IsNotNull(mesh1);
            Assert.IsNotNull(mesh3);
            Assert.AreSame(mesh1, mesh2);
            Assert.AreNotSame(mesh1, mesh3);
            Assert.AreNotSame(mesh2, mesh3);

            Assert.AreEqual(2, modelFactory.Count);
            Assert.AreEqual(2, modelFactory.CountBoxes);
        }

        [Test]
        public void FromFileTest()
        {
            ITexture texture = mockery.NewMock<ITexture>();
            Stub.On(mesh).
                GetProperty("Declaration").
                Will(Return.Value(pntxDeclaration));
            Expect.Once.On(graphicsFactory).
                Method("MeshFromFile").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            Expect.Once.On(graphicsFactory).
                Method("TextureFromFile").
                WithAnyArguments().
                Will(Return.Value(texture));
            IModel model1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.EnsureTangents | ModelFactory.Options.NoOptimization);
            Assert.AreEqual(2, model1.Materials.Length);
            Assert.AreEqual(model1.Materials[0].Ambient, model1.Materials[0].Diffuse);
            Assert.AreEqual(model1.Materials[1].Ambient, model1.Materials[1].Diffuse);
            Assert.AreEqual(texture, model1.Materials[0].DiffuseTexture);
            Assert.AreEqual(null, model1.Materials[1].DiffuseTexture);
            Assert.IsNotNull(model1);
        }

        [Test]
        public void FromFileNoTangentsTest()
        {
            Stub.On(mesh).
                GetProperty("Declaration").
                Will(Return.Value(pnxDeclaration));
            Expect.Once.On(graphicsFactory).
                Method("MeshFromFile").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            Expect.Once.On(graphicsFactory).
                Method("TextureFromFile").
                WithAnyArguments().
                Will(Return.Value(null));
            Expect.Once.On(mesh).
                Method("Clone").
                With(MeshFlags.Managed, pntbxDeclaration, null).
                Will(Return.Value(mesh));
            Expect.Once.On(mesh).
                Method("Dispose");
            Expect.Once.On(mesh).
                Method("GenerateAdjacency").
                WithAnyArguments();
            Expect.Once.On(mesh).
                Method("Clean").
                With(Is.EqualTo(CleanType.BowTies | CleanType.BackFacing), Is.Anything, Is.Anything).
                Will(new SetOutAdjaceny(mesh));
            Expect.Once.On(mesh).
                Method("ComputeTangent").
                With(0, 0, 0, 0);
            IModel mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.EnsureTangents | ModelFactory.Options.NoOptimization);
            Assert.IsNotNull(mesh1);
        }

        [Test]
        public void FromFileNoNormalsTest1()
        {
            Stub.On(mesh).
                GetProperty("Declaration").
                Will(Return.Value(pxDeclaration));
            Expect.Once.On(graphicsFactory).
                Method("MeshFromFile").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            Expect.Once.On(graphicsFactory).
                Method("TextureFromFile").
                WithAnyArguments().
                Will(Return.Value(null));
            //FIXME: Do the real test!
            Expect.Once.On(mesh).
                Method("Clone").
                WithAnyArguments().//(MeshFlags.Managed, pnxDeclaration, null).
                Will(Return.Value(mesh));
            Expect.Once.On(mesh).
                Method("Dispose");
            Expect.Once.On(mesh).
                Method("ComputeNormals");
            IModel mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.NoOptimization);
            Assert.IsNotNull(mesh1);
        }

        [Test]
        public void FromFileNoNormalsTest2()
        {
            Stub.On(mesh).
                GetProperty("Declaration").
                Will(Return.Value(pxDeclaration));
            Expect.Once.On(graphicsFactory).
                Method("MeshFromFile").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            Expect.Once.On(graphicsFactory).
                Method("TextureFromFile").
                WithAnyArguments().
                Will(Return.Value(null));
            Expect.Once.On(mesh).
                Method("Clone").
                With(MeshFlags.Managed, pntbxDeclaration, null).
                Will(Return.Value(mesh));
            Expect.Once.On(mesh).
                Method("Dispose");
            Expect.Once.On(mesh).
                Method("ComputeNormals");
            Expect.Once.On(mesh).
                Method("ComputeTangentFrame").
                With(TangentOptions.GenerateInPlace | TangentOptions.WeightEqual);
            IModel mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.EnsureTangents | ModelFactory.Options.NoOptimization);
            Assert.IsNotNull(mesh1);
        }

        [Test]
        public void FromFileNoNormalsNoTexTest1()
        {
            Stub.On(mesh).
                GetProperty("Declaration").
                Will(Return.Value(pDeclaration));
            Expect.Once.On(graphicsFactory).
                Method("MeshFromFile").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            Expect.Once.On(graphicsFactory).
                Method("TextureFromFile").
                WithAnyArguments().
                Will(Return.Value(null));
            Expect.Once.On(mesh).
                Method("Clone").
                With(MeshFlags.Managed, pnDeclaration, null).
                Will(Return.Value(mesh));
            Expect.Once.On(mesh).
                Method("Dispose");
            Expect.Once.On(mesh).
                Method("ComputeNormals");
            IModel mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.NoOptimization);
            Assert.IsNotNull(mesh1);
        }

        [Test]
        public void FromFileOptimizeTest()
        {
            Stub.On(mesh).
                GetProperty("Declaration").
                Will(Return.Value(pntxDeclaration));
            Expect.Once.On(graphicsFactory).
                Method("MeshFromFile").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            Expect.Once.On(graphicsFactory).
                Method("TextureFromFile").
                WithAnyArguments().
                Will(Return.Value(null));
            Expect.Once.On(mesh).
                Method("GenerateAdjacency").
                WithAnyArguments();
            Expect.Once.On(mesh).
                Method("OptimizeInPlace").
                WithAnyArguments();
            IModel mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.None);
            Assert.IsNotNull(mesh1);
        }

        [Test]
        public void MultipleFromFileTest()
        {
            Stub.On(mesh).
                GetProperty("Declaration").
                Will(Return.Value(pntxDeclaration));
            Expect.Exactly(2).On(graphicsFactory).
                Method("MeshFromFile").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            Expect.Once.On(graphicsFactory).
                Method("TextureFromFile").
                WithAnyArguments().
                Will(Return.Value(null));
            Expect.Exactly(2).On(mesh).
                Method("GenerateAdjacency").
                WithAnyArguments();
            Expect.Exactly(2).On(mesh).
                Method("OptimizeInPlace").
                WithAnyArguments();
            IModel mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.None);
            IModel mesh2 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.None);
            IModel mesh3 = modelFactory.FromFile("MeshFile2", ModelFactory.Options.None);

            Assert.IsNotNull(mesh1);
            Assert.IsNotNull(mesh3);
            Assert.AreSame(mesh1, mesh2);
            Assert.AreNotSame(mesh1, mesh3);
            Assert.AreNotSame(mesh2, mesh3);

            Assert.AreEqual(2, modelFactory.Count);
            Assert.AreEqual(2, modelFactory.CountFiles);
        }

        [Test]
        public void CreateFileOptionsTest()
        {
            Stub.On(mesh).
                GetProperty("Declaration").
                Will(Return.Value(pntxDeclaration));
            Expect.Exactly(2).On(graphicsFactory).
                Method("MeshFromFile").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            Expect.Once.On(graphicsFactory).
                Method("TextureFromFile").
                WithAnyArguments().
                Will(Return.Value(null));
            Expect.Once.On(mesh).
                Method("GenerateAdjacency").
                WithAnyArguments();
            Expect.Once.On(mesh).
                Method("OptimizeInPlace").
                WithAnyArguments();
            IModel mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.None);
            IModel mesh2 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.NoOptimization | ModelFactory.Options.EnsureTangents);
            IModel mesh3 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.NoOptimization | ModelFactory.Options.EnsureTangents);

            Assert.IsNotNull(mesh1);
            Assert.IsNotNull(mesh2);
            Assert.AreSame(mesh2, mesh3);
            Assert.AreNotSame(mesh1, mesh2);
            Assert.AreNotSame(mesh1, mesh3);

            Assert.AreEqual(2, modelFactory.Count);
            Assert.AreEqual(2, modelFactory.CountFiles);
        }
    }
}
