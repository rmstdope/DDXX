using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

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
            get { return 13; }
        }

        protected override int NumSpecificVariables
        {
            get { return Target.Texture2DParameters.Count; }
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            return Factory.CreateTweakableObject(Target.Texture2DParameters[index]);
        }

        protected override void ParseSpecficXmlNode(XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void WriteSpecificXmlNode(XmlDocument xmlDocument, XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void CreateControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            if (index == status.Selection)
                new BoxControl(new Vector4(0, y, 1, height), GetAlpha(status, index), Color.Black, status.RootControl);
            new TextControl("TextureFactory", new Vector4(0, y, 0.45f, height), TextFormatting.Right | TextFormatting.VerticalCenter, GetAlpha(status, index), Color.White, status.RootControl);

            new TextControl("<ITextureFactory>",
                new Vector4(0.55f, y, 0.45f, height), TextFormatting.Center | TextFormatting.VerticalCenter,
                GetAlpha(status, index), GetTextColor(status, index, 0), status.RootControl);
        }
    
        protected byte GetAlpha(TweakerStatus status, int selection)
        {
            if (selection == status.Selection)
                return 200;
            return 75;
        }

    }
}
