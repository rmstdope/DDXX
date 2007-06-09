using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using NMock2;
using Dope.DDXX.Utility;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class SpinningBackgroundEffectTest : D3DMockTest
    {
        private SpinningBackgroundEffect spin;
        private ISprite sprite;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Time.Initialize();

            spin = new SpinningBackgroundEffect(0, 10);
            sprite = mockery.NewMock<ISprite>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitialize()
        {
            ExpectSprite();
            spin.Initialize(graphicsFactory, device);
        }

        [Test]
        public void TestOneTextureLayer()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("file").Will(Return.Value(texture));
            spin.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("file", 2 * (float)Math.PI, Color.Aqua, 0.2f));

            ExpectSprite();
            spin.Initialize(graphicsFactory, device);
        }

        [Test]
        public void TestThreeTextureLayer()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("file1").Will(Return.Value(texture));
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("file1").Will(Return.Value(texture));
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("file2").Will(Return.Value(texture));
            spin.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("file1", 2, Color.Aqua, 0.2f));
            spin.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("file1", 1, Color.Aquamarine, 0.3f));
            spin.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("file2", -2, Color.Azure, 0.4f));

            ExpectSprite();
            spin.Initialize(graphicsFactory, device);
        }

        [Test]
        public void TestRender1()
        {
            ExpectSprite();
            spin.Initialize(graphicsFactory, device);
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(renderStateManager).SetProperty("ZBufferEnable").To(false);
            Expect.Once.On(sprite).Method("End");
            spin.Render();
        }

        [Test]
        public void TestRender2()
        {
            Time.Pause();
            Time.CurrentTime = 0.5f;
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(renderStateManager).SetProperty("ZBufferEnable").To(false);
            Expect.Once.On(sprite).Method("Draw2D").
                With(Is.EqualTo(texture), Is.EqualTo(Rectangle.Empty), Is.Anything, Is.Anything, new FloatMatcher((float)(Math.PI / 2.0f)), Is.Anything, Is.EqualTo(Color.FromArgb((int)(0.2f * 255), Color.Aqua)));
            Expect.Once.On(sprite).Method("Draw2D").
                With(Is.EqualTo(texture), Is.EqualTo(Rectangle.Empty), Is.Anything, Is.Anything, new FloatMatcher((float)Math.PI), Is.Anything, Is.EqualTo(Color.FromArgb((int)(0.3f * 255), Color.Aquamarine)));
            Expect.Once.On(sprite).Method("Draw2D").
                With(Is.EqualTo(texture), Is.EqualTo(Rectangle.Empty), Is.Anything, Is.Anything, new FloatMatcher((float)(-Math.PI / 2)), Is.Anything, Is.EqualTo(Color.FromArgb((int)(0.4f * 255), Color.Azure)));
            Expect.Once.On(sprite).Method("End");
            TestThreeTextureLayer();
            spin.Render();
        }

        private void ExpectSprite()
        {
            Expect.Once.On(graphicsFactory).
                Method("CreateSprite").
                WithAnyArguments().
                Will(Return.Value(sprite));
        }

    }
}
