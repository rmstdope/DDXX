using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Dope.DDXX.Graphics
{
    public class ContentManagerAdapter : IContentManager
    {
        private ContentManager manager;

        public ContentManagerAdapter(ContentManager manager)
        {
            this.manager = manager;
        }

        #region IContentManager Members

        public string RootDirectory
        {
            get { return manager.RootDirectory; }
        }

        public IServiceProvider ServiceProvider
        {
            get { return manager.ServiceProvider; }
        }

        public T Load<T>(string assetName)
        {
            return manager.Load<T>(assetName);
        }

        public void Unload()
        {
            manager.Unload();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            manager.Dispose();
        }

        #endregion
    }
}
