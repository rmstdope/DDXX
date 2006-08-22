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

        Mockery mockery;
        IGraphicsFactory graphicsFactory;
        IMesh mesh;

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
            modelFactory = new ModelFactory(null, graphicsFactory);
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
                invocation.Result = mesh;
                invocation.Parameters[2] = new ExtendedMaterial[1];
            }
            #endregion
            #region ISelfDescribing Members
            void  ISelfDescribing.DescribeTo(System.IO.TextWriter writer)
            {
                writer.Write("Setting Material.");
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
            Model mesh1 = modelFactory.CreateBox(10.0f, 20.0f, 30.0f);
            Model mesh2 = modelFactory.CreateBox(10.0f, 20.0f, 30.0f);
            Model mesh3 = modelFactory.CreateBox(10.0f, 22.0f, 30.0f);

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
            Stub.On(mesh).
                GetProperty("Declaration").
                Will(Return.Value(pntxDeclaration));
            Expect.Once.On(graphicsFactory).
                Method("MeshFromFile").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            Model mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.EnsureTangents | ModelFactory.Options.NoOptimization);
            Assert.IsNotNull(mesh1);
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
            Expect.Once.On(mesh).
                Method("Clone").
                With(MeshFlags.Managed, pntxDeclaration, null).
                Will(Return.Value(mesh));
            Expect.Once.On(mesh).
                Method("Dispose");
            Expect.Once.On(mesh).
                Method("ComputeTangentFrame").
                With(TangentOptions.GenerateInPlace | TangentOptions.WeightEqual);
            Model mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.EnsureTangents | ModelFactory.Options.NoOptimization);
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
            //FIXME: Do the real test!
            Expect.Once.On(mesh).
                Method("Clone").
                WithAnyArguments().//(MeshFlags.Managed, pnxDeclaration, null).
                Will(Return.Value(mesh));
            Expect.Once.On(mesh).
                Method("Dispose");
            Expect.Once.On(mesh).
                Method("ComputeNormals");
            Model mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.NoOptimization);
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
            Expect.Once.On(mesh).
                Method("Clone").
                With(MeshFlags.Managed, pntxDeclaration, null).
                Will(Return.Value(mesh));
            Expect.Once.On(mesh).
                Method("Dispose");
            Expect.Once.On(mesh).
                Method("ComputeNormals");
            Expect.Once.On(mesh).
                Method("ComputeTangentFrame").
                With(TangentOptions.GenerateInPlace | TangentOptions.WeightEqual);
            Model mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.EnsureTangents | ModelFactory.Options.NoOptimization);
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
            Expect.Once.On(mesh).
                Method("Clone").
                With(MeshFlags.Managed, pnDeclaration, null).
                Will(Return.Value(mesh));
            Expect.Once.On(mesh).
                Method("Dispose");
            Expect.Once.On(mesh).
                Method("ComputeNormals");
            Model mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.NoOptimization);
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
            Expect.Once.On(mesh).
                Method("GenerateAdjacency").
                WithAnyArguments();
            Expect.Once.On(mesh).
                Method("OptimizeInPlace").
                WithAnyArguments();
            Model mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.None);
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
            Expect.Exactly(2).On(mesh).
                Method("GenerateAdjacency").
                WithAnyArguments();
            Expect.Exactly(2).On(mesh).
                Method("OptimizeInPlace").
                WithAnyArguments();
            Model mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.None);
            Model mesh2 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.None);
            Model mesh3 = modelFactory.FromFile("MeshFile2", ModelFactory.Options.None);

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
            Expect.Once.On(mesh).
                Method("GenerateAdjacency").
                WithAnyArguments();
            Expect.Once.On(mesh).
                Method("OptimizeInPlace").
                WithAnyArguments();
            Model mesh1 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.None);
            Model mesh2 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.NoOptimization | ModelFactory.Options.EnsureTangents);
            Model mesh3 = modelFactory.FromFile("MeshFile1", ModelFactory.Options.NoOptimization | ModelFactory.Options.EnsureTangents);

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
