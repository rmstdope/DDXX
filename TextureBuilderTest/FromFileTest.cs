using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NMock2;
using NMock2.Actions;
using NMock2.Monitoring;
using System.IO;

namespace Dope.DDXX.TextureBuilder
{
    [TestFixture]
    public class FromFileTest : D3DMockTest
    {
        private FromFile fromFile;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            fromFile = new FromFile();
            fromFile.TextureFactory = textureFactory;
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void Getters()
        {
            // Setup
            fromFile.Filename = "fn";
            // Verify
            Assert.AreEqual("fn", fromFile.Filename);
            Assert.AreEqual(textureFactory, fromFile.TextureFactory);
        }

        [Test]
        public void Size1x1()
        {
            // Setup
            ExpectTexture(1, 1, new Color[] { new Color(1, 2, 3, 4) });
            // Exercise SUT
            Color p1 = new Color(fromFile.GetPixel(new Vector2(0, 0), Vector2.Zero));
            Color p2 = new Color(fromFile.GetPixel(new Vector2(1, 1), Vector2.Zero));
            // Verify
            Assert.AreEqual(new Color(1, 2, 3, 4), p1);
            Assert.AreEqual(new Color(1, 2, 3, 4), p2);
        }

        [Test]
        public void Size2x2NoResample()
        {
            // Setup
            ExpectTexture(2, 2, new Color[] { 
                new Color(1, 1, 1, 1),
                new Color(2, 2, 2, 2),
                new Color(3, 3, 3, 3),
                new Color(4, 4, 4, 4)
            });
            // Exercise SUT
            Color p1 = new Color(fromFile.GetPixel(new Vector2(0, 0), Vector2.Zero));
            Color p2 = new Color(fromFile.GetPixel(new Vector2(1, 0), Vector2.Zero));
            Color p3 = new Color(fromFile.GetPixel(new Vector2(0, 1), Vector2.Zero));
            Color p4 = new Color(fromFile.GetPixel(new Vector2(1, 1), Vector2.Zero));
            // Verify
            Assert.AreEqual(new Color(1, 1, 1, 1), p1);
            Assert.AreEqual(new Color(2, 2, 2, 2), p2);
            Assert.AreEqual(new Color(3, 3, 3, 3), p3);
            Assert.AreEqual(new Color(4, 4, 4, 4), p4);
        }

        [Test]
        public void Size2x2Upsample()
        {
            // Setup
            ExpectTexture(2, 2, new Color[] { 
                new Color(2, 2, 2, 2),
                new Color(4, 4, 4, 4),
                new Color(6, 6, 6, 6),
                new Color(8, 8, 8, 8)
            });
            // Exercise SUT
            Color p1 = new Color(fromFile.GetPixel(new Vector2(0.0f, 0.0f), Vector2.Zero));
            Color p2 = new Color(fromFile.GetPixel(new Vector2(0.5f, 0.0f), Vector2.Zero));
            Color p3 = new Color(fromFile.GetPixel(new Vector2(1.0f, 0.0f), Vector2.Zero));
            Color p4 = new Color(fromFile.GetPixel(new Vector2(0.0f, 0.5f), Vector2.Zero));
            Color p5 = new Color(fromFile.GetPixel(new Vector2(0.5f, 0.5f), Vector2.Zero));
            Color p6 = new Color(fromFile.GetPixel(new Vector2(1.0f, 0.5f), Vector2.Zero));
            Color p7 = new Color(fromFile.GetPixel(new Vector2(0.0f, 1.0f), Vector2.Zero));
            Color p8 = new Color(fromFile.GetPixel(new Vector2(0.5f, 1.0f), Vector2.Zero));
            Color p9 = new Color(fromFile.GetPixel(new Vector2(1.0f, 1.0f), Vector2.Zero));
            // Verify
            Assert.AreEqual(new Color(2, 2, 2, 2), p1);
            Assert.AreEqual(new Color(3, 3, 3, 3), p2);
            Assert.AreEqual(new Color(4, 4, 4, 4), p3);
            Assert.AreEqual(new Color(4, 4, 4, 4), p4);
            Assert.AreEqual(new Color(5, 5, 5, 5), p5);
            Assert.AreEqual(new Color(6, 6, 6, 6), p6);
            Assert.AreEqual(new Color(6, 6, 6, 6), p7);
            Assert.AreEqual(new Color(7, 7, 7, 7), p8);
            Assert.AreEqual(new Color(8, 8, 8, 8), p9);
        }

        [Test]
        public void Size3x3Downsample()
        {
            // Setup
            ExpectTexture(3, 3, new Color[] { 
                new Color(2, 2, 2, 2),
                new Color(4, 4, 4, 4),
                new Color(6, 6, 6, 6),
                new Color(6, 6, 6, 6),
                new Color(4, 4, 4, 4),
                new Color(2, 2, 2, 2),
                new Color(4, 4, 4, 4),
                new Color(2, 2, 2, 2),
                new Color(6, 6, 6, 6),
            });
            // Exercise SUT
            Color p1 = new Color(fromFile.GetPixel(new Vector2(0, 0), Vector2.Zero));
            Color p2 = new Color(fromFile.GetPixel(new Vector2(1, 0), Vector2.Zero));
            Color p3 = new Color(fromFile.GetPixel(new Vector2(0, 1), Vector2.Zero));
            Color p4 = new Color(fromFile.GetPixel(new Vector2(1, 1), Vector2.Zero));
            // Verify
            Assert.AreEqual(new Color(2, 2, 2, 2), p1);
            Assert.AreEqual(new Color(6, 6, 6, 6), p2);
            Assert.AreEqual(new Color(4, 4, 4, 4), p3);
            Assert.AreEqual(new Color(6, 6, 6, 6), p4);
        }

        private void ExpectTexture(int width, int height, Color[] data)
        {
            fromFile.Filename = "Name";
            Expect.Once.On(textureFactory).Method("CreateFromName").With("Name").Will(Return.Value(texture2D));
            Stub.On(texture2D).GetProperty("Width").Will(Return.Value(width));
            Stub.On(texture2D).GetProperty("Height").Will(Return.Value(height));
            Stub.On(texture2D).Method("GetData").Will(new SetArrayAction<Color>(data));
        }

        private class SetArrayAction<T> : IAction
        {
            private T[] source;
            public SetArrayAction(T[] source)
            {
                this.source = source;
            }

            public void Invoke(Invocation invocation)
            {
                T[] target = invocation.Parameters[0] as T[];
                source.CopyTo(target, 0);
            }

            public void DescribeTo(TextWriter writer)
            {
            }

        }
    }
}
