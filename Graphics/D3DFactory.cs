using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics.Skinning;

namespace Dope.DDXX.Graphics
{
    public class D3DFactory : IGraphicsFactory
    {
        private DeviceManager manager = null;
        private SphericalHarmonicsAdapter sphericalHarmonics = null;

        #region IFactory Members

        public IDevice CreateDevice(int adapter, DeviceType deviceType, Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters)
        {
            return new DeviceAdapter(adapter, deviceType, renderWindow, behaviorFlags, presentationParameters);
        }

        public IManager Manager
        {
            get
            {
                if (manager == null)
                    manager = new DeviceManager();
                return manager;
            }
        }

        public ISphericalHarmonics SphericalHarmonics
        {
            get
            {
                if (sphericalHarmonics == null)
                    sphericalHarmonics = new SphericalHarmonicsAdapter();
                return sphericalHarmonics;
            }
        }

        public ITexture CreateTexture(IDevice device, Bitmap image, Usage usage, Pool pool)
        {
            return new TextureAdapter(new Texture(((DeviceAdapter)device).DXDevice, image, usage, pool));
        }

        public ITexture CreateTexture(IDevice device, Stream data, Usage usage, Pool pool)
        {
            return new TextureAdapter(new Texture(((DeviceAdapter)device).DXDevice, data, usage, pool));
        }

        public ITexture CreateTexture(IDevice device, int width, int height, int numLevels, Usage usage, Format format, Pool pool)
        {
            return new TextureAdapter(new Texture(((DeviceAdapter)device).DXDevice, width, height, numLevels, usage, format, pool));
        }

        public ICubeTexture CreateCubeTexture(IDevice device, int edgeLength, int levels, Usage usage, Format format, Pool pool)
        {
            return new CubeTextureAdapter(new CubeTexture(((DeviceAdapter)device).DXDevice, edgeLength, levels, usage, format, pool));
        }

        public IMesh CreateMesh(int numFaces, int numVertices, MeshFlags options, VertexElement[] declaration, IDevice device)
        {
            return new MeshAdapter(new Mesh(numFaces, numVertices, options, declaration, ((DeviceAdapter)device).DXDevice));
        }

        public IMesh CreateMesh(int numFaces, int numVertices, MeshFlags options, VertexFormats vertexFormat, IDevice device)
        {
            return new MeshAdapter(new Mesh(numFaces, numVertices, options, vertexFormat, ((DeviceAdapter)device).DXDevice));
        }

        public IMesh MeshFromFile(IDevice device, string fileName, out EffectInstance[] effectInstance)
        {
            FileStream stream = FileUtility.OpenStream(fileName);
            IMesh mesh = new MeshAdapter(Mesh.FromStream(stream, MeshFlags.Managed, ((DeviceAdapter)device).DXDevice, out effectInstance));
            stream.Close();
            return mesh;
        }

        public IMesh MeshFromFile(IDevice device, string fileName, out ExtendedMaterial[] materials)
        {
            FileStream stream = FileUtility.OpenStream(fileName);
            IMesh mesh = new MeshAdapter(Mesh.FromStream(stream, MeshFlags.Managed, ((DeviceAdapter)device).DXDevice, out materials));
            stream.Close();
            return mesh;
        }

        public IMesh CreateBoxMesh(IDevice device, float width, float height, float depth)
        {
            return MeshAdapter.Box(((DeviceAdapter)device).DXDevice, width, height, depth);
        }

        public IAnimationRootFrame SkinnedMeshFromFile(IDevice device, string fileName, AllocateHierarchy allocHierarchy)
        {
            FileStream stream = FileUtility.OpenStream(fileName);
            IAnimationRootFrame rootFrame = new AnimationRootFrameAdapter(Mesh.LoadHierarchy(stream, MeshFlags.Managed, ((DeviceAdapter)device).DXDevice, allocHierarchy, null));
            stream.Close();
            return rootFrame;
        }

        public IAnimationRootFrame LoadHierarchy(string fileName, IDevice device, AllocateHierarchy allocHierarchy, LoadUserData loadUserData)
        {
            FileStream stream = FileUtility.OpenStream(fileName);
            IAnimationRootFrame rootFrame = new AnimationRootFrameAdapter(Mesh.LoadHierarchy(stream, MeshFlags.Managed, ((DeviceAdapter)device).DXDevice, allocHierarchy, loadUserData));
            stream.Close();
            return rootFrame;
        }

        public IEffect EffectFromFile(IDevice device, string sourceDataFile, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        {
            FileStream stream = FileUtility.OpenStream(sourceDataFile);
            IEffect effect = new EffectAdapter(Effect.FromStream(((DeviceAdapter)device).DXDevice, stream, includeFile, skipConstants, flags, pool));
            stream.Close();
            return effect;
        }

        public ITexture TextureFromFile(IDevice device, string srcFile, int width, int height, int mipLevels, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            FileStream stream = FileUtility.OpenStream(srcFile);
            ITexture texture = new TextureAdapter(TextureLoader.FromStream(((DeviceAdapter)device).DXDevice, stream, width, height, mipLevels, usage, format, pool, filter, mipFilter, colorKey));
            stream.Close();
            return texture;
        }

        public ICubeTexture CubeTextureFromFile(IDevice device, string fileName)
        {
            FileStream stream = FileUtility.OpenStream(fileName);
            ICubeTexture texture = new CubeTextureAdapter(TextureLoader.FromCubeStream(((DeviceAdapter)device).DXDevice, stream));
            stream.Close();
            return texture;
        }

        public ICubeTexture CubeTextureFromFile(IDevice device, string fileName, int size, int mipLevels, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            FileStream stream = FileUtility.OpenStream(fileName);
            ICubeTexture texture = new CubeTextureAdapter(TextureLoader.FromCubeStream(((DeviceAdapter)device).DXDevice, stream, size, mipLevels, usage, format, pool, filter, mipFilter, colorKey));
            stream.Close();
            return texture;
        }

        public ISprite CreateSprite(IDevice device)
        {
            return new SpriteAdapter(((DeviceAdapter)device).DXDevice);
        }

        public IVertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, IDevice device, Usage usage, VertexFormats vertexFormat, Pool pool)
        {
            return new VertexBufferAdapter(new VertexBuffer(typeVertexType, numVerts, ((DeviceAdapter)device).DXDevice, usage, vertexFormat, pool));
        }

        public ILine CreateLine(IDevice device)
        {
            return new LineAdapter(new Line(((DeviceAdapter)device).DXDevice));
        }

        public VertexDeclaration CreateVertexDeclaration(IDevice device, VertexElement[] elements)
        {
            return new VertexDeclaration(((DeviceAdapter)device).DXDevice, elements);
        }

        public IFont CreateFont(IDevice device, FontDescription description)
        {
            return new FontAdapter(new Microsoft.DirectX.Direct3D.Font(((DeviceAdapter)device).DXDevice, description));
        }

        public IFont CreateFont(IDevice device, System.Drawing.Font font)
        {
            return new FontAdapter(new Microsoft.DirectX.Direct3D.Font(((DeviceAdapter)device).DXDevice, font));
        }

        public IFont CreateFont(IDevice device, int height, int width, FontWeight weight, int miplevels, bool italic, CharacterSet charset, Precision outputPrecision, FontQuality quality, PitchAndFamily pitchFamily, string faceName)
        {
            return new FontAdapter(new Microsoft.DirectX.Direct3D.Font(((DeviceAdapter)device).DXDevice, height, width, weight, miplevels, italic, charset, outputPrecision, quality, pitchFamily, faceName));
        }

        #endregion

    }
}
