using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Dope.DDXX.Sound
{
    public class CueAdapter : ICue
    {
        private Cue cue;

        public CueAdapter(Cue cue)
        {
            this.cue = cue;
        }

        #region ICue Members

        public bool IsCreated
        {
            get { return cue.IsCreated; }
        }

        public bool IsDisposed
        {
            get { return cue.IsDisposed; }
        }

        public bool IsPaused
        {
            get { return cue.IsPaused; }
        }

        public bool IsPlaying
        {
            get { return cue.IsPlaying; }
        }

        public bool IsPrepared
        {
            get { return cue.IsPrepared; }
        }

        public bool IsPreparing
        {
            get { return cue.IsPreparing; }
        }

        public bool IsStopped
        {
            get { return cue.IsStopped; }
        }

        public bool IsStopping
        {
            get { return cue.IsStopping; }
        }

        public string Name
        {
            get { return cue.Name; }
        }

        public void Apply3D(AudioListener listener, AudioEmitter emitter)
        {
            cue.Apply3D(listener, emitter);
        }

        public float GetVariable(string name)
        {
            return cue.GetVariable(name);
        }

        public void Pause()
        {
            cue.Pause();
        }

        public void Play()
        {
            cue.Play();
        }

        public void Resume()
        {
            cue.Resume();
        }

        public void SetVariable(string name, float value)
        {
            cue.SetVariable(name, value);
        }

        public void Stop(AudioStopOptions options)
        {
            cue.Stop(options);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
