using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class LineNodeTest : ILine, IScene
    {
        private bool drawing;
        private Vector3[] vertexList;
        private Matrix transform;
        private Color color;
        private CameraNode camera;

        [SetUp]
        public void SetUp()
        {
            camera = new CameraNode("");
            drawing = false;
            vertexList = null;
        }

        [Test]
        public void RenderLine1()
        {
            // Setup fixture
            LineNode node = new LineNode("", this, new Vector3(0, 0, 0), new Vector3(1, 1, 1), Color.Wheat);
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(new Vector3[] { new Vector3(0, 0, 0), new Vector3(1, 1, 1) }, vertexList);
            Assert.AreEqual(Color.Wheat, color);
            Assert.AreEqual(camera.ViewMatrix * camera.ProjectionMatrix, transform);
        }

        [Test]
        public void RenderLine2()
        {
            // Setup fixture
            LineNode node = new LineNode("", this, new Vector3(2, 3, 4), new Vector3(5, 6, 7), Color.Wheat);
            node.Position = new Vector3(1, 2, 3);
            Matrix world = Matrix.Translation(1, 2, 3);
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(new Vector3[] { new Vector3(2, 3, 4), new Vector3(5, 6, 7) }, vertexList);
            Assert.AreEqual(Color.Wheat, color);
            Assert.AreEqual(world * camera.ViewMatrix * camera.ProjectionMatrix, transform);
        }

        #region ILine Members

        public bool Antialias
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool Disposed
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool GlLines
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public int Pattern
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public float PatternScale
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public float Width
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void Begin()
        {
            Assert.IsFalse(drawing);
            drawing = true;
        }

        public void Draw(Microsoft.DirectX.Vector2[] vertexList, System.Drawing.Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Draw(Microsoft.DirectX.Vector2[] vertexList, int color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawTransform(Microsoft.DirectX.Vector3[] vertexList, Microsoft.DirectX.Matrix transform, System.Drawing.Color color)
        {
            Assert.IsNull(this.vertexList);
            this.vertexList = vertexList;
            this.transform = transform;
            this.color = color;
        }

        public void DrawTransform(Microsoft.DirectX.Vector3[] vertexList, Microsoft.DirectX.Matrix transform, int color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void End()
        {
            Assert.IsTrue(drawing);
            drawing = false;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IScene Members

        public IRenderableCamera ActiveCamera
        {
            get
            {
                return camera;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public Microsoft.DirectX.Direct3D.ColorValue AmbientColor
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public int NumNodes
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void AddNode(INode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Step()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Render()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public INode GetNodeByName(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Validate()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DebugPrintGraph()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void HandleHierarchy(IAnimationRootFrame hierarchy)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IScene Members


        public void SetEffectParameters()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
