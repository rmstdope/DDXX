using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Dope.DDXX.Graphics
{
    public interface IContentManager : IDisposable
    {
        // Summary:
        //     Gets the root directory associated with this ContentManager.
        //
        // Returns:
        //     The root directory associated with this ContentManager.
        string RootDirectory { get; }
        //
        // Summary:
        //     Gets the service provider associated with the ContentManager.
        //
        // Returns:
        //     The service provider associated with the ContentManager.
        IServiceProvider ServiceProvider { get; }
        //
        // Summary:
        //     Loads an asset that has been processed by the Content Pipeline.
        //
        // Parameters:
        //   assetName:
        //     Asset name, relative to the loader root directory, and not including the
        //     .xnb file extension.
        //
        // Returns:
        //     The loaded asset. Repeated calls to load the same asset will return the same
        //     object instance.
        T Load<T>(string assetName);
        //
        // Summary:
        //     Disposes all data that was loaded by this ContentManager.
        void Unload();

    }
}
