using System;

namespace Dope.DDXX.Physics
{
    public enum ConstraintPriority
    {
        PositionPriority = 0,
        StickPriority = 1,
        DummyPriority = 2
    }

    public interface IConstraint
    {
        ConstraintPriority Priority { get; }
        void Satisfy();
    }
}
