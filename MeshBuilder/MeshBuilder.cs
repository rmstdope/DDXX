using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.MeshBuilder
{
    public class MeshBuilder
    {
        private IGraphicsFactory factory;
        private ITextureFactory textureFactory;
        private IDevice device;
        private Dictionary<string, IPrimitive> primitives = new Dictionary<string,IPrimitive>();

        public MeshBuilder(IGraphicsFactory factory, ITextureFactory textureFactory, IDevice device)
        {
            this.factory = factory;
            this.textureFactory = textureFactory;
            this.device = device;
        }

        public void AddPrimitive(IPrimitive primitive, string name)
        {
            if (primitives.ContainsKey(name))
                throw new DDXXException("Can not add the two primitives with the same name.");
            primitives[name] = primitive;
        }

        public IModel CreateModel(string name)
        {
            if (!primitives.ContainsKey(name))
                throw new DDXXException("Can not create mesh from a primitive that does not exist.");
            return primitives[name].CreateModel(factory, textureFactory, device);
        }
    }
}
