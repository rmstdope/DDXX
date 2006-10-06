using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    //[TestFixture]
    //public class ParticleSystemNodeTest : D3DMockTest
    //{
    //    private ParticleSystemNode particleSystem;
    //    private IEffect effect;
    //    private EffectHandle technique;

    //    [SetUp]
    //    public override void SetUp()
    //    {
    //        base.SetUp();
    //        particleSystem = new ParticleSystemNode("PS");
    //        effect = mockery.NewMock<IEffect>();
    //        technique = EffectHandle.FromString("1");
    //        D3DDriver.EffectFactory = new EffectFactory(device, factory);
    //    }

    //    [TearDown]
    //    public override void TearDown()
    //    {
    //        base.TearDown();
    //    }

    //    [Test]
    //    public void TestInitialize()
    //    {
    //        // Effect creation
    //        Expect.Once.On(factory).
    //            Method("EffectFromFile").
    //            WithAnyArguments().
    //            Will(Return.Value(effect));
    //        // EffectHandler
    //        Expect.Once.On(effect).
    //            Method("FindNextValidTechnique").
    //            WithAnyArguments().
    //            Will(Return.Value(EffectHandle.FromString("1")));
    //        Stub.On(effect).
    //            Method("GetParameter").
    //            WithAnyArguments().
    //            Will(Return.Value(EffectHandle.FromString("1")));
    //        particleSystem.Initialize(100);
    //        Assert.AreEqual(100, particleSystem.NumParticles);
    //    }
    //}
}
