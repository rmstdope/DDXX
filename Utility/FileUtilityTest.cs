using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class FileUtilityTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestInitialize()
        {
            FileUtility.Initialize("aaa", "bbb");
            Assert.AreEqual(new string[] {"aaa", "bbb"}, FileUtility.LoadPaths());

            FileUtility.Initialize("ccc", "ddd", "eee");
            Assert.AreEqual(new string[] { "ccc", "ddd", "eee" }, FileUtility.LoadPaths());
        }

        [Test]
        public void TestFilePath()
        {
            FileUtility.Initialize("../", "../../");
            Assert.AreEqual("../../FileUtilityTest.cs", FileUtility.FilePath("FileUtilityTest.cs"));
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestFilePathFail()
        {
            FileUtility.Initialize("../");
            FileUtility.FilePath("FileUtilityTest.cs");
        }

    }
}
