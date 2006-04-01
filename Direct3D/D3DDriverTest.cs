using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Direct3D
{
    using NUnit.Framework;

    [TestFixture]
    public class D3DDriverTest
    {
        [Test]
        public void SingletonTest()
        {
            D3DDriver driver1 = D3DDriver.GetInstance();
            D3DDriver driver2 = D3DDriver.GetInstance();

            Assert.AreSame(driver1, driver2);
        }
    
        [Test]
        public void InitTest()
        {
            D3DDriver driver = D3DDriver.GetInstance();
            TestWindow window = new TestWindow();
            DeviceDescription desc = new DeviceDescription();

            try
            {
                desc.deviceType = DeviceType.Software;
                desc.windowed = true;
                driver.Init(window, desc);
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.AreEqual(null, driver.GetDevice());
            }

            try
            {
                desc.deviceType = DeviceType.Reference;
                desc.windowed = false;
                driver.Init(window, desc);
                Assert.Fail();
            }
            catch (DirectXException)
            {
                // This should trigger an DX exception
                Assert.AreEqual(null, driver.GetDevice());
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            desc.windowed = true;
            driver.Init(window, desc);
            Assert.AreNotEqual(null, driver.GetDevice());
        }
    }

    public class TestWindow : System.Windows.Forms.Form
    {
        private System.ComponentModel.Container components = null;

        public TestWindow()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Size = new System.Drawing.Size(800, 600);
            this.Text = "Engine Test";
        }
    }
}
