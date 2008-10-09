using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.SceneGraph;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableNodeTest
    {
        private TweakableNode tweakable;
        private Mockery mockery;
        private ITweakableFactory factory;
        private INode target;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = new DummyNode("");
            factory = new DemoTweakerHandler(null, null);
            tweakable = new TweakableNode(target, factory);
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
        public void NumVariablesMoreNodes()
        {
            // Setup
            target.AddChild(new DummyNode("d1"));
            target.AddChild(new DummyNode("d2"));
            // Exercise SUT and verify
            Assert.AreEqual(4, tweakable.NumVariables);
        }

        [Test]
        public void GetModeNodes()
        {
            // Setup
            target.AddChild(new DummyNode("d1"));
            target.AddChild(new DummyNode("d2"));
            // Exercise SUT and verify
            for (int i = 0; i < 2; i++)
                Assert.IsInstanceOfType(typeof(TweakableNode), tweakable.GetChild(i));
        }

    }
}
