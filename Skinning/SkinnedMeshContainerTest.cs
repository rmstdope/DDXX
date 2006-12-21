using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Skinning
{
    [TestFixture]
    public class SkinnedMeshContainerTest
    {
        private SkinnedMeshContainer container;
        private MeshData meshData;
        private ExtendedMaterial[] materials;
        private EffectInstance[] instances;

        [SetUp]
        public void SetUp()
        {
            meshData = new MeshData();
            materials = new ExtendedMaterial[1];
            instances = new EffectInstance[1];
        }

        [Test]
        public void TestConstructor()
        {
            container = new SkinnedMeshContainer("MeshName", meshData, materials, 
                                                 instances, null, null);
            Assert.AreEqual("MeshName", container.Name);
            Assert.AreEqual(meshData, container.MeshData);
            Assert.AreEqual(1, container.GetMaterials().Length);
            Assert.AreEqual(materials[0].ToString(), container.GetMaterials()[0].ToString());
            Assert.AreEqual(1, container.GetEffectInstances().Length);
            Assert.AreEqual(instances[0].ToString(), container.GetEffectInstances()[0].ToString());
        }

        //[Test]
        //public void TestConstructorFail()
        //{
        //    container = new SkinnedMeshContainer("MeshName", meshData, materials,
        //                                         instances, null, null);
        //}
    }
}
