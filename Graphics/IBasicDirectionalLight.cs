using System;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface IBasicDirectionalLight
    {
        // Summary:
        //     Gets or sets the diffuse color of this light.
        //
        // Returns:
        //     The diffuse color of this light.
        Vector3 DiffuseColor { get; set; }
        //
        // Summary:
        //     Gets or sets light direciton.
        //
        // Returns:
        //     Gets or sets the light direciton. This value must be a unit vector.
        Vector3 Direction { get; set; }
        //
        // Summary:
        //     Enables this light.
        //
        // Returns:
        //     true to enable this light; false otherwise.
        bool Enabled { get; set; }
        //
        // Summary:
        //     Gets or sets the specular color of the light.
        //
        // Returns:
        //     The specular color of the light.
        Vector3 SpecularColor { get; set; }
    }
}
