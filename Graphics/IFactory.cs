using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace Graphics
{
    public interface IFactory
    {
        IDevice CreateDevice(int adapter, DeviceType deviceType, Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters);
        IManager CreateManager();
        ITexture CreateTexture(IDevice device, Bitmap image, Usage usage, Pool pool);
        ITexture CreateTexture(IDevice device, Stream data, Usage usage, Pool pool);
        ITexture CreateTexture(IDevice device, int width, int height, int numLevels, Usage usage, Format format, Pool pool);
        IMesh CreateBoxMesh(IDevice device, float width, float height, float depth);
        IEffect CreateEffectFromFile(IDevice device, string sourceDataFile, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool);
    }
}
