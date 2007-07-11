using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoEffectTypes
    {
        IDictionary<string, Type> IRegisterables { get; }
        IDictionary<string, Type> IGenerators { get; }
        IRegisterable CreateInstance(string className, string effectName, float startTime, float endTime);
        IGenerator CreateGenerator(string name);
        void SetProperty(object asset, string name, object value);
        object GetProperty(IRegisterable ei, string name);
        void CallSetup(object asset, string method, List<object> parameters);
    }
}
