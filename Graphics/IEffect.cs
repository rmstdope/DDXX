using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IEffect : IDisposable
    {
        // Summary:
        //     Gets the name of the effect creator.
        //
        // Returns:
        //     The name of the effect creator.
        string Creator { get; }
        //
        // Summary:
        //     Gets or sets the active technique.
        //
        // Returns:
        //     The current technique.
        IEffectTechnique CurrentTechnique { get; set; }
        //
        // Summary:
        //     Gets an EffectPool representing the pool of shared parameters.
        //
        // Returns:
        //     The pool of shared parameters.
        EffectPool EffectPool { get; }
        //
        // Summary:
        //     Gets a collection of functions that can render the effect.
        //
        // Returns:
        //     Collection of functions that can render the effect.
        ICollectionAdapter<IEffectFunction> Functions { get; }
        //
        // Summary:
        //     Gets the graphics device that created the effect.
        //
        // Returns:
        //     The graphics device that created the effect.
        IGraphicsDevice GraphicsDevice { get; }
        //
        // Summary:
        //     Gets a collection of parameters used for this effect.
        //
        // Returns:
        //     The collection of parameters used for this effect.
        ICollectionAdapter<IEffectParameter> Parameters { get; }
        //
        // Summary:
        //     Gets a collection of techniques that are defined for this effect.
        //
        // Returns:
        //     A collection of techniques that are defined for this effect.
        ICollectionAdapter<IEffectTechnique> Techniques { get; }
        // Summary:
        //     Begins application of the active technique.
        void Begin();
        //
        // Summary:
        //     Begins application of the active technique, specifying options for saving
        //     state.
        //
        // Parameters:
        //   saveStateMode:
        //     Options for saving the state prior to application of the technique.
        void Begin(SaveStateMode saveStateMode);
        //
        // Summary:
        //     Creates a clone of an effect.
        //
        // Parameters:
        //   device:
        //     The device associated with the effect.
        //
        // Returns:
        //     The cloned effect.
        IEffect Clone(IGraphicsDevice device);
        //
        // Summary:
        //     Propagates the state change that occurs inside of an active pass to the device
        //     before rendering.
        void CommitChanges();
#if (!XBOX)
        //
        // Summary:
        //     [Windows Only] Disassembles this effect.
        //
        // Parameters:
        //   enableColorCode:
        //     true to enable color coding to make the disassembly easier to read.
        //
        // Returns:
        //     A string that contains the effect assembly (ASM).
        string Disassemble(bool enableColorCode);
#endif
        //
        // Summary:
        //     Ends the application of the current technique.
        void End();
    }
}
