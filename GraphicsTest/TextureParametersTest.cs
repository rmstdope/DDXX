using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class TextureParametersTest
    {
        [Test]
        public void FromFile2D()
        {
            // Setup
            Mockery mockery = new Mockery();
            ITexture2D texture = mockery.NewMock<ITexture2D>();
            // Exercise SUT
            Texture2DParameters parameters = new Texture2DParameters("Filename", texture);
            // Verify
            Assert.AreEqual("Filename", parameters.Name);
            Assert.AreSame(texture, parameters.Texture);
        }

        [Test]
        public void FromFileCube()
        {
            // Setup
            Mockery mockery = new Mockery();
            ITextureCube texture = mockery.NewMock<ITextureCube>();
            // Exercise SUT
            TextureCubeParameters parameters = new TextureCubeParameters("Filename", texture);
            // Verify
            Assert.AreEqual("Filename", parameters.Name);
            Assert.AreSame(texture, parameters.Texture);
        }

    }
}
