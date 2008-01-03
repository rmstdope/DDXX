using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Physics
{
    public class BoundingSphere : Dope.DDXX.Physics.IBoundingObject
    {
        private float radius;
        private Vector3 center;

        public Vector3 Center
        {
            get { return center; }
            set { center = value; }
        }

        public BoundingSphere(float radius)
            : this(new Vector3(0, 0, 0), radius)
        {
        }

        public BoundingSphere(Vector3 center, float radius)
        {
            this.radius = radius;
            this.center = center;
        }

        public bool IsInside(Vector3 position)
        {
            if ((position - center).Length() < radius)
                return true;
            return false;
        }

        public Vector3 ConstrainOutside(Vector3 position)
        {
            if (IsInside(position))
                return MoveToSurface(position);
            return position;
        }

        private Vector3 MoveToSurface(Vector3 position)
        {
            position -= center;
            if (position == new Vector3(0, 0, 0))
                position = new Vector3(1, 0, 0);
            position.Normalize();
            return position * radius + center;
        }
    }
}
