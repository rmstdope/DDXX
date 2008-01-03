using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Dope.DDXX.Graphics
{
    public class GraphicsFactory : IGraphicsFactory
    {
        private IDeviceManager deviceManager;
        private IContentManager contentManager;

        public GraphicsFactory(Game game, IServiceProvider serviceProvider)
        {
            deviceManager = new GraphicsDeviceManagerAdapter(new GraphicsDeviceManager(game));
            contentManager = new ContentManagerAdapter(new ContentManager(serviceProvider));
        }

        public IDeviceManager GraphicsDeviceManager
        {
            get { return deviceManager; }
        }

        //public ISphericalHarmonics SphericalHarmonics
        //{
        //    get
        //    {
        //        if (sphericalHarmonics == null)
        //            sphericalHarmonics = new SphericalHarmonicsAdapter();
        //        return sphericalHarmonics;
        //    }
        //}

        //public ITexture CreateTexture(IDevice device, Bitmap image, Usage usage, Pool pool)
        //{
        //    return new TextureAdapter(new Texture(((DeviceAdapter)device).DXDevice, image, usage, pool));
        //}

        //public ITexture CreateTexture(IDevice device, Stream data, Usage usage, Pool pool)
        //{
        //    return new TextureAdapter(new Texture(((DeviceAdapter)device).DXDevice, data, usage, pool));
        //}

        //public ITexture CreateTexture(IDevice device, int width, int height, int numLevels, Usage usage, Format format, Pool pool)
        //{
        //    return new TextureAdapter(new Texture(((DeviceAdapter)device).DXDevice, width, height, numLevels, usage, format, pool));
        //}

        //public ICubeTexture CreateCubeTexture(IDevice device, int edgeLength, int levels, Usage usage, Format format, Pool pool)
        //{
        //    return new CubeTextureAdapter(new CubeTexture(((DeviceAdapter)device).DXDevice, edgeLength, levels, usage, format, pool));
        //}

        //public IMesh CreateMesh(int numFaces, int numVertices, MeshFlags options, VertexElement[] declaration, IDevice device)
        //{
        //    return new MeshAdapter(new Mesh(numFaces, numVertices, options, declaration, ((DeviceAdapter)device).DXDevice));
        //}

        //public IMesh CreateMesh(int numFaces, int numVertices, MeshFlags options, VertexFormats vertexFormat, IDevice device)
        //{
        //    return new MeshAdapter(new Mesh(numFaces, numVertices, options, vertexFormat, ((DeviceAdapter)device).DXDevice));
        //}

        //public IMesh MeshFromFile(IDevice device, string fileName, out EffectInstance[] effectInstance)
        //{
        //    Stream stream = FileUtility.OpenStream(fileName);
        //    IMesh mesh = new MeshAdapter(Mesh.FromStream(stream, MeshFlags.Managed, ((DeviceAdapter)device).DXDevice, out effectInstance));
        //    stream.Close();
        //    return mesh;
        //}

        //public IMesh MeshFromFile(IDevice device, string fileName, out ExtendedMaterial[] materials)
        //{
        //    Stream stream = FileUtility.OpenStream(fileName);
        //    IMesh mesh = new MeshAdapter(Mesh.FromStream(stream, MeshFlags.Managed, ((DeviceAdapter)device).DXDevice, out materials));
        //    stream.Close();
        //    return mesh;
        //}

        //public IMesh CreateBoxMesh(IDevice device, float width, float height, float depth)
        //{
        //    return MeshAdapter.Box(((DeviceAdapter)device).DXDevice, width, height, depth);
        //}

        //public IAnimationRootFrame SkinnedMeshFromFile(IDevice device, string fileName, AllocateHierarchy allocHierarchy)
        //{
        //    Stream stream = FileUtility.OpenStream(fileName);
        //    IAnimationRootFrame rootFrame = new AnimationRootFrameAdapter(Mesh.LoadHierarchy(stream, MeshFlags.Managed, ((DeviceAdapter)device).DXDevice, allocHierarchy, null));
        //    stream.Close();
        //    return rootFrame;
        //}

        //public IAnimationRootFrame LoadHierarchy(string fileName, IDevice device, AllocateHierarchy allocHierarchy, LoadUserData loadUserData)
        //{
        //    Stream stream = FileUtility.OpenStream(fileName);
        //    IAnimationRootFrame rootFrame = new AnimationRootFrameAdapter(Mesh.LoadHierarchy(stream, MeshFlags.Managed, ((DeviceAdapter)device).DXDevice, allocHierarchy, loadUserData));
        //    stream.Close();
        //    return rootFrame;
        //}

        //public IEffect EffectFromFile(IDevice device, string sourceDataFile, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        //{
        //    Stream stream = FileUtility.OpenStream(sourceDataFile);
        //    IEffect effect = new EffectAdapter(Effect.FromStream(((DeviceAdapter)device).DXDevice, stream, includeFile, skipConstants, flags, pool));
        //    stream.Close();
        //    return effect;
        //}

        //public ICubeTexture CubeTextureFromFile(IDevice device, string fileName)
        //{
        //    Stream stream = FileUtility.OpenStream(fileName);
        //    ICubeTexture texture = new CubeTextureAdapter(TextureLoader.FromCubeStream(((DeviceAdapter)device).DXDevice, stream));
        //    stream.Close();
        //    return texture;
        //}

        //public ICubeTexture CubeTextureFromFile(IDevice device, string fileName, int size, int mipLevels, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        //{
        //    Stream stream = FileUtility.OpenStream(fileName);
        //    ICubeTexture texture = new CubeTextureAdapter(TextureLoader.FromCubeStream(((DeviceAdapter)device).DXDevice, stream, size, mipLevels, usage, format, pool, filter, mipFilter, colorKey));
        //    stream.Close();
        //    return texture;
        //}

        //public ISprite CreateSprite(IDevice device)
        //{
        //    return new SpriteAdapter(((DeviceAdapter)device).DXDevice);
        //}

        //public IVertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, IDevice device, Usage usage, VertexFormats vertexFormat, Pool pool)
        //{
        //    return new VertexBufferAdapter(new VertexBuffer(typeVertexType, numVerts, ((DeviceAdapter)device).DXDevice, usage, vertexFormat, pool));
        //}

        //public ILine CreateLine(IDevice device)
        //{
        //    return new LineAdapter(new Line(((DeviceAdapter)device).DXDevice));
        //}

        //public VertexDeclaration CreateVertexDeclaration(IDevice device, VertexElement[] elements)
        //{
        //    return new VertexDeclaration(((DeviceAdapter)device).DXDevice, elements);
        //}

        //public IFont CreateFont(IDevice device, FontDescription description)
        //{
        //    return new FontAdapter(new Microsoft.DirectX.Direct3D.Font(((DeviceAdapter)device).DXDevice, description));
        //}

        //public IFont CreateFont(IDevice device, System.Drawing.Font font)
        //{
        //    return new FontAdapter(new Microsoft.DirectX.Direct3D.Font(((DeviceAdapter)device).DXDevice, font));
        //}

        //public IFont CreateFont(IDevice device, int height, int width, FontWeight weight, int miplevels, bool italic, CharacterSet charset, Precision outputPrecision, FontQuality quality, PitchAndFamily pitchFamily, string faceName)
        //{
        //    return new FontAdapter(new Microsoft.DirectX.Direct3D.Font(((DeviceAdapter)device).DXDevice, height, width, weight, miplevels, italic, charset, outputPrecision, quality, pitchFamily, faceName));
        //}

        private GraphicsDevice GraphicsDevice
        {
            get { return (deviceManager.GraphicsDevice as GraphicsDeviceAdapter).DxGraphicsDevice; }
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, int numLevels,
            SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality)
        {
            return new RenderTarget2DAdapter(new RenderTarget2D(GraphicsDevice, width, height, numLevels, 
                format, multiSampleType, multiSampleQuality, RenderTargetUsage.PreserveContents));
        }

        public ITexture2D CreateTexture2D(int width, int height, int numLevels,
            TextureUsage usage, SurfaceFormat format)
        {
            return new Texture2DAdapter(new Texture2D(GraphicsDevice, width, height, numLevels, usage, format));
        }

        public IDepthStencilBuffer CreateDepthStencilBuffer(int width, int height, DepthFormat format, MultiSampleType multiSampleType)
        {
            return new DepthStencilBufferAdapter(new DepthStencilBuffer(GraphicsDevice, width, height, format, multiSampleType, 0));
        }

        public ITexture2D Texture2DFromFile(string srcFile)
        {
            return new Texture2DAdapter(contentManager.Load<Texture2D>(srcFile));
        }

        public ITextureCube TextureCubeFromFile(string srcFile)
        {
            return new TextureCubeAdapter(contentManager.Load<TextureCube>(srcFile));
        }

        public IVertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, BufferUsage usage)
        {
            return new VertexBufferAdapter(new VertexBuffer(GraphicsDevice, typeVertexType, numVerts, usage));
        }

        public ISpriteBatch CreateSpriteBatch()
        {
            return new SpriteBatchAdapter(new SpriteBatch(GraphicsDevice));
        }

        public IEffect EffectFromFile(string filename)
        {
            Effect effect = contentManager.Load<Effect>(filename);
            return new EffectAdapter(effect.Clone(GraphicsDevice));
        }

        public ISpriteFont SpriteFontFromFile(string name)
        {
            return new SpriteFontAdapter(contentManager.Load<SpriteFont>(name));
        }

        public IModel ModelFromFile(string name)
        {
            return new ModelAdapter(contentManager.Load<Model>(name));
        }


        public IIndexBuffer CreateIndexBuffer(Type indexType, int elementCount, BufferUsage usage)
        {
            return new IndexBufferAdapter(new IndexBuffer(GraphicsDevice, indexType, elementCount, usage));
        }

        public IVertexDeclaration CreateVertexDeclaration(VertexElement[] elements)
        {
            return new VertexDeclarationAdapter(new VertexDeclaration(GraphicsDevice, elements));
        }

    }
}
