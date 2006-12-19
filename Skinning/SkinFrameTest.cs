using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Skinning
{
    [TestFixture]
    public class SkinFrameTest
    {
        [Test]
        public void TestConstructor()
        {
            SkinFrame frame = new SkinFrame("TestName");
            Assert.AreEqual("TestName", frame.Name);

            frame = new SkinFrame();
            Assert.AreEqual("Unnamed", frame.Name);
        }
    }
}
