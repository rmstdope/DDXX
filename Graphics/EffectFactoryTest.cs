using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NUnit.Framework;
using NMock2;

namespace Graphics
{

    [TestFixture]
    public class EffectFactoryTest
    {
        EffectFactory effectFactory;

        Mockery mockery;
        IFactory factory;
        IEffect effect1;
        IEffect effect2;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            effect1 = mockery.NewMock<IEffect>();
            effect2 = mockery.NewMock<IEffect>();
            factory = mockery.NewMock<IFactory>();
            effectFactory = new EffectFactory(null, factory);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateFromFileTest()
        {
            Expect.Once.On(factory).
                Method("CreateEffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect1));
            IEffect newEffect1 = effectFactory.CreateFromFile("fileName");
            Assert.AreSame(effect1, newEffect1);
            IEffect newEffect2 = effectFactory.CreateFromFile("fileName");
            Assert.AreSame(newEffect1, newEffect2);

            Expect.Once.On(factory).
                Method("CreateEffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect2));
            IEffect newEffect3 = effectFactory.CreateFromFile("fileName2");
            Assert.AreSame(effect2, newEffect3);
        }
    }
}
