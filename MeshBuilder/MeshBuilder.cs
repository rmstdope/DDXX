using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace MeshBuilder
{
    public class MeshBuilder
    {
        private IGraphicsFactory factory;
        private Dictionary<string, IPrimitive> primitives = new Dictionary<string,IPrimitive>();

        public MeshBuilder(IGraphicsFactory factory)
        {
            this.factory = factory;
        }

        public void AddPrimitive(IPrimitive primitive, string name)
        {
            if (primitives.ContainsKey(name))
                throw new DDXXException("Can not add the two primitives with the same name.");
            primitives[name] = primitive;
        }

        public IMesh CreateMesh(string name)
        {
            if (!primitives.ContainsKey(name))
                throw new DDXXException("Can not create mesh from a primitive that does not exist.");
            return primitives[name].CreateMesh(factory);
        }
    }
}
