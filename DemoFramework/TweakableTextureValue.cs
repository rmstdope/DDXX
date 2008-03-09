using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableTextureValue : TweakableProperty<ITexture2D>
    {
        public TweakableTextureValue(PropertyInfo property, object target, ITweakableFactory factory)
            : base(property, target, factory)
        {
        }

        protected override void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h, ITweakerSettings settings)
        {
            float xPos = x + w / 2 - h / 2;
            BaseControl rightBox = new BoxControl(new Vector4(x + w / 2, y, w / 2, h), 0, Color.White, status.RootControl);
            new TextControl(TextureName, new Vector4(x, y, w / 2, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                settings.TextAlpha, GetTextColor(status, index, 0), status.RootControl);
            new BoxControl(new Vector4(0, 0, -1, 1), 255, Value, rightBox);
        }

        private string TextureName
        {
            get
            {
                Texture2DParameters parameter = Factory.TextureFactory.Texture2DParameters.Find(delegate(Texture2DParameters param)
                {
                    return param.Texture == Value;
                });
                if (parameter != null)
                    return parameter.Name;
                return "<null>";
            }
        }

        public override int Dimension
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override void IncreaseValue(int selectionIndex)
        {
            int index = CorrectIndexBounds(TextureIndex + 1);
            SetValueFromIndex(index);
        }

        public override void DecreaseValue(int selectionIndex)
        {
            int index = CorrectIndexBounds(TextureIndex - 1);
            SetValueFromIndex(index);
        }

        private void SetValueFromIndex(int index)
        {
            if (index == -1)
                Value = null;
            else
                Value = Factory.TextureFactory.Texture2DParameters[index].Texture;
        }

        private int CorrectIndexBounds(int index)
        {
            if (index < -1)
                index = Factory.TextureFactory.Texture2DParameters.Count - 1;
            if (index >= Factory.TextureFactory.Texture2DParameters.Count)
                index = -1;
            return index;
        }

        private int TextureIndex
        {
            get
            {
                return Factory.TextureFactory.Texture2DParameters.FindIndex(delegate(Texture2DParameters param)
                {
                    return param.Texture == Value;
                });
            }
        }

        public override void SetFromString(string value)
        {
            if (value == "")
            {
                Value = null;
            }
            else
            {
                Value = Factory.TextureFactory.CreateFromName(value);
            }
        }

        public override void SetFromString(int index, string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetToString()
        {
            if (TextureIndex < 0)
                return "";
            return Factory.TextureFactory.Texture2DParameters[TextureIndex].Name;
        }

    }
}
