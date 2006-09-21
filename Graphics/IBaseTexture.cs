using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface IBaseTexture
    {
        // Summary:
        //     Retrieves the device associated with a resource.
        Device Device { get; }
        //
        // Summary:
        //     Retrieves or sets the priority for the current resource.
        int Priority { get; set; }
        //
        // Summary:
        //     Retrieves the type of a resource.
        ResourceType Type { get; }
        // Summary:
        //     Immediately releases the unmanaged resources used by the Microsoft.DirectX.Direct3D.Resource
        //     object.
        void Dispose();
        //
        // Summary:
        //     Preloads a managed resource.
        void PreLoad();
        //
        // Summary:
        //     Assigns the resource-management priority for the current resource.
        //
        // Parameters:
        //   priorityNew:
        //     New resource-management priority for the resource.
        //
        // Returns:
        //     Previous priority value for the resource.
        int SetPriority(int priorityNew);
        // Summary:
        //     Retrieves or sets the filter type used for automatically generated sublevels.
        TextureFilter AutoGenerateFilterType { get; set; }
        //
        // Summary:
        //     Retrieves the number of texture levels in a multilevel texture.
        int LevelCount { get; }
        //
        // Summary:
        //     Retrieves or sets the most detailed level of detail (LOD) for a managed texture.
        int LevelOfDetail { get; set; }
        //
        // Summary:
        //     Generates mipmap sublevels.
        void GenerateMipSubLevels();
        //
        // Summary:
        //     Sets the most detailed level of detail (LOD) for a managed texture.
        //
        // Parameters:
        //   lodNew:
        //     Most detailed LOD value to set for the chain.
        //
        // Returns:
        //     An System.Int32 value, clamped to the maximum LOD value (one less than the
        //     total number of levels). Subsequent calls to this method return the maximum
        //     value, not the previously set LOD value.
        int SetLevelOfDetail(int lodNew);
    }
}
