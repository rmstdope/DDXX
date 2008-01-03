using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;
using Dope.DDXX.NUnitExtension;

namespace Dope.DDXX.Animation
{
    [TestFixture]
    public class AnimationControllerTest
    {
        private Mockery mockery;
        private IAnimationController controller;
        private ISkinInformation skin;
        private IAnimationClip animationClip;
        private Dictionary<string, IAnimationClip> clips;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            skin = mockery.NewMock<ISkinInformation>();
            clips = new Dictionary<string, IAnimationClip>();
            animationClip = mockery.NewMock<IAnimationClip>();
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructorNoClips()
        {
            // Exercise
            controller = new AnimationController(clips, skin);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ContructorClipsIsNull()
        {
            // Exercise
            controller = new AnimationController(null, skin);
        }

        [Test]
        public void Getters()
        {
            // Setup
            Stub.On(skin).GetProperty("NumBones").Will(Return.Value(5));
            clips.Add("Key", null);
            // Exercise
            controller = new AnimationController(clips, skin);
            // Verify
            Assert.AreEqual(clips, controller.AnimationClips);
            Assert.AreEqual(skin, controller.SkinInformation);
            Assert.AreEqual(5, controller.WorldMatrices.Length);
        }

        [Test]
        public void Step()
        {
            // Setup
            SetTime(2.3f);
            Matrix root = Matrix.CreateRotationX(1);
            Matrix bone1 = Matrix.CreateRotationY(2);
            Matrix bone2 = Matrix.CreateRotationZ(3);
            Matrix invSkin1 = Matrix.CreateRotationX(4);
            Matrix invSkin2 = Matrix.CreateRotationY(5);
            Stub.On(skin).GetProperty("NumBones").Will(Return.Value(2));
            Stub.On(animationClip).GetProperty("Duration").Will(Return.Value(3.0f));
            clips.Add("Key", animationClip);
            Stub.On(skin).Method("GetParentIndex").With(1).Will(Return.Value(0));
            Stub.On(skin).Method("GetInvSkinPose").With(0).Will(Return.Value(invSkin1));
            Stub.On(skin).Method("GetInvSkinPose").With(1).Will(Return.Value(invSkin2));
            Expect.Once.On(animationClip).Method("GetBoneTransforms").With(2.3f).
                Will(Return.Value(new Matrix[] { bone1, bone2 }));
             controller = new AnimationController(clips, skin);
            // Exercise
            controller.Step(root);
            // Verify
            Assert.That(new Matrix[] { 
                invSkin1 * bone1 * root, 
                invSkin2 * bone2 * bone1 * root },
                new XnaConstraint(controller.WorldMatrices).Within(0.0001f));
        }

        [Test]
        public void StepWithLoop()
        {
            // Setup
            SetTime(5.0f);
            Stub.On(skin).GetProperty("NumBones").Will(Return.Value(1));
            Stub.On(animationClip).GetProperty("Duration").Will(Return.Value(3.0f));
            clips.Add("Key", animationClip);
            Stub.On(skin).Method("GetInvSkinPose").With(0).Will(Return.Value(Matrix.Identity));
            Expect.Once.On(animationClip).Method("GetBoneTransforms").With(2.0f).
                Will(Return.Value(new Matrix[] { Matrix.Identity }));
            controller = new AnimationController(clips, skin);
            // Exercise
            controller.Step(Matrix.Identity);
        }

        [Test]
        public void StepWithPingPongLoopPing()
        {
            // Setup
            SetTime(8.0f);
            Stub.On(skin).GetProperty("NumBones").Will(Return.Value(1));
            Stub.On(animationClip).GetProperty("Duration").Will(Return.Value(3.0f));
            clips.Add("Key", animationClip);
            Stub.On(skin).Method("GetInvSkinPose").With(0).Will(Return.Value(Matrix.Identity));
            Expect.Once.On(animationClip).Method("GetBoneTransforms").With(2.0f).
                Will(Return.Value(new Matrix[] { Matrix.Identity }));
            controller = new AnimationController(clips, skin);
            controller.PlayMode = PlayMode.PingPongLoop;
            // Exercise
            controller.Step(Matrix.Identity);
        }

        [Test]
        public void StepWithPingPongLoopPong()
        {
            // Setup
            SetTime(5.0f);
            Stub.On(skin).GetProperty("NumBones").Will(Return.Value(1));
            Stub.On(animationClip).GetProperty("Duration").Will(Return.Value(3.0f));
            clips.Add("Key", animationClip);
            Stub.On(skin).Method("GetInvSkinPose").With(0).Will(Return.Value(Matrix.Identity));
            Expect.Once.On(animationClip).Method("GetBoneTransforms").With(1.0f).
                Will(Return.Value(new Matrix[] { Matrix.Identity }));
            controller = new AnimationController(clips, skin);
            controller.PlayMode = PlayMode.PingPongLoop;
            // Exercise
            controller.Step(Matrix.Identity);
        }

        [Test]
        public void StepWithoutLoop()
        {
            // Setup
            SetTime(5.0f);
            Stub.On(skin).GetProperty("NumBones").Will(Return.Value(1));
            Stub.On(animationClip).GetProperty("Duration").Will(Return.Value(3.0f));
            clips.Add("Key", animationClip);
            Stub.On(skin).Method("GetInvSkinPose").With(0).Will(Return.Value(Matrix.Identity));
            Expect.Once.On(animationClip).Method("GetBoneTransforms").With(5.0f).
                Will(Return.Value(new Matrix[] { Matrix.Identity }));
            controller = new AnimationController(clips, skin);
            controller.PlayMode = PlayMode.Forward;
            // Exercise
            controller.Step(Matrix.Identity);
        }

        [Test]
        public void StepWithScale()
        {
            // Setup
            SetTime(1.5f);
            Stub.On(skin).GetProperty("NumBones").Will(Return.Value(1));
            Stub.On(animationClip).GetProperty("Duration").Will(Return.Value(9.0f));
            clips.Add("Key", animationClip);
            Stub.On(skin).Method("GetInvSkinPose").With(0).Will(Return.Value(Matrix.Identity));
            Expect.Once.On(animationClip).Method("GetBoneTransforms").With(3.0f).
                Will(Return.Value(new Matrix[] { Matrix.Identity }));
            controller = new AnimationController(clips, skin);
            controller.Speed = 2.0f;
            // Exercise
            controller.Step(Matrix.Identity);
        }

        private void SetTime(float t)
        {
            Time.Pause();
            Time.CurrentTime = t;
        }
    }
}
