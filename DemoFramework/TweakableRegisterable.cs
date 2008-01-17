using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

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

        public override void ReadFromXmlFile(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                foreach (ITweakableValue handler in PropertyHandlers)
                {
                    if (child.Name == handler.Property.Name)
                        handler.SetFromString(child.InnerText);
                }
            }
        }
    }
}
