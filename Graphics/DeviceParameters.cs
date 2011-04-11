using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class DeviceParameters : IDeviceParameters
    {
        private GraphicsProfile graphicsProfile;
        private bool referenceDevice;
        private SurfaceFormat backBufferFormat;
        private int backBufferWidth;
        private int backBufferHeight;
        private bool fullScreen;
        private int multiSampleCount;
        private DepthFormat depthStencilFormat;
        private SurfaceFormat renderTargetFormat;

        public GraphicsProfile GraphicsProfile { get { return graphicsProfile; } }
        public bool ReferenceDevice { get { return referenceDevice; } }
        public SurfaceFormat BackBufferFormat { get { return backBufferFormat; } }
        public int BackBufferWidth { get { return backBufferWidth; } }
        public int BackBufferHeight { get { return backBufferHeight; } }
        public bool FullScreen { get { return fullScreen; } }
        public int MultiSampleCount { get { return multiSampleCount; } }
        public DepthFormat DepthStencilFormat { get { return depthStencilFormat; } }
        public SurfaceFormat RenderTargetFormat { get { return renderTargetFormat; } }

        public DeviceParameters(int width, int height, bool fullScreen, bool hiDef, bool preferMultiSampling, bool useStencil)
        {
            this.graphicsProfile = hiDef? GraphicsProfile.HiDef : GraphicsProfile.Reach;
            this.renderTargetFormat = SurfaceFormat.Color;
            this.backBufferFormat = SurfaceFormat.Color;
            this.backBufferWidth = width;
            this.backBufferHeight = height;
            this.fullScreen = fullScreen;
            //this.multiSampleType = GetMultiSampling(preferMultiSampling, graphicsProfile, backBufferFormat, fullScreen);
            this.depthStencilFormat = GetDepthStencilFormat(useStencil, graphicsProfile, backBufferFormat, renderTargetFormat);
        }

        private DepthFormat GetDepthStencilFormat(bool useStencil, GraphicsProfile graphicsProfile, SurfaceFormat adapterFormat, SurfaceFormat renderTargetFormats)
        {
            DepthFormat[] formats = useStencil ?
                new DepthFormat[] {
                    DepthFormat.Depth24Stencil8,
                } :
                new DepthFormat[] {
                    DepthFormat.Depth24, 
                    DepthFormat.Depth16 
                };
            return formats[0];
            //foreach (DepthFormat format in formats)
            //    if (GraphicsAdapter.DefaultAdapter.CheckDepthStencilMatch(graphicsProfile, adapterFormat, renderTargetFormats, format))
            //        return format;
            //throw new DDXXException("Could not find a valid Depth/Stencil format to use.");
        }

        //private MultiSampleType GetMultiSampling(bool preferMultiSampling, graphicsProfile graphicsProfile, SurfaceFormat backBufferFormat, bool fullScreen)
        //{
        //    MultiSampleType[] types = new MultiSampleType[] { 
        //            MultiSampleType.FourSamples, 
        //            MultiSampleType.ThreeSamples, 
        //            MultiSampleType.TwoSamples 
        //        };

        //    if (preferMultiSampling)
        //    {
        //        foreach (MultiSampleType type in types)
        //            if (GraphicsAdapter.DefaultAdapter.CheckDeviceMultiSampleType(graphicsProfile, backBufferFormat, fullScreen, type))
        //                return type;
        //    }
        //    return MultiSampleType.None;
        //}
    }
}
