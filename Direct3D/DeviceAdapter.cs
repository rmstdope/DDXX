using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using Utility;

namespace Direct3D
{
    class DeviceAdapter : IDevice
    {
        Device device;

        public DeviceAdapter(int adapter, DeviceType deviceType, Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters)
        {
            try
            {   
                device = new Device(adapter, deviceType, renderWindow, behaviorFlags, presentationParameters);
            }
            catch (InvalidCallException exception)
            {
                throw new DDXXException(exception.Message);
            }
        }

        #region IDevice Members

        public int AvailableTextureMemory
        {
            get { return device.AvailableTextureMemory; }
        }

        public ClipPlanes ClipPlanes
        {
            get { return device.ClipPlanes; }
        }

        public ClipStatus ClipStatus
        {
            get
            {
                return device.ClipStatus;
            }
            set
            {
                device.ClipStatus = value;
            }
        }

        public DeviceCreationParameters CreationParameters
        {
            get { return device.CreationParameters; }
        }

        public int CurrentTexturePalette
        {
            get
            {
                return device.CurrentTexturePalette;
            }
            set
            {
                device.CurrentTexturePalette = value;
            }
        }

        public Surface DepthStencilSurface
        {
            get
            {
                return device.DepthStencilSurface;
            }
            set
            {
                device.DepthStencilSurface = value;
            }
        }

        public Caps DeviceCaps
        {
            get { return device.DeviceCaps; }
        }

        public DisplayMode DisplayMode
        {
            get { return device.DisplayMode; }
        }

        public bool Disposed
        {
            get { return device.Disposed; }
        }

        public IndexBuffer Indices
        {
            get
            {
                return device.Indices;
            }
            set
            {
                device.Indices = value;
            }
        }

        public LightsCollection Lights
        {
            get { return device.Lights; }
        }

        public Material Material
        {
            get
            {
                return device.Material;
            }
            set
            {
                device.Material = value;
            }
        }

        public float NPatchMode
        {
            get
            {
                return device.NPatchMode;
            }
            set
            {
                device.NPatchMode = value;
            }
        }

        public int NumberOfSwapChains
        {
            get { return device.NumberOfSwapChains; } 
        }

        public PixelShader PixelShader
        {
            get
            {
                return device.PixelShader;
            }
            set
            {
                device.PixelShader = value;
            }
        }

        public PresentParameters PresentationParameters
        {
            get { return device.PresentationParameters; }
        }

        public RasterStatus RasterStatus
        {
            get { return device.RasterStatus; }
        }

        public RenderStateManager RenderState
        {
            get { return device.RenderState; }
        }

        public SamplerStateManagerCollection SamplerState
        {
            get { return device.SamplerState; }
        }

        public Rectangle ScissorRectangle
        {
            get
            {
                return device.ScissorRectangle;
            }
            set
            {
                device.ScissorRectangle = value;
            }
        }

        public bool SoftwareVertexProcessing
        {
            get
            {
                return device.SoftwareVertexProcessing;
            }
            set
            {
                device.SoftwareVertexProcessing = value;
            }
        }

        public TextureStateManagerCollection TextureState
        {
            get { return device.TextureState; }
        }

        public Transforms Transform
        {
            get { return device.Transform; }
        }

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return device.VertexDeclaration;
            }
            set
            {
                device.VertexDeclaration = value;
            }
        }

        public VertexFormats VertexFormat
        {
            get
            {
                return device.VertexFormat;
            }
            set
            {
                device.VertexFormat = value;
            }
        }

        public VertexShader VertexShader
        {
            get
            {
                return device.VertexShader;
            }
            set
            {
                device.VertexShader = value;
            }
        }

        public Viewport Viewport
        {
            get
            {
                return device.Viewport;
            }
            set
            {
                device.Viewport = value;
            }
        }

        public void BeginScene()
        {
            device.BeginScene();
        }

        public void BeginStateBlock()
        {
            device.BeginStateBlock();
        }

        public bool CheckCooperativeLevel()
        {
            return device.CheckCooperativeLevel();
        }

        public bool CheckCooperativeLevel(out int result)
        {
            return device.CheckCooperativeLevel(out result);
        }

        public void Clear(ClearFlags flags, Color color, float zdepth, int stencil)
        {
            device.Clear(flags, color, zdepth, stencil);
        }

        public void Clear(ClearFlags flags, int color, float zdepth, int stencil)
        {
            device.Clear(flags, color, zdepth, stencil); 
        }

        public void Clear(ClearFlags flags, Color color, float zdepth, int stencil, Rectangle[] rect)
        {
            device.Clear(flags, color, zdepth, stencil, rect);
        }

        public void Clear(ClearFlags flags, int color, float zdepth, int stencil, Rectangle[] regions)
        {
            device.Clear(flags, color, zdepth, stencil, regions);
        }

        public void ColorFill(Surface surface, Rectangle rect, Color color)
        {
            device.ColorFill(surface, rect, color);
        }

        public void ColorFill(Surface surface, Rectangle rect, int color)
        {
            device.ColorFill(surface, rect, color);
        }

        public Surface CreateDepthStencilSurface(int width, int height, DepthFormat format, MultiSampleType multiSample, int multiSampleQuality, bool discard)
        {
            return device.CreateDepthStencilSurface(width, height, format, multiSample, multiSampleQuality, discard);
        }

        public Surface CreateOffscreenPlainSurface(int width, int height, Format format, Pool pool)
        {
            return device.CreateOffscreenPlainSurface(width, height, format, pool);
        }

        public Surface CreateRenderTarget(int width, int height, Format format, MultiSampleType multiSample, int multiSampleQuality, bool lockable)
        {
            return device.CreateRenderTarget(width, height, format, multiSample, multiSampleQuality, lockable);
        }

        public void DeletePatch(int handle)
        {
            device.DeletePatch(handle);
        }

        public void Dispose()
        {
            device.Dispose();
        }

        public void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primCount)
        {
            device.DrawIndexedPrimitives(primitiveType, baseVertex, minVertexIndex, numVertices, startIndex, primCount);
        }

        public void DrawIndexedUserPrimitives(PrimitiveType primitiveType, int minVertexIndex, int numVertexIndices, int primitiveCount, object indexData, bool sixteenBitIndices, object vertexStreamZeroData)
        {
            device.DrawIndexedUserPrimitives(primitiveType, minVertexIndex, numVertexIndices, primitiveCount, indexData, sixteenBitIndices, vertexStreamZeroData);
        }

        public void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            device.DrawPrimitives(primitiveType, startVertex, primitiveCount);
        }

        public void DrawRectanglePatch(int handle, float[] numSegs)
        {
            device.DrawRectanglePatch(handle, numSegs);
        }

        public void DrawRectanglePatch(int handle, Plane numSegs)
        {
            device.DrawRectanglePatch(handle, numSegs);
        }

        public void DrawRectanglePatch(int handle, float[] numSegs, RectanglePatchInformation rectPatchInformation)
        {
            device.DrawRectanglePatch(handle, numSegs, rectPatchInformation);
        }

        public void DrawRectanglePatch(int handle, Plane numSegs, RectanglePatchInformation rectPatchInformation)
        {
            device.DrawRectanglePatch(handle, numSegs, rectPatchInformation);
        }

        public void DrawTrianglePatch(int handle, float[] numSegs)
        {
            device.DrawTrianglePatch(handle, numSegs);
        }

        public void DrawTrianglePatch(int handle, Plane numSegs)
        {
            device.DrawTrianglePatch(handle, numSegs);
        }

        public void DrawTrianglePatch(int handle, float[] numSegs, TrianglePatchInformation triPatchInformation)
        {
            device.DrawTrianglePatch(handle, numSegs, triPatchInformation);
        }

        public void DrawTrianglePatch(int handle, Plane numSegs, TrianglePatchInformation triPatchInformation)
        {
            device.DrawTrianglePatch(handle, numSegs, triPatchInformation);
        }

        public void DrawUserPrimitives(PrimitiveType primitiveType, int primitiveCount, object vertexStreamZeroData)
        {
            device.DrawUserPrimitives(primitiveType, primitiveCount, vertexStreamZeroData);
        }

        public void EndScene()
        {
            device.EndScene();
        }

        public StateBlock EndStateBlock()
        {
            return device.EndStateBlock();
        }

        public void EvictManagedResources()
        {
            device.EvictManagedResources();
        }

        public Surface GetBackBuffer(int swapChain, int backBuffer, BackBufferType backBufferType)
        {
            return device.GetBackBuffer(swapChain, backBuffer, backBufferType);
        }

        public CubeTexture GetCubeTexture(int stage)
        {
            return GetCubeTexture(stage);
        }

        public void GetFrontBufferData(int swapChain, Surface buffer)
        {
            device.GetFrontBufferData(swapChain, buffer);
        }

        public GammaRamp GetGammaRamp(int swapChain)
        {
            return device.GetGammaRamp(swapChain);
        }

        public PaletteEntry[] GetPaletteEntries(int paletteNumber)
        {
            return device.GetPaletteEntries(paletteNumber);
        }

        public bool[] GetPixelShaderBooleanConstant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderBooleanConstant(startRegister, constantCount);
        }

        public int[] GetPixelShaderInt32Constant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderInt32Constant(startRegister, constantCount);
        }

        public float[] GetPixelShaderSingleConstant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderSingleConstant(startRegister, constantCount);
        }

        public RasterStatus GetRasterStatus(int swapChain)
        {
            return device.GetRasterStatus(swapChain);
        }

        public bool GetRenderStateBoolean(RenderStates state)
        {
            return device.GetRenderStateBoolean(state);
        }

        public int GetRenderStateInt32(RenderStates state)
        {
            return device.GetRenderStateInt32(state);
        }

        public float GetRenderStateSingle(RenderStates state)
        {
            return device.GetRenderStateSingle(state);
        }

        public Surface GetRenderTarget(int renderTargetIndex)
        {
            return device.GetRenderTarget(renderTargetIndex);
        }

        public void GetRenderTargetData(Surface renderTarget, Surface destSurface)
        {
            device.GetRenderTargetData(renderTarget, destSurface);
        }

        public bool GetSamplerStageStateBoolean(int stage, SamplerStageStates state)
        {
            return device.GetSamplerStageStateBoolean(stage, state);
        }

        public int GetSamplerStageStateInt32(int stage, SamplerStageStates state)
        {
            return device.GetSamplerStageStateInt32(stage, state);
        }

        public float GetSamplerStageStateSingle(int stage, SamplerStageStates state)
        {
            return device.GetSamplerStageStateSingle(stage, state);
        }

        public VertexBuffer GetStreamSource(int streamNumber, out int offsetInBytes, out int stride)
        {
            return device.GetStreamSource(streamNumber, out offsetInBytes, out stride);
        }

        public int GetStreamSourceFrequency(int streamNumber)
        {
            return device.GetStreamSourceFrequency(streamNumber);
        }

        public SwapChain GetSwapChain(int swapChain)
        {
            return device.GetSwapChain(swapChain);
        }

        public Texture GetTexture(int stage)
        {
            return device.GetTexture(stage);
        }

        public bool GetTextureStageStateBoolean(int stage, TextureStageStates state)
        {
            return device.GetTextureStageStateBoolean(stage, state);
        }

        public int GetTextureStageStateInt32(int stage, TextureStageStates state)
        {
            return device.GetTextureStageStateInt32(stage, state);
        }

        public float GetTextureStageStateSingle(int stage, TextureStageStates state)
        {
            return device.GetTextureStageStateSingle(stage, state);
        }

        public Matrix GetTransform(TransformType state)
        {
            return device.GetTransform(state);
        }

        public bool[] GetVertexShaderBooleanConstant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderBooleanConstant(startRegister, constantCount);
        }

        public int[] GetVertexShaderInt32Constant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderInt32Constant(startRegister, constantCount);
        }

        public float[] GetVertexShaderSingleConstant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderSingleConstant(startRegister, constantCount);
        }

        public VolumeTexture GetVolumeTexture(int stage)
        {
            return device.GetVolumeTexture(stage);
        }

        public void MultiplyTransform(TransformType state, Matrix matrix)
        {
            device.MultiplyTransform(state, matrix);
        }

        public void Present()
        {
            device.Present();
        }

        public void Present(Control overrideWindow)
        {
            device.Present(overrideWindow);
        }

        public void Present(IntPtr overrideWindowHandle)
        {
            device.Present(overrideWindowHandle);
        }

        public void Present(Rectangle rectPresent, bool sourceRectangle)
        {
            device.Present(rectPresent, sourceRectangle);
        }

        public void Present(Rectangle rectPresent, Control overrideWindow, bool sourceRectangle)
        {
            device.Present(rectPresent, overrideWindow, sourceRectangle);
        }

        public void Present(Rectangle rectPresent, IntPtr overrideWindowHandle, bool sourceRectangle)
        {
            device.Present(rectPresent, overrideWindowHandle, sourceRectangle);
        }

        public void Present(Rectangle sourceRectangle, Rectangle destRectangle, Control overrideWindow)
        {
            device.Present(sourceRectangle, destRectangle, overrideWindow);
        }

        public void Present(Rectangle sourceRectangle, Rectangle destRectangle, IntPtr overrideWindowHandle)
        {
            device.Present(sourceRectangle, destRectangle, overrideWindowHandle);
        }

        public void ProcessVertices(int srcStartIndex, int destIndex, int vertexCount, VertexBuffer destBuffer, VertexDeclaration vertexDeclaration)
        {
            device.ProcessVertices(srcStartIndex, destIndex, vertexCount, destBuffer, vertexDeclaration);
        }

        public void ProcessVertices(int srcStartIndex, int destIndex, int vertexCount, VertexBuffer destBuffer, VertexDeclaration vertexDeclaration, bool copyData)
        {
            device.ProcessVertices(srcStartIndex, destIndex, vertexCount, destBuffer, vertexDeclaration, copyData);
        }

        public void Reset(params PresentParameters[] presentationParameters)
        {
            device.Reset(presentationParameters);
        }

        public void SetCursor(Cursor cursor, bool addWaterMark)
        {
            device.SetCursor(cursor, addWaterMark);
        }

        public void SetCursorPosition(int positionX, int positionY, bool updateImmediate)
        {
            device.SetCursorPosition(positionX, positionY, updateImmediate);
        }

        public void SetCursorProperties(int hotSpotX, int hotSpotY, Surface cursorBitmap)
        {
            device.SetCursorProperties(hotSpotX, hotSpotY, cursorBitmap);
        }

        public void SetDialogBoxesEnabled(bool value)
        {
            device.SetDialogBoxesEnabled(value);
        }

        public void SetGammaRamp(int swapChain, bool calibrate, GammaRamp ramp)
        {
            device.SetGammaRamp(swapChain, calibrate, ramp);
        }

        public void SetPaletteEntries(int paletteNumber, PaletteEntry[] entries)
        {
            device.SetPaletteEntries(paletteNumber, entries);
        }

        public void SetPixelShaderConstant(int startRegister, bool[] constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, float[] constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, int[] constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Matrix constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Matrix[] constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Vector4 constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Vector4[] constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstantBoolean(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            device.SetPixelShaderConstantBoolean(startRegister, constantData, numberRegisters);
        }

        public void SetPixelShaderConstantInt32(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            device.SetPixelShaderConstantInt32(startRegister, constantData, numberRegisters);
        }

        public void SetPixelShaderConstantSingle(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            device.SetPixelShaderConstantSingle(startRegister, constantData, numberRegisters);
        }

        public void SetRenderState(RenderStates state, bool value)
        {
            device.SetRenderState(state, value);
        }

        public void SetRenderState(RenderStates state, float value)
        {
            device.SetRenderState(state, value);
        }

        public void SetRenderState(RenderStates state, int value)
        {
            device.SetRenderState(state, value);
        }

        public void SetRenderTarget(int renderTargetIndex, Surface renderTarget)
        {
            device.SetRenderTarget(renderTargetIndex, renderTarget);
        }

        public void SetSamplerState(int stage, SamplerStageStates state, bool value)
        {
            device.SetSamplerState(stage, state, value);
        }

        public void SetSamplerState(int stage, SamplerStageStates state, float value)
        {
            device.SetSamplerState(stage, state, value);
        }

        public void SetSamplerState(int stage, SamplerStageStates state, int value)
        {
            device.SetSamplerState(stage, state, value);
        }

        public void SetStreamSource(int streamNumber, VertexBuffer streamData, int offsetInBytes)
        {
            device.SetStreamSource(streamNumber, streamData, offsetInBytes);
        }

        public void SetStreamSource(int streamNumber, VertexBuffer streamData, int offsetInBytes, int stride)
        {
            device.SetStreamSource(streamNumber, streamData, offsetInBytes, stride);
        }

        public void SetStreamSourceFrequency(int streamNumber, int divider)
        {
            device.SetStreamSourceFrequency(streamNumber, divider);
        }

        public void SetTexture(int stage, BaseTexture texture)
        {
            device.SetTexture(stage, texture);
        }

        public void SetTextureStageState(int stage, TextureStageStates state, bool value)
        {
            device.SetTextureStageState(stage, state, value);
        }

        public void SetTextureStageState(int stage, TextureStageStates state, float value)
        {
            device.SetTextureStageState(stage, state, value);
        }

        public void SetTextureStageState(int stage, TextureStageStates state, int value)
        {
            device.SetTextureStageState(stage, state, value);
        }

        public void SetTransform(TransformType state, Matrix matrix)
        {
            device.SetTransform(state, matrix);
        }

        public void SetVertexShaderConstant(int startRegister, bool[] constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, float[] constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, int[] constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Matrix constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Matrix[] constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Vector4 constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Vector4[] constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstantBoolean(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            device.SetVertexShaderConstantBoolean(startRegister, constantData, numberRegisters);
        }

        public void SetVertexShaderConstantInt32(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            device.SetVertexShaderConstantInt32(startRegister, constantData, numberRegisters);
        }

        public void SetVertexShaderConstantSingle(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            device.SetVertexShaderConstantSingle(startRegister, constantData, numberRegisters);
        }

        public bool ShowCursor(bool canShow)
        {
            return device.ShowCursor(canShow);
        }

        public void StretchRectangle(Surface sourceSurface, Rectangle sourceRectangle, Surface destSurface, Rectangle destRectangle, TextureFilter filter)
        {
            device.StretchRectangle(sourceSurface, sourceRectangle, destSurface, destRectangle, filter);
        }

        public void TestCooperativeLevel()
        {
            device.TestCooperativeLevel();
        }

        public void UpdateSurface(Surface sourceSurface, Surface destinationSurface)
        {
            device.UpdateSurface(sourceSurface, destinationSurface);
        }

        public void UpdateSurface(Surface sourceSurface, Rectangle sourceRect, Surface destinationSurface)
        {
            device.UpdateSurface(sourceSurface, sourceRect, destinationSurface);
        }

        public void UpdateSurface(Surface sourceSurface, Surface destinationSurface, Point destPoint)
        {
            device.UpdateSurface(sourceSurface, destinationSurface, destPoint);
        }

        public void UpdateSurface(Surface sourceSurface, Rectangle sourceRect, Surface destinationSurface, Point destPoint)
        {
            device.UpdateSurface(sourceSurface, sourceRect, destinationSurface, destPoint);
        }

        public void UpdateTexture(BaseTexture sourceTexture, BaseTexture destinationTexture)
        {
            device.UpdateTexture(sourceTexture, destinationTexture);
        }

        public ValidateDeviceParams ValidateDevice()
        {
            return device.ValidateDevice();
        }

        #endregion

    }
}
