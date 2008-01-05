using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.MidiProcessorLib;
using Microsoft.Xna.Framework;
using NMock2;
using Dope.DDXX.MidiExtractor;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Dope.DDXX.MidiProcessorLibTest
{
    [TestFixture]
    public class MidiTypeWriterTest
    {
        MidiTypeWriter typeWriter;
        TargetPlatform platform;

        [SetUp]
        public void Setup()
        {
            typeWriter = new MidiTypeWriter();
            platform = new TargetPlatform();
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void TestRuntimeType()
        {
            string expectedTypeName = typeof(CompiledMidi).AssemblyQualifiedName;
            Assert.AreEqual(expectedTypeName, typeWriter.GetRuntimeType(platform));
        }

        [Test]
        public void TestRuntimeReader()
        {
            string expectedReader =
                "Dope.DDXX.MidiProcessorLib.MidiTypeReader, " + 
            "Dope.DDXX.MidiProcessorLib, Version=1.0.0.0, Culture=neutral";
            Assert.AreEqual(expectedReader, typeWriter.GetRuntimeReader(platform));
        }
    }
}
