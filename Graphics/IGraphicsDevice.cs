using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface IGraphicsDevice : IDisposable
    {
        // Summary:
        //     Retrieves the clipping planes of the current GraphicsDevice.
        //
        // Returns:
        //     The clipping planes.
        ClipPlaneCollection ClipPlanes { get; }
        //
        // Summary:
        //     Retrieves the creation parameters of the GraphicsDevice.
        //
        // Returns:
        //     The creation parameters.
        GraphicsDeviceCreationParameters CreationParameters { get; }
        //
        // Summary:
        //     Gets or sets the depth stencil surface of this GraphicsDevice.
        //
        // Returns:
        //     The depth stencil buffer. If no depth stencil buffer is associated with the
        //     GraphicsDevice, the return value is null.Setting this value to null disables
        //     the depth stencil operation.
        IDepthStencilBuffer DepthStencilBuffer { get; set; }
        //
        // Summary:
        //     Retrieves the display mode's spatial resolution, color resolution, and refresh
        //     frequency.
        //
        // Returns:
        //     Data describing the display mode of the adapter (as opposed to the display
        //     mode of the GraphicsDevice, which might not be active if the GraphicsDevice
        //     does not own full-screen mode).
        DisplayMode DisplayMode { get; }
        //
        // Summary:
        //     Returns the driver level.
        //
        // Returns:
        //     This method returns the driver version, which is one of the following: 700
        //     - Direct3D 7 level driver800 - Direct3D 8 level driver900 - Direct3D 9 level
        //     driver
        int DriverLevel { get; }
        //
        // Summary:
        //     Gets the capabilities of the graphics device.
        //
        // Returns:
        //     The capabilities.
        GraphicsDeviceCapabilities GraphicsDeviceCapabilities { get; }
        //
        // Summary:
        //     Retrieves the status of the device
        //
        // Returns:
        //     The status of the device
        GraphicsDeviceStatus GraphicsDeviceStatus { get; }
        //
        // Summary:
        //     Gets or sets index data.
        //
        // Returns:
        //     The index data.
        IIndexBuffer Indices { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        //
        // Returns:
        //     true if the object is disposed; false otherwise.
        bool IsDisposed { get; }
        //
        // Summary:
        //     Gets or sets the current pixel shader.
        //
        // Returns:
        //     The current pixel shader or a pixel shader object to set.
        PixelShader PixelShader { get; set; }
        //
        // Summary:
        //     Gets the presentation parameters associated with this graphics device.
        //
        // Returns:
        //     The presentation parameters associated with this graphics device.
        PresentationParameters PresentationParameters { get; }
        //
        // Summary:
        //     Retrieves information that describes the raster of the monitor on which the
        //     swap chain is presented.
        //
        // Returns:
        //     Information about the position or other status of the raster on the monitor
        //     driven by the current adapter.
        RasterStatus RasterStatus { get; }
        //
        // Summary:
        //     Retrieves a render-state value for a GraphicsDevice.
        //
        // Returns:
        //     The render state.
        IRenderState RenderState { get; }
        //
        // Summary:
        //     Retrieves a collection of SamplerState objects for the current GraphicsDevice.
        //
        // Returns:
        //     The sample states of this GraphicsDevice.
        SamplerStateCollection SamplerStates { get; }
        //
        // Summary:
        //     Gets or sets the rectangle used for scissor testing.
        //
        // Returns:
        //     Defines the rendering area within the render target, if scissor testing is
        //     enabled.
        Rectangle ScissorRectangle { get; set; }
        //
        // Summary:
        //     Returns the collection of textures that have been assigned to the texture
        //     stages of the device.
        //
        // Returns:
        //     The texture collection.
        TextureCollection Textures { get; }
        //
        // Summary:
        //     Gets or sets a vertex shader declaration.
        //
        // Returns:
        //     The vertex shader declaration.
        IVertexDeclaration VertexDeclaration { get; set; }
        //
        // Summary:
        //     Gets the collection of vertex sampler states.
        //
        // Returns:
        //     The collection of vertex sampler states.
        SamplerStateCollection VertexSamplerStates { get; }
        //
        // Summary:
        //     Gets or sets the current vertex shader.
        //
        // Returns:
        //     The GraphicsDevice's current vertex shader or a vertex shader object to set.
        VertexShader VertexShader { get; set; }
        //
        // Summary:
        //     Gets the collection of vertex textures that support texture lookup in the
        //     vertex shader using the texldl - vs texture load statement. The vertex engine
        //     contains four texture sampler stages.
        //
        // Returns:
        //     The collection of vertex textures.
        TextureCollection VertexTextures { get; }
        //
        // Summary:
        //     Gets the vertex stream collection.
        //
        // Returns:
        //     The vertex stream collection.
        IVertexStreamCollection Vertices { get; }
        //
        // Summary:
        //     Gets or sets a viewport identifying the portion of the render target to receive
        //     draw calls.
        //
        // Returns:
        //     The viewport to set or get.
        Viewport Viewport { get; set; }

        //// Summary:
        ////     Occurs when a GraphicsDevice is about to be lost (for example, immediately
        ////     before a reset).
        //event EventHandler DeviceLost;
        ////
        //// Summary:
        ////     Occurs after a GraphicsDevice is reset, allowing an application to recreate
        ////     all resources.
        //event EventHandler DeviceReset;
        ////
        //// Summary:
        ////     Occurs when a GraphicsDevice is resetting, allowing the application to cancel
        ////     the default handling of the reset.
        //event EventHandler DeviceResetting;
        ////
        //// Summary:
        ////     Occurs when Dispose is called or when this object is finalized and collected
        ////     by the garbage collector of the Microsoft .NET common language runtime.
        //event EventHandler Disposing;
        ////
        //// Summary:
        ////     Occurs when a resource is created.
        //event EventHandler<ResourceCreatedEventArgs> ResourceCreated;
        ////
        //// Summary:
        ////     Occurs when a resource is destroyed.
        //event EventHandler<ResourceDestroyedEventArgs> ResourceDestroyed;

        // Summary:
        //     Clears the viewport to a specified color.
        //
        // Parameters:
        //   color:
        //     Color value to which the render target surface is cleared.
        void Clear(Color color);
        //
        // Summary:
        //     Clears the viewport to a specified color, clears the depth buffer, and erases
        //     the stencil buffer.
        //
        // Parameters:
        //   options:
        //     Flags indicating which surfaces to clear.
        //
        //   color:
        //     Color value to which the render target surface is cleared.
        //
        //   depth:
        //     New depth value that this method stores in the depth buffer. This parameter
        //     can be in the range of 0.0 through 1.0 (for z-based or w-based depth buffers).
        //     A value of 0.0 represents the nearest distance to the viewer; a value of
        //     1.0 represents the farthest distance.
        //
        //   stencil:
        //     Integer value to store in each stencil-buffer entry. This parameter can be
        //     in the range of 0 through 2n−1, where n is the bit depth of the stencil buffer.
        void Clear(ClearOptions options, Color color, float depth, int stencil);
        //
        // Summary:
        //     Clears the viewport to a specified color, clears the depth buffer, and erases
        //     the stencil buffer.
        //
        // Parameters:
        //   options:
        //     The surfaces to clear.
        //
        //   color:
        //     Color value to which the render target surface is cleared.
        //
        //   depth:
        //     New z value that this method stores in the depth buffer. This parameter can
        //     be in the range of 0.0 through 1.0 (for z-based or w-based depth buffers).
        //     A value of 0.0 represents the nearest distance to the viewer; a value of
        //     1.0 represents the farthest distance.
        //
        //   stencil:
        //     Integer value to store in each stencil-buffer entry. This parameter can be
        //     in the range of 0 through 2n−1, where n is the bit depth of the stencil buffer.
        void Clear(ClearOptions options, Vector4 color, float depth, int stencil);
        //
        // Summary:
        //     Clears a set of regions to a specified color, clears the depth buffer, and
        //     erases the stencil buffer.
        //
        // Parameters:
        //   options:
        //     Flags that indicate which surfaces to clear.
        //
        //   color:
        //     Color value to which the render target surface is cleared.
        //
        //   depth:
        //     New depth value to store in the depth buffer. This parameter can be in the
        //     range of 0.0 through 1.0 (for z-based or w-based depth buffers). A value
        //     of 0.0 represents the nearest distance to the viewer; a value of 1.0 represents
        //     the farthest distance.
        //
        //   stencil:
        //     Integer value to store in each stencil-buffer entry. This parameter can be
        //     in the range of 0 through 2n−1, where n is the bit depth of the stencil buffer.
        //
        //   regions:
        //     The regions to clear.
        void Clear(ClearOptions options, Color color, float depth, int stencil, Rectangle[] regions);
        //
        // Summary:
        //     Clears a set of regions to a specified color, clears the depth buffer, and
        //     erases the stencil buffer.
        //
        // Parameters:
        //   options:
        //     The surfaces to clear.
        //
        //   color:
        //     Color value to which the render target surface is cleared.
        //
        //   depth:
        //     New z value that this method stores in the depth buffer. This parameter can
        //     be in the range of 0.0 through 1.0 (for z-based or w-based depth buffers).
        //     A value of 0.0 represents the nearest distance to the viewer; a value of
        //     1.0 represents the farthest distance.
        //
        //   stencil:
        //     Integer value to store in each stencil-buffer entry. This parameter can be
        //     in the range of 0 through 2n−1, where n is the bit depth of the stencil buffer.
        //
        //   regions:
        //     The regions to clear.
        void Clear(ClearOptions options, Vector4 color, float depth, int stencil, Rectangle[] regions);
        //
        // Summary:
        //     Renders the specified geometric primitive, based on indexing into an array
        //     of vertices.
        //
        // Parameters:
        //   primitiveType:
        //     Describes the type of primitive to render. PrimitiveType.PointList is not
        //     supported with this method.
        //
        //   baseVertex:
        //     Offset to add to each vertex index in the index buffer.
        //
        //   minVertexIndex:
        //     Minimum vertex index for vertices used during the call. The minVertexIndex
        //     parameter and all of the indices in the index stream are relative to the
        //     baseVertex parameter.
        //
        //   numVertices:
        //     A number of vertices used during the call. The first vertex is located at
        //     index: baseVertex + minVertexIndex.
        //
        //   startIndex:
        //     Location in the index array at which to start reading vertices.
        //
        //   primitiveCount:
        //     A number of primitives to render. The number of vertices used is a function
        //     of primitiveCount and primitiveType. To determine the maximum number of primitives
        //     allowed, check the MaxPrimitiveCount property member of the Capabilities
        //     structure.
        void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCount);
        //
        // Summary:
        //     Renders a sequence of non-indexed geometric primitives of the specified type
        //     from the current set of data input streams.
        //
        // Parameters:
        //   primitiveType:
        //     Describes the type of primitive to render.
        //
        //   startVertex:
        //     Index of the first vertex to load. Beginning at startVertex, the correct
        //     number of vertices is read out of the vertex buffer.
        //
        //   primitiveCount:
        //     Number of primitives to render. To determine the maximum number of primitives
        //     allowed, check GraphicsDeviceCapabilities.MaxPrimitiveCount. The primitiveCount
        //     is the number of primitives as determined by the primitive type. If it is
        //     a line list, each primitive has two vertices. If it is a triangle list, each
        //     primitive has three vertices.
        void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount);
        //
        // Summary:
        //     Renders geometric primitives with indexed data specified by the user, specifying
        //     an index buffer as an array of type System.Int32.
        //
        // Parameters:
        //   primitiveType:
        //     Describes the type of primitive to render.
        //
        //   vertexData:
        //     The vertex buffer indexed by indexData.
        //
        //   vertexOffset:
        //     Offset to add to each vertex index in the index buffer.
        //
        //   numVertices:
        //     Number of vertices used during this call. The first vertex is located at
        //     index minVertexIndex.
        //
        //   indexData:
        //     A list of indices into the vertex buffer, given in the order that you want
        //     the vertices to render. Using an array of type System.Int32, which uses 32
        //     bits per element, allows you to index a greater number of elements in the
        //     vertex buffer.
        //
        //   indexOffset:
        //     Location in the index array at which to start reading vertices.
        //
        //   primitiveCount:
        //     Number of primitives to render. The maximum number of primitives allowed
        //     is determined by checking GraphicsDeviceCapabilities.MaxPrimitiveCount. (The
        //     number of indices is a function of the primitive count and the primitive
        //     type.)
        void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount) where T : struct;
        //
        // Summary:
        //     Renders geometric primitives with indexed data specified by the user, specifying
        //     an index buffer as an array of type System.Int16.
        //
        // Parameters:
        //   primitiveType:
        //     Describes the type of primitive to render.
        //
        //   vertexData:
        //     The vertex buffer indexed by indexData.
        //
        //   vertexOffset:
        //     Offset to add to each vertex index in the index buffer.
        //
        //   numVertices:
        //     Number of vertices used during this call. The first vertex is located at
        //     index minVertexIndex.
        //
        //   indexData:
        //     A list of indices into the vertex buffer, given in the order that you want
        //     the vertices to render. Using an array of type System.Int16, which uses 16
        //     bits per element, allows you conserve resources if the index buffer does
        //     not require a 32-bit depth array.
        //
        //   indexOffset:
        //     Location in the index array at which to start reading vertices.
        //
        //   primitiveCount:
        //     Number of primitives to render. The maximum number of primitives allowed
        //     is determined by checking GraphicsDeviceCapabilities.MaxPrimitiveCount. (The
        //     number of indices is a function of the primitive count and the primitive
        //     type.)
        void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount) where T : struct;
        //
        // Summary:
        //     Renders the given geometric primitive with data specified by the user.
        //
        // Parameters:
        //   primitiveType:
        //     Describes the type of primitive to render.
        //
        //   vertexData:
        //     The vertex data.
        //
        //   vertexOffset:
        //     Offset at which to begin reading vertexData.
        //
        //   primitiveCount:
        //     Number of primitives to render. The maximum number of primitives allowed
        //     is determined by checking GraphicsDeviceCapabilities.MaxPrimitiveCount. The
        //     number of indices is a function of primitiveCount and primitiveType.
        void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount) where T : struct;
        //
        // Summary:
        //     Evicts all managed resources, including Microsoft Direct3D resources and
        //     those that are driver managed.
        void EvictManagedResources();
        //
        // Summary:
        //     Gets the gamma correction ramp.
        //
        // Returns:
        //     The gamma correction ramp.
        GammaRamp GetGammaRamp();
        //
        // Summary:
        //     Gets an array of System.Boolean values from the pixel shader constant Boolean
        //     registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader Boolean constant register of the first constant.
        //
        //   constantCount:
        //     Number of System.Boolean values to retrieve.
        //
        // Returns:
        //     Array of System.Boolean values retrieved from the constant registers.
        bool[] GetPixelShaderBooleanConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets an array of System.Int32 values from the pixel shader constant integer
        //     registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader integer register of the first constant.
        //
        //   constantCount:
        //     Number of System.Int32 values to retrieve.
        //
        // Returns:
        //     Array of System.Int32 values retrieved from the constant registers.
        int[] GetPixelShaderInt32Constant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets an array of Framework.Matrix values from the pixel shader constant float
        //     registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of Framework.Matrix values to retrieve.
        //
        // Returns:
        //     Array of Framework.Matrix values retrieved from the constant registers.
        Matrix[] GetPixelShaderMatrixArrayConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets a Framework.Matrix value from the pixel shader constant float registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        // Returns:
        //     Framework.Matrix value retrieved from the constant registers.
        Matrix GetPixelShaderMatrixConstant(int startRegister);
        //
        // Summary:
        //     Gets an array of Framework.Quaternion values from the pixel shader constant
        //     float registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of Framework.Quaternion values to retrieve.
        //
        // Returns:
        //     Array of Framework.Quaternion values retrieved from the constant registers.
        Quaternion[] GetPixelShaderQuaternionArrayConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets a Framework.Quaternion value from the pixel shader constant float registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        // Returns:
        //     Framework.Quaternion value retrieved from the constant registers.
        Quaternion GetPixelShaderQuaternionConstant(int startRegister);
        //
        // Summary:
        //     Gets an array of System.Single values from the pixel shader constant float
        //     registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of System.Single values to retrieve.
        //
        // Returns:
        //     Array of System.Single values retrieved from the constant registers.
        float[] GetPixelShaderSingleConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets an array of Framework.Vector2 values from the pixel shader constant
        //     float registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of Framework.Vector2 values to retrieve.
        //
        // Returns:
        //     Array of Framework.Vector2 values retrieved from the constant registers.
        Vector2[] GetPixelShaderVector2ArrayConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets a Framework.Vector2 value from the pixel shader constant float registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        // Returns:
        //     Framework.Vector2 value retrieved from the constant registers.
        Vector2 GetPixelShaderVector2Constant(int startRegister);
        //
        // Summary:
        //     Gets an array of Framework.Vector3 values from the pixel shader constant
        //     float registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of Framework.Vector3 values to retrieve.
        //
        // Returns:
        //     Array of Framework.Vector3 values retrieved from the constant registers.
        Vector3[] GetPixelShaderVector3ArrayConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets a Framework.Vector3 value from the pixel shader constant float registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        // Returns:
        //     Framework.Vector3 value retrieved from the constant registers.
        Vector3 GetPixelShaderVector3Constant(int startRegister);
        //
        // Summary:
        //     Gets an array of Framework.Vector4 values from the pixel shader constant
        //     float registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of Framework.Vector4 values to retrieve.
        //
        // Returns:
        //     Array of Framework.Vector4 values retrieved from the constant registers.
        Vector4[] GetPixelShaderVector4ArrayConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets a Framework.Vector4 value from the pixel shader constant float registers.
        //
        // Parameters:
        //   startRegister:
        //     Pixel shader constant float register of the first constant.
        //
        // Returns:
        //     Framework.Vector4 value retrieved from the constant registers.
        Vector4 GetPixelShaderVector4Constant(int startRegister);
        //
        // Summary:
        //     Gets a render target surface.
        //
        // Parameters:
        //   renderTargetIndex:
        //     Index of the render target.
        //
        // Returns:
        //     The render target surface of the current graphics device.
        IRenderTarget GetRenderTarget(int renderTargetIndex);
        //
        // Summary:
        //     Gets an array of System.Boolean values from the vertex shader constant Boolean
        //     registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant Boolean register of the first constant.
        //
        //   constantCount:
        //     Number of System.Boolean values to retrieve.
        //
        // Returns:
        //     Array of System.Boolean values retrieved from the constant registers.
        bool[] GetVertexShaderBooleanConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets an array of System.Int32 values from the vertex shader constant integer
        //     registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant integer register of the first constant.
        //
        //   constantCount:
        //     Number of System.Int32 values to retrieve.
        //
        // Returns:
        //     Array of System.Int32 values retrieved from the constant registers.
        int[] GetVertexShaderInt32Constant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets an array of Framework.Matrix values from the vertex shader constant
        //     float registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of Framework.Matrix values to retrieve.
        //
        // Returns:
        //     Array of Framework.Matrix values retrieved from the constant registers.
        Matrix[] GetVertexShaderMatrixArrayConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets a Framework.Matrix value from the vertex shader constant float registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        // Returns:
        //     Framework.Matrix value retrieved from the constant registers.
        Matrix GetVertexShaderMatrixConstant(int startRegister);
        //
        // Summary:
        //     Gets an array of Framework.Quaternion values from the vertex shader constant
        //     float registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of Framework.Quaternion values to retrieve.
        //
        // Returns:
        //     Array of Framework.Quaternion values retrieved from the constant registers.
        Quaternion[] GetVertexShaderQuaternionArrayConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets a Framework.Quaternion value from the vertex shader constant float registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        // Returns:
        //     Framework.Quaternion value retrieved from the constant registers.
        Quaternion GetVertexShaderQuaternionConstant(int startRegister);
        //
        // Summary:
        //     Gets an array of System.Single values from the vertex shader constant float
        //     registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of System.Single values to retrieve.
        //
        // Returns:
        //     Array of System.Single values retrieved from the constant registers.
        float[] GetVertexShaderSingleConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets an array of Framework.Vector2 values from the vertex shader constant
        //     float registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of Framework.Vector2 values to retrieve.
        //
        // Returns:
        //     Array of Framework.Vector2 values retrieved from the constant registers.
        Vector2[] GetVertexShaderVector2ArrayConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets a Framework.Vector2 value from the vertex shader constant float registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        // Returns:
        //     Framework.Vector2 value retrieved from the constant registers.
        Vector2 GetVertexShaderVector2Constant(int startRegister);
        //
        // Summary:
        //     Gets an array of Framework.Vector3 values from the vertex shader constant
        //     float registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of Framework.Vector3 values to retrieve.
        //
        // Returns:
        //     Array of Framework.Vector3 values retrieved from the constant registers.
        Vector3[] GetVertexShaderVector3ArrayConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets a Framework.Vector3 value from the vertex shader constant float registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        // Returns:
        //     Framework.Vector3 value retrieved from the constant registers.
        Vector3 GetVertexShaderVector3Constant(int startRegister);
        //
        // Summary:
        //     Gets an array of Framework.Vector4 values from the vertex shader constant
        //     float registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        //   constantCount:
        //     Number of Framework.Vector4 values to retrieve.
        //
        // Returns:
        //     Array of Framework.Vector4 values retrieved from the constant registers.
        Vector4[] GetVertexShaderVector4ArrayConstant(int startRegister, int constantCount);
        //
        // Summary:
        //     Gets a Framework.Vector4 value from the vertex shader constant float registers.
        //
        // Parameters:
        //   startRegister:
        //     Vertex shader constant float register of the first constant.
        //
        // Returns:
        //     Framework.Vector4 value retrieved from the constant registers.
        Vector4 GetVertexShaderVector4Constant(int startRegister);
        //
        // Summary:
        //     Presents the display with the contents of the next buffer in the sequence
        //     of back buffers owned by the GraphicsDevice.
        void Present();
        //
        // Summary:
        //     Specifies the window target for a presentation and presents the display with
        //     the contents of the next buffer in the sequence of back buffers owned by
        //     the GraphicsDevice.
        //
        // Parameters:
        //   overrideWindowHandle:
        //     Destination window containing the client area that is the target for this
        //     presentation. If not specified, this is PresentationParameters.DeviceWindowHandle.
        void Present(IntPtr overrideWindowHandle);
        //
        // Summary:
        //     Specifies the window target for a presentation and presents the display with
        //     the contents of the next buffer in the sequence of back buffers owned by
        //     the GraphicsDevice.
        //
        // Parameters:
        //   sourceRectangle:
        //     The source rectangle. If null, the entire source surface is presented. If
        //     the rectangle exceeds the source surface, the rectangle is clipped to the
        //     source surface. This parameter must be null unless the swap chain was created
        //     with SwapEffect.Copy.
        //
        //   destinationRectangle:
        //     The destination rectangle, in window client coordinates. If null, the entire
        //     client area is filled. If the rectangle exceeds the destination client area,
        //     the rectangle is clipped to the destination client area. This parameter must
        //     be null unless the swap chain was created with SwapEffect.Copy.
        //
        //   overrideWindowHandle:
        //     Destination window containing the client area that is the target for this
        //     presentation. If not specified, this is PresentationParameters.DeviceWindowHandle.
        void Present(Rectangle? sourceRectangle, Rectangle? destinationRectangle, IntPtr overrideWindowHandle);
        //
        // Summary:
        //     Resets the presentation parameters for the current GraphicsDevice.
        void Reset();
        //
        // Summary:
        //     Resets the presentation parameters for the specified Microsoft.Xna.Framework.Graphics.GraphicsDevice.Reset(Microsoft.Xna.Framework.Graphics.GraphicsAdapter).
        //
        // Parameters:
        //   graphicsAdapter:
        //     The graphics device being reset.
        void Reset(GraphicsAdapter graphicsAdapter);
        //
        // Summary:
        //     Resets the current GraphicsDevice with the specified PresentationParameters.
        //
        // Parameters:
        //   presentationParameters:
        //     Describes the new presentation parameters. This value cannot be null.
        void Reset(PresentationParameters presentationParameters);
        //
        // Summary:
        //     Resets the specified Microsoft.Xna.Framework.Graphics.GraphicsDevice.Reset(Microsoft.Xna.Framework.Graphics.PresentationParameters,Microsoft.Xna.Framework.Graphics.GraphicsAdapter)
        //     with the specified presentation parameters.
        //
        // Parameters:
        //   presentationParameters:
        //     Describes the new presentation parameters. This value cannot be null.
        //
        //   graphicsAdapter:
        //     The graphics device being reset.
        void Reset(PresentationParameters presentationParameters, GraphicsAdapter graphicsAdapter);
        //
        // Summary:
        //     Copies the current back buffer contents to a texture.
        //
        // Parameters:
        //   resolveTarget:
        //     Texture to update with the resolved back buffer.
        void ResolveBackBuffer(IResolveTexture2D resolveTarget);
        //
        // Summary:
        //     Copies the contents of the back buffer at the specified index to a texture.
        //
        // Parameters:
        //   resolveTarget:
        //     Texture to update with the resolved back buffer.
        //
        //   backBufferIndex:
        //     Index of the back buffer to resolve.
        void ResolveBackBuffer(IResolveTexture2D resolveTarget, int backBufferIndex);
        //
        // Summary:
        //     Sets the gamma correction ramp.
        //
        // Parameters:
        //   calibrate:
        //     true to indicate that correction should be applied. false to indicate that
        //     no gamma correction should be applied. The supplied gamma table is transferred
        //     directly to the GraphicsDevice.
        //
        //   ramp:
        //     The gamma correction ramp to set.
        void SetGammaRamp(bool calibrate, GammaRamp ramp);
        //
        // Summary:
        //     Sets the specified pixel shader constant Boolean registers to an array of
        //     System.Boolean values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant Boolean register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, bool[] constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float registers to an array of System.Single
        //     values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, float[] constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant integer registers to an array of
        //     System.Int32 values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant integer register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, int[] constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float registers to a Framework.Matrix
        //     value.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, Matrix constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float registers to an array of Framework.Matrix
        //     values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, Matrix[] constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float registers to a Framework.Quaternion
        //     value.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, Quaternion constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float registers to an array of Framework.Quaternion
        //     values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, Quaternion[] constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float register to a Framework.Vector2
        //     value.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, Vector2 constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float registers to an array of Framework.Vector2
        //     values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, Vector2[] constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float register to a Framework.Vector3
        //     value.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, Vector3 constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float registers to an array of Framework.Vector3
        //     values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, Vector3[] constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float register to a Framework.Vector4
        //     value.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, Vector4 constantData);
        //
        // Summary:
        //     Sets the specified pixel shader constant float registers to an array of Framework.Vector4
        //     values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the pixel shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetPixelShaderConstant(int startRegister, Vector4[] constantData);
        //
        // Summary:
        //     Sets a new render target for this GraphicsDevice.
        //
        // Parameters:
        //   renderTargetIndex:
        //     Index of the render target. The number of render targets supported by a GraphicsDevice
        //     is contained in GraphicsDeviceCapabilities.MaxSimultaneousRenderTargets.
        //
        //   renderTarget:
        //     A new render target for the device, or null to set the device render target
        //     to the back buffer of the device.
        void SetRenderTarget(int renderTargetIndex, IRenderTarget2D renderTarget);
        //
        // Summary:
        //     Sets a new render target for this GraphicsDevice.
        //
        // Parameters:
        //   renderTargetIndex:
        //     Index of the render target. The number of render targets supported by a GraphicsDevice
        //     is contained in GraphicsDeviceCapabilities.MaxSimultaneousRenderTargets.
        //
        //   renderTarget:
        //     A new render target for the device, or null to set the device render target
        //     to the back buffer of the device.
        //
        //   faceType:
        //     The cube map face type.
        void SetRenderTarget(int renderTargetIndex, IRenderTargetCube renderTarget, CubeMapFace faceType);
        //
        // Summary:
        //     Sets the specified vertex shader constant Boolean registers to an array of
        //     System.Boolean values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant Boolean register at which
        //     to begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, bool[] constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float registers to an array of
        //     System.Single values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, float[] constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant integer registers to an array of
        //     System.Int32 values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant integer register at which
        //     to begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, int[] constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float registers to a Framework.Matrix
        //     value.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data. This value will be transposed when the vertex shader constant
        //     is set.
        void SetVertexShaderConstant(int startRegister, Matrix constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float registers to an array of
        //     Framework.Matrix values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data. This value will be transposed when the vertex shader constant
        //     is set.
        void SetVertexShaderConstant(int startRegister, Matrix[] constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float register to a Framework.Quaternion
        //     value.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, Quaternion constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float registers to an array of
        //     Framework.Quaternion values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, Quaternion[] constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float register to a Framework.Vector2
        //     value.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, Vector2 constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float registers to an array of
        //     Framework.Vector2 values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, Vector2[] constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float register to a Framework.Vector3
        //     value.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, Vector3 constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float registers to an array of
        //     Framework.Vector3 values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, Vector3[] constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float register to a Framework.Vector4
        //     value.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, Vector4 constantData);
        //
        // Summary:
        //     Sets the specified vertex shader constant float registers to an array of
        //     Framework.Vector4 values.
        //
        // Parameters:
        //   startRegister:
        //     Zero-based index of the vertex shader constant float register at which to
        //     begin setting values.
        //
        //   constantData:
        //     The constant data.
        void SetVertexShaderConstant(int startRegister, Vector4[] constantData);

        float AspectRatio { get; }
    }
}
