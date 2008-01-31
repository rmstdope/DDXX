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

        public TweakableDemo(IDemoRegistrator target, IDemoEffectBuilder builder, ITweakableFactory factory)
            : base(target, factory)
        {
            this.builder = builder;
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
                case "Texture":
                case "Generator":
                    break;
                case "TextureFactory":
                    Factory.CreateTweakableObject(Target.TextureFactory).ReadFromXmlFile(node);
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
                case "Texture":
                case "Generator":
                    break;
                default:
                    throw new DDXXException("Invalid XML element " + node.Name +
                        " within element " + node.Name + ".");
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
