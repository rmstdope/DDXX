using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    [TestFixture]
    public class KeyFrameTest
    {
        [Test]
        public void Constructor()
        {
            KeyFrame keyFrame = new KeyFrame(10000000, Matrix.CreateFromYawPitchRoll(1, 2, 3));
            Assert.AreEqual(keyFrame.Time, 10000000);
            Assert.AreEqual(keyFrame.Transform, Matrix.CreateFromYawPitchRoll(1, 2, 3));
        }

    }
}
