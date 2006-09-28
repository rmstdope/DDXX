// Test read
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

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
        public void TestLoadPaths()
        {
            FileUtility.SetLoadPaths("aaa", "bbb");
            Assert.AreEqual(new string[] {"aaa", "bbb"}, FileUtility.GetLoadPaths());

            FileUtility.SetLoadPaths("ccc", "ddd", "eee");
            Assert.AreEqual(new string[] { "ccc", "ddd", "eee" }, FileUtility.GetLoadPaths());
        }
        
        [Test]
        public void TestBlockFile()
        {
            FileUtility.SetBlockFile("aaa");
            Assert.AreEqual("aaa", FileUtility.GetBlockFile());

            FileUtility.SetBlockFile("eee");
            Assert.AreEqual("eee", FileUtility.GetBlockFile());
        }

        [Test]
        public void TestOpenStreamFileOK1()
        {
            FileUtility.SetLoadPaths("../", "../../");
            FileUtility.SetBlockFile("invalidblockfile");
            FileStream stream = FileUtility.OpenStream("FileUtilityTest.cs");
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestOpenStreamFileFail1()
        {
            FileUtility.SetLoadPaths();
            FileUtility.SetBlockFile("");
            FileStream stream = FileUtility.OpenStream("FileUtilityTest.cs");
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestOpenStreamFileFail2()
        {
            FileUtility.SetLoadPaths("../", "../../");
            FileUtility.SetBlockFile("invalidblockfile");
            FileStream stream = FileUtility.OpenStream("invalidfile");
        }

        [Test]
        public void TestGetPathOK()
        {
            FileUtility.SetLoadPaths("../", "../../");
            FileUtility.SetBlockFile("invalidblockfile");
            Assert.AreEqual("../../FileUtilityTest.cs", FileUtility.FilePath("FileUtilityTest.cs"));
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestGetPathFail1()
        {
            FileUtility.SetLoadPaths();
            FileUtility.SetBlockFile("");
            FileUtility.FilePath("FileUtilityTest.cs");
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestGetPathFail2()
        {
            FileUtility.SetLoadPaths("../");
            FileUtility.SetBlockFile("invalidblockfile");
            FileUtility.FilePath("FileUtilityTest.cs");
        }

    }
}
