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
            get { return Target.GetAllRegisterables().Count; }
        }

        protected override ITweakableObject GetSpecificVariable(int index)
        {
            return Factory.CreateTweakableObject(Target.GetAllRegisterables()[index]);
        }

        protected override void CreateSpecificVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            Color boxColor = GetBoxColor(status, index, settings);
            Color textColor = GetTextColor(status, index, -1);
            List<IRegisterable> allTweakables = Target.GetAllRegisterables();
            float ex1 = (allTweakables[index].StartTime - status.StartTime) / status.TimeScale;
            if (ex1 < 0)
                ex1 = 0;
            float ex2 = (allTweakables[index].EndTime - status.StartTime) / status.TimeScale;
            if (ex2 > 1)
                ex2 = 1;
            if (ex1 < 1 && ex2 > 0)
            {
                BoxControl trackWindow = new BoxControl(new Vector4(ex1, y, ex2 - ex1, height), settings.Alpha, boxColor, status.RootControl);
                new TextControl(allTweakables[index].GetType().Name, new Vector4(0, 0, 1, 1), TextFormatting.Center | TextFormatting.VerticalCenter, settings.TextAlpha, textColor, trackWindow);
            }
            else if (ex1 >= 1.0f)
            {
                BoxControl trackWindow = new BoxControl(new Vector4(0, y, 1, height), 0, boxColor, status.RootControl);
                new TextControl(allTweakables[index].GetType().Name + "-->", new Vector4(0, 0, 1, 1), TextFormatting.Right | TextFormatting.VerticalCenter, settings.TextAlpha, textColor, trackWindow);
            }
            else
            {
                BoxControl trackWindow = new BoxControl(new Vector4(0, y, 1, height), 0, boxColor, status.RootControl);
                new TextControl("<--" + allTweakables[index].GetType().Name, new Vector4(0, 0, 1, 1), TextFormatting.Left | TextFormatting.VerticalCenter, settings.TextAlpha, textColor, trackWindow);
            }
        }

        protected override void ParseSpecficXmlNode(XmlNode node)
        {
            if (!(node is XmlElement))
                return;
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
                default:
                    throw new DDXXException("Invalid XML element " + node.Name + 
                        " within element " + node.Name + ".");
            }
        }

        protected override void WriteSpecificXmlNode(XmlNode node)
        {
        }

        private void RegisterEffect(XmlNode node)
        {
            builder.AddEffect(GetStringAttribute(node, "class"), GetStringAttribute(node, "name"),
                GetIntAttribute(node, "track"), GetFloatAttribute(node, "starttime"), GetFloatAttribute(node, "endtime"));
            int index = Target.GetAllRegisterables().FindIndex(delegate(IRegisterable r) { return r.Name == GetStringAttribute(node, "name"); });
            (GetTweakableChild(index) as ITweakableObject).ReadFromXmlFile(node);
        }

        private void RegisterPostEffect(XmlNode node)
        {
            builder.AddPostEffect(GetStringAttribute(node, "class"), GetStringAttribute(node, "name"),
                GetIntAttribute(node, "track"), GetFloatAttribute(node, "starttime"), GetFloatAttribute(node, "endtime"));
            int index = Target.GetAllRegisterables().FindIndex(delegate(IRegisterable r) { return r.Name == GetStringAttribute(node, "name"); });
            (GetTweakableChild(index) as ITweakableObject).ReadFromXmlFile(node);
        }

    }
}
