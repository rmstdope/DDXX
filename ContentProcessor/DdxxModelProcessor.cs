using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.IO;
using System.Diagnostics;
using Dope.DDXX.Animation;

namespace Dope.DDXX.ContentProcessor
{   
    [ContentProcessor]
    public class DdxxModelProcessor : ModelProcessor
    {
        private const int MaxNumBones = 59;
        private string directory;
        private ContentProcessorContext context;
        private BoneContent skeleton;
        private IList<BoneContent> bones;
        private List<Matrix> bindPose;

        public override ModelContent Process(NodeContent rootNode, ContentProcessorContext context)
        {
            System.Diagnostics.Debug.WriteLine("==============================");
            System.Diagnostics.Debug.WriteLine("Processing: " + rootNode.Name);
            directory = Path.GetDirectoryName(rootNode.Identity.SourceFilename);
            this.context = context;

            ValidateHierarchy(rootNode);
            ExctractSkeleton(rootNode);
            FlattenTransforms(rootNode, skeleton);

            bindPose = new List<Matrix>();
            List<Matrix> inverseBindPose = new List<Matrix>();
            List<int> skeletonHierarchy = new List<int>();
            foreach (BoneContent bone in bones)
            {
                bindPose.Add(bone.Transform);
                inverseBindPose.Add(Matrix.Invert(bone.AbsoluteTransform));
                skeletonHierarchy.Add(bones.IndexOf(bone.Parent as BoneContent));
            }
            Dictionary<string, IAnimationClip> animationClips = null;
            if (skeleton != null)
                animationClips = ProcessAnimations(skeleton.Animations);
            
            ProcessHierarchy(rootNode);

            ModelContent model = base.Process(rootNode, context);
            if (animationClips != null && animationClips.Count > 0)
                model.Tag = new AnimationController(animationClips,
                    new SkinInformation(bindPose, inverseBindPose, skeletonHierarchy));

            return model;
        }

        private Dictionary<string, IAnimationClip> 
            ProcessAnimations(AnimationContentDictionary animations)
        {
            Dictionary<string, int> boneMap = new Dictionary<string, int>();

            for (int i = 0; i < bones.Count; i++)
            {
                string boneName = bones[i].Name;

                if (!string.IsNullOrEmpty(boneName))
                    boneMap.Add(boneName, i);
            }

            // Convert each animation in turn.
            Dictionary<string, IAnimationClip> animationClips = new Dictionary<string, IAnimationClip>();

            foreach (KeyValuePair<string, AnimationContent> animation in animations)
            {
                IAnimationClip processed = ProcessAnimation(animation.Value, boneMap);

                animationClips.Add(animation.Key, processed);
                //context.Logger.LogWarning(null, null, "Animation: {0}", animation.Key);
            }

            if (animationClips.Count == 0)
            {
                throw new InvalidContentException(
                            "Input file does not contain any animations.");
            }

            return animationClips;
        }

        private IAnimationClip ProcessAnimation(AnimationContent animation, Dictionary<string, int> boneMap)
        {
            if (animation.Duration <= TimeSpan.Zero)
                throw new InvalidContentException("Animation has a zero duration.");

            IAnimationClip animationClip = 
                new AnimationClip(animation.Duration.Ticks / (float)TimeSpan.TicksPerSecond, boneMap.Count);

            foreach (KeyValuePair<string, AnimationChannel> channel in animation.Channels)
            {
                int boneIndex;
                //context.Logger.LogWarning(null, null, "Bone {0}", channel.Key);

                if (!boneMap.TryGetValue(channel.Key, out boneIndex))
                    throw new InvalidContentException(string.Format("Found animation for bone '{0}', " +
                        "which is not part of the skeleton.", channel.Key));

                List<KeyFrame> keyFrames = new List<KeyFrame>();
                foreach (AnimationKeyframe keyframe in channel.Value)
                {
                    keyFrames.Add(new KeyFrame(keyframe.Time.Ticks / (float)TimeSpan.TicksPerSecond, 
                        keyframe.Transform));
                }
                animationClip.SetAnimation(new KeyFrameAnimation(keyFrames), boneIndex);
            }

            for (int i = 0; i < animationClip.Animations.Length; i++)
            {
                if (animationClip.Animations[i] == null)
                {
                    List<KeyFrame> frames = new List<KeyFrame>();
                    frames.Add(new KeyFrame(0, bindPose[i]));
                    animationClip.SetAnimation(new KeyFrameAnimation(frames), i);
                }
            }

            animationClip.ValidateAndSort();

            return animationClip;
        }

        private void FlattenTransforms(NodeContent node, BoneContent skeleton)
        {
            if (node == skeleton)
                return;
            MeshHelper.TransformScene(node, node.Transform);
            node.Transform = Matrix.Identity;
            foreach (NodeContent child in node.Children)
                FlattenTransforms(child, skeleton);
        }

        private void ExctractSkeleton(NodeContent rootNode)
        {
            bones = new List<BoneContent>();
            skeleton = MeshHelper.FindSkeleton(rootNode);
            if (skeleton != null)
            {
                bones = MeshHelper.FlattenSkeleton(skeleton);
                if (bones.Count > MaxNumBones)
                    throw new InvalidContentException(string.Format(
                        "Skeleton has {0} bones, but the maximum supported is {1}.",
                        bones.Count, MaxNumBones));
            }
        }

        private void ValidateHierarchy(NodeContent node)
        {
            if (node is MeshContent && node.Parent is BoneContent)
                context.Logger.LogWarning(null, null, "Mesh {0} is a child of bone {1}.", 
                    node.Name, node.Parent.Name);
            foreach (NodeContent child in new List<NodeContent>(node.Children))
                ValidateHierarchy(child);
        }

        private void ProcessHierarchy(NodeContent node)
        {
            if (node is MeshContent)
            {
                MeshContent mesh = node as MeshContent;
                bool normalMapped = IsMeshNormalMapped(mesh);
                if (normalMapped)
                    Debug.WriteLine("Normalmapped.");

                Debug.WriteLine("Mesh OpaqueData:");
                foreach (KeyValuePair<string, object> pair in mesh.OpaqueData)
                {
                    System.Diagnostics.Debug.WriteLine(pair.Key);
                }

                GenerateTangents(mesh);
                if (normalMapped)
                {
                    ExtractNormalMap(mesh);
                }
            }

            foreach (NodeContent child in node.Children)
            {
                ProcessHierarchy(child);
            }
        }

        private void ExtractNormalMap(MeshContent mesh)
        {
            string normalMapFile =
                mesh.OpaqueData.GetValue<string>("NormalMap", null);

            normalMapFile = Path.Combine(directory, normalMapFile);

            foreach (GeometryContent geometry in mesh.Geometry)
            {
                geometry.Material.Textures.Add("NormalMap",
                    new ExternalReference<TextureContent>(normalMapFile));
            }
        }

        private void GenerateTangents(MeshContent mesh)
        {
           MeshHelper.CalculateTangentFrames(mesh, VertexChannelNames.TextureCoordinate(0),
               VertexChannelNames.Tangent(0), VertexChannelNames.Binormal(0));
        }

        private bool IsMeshNormalMapped(MeshContent mesh)
        {
            return (mesh.OpaqueData.GetValue<string>("NormalMap", null) != null);
        }

        protected override MaterialContent ConvertMaterial(MaterialContent material, ContentProcessorContext context)
        {
            EffectMaterialContent normalMappingMaterial = new EffectMaterialContent();

            System.Diagnostics.Debug.WriteLine("--------------------");
            System.Diagnostics.Debug.WriteLine("Material OpaqueData:");
            foreach (KeyValuePair<string, object> pair in material.OpaqueData)
                normalMappingMaterial.OpaqueData.Add(pair.Key, pair.Value);

            EnsureColors(normalMappingMaterial);

            foreach (KeyValuePair<string, object> pair in material.OpaqueData)
                System.Diagnostics.Debug.WriteLine(pair);

            string file = Path.Combine(directory, "../../Effects/Content/effects/DefaultEffect.fx");
            if (!File.Exists(file))
                file = Path.Combine(directory, "../effects/DefaultEffect.fx");

            normalMappingMaterial.Effect =
                new ExternalReference<EffectContent>(file);

            System.Diagnostics.Debug.WriteLine("Textures:");
            foreach (KeyValuePair<String, ExternalReference<TextureContent>> texture in material.Textures)
            {
                System.Diagnostics.Debug.WriteLine(texture);
                normalMappingMaterial.Textures.Add(texture.Key, texture.Value);
            }

            return context.Convert<MaterialContent, MaterialContent>(normalMappingMaterial, 
                                                                     typeof(DdxxMaterialProcessor).Name);
        }

        private void EnsureColors(EffectMaterialContent material)
        {
            if (!material.OpaqueData.ContainsKey("DiffuseColor"))
                material.OpaqueData.Add("DiffuseColor", new Vector3(0.6f, 0.6f, 0.6f));
            if (!material.OpaqueData.ContainsKey("EmissiveColor"))
                material.OpaqueData.Add("EmissiveColor", new Vector3(0, 0, 0));
            if (!material.OpaqueData.ContainsKey("AmbientColor"))
                material.OpaqueData.Add("AmbientColor", material.OpaqueData["DiffuseColor"]);
            if (!material.OpaqueData.ContainsKey("SpecularColor"))
                material.OpaqueData.Add("SpecularColor", new Vector3(0, 0, 0));
            if (!material.OpaqueData.ContainsKey("Shininess"))
                material.OpaqueData.Add("Shininess", 1.0f);
            if (!material.OpaqueData.ContainsKey("SpecularPower"))
                material.OpaqueData.Add("SpecularPower", 1);
        }

        private bool IsMaterialNormalMapped(MaterialContent material)
        {
            return material.Textures.ContainsKey("NormalMap");
        }

    }
}
