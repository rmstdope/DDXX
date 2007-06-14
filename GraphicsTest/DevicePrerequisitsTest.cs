using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class DevicePrerequisitsTest// : D3DMockTest
    {
        private Mockery mockery;
        private IManager manager;
        private DevicePrerequisits prereq;
        private Caps caps;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            manager = mockery.NewMock<IManager>();

            prereq  = new DevicePrerequisits();
            caps = new Caps();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void CheckPrerequisitsOK()
        {
            Expect.Once.On(manager).
                Method("GetDeviceCaps").
                With(0, DeviceType.Hardware).
                Will(Return.Value(caps));
            prereq.CheckPrerequisits(manager, 0, DeviceType.Hardware); 
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void CheckPrerequisitsFail1()
        {
            prereq.PixelShaderVersion = new Version(1, 1);
            Expect.Once.On(manager).
                Method("GetDeviceCaps").
                With(0, DeviceType.Hardware).
                Will(Return.Value(caps));
            prereq.CheckPrerequisits(manager, 0, DeviceType.Hardware);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void CheckPrerequisitsFail2()
        {
            prereq.VertexShaderVersion = new Version(1, 1);
            Expect.Once.On(manager).
                Method("GetDeviceCaps").
                With(0, DeviceType.Hardware).
                Will(Return.Value(caps));
            prereq.CheckPrerequisits(manager, 0, DeviceType.Hardware);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void CheckPrerequisitsFail3()
        {
            prereq.VertexShaderVersion = new Version(1, 1);
            Expect.Once.On(manager).
                Method("GetDeviceCaps").
                With(0, DeviceType.Hardware).
                Will(Return.Value(caps));
            prereq.CheckPrerequisits(manager, 0, DeviceType.Hardware);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void CheckPrerequisitsFail4()
        {
            prereq.ShaderModel = 2;
            Expect.Once.On(manager).
                Method("GetDeviceCaps").
                With(0, DeviceType.Hardware).
                Will(Return.Value(caps));
            prereq.CheckPrerequisits(manager, 0, DeviceType.Hardware);
        }
    }
}
