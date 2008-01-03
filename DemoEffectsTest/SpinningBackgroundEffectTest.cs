using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;
using Dope.DDXX.DemoFramework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class SpinningBackgroundEffectTest : DemoMockTest
    {
        private SpinningBackgroundEffect spin;
        private ITexture2D texture2;
        private ITexture2D texture3;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            spin = new SpinningBackgroundEffect("", 0, 10);
            texture2 = mockery.NewMock<ITexture2D>();
            texture3 = mockery.NewMock<ITexture2D>();
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
            spin.Initialize(graphicsFactory, effectFactory, textureFactory, mixer, postProcessor);
        }

        [Test]
        public void TestOneTextureLayer()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("file").Will(Return.Value(texture2D));
            spin.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("file", 2 * (float)Math.PI, new Color(1, 2, 3, 4)));

            ExpectSprite();
            spin.Initialize(graphicsFactory, effectFactory, textureFactory, mixer, postProcessor);
        }

        [Test]
        public void TestThreeTextureLayer()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("file1").Will(Return.Value(texture2D));
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("file1").Will(Return.Value(texture2));
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("file2").Will(Return.Value(texture3));
            spin.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("file1", 2, new Color(1, 2, 3, 4)));
            spin.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("file1", 1, new Color(4, 3, 2, 1)));
            spin.AddTextureLayer(new SpinningBackgroundEffect.TextureLayer("file2", -2, new Color(5, 6, 7, 8)));

            ExpectSprite();
            spin.Initialize(graphicsFactory, effectFactory, textureFactory, mixer, postProcessor);
        }

        [Test]
        public void TestRender1()
        {
            ExpectSprite();
            spin.Initialize(graphicsFactory, effectFactory, textureFactory, mixer, postProcessor);
            Expect.Once.On(spriteBatch).Method("Begin").With(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            Expect.Once.On(renderState).SetProperty("DepthBufferEnable").To(false);
            Expect.Once.On(spriteBatch).Method("End");
            spin.Render();
        }

        [Test]
        public void TestRender2()
        {
            int bbWidth2 = presentParameters.BackBufferWidth * 2;
            int bbHeight2 = presentParameters.BackBufferHeight * 2;
            Time.CurrentTime = 0.5f;
            Expect.Once.On(spriteBatch).Method("Begin").With(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            Expect.Once.On(renderState).SetProperty("DepthBufferEnable").To(false);
            Expect.Once.On(spriteBatch).Method("Draw").
                With(texture2D, new Rectangle(0, 0, bbWidth2, bbHeight2), new Rectangle(0, 0, 1, 10), new Color(1, 2, 3, 4), (float)(Math.PI / 2.0f), 
                new Vector2(0.5f, 5.0f), SpriteEffects.None, 1.0f);
            Expect.Once.On(spriteBatch).Method("Draw").
                With(texture2, new Rectangle(0, 0, bbWidth2, bbHeight2), new Rectangle(0, 0, 2, 20), new Color(4, 3, 2, 1), (float)(Math.PI / 1.0f),
                new Vector2(1.0f, 10.0f), SpriteEffects.None, 1.0f);
            Expect.Once.On(spriteBatch).Method("Draw").
                With(texture3, new Rectangle(0, 0, bbWidth2, bbHeight2), new Rectangle(0, 0, 3, 30), new Color(5, 6, 7, 8), (float)(-Math.PI / 2.0f),
                new Vector2(1.5f, 15.0f), SpriteEffects.None, 1.0f);
            Expect.Once.On(spriteBatch).Method("End");
            Stub.On(texture2D).GetProperty("Width").Will(Return.Value(1));
            Stub.On(texture2).GetProperty("Width").Will(Return.Value(2));
            Stub.On(texture3).GetProperty("Width").Will(Return.Value(3));
            Stub.On(texture2D).GetProperty("Height").Will(Return.Value(10));
            Stub.On(texture2).GetProperty("Height").Will(Return.Value(20));
            Stub.On(texture3).GetProperty("Height").Will(Return.Value(30));
            TestThreeTextureLayer();
            spin.Render();
        }

        private void ExpectSprite()
        {
            Expect.Once.On(graphicsFactory).
                Method("CreateSpriteBatch").
                Will(Return.Value(spriteBatch));
        }

    }
}
