using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableContainer : ITweakableContainer
    {
        private List<PropertyInfo> properties;
        private List<float> steps;

        public TweakableContainer()
        {
            properties = GetProperties();
            steps = new List<float>();
            for (int i = 0; i < properties.Count; i++)
                steps.Add(1.0f);
        }

        #region ITweakableContainer Members

        public TweakableType GetTweakableType(int num)
        {
            switch (properties[num].PropertyType.Name)
            {
                case "Int32":
                    return TweakableType.Integer;
                case "Single":
                    return TweakableType.Float;
                case "Vector3":
                    return TweakableType.Vector3;
                case "Color":
                    return TweakableType.Color;
                case "String":
                    return TweakableType.String;
                case "Boolean":
                    return TweakableType.Bool;
                default:
                    return TweakableType.Unknown;
            }
        }

        public int GetTweakableNumber(string name)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i].Name == name)
                    return i;
            }
            throw new DDXXException("Could not find any property with name " + name +
                ". Make sure it has both get and set properties.");
        }

        public int GetNumTweakables()
        {
            return properties.Count;
        }

        public int GetIntValue(int num)
        {
            return (int)properties[num].GetGetMethod().Invoke(this, null);
        }

        public float GetFloatValue(int num)
        {
            return (float)properties[num].GetGetMethod().Invoke(this, null);
        }

        public Vector3 GetVector3Value(int num)
        {
            return (Vector3)properties[num].GetGetMethod().Invoke(this, null);
        }

        public string GetStringValue(int num)
        {
            return (string)properties[num].GetGetMethod().Invoke(this, null);
        }

        public Color GetColorValue(int num)
        {
            return (Color)properties[num].GetGetMethod().Invoke(this, null);
        }

        public bool GetBoolValue(int num)
        {
            return (bool)properties[num].GetGetMethod().Invoke(this, null);
        }

        public void SetValue(int num, object value)
        {
            properties[num].GetSetMethod().Invoke(this, new object[] { value });
        }

        public void SetStepSize(int num, float size)
        {
            steps[num] = size;
        }

        public float GetStepSize(int num)
        {
            return steps[num];
        }

        //public void SetStepSize(string variableName, float size)
        //{
        //    steps[
        //    throw new Exception("The method or operation is not implemented.");
        //}

        public string GetTweakableName(int index)
        {
            return properties[index].Name;
        }

        public void UpdateListener(IEffectChangeListener changeListener)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i].Name == "StartTime")
                    changeListener.SetStartTime(GetType().Name, GetFloatValue(i));
                else if (properties[i].Name == "EndTime")
                    changeListener.SetEndTime(GetType().Name, GetFloatValue(i));
                else
                {
                    switch (GetTweakableType(i))
                    {
                        case TweakableType.Integer:
                            changeListener.SetIntParam(GetType().Name, 
                                GetTweakableName(i), GetIntValue(i));
                            break;
                        case TweakableType.Float:
                            changeListener.SetFloatParam(GetType().Name, 
                                GetTweakableName(i), GetFloatValue(i));
                            break;
                        case TweakableType.String:
                            changeListener.SetStringParam(GetType().Name, 
                                GetTweakableName(i), GetStringValue(i));
                            break;
                        case TweakableType.Vector3:
                            changeListener.SetVector3Param(GetType().Name, 
                                GetTweakableName(i), GetVector3Value(i));
                            break;
                        case TweakableType.Color:
                            changeListener.SetColorParam(GetType().Name, 
                                GetTweakableName(i), GetColorValue(i));
                            break;
                        case TweakableType.Bool:
                            changeListener.SetBoolParam(GetType().Name,
                                GetTweakableName(i), GetBoolValue(i));
                            break;
                    }
                }
            }
        }

        #endregion

        private List<PropertyInfo> GetProperties()
        {
            List<Type> validTypes = new List<Type>(new Type[] { 
                typeof(int), 
                typeof(float),
                typeof(Vector3),
                typeof(string),
                typeof(Color),
                typeof(bool)
            });
            PropertyInfo[] array = this.GetType().GetProperties();
            List<PropertyInfo> list = new List<PropertyInfo>(array);
            list = list.FindAll(delegate(PropertyInfo info)
            {
                if (info.CanRead &&
                    info.CanWrite &&
                    validTypes.IndexOf(info.PropertyType) != -1)
                    return true;
                else
                    return false;
            });
            return list;
        }

    }
}
