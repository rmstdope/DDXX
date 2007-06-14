using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface IAnimationController : IDisposable
    {
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        bool Disposed { get; }
        //
        // Summary:
        //     Retrieves the maximum number of animation outputs the controller can support.
        int MaxNumberAnimationOutputs { get; }
        //
        // Summary:
        //     Retrieves the maximum number of animation sets the controller can support.
        int MaxNumberAnimationSets { get; }
        //
        // Summary:
        //     Retrieves the maximum number of events the controller can support.
        int MaxNumberEvents { get; }
        //
        // Summary:
        //     Retrieves the maximum number of tracks in the mixer.
        int MaxNumberTracks { get; }
        //
        // Summary:
        //     Returns the number of animation sets currently registered in the animation
        //     controller.
        int NumberAnimationSets { get; }
        //
        // Summary:
        //     Retrieves or sets the track blending weight.
        float PriorityBlend { get; set; }
        //
        // Summary:
        //     Retrieves the local animation time.
        double Time { get; }
        // Summary:
        //     Animates the mesh and advances the global animation time by a specified amount.
        //
        // Parameters:
        //   timeDelta:
        //     Amount, in seconds, by which to advance the global animation time. This value
        //     must be non-negative or 0.
        void AdvanceTime(double timeDelta);
        //
        // Summary:
        //     Animates the mesh and advances the global animation time by a specified amount.
        //
        // Parameters:
        //   timeDelta:
        //     Amount, in seconds, by which to advance the global animation time. This value
        //     must be non-negative or 0.
        //
        //   callbackHandler:
        //     A Microsoft.DirectX.Direct3D.HandleAnimationCallback user-defined animation
        //     callback handler.
        void AdvanceTime(double timeDelta, HandleAnimationCallback callbackHandler);
        //
        // Summary:
        //     Clones, or copies, an animation controller.
        //
        // Parameters:
        //   maxNumberAnimationOutputs:
        //     Maximum number of animation outputs the controller can support.
        //
        //   maxNumberAnimationSets:
        //     Maximum number of animation sets the controller can support.
        //
        //   maxNumberTracks:
        //     Maximum number of tracks the controller can support.
        //
        //   maxNumberEvents:
        //     Maximum number of events the controller can support.
        //
        // Returns:
        //     A cloned Microsoft.DirectX.Direct3D.AnimationController object.
        AnimationController Clone(int maxNumberAnimationOutputs, int maxNumberAnimationSets, int maxNumberTracks, int maxNumberEvents);
        //
        // Summary:
        //     Retrieves the animation set.
        //
        // Parameters:
        //   index:
        //     An integer that represent the index of the animation set to retrieve.
        //
        // Returns:
        //     An Microsoft.DirectX.Direct3D.AnimationSet object at the given Microsoft.DirectX.Direct3D.AnimationController.GetAnimationSet().
        AnimationSet GetAnimationSet(int index);
        //
        // Summary:
        //     Retrieves the animation set.
        //
        // Parameters:
        //   animationName:
        //     A string that contains the name of the animation set to retrieve.
        //
        // Returns:
        //     An Microsoft.DirectX.Direct3D.AnimationSet object at the given Microsoft.DirectX.Direct3D.AnimationController.GetAnimationSet().
        AnimationSet GetAnimationSet(string animationName);
        //
        // Summary:
        //     Returns an event handle to a priority blend event that is currently running.
        //
        // Returns:
        //     Priority event. If no priority blend event is currently running, a value
        //     of 0 is returned.
        int GetCurrentPriorityBlend();
        //
        // Summary:
        //     Returns an event handle to the event that is currently running on the specified
        //     animation track.
        //
        // Parameters:
        //   track:
        //     Track identifier.
        //
        //   eventType:
        //     Type of event to query.
        //
        // Returns:
        //     Event currently running on the specified track. If no event is running on
        //     the specified track, a value of 0 is returned.
        int GetCurrentTrackEvent(int track, EventType eventType);
        //
        // Summary:
        //     Retrieves the animation set for a given track.
        //
        // Parameters:
        //   track:
        //     Track identifier.
        //
        // Returns:
        //     An Microsoft.DirectX.Direct3D.AnimationSet class for the given track.
        AnimationSet GetTrackAnimationSet(int track);
        //
        // Summary:
        //     Retrieves the description for a track.
        //
        // Parameters:
        //   trackNumber:
        //     Track identifier.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.TrackDescription structure.
        TrackDescription GetTrackDescription(int trackNumber);
        //
        // Summary:
        //     Returns an event handle to the next priority blend event scheduled to occur
        //     after a specified event.
        //
        // Parameters:
        //   eventHandle:
        //     Event handle to the specified event. If set to 0, the method returns the
        //     next scheduled priority blend event.
        //
        // Returns:
        //     Event handle to the next scheduled priority blend event. If no new priority
        //     blend event is scheduled, a value of 0 is returned.
        int GetUpcomingPriorityBlend(int eventHandle);
        //
        // Summary:
        //     Returns an event handle to the next event scheduled to occur after a specified
        //     event on an animation track.
        //
        // Parameters:
        //   track:
        //     Track identifier.
        //
        //   eventHandle:
        //     Event handle to the specified event. If set to 0, the method returns the
        //     next scheduled event.
        //
        // Returns:
        //     Event handle to the next event scheduled to run on the specified track. If
        //     no new event is scheduled, a value of 0 is returned.
        int GetUpcomingTrackEvent(int track, int eventHandle);
        //
        // Summary:
        //     Determines whether a specified event handle is valid and an animation event
        //     has completed.
        //
        // Parameters:
        //   eventHandle:
        //     Event handle to the animation event.
        //
        // Returns:
        //     Returns true if the method succeeds; false if it fails.
        bool IsEventValid(int eventHandle);
        //
        // Summary:
        //     Determines whether a specified event handle is valid and an animation event
        //     has completed.
        //
        // Parameters:
        //   eventHandle:
        //     Event handle to the animation event.
        //
        //   result:
        //     HRESULT code passed back from the method.
        //
        // Returns:
        //     Returns true if the method succeeds; false if it fails.
        bool IsEventValid(int eventHandle, out int result);
        //
        // Summary:
        //     Sets blending event keys for the specified animation track.
        //
        // Parameters:
        //   newBlendWeight:
        //     Number between 0 and 1 that is used to blend tracks.
        //
        //   startTime:
        //     Global time at which to start the blend.
        //
        //   duration:
        //     Global time duration of the blend.
        //
        //   method:
        //     Transition type used for the duration of the blend. For more information,
        //     see Microsoft.DirectX.Direct3D.TransitionType.
        //
        // Returns:
        //     Handle to the priority blend event. If one or more of the input parameters
        //     is invalid, or if no free event is available, a value of 0 is returned.
        int KeyPriorityBlend(float newBlendWeight, double startTime, double duration, TransitionType method);
        //
        // Summary:
        //     Sets an event key that enables or disables an animation track.
        //
        // Parameters:
        //   track:
        //     Identifier of the animation track to modify.
        //
        //   newEnable:
        //     Set to true to enable the animation track. Set to false to disable the animation
        //     track.
        //
        //   startTime:
        //     Global time key that specifies the global time at which the change will occur.
        //
        // Returns:
        //     Priority blend event. If the track is invalid, a value of 0 is returned.
        int KeyTrackEnable(int track, bool newEnable, double startTime);
        //
        // Summary:
        //     Sets an event key that changes the local time of an animation track.
        //
        // Parameters:
        //   track:
        //     Identifier of the track to modify.
        //
        //   newPosition:
        //     New local time of the animation track.
        //
        //   startTime:
        //     Global time key that specifies the global time at which the change will occur.
        //
        // Returns:
        //     Priority blend event. If track is invalid, or if no free event is available,
        //     a value of 0 is returned.
        int KeyTrackPosition(int track, double newPosition, double startTime);
        //
        // Summary:
        //     Sets an event key that changes the local time of an animation track.
        //
        // Parameters:
        //   track:
        //     Identifier of the track to modify.
        //
        //   newSpeed:
        //     New speed of the animation track.
        //
        //   startTime:
        //     Global time key that specifies the global time at which the change will occur.
        //
        //   duration:
        //     Transition time, which specifies how long the smooth transition will take
        //     to complete.
        //
        //   method:
        //     Transition type used for transitioning between speeds. For more information,
        //     see Microsoft.DirectX.Direct3D.TransitionType.
        //
        // Returns:
        //     Event handle to the priority blend event. If one or more of the input parameters
        //     is invalid, or if no free event is available, a value of 0 is returned.
        int KeyTrackSpeed(int track, float newSpeed, double startTime, double duration, TransitionType method);
        //
        // Summary:
        //     Sets an event key that changes the weight of an animation track.
        //
        // Parameters:
        //   track:
        //     Identifier of the track to modify.
        //
        //   newWeight:
        //     New weight of the track.
        //
        //   startTime:
        //     Global time key that specifies the global time at which the change will occur.
        //
        //   duration:
        //     Transition time, which specifies how long the smooth transition will take
        //     to complete.
        //
        //   method:
        //     Transition type used for transitioning between weights. For more information,
        //     see Microsoft.DirectX.Direct3D.TransitionType.
        //
        // Returns:
        //     Event handle to the priority blend event. If one or more of the input parameters
        //     is invalid, or if no free event is available, a value of 0 is returned.
        int KeyTrackWeight(int track, float newWeight, double startTime, double duration, TransitionType method);
        //
        // Summary:
        //     Adds an animation output to the animation controller and registers objects
        //     for scale, rotate, and translate (SRT) transformations.
        //
        // Parameters:
        //   output:
        //     An Microsoft.DirectX.Direct3D.AnimationOutput object that encapsulates the
        //     scale, rotate, and translate (SRT) transformations objects.
        void RegisterAnimationOutput(AnimationOutput output);
        //
        // Summary:
        //     Adds an animation output to the animation controller and registers objects
        //     for scale, rotate, and translate (SRT) transformations.
        //
        // Parameters:
        //   animationFrame:
        //     A Microsoft.DirectX.Direct3D.Frame object that encapsulates a transform frame
        //     in a transformation frame hierarchy. Uses the Microsoft.DirectX.Direct3D.Frame.TransformationMatrix
        //     property.
        void RegisterAnimationOutput(Frame animationFrame);
        //
        // Summary:
        //     Adds an animation set to the animation controller.
        //
        // Parameters:
        //   animationSet:
        //     The Microsoft.DirectX.Direct3D.AnimationSet to add.
        void RegisterAnimationSet(AnimationSet animationSet);
        //
        // Summary:
        //     Resets the global animation time to zero. Any pending events retain their
        //     original schedules, but in the new time frame.
        void ResetTime();
        //
        // Summary:
        //     Applies the animation set to the specified track.
        //
        // Parameters:
        //   track:
        //     Identifier of the track to which the animation set is applied.
        //
        //   animationSet:
        //     The Microsoft.DirectX.Direct3D.AnimationSet to add to the track.
        void SetTrackAnimationSet(int track, AnimationSet animationSet);
        //
        // Summary:
        //     Sets the track description.
        //
        // Parameters:
        //   trackNumber:
        //     Identifier of the track to modify.
        //
        //   value:
        //     Description of the track.
        void SetTrackDescription(int trackNumber, TrackDescription value);
        //
        // Summary:
        //     Enables or disables a track in the animation controller.
        //
        // Parameters:
        //   track:
        //     Track identifier.
        //
        //   enable:
        //     Set to true if the track is enabled in the controller. Set to false if the
        //     track should not be mixed.
        void SetTrackEnable(int track, bool enable);
        //
        // Summary:
        //     Sets the track to the specified local animation time.
        //
        // Parameters:
        //   track:
        //     Track identifier.
        //
        //   position:
        //     Local animation time value to assign to the track.
        void SetTrackPosition(int track, double position);
        //
        // Summary:
        //     Sets the priority blending weight for the specified animation track.
        //
        // Parameters:
        //   track:
        //     Track identifier.
        //
        //   priority:
        //     Track priority; should be set to one of the Microsoft.DirectX.Direct3D.PriorityType
        //     constants.
        void SetTrackPriority(int track, PriorityType priority);
        //
        // Summary:
        //     Sets the priority blending weight for the specified animation track.
        //
        // Parameters:
        //   track:
        //     Track identifier.
        //
        //   speed:
        //     Track priority; should be set to one of the constants from Microsoft.DirectX.Direct3D.PriorityType.
        void SetTrackSpeed(int track, float speed);
        //
        // Summary:
        //     Sets the track weight, which is used to determine how to blend multiple tracks
        //     together.
        //
        // Parameters:
        //   track:
        //     Identifier of the track for which to set the weight.
        //
        //   weight:
        //     Weight value.
        void SetTrackWeight(int track, float weight);
        //
        // Summary:
        //     Removes all scheduled priority blend events from the animation controller.
        void UnkeyAllPriorityBlends();
        //
        // Summary:
        //     Removes all events from a specified animation track.
        //
        // Parameters:
        //   track:
        //     Identifier of the track from which to remove events.
        void UnkeyAllTrackEvents(int track);
        //
        // Summary:
        //     Removes a specified event from an animation track, preventing the event from
        //     being run.
        //
        // Parameters:
        //   eventHandle:
        //     Event handle to the event to remove.
        void UnkeyEvent(int eventHandle);
        //
        // Summary:
        //     Removes an animation output from the animation controller.
        //
        // Parameters:
        //   output:
        //     An Microsoft.DirectX.Direct3D.AnimationOutput object to remove.
        void UnregisterAnimationOutput(AnimationOutput output);
        //
        // Summary:
        //     Removes an animation output from the animation controller.
        //
        // Parameters:
        //   animationFrame:
        //     A Microsoft.DirectX.Direct3D.Frame object to remove.
        void UnregisterAnimationOutput(Frame animationFrame);
        //
        // Summary:
        //     Removes an animation output from the animation controller.
        //
        // Parameters:
        //   name:
        //     A string that represents the name of the animation output to remove.
        void UnregisterAnimationOutput(string name);
        //
        // Summary:
        //     Removes an animation set from the animation controller.
        //
        // Parameters:
        //   animationSet:
        //     The Microsoft.DirectX.Direct3D.AnimationSet to remove.
        void UnregisterAnimationSet(AnimationSet animationSet);
    }
}
