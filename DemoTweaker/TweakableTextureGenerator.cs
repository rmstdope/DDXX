using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Dope.DDXX.UserInterface;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoTweaker
{
    public class TweakableTextureGenerator : TweakableObjectBase<ITextureGenerator>
    {
        private ITexture2D texture;

        public TweakableTextureGenerator(ITextureGenerator target, ITweakableFactory factory)
            : base(target, factory)
        {
            texture = Factory.GraphicsFactory.TextureFactory.CreateFromGenerator("", 64, 64, 1, TextureUsage.None, SurfaceFormat.Color, Target);
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
            float sizeY = status.VariableSpacing * 0.9f;
            float halfSizeY = sizeY / 2;
            float sizeX = sizeY / 2;
            float halfSizeX = sizeX / 2;
            float spacingY = status.VariableSpacing * 0.1f;
            float spacingX = sizeX * 0.1f;
            if (index == status.Selection)
                new BoxControl(new Vector4(0, y, 1, sizeY), settings.Alpha, settings.SelectedColor, status.RootControl);
            new TextControl(Target.GetType().Name, new Vector4(0, y, 0.45f, sizeY), Positioning.Right | Positioning.VerticalCenter, settings.TextAlpha, Color.White, status.RootControl);

            int indent = 1 - GetIndentation(Target);
            float x = 0.55f - (sizeX + spacingX) * indent + 0.225f - sizeY / 2;
            new BoxControl(new Vector4(x, y, sizeY / 2, sizeY), 255, texture, status.RootControl);
            for (int i = 0; i < Target.NumInputPins; i++)
            {
                int inputIndent = 1 - GetIndentation(Target.GetInput(i));
                if (inputIndent == indent)
                {
                    new LineControl(new Vector4(x + halfSizeX, y - spacingY, 0, spacingY), 
                        255, Color.White, status.RootControl);
                }
                else
                {
                    new LineControl(new Vector4(x + sizeX, y + halfSizeY, spacingX + halfSizeX, 0), 
                        255, Color.White, status.RootControl);
                    new LineControl(new Vector4(x + sizeX + spacingX + halfSizeX, y + halfSizeY - sizeY, 0, sizeY), 
                        255, Color.White, status.RootControl);
                }
            }
        }

        private int GetIndentation(ITextureGenerator generator)
        {
            if (generator.Output != null)
            {
                int index = generator.Output.GetInputIndex(generator);
                return index + GetIndentation(generator.Output);
            }
            return 0;
        }

    }
}
