using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class EffectAnnotationCollectionAdapter : ICollectionAdapter<IEffectAnnotation>
    {
        private EffectAnnotationCollection collection;

        public EffectAnnotationCollectionAdapter(EffectAnnotationCollection collection)
        {
            this.collection = collection;
        }

        public int Count 
        {
            get { return collection.Count; }
        }

        public IEffectAnnotation this[int index] 
        {
            get 
            {
                EffectAnnotation annotation = collection[index];
                if (annotation == null)
                    return null;
                return new EffectAnnotationAdapter(annotation);
            }
        }

        public IEffectAnnotation this[string name] 
        {
            get 
            { 
                EffectAnnotation annotation = collection[name];
                if (annotation == null)
                    return null;
                return new EffectAnnotationAdapter(annotation); 
            }
        }

        public IEnumerator<IEffectAnnotation> GetEnumerator()
        {
            return new EffectAnnotationEnumeratorAdapter(collection.GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new EffectAnnotationEnumeratorAdapter(collection.GetEnumerator());
        }
    }

    public class EffectAnnotationEnumeratorAdapter : IEnumerator<IEffectAnnotation>
    {
        private IEnumerator<EffectAnnotation> enumerator;

        public EffectAnnotationEnumeratorAdapter(IEnumerator<EffectAnnotation> enumerator)
        {
            this.enumerator = enumerator;
        }

        public IEffectAnnotation Current
        {
            get { return new EffectAnnotationAdapter(enumerator.Current); }
        }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return new EffectAnnotationAdapter(enumerator.Current); }
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
