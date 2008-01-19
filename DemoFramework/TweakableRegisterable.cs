using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Dope.DDXX.Utility;

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
            get { return 15; }
        }

        protected override int NumSpecificVariables
        {
            get { return 0; }
        }

        protected override ITweakableObject GetSpecificVariable(int index)
        {
            throw new Exception("The method should never be called.");
        }

        protected override void CreateSpecificVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            throw new Exception("The method should never be called.");
        }

        protected override void ParseSpecficXmlNode(XmlNode node)
        {
            if (!(node is XmlElement))
                return;
            bool handled = false;
            foreach (ITweakableValue handler in PropertyHandlers)
            {
                if (node.Name == handler.Property.Name)
                {
                    handler.SetFromString(node.InnerText);
                    handled = true;
                }
            }
            if (!handled)
                throw new DDXXException("Missing property " + node.Name + " in class " + Target.GetType().Name);
        }

        protected override void WriteSpecificXmlNode(XmlNode node)
        {
        }

    }
}
