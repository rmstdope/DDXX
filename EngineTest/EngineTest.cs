using System;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Input;
using Dope.DDXX.Sound;
using Microsoft.Xna.Framework.Input;
using Dope.DDXX.Utility;
using System.Reflection;
using Dope.DDXX.DemoEffects;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.MidiProcessorLib;
using Microsoft.Xna.Framework.Audio;

namespace EngineTest
{
    static class EngineTest
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Assembly[] assemblies = new Assembly[] { 
                Assembly.GetExecutingAssembly(),
                typeof(GlowPostEffect).Assembly,
                typeof(TextureBuilder).Assembly };
            FileUtility.SetLoadPaths(new string[] { "./", "../../../", "xml/" });
            DemoWindow window = new DemoWindow("Pelle", "EngineTest.xml", assemblies);

            CompiledMidi midi = window.Content.Load<CompiledMidi>("Content\\sound\\music_machinefunk_loop");
            System.Diagnostics.Debug.WriteLine("CompiledMidi loaded. Number of tracks: " + 
                midi.Tracks.Count);
            int i = 0;
            foreach (CompiledMidi.CompiledMidiTrack track in midi.Tracks)
            {
                System.Diagnostics.Debug.WriteLine("Track " + i + " contains " + track.NotesAndTimes.Length/2 + " notes.");
                i++;
            }
            if (window.SetupDialog())
                window.Run();
        }
    }
}

