using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using System.Xml;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableDemoTest
    {
        private class RegistratorStub : IDemoRegistrator
        {
            public float StartTime
            { get { throw new Exception("The method or operation is not implemented."); } }

            public float EndTime
            { get { throw new Exception("The method or operation is not implemented."); } }

            public List<ITrack> Tracks
            { get { throw new Exception("The method or operation is not implemented."); } }

            public IDemoEffect[] Effects(int track)
            { throw new Exception("The method or operation is not implemented."); }

            public IDemoPostEffect[] PostEffects(int track)
            { throw new Exception("The method or operation is not implemented."); }

            public void Register(int track, IDemoEffect effect)
            { throw new Exception("The method or operation is not implemented."); }

            public void Register(int track, IDemoPostEffect postEffect)
            { throw new Exception("The method or operation is not implemented."); }

            private Color clearColor;
            public Color ClearColor
            {
                get { return clearColor; }
                set { clearColor = value; }
            }
        }

        private TweakableDemo tweakable;
        private Mockery mockery;
        private IDemoRegistrator target;
        private IDemoEffectBuilder builder;
        private ITweakableFactory factory;
        private XmlNode node;
        private RegistratorStub registrator;
        private List<ITrack> tracks;
        private XmlDocument document;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = mockery.NewMock<IDemoRegistrator>();
            builder = mockery.NewMock<IDemoEffectBuilder>();
            factory = mockery.NewMock<ITweakableFactory>();
            tweakable = new TweakableDemo(target, builder, factory);
            tracks = new List<ITrack>();
            Stub.On(target).GetProperty("Tracks").Will(Return.Value(tracks));
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
            for (int i = 0; i < 5; i++)
                tracks.Add(null);
            // Exercise SUT and verify
            Assert.AreEqual(5, tweakable.NumVariables);
        }

        [Test]
        public void ReadEmptyXml()
        {
            // Setup
            CreateXmlNode("<Demo></Demo>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void WriteEmptyXml()
        {
            // Setup
            ReadEmptyXml();
            // Exercise SUT
            tweakable.WriteToXmlFile(node);
            // Verify
            Assert.AreEqual("<Demo></Demo>", node.OuterXml);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ReadInvalidXml()
        {
            // Setup
            CreateXmlNode("<Demo> <Invalid/> </Demo>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadEffectFromXml()
        {
            // Setup
            ITrack track = mockery.NewMock<ITrack>();
            ITweakableObject tweakableTrack = mockery.NewMock<ITweakableObject>();
            tracks.Add(null);
            tracks.Add(track);
            CreateXmlNode("<Demo><Effect name=\"Name\" class=\"Class\" track=\"1\" startTime=\"10\" endTime=\"11\"/></Demo>");
            Expect.Once.On(builder).Method("AddEffect").With("Class", "Name", 1, 10.0f, 11.0f);
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
            tracks.Add(track);
            CreateXmlNode("<Demo><PostEffect name=\"Name1\" class=\"Class1\" track=\"0\" startTime=\"12.2\" endTime=\"13.3\"/></Demo>");
            Expect.Once.On(builder).Method("AddPostEffect").With("Class1", "Name1", 0, 12.2f, 13.3f);
            Expect.Once.On(factory).Method("CreateTweakableObject").With(track).Will(Return.Value(tweakableTrack));
            Expect.Once.On(tweakableTrack).Method("ReadFromXmlFile").With(node.FirstChild);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadUnhandledFromXml()
        {
            // Setup
            CreateXmlNode("<Demo><Transition /> <Texture /><Generator /></Demo>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        //[Test]
        //public void WriteToXml()
        //{
        //    // Setup
        //    ITrack track1 = mockery.NewMock<ITrack>();
        //    ITrack track2 = mockery.NewMock<ITrack>();
        //    tracks.Add(track1);
        //    tracks.Add(track2);
        //    ITweakableObject tweakableTrack1 = mockery.NewMock<ITweakableObject>();
        //    ITweakableObject tweakableTrack2 = mockery.NewMock<ITweakableObject>();
        //    Expect.Once.On(factory).Method("CreateTweakableObject").With(track1).Will(Return.Value(tweakableTrack1));
        //    Expect.Once.On(factory).Method("CreateTweakableObject").With(track2).Will(Return.Value(tweakableTrack2));
        //    CreateXmlNode("<Demo><Child /></Demo>");
        //    Expect.Once.On(tweakableTrack1).Method("WriteToXmlFile").With(node);
        //    Expect.Once.On(tweakableTrack2).Method("WriteToXmlFile").With(node);
        //    // Exercise SUT
        //    tweakable.WriteToXmlFile(node);
        //}

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ReadEffectWithMissingAttribute()
        {
            // Setup
            CreateXmlNode("<Demo><Effect/></Demo>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadWithProperty()
        {
            // Setup
            registrator = new RegistratorStub();
            tweakable = new TweakableDemo(registrator, builder, factory);
            CreateXmlNode("<Demo><ClearColor>1,2,3,4</ClearColor></Demo>");
            // Exercise SUT
            tweakable.ReadFromXmlFile(node);
            // Verify
            Assert.AreEqual(new Color(1, 2, 3, 4), registrator.ClearColor);
        }

        [Test]
        public void WriteWithProperty()
        {
            // Setup
            ReadWithProperty();
            // Exercise SUT
            registrator.ClearColor = new Color(5, 6, 7, 8);
            tweakable.WriteToXmlFile(node);
            // Verify
            Assert.AreEqual("<Demo><ClearColor>5, 6, 7, 8</ClearColor></Demo>", node.OuterXml);
        }

        private void CreateXmlNode(string xml)
        {
            document = new XmlDocument();
            document.PreserveWhitespace = true;
            document.LoadXml(xml);
            node = document.DocumentElement;
        }

    }
}
