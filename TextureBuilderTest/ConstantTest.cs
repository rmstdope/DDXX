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
            Vector4[,] data = constant.GenerateTexture(1, 1);
            Assert.AreEqual(Vector4.Zero, data[0, 0]);
        }

        [Test]
        public void NotZero()
        {
            // Setup
            Constant constant = new Constant();
            constant.Color = new Vector4(1, 2, 3, 4);
            // Verify
            Vector4[,] data = constant.GenerateTexture(1, 1);
            Assert.AreEqual(new Vector4(1, 2, 3, 4), data[0, 0]);
        }
    
    }
}
