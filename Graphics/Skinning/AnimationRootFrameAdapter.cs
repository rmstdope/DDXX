using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    public class AnimationRootFrameAdapter : IAnimationRootFrame
    {
        private AnimationRootFrame animationRootFrame;

        public AnimationRootFrameAdapter(AnimationRootFrame rootFrame)
        {
            animationRootFrame = rootFrame;
        }

        #region IAnimationRootFrame Members

        public IAnimationController AnimationController
        {
            get { return new AnimationControllerAdapter(animationRootFrame.AnimationController); }
        }

        public IFrame FrameHierarchy
        {
            get { return new FrameAdapter(animationRootFrame.FrameHierarchy); }
        }

        #endregion
    }
}
