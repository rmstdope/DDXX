using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Direct3D
{
    public interface IDevice
    {
        // Summary:
        //     Retrieves an estimate of the amount of available texture memory.
        int AvailableTextureMemory { get; }
        //
        // Summary:
        //     Retrieves the clipping planes on the current device.
        ClipPlanes ClipPlanes { get; }
        //
        // Summary:
        //     Retrieves or sets a Microsoft.DirectX.Direct3D.ClipStatus object.
        ClipStatus ClipStatus { get; set; }
        //
        // Summary:
        //     Retrieves the creation parameters of the device.
        DeviceCreationParameters CreationParameters { get; }
        //
        // Summary:
        //     Retrieves or sets the current texture palette.
        int CurrentTexturePalette { get; set; }
        //
        // Summary:
        //     Retrieves or sets the depth stencil surface owned by the Microsoft.DirectX.Direct3D.Device
        //     object.
        ISurface DepthStencilSurface { get; set; }
        //
        // Summary:
        //     Retrieves the capabilities of the rendering device.
        Caps DeviceCaps { get; }
        //
        // Summary:
        //     Retrieves the display mode's spatial resolution, color resolution, and refresh
        //     frequency.
        DisplayMode DisplayMode { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        bool Disposed { get; }
        //
        // Summary:
        //     Retrieves or sets index data.
        IndexBuffer Indices { get; set; }
        //
        // Summary:
        //     Retrieves or sets a value that indicates whether the device should use event
        //     handlers.
        //static bool IsUsingEventHandlers { get; set; }
        //
        // Summary:
        //     Retrieves the Microsoft.DirectX.Direct3D.LightsCollection collection on the
        //     current device.
        LightsCollection Lights { get; }
        //
        // Summary:
        //     Retrieves or sets the current material properties for the device.
        Material Material { get; set; }
        //
        // Summary:
        //     Retrieves or sets the N-patch mode segments.
        float NPatchMode { get; set; }
        //
        // Summary:
        //     Retrieves the number of implicit swap chains.
        int NumberOfSwapChains { get; }
        //
        // Summary:
        //     Retrieves or sets the current pixel shader.
        PixelShader PixelShader { get; set; }
        //
        // Summary:
        //     Retrieves presentation parameters for a device.
        PresentParameters PresentationParameters { get; }
        //
        // Summary:
        //     Retrieves information that describes the raster of the monitor on which the
        //     swap chain is presented.
        RasterStatus RasterStatus { get; }
        //
        // Summary:
        //     Retrieves a render-state value for a device.
        RenderStateManager RenderState { get; }
        //
        // Summary:
        //     Retrieves a device's sampler states.
        SamplerStateManagerCollection SamplerState { get; }
        //
        // Summary:
        //     Retrieves or sets the scissor rectangle.
        Rectangle ScissorRectangle { get; set; }
        //
        // Summary:
        //     Retrieves or sets the vertex processing mode.
        bool SoftwareVertexProcessing { get; set; }
        //
        // Summary:
        //     Retrieves a state value for an assigned texture.
        TextureStateManagerCollection TextureState { get; }
        //
        // Summary:
        //     Retrieves a matrix that describes a transformation state.
        Transforms Transform { get; }
        //
        // Summary:
        //     Retrieves or sets a vertex shader declaration.
        VertexDeclaration VertexDeclaration { get; set; }
        //
        // Summary:
        //     Retrieves or sets the supported flexible vertex formats.
        VertexFormats VertexFormat { get; set; }
        //
        // Summary:
        //     Retrieves or sets the current vertex shader.
        VertexShader VertexShader { get; set; }
        //
        // Summary:
        //     Retrieves or sets the viewport parameters for the current device.
        Viewport Viewport { get; set; }
        // Summary:
        //     Begins a scene.
        void BeginScene();
        //
        // Summary:
        //     Signals Microsoft Direct3D to begin recording a device state block.
        void BeginStateBlock();
        //
        // Summary:
        //     Reports the current cooperative-level status of the Microsoft Direct3D device
        //     for a windowed or full-screen application.
        //
        // Returns:
        //     Current cooperative-level status of the device for a windowed or full-screen
        //     application. A return value of true indicates that the device is operational
        //     and that the calling application can continue; a value of false indicates
        //     that the device is lost or needs to be reset.
        bool CheckCooperativeLevel();
        //
        // Summary:
        //     Reports the current cooperative-level status of the Microsoft Direct3D device
        //     for a windowed or full-screen application.
        //
        // Parameters:
        //   result:
        //     [in, out] Current cooperative-level status of the device for a windowed or
        //     full-screen application, reported using a Microsoft.DirectX.Direct3D.ResultCode
        //     value. A Microsoft.DirectX.Direct3D.ResultCode.Success result indicates that
        //     the device is operational and that the calling application can continue.
        //     A Microsoft.DirectX.Direct3D.ResultCode.DeviceLost result indicates that
        //     the device is lost but cannot be reset at this time; therefore, rendering
        //     is not possible. A Microsoft.DirectX.Direct3D.ResultCode.DeviceNotReset result
        //     indicates that the device is lost but can be reset at this time.
        //
        // Returns:
        //     Current cooperative-level status of the device for a windowed or full-screen
        //     application. A return value of true indicates that the device is operational
        //     and that the calling application can continue; a value of false indicates
        //     that the device is lost or needs to be reset.
        bool CheckCooperativeLevel(out int result);
        //
        // Summary:
        //     Clears the viewport or a set of rectangles in the viewport to a specified
        //     RGBA color, clears the depth buffer, and erases the stencil buffer.
        //
        // Parameters:
        //   flags:
        //     Flags that indicate which surfaces to clear. This parameter can be any combination
        //     of the following flags, but at least one flag must be used. Microsoft.DirectX.Direct3D.ClearFlags.Stencil:
        //     Clears the stencil buffer to the value in the Microsoft.DirectX.Direct3D.Device.Clear()
        //     parameter.Microsoft.DirectX.Direct3D.ClearFlags.Target: Clears the render
        //     target to the color in the Microsoft.DirectX.Direct3D.Device.Clear() parameter.Microsoft.DirectX.Direct3D.ClearFlags.ZBuffer:
        //     Clears the depth buffer to the value in the Z parameter.
        //
        //   color:
        //     A System.Drawing.Color object that represents the color to which the render
        //     target surface is cleared.
        //
        //   zdepth:
        //     New z value that this method stores in the depth buffer. This parameter can
        //     be in the range of 0.0 through 1.0 (for z-based or w-based depth buffers).
        //     A value of 0.0 represents the nearest distance to the viewer; a value of
        //     1.0 represents the farthest distance.
        //
        //   stencil:
        //     Integer value to store in each stencil-buffer entry. This parameter can be
        //     in the range of 0 through 2n-1, where n is the bit depth of the stencil buffer.
        void Clear(ClearFlags flags, Color color, float zdepth, int stencil);
        //
        // Summary:
        //     Clears the viewport or a set of rectangles in the viewport to a specified
        //     RGBA color, clears the depth buffer, and erases the stencil buffer.
        //
        // Parameters:
        //   flags:
        //     Flags that indicate which surfaces to clear. This parameter can be any combination
        //     of the following flags, but at least one flag must be used. Microsoft.DirectX.Direct3D.ClearFlags.Stencil:
        //     Clears the stencil buffer to the value in the Microsoft.DirectX.Direct3D.Device.Clear()
        //     parameter.Microsoft.DirectX.Direct3D.ClearFlags.Target: Clears the render
        //     target to the color in the Microsoft.DirectX.Direct3D.Device.Clear() parameter.Microsoft.DirectX.Direct3D.ClearFlags.ZBuffer:
        //     Clears the depth buffer to the value in the Z parameter.
        //
        //   color:
        //     A 32-bit ARGB color value to which the render target surface is cleared.
        //
        //   zdepth:
        //     New z value that this method stores in the depth buffer. This parameter can
        //     be in the range of 0.0 through 1.0 (for z-based or w-based depth buffers).
        //     A value of 0.0 represents the nearest distance to the viewer; a value of
        //     1.0 represents the farthest distance.
        //
        //   stencil:
        //     Integer value to store in each stencil-buffer entry. This parameter can be
        //     in the range of 0 through 2n-1, where n is the bit depth of the stencil buffer.
        void Clear(ClearFlags flags, int color, float zdepth, int stencil);
        //
        // Summary:
        //     Clears the viewport or a set of rectangles in the viewport to a specified
        //     RGBA color, clears the depth buffer, and erases the stencil buffer.
        //
        // Parameters:
        //   flags:
        //     Flags that indicate which surfaces to clear. This parameter can be any combination
        //     of the following flags, but at least one flag must be used. Microsoft.DirectX.Direct3D.ClearFlags.Stencil:
        //     Clears the stencil buffer to the value in the Microsoft.DirectX.Direct3D.Device.Clear()
        //     parameter.Microsoft.DirectX.Direct3D.ClearFlags.Target: Clears the render
        //     target to the color in the Microsoft.DirectX.Direct3D.Device.Clear() parameter.Microsoft.DirectX.Direct3D.ClearFlags.ZBuffer:
        //     Clears the depth buffer to the value in the Z parameter.
        //
        //   color:
        //     A System.Drawing.Color object that represents the color to which the render
        //     target surface is cleared.
        //
        //   zdepth:
        //     New z value that this method stores in the depth buffer. This parameter can
        //     be in the range of 0.0 through 1.0 (for z-based or w-based depth buffers).
        //     A value of 0.0 represents the nearest distance to the viewer; a value of
        //     1.0 represents the farthest distance.
        //
        //   stencil:
        //     Integer value to store in each stencil-buffer entry. This parameter can be
        //     in the range of 0 through 2n-1, where n is the bit depth of the stencil buffer.
        //
        //   rect:
        //     Array of System.Drawing.Rectangle structures that describe the rectangles
        //     to clear. To clear the entire surface, set a rectangle to the dimensions
        //     of the rendering target. Each rectangle uses screen coordinates that correspond
        //     to points on the render target surface. Coordinates are clipped to the bounds
        //     of the viewport rectangle.
        void Clear(ClearFlags flags, Color color, float zdepth, int stencil, Rectangle[] rect);
        //
        // Summary:
        //     Clears the viewport or a set of rectangles in the viewport to a specified
        //     RGBA color, clears the depth buffer, and erases the stencil buffer.
        //
        // Parameters:
        //   flags:
        //     Flags that indicate which surfaces to clear. This parameter can be any combination
        //     of the following flags, but at least one flag must be used. Microsoft.DirectX.Direct3D.ClearFlags.Stencil:
        //     Clears the stencil buffer to the value in the Microsoft.DirectX.Direct3D.Device.Clear()
        //     parameter.Microsoft.DirectX.Direct3D.ClearFlags.Target: Clears the render
        //     target to the color in the Microsoft.DirectX.Direct3D.Device.Clear() parameter.Microsoft.DirectX.Direct3D.ClearFlags.ZBuffer:
        //     Clears the depth buffer to the value in the Z parameter.
        //
        //   color:
        //     A 32-bit ARGB color value to which the render target surface is cleared.
        //
        //   zdepth:
        //     New z value that this method stores in the depth buffer. This parameter can
        //     be in the range of 0.0 through 1.0 (for z-based or w-based depth buffers).
        //     A value of 0.0 represents the nearest distance to the viewer; a value of
        //     1.0 represents the farthest distance.
        //
        //   stencil:
        //     Integer value to store in each stencil-buffer entry. This parameter can be
        //     in the range of 0 through 2n-1, where n is the bit depth of the stencil buffer.
        //
        //   regions:
        //     Array of System.Drawing.Rectangle structures that describe the rectangles
        //     to clear. To clear the entire surface, set a rectangle to the dimensions
        //     of the rendering target. Each rectangle uses screen coordinates that correspond
        //     to points on the render target surface. Coordinates are clipped to the bounds
        //     of the viewport rectangle.
        void Clear(ClearFlags flags, int color, float zdepth, int stencil, Rectangle[] regions);
        //
        // Summary:
        //     Allows an application to fill a rectangular area of a Microsoft.DirectX.Direct3D.Pool.Default
        //     surface with a specified color.
        //
        // Parameters:
        //   surface:
        //     Surface to be filled.
        //
        //   rect:
        //     Source rectangle. To fill the entire surface, specify null.
        //
        //   color:
        //     Color used for filling.
        void ColorFill(ISurface surface, Rectangle rect, Color color);
        //
        // Summary:
        //     Allows an application to fill a rectangular area of a Microsoft.DirectX.Direct3D.Pool.Default
        //     surface with a specified color.
        //
        // Parameters:
        //   surface:
        //     Surface to be filled.
        //
        //   rect:
        //     Source rectangle. To fill the entire surface, specify null.
        //
        //   color:
        //     Color used for filling.
        void ColorFill(ISurface surface, Rectangle rect, int color);
        //
        // Summary:
        //     Creates a depth stencil resource.
        //
        // Parameters:
        //   width:
        //     Width of the depth stencil surface in pixels.
        //
        //   height:
        //     Height of the depth stencil surface in pixels.
        //
        //   format:
        //     Member of the Microsoft.DirectX.Direct3D.DepthFormat enumerated type that
        //     describes the format of the depth stencil surface. This value must be one
        //     of the enumerated depth stencil formats for the current device.
        //
        //   multiSample:
        //     Member of the Microsoft.DirectX.Direct3D.MultiSampleType enumerated type
        //     that describes the multisampling buffer type. This value must be one of the
        //     supported multisample types. When this surface is passed to the Microsoft.DirectX.Direct3D.Device.DepthStencilSurface
        //     property, its multisample type must be the same as that of the render target
        //     set by Microsoft.DirectX.Direct3D.Device.SetRenderTarget(System.Int32,Microsoft.DirectX.Direct3D.Surface).
        //
        //   multiSampleQuality:
        //     Quality level. The valid range is between 0 and one less than the level returned
        //     by the Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType() parameter
        //     of the Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType() method.
        //     Passing a larger value results in a Microsoft.DirectX.Direct3D.GraphicsException
        //     with an Microsoft.DirectX.Direct3D.ResultCode.InvalidCall value. The Microsoft.DirectX.Direct3D.Device.CreateDepthStencilSurface(System.Int32,System.Int32,Microsoft.DirectX.Direct3D.DepthFormat,Microsoft.DirectX.Direct3D.MultiSampleType,System.Int32,System.Boolean)
        //     values of paired render targets, depth stencil surfaces, and the Microsoft.DirectX.Direct3D.Device.CreateDepthStencilSurface(System.Int32,System.Int32,Microsoft.DirectX.Direct3D.DepthFormat,Microsoft.DirectX.Direct3D.MultiSampleType,System.Int32,System.Boolean)
        //     type must all match.
        //
        //   discard:
        //     Set to true to enable z-buffer discarding; otherwise false. If this flag
        //     is set, the contents of the depth stencil buffer are invalid after Microsoft.DirectX.Direct3D.Device.Present()
        //     is called or when Microsoft.DirectX.Direct3D.Device.DepthStencilSurface is
        //     set with a different depth surface. This flag's behavior is the same as passing
        //     the enumerated value Microsoft.DirectX.Direct3D.PresentFlag.DiscardDepthStencil
        //     to the Microsoft.DirectX.Direct3D.Device.Microsoft.DirectX.Direct3D.Device.Present()
        //     method.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Surface that represents the created depth stencil
        //     surface resource.
        ISurface CreateDepthStencilSurface(int width, int height, DepthFormat format, MultiSampleType multiSample, int multiSampleQuality, bool discard);
        //
        // Summary:
        //     Creates an off-screen surface.
        //
        // Parameters:
        //   width:
        //     Width of the surface in pixels.
        //
        //   height:
        //     Height of the surface in pixels.
        //
        //   format:
        //     Format of the surface. For more information, see Microsoft.DirectX.Direct3D.Format.
        //
        //   pool:
        //     Surface pool type. For more information, see Microsoft.DirectX.Direct3D.Pool.
        //
        // Returns:
        //     The Microsoft.DirectX.Direct3D.Surface object that is created.
        ISurface CreateOffscreenPlainSurface(int width, int height, Format format, Pool pool);
        //
        // Summary:
        //     Creates a render target surface.
        //
        // Parameters:
        //   width:
        //     Width of the render-target surface in pixels.
        //
        //   height:
        //     Height of the render-target surface in pixels.
        //
        //   format:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumerated type that describes
        //     the format of the render target.
        //
        //   multiSample:
        //     Member of the Microsoft.DirectX.Direct3D.MultiSampleType enumerated type
        //     that describes the multisampling buffer type. This parameter specifies the
        //     antialiasing type for the render target. When this surface is passed to Microsoft.DirectX.Direct3D.Device.SetRenderTarget(System.Int32,Microsoft.DirectX.Direct3D.Surface),
        //     its multisample type must be the same as that of the depth stencil property
        //     Microsoft.DirectX.Direct3D.Device.DepthStencilSurface.
        //
        //   multiSampleQuality:
        //     Quality level. The valid range is between 0 and one less than the level returned
        //     by the Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType() parameter
        //     of the Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType(). Passing
        //     a larger value causes an Microsoft.DirectX.Direct3D.InvalidCallException.
        //     The Microsoft.DirectX.Direct3D.Device.CreateRenderTarget(System.Int32,System.Int32,Microsoft.DirectX.Direct3D.Format,Microsoft.DirectX.Direct3D.MultiSampleType,System.Int32,System.Boolean)
        //     values of paired render targets, depth stencil surfaces, and the multisample
        //     type must all match.
        //
        //   lockable:
        //     Set to true if render targets are lockable; otherwise, false. Note that lockable
        //     render targets reduce performance on some graphics hardware.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Surface.
        ISurface CreateRenderTarget(int width, int height, Format format, MultiSampleType multiSample, int multiSampleQuality, bool lockable);
        //
        // Summary:
        //     Frees a cached high-order patch.
        //
        // Parameters:
        //   handle:
        //     Handle of the cached high-order patch to delete.
        void DeletePatch(int handle);
        //
        // Summary:
        //     Immediately releases the unmanaged resources used by the Microsoft.DirectX.Direct3D.Device
        //     object.
        void Dispose();
        //
        // Summary:
        //     Renders the specified geometric primitive, based on indexing into an array
        //     of vertices.
        //
        // Parameters:
        //   primitiveType:
        //     Member of the Microsoft.DirectX.Direct3D.PrimitiveType enumerated type that
        //     describes the type of primitive to render. The Microsoft.DirectX.Direct3D.PrimitiveType.PointList
        //     constant is not supported with this method. See Remarks.
        //
        //   baseVertex:
        //     Offset from the start of the index buffer to the first vertex index.
        //
        //   minVertexIndex:
        //     Minimum vertex index for vertices used during the call.
        //
        //   numVertices:
        //     Number of vertices used during the call, starting from Microsoft.DirectX.Direct3D.Device.DrawIndexedPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)
        //     + Microsoft.DirectX.Direct3D.Device.DrawIndexedPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32).
        //
        //   startIndex:
        //     Location in the index array at which to start reading vertices.
        //
        //   primCount:
        //     Number of primitives to render. The number of vertices used is a function
        //     of the Microsoft.DirectX.Direct3D.Device.DrawIndexedPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)
        //     and Microsoft.DirectX.Direct3D.Device.DrawIndexedPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32).
        //     To determine the maximum number of primitives allowed, check the Microsoft.DirectX.Direct3D.Caps.MaxPrimitiveCount
        //     member of the Microsoft.DirectX.Direct3D.Caps structure.
        void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primCount);
        //
        // Summary:
        //     Renders the specified geometric primitive with data specified by a user memory
        //     pointer.
        //
        // Parameters:
        //   primitiveType:
        //     Member of the Microsoft.DirectX.Direct3D.PrimitiveType enumerated type that
        //     describes the type of primitive to render.
        //
        //   minVertexIndex:
        //     Minimum vertex index, relative to 0 (the start of Microsoft.DirectX.Direct3D.Device.DrawIndexedUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType,System.Int32,System.Int32,System.Int32,System.Object,System.Boolean,System.Object)),
        //     for vertices used during the call.
        //
        //   numVertexIndices:
        //     Number of vertices used during the call, starting from Microsoft.DirectX.Direct3D.Device.DrawIndexedUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType,System.Int32,System.Int32,System.Int32,System.Object,System.Boolean,System.Object).
        //
        //   primitiveCount:
        //     Number of primitives to render. The number of indices used is a function
        //     of the primitive count and primitive type. To determine the maximum number
        //     of primitives allowed, check the Microsoft.DirectX.Direct3D.Caps.MaxPrimitiveCount
        //     member of the Microsoft.DirectX.Direct3D.Caps structure.
        //
        //   indexData:
        //     User memory pointer to the index data.
        //
        //   sixteenBitIndices:
        //     Set to true to indicate 16-bit indices. Set to false if to indicate 32-bit
        //     indices.
        //
        //   vertexStreamZeroData:
        //     User memory pointer to the vertex data to use for vertex stream 0.
        void DrawIndexedUserPrimitives(PrimitiveType primitiveType, int minVertexIndex, int numVertexIndices, int primitiveCount, object indexData, bool sixteenBitIndices, object vertexStreamZeroData);
        //
        // Summary:
        //     Renders a sequence of non-indexed geometric primitives of the specified type
        //     from the current set of data input streams.
        //
        // Parameters:
        //   primitiveType:
        //     Member of the Microsoft.DirectX.Direct3D.PrimitiveType enumerated type that
        //     describes the type of primitive to render.
        //
        //   startVertex:
        //     Index of the first vertex to load. Beginning at Microsoft.DirectX.Direct3D.Device.DrawPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType,System.Int32,System.Int32),
        //     the correct number of vertices is read out of the vertex buffer.
        //
        //   primitiveCount:
        //     Number of primitives to render. To determine the maximum number of primitives
        //     allowed, check Microsoft.DirectX.Direct3D.Caps.Microsoft.DirectX.Direct3D.Caps.MaxPrimitiveCount.
        //     The Microsoft.DirectX.Direct3D.Device.DrawPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType,System.Int32,System.Int32)
        //     is the number of primitives as determined by the primitive type. If it is
        //     a line list, each primitive has two vertices. If it is a triangle list, each
        //     primitive has three vertices.
        void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount);
        //
        // Summary:
        //     Draws a rectangular patch using the currently set streams.
        //
        // Parameters:
        //   handle:
        //     Handle to the rectangular patch to draw.
        //
        //   numSegs:
        //     Array of four floating-point values that identify the number of segments
        //     into which each edge of the rectangle patch should be divided when tessellated.
        //     For more information, see Microsoft.DirectX.Direct3D.RectanglePatchInformation.
        void DrawRectanglePatch(int handle, float[] numSegs);
        //
        // Summary:
        //     Draws a rectangular patch using the currently set streams.
        //
        // Parameters:
        //   handle:
        //     Handle to the rectangular patch to draw.
        //
        //   numSegs:
        //     A Microsoft.DirectX.Plane structure. The Microsoft.DirectX.Plane.A, Microsoft.DirectX.Plane.B,
        //     Microsoft.DirectX.Plane.C and Microsoft.DirectX.Plane.D fields identify the
        //     number of segments each edge of the rectangle patch should be divided into
        //     when tessellated.
        void DrawRectanglePatch(int handle, Plane numSegs);
        //
        // Summary:
        //     Draws a rectangular patch using the currently set streams.
        //
        // Parameters:
        //   handle:
        //     Handle to the rectangular patch to draw.
        //
        //   numSegs:
        //     Array of four floating-point values that identify the number of segments
        //     into which each edge of the rectangle patch should be divided when tessellated.
        //     For more information, see Microsoft.DirectX.Direct3D.RectanglePatchInformation.
        //
        //   rectPatchInformation:
        //     A pointer to a Microsoft.DirectX.Direct3D.RectanglePatchInformation structure
        //     that describes the rectangular patch to draw. Use this if your code block
        //     is marked unsafe.
        void DrawRectanglePatch(int handle, float[] numSegs, RectanglePatchInformation rectPatchInformation);
        //
        // Summary:
        //     Draws a rectangular patch using the currently set streams.
        //
        // Parameters:
        //   handle:
        //     Handle to the rectangular patch to draw.
        //
        //   numSegs:
        //     A Microsoft.DirectX.Plane structure. The Microsoft.DirectX.Plane.A, Microsoft.DirectX.Plane.B,
        //     Microsoft.DirectX.Plane.C and Microsoft.DirectX.Plane.D fields identify the
        //     number of segments each edge of the rectangle patch should be divided into
        //     when tessellated.
        //
        //   rectPatchInformation:
        //     A pointer to a Microsoft.DirectX.Direct3D.RectanglePatchInformation structure
        //     that describes the rectangular patch to draw. Use this if your code block
        //     is marked unsafe.
        void DrawRectanglePatch(int handle, Plane numSegs, RectanglePatchInformation rectPatchInformation);
        //
        // Summary:
        //     Draws a triangular patch using the currently set streams.
        //
        // Parameters:
        //   handle:
        //     Handle to the triangular patch to draw.
        //
        //   numSegs:
        //     Pointer to an array of three floating-point values that identify the number
        //     of segments each edge of the triangle patch should be divided into when tessellated.
        //     For more information, see Microsoft.DirectX.Direct3D.TrianglePatchInformation.
        void DrawTrianglePatch(int handle, float[] numSegs);
        //
        // Summary:
        //     Draws a triangular patch using the currently set streams.
        //
        // Parameters:
        //   handle:
        //     Handle to the triangular patch to draw.
        //
        //   numSegs:
        //     A Microsoft.DirectX.Plane structure. The Microsoft.DirectX.Plane.A, Microsoft.DirectX.Plane.B,
        //     Microsoft.DirectX.Plane.C, and Microsoft.DirectX.Plane.D fields identify
        //     the number of segments into which each edge of the triangle patch should
        //     be divided when tessellated.
        void DrawTrianglePatch(int handle, Plane numSegs);
        //
        // Summary:
        //     Draws a triangular patch using the currently set streams.
        //
        // Parameters:
        //   handle:
        //     Handle to the triangular patch to draw.
        //
        //   numSegs:
        //     Pointer to an array of three floating-point values that identify the number
        //     of segments each edge of the triangle patch should be divided into when tessellated.
        //     For more information, see Microsoft.DirectX.Direct3D.TrianglePatchInformation.
        //
        //   triPatchInformation:
        //     A pointer to a Microsoft.DirectX.Direct3D.TrianglePatchInformation structure
        //     that describes the triangular high-order patch to draw. Use this if your
        //     code block is marked unsafe.
        void DrawTrianglePatch(int handle, float[] numSegs, TrianglePatchInformation triPatchInformation);
        //
        // Summary:
        //     Draws a triangular patch using the currently set streams.
        //
        // Parameters:
        //   handle:
        //     Handle to the triangular patch to draw.
        //
        //   numSegs:
        //     A Microsoft.DirectX.Plane structure. The Microsoft.DirectX.Plane.A, Microsoft.DirectX.Plane.B,
        //     Microsoft.DirectX.Plane.C, and Microsoft.DirectX.Plane.D fields identify
        //     the number of segments into which each edge of the triangle patch should
        //     be divided when tessellated.
        //
        //   triPatchInformation:
        //     A pointer to a Microsoft.DirectX.Direct3D.TrianglePatchInformation structure
        //     that describes the triangular high-order patch to draw. Use this if your
        //     code block is marked unsafe.
        void DrawTrianglePatch(int handle, Plane numSegs, TrianglePatchInformation triPatchInformation);
        //
        // Summary:
        //     Renders data specified by a user memory pointer as a sequence of geometric
        //     primitives of the specified type.
        //
        // Parameters:
        //   primitiveType:
        //     Member of the Microsoft.DirectX.Direct3D.PrimitiveType enumerated type that
        //     describes the type of primitive to render.
        //
        //   primitiveCount:
        //     Number of primitives to render. To determine the maximum number of primitives
        //     allowed, check the Microsoft.DirectX.Direct3D.Caps.Microsoft.DirectX.Direct3D.Caps.MaxPrimitiveCount
        //     member.
        //
        //   vertexStreamZeroData:
        //     User memory vertex data to use for vertex stream 0.
        void DrawUserPrimitives(PrimitiveType primitiveType, int primitiveCount, object vertexStreamZeroData);
        //
        // Summary:
        //     Ends a scene that was started by calling the Microsoft.DirectX.Direct3D.Device.BeginScene()
        //     method.
        void EndScene();
        //
        // Summary:
        //     Signals Microsoft Direct3D to stop recording a device state block and retrieve
        //     a pointer to the state block interface.
        //
        // Returns:
        //     State block object. For more information, see Microsoft.DirectX.Direct3D.StateBlock.
        StateBlock EndStateBlock();
        //
        // Summary:
        //     Returns a value that indicates whether the current instance is equal to a
        //     specified object.
        //
        // Parameters:
        //   compare:
        //     Object to compare to this object.
        //
        // Returns:
        //     Returns true if the objects are the same; otherwise, false.
        bool Equals(object compare);
        //
        // Summary:
        //     Evicts all managed resources, including Microsoft Direct3D resources and
        //     those that are driver-managed.
        void EvictManagedResources();
        //
        // Summary:
        //     Retrieves a back buffer from a device's swap chain.
        //
        // Parameters:
        //   swapChain:
        //     Unsigned integer that specifies the swap chain.
        //
        //   backBuffer:
        //     Index of the back buffer object to return.
        //
        //   backBufferType:
        //     Stereo view is not supported in Microsoft DirectX 9.0, so the only valid
        //     value for this parameter is Microsoft.DirectX.Direct3D.BackBufferType.Mono.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Surface that represents the returned back buffer
        //     surface.
        ISurface GetBackBuffer(int swapChain, int backBuffer, BackBufferType backBufferType);
        //
        // Summary:
        //     Retrieves the cube texture assigned to a stage for a device.
        //
        // Parameters:
        //   stage:
        //     Stage identifier of the Microsoft.DirectX.Direct3D.CubeTexture to retrieve.
        //     Stage identifiers are zero-based.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.CubeTexture object that represents the returned
        //     Microsoft.DirectX.Direct3D.CubeTexture.
        CubeTexture GetCubeTexture(int stage);
        //
        // Summary:
        //     Generates a copy of a device's front buffer and places it in a system memory
        //     buffer provided by the application.
        //
        // Parameters:
        //   swapChain:
        //     Unsigned integer that specifies the swap chain.
        //
        //   buffer:
        //     [in, out] A Microsoft.DirectX.Direct3D.Surface class that receives a copy
        //     of the front buffer's contents. The data is returned in successive rows with
        //     no intervening space, proceeding from the highest vertical row on the device's
        //     output to the lowest. For windowed mode, the size of the destination surface
        //     should be the desktop size. For full-screen mode, the size of the destination
        //     surface should be the screen size.
        void GetFrontBufferData(int swapChain, ISurface buffer);
        //
        // Summary:
        //     Retrieves the gamma correction ramp for the swap chain.
        //
        // Parameters:
        //   swapChain:
        //     Unsigned integer that specifies the swap chain.
        //
        // Returns:
        //     [in, out] Application-supplied Microsoft.DirectX.Direct3D.GammaRamp structure
        //     to fill with the gamma correction ramp.
        GammaRamp GetGammaRamp(int swapChain);
        //
        // Summary:
        //     Returns the hash code for the current instance.
        //
        // Returns:
        //     Hash code for the instance.
        int GetHashCode();
        //
        // Summary:
        //     Retrieves palette entries.
        //
        // Parameters:
        //   paletteNumber:
        //     Ordinal value that identifies the palette to retrieve.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.PaletteEntry structure that represents the returned
        //     palette entries.
        PaletteEntry[] GetPaletteEntries(int paletteNumber);
        //
        // Summary:
        //     Retrieves a Boolean shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantCount:
        //     Number of Boolean values in the array of constants.
        //
        // Returns:
        //     Array of constants.
        bool[] GetPixelShaderBooleanConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Retrieves an integer shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantCount:
        //     Number of four-integer vectors in the array of constants.
        //
        // Returns:
        //     Array of constants.
        int[] GetPixelShaderInt32Constant(int startRegister, int constantCount);
        //
        // Summary:
        //     Retrieves a floating-point shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantCount:
        //     Number of four-float vectors in the array of constants.
        //
        // Returns:
        //     Array of constants.
        float[] GetPixelShaderSingleConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Returns information that describes the raster of the monitor on which the
        //     swap chain is presented.
        //
        // Parameters:
        //   swapChain:
        //     Unsigned integer that specifies the swap chain.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.RasterStatus structure that contains information
        //     about the position or other status of the raster on the monitor driven by
        //     this adapter.
        RasterStatus GetRasterStatus(int swapChain);
        //
        // Summary:
        //     Retrieves the Boolean value of a given render state.
        //
        // Parameters:
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.RenderStates enumeration that
        //     represents the render state value to retreive.
        //
        // Returns:
        //     The render state value retrieved.
        bool GetRenderStateBoolean(RenderStates state);
        //
        // Summary:
        //     Retrieves the integer value of a given render state.
        //
        // Parameters:
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.RenderStates enumeration that
        //     represents the render state value to retreive.
        //
        // Returns:
        //     The render state value retrieved.
        int GetRenderStateInt32(RenderStates state);
        //
        // Summary:
        //     Retrieves the floating-point value of a given render state.
        //
        // Parameters:
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.RenderStates enumeration that
        //     represents the render state value to retreive.
        //
        // Returns:
        //     The render state value retrieved.
        float GetRenderStateSingle(RenderStates state);
        //
        // Summary:
        //     Retrieves a render target surface.
        //
        // Parameters:
        //   renderTargetIndex:
        //     Index of the render target. See Remarks.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Surface that represents the returned render
        //     target surface for the current device.
        ISurface GetRenderTarget(int renderTargetIndex);
        //
        // Summary:
        //     Copies the render target data from device memory to system memory.
        //
        // Parameters:
        //   renderTarget:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents a render target.
        //
        //   destSurface:
        //     [in, out] A Microsoft.DirectX.Direct3D.Surface object that represents a destination
        //     surface.
        void GetRenderTargetData(ISurface renderTarget, ISurface destSurface);
        //
        // Summary:
        //     Retrieves the Boolean value of a given sampler stage state.
        //
        // Parameters:
        //   stage:
        //     Index value of the sampler stage to retrieve.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.SamplerStageStates enumeration
        //     that represents the sampler stage state value to retreive.
        //
        // Returns:
        //     The sampler stage state value retrieved.
        bool GetSamplerStageStateBoolean(int stage, SamplerStageStates state);
        //
        // Summary:
        //     Retrieves the integer value of a given sampler stage state.
        //
        // Parameters:
        //   stage:
        //     Index value of the sampler stage to retrieve.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.SamplerStageStates enumeration
        //     that represents the sampler stage state value to retreive.
        //
        // Returns:
        //     The sampler stage state value retrieved.
        int GetSamplerStageStateInt32(int stage, SamplerStageStates state);
        //
        // Summary:
        //     Retrieves the floating-point value of a given sampler stage state.
        //
        // Parameters:
        //   stage:
        //     Index value of the sampler stage to retrieve.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.SamplerStageStates enumeration
        //     that represents the sampler stage state value to retreive.
        //
        // Returns:
        //     The sampler stage state value retrieved.
        float GetSamplerStageStateSingle(int stage, SamplerStageStates state);
        //
        // Summary:
        //     Retrieves a vertex buffer bound to the specified data stream.
        //
        // Parameters:
        //   streamNumber:
        //     Number of the data stream, in the range of 0 to the maximum number of streams
        //     -1.
        //
        //   offsetInBytes:
        //     Offset from the beginning of the stream to the beginning of the vertex data,
        //     in bytes. See Remarks.
        //
        //   stride:
        //     Pointer to a returned stride of the component, in bytes. See Remarks.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.VertexBuffer class that represents the returned
        //     vertex buffer bound to the specified data stream.
        VertexBuffer GetStreamSource(int streamNumber, out int offsetInBytes, out int stride);
        //
        // Summary:
        //     Retrieves the stream source frequency divider value.
        //
        // Parameters:
        //   streamNumber:
        //     Stream source number.
        //
        // Returns:
        //     Frequency divider value.
        int GetStreamSourceFrequency(int streamNumber);
        //
        // Summary:
        //     Retrieves a reference to a swap chain.
        //
        // Parameters:
        //   swapChain:
        //     Swap chain ordinal value. For more information, see Microsoft.DirectX.Direct3D.Caps.Microsoft.DirectX.Direct3D.Caps.NumberOfAdaptersInGroup.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.SwapChain class that receives a copy of the
        //     swap chain.
        SwapChain GetSwapChain(int swapChain);
        //
        // Summary:
        //     Retrieves a texture assigned to a stage for a device.
        //
        // Parameters:
        //   stage:
        //     Stage identifier of the texture to retrieve. Stage identifiers are zero-based.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.BaseTexture object that represents the returned
        //     texture.
        Texture GetTexture(int stage);
        //
        // Summary:
        //     Retrieves the Boolean value of a given texture stage state.
        //
        // Parameters:
        //   stage:
        //     Index value of the texture stage to retrieve.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.TextureStageStates enumeration
        //     that represents the texture stage state value to retreive.
        //
        // Returns:
        //     The texture stage state value retrieved.
        bool GetTextureStageStateBoolean(int stage, TextureStageStates state);
        //
        // Summary:
        //     Retrieves the integer value of a given texture stage state.
        //
        // Parameters:
        //   stage:
        //     Index value of the texture stage to retrieve.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.TextureStageStates enumeration
        //     that represents the texture stage state value to retreive.
        //
        // Returns:
        //     The texture stage state value retrieved.
        int GetTextureStageStateInt32(int stage, TextureStageStates state);
        //
        // Summary:
        //     Retrieves the floating-point value of a given texture stage state.
        //
        // Parameters:
        //   stage:
        //     Index value of the texture stage to retrieve.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.TextureStageStates enumeration
        //     that represents the texture stage state value to retreive.
        //
        // Returns:
        //     The texture stage state value retrieved.
        float GetTextureStageStateSingle(int stage, TextureStageStates state);
        //
        // Summary:
        //     Retrieves a matrix that describes a transformation state.
        //
        // Parameters:
        //   state:
        //     Transform that is being retrieved. This parameter can be any member of the
        //     Microsoft.DirectX.Direct3D.TransformType enumerated type.
        //
        // Returns:
        //     A Microsoft.DirectX.Matrix structure that describes the returned transformation
        //     state.
        Matrix GetTransform(TransformType state);
        //
        // Summary:
        //     Retrieves a Boolean vertex shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantCount:
        //     Number of Boolean values in the array of constants.
        //
        // Returns:
        //     Array of constants.
        bool[] GetVertexShaderBooleanConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Retrieves an integer vertex shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantCount:
        //     Number of four-integer vectors in the array of constants.
        //
        // Returns:
        //     Array of constants.
        int[] GetVertexShaderInt32Constant(int startRegister, int constantCount);
        //
        // Summary:
        //     Retrieves a floating-point vertex shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantCount:
        //     Number of four-float vectors in the array of constants.
        //
        // Returns:
        //     Array of constants.
        float[] GetVertexShaderSingleConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Retrieves the volume texture assigned to a stage for a device.
        //
        // Parameters:
        //   stage:
        //     Stage identifier of the Microsoft.DirectX.Direct3D.VolumeTexture to retrieve.
        //     Stage identifiers are zero-based.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.VolumeTexture object that represents the returned
        //     Microsoft.DirectX.Direct3D.VolumeTexture.
        VolumeTexture GetVolumeTexture(int stage);
        //
        // Summary:
        //     Multiplies a device's world, view, or projection matrices by a specified
        //     matrix.
        //
        // Parameters:
        //   state:
        //     Member of the Microsoft.DirectX.Direct3D.TransformType enumerated type that
        //     identifies which device matrix to modify. The most common setting, Microsoft.DirectX.Direct3D.TransformType.World,
        //     modifies the world matrix, but the view or projection matrices also can be
        //     modified.
        //
        //   matrix:
        //     A Microsoft.DirectX.Matrix structure that modifies the current transformation.
        void MultiplyTransform(TransformType state, Matrix matrix);
        //
        // Summary:
        //     Presents the display with the contents of the next buffer in the sequence
        //     of back buffers owned by the device.
        void Present();
        //
        // Summary:
        //     Presents the display with the contents of the next buffer in the sequence
        //     of back buffers owned by the device.
        //
        // Parameters:
        //   overrideWindow:
        //     Pointer to a destination window whose client area is taken as the target
        //     for this presentation. If this parameter is omitted, Microsoft.DirectX.Direct3D.PresentParameters.Microsoft.DirectX.Direct3D.PresentParameters.DeviceWindow
        //     is taken.
        void Present(Control overrideWindow);
        //
        // Summary:
        //     Presents the display with the contents of the next buffer in the sequence
        //     of back buffers owned by the device.
        //
        // Parameters:
        //   overrideWindowHandle:
        //     Destination window whose client area is taken as the target for this presentation.
        //     If this parameter is not used, Microsoft.DirectX.Direct3D.PresentParameters.Microsoft.DirectX.Direct3D.PresentParameters.DeviceWindow
        //     is taken.
        void Present(IntPtr overrideWindowHandle);
        //
        // Summary:
        //     Presents the display with the contents of the next buffer in the sequence
        //     of back buffers owned by the device.
        //
        // Parameters:
        //   rectPresent:
        //     The System.Drawing.Rectangle to present.
        //
        //   sourceRectangle:
        //     Set to true if a System.Drawing.Rectangle is being passed to the Microsoft.DirectX.Direct3D.Device.Present()
        //     parameter; otherwise, false.
        void Present(Rectangle rectPresent, bool sourceRectangle);
        //
        // Summary:
        //     Presents the display with the contents of the next buffer in the sequence
        //     of back buffers owned by the device.
        //
        // Parameters:
        //   rectPresent:
        //     The System.Drawing.Rectangle to present.
        //
        //   overrideWindow:
        //     Pointer to a destination window whose client area is taken as the target
        //     for this presentation. If this parameter is omitted, Microsoft.DirectX.Direct3D.PresentParameters.Microsoft.DirectX.Direct3D.PresentParameters.DeviceWindow
        //     is taken.
        //
        //   sourceRectangle:
        //     Set to true if a System.Drawing.Rectangle is being passed to the Microsoft.DirectX.Direct3D.Device.Present()
        //     parameter; otherwise, false.
        void Present(Rectangle rectPresent, Control overrideWindow, bool sourceRectangle);
        //
        // Summary:
        //     Presents the display with the contents of the next buffer in the sequence
        //     of back buffers owned by the device.
        //
        // Parameters:
        //   rectPresent:
        //     The System.Drawing.Rectangle to present.
        //
        //   overrideWindowHandle:
        //     Destination window whose client area is taken as the target for this presentation.
        //     If this parameter is not used, Microsoft.DirectX.Direct3D.PresentParameters.Microsoft.DirectX.Direct3D.PresentParameters.DeviceWindow
        //     is taken.
        //
        //   sourceRectangle:
        //     Set to true if a System.Drawing.Rectangle is being passed to the Microsoft.DirectX.Direct3D.Device.Present()
        //     parameter; otherwise, false.
        void Present(Rectangle rectPresent, IntPtr overrideWindowHandle, bool sourceRectangle);
        //
        // Summary:
        //     Presents the display with the contents of the next buffer in the sequence
        //     of back buffers owned by the device.
        //
        // Parameters:
        //   sourceRectangle:
        //     A System.Drawing.Rectangle that contains the source rectangle. If the rectangle
        //     exceeds the source surface, it is clipped to the source surface. This parameter
        //     can be used only if the swap chain was created with Microsoft.DirectX.Direct3D.SwapEffect.Copy.
        //
        //   destRectangle:
        //     A System.Drawing.Rectangle that contains the destination rectangle. This
        //     parameter can be used only if the swap chain was created with Microsoft.DirectX.Direct3D.SwapEffect.Copy.
        //
        //   overrideWindow:
        //     Pointer to a destination window whose client area is taken as the target
        //     for this presentation. If this parameter is omitted, Microsoft.DirectX.Direct3D.PresentParameters.Microsoft.DirectX.Direct3D.PresentParameters.DeviceWindow
        //     is taken.
        void Present(Rectangle sourceRectangle, Rectangle destRectangle, Control overrideWindow);
        //
        // Summary:
        //     Presents the display with the contents of the next buffer in the sequence
        //     of back buffers owned by the device.
        //
        // Parameters:
        //   sourceRectangle:
        //     A System.Drawing.Rectangle that contains the source rectangle. If the rectangle
        //     exceeds the source surface, it is clipped to the source surface. This parameter
        //     can be used only if the swap chain was created with Microsoft.DirectX.Direct3D.SwapEffect.Copy.
        //
        //   destRectangle:
        //     A System.Drawing.Rectangle that contains the destination rectangle. This
        //     parameter can be used only if the swap chain was created with Microsoft.DirectX.Direct3D.SwapEffect.Copy.
        //
        //   overrideWindowHandle:
        //     Destination window whose client area is taken as the target for this presentation.
        //     If this parameter is not used, Microsoft.DirectX.Direct3D.PresentParameters.Microsoft.DirectX.Direct3D.PresentParameters.DeviceWindow
        //     is taken.
        void Present(Rectangle sourceRectangle, Rectangle destRectangle, IntPtr overrideWindowHandle);
        //
        // Summary:
        //     Applies the vertex processing defined by the vertex shader to the set of
        //     input data streams, generating a single stream of interleaved vertex data
        //     to the destination vertex buffer.
        //
        // Parameters:
        //   srcStartIndex:
        //     Index of first vertex to load.
        //
        //   destIndex:
        //     Index of first vertex in the destination vertex buffer into which the results
        //     are placed.
        //
        //   vertexCount:
        //     Number of vertices to process.
        //
        //   destBuffer:
        //     A Microsoft.DirectX.Direct3D.VertexBuffer class; the destination vertex buffer
        //     that represents the stream of interleaved vertex data.
        //
        //   vertexDeclaration:
        //     A Microsoft.DirectX.Direct3D.VertexDeclaration class that represents the
        //     output vertex data declaration. When vertex shader 3.0 or later is set as
        //     the current vertex shader, the output vertex declaration must be present.
        void ProcessVertices(int srcStartIndex, int destIndex, int vertexCount, VertexBuffer destBuffer, VertexDeclaration vertexDeclaration);
        //
        // Summary:
        //     Applies the vertex processing defined by the vertex shader to the set of
        //     input data streams, generating a single stream of interleaved vertex data
        //     to the destination vertex buffer.
        //
        // Parameters:
        //   srcStartIndex:
        //     Index of first vertex to load.
        //
        //   destIndex:
        //     Index of first vertex in the destination vertex buffer into which the results
        //     are placed.
        //
        //   vertexCount:
        //     Number of vertices to process.
        //
        //   destBuffer:
        //     A Microsoft.DirectX.Direct3D.VertexBuffer class; the destination vertex buffer
        //     that represents the stream of interleaved vertex data.
        //
        //   vertexDeclaration:
        //     A Microsoft.DirectX.Direct3D.VertexDeclaration class that represents the
        //     output vertex data declaration. When vertex shader 3.0 or later is set as
        //     the current vertex shader, the output vertex declaration must be present.
        //
        //   copyData:
        //     Set to true for default processing. Set to false to prevent the system from
        //     copying vertex data not affected by the vertex operation into the destination
        //     buffer.
        void ProcessVertices(int srcStartIndex, int destIndex, int vertexCount, VertexBuffer destBuffer, VertexDeclaration vertexDeclaration, bool copyData);
        //
        // Summary:
        //     Resets the presentation parameters for the current device.
        //
        // Parameters:
        //   presentationParameters:
        //     [in, out] A Microsoft.DirectX.Direct3D.PresentParameters structure that describes
        //     the new presentation parameters. This value cannot be null.
        void Reset(params PresentParameters[] presentationParameters);
        //
        // Summary:
        //     Sets the current cursor.
        //
        // Parameters:
        //   cursor:
        //     Handle to the cursor.
        //
        //   addWaterMark:
        //     Set to true to add small grey characters that read "D3D" to the upper-left
        //     corner of the cursor image.
        void SetCursor(Cursor cursor, bool addWaterMark);
        //
        // Summary:
        //     Sets the cursor position and update options.
        //
        // Parameters:
        //   positionX:
        //     New X-position of the cursor in virtual desktop coordinates. See Remarks.
        //
        //   positionY:
        //     New Y-position of the cursor in virtual desktop coordinates. See Remarks.
        //
        //   updateImmediate:
        //     Set to true if the system guarantees that the cursor is updated at a minimum
        //     of half of the display refresh rate, but never more frequently than the display
        //     refresh rate. Set to false if the method delays cursor updates until the
        //     next Microsoft.DirectX.Direct3D.Device.Present() call. Setting this parameter
        //     to false usually results in better performance. However, applications should
        //     use true if the rate of calls to Microsoft.DirectX.Direct3D.Device.Present()
        //     is low enough that users would notice a significant delay in cursor motion.
        //     This flag has no effect in an application in windowed mode. Some video cards
        //     implement hardware color cursors; this flag does not affect these cards.
        void SetCursorPosition(int positionX, int positionY, bool updateImmediate);
        //
        // Summary:
        //     Sets properties for the cursor.
        //
        // Parameters:
        //   hotSpotX:
        //     X-coordinate offset (in pixels) that marks the center of the cursor. The
        //     offset is relative to the upper-left corner of the cursor. When the cursor
        //     is given a new position, the image is drawn at an offset from the new position.
        //     The offset is determined by subtracting the hot spot coordinates from the
        //     position.
        //
        //   hotSpotY:
        //     Y-coordinate offset (in pixels) that marks the center of the cursor. The
        //     offset is relative to the upper-left corner of the cursor. When the cursor
        //     is given a new position, the image is drawn at an offset from the new position.
        //     The offset is determined by subtracting the hot spot coordinates from the
        //     position.
        //
        //   cursorBitmap:
        //     A Microsoft.DirectX.Direct3D.Surface object. This parameter must be an 8888
        //     ARGB surface (format Microsoft.DirectX.Direct3D.Format.A8R8G8B8). The contents
        //     of this surface are copied and potentially converted into an internal buffer
        //     from which the cursor is displayed. The dimensions of this surface must be
        //     less than the dimensions of the display mode, and must be a power of two
        //     in each direction, although not necessarily the same power of two. The alpha
        //     channel must be either 0.0 or 1.0.
        void SetCursorProperties(int hotSpotX, int hotSpotY, ISurface cursorBitmap);
        //
        // Summary:
        //     Enables the use of Microsoft Windows Graphics Device Interface (GDI) dialog
        //     boxes in full-screen applications.
        //
        // Parameters:
        //   value:
        //     Set to true to enable Windows Graphics Device Interface (GDI) dialog boxes.
        //     Set to false to disable Windows Graphics Device Interface (GDI) dialog boxes.
        void SetDialogBoxesEnabled(bool value);
        //
        // Summary:
        //     Sets the gamma correction ramp for the implicit swap chain.
        //
        // Parameters:
        //   swapChain:
        //     Unsigned integer that specifies the swap chain.
        //
        //   calibrate:
        //     Set to true to indicate that correction should be applied. Set to false to
        //     indicate that no gamma correction should be applied. The supplied gamma table
        //     is transferred directly to the device.
        //
        //   ramp:
        //     A Microsoft.DirectX.Direct3D.GammaRamp structure that represents the gamma
        //     correction ramp to set for the implicit swap chain.
        void SetGammaRamp(int swapChain, bool calibrate, GammaRamp ramp);
        //
        // Summary:
        //     Sets palette entries.
        //
        // Parameters:
        //   paletteNumber:
        //     Ordinal value that identifies the palette on which the operation is performed.
        //
        //   entries:
        //     Pointer to a Microsoft.DirectX.Direct3D.PaletteEntry structure that represents
        //     the palette entries to set. The number of Microsoft.DirectX.Direct3D.PaletteEntry
        //     structures passed into Microsoft.DirectX.Direct3D.Device.SetPaletteEntries(System.Int32,Microsoft.DirectX.Direct3D.PaletteEntry[])
        //     is assumed to be 256. See Remarks.
        void SetPaletteEntries(int paletteNumber, PaletteEntry[] entries);
        //
        // Summary:
        //     Sets a pixel shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     Array of System.Boolean constants.
        void SetPixelShaderConstant(int startRegister, bool[] constantData);
        //
        // Summary:
        //     Sets a pixel shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     Array of System.Single constants.
        void SetPixelShaderConstant(int startRegister, float[] constantData);
        //
        // Summary:
        //     Sets a pixel shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     Array of System.Int32 constants.
        void SetPixelShaderConstant(int startRegister, int[] constantData);
        //
        // Summary:
        //     Sets a pixel shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     A Microsoft.DirectX.Matrix object representing the constant data to set.
        void SetPixelShaderConstant(int startRegister, Matrix constantData);
        //
        // Summary:
        //     Sets a pixel shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     Array of Microsoft.DirectX.Matrix constants.
        void SetPixelShaderConstant(int startRegister, Matrix[] constantData);
        //
        // Summary:
        //     Sets a pixel shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     A Microsoft.DirectX.Vector4 object representing the constant data to set.
        void SetPixelShaderConstant(int startRegister, Vector4 constantData);
        //
        // Summary:
        //     Sets a pixel shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     Array of Microsoft.DirectX.Vector4 constants.
        void SetPixelShaderConstant(int startRegister, Vector4[] constantData);
        //
        // Summary:
        //     Sets Boolean shader constants.
        //
        // Parameters:
        //   startRegister:
        //     Register number that will contain the first constant value.
        //
        //   constantData:
        //     A Microsoft.DirectX.GraphicsStream object that contains the shader constants
        //     to set.
        //
        //   numberRegisters:
        //     Number of constants contained within Microsoft.DirectX.Direct3D.Device.SetPixelShaderConstantBoolean(System.Int32,Microsoft.DirectX.GraphicsStream,System.Int32).
        void SetPixelShaderConstantBoolean(int startRegister, GraphicsStream constantData, int numberRegisters);
        //
        // Summary:
        //     Sets integer shader constants.
        //
        // Parameters:
        //   startRegister:
        //     Register number that will contain the first constant value.
        //
        //   constantData:
        //     A Microsoft.DirectX.GraphicsStream object that contains the shader constants
        //     to set.
        //
        //   numberRegisters:
        //     Number of constants contained within Microsoft.DirectX.Direct3D.Device.SetPixelShaderConstantInt32(System.Int32,Microsoft.DirectX.GraphicsStream,System.Int32).
        void SetPixelShaderConstantInt32(int startRegister, GraphicsStream constantData, int numberRegisters);
        //
        // Summary:
        //     Sets floating-point shader constants.
        //
        // Parameters:
        //   startRegister:
        //     Register number that will contain the first constant value.
        //
        //   constantData:
        //     A Microsoft.DirectX.GraphicsStream object that contains the shader constants
        //     to set.
        //
        //   numberRegisters:
        //     Number of constants contained within Microsoft.DirectX.Direct3D.Device.SetPixelShaderConstantSingle(System.Int32,Microsoft.DirectX.GraphicsStream,System.Int32).
        void SetPixelShaderConstantSingle(int startRegister, GraphicsStream constantData, int numberRegisters);
        //
        // Summary:
        //     Sets a render state value.
        //
        // Parameters:
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.RenderStates enumeration that
        //     represents the render state value to set.
        //
        //   value:
        //     A Boolean render state value to set.
        void SetRenderState(RenderStates state, bool value);
        //
        // Summary:
        //     Sets a render state value.
        //
        // Parameters:
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.RenderStates enumeration that
        //     represents the render state value to set.
        //
        //   value:
        //     A floating-point render state value to set.
        void SetRenderState(RenderStates state, float value);
        //
        // Summary:
        //     Sets a render state value.
        //
        // Parameters:
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.RenderStates enumeration that
        //     represents the render state value to set.
        //
        //   value:
        //     An integer render state value to set.
        void SetRenderState(RenderStates state, int value);
        //
        // Summary:
        //     Sets a new color buffer for a device.
        //
        // Parameters:
        //   renderTargetIndex:
        //     Index of the render target. See Remarks.
        //
        //   renderTarget:
        //     New color buffer. If set to null, the color buffer for the corresponding
        //     Microsoft.DirectX.Direct3D.Device.SetRenderTarget(System.Int32,Microsoft.DirectX.Direct3D.Surface)
        //     is disabled. Devices must always be associated with a color buffer. The new
        //     render-target surface must have at least Microsoft.DirectX.Direct3D.Usage.RenderTarget
        //     specified.
        void SetRenderTarget(int renderTargetIndex, ISurface renderTarget);
        //
        // Summary:
        //     Sets a sampler stage state value.
        //
        // Parameters:
        //   stage:
        //     Index value of the sampler stage to set.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.SamplerStageStates enumeration
        //     that represents the sampler stage state value to set.
        //
        //   value:
        //     A Boolean value to set for the given sampler stage state.
        void SetSamplerState(int stage, SamplerStageStates state, bool value);
        //
        // Summary:
        //     Sets a sampler stage state value.
        //
        // Parameters:
        //   stage:
        //     Index value of the sampler stage to set.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.SamplerStageStates enumeration
        //     that represents the sampler stage state value to set.
        //
        //   value:
        //     A floating-point value to set for the given sampler stage state.
        void SetSamplerState(int stage, SamplerStageStates state, float value);
        //
        // Summary:
        //     Sets a sampler stage state value.
        //
        // Parameters:
        //   stage:
        //     Index value of the sampler stage to set.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.SamplerStageStates enumeration
        //     that represents the sampler stage state value to set.
        //
        //   value:
        //     An integer value to set for the given sampler stage state.
        void SetSamplerState(int stage, SamplerStageStates state, int value);
        //
        // Summary:
        //     Binds a vertex buffer to a device data stream.
        //
        // Parameters:
        //   streamNumber:
        //     Data stream in the range of 0 to the maximum number of streams -1.
        //
        //   streamData:
        //     Pointer to a Microsoft.DirectX.Direct3D.VertexBuffer class that represents
        //     the vertex buffer to bind to the specified data stream.
        //
        //   offsetInBytes:
        //     Offset from the beginning of the stream to the beginning of the vertex data,
        //     in bytes. To determine whether the device supports stream offsets, see Microsoft.DirectX.Direct3D.DeviceCaps.SupportsStreamOffset.
        void SetStreamSource(int streamNumber, VertexBuffer streamData, int offsetInBytes);
        //
        // Summary:
        //     Binds a vertex buffer to a device data stream.
        //
        // Parameters:
        //   streamNumber:
        //     Data stream in the range of 0 to the maximum number of streams -1.
        //
        //   streamData:
        //     Pointer to a Microsoft.DirectX.Direct3D.VertexBuffer class that represents
        //     the vertex buffer to bind to the specified data stream.
        //
        //   offsetInBytes:
        //     Offset from the beginning of the stream to the beginning of the vertex data,
        //     in bytes. To determine whether the device supports stream offsets, see Microsoft.DirectX.Direct3D.DeviceCaps.SupportsStreamOffset.
        //
        //   stride:
        //     Stride of the component, in bytes. See Remarks.
        void SetStreamSource(int streamNumber, VertexBuffer streamData, int offsetInBytes, int stride);
        //
        // Summary:
        //     Sets the stream source frequency divider value.
        //
        // Parameters:
        //   streamNumber:
        //     Stream source number.
        //
        //   divider:
        //     Frequency divider value.
        void SetStreamSourceFrequency(int streamNumber, int divider);
        //
        // Summary:
        //     Assigns a texture to a device stage.
        //
        // Parameters:
        //   stage:
        //     Index value for the device stage.
        //
        //   texture:
        //     A Microsoft.DirectX.Direct3D.BaseTexture object that represents the texture
        //     being set.
        void SetTexture(int stage, BaseTexture texture);
        //
        // Summary:
        //     Sets a texture stage state value.
        //
        // Parameters:
        //   stage:
        //     Index value of the texture stage to set.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.TextureStageStates enumeration
        //     that represents the texture stage state value to set.
        //
        //   value:
        //     A Boolean value to set for the given texture stage state.
        void SetTextureStageState(int stage, TextureStageStates state, bool value);
        //
        // Summary:
        //     Sets a texture stage state value.
        //
        // Parameters:
        //   stage:
        //     Index value of the texture stage to set.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.TextureStageStates enumeration
        //     that represents the texture stage state value to set.
        //
        //   value:
        //     A floating-point value to set for the given texture stage state.
        void SetTextureStageState(int stage, TextureStageStates state, float value);
        //
        // Summary:
        //     Sets a texture stage state value.
        //
        // Parameters:
        //   stage:
        //     Index value of the texture stage to set.
        //
        //   state:
        //     A member from the Microsoft.DirectX.Direct3D.TextureStageStates enumeration
        //     that represents the texture stage state value to set.
        //
        //   value:
        //     An integer value to set for the given texture stage state.
        void SetTextureStageState(int stage, TextureStageStates state, int value);
        //
        // Summary:
        //     Sets a single device transform.
        //
        // Parameters:
        //   state:
        //     Type of transform that is being modified; can be any member of the Microsoft.DirectX.Direct3D.TransformType
        //     enumerated type.
        //
        //   matrix:
        //     A Microsoft.DirectX.Matrix structure that modifies the current transform.
        void SetTransform(TransformType state, Matrix matrix);
        //
        // Summary:
        //     Sets a vertex shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     Array of System.Boolean constants.
        void SetVertexShaderConstant(int startRegister, bool[] constantData);
        //
        // Summary:
        //     Sets a vertex shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     Array of System.Single constants.
        void SetVertexShaderConstant(int startRegister, float[] constantData);
        //
        // Summary:
        //     Sets a vertex shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     Array of System.Int32 constants.
        void SetVertexShaderConstant(int startRegister, int[] constantData);
        //
        // Summary:
        //     Sets a vertex shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     A Microsoft.DirectX.Matrix object representing the constant data to set.
        void SetVertexShaderConstant(int startRegister, Matrix constantData);
        //
        // Summary:
        //     Sets a vertex shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     Array of Microsoft.DirectX.Matrix constants.
        void SetVertexShaderConstant(int startRegister, Matrix[] constantData);
        //
        // Summary:
        //     Sets a vertex shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     A Microsoft.DirectX.Vector4 object representing the constant data to set.
        void SetVertexShaderConstant(int startRegister, Vector4 constantData);
        //
        // Summary:
        //     Sets a vertex shader constant.
        //
        // Parameters:
        //   startRegister:
        //     Register number that contains the first constant value.
        //
        //   constantData:
        //     Array of Microsoft.DirectX.Vector4 constants.
        void SetVertexShaderConstant(int startRegister, Vector4[] constantData);
        //
        // Summary:
        //     Sets Boolean vertex shader constants.
        //
        // Parameters:
        //   startRegister:
        //     Register number that will contain the first constant value.
        //
        //   constantData:
        //     A Microsoft.DirectX.GraphicsStream object that contains the shader constants
        //     to set.
        //
        //   numberRegisters:
        //     Number of constants contained within Microsoft.DirectX.Direct3D.Device.SetVertexShaderConstantBoolean(System.Int32,Microsoft.DirectX.GraphicsStream,System.Int32).
        void SetVertexShaderConstantBoolean(int startRegister, GraphicsStream constantData, int numberRegisters);
        //
        // Summary:
        //     Sets integer vertex shader constants.
        //
        // Parameters:
        //   startRegister:
        //     Register number that will contain the first constant value.
        //
        //   constantData:
        //     A Microsoft.DirectX.GraphicsStream object that contains the shader constants
        //     to set.
        //
        //   numberRegisters:
        //     Number of constants contained within Microsoft.DirectX.Direct3D.Device.SetVertexShaderConstantInt32(System.Int32,Microsoft.DirectX.GraphicsStream,System.Int32).
        void SetVertexShaderConstantInt32(int startRegister, GraphicsStream constantData, int numberRegisters);
        //
        // Summary:
        //     Sets floating-point vertex shader constants.
        //
        // Parameters:
        //   startRegister:
        //     Register number that will contain the first constant value.
        //
        //   constantData:
        //     A Microsoft.DirectX.GraphicsStream object that contains the shader constants
        //     to set.
        //
        //   numberRegisters:
        //     Number of constants contained within Microsoft.DirectX.Direct3D.Device.SetVertexShaderConstantSingle(System.Int32,Microsoft.DirectX.GraphicsStream,System.Int32).
        void SetVertexShaderConstantSingle(int startRegister, GraphicsStream constantData, int numberRegisters);
        //
        // Summary:
        //     Displays or hides the cursor.
        //
        // Parameters:
        //   canShow:
        //     Set to true if the cursor is shown. Set to false if the cursor is hidden.
        //
        // Returns:
        //     Value that indicates whether the cursor was previously visible. If set to
        //     true, the cursor was visible; if set to false, it was not.
        bool ShowCursor(bool canShow);
        //
        // Summary:
        //     Copies the contents of the source rectangle to the destination rectangle.
        //
        // Parameters:
        //   sourceSurface:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the source surface.
        //
        //   sourceRectangle:
        //     A System.Drawing.Rectangle object that represents the source rectangle. If
        //     set to null, the entire source surface is used.
        //
        //   destSurface:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the destination
        //     surface.
        //
        //   destRectangle:
        //     A System.Drawing.Rectangle object that represents the destination rectangle.
        //     If set to null, the entire destination surface is used.
        //
        //   filter:
        //     Filter type. Allowable values are Microsoft.DirectX.Direct3D.TextureFilter.None,
        //     Microsoft.DirectX.Direct3D.TextureFilter.Point, or Microsoft.DirectX.Direct3D.TextureFilter.Linear.
        //     For more information, see Microsoft.DirectX.Direct3D.TextureFilter.
        void StretchRectangle(ISurface sourceSurface, Rectangle sourceRectangle, ISurface destSurface, Rectangle destRectangle, TextureFilter filter);
        //
        // Summary:
        //     Reports the current cooperative-level status of the Microsoft Direct3D device
        //     for a windowed or full-screen application.
        void TestCooperativeLevel();
        //
        // Summary:
        //     Copies rectangular subsets of pixels from one surface to another.
        //
        // Parameters:
        //   sourceSurface:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the source surface;
        //     must point to a surface other than Microsoft.DirectX.Direct3D.Device.UpdateSurface().
        //
        //   destinationSurface:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the destination
        //     surface.
        void UpdateSurface(ISurface sourceSurface, ISurface destinationSurface);
        //
        // Summary:
        //     Copies rectangular subsets of pixels from one surface to another.
        //
        // Parameters:
        //   sourceSurface:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the source surface;
        //     must point to a surface other than Microsoft.DirectX.Direct3D.Device.UpdateSurface().
        //
        //   sourceRect:
        //     A System.Drawing.Rectangle object that represents the rectangle on the source
        //     surface. Specifying null for this parameter causes the entire surface to
        //     be copied.
        //
        //   destinationSurface:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the destination
        //     surface.
        void UpdateSurface(ISurface sourceSurface, Rectangle sourceRect, ISurface destinationSurface);
        //
        // Summary:
        //     Copies rectangular subsets of pixels from one surface to another.
        //
        // Parameters:
        //   sourceSurface:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the source surface;
        //     must point to a surface other than Microsoft.DirectX.Direct3D.Device.UpdateSurface().
        //
        //   destinationSurface:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the destination
        //     surface.
        //
        //   destPoint:
        //     A System.Drawing.Point object that represents the upper-left corner of the
        //     destination rectangle. Specifying null for this parameter causes the entire
        //     surface to be copied.
        void UpdateSurface(ISurface sourceSurface, ISurface destinationSurface, Point destPoint);
        //
        // Summary:
        //     Copies rectangular subsets of pixels from one surface to another.
        //
        // Parameters:
        //   sourceSurface:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the source surface;
        //     must point to a surface other than Microsoft.DirectX.Direct3D.Device.UpdateSurface().
        //
        //   sourceRect:
        //     A System.Drawing.Rectangle object that represents the rectangle on the source
        //     surface. Specifying null for this parameter causes the entire surface to
        //     be copied.
        //
        //   destinationSurface:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the destination
        //     surface.
        //
        //   destPoint:
        //     A System.Drawing.Point object that represents the upper-left corner of the
        //     destination rectangle. Specifying null for this parameter causes the entire
        //     surface to be copied.
        void UpdateSurface(ISurface sourceSurface, Rectangle sourceRect, ISurface destinationSurface, Point destPoint);
        //
        // Summary:
        //     Updates the dirty portions of a texture.
        //
        // Parameters:
        //   sourceTexture:
        //     A Microsoft.DirectX.Direct3D.BaseTexture object that represents the source
        //     texture, which must be in system memory (Microsoft.DirectX.Direct3D.Pool.Pool.SystemMemory).
        //
        //   destinationTexture:
        //     A Microsoft.DirectX.Direct3D.BaseTexture object that represents the destination
        //     texture, which must be in the default memory pool (Microsoft.DirectX.Direct3D.Pool.Pool.Default).
        void UpdateTexture(BaseTexture sourceTexture, BaseTexture destinationTexture);
        //
        // Summary:
        //     Reports the device's ability to render the current texture-blending operations
        //     and arguments in a single pass.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.ValidateDeviceParams object that provides the
        //     number of passes and the result code of the validation check.
        ValidateDeviceParams ValidateDevice();

    }
}
