using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class EffectParameterCollectionAdapter : ICollectionAdapter<IEffectParameter>
    {
        private EffectParameterCollection collection;

        public EffectParameterCollectionAdapter(EffectParameterCollection collection)
        {
            this.collection = collection;
        }

        public int Count 
        {
            get { return collection.Count; }
        }

        public IEffectParameter this[int index] 
        {
            get { return new EffectParameterAdapter(collection[index]); }
        }

        public IEffectParameter this[string name] 
        {
            get 
            {
                EffectParameter parameter = collection[name];
                if (parameter == null)
                    return null;
                return new EffectParameterAdapter(parameter);
            }
        }

        public IEnumerator<IEffectParameter> GetEnumerator()
        {
            return new EffectParameterEnumeratorAdapter(collection.GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new Exception("This method is not implemented.");
            //return new EffectParameterEnumeratorAdapter(collection..GetEnumerator());
        }
    }

    public class EffectParameterEnumeratorAdapter : IEnumerator<IEffectParameter>
    {
        private IEnumerator<EffectParameter> enumerator;

        public EffectParameterEnumeratorAdapter(IEnumerator<EffectParameter> enumerator)
        {
            this.enumerator = enumerator;
        }

        public IEffectParameter Current
        {
            get { return new EffectParameterAdapter(enumerator.Current); }
        }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return new EffectParameterAdapter(enumerator.Current); }
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
