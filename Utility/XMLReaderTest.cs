using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NUnit.Framework;
using NMock2;
using System.Xml;
using Microsoft.DirectX;
using System.Reflection;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class XMLReaderTest
    {
        #region Builder stub
        class BuilderStub : IDemoEffectBuilder
        {
            class Effect
            {
                public string name;
                public int track;
                public Dictionary<string, Parameter> parameters;
                public Effect(string name, int track)
                {
                    this.name = name;
                    this.track = track;
                    parameters = new Dictionary<string, Parameter>();
                }
            }
            private Queue<Effect> effects = new Queue<Effect>();
            private Effect lastEffect;
            private Effect currentEffect;

            #region IDemoEffectBuilder implementation
            public void AddEffect(string name, int track)
            {
                lastEffect = new Effect(name, track);
                effects.Enqueue(lastEffect);
            }

            private void AddParameter(string name, Parameter value)
            {
                if (lastEffect != null)
                {
                    lastEffect.parameters.Add(value.name, value);
                }
                else
                {
                    throw new InvalidOperationException("Adding parameters before any effects");
                }
            }

            public void AddIntParameter(string name, int value)
            {
                AddParameter(name, new Parameter(name, ParameterType.Integer, value));
            }
            public void AddFloatParameter(string name, float value)
            {
                AddParameter(name, new Parameter(name, ParameterType.Float, value));
            }
            public void AddStringParameter(string name, string value)
            {
                AddParameter(name, new Parameter(name, ParameterType.String, value));
            }
            public void AddVector3Parameter(string name, Vector3 value)
            {
                AddParameter(name, new Parameter(name, ParameterType.Vector3, value));
            }
            #endregion

            #region Test access

            public bool NextEffect()
            {
                if (effects.Count > 0)
                {
                    currentEffect = effects.Dequeue();
                    return true;
                }
                else
                {
                    currentEffect = null;
                    return false;
                }
            }
            public int EffectTrack
            {
                get
                {
                    if (currentEffect != null)
                        return currentEffect.track;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }
            public string EffectName
            {
                get
                {
                    if (currentEffect != null)
                        return currentEffect.name;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }
            #endregion

            public Dictionary<string, Parameter> GetParameters()
            {
                if (currentEffect != null)
                    return currentEffect.parameters;
                else
                    throw new InvalidOperationException("No current effect");
            }
        }
        #endregion


        List<string> tempFiles = new List<string>();
        BuilderStub effectBuilder;

        static string twoEffectContents =
@"<Effects>
<Effect name=""fooeffect"" track=""1"">
<Parameter name=""fooparam"" int=""3"" />
<Parameter name=""barparam"" float=""4.3"" />
<Parameter name=""strparam"" string=""foostr"" />
</Effect>
<Effect name=""bareffect"">
<Parameter name=""goo"" string=""string value"" />
<Parameter name=""vecparam"" Vector3=""5.4, 4.3, 3.2"" />
</Effect>
</Effects>
";
        [SetUp]
        public void SetUp()
        {
            effectBuilder = new BuilderStub();
        }

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
                ReadXML(@"<Foo></Foo>");
                Assert.Fail("Expected XmlException because no <Effects> node found");
            }
            catch (XmlException)
            {
            }
        }

        [Test]
        public void TestNextEffect()
        {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("fooeffect", effectBuilder.EffectName);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("bareffect", effectBuilder.EffectName);
            Assert.IsFalse(effectBuilder.NextEffect());
        }

        [Test]
        public void TestEffectTrack()
        {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("fooeffect", effectBuilder.EffectName);
            Assert.AreEqual(1, effectBuilder.EffectTrack);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("bareffect", effectBuilder.EffectName);
            Assert.AreEqual(0, effectBuilder.EffectTrack);
        }

        [Test]
        public void TestGetParameters()
        {
            ReadXML(twoEffectContents);
            effectBuilder.NextEffect();
            Assert.AreEqual("fooeffect", effectBuilder.EffectName);
            Dictionary<string, Parameter> parameters = effectBuilder.GetParameters();
            Assert.AreEqual(3, parameters.Count);
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("fooparam", out parameter));
            Assert.AreEqual(ParameterType.Integer, parameter.Type);
            Assert.AreEqual(3, parameter.IntValue);
            Assert.IsTrue(parameters.TryGetValue("barparam", out parameter));
            Assert.AreEqual(ParameterType.Float, parameter.Type);
            Assert.AreEqual(4.3f, parameter.FloatValue);
            Assert.IsTrue(parameters.TryGetValue("strparam", out parameter));
            Assert.AreEqual(ParameterType.String, parameter.Type);
            Assert.AreEqual("foostr", parameter.StringValue);
        }

        [Test]
        public void TestVector3Param()
        {
            ReadXML(twoEffectContents);
            effectBuilder.NextEffect();
            effectBuilder.NextEffect();
            Assert.AreEqual("bareffect", effectBuilder.EffectName);
            Dictionary<string, Parameter> parameters = effectBuilder.GetParameters();
            Assert.AreEqual(2, parameters.Count);
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("vecparam", out parameter));
            Assert.AreEqual(ParameterType.Vector3, parameter.Type);
            Assert.AreEqual(new Vector3(5.4f, 4.3f, 3.2f), parameter.Vector3Value);
        }

        [Test]
        public void TestWrongParams()
        {
            string wrongParamContents =
@"<Effects><Effect><Parameter name=""foo"" /></Effect></Effects>";
            try
            {
                ReadXML(wrongParamContents);
                Assert.Fail("Expected exception due to missing parameter type");
            }
            catch (XmlException)
            {
            }
        }

        [Test]
        public void TestWrongParamType()
        {
            string wrongParamTypeContents =
@"<Effects><Effect><Parameter name=""foo"" sunktype=""44"" /></Effect></Effects>";
            try
            {
                ReadXML(wrongParamTypeContents);
                Assert.Fail("Expected exception due to unknown parameter type");
            }
            catch (XmlException)
            {
            }
        }

        private void ReadXML(string contents)
        {
            string tempFilename = WriteTempFile(contents);
            Stream inputStream = new FileStream(tempFilename, FileMode.Open);
            XMLReader reader = new XMLReader(effectBuilder);
            reader.Read(inputStream);
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
