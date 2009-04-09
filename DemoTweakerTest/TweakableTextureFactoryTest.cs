using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using NMock2;
using System.Xml;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoTweaker
{
    [TestFixture]
    public class TweakableTextureFactoryTest
    {
        private TweakableTextureFactory tweakable;
        private Mockery mockery;
        private ITextureFactory target;
        private ITweakableFactory factory;
        private IDemoEffectTypes effectTypes;
        private ITexture2D texture2D;
        List<Texture2DParameters> list;
        private XmlDocument document;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = mockery.NewMock<ITextureFactory>();
            factory = mockery.NewMock<ITweakableFactory>();
            effectTypes = mockery.NewMock<IDemoEffectTypes>();
            texture2D = mockery.NewMock<ITexture2D>();
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
            Assert.AreEqual(3, tweakable.NumVisableVariables);
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
            SetupTextures(1);
            XmlNode node = CreateXmlNode("<TextureFactory><Texture name=\"0\" /></TextureFactory>");
            ITweakable tweakableGenerator = mockery.NewMock<ITweakable>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0].Generator).Will(Return.Value(tweakableGenerator));
            Expect.Once.On(tweakableGenerator).Method("WriteToXmlFile");
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<TextureFactory><Texture name=\"0\"><Generator class=\"MockObject\" /></Texture></TextureFactory>", node.OuterXml);
        }

        [Test]
        public void WriteToXmlOneCreation()
        {
            // Setup
            SetupTextures(1);
            XmlNode node = CreateXmlNode("<TextureFactory></TextureFactory>");
            ITweakable tweakableGenerator = mockery.NewMock<ITweakable>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0].Generator).Will(Return.Value(tweakableGenerator));
            Expect.Once.On(tweakableGenerator).Method("WriteToXmlFile");
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<TextureFactory><Texture name=\"0\" width=\"1\" height=\"2\" miplevels=\"0\"><Generator class=\"MockObject\" /></Texture></TextureFactory>", node.OuterXml);
        }

        [Test]
        public void WriteToXmlOneCreationOneExisting()
        {
            // Setup
            SetupTextures(2);
            XmlNode node = CreateXmlNode(
@"<TextureFactory>
	<Texture name=""0"" />
</TextureFactory>");
            ITweakable tweakableGenerator = mockery.NewMock<ITweakable>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0].Generator).Will(Return.Value(tweakableGenerator));
            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[1].Generator).Will(Return.Value(tweakableGenerator));
            Expect.Exactly(2).On(tweakableGenerator).Method("WriteToXmlFile");
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual(
@"<TextureFactory>
	<Texture name=""0"">
		<Generator class=""MockObject"" />
	</Texture>
	<Texture name=""1"" width=""1"" height=""2"" miplevels=""0"">
		<Generator class=""MockObject"" />
	</Texture>
</TextureFactory>", node.OuterXml);
        }

        [Test]
        public void WriteToXmlMultipleGeneratorsGenerator()
        {
            // Setup
            list = new List<Texture2DParameters>();
            ITexture2D texture = mockery.NewMock<ITexture2D>();
            ITextureGenerator circle = new Circle();
            ITextureGenerator constant = new Constant();
            ITextureGenerator modulate = new Modulate();
            modulate.ConnectToInput(0, circle);
            modulate.ConnectToInput(1, constant);
            list.Add(new Texture2DParameters("Tex", texture, modulate));
            Stub.On(target).GetProperty("Texture2DParameters").Will(Return.Value(list));
            XmlNode node = CreateXmlNode(
@"<TextureFactory>
	<Texture name=""Tex"" />
</TextureFactory>");
            ITweakable tweakableGenerator = mockery.NewMock<ITweakable>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(circle).Will(Return.Value(tweakableGenerator));
            Expect.Once.On(factory).Method("CreateTweakableObject").With(constant).Will(Return.Value(tweakableGenerator));
            Expect.Once.On(factory).Method("CreateTweakableObject").With(modulate).Will(Return.Value(tweakableGenerator));
            Expect.Exactly(3).On(tweakableGenerator).Method("WriteToXmlFile");
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual(
@"<TextureFactory>
	<Texture name=""Tex"">
		<Generator class=""Constant"" />
		<Generator class=""Circle"" />
		<Generator class=""Modulate"" />
	</Texture>
</TextureFactory>", node.OuterXml);
        }

        [Test]
        public void WriteToXmlOldGeneratorRemoved()
        {
            // Setup
            SetupTextures(1);
            XmlNode node = CreateXmlNode("<TextureFactory><Texture name=\"0\"><Generator class=\"OldJunk\" /></Texture></TextureFactory>");
            ITweakable tweakableGenerator = mockery.NewMock<ITweakable>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0].Generator).Will(Return.Value(tweakableGenerator));
            Expect.Once.On(tweakableGenerator).Method("WriteToXmlFile");
            // Exercise SUT
            tweakable.WriteToXmlFile(document, node);
            // Verify
            Assert.AreEqual("<TextureFactory><Texture name=\"0\"><Generator class=\"MockObject\" /></Texture></TextureFactory>", node.OuterXml);
        }

        //[Test]
        //public void CreateNew()
        //{
        //    // Setup
        //    TweakerStatus status = new TweakerStatus(1, 1);
        //    SetupTextures(2);
        //    Expect.Once.On(target).Method("CreateFromGenerator").Will(Return.Value(texture2D));
        //    ///list.Add(new Texture2DParameters(i.ToString(), texture, generator));
        //    // Exercise SUT
        //    tweakable.InsertNew(status);
        //    // Verify
        //    Assert.AreEqual(2, status.Selection);
        //}

        private void SetupTextures(int num)
        {
            list = new List<Texture2DParameters>();
            for (int i = 0; i < num; i++)
            {
                ITexture2D texture = mockery.NewMock<ITexture2D>();
                Stub.On(texture).GetProperty("Width").Will(Return.Value(1));
                Stub.On(texture).GetProperty("Height").Will(Return.Value(2));
                Stub.On(texture).GetProperty("LevelCount").Will(Return.Value(3));
                ITextureGenerator generator = mockery.NewMock<ITextureGenerator>();
                Stub.On(generator).GetProperty("NumInputPins").Will(Return.Value(0));
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
