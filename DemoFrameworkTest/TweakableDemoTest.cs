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
    public class TweakableDemoTest
    {
        private TweakableDemo tweakable;
        private Mockery mockery;
        private IDemoRegistrator target;
        private IDemoEffectBuilder builder;
        private ITweakableFactory factory;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = mockery.NewMock<IDemoRegistrator>();
            builder = mockery.NewMock<IDemoEffectBuilder>();
            factory = mockery.NewMock<ITweakableFactory>();
            tweakable = new TweakableDemo(target, builder, factory);
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
            Assert.AreEqual(3, tweakable.NumVisableVariables);
        }

        [Test]
        public void NumVariables()
        {
            // Setup
            List<ITrack> tracks = new List<ITrack>();
            for (int i = 0; i < 5; i++)
                tracks.Add(null);
            Expect.Once.On(target).GetProperty("Tracks").Will(Return.Value(tracks));
            // Exercise SUT and verify
            Assert.AreEqual(5, tweakable.NumVariables);
        }

        [Test]
        public void ReadEmptyXml()
        {
            // Setup
            XmlNode node = CreateXmlNode("<Demo></Demo>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ReadInvalidXml()
        {
            // Setup
            XmlNode node = CreateXmlNode("<Demo> <Invalid/> </Demo>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadEffectFromXml()
        {
            // Setup
            ITrack track = mockery.NewMock<ITrack>();
            ITweakableObject tweakableTrack = mockery.NewMock<ITweakableObject>();
            List<ITrack> tracks = new List<ITrack>();
            tracks.Add(null);
            tracks.Add(track);
            XmlNode node = CreateXmlNode("<Demo><Effect name=\"Name\" class=\"Class\" track=\"1\" startTime=\"10\" endTime=\"11\"/></Demo>");
            Expect.Once.On(builder).Method("AddEffect").With("Class", "Name", 1, 10.0f, 11.0f);
            Stub.On(target).GetProperty("Tracks").Will(Return.Value(tracks));
            Expect.Once.On(factory).Method("CreateTweakableObject").With(track).Will(Return.Value(tweakableTrack));
            Expect.Once.On(tweakableTrack).Method("ReadFromXmlFile").With(node.FirstChild);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadPostEffectFromXml()
        {
            // Setup
            ITrack track = mockery.NewMock<ITrack>();
            ITweakableObject tweakableTrack = mockery.NewMock<ITweakableObject>();
            List<ITrack> tracks = new List<ITrack>();
            tracks.Add(track);
            XmlNode node = CreateXmlNode("<Demo><PostEffect name=\"Name1\" class=\"Class1\" track=\"0\" startTime=\"12.2\" endTime=\"13.3\"/></Demo>");
            Expect.Once.On(builder).Method("AddPostEffect").With("Class1", "Name1", 0, 12.2f, 13.3f);
            Stub.On(target).GetProperty("Tracks").Will(Return.Value(tracks));
            Expect.Once.On(factory).Method("CreateTweakableObject").With(track).Will(Return.Value(tweakableTrack));
            Expect.Once.On(tweakableTrack).Method("ReadFromXmlFile").With(node.FirstChild);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadUnhandledFromXml()
        {
            // Setup
            XmlNode node = CreateXmlNode("<Demo><Transition/><Texture/><Generator/></Demo>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ReadEffectWithMissingAttribute()
        {
            // Setup
            XmlNode node = CreateXmlNode("<Demo><Effect/></Demo>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
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
