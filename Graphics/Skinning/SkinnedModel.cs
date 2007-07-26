using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class SkinnedModel : ModelBase
    {
        private IAnimationRootFrame rootFrame;
        private IAnimationSet animationSet;
        private const int MAX_NUM_BONES = 60;
        private IFrame frame;
        private IMesh mesh;
        private int numBones;
        private float animationStartTime;
        private float animationSpeed;

        public IAnimationRootFrame AnimationRootFrame
        {
            get { return rootFrame; }
        }

        private SkinnedModel()
        {
        }

        public void SetAnimationSet(string name, float timeOffset, float speed)
        {
            animationSet = rootFrame.AnimationController.GetAnimationSet(name);
            rootFrame.AnimationController.SetTrackAnimationSet(0, animationSet);
            animationStartTime = timeOffset;
            this.animationSpeed = speed;
        }

        public void SetAnimationSet(int num, float timeOffset, float speed)
        {
            animationSet = rootFrame.AnimationController.GetAnimationSet(num);
            rootFrame.AnimationController.SetTrackAnimationSet(0, animationSet);
            animationStartTime = timeOffset;
            this.animationSpeed = speed;
        }

        public SkinnedModel(IAnimationRootFrame rootFrame, IFrame frame, ITextureFactory textureFactory)
        {
            this.rootFrame = rootFrame;
            Materials = CreateModelMaterials(textureFactory, frame.ExtendedMaterials); 
            this.frame = frame;

            if (frame.MeshContainer.SkinInformation != null)
            {
                if (rootFrame.AnimationController != null)
                    animationSet = rootFrame.AnimationController.GetAnimationSet(0);
                animationStartTime = 0;
                numBones = Math.Min(MAX_NUM_BONES, frame.MeshContainer.SkinInformation.NumberBones);
                int influences = 0;
                BoneCombination[] bones = null;
                mesh = frame.MeshContainer.SkinInformation.ConvertToIndexedBlendedMesh(
                    frame.MeshContainer.MeshData.Mesh, MeshFlags.Managed | MeshFlags.OptimizeVertexCache,
                    frame.MeshContainer.GetAdjacencyStream(), numBones, out influences,
                    out bones);
                //MeshDataAdapter data = new MeshDataAdapter();
                //data.Mesh = mesh;
                //frame.MeshContainer.MeshData = data;
                frame.MeshContainer.Bones = bones;

                Matrix[] offsetMatrices = new Matrix[frame.MeshContainer.SkinInformation.NumberBones];
                for (int i = 0; i < frame.MeshContainer.SkinInformation.NumberBones; i++)
                    offsetMatrices[i] = frame.MeshContainer.SkinInformation.GetBoneOffsetMatrix(i);
                frame.MeshContainer.RestMatrices = offsetMatrices;

                IFrame[] frameMatrix = new IFrame[frame.MeshContainer.SkinInformation.NumberBones];
                for (int i = 0; i < frame.MeshContainer.SkinInformation.NumberBones; i++)
                {
                    IFrame foundFrame = frame.Find(rootFrame.FrameHierarchy,
                        frame.MeshContainer.SkinInformation.GetBoneName(i));
                    if (foundFrame == null)
                        throw new InvalidOperationException("Could not find valid bone.");
                    frameMatrix[i] = foundFrame;
                }
                frame.MeshContainer.Frames = frameMatrix;
                if (rootFrame.AnimationController != null)
                    SetAnimationSet(0, 0, 1);
            }
            else
            {
                mesh = frame.MeshContainer.MeshData.Mesh;
            }
        }

        public override IMesh Mesh
        {
            get { return mesh; }
            set { mesh = value; }
        }

        public override bool IsSkinned()
        {
            return true;
        }

        protected override void HandleSkin(IEffectHandler effectHandler, int j)
        {
            if (frame.MeshContainer.SkinInformation != null)
            {
                Matrix[] skinMatrices = GetBoneMatrices(j);
                effectHandler.SetBones(skinMatrices);
            }
        }

        public Matrix[] GetBoneMatrices(int material)
        {
            BoneCombination[] bones = frame.MeshContainer.Bones;
            Matrix[] skinMatrices = new Matrix[frame.MeshContainer.SkinInformation.NumberBones];
            for (int i = 0; i < numBones; i++)
            {
                int index = bones[material].BoneId[i];
                if (index != -1)
                {
                    skinMatrices[i] = frame.MeshContainer.RestMatrices[index] *
                        frame.MeshContainer.Frames[index].CombinedTransformationMatrix;
                }
            }
            return skinMatrices;
        }

        public override void Step()
        {
            StepAnimation();
            UpdateFrameMatrices(rootFrame.FrameHierarchy, Matrix.Identity);
        }

        private void StepAnimation()
        {
            IAnimationController controller = rootFrame.AnimationController;
            if (controller != null)
            {
                controller.AdvanceTime(animationSet.Period - (controller.Time % animationSet.Period));
                controller.AdvanceTime((Time.StepTime - animationStartTime) * animationSpeed);
            }
        }

        private void UpdateFrameMatrices(IFrame frame, Matrix parentMatrix)
        {
            frame.CombinedTransformationMatrix = frame.TransformationMatrix * parentMatrix;

            if (frame.FrameSibling != null)
            {
                UpdateFrameMatrices(frame.FrameSibling, parentMatrix);
            }

            if (frame.FrameFirstChild != null)
            {
                UpdateFrameMatrices(frame.FrameFirstChild, frame.CombinedTransformationMatrix);
            }
        }

        public IFrame GetFrame(string name)
        {
            return rootFrame.FrameHierarchy.Find(rootFrame.FrameHierarchy, name);
        }

        public override IModel Clone()
        {
            SkinnedModel newModel = new SkinnedModel();
            newModel.rootFrame = rootFrame;
            newModel.animationSet = animationSet;
            newModel.animationStartTime = animationStartTime;
            newModel.frame = frame;
            newModel.mesh = mesh;
            newModel.numBones = numBones;
            newModel.CullMode = CullMode;
            newModel.Materials = new ModelMaterial[Materials.Length];
            for (int i = 0; i < Materials.Length; i++)
                newModel.Materials[i] = Materials[i].Clone();
            return newModel;
        }
    }
}
