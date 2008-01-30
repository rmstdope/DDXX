using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class DemoEffectTypes : IDemoEffectTypes
    {
        private Dictionary<string, Type> iRegisterables = new Dictionary<string, Type>();
        private Dictionary<string, Type> iGenerators = new Dictionary<string, Type>();

        public IDictionary<string, Type> IRegisterables
        {
            get
            {
                return iRegisterables;
            }
        }

        public IDictionary<string, Type> IGenerators
        {
            get
            {
                return iGenerators;
            }
        }

        public DemoEffectTypes(Assembly[] assemblies)
        {
            iRegisterables.Clear();
            iGenerators.Clear();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly == null)
                    continue;
                //try
                //{
                    foreach (Type t in assembly.GetTypes())
                    {
                        foreach (Type type in t.GetInterfaces())
                        {
                            if (type.FullName == "Dope.DDXX.DemoFramework.IRegisterable")
                                iRegisterables.Add(t.Name, t);
                            if (type.FullName == "Dope.DDXX.TextureBuilder.IGenerator")
                                iGenerators.Add(t.Name, t);
                        }
                        //TypeFilter filter = new TypeFilter(delegate(Type ty, object comp)
                        //{
                        //    if (ty.FullName == (string)comp)
                        //        return true;
                        //    else
                        //        return false;
                        //});
                        //Type[] interfaces = t.FindInterfaces(filter, "Dope.DDXX.DemoFramework.IRegisterable");
                        //if (interfaces.Length > 0)
                        //{
                        //    iRegisterables.Add(t.Name, t);
                        //}
                        //interfaces = t.FindInterfaces(filter, "Dope.DDXX.TextureBuilder.IGenerator");
                        //if (interfaces.Length > 0)
                        //{
                        //    iGenerators.Add(t.Name, t);
                        //}
                    }
                //}
                //catch (ReflectionTypeLoadException e)
                //{
                //    throw new DDXXException(e.ToString() + "LoaderExceptions:" + e.LoaderExceptions.ToString());
                //}
            }
        }

        public IRegisterable CreateInstance(string className, string effectName, float startTime, float endTime)
        {
            Type effect;
            if (!iRegisterables.TryGetValue(className, out effect))
                throw new DDXXException("Could not find effect " + effectName + " among valid effects.");
            ConstructorInfo constrInfo = effect.GetConstructor(new Type[] {typeof(string), typeof(float), typeof(float)});
            if (constrInfo == null)
                throw new DDXXException("Public constructor(string, float, float) not found in Effect " + effectName);
            IRegisterable demoEffect = (IRegisterable)constrInfo.Invoke(new object[] { effectName, startTime, endTime });
            return demoEffect;
        }

        public ITextureGenerator CreateGenerator(string name)
        {
            Type type;
            if (!iGenerators.TryGetValue(name, out type))
                throw new DDXXException("Could not find generator " + name + " among valid generators.");
            ConstructorInfo constrInfo = type.GetConstructor(new Type[] { });
            if (constrInfo == null)
                throw new DDXXException("Public constructor() not found in generator " + name);
            ITextureGenerator generator = (ITextureGenerator)constrInfo.Invoke(new object[] { });
            return generator;
        }

        public void SetProperty(object asset, string name, object value)
        {
            Type t = asset.GetType();
            PropertyInfo property = t.GetProperty(name);
            if (property == null)
                throw new DDXXException("Type " + t.Name + " does not contain a property named " + name);
            MethodInfo method = property.GetSetMethod();
            method.Invoke(asset, new object[] { value });
            //t.InvokeMember(name, BindingFlags.SetProperty, null, asset, new object[] { value }, null, null, null);
        }

        public object GetProperty(IRegisterable ei, string name)
        {
            Type t = ei.GetType();
            PropertyInfo property = t.GetProperty(name);
            MethodInfo method = property.GetGetMethod();
            return method.Invoke(ei, new object[] { });
            //return ei.GetType().InvokeMember(name, BindingFlags.GetProperty, null, ei, null, null, null, null);
        }

        public void CallSetup(object asset, string method, List<object> parameters)
        {
            object[] arr = new object[parameters.Count];
            parameters.CopyTo(arr);
            asset.GetType().InvokeMember(method, BindingFlags.InvokeMethod, null, asset, arr);
        }
    }
}
