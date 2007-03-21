using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Graphics.Skinning;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class NodeFactory : Dope.DDXX.SceneGraph.INodeFactory
    {
        private ITextureFactory textureFactory;
        public NodeFactory(ITextureFactory textureFactory)
        {
            this.textureFactory = textureFactory;
        }

        public ModelNode CreateModelNode(IFrame frame, IEffect effect, string prefix)
        {
            IModel model = new Model(frame.Mesh, textureFactory, frame.ExtendedMaterials);
            IEffectHandler effectHandler = new EffectHandler(effect, prefix, model);
            ModelNode node = new ModelNode(frame.Name, model, effectHandler);
            SetWorldState(frame, node);
            return node;
        }

        public CameraNode CreateCameraNode(IFrame frame)
        {
            CameraNode node = new CameraNode(frame.Name);
            SetWorldState(frame, node);
            return node;
        }

        public DummyNode CreateDummyNode(IFrame frame)
        {
            DummyNode node = new DummyNode(frame.Name);
            SetWorldState(frame, node);
            return node;
        }

        private static void SetWorldState(IFrame frame, INode node)
        {
            // 3DS -> DX conversion
            Matrix m2 = Matrix.Identity;
            m2.M11 = -1;
            m2.M22 = 0;
            m2.M23 = 1;
            m2.M33 = 0;
            m2.M32 = 1;
            Matrix m3 = frame.TransformationMatrix * m2;
            node.WorldState.Position = new Vector3(m3.M41, m3.M42, m3.M43);
            node.WorldState.Rotation = Quaternion.RotationMatrix(m3);
        }

    }
}
