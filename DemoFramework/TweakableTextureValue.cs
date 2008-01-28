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
        public TweakableTextureValue(PropertyInfo property, object target)
            : base(property, target)
        {
        }

        protected override void CreateValueControls(TweakerStatus status, int index, float x, float y, float w, float h)
        {
            new TextControl("<ITexture2D>",
                new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter,
                GetAlpha(status, index), GetTextColor(status, index, 0), status.RootControl);
        }

        public override int Dimension
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override void IncreaseValue(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DecreaseValue(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetFromString(string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetFromString(int index, string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetToString()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int NumVisableVariables
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override int NumVariables
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override ITweakable GetTweakableChild(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool IsObject()
        {
            return true;
        }

        public override void CreateBaseControls(TweakerStatus status, ITweakerSettings settings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void NextIndex(TweakerStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void IncreaseValue(TweakerStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DecreaseValue(TweakerStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetValue(TweakerStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ReadFromXmlFile(System.Xml.XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void WriteToXmlFile(System.Xml.XmlDocument xmlDocument, System.Xml.XmlNode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
