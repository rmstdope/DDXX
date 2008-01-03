using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IRenderState
    {
        // Summary:
        //     Gets or sets a value to enable alpha-blended transparency. The default value
        //     is false.
        //
        // Returns:
        //     true if alpha-blended transparency is enabled; false otherwise.
        bool AlphaBlendEnable { get; set; }
        //
        // Summary:
        //     Gets or sets the arithmetic operation applied to separate alpha blending.
        //     The default is BlendFunction.Add.
        //
        // Returns:
        //     A value from the BlendFunction enumeration.
        BlendFunction AlphaBlendOperation { get; set; }
        //
        // Summary:
        //     Gets or sets the separate alpha channel blending factor. This factor represents
        //     a destination value by which to multiply the alpha channel only. The default
        //     is Blend.One.
        //
        // Returns:
        //     A value from the Blend enumeration. BothSourceAlpha and BothInverseSourceAlpha
        //     are not supported on Xbox 360.
        Blend AlphaDestinationBlend { get; set; }
        //
        // Summary:
        //     Gets or sets the comparison function for the alpha test. The default is CompareFunction.Always.
        //
        // Returns:
        //     A member of the CompareFunction enumeration that represents the comparison
        //     function to set or get.
        CompareFunction AlphaFunction { get; set; }
        //
        // Summary:
        //     Gets or sets the separate alpha channel blending factor. This factor represents
        //     a value by which to multiply the alpha channel only. The default is Blend.One.
        //
        // Returns:
        //     A value from the Blend enumeration.
        Blend AlphaSourceBlend { get; set; }
        //
        // Summary:
        //     Gets or sets a render state that enables a per-pixel alpha test. The default
        //     value is false.
        //
        // Returns:
        //     true if per-pixel alpha blending is enabled; false otherwise.
        bool AlphaTestEnable { get; set; }
        //
        // Summary:
        //     Gets or sets the color used for a constant-blend factor during alpha blending.
        //     The default is Color.White.
        //
        // Returns:
        //     The color used for a constant-blend factor during alpha blending.
        Color BlendFactor { get; set; }
        //
        // Summary:
        //     Gets or sets a value to select the arithmetic operation to apply to the source
        //     and destination pixel components when RenderState.AlphaBlendEnable is set
        //     to true. The default is BlendFunction.Add.
        //
        // Returns:
        //     The blending operation to set or get.
        BlendFunction BlendFunction { get; set; }
        //
        // Summary:
        //     Gets or sets a value that enables a per-channel write for the render target
        //     color buffer. The default value is ColorWriteChannels.None.
        //
        // Returns:
        //     Value of the ColorWriteChannels enumeration that specifies the color channel
        //     to set or get.
        ColorWriteChannels ColorWriteChannels { get; set; }
        //
        // Summary:
        //     Gets or sets a value that enables a per-channel write for the render target
        //     color buffer. The default value is ColorWriteChannels.None.
        //
        // Returns:
        //     Value of the ColorWriteChannels enumeration that specifies the color channel
        //     to set or get.
        ColorWriteChannels ColorWriteChannels1 { get; set; }
        //
        // Summary:
        //     Gets or sets a value that enables a per-channel write for the render target
        //     color buffer. The default value is ColorWriteChannels.None.
        //
        // Returns:
        //     Value of the ColorWriteChannels enumeration that specifies the color channel
        //     to set or get.
        ColorWriteChannels ColorWriteChannels2 { get; set; }
        //
        // Summary:
        //     Gets or sets a value that enables a per-channel write for the render target
        //     color buffer. The default value is ColorWriteChannels.None.
        //
        // Returns:
        //     Value of the ColorWriteChannels enumeration that specifies the color channel
        //     to set or get.
        ColorWriteChannels ColorWriteChannels3 { get; set; }
        //
        // Summary:
        //     Gets or sets the stencil operation to perform if the stencil test passes
        //     and the depth-buffer test fails for a counterclockwise triangle. The default
        //     is StencilOperation.Keep.
        //
        // Returns:
        //     The stencil operation to perform.
        StencilOperation CounterClockwiseStencilDepthBufferFail { get; set; }
        //
        // Summary:
        //     Gets or sets the stencil operation to perform if the stencil test fails for
        //     a counterclockwise triangle. The default is StencilOperation.Keep.
        //
        // Returns:
        //     The stencil operation to perform.
        StencilOperation CounterClockwiseStencilFail { get; set; }
        //
        // Summary:
        //     Gets or sets the comparison function to use for counterclockwise stencil
        //     tests. The default is CompareFunction.Always.
        //
        // Returns:
        //     A CompareFunction value indicating which test to perform.
        CompareFunction CounterClockwiseStencilFunction { get; set; }
        //
        // Summary:
        //     Gets or sets the stencil operation to perform if the stencil and z-tests
        //     pass for a counterclockwise triangle. The default is StencilOperation.Keep.
        //
        // Returns:
        //     The stencil operation to perform.
        StencilOperation CounterClockwiseStencilPass { get; set; }
        //
        // Summary:
        //     Specifies how back-facing triangles are culled, if at all. The default value
        //     is CullMode.CounterClockwise.
        //
        // Returns:
        //     The culling mode to set or get.
        CullMode CullMode { get; set; }
        //
        // Summary:
        //     Sets or retrieves the depth bias for polygons. The default value is 0.
        //
        // Returns:
        //     Depth bias for polygons.
        float DepthBias { get; set; }
        //
        // Summary:
        //     Enables or disables depth buffering. The default is true.
        //
        // Returns:
        //     true if depth buffering is enabled; false otherwise.
        bool DepthBufferEnable { get; set; }
        //
        // Summary:
        //     Gets or sets the comparison function for the depth-buffer test. The default
        //     is CompareFunction.LessEqual
        //
        // Returns:
        //     Value of a CompareFunction that represents the comparison function to set
        //     or get.
        CompareFunction DepthBufferFunction { get; set; }
        //
        // Summary:
        //     Enables or disables writing to the depth buffer. The default is true.
        //
        // Returns:
        //     true if writing to the depth buffer is enabled; false otherwise.
        bool DepthBufferWriteEnable { get; set; }
        //
        // Summary:
        //     Gets or sets the color blending factor. This factor represents a value by
        //     which to multiply the destination pixel color before adding it to the source
        //     pixel to produce a color that is a blend of the two. The default is Blend.Zero.
        //
        // Returns:
        //     A Blend factor to set or get for the destination pixel.
        Blend DestinationBlend { get; set; }
        //
        // Summary:
        //     Represents the fill mode. The default is FillMode.Solid.
        //
        // Returns:
        //     Value of a FillMode that specifies the fill mode to set or get.
        FillMode FillMode { get; set; }
#if (!XBOX)
        //
        // Summary:
        //     [Windows Only] Gets or sets the fog color. The default value is Color.TransparentBlack.
        //
        // Returns:
        //     A color that specifies the fog color to set or get.
        Color FogColor { get; set; }
        //
        // Summary:
        //     [Windows Only] Gets or sets the fog density for pixel or vertex fog used
        //     in exponential fog modes. The default value is 1.0f.
        //
        // Returns:
        //     Value that represents the fog density to set or get.
        float FogDensity { get; set; }
        //
        // Summary:
        //     [Windows Only] Enables or disables fog blending. The default is false.
        //
        // Returns:
        //     true if fog blending is enabled; false otherwise.
        bool FogEnable { get; set; }
        //
        // Summary:
        //     [Windows Only] Gets or sets the depth at which pixel or vertex fog effects
        //     end for linear fog mode. The default value is 1.0f.
        //
        // Returns:
        //     Value that represents the ending depth to set or get.
        float FogEnd { get; set; }
        //
        // Summary:
        //     [Windows Only] Gets or sets the depth at which pixel or vertex fog effects
        //     begin for linear fog mode. The default value is 0.0f.
        //
        // Returns:
        //     Value that represents the beginning depth to set or get.
        float FogStart { get; set; }
        //
        // Summary:
        //     [Windows Only] Gets or sets the fog formula to use for pixel fog. The default
        //     is None.
        //
        // Returns:
        //     Value of a FogMode that specifies the fog mode to set or get.
        FogMode FogTableMode { get; set; }
        //
        // Summary:
        //     [Windows Only] Gets or sets the fog formula to use for vertex fog. The default
        //     is FogMode.None.
        //
        // Returns:
        //     Value of a FogMode that specifies the fog mode to set or get.
        FogMode FogVertexMode { get; set; }
#endif
        //
        // Summary:
        //     Enables or disables multisample antialiasing. The default is true.
        //
        // Returns:
        //     true to enable multisample antialiasing; false otherwise.
        bool MultiSampleAntiAlias { get; set; }
        //
        // Summary:
        //     Gets or sets a bitmask controlling modification of the samples in a multisample
        //     render target. The default is 0xffffffff.
        //
        // Returns:
        //     A bitmask value controlling write enables for the samples. Each bit in this
        //     mask, starting at the least-significant bit, controls modification of one
        //     of the samples in a multisample render target. Thus, for an 8-sample render
        //     target, the low byte contains the eight write enables for each of the eight
        //     samples. This render state has no effect when rendering to a single sample
        //     buffer.
        int MultiSampleMask { get; set; }
        //
        // Summary:
        //     Gets or sets the size to use for point size computation in cases where point
        //     size is not specified for each vertex. The default value is the value a driver
        //     returns. If a driver returns 0 or 1, the default value is 64, which allows
        //     software point size emulation.
        //
        // Returns:
        //     This value is in world space units.
        float PointSize { get; set; }
        //
        // Summary:
        //     Gets or sets the maximum size of point primitives. The default is 64.0f.
        //
        // Returns:
        //     The maximum size of point primitives. Must be less than or equal to Capabilities.MaxPointSize
        //     and greater than or equal to RenderState.PointSizeMin.
        float PointSizeMax { get; set; }
        //
        // Summary:
        //     Gets or sets the minimum size of point primitives. The default is 1.0f.
        //
        // Returns:
        //     The minimum size of point primitives.
        float PointSizeMin { get; set; }
        //
        // Summary:
        //     Enables or disables full texture mapping on each point. The default is false.
        //
        // Returns:
        //     true to set texture coordinates of point primitives so that full textures
        //     are mapped on each point; false otherwise. When false, the vertex texture
        //     coordinates are used for the entire point.
        bool PointSpriteEnable { get; set; }
#if (!XBOX)
        //
        // Summary:
        //     [Windows Only] Gets or sets enabling of range-based vertex fog. The default
        //     value is false.
        //
        // Returns:
        //     true if range-based vertex fog is enabled; false otherwise. If false, depth-based
        //     fog is used.
        bool RangeFogEnable { get; set; }
#endif
        //
        // Summary:
        //     Specifies a reference alpha value against which pixels are tested when alpha
        //     testing is enabled. The default value is 0.
        //
        // Returns:
        //     Integer that specifies the reference alpha value to set or get. This is an
        //     8-bit value placed in the low 8 bits of the DWORD render-state value. Values
        //     can range from 0x00000000 through 0x000000FF.
        int ReferenceAlpha { get; set; }
        //
        // Summary:
        //     Specifies a reference value to use for the stencil test. The default is 0.
        //
        // Returns:
        //     Integer that specifies the stencil test value to set or get.
        int ReferenceStencil { get; set; }
        //
        // Summary:
        //     Enables or disables scissor testing. The default is false.
        //
        // Returns:
        //     true to enable scissor testing; false otherwise.
        bool ScissorTestEnable { get; set; }
        //
        // Summary:
        //     Enables or disables the separate blend mode for the alpha channel. The default
        //     is false.
        //
        // Returns:
        //     true to enable the separate blend mode for the alpha channel; false otherwise.
        bool SeparateAlphaBlendEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets a value used to determine how much bias can be applied to coplanar
        //     primitives to reduce flimmering z-fighting. The default is 0.
        //
        // Returns:
        //     Value that specifies the slope scale bias to apply.
        float SlopeScaleDepthBias { get; set; }
        //
        // Summary:
        //     Gets or sets the color blending factor. This factor represents a value by
        //     which to multiply the source pixel color before adding it to the destination
        //     pixel to produce a color that is a blend of the two. The default is Blend.One.
        //
        // Returns:
        //     A Blend factor to set or get for the source pixel.
        Blend SourceBlend { get; set; }
        //
        // Summary:
        //     Gets or sets the stencil operation to perform if the stencil test passes
        //     and the depth-test fails. The default is StencilOperation.Keep.
        //
        // Returns:
        //     The stencil operation to perform.
        StencilOperation StencilDepthBufferFail { get; set; }
        //
        // Summary:
        //     Gets or sets stencil enabling. The default is false.
        //
        // Returns:
        //     true if stenciling is enabled; false otherwise.
        bool StencilEnable { get; set; }
        //
        // Summary:
        //     Gets or sets the stencil operation to perform if the stencil test fails.
        //     The default is StencilOperation.Keep.
        //
        // Returns:
        //     The stencil operation to perform.
        StencilOperation StencilFail { get; set; }
        //
        // Summary:
        //     Gets or sets the comparison function for the stencil test. The default is
        //     CompareFunction.Always.
        //
        // Returns:
        //     Value of a CompareFunction that represents the comparison function to set
        //     or get.
        CompareFunction StencilFunction { get; set; }
        //
        // Summary:
        //     Gets or sets the mask applied to the reference value and each stencil buffer
        //     entry to determine the significant bits for the stencil test. The default
        //     mask is Int32.MaxValue.
        //
        // Returns:
        //     Value that represents the mask to set or get.
        int StencilMask { get; set; }
        //
        // Summary:
        //     Gets or sets the stencil operation to perform if the stencil test passes.
        //     The default is StencilOperation.Keep.
        //
        // Returns:
        //     The stencil operation to perform.
        StencilOperation StencilPass { get; set; }
        //
        // Summary:
        //     Gets or sets the write mask applied to values written into the stencil buffer.
        //     The default mask is Int32.MaxValue.
        //
        // Returns:
        //     Value that represents the write mask to set or get.
        int StencilWriteMask { get; set; }
        //
        // Summary:
        //     Enables or disables two-sided stenciling. The default is false.
        //
        // Returns:
        //     true to enable two-sided stenciling; false otherwise.
        bool TwoSidedStencilMode { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap0 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap1 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap10 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap11 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap12 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap13 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap14 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap15 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap2 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap3 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap4 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap5 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap6 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap7 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap8 { get; set; }
        //
        // Summary:
        //     Gets or sets the texture-wrapping behavior for multiple sets of texture coordinates.
        //     The default value for this render state is TextureWrapCoordinates.Zero (wrapping
        //     disabled in all directions).
        //
        // Returns:
        //     Combination of values from TextureWrapCoordinates to set or get.
        TextureWrapCoordinates Wrap9 { get; set; }
    }
}
