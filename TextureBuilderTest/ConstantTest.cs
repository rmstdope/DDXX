using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Dope.DDXX.TextureBuilder
{
    [TestFixture]
    public class ConstantTest
    {

        [Test]
        public void Zero()
        {
            // Setup
            Constant constant = new Constant();
            constant.Color = new Vector4();
            // Verify
            Assert.AreEqual(new Vector4(), constant.GetPixel(new Vector2(), Vector2.Zero));
        }

        [Test]
        public void NotZero()
        {
            // Setup
            Constant constant = new Constant();
            constant.Color = new Vector4(1, 2, 3, 4);
            // Verify
            Assert.AreEqual(new Vector4(1, 2, 3, 4), constant.GetPixel(new Vector2(3, 3), Vector2.Zero));
        }
    
    }
}
