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

            public List<IRegisterable> GetAllRegisterables()
            { throw new Exception("The method or operation is not implemented."); }
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
        private List<IRegisterable> registerables;

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
            Assert.AreEqual(13, tweakable.NumVisableVariables);
        }

        [Test]
        public void NumVariables()
        {
            // Setup
            CreateRegisterables(5);
            Stub.On(target).Method("GetAllRegisterables").Will(Return.Value(registerables));
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
            CreateRegisterables(2);
            Stub.On(target).Method("GetAllRegisterables").Will(Return.Value(registerables));
            ITweakableObject tweakableRegisterable = mockery.NewMock<ITweakableObject>();
            CreateXmlNode("<Demo><Effect name=\"R1\" class=\"Class\" track=\"1\" startTime=\"10\" endTime=\"11\"/></Demo>");
            Expect.Once.On(builder).Method("AddEffect").With("Class", "R1", 1, 10.0f, 11.0f);
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[1]).Will(Return.Value(tweakableRegisterable));
            Expect.Once.On(tweakableRegisterable).Method("ReadFromXmlFile").With(node.FirstChild);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void WriteEffectToXml()
        {
            // Setup
            CreateRegisterables(1);
            Stub.On(target).Method("GetAllRegisterables").Will(Return.Value(registerables));
            CreateXmlNode("<Demo><Effect name=\"R0\" /></Demo>");
            ITweakableObject tweakableRegisterable1 = mockery.NewMock<ITweakableObject>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[0]).Will(Return.Value(tweakableRegisterable1));
            Expect.Once.On(tweakableRegisterable1).Method("WriteToXmlFile").With(node.ChildNodes[0]);
            // Exercise SUT
            tweakable.WriteToXmlFile(node);
        }

        [Test]
        public void ReadPostEffectFromXml()
        {
            // Setup
            CreateRegisterables(2);
            Stub.On(target).Method("GetAllRegisterables").Will(Return.Value(registerables));
            ITweakableObject tweakableRegisterable = mockery.NewMock<ITweakableObject>();
            CreateXmlNode("<Demo><PostEffect name=\"R0\" class=\"Class1\" track=\"0\" startTime=\"12.2\" endTime=\"13.3\"/></Demo>");
            Expect.Once.On(builder).Method("AddPostEffect").With("Class1", "R0", 0, 12.2f, 13.3f);
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[0]).Will(Return.Value(tweakableRegisterable));
            Expect.Once.On(tweakableRegisterable).Method("ReadFromXmlFile").With(node.FirstChild);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void WriteEffectAndPostEffectToXml()
        {
            // Setup
            CreateRegisterables(2);
            Stub.On(target).Method("GetAllRegisterables").Will(Return.Value(registerables));
            CreateXmlNode("<Demo><Effect name=\"R1\" /><PostEffect name=\"R0\" /></Demo>");
            ITweakableObject tweakableRegisterable1 = mockery.NewMock<ITweakableObject>();
            ITweakableObject tweakableRegisterable2 = mockery.NewMock<ITweakableObject>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[0]).Will(Return.Value(tweakableRegisterable1));
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[1]).Will(Return.Value(tweakableRegisterable2));
            Expect.Once.On(tweakableRegisterable1).Method("WriteToXmlFile").With(node.ChildNodes[1]);
            Expect.Once.On(tweakableRegisterable2).Method("WriteToXmlFile").With(node.ChildNodes[0]);
            // Exercise SUT
            tweakable.WriteToXmlFile(node);
        }

        [Test]
        public void ReadUnhandledFromXml()
        {
            // Setup
            CreateXmlNode("<Demo><Transition /> <Texture /><Generator /></Demo>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void WriteUnhandledToXml()
        {
            // Setup
            CreateXmlNode("<Demo><Transition /> <Texture /><Generator /></Demo>");
            // Exercise SUT
            tweakable.WriteToXmlFile(node);
        }

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

        private void CreateRegisterables(int num)
        {
            registerables = new List<IRegisterable>();
            for (int i = 0; i < num; i++)
                registerables.Add(new Registerable("R" + i, 0 + i, 1 + i));
        }

    }
}
