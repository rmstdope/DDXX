using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TrackTest : DemoMockTest
    {
        private Track track;
        private IEffectChangeListener effectChangeListener;
        private ITextureBuilder textureBuilder;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            track = new Track();
            Time.Initialize();
            effectChangeListener = mockery.NewMock<IEffectChangeListener>();
            textureBuilder = mockery.NewMock<ITextureBuilder>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestSingleEffectRegistration()
        {
            IDemoEffect e1 = CreateMockEffect(0, 10);

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
            IDemoPostEffect e1 = CreateMockPostEffect(0, 10);

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
            IDemoEffect e1 = CreateMockEffect(0, 10);
            IDemoEffect e2 = CreateMockEffect(5, 15);

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
            IDemoPostEffect e1 = CreateMockPostEffect(0, 10);
            IDemoPostEffect e2 = CreateMockPostEffect(5, 15);

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
            IDemoEffect e1 = CreateMockEffect(5, 10);
            IDemoEffect e2 = CreateMockEffect(0, 15);
            IDemoEffect e3 = CreateMockEffect(10, 15);
            IDemoEffect e4 = CreateMockEffect(0, 10);
            IDemoEffect e5 = CreateMockEffect(5, 15);

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
            IDemoPostEffect e1 = CreateMockPostEffect(5, 10);
            IDemoPostEffect e2 = CreateMockPostEffect(0, 15);
            IDemoPostEffect e3 = CreateMockPostEffect(10, 15);
            IDemoPostEffect e4 = CreateMockPostEffect(0, 10);
            IDemoPostEffect e5 = CreateMockPostEffect(5, 15);

            track.Register(e1);
            track.Register(e2);
            track.Register(e3);
            track.Register(e4);
            track.Register(e5);

            Assert.AreEqual(new IDemoPostEffect[] { e4, e2, e1, e5, e3 }, track.PostEffects);

            Assert.AreEqual(new IDemoPostEffect[] { e4, e2, e1, e5 }, track.GetPostEffects(5.0f));
        }

        [Test]
        public void TestEndTimeOneEffect()
        {
            track.Register(CreateMockEffect(5, 10));
            Assert.AreEqual(10, track.EndTime);
        }

        [Test]
        public void TestEndTimeMoreEffects()
        {
            track.Register(CreateMockEffect(5, 10));
            track.Register(CreateMockEffect(5, 15));
            track.Register(CreateMockEffect(5, 12));
            Assert.AreEqual(15, track.EndTime);
        }

        [Test]
        public void TestEndTimeSingleEffectAndPostEffect()
        {
            track.Register(CreateMockEffect(5, 10));
            track.Register(CreateMockPostEffect(5, 12));
            Assert.AreEqual(12, track.EndTime);
        }

        [Test]
        public void TestEndTimeMoreEffectAndPostEffect()
        {
            track.Register(CreateMockEffect(5, 10));
            track.Register(CreateMockEffect(5, 15));
            track.Register(CreateMockPostEffect(5, 12));
            track.Register(CreateMockPostEffect(5, 18));
            Assert.AreEqual(18, track.EndTime);
        }

        [Test]
        public void TestInitializeOneEffectAndPostEffect()
        {
            IDemoEffect e1 = CreateMockEffect(5, 10);
            track.Register(e1);
            IDemoPostEffect pe1 = CreateMockPostEffect(5, 12);
            track.Register(pe1);
            Expect.Once.On(e1).Method("Initialize").With(graphicsFactory, device);
            Expect.Once.On(pe1).Method("Initialize").With(postProcessor, textureFactory, textureBuilder, device);
            track.Initialize(graphicsFactory, device, textureFactory, textureBuilder, postProcessor);
        }

        [Test]
        public void TestInitializeMoreEffectAndPostEffect()
        {
            for (int i = 0; i < 50; i++)
            {
                IDemoEffect e1 = CreateMockEffect(5, 10);
                track.Register(e1);
                Expect.Once.On(e1).Method("Initialize").With(graphicsFactory, device);
                IDemoPostEffect pe1 = CreateMockPostEffect(5, 12);
                track.Register(pe1);
                Expect.Once.On(pe1).Method("Initialize").With(postProcessor, textureFactory, textureBuilder, device);
            }
            track.Initialize(graphicsFactory, device, textureFactory, textureBuilder, postProcessor);
        }

        [Test]
        public void TestStepOneEffectWithinTime()
        {
            Time.Pause();
            Time.CurrentTime = 6.0f;
            IDemoEffect e1 = CreateMockEffect(5, 10);
            track.Register(e1);
            Expect.Once.On(e1).Method("Step");
            track.Step();
        }

        [Test]
        public void TestStepOneEffectOutsideTime()
        {
            Time.Pause();
            Time.CurrentTime = 1.0f;
            IDemoEffect e1 = CreateMockEffect(5, 10);
            track.Register(e1);
            track.Step();
        }

        [Test]
        public void TestStepMoreEffects()
        {
            Time.Pause();
            Time.CurrentTime = 6.0f;
            IDemoEffect e1 = CreateMockEffect(4, 10);
            track.Register(e1);
            Expect.Once.On(e1).Method("Step");
            e1 = CreateMockEffect(1, 19);
            track.Register(e1);
            Expect.Once.On(e1).Method("Step");
            e1 = CreateMockEffect(7, 10);
            track.Register(e1);
            e1 = CreateMockEffect(3, 5);
            track.Register(e1);
            track.Step();
        }

        [Test]
        public void TestRenderNoEffects()
        {
            using (mockery.Ordered)
            {
                Expect.Once.On(device).Method("BeginScene");
                Expect.Once.On(device).Method("EndScene");
            }
            track.Render(device);
        }

        [Test]
        public void TestRenderOneEffect()
        {
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoEffect e1 = CreateMockEffect(4, 10);
            track.Register(e1);
            using (mockery.Ordered)
            {
                Expect.Once.On(device).Method("BeginScene");
                Expect.Once.On(e1).Method("Render");
                Expect.Once.On(device).Method("EndScene");
            }
            track.Render(device);
        }

        [Test]
        public void TestRenderOnePostEffect()
        {
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoPostEffect pe1 = CreateMockPostEffect(4, 10);
            track.Register(pe1);
            Stub.On(pe1).GetProperty("DrawOrder").Will(Return.Value(0));
            using (mockery.Ordered)
            {
                Expect.Once.On(device).Method("BeginScene");
                Expect.Once.On(device).Method("EndScene");
                Expect.Once.On(pe1).Method("Render");
            }
            track.Render(device);
        }

        [Test]
        public void TestRenderDrawOrder1()
        {
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoPostEffect pe1 = CreateMockPostEffect(4, 10);
            track.Register(pe1);
            Stub.On(pe1).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoPostEffect pe2 = CreateMockPostEffect(5, 10);
            track.Register(pe2);
            Stub.On(pe2).GetProperty("DrawOrder").Will(Return.Value(1));
            using (mockery.Ordered)
            {
                Expect.Once.On(device).Method("BeginScene");
                Expect.Once.On(device).Method("EndScene");
                Expect.Once.On(pe1).Method("Render");
                Expect.Once.On(pe2).Method("Render");
            }
            track.Render(device);
        }

        [Test]
        public void TestRenderDrawOrder2()
        {
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoPostEffect pe1 = CreateMockPostEffect(4, 10);
            track.Register(pe1);
            Stub.On(pe1).GetProperty("DrawOrder").Will(Return.Value(1));
            IDemoPostEffect pe2 = CreateMockPostEffect(5, 10);
            track.Register(pe2);
            Stub.On(pe2).GetProperty("DrawOrder").Will(Return.Value(0));
            using (mockery.Ordered)
            {
                Expect.Once.On(device).Method("BeginScene");
                Expect.Once.On(device).Method("EndScene");
                Expect.Once.On(pe2).Method("Render");
                Expect.Once.On(pe1).Method("Render");
            }
            track.Render(device);
        }

        [Test]
        public void TestRenderDrawOrderMulti()
        {
            Time.Pause();
            Time.CurrentTime = 8.0f;
            List<IDemoPostEffect> postEffects = new List<IDemoPostEffect>();
            for (int i = 0; i < 10; i++)
            {
                IDemoPostEffect pe = CreateMockPostEffect(4, 10);
                track.Register(pe);
                Stub.On(pe).GetProperty("DrawOrder").Will(Return.Value(i));
                postEffects.Add(pe);
            }
            using (mockery.Ordered)
            {
                Expect.Once.On(device).Method("BeginScene");
                Expect.Once.On(device).Method("EndScene");
                for (int i = 0; i < 10; i++)
                {
                    Expect.Once.On(postEffects[i]).Method("Render");
                }
            }
            track.Render(device);
        }

        [Test]
        public void TestRenderMoreEffects()
        {
            Time.Pause();
            Time.CurrentTime = 8.0f;
            IDemoEffect e1 = CreateMockEffect(4, 10);
            track.Register(e1);
            IDemoEffect e2 = CreateMockEffect(5, 10);
            track.Register(e2);
            IDemoPostEffect pe1 = CreateMockPostEffect(4, 10);
            track.Register(pe1);
            Stub.On(pe1).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoPostEffect pe2 = CreateMockPostEffect(5, 10);
            track.Register(pe2);
            Stub.On(pe2).GetProperty("DrawOrder").Will(Return.Value(0));
            using (mockery.Ordered)
            {
                Expect.Once.On(device).Method("BeginScene");
                Expect.Once.On(e1).Method("Render");
                Expect.Once.On(e2).Method("Render");
                Expect.Once.On(device).Method("EndScene");
                Expect.Once.On(pe1).Method("Render");
                Expect.Once.On(pe2).Method("Render");
            }
            track.Render(device);
        }

        [Test]
        public void TestRenderOnlyWithinTime()
        {
            Time.Pause();
            Time.CurrentTime = 4.0f;
            IDemoEffect e1 = CreateMockEffect(4, 10);
            track.Register(e1);
            IDemoEffect e2 = CreateMockEffect(5, 10);
            track.Register(e2);
            IDemoPostEffect pe1 = CreateMockPostEffect(4, 10);
            track.Register(pe1);
            Stub.On(pe1).GetProperty("DrawOrder").Will(Return.Value(0));
            IDemoPostEffect pe2 = CreateMockPostEffect(5, 10);
            track.Register(pe2);
            Stub.On(pe2).GetProperty("DrawOrder").Will(Return.Value(0));
            using (mockery.Ordered)
            {
                Expect.Once.On(device).Method("BeginScene");
                Expect.Once.On(e1).Method("Render");
                Expect.Once.On(device).Method("EndScene");
                Expect.Once.On(pe1).Method("Render");
            }
            track.Render(device);
        }

        [Test]
        public void TestUpdateListenerOneEffect()
        {
            IDemoEffect e1 = CreateMockEffect(4, 10);
            track.Register(e1);
            Expect.Once.On(e1).Method("UpdateListener").With(effectChangeListener);
            track.UpdateListener(effectChangeListener);
        }

        [Test]
        public void TestUpdateListenerOnePostEffect()
        {
            IDemoPostEffect pe1 = CreateMockPostEffect(4, 10);
            track.Register(pe1);
            Expect.Once.On(pe1).Method("UpdateListener").With(effectChangeListener);
            track.UpdateListener(effectChangeListener);
        }

        [Test]
        public void TestUpdateListenerMoreEffect()
        {
            for (int i = 0; i < 10; i++)
            {
                IDemoEffect e1 = CreateMockEffect(4, 10);
                track.Register(e1);
                Expect.Once.On(e1).Method("UpdateListener").With(effectChangeListener);
                IDemoPostEffect pe1 = CreateMockPostEffect(4, 10);
                track.Register(pe1);
                Expect.Once.On(pe1).Method("UpdateListener").With(effectChangeListener);
            }
            track.UpdateListener(effectChangeListener);
        }
    }
}
