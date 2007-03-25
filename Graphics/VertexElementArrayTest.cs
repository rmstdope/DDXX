using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using NUnit.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class VertexElementArrayTest
    {
        private VertexElement[] elementsPNTBX2 = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
            new VertexElement(0, 24, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            new VertexElement(0, 32, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
            new VertexElement(0, 44, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.BiNormal, 0),
            VertexElement.VertexDeclarationEnd,
        };
        private VertexElement[] elementsPNTX2 = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
            new VertexElement(0, 24, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            new VertexElement(0, 32, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Tangent, 0),
            VertexElement.VertexDeclarationEnd,
        };
        private VertexElement[] elementsPNX2 = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
            new VertexElement(0, 24, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            VertexElement.VertexDeclarationEnd
        };
        private VertexElement[] elementsPN = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
            VertexElement.VertexDeclarationEnd
        };
        private VertexElement[] elementsPX1 = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            VertexElement.VertexDeclarationEnd
        };
        private VertexElement[] elementsPX2 = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            VertexElement.VertexDeclarationEnd
        };
        private VertexElement[] elementsPX3 = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            VertexElement.VertexDeclarationEnd
        };
        private VertexElement[] elementsPX4 = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            new VertexElement(0, 12, DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
            VertexElement.VertexDeclarationEnd
        };
        private VertexElement[] elementsP = new VertexElement[]
        {
            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
            VertexElement.VertexDeclarationEnd
        };
        private VertexElement[] elementsEmpty = new VertexElement[]
        {
            VertexElement.VertexDeclarationEnd
        };

        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestEmptyArray()
        {
            VertexElementArray array = new VertexElementArray();
            Assert.AreEqual(elementsEmpty.Length, array.VertexElements.Length, 
                "Array should have only VertexDeclarationEnd element.");
            for (int i = 0; i < elementsEmpty.Length; i++)
                Assert.IsTrue(elementsEmpty[i].Equals(array.VertexElements[i]), "Index is " + i);
        }

        [Test]
        public void TestAddPosition()
        {
            VertexElementArray array = new VertexElementArray();
            array.AddPositions();
            Assert.AreEqual(elementsP.Length, array.VertexElements.Length,
                "Array should have only VertexDeclarationEnd element.");
            for (int i = 0; i < elementsP.Length; i++)
                Assert.IsTrue(elementsP[i].Equals(array.VertexElements[i]), "Index is " + i);
        }

        [Test]
        public void TestAddNormals1()
        {
            VertexElementArray array = new VertexElementArray(elementsP);
            array.AddNormals();
            Assert.AreEqual(elementsPN.Length, array.VertexElements.Length);
            for (int i = 0; i < elementsPN.Length; i++)
                Assert.IsTrue(elementsPN[i].Equals(array.VertexElements[i]), "Index is " + i);
        }

        [Test]
        public void TestAddNormals2()
        {
            VertexElementArray array = new VertexElementArray(elementsPX2);
            array.AddNormals();
            Assert.AreEqual(elementsPNX2.Length, array.VertexElements.Length);
            for (int i = 0; i < elementsPN.Length; i++)
                Assert.IsTrue(elementsPNX2[i].Equals(array.VertexElements[i]), "Index is " + i);
        }

        [Test]
        public void TestAddTangents1()
        {
            VertexElementArray array = new VertexElementArray(elementsPNX2);
            array.AddTangents();
            Assert.AreEqual(elementsPNTX2.Length, array.VertexElements.Length);
            for (int i = 0; i < elementsPNTX2.Length; i++)
                Assert.IsTrue(elementsPNTX2[i].Equals(array.VertexElements[i]), "Index is " + i);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAddTangents2()
        {
            VertexElementArray array = new VertexElementArray(elementsPN);
            array.AddTangents();
        }

        [Test]
        public void TestAddBiNormals1()
        {
            VertexElementArray array = new VertexElementArray(elementsPNTX2);
            array.AddBiNormals();
            Assert.AreEqual(elementsPNTBX2.Length, array.VertexElements.Length);
            for (int i = 0; i < elementsPNTBX2.Length; i++)
                Assert.IsTrue(elementsPNTBX2[i].Equals(array.VertexElements[i]), "Index is " + i);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAddBiNormals2()
        {
            VertexElementArray array = new VertexElementArray(elementsPNX2);
            array.AddBiNormals();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAddTexCoords0()
        {
            VertexElementArray array = new VertexElementArray(elementsP);
            array.AddTexCoords(0, 0);
        }

        [Test]
        public void TestAddTexCoords1()
        {
            VertexElementArray array = new VertexElementArray(elementsP);
            array.AddTexCoords(0, 1);
            Assert.AreEqual(elementsPX1.Length, array.VertexElements.Length);
            for (int i = 0; i < elementsPX1.Length; i++)
                Assert.IsTrue(elementsPX1[i].Equals(array.VertexElements[i]), "Index is " + i);
        }

        [Test]
        public void TestAddTexCoords2()
        {
            VertexElementArray array = new VertexElementArray(elementsP);
            array.AddTexCoords(0, 2);
            Assert.AreEqual(elementsPX2.Length, array.VertexElements.Length);
            for (int i = 0; i < elementsPX2.Length; i++)
                Assert.IsTrue(elementsPX2[i].Equals(array.VertexElements[i]), "Index is " + i);
        }

        [Test]
        public void TestAddTexCoords3()
        {
            VertexElementArray array = new VertexElementArray(elementsP);
            array.AddTexCoords(0, 3);
            Assert.AreEqual(elementsPX3.Length, array.VertexElements.Length);
            for (int i = 0; i < elementsPX3.Length; i++)
                Assert.IsTrue(elementsPX3[i].Equals(array.VertexElements[i]), "Index is " + i);
        }

        [Test]
        public void TestAddTexCoords4()
        {
            VertexElementArray array = new VertexElementArray(elementsP);
            array.AddTexCoords(0, 4);
            Assert.AreEqual(elementsPX4.Length, array.VertexElements.Length);
            for (int i = 0; i < elementsPX4.Length; i++)
                Assert.IsTrue(elementsPX4[i].Equals(array.VertexElements[i]), "Index is " + i);
        }

        [Test]
        public void TestLongArray()
        {
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                VertexElement.VertexDeclarationEnd,
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0)
            };
            VertexElementArray array = new VertexElementArray(elements);
            Assert.AreEqual(elementsP.Length, array.VertexElements.Length);
            for (int i = 0; i < elementsP.Length; i++)
                Assert.IsTrue(elementsP[i].Equals(array.VertexElements[i]), "Index is " + i);
        }
    }
}
