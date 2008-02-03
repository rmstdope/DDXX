using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableTextureFactory : TweakableObjectBase<ITextureFactory>
    {
        public TweakableTextureFactory(ITextureFactory target, ITweakableFactory factory)
            : base(target, factory)
        {
        }

        public override int NumVisableVariables
        {
            get { return 5; }
        }

        protected override int NumSpecificVariables
        {
            get { return TextureParameters.Count; }
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            return Factory.CreateTweakableObject(TextureParameters[index]);
        }

        protected override void ParseSpecficXmlNode(XmlNode node)
        {
            if (node.Name != "Texture")
                throw new DDXXException("Unknown node \"" + node.Name + "\" in TextureFactory");
            RegisterTexture(node);
        }

        protected override void WriteSpecificXmlNode(XmlDocument xmlDocument, XmlNode node)
        {
            Texture2DParameters parameter = TextureParameters.Find(delegate(Texture2DParameters param) { return param.Name == GetStringAttribute(node, "name"); });
            if (parameter != null)
            {
                UpdateTexture(xmlDocument, node, parameter);
            }
        }

        protected override void WriteNewNodes(XmlDocument xmlDocument, XmlNode node)
        {
            foreach (Texture2DParameters parameter in TextureParameters)
            {
                bool found = false;
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.Name == "Texture" && GetStringAttribute(child, "name") == parameter.Name)
                        found = true;
                }
                if (!found)
                {
                    XmlNode newNode = InsertNewXmlNode(xmlDocument, node, "Texture");
                    XmlAttribute newAttribute = xmlDocument.CreateAttribute("name");
                    newAttribute.Value = parameter.Name;
                    newNode.Attributes.Append(newAttribute);
                    UpdateTexture(xmlDocument, newNode, parameter);
                }
            }
        }

        private void UpdateTexture(XmlDocument xmlDocument, XmlNode newNode, Texture2DParameters parameter)
        {
            //ITweakable tweakableGenerator = Factory.CreateTweakableObject(parameter);
            //tweakableGenerator.WriteToXmlFile(xmlDocument, newNode);
        }

        private void RegisterTexture(XmlNode node)
        {
            TextureDirector director = new TextureDirector(Target);
            foreach (XmlNode child in node.ChildNodes)
            {
                if (!(child is XmlElement))
                    continue;
                if (child.Name != "Generator")
                    throw new DDXXException("Unknown node \"" + child.Name + "\" in Texture");
                ITextureGenerator generator = Factory.EffectTypes.CreateGenerator(GetStringAttribute(child, "class"));
                director.AddGenerator(generator);
                ITweakable tweakableGenerator = Factory.CreateTweakableObject(generator);
                tweakableGenerator.ReadFromXmlFile(child);
            }
            director.Generate(GetStringAttribute(node, "name"), GetIntAttribute(node, "width"),
                GetIntAttribute(node, "height"), GetIntAttribute(node, "miplevels"), SurfaceFormat.Color);
        }

        public override void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            if (index == status.Selection)
                new BoxControl(new Vector4(0, y, 1, height), settings.Alpha, settings.SelectedColor, status.RootControl);
            new TextControl("TextureFactory", new Vector4(0, y, 0.45f, height), TextFormatting.Right | TextFormatting.VerticalCenter, settings.TextAlpha, Color.White, status.RootControl);

            new TextControl("<ITextureFactory>",
                new Vector4(0.55f, y, 0.45f, height), TextFormatting.Center | TextFormatting.VerticalCenter,
                settings.TextAlpha, Color.White, status.RootControl);
        }

        private List<Texture2DParameters> TextureParameters
        {
            get
            {
                return Target.Texture2DParameters.FindAll(delegate(Texture2DParameters param) { return param.IsGenerated; });
            }
        }
    }
}
