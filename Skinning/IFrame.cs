using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Skinning
{
    public interface IFrame
    {
        // Summary:
        //     Retrieves the first child of the frame.
        IFrame FrameFirstChild { get; }
        //
        // Summary:
        //     Retrieves the first sibling of the current frame in the hierarchy.
        IFrame FrameSibling { get; }
        //
        // Summary:
        //     Retrieves or sets a Microsoft.DirectX.Direct3D.MeshContainer object in a
        //     transformation frame hierarchy.
        IMeshContainer MeshContainer { get; set; }
        //
        // Summary:
        //     Retrieves or sets the name of a frame.
        string Name { get; set; }
        //
        // Summary:
        //     Retrieves or sets the transformation Microsoft.DirectX.Matrix of a frame.
        Matrix TransformationMatrix { get; set; }
    }
}
