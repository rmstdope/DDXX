using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface IManager
    {
        DisplayMode CurrentDisplayMode(int adapter);
        int NumAdapters();
        DisplayMode[] SupportedDisplayModes(int adapter);

        // Summary:
        //     Determines whether a depth stencil format is compatible with a render target
        //     format in a particular display mode.
        //
        // Parameters:
        //   adapter:
        //     Ordinal number that denotes the display adapter to query. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter.
        //
        //   deviceType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration that identifies
        //     the device type.
        //
        //   adapterFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the display mode into which the adapter will be placed.
        //
        //   renderTargetFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the render-target surface to be tested.
        //
        //   depthStencilFormat:
        //     Member of the Microsoft.DirectX.Direct3D.DepthFormat enumeration that identifies
        //     the format of the depth stencil surface to be tested.
        //
        // Returns:
        //     Value that is true if the method succeeds; false if it fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDepthStencilMatch()
        //     parameter for the HRESULT code returned. If a depth stencil format is not
        //     compatible with the render target in the display mode, Microsoft.DirectX.Direct3D.Manager.CheckDepthStencilMatch()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable.
        bool CheckDepthStencilMatch(int adapter, DeviceType deviceType, Format adapterFormat, Format renderTargetFormat, DepthFormat depthStencilFormat);
        //
        // Summary:
        //     Determines whether a depth stencil format is compatible with a render target
        //     format in a particular display mode.
        //
        // Parameters:
        //   adapter:
        //     Ordinal number that denotes the display adapter to query. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter.
        //
        //   deviceType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration that identifies
        //     the device type.
        //
        //   adapterFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the display mode into which the adapter will be placed.
        //
        //   renderTargetFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the render-target surface to be tested.
        //
        //   depthStencilFormat:
        //     Member of the Microsoft.DirectX.Direct3D.DepthFormat enumeration that identifies
        //     the format of the depth stencil surface to be tested.
        //
        //   result:
        //     HRESULT code passed back from the method.
        //
        // Returns:
        //     Value that is true if the method succeeds; false if it fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDepthStencilMatch()
        //     parameter for the HRESULT code returned. If a depth stencil format is not
        //     compatible with the render target in the display mode, Microsoft.DirectX.Direct3D.Manager.CheckDepthStencilMatch()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable.
        bool CheckDepthStencilMatch(int adapter, DeviceType deviceType, Format adapterFormat, Format renderTargetFormat, DepthFormat depthStencilFormat, out int result);
        //
        // Summary:
        //     Determines whether a surface format is available as a specified resource
        //     type and can be used as a texture, depth stencil buffer, render target, or
        //     any combination of the three, on a device representing the current adapter.
        //
        // Parameters:
        //   adapter:
        //     Ordinal number that denotes the display adapter to query. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter. This method returns Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     if this value equals or exceeds the number of display adapters in the system.
        //
        //   deviceType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration that identifies
        //     the device type.
        //
        //   adapterFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the display mode into which the adapter will be placed.
        //
        //   usage:
        //     Requested usage options for the surface. Usage options are any combination
        //     of Microsoft.DirectX.Direct3D.Usage enumeration values (only a subset of
        //     the Microsoft.DirectX.Direct3D.Usage values are valid for Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()).
        //     For more information, see Microsoft.DirectX.Direct3D.Usage.
        //
        //   resourceType:
        //     A Microsoft.DirectX.Direct3D.ResourceType requested for use with the queried
        //     format.
        //
        //   checkFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the surfaces that can be used, as defined by Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat().
        //
        // Returns:
        //     Value that is true if the method succeeds; false if it fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     parameter for the HRESULT code returned. If the format is not acceptable
        //     to the device for this usage, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable.
        //     If Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat() equals or exceeds
        //     the number of display adapters in the system, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall.
        bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, DepthFormat checkFormat);
        //
        // Summary:
        //     Determines whether a surface format is available as a specified resource
        //     type and can be used as a texture, depth stencil buffer, render target, or
        //     any combination of the three, on a device representing the current adapter.
        //
        // Parameters:
        //   adapter:
        //     Ordinal number that denotes the display adapter to query. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter. This method returns Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     if this value equals or exceeds the number of display adapters in the system.
        //
        //   deviceType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration that identifies
        //     the device type.
        //
        //   adapterFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the display mode into which the adapter will be placed.
        //
        //   usage:
        //     Requested usage options for the surface. Usage options are any combination
        //     of Microsoft.DirectX.Direct3D.Usage enumeration values (only a subset of
        //     the Microsoft.DirectX.Direct3D.Usage values are valid for Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()).
        //     For more information, see Microsoft.DirectX.Direct3D.Usage.
        //
        //   resourceType:
        //     A Microsoft.DirectX.Direct3D.ResourceType requested for use with the queried
        //     format.
        //
        //   checkFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the surfaces that can be used, as defined by Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat().
        //
        // Returns:
        //     Value that is true if the method succeeds; false if it fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     parameter for the HRESULT code returned. If the format is not acceptable
        //     to the device for this usage, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable.
        //     If Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat() equals or exceeds
        //     the number of display adapters in the system, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall.
        bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, Format checkFormat);
        //
        // Summary:
        //     Determines whether a surface format is available as a specified resource
        //     type and can be used as a texture, depth stencil buffer, render target, or
        //     any combination of the three, on a device representing the current adapter.
        //
        // Parameters:
        //   adapter:
        //     Ordinal number that denotes the display adapter to query. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter. This method returns Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     if this value equals or exceeds the number of display adapters in the system.
        //
        //   deviceType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration that identifies
        //     the device type.
        //
        //   adapterFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the display mode into which the adapter will be placed.
        //
        //   usage:
        //     Requested usage options for the surface. Usage options are any combination
        //     of Microsoft.DirectX.Direct3D.Usage enumeration values (only a subset of
        //     the Microsoft.DirectX.Direct3D.Usage values are valid for Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()).
        //     For more information, see Microsoft.DirectX.Direct3D.Usage.
        //
        //   resourceType:
        //     A Microsoft.DirectX.Direct3D.ResourceType requested for use with the queried
        //     format.
        //
        //   checkFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the surfaces that can be used, as defined by Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat().
        //
        //   result:
        //     HRESULT code passed back from the method.
        //
        // Returns:
        //     Value that is true if the method succeeds; false if it fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     parameter for the HRESULT code returned. If the format is not acceptable
        //     to the device for this usage, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable.
        //     If Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat() equals or exceeds
        //     the number of display adapters in the system, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall.
        bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, DepthFormat checkFormat, out int result);
        //
        // Summary:
        //     Determines whether a surface format is available as a specified resource
        //     type and can be used as a texture, depth stencil buffer, render target, or
        //     any combination of the three, on a device representing the current adapter.
        //
        // Parameters:
        //   adapter:
        //     Ordinal number that denotes the display adapter to query. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter. This method returns Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     if this value equals or exceeds the number of display adapters in the system.
        //
        //   deviceType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration that identifies
        //     the device type.
        //
        //   adapterFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the display mode into which the adapter will be placed.
        //
        //   usage:
        //     Requested usage options for the surface. Usage options are any combination
        //     of Microsoft.DirectX.Direct3D.Usage enumeration values (only a subset of
        //     the Microsoft.DirectX.Direct3D.Usage values are valid for Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()).
        //     For more information, see Microsoft.DirectX.Direct3D.Usage.
        //
        //   resourceType:
        //     A Microsoft.DirectX.Direct3D.ResourceType requested for use with the queried
        //     format.
        //
        //   checkFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that identifies
        //     the format of the surfaces that can be used, as defined by Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat().
        //
        //   result:
        //     HRESULT code passed back from the method.
        //
        // Returns:
        //     Value that is true if the method succeeds; false if it fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     parameter for the HRESULT code returned. If the format is not acceptable
        //     to the device for this usage, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable.
        //     If Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat() equals or exceeds
        //     the number of display adapters in the system, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormat()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall.
        bool CheckDeviceFormat(int adapter, DeviceType deviceType, Format adapterFormat, Usage usage, ResourceType resourceType, Format checkFormat, out int result);
        //
        // Summary:
        //     Tests a device to determine whether it supports conversion from one display
        //     format to another.
        //
        // Parameters:
        //   adapter:
        //     Display adapter ordinal number. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter. This method returns Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     when the value equals or exceeds the number of display adapters in the system.
        //
        //   deviceType:
        //     Device type. Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration.
        //
        //   sourceFormat:
        //     Source adapter format. Member of the Microsoft.DirectX.Direct3D.Format enumeration.
        //
        //   targetFormat:
        //     Target adapter format. Member of the Microsoft.DirectX.Direct3D.Format enumeration.
        //
        // Returns:
        //     Returns true if the method succeeds; false if it fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormatConversion()
        //     parameter for the HRESULT code returned.If the method fails, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormatConversion()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall. If
        //     the hardware does not support conversion between the two formats, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormatConversion()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable.
        bool CheckDeviceFormatConversion(int adapter, DeviceType deviceType, Format sourceFormat, Format targetFormat);
        //
        // Summary:
        //     Tests a device to determine whether it supports conversion from one display
        //     format to another.
        //
        // Parameters:
        //   adapter:
        //     Display adapter ordinal number. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter. This method returns Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     when the value equals or exceeds the number of display adapters in the system.
        //
        //   deviceType:
        //     Device type. Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration.
        //
        //   sourceFormat:
        //     Source adapter format. Member of the Microsoft.DirectX.Direct3D.Format enumeration.
        //
        //   targetFormat:
        //     Target adapter format. Member of the Microsoft.DirectX.Direct3D.Format enumeration.
        //
        //   result:
        //     HRESULT code passed back from the method.
        //
        // Returns:
        //     Returns true if the method succeeds; false if it fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormatConversion()
        //     parameter for the HRESULT code returned.If the method fails, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormatConversion()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall. If
        //     the hardware does not support conversion between the two formats, Microsoft.DirectX.Direct3D.Manager.CheckDeviceFormatConversion()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable.
        bool CheckDeviceFormatConversion(int adapter, DeviceType deviceType, Format sourceFormat, Format targetFormat, out int result);
        //
        // Summary:
        //     Determines whether a multisampling technique is available on the current
        //     device.
        //
        // Parameters:
        //   adapter:
        //     Display adapter ordinal number. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter. This method returns Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     when the value equals or exceeds the number of display adapters in the system.
        //
        //   deviceType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration.
        //
        //   surfaceFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that specifies
        //     the format of the surface to be multisampled. See Remarks.
        //
        //   windowed:
        //     Set to true to inquire about windowed multisampling. Set to false to inquire
        //     about full-screen multisampling.
        //
        //   multiSampleType:
        //     Member of the Microsoft.DirectX.Direct3D.MultiSampleType enumeration that
        //     identifies the multisampling technique to test.
        //
        // Returns:
        //     Returns true if the method succeeds; false if it fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType()
        //     parameter for the HRESULT code returned.If the method fails, Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType()
        //     is set to one of the following values: Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     if the Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType() or
        //     Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType() parameters
        //     are invalid, Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable
        //     if the device does not support the queried multisampling technique, or Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidDevice
        //     if Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType() does not
        //     apply to the adapter.
        bool CheckDeviceMultiSampleType(int adapter, DeviceType deviceType, Format surfaceFormat, bool windowed, MultiSampleType multiSampleType);
        //
        // Summary:
        //     Determines whether a multisampling technique is available on the current
        //     device.
        //
        // Parameters:
        //   adapter:
        //     Display adapter ordinal number. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter. This method returns Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     when the value equals or exceeds the number of display adapters in the system.
        //
        //   deviceType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration.
        //
        //   surfaceFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that specifies
        //     the format of the surface to be multisampled. See Remarks.
        //
        //   windowed:
        //     Set to true to inquire about windowed multisampling. Set to false to inquire
        //     about full-screen multisampling.
        //
        //   multiSampleType:
        //     Member of the Microsoft.DirectX.Direct3D.MultiSampleType enumeration that
        //     identifies the multisampling technique to test.
        //
        //   result:
        //     HRESULT code passed back from the method.
        //
        //   qualityLevels:
        //     Number of quality stops available for a given multisample type; can be null
        //     if it is not necessary to return the values.
        //
        // Returns:
        //     Returns true if the method succeeds; false if it fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType()
        //     parameter for the HRESULT code returned.If the method fails, Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType()
        //     is set to one of the following values: Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     if the Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType() or
        //     Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType() parameters
        //     are invalid, Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable
        //     if the device does not support the queried multisampling technique, or Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidDevice
        //     if Microsoft.DirectX.Direct3D.Manager.CheckDeviceMultiSampleType() does not
        //     apply to the adapter.
        bool CheckDeviceMultiSampleType(int adapter, DeviceType deviceType, Format surfaceFormat, bool windowed, MultiSampleType multiSampleType, out int result, out int qualityLevels);
        //
        // Summary:
        //     Specifies whether a hardware-accelerated device type can be used on the current
        //     adapter.
        //
        // Parameters:
        //   adapter:
        //     Display adapter ordinal number. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter. This method returns Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     when the value equals or exceeds the number of display adapters in the system.
        //
        //   checkType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration that indicates
        //     the device type to check.
        //
        //   displayFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that indicates
        //     the format of the adapter display mode for which the device type is being
        //     checked. For example, some devices operate only in modes of 16 bits per pixel.
        //
        //   backBufferFormat:
        //     Back buffer format. For more information about formats, see Microsoft.DirectX.Direct3D.Format.
        //     This value must be one of the render target formats. Microsoft.DirectX.Direct3D.Device.Microsoft.DirectX.Direct3D.Device.DisplayMode
        //     can be used to obtain the current format.For windowed applications, the back
        //     buffer format does not need to match the display mode format if the hardware
        //     supports color conversion. The set of possible back buffer formats is constrained,
        //     but the runtime allows any valid back buffer format to be presented to any
        //     desktop format. Additionally, the device must be operable in desktop mode
        //     because devices typically do not operate in modes of 8 bits per pixel.Full-screen
        //     applications cannot perform color conversion.Microsoft.DirectX.Direct3D.Format.Format.Unknown
        //     is allowed for windowed mode.
        //
        //   windowed:
        //     Set to true if the device type will be used in windowed mode. Set to false
        //     if the device type will be used in full-screen.
        //
        // Returns:
        //     Returns true if the method succeeds and the device can be used on this adapter;
        //     false if the method fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDeviceType()
        //     parameter for the HRESULT code returned.If the method fails, Microsoft.DirectX.Direct3D.Manager.CheckDeviceType()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall, provided
        //     Microsoft.DirectX.Direct3D.Manager.CheckDeviceType() equals or exceeds the
        //     number of display adapters in the system. Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     also is returned if Microsoft.DirectX.Direct3D.Manager.CheckDeviceType()
        //     specified a device that does not exist.If the requested back buffer format
        //     is not supported, or if hardware acceleration is not available for the specified
        //     formats, Microsoft.DirectX.Direct3D.Manager.CheckDeviceType() is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable.
        bool CheckDeviceType(int adapter, DeviceType checkType, Format displayFormat, Format backBufferFormat, bool windowed);
        //
        // Summary:
        //     Specifies whether a hardware-accelerated device type can be used on the current
        //     adapter.
        //
        // Parameters:
        //   adapter:
        //     Display adapter ordinal number. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter. This method returns Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     when the value equals or exceeds the number of display adapters in the system.
        //
        //   checkType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration that indicates
        //     the device type to check.
        //
        //   displayFormat:
        //     Member of the Microsoft.DirectX.Direct3D.Format enumeration that indicates
        //     the format of the adapter display mode for which the device type is being
        //     checked. For example, some devices operate only in modes of 16 bits per pixel.
        //
        //   backBufferFormat:
        //     Back buffer format. For more information about formats, see Microsoft.DirectX.Direct3D.Format.
        //     This value must be one of the render target formats. Microsoft.DirectX.Direct3D.Device.Microsoft.DirectX.Direct3D.Device.DisplayMode
        //     can be used to obtain the current format.For windowed applications, the back
        //     buffer format does not need to match the display mode format if the hardware
        //     supports color conversion. The set of possible back buffer formats is constrained,
        //     but the runtime allows any valid back buffer format to be presented to any
        //     desktop format. Additionally, the device must be operable in desktop mode
        //     because devices typically do not operate in modes of 8 bits per pixel.Full-screen
        //     applications cannot perform color conversion.Microsoft.DirectX.Direct3D.Format.Format.Unknown
        //     is allowed for windowed mode.
        //
        //   windowed:
        //     Set to true if the device type will be used in windowed mode. Set to false
        //     if the device type will be used in full-screen.
        //
        //   result:
        //     HRESULT code passed back from the method.
        //
        // Returns:
        //     Returns true if the method succeeds and the device can be used on this adapter;
        //     false if the method fails. Check the Microsoft.DirectX.Direct3D.Manager.CheckDeviceType()
        //     parameter for the HRESULT code returned.If the method fails, Microsoft.DirectX.Direct3D.Manager.CheckDeviceType()
        //     is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall, provided
        //     Microsoft.DirectX.Direct3D.Manager.CheckDeviceType() equals or exceeds the
        //     number of display adapters in the system. Microsoft.DirectX.Direct3D.ResultCode.ResultCode.InvalidCall
        //     also is returned if Microsoft.DirectX.Direct3D.Manager.CheckDeviceType()
        //     specified a device that does not exist.If the requested back buffer format
        //     is not supported, or if hardware acceleration is not available for the specified
        //     formats, Microsoft.DirectX.Direct3D.Manager.CheckDeviceType() is set to Microsoft.DirectX.Direct3D.ResultCode.ResultCode.NotAvailable.
        bool CheckDeviceType(int adapter, DeviceType checkType, Format displayFormat, Format backBufferFormat, bool windowed, out int result);
        //
        // Summary:
        //     Blocks D3DSpy from monitoring an application.
        //
        // Returns:
        //     Returns true if D3DSpy is monitoring this application; otherwise, false.
        bool DisableD3DSpy();
        //
        // Summary:
        //     Generates a breakpoint in D3DSpy when called within the application.
        //
        // Returns:
        //     Returns true if D3DSpy is monitoring this application; otherwise, false.
        bool GenerateD3DSpyBreak();
        //
        // Summary:
        //     Retrieves information specific to a device.
        //
        // Parameters:
        //   adapter:
        //     Ordinal number that denotes the display adapter. Microsoft.DirectX.Direct3D.AdapterListCollection.Microsoft.DirectX.Direct3D.AdapterListCollection.Default
        //     is always the primary display adapter.
        //
        //   deviceType:
        //     Member of the Microsoft.DirectX.Direct3D.DeviceType enumeration that identifies
        //     the device type.
        //
        // Returns:
        //     If the method succeeds, the return value is a Microsoft.DirectX.Direct3D.Caps
        //     object that contains information that describes the device's capabilities.
        Caps GetDeviceCaps(int adapter, DeviceType deviceType);
    }
}
