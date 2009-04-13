using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using NMock2;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.UserInterface;
using Microsoft.Xna.Framework;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoTweaker
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
            public int DrawControl(IDrawResources resources) { throw new NotImplementedException(); }
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
            Stub.On(factory).Method("CreateTweakableValue").Will(Return.Value(null));
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
            FakeControl<Type> control = new FakeControl<Type>();
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
            FakeControl<Type> control = new FakeControl<Type>();
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
            FakeControl<Type> control = new FakeControl<Type>();
            ITextureGenerator generator = mockery.NewMock<ITextureGenerator>();
            Stub.On(generator).GetProperty("NumInputPins").Will(Return.Value(0));
            target = new Texture2DParameters("Test", null, generator);
            tweakable = new TweakableTexture2DParameters(target, factory);
            Expect.Once.On(factory).Method("CreateMenuControl").Will(Return.Value(control));
            // Exercise SUT
            Assert.AreEqual(control, tweakable.InsertNew(status, drawResources));
            foreach (string generatorName in control.Texts)
                Assert.AreNotEqual(TwoInputGenerator, generatorName);
        }

    }
}
