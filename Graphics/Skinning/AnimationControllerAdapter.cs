using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    class AnimationControllerAdapter : IAnimationController
    {
        private AnimationController controller;

        public AnimationControllerAdapter(AnimationController controller)
        {
            this.controller = controller;
        }

        #region IAnimationController Members

        public bool Disposed
        {
            get { return controller.Disposed; }
        }

        public int MaxNumberAnimationOutputs
        {
            get { return controller.MaxNumberAnimationOutputs; }
        }

        public int MaxNumberAnimationSets
        {
            get { return controller.MaxNumberAnimationSets; }
        }

        public int MaxNumberEvents
        {
            get { return controller.MaxNumberEvents; }
        }

        public int MaxNumberTracks
        {
            get { return controller.MaxNumberTracks; }
        }

        public int NumberAnimationSets
        {
            get { return controller.NumberAnimationSets; }
        }

        public float PriorityBlend
        {
            get
            {
                return controller.PriorityBlend;
            }
            set
            {
                controller.PriorityBlend = value;
            }
        }

        public double Time
        {
            get { return controller.Time; }
        }

        public void AdvanceTime(double timeDelta)
        {
            controller.AdvanceTime(timeDelta);
        }

        public void AdvanceTime(double timeDelta, HandleAnimationCallback callbackHandler)
        {
            controller.AdvanceTime(timeDelta, callbackHandler);
        }

        public AnimationController Clone(int maxNumberAnimationOutputs, int maxNumberAnimationSets, int maxNumberTracks, int maxNumberEvents)
        {
            return controller.Clone(maxNumberAnimationOutputs, maxNumberAnimationSets, maxNumberTracks, maxNumberEvents);
        }

        public AnimationSet GetAnimationSet(int index)
        {
            return controller.GetAnimationSet(index);
        }

        public AnimationSet GetAnimationSet(string animationName)
        {
            return controller.GetAnimationSet(animationName);
        }

        public int GetCurrentPriorityBlend()
        {
            return controller.GetCurrentPriorityBlend();
        }

        public int GetCurrentTrackEvent(int track, EventType eventType)
        {
            return controller.GetCurrentTrackEvent(track, eventType);
        }

        public AnimationSet GetTrackAnimationSet(int track)
        {
            return controller.GetTrackAnimationSet(track);
        }

        public TrackDescription GetTrackDescription(int trackNumber)
        {
            return controller.GetTrackDescription(trackNumber);
        }

        public int GetUpcomingPriorityBlend(int eventHandle)
        {
            return controller.GetUpcomingPriorityBlend(eventHandle);
        }

        public int GetUpcomingTrackEvent(int track, int eventHandle)
        {
            return controller.GetUpcomingTrackEvent(track, eventHandle);
        }

        public bool IsEventValid(int eventHandle)
        {
            return controller.IsEventValid(eventHandle);
        }

        public bool IsEventValid(int eventHandle, out int result)
        {
            return controller.IsEventValid(eventHandle, out result);
        }

        public int KeyPriorityBlend(float newBlendWeight, double startTime, double duration, TransitionType method)
        {
            return controller.KeyPriorityBlend(newBlendWeight, startTime, duration, method);
        }

        public int KeyTrackEnable(int track, bool newEnable, double startTime)
        {
            return controller.KeyTrackEnable(track, newEnable, startTime);
        }

        public int KeyTrackPosition(int track, double newPosition, double startTime)
        {
            return controller.KeyTrackPosition(track, newPosition, startTime);
        }

        public int KeyTrackSpeed(int track, float newSpeed, double startTime, double duration, TransitionType method)
        {
            return controller.KeyTrackSpeed(track, newSpeed, startTime, duration, method);
        }

        public int KeyTrackWeight(int track, float newWeight, double startTime, double duration, TransitionType method)
        {
            return controller.KeyTrackWeight(track, newWeight, startTime, duration, method);
        }

        public void RegisterAnimationOutput(AnimationOutput output)
        {
            controller.RegisterAnimationOutput(output);
        }

        public void RegisterAnimationOutput(Frame animationFrame)
        {
            controller.RegisterAnimationOutput(animationFrame);
        }

        public void RegisterAnimationSet(AnimationSet animationSet)
        {
            controller.RegisterAnimationSet(animationSet);
        }

        public void ResetTime()
        {
            controller.ResetTime();
        }

        public void SetTrackAnimationSet(int track, AnimationSet animationSet)
        {
            controller.SetTrackAnimationSet(track, animationSet);
        }

        public void SetTrackDescription(int trackNumber, TrackDescription value)
        {
            controller.SetTrackDescription(trackNumber, value);
        }

        public void SetTrackEnable(int track, bool enable)
        {
            controller.SetTrackEnable(track, enable);
        }

        public void SetTrackPosition(int track, double position)
        {
            controller.SetTrackPosition(track, position);
        }

        public void SetTrackPriority(int track, PriorityType priority)
        {
            controller.SetTrackPriority(track, priority);
        }

        public void SetTrackSpeed(int track, float speed)
        {
            controller.SetTrackSpeed(track, speed);
        }

        public void SetTrackWeight(int track, float weight)
        {
            controller.SetTrackWeight(track, weight);
        }

        public void UnkeyAllPriorityBlends()
        {
            controller.UnkeyAllPriorityBlends();
        }

        public void UnkeyAllTrackEvents(int track)
        {
            controller.UnkeyAllTrackEvents(track);
        }

        public void UnkeyEvent(int eventHandle)
        {
            controller.UnkeyEvent(eventHandle);
        }

        public void UnregisterAnimationOutput(AnimationOutput output)
        {
            controller.UnregisterAnimationOutput(output);
        }

        public void UnregisterAnimationOutput(Frame animationFrame)
        {
            controller.UnregisterAnimationOutput(animationFrame);
        }

        public void UnregisterAnimationOutput(string name)
        {
            controller.UnregisterAnimationOutput(name);
        }

        public void UnregisterAnimationSet(AnimationSet animationSet)
        {
            controller.UnregisterAnimationSet(animationSet);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            controller.Dispose();
        }

        #endregion

    }
}
