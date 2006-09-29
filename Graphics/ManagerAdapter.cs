using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class DeviceManager : IManager
    {
        #region IManager Members

        public DisplayMode CurrentDisplayMode(int adapter)
        {
            return Manager.Adapters[adapter].CurrentDisplayMode;
        }

        public int NumAdapters()
        {
            return Manager.Adapters.Count;
        }

        public DisplayMode[] SupportedDisplayModes(int adapter)
        {
            DisplayModeCollection collection = Manager.Adapters[adapter].SupportedDisplayModes;
            DisplayMode[] modes = new DisplayMode[collection.Count];
            int i = 0;
            foreach (DisplayMode mode in collection)
            {
                modes[i++] = mode;
            }
            return modes;
        }

        public bool CheckDepthStencilMatch(int adapter, DeviceType deviceType, Format adapterFormat, Format renderTargetFormat, DepthFormat depthStencilFormat, out int result)
        {
            return Manager.CheckDepthStencilMatch(adapter, deviceType, adapterFormat, renderTargetFormat, depthStencilFormat, out result);
        }

        public bool CheckDepthStencilMatch(int adapter, DeviceType deviceType, Format adapterFormat, Format renderTargetFormat, DepthFormat depthStencilFormat)
        {
            return Manager.CheckDepthStencilMatch(adapter, deviceType, adapterFormat, renderTargetFormat, depthStencilFormat);
        }

        public bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, DepthFormat checkFormat)
        {
            return Manager.CheckDeviceFormat(adapter, deviceType, adapterFormat, usage, resourceType, checkFormat);
        }

        public bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, Format checkFormat)
        {
            return Manager.CheckDeviceFormat(adapter, deviceType, adapterFormat, usage, resourceType, checkFormat);
        }

        public bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, DepthFormat checkFormat, out int result)
        {
            return Manager.CheckDeviceFormat(adapter, deviceType, adapterFormat, usage, resourceType, checkFormat, out result);
        }

        public bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, Format checkFormat, out int result)
        {
            return Manager.CheckDeviceFormat(adapter, deviceType, adapterFormat, usage, resourceType, checkFormat, out result);
        }

        public bool CheckDeviceFormatConversion(int adapter, DeviceType deviceType, Format sourceFormat, Format targetFormat)
        {
            return Manager.CheckDeviceFormatConversion(adapter, deviceType, sourceFormat, targetFormat);
        }

        public bool CheckDeviceFormatConversion(int adapter, DeviceType deviceType, Format sourceFormat, Format targetFormat, out int result)
        {
            return Manager.CheckDeviceFormatConversion(adapter, deviceType, sourceFormat, targetFormat, out result);
        }

        public bool CheckDeviceMultiSampleType(int adapter, DeviceType deviceType, Format surfaceFormat, bool windowed, MultiSampleType multiSampleType)
        {
            return Manager.CheckDeviceMultiSampleType(adapter, deviceType, surfaceFormat, windowed, multiSampleType);
        }

        public bool CheckDeviceMultiSampleType(int adapter, DeviceType deviceType, Format surfaceFormat, bool windowed, MultiSampleType multiSampleType, out int result, out int qualityLevels)
        {
            return Manager.CheckDeviceMultiSampleType(adapter, deviceType, surfaceFormat, windowed, multiSampleType, out result, out qualityLevels);
        }

        public bool CheckDeviceType(int adapter, DeviceType checkType, Format displayFormat, Format backBufferFormat, bool windowed)
        {
            return Manager.CheckDeviceType(adapter, checkType, displayFormat, backBufferFormat, windowed);
        }

        public bool CheckDeviceType(int adapter, DeviceType checkType, Format displayFormat, Format backBufferFormat, bool windowed, out int result)
        {
            return Manager.CheckDeviceType(adapter, checkType, displayFormat, backBufferFormat, windowed, out result);
        }

        public bool DisableD3DSpy()
        {
            return Manager.DisableD3DSpy();
        }

        public bool GenerateD3DSpyBreak()
        {
            return Manager.GenerateD3DSpyBreak();
        }

        public Caps GetDeviceCaps(int adapter, DeviceType deviceType)
        {
            return Manager.GetDeviceCaps(adapter, deviceType);
        }

        #endregion
    }
}
