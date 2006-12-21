using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    public interface IMeshData
    {
        // Summary:
        //     Retrieves or sets the Microsoft.DirectX.Direct3D.Mesh contained in the current
        //     object.
        IMesh Mesh { get; set; }
        //
        // Summary:
        //     Retrieves or sets the Microsoft.DirectX.Direct3D.PatchMesh contained in the
        //     current object.
        PatchMesh PatchMesh { get; set; }
        //
        // Summary:
        //     Retrieves or sets the Microsoft.DirectX.Direct3D.ProgressiveMesh contained
        //     in the current object.
        ProgressiveMesh ProgressiveMesh { get; set; }
    }
}
