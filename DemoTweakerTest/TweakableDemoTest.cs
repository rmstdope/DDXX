using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using System.Xml;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoTweaker
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

            #region IDemoRegistrator Members

            public IGraphicsFactory GraphicsFactory
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public IDemoEffectTypes EffectTypes
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            public void SetSong(string filename)
            {
                throw new NotImplementedException();
            }

            #endregion

            #region IDemoEffectBuilder Members

            public void AddEffect(string className, string effectName, int effectTrack, float startTime, float endTime)
            {
                throw new NotImplementedException();
            }

            public void AddPostEffect(string className, string postEffectName, int effectTrack, float startTime, float endTime)
            {
                throw new NotImplementedException();
            }

            public void AddTransition(string className, string transitionName, int destinationTrack, float startTime, float endTime)
            {
                throw new NotImplementedException();
            }

            public void AddGenerator(string generatorName, string className)
            {
                throw new NotImplementedException();
            }

            public void AddTexture(string textureName, string generatorName, int width, int height, int mipLevels)
            {
                throw new NotImplementedException();
            }

            public void AddFloatParameter(string name, float value, float stepSize)
            {
                throw new NotImplementedException();
            }

            public void AddIntParameter(string name, int value, float stepSize)
            {
                throw new NotImplementedException();
            }

            public void AddStringParameter(string name, string value)
            {
                throw new NotImplementedException();
            }

            public void AddVector2Parameter(string name, Microsoft.Xna.Framework.Vector2 value, float stepSize)
            {
                throw new NotImplementedException();
            }

            public void AddVector3Parameter(string name, Microsoft.Xna.Framework.Vector3 value, float stepSize)
            {
                throw new NotImplementedException();
            }

            public void AddVector4Parameter(string name, Microsoft.Xna.Framework.Vector4 value, float stepSize)
            {
                throw new NotImplementedException();
            }

            public void AddColorParameter(string parameterName, Color color)
            {
                throw new NotImplementedException();
            }

            public void AddBoolParameter(string parameterName, bool color)
            {
                throw new NotImplementedException();
            }

            public void AddSetupCall(string name, List<object> parameters)
            {
                throw new NotImplementedException();
            }

            public void AddGeneratorInput(int num, string generatorName)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        private ITweakable tweakable;
        private Mockery mockery;
        private IDemoRegistrator target;
        private ITweakableFactory factory;
        private XmlNode node;
        private RegistratorStub registrator;
        private List<ITrack> tracks;
        private XmlDocument document;
        private List<IRegisterable> registerables;
        private IGraphicsFactory graphicsFactory;
        private ITextureFactory textureFactory;
        private IModelFactory modelFactory;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = mockery.NewMock<IDemoRegistrator>();
            factory = mockery.NewMock<ITweakableFactory>();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            modelFactory = mockery.NewMock<IModelFactory>();
            tweakable = new TweakableDemo(target, factory);
            tracks = new List<ITrack>();
            Stub.On(target).GetProperty("Tracks").Will(Return.Value(tracks));
            Stub.On(factory).GetProperty("GraphicsFactory").Will(Return.Value(graphicsFactory));
            Stub.On(graphicsFactory).GetProperty("TextureFactory").Will(Return.Value(textureFactory));
            Stub.On(graphicsFactory).GetProperty("ModelFactory").Will(Return.Value(modelFactory));
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
            // Exercise SUT and verify
            Assert.AreEqual(5 + 2, tweakable.NumVariables);
        }

        [Test]
        public void GetChildRegisterable()
        {
            // Setup
            ITweakable childTweakable;
            childTweakable = mockery.NewMock<ITweakable>();
            CreateRegisterables(1);
            Expect.Once.On(factory).Method("CreateTweakableObject").
                With(registerables[0]).Will(Return.Value(childTweakable));
            // Exercise SUT and verify
            Assert.AreSame(childTweakable , tweakable.GetChild(0));
        }

        [Test]
        public void GetChildFactories()
        {
            // Setup
            ITweakable childTweakable1 = mockery.NewMock<ITweakable>();
            ITweakable childTweakable2 = mockery.NewMock<ITweakable>();
            CreateRegisterables(1);
            Expect.Once.On(factory).Method("CreateTweakableObject").
                With(textureFactory).Will(Return.Value(childTweakable1));
            Expect.Once.On(factory).Method("CreateTweakableObject").
                With(modelFactory).Will(Return.Value(childTweakable2));
            // Exercise SUT and verify
            Assert.AreSame(childTweakable1, tweakable.GetChild(1));
            Assert.AreSame(childTweakable2, tweakable.GetChild(2));
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
            CreateXmlNode("<Demo> <TextureFactory /> <ModelFactory /> </Demo>");
            ExpectWriteToXml(textureFactory);
            ExpectWriteToXml(modelFactory);
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<Demo> <TextureFactory /> <ModelFactory /> </Demo>", node.OuterXml);
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
            ITweakable tweakableRegisterable = mockery.NewMock<ITweakable>();
            CreateXmlNode("<Demo><Effect name=\"R1\" class=\"Class\" track=\"1\" startTime=\"10\" endTime=\"11\"/></Demo>");
            Expect.Once.On(target).Method("AddEffect").With("Class", "R1", 1, 10.0f, 11.0f);
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
            CreateXmlNode("<Demo><Effect name=\"R0\" /></Demo>");
            ITweakable tweakableRegisterable1 = mockery.NewMock<ITweakable>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[0]).Will(Return.Value(tweakableRegisterable1));
            Expect.Once.On(tweakableRegisterable1).Method("WriteToXmlFile").With(document, node.ChildNodes[0]);
            ExpectWriteToXml(textureFactory);
            ExpectWriteToXml(modelFactory);
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<Demo><Effect name=\"R0\" /><TextureFactory /><ModelFactory /></Demo>", node.OuterXml);
        }

        [Test]
        public void ReadPostEffectFromXml()
        {
            // Setup
            CreateRegisterables(1);
            ITweakable tweakableRegisterable = mockery.NewMock<ITweakable>();
            CreateXmlNode("<Demo><PostEffect name=\"R0\" class=\"Class1\" track=\"0\" startTime=\"12.2\" endTime=\"13.3\"/></Demo>");
            Expect.Once.On(target).Method("AddPostEffect").With("Class1", "R0", 0, 12.2f, 13.3f);
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[0]).Will(Return.Value(tweakableRegisterable));
            Expect.Once.On(tweakableRegisterable).Method("ReadFromXmlFile").With(node.FirstChild);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadTransitionFromXml()
        {
            // Setup
            CreateRegisterables(1);
            ITweakable tweakableRegisterable = mockery.NewMock<ITweakable>();
            CreateXmlNode("<Demo><Transition name=\"R0\" class=\"Class1\" destinationTrack=\"7\" startTime=\"12.2\" endTime=\"13.3\"/></Demo>");
            Expect.Once.On(target).Method("AddTransition").With("Class1", "R0", 7, 12.2f, 13.3f);
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[0]).Will(Return.Value(tweakableRegisterable));
            Expect.Once.On(tweakableRegisterable).Method("ReadFromXmlFile").With(node.FirstChild);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void WriteEffectPostEffectAndTransitionToXml()
        {
            // Setup
            CreateRegisterables(3);
            CreateXmlNode("<Demo><Effect name=\"R2\" /><PostEffect name=\"R1\" /><Transition name=\"R0\" /></Demo>");
            ITweakable tweakableRegisterable1 = mockery.NewMock<ITweakable>();
            ITweakable tweakableRegisterable2 = mockery.NewMock<ITweakable>();
            ITweakable tweakableRegisterable3 = mockery.NewMock<ITweakable>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[0]).Will(Return.Value(tweakableRegisterable1));
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[1]).Will(Return.Value(tweakableRegisterable2));
            Expect.Once.On(factory).Method("CreateTweakableObject").With(registerables[2]).Will(Return.Value(tweakableRegisterable3));
            Expect.Once.On(tweakableRegisterable1).Method("WriteToXmlFile").With(document, node.ChildNodes[2]);
            Expect.Once.On(tweakableRegisterable2).Method("WriteToXmlFile").With(document, node.ChildNodes[1]);
            Expect.Once.On(tweakableRegisterable3).Method("WriteToXmlFile").With(document, node.ChildNodes[0]);
            ExpectWriteToXml(textureFactory);
            ExpectWriteToXml(modelFactory);
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<Demo><Effect name=\"R2\" /><PostEffect name=\"R1\" /><Transition name=\"R0\" /><TextureFactory /><ModelFactory /></Demo>", node.OuterXml);
        }

        [Test]
        public void ReadTextureFactoryFromXml()
        {
            // Setup
            ITweakable tweakableTextureFactory = mockery.NewMock<ITweakable>();
            CreateXmlNode("<Demo><TextureFactory/></Demo>");
            Expect.Once.On(factory).Method("CreateTweakableObject").With(textureFactory).Will(Return.Value(tweakableTextureFactory));
            Expect.Once.On(tweakableTextureFactory).Method("ReadFromXmlFile").With(node.FirstChild);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
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
            Expect.Once.On(factory).Method("CreateTweakableValue").
                With(registrator.GetType().GetProperty("ClearColor"), registrator).
                Will(Return.Value(new TweakableColor(registrator.GetType().GetProperty("ClearColor"), registrator, null)));
            tweakable = new TweakableDemo(registrator, factory);
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
            ExpectWriteToXml(textureFactory);
            ExpectWriteToXml(modelFactory);
            // Exercise SUT
            registrator.ClearColor = new Color(5, 6, 7, 8);
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<Demo><ClearColor>5, 6, 7, 8</ClearColor><TextureFactory /><ModelFactory /></Demo>", node.OuterXml);
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
            Stub.On(target).Method("GetAllRegisterables").Will(Return.Value(registerables));
        }

        private void ExpectWriteToXml(object target)
        {
            ITweakable tweakable = mockery.NewMock<ITweakable>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(target).Will(Return.Value(tweakable));
            Expect.Once.On(tweakable).Method("WriteToXmlFile").WithAnyArguments();
        }

    }
}
