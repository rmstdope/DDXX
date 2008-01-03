using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    [TestFixture]
    public class AnimationClipTest
    {
        private Mockery mockery;
        private AnimationClip clip;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void ConstructorNoAnimations()
        {
            // Exercise
            clip = new AnimationClip(1, 0);
            // Verify
            Assert.AreEqual(1, clip.Duration);
            Assert.AreEqual(0, clip.Animations.Length);
        }

        [Test]
        public void ConstructorWithAnimations()
        {
            // Setup
            IKeyFrameAnimation[] animations = new IKeyFrameAnimation[2];
            // Exercise
            clip = new AnimationClip(2, animations);
            // Verify
            Assert.AreEqual(2, clip.Duration);
            Assert.AreSame(animations, clip.Animations);
        }

        [Test]
        public void SetAnimationWithoutAdd()
        {
            // Exercise
            clip = new AnimationClip(1, 5);
            // Verify
            Assert.AreEqual(5, clip.Animations.Length);
        }

        [Test]
        public void SetAnimationWithAdd()
        {
            // Setup
            KeyFrameAnimation animation = new KeyFrameAnimation();
            // Exercise
            clip = new AnimationClip(1, 11);
            clip.SetAnimation(animation, 10);
            // Verify
            Assert.AreEqual(11, clip.Animations.Length);
            Assert.AreSame(animation, clip.Animations[10]);
        }

        [Test]
        public void ValidateAndSort()
        {
            // Setup
            IKeyFrameAnimation[] animations = new IKeyFrameAnimation[5];
            for (int i = 0; i < 5; i++)
            {  
                animations[i] = mockery.NewMock<IKeyFrameAnimation>();
                Expect.Once.On(animations[i]).Method("Sort");
            }
            clip = new AnimationClip(1, animations);
            // Exercise
            clip.ValidateAndSort();
        }

        [Test]
        public void GetBoneTransform()
        {
            // Setup
            IKeyFrameAnimation[] animations = new IKeyFrameAnimation[3];
            for (int i = 0; i < 3; i++)
            {
                animations[i] = mockery.NewMock<IKeyFrameAnimation>();
                Expect.Once.On(animations[i]).Method("GetBoneTransform").With(3.6f).
                    Will(Return.Value(Matrix.CreateRotationX(i)));
            }
            clip = new AnimationClip(1, animations);
            // Exercise
            Matrix[] matrices = clip.GetBoneTransforms(3.6f);
            // Verify
            Assert.AreEqual(new Matrix[] { 
                Matrix.CreateRotationX(0),
                Matrix.CreateRotationX(1),
                Matrix.CreateRotationX(2) }, matrices);
        }

    }
}
