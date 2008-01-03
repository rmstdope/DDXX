using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml;
using System.Reflection;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoXMLReaderTest
    {
        #region Builder stub
        class BuilderStub : IDemoEffectBuilder
        {
            class Asset
            {
                public Dictionary<string, Parameter> parameters;
                public Dictionary<string, List<object>> setups;
                public Asset()
                {
                    parameters = new Dictionary<string, Parameter>();
                    setups = new Dictionary<string, List<object>>();
                }
            }
            class Effect : Asset
            {
                public string name;
                public string className;
                public int track;
                public float startTime;
                public float endTime;
                public Effect(string className, string name, int track)
                    : this(className, name, track, 0, 0)
                {
                }
                public Effect(string className, string name, int track, float startTime, float endTime)
                    : base()
                {
                    this.className = className;
                    this.name = name;
                    this.track = track;
                    this.startTime = startTime;
                    this.endTime = endTime;
                }
            }
            class Generator : Asset
            {
                public string name;
                public string className;
                public Dictionary<int, string> inputs;
                public Generator(string name, string className)
                {
                    this.name = name;
                    this.className = className;
                    inputs = new Dictionary<int, string>();
                }
            }
            class Texture
            {
                public string name;
                public string generator;
                public int width;
                public int height;
                public int mipLevels;
                public Texture(string name, string generator, int width, int height, int mipLevels)
                {
                    this.name = name;
                    this.generator = generator;
                    this.width = width;
                    this.height = height;
                    this.mipLevels = mipLevels;
                }
            }
            private Queue<Effect> effects = new Queue<Effect>();
            private Queue<Effect> postEffects = new Queue<Effect>();
            private Queue<Effect> transitions = new Queue<Effect>();
            private Queue<Generator> generators = new Queue<Generator>();
            private Queue<Texture> textures = new Queue<Texture>();
            private Asset lastAsset;
            private Effect currentEffect;
            private Effect currentPostEffect;
            private Effect currentTransition;
            private Generator currentGenerator;
            private Texture currentTexture;
            private string songName;

            #region IDemoEffectBuilder implementation

            public void AddTransition(string className, string name, int destinationTrack, float startTime, float endTime)
            {
                Effect newEffect = new Effect(className, name, destinationTrack, startTime, endTime);
                lastAsset = newEffect;
                transitions.Enqueue(newEffect);
            }

            public void AddPostEffect(string className, string name, int track, float startTime, float endTime)
            {
                Effect newEffect = new Effect(className, name, track, startTime, endTime);
                lastAsset = newEffect;
                postEffects.Enqueue(newEffect);
            }

            public void AddEffect(string className, string name, int track, float startTime, float endTime)
            {
                Effect newEffect = new Effect(className, name, track, startTime, endTime);
                lastAsset = newEffect;
                effects.Enqueue(newEffect);
            }

            public void AddGenerator(string name, string className)
            {
                Generator newGenerator = new Generator(name, className);
                lastAsset = newGenerator;
                generators.Enqueue(newGenerator);
            }

            public void AddTexture(string name, string generator, int width, int height, int mipLevels)
            {
                textures.Enqueue(new Texture(name, generator, width, height, mipLevels));
            }

            private void AddParameter(string name, Parameter value)
            {
                if (lastAsset != null)
                {
                    lastAsset.parameters.Add(value.name, value);
                }
                else
                {
                    throw new InvalidOperationException("Adding parameters before any effects");
                }
            }

            public void AddIntParameter(string name, int value, float stepSize)
            {
                AddParameter(name, new Parameter(name, TweakableType.Integer, value, stepSize));
            }
            public void AddFloatParameter(string name, float value, float stepSize)
            {
                AddParameter(name, new Parameter(name, TweakableType.Float, value, stepSize));
            }
            public void AddStringParameter(string name, string value)
            {
                AddParameter(name, new Parameter(name, TweakableType.String, value));
            }
            public void AddVector2Parameter(string name, Vector2 value, float stepSize)
            {
                AddParameter(name, new Parameter(name, TweakableType.Vector2, value, stepSize));
            }
            public void AddVector3Parameter(string name, Vector3 value, float stepSize)
            {
                AddParameter(name, new Parameter(name, TweakableType.Vector3, value, stepSize));
            }
            public void AddVector4Parameter(string name, Vector4 value, float stepSize)
            {
                AddParameter(name, new Parameter(name, TweakableType.Vector4, value, stepSize));
            }
            public void AddColorParameter(string name, Color value)
            {
                AddParameter(name, new Parameter(name, TweakableType.Color, value));
            }
            public void AddBoolParameter(string name, bool value)
            {
                AddParameter(name, new Parameter(name, TweakableType.Bool, value));
            }
            public void AddSetupCall(string name, List<object> parameters)
            {
                if (lastAsset != null)
                {
                    lastAsset.setups.Add(name, parameters);
                }
                else
                {
                    throw new InvalidOperationException("Adding parameters before any effects");
                }
            }

            public void AddGeneratorInput(int num, string generatorName)
            {
                if (lastAsset != null && lastAsset is Generator)
                {
                    (lastAsset as Generator).inputs.Add(num, generatorName);
                }
                else
                {
                    throw new InvalidOperationException("Adding parameters before any effects");
                }
            }

            public void SetSong(string filename)
            {
                songName = filename;
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

            public bool NextPostEffect()
            {
                if (postEffects.Count > 0)
                {
                    currentPostEffect = postEffects.Dequeue();
                    return true;
                }
                else
                {
                    currentPostEffect = null;
                    return false;
                }
            }

            public bool NextTransition()
            {
                if (transitions.Count > 0)
                {
                    currentTransition = transitions.Dequeue();
                    return true;
                }
                else
                {
                    currentTransition = null;
                    return false;
                }
            }

            public bool NextGenerator()
            {
                if (generators.Count > 0)
                {
                    currentGenerator = generators.Dequeue();
                    return true;
                }
                else
                {
                    currentGenerator = null;
                    return false;
                }
            }

            public bool NextTexture()
            {
                if (textures.Count > 0)
                {
                    currentTexture = textures.Dequeue();
                    return true;
                }
                else
                {
                    currentTexture = null;
                    return false;
                }
            }

            public float EffectStartTime
            {
                get
                {
                    if (currentEffect != null)
                        return currentEffect.startTime;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }
            public float EffectEndTime
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
            public string EffectClass
            {
                get
                {
                    if (currentEffect != null)
                        return currentEffect.className;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }

            public int PostEffectTrack
            {
                get
                {
                    if (currentPostEffect != null)
                        return currentPostEffect.track;
                    else
                        throw new InvalidOperationException("No current post effect");
                }
            }
            public string PostEffectName
            {
                get
                {
                    if (currentPostEffect != null)
                        return currentPostEffect.name;
                    else
                        throw new InvalidOperationException("No current post effect");
                }
            }
            public string PostEffectClass
            {
                get
                {
                    if (currentPostEffect != null)
                        return currentPostEffect.className;
                    else
                        throw new InvalidOperationException("No current post effect");
                }
            }
            public float PostEffectStartTime
            {
                get
                {
                    if (currentPostEffect != null)
                        return currentPostEffect.startTime;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }
            public float PostEffectEndTime
            {
                get
                {
                    if (currentPostEffect != null)
                        return currentPostEffect.endTime;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }

            public int TransitionDestinationTrack
            {
                get
                {
                    if (currentTransition != null)
                        return currentTransition.track;
                    else
                        throw new InvalidOperationException("No current transition");
                }
            }
            public string TransitionName
            {
                get
                {
                    if (currentTransition != null)
                        return currentTransition.name;
                    else
                        throw new InvalidOperationException("No current transition");
                }
            }
            public string TransitionClass
            {
                get
                {
                    if (currentTransition != null)
                        return currentTransition.className;
                    else
                        throw new InvalidOperationException("No current transition");
                }
            }
            public float TransitionStartTime
            {
                get
                {
                    if (currentTransition != null)
                        return currentTransition.startTime;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }
            public float TransitionEndTime
            {
                get
                {
                    if (currentTransition != null)
                        return currentTransition.endTime;
                    else
                        throw new InvalidOperationException("No current effect");
                }
            }

            public string GeneratorName
            {
                get
                {
                    if (currentGenerator != null)
                        return currentGenerator.name;
                    else
                        throw new InvalidOperationException("No current generator");
                }
            }
            public string GeneratorClass
            {
                get
                {
                    if (currentGenerator != null)
                        return currentGenerator.className;
                    else
                        throw new InvalidOperationException("No current generator");
                }
            }

            public string TextureName
            {
                get
                {
                    if (currentTexture != null)
                        return currentTexture.name;
                    else
                        throw new InvalidOperationException("No current texture");
                }
            }
            public string TextureGenerator
            {
                get
                {
                    if (currentTexture != null)
                        return currentTexture.generator;
                    else
                        throw new InvalidOperationException("No current texture");
                }
            }

            public int TextureWidth
            {
                get
                {
                    if (currentTexture != null)
                        return currentTexture.width;
                    else
                        throw new InvalidOperationException("No current texture");
                }
            }
            public int TextureHeight
            {
                get
                {
                    if (currentTexture != null)
                        return currentTexture.height;
                    else
                        throw new InvalidOperationException("No current texture");
                }
            }
            public int TextureMipLevels
            {
                get
                {
                    if (currentTexture != null)
                        return currentTexture.mipLevels;
                    else
                        throw new InvalidOperationException("No current texture");
                }
            }

            public string SongName
            {
                get { return songName; }
            }

            #endregion

            public Dictionary<string, Parameter> GetParameters()
            {
                if (currentEffect != null)
                    return currentEffect.parameters;
                else
                    throw new InvalidOperationException("No current effect");
            }

            public Dictionary<string, Parameter> GetPostEffectParameters()
            {
                if (currentPostEffect != null)
                    return currentPostEffect.parameters;
                else
                    throw new InvalidOperationException("No current post effect");
            }

            public Dictionary<string, Parameter> GetTransitionParameters()
            {
                if (currentTransition != null)
                    return currentTransition.parameters;
                else
                    throw new InvalidOperationException("No current transition");
            }

            public Dictionary<string, Parameter> GetGeneratorParameters()
            {
                if (currentGenerator != null)
                    return currentGenerator.parameters;
                else
                    throw new InvalidOperationException("No current generator");
            }

            public Dictionary<string, List<object>> GetSetups()
            {
                if (currentEffect != null)
                    return currentEffect.setups;
                else
                    throw new InvalidOperationException("No current effect");
            }

            public Dictionary<int, string> GetGeneratorInputs()
            {
                if (currentGenerator != null)
                    return currentGenerator.inputs;
                else
                    throw new InvalidOperationException("No current generator");
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
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Demo>
<!-- Here is a comment -->
<Effect class=""fooeffect"" name=""fooname"" track=""1"" startTime=""3.5"" endTime=""7.5"">
<Parameter name=""intparam"" int=""3"" />
<Parameter name=""floatparam"" float=""4.3"" />
<Parameter name=""strparam"" string=""foostr"" />
<Parameter name=""colparamnamed"" Color=""1, 2, 3, 4"" />
<Parameter name=""colparam"" Color=""1, 2, 3, 4"" />
<Parameter name=""boolparam"" bool=""true"" />
<Parameter name=""floatstepparam"" float=""3.4"" step=""0.1"" />
<SetupCall name=""AddTextureLayer"">
<Parameter string=""BlurBackground.jpg"" />
<Parameter float=""35.0"" />
<Parameter Color=""5, 6, 7, 8"" />
<Parameter int=""2"" />
</SetupCall>
</Effect>
<!-- Here is another comment -->
<Effect class=""bareffect"" name=""barname"" endTime=""8.5"">
<Parameter name=""goo"" string=""string value"" />
<Parameter name=""background"" Color=""0, 0, 0, 0"" />
<Parameter name=""vec2param"" Vector2=""5.4, 4.3"" />
<Parameter name=""vec3param"" Vector3=""5.4, 4.3, 3.2"" />
<Parameter name=""vec4param"" Vector4=""5.4, 4.3, 3.2, 2.1"" />
</Effect>
<!-- <PostEffect name=""fooglow"" track=""2""> -->
<PostEffect class=""fooglow"" name=""glowname"" track=""2"" startTime=""2"" endTime=""5"">
<Parameter name=""glowparam"" float=""5.4"" />
<Parameter name=""glowdir2"" Vector2=""1.1, 2.2"" />
<Parameter name=""glowdir3"" Vector3=""1.1, 2.2, 3.3"" />
<Parameter name=""glowdir4"" Vector4=""1.1, 2.2, 3.3, 4.4"" />
<!-- <Parameter name=""glowparam"" float=""5.4"" /> -->
</PostEffect>
<Transition class=""footrans"" name=""transname"" track=""1"" startTime=""8"" endTime=""9"">
<Parameter name=""transparam"" string=""tranny"" />
</Transition>
</Demo>
";

        [SetUp]
        public void SetUp()
        {
            FileUtility.SetLoadPaths("");
            effectBuilder = new BuilderStub();
        }

        [TearDown]
        public void TearDown()
        {
            tempFiles.Delete();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestNoDemoNode()
        {
            ReadXML(@"<Foo></Foo>");
        }

        [Test]
        public void TestClassName()
        {
            ReadXML(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Demo>
    <Effect class=""c1"" name=""n1""/>
    <PostEffect class=""c2"" name=""n2""/>
    <Transition class=""c3"" name=""n3""/>
</Demo>");
            effectBuilder.NextEffect();
            Assert.AreEqual("n1", effectBuilder.EffectName);
            Assert.AreEqual("c1", effectBuilder.EffectClass);
            effectBuilder.NextPostEffect();
            Assert.AreEqual("n2", effectBuilder.PostEffectName);
            Assert.AreEqual("c2", effectBuilder.PostEffectClass);
            effectBuilder.NextTransition();
            Assert.AreEqual("n3", effectBuilder.TransitionName);
            Assert.AreEqual("c3", effectBuilder.TransitionClass);
        }

        [Test]
        public void TestNoname()
        {
            ReadXML(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Demo>
    <Effect class=""c1""/>
    <PostEffect class=""c2""/>
    <Transition class=""c3""/>
</Demo>");
            effectBuilder.NextEffect();
            Assert.AreEqual("c1", effectBuilder.EffectName);
            Assert.AreEqual("c1", effectBuilder.EffectClass);
            effectBuilder.NextPostEffect();
            Assert.AreEqual("c2", effectBuilder.PostEffectName);
            Assert.AreEqual("c2", effectBuilder.PostEffectClass);
            effectBuilder.NextTransition();
            Assert.AreEqual("c3", effectBuilder.TransitionName);
            Assert.AreEqual("c3", effectBuilder.TransitionClass);
        }

        [Test]
        public void TestNextEffect()
        {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("fooeffect", effectBuilder.EffectClass);
            Assert.AreEqual("fooname", effectBuilder.EffectName);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("bareffect", effectBuilder.EffectClass);
            Assert.AreEqual("barname", effectBuilder.EffectName);
            Assert.IsFalse(effectBuilder.NextEffect());
        }

        [Test]
        public void TestPostEffect()
        {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextPostEffect());
            Assert.AreEqual("glowname", effectBuilder.PostEffectName);
            Assert.AreEqual("fooglow", effectBuilder.PostEffectClass);
            Assert.AreEqual(2, effectBuilder.PostEffectTrack);
            Dictionary<string, Parameter> parameters = effectBuilder.GetPostEffectParameters();
            Assert.AreEqual(4, parameters.Count);
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("glowparam", out parameter));
            Assert.AreEqual(TweakableType.Float, parameter.Type);
            Assert.AreEqual(5.4f, parameter.FloatValue);
            Assert.IsFalse(effectBuilder.NextPostEffect());
        }

        [Test]
        public void TestTransition()
        {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextTransition());
            Assert.AreEqual("transname", effectBuilder.TransitionName);
            Assert.AreEqual("footrans", effectBuilder.TransitionClass);
            Assert.AreEqual(1, effectBuilder.TransitionDestinationTrack);
            Dictionary<string, Parameter> parameters = effectBuilder.GetTransitionParameters();
            Assert.AreEqual(1, parameters.Count);
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("transparam", out parameter));
            Assert.AreEqual(TweakableType.String, parameter.Type);
            Assert.AreEqual("tranny", parameter.StringValue);
            Assert.IsFalse(effectBuilder.NextTransition());
        }

        [Test]
        public void TestEffectTrack()
        {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("fooeffect", effectBuilder.EffectClass);
            Assert.AreEqual(1, effectBuilder.EffectTrack);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("bareffect", effectBuilder.EffectClass);
            Assert.AreEqual(0, effectBuilder.EffectTrack);
        }

        [Test]
        public void TestStartEndTimeOfEffects()
        {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("fooeffect", effectBuilder.EffectClass);
            Assert.AreEqual(3.5, effectBuilder.EffectStartTime);
            Assert.AreEqual(7.5, effectBuilder.EffectEndTime);
            Assert.IsTrue(effectBuilder.NextEffect());
            Assert.AreEqual("bareffect", effectBuilder.EffectClass);
            Assert.AreEqual(0.0, effectBuilder.EffectStartTime);
            Assert.AreEqual(8.5, effectBuilder.EffectEndTime);
            Assert.AreEqual(0, effectBuilder.EffectTrack);
            Assert.IsFalse(effectBuilder.NextEffect());
        }

        [Test]
        public void TestStartEndTimeOfPostEffects()
        {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextPostEffect());
            Assert.AreEqual("fooglow", effectBuilder.PostEffectClass);
            Assert.AreEqual(2, effectBuilder.PostEffectStartTime);
            Assert.AreEqual(5, effectBuilder.PostEffectEndTime);
            Assert.AreEqual(2, effectBuilder.PostEffectTrack);
            Assert.IsFalse(effectBuilder.NextPostEffect());
        }

        [Test]
        public void TestStartEndTimeOfTransitions()
        {
            ReadXML(twoEffectContents);
            Assert.IsTrue(effectBuilder.NextTransition());
            Assert.AreEqual("footrans", effectBuilder.TransitionClass);
            Assert.AreEqual(8, effectBuilder.TransitionStartTime);
            Assert.AreEqual(9, effectBuilder.TransitionEndTime);
            Assert.AreEqual(1, effectBuilder.TransitionDestinationTrack);
            Assert.IsFalse(effectBuilder.NextTransition());
        }

        private Dictionary<string, Parameter> GetFooEffectParameters()
        {
            ReadXML(twoEffectContents);
            effectBuilder.NextEffect();
            Dictionary<string, Parameter> parameters = effectBuilder.GetParameters();
            return parameters;
        }

        [Test]
        public void TestGetNumParameters()
        {
            Dictionary<string, Parameter> parameters = GetFooEffectParameters();
            Assert.AreEqual(7, parameters.Count);
        }

        [Test]
        public void TestIntParameter()
        {
            Dictionary<string, Parameter> parameters = GetFooEffectParameters();
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("intparam", out parameter));
            Assert.AreEqual(TweakableType.Integer, parameter.Type);
            Assert.AreEqual(3, parameter.IntValue);
            Assert.AreEqual(-1, parameter.StepSize);
        }

        [Test]
        public void TestFloatParameter()
        {
            Dictionary<string, Parameter> parameters = GetFooEffectParameters();
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("floatparam", out parameter));
            Assert.AreEqual(TweakableType.Float, parameter.Type);
            Assert.AreEqual(4.3f, parameter.FloatValue);
            Assert.AreEqual(-1, parameter.StepSize);
        }

        [Test]
        public void TestStringParameter()
        {
            Dictionary<string, Parameter> parameters = GetFooEffectParameters();
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("strparam", out parameter));
            Assert.AreEqual(TweakableType.String, parameter.Type);
            Assert.AreEqual("foostr", parameter.StringValue);
        }

        [Test]
        public void TestColorParameter()
        {
            Dictionary<string, Parameter> parameters = GetFooEffectParameters();
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("colparam", out parameter));
            Assert.AreEqual(TweakableType.Color, parameter.Type);
            Assert.AreEqual(new Color(1, 2, 3, 4), parameter.ColorValue);
        }

        [Test]
        public void TestBoolParameter()
        {
            Dictionary<string, Parameter> parameters = GetFooEffectParameters();
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("boolparam", out parameter));
            Assert.AreEqual(TweakableType.Bool, parameter.Type);
            Assert.AreEqual(true, parameter.BoolValue);
        }

        [Test]
        public void TestFloatParameterStep()
        {
            Dictionary<string, Parameter> parameters = GetFooEffectParameters();
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("floatstepparam", out parameter));
            Assert.AreEqual(TweakableType.Float, parameter.Type);
            Assert.AreEqual(3.4f, parameter.FloatValue);
            Assert.AreEqual(0.1f, parameter.StepSize);
        }

        [Test]
        public void TestVector2Param()
        {
            ReadXML(twoEffectContents);
            effectBuilder.NextEffect();
            effectBuilder.NextEffect();
            Assert.AreEqual("bareffect", effectBuilder.EffectClass);
            Dictionary<string, Parameter> parameters = effectBuilder.GetParameters();
            Assert.AreEqual(5, parameters.Count);
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("vec2param", out parameter));
            Assert.AreEqual(TweakableType.Vector2, parameter.Type);
            Assert.AreEqual(new Vector2(5.4f, 4.3f), parameter.Vector2Value);
        }

        [Test]
        public void TestVector3Param()
        {
            ReadXML(twoEffectContents);
            effectBuilder.NextEffect();
            effectBuilder.NextEffect();
            Assert.AreEqual("bareffect", effectBuilder.EffectClass);
            Dictionary<string, Parameter> parameters = effectBuilder.GetParameters();
            Assert.AreEqual(5, parameters.Count);
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("vec3param", out parameter));
            Assert.AreEqual(TweakableType.Vector3, parameter.Type);
            Assert.AreEqual(new Vector3(5.4f, 4.3f, 3.2f), parameter.Vector3Value);
        }

        [Test]
        public void TestVector4Param()
        {
            ReadXML(twoEffectContents);
            effectBuilder.NextEffect();
            effectBuilder.NextEffect();
            Assert.AreEqual("bareffect", effectBuilder.EffectClass);
            Dictionary<string, Parameter> parameters = effectBuilder.GetParameters();
            Assert.AreEqual(5, parameters.Count);
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("vec4param", out parameter));
            Assert.AreEqual(TweakableType.Vector4, parameter.Type);
            Assert.AreEqual(new Vector4(5.4f, 4.3f, 3.2f, 2.1f), parameter.Vector4Value);
        }

        [Test]
        public void TestSetupCall()
        {
            ReadXML(twoEffectContents);
            effectBuilder.NextEffect();
            Assert.AreEqual("fooeffect", effectBuilder.EffectClass);
            Dictionary<string, List<object>> setups = effectBuilder.GetSetups();
            Assert.AreEqual(1, setups.Count);
            List<object> list;
            Assert.IsTrue(setups.TryGetValue("AddTextureLayer", out list));
            Assert.AreEqual(4, list.Count);
            Assert.AreEqual("BlurBackground.jpg", list[0]);
            Assert.AreEqual(35.0f, list[1]);
            Assert.AreEqual(new Color(5, 6, 7, 8), list[2]);
            Assert.AreEqual(2, list[3]);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestWrongParams()
        {
            string wrongParamContents =
@"<Demo><Effect><Parameter name=""foo"" /></Effect></Demo>";
            ReadXML(wrongParamContents);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestWrongParamType()
        {
            string wrongParamTypeContents =
@"<Demo><Effect><Parameter name=""foo"" sunktype=""44"" /></Effect></Demo>";
            ReadXML(wrongParamTypeContents);
        }

        [Test]
        public void TestParamChanged()
        {
            DemoXMLReader reader = ReadXMLString(twoEffectContents);
            reader.SetColorParam("fooeffect", "fooname", "colparamnamed", Color.SpringGreen);
            reader.SetColorParam("fooeffect", "fooname", "colparam", new Color(255, 100, 101, 102));
            reader.SetFloatParam("fooeffect", "fooname", "floatparam", 8.6f);
            reader.SetBoolParam("fooeffect", "fooname", "boolparam", false);
            reader.SetIntParam("fooeffect", "fooname", "intparam", 7);
            reader.SetStringParam("bareffect", "barname", "goo", "goovalue");
            reader.SetVector2Param("fooglow", "glowname", "glowdir2", new Vector2(1.2f, 2.3f));
            reader.SetVector3Param("fooglow", "glowname", "glowdir3", new Vector3(1.2f, 2.3f, 3.4f));
            reader.SetVector4Param("fooglow", "glowname", "glowdir4", new Vector4(1.2f, 2.3f, 3.4f, 4.5f));
            reader.SetStartTime("fooeffect", "fooname", 15);
            reader.SetStartTime("bareffect", "barname", 4);
            reader.SetEndTime("bareffect", "barname", 19);
            string filename = tempFiles.New();
            reader.Write(filename);

            string written = File.ReadAllText(filename);
            effectBuilder = new BuilderStub();
            reader = ReadXML(written);
            effectBuilder.NextEffect();
            Assert.AreEqual("fooeffect", effectBuilder.EffectClass);
            Assert.AreEqual(15, effectBuilder.EffectStartTime);
            Dictionary<string, Parameter> parameters = effectBuilder.GetParameters();
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("colparamnamed", out parameter));
            Assert.AreEqual(TweakableType.Color, parameter.Type);
            Assert.AreEqual(Color.SpringGreen, parameter.ColorValue);
            Assert.IsTrue(parameters.TryGetValue("colparam", out parameter));
            Assert.AreEqual(TweakableType.Color, parameter.Type);
            Assert.AreEqual((object)new Color(255, 100, 101, 102), (object)parameter.ColorValue);
            Assert.IsTrue(parameters.TryGetValue("boolparam", out parameter));
            Assert.AreEqual(TweakableType.Bool, parameter.Type);
            Assert.AreEqual(false, (object)parameter.BoolValue);
            Assert.IsTrue(parameters.TryGetValue("floatparam", out parameter));
            Assert.AreEqual(TweakableType.Float, parameter.Type);
            Assert.AreEqual(8.6f, parameter.FloatValue);
            Assert.IsTrue(parameters.TryGetValue("intparam", out parameter));
            Assert.AreEqual(TweakableType.Integer, parameter.Type);
            Assert.AreEqual(7, parameter.IntValue);
            effectBuilder.NextEffect();
            parameters = effectBuilder.GetParameters();
            Assert.AreEqual("bareffect", effectBuilder.EffectClass);
            Assert.AreEqual(4, effectBuilder.EffectStartTime);
            Assert.AreEqual(19, effectBuilder.EffectEndTime);
            Assert.IsTrue(parameters.TryGetValue("goo", out parameter));
            Assert.AreEqual(TweakableType.String, parameter.Type);
            Assert.AreEqual("goovalue", parameter.StringValue);
            effectBuilder.NextPostEffect();
            parameters = effectBuilder.GetPostEffectParameters();
            Assert.AreEqual("fooglow", effectBuilder.PostEffectClass);
            Assert.IsTrue(parameters.TryGetValue("glowdir2", out parameter));
            Assert.AreEqual(TweakableType.Vector2, parameter.Type);
            Assert.AreEqual(new Vector2(1.2f, 2.3f), parameter.Vector2Value);
            Assert.IsTrue(parameters.TryGetValue("glowdir3", out parameter));
            Assert.AreEqual(TweakableType.Vector3, parameter.Type);
            Assert.AreEqual(new Vector3(1.2f, 2.3f, 3.4f), parameter.Vector3Value);
            Assert.IsTrue(parameters.TryGetValue("glowdir4", out parameter));
            Assert.AreEqual(TweakableType.Vector4, parameter.Type);
            Assert.AreEqual(new Vector4(1.2f, 2.3f, 3.4f, 4.5f), parameter.Vector4Value);
        }

        [Test]
        public void TestAddingParameter()
        {
            Parameter parameter;
            DemoXMLReader reader = ReadXMLString(twoEffectContents);
            effectBuilder.NextEffect();
            Dictionary<string, Parameter> parameters = effectBuilder.GetParameters();
            Assert.IsFalse(parameters.TryGetValue("newParameter", out parameter));
            reader.SetFloatParam("fooeffect", "fooname", "newParameter", 1.2f);
            string filename = tempFiles.New();
            reader.Write(filename);

            string written = File.ReadAllText(filename);
            effectBuilder = new BuilderStub();
            reader = ReadXML(written);
            effectBuilder.NextEffect();
            parameters = effectBuilder.GetParameters();
            Assert.IsTrue(parameters.TryGetValue("newParameter", out parameter));
            Assert.AreEqual(TweakableType.Float, parameter.Type);
            Assert.AreEqual(1.2f, parameter.FloatValue);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestSetMissingEffectParam()
        {
            DemoXMLReader reader = ReadXMLString(twoEffectContents);
            reader.SetIntParam("gazonkeffect", "gazonkname", "floatparam", 8);
        }

        [Test]
        public void TestWrite()
        {
            DemoXMLReader reader = ReadXMLString(twoEffectContents);
            string tempFilename = tempFiles.New();
            reader.Write(tempFilename);
            string written = File.ReadAllText(tempFilename);
            Assert.AreEqual(twoEffectContents, written);
        }

        [Test]
        public void TestSong1()
        {
            string songXml =
@"<Demo song=""song1.mp3""></Demo>";
            ReadXML(songXml);
            Assert.AreEqual("song1.mp3", effectBuilder.SongName);
        }

        [Test]
        public void TestSong2()
        {
            string songXml =
@"<Demo song=""song2.mp3""></Demo>";
            ReadXML(songXml);
            Assert.AreEqual("song2.mp3", effectBuilder.SongName);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestUnknownAttribute()
        {
            string songXml =
@"<Demo unknown=""x""></Demo>";
            ReadXML(songXml);
        }

        [Test]
        public void TestOneGenerator()
        {
            string textureXml =
                @"<Demo><Generator name=""gen1"" class=""noiser""/></Demo>";
            ReadXML(textureXml);
            Assert.IsTrue(effectBuilder.NextGenerator());
            Assert.AreEqual("gen1", effectBuilder.GeneratorName);
            Assert.AreEqual("noiser", effectBuilder.GeneratorClass);
            Assert.IsFalse(effectBuilder.NextGenerator());
        }

        [Test]
        public void TestTwoGenerators()
        {
            string textureXml =
                @"<Demo><Generator name=""gen1"" class=""noiser""/><Generator name=""gen2"" class=""noiser2""/></Demo>";
            ReadXML(textureXml);
            Assert.IsTrue(effectBuilder.NextGenerator());
            Assert.AreEqual("gen1", effectBuilder.GeneratorName);
            Assert.AreEqual("noiser", effectBuilder.GeneratorClass);
            Assert.IsTrue(effectBuilder.NextGenerator());
            Assert.AreEqual("gen2", effectBuilder.GeneratorName);
            Assert.AreEqual("noiser2", effectBuilder.GeneratorClass);
            Assert.IsFalse(effectBuilder.NextGenerator());
        }

        [Test]
        public void TestOneGeneratorWithOneParameter()
        {
            string textureXml =
@"<Demo>
    <Generator name=""gen1"" class=""noiser"">
        <Parameter name=""Para"" bool=""true""/>
    </Generator>
</Demo>";
            ReadXML(textureXml);
            Assert.IsTrue(effectBuilder.NextGenerator());
            Dictionary<string, Parameter> parameters = effectBuilder.GetGeneratorParameters();
            Parameter parameter;
            Assert.IsTrue(parameters.TryGetValue("Para", out parameter));
            Assert.AreEqual(TweakableType.Bool, parameter.Type);
            Assert.AreEqual(true, parameter.BoolValue);
        }

        [Test]
        public void TestOneGeneratorWithOneInput()
        {
            string textureXml =
@"<Demo>
    <Generator name=""gen1"" class=""noiser"">
        <Input number=""0"" generator=""gen2""/>
    </Generator>
</Demo>";
            ReadXML(textureXml);
            Assert.IsTrue(effectBuilder.NextGenerator());
            Dictionary<int, string> inputs = effectBuilder.GetGeneratorInputs();
            string str;
            Assert.IsTrue(inputs.TryGetValue(0, out str));
            Assert.AreEqual("gen2", str);
        }

        [Test]
        public void TestOneGeneratorWithTwoInputs()
        {
            string textureXml =
@"<Demo>
    <Generator name=""gen1"" class=""noiser"">
        <Input number=""50"" generator=""x""/>
        <Input number=""100"" generator=""y""/>
    </Generator>
</Demo>";
            ReadXML(textureXml);
            Assert.IsTrue(effectBuilder.NextGenerator());
            Dictionary<int, string> inputs = effectBuilder.GetGeneratorInputs();
            string str;
            Assert.IsTrue(inputs.TryGetValue(50, out str));
            Assert.AreEqual("x", str);
            Assert.IsTrue(inputs.TryGetValue(100, out str));
            Assert.AreEqual("y", str);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInputWithoutNumber()
        {
            string textureXml =
@"<Demo>
    <Generator name=""gen1"" class=""noiser"">
        <Input generator=""gen2""/>
    </Generator>
</Demo>";
            ReadXML(textureXml);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInputWithoutGenerator()
        {
            string textureXml =
@"<Demo>
    <Generator name=""gen1"" class=""noiser"">
        <Input number=""2""/>
    </Generator>
</Demo>";
            ReadXML(textureXml);
        }

        [Test]
        public void TestOneTextureDefaultValues()
        {
            string textureXml =
                @"<Demo><Texture name=""tex1"" generator=""noiser""/></Demo>";
            ReadXML(textureXml);
            Assert.IsTrue(effectBuilder.NextTexture());
            Assert.AreEqual("tex1", effectBuilder.TextureName);
            Assert.AreEqual("noiser", effectBuilder.TextureGenerator);
            Assert.AreEqual(128, effectBuilder.TextureWidth);
            Assert.AreEqual(128, effectBuilder.TextureHeight);
            Assert.AreEqual(0, effectBuilder.TextureMipLevels);
            Assert.IsFalse(effectBuilder.NextTexture());
        }

        [Test]
        public void TestOneTextureNotDefaultValues()
        {
            string textureXml =
                @"<Demo><Texture name=""tex1"" generator=""noiser"" width=""1"" height=""666"" miplevels=""7""/></Demo>";
            ReadXML(textureXml);
            Assert.IsTrue(effectBuilder.NextTexture());
            Assert.AreEqual("tex1", effectBuilder.TextureName);
            Assert.AreEqual("noiser", effectBuilder.TextureGenerator);
            Assert.AreEqual(1, effectBuilder.TextureWidth);
            Assert.AreEqual(666, effectBuilder.TextureHeight);
            Assert.AreEqual(7, effectBuilder.TextureMipLevels);
            Assert.IsFalse(effectBuilder.NextTexture());
        }

        [Test]
        public void TestTwoTextures()
        {
            string textureXml =
@"<Demo>
    <Texture name=""tex2"" generator=""gen2""/>
    <Texture name=""tex3"" generator=""gen3""/>
  </Demo>";
            ReadXML(textureXml);
            Assert.IsTrue(effectBuilder.NextTexture());
            Assert.AreEqual("tex2", effectBuilder.TextureName);
            Assert.AreEqual("gen2", effectBuilder.TextureGenerator);
            Assert.IsTrue(effectBuilder.NextTexture());
            Assert.AreEqual("tex3", effectBuilder.TextureName);
            Assert.AreEqual("gen3", effectBuilder.TextureGenerator);
            Assert.IsFalse(effectBuilder.NextTexture());
        }

        private DemoXMLReader ReadXML(string contents)
        {
            string tempFilename = tempFiles.New(contents);
            DemoXMLReader reader = new DemoXMLReader(effectBuilder);
            reader.Read(tempFilename);
            return reader;
        }

        private DemoXMLReader ReadXMLString(string contents)
        {
            DemoXMLReader reader = new DemoXMLReader(effectBuilder);
            reader.ReadString(contents);
            return reader;
        }
    }
}
