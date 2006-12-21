using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics.Skinning
{
    public interface IFrame
    {
        // Summary:
        //     Retrieves the first child of the frame1.
        IFrame FrameFirstChild { get; }
        //
        // Summary:
        //     Retrieves the first sibling of the current frame1 in the hierarchy.
        IFrame FrameSibling { get; }
        //
        // Summary:
        //     Retrieves or sets a Microsoft.DirectX.Direct3D.MeshContainer object in a
        //     transformation frame1 hierarchy.
        IMeshContainer MeshContainer { get; set; }
        //
        // Summary:
        //     Retrieves or sets the name of a frame1.
        string Name { get; set; }
        //
        // Summary:
        //     Retrieves or sets the transformation Microsoft.DirectX.Matrix of a frame1.
        Matrix TransformationMatrix { get; set; }
    }
}
