using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Microsoft.Xna.Framework.Input;
using Dope.DDXX.UserInterface;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableTexture2DParameters : TweakableObjectBase<Texture2DParameters>
    {
        private List<List<ITextureGenerator>> generators = new List<List<ITextureGenerator>>();

        public TweakableTexture2DParameters(Texture2DParameters target, ITweakableFactory factory)
            : base(target, factory)
        {
            ITextureGenerator generator = Target.Generator;
            while (generator != null)
            {
                generators.Add(new List<ITextureGenerator>());
                generators[generators.Count - 1].Add(generator);
                if (generator.NumInputPins > 0)
                    generator = generator.GetInput(0);
                else
                    generator = null;
            }
        }

        public override int NumVisableVariables
        {
            get { return 5; }
        }

        protected override int NumSpecificVariables
        {
            get { return generators.Count; }
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            return Factory.CreateTweakableObject(generators[index][0]);
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
            new TextControl(Target.Name + " (<ITexture2D>)", new Vector4(0, y, 0.45f, height), Positioning.Right | Positioning.VerticalCenter, settings.TextAlpha, Color.White, status.RootControl);

            new BoxControl(new Vector4(0.55f + 0.225f - height / 2, y, height / 2, height), 255, Target.Texture, status.RootControl);
        }

    }
}
