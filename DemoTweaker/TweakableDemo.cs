using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoTweaker
{
    public class TweakableDemo : TweakableObjectBase<IDemoRegistrator>
    {
        private bool textureFactoryWritten;
        private bool modelFactoryWritten;

        public TweakableDemo(IDemoRegistrator target, ITweakableFactory factory)
            : base(target, factory)
        {
            textureFactoryWritten = false;
            modelFactoryWritten = false;
        }

        public override int NumVisableVariables
        {
            get { return 13; }
        }

        protected override int NumSpecificVariables
        {
            get { return Target.GetAllRegisterables().Count + 2; }
        }

        public override void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            if (index < Target.GetAllRegisterables().Count)
                return Factory.CreateTweakableObject(Target.GetAllRegisterables()[index]);
            else if (index == Target.GetAllRegisterables().Count)
                return Factory.CreateTweakableObject(Factory.GraphicsFactory.TextureFactory);
            return Factory.CreateTweakableObject(Factory.GraphicsFactory.ModelFactory);
        }

        protected override void ParseSpecficXmlNode(XmlNode node)
        {
            switch (node.Name)
            {
                case "Effect":
                    RegisterEffect(node);
                    break;
                case "PostEffect":
                    RegisterPostEffect(node);
                    break;
                case "Transition":
                    RegisterTransition(node);
                    break;
                case "TextureFactory":
                    Factory.CreateTweakableObject(Factory.GraphicsFactory.TextureFactory).ReadFromXmlFile(node);
                    break;
                case "ModelFactory":
                    Factory.CreateTweakableObject(Factory.GraphicsFactory.ModelFactory).ReadFromXmlFile(node);
                    break;
                default:
                    throw new DDXXException("Invalid XML element " + node.Name + 
                        " within element " + node.Name + ".");
            }
        }

        protected override void WriteSpecificXmlNode(XmlDocument xmlDocument, XmlNode node)
        {
            if (!(node is XmlElement))
                return;
            switch (node.Name)
            {
                case "Effect":
                case "PostEffect":
                case "Transition":
                    int index = Target.GetAllRegisterables().FindIndex(delegate(IRegisterable r) { return r.Name == GetStringAttribute(node, "name"); });
                    GetChild(index).WriteToXmlFile(xmlDocument, node);
                    break;
                case "TextureFactory":
                    textureFactoryWritten = true;
                    Factory.CreateTweakableObject(Factory.GraphicsFactory.TextureFactory).WriteToXmlFile(xmlDocument, node);
                    break;
                case "ModelFactory":
                    modelFactoryWritten = true;
                    Factory.CreateTweakableObject(Factory.GraphicsFactory.ModelFactory).WriteToXmlFile(xmlDocument, node);
                    break;
                default:
                    throw new DDXXException("Invalid XML element " + node.Name +
                        " within element " + node.Name + ".");
            }
        }

        protected override void WriteNewNodes(XmlDocument xmlDocument, XmlNode node)
        {
            if (!textureFactoryWritten)
            {
                node.AppendChild(xmlDocument.CreateElement("TextureFactory"));
                Factory.CreateTweakableObject(Factory.GraphicsFactory.TextureFactory).WriteToXmlFile(xmlDocument, node);
            }
            if (!modelFactoryWritten)
            {
                node.AppendChild(xmlDocument.CreateElement("ModelFactory"));
                Factory.CreateTweakableObject(Factory.GraphicsFactory.ModelFactory).WriteToXmlFile(xmlDocument, node);
            }
        }

        private void RegisterEffect(XmlNode node)
        {
            Target.AddEffect(GetStringAttribute(node, "class"), GetStringAttribute(node, "name"),
                GetIntAttribute(node, "track"), GetFloatAttribute(node, "starttime"), GetFloatAttribute(node, "endtime"));
            int index = Target.GetAllRegisterables().FindIndex(delegate(IRegisterable r) { return r.Name == GetStringAttribute(node, "name"); });
            GetChild(index).ReadFromXmlFile(node);
        }

        private void RegisterPostEffect(XmlNode node)
        {
            Target.AddPostEffect(GetStringAttribute(node, "class"), GetStringAttribute(node, "name"),
                GetIntAttribute(node, "track"), GetFloatAttribute(node, "starttime"), GetFloatAttribute(node, "endtime"));
            int index = Target.GetAllRegisterables().FindIndex(delegate(IRegisterable r) { return r.Name == GetStringAttribute(node, "name"); });
            GetChild(index).ReadFromXmlFile(node);
        }

        private void RegisterTransition(XmlNode node)
        {
            // TODO: DestinationTrack existis both as attribute and node
            Target.AddTransition(GetStringAttribute(node, "class"), GetStringAttribute(node, "name"),
                GetIntAttribute(node, "destinationtrack"), GetFloatAttribute(node, "starttime"), GetFloatAttribute(node, "endtime"));
            int index = Target.GetAllRegisterables().FindIndex(delegate(IRegisterable r) { return r.Name == GetStringAttribute(node, "name"); });
            GetChild(index).ReadFromXmlFile(node);
        }

    }
}
