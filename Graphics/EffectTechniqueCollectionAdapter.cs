using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class EffectTechniqueCollectionAdapter : ICollectionAdapter<IEffectTechnique>
    {
        private EffectTechniqueCollection collection;

        public EffectTechniqueCollectionAdapter(EffectTechniqueCollection collection)
        {
            this.collection = collection;
        }

        public int Count
        {
            get { return collection.Count; }
        }

        public IEffectTechnique this[int index]
        {
            get { return new EffectTechniqueAdapter(collection[index]); }
        }

        public IEffectTechnique this[string name]
        {
            get { return new EffectTechniqueAdapter(collection[name]); }
        }

        public IEnumerator<IEffectTechnique> GetEnumerator()
        {
            return new EffectTechniqueEnumeratorAdapter(collection.GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new EffectTechniqueEnumeratorAdapter(collection.GetEnumerator());
        }
    }

    public class EffectTechniqueEnumeratorAdapter : IEnumerator<IEffectTechnique>
    {
        private IEnumerator<EffectTechnique> enumerator;

        public EffectTechniqueEnumeratorAdapter(IEnumerator<EffectTechnique> enumerator)
        {
            this.enumerator = enumerator;
        }

        public IEffectTechnique Current
        {
            get { return new EffectTechniqueAdapter(enumerator.Current); }
        }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return new EffectTechniqueAdapter(enumerator.Current); }
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
