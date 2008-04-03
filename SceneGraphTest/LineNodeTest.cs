using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Animation;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class LineNodeTest : D3DMockTest
    {
        //private IModel model;
        //private IModelMesh modelMesh1;
        //private IModelMesh modelMesh2;
        //private IModelMeshPart modelMeshPart1;
        //private IModelMeshPart modelMeshPart2;
        //private IModelMeshPart modelMeshPart3;
        //private IScene scene;
        //private IRenderableCamera camera;
        //private IMaterialHandler materialHandler1;
        //private IMaterialHandler materialHandler2;
        //private IMaterialHandler materialHandler3;
        //private Matrix worldMatrix = Matrix.Identity;
        //private Matrix viewMatrix = Matrix.CreateRotationX(4.0f);
        //private Matrix projectionMatrix = Matrix.CreateRotationY(1.3f);
        //private Color sceneAmbient = new Color(1, 2, 3);
        //private LightState lightState = new LightState();
        //private IAnimationController animationController;

        //private LineNode node = null;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            //model = mockery.NewMock<IModel>();
            //scene = mockery.NewMock<IScene>();
            //camera = mockery.NewMock<IRenderableCamera>();
            //modelMesh1 = mockery.NewMock<IModelMesh>();
            //modelMesh2 = mockery.NewMock<IModelMesh>();
            //modelMeshPart1 = mockery.NewMock<IModelMeshPart>();
            //modelMeshPart2 = mockery.NewMock<IModelMeshPart>();
            //modelMeshPart3 = mockery.NewMock<IModelMeshPart>();
            //materialHandler1 = mockery.NewMock<IMaterialHandler>();
            //materialHandler2 = mockery.NewMock<IMaterialHandler>();
            //materialHandler3 = mockery.NewMock<IMaterialHandler>();
            //animationController = mockery.NewMock<IAnimationController>();

            //Stub.On(scene).GetProperty("ActiveCamera").Will(Return.Value(camera));
            //Stub.On(scene).GetProperty("AmbientColor").Will(Return.Value(sceneAmbient));
            //Stub.On(camera).GetProperty("ViewMatrix").Will(Return.Value(viewMatrix));
            //Stub.On(camera).GetProperty("ProjectionMatrix").Will(Return.Value(projectionMatrix));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void SetGet()
        {
            // Exersise SUT
            //node = new LineNode("Name");
            // Verify
            //Assert.AreEqual("Name", node.Name);
        }

        //[Test]
        //public void RenderOnePart()
        //{
        //    // Setup
        //    CreateWithSinglePart(true);
        //    Stub.On(animationController).GetProperty("WorldMatrices").Will(Return.Value(new Matrix[] { worldMatrix }));
        //    Expect.Once.On(modelMeshPart1).GetProperty("MaterialHandler").Will(Return.Value(materialHandler1));
        //    Expect.Once.On(materialHandler1).Method("SetupRendering").
        //        With(new Matrix[] { worldMatrix }, viewMatrix, projectionMatrix, sceneAmbient, null);
        //    Expect.Once.On(renderState).SetProperty("CullMode").To(CullMode.CullCounterClockwiseFace);
        //    Expect.Once.On(modelMesh1).Method("Draw");

        //    // Exercise SUT
        //    node.Render(scene);
        //}

        //[Test]
        //public void RenderOnePartNoAnimation()
        //{
        //    // Setup
        //    CreateWithSinglePart(false);
        //    Expect.Once.On(modelMeshPart1).GetProperty("MaterialHandler").Will(Return.Value(materialHandler1));
        //    Expect.Once.On(materialHandler1).Method("SetupRendering").
        //        With(new Matrix[] { worldMatrix }, viewMatrix, projectionMatrix, sceneAmbient, null);
        //    Expect.Once.On(renderState).SetProperty("CullMode").To(CullMode.CullCounterClockwiseFace);
        //    Expect.Once.On(modelMesh1).Method("Draw");

        //    // Exercise SUT
        //    node.Render(scene);
        //}

        //[Test]
        //public void RenderOnePartWithLightState()
        //{
        //    // Setup
        //    CreateWithSinglePart(true);
        //    node.SetLightState(lightState);
        //    Stub.On(animationController).GetProperty("WorldMatrices").Will(Return.Value(new Matrix[] { worldMatrix }));
        //    Expect.Once.On(modelMeshPart1).GetProperty("MaterialHandler").Will(Return.Value(materialHandler1));
        //    Expect.Once.On(materialHandler1).Method("SetupRendering").
        //        With(new Matrix[] { worldMatrix }, viewMatrix, projectionMatrix, sceneAmbient, lightState);
        //    Expect.Once.On(renderState).SetProperty("CullMode").To(CullMode.CullCounterClockwiseFace);
        //    Expect.Once.On(modelMesh1).Method("Draw");

        //    // Exercise SUT
        //    node.Render(scene);
        //}

        //[Test]
        //public void RenderOnePartDifferentCullMode()
        //{
        //    // Setup
        //    CreateWithSinglePart(true);
        //    node.CullMode = CullMode.None;
        //    Stub.On(animationController).GetProperty("WorldMatrices").Will(Return.Value(new Matrix[] { worldMatrix }));
        //    Expect.Once.On(modelMeshPart1).GetProperty("MaterialHandler").Will(Return.Value(materialHandler1));
        //    Expect.Once.On(materialHandler1).Method("SetupRendering").
        //        With(new Matrix[] { worldMatrix }, viewMatrix, projectionMatrix, sceneAmbient, null);
        //    Expect.Once.On(renderState).SetProperty("CullMode").To(CullMode.None);
        //    Expect.Once.On(modelMesh1).Method("Draw");

        //    // Exercise SUT
        //    node.Render(scene);
        //}

        //[Test]
        //public void RenderThreeParts()
        //{
        //    // Setup
        //    CreateWithThreeParts(true);
        //    Stub.On(animationController).GetProperty("WorldMatrices").Will(Return.Value(new Matrix[] { worldMatrix }));
        //    Expect.Once.On(modelMeshPart1).GetProperty("MaterialHandler").Will(Return.Value(materialHandler1));
        //    Expect.Once.On(modelMeshPart2).GetProperty("MaterialHandler").Will(Return.Value(materialHandler2));
        //    Expect.Once.On(modelMeshPart3).GetProperty("MaterialHandler").Will(Return.Value(materialHandler3));
        //    Expect.Once.On(materialHandler1).Method("SetupRendering").
        //        With(new Matrix[] { worldMatrix }, viewMatrix, projectionMatrix, sceneAmbient, null);
        //    Expect.Once.On(materialHandler2).Method("SetupRendering").
        //        With(new Matrix[] { worldMatrix }, viewMatrix, projectionMatrix, sceneAmbient, null);
        //    Expect.Once.On(materialHandler3).Method("SetupRendering").
        //        With(new Matrix[] { worldMatrix }, viewMatrix, projectionMatrix, sceneAmbient, null);
        //    Expect.Exactly(2).On(renderState).SetProperty("CullMode").To(CullMode.CullCounterClockwiseFace);
        //    Expect.Once.On(modelMesh1).Method("Draw");
        //    Expect.Once.On(modelMesh2).Method("Draw");

        //    // Exercise SUT
        //    node.Render(scene);
        //}

        //[Test]
        //public void TestStepWithAnimation()
        //{
        //    // Setup
        //    CreateWithSinglePart(true);
        //    Expect.Once.On(animationController).Method("Step").With(worldMatrix);

        //    // Exercise SUT
        //    node.Step();
        //}

        //[Test]
        //public void TestStepWithoutAnimation()
        //{
        //    // Setup
        //    CreateWithSinglePart(false);

        //    // Exercise SUT
        //    node.Step();
        //}

        //private void CreateWithSinglePart(bool animated)
        //{
        //    StubModelMesh(model, new IModelMesh[] { modelMesh1 });
        //    StubModelMeshPart(modelMesh1, new IModelMeshPart[] { modelMeshPart1 });
        //    node = new LineNode("Name", model, device);
        //    if (animated)
        //        Stub.On(model).GetProperty("AnimationController").Will(Return.Value(animationController));
        //    else
        //        Stub.On(model).GetProperty("AnimationController").Will(Return.Value(null));
        //}

        //private void CreateWithThreeParts(bool animated)
        //{
        //    StubModelMesh(model, new IModelMesh[] { modelMesh1, modelMesh2 });
        //    StubModelMeshPart(modelMesh1, new IModelMeshPart[] { modelMeshPart1, modelMeshPart2 });
        //    StubModelMeshPart(modelMesh2, new IModelMeshPart[] { modelMeshPart3 });
        //    node = new LineNode("Name", model, device);
        //    if (animated)
        //        Stub.On(model).GetProperty("AnimationController").Will(Return.Value(animationController));
        //    else
        //        Stub.On(model).GetProperty("AnimationController").Will(Return.Value(null));
        //}

    }
}
