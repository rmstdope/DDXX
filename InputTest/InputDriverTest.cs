using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Input;

namespace Dope.DDXX.Input
{
    [TestFixture]
    public class InputDriverTest
    {
        private Mockery mockery;
        private IInputFactory factory;
        private InputDriver driver;

        private float[] slowRepeatTimes = new float[] { 0.5f, 0.1f, 0.02f };
        private int[] slowRepeatNums = new int[] { 3, 12, 100 };

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            factory = mockery.NewMock<IInputFactory>();

            driver = InputDriver.GetInstance();
            driver.Reset();
            InputDriver.Factory = factory;
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
        public void TestKeysRepeat()
        {
            Expect.Once.On(factory).
                Method("KeyPressed").
                With(Keys.Escape).
                Will(Return.Value(false));
            Assert.IsFalse(driver.KeyPressed(Keys.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(Keys.Escape).
                Will(Return.Value(true));
            Assert.IsTrue(driver.KeyPressed(Keys.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(Keys.Escape).
                Will(Return.Value(true));
            Assert.IsTrue(driver.KeyPressed(Keys.Escape));
        }

        [Test]
        public void TestKeysNoRepeat()
        {
            Expect.Once.On(factory).
                Method("KeyPressed").
                With(Keys.Escape).
                Will(Return.Value(false));
            Assert.IsFalse(driver.KeyPressedNoRepeat(Keys.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(Keys.Escape).
                Will(Return.Value(true));
            Assert.IsTrue(driver.KeyPressedNoRepeat(Keys.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(Keys.Escape).
                Will(Return.Value(true));
            Assert.IsFalse(driver.KeyPressedNoRepeat(Keys.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(Keys.Escape).
                Will(Return.Value(false));
            Assert.IsFalse(driver.KeyPressedNoRepeat(Keys.Escape));

            Expect.Once.On(factory).
                Method("KeyPressed").
                With(Keys.Escape).
                Will(Return.Value(true));
            Assert.IsTrue(driver.KeyPressedNoRepeat(Keys.Escape));
        }

        [Test]
        public void TestKeysSlowRepeatLongPress()
        {
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
            Time.Pause();

            // One keypress at time 0 (true)
            KeyPressAtTime(0.0f, true);
            // One keypress at time 0.5 (true)
            KeyPressAtTime(0.6f, true);
            // One keypress at time 1.0 (true)
            KeyPressAtTime(1.2f, true);
            // Reset presser
            ExpectKeyPress(Keys.Escape, false);
            Assert.IsFalse(driver.KeyPressedSlowRepeat(Keys.Escape));
            // One keypress at time 1.5 (true)
            KeyPressAtTime(1.8f, true);
            // One keypress at time 1.9 (false)
            KeyPressAtTime(2.2f, false);
        }

        private void KeyPressAtTime(float time, bool shouldBeTrue)
        {
            Time.CurrentTime = time;
            ExpectKeyPress(Keys.Escape, true);
            Assert.AreEqual(shouldBeTrue, driver.KeyPressedSlowRepeat(Keys.Escape),
                "At time " + time + " KeyPressedSlowRepeat should return " + shouldBeTrue);
        }

        private void ExpectKeyPress(Keys key, bool returnValue)
        {
            Expect.Once.On(factory).
                Method("KeyPressed").
                With(key).
                Will(Return.Value(returnValue));
        }
    }
}
