using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TrackTest : DemoMockTest
    {
        private Track track;
        private IEffectChangeListener effectChangeListener;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            track = new Track();
            effectChangeListener = mockery.NewMock<IEffectChangeListener>();
            Stub.On(graphicsFactory).Method("CreateSpriteBatch").Will(Return.Value(spriteBatch));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestSingleEffectRegistration()
        {
            IDemoEffect e1 = CreateMockEffect("name", 0, 10);

            track.Register(e1);

            Assert.AreEqual(new IDemoEffect[] { e1 }, track.Effects);

            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(-5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(0.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(10.0f));
            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(15.0f));

            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(-5.0f, -0.1f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(-5.0f, 0.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(0.0f, 5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(10.0f, 15.0f));
            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(11.0f, 15.0f));
        }

        [Test]
        public void TestSinglePostEffectRegistration()
        {
            IDemoPostEffect e1 = CreateMockPostEffect("name", 0, 10);

            track.Register(e1);

            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.PostEffects);

            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(-5.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(0.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(5.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(10.0f));
            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(15.0f));

            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(-5.0f, -0.1f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(-5.0f, 0.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(0.0f, 5.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(10.0f, 15.0f));
            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(11.0f, 15.0f));
        }

        [Test]
        public void TestMultipleEffectRegistrations()
        {
            IDemoEffect e1 = CreateMockEffect("name", 0, 10);
            IDemoEffect e2 = CreateMockEffect("name", 5, 15);

            track.Register(e1);
            track.Register(e2);

            Assert.AreEqual(new IDemoEffect[] { e1, e2 }, track.Effects);

            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(-5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(0.0f));
            Assert.AreEqual(new IDemoEffect[] { e1, e2 }, track.GetEffects(5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1, e2 }, track.GetEffects(10.0f));
            Assert.AreEqual(new IDemoEffect[] { e2 }, track.GetEffects(15.0f));
            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(20.0f));

            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(-5.0f, -1.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(-1.0f, 3.0f));
            Assert.AreEqual(new IDemoEffect[] { e1, e2 }, track.GetEffects(3.0f, 6.0f));
            Assert.AreEqual(new IDemoEffect[] { e2 }, track.GetEffects(11.0f, 16.0f));
            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(16.0f, 17.0f));
        }
        [Test]
        public void TestMultiplePostEffectRegistrations()
        {
            IDemoPostEffect e1 = CreateMockPostEffect("name", 0, 10);
            IDemoPostEffect e2 = CreateMockPostEffect("name", 5, 15);

            track.Register(e1);
            track.Register(e2);

            Assert.AreEqual(new IDemoPostEffect[] { e1, e2 }, track.PostEffects);

            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(-5.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(0.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1, e2 }, track.GetPostEffects(5.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1, e2 }, track.GetPostEffects(10.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e2 }, track.GetPostEffects(15.0f));
            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(20.0f));

            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(-5.0f, -1.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(-1.0f, 3.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1, e2 }, track.GetPostEffects(3.0f, 6.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e2 }, track.GetPostEffects(11.0f, 16.0f));
            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(16.0f, 17.0f));
        }

        [Test]
        public void TestEffectSorting()
        {
            IDemoEffect e1 = CreateMockEffect("name", 5, 10);
            IDemoEffect e2 = CreateMockEffect("name", 0, 15);
            IDemoEffect e3 = CreateMockEffect("name", 10, 15);
            IDemoEffect e4 = CreateMockEffect("name", 0, 10);
            IDemoEffect e5 = CreateMockEffect("name", 5, 15);

            track.Register(e1);
            track.Register(e2);
            track.Register(e3);
            track.Register(e4);
            track.Register(e5);

            Assert.AreEqual(new IDemoEffect[] { e4, e2, e1, e5, e3 }, track.Effects);

            Assert.AreEqual(new IDemoEffect[] { e4, e2, e1, e5 }, track.GetEffects(5.0f));
        }
        [Test]
        public void TestPostEffectSorting()
        {
            IDemoPostEffect e1 = CreateMockPostEffect("name", 5, 10);
            IDemoPostEffect e2 = CreateMockPostEffect("name", 0, 15);
            IDemoPostEffect e3 = CreateMockPostEffect("name", 10, 15);
            IDemoPostEffect e4 = CreateMockPostEffect("name", 0, 10);
            IDemoPostEffect e5 = CreateMockPostEffect("name", 5, 15);

            track.Register(e1);
            track.Register(e2);
            track.Register(e3);
            track.Register(e4);
            track.Register(e5);

            Assert.AreEqual(new IDemoPostEffect[] { e4, e2, e1, e5, e3 }, track.PostEffects);

            Assert.AreEqual(new IDemoPostEffect[] { e4, e2, e1, e5 }, track.GetPostEffects(5.0f));
        }

        [Test]
        public void TestStartTimeOneEffect()
        {
            track.Register(CreateMockEffect("name", 5, 10));
            Assert.AreEqual(5, track.StartTime);
        }

        [Test]
        public void TestEndTimeOneEffect()
        {
            track.Register(CreateMockEffect("name", 5, 10));
            Assert.AreEqual(10, track.EndTime);
        }

        [Test]
        public void TestStartTimeMoreEffects()
        {
            track.Register(CreateMockEffect("name", 6, 10));
            track.Register(CreateMockEffect("name", 2, 15));
            track.Register(CreateMockEffect("name", 5, 12));
            Assert.AreEqual(2, track.StartTime);
        }

        [Test]
        public void TestEndTimeMoreEffects()
        {
            track.Register(CreateMockEffect("name", 5, 10));
            track.Register(CreateMockEffect("name", 5, 15));
            track.Register(CreateMockEffect("name", 5, 12));
            Assert.AreEqual(15, track.EndTime);
        }

        [Test]
        public void TestStartTimeSingleEffectAndPostEffect()
        {
            track.Register(CreateMockEffect("name", 2, 10));
            track.Register(CreateMockPostEffect("name", 1, 12));
            Assert.AreEqual(1, track.StartTime);
        }

        [Test]
        public void TestEndTimeSingleEffectAndPostEffect()
        {
            track.Register(CreateMockEffect("name", 5, 10));
            track.Register(CreateMockPostEffect("name", 5, 12));
            Assert.AreEqual(12, track.EndTime);
        }

        [Test]
        public void TestStartTimeMoreEffectAndPostEffect()
        {
            track.Register(CreateMockEffect("name", 5, 10));
            track.Register(CreateMockEffect("name", 2, 15));
            track.Register(CreateMockPostEffect("name", 1, 12));
            track.Register(CreateMockPostEffect("name", 7, 18));
            Assert.AreEqual(1, track.StartTime);
        }

        [Test]
        public void TestEndTimeMoreEffectAndPostEffect()
        {
            track.Register(CreateMockEffect("name", 5, 10));
            track.Register(CreateMockEffect("name", 5, 15));
            track.Register(CreateMockPostEffect("name", 5, 12));
            track.Register(CreateMockPostEffect("name", 5, 18));
            Assert.AreEqual(18, track.EndTime);
        }

        [Test]
        public void TestInitializeOneEffectAndPostEffect()
        {
            IDemoEffect e1 = CreateMockEffect("name", 5, 10);
            track.Register(e1);
            IDemoPostEffect pe1 = CreateMockPostEffect("name", 5, 12);
            track.Register(pe1);
            ExpectInitialize(e1);
            ExpectInitialize(pe1);
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
        }

        [Test]
        public void TestInitializeMoreEffectAndPostEffect()
        {
            for (int i = 0; i < 10; i++)
            {
                IDemoEffect e1 = CreateMockEffect("name", 5, 10);
                track.Register(e1);
                ExpectInitialize(e1);
                IDemoPostEffect pe1 = CreateMockPostEffect("name", 5, 12);
                track.Register(pe1);
                ExpectInitialize(pe1);
            }
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
        }

        [Test]
        public void TestStepOneEffectWithinTime()
        {
            Time.Pause();
            Time.CurrentTime = 6.0f;
            IDemoEffect e1 = CreateMockEffect("name", 5, 10);
            track.Register(e1);
            Expect.Once.On(e1).Method("Step");
            track.Step();
        }

        [Test]
        public void TestStepOneEffectOutsideTime()
        {
            Time.Pause();
            Time.CurrentTime = 1.0f;
            IDemoEffect e1 = CreateMockEffect("name", 5, 10);
            track.Register(e1);
            track.Step();
        }

        [Test]
        public void TestStepMoreEffects()
        {
            Time.Pause();
            Time.CurrentTime = 6.0f;
            IDemoEffect e1 = CreateMockEffect("name", 4, 10);
            track.Register(e1);
            Expect.Once.On(e1).Method("Step");
            e1 = CreateMockEffect("name", 1, 19);
            track.Register(e1);
            Expect.Once.On(e1).Method("Step");
            e1 = CreateMockEffect("name", 7, 10);
            track.Register(e1);
            e1 = CreateMockEffect("name", 3, 5);
            track.Register(e1);
            track.Step();
        }

        [Test]
        public void TestRenderNoEffects()
        {
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
            using (mockery.Ordered)
            {
                ExpectRenderStart(Color.Aquamarine);
                ExpectRenderEffectEnd();
                Expect.Once.On(postProcessor).Method("StartFrame").With(renderTarget);
                ExpectRenderEnd();
            }
            Assert.AreSame(renderTarget, track.Render(device, renderTarget, renderTarget, depthStencilBuffer, Color.Aquamarine));
        }

        [Test]
        public void TestRenderOneEffect()
        {
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoEffect e1 = CreateMockEffect("name", 4, 10);
            track.Register(e1);
            Stub.On(e1).GetProperty("DrawOrder").Will(Return.Value(0));
            using (mockery.Ordered)
            {
                ExpectRenderStart(Color.DarkSlateBlue);
                Expect.Once.On(e1).Method("Render");
                ExpectRenderEffectEnd();
                Expect.Once.On(postProcessor).Method("StartFrame").With(renderTarget);
                ExpectRenderEnd();
            }
            Assert.AreSame(renderTarget, track.Render(device, renderTarget, renderTarget, depthStencilBuffer, Color.DarkSlateBlue));
        }

        [Test]
        public void TestRenderEffectDrawOrder1()
        {
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoEffect e1 = CreateMockEffect("name", 4, 10);
            track.Register(e1);
            Stub.On(e1).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoEffect e2 = CreateMockEffect("name", 5, 10);
            track.Register(e2);
            Stub.On(e2).GetProperty("DrawOrder").Will(Return.Value(1));
            using (mockery.Ordered)
            {
                ExpectRenderStart(Color.DarkSlateBlue);
                Expect.Once.On(e1).Method("Render");
                Expect.Once.On(e2).Method("Render");
                ExpectRenderEffectEnd();
                Expect.Once.On(postProcessor).Method("StartFrame").With(renderTarget);
                ExpectRenderEnd();
            }
            Assert.AreSame(renderTarget, track.Render(device, renderTarget, renderTarget, depthStencilBuffer, Color.DarkSlateBlue));
        }

        [Test]
        public void TestRenderEffectDrawOrder2()
        {
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoEffect e1 = CreateMockEffect("name", 4, 10);
            track.Register(e1);
            Stub.On(e1).GetProperty("DrawOrder").Will(Return.Value(1));
            IDemoEffect e2 = CreateMockEffect("name", 5, 10);
            track.Register(e2);
            Stub.On(e2).GetProperty("DrawOrder").Will(Return.Value(0));
            using (mockery.Ordered)
            {
                ExpectRenderStart(Color.DarkSlateBlue);
                Expect.Once.On(e2).Method("Render");
                Expect.Once.On(e1).Method("Render");
                ExpectRenderEffectEnd();
                Expect.Once.On(postProcessor).Method("StartFrame").With(renderTarget);
                ExpectRenderEnd();
            }
            Assert.AreSame(renderTarget, track.Render(device, renderTarget, renderTarget, depthStencilBuffer, Color.DarkSlateBlue));
        }

        [Test]
        public void TestRenderOnePostEffect()
        {
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoPostEffect pe1 = CreateMockPostEffect("name", 4, 10);
            track.Register(pe1);
            Stub.On(pe1).GetProperty("DrawOrder").Will(Return.Value(0));
            using (mockery.Ordered)
            {
                ExpectRenderStart(Color.DarkSlateBlue);
                ExpectRenderEffectEnd();
                Expect.Once.On(postProcessor).Method("StartFrame").With(renderTarget);
                Expect.Once.On(pe1).Method("Render");
                ExpectRenderEnd();
            }
            Assert.AreSame(renderTarget, track.Render(device, renderTarget, renderTarget, depthStencilBuffer, Color.DarkSlateBlue));
        }

        [Test]
        public void TestRenderPostEffectDrawOrder1()
        {
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoPostEffect pe1 = CreateMockPostEffect("name", 4, 10);
            track.Register(pe1);
            Stub.On(pe1).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoPostEffect pe2 = CreateMockPostEffect("name", 5, 10);
            track.Register(pe2);
            Stub.On(pe2).GetProperty("DrawOrder").Will(Return.Value(1));
            using (mockery.Ordered)
            {
                ExpectRenderStart(Color.DarkSlateBlue);
                ExpectRenderEffectEnd();
                Expect.Once.On(postProcessor).Method("StartFrame").With(renderTarget);
                Expect.Once.On(pe1).Method("Render");
                Expect.Once.On(pe2).Method("Render");
                ExpectRenderEnd();
            }
            Assert.AreSame(renderTarget, track.Render(device, renderTarget, renderTarget, depthStencilBuffer, Color.DarkSlateBlue));
        }

        [Test]
        public void TestRenderPostEffectDrawOrder2()
        {
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoPostEffect pe1 = CreateMockPostEffect("name", 4, 10);
            track.Register(pe1);
            Stub.On(pe1).GetProperty("DrawOrder").Will(Return.Value(1));
            IDemoPostEffect pe2 = CreateMockPostEffect("name", 5, 10);
            track.Register(pe2);
            Stub.On(pe2).GetProperty("DrawOrder").Will(Return.Value(0));
            using (mockery.Ordered)
            {
                ExpectRenderStart(Color.DarkSlateBlue);
                ExpectRenderEffectEnd();
                Expect.Once.On(postProcessor).Method("StartFrame").With(renderTarget);
                Expect.Once.On(pe2).Method("Render");
                Expect.Once.On(pe1).Method("Render");
                ExpectRenderEnd();
            }
            Assert.AreSame(renderTarget, track.Render(device, renderTarget, renderTarget, depthStencilBuffer, Color.DarkSlateBlue));
        }

        [Test]
        public void TestRenderPostEffectDrawOrderMulti()
        {
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
            Time.Pause();
            Time.CurrentTime = 8.0f;
            List<IDemoPostEffect> postEffects = new List<IDemoPostEffect>();
            for (int i = 0; i < 10; i++)
            {
                IDemoPostEffect pe = CreateMockPostEffect("name", 4, 10);
                track.Register(pe);
                Stub.On(pe).GetProperty("DrawOrder").Will(Return.Value(i));
                postEffects.Add(pe);
            }
            using (mockery.Ordered)
            {
                ExpectRenderStart(Color.DarkSlateBlue);
                ExpectRenderEffectEnd();
                Expect.Once.On(postProcessor).Method("StartFrame").With(renderTarget);
                for (int i = 0; i < 10; i++)
                {
                    Expect.Once.On(postEffects[i]).Method("Render");
                }
                ExpectRenderEnd();
            }
            Assert.AreSame(renderTarget, track.Render(device, renderTarget, renderTarget, depthStencilBuffer, Color.DarkSlateBlue));
        }

        [Test]
        public void TestRenderMoreEffects()
        {
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoEffect e1 = CreateMockEffect("name", 4, 10);
            track.Register(e1);
            Stub.On(e1).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoEffect e2 = CreateMockEffect("name", 5, 10);
            track.Register(e2);
            Stub.On(e2).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoPostEffect pe1 = CreateMockPostEffect("name", 4, 10);
            track.Register(pe1);
            Stub.On(pe1).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoPostEffect pe2 = CreateMockPostEffect("name", 5, 10);
            track.Register(pe2);
            Stub.On(pe2).GetProperty("DrawOrder").Will(Return.Value(0));
            using (mockery.Ordered)
            {
                ExpectRenderStart(Color.DarkSlateBlue);
                Expect.Once.On(e1).Method("Render");
                Expect.Once.On(e2).Method("Render");
                ExpectRenderEffectEnd();
                Expect.Once.On(postProcessor).Method("StartFrame").With(renderTarget);
                Expect.Once.On(pe1).Method("Render");
                Expect.Once.On(pe2).Method("Render");
                ExpectRenderEnd();
            }
            Assert.AreSame(renderTarget, track.Render(device, renderTarget, renderTarget, depthStencilBuffer, Color.DarkSlateBlue));
        }

        [Test]
        public void TestRenderOnlyWithinTime()
        {
            track.Initialize(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, mixer, postProcessor);
            Time.Pause();
            Time.CurrentTime = 4.0f;
            IDemoEffect e1 = CreateMockEffect("name", 4, 10);
            track.Register(e1);
            Stub.On(e1).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoEffect e2 = CreateMockEffect("name", 5, 10);
            track.Register(e2);
            Stub.On(e2).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoPostEffect pe1 = CreateMockPostEffect("name", 4, 10);
            track.Register(pe1);
            Stub.On(pe1).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoPostEffect pe2 = CreateMockPostEffect("name", 5, 10);
            track.Register(pe2);
            Stub.On(pe2).GetProperty("DrawOrder").Will(Return.Value(0));
            using (mockery.Ordered)
            {
                ExpectRenderStart(Color.DarkSlateBlue);
                Expect.Once.On(e1).Method("Render");
                ExpectRenderEffectEnd();
                Expect.Once.On(postProcessor).Method("StartFrame").With(renderTarget);
                Expect.Once.On(pe1).Method("Render");
                ExpectRenderEnd();
            }
            Assert.AreSame(renderTarget, track.Render(device, renderTarget, renderTarget, depthStencilBuffer, Color.DarkSlateBlue));
        }

        //[Test]
        //public void TestUpdateListenerOneEffect()
        //{
        //    IDemoEffect e1 = CreateMockEffect("name", 4, 10);
        //    track.Register(e1);
        //    Expect.Once.On(e1).Method("UpdateListener").With(effectChangeListener);
        //    track.UpdateListener(effectChangeListener);
        //}

        //[Test]
        //public void TestUpdateListenerOnePostEffect()
        //{
        //    IDemoPostEffect pe1 = CreateMockPostEffect("name", 4, 10);
        //    track.Register(pe1);
        //    Expect.Once.On(pe1).Method("UpdateListener").With(effectChangeListener);
        //    track.UpdateListener(effectChangeListener);
        //}

        //[Test]
        //public void TestUpdateListenerMoreEffect()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        IDemoEffect e1 = CreateMockEffect("name", 4, 10);
        //        track.Register(e1);
        //        Expect.Once.On(e1).Method("UpdateListener").With(effectChangeListener);
        //        IDemoPostEffect pe1 = CreateMockPostEffect("name", 4, 10);
        //        track.Register(pe1);
        //        Expect.Once.On(pe1).Method("UpdateListener").With(effectChangeListener);
        //    }
        //    track.UpdateListener(effectChangeListener);
        //}

        [Test]
        public void IsEffectRegisteredTrue()
        {
            // Setup
            IDemoEffect e1 = CreateMockEffect("name", 4, 10);
            // Exercise SUT
            track.Register(e1);
            // Verify
            Assert.IsTrue(track.IsEffectRegistered("name", e1.GetType()));
        }

        [Test]
        public void IsEffectRegisteredIncorrectType()
        {
            // Setup
            IDemoEffect e1 = CreateMockEffect("name", 4, 10);
            // Exercise SUT
            track.Register(e1);
            // Verify
            Assert.IsFalse(track.IsEffectRegistered("name", typeof(float)));
        }

        [Test]
        public void IsEffectRegisteredIncorrectName()
        {
            // Setup
            IDemoEffect e1 = CreateMockEffect("name", 4, 10);
            // Exercise SUT
            track.Register(e1);
            // Verify
            Assert.IsFalse(track.IsEffectRegistered("unknownname", e1.GetType()));
        }

        [Test]
        public void IsPostEffectRegisteredTrue()
        {
            // Setup
            IDemoPostEffect e1 = CreateMockPostEffect("name", 4, 10);
            // Exercise SUT
            track.Register(e1);
            // Verify
            Assert.IsTrue(track.IsPostEffectRegistered("name", e1.GetType()));
        }

        [Test]
        public void IsPostEffectRegisteredIncorrectType()
        {
            // Setup
            IDemoPostEffect e1 = CreateMockPostEffect("name", 4, 10);
            // Exercise SUT
            track.Register(e1);
            // Verify
            Assert.IsFalse(track.IsPostEffectRegistered("name", typeof(float)));
        }

        [Test]
        public void IsPostEffectRegisteredIncorrectName()
        {
            // Setup
            IDemoPostEffect e1 = CreateMockPostEffect("name", 4, 10);
            // Exercise SUT
            track.Register(e1);
            // Verify
            Assert.IsFalse(track.IsPostEffectRegistered("unknownname", e1.GetType()));
        }

        private void ExpectRenderStart(Color color)
        {
            Expect.Once.On(device).Method("SetRenderTarget").With(0, renderTarget);
            Expect.Once.On(device).SetProperty("DepthStencilBuffer").To(depthStencilBuffer);
            Expect.Once.On(device).Method("Clear").
                With(ClearOptions.Target | ClearOptions.DepthBuffer, color, 1.0f, 0);
        }

        private void ExpectRenderEffectEnd()
        {
            Expect.Once.On(device).Method("SetRenderTarget").With(0, null);
            Expect.Once.On(device).SetProperty("DepthStencilBuffer").To(Is.Null);
        }

        private void ExpectRenderEnd()
        {
            //Expect.Once.On(device).Method("SetRenderTarget").With(0, originalSurface);
            //Expect.Once.On(originalSurface).Method("Dispose");
            Expect.Once.On(postProcessor).GetProperty("OutputTexture").Will(Return.Value(renderTarget));
        }

        private void ExpectInitialize(IDemoPostEffect pe1)
        {
            Expect.Once.On(pe1).Method("Initialize").With(graphicsFactory, postProcessor, textureFactory, textureBuilder);
        }

        private void ExpectInitialize(IDemoEffect e1)
        {
            Expect.Once.On(e1).Method("Initialize").With(graphicsFactory, effectFactory, textureFactory, mixer, postProcessor);
        }

    }
}
