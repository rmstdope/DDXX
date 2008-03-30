using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public class KeyFrame<T>
    {
        private float time;
        private T value;

        public float Time
        {
            get { return time; }
            set { time = value; }
        }

        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public KeyFrame(float time, T value)
        {
            this.time = time;
            this.value = value;
        }
    }
}
