using System;

namespace Dope.DDXX.DemoFramework
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class TweakStepAttribute : Attribute
    {
        private float step;

        public TweakStepAttribute(float step)
        {
            this.step = step;
        }

        public float Step
        {
            get { return step; }
        }
    }
}
