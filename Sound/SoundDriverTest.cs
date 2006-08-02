using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using NMock2.Monitoring;
using Utility;
using FMOD;

namespace Sound
{
    [TestFixture]
    public class SoundDriverTest
    {
        SoundDriver driver = SoundDriver.GetInstance();
        private Mockery mockery;
        private IFactory factory;
        private ISystem system;
        private Channel channel;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            factory = mockery.NewMock<IFactory>();
            system = mockery.NewMock<ISystem>();
            Stub.On(factory).
                Method("CreateSystem").
                Will(Return.Value(system));
            SoundDriver.SetFactory(factory);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void SingletonTest()
        {
            SoundDriver driver2 = SoundDriver.GetInstance();

            Assert.AreSame(driver, driver2);
        }

        //internal class ReturnVersion : IAction
        //{
        //    private uint number;

        //    #region ISelfDescribing Members

        //    public ReturnVersion(uint version)
        //    {
        //        number = version;
        //    }

        //    public void Invoke(Invocation inv)
        //    {
        //        uint version = (uint)inv.Parameters[0];
        //        version = number;
        //    }

        //    public void DescribeTo(System.IO.TextWriter writer)
        //    {
        //        writer.Write("Setting version number to : " + number);
        //    }

        //    #endregion
        //}

        [Test]
        public void TestInitFail1()
        {
            try
            {
                Expect.Once.On(system).
                    Method("GetVersion").
                    WithAnyArguments().
                    Will(Return.Value(RESULT.ERR_SUBSOUNDS));
                driver.Init();
                Assert.Fail();
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestInitFail2()
        {
            try
            {
                Expect.Once.On(system).
                    Method("GetVersion").
                    WithAnyArguments().
                    Will(Return.Value(RESULT.OK));
                Expect.Once.On(system).
                    Method("Init").
                    With(64, FMOD.INITFLAG.NORMAL, (IntPtr)null).
                    Will(Return.Value(RESULT.ERR_SUBSOUNDS));
                driver.Init();
                Assert.Fail();
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestInitOK()
        {
            Expect.Once.On(system).
                Method("GetVersion").
                WithAnyArguments().
                Will(Return.Value(RESULT.OK));
            Expect.Once.On(system).
                Method("Init").
                With(64, FMOD.INITFLAG.NORMAL, (IntPtr)null).
                Will(Return.Value(RESULT.OK));
            driver.Init();
        }

        [Test]
        public void TestCreateSound()
        {
            FMOD.Sound sound;

            TestInitOK();

            try
            {
                Expect.Once.On(system).
                    Method("CreateSound").With(Is.EqualTo("../../Data/test.mp3"), Is.EqualTo(FMOD.MODE._2D | FMOD.MODE.HARDWARE | FMOD.MODE.CREATESTREAM), Is.Anything).
                    Will(Return.Value(RESULT.ERR_SUBSOUNDS));
                sound = driver.CreateSound("test.mp3");
                Assert.Fail();
            }
            catch (DDXXException) { }

            Expect.Once.On(system).
                Method("CreateSound").With(Is.EqualTo("../../Data/test.mp3"), Is.EqualTo(FMOD.MODE._2D | FMOD.MODE.HARDWARE | FMOD.MODE.CREATESTREAM), Is.Anything).
                Will(Return.Value(RESULT.OK));
            sound = driver.CreateSound("test.mp3");
            Assert.IsNotNull(sound);
        }

        [Test]
        public void TestPlaySoundFail()
        {
            FMOD.Sound sound = new FMOD.Sound();

            TestInitOK();
            Expect.Once.On(system).
                Method("PlaySound").With(Is.EqualTo(FMOD.CHANNELINDEX.FREE), Is.EqualTo(sound), Is.EqualTo(false), Is.NotNull).
                Will(Return.Value(RESULT.ERR_ALREADYLOCKED));

            try
            {
                channel = driver.PlaySound(sound);
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestPlaySoundOK()
        {
            FMOD.Sound sound = new FMOD.Sound();
            TestInitOK();

            Expect.Once.On(system).
                Method("PlaySound").With(Is.EqualTo(FMOD.CHANNELINDEX.FREE), Is.EqualTo(sound), Is.EqualTo(false), Is.NotNull).
                Will(Return.Value(RESULT.OK));
            channel = driver.PlaySound(sound);
        }

        [Test]
        public void TestPauseFail()
        {
            TestPlaySoundOK();

            Expect.Once.On(system).
                Method("SetPaused").
                With(channel, true).
                Will(Return.Value(RESULT.ERR_VERSION));
            try
            {
                driver.PauseChannel(channel);
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestPauseOK()
        {
            TestPlaySoundOK();

            Expect.Once.On(system).
                Method("SetPaused").
                With(channel, true).
                Will(Return.Value(RESULT.OK));
            driver.PauseChannel(channel);
        }

        [Test]
        public void TestResumeFail()
        {
            TestPlaySoundOK();

            Expect.Once.On(system).
                Method("SetPaused").
                With(channel, false).
                Will(Return.Value(RESULT.ERR_VERSION));
            try
            {
                driver.ResumeChannel(channel);
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestResumeOK()
        {
            TestPlaySoundOK();

            Expect.Once.On(system).
                Method("SetPaused").
                With(channel, false).
                Will(Return.Value(RESULT.OK));
            driver.ResumeChannel(channel);
        }

        [Test]
        public void TestGetPositionFail()
        {
            TestPlaySoundOK();

            Expect.Once.On(system).
                Method("GetPosition").
                With(channel, (uint)0, TIMEUNIT.MS).
                Will(Return.Value(RESULT.ERR_VERSION));
            try
            {
                float pos = driver.GetPosition(channel);
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestGetPositionOK()
        {
            TestPlaySoundOK();

            Expect.Once.On(system).
                Method("GetPosition").
                With(channel, (uint)0, TIMEUNIT.MS).
                Will(Return.Value(RESULT.OK));
            float pos = driver.GetPosition(channel);
        }

        [Test]
        public void TestSetPositionFail()
        {
            TestPlaySoundOK();

            Expect.Once.On(system).
                Method("SetPosition").
                With(channel, (uint)2345, TIMEUNIT.MS).
                Will(Return.Value(RESULT.ERR_VERSION));
            try
            {
                driver.SetPosition(channel, 2.345f);
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestSetPositionOK()
        {
            TestPlaySoundOK();

            Expect.Once.On(system).
                Method("SetPosition").
                With(channel, (uint)2345, TIMEUNIT.MS).
                Will(Return.Value(RESULT.OK));
            driver.SetPosition(channel, 2.345f);
        }

    }
}