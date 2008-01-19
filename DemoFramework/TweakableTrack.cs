using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableTrack : TweakableObjectBase<ITrack>
    {
        public TweakableTrack(ITrack target, ITweakableFactory factory)
            : base(target, factory)
        {
        }

        public override int NumVisableVariables
        {
            get { return 13; }
        }

        protected override int NumSpecificVariables
        {
            get { return GetAllRegisterables().Length; }
        }

        protected override ITweakableObject GetSpecificVariable(int index)
        {
            return Factory.CreateTweakableObject(GetAllRegisterables()[index]);
        }

        protected override bool ParseShouldTraverseChildren
        {
            get { return false; }
        }

        protected override void CreateSpecificVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            Color boxColor = GetBoxColor(status, index, settings);
            Color textColor = GetTextColor(status, index, -1);
            IRegisterable[] allTweakables = GetAllRegisterables();            
            float ex1 = ((allTweakables[index] as IRegisterable).StartTime - status.StartTime) / status.TimeScale;
            if (ex1 < 0)
                ex1 = 0;
            float ex2 = ((allTweakables[index] as IRegisterable).EndTime - status.StartTime) / status.TimeScale;
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

        private IRegisterable[] GetAllRegisterables()
        {
            IRegisterable[] allEffects = new IRegisterable[Target.Effects.Length + Target.PostEffects.Length];
            Array.Copy(Target.Effects, allEffects, Target.Effects.Length);
            Array.Copy(Target.PostEffects, 0, allEffects, Target.Effects.Length, Target.PostEffects.Length);
            return allEffects;
        }

        protected override void ParseSpecficXmlNode(XmlNode node)
        {
            IRegisterable[] allEffects = GetAllRegisterables();
            for (int i = 0; i < allEffects.Length; i++)
            {
                if (allEffects[i].Name == GetStringAttribute(node, "name"))
                    (GetTweakableChild(i) as ITweakableObject).ReadFromXmlFile(node);
            }
        }

        protected override void WriteSpecificXmlNode(XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}