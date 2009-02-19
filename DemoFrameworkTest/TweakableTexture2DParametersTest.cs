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
using Dope.DDXX.UserInterface;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{    
    [TestFixture]
    public class TweakableTexture2DParametersTest
    {
        #region FakeControl
        private class FakeControl<T> : IMenuControl<T>
        {
            public List<T> Actions = new List<T>();
            public List<string> Texts = new List<string>();

            public T Action { get { throw new NotImplementedException(); } }
            public void AddOption(string text, T action) { Texts.Add(text); Actions.Add(action); }
            public void ClearOptions() { throw new NotImplementedException(); }
            public void Next() { throw new NotImplementedException(); }
            public int NumOptions { get { throw new NotImplementedException(); } }
            public void Previous() { throw new NotImplementedException(); }
            public int Selected 
            { 
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
            public byte Alpha 
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
            public void Draw(IDrawResources resources) { throw new NotImplementedException(); }
            public Color SelectedTextColor
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
            public string SubTitle
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
            public Color TextColor
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
            public ITexture2D Texture
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
            public string Title
            {
                get { throw new NotImplementedException(); }
                set { }
            }
            public void AddChild(IControl control) { throw new NotImplementedException(); }
            public void DrawControl(IDrawResources resources) { throw new NotImplementedException(); }
            public Vector4 Rectangle { get { throw new NotImplementedException(); } }
            public void RemoveChildren() { throw new NotImplementedException(); }
            public void RemoveFromParent() { throw new NotImplementedException(); }
            public float GetHeight(IDrawResources resources) { throw new NotImplementedException(); }
            public float GetWidth(IDrawResources resources) { throw new NotImplementedException(); }
            public float GetX1(IDrawResources resources) { throw new NotImplementedException(); }
            public float GetY1(IDrawResources resources) { throw new NotImplementedException(); }
            public void RemoveChild(IControl control) { throw new NotImplementedException(); }
        }
        #endregion
        
        private const string OneInputGenerator = "Madd";
        private const string TwoInputGenerator = "FactorBlend";
        private TweakableTexture2DParameters tweakable;
        private Mockery mockery;
        private Texture2DParameters target;
        private ITweakableFactory factory;
        private TweakerStatus status;
        private IDrawResources drawResources;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            factory = mockery.NewMock<ITweakableFactory>();
            drawResources = mockery.NewMock<IDrawResources>();
            status = new TweakerStatus(1, 1);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void NumSpecificVariablesIsOne()
        {
            // Setup
            FakeControl<int> control = new FakeControl<int>();
            ITextureGenerator generator = mockery.NewMock<ITextureGenerator>();
            Stub.On(generator).GetProperty("NumInputPins").Will(Return.Value(0));
            target = new Texture2DParameters("Test", null, generator);
            tweakable = new TweakableTexture2DParameters(target, factory);
            // Exercise SUT
            Assert.AreEqual(1, tweakable.NumVariables);
        }

        [Test]
        public void NumSpecificVariablesIsFour()
        {
            // Setup
            FakeControl<int> control = new FakeControl<int>();
            ITextureGenerator generator1 = mockery.NewMock<ITextureGenerator>();
            ITextureGenerator generator2 = mockery.NewMock<ITextureGenerator>();
            ITextureGenerator generator3 = mockery.NewMock<ITextureGenerator>();
            ITextureGenerator generator4 = mockery.NewMock<ITextureGenerator>();
            Stub.On(generator1).GetProperty("NumInputPins").Will(Return.Value(2));
            Stub.On(generator2).GetProperty("NumInputPins").Will(Return.Value(1));
            Stub.On(generator3).GetProperty("NumInputPins").Will(Return.Value(0));
            Stub.On(generator4).GetProperty("NumInputPins").Will(Return.Value(0));
            Stub.On(generator1).Method("GetInput").With(0).Will(Return.Value(generator2));
            Stub.On(generator1).Method("GetInput").With(1).Will(Return.Value(generator3));
            Stub.On(generator2).Method("GetInput").With(0).Will(Return.Value(generator4));
            target = new Texture2DParameters("Test", null, generator1);
            tweakable = new TweakableTexture2DParameters(target, factory);
            // Exercise SUT
            Assert.AreEqual(4, tweakable.NumVariables);
        }

        [Test]
        public void InsertNew()
        {
            // Setup
            FakeControl<int> control = new FakeControl<int>();
            ITextureGenerator generator = mockery.NewMock<ITextureGenerator>();
            Stub.On(generator).GetProperty("NumInputPins").Will(Return.Value(0));
            target = new Texture2DParameters("Test", null, generator);
            tweakable = new TweakableTexture2DParameters(target, factory);
            Expect.Once.On(factory).Method("CreateMenuControl").Will(Return.Value(control));
            // Exercise SUT
            Assert.AreEqual(control, tweakable.InsertNew(status, drawResources, false));
            foreach (string generatorName in control.Texts)
                Assert.AreNotEqual(TwoInputGenerator, generatorName);
        }

        //        [Test]
//        public void NumVisable()
//        {
//            // Exercise SUT and verify
//            Assert.AreEqual(5, tweakable.NumVisableVariables);
//        }

//        [Test]
//        public void NumVariables()
//        {
//            // Setup
//            SetupTextures(2);
//            // Exercise SUT and verify
//            Assert.AreEqual(2, tweakable.NumVariables);
//        }

//        [Test]
//        public void ChildVariables()
//        {
//            // Setup
//            SetupTextures(2);
//            ITweakable expectedChild = mockery.NewMock<ITweakable>();
//            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0]).Will(Return.Value(expectedChild));
//            // Exercise SUT
//            ITweakable child = tweakable.GetChild(0);
//            // Verify
//            Assert.AreSame(expectedChild, child);
//        }

//        [Test]
//        public void ReadFromXmlNoTextures()
//        {
//            // Setup
//            XmlNode node = CreateXmlNode("<TextureFactory></TextureFactory>");
//            // Exercise SUT and verify
//            tweakable.ReadFromXmlFile(node);
//        }

//        [Test]
//        [ExpectedException(typeof(DDXXException))]
//        public void ReadFromXmlTextureMissingAttributes()
//        {
//            // Setup
//            XmlNode node = CreateXmlNode("<TextureFactory><Texture/></TextureFactory>");
//            // Exercise SUT and verify
//            tweakable.ReadFromXmlFile(node);
//        }

//        [Test]
//        [ExpectedException(typeof(DDXXException))]
//        public void ReadFromXmlTextureNoGenerator()
//        {
//            // Setup
//            XmlNode node = CreateXmlNode("<TextureFactory><Texture name=\"x\" width=\"1\" height=\"2\" miplevels=\"3\"/></TextureFactory>");
//            // Exercise SUT and verify
//            tweakable.ReadFromXmlFile(node);
//        }

//        [Test]
//        [ExpectedException(typeof(DDXXException))]
//        public void ReadFromXmlClasslessGenerator()
//        {
//            // Setup
//            XmlNode node = CreateXmlNode("<TextureFactory><Texture name='x' width='1' height='2' miplevels='3'><Generator /></Texture></TextureFactory>");
//            // Exercise SUT and verify
//            tweakable.ReadFromXmlFile(node);
//        }

//        [Test]
//        [ExpectedException(typeof(DDXXException))]
//        public void ReadFromXmlUnknownChild()
//        {
//            // Setup
//            XmlNode node = CreateXmlNode("<TextureFactory><Texture name='x' width='1' height='2' miplevels='3'> <X /></Texture></TextureFactory>");
//            // Exercise SUT and verify
//            tweakable.ReadFromXmlFile(node);
//        }

//        [Test]
//        public void ReadFromXmlTextureSimpleGenerator()
//        {
//            // Setup
//            XmlNode node = CreateXmlNode("<TextureFactory><Texture name='x' width='1' height='2' miplevels='3'><Generator class='y' /></Texture></TextureFactory>");
//            ITextureGenerator generator = mockery.NewMock<ITextureGenerator>();
//            ITweakable tweakableChild = mockery.NewMock<ITweakable>();
//            Stub.On(generator).GetProperty("NumInputPins").Will(Return.Value(0));
//            Expect.Once.On(effectTypes).Method("CreateGenerator").With("y").Will(Return.Value(generator));
//            Expect.Once.On(factory).Method("CreateTweakableObject").With(generator).Will(Return.Value(tweakableChild));
//            Expect.Once.On(tweakableChild).Method("ReadFromXmlFile").With(node.FirstChild.FirstChild);
//            Expect.Once.On(target).Method("CreateFromGenerator").With("x", 1, 2, 3, TextureUsage.None, SurfaceFormat.Color, generator);
//            // Exercise SUT and verify
//            tweakable.ReadFromXmlFile(node);
//        }

//        [Test]
//        public void WriteToXmlNoTextures()
//        {
//            // Setup
//            SetupTextures(0);
//            XmlNode node = CreateXmlNode("<TextureFactory></TextureFactory>");
//            // Exercise SUT
//            tweakable.WriteToXmlFile(document, node);
//            // Verify
//            Assert.AreEqual("<TextureFactory></TextureFactory>", node.OuterXml);
//        }

//        [Test]
//        public void WriteToXmlOneUpdate()
//        {
//            // Setup
//            SetupTextures(1);
//            XmlNode node = CreateXmlNode("<TextureFactory><Texture name=\"0\" /></TextureFactory>");
//            ITweakable tweakableGenerator = mockery.NewMock<ITweakable>();
//            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0].Generator).Will(Return.Value(tweakableGenerator));
//            Expect.Once.On(tweakableGenerator).Method("WriteToXmlFile");
//            // Exercise SUT
//            tweakable.WriteToXmlFile(document, node);
//            // Verify
//            Assert.AreEqual("<TextureFactory><Texture name=\"0\"><Generator class=\"MockObject\" /></Texture></TextureFactory>", node.OuterXml);
//        }

//        [Test]
//        public void WriteToXmlOneCreation()
//        {
//            // Setup
//            SetupTextures(1);
//            XmlNode node = CreateXmlNode("<TextureFactory></TextureFactory>");
//            ITweakable tweakableGenerator = mockery.NewMock<ITweakable>();
//            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0].Generator).Will(Return.Value(tweakableGenerator));
//            Expect.Once.On(tweakableGenerator).Method("WriteToXmlFile");
//            // Exercise SUT
//            tweakable.WriteToXmlFile(document, node);
//            // Verify
//            Assert.AreEqual("<TextureFactory><Texture name=\"0\" width=\"1\" height=\"2\" miplevels=\"0\"><Generator class=\"MockObject\" /></Texture></TextureFactory>", node.OuterXml);
//        }

//        [Test]
//        public void WriteToXmlOneCreationOneExisting()
//        {
//            // Setup
//            SetupTextures(2);
//            XmlNode node = CreateXmlNode(
//@"<TextureFactory>
//	<Texture name=""0"" />
//</TextureFactory>");
//            ITweakable tweakableGenerator = mockery.NewMock<ITweakable>();
//            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0].Generator).Will(Return.Value(tweakableGenerator));
//            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[1].Generator).Will(Return.Value(tweakableGenerator));
//            Expect.Exactly(2).On(tweakableGenerator).Method("WriteToXmlFile");
//            // Exercise SUT
//            tweakable.WriteToXmlFile(document, node);
//            // Verify
//            Assert.AreEqual(
//@"<TextureFactory>
//	<Texture name=""0"">
//		<Generator class=""MockObject"" />
//	</Texture>
//	<Texture name=""1"" width=""1"" height=""2"" miplevels=""0"">
//		<Generator class=""MockObject"" />
//	</Texture>
//</TextureFactory>", node.OuterXml);
//        }

//        [Test]
//        public void WriteToXmlMultipleGeneratorsGenerator()
//        {
//            // Setup
//            list = new List<Texture2DParameters>();
//            ITexture2D texture = mockery.NewMock<ITexture2D>();
//            ITextureGenerator circle = new Circle();
//            ITextureGenerator constant = new Constant();
//            ITextureGenerator modulate = new Modulate();
//            modulate.ConnectToInput(0, circle);
//            modulate.ConnectToInput(1, constant);
//            list.Add(new Texture2DParameters("Tex", texture, modulate));
//            Stub.On(target).GetProperty("Texture2DParameters").Will(Return.Value(list));
//            XmlNode node = CreateXmlNode(
//@"<TextureFactory>
//	<Texture name=""Tex"" />
//</TextureFactory>");
//            ITweakable tweakableGenerator = mockery.NewMock<ITweakable>();
//            Expect.Once.On(factory).Method("CreateTweakableObject").With(circle).Will(Return.Value(tweakableGenerator));
//            Expect.Once.On(factory).Method("CreateTweakableObject").With(constant).Will(Return.Value(tweakableGenerator));
//            Expect.Once.On(factory).Method("CreateTweakableObject").With(modulate).Will(Return.Value(tweakableGenerator));
//            Expect.Exactly(3).On(tweakableGenerator).Method("WriteToXmlFile");
//            // Exercise SUT
//            tweakable.WriteToXmlFile(document, node);
//            // Verify
//            Assert.AreEqual(
//@"<TextureFactory>
//	<Texture name=""Tex"">
//		<Generator class=""Constant"" />
//		<Generator class=""Circle"" />
//		<Generator class=""Modulate"" />
//	</Texture>
//</TextureFactory>", node.OuterXml);
//        }

//        [Test]
//        public void WriteToXmlOldGeneratorRemoved()
//        {
//            // Setup
//            SetupTextures(1);
//            XmlNode node = CreateXmlNode("<TextureFactory><Texture name=\"0\"><Generator class=\"OldJunk\" /></Texture></TextureFactory>");
//            ITweakable tweakableGenerator = mockery.NewMock<ITweakable>();
//            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0].Generator).Will(Return.Value(tweakableGenerator));
//            Expect.Once.On(tweakableGenerator).Method("WriteToXmlFile");
//            // Exercise SUT
//            tweakable.WriteToXmlFile(document, node);
//            // Verify
//            Assert.AreEqual("<TextureFactory><Texture name=\"0\"><Generator class=\"MockObject\" /></Texture></TextureFactory>", node.OuterXml);
//        }

//        //[Test]
//        //public void CreateNew()
//        //{
//        //    // Setup
//        //    TweakerStatus status = new TweakerStatus(1, 1);
//        //    SetupTextures(2);
//        //    Expect.Once.On(target).Method("CreateFromGenerator").Will(Return.Value(texture2D));
//        //    ///list.Add(new Texture2DParameters(i.ToString(), texture, generator));
//        //    // Exercise SUT
//        //    tweakable.InsertNew(status);
//        //    // Verify
//        //    Assert.AreEqual(2, status.Selection);
//        //}

//        private void SetupTextures(int num)
//        {
//            list = new List<Texture2DParameters>();
//            for (int i = 0; i < num; i++)
//            {
//                ITexture2D texture = mockery.NewMock<ITexture2D>();
//                Stub.On(texture).GetProperty("Width").Will(Return.Value(1));
//                Stub.On(texture).GetProperty("Height").Will(Return.Value(2));
//                Stub.On(texture).GetProperty("LevelCount").Will(Return.Value(3));
//                ITextureGenerator generator = mockery.NewMock<ITextureGenerator>();
//                Stub.On(generator).GetProperty("NumInputPins").Will(Return.Value(0));
//                list.Add(new Texture2DParameters(i.ToString(), texture, generator));
//            }
//            Stub.On(target).GetProperty("Texture2DParameters").Will(Return.Value(list));
//        }

//        private XmlNode CreateXmlNode(string xml)
//        {
//            document = new XmlDocument();
//            document.PreserveWhitespace = true;
//            document.LoadXml(xml);
//            return document.DocumentElement;
//        }

    }
}
