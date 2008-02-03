using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableDemo : TweakableObjectBase<IDemoRegistrator>
    {
        private IDemoEffectBuilder builder;
        private bool textureFactoryWritten;

        public TweakableDemo(IDemoRegistrator target, IDemoEffectBuilder builder, ITweakableFactory factory)
            : base(target, factory)
        {
            this.builder = builder;
            textureFactoryWritten = false;
        }

        public override int NumVisableVariables
        {
            get { return 13; }
        }

        protected override int NumSpecificVariables
        {
            get { return Target.GetAllRegisterables().Count + 1; }
        }

        public override void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            if (index < Target.GetAllRegisterables().Count)
                return Factory.CreateTweakableObject(Target.GetAllRegisterables()[index]);
            return Factory.CreateTweakableObject(Target.TextureFactory);
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
                    break;
                case "TextureFactory":
                    Factory.CreateTweakableObject(Factory.TextureFactory).ReadFromXmlFile(node);
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
                    int index = Target.GetAllRegisterables().FindIndex(delegate(IRegisterable r) { return r.Name == GetStringAttribute(node, "name"); });
                    GetChild(index).WriteToXmlFile(xmlDocument, node);
                    break;
                case "Transition":
                    break;
                case "TextureFactory":
                    textureFactoryWritten = true;
                    Factory.CreateTweakableObject(Factory.TextureFactory).WriteToXmlFile(xmlDocument, node);
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
                Factory.CreateTweakableObject(Factory.TextureFactory).WriteToXmlFile(xmlDocument, node);
            }
        }

        private void RegisterEffect(XmlNode node)
        {
            builder.AddEffect(GetStringAttribute(node, "class"), GetStringAttribute(node, "name"),
                GetIntAttribute(node, "track"), GetFloatAttribute(node, "starttime"), GetFloatAttribute(node, "endtime"));
            int index = Target.GetAllRegisterables().FindIndex(delegate(IRegisterable r) { return r.Name == GetStringAttribute(node, "name"); });
            GetChild(index).ReadFromXmlFile(node);
        }

        private void RegisterPostEffect(XmlNode node)
        {
            builder.AddPostEffect(GetStringAttribute(node, "class"), GetStringAttribute(node, "name"),
                GetIntAttribute(node, "track"), GetFloatAttribute(node, "starttime"), GetFloatAttribute(node, "endtime"));
            int index = Target.GetAllRegisterables().FindIndex(delegate(IRegisterable r) { return r.Name == GetStringAttribute(node, "name"); });
            GetChild(index).ReadFromXmlFile(node);
        }

    }
}
