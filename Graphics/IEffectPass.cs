using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IEffectPass
    {
        // Summary:
        //     Gets the set of EffectAnnotation objects for this EffectPass.
        //
        // Returns:
        //     The EffectAnnotationCollection containing EffectAnnotation objects for this
        //     EffectPass.
        ICollectionAdapter<IEffectAnnotation> Annotations { get; }
        //
        // Summary:
        //     Gets the name of this pass.
        //
        // Returns:
        //     The name of this pass.
        string Name { get; }
        // Summary:
        //     Begins a pass within the active technique.
        void Begin();
        //
        // Summary:
        //     Ends a pass within the active technique.
        void End();
    }
}
