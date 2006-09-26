using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Dope.DDXX.DemoFramework
{
    using TypeDict = Dictionary<string, Type>;
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

        public void Initialize(Assembly assembly)
        {
            types.Clear();
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
                throw new Exception(e.ToString() + "LoaderExceptions:" + e.LoaderExceptions.ToString());
            }
        }

        public IRegisterable CreateInstance(string effectName, float startTime, float endTime)
        {
            Type effect;
            if (!types.TryGetValue(effectName, out effect))
                return null;
            ConstructorInfo constrInfo = effect.GetConstructor(Type.EmptyTypes);
            if (constrInfo == null)
                return null;
            IRegisterable demoEffect = (IRegisterable)constrInfo.Invoke(null);
            demoEffect.StartTime = startTime;
            demoEffect.EndTime = endTime;
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
    }
}
