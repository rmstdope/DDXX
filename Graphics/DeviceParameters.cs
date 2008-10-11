using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class DeviceParameters : IDeviceParameters
    {
        private DeviceType deviceType;
        private SurfaceFormat backBufferFormat;
        private int backBufferWidth;
        private int backBufferHeight;
        private bool fullScreen;
        private MultiSampleType multiSampleType;
        private DepthFormat depthStencilFormat;
        private SurfaceFormat renderTargetFormat;

        public DeviceType DeviceType { get { return deviceType; } }
        public SurfaceFormat BackBufferFormat { get { return backBufferFormat; } }
        public int BackBufferWidth { get { return backBufferWidth; } }
        public int BackBufferHeight { get { return backBufferHeight; } }
        public bool FullScreen { get { return fullScreen; } }
        public MultiSampleType MultiSampleType { get { return multiSampleType; } }
        public DepthFormat DepthStencilFormat { get { return depthStencilFormat; } }
        public SurfaceFormat RenderTargetFormat { get { return renderTargetFormat; } }

        public DeviceParameters(int width, int height, bool fullScreen, bool referenceDriver, bool preferMultiSampling, bool useStencil)
        {
            this.deviceType = referenceDriver ? DeviceType.Reference : DeviceType.Hardware;
            this.renderTargetFormat = SurfaceFormat.Color;
            this.backBufferFormat = SurfaceFormat.Bgr32;
            this.backBufferWidth = width;
            this.backBufferHeight = height;
            this.fullScreen = fullScreen;
            this.multiSampleType = GetMultiSampling(preferMultiSampling, deviceType, backBufferFormat, fullScreen);
            this.depthStencilFormat = GetDepthStencilFormat(useStencil, deviceType, backBufferFormat, renderTargetFormat);
        }

        private DepthFormat GetDepthStencilFormat(bool useStencil, DeviceType deviceType, SurfaceFormat adapterFormat, SurfaceFormat renderTargetFormats)
        {
            DepthFormat[] formats = useStencil ?
                new DepthFormat[] {
                    DepthFormat.Depth24Stencil8Single,
                    DepthFormat.Depth24Stencil8,
                    DepthFormat.Depth24Stencil4,
                    DepthFormat.Depth15Stencil1
                } :
                new DepthFormat[] {
                    DepthFormat.Depth32, 
                    DepthFormat.Depth24, 
                    DepthFormat.Depth16 
                };

            foreach (DepthFormat format in formats)
                if (GraphicsAdapter.DefaultAdapter.CheckDepthStencilMatch(deviceType, adapterFormat, renderTargetFormats, format))
                    return format;
            throw new DDXXException("Could not find a valid Depth/Stencil format to use.");
        }

        private MultiSampleType GetMultiSampling(bool preferMultiSampling, DeviceType deviceType, SurfaceFormat backBufferFormat, bool fullScreen)
        {
            MultiSampleType[] types = new MultiSampleType[] { 
                    MultiSampleType.FourSamples, 
                    MultiSampleType.ThreeSamples, 
                    MultiSampleType.TwoSamples 
                };

            if (preferMultiSampling)
            {
                foreach (MultiSampleType type in types)
                    if (GraphicsAdapter.DefaultAdapter.CheckDeviceMultiSampleType(deviceType, backBufferFormat, fullScreen, type))
                        return type;
            }
            return MultiSampleType.None;
        }
    }
}
