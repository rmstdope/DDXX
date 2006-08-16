using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NUnit.Framework;
using NMock2;
using System.Xml;

// TODO:

namespace Utility
{
    [TestFixture]
    public class XMLReaderTest
    {
        List<string> tempFiles = new List<string>();
        XMLReader reader;
        static string twoEffectContents =
@"<Effects>
<Effect name=""fooeffect"" track=""1"">
<Parameter name=""fooparam"" int=""3"" />
<Parameter name=""barparam"" float=""4.3f"" />
</Effect>
<Effect name=""bareffect"">
<Parameter name=""goo"" string=""string value"" />
</Effect>
</Effects>
";

        [TearDown]
        public void TearDown()
        {
            foreach (string s in tempFiles)
            {
                Console.WriteLine("deleting temp file: " + s);
                File.Delete(s);
            }
        }

        [Test]
        public void TestNoEffectsNode()
        {
            try
            {
                reader = OpenXML(@"<Foo></Foo>");
                Assert.Fail("Expected XmlException because no <Effects> node found");
                reader.Close();
            }
            catch (XmlException)
            {
            }
        }

        [Test]
        public void TestNextEffect()
        {
            reader = OpenXML(twoEffectContents);
            Assert.IsTrue(reader.NextEffect());
            Assert.AreEqual("fooeffect", reader.EffectName);
            Assert.IsTrue(reader.NextEffect());
            Assert.AreEqual("bareffect", reader.EffectName);
            Assert.IsFalse(reader.NextEffect());
            reader.Close();
        }

        [Test]
        public void TestEffectTrack()
        {
            reader = OpenXML(twoEffectContents);
            Assert.IsTrue(reader.NextEffect());
            Assert.AreEqual("fooeffect", reader.EffectName);
            Assert.AreEqual(1, reader.EffectTrack);
            Assert.IsTrue(reader.NextEffect());
            Assert.AreEqual("bareffect", reader.EffectName);
            Assert.AreEqual(0, reader.EffectTrack);
            reader.Close();
        }

        [Test]
        public void TestGetParameters()
        {
            reader = OpenXML(twoEffectContents);
            reader.NextEffect();
            Assert.AreEqual("fooeffect", reader.EffectName);
            Dictionary<string, Parameter> parameters = reader.GetParameters();
            Assert.AreEqual(2, parameters.Count);
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("fooparam", out parameter));
            Assert.AreEqual(ParameterType.Integer, parameter.Type);
            Assert.AreEqual("3", parameter.Value);
            Assert.IsTrue(parameters.TryGetValue("barparam", out parameter));
            Assert.AreEqual(ParameterType.Float, parameter.Type);
            Assert.AreEqual("4.3f", parameter.Value);
            reader.Close();
        }

        [Test]
        public void TestWrongParams()
        {
            string wrongParamContents =
@"<Effects><Effect><Parameter name=""foo"" /></Effect></Effects>";
            reader = OpenXML(wrongParamContents);
            reader.NextEffect();
            try
            {
                reader.GetParameters();
                Assert.Fail("Expected exception due to missing parameter type");
            }
            catch (XmlException)
            {
            }
            finally
            {
                reader.Close();
            }
        }

        [Test]
        public void TestWrongParamType()
        {
            string wrongParamTypeContents =
@"<Effects><Effect><Parameter name=""foo"" sunktype=""44"" /></Effect></Effects>";
            reader = OpenXML(wrongParamTypeContents);
            reader.NextEffect();
            try
            {
                reader.GetParameters();
                Assert.Fail("Expected exception due to unknown parameter type");
            }
            catch (XmlException)
            {
            }
            finally
            {
                reader.Close();
            }
        }


        private XMLReader OpenXML(string contents)
        {
            string tempFilename = WriteTempFile(contents);
            Stream inputStream = new FileStream(tempFilename, FileMode.Open);
            XMLReader reader = new XMLReader();
            try
            {
                reader.Start(inputStream);
                return reader;
            }
            catch (Exception e)
            {
                reader.Close();
                throw e;
            }
        }

        internal string WriteTempFile(string contents)
        {
            string tempFilename = Path.GetTempFileName();
            StreamWriter writer = new StreamWriter(tempFilename);
            writer.Write(contents);
            writer.Close();
            tempFiles.Add(tempFilename);
            return tempFilename;
        }

    }
}
