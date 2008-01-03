using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IEffectTechnique
    {
        // Summary:
        //     Gets the EffectAnnotation objects associated with this technique.
        //
        // Returns:
        //     The EffectAnnotation objects associated with this technique.
        ICollectionAdapter<IEffectAnnotation> Annotations { get; }
        //
        // Summary:
        //     Gets the name of this technique.
        //
        // Returns:
        //     The name of this technique.
        string Name { get; }
        //
        // Summary:
        //     Gets the number of passes this rendering technique requires.
        //
        // Returns:
        //     The number of passes this rendering technique requires.
        ICollectionAdapter<IEffectPass> Passes { get; }
        //
        // Summary:
        //     Determines whether a given EffectParameter is used by this technique.
        //
        // Parameters:
        //   parameter:
        //     The effect parameter to check.
        //
        // Returns:
        //     true if the parameter is used by this technique; false otherwise.
        bool IsParameterUsed(IEffectParameter parameter);
        //
        // Summary:
        //     Validates this technique.
        //
        // Returns:
        //     true if the technique is valid; false otherwise.
        bool Validate();
    }
}
