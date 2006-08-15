using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.DirectInput;
using Utility;

namespace Input
{
    [TestFixture]
    public class InputDriverTest
    {
        private Mockery mockery;
        private IInputFactory factory;

        private InputDriver driver;
        Device keyboard = new Device(SystemGuid.Keyboard);

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            factory = mockery.NewMock<IInputFactory>();

            InputDriver.Factory = factory;
            driver = InputDriver.GetInstance();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestSingleton()
        {
            InputDriver driver2 = InputDriver.GetInstance();
            Assert.AreSame(driver, driver2);
        }

        [Test]
        public void TestInitFail()
        {
            DeviceInstance[] devices = { new DeviceInstance() };
            Stub.On(factory).
                GetProperty("Keyboard").
                Will(Return.Value(null));

            // No keyboard found
            try
            {
                driver.Initialize(null);
                Assert.Fail();
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestInitOK()
        {
            Stub.On(factory).
                GetProperty("Keyboard").
                Will(Return.Value(keyboard));
            Expect.Once.On(factory).
                Method("SetCooperativeLevel").
                With(keyboard, null, CooperativeLevelFlags.NonExclusive | CooperativeLevelFlags.Background);
            Expect.Once.On(factory).
                Method("Acquire").
                With(keyboard);

            driver.Initialize(null);
        }

        [Test]
        public void TestKeys()
        {
            TestInitOK();

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(keyboard, Key.Escape).
                Will(Return.Value(false));
            Assert.IsFalse(driver.KeyPressed(Key.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(keyboard, Key.Escape).
                Will(Return.Value(true));
            Assert.IsTrue(driver.KeyPressed(Key.Escape));
        }
    }
}
