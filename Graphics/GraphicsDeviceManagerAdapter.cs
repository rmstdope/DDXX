using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class GraphicsDeviceManagerAdapter : IDeviceManager
    {
        private GraphicsDeviceManager manager;

        public GraphicsDeviceManagerAdapter(GraphicsDeviceManager manager)
        {
            this.manager = manager;
            manager.DeviceCreated += new EventHandler(OnDeviceCreated);
            manager.DeviceDisposing += new EventHandler(OnDeviceDisposing);
            manager.DeviceReset += new EventHandler(OnDeviceReset);
            manager.DeviceResetting += new EventHandler(OnDeviceResetting);
            manager.Disposed += new EventHandler(OnDisposed);
            manager.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(OnPreparingDeviceSettings);
        }

        void manager_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            PreparingDeviceSettings(sender, e);
        }

        #region IGraphicsDeviceManager Members

        public IGraphicsDevice GraphicsDevice
        {
            get { return new GraphicsDeviceAdapter(manager.GraphicsDevice); }
        }

        public bool IsFullScreen
        {
            get
            {
                return manager.IsFullScreen;
            }
            set
            {
                manager.IsFullScreen = value;
            }
        }

        public ShaderProfile MinimumPixelShaderProfile
        {
            get
            {
                return manager.MinimumPixelShaderProfile;
            }
            set
            {
                manager.MinimumPixelShaderProfile = value;
            }
        }

        public ShaderProfile MinimumVertexShaderProfile
        {
            get
            {
                return manager.MinimumVertexShaderProfile;
            }
            set
            {
                manager.MinimumVertexShaderProfile = value;
            }
        }

        public bool PreferMultiSampling
        {
            get
            {
                return manager.PreferMultiSampling;
            }
            set
            {
                manager.PreferMultiSampling = value;
            }
        }

        public SurfaceFormat PreferredBackBufferFormat
        {
            get
            {
                return manager.PreferredBackBufferFormat;
            }
            set
            {
                manager.PreferredBackBufferFormat = value;
            }
        }

        public int PreferredBackBufferHeight
        {
            get
            {
                return manager.PreferredBackBufferHeight;
            }
            set
            {
                manager.PreferredBackBufferHeight = value;
            }
        }

        public int PreferredBackBufferWidth
        {
            get
            {
                return manager.PreferredBackBufferWidth;
            }
            set
            {
                manager.PreferredBackBufferWidth = value;
            }
        }

        public DepthFormat PreferredDepthStencilFormat
        {
            get
            {
                return manager.PreferredDepthStencilFormat;
            }
            set
            {
                manager.PreferredDepthStencilFormat = value;
            }
        }

        public bool SynchronizeWithVerticalRetrace
        {
            get
            {
                return manager.SynchronizeWithVerticalRetrace;
            }
            set
            {
                manager.SynchronizeWithVerticalRetrace = value;
            }
        }

        public event EventHandler DeviceCreated;

        public event EventHandler DeviceDisposing;

        public event EventHandler DeviceReset;

        public event EventHandler DeviceResetting;

        public event EventHandler Disposed;

        public event EventHandler<PreparingDeviceSettingsEventArgs> PreparingDeviceSettings;

        public void OnDeviceCreated(object sender, EventArgs e)
        {
            if (DeviceCreated != null)
                DeviceCreated(sender, e);
        }

        public void OnDeviceReset(object sender, EventArgs e)
        {
            if (DeviceReset != null)
                DeviceReset(sender, e);
        }

        public void OnDeviceDisposing(object sender, EventArgs e)
        {
            if (DeviceDisposing != null)
                DeviceDisposing(sender, e);
        }

        public void OnDisposed(object sender, EventArgs e)
        {
            if (Disposed != null)
                Disposed(sender, e);
        }

        public void OnDeviceResetting(object sender, EventArgs e)
        {
            if (DeviceResetting != null)
                DeviceResetting(sender, e);
        }

        public void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            if (PreparingDeviceSettings != null)
                PreparingDeviceSettings(sender, e);
        }

        public void ApplyChanges()
        {
            manager.ApplyChanges();
        }

        public void ToggleFullScreen()
        {
            manager.ToggleFullScreen();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion


    }
}
