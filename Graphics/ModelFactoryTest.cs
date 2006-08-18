using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NUnit.Framework;
using NMock2;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class ModelFactoryTest
    {
        ModelFactory meshFactory;

        Mockery mockery;
        IGraphicsFactory graphicsFactory;
        IMesh mesh;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            mesh = mockery.NewMock<IMesh>();
            meshFactory = new ModelFactory(null, graphicsFactory);
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
 	            throw new Exception("The method or operation is not implemented.");
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
            Model mesh1 = meshFactory.CreateBox(10.0f, 20.0f, 30.0f);
            Model mesh2 = meshFactory.CreateBox(10.0f, 20.0f, 30.0f);
            Model mesh3 = meshFactory.CreateBox(10.0f, 22.0f, 30.0f);

            Assert.IsNotNull(mesh1);
            Assert.IsNotNull(mesh3);
            Assert.AreSame(mesh1, mesh2);
            Assert.AreNotSame(mesh1, mesh3);
            Assert.AreNotSame(mesh2, mesh3);

            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountBoxes);
        }

        [Test]
        public void CreateFileAutoExpireTest()
        {
            Expect.Exactly(2).On(graphicsFactory).
                Method("MeshFromFile").
                WithAnyArguments().
                Will(new SetMaterial(mesh));
            Model mesh1 = meshFactory.FromFile("MeshFile1");
            Model mesh2 = meshFactory.FromFile("MeshFile1");
            Model mesh3 = meshFactory.FromFile("MeshFile2");

            Assert.IsNotNull(mesh1);
            Assert.IsNotNull(mesh3);
            Assert.AreSame(mesh1, mesh2);
            Assert.AreNotSame(mesh1, mesh3);
            Assert.AreNotSame(mesh2, mesh3);

            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountFiles);
        }

    }
}
