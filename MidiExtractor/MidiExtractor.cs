using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dope.DDXX.MidiExtractor
{
    public class MidiExtractor
    {

        public void Parse(Stream stream, IEventParser eventParser, out MThd header, out List<IMTrk> tracks)
        {
            BinaryReader reader = new BinaryReader(stream);
            header = new MThd(reader);
            tracks = new List<IMTrk>();
            for (int i = 0; i < header.NumTracks; i++)
            {
                IMTrk track = new MTrk(reader, eventParser);
                tracks.Add(track);
            }
        }

        public void CalcRealTime(MThd header, List<IMTrk> tracks)
        {
            List<TimeInfo> timeInfo = null;
            if (tracks.Count > 0)
                timeInfo = tracks[0].ExtractTimeInfo(header.Division);

            foreach (IMTrk track in tracks)
            {
                track.CalcRealTime(timeInfo);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
                Console.WriteLine("Usage: MidiExtractor <midi file> <command>");
            else
            {
                
                MidiExtractor extractor = new MidiExtractor();
                MThd header;
                List<IMTrk> tracks = null;
                try
                {
                    FileStream stream = new FileStream("../../" + args[0], FileMode.Open);
                    extractor.Parse(stream, new EventParser(), out header, out tracks);
                    extractor.CalcRealTime(header, tracks);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Program terminated with exception: " + exception.Message);
                    System.Threading.Thread.CurrentThread.Abort();
                }
                switch (args[1])
                {
                    case "info":
                        Console.WriteLine("Song consists of " + tracks.Count + " tracks.");
                        for (int i = 0; i < tracks.Count; i++)
                        {
                            Console.WriteLine("--------");
                            Console.WriteLine("Track " + i);
                            Console.WriteLine("Name: " + tracks[i].Name);
                            Console.WriteLine("Instrument: " + tracks[i].InstrumentName);
                        }
                        break;
                    case "meta":
                        for (int i = 0; i < tracks.Count; i++)
                        {
                            Console.WriteLine("--------");
                            Console.WriteLine("Track " + i);
                            tracks[i].WriteMetaData();
                        }
                        break;
                    case "noteon2c#":
                        if (args.Length != 3)
                        {
                            Console.WriteLine("Usage: MidiExtractor <file> noteon2c# <track>");
                        }
                        else
                        {
                            int track = Int32.Parse(args[2]);
                            string code = "float[] notesAndTimes = new float[] {\n";
                            int i = 0;
                            foreach (MidiEvent e in tracks[track].Events)
                            {
                                if (e.EventType == 0x90)
                                {
                                    code += e.RealTime + "f, " + e.Arg1 + "f, ";
                                    if ((++i % 8) == 0)
                                        code += "\n";
                                }
                            }
                            code += "};\n";
                            Console.WriteLine(code);
                        }
                        break;
                    default:
                        Console.WriteLine("Unknown command " + args[1]);
                        break;
                }
            }
        }

    }
}
