﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Physics
{
    public interface IBody
    {
        void AddConstraint(IConstraint constraint);
        void AddParticle(IPhysicalParticle particle);
        Vector3 Gravity { get; set; }
        void Step();
        List<IPhysicalParticle> Particles { get; }
        List<IConstraint> Constraints { get; }
        void ApplyForce(Vector3 vector3);
    }
}
