using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using TImport = Dope.DDXX.MidiProcessorLib.MidiSource;
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
        DefaultProcessor = "MidiProcessor", CacheImportedData = true)]
    public class MidiImporter : ContentImporter<TImport>
    {
        public override TImport Import(string filename, ContentImporterContext context)
        {
            // TODO: read the specified file into an instance of the imported type.
            throw new NotImplementedException();
        }

        public TImport ImportFromStream(Stream midiStream)
        {
            MidiExtractor.MidiExtractor extractor = new MidiExtractor.MidiExtractor();
            MThd header;
            List<IMTrk> tracks;
            extractor.Parse(midiStream, null, out header, out tracks);
            return new MidiSource(header, tracks);
        }
    }
}
