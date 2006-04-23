using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Direct3D;
using Utility;

namespace DemoFramework
{
    public class DemoExecuter
    {
        List<Track> tracks = new List<Track>();

        public float StartTime
        {
            get
            {
                return 0.0f;
            }
        }

        public float EndTime
        {
            get
            {
                float t = 0.0f;
                foreach (Track track in tracks)
                {
                    foreach (IEffect effect in track.Effects)
                    {
                        if (effect.EndTime > t)
                            t = effect.EndTime;
                    }
                }
                return t;
            }
        }

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


        internal void Step()
        {
            foreach (Track track in tracks)
            {
                foreach (IEffect effect in track.GetEffects(Time.StepTime))
                {
                    effect.Step();
                }
            }
        }

        public void Run()
        {
            Time.Initialize();
            while (Time.CurrentTime < EndTime + 2.0f)
            {
                Time.Step();
            }
        }
    }
}
