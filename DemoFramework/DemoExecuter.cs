using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Direct3D;

namespace DemoFramework
{
    public class DemoExecuter
    {
        List<Track> tracks = new List<Track>();

        public int NumTracks
        {
            get
            {
                return tracks.Count;
            }
        }

        public DemoExecuter()
        {
        }

        public void Initialize()
        {
        }

        internal void Register(int track, IEffect effect)
        {
            while (NumTracks <= track)
            {
                tracks.Add(new Track());
            }
            tracks[track].Register(effect);
        }

    }
}
