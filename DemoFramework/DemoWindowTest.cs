using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFramework
{
    using NUnit.Framework;

    [TestFixture]
    public class DemoWindowTest
    {
        DemoWindow window;

        [SetUp]
        public void Setup()
        {
            window = new DemoWindow();
        }

        [Test]
        public void TestInitialize_4_3()
        {
            const int Width = 800;
            const int Height = 600;
            string WindowText = "4_3_Window";

            window.Initialize(Width, Height, WindowText);
            Assert.AreEqual(window.Text, WindowText);
            Assert.AreEqual(window.ClientSize.Height, Height);
            Assert.AreEqual(window.ClientSize.Width, Width);
            Assert.AreEqual(window.CompanyName, "Dope");
            Assert.IsTrue(window.Created);
            Assert.IsTrue(window.Enabled);
            Assert.IsTrue(window.Visible);
            Assert.AreEqual(window.AspectRatio, DemoWindow.Aspect.ASPECT_4_3);
        }

        [Test]
        public void TestInitialize_16_9()
        {
            const int Width = 800;
            const int Height = 450;
            string WindowText = "16_9_Window";

            window.Initialize(Width, Height, WindowText);
            Assert.AreEqual(window.Text, WindowText);
            Assert.AreEqual(window.ClientSize.Height, Height);
            Assert.AreEqual(window.ClientSize.Width, Width);
            Assert.AreEqual(window.CompanyName, "Dope");
            Assert.IsTrue(window.Created);
            Assert.IsTrue(window.Enabled);
            Assert.IsTrue(window.Visible);
            Assert.AreEqual(window.AspectRatio, DemoWindow.Aspect.ASPECT_16_9);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInitializeFail()
        {
            const int Width = 800;
            const int Height = 601;

            window.Initialize(Width, Height, "nisse");
        }
    }
}
