using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using NMock2.Monitoring;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Sound
{
    [TestFixture]
    public class SoundDriverTest
    {
        SoundDriver driver = SoundDriver.GetInstance();
        private Mockery mockery;
        private ISoundFactory factory;
        private ICue cue;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            factory = mockery.NewMock<ISoundFactory>();
            cue = mockery.NewMock<ICue>();
            SoundDriver.Factory = factory;
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestSingleton()
        {
            // Exercise SUT
            SoundDriver driver2 = SoundDriver.GetInstance();
            // Verify
            Assert.AreSame(driver, driver2);
        }

        [Test]
        public void TestInitialize()
        {
            // Setup
            Expect.Once.On(factory).
                Method("Initialize").With("name");
            // Exercise SUT
            driver.Initialize("name");
        }

        [Test]
        public void TestPlaySoundOK()
        {
            // Setup
            ICue cue;
            TestInitialize();
            Expect.Once.On(factory).Method("CreateCue").
                With("name").Will(Return.Value(this.cue));
            Expect.Once.On(this.cue).Method("Play");
            // Exercise SUT
            cue = driver.PlaySound("name");
            // Verify
            Assert.AreSame(this.cue, cue);
        }

    }
}
