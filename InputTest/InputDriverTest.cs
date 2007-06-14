using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.DirectInput;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Input
{
    [TestFixture]
    public class InputDriverTest
    {
        private Mockery mockery;
        private IInputFactory factory;

        private InputDriver driver;
        Device keyboard = new Device(SystemGuid.Keyboard);

        private float[] slowRepeatTimes = new float[] { 0.5f, 0.1f, 0.02f };
        private int[] slowRepeatNums = new int[] { 3, 12, 100 };

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            factory = mockery.NewMock<IInputFactory>();

            InputDriver.Factory = factory;
            driver = InputDriver.GetInstance();
            driver.Reset();

            Time.Initialize();
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
        public void TestKeysRepeat()
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

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(keyboard, Key.Escape).
                Will(Return.Value(true));
            Assert.IsTrue(driver.KeyPressed(Key.Escape));
        }

        [Test]
        public void TestKeysNoRepeat()
        {
            TestInitOK();

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(keyboard, Key.Escape).
                Will(Return.Value(false));
            Assert.IsFalse(driver.KeyPressedNoRepeat(Key.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(keyboard, Key.Escape).
                Will(Return.Value(true));
            Assert.IsTrue(driver.KeyPressedNoRepeat(Key.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(keyboard, Key.Escape).
                Will(Return.Value(true));
            Assert.IsFalse(driver.KeyPressedNoRepeat(Key.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(keyboard, Key.Escape).
                Will(Return.Value(false));
            Assert.IsFalse(driver.KeyPressedNoRepeat(Key.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(keyboard, Key.Escape).
                Will(Return.Value(true));
            Assert.IsTrue(driver.KeyPressedNoRepeat(Key.Escape));
        }

        [Test]
        public void TestKeysSlowRepeatLongPress()
        {
            TestInitOK();
            Time.Pause();

            float t = 0;
            for (int i = 0; i < slowRepeatNums.Length; i++)
            {
                for (int j = 0; j < slowRepeatNums[i]; j++)
                {
                    // One keypress at time t (true)
                    KeyPressAtTime(t, true);
                    // One keypress at time t + X (false)
                    KeyPressAtTime(t + slowRepeatTimes[i] - 0.0001f, false);
                    t += slowRepeatTimes[i] + 0.0001f;
                }
            }
        }

        [Test]
        public void TestKeysSlowRepeatHaltedPress()
        {
            TestInitOK();
            Time.Pause();

            // One keypress at time 0 (true)
            KeyPressAtTime(0.0f, true);
            // One keypress at time 0.5 (true)
            KeyPressAtTime(0.6f, true);
            // One keypress at time 1.0 (true)
            KeyPressAtTime(1.2f, true);
            // Reset presser
            ExpectKeyPress(Key.Escape, false);
            Assert.IsFalse(driver.KeyPressedSlowRepeat(Key.Escape));
            // One keypress at time 1.5 (true)
            KeyPressAtTime(1.8f, true);
            // One keypress at time 1.9 (false)
            KeyPressAtTime(2.2f, false);
        }

        private void KeyPressAtTime(float time, bool shouldBeTrue)
        {
            Time.CurrentTime = time;
            ExpectKeyPress(Key.Escape, true);
            Assert.AreEqual(shouldBeTrue, driver.KeyPressedSlowRepeat(Key.Escape),
                "At time " + time + " KeyPressedSlowRepeat should return " + shouldBeTrue);
        }

        private void ExpectKeyPress(Key key, bool returnValue)
        {
            Expect.Once.On(factory).
                Method("KeyPressed").
                With(keyboard, key).
                Will(Return.Value(returnValue));
        }
    }
}
