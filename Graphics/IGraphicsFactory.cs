using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace Dope.DDXX.Graphics
{
    public interface IGraphicsFactory
    {
        IManager Manager { get; }
        ISphericalHarmonics SphericalHarmonics { get; }
        IDevice CreateDevice(int adapter, DeviceType deviceType, Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters);
        ITexture CreateTexture(IDevice device, Bitmap image, Usage usage, Pool pool);
        ITexture CreateTexture(IDevice device, Stream data, Usage usage, Pool pool);
        ITexture CreateTexture(IDevice device, int width, int height, int numLevels, Usage usage, Format format, Pool pool);
        ICubeTexture CreateCubeTexture(IDevice device, int edgeLength, int levels, Usage usage, Format format, Pool pool);
        IMesh CreateMesh(int numFaces, int numVertices, MeshFlags options, VertexElement[] declaration, IDevice device);
        IMesh MeshFromFile(IDevice device, string fileName, out EffectInstance[] effectInstance);
        IMesh MeshFromFile(IDevice device, string fileName, out ExtendedMaterial[] materials);
        IMesh CreateBoxMesh(IDevice device, float width, float height, float depth);
        IEffect EffectFromFile(IDevice device, string sourceDataFile, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool);
        ITexture TextureFromFile(IDevice device, string srcFile, int width, int height, int mipLevels, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey);
        ICubeTexture CubeTextureFromFile(IDevice device, string fileName);
        ICubeTexture CubeTextureFromFile(IDevice device, string fileName, int size, int mipLevels, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey);
        ISprite CreateSprite(IDevice device);
        IVertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, IDevice device, Usage usage, VertexFormats vertexFormat, Pool pool);
        ILine CreateLine(IDevice device);
        VertexDeclaration CreateVertexDeclaration(IDevice device, VertexElement[] elements);
    }
}
