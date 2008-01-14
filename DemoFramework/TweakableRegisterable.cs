using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableRegisterable : TweakableObjectBase<IRegisterable>
    {
        public TweakableRegisterable(IRegisterable target)
            : base(target)
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

        protected override string SpecificVariableName(int index)
        {
            throw new Exception("The method should never be called.");
        }

        protected override ITweakableObject GetSpecificVariable(int index)
        {
            throw new Exception("The method should never be called.");
        }

        protected override void CreateSpecificVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            throw new Exception("The method should never be called.");
        }
    }
}
