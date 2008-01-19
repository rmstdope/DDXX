using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Dope.DDXX.DemoFramework
{
    public interface ITweakableValue : ITweakable
    {
        int Dimension { get; }
        void IncreaseValue(int index);
        void DecreaseValue(int index);
        void SetFromString(string value);
        void SetFromString(int index, string value);
        PropertyInfo Property { get; }
        string GetToString();
    }
}
