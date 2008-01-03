using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class EffectFunctionCollectionAdapter : ICollectionAdapter<IEffectFunction>
    {
        private EffectFunctionCollection collection;

        public EffectFunctionCollectionAdapter(EffectFunctionCollection collection)
        {
            this.collection = collection;
        }

        public int Count
        {
            get { return collection.Count; }
        }

        public IEffectFunction this[int index]
        {
            get { return new EffectFunctionAdapter(collection[index]); }
        }

        public IEffectFunction this[string name]
        {
            get { return new EffectFunctionAdapter(collection[name]); }
        }

        public IEnumerator<IEffectFunction> GetEnumerator()
        {
            return new EffectFunctionEnumeratorAdapter(collection.GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new EffectFunctionEnumeratorAdapter(collection.GetEnumerator());
        }
    }

    public class EffectFunctionEnumeratorAdapter : IEnumerator<IEffectFunction>
    {
        private IEnumerator<EffectFunction> enumerator;

        public EffectFunctionEnumeratorAdapter(IEnumerator<EffectFunction> enumerator)
        {
            this.enumerator = enumerator;
        }

        public IEffectFunction Current
        {
            get { return new EffectFunctionAdapter(enumerator.Current); }
        }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return new EffectFunctionAdapter(enumerator.Current); }
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
