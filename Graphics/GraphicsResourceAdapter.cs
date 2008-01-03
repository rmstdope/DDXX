using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class GraphicsResourceAdapter : IGraphicsResource
    {
        private GraphicsResource resource;

        public GraphicsResourceAdapter(GraphicsResource resource)
        {
            this.resource = resource;
        }

        #region IGraphicsResource Members

        public IGraphicsDevice GraphicsDevice
        {
            get { return new GraphicsDeviceAdapter(resource.GraphicsDevice); }
        }

        public string Name
        {
            get
            {
                return resource.Name;
            }
            set
            {
                resource.Name = value;
            }
        }

        public int Priority
        {
            get
            {
                return resource.Priority;
            }
            set
            {
                resource.Priority = value;
            }
        }

        public ResourceType ResourceType
        {
            get { return resource.ResourceType; }
        }

        public object Tag
        {
            get
            {
                return resource.Tag;
            }
            set
            {
                resource.Tag = value;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            resource.Dispose();
        }

        #endregion
    }
}
