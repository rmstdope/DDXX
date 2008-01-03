using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IBasicEffect : IEffect
    {
        // Summary:
        //     Gets or sets the alpha this effect material.
        //
        // Returns:
        //     The alpha of this effect material.
        float Alpha { get; set; }
        //
        // Summary:
        //     Gets or sets the ambient light color of this effect.
        //
        // Returns:
        //     The ambient light color of this effect. Valid values are between 0 and 1.
        Vector3 AmbientLightColor { get; set; }
        //
        // Summary:
        //     Gets or sets the diffuse color of this effect material. Value takes 0 to
        //     1.
        //
        // Returns:
        //     The diffuse color of this effect material. Valid values are between 0 and
        //     1.
        Vector3 DiffuseColor { get; set; }
        //
        // Summary:
        //     Gets the first directional light for this effect.
        //
        // Returns:
        //     A directional light for this effect.
        IBasicDirectionalLight DirectionalLight0 { get; }
        //
        // Summary:
        //     Gets the second directional light for this effect.
        //
        // Returns:
        //     A directional light for this effect.
        IBasicDirectionalLight DirectionalLight1 { get; }
        //
        // Summary:
        //     Gets the third directional light for this effect.
        //
        // Returns:
        //     A directional light for this effect.
        IBasicDirectionalLight DirectionalLight2 { get; }
        //
        // Summary:
        //     Gets or sets the emissive color of the effect material.
        //
        // Returns:
        //     The emissive color of the effect material. Valid values are between 0 and
        //     1.
        Vector3 EmissiveColor { get; set; }
        //
        // Summary:
        //     Gets or sets the fog color for this effect.
        //
        // Returns:
        //     The fog color for this effect.
        Vector3 FogColor { get; set; }
        //
        // Summary:
        //     Enables fog for this effect.
        //
        // Returns:
        //     true to enable fog; false otherwise.
        bool FogEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets the ending distance of fog.
        //
        // Returns:
        //     Fog end distance specified as a positive value.
        float FogEnd { get; set; }
        //
        // Summary:
        //     Gets or sets the fog start distance.
        //
        // Returns:
        //     Fog start distance specified as a positive value.
        float FogStart { get; set; }
        //
        // Summary:
        //     Enables lighting for this effect.
        //
        // Returns:
        //     true to enable lighting; false otherwise.
        bool LightingEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating that per-pixel lighting should be used if
        //     it is available for the current adapter. Per-pixel lighting is available
        //     if a graphics adapter supports Pixel Shader Model 2.0.
        //
        // Returns:
        //     true to use per-pixel lighting if it is available; false to disable per-pixel
        //     lighting. The default value is true. When Microsoft.Xna.Framework.Graphics.BasicEffect.PreferPerPixelLighting
        //     is true, if the graphics adapter does not support a minimum of Pixel Shader
        //     Model 2.0, BasicEffect will automatically fall-back to per-vertex lighting.
        //     When Microsoft.Xna.Framework.Graphics.BasicEffect.PreferPerPixelLighting
        //     is false, per-vertex lighting is used regardless of whether per-pixel lighting
        //     is supported by the graphics adapter.
        bool PreferPerPixelLighting { get; set; }
        //
        // Summary:
        //     Gets or sets the projection matrix.
        //
        // Returns:
        //     The projection matrix.
        Matrix Projection { get; set; }
        //
        // Summary:
        //     Gets or sets the specular color of this effect material.
        //
        // Returns:
        //     The specular color of this effect material. Valid values are between 0 and
        //     1.
        Vector3 SpecularColor { get; set; }
        //
        // Summary:
        //     Gets or sets the specular power of this effect material.
        //
        // Returns:
        //     The specular power of this effect material.
        float SpecularPower { get; set; }
        //
        // Summary:
        //     Gets or sets a texture to be applied by this effect.
        //
        // Returns:
        //     Texture to be applied by this effect.
        ITexture2D Texture { get; set; }
        //
        // Summary:
        //     Enables textures for this effect.
        //
        // Returns:
        //     true to enable textures; false otherwise.
        bool TextureEnabled { get; set; }
        //
        // Summary:
        //     Enables use vertex colors for this effect.
        //
        // Returns:
        //     true to enable vertex colors; false otherwise.
        bool VertexColorEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets the view matrix.
        //
        // Returns:
        //     The view matrix.
        Matrix View { get; set; }
        //
        // Summary:
        //     Gets or sets the world matrix.
        //
        // Returns:
        //     The world matrix.
        Matrix World { get; set; }
        //
        // Summary:
        //     Enables default lighting for this effect.
        void EnableDefaultLighting();
    }
}
