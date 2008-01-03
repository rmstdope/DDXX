using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class VertexStreamCollectionAdapter : IVertexStreamCollection
    {
        private VertexStreamCollection collection;

        public VertexStreamCollectionAdapter(VertexStreamCollection collection)
        {
            this.collection = collection;
        }

        #region IVertexStreamCollection Members

        public IVertexStream this[int index]
        {
            get { return new VertexStreamAdapter(collection[index]); }
        }

        #endregion
    }
}
