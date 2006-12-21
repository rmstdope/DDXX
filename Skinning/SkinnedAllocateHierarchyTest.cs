using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Skinning
{
    [TestFixture]
    public class SkinnedAllocateHierarchyTest
    {
        SkinnedAllocateHierarchy alloc = new SkinnedAllocateHierarchy();

        [Test]
        public void TestFrame()
        {
            Frame frame = alloc.CreateFrame("Testname");
            Assert.AreEqual(frame.Name, "Testname");
        }
    }
}
