using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class NodeFactory : INodeFactory
    {
        private ITextureFactory textureFactory;

        public NodeFactory(ITextureFactory textureFactory)
        {
            this.textureFactory = textureFactory;
        }

        public ModelNode CreateModelNode(IFrame frame, IEffect effect, MeshTechniqueChooser prefix)
        {
            IModel model = new Model(frame.Mesh, textureFactory, frame.ExtendedMaterials);
            return CommonCreateModelNode(frame, effect, prefix, model);
        }

        public ModelNode CreateSkinnedModelNode(IAnimationRootFrame animationRootFrame, IFrame frame, IEffect effect, MeshTechniqueChooser prefix)
        {
            IModel model = new SkinnedModel(animationRootFrame, frame, textureFactory);
            return CommonCreateModelNode(frame, effect, prefix, model);
        }

        private static ModelNode CommonCreateModelNode(IFrame frame, IEffect effect, MeshTechniqueChooser prefix, IModel model)
        {
            IEffectHandler effectHandler = new EffectHandler(effect, 
                prefix(frame.Name), model);
            ModelNode node = new ModelNode(frame.Name, model, effectHandler);
            node.EnableFrameHandling(frame);
            return node;
        }

        public CameraNode CreateCameraNode(IFrame frame)
        {
            CameraNode node = new CameraNode(frame.Name);
            node.SetFOV((float)Math.PI / 2);
            node.EnableFrameHandling(frame);
            return node;
        }

        public DummyNode CreateDummyNode(IFrame frame)
        {
            DummyNode node = new DummyNode(frame.Name);
            node.EnableFrameHandling(frame);
            return node;
        }

        //private static void SetWorldState(IFrame frame, INode node, bool camera)
        //{
        //    Matrix m3;
        //    if (!camera)
        //    {
        //        // 3DS -> DX conversion
        //        Matrix m2 = Matrix.Identity;
        //        m2.M11 = -1;
        //        m2.M22 = 0;
        //        m2.M23 = 1;
        //        m2.M33 = 0;
        //        m2.M32 = 1;
        //        m3 = frame.TransformationMatrix * m2;
        //    }
        //    else
        //    {
        //        m3 = frame.TransformationMatrix;
        //    }
        //    node.WorldState.Position = new Vector3(m3.M41, m3.M42, m3.M43);
        //    node.WorldState.Rotation = Quaternion.RotationMatrix(m3);
        //}

    }
}
