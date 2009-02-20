using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;
using Dope.DDXX.UserInterface;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoTweaker
{
    public class TweakableModelMeshPart : TweakableObjectBase<IModelMeshPart>
    {
        public TweakableModelMeshPart(IModelMeshPart target, ITweakableFactory factory)
            : base(target, factory)
        {
        }

        public override int NumVisableVariables
        {
            get { return 15; }
        }

        protected override int NumSpecificVariables
        {
            get { return 1; }
        }

        protected override ITweakable GetSpecificVariable(int index)
        {
            return Factory.CreateTweakableObject(Target.MaterialHandler);
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
            new TextControl("ModelMeshPart", new Vector4(0, y, 0.45f, height), Positioning.Right | Positioning.VerticalCenter, settings.TextAlpha, Color.White, status.RootControl);

            new TextControl("<IModelMeshPart>",
                new Vector4(0.55f, y, 0.45f, height), Positioning.Center | Positioning.VerticalCenter,
                settings.TextAlpha, Color.White, status.RootControl);
        }
    }
}
