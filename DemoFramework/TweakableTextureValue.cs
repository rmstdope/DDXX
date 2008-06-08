using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.UserInterface;

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
            new TextControl(GetTextureName(),
                new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                settings.TextAlpha, GetTextColor(status, index, 0), status.RootControl);
        }

        private string GetTextureName()
        {
            Texture2DParameters parameter = Factory.TextureFactory.Texture2DParameters.Find(delegate(Texture2DParameters param) { return param.Texture == Value; });
            if (parameter == null)
                return "";
            return parameter.Name;
        }

        private int GetTextureIndex()
        {
            return Factory.TextureFactory.Texture2DParameters.IndexOf(GetParameters());
        }

        private Texture2DParameters GetParameters()
        {
            return Factory.TextureFactory.Texture2DParameters.Find(delegate(Texture2DParameters param) { return param.Texture == Value; });
        }

        private int GetTextureIndexFromName(string name)
        {
            return Factory.TextureFactory.Texture2DParameters.IndexOf(Factory.TextureFactory.Texture2DParameters.Find(delegate(Texture2DParameters param) { return param.Name == name; }));
        }

        private int GetNumTextures()
        {
            return Factory.TextureFactory.Texture2DParameters.Count;
        }

        public override int Dimension
        {
            get { return 1; }
        }

        public override void IncreaseValue(int index)
        {
            int i = GetTextureIndex() + 1;
            if (i >= GetNumTextures())
                i = -1;
            SetTexture(i);
        }

        public override void DecreaseValue(int index)
        {
            int i = GetTextureIndex() - 1;
            if (i < -1)
                i = GetNumTextures() - 1;
            SetTexture(i);
        }

        private void SetTexture(int i)
        {
            if (i == -1)
                Value = null;
            else
                Value = Factory.TextureFactory.Texture2DParameters[i].Texture;
        }

        public override void SetFromString(string value)
        {
            SetTexture(GetTextureIndexFromName(value));
        }

        public override void SetFromString(int index, string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetToString()
        {
            return GetTextureName();
        }

    }
}
