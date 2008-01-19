using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using NUnit.Framework;
using NMock2;
using Microsoft.Xna.Framework.Input;
using Dope.DDXX.Input;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerTest : ITweakableObject
    {
        private Mockery mockery;
        private TweakerSettings settings;
        private IDemoTweaker demoTweaker;
        private IInputDriver inputDriver;
        private IDemoRegistrator registrator;
        private IUserInterface userInterface;
        private TweakerStatus status;
        private bool nextIndexExpected;
        private bool increaseValueExpected;
        private bool decreaseValueExpected;
        private string setValueExpectedString;
        private int numVisableVariables;
        private int numVariables;
        private ITweakableObject getVariableReturn;
        private bool readFromXmlExpected;
        private XmlNode node;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            inputDriver = mockery.NewMock<IInputDriver>();
            registrator = mockery.NewMock<IDemoRegistrator>();
            userInterface = mockery.NewMock<IUserInterface>();
            settings = new TweakerSettings();
            status = null;
            numVisableVariables = 1;
            numVariables = 1;
            nextIndexExpected = false;
            increaseValueExpected = false;
            decreaseValueExpected = false;
            setValueExpectedString = null;
            getVariableReturn = null;
            readFromXmlExpected = false;
        }

        [TearDown]
        public void TearDown()
        {
            Assert.IsFalse(nextIndexExpected);
            Assert.IsFalse(increaseValueExpected);
            Assert.IsFalse(decreaseValueExpected);
            Assert.IsNull(setValueExpectedString);
            Assert.IsFalse(readFromXmlExpected);
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void InitialStatus()
        {
            // Setup
            numVisableVariables = 2;
            nextIndexExpected = true;
            demoTweaker = new DemoTweaker(settings, this);
            // Exercise SUT
            SimulateKeypress(Keys.Tab);
            // Verify
            Assert.AreEqual(0, status.Index);
            Assert.AreEqual(null, status.InputString);
            Assert.AreEqual(0, status.Selection);
            Assert.AreEqual(0.0f, status.StartTime);
            Assert.AreEqual(10.0f, status.TimeScale);
            Assert.AreEqual(0.5f, status.VariableSpacing);
            Assert.AreEqual(null, status.RootControl);
        }

        [Test]
        public void InitialStatusAfterInitialize()
        {
            // Setup
            nextIndexExpected = true;
            demoTweaker = new DemoTweaker(settings, this);
            Expect.Once.On(registrator).GetProperty("StartTime").Will(Return.Value(2.4f));
            // Exercise SUT
            demoTweaker.Initialize(registrator, userInterface);
            SimulateKeypress(Keys.Tab);
            // Verify
            Assert.AreEqual(2.4f, status.StartTime);
            Assert.IsInstanceOfType(typeof(BoxControl), status.RootControl);
        }

        [Test]
        public void UpWhenAtTop()
        {
            // Setup
            nextIndexExpected = true;
            demoTweaker = new DemoTweaker(settings, this);
            // Exercise SUT
            SimulateKeypress(Keys.Up);
            SimulateKeypress(Keys.Tab);
            // Verify
            Assert.AreEqual(0, status.Selection);
        }

        [Test]
        public void DownToOne()
        {
            // Setup
            numVariables = 2;
            nextIndexExpected = true;
            demoTweaker = new DemoTweaker(settings, this);
            // Exercise SUT
            SimulateKeypress(Keys.Down);
            SimulateKeypress(Keys.Tab);
            // Verify
            Assert.AreEqual(1, status.Selection);
        }

        [Test]
        public void DownWhenAtbottom()
        {
            // Setup
            numVariables = 2;
            nextIndexExpected = true;
            demoTweaker = new DemoTweaker(settings, this);
            // Exercise SUT
            SimulateKeypress(Keys.Down);
            SimulateKeypress(Keys.Down);
            SimulateKeypress(Keys.Tab);
            // Verify
            Assert.AreEqual(1, status.Selection);
        }

        [Test]
        public void DownThenUp()
        {
            // Setup
            numVariables = 2;
            nextIndexExpected = true;
            demoTweaker = new DemoTweaker(settings, this);
            // Exercise SUT
            SimulateKeypress(Keys.Down);
            SimulateKeypress(Keys.Up);
            SimulateKeypress(Keys.Tab);
            // Verify
            Assert.AreEqual(0, status.Selection);
        }

        [Test]
        public void PageUp()
        {
            // Setup
            increaseValueExpected = true;
            demoTweaker = new DemoTweaker(settings, this);
            // Exercise SUT
            SimulateKeypress(Keys.PageUp);
            // Verify
            Assert.IsNotNull(status);
        }

        [Test]
        public void PageDown()
        {
            // Setup
            decreaseValueExpected = true;
            demoTweaker = new DemoTweaker(settings, this);
            // Exercise SUT
            SimulateKeypress(Keys.PageDown);
            // Verify
            Assert.IsNotNull(status);
        }

        [Test]
        public void GenericKeyboardInput()
        {
            // Setup
            setValueExpectedString = "..--01234567890123456789";
            demoTweaker = new DemoTweaker(settings, this);
            // Exercise SUT
            SimulateKeypressDoubleEnter(Keys.Decimal);
            SimulateKeypressDoubleEnter(Keys.OemPeriod);
            SimulateKeypressDoubleEnter(Keys.Subtract);
            SimulateKeypressDoubleEnter(Keys.OemMinus);
            SimulateKeypressDoubleEnter(Keys.D0);
            SimulateKeypressDoubleEnter(Keys.D1);
            SimulateKeypressDoubleEnter(Keys.D2);
            SimulateKeypressDoubleEnter(Keys.D3);
            SimulateKeypressDoubleEnter(Keys.D4);
            SimulateKeypressDoubleEnter(Keys.D5);
            SimulateKeypressDoubleEnter(Keys.D6);
            SimulateKeypressDoubleEnter(Keys.D7);
            SimulateKeypressDoubleEnter(Keys.D8);
            SimulateKeypressDoubleEnter(Keys.D9);
            SimulateKeypressDoubleEnter(Keys.NumPad0);
            SimulateKeypressDoubleEnter(Keys.NumPad1);
            SimulateKeypressDoubleEnter(Keys.NumPad2);
            SimulateKeypressDoubleEnter(Keys.NumPad3);
            SimulateKeypressDoubleEnter(Keys.NumPad4);
            SimulateKeypressDoubleEnter(Keys.NumPad5);
            SimulateKeypressDoubleEnter(Keys.NumPad6);
            SimulateKeypressDoubleEnter(Keys.NumPad7);
            SimulateKeypressDoubleEnter(Keys.NumPad8);
            SimulateKeypressDoubleEnter(Keys.NumPad9);
            SimulateKeypressDoubleEnter(Keys.Enter);
        }

        [Test]
        public void ChangeTweakerButNoNested()
        {
            // Setup
            demoTweaker = new DemoTweaker(settings, this);
            // Exercise SUT
            IDemoTweaker newTeaker = SimulateKeypress(Keys.Enter);
            // Verify
            Assert.IsNull(newTeaker);
        }

        [Test]
        public void ChangeTweakerWithNesting()
        {
            // Setup
            getVariableReturn = mockery.NewMock<ITweakableObject>();
            Stub.On(getVariableReturn).GetProperty("NumVisableVariables").Will(Return.Value(1));
            Expect.Exactly(2).On(registrator).GetProperty("StartTime").Will(Return.Value(2.4f));
            demoTweaker = new DemoTweaker(settings, this);
            demoTweaker.Initialize(registrator, userInterface);
            // Exercise SUT
            DemoTweaker newTeaker = SimulateKeypress(Keys.Enter) as DemoTweaker;
            // Verify
            Assert.IsNotNull(newTeaker);
        }

        [Test]
        public void ReadFromXml()
        {
            // Setup
            readFromXmlExpected = true;
            demoTweaker = new DemoTweaker(settings, this);
            XmlNode node = CreateXmlNode("<Demo/>");
            // Exercise SUT
            demoTweaker.ReadFromXmlFile(node);
            // Verify
            Assert.AreEqual(this.node, node);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ReadFromXmlFailure()
        {
            // Setup
            demoTweaker = new DemoTweaker(settings, this);
            XmlNode node = CreateXmlNode("<DemoX/>");
            // Exercise SUT
            demoTweaker.ReadFromXmlFile(node);
        }

        private XmlNode CreateXmlNode(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            return document.DocumentElement;
        }

        private IDemoTweaker SimulateKeypress(Keys key)
        {
            return SimulateKeypress(key, false);
        }

        private IDemoTweaker SimulateKeypressDoubleEnter(Keys key)
        {
            return SimulateKeypress(key, true);
        }

        private IDemoTweaker SimulateKeypress(Keys key, bool doubleEnter)
        {
            Keys[] allKeys = new Keys[] {
                Keys.Up, Keys.Down, Keys.Tab, Keys.PageUp, Keys.PageDown, 
                Keys.D0, Keys.NumPad0, Keys.D1, Keys.NumPad1, Keys.D2, Keys.NumPad2, Keys.D3, Keys.NumPad3, 
                Keys.D4, Keys.NumPad4, Keys.D5, Keys.NumPad5, Keys.D6, Keys.NumPad6, Keys.D7, Keys.NumPad7, 
                Keys.D8, Keys.NumPad8, Keys.D9, Keys.NumPad9, Keys.Decimal, Keys.OemPeriod, Keys.Subtract, 
                Keys.OemMinus, Keys.Enter
            };

            bool skipNext = false;
            foreach (Keys currentKey in allKeys)
            {
                bool retVal = false;
                string method = "KeyPressedNoRepeat";
                if (currentKey == key)
                    retVal = true;
                if (currentKey == Keys.PageDown || currentKey == Keys.PageUp)
                    method = "KeyPressedSlowRepeat"; 
                if (!skipNext)
                    Expect.Once.On(inputDriver).Method(method).
                        With(currentKey).Will(Return.Value(retVal));
                skipNext = false;
                if (retVal && (
                    ((int)currentKey >= (int)Keys.D0 && (int)currentKey <= (int)Keys.D9) || 
                    currentKey == Keys.Decimal || currentKey == Keys.Subtract))
                    skipNext = true;
            }
            if (doubleEnter)
                Expect.Once.On(inputDriver).Method("KeyPressedNoRepeat").
                    With(Keys.Enter).Will(Return.Value(key == Keys.Enter));
            
            return demoTweaker.HandleInput(inputDriver);
        }

        #region ITweakableObject Members

        public int NumVisableVariables
        {
            get { return numVisableVariables; }
        }

        public int NumVariables
        {
            get { return numVariables; }
        }

        public ITweakable GetTweakableChild(int index)
        {
            return getVariableReturn;
        }

        public void CreateBaseControls(TweakerStatus status, ITweakerSettings settings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CreateVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void NextIndex(TweakerStatus status)
        {
            Assert.IsTrue(nextIndexExpected);
            nextIndexExpected = false;
            this.status = status;
        }

        public void IncreaseValue(TweakerStatus status)
        {
            Assert.IsTrue(increaseValueExpected);
            increaseValueExpected = false;
            this.status = status;
        }

        public void DecreaseValue(TweakerStatus status)
        {
            Assert.IsTrue(decreaseValueExpected);
            decreaseValueExpected = false;
            this.status = status;
        }

        public void SetValue(TweakerStatus status)
        {
            Assert.AreEqual(setValueExpectedString, status.InputString);
            setValueExpectedString = null;
            this.status = status;
        }

        public void ReadFromXmlFile(XmlNode node)
        {
            Assert.IsTrue(readFromXmlExpected);
            readFromXmlExpected = false;
            this.node = node;
        }

        #endregion

        #region ITweakableObject Members


        public void WriteToXmlFile(XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
