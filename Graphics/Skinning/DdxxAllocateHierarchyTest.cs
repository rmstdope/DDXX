using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    [TestFixture]
    public class DdxxAllocateHierarchyTest
    {
        DdxxAllocateHierarchy alloc = new DdxxAllocateHierarchy();

        [Test]
        public void TestFrame()
        {
            Frame frame = alloc.CreateFrame("Testname");
            Assert.AreEqual(frame.Name, "Testname");
        }
    }
}
