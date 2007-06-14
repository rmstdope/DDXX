using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;

namespace Dope.DDXX.Physics
{
    [TestFixture]
    public class BoundingConstraintTest : IPhysicalParticle, IBoundingObject
    {
        private Vector3 particlePosition;
        private Vector3 constraintPosition;

        [Test]
        public void TestMovement()
        {
            particlePosition = new Vector3(1, 2, 3);
            constraintPosition = new Vector3(4, 5, 6);
            TestConstraint();
        }

        [Test]
        public void TestMovement2()
        {
            particlePosition = new Vector3(4, 5, 6);
            constraintPosition = new Vector3(1, 2, 3);
            TestConstraint();
        }

        [Test]
        public void TestNoMovement()
        {
            particlePosition = new Vector3(1, 2, 3);
            constraintPosition = new Vector3(1, 2, 3);
            TestConstraint();
        }

        private void TestConstraint()
        {
            BoundingConstraint constraint = new BoundingConstraint(this, this);
            constraint.Satisfy();
            Assert.AreEqual(constraintPosition, particlePosition,
                "ParticlesPosition should equal ConstraintPosition.");
        }


        #region IPhysicalParticle Members

        public float InvMass
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public Vector3 Position
        {
            get
            {
                return particlePosition;
            }
            set
            {
                particlePosition = value;
            }
        }

        public void Step(Vector3 gravity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ApplyForce(Vector3 force)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IBoundingObject Members

        public Vector3 ConstrainOutside(Vector3 position)
        {
            return constraintPosition;
        }

        public bool IsInside(Vector3 position)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector3 Center
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }
}
