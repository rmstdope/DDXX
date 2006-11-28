using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Dope.DDXX.DemoFramework
{
    using TypeDict = Dictionary<string, Type>;
    using Dope.DDXX.Utility;
    public class DemoEffectTypes
    {
        private TypeDict types = new TypeDict();

        public IDictionary<string, Type> Types
        {
            get
            {
                return types;
            }
        }

        public void Initialize(Assembly[] assemblies)
        {
            types.Clear();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly == null)
                    continue;
                try
                {
                    foreach (Type t in assembly.GetTypes())
                    {
                        TypeFilter filter = new TypeFilter(delegate(Type ty, object comp)
                        {
                            if (ty.FullName == (string)comp)
                                return true;
                            else
                                return false;
                        });
                        Type[] interfaces = t.FindInterfaces(filter, "Dope.DDXX.DemoFramework.IRegisterable");
                        if (interfaces.Length > 0)
                        {
                            types.Add(t.Name, t);
                        }
                    }
                }
                catch (ReflectionTypeLoadException e)
                {
                    throw new DDXXException(e.ToString() + "LoaderExceptions:" + e.LoaderExceptions.ToString());
                }
            }
        }

        public IRegisterable CreateInstance(string effectName, float startTime, float endTime)
        {
            Type effect;
            if (!types.TryGetValue(effectName, out effect))
                throw new DDXXException("Could not find effect " + effectName + " among valid effects.");
            ConstructorInfo constrInfo = effect.GetConstructor(new Type[] {typeof(float), typeof(float)});
            if (constrInfo == null)
                throw new DDXXException("Public constructor(float, float) not found in Effect " + effectName);
            IRegisterable demoEffect = (IRegisterable)constrInfo.Invoke(new object[] { startTime, endTime });
            return demoEffect;
        }

        public void SetProperty(IRegisterable ei, string name, object value)
        {
            ei.GetType().InvokeMember(name, BindingFlags.SetProperty, null, ei, new object[] { value }, null, null, null);
        }

        public object GetProperty(IRegisterable ei, string name)
        {
            return ei.GetType().InvokeMember(name, BindingFlags.GetProperty, null, ei, null, null, null, null);
        }

        public void CallSetup(IRegisterable ei, string method, List<object> parameters)
        {
            object[] arr = new object[parameters.Count];
            parameters.CopyTo(arr);
            ei.GetType().InvokeMember(method, BindingFlags.InvokeMethod, null, ei, arr);
        }
    }
}
