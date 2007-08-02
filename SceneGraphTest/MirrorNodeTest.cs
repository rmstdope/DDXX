using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class MirrorNodeTest : IModel, IScene, IRenderableCamera
    {
        private string originalName;
        private Cull originalCulling;
        private Cull newCulling;

        [Test]
        public void NodeName()
        {
            //Setup
            originalName = "NodeName";
            // Exercise SUT
            MirrorNode node = new MirrorNode(this);
            // Verify
            Assert.AreEqual("NodeName_Mirror", node.Name);
        }

        [Test]
        public void ReflectY()
        {
            //Setup
            DummyNode dummy = new DummyNode("");
            MirrorNode node = new MirrorNode(dummy);
            // Verify
            Assert.AreEqual(new Vector3(1, -1, 1), node.WorldState.Scaling);
            Assert.AreEqual(Matrix.Identity, dummy.WorldMatrix);
        }

        [Test]
        public void CullingClockwise()
        {
            // Setup
            originalCulling = Cull.CounterClockwise;
            ModelNode modelNode = new ModelNode("", this, null, null);
            MirrorNode node = new MirrorNode(modelNode);
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(Cull.Clockwise, newCulling);
        }

        [Test]
        public void CullingCouterClockwise()
        {
            // Setup
            originalCulling = Cull.Clockwise;
            ModelNode modelNode = new ModelNode("", this, null, null);
            MirrorNode node = new MirrorNode(modelNode);
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(Cull.CounterClockwise, newCulling);
        }

        [Test]
        public void CullingNone()
        {
            // Setup
            originalCulling = Cull.None;
            newCulling = Cull.None;
            ModelNode modelNode = new ModelNode("", this, null, null);
            MirrorNode node = new MirrorNode(modelNode);
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(Cull.None, newCulling);
        }

        [Test]
        public void CullingWithChildren()
        {
            // Setup
            originalCulling = Cull.CounterClockwise;
            newCulling = Cull.None;
            DummyNode baseNode = new DummyNode(""); ;
            ModelNode modelNode = new ModelNode("", this, null, null);
            MirrorNode node = new MirrorNode(baseNode);
            baseNode.AddChild(modelNode);
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(Cull.Clockwise, newCulling);
        }

        #region IScene Members

        public IRenderableCamera ActiveCamera
        {
            get
            {
                return this;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public ColorValue AmbientColor
        {
            get
            {
                return ColorValue.FromArgb(0);
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

        public void SetEffectParameters()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IModel Members

        public ModelMaterial[] Materials
        {
            get
            {
                return new ModelMaterial[0];
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public IMesh Mesh
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

        public bool IsSkinned()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Render(IDevice device, IEffectHandler effectHandler, ColorValue ambient, Microsoft.DirectX.Matrix world, Microsoft.DirectX.Matrix view, Microsoft.DirectX.Matrix projection)
        {
            // Culling should have been switched
            if (originalCulling == Cull.Clockwise)
                Assert.AreEqual(Cull.CounterClockwise, newCulling);
            else if (originalCulling == Cull.CounterClockwise)
                Assert.AreEqual(Cull.Clockwise, newCulling);
            else
                Assert.AreEqual(Cull.None, newCulling);
        }

        public void Step(IRenderableCamera camera)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Step()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IModel Clone()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Cull CullMode
        {
            get
            {
                return originalCulling;
            }
            set
            {
                newCulling = value;
            }
        }

        #endregion

        #region INode Members

        public string Name
        {
            get { return originalName; }
        }

        public INode Parent
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public List<INode> Children
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Dope.DDXX.Physics.WorldState WorldState
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Matrix WorldMatrix
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void AddChild(INode child)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool HasChild(INode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetLightState(LightState state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Render(IScene scene)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int CountNodes()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void EnableFrameHandling(IFrame frame)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.DirectX.Vector3 Position
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IRenderableCamera Members

        public Microsoft.DirectX.Matrix ProjectionMatrix
        {
            get { return Matrix.Identity; }
        }

        public Microsoft.DirectX.Matrix ViewMatrix
        {
            get { return Matrix.Identity; }
        }

        #endregion

        #region IScene Members


        public void RemoveNode(INode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region INode Members


        public void RemoveChild(INode child)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IModel Members


        public bool UseStencil
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

        public int StencilReference
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

        #endregion

        #region IRenderableCamera Members


        public void SetClippingPlanes(float near, float far)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IRenderableCamera Members


        public void SetFOV(float fov)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float GetFOV()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
