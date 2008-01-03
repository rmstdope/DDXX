using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Dope.DDXX.Sound
{
    public interface ICue : IDisposable
    {
        // Summary:
        //     Returns whether the cue has been created.
        //
        // Returns:
        //     true if the cue is created; false otherwise.
        bool IsCreated { get; }
        //
        // Summary:
        //     Gets a value indicating whether the object has been disposed.
        //
        // Returns:
        //     true if the object has been disposed; false otherwise.
        bool IsDisposed { get; }
        //
        // Summary:
        //     Returns whether the cue is currently paused.
        //
        // Returns:
        //     true if the cue is paused; false otherwise.
        bool IsPaused { get; }
        //
        // Summary:
        //     Returns whether the cue is playing.
        //
        // Returns:
        //     true if the cue is playing; false otherwise.
        bool IsPlaying { get; }
        //
        // Summary:
        //     Returns whether the cue is prepared to play.
        //
        // Returns:
        //     true if the cue is prepared to play; false otherwise.
        bool IsPrepared { get; }
        //
        // Summary:
        //     Returns whether the cue is preparing to play.
        //
        // Returns:
        //     true if the cue is preparing to play; false otherwise.
        bool IsPreparing { get; }
        //
        // Summary:
        //     Returns whether the cue is currently stopped.
        //
        // Returns:
        //     true if the cue is stopped; false if otherwise.
        bool IsStopped { get; }
        //
        // Summary:
        //     Returns whether the cue is stopping playback.
        //
        // Returns:
        //     true if the cue is stopping; false if otherwise.
        bool IsStopping { get; }
        //
        // Summary:
        //     Returns the friendly name of the cue.
        //
        // Returns:
        //     Friendly name of the cue.
        string Name { get; }
        // Summary:
        //     Calculates the 3D audio values between an AudioEmitter and an AudioListener
        //     object, and applies the resulting values to this Cue.
        //
        // Parameters:
        //   listener:
        //     The listener to calculate.
        //
        //   emitter:
        //     The emitter to calculate.
        void Apply3D(AudioListener listener, AudioEmitter emitter);
        //
        // Summary:
        //     Gets a cue-instance variable value based on its friendly name.
        //
        // Parameters:
        //   name:
        //     Friendly name of the variable.
        //
        // Returns:
        //     Value of the variable.
        float GetVariable(string name);
        //
        // Summary:
        //     Pauses playback.
        void Pause();
        //
        // Summary:
        //     Requests playback of a prepared or preparing Cue.
        void Play();
        //
        // Summary:
        //     Resumes playback of a paused Cue.
        void Resume();
        //
        // Summary:
        //     Sets the value of a cue-instance variable based on its friendly name.
        //
        // Parameters:
        //   name:
        //     Friendly name of the variable to set.
        //
        //   value:
        //     Value to assign to the variable.
        void SetVariable(string name, float value);
        //
        // Summary:
        //     Stops playback of a Cue.
        //
        // Parameters:
        //   options:
        //     Enumerated value specifying how the sound should stop. If set to None, the
        //     sound will play any release phase or transition specified in the audio designer.
        //     If set to Immediate, the sound will stop immediately, ignoring any release
        //     phases or transitions.
        void Stop(AudioStopOptions options);
    }
}
