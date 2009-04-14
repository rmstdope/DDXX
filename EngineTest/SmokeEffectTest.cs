using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace EngineTest
{
    [TestFixture]
    public class GridTest
    {
        Grid grid;

        [SetUp]
        public void Setup()
        {
            grid = new Grid(4);
            // write
            float x = 4;
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    grid[i, j] = x;
                    x += 0.5f;
                }
            }
        }

        [Test]
        public void TestIndices()
        {
            // read and check
            float x = 4.0f;
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    Assert.AreEqual(x, grid[i, j]);
                    x += 0.5f;
                }
            }
        }

        [Test]
        public void TestSwap()
        {
            Grid g2 = new Grid(2);
            SmokeSimulator.Swap(ref grid, ref g2);
            Assert.AreEqual(2, grid.Size);
            Assert.AreEqual(4, g2.Size);
            Assert.AreEqual(4.0f, g2[0, 0]);
        }

        [Test]
        public void TestAddMul()
        {
            grid.AddMul(3.0f, grid);
            // read and check
            float x = 4.0f;
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j <= 5; j++)
                {
                    Assert.AreEqual(x * 3.0f + x, grid[i, j]);
                    x += 0.5f;
                }
            }
        }
    }
}
