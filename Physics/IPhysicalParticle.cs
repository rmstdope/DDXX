﻿using System;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Physics
{
    public interface IPhysicalParticle
    {
        float InvMass { get; set; }
        Vector3 Position { get; set; }
        void Step(Vector3 gravity);
        void ApplyForce(Vector3 force);
    }
}
