using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using Dope.DDXX.MidiExtractor;

namespace Dope.DDXX.MidiProcessorLib
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to write the specified data type into binary .xnb format.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    /// </summary>
    [ContentTypeWriter]
    public class MidiTypeWriter : ContentTypeWriter<CompiledMidi>
    {
        protected override void Write(ContentWriter output, CompiledMidi value)
        {
            output.Write(value.Tracks.Count);
            foreach (CompiledMidi.CompiledMidiTrack track in value.Tracks)
            {
                output.WriteObject<float[]>(track.NotesAndTimes);
            }
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(CompiledMidi).AssemblyQualifiedName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            string ns = "Dope.DDXX.MidiProcessorLib";
            string cls = "MidiTypeReader";
            string asm = ns;
            string ver = "1.0.0.0";
            return ns + "." + cls + ", " + asm + ", Version=" + ver + ", Culture=neutral";
        }
    }
}
