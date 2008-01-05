using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.IO;
using Dope.DDXX.MidiExtractor;

namespace Dope.DDXX.MidiProcessorLib
{
    /// <summary>
    /// This class will be instantiated by the XNA Content Pipeline
    /// to import a file from disk into the specified type, TImport.
    /// 
    /// This should be part of a Content Pipeline Extension Library project.
    /// TODO: change the ContentImporter attribute to specify the correct file
    /// extension, display name, and default processor for this importer.
    /// </summary>
    [ContentImporter(".midi", ".mid", DisplayName = "MIDI Importer", 
        DefaultProcessor = "MidiProcessor", CacheImportedData = false)]
    public class MidiImporter : ContentImporter<MidiSource>
    {
        public override MidiSource Import(string filename, ContentImporterContext context)
        {
            using (FileStream inputStream = new FileStream(filename, FileMode.Open))
            {
                return ImportFromStream(inputStream);
            }
        }

        public MidiSource ImportFromStream(Stream midiStream)
        {
            MidiExtractor.MidiExtractor extractor = new MidiExtractor.MidiExtractor();
            MThd header;
            List<IMTrk> tracks;
            extractor.Parse(midiStream, new EventParser(), out header, out tracks);
            return new MidiSource(header, tracks);
        }
    }
}
