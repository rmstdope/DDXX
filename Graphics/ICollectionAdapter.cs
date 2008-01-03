using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Graphics
{
    public interface ICollectionAdapter<T> : IEnumerable<T>
    {
        // Summary:
        //     Gets the number of EffectParameter objects in this EffectParameterCollection.
        //
        // Returns:
        //     The number of EffectParameter objects in this EffectParameterCollection.
        int Count { get; }
        // Summary:
        //     Gets a specific EffectParameter object by using an index value.
        //
        // Parameters:
        //   index:
        //     Index of the EffectParameter to get.
        //
        // Returns:
        //     The EffectParameter object at index index.
        T this[int index] { get; }
        //
        // Summary:
        //     Gets a specific EffectParameter by name.
        //
        // Parameters:
        //   name:
        //     The name of the EffectParameter to retrieve.
        //
        // Returns:
        //     The EffectParameter object named name.
        T this[string name] { get; }
    }
}
