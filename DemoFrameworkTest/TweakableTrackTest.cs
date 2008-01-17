using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using System.Xml;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableTrackTest
    {
        private TweakableTrack tweakable;
        private Mockery mockery;
        private ITrack target;
        private ITweakableFactory factory;
        private IDemoEffect[] effects;
        private IDemoPostEffect[] postEffects;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = mockery.NewMock<ITrack>();
            factory = mockery.NewMock<ITweakableFactory>();
            tweakable = new TweakableTrack(target, factory);
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
        public void NumVariablesZero()
        {
            // Setup
            CreateEffectsAndPostEffectsForTarget(0, 0);
            // Exercise SUT and verify
            Assert.AreEqual(0, tweakable.NumVariables);
        }

        [Test]
        public void NumVariables()
        {
            // Setup
            CreateEffectsAndPostEffectsForTarget(2, 4);
            // Exercise SUT and verify
            Assert.AreEqual(6, tweakable.NumVariables);
        }

        [Test]
        public void ReadEffectFromXml()
        {
            // Setup
            CreateEffectsAndPostEffectsForTarget(3, 1);
            ITweakableObject tweakableEffect = mockery.NewMock<ITweakableObject>();
            XmlNode node = CreateXmlNode("<Effect name=\"Effect1\"></Effect>");
            Expect.Once.On(factory).Method("CreateTweakableObject").With(effects[1]).Will(Return.Value(tweakableEffect));
            Expect.Once.On(tweakableEffect).Method("ReadFromXmlFile").With(node);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadPostEffectFromXml()
        {
            // Setup
            CreateEffectsAndPostEffectsForTarget(2, 4);
            ITweakableObject tweakableEffect = mockery.NewMock<ITweakableObject>();
            XmlNode node = CreateXmlNode("<PostEffect name=\"PostEffect3\"></PostEffect>");
            Expect.Once.On(factory).Method("CreateTweakableObject").With(postEffects[3]).Will(Return.Value(tweakableEffect));
            Expect.Once.On(tweakableEffect).Method("ReadFromXmlFile").With(node);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        private void CreateEffectsAndPostEffectsForTarget(int numEffects, int numPostEffects)
        {
            effects = new IDemoEffect[numEffects];
            postEffects = new IDemoPostEffect[numPostEffects];
            for (int i = 0; i < numEffects; i++)
            {
                effects[i] = mockery.NewMock<IDemoEffect>();
                Stub.On(effects[i]).GetProperty("Name").Will(Return.Value("Effect" + i));
            }
            for (int i = 0; i < numPostEffects; i++)
            {
                postEffects[i] = mockery.NewMock<IDemoPostEffect>();
                Stub.On(postEffects[i]).GetProperty("Name").Will(Return.Value("PostEffect" + i));
            }
            Stub.On(target).GetProperty("Effects").Will(Return.Value(effects));
            Stub.On(target).GetProperty("PostEffects").Will(Return.Value(postEffects));
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
