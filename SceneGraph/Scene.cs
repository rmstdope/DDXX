using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics.Skinning;

namespace Dope.DDXX.SceneGraph
{
    public class Scene : IScene
    {
        private IEffect effect;
        private EffectHandle lightDiffuseHandle;
        private EffectHandle lightSpecularHandle;
        private EffectHandle lightPositionHandle;
        private EffectHandle eyePositionHandle;

        private NodeBase rootNode;
        private IRenderableCamera activeCamera;
        private IDevice device;
        private ColorValue ambientColor;
        private List<IAnimationRootFrame> hierarchies = new List<IAnimationRootFrame>();

        public Scene()
        {
            rootNode = new DummyNode("Scene Root Node");
            device = D3DDriver.GetInstance().Device;
            ambientColor = new ColorValue(0.5f, 0.5f, 0.5f, 0.5f);
            IEffectFactory effectFactory = D3DDriver.EffectFactory;
            effect = effectFactory.CreateFromFile("PoolEffect.fxo");
            lightDiffuseHandle = effect.GetParameter(null, "LightDiffuseColors");
            lightSpecularHandle = effect.GetParameter(null, "LightSpecularColors");
            lightPositionHandle = effect.GetParameter(null, "LightPositions");
            eyePositionHandle = effect.GetParameter(null, "EyePosition");

            if (lightDiffuseHandle == null || lightSpecularHandle == null || lightPositionHandle == null || eyePositionHandle == null)
                throw new DDXXException("Can't find mandatory handles in PoolEffect");
        }

        public int NumNodes
        {
            get
            {
                return rootNode.CountNodes();
            }
        }

        public void AddNode(INode node)
        {
            rootNode.AddChild(node);
        }

        public void Step()
        {
            foreach (IAnimationRootFrame hierarchy in hierarchies)
            {
                IAnimationController controller = hierarchy.AnimationController;
                if (controller != null)
                    controller.AdvanceTime(Time.DeltaTime);
            }
            rootNode.Step();

            LightState state = new LightState();
            rootNode.SetLightState(state);
            effect.SetValue(lightDiffuseHandle, state.DiffuseColor);
            effect.SetValue(lightSpecularHandle, state.SpecularColor);
            effect.SetValue(lightPositionHandle, state.Positions);

            Vector3 eyePos = ActiveCamera.Position;
            effect.SetValue(eyePositionHandle, new Vector4(eyePos.X, eyePos.Y, eyePos.Z, 1.0f));
        }

        public void Render()
        {
            if (ActiveCamera == null)
                throw new DDXXException("Must have an active camera set before a scene can be rendered.");

            rootNode.Render(this);
        }

        public IRenderableCamera ActiveCamera
        {
            get { return activeCamera; }
            set
            {
                if (GetNodeByName(value.Name) != null)
                    activeCamera = value;
                else
                    throw new DDXXException("The active camera must be part of the scene graph.");
            }
        }

        public ColorValue AmbientColor
        {
            get { return ambientColor; }
            set { ambientColor = value; }
        }

        public INode GetNodeByName(string name)
        {
            return FindNodeByName(rootNode, name, null);
        }

        private INode FindNodeByName(INode node, string name, INode exclude)
        {
            if (node != exclude && node.Name == name)
                return node;
            foreach (INode child in node.Children)
            {
                INode result = FindNodeByName(child, name, exclude);
                if (result != null)
                    return result;
            }
            return null;
        }

        public void Validate()
        {
            ValidateNode(rootNode);
        }

        private void ValidateNode(INode node)
        {
            if (FindNodeByName(rootNode, node.Name, node) != null)
                throw new DDXXException("Two nodes with the same name (" + node.Name + 
                    ") exist in the same Scene.");
            foreach (INode child in node.Children)
            {
                ValidateNode(child);
            }
        }

        public void DebugPrintGraph()
        {
            PrintNode(rootNode, 0);
        }

        private void PrintNode(INode node, int indent)
        {
            for (int i = 0; i < indent; i++)
                Debug.Write('|');
            Debug.WriteLine(node.Name);
            foreach (INode child in node.Children)
            {
                PrintNode(child, indent + 1);
            }
        }

        public void HandleHierarchy(IAnimationRootFrame hierarchy)
        {
            hierarchies.Add(hierarchy);
        }
    }
}
