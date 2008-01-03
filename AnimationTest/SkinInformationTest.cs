using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Animation
{
    [TestFixture]
    public class SkinInformationTest
    {
        private IList<Matrix> bindPose;
        private IList<Matrix> invBindPose;
        private IList<int> hierarchy;
        private SkinInformation skin;

        [SetUp]
        public void SetUp()
        {
            bindPose = new List<Matrix>();
            invBindPose = new List<Matrix>();
            hierarchy = new List<int>();
        }

        [Test]
        public void Getters()
        {
            skin = new SkinInformation(bindPose, invBindPose, hierarchy);
            Assert.AreSame(bindPose, skin.BindPose);
            Assert.AreSame(invBindPose, skin.InverseBindPose);
            Assert.AreSame(hierarchy, skin.SkeletonHierarchy);
        }

        [Test]
        public void Hierarchy()
        {
            hierarchy.Add(2);
            hierarchy.Add(4);
            hierarchy.Add(6);
            skin = new SkinInformation(bindPose, invBindPose, hierarchy);
            Assert.AreEqual(2, skin.GetParentIndex(0));
            Assert.AreEqual(4, skin.GetParentIndex(1));
            Assert.AreEqual(6, skin.GetParentIndex(2));
        }

    }
}
