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
        private TweakableRegisterable tweakable;
        private Mockery mockery;
        private IRegisterable target;
        private ITweakableFactory factory;

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
        [ExpectedException(typeof(DDXXException))]
        public void ReadFromXmlUnknownParameter()
        {
            // Setup
            XmlNode node = CreateXmlNode("<Effect><P1 /></Effect>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadFromXmlTwoParameters()
        {
            // Setup
            target = new Registerable("", 0, 0);
            tweakable = new TweakableRegisterable(target, factory);
            XmlNode node = CreateXmlNode("<Effect> <StartTime>1.2</StartTime> <EndTime>2.2</EndTime></Effect>");
            // Exercise SUT
            tweakable.ReadFromXmlFile(node);
            // Verify
            Assert.AreEqual(1.2f, target.StartTime);
            Assert.AreEqual(2.2f, target.EndTime);
        }

        private XmlNode CreateXmlNode(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.PreserveWhitespace = true;
            document.LoadXml(xml);
            return document.DocumentElement;
        }

    }
}
