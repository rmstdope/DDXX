using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using NMock2;
using System.Xml;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableTextureFactoryTest
    {
        private TweakableTextureFactory tweakable;
        private Mockery mockery;
        private ITextureFactory target;
        private ITweakableFactory factory;
        private IDemoEffectTypes effectTypes;
        List<Texture2DParameters> list;
        private XmlDocument document;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = mockery.NewMock<ITextureFactory>();
            factory = mockery.NewMock<ITweakableFactory>();
            effectTypes = mockery.NewMock<IDemoEffectTypes>();
            tweakable = new TweakableTextureFactory(target, factory);
            Stub.On(factory).GetProperty("EffectTypes").Will(Return.Value(effectTypes));
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void EmptyFunctions()
        {
            // Exercise SUT
            //tweakable.DecreaseValue(5);
            //tweakable.IncreaseValue(10);
            //tweakable.NextIndex(null);
            //tweakable.SetFromString("x");
            //tweakable.SetValue(null);
        }

        [Test]
        public void NumVisable()
        {
            // Exercise SUT and verify
            Assert.AreEqual(5, tweakable.NumVisableVariables);
        }

        [Test]
        public void NumVariables()
        {
            // Setup
            SetupTextures(2);
            // Exercise SUT and verify
            Assert.AreEqual(2, tweakable.NumVariables);
        }

        [Test]
        public void ChildVariables()
        {
            // Setup
            SetupTextures(2);
            ITweakable expectedChild = mockery.NewMock<ITweakable>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0]).Will(Return.Value(expectedChild));
            // Exercise SUT
            ITweakable child = tweakable.GetChild(0);
            // Verify
            Assert.AreSame(expectedChild, child);
        }

        [Test]
        public void ReadFromXmlNoTextures()
        {
            // Setup
            XmlNode node = CreateXmlNode("<TextureFactory></TextureFactory>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ReadFromXmlTextureMissingAttributes()
        {
            // Setup
            XmlNode node = CreateXmlNode("<TextureFactory><Texture/></TextureFactory>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ReadFromXmlTextureNoGenerator()
        {
            // Setup
            XmlNode node = CreateXmlNode("<TextureFactory><Texture name=\"x\" width=\"1\" height=\"2\" miplevels=\"3\"/></TextureFactory>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ReadFromXmlClasslessGenerator()
        {
            // Setup
            XmlNode node = CreateXmlNode("<TextureFactory><Texture name='x' width='1' height='2' miplevels='3'><Generator /></Texture></TextureFactory>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ReadFromXmlUnknownChild()
        {
            // Setup
            XmlNode node = CreateXmlNode("<TextureFactory><Texture name='x' width='1' height='2' miplevels='3'> <X /></Texture></TextureFactory>");
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void ReadFromXmlTextureSimpleGenerator()
        {
            // Setup
            XmlNode node = CreateXmlNode("<TextureFactory><Texture name='x' width='1' height='2' miplevels='3'><Generator class='y' /></Texture></TextureFactory>");
            ITextureGenerator generator = mockery.NewMock<ITextureGenerator>();
            ITweakable tweakableChild = mockery.NewMock<ITweakable>();
            Stub.On(generator).GetProperty("NumInputPins").Will(Return.Value(0));
            Expect.Once.On(effectTypes).Method("CreateGenerator").With("y").Will(Return.Value(generator));
            Expect.Once.On(factory).Method("CreateTweakableObject").With(generator).Will(Return.Value(tweakableChild));
            Expect.Once.On(tweakableChild).Method("ReadFromXmlFile").With(node.FirstChild.FirstChild);
            Expect.Once.On(target).Method("CreateFromGenerator").With("x", 1, 2, 3, TextureUsage.None, SurfaceFormat.Color, generator);
            // Exercise SUT and verify
            tweakable.ReadFromXmlFile(node);
        }

        [Test]
        public void WriteToXmlNoTextures()
        {
            // Setup
            SetupTextures(0);
            XmlNode node = CreateXmlNode("<TextureFactory></TextureFactory>");
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<TextureFactory></TextureFactory>", node.OuterXml);
        }

        [Test]
        public void WriteToXmlOneUpdate()
        {
            // Setup
            //ITweakable expectedChild = mockery.NewMock<ITweakable>();
            SetupTextures(1);
            XmlNode node = CreateXmlNode("<TextureFactory><Texture name=\"0\" /></TextureFactory>");
            //Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0]).Will(Return.Value(expectedChild));
            //Expect.Once.On(expectedChild).Method("WriteToXmlFile").With(document, node.FirstChild);
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<TextureFactory><Texture name=\"0\" /></TextureFactory>", node.OuterXml);
        }

        [Test]
        public void WriteToXmlOneCreation()
        {
            // Setup
            //ITweakable expectedChild = mockery.NewMock<ITweakable>();
            SetupTextures(1);
            XmlNode node = CreateXmlNode("<TextureFactory></TextureFactory>");
            //Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0]).Will(Return.Value(expectedChild));
            //Expect.Once.On(expectedChild).Method("WriteToXmlFile").With(Is.EqualTo(document), Is.Anything);
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<TextureFactory><Texture name=\"0\" /></TextureFactory>", node.OuterXml);
        }

        [Test]
        public void WriteToXmlOneCreationOneExisting()
        {
            // Setup
            //ITweakable expectedChild0 = mockery.NewMock<ITweakable>();
            //ITweakable expectedChild1 = mockery.NewMock<ITweakable>();
            SetupTextures(2);
            XmlNode node = CreateXmlNode(
@"<TextureFactory>
    <Texture name=""0"" />
</TextureFactory>");
            //Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0]).Will(Return.Value(expectedChild0));
            //Expect.Once.On(factory).Method("CreateTweakableObject").With(list[1]).Will(Return.Value(expectedChild1));
            //Expect.Once.On(expectedChild0).Method("WriteToXmlFile").With(Is.EqualTo(document), Is.Anything);
            //Expect.Once.On(expectedChild1).Method("WriteToXmlFile").With(Is.EqualTo(document), Is.Anything);
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual(
@"<TextureFactory>
    <Texture name=""0"" />
    <Texture name=""1"" />
</TextureFactory>", node.OuterXml);
        }

//        [Test]
//        public void WriteToXmlOneUpdate()
//        {
//            // Setup
//            SetupTextures(1);
//            XmlNode node = CreateXmlNode("<TextureFactory><Texture name=\"0\" /></TextureFactory>");
//            // Exercise SUT
//            tweakable.WriteToXmlFile(document, node);
//            // Verify
//            Assert.AreEqual(
//@"<TextureFactory>
//    <Texture name=""0"">
//        <Generator class=""Circle"">
//            <InnerRadius>1</InnerRadius>
//            <OuterRadius>1</OuterRadius>
//        </Generator>
//    </Texture>
//</TextureFactory>", node.OuterXml);
//        }

        private void SetupTextures(int num)
        {
            list = new List<Texture2DParameters>();
            for (int i = 0; i < num; i++)
            {
                ITexture2D texture = mockery.NewMock<ITexture2D>();
                ITextureGenerator generator = mockery.NewMock<ITextureGenerator>();
                list.Add(new Texture2DParameters(i.ToString(), texture, generator));
            }
            Stub.On(target).GetProperty("Texture2DParameters").Will(Return.Value(list));
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
