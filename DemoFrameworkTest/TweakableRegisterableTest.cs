using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using System.Xml;
using Dope.DDXX.Utility;
using Dope.DDXX.SceneGraph;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableRegisterableTest
    {
        private class TestEffect : BaseDemoEffect
        {
            public TestEffect(Mockery mockery)
                : base("", 1, 2)
            {
            }

            protected override void Initialize()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override void Step()
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override void Render()
            {
                throw new Exception("The method or operation is not implemented.");
            }

        }

        private class RegisterableStub : Registerable
        {
            public RegisterableStub()
                : base("", 0, 0)
            {
            }

            private float parameter1;
            public float Parameter1
            {
                get { return parameter1; }
                set { parameter1 = value; }
            }

            private float parameter2;
            public float Parameter2
            {
                get { return parameter2; }
                set { parameter2 = value; }
            }

        }

        private TweakableRegisterable tweakable;
        private Mockery mockery;
        private IRegisterable target;
        private ITweakableFactory factory;
        private XmlDocument document;
        private IDemoEffect effectTarget;
        private IScene scene;
        private ITweakable tweakableScene;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = mockery.NewMock<IRegisterable>();
            effectTarget = mockery.NewMock<IDemoEffect>();
            scene = mockery.NewMock<IScene>();
            tweakableScene = mockery.NewMock<ITweakable>();
            Stub.On(effectTarget).GetProperty("Scene").Will(Return.Value(scene));
            factory = new DemoTweakerHandler(null, null);// mockery.NewMock<ITweakableFactory>();
            tweakable = new TweakableRegisterable(target, factory);
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
            Assert.AreEqual(10, tweakable.NumVisableVariables);
        }

        [Test]
        public void NumVariablesNotEffect()
        {
            // Exercise SUT and verify
            Assert.AreEqual(0, tweakable.NumVariables);
        }

        [Test]
        public void NumVariablesEffect()
        {
            // Setup
            TestEffect newTarget = new TestEffect(mockery);
            tweakable = new TweakableRegisterable(newTarget, factory);
            // Exercise SUT and verify
            Assert.AreEqual(3 + 1, tweakable.NumVariables);
        }

        [Test]
        public void GetSceneFromEffect()
        {
            // Setup
            TestEffect newTarget = new TestEffect(mockery);
            tweakable = new TweakableRegisterable(newTarget, factory);
            // Exercise SUT and verify
            Assert.IsInstanceOfType(typeof(TweakableScene), tweakable.GetChild(0));
        }

        [Test]
        public void ReadFromXmlNoParameters()
        {
            // Setup
            XmlNode node = CreateXmlNode("<Effect></Effect>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadFromXmlWithScene()
        {
            // Setup
            factory = mockery.NewMock<ITweakableFactory>();
            tweakable = new TweakableRegisterable(effectTarget, factory);
            XmlNode node = CreateXmlNode("<Effect><Scene /></Effect>");
            Stub.On(factory).Method("CreateTweakableObject").With(scene).Will(Return.Value(tweakableScene));
            Expect.Once.On(tweakableScene).Method("ReadFromXmlFile");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void WriteToXmlNoParameters()
        {
            // Setup
            XmlNode node = CreateXmlNode("<Effect></Effect>");
            // Exercise SUT and verify
            tweakable.WriteToXmlFile(document, node);
        }

        [Test]
        public void WriteToXmlWithScene()
        {
            // Setup
            factory = mockery.NewMock<ITweakableFactory>();
            tweakable = new TweakableRegisterable(effectTarget, factory);
            XmlNode node = CreateXmlNode("<Effect><Scene /></Effect>");
            Stub.On(factory).Method("CreateTweakableObject").With(scene).Will(Return.Value(tweakableScene));
            Expect.Once.On(tweakableScene).Method("WriteToXmlFile");
            // Exercise SUT and verify
            tweakable.WriteToXmlFile(document, node);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ReadFromXmlUnknownParameter()
        {
            // Setup
            XmlNode node = CreateXmlNode("<Effect><P1 /></Effect>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void WriteToXmlTwoParameters()
        {
            // Setup
            target = new RegisterableStub();
            (target as RegisterableStub).Parameter1 = 1.2f;
            (target as RegisterableStub).Parameter2 = 2.2f;
            tweakable = new TweakableRegisterable(target, factory);
            XmlNode node = CreateXmlNode("<Effect><Parameter1>0</Parameter1> <Parameter2>0</Parameter2></Effect>");
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<Effect><Parameter1>1.2</Parameter1> <Parameter2>2.2</Parameter2></Effect>", node.OuterXml);
        }

        [Test]
        public void WriteToXmlNewParameter()
        {
            // Setup
            target = new RegisterableStub();
            (target as RegisterableStub).Parameter1 = 2.3f;
            (target as RegisterableStub).Parameter2 = 3.3f;
            tweakable = new TweakableRegisterable(target, factory);
            XmlNode node = CreateXmlNode(
@"<Effect>
		<Parameter2>0</Parameter2>
</Effect>");
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual(
@"<Effect>
		<Parameter2>3.3</Parameter2>
		<Parameter1>2.3</Parameter1>
</Effect>", node.OuterXml);
        }

        [Test]
        public void WriteToXmlNewParameterNoWhitespace()
        {
            // Setup
            target = new RegisterableStub();
            (target as RegisterableStub).Parameter1 = 2.3f;
            (target as RegisterableStub).Parameter2 = 3.3f;
            tweakable = new TweakableRegisterable(target, factory);
            XmlNode node = CreateXmlNode(
@"<Effect><Parameter2>0</Parameter2>
</Effect>");
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual(
@"<Effect><Parameter2>3.3</Parameter2><Parameter1>2.3</Parameter1>
</Effect>", node.OuterXml);
        }

        private XmlNode CreateXmlNode(string xml)
        {
            document = new XmlDocument();
            document.PreserveWhitespace = true;
            document.LoadXml(xml);
            return document.DocumentElement;
        }

    }
}
