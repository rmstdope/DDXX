﻿using System;
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
            public T action;

            public FakeControl(T action)
            {
                this.action = action;
            }

            public List<T> Actions = new List<T>();
            public List<string> Texts = new List<string>();

            public T Action { get { return action; } }
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

        private Type ZeroInputGenerator;
        private Type OneInputGenerator;
        private Type TwoInputGenerator;
        private TweakableTexture2DParameters tweakable;
        private Mockery mockery;
        private Texture2DParameters target;
        private ITweakableFactory factory;
        private TweakerStatus status;
        private IDrawResources drawResources;
        private FakeControl<Type> control;
        private ITextureGenerator perlinNoiseGenerator;
        private ITextureGenerator colorModulationGenerator;
        private ITextureGenerator sineGenerator;
        private ITextureGenerator marbleGenerator;
        private ITextureGenerator addGenerator;

        [SetUp]
        public void SetUp()
        {
            ZeroInputGenerator = typeof(PerlinNoise);
            OneInputGenerator = typeof(Madd);
            TwoInputGenerator = typeof(FactorBlend);
            mockery = new Mockery();
            factory = mockery.NewMock<ITweakableFactory>();
            drawResources = mockery.NewMock<IDrawResources>();
            status = new TweakerStatus(1, 1);
            Stub.On(factory).Method("CreateTweakableValue").Will(Return.Value(null));
            control = new FakeControl<Type>(typeof(ColorModulation));
            perlinNoiseGenerator = new PerlinNoise();
            colorModulationGenerator = new ColorModulation();
            marbleGenerator = new Marble();
            addGenerator = new Add();
            sineGenerator = new Sine();
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
            CreateTweakable(perlinNoiseGenerator);
            // Exercise SUT
            Assert.AreEqual(1, tweakable.NumVariables);
        }

        [Test]
        public void NumSpecificVariablesIsFour()
        {
            // Setup
            colorModulationGenerator.ConnectToInput(0, perlinNoiseGenerator);
            addGenerator.ConnectToInput(0, colorModulationGenerator);
            addGenerator.ConnectToInput(1, marbleGenerator);
            CreateTweakable(addGenerator);
            // Exercise SUT
            Assert.AreEqual(4, tweakable.NumVariables);
        }

        [Test]
        public void InsertNew()
        {
            // Setup
            CreateTweakable(perlinNoiseGenerator);
            Expect.Once.On(factory).Method("CreateMenuControl").Will(Return.Value(control));
            // Exercise SUT
            Assert.AreEqual(control, tweakable.InsertNew(status, drawResources));
            Assert.IsFalse(control.Actions.Contains(ZeroInputGenerator));
            Assert.IsTrue(control.Actions.Contains(OneInputGenerator));
            Assert.IsTrue(control.Actions.Contains(TwoInputGenerator));
        }

        [Test]
        public void AddGeneratorAtEnd()
        {
            // Setup
            CreateTweakable(perlinNoiseGenerator);
            Expect.Once.On(factory).Method("CreateMenuControl").Will(Return.Value(control));
            tweakable.InsertNew(status, drawResources);
            // Exercise SUT
            tweakable.ChoiceMade(status, 0);
            // Verify
            Assert.IsInstanceOfType(typeof(ColorModulation), target.Generator);
            Assert.AreSame(perlinNoiseGenerator, target.Generator.GetInput(0));
        }

        [Test]
        public void AddGeneratorInBetween()
        {
            // Setup
            colorModulationGenerator.ConnectToInput(0, perlinNoiseGenerator);
            CreateTweakable(colorModulationGenerator);
            Expect.Once.On(factory).Method("CreateMenuControl").Will(Return.Value(control));
            tweakable.InsertNew(status, drawResources);
            // Exercise SUT
            tweakable.ChoiceMade(status, 0);
            // Verify
            Assert.AreSame(colorModulationGenerator, target.Generator);
            Assert.IsInstanceOfType(typeof(ColorModulation), target.Generator.GetInput(0));
            Assert.AreSame(perlinNoiseGenerator, target.Generator.GetInput(0).GetInput(0));
        }

        [Test]
        public void AddTwoInputGenerator()
        {
            // Setup
            CreateTweakable(perlinNoiseGenerator);
            Expect.Exactly(2).On(factory).Method("CreateMenuControl").Will(Return.Value(control));
            tweakable.InsertNew(status, drawResources);
            control.action = typeof(Add);
            // Exercise SUT and verify
            Assert.AreSame(control, tweakable.ChoiceMade(status, 0));
        }

        [Test]
        public void AddTwoInputGeneratorCont()
        {
            // Setup
            AddTwoInputGenerator();
            control.action = typeof(Marble);
            // Exercise SUT and verify
            Assert.IsNull(tweakable.ChoiceMade(status, 0));
            // Verify
            Assert.IsInstanceOfType(typeof(Add), target.Generator);
            Assert.AreSame(perlinNoiseGenerator, target.Generator.GetInput(0));
            Assert.IsInstanceOfType(typeof(Marble), target.Generator.GetInput(1));
        }

        [Test]
        public void DeleteLastInChain()
        {
            // Setup
            colorModulationGenerator.ConnectToInput(0, perlinNoiseGenerator);
            CreateTweakable(colorModulationGenerator);
            // Exercise SUT
            status.Selection = 1;
            tweakable.Delete(status);
            // Verify
            Assert.AreSame(perlinNoiseGenerator, target.Generator);
        }

        [Test]
        public void DeleteMiddleOfChain()
        {
            // Setup
            sineGenerator.ConnectToInput(0, colorModulationGenerator);
            colorModulationGenerator.ConnectToInput(0, perlinNoiseGenerator);
            CreateTweakable(sineGenerator);
            // Exercise SUT
            status.Selection = 1;
            tweakable.Delete(status);
            // Verify
            Assert.AreSame(sineGenerator, target.Generator);
            Assert.AreSame(perlinNoiseGenerator, target.Generator.GetInput(0));
        }

        private void CreateTweakable(ITextureGenerator rootGenerator)
        {
            target = new Texture2DParameters("Test", null, rootGenerator);
            tweakable = new TweakableTexture2DParameters(target, factory);
        }

    }
}
