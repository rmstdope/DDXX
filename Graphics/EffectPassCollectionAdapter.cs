using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class EffectPassCollectionAdapter : ICollectionAdapter<IEffectPass>
    {
        private EffectPassCollection collection;

        public EffectPassCollectionAdapter(EffectPassCollection collection)
        {
            this.collection = collection;
        }

        public int Count 
        {
            get { return collection.Count; }
        }

        public IEffectPass this[int index] 
        {
            get { return new EffectPassAdapter(collection[index]); }
        }

        public IEffectPass this[string name] 
        {
            get { return new EffectPassAdapter(collection[name]); }
        }

        public IEnumerator<IEffectPass> GetEnumerator()
        {
            return new EffectPassEnumeratorAdapter(collection.GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new Exception("This method is not implemented.");
            //return new EffectParameterEnumeratorAdapter(collection..GetEnumerator());
        }
    }

    public class EffectPassEnumeratorAdapter : IEnumerator<IEffectPass>
    {
        private IEnumerator<EffectPass> enumerator;

        public EffectPassEnumeratorAdapter(IEnumerator<EffectPass> enumerator)
        {
            this.enumerator = enumerator;
        }

        public IEffectPass Current
        {
            get { return new EffectPassAdapter(enumerator.Current); }
        }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return new EffectPassAdapter(enumerator.Current); }
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
        }

    }
}
