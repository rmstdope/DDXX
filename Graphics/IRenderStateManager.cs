using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.Graphics
{
    public interface IRenderStateManager
    {
        // Summary:
        //     Amount to adaptively tessellate in the w direction. The default value is
        //     0.0f.
        float AdaptiveTessellateW { get; set; }
        //
        // Summary:
        //     Amount to adaptively tessellate in the x direction. The default value is
        //     0.0f.
        float AdaptiveTessellateX { get; set; }
        //
        // Summary:
        //     Amount to adaptively tessellate in the y direction. The default value is
        //     0.0f.
        float AdaptiveTessellateY { get; set; }
        //
        // Summary:
        //     Amount to adaptively tessellate in the z direction. The default value is
        //     1.0f.
        float AdaptiveTessellateZ { get; set; }
        //
        // Summary:
        //     Retrieves or sets a value to enable alpha-blended transparency.
        bool AlphaBlendEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets a value to select the arithmetic operation applied to the
        //     separate for the alpha channel (also known as separate alpha blending) when
        //     the render state, Microsoft.DirectX.Direct3D.RenderStateManager.SeparateAlphaBlendEnabled,
        //     is set to true.
        BlendOperation AlphaBlendOperation { get; set; }
        //
        // Summary:
        //     Contains a member of the Microsoft.DirectX.Direct3D.Blend enumeration that
        //     represents the destination .
        Blend AlphaDestinationBlend { get; set; }
        //
        // Summary:
        //     Retrieves or sets the comparison function for the alpha test.
        Compare AlphaFunction { get; set; }
        //
        // Summary:
        //     Contains a member of the Microsoft.DirectX.Direct3D.Blend enumeration that
        //     represents the source .
        Blend AlphaSourceBlend { get; set; }
        //
        // Summary:
        //     Retrieves or sets a render state that enables a per-pixel alpha test.
        bool AlphaTestEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets the ambient light color.
        Color Ambient { get; set; }
        //
        // Summary:
        //     Retrieves or sets the ambient light color.
        int AmbientColor { get; set; }
        //
        // Summary:
        //     Retrieves or sets the ambient color source for lighting calculations.
        ColorSource AmbientMaterialSource { get; set; }
        //
        // Summary:
        //     Retrieves or sets antialiasing of lines.
        bool AntiAliasedLineEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets a System.Drawing.Color object to use for a constant blend
        //     factor during alpha blending.
        Color BlendFactor { get; set; }
        //
        // Summary:
        //     Retrieves or sets a System.Drawing.Color object to use for a constant blend
        //     factor during alpha blending.
        int BlendFactorColor { get; set; }
        //
        // Summary:
        //     Retrieves or sets a value used to select the arithmetic operation to apply
        //     when the alpha blend render state, Microsoft.DirectX.Direct3D.RenderStateManager.AlphaBlendEnable,
        //     is set to true.
        BlendOperation BlendOperation { get; set; }
        //
        // Summary:
        //     Retrieves or sets a value to enable primitive clipping by Microsoft Direct3D.
        bool Clipping { get; set; }
        //
        // Summary:
        //     Retrieves or sets a value to enable or disable per-vertex color.
        bool ColorVertex { get; set; }
        //
        // Summary:
        //     Retrieves or sets a value that enables a per-channel write for the render
        //     target color buffer.
        ColorWriteEnable ColorWriteEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets additional Microsoft.DirectX.Direct3D.RenderStateManager.ColorWriteEnable
        //     values for the device.
        ColorWriteEnable ColorWriteEnable1 { get; set; }
        //
        // Summary:
        //     Retrieves or sets additional Microsoft.DirectX.Direct3D.RenderStateManager.ColorWriteEnable
        //     values for devices.
        ColorWriteEnable ColorWriteEnable2 { get; set; }
        //
        // Summary:
        //     Retrieves or sets additional Microsoft.DirectX.Direct3D.RenderStateManager.ColorWriteEnable
        //     values for devices.
        ColorWriteEnable ColorWriteEnable3 { get; set; }
        //
        // Summary:
        //     Retrieves the Microsoft.DirectX.Direct3D.StencilOperation or sets it to perform
        //     if the counterclockwise (CCW) stencil test fails.
        StencilOperation CounterClockwiseStencilFail { get; set; }
        //
        // Summary:
        //     Retrieves or sets the comparison function used by the counterclockwise (CCW)
        //     stencil test.
        Compare CounterClockwiseStencilFunction { get; set; }
        //
        // Summary:
        //     Retrieves the Microsoft.DirectX.Direct3D.StencilOperation or sets it to perform,
        //     if both counterclockwise (CCW) stencil and z-tests pass.
        StencilOperation CounterClockwiseStencilPass { get; set; }
        //
        // Summary:
        //     Retrieves the Microsoft.DirectX.Direct3D.StencilOperation or sets it to perform
        //     if the counterclockwise (CCW) stencil test passes and the z-test fails.
        StencilOperation CounterClockwiseStencilZBufferFail { get; set; }
        //
        // Summary:
        //     Specifies how back-facing triangles are culled, if at all.
        Cull CullMode { get; set; }
        //
        // Summary:
        //     Enables or disables the debug monitor token.
        bool DebugMonitorTokenEnabled { get; set; }
        //
        // Summary:
        //     Sets or retrieves the depth bias for polygons.
        float DepthBias { get; set; }
        //
        // Summary:
        //     Contains a member of the Microsoft.DirectX.Direct3D.Blend enumeration that
        //     represents the destination .
        Blend DestinationBlend { get; set; }
        //
        // Summary:
        //     Retrieves or sets the diffuse color source for lighting calculations.
        ColorSource DiffuseMaterialSource { get; set; }
        //
        // Summary:
        //     Enables or disables dithering.
        bool DitherEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets the emissive color source for lighting calculations.
        ColorSource EmissiveMaterialSource { get; set; }
        //
        // Summary:
        //     Enables or disables adaptive tessellation.
        bool EnableAdaptiveTessellation { get; set; }
        //
        // Summary:
        //     Contains a member of the Microsoft.DirectX.Direct3D.FillMode enumeration
        //     that represents the fill mode.
        FillMode FillMode { get; set; }
        //
        // Summary:
        //     Retrieves or sets the fog color.
        Color FogColor { get; set; }
        //
        // Summary:
        //     Retrieves or sets the fog color.
        int FogColorValue { get; set; }
        //
        // Summary:
        //     Retrieves or sets the fog density for pixel or vertex fog used in exponential
        //     fog modes.
        float FogDensity { get; set; }
        //
        // Summary:
        //     Enables or disables fog blending.
        bool FogEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets the depth at which pixel or vertex fog effects end for
        //     linear fog mode.
        float FogEnd { get; set; }
        //
        // Summary:
        //     Retrieves or sets the depth at which pixel or vertex fog effects begin for
        //     linear fog mode.
        float FogStart { get; set; }
        //
        // Summary:
        //     Retrieves or sets the fog formula to use for pixel fog.
        FogMode FogTableMode { get; set; }
        //
        // Summary:
        //     Retrieves or sets the fog formula to use for vertex fog.
        FogMode FogVertexMode { get; set; }
        //
        // Summary:
        //     Enables or disables indexed vertex blending.
        bool IndexedVertexBlendEnable { get; set; }
        //
        // Summary:
        //     Enables or disables drawing of the last pixel in a line.
        bool LastPixel { get; set; }
        //
        // Summary:
        //     Enables or disables Microsoft Direct3D lighting.
        bool Lighting { get; set; }
        //
        // Summary:
        //     Specifies whether to use camera-relative specular highlights or orthogonal
        //     specular highlights.
        bool LocalViewer { get; set; }
        //
        // Summary:
        //     Retrieves or sets the maximum tessellation level.
        float MaxTessellationLevel { get; set; }
        //
        // Summary:
        //     Retrieves or sets the minimum tessellation level.
        float MinTessellationLevel { get; set; }
        //
        // Summary:
        //     Determines how individual samples are computed when using a multisample render
        //     target buffer.
        bool MultiSampleAntiAlias { get; set; }
        //
        // Summary:
        //     Retrieves or sets a render state that enables use of a multisample buffer
        //     as an accumulation buffer.
        int MultiSampleMask { get; set; }
        //
        // Summary:
        //     Retrieves or sets the degree of interpolation (linear, cubic, quadratic,
        //     or quintic) using the N-patch normal.
        DegreeType NormalDegree { get; set; }
        //
        // Summary:
        //     Enables or disables automatic normalization of vertex normals.
        bool NormalizeNormals { get; set; }
        //
        // Summary:
        //     Retrieves or sets the tessellation mode for patch edges.
        PatchEdge PatchEdgeStyle { get; set; }
        //
        // Summary:
        //     Controls the distance-based size attenuation for point primitives.
        float PointScaleA { get; set; }
        //
        // Summary:
        //     Controls the distance-based size attenuation for point primitives.
        float PointScaleB { get; set; }
        //
        // Summary:
        //     Controls the distance-based size attenuation for point primitives.
        float PointScaleC { get; set; }
        //
        // Summary:
        //     Controls how the computation of size for point sprites is handled.
        bool PointScaleEnable { get; set; }
        //
        // Summary:
        //     Specifies the size to use for point size computation in cases in which point
        //     size is not specified for each vertex.
        float PointSize { get; set; }
        //
        // Summary:
        //     Specifies the maximum size to which point sprites can be set.
        float PointSizeMax { get; set; }
        //
        // Summary:
        //     Specifies the minimum size to which point sprites can be set.
        float PointSizeMin { get; set; }
        //
        // Summary:
        //     Controls how point sprites are rendered.
        bool PointSpriteEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets the N-patch position interpolation degree.
        DegreeType PositionDegree { get; set; }
        //
        // Summary:
        //     Retrieves or sets enabling of range-based vertex fog.
        bool RangeFogEnable { get; set; }
        //
        // Summary:
        //     Specifies a reference alpha value against which pixels are tested when alpha
        //     testing is enabled.
        int ReferenceAlpha { get; set; }
        //
        // Summary:
        //     Specifies a reference value to use for the stencil test.
        int ReferenceStencil { get; set; }
        //
        // Summary:
        //     Enables or disables scissor testing.
        bool ScissorTestEnable { get; set; }
        //
        // Summary:
        //     Enables or disables the separate for the alpha channel.
        bool SeparateAlphaBlendEnabled { get; set; }
        //
        // Summary:
        //     Contains one or more members of the Microsoft.DirectX.Direct3D.ShadeMode
        //     enumeration.
        ShadeMode ShadeMode { get; set; }
        //
        // Summary:
        //     Retrieves or sets a value used to determine how much bias can be applied
        //     to coplanar primitives to reduce z-fighting.
        float SlopeScaleDepthBias { get; set; }
        //
        // Summary:
        //     Contains a member of the Microsoft.DirectX.Direct3D.Blend enumeration.
        Blend SourceBlend { get; set; }
        //
        // Summary:
        //     Retrieves or sets a render state that enables specular highlights.
        bool SpecularEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets the specular color source for lighting calculations.
        ColorSource SpecularMaterialSource { get; set; }
        //
        // Summary:
        //     Retrieves or sets a render state that enables render-target writes to be
        //     gamma corrected to sRGB.
        bool SrgbWriteEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets stencil enabling.
        bool StencilEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets the stencil operation to perform if the stencil test fails.
        StencilOperation StencilFail { get; set; }
        //
        // Summary:
        //     Retrieves or sets the comparison function for the stencil test.
        Compare StencilFunction { get; set; }
        //
        // Summary:
        //     Retrieves or sets the mask applied to the reference value and each stencil
        //     buffer entry to determine the significant bits for the stencil test.
        int StencilMask { get; set; }
        //
        // Summary:
        //     Retrieves or sets the stencil operation to perform if both the stencil test
        //     and the depth test (z-test) pass.
        StencilOperation StencilPass { get; set; }
        //
        // Summary:
        //     Retrieves or sets the write mask applied to values written into the stencil
        //     buffer.
        int StencilWriteMask { get; set; }
        //
        // Summary:
        //     Retrieves or sets the stencil operation to perform if the stencil test passes
        //     and the depth test (z-test) fails.
        StencilOperation StencilZBufferFail { get; set; }
        //
        // Summary:
        //     Retrieves or sets the color used for multiple-texture blending with the Microsoft.DirectX.Direct3D.TextureArgument.TextureArgument.TFactor
        //     texture blending argument or the Microsoft.DirectX.Direct3D.TextureOperation.TextureOperation.BlendFactorAlpha
        //     texture blending operation.
        int TextureFactor { get; set; }
        //
        // Summary:
        //     Retrieves or sets a floating-point value that controls the tween factor.
        float TweenFactor { get; set; }
        //
        // Summary:
        //     Enables or disables two-sided stenciling.
        bool TwoSidedStencilMode { get; set; }
        //
        // Summary:
        //     Enables or disables w-buffering.
        bool UseWBuffer { get; set; }
        //
        // Summary:
        //     Retrieves or sets the number of matrices to use to perform geometry blending.
        VertexBlend VertexBlend { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap0 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap1 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap10 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap11 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap12 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap13 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap14 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap15 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap2 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap3 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap4 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap5 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap6 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap7 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap8 { get; set; }
        //
        // Summary:
        //     Retrieves or sets the texture-wrapping behavior for multiple sets of texture
        //     coordinates.
        WrapCoordinates Wrap9 { get; set; }
        //
        // Summary:
        //     Enables or disables depth buffering.
        bool ZBufferEnable { get; set; }
        //
        // Summary:
        //     Retrieves or sets the comparison function for the z-buffer test.
        Compare ZBufferFunction { get; set; }
        //
        // Summary:
        //     Enables or disables writing to the depth buffer.
        bool ZBufferWriteEnable { get; set; }

        // Summary:
        //     Obtains a string representation of the current instance.
        //
        // Returns:
        //     String that represents the object.
        string ToString();
    }
}
