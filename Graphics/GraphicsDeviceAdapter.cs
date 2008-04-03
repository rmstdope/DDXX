using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class GraphicsDeviceAdapter : IGraphicsDevice
    {
        private GraphicsDevice device;

        public GraphicsDeviceAdapter(GraphicsDevice device)
        {
            this.device = device;
            DeviceLost +=new EventHandler(this.OnDeviceLost);
            DeviceReset += new EventHandler(this.OnDeviceReset);
            DeviceResetting += new EventHandler(this.OnDeviceResetting);
            Disposing += new EventHandler(this.OnDisposing);
            ResourceCreated +=new EventHandler<ResourceCreatedEventArgs>(this.OnResourceCreated);
            ResourceDestroyed +=new EventHandler<ResourceDestroyedEventArgs>(this.OnResourceDestroyed);
        }

        public GraphicsDevice DxGraphicsDevice
        {
            get { return device; }
        }

        #region IGraphicsDevice Members

        public ClipPlaneCollection ClipPlanes
        {
            get { return device.ClipPlanes; }
        }

        public GraphicsDeviceCreationParameters CreationParameters
        {
            get { return device.CreationParameters; }
        }

        public IDepthStencilBuffer DepthStencilBuffer
        {
            get
            {
                return new DepthStencilBufferAdapter(device.DepthStencilBuffer);
            }
            set
            {
                if (value == null)
                    device.DepthStencilBuffer = null;
                else
                    device.DepthStencilBuffer = (value as DepthStencilBufferAdapter).DxDepthStencilBuffer;
            }
        }

        public DisplayMode DisplayMode
        {
            get { return device.DisplayMode; }
        }

        public int DriverLevel
        {
            get { return device.DriverLevel; }
        }

        public GraphicsDeviceCapabilities GraphicsDeviceCapabilities
        {
            get { return device.GraphicsDeviceCapabilities; }
        }

        public GraphicsDeviceStatus GraphicsDeviceStatus
        {
            get { return device.GraphicsDeviceStatus; }
        }

        public IIndexBuffer Indices
        {
            get
            {
                return new IndexBufferAdapter(device.Indices);
            }
            set
            {
                if (value == null)
                    device.Indices = null;
                else
                    device.Indices = (value as IndexBufferAdapter).DxIndexBuffer;
            }
        }

        public bool IsDisposed
        {
            get { return device.IsDisposed; }
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

        public PresentationParameters PresentationParameters
        {
            get { return device.PresentationParameters; }
        }

        public RasterStatus RasterStatus
        {
            get { return device.RasterStatus; }
        }

        public IRenderState RenderState
        {
            get { return new RenderStateAdapter(device.RenderState); }
        }

        public SamplerStateCollection SamplerStates
        {
            get { return device.SamplerStates; }
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

        public TextureCollection Textures
        {
            get { return device.Textures; }
        }

        public IVertexDeclaration VertexDeclaration
        {
            get
            {
                return new VertexDeclarationAdapter(device.VertexDeclaration);
            }
            set
            {
                device.VertexDeclaration = (value as VertexDeclarationAdapter).DxVertexDeclaration;
            }
        }

        public SamplerStateCollection VertexSamplerStates
        {
            get { return device.VertexSamplerStates; }
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

        public TextureCollection VertexTextures
        {
            get { return device.VertexTextures; }
        }

        public IVertexStreamCollection Vertices
        {
            get { return new VertexStreamCollectionAdapter(device.Vertices); }
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

        public event EventHandler DeviceLost;

        public event EventHandler DeviceReset;

        public event EventHandler DeviceResetting;

        public event EventHandler Disposing;

        public event EventHandler<ResourceCreatedEventArgs> ResourceCreated;

        public event EventHandler<ResourceDestroyedEventArgs> ResourceDestroyed;

        private void OnDeviceLost(object sender, EventArgs e)
        {
            DeviceLost(sender, e);
        }

        private void OnDeviceReset(object sender, EventArgs e)
        {
            DeviceReset(sender, e);
        }

        private void OnDeviceResetting(object sender, EventArgs e)
        {
            DeviceResetting(sender, e);
        }

        private void OnDisposing(object sender, EventArgs e)
        {
            Disposing(sender, e);
        }

        private void OnResourceCreated(object sender, ResourceCreatedEventArgs e)
        {
            ResourceCreated(sender, e);
        }

        private void OnResourceDestroyed(object sender, ResourceDestroyedEventArgs e)
        {
            ResourceDestroyed(sender, e);
        }

        public void Clear(Color color)
        {
            device.Clear(color);
        }

        public void Clear(ClearOptions options, Color color, float depth, int stencil)
        {
            device.Clear(options, color, depth, stencil);
        }

        public void Clear(ClearOptions options, Microsoft.Xna.Framework.Vector4 color, float depth, int stencil)
        {
            device.Clear(options, color, depth, stencil);
        }

        public void Clear(ClearOptions options, Color color, float depth, int stencil, Microsoft.Xna.Framework.Rectangle[] regions)
        {
            device.Clear(options, color, depth, stencil, regions);
        }

        public void Clear(ClearOptions options, Microsoft.Xna.Framework.Vector4 color, float depth, int stencil, Microsoft.Xna.Framework.Rectangle[] regions)
        {
            device.Clear(options, color, depth, stencil, regions);
        }

        public void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCount)
        {
            device.DrawIndexedPrimitives(primitiveType, baseVertex, minVertexIndex, numVertices, startIndex, primitiveCount);
        }

        public void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            device.DrawPrimitives(primitiveType, startVertex, primitiveCount);
        }

        public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount)
            where T : struct
        {
            device.DrawUserIndexedPrimitives<T>(primitiveType, vertexData, vertexOffset, numVertices, indexData, indexOffset, primitiveCount);
        }

        public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount)
            where T : struct
        {
            device.DrawUserIndexedPrimitives<T>(primitiveType, vertexData, vertexOffset, numVertices, indexData, indexOffset, primitiveCount);
        }

        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount)
            where T : struct
        {
            device.DrawUserPrimitives<T>(primitiveType, vertexData, vertexOffset, primitiveCount);
        }

        public void EvictManagedResources()
        {
            device.EvictManagedResources();
        }

        public GammaRamp GetGammaRamp()
        {
            return device.GetGammaRamp();
        }

        public bool[] GetPixelShaderBooleanConstant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderBooleanConstant(startRegister, constantCount);
        }

        public int[] GetPixelShaderInt32Constant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderInt32Constant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Matrix[] GetPixelShaderMatrixArrayConstant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderMatrixArrayConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Matrix GetPixelShaderMatrixConstant(int startRegister)
        {
            return device.GetPixelShaderMatrixConstant(startRegister);
        }

        public Microsoft.Xna.Framework.Quaternion[] GetPixelShaderQuaternionArrayConstant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderQuaternionArrayConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Quaternion GetPixelShaderQuaternionConstant(int startRegister)
        {
            return device.GetPixelShaderQuaternionConstant(startRegister);
        }

        public float[] GetPixelShaderSingleConstant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderSingleConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Vector2[] GetPixelShaderVector2ArrayConstant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderVector2ArrayConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Vector2 GetPixelShaderVector2Constant(int startRegister)
        {
            return device.GetPixelShaderVector2Constant(startRegister);
        }

        public Microsoft.Xna.Framework.Vector3[] GetPixelShaderVector3ArrayConstant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderVector3ArrayConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Vector3 GetPixelShaderVector3Constant(int startRegister)
        {
            return device.GetPixelShaderVector3Constant(startRegister);
        }

        public Microsoft.Xna.Framework.Vector4[] GetPixelShaderVector4ArrayConstant(int startRegister, int constantCount)
        {
            return device.GetPixelShaderVector4ArrayConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Vector4 GetPixelShaderVector4Constant(int startRegister)
        {
            return device.GetPixelShaderVector4Constant(startRegister);
        }

        public IRenderTarget GetRenderTarget(int renderTargetIndex)
        {
            return new RenderTargetAdapter(device.GetRenderTarget(renderTargetIndex));
        }

        public bool[] GetVertexShaderBooleanConstant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderBooleanConstant(startRegister, constantCount);
        }

        public int[] GetVertexShaderInt32Constant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderInt32Constant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Matrix[] GetVertexShaderMatrixArrayConstant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderMatrixArrayConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Matrix GetVertexShaderMatrixConstant(int startRegister)
        {
            return device.GetVertexShaderMatrixConstant(startRegister);
        }

        public Microsoft.Xna.Framework.Quaternion[] GetVertexShaderQuaternionArrayConstant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderQuaternionArrayConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Quaternion GetVertexShaderQuaternionConstant(int startRegister)
        {
            return device.GetVertexShaderQuaternionConstant(startRegister);
        }

        public float[] GetVertexShaderSingleConstant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderSingleConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Vector2[] GetVertexShaderVector2ArrayConstant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderVector2ArrayConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Vector2 GetVertexShaderVector2Constant(int startRegister)
        {
            return device.GetVertexShaderVector2Constant(startRegister);
        }

        public Microsoft.Xna.Framework.Vector3[] GetVertexShaderVector3ArrayConstant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderVector3ArrayConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Vector3 GetVertexShaderVector3Constant(int startRegister)
        {
            return device.GetVertexShaderVector3Constant(startRegister);
        }

        public Microsoft.Xna.Framework.Vector4[] GetVertexShaderVector4ArrayConstant(int startRegister, int constantCount)
        {
            return device.GetVertexShaderVector4ArrayConstant(startRegister, constantCount);
        }

        public Microsoft.Xna.Framework.Vector4 GetVertexShaderVector4Constant(int startRegister)
        {
            return device.GetVertexShaderVector4Constant(startRegister);
        }

        public void Present()
        {
            device.Present();
        }

        public void Present(IntPtr overrideWindowHandle)
        {
            device.Present(overrideWindowHandle);
        }

        public void Present(Microsoft.Xna.Framework.Rectangle? sourceRectangle, Microsoft.Xna.Framework.Rectangle? destinationRectangle, IntPtr overrideWindowHandle)
        {
            device.Present(sourceRectangle, destinationRectangle, overrideWindowHandle);
        }

        public void Reset()
        {
            device.Reset();
        }

        public void Reset(GraphicsAdapter graphicsAdapter)
        {
            device.Reset(graphicsAdapter);
        }

        public void Reset(PresentationParameters presentationParameters)
        {
            device.Reset(presentationParameters);
        }

        public void Reset(PresentationParameters presentationParameters, GraphicsAdapter graphicsAdapter)
        {
            device.Reset(presentationParameters, graphicsAdapter);
        }

        public void ResolveBackBuffer(IResolveTexture2D resolveTarget)
        {
            device.ResolveBackBuffer((resolveTarget as ResolveTexture2DAdapter).DxResolveTexture2D);
        }

        public void ResolveBackBuffer(IResolveTexture2D resolveTarget, int backBufferIndex)
        {
            device.ResolveBackBuffer((resolveTarget as ResolveTexture2DAdapter).DxResolveTexture2D, backBufferIndex);
        }

        public void SetGammaRamp(bool calibrate, GammaRamp ramp)
        {
            device.SetGammaRamp(calibrate, ramp);
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

        public void SetPixelShaderConstant(int startRegister, Microsoft.Xna.Framework.Matrix constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.Xna.Framework.Matrix[] constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.Xna.Framework.Quaternion constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.Xna.Framework.Quaternion[] constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector2 constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector2[] constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector3 constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector3[] constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector4 constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector4[] constantData)
        {
            device.SetPixelShaderConstant(startRegister, constantData);
        }

        public void SetRenderTarget(int renderTargetIndex, IRenderTarget2D renderTarget)
        {
            if (renderTarget == null)
                device.SetRenderTarget(renderTargetIndex, null);
            else
                device.SetRenderTarget(renderTargetIndex, (renderTarget as RenderTarget2DAdapter).DxRenderTarget2D);
        }

        public void SetRenderTarget(int renderTargetIndex, IRenderTargetCube renderTarget, CubeMapFace faceType)
        {
            device.SetRenderTarget(renderTargetIndex, (renderTarget as RenderTargetCubeAdapter).DxRenderTargetCube, faceType);
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

        public void SetVertexShaderConstant(int startRegister, Microsoft.Xna.Framework.Matrix constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.Xna.Framework.Matrix[] constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.Xna.Framework.Quaternion constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.Xna.Framework.Quaternion[] constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector2 constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector2[] constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector3 constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector3[] constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector4 constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.Xna.Framework.Vector4[] constantData)
        {
            device.SetVertexShaderConstant(startRegister, constantData);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            device.Dispose();
        }

        #endregion

        public float AspectRatio 
        {
            get
            {
                return (float)PresentationParameters.BackBufferWidth / (float)PresentationParameters.BackBufferHeight;
            }
        }
    }
}
