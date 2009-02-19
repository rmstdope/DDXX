using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Dope.DDXX.UserInterface;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableTextureGenerator : TweakableObjectBase<ITextureGenerator>
    {
        private ITexture2D texture;

        public TweakableTextureGenerator(ITextureGenerator target, ITweakableFactory factory)
            : base(target, factory)
        {
            texture = Factory.GraphicsFactory.TextureFactory.CreateFromGenerator("", 32, 32, 1, TextureUsage.None, SurfaceFormat.Color, Target);
        }

        public override int NumVisableVariables
        {
            get { return 13; }
        }

        protected override int NumSpecificVariables
        {
            get { return 0; }
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            throw new Exception("The method or operation is not implemented.");
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
                new BoxControl(new Vector4(0, y, 1, height), settings.Alpha, settings.SelectedColor, status.RootControl);
            new TextControl(Target.GetType().Name, new Vector4(0, y, 0.45f, height), Positioning.Right | Positioning.VerticalCenter, settings.TextAlpha, Color.White, status.RootControl);

            int i = 0;// Rand.Int(0, 1);
            new BoxControl(new Vector4(0.55f - height / 2 * i + 0.225f - height / 2, y, height / 2, height), 255, texture, status.RootControl);
        }

    }
}
