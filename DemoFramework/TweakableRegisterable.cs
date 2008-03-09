using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableRegisterable : TweakableObjectBase<IRegisterable>
    {
        public TweakableRegisterable(IRegisterable target, ITweakableFactory factory)
            : base(target, factory)
        {
        }

        public override int NumVisableVariables
        {
            get { return 10; }
        }

        protected override int NumSpecificVariables
        {
            get { return 0; }
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            throw new Exception("The method should never be called.");
        }

        protected override void ParseSpecficXmlNode(XmlNode node)
        {
            throw new DDXXException("Missing property " + node.Name + " in class " + Target.GetType().Name);
        }

        protected override void WriteSpecificXmlNode(XmlDocument xmlDocument, XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            Color boxColor = GetBoxColor(status, index, settings);
            Color textColor = GetTextColor(status, index, -1);
            float ex1 = (Target.StartTime - status.StartTime) / status.TimeScale;
            if (ex1 < 0)
                ex1 = 0;
            float ex2 = (Target.EndTime - status.StartTime) / status.TimeScale;
            if (ex2 > 1)
                ex2 = 1;
            if (ex1 < 1 && ex2 > 0)
            {
                BoxControl trackWindow = new BoxControl(new Vector4(ex1, y, ex2 - ex1, height), settings.Alpha, boxColor, status.RootControl);
                new TextControl(Target.GetType().Name, new Vector4(0, 0, 1, 1), TextFormatting.Center | TextFormatting.VerticalCenter, settings.TextAlpha, textColor, trackWindow);
            }
            else if (ex1 >= 1.0f)
            {
                BoxControl trackWindow = new BoxControl(new Vector4(0, y, 1, height), 0, boxColor, status.RootControl);
                new TextControl(Target.GetType().Name + "-->", new Vector4(0, 0, 1, 1), TextFormatting.Right | TextFormatting.VerticalCenter, settings.TextAlpha, textColor, trackWindow);
            }
            else
            {
                BoxControl trackWindow = new BoxControl(new Vector4(0, y, 1, height), 0, boxColor, status.RootControl);
                new TextControl("<--" + Target.GetType().Name, new Vector4(0, 0, 1, 1), TextFormatting.Left | TextFormatting.VerticalCenter, settings.TextAlpha, textColor, trackWindow);
            }
        }
    }
}
