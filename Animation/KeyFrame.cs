using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    public class KeyFrame
    {
        private float time;
        private Matrix transform;

        public float Time
        {
            get { return time; }
        }

        public Matrix Transform
        {
            get { return transform; }
        }

        public KeyFrame(float time, Matrix transform)
        {
            this.time = time;
            this.transform = transform;
        }
    }
}
