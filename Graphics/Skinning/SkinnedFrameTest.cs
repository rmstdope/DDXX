using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Graphics.Skinning
{
    [TestFixture]
    public class SkinnedFrameTest
    {
        [Test]
        public void TestConstructor()
        {
            SkinnedFrame frame = new SkinnedFrame("TestName");
            Assert.AreEqual("TestName", frame.Name);
        }
    }
}
