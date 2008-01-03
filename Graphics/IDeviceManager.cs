using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface IDeviceManager : IDisposable
    {
        // Summary:
        //     Gets the Framework.Graphics.GraphicsDevice associated with the GraphicsDeviceManager.
        //
        // Returns:
        //     The Framework.Graphics.GraphicsDevice associated with the GraphicsDeviceManager.
        IGraphicsDevice GraphicsDevice { get; }
        //
        // Summary:
        //     Gets or sets a value that indicates whether the device should start in full-screen
        //     mode.
        //
        // Returns:
        //     Value that indicates whether the device should start in full-screen mode.
        bool IsFullScreen { get; set; }
        //
        // Summary:
        //     Gets or sets the minimum pixel shader version required by the GraphicsDeviceManager.
        //
        // Returns:
        //     The minimum pixel shader version required by the GraphicsDeviceManager.
        ShaderProfile MinimumPixelShaderProfile { get; set; }
        //
        // Summary:
        //     Gets or sets the minimum vertex shader version required by the GraphicsDeviceManager.
        //
        // Returns:
        //     The minimum vertex shader version required by the GraphicsDeviceManager.
        ShaderProfile MinimumVertexShaderProfile { get; set; }
        //
        // Summary:
        //     Gets or sets a value that indicates whether to enable a multisampled back
        //     buffer.
        //
        // Returns:
        //     Value indicating whether multisample is enabled on the back buffer.
        bool PreferMultiSampling { get; set; }
        //
        // Summary:
        //     Gets or sets the format of the back buffer.
        //
        // Returns:
        //     The format of the back buffer.
        SurfaceFormat PreferredBackBufferFormat { get; set; }
        //
        // Summary:
        //     Gets or sets the preferred back-buffer height.
        //
        // Returns:
        //     The preferred back-buffer height.
        int PreferredBackBufferHeight { get; set; }
        //
        // Summary:
        //     Gets or sets the preferred back-buffer width.
        //
        // Returns:
        //     The preferred back-buffer width.
        int PreferredBackBufferWidth { get; set; }
        //
        // Summary:
        //     Gets or sets the format of the depth stencil.
        //
        // Returns:
        //     The format of the depth stencil.
        DepthFormat PreferredDepthStencilFormat { get; set; }
        //
        // Summary:
        //     Gets or sets a value that indicates whether to sync to the vertical trace
        //     (vsync) when presenting the back buffer.
        //
        // Returns:
        //     Value that indicates whether to sync to the vertical trace (vsync) when presenting
        //     the back buffer.
        bool SynchronizeWithVerticalRetrace { get; set; }
        // Summary:
        //     Raised when a new graphics device is created.
        event EventHandler DeviceCreated;
        //
        // Summary:
        //     Raised when the GraphicsDeviceManager is being disposed.
        event EventHandler DeviceDisposing;
        //
        // Summary:
        //     Raised when the GraphicsDeviceManager is reset.
        event EventHandler DeviceReset;
        //
        // Summary:
        //     Raised when the GraphicsDeviceManager is about to be reset.
        event EventHandler DeviceResetting;
        //
        // Summary:
        //     Raised when the GraphicsDeviceManager is disposed.
        event EventHandler Disposed;
        //
        // Summary:
        //     Raised when the GraphicsDeviceManager is changing the Graphics.GraphicsDevice
        //     settings (during reset or recreation of the Graphics.GraphicsDevice).
        event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;
        // Summary:
        //     Applies any changes to device-related properties, changing the graphics device
        //     as necessary.
        void ApplyChanges();
        //
        // Summary:
        //     Called when a device is created. Raises the GraphicsDeviceManager.DeviceCreated
        //     event.
        //
        // Parameters:
        //   sender:
        //     The GraphicsDeviceManager.
        //
        //   args:
        //     Arguments for the GraphicsDeviceManager.DeviceCreated event.
        void OnDeviceCreated(object sender, EventArgs args);
        //
        // Summary:
        //     Called when a device is being disposed. Raises the GraphicsDeviceManager.DeviceDisposing
        //     event.
        //
        // Parameters:
        //   sender:
        //     The GraphicsDeviceManager.
        //
        //   args:
        //     Arguments for the GraphicsDeviceManager.DeviceDisposing event.
        void OnDeviceDisposing(object sender, EventArgs args);
        //
        // Summary:
        //     Called when the device has been reset. Raises the GraphicsDeviceManager.DeviceReset
        //     event.
        //
        // Parameters:
        //   sender:
        //     The GraphicsDeviceManager.
        //
        //   args:
        //     Arguments for the GraphicsDeviceManager.DeviceReset event.
        void OnDeviceReset(object sender, EventArgs args);
        //
        // Summary:
        //     Called when the device is about to be reset. Raises the GraphicsDeviceManager.DeviceResetting
        //     event.
        //
        // Parameters:
        //   sender:
        //     The GraphicsDeviceManager.
        //
        //   args:
        //     Arguments for the GraphicsDeviceManager.DeviceResetting event.
        void OnDeviceResetting(object sender, EventArgs args);
        //
        // Summary:
        //     Called when the GraphicsDeviceManager is changing the Graphics.GraphicsDevice
        //     settings (during reset or recreation of the Graphics.GraphicsDevice).  Raises
        //     the GraphicsDeviceManager.PreparingDeviceSettings event.
        //
        // Parameters:
        //   sender:
        //     The GraphicsDeviceManager.
        //
        //   args:
        //     The graphics device information to modify.
        void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs args);
        //
        // Summary:
        //     Toggles between full screen and windowed mode.
        void ToggleFullScreen();
    }
}
