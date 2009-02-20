using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.SceneGraph;
using NMock2;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoTweaker
{
    [TestFixture]
    public class TweakableSceneTest
    {
        private TweakableScene tweakable;
        private Mockery mockery;
        private ITweakableFactory factory;
        private IScene target;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = new Scene();
            factory = new DemoTweakerHandler(null);
            tweakable = new TweakableScene(target, factory);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void NumVisableVariable()
        {
            // Exercise SUT and verify
            Assert.AreEqual(15, tweakable.NumVisableVariables);
        }

        [Test]
        public void NumVariables()
        {
            // Exercise SUT and verify
            Assert.AreEqual(2, tweakable.NumVariables);
        }

        [Test]
        public void GetRootNode()
        {
            // Exercise SUT and verify
            Assert.IsInstanceOfType(typeof(TweakableNode), tweakable.GetChild(0));
        }

        //[Test]
        //public void NumVariablesMoreNodes()
        //{
        //    // Setup
        //    target.AddNode(new DummyNode("d1"));
        //    target.AddNode(new DummyNode("d2"));
        //    // Exercise SUT and verify
        //    Assert.AreEqual(4, tweakable.NumVariables);
        //}

        //[Test]
        //public void GetModeNodes()
        //{
        //    // Setup
        //    target.AddNode(new DummyNode("d1"));
        //    target.AddNode(new DummyNode("d2"));
        //    // Exercise SUT and verify
        //    for (int i = 0; i < 3; i++)
        //        Assert.IsInstanceOfType(typeof(TweakableNode), tweakable.GetChild(i));
        //}

    }
}
