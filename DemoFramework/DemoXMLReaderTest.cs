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
using System.Drawing;

namespace Dope.DDXX.Utility {
    [TestFixture]
    public class DemoXMLReaderTest {
        #region Builder stub
        class BuilderStub : IDemoEffectBuilder {
            class Effect {
                public string name;
                public int track;
                public float startTime;
                public float endTime;
                public Dictionary<string, Parameter> parameters;
                public Dictionary<string, List<object>> setups;
                public Effect(string name, int track)
                    : this(name, track, 0, 0)
                {
                }
                public Effect(string name, int track, float startTime, float endTime) {
                    this.name = name;
                    this.track = track;
                    this.startTime = startTime;
                    this.endTime = endTime;
                    parameters = new Dictionary<string, Parameter>();
                    setups = new Dictionary<string, List<object>>();
                }
            }
            private Queue<Effect> effects = new Queue<Effect>();
            private Queue<Effect> postEffects = new Queue<Effect>();
            private Queue<Effect> transitions = new Queue<Effect>();
            private Effect lastEffect;
            private Effect currentEffect;
            private Effect currentPostEffect;
            private Effect currentTransition;

            #region IDemoEffectBuilder implementation

            public void AddTransition(string name, int destinationTrack) {
                lastEffect = new Effect(name, destinationTrack);
                transitions.Enqueue(lastEffect);
            }

            public void AddPostEffect(string name, int track, float startTime, float endTime)
            {
                lastEffect = new Effect(name, track, startTime, endTime);
                postEffects.Enqueue(lastEffect);
            }

            public void AddEffect(string name, int track, float startTime, float endTime) {
                lastEffect = new Effect(name, track, startTime, endTime);
                effects.Enqueue(lastEffect);
            }

            private void AddParameter(string name, Parameter value) {
                if (lastEffect != null) {
                    lastEffect.parameters.Add(value.name, value);
                } else {
                    throw new InvalidOperationException("Adding parameters before any effects");
                }
            }

            public void AddIntParameter(string name, int value) {
                AddParameter(name, new Parameter(name, ParameterType.Integer, value));
            }
            public void AddFloatParameter(string name, float value) {
                AddParameter(name, new Parameter(name, ParameterType.Float, value));
            }
            public void AddStringParameter(string name, string value) {
                AddParameter(name, new Parameter(name, ParameterType.String, value));
            }
            public void AddVector3Parameter(string name, Vector3 value) {
                AddParameter(name, new Parameter(name, ParameterType.Vector3, value));
            }
            public void AddColorParameter(string name, Color value) {
                AddParameter(name, new Parameter(name, ParameterType.Color, value));
            }
            public void AddSetupCall(string name, List<object> parameters) {
                if (lastEffect != null) {
                    lastEffect.setups.Add(name, parameters);
                } else {
                    throw new InvalidOperationException("Adding parameters before any effects");
                }
            }
            #endregion

            #region Test access

            public bool NextEffect() {
                if (effects.Count > 0) {
                    currentEffect = effects.Dequeue();
                    return true;
                } else {
                    currentEffect = null;
                    return false;
                }
            }

            public bool NextPostEffect() {
                if (postEffects.Count > 0) {
                    currentPostEffect = postEffects.Dequeue();
                    return true;
                } else {
                    currentPostEffect = null;
                    return false;
                }
            }

            public bool NextTransition() {
                if (transitions.Count > 0) {
                    currentTransition = transitions.Dequeue();
                    return true;
                } else {
                    currentTransition = null;
                    return false;
                }
            }

            public float StartTime
            {
                get
                {
                    if (currentEffect != null)
                        return currentEffect.startTime;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }
            public float EndTime
            {
                get
                {
                    if (currentEffect != null)
                        return currentEffect.endTime;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }
            public int EffectTrack
            {
                get {
                    if (currentEffect != null)
                        return currentEffect.track;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }
            public string EffectName {
                get {
                    if (currentEffect != null)
                        return currentEffect.name;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }

            public int PostEffectTrack {
                get {
                    if (currentPostEffect != null)
                        return currentPostEffect.track;
                    else
                        throw new InvalidOperationException("No current post effect");
                }
            }
            public string PostEffectName {
                get {
                    if (currentPostEffect != null)
                        return currentPostEffect.name;
                    else
                        throw new InvalidOperationException("No current post effect");
                }
            }

            public int TransitionDestinationTrack {
                get {
                    if (currentTransition != null)
                        return currentTransition.track;
                    else
                        throw new InvalidOperationException("No current transition");
                }
            }
            public string TransitionName {
                get {
                    if (currentTransition != null)
                        return currentTransition.name;
                    else
                        throw new InvalidOperationException("No current transition");
                }
            }

            #endregion

            public Dictionary<string, Parameter> GetParameters() {
                if (currentEffect != null)
                    return currentEffect.parameters;
                else
                    throw new InvalidOperationException("No current effect");
            }

            public Dictionary<string, Parameter> GetPostEffectParameters() {
                if (currentPostEffect != null)
                    return currentPostEffect.parameters;
                else
                    throw new InvalidOperationException("No current post effect");
            }

            public Dictionary<string, Parameter> GetTransitionParameters() {
                if (currentTransition != null)
                    return currentTransition.parameters;
                else
                    throw new InvalidOperationException("No current transition");
            }

            public Dictionary<string, List<object>> GetSetups() {
                if (currentEffect != null)
                    return currentEffect.setups;
                else
                    throw new InvalidOperationException("No current effect");
            }
        }
        #endregion

        internal class TempFiles
        {
            List<string> files = new List<string>();
            public string New()
            {
                string s = Path.GetTempFileName();
                files.Add(s);
                return s;
            }
            public string New(string contents)
            {
                string tempFilename = New();
                StreamWriter writer = new StreamWriter(tempFilename);
                writer.Write(contents);
                writer.Close();
                return tempFilename;
            }

            ~TempFiles()
            {
                Delete();
            }

            public void Delete()
            {
                foreach (string s in files)
                {
                    Console.WriteLine("deleting temp file: " + s);
                    File.Delete(s);
                }
                files.Clear();
            }            
        }

        BuilderStub effectBuilder;
        TempFiles tempFiles = new TempFiles();

        public string twoEffectContents =
@"<Effects>
<Effect name=""fooeffect"" track=""1"" startTime=""3.5"" endTime=""7.5"">
<Parameter name=""fooparam"" int=""3"" />
<Parameter name=""barparam"" float=""4.3"" />
<Parameter name=""strparam"" string=""foostr"" />
<SetupCall name=""AddTextureLayer"">
<Parameter string=""BlurBackground.jpg""/>
<Parameter float=""35.0""/>
<Parameter Color=""Beige""/>
<Parameter int=""2""/>
</SetupCall>
</Effect>
<Effect name=""bareffect"" endTime=""8.5"">
<Parameter name=""goo"" string=""string value"" />
<Parameter name=""vecparam"" Vector3=""5.4, 4.3, 3.2"" />
</Effect>
<PostEffect name=""fooglow"" track=""2"">
<Parameter name=""glowparam"" float=""5.4"" />
</PostEffect>
<Transition name=""footrans"" destinationTrack=""1"">
<Parameter name=""transparam"" string=""tranny"" />
</Transition>
</Effects>
";
        [SetUp]
        public void SetUp() {
            FileUtility.SetLoadPaths("");
            effectBuilder = new BuilderStub();
        }

        [TearDown]
        public void TearDown() {
            tempFiles.Delete();
        }

        [Test]
        public void TestNoEffectsNode() {
            try {
                ReadXML(@"<Foo></Foo>");
                Assert.Fail("Expected XmlException because no <Effects> node found");
            } catch (XmlException) {
            }
        }

        [Test]
        public void TestNextEffect() {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("fooeffect", effectBuilder.EffectName);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("bareffect", effectBuilder.EffectName);
            Assert.IsFalse(effectBuilder.NextEffect());
        }

        [Test]
        public void TestPostEffect() {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextPostEffect());
            Assert.AreEqual("fooglow", effectBuilder.PostEffectName);
            Assert.AreEqual(2, effectBuilder.PostEffectTrack);
            Dictionary<string, Parameter> parameters = effectBuilder.GetPostEffectParameters();
            Assert.AreEqual(1, parameters.Count);
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("glowparam", out parameter));
            Assert.AreEqual(ParameterType.Float, parameter.Type);
            Assert.AreEqual(5.4, parameter.FloatValue);
            Assert.IsFalse(effectBuilder.NextPostEffect());
        }

        [Test]
        public void TestTransition() {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextTransition());
            Assert.AreEqual("footrans", effectBuilder.TransitionName);
            Assert.AreEqual(1, effectBuilder.TransitionDestinationTrack);
            Dictionary<string, Parameter> parameters = effectBuilder.GetTransitionParameters();
            Assert.AreEqual(1, parameters.Count);
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("transparam", out parameter));
            Assert.AreEqual(ParameterType.String, parameter.Type);
            Assert.AreEqual("tranny", parameter.StringValue);
            Assert.IsFalse(effectBuilder.NextTransition());
        }

        [Test]
        public void TestEffectTrack() {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("fooeffect", effectBuilder.EffectName);
            Assert.AreEqual(1, effectBuilder.EffectTrack);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("bareffect", effectBuilder.EffectName);
            Assert.AreEqual(0, effectBuilder.EffectTrack);
        }

        [Test]
        public void TestStartEndTime()
        {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("fooeffect", effectBuilder.EffectName);
            Assert.AreEqual(3.5, effectBuilder.StartTime);
            Assert.AreEqual(7.5, effectBuilder.EndTime);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("bareffect", effectBuilder.EffectName);
            Assert.AreEqual(0.0, effectBuilder.StartTime);
            Assert.AreEqual(8.5, effectBuilder.EndTime);
            Assert.AreEqual(0, effectBuilder.EffectTrack);
        }

        [Test]
        public void TestGetParameters() {
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
        public void TestVector3Param() {
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
        public void TestSetupCall()
        {
            ReadXML(twoEffectContents);
            effectBuilder.NextEffect();
            Assert.AreEqual("fooeffect", effectBuilder.EffectName);
            Dictionary<string, List<object>> setups = effectBuilder.GetSetups();
            Assert.AreEqual(1, setups.Count);
            List<object> list;
            Assert.IsTrue(setups.TryGetValue("AddTextureLayer", out list));
            Assert.AreEqual(4, list.Count);
            Assert.AreEqual("BlurBackground.jpg", list[0]);
            Assert.AreEqual(35.0f, list[1]);
            Assert.AreEqual(Color.Beige, list[2]);
            Assert.AreEqual(2, list[3]);
        }

        [Test]
        public void TestWrongParams() {
            string wrongParamContents =
@"<Effects><Effect><Parameter name=""foo"" /></Effect></Effects>";
            try {
                ReadXML(wrongParamContents);
                Assert.Fail("Expected exception due to missing parameter type");
            } catch (XmlException) {
            }
        }

        [Test]
        public void TestWrongParamType() {
            string wrongParamTypeContents =
@"<Effects><Effect><Parameter name=""foo"" sunktype=""44"" /></Effect></Effects>";
            try {
                ReadXML(wrongParamTypeContents);
                Assert.Fail("Expected exception due to unknown parameter type");
            } catch (XmlException) {
            }
        }

        private void ReadXML(string contents) {
            string tempFilename = tempFiles.New(contents);
            DemoXMLReader reader = new DemoXMLReader(effectBuilder);
            reader.Read(tempFilename);
        }
    }
}
