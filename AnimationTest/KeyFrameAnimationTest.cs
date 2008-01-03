using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    [TestFixture]
    public class KeyFrameAnimationTest
    {
        [Test]
        public void EmptyConstructor()
        {
            KeyFrameAnimation animation = new KeyFrameAnimation();
            Assert.AreEqual(0, animation.KeyFrames.Count);
        }

        [Test]
        public void Constructor()
        {
            List<KeyFrame> keyFrames = new List<KeyFrame>();
            keyFrames.Add(new KeyFrame(1, Matrix.CreateFromYawPitchRoll(1, 2, 3)));
            keyFrames.Add(new KeyFrame(2, Matrix.CreateFromYawPitchRoll(4, 5, 6)));
            KeyFrameAnimation animation = new KeyFrameAnimation(keyFrames);
            Assert.AreEqual(2, animation.KeyFrames.Count);
            Assert.AreEqual(1, animation.KeyFrames[0].Time);
            Assert.AreEqual(2, animation.KeyFrames[1].Time);
        }

        [Test]
        public void SortOrdered()
        {
            List<KeyFrame> keyFrames = new List<KeyFrame>();
            keyFrames.Add(new KeyFrame(1, Matrix.CreateFromYawPitchRoll(1, 2, 3)));
            keyFrames.Add(new KeyFrame(2, Matrix.CreateFromYawPitchRoll(4, 5, 6)));
            KeyFrameAnimation animation = new KeyFrameAnimation(keyFrames);
            animation.Sort();
            Assert.AreEqual(2, animation.KeyFrames.Count);
            Assert.AreEqual(1, animation.KeyFrames[0].Time);
            Assert.AreEqual(2, animation.KeyFrames[1].Time);
        }

        [Test]
        public void SortUnordered()
        {
            List<KeyFrame> keyFrames = new List<KeyFrame>();
            keyFrames.Add(new KeyFrame(2, Matrix.CreateFromYawPitchRoll(1, 2, 3)));
            keyFrames.Add(new KeyFrame(1, Matrix.CreateFromYawPitchRoll(4, 5, 6)));
            KeyFrameAnimation animation = new KeyFrameAnimation(keyFrames);
            animation.Sort();
            Assert.AreEqual(2, animation.KeyFrames.Count);
            Assert.AreEqual(1, animation.KeyFrames[0].Time);
            Assert.AreEqual(2, animation.KeyFrames[1].Time);
        }

        [Test]
        public void GetBoneTransformBeforeFirst()
        {
            List<KeyFrame> keyFrames = new List<KeyFrame>();
            keyFrames.Add(new KeyFrame(1, Matrix.CreateRotationX(1)));
            keyFrames.Add(new KeyFrame(2, Matrix.CreateRotationY(2)));
            KeyFrameAnimation animation = new KeyFrameAnimation(keyFrames);
            Assert.AreEqual(Matrix.CreateRotationX(1), animation.GetBoneTransform(0));
        }

        [Test]
        public void GetBoneTransformAfterLast()
        {
            List<KeyFrame> keyFrames = new List<KeyFrame>();
            keyFrames.Add(new KeyFrame(1, Matrix.CreateRotationX(1)));
            keyFrames.Add(new KeyFrame(2, Matrix.CreateRotationY(2)));
            KeyFrameAnimation animation = new KeyFrameAnimation(keyFrames);
            Assert.AreEqual(Matrix.CreateRotationY(2), animation.GetBoneTransform(3));
        }

        [Test]
        public void GetBoneTransformInTheMiddle()
        {
            List<KeyFrame> keyFrames = new List<KeyFrame>();
            keyFrames.Add(new KeyFrame(1, Matrix.CreateRotationX(1)));
            keyFrames.Add(new KeyFrame(2, Matrix.CreateRotationY(2)));
            keyFrames.Add(new KeyFrame(3, Matrix.CreateRotationZ(3)));
            KeyFrameAnimation animation = new KeyFrameAnimation(keyFrames);
            Assert.AreEqual(Matrix.CreateRotationY(2), animation.GetBoneTransform(2));
        }
    }
}
