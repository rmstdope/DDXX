using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface IAnimationSet : IDisposable
    {
        //
        // Summary:
        //     Retrieves the name of an animation set.
        string Name { get; }
        //
        // Summary:
        //     Retrieves a value that indicates the number of animations in the animation
        //     set.
        int NumberAnimations { get; }
        //
        // Summary:
        //     Retrieves the period of an animation set.
        double Period { get; }
        //
        // Summary:
        //     Retrieves the index of an animation, given its name.
        //
        // Parameters:
        //   name:
        //     Animation name.
        //
        // Returns:
        //     Returns the index of the named animation; otherwise, throws a Microsoft.DirectX.Direct3D.Direct3DXException.
        int GetAnimationIndex(string name);
        //
        // Summary:
        //     Retrieves the name of an animation, given its index.
        //
        // Parameters:
        //   index:
        //     Animation index.
        //
        // Returns:
        //     Returns a string that contains the animation name; otherwise, throws a Microsoft.DirectX.Direct3D.Direct3DXException.
        string GetAnimationName(int index);
        //
        // Summary:
        //     Retrieves information about a specific callback in an animation set.
        //
        // Parameters:
        //   position:
        //     Position from which to find callbacks.
        //
        //   flags:
        //     Callback search flags. This parameter can be set to one or more Microsoft.DirectX.Direct3D.CallbackSearchFlags
        //     constants.
        //
        // Returns:
        //     This method's return values are implemented by an application programmer.
        //     In general, if no error occurs, the method should return an empty structure;
        //     otherwise, it should throw an exception. For more information, see Microsoft.DirectX.DirectXException.
        CallbackData GetCallback(double position, CallbackSearchFlags flags);
        //
        // Summary:
        //     Returns the time position in the local timeframe of an animation set.
        //
        // Parameters:
        //   position:
        //     Local time of the animation set.
        //
        // Returns:
        //     Time position as measured in the timeframe of the animation set. This value
        //     is bounded by the period of the animation set.
        double GetPeriodicPosition(double position);
        //
        // Summary:
        //     Retrieves the scale, rotate, and translate (SRT) values of an animation set.
        //
        // Parameters:
        //   periodicPosition:
        //     Position of the animation set; can be obtained by calling Microsoft.DirectX.Direct3D.AnimationSet.GetPeriodicPosition(System.Double).
        //
        //   animation:
        //     Animation index.
        //
        // Returns:
        //     Returns a Microsoft.DirectX.Direct3D.ScaleRotateTranslate structure.
        ScaleRotateTranslate GetScaleRotateTranslate(double periodicPosition, int animation);
    }
}
