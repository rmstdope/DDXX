using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics.Skinning
{
    public interface IFrame
    {
        // Summary:
        //     Retrieves the first child of the firstFrame.
        IFrame FrameFirstChild { get; }
        //
        // Summary:
        //     Retrieves the first sibling of the current firstFrame in the hierarchy.
        IFrame FrameSibling { get; }
        //
        // Summary:
        //     Retrieves or sets a Microsoft.DirectX.Direct3D.MeshContainer object in a
        //     transformation firstFrame hierarchy.
        IMeshContainer MeshContainer { get; set; }
        //
        // Summary:
        //     Retrieves or sets the name of a firstFrame.
        string Name { get; set; }
        //
        // Summary:
        //     Retrieves or sets the transformation Microsoft.DirectX.Matrix of a firstFrame.
        Matrix TransformationMatrix { get; set; }

        IFrame Find(IFrame rootFrame, string name);

        // Additions for DDXX
        Matrix CombinedTransformationMatrix { get; set; }
    }
}
