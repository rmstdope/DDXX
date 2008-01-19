using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using System.Xml;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableRegisterableTest
    {
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

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = mockery.NewMock<IRegisterable>();
            factory = mockery.NewMock<ITweakableFactory>();
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
            Assert.AreEqual(15, tweakable.NumVisableVariables);
        }

        [Test]
        public void NumVariables()
        {
            // Exercise SUT and verify
            Assert.AreEqual(0, tweakable.NumVariables);
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
        public void WriteToXmlNoParameters()
        {
            // Setup
            XmlNode node = CreateXmlNode("<Effect></Effect>");
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
