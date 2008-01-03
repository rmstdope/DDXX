using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    public enum PlayMode
    {
        Forward,
        Loop,
        PingPongLoop
    }

    public interface IAnimationController
    {
        IDictionary<string, IAnimationClip> AnimationClips { get; }
        ISkinInformation SkinInformation { get; }
        Matrix[] WorldMatrices { get; }
        void Step(Matrix rootWorldMatrix);
        float Speed { get; set; }
        PlayMode PlayMode { get; set; }
    }
}
