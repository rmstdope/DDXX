using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class AnimationSetAdapter : IAnimationSet
    {
        private AnimationSet animationSet;

        internal AnimationSetAdapter(AnimationSet animationSet)
        {
            this.animationSet = animationSet;
        }

        internal AnimationSet DxAnimationSet
        {
            get { return animationSet; }
        }

        #region IAnimationSet Members

        public string Name
        {
            get { return animationSet.Name; }
        }

        public int NumberAnimations
        {
            get { return animationSet.NumberAnimations; }
        }

        public double Period
        {
            get { return animationSet.Period; }
        }

        public int GetAnimationIndex(string name)
        {
            return animationSet.GetAnimationIndex(name);
        }

        public string GetAnimationName(int index)
        {
            return animationSet.GetAnimationName(index);
        }

        public CallbackData GetCallback(double position, CallbackSearchFlags flags)
        {
            return animationSet.GetCallback(position, flags);
        }

        public double GetPeriodicPosition(double position)
        {
            return animationSet.GetPeriodicPosition(position);
        }

        public ScaleRotateTranslate GetScaleRotateTranslate(double periodicPosition, int animation)
        {
            return animationSet.GetScaleRotateTranslate(periodicPosition, animation);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            animationSet.Dispose();
        }

        #endregion
    }
}
