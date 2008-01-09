using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.SceneGraph;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableContainer : ITweakableContainer, ITweakable
    {
        private string name;
        private List<PropertyInfo> properties;
        private List<float> steps;

        public string Name
        {
            get { return name; }
        }

        public TweakableContainer(string name)
        {
            this.name = name;
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
                case "Vector2":
                    return TweakableType.Vector2;
                case "Vector3":
                    return TweakableType.Vector3;
                case "Vector4":
                    return TweakableType.Vector4;
                case "Color":
                    return TweakableType.Color;
                case "String":
                    return TweakableType.String;
                case "Boolean":
                    return TweakableType.Bool;
                case "Scene": 
                    return TweakableType.IScene;
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

        public Vector2 GetVector2Value(int num)
        {
            return (Vector2)properties[num].GetGetMethod().Invoke(this, null);
        }

        public Vector3 GetVector3Value(int num)
        {
            return (Vector3)properties[num].GetGetMethod().Invoke(this, null);
        }

        public Vector4 GetVector4Value(int num)
        {
            return (Vector4)properties[num].GetGetMethod().Invoke(this, null);
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

        public string GetTweakableName(int index)
        {
            return properties[index].Name;
        }

        public void UpdateListener(IEffectChangeListener changeListener)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                if (properties[i].Name == "StartTime")
                    changeListener.SetStartTime(GetType().Name, name, GetFloatValue(i));
                else if (properties[i].Name == "EndTime")
                    changeListener.SetEndTime(GetType().Name, name, GetFloatValue(i));
                else
                {
                    switch (GetTweakableType(i))
                    {
                        case TweakableType.Integer:
                            changeListener.SetIntParam(GetType().Name,
                                name, GetTweakableName(i), GetIntValue(i));
                            break;
                        case TweakableType.Float:
                            changeListener.SetFloatParam(GetType().Name,
                                name, GetTweakableName(i), GetFloatValue(i));
                            break;
                        case TweakableType.String:
                            changeListener.SetStringParam(GetType().Name,
                                name, GetTweakableName(i), GetStringValue(i));
                            break;
                        case TweakableType.Vector2:
                            changeListener.SetVector2Param(GetType().Name,
                                name, GetTweakableName(i), GetVector2Value(i));
                            break;
                        case TweakableType.Vector3:
                            changeListener.SetVector3Param(GetType().Name,
                                name, GetTweakableName(i), GetVector3Value(i));
                            break;
                        case TweakableType.Vector4:
                            changeListener.SetVector4Param(GetType().Name,
                                name, GetTweakableName(i), GetVector4Value(i));
                            break;
                        case TweakableType.Color:
                            changeListener.SetColorParam(GetType().Name,
                                name, GetTweakableName(i), GetColorValue(i));
                            break;
                        case TweakableType.Bool:
                            changeListener.SetBoolParam(GetType().Name,
                                name, GetTweakableName(i), GetBoolValue(i));
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
                typeof(Vector2),
                typeof(Vector3),
                typeof(Vector4),
                typeof(string),
                typeof(Color),
                typeof(bool),
                typeof(IScene)
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

        #region ITweakable Members

        public int NumVisableVariables
        {
            get { return 15; }
        }

        public int NumVariables
        {
            get { return GetNumTweakables(); }
        }

        public string GetVariableName(int index)
        {
            return GetTweakableName(index);
        }

        public ITweakable GetVariable(int index)
        {
            return null;
        }

        public void CreateVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            Color boxColor;
            byte alpha;
            float height = status.VariableSpacing * 0.9f;
            GetColors(index, status.Selection, out boxColor, out alpha);
            if (boxColor != Color.Black)
                new BoxControl(new Vector4(0, y, 1, height), alpha, Color.Black, status.RootControl);
            new TextControl(GetTweakableName(index), new Vector4(0, y, 0.45f, height), TextFormatting.Right | TextFormatting.VerticalCenter, alpha, Color.White, status.RootControl);
            CreateValueControls(status, index, alpha, 0.55f, y, 0.45f, height);
        }

        private void CreateValueControls(TweakerStatus status, int num, byte alpha, float x, float y, float w, float h)
        {
            switch (GetTweakableType(num))
            {
                case TweakableType.Float:
                    new TextControl(GetFloatValue(num).ToString("N6", System.Globalization.CultureInfo.InvariantCulture), new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 0, alpha), status.RootControl);
                    break;
                case TweakableType.Integer:
                    new TextControl(GetIntValue(num).ToString(System.Globalization.CultureInfo.InvariantCulture), new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 0, alpha), status.RootControl);
                    break;
                case TweakableType.Vector2:
                    new TextControl("X: " + GetVector2Value(num).X.ToString("N3", System.Globalization.CultureInfo.InvariantCulture), new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 0, alpha), status.RootControl);
                    new TextControl("Y: " + GetVector2Value(num).Y.ToString("N3", System.Globalization.CultureInfo.InvariantCulture), new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 1, alpha), status.RootControl);
                    break;
                case TweakableType.Vector3:
                    new TextControl("X: " + GetVector3Value(num).X.ToString("N3", System.Globalization.CultureInfo.InvariantCulture), new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 0, alpha), status.RootControl);
                    new TextControl("Y: " + GetVector3Value(num).Y.ToString("N3", System.Globalization.CultureInfo.InvariantCulture), new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 1, alpha), status.RootControl);
                    new TextControl("Z: " + GetVector3Value(num).Z.ToString("N3", System.Globalization.CultureInfo.InvariantCulture), new Vector4(x + 2 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 2, alpha), status.RootControl);
                    break;
                case TweakableType.Vector4:
                    new TextControl("X: " + GetVector4Value(num).X.ToString("N3", System.Globalization.CultureInfo.InvariantCulture), new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 0, alpha), status.RootControl);
                    new TextControl("Y: " + GetVector4Value(num).Y.ToString("N3", System.Globalization.CultureInfo.InvariantCulture), new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 1, alpha), status.RootControl);
                    new TextControl("Z: " + GetVector4Value(num).Z.ToString("N3", System.Globalization.CultureInfo.InvariantCulture), new Vector4(x + 2 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 2, alpha), status.RootControl);
                    new TextControl("W: " + GetVector4Value(num).W.ToString("N3", System.Globalization.CultureInfo.InvariantCulture), new Vector4(x + 3 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 3, alpha), status.RootControl);
                    break;
                case TweakableType.Color:
                    new TextControl("R: " + GetColorValue(num).R.ToString(), new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 0, alpha), status.RootControl);
                    new TextControl("G: " + GetColorValue(num).G.ToString(), new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 1, alpha), status.RootControl);
                    new TextControl("B: " + GetColorValue(num).B.ToString(), new Vector4(x + 2 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 2, alpha), status.RootControl);
                    new TextControl("A: " + GetColorValue(num).A.ToString(), new Vector4(x + 3 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 3, alpha), status.RootControl);
                    Color alphaColor = new Color(GetColorValue(num).A, GetColorValue(num).A, GetColorValue(num).A, GetColorValue(num).A);
                    new BoxControl(new Vector4(x + 4 * w / 5 + 0 * w / 10, y, w / 10, h), 255, GetColorValue(num), status.RootControl);
                    new BoxControl(new Vector4(x + 4 * w / 5 + 1 * w / 10, y, w / 10, h), 255, alphaColor, status.RootControl);
                    break;
                case TweakableType.String:
                    new TextControl(GetStringValue(num), new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 0, alpha), status.RootControl);
                    break;
                case TweakableType.Bool:
                    new TextControl(GetBoolValue(num) ? "true" : "false", new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(status, 0, alpha), status.RootControl);
                    break;
                default:
                    break;
            }
        }

        private Color GetSelectionColor(TweakerStatus status, int index, byte alpha)
        {
            if (index == status.Index && alpha == 200)
                return Color.Red;
            else
                return Color.White;
        }

        private void GetColors(int i, int selection, out Color boxColor, out byte alpha)
        {
            if (i == selection)
            {
                boxColor = Color.White;
                alpha = 200;
            }
            else
            {
                boxColor = Color.Black;
                alpha = 75;
            }
        }

        public void NextIndex(TweakerStatus status)
        {
            int dimension = 1;
            if (GetTweakableType(status.Selection) == TweakableType.Color)
                dimension = 4;
            else if (GetTweakableType(status.Selection) == TweakableType.Vector2)
                dimension = 2;
            else if (GetTweakableType(status.Selection) == TweakableType.Vector3)
                dimension = 3;
            else if (GetTweakableType(status.Selection) == TweakableType.Vector4)
                dimension = 4;
            status.Index++;
            if (status.Index == dimension)
                status.Index = 0;
        }

        public void IncreaseValue(TweakerStatus status)
        {
            ChangeValue(status, GetStepSize(status.Selection));
        }

        public void DecreaseValue(TweakerStatus status)
        {
            ChangeValue(status, -GetStepSize(status.Selection));
        }

        private void ChangeValue(TweakerStatus status, float stepValue)
        {
            switch (GetTweakableType(status.Selection))
            {
                case TweakableType.Float:
                    SetValue(status.Selection, GetFloatValue(status.Selection) + stepValue);
                    break;
                case TweakableType.Integer:
                    SetValue(status.Selection, GetIntValue(status.Selection) + (int)stepValue);
                    break;
                case TweakableType.Vector2:
                    SetValue(status.Selection, GetVector2Value(status.Selection) + 
                        new Vector2(status.Index == 0 ? stepValue : 0, status.Index == 1 ? stepValue : 0));
                    break;
                case TweakableType.Vector3:
                    SetValue(status.Selection, GetVector3Value(status.Selection) +
                        new Vector3(status.Index == 0 ? stepValue : 0, status.Index == 1 ? stepValue : 0, status.Index == 2 ? stepValue : 0));
                    break;
                case TweakableType.Vector4:
                    SetValue(status.Selection, GetVector4Value(status.Selection) +
                        new Vector4(status.Index == 0 ? stepValue : 0, status.Index == 1 ? stepValue : 0, status.Index == 2 ? stepValue : 0, status.Index == 3 ? stepValue : 0));
                    break;
                case TweakableType.Color:
                    Color color = GetColorValue(status.Selection);
                    SetValue(status.Selection, new Color(status.Index == 0 ? (byte)Math.Max(0, Math.Min(color.R + stepValue, 255)) : color.R,
                        status.Index == 1 ? (byte)Math.Max(0, Math.Min(color.G + stepValue, 255)) : color.G,
                        status.Index == 2 ? (byte)Math.Max(0, Math.Min(color.B + stepValue, 255)) : color.B,
                        status.Index == 3 ? (byte)Math.Max(0, Math.Min(color.A + stepValue, 255)) : color.A));
                    break;
                case TweakableType.String:
                    // Next value can not be performed on strings
                    break;
                case TweakableType.Bool:
                    SetValue(status.Selection, !GetBoolValue(status.Selection));
                    break;
                default:
                    throw new DDXXException("Not implemented for this TweakableType.");
            }
        }

        public void CreateBaseControls(TweakerStatus status, ITweakerSettings settings)
        {
            string displayText = "<No Input>";
            BoxControl inputBox = new BoxControl(new Vector4(0, 0.95f, 1, 0.05f),
                settings.Alpha, settings.TitleColor, status.RootControl);
            if (status.InputString != "")
                displayText = "Input: " + status.InputString;
            TextControl text = new TextControl(displayText, new Vector2(0.5f, 0.5f),
                TextFormatting.Center | TextFormatting.VerticalCenter, 255,
                Color.White, inputBox);
        }

        public void SetValue(TweakerStatus status)
        {
            float floatValue;
            byte byteValue;
            switch (GetTweakableType(status.Selection))
            {
                case TweakableType.Float:
                    SetValue(status.Selection, float.Parse(status.InputString, System.Globalization.NumberFormatInfo.InvariantInfo));
                    break;
                case TweakableType.Integer:
                    SetValue(status.Selection, int.Parse(status.InputString, System.Globalization.NumberFormatInfo.InvariantInfo));
                    break;
                case TweakableType.Vector2:
                    floatValue = float.Parse(status.InputString, System.Globalization.NumberFormatInfo.InvariantInfo);
                    Vector2 oldVector2 = GetVector2Value(status.Selection);
                    SetValue(status.Selection, new Vector2(
                        status.Index == 0 ? floatValue : oldVector2.X,
                        status.Index == 1 ? floatValue : oldVector2.Y));
                    break;
                case TweakableType.Vector3:
                    floatValue = float.Parse(status.InputString, System.Globalization.NumberFormatInfo.InvariantInfo);
                    Vector3 oldVector3 = GetVector3Value(status.Selection);
                    SetValue(status.Selection, new Vector3(
                        status.Index == 0 ? floatValue : oldVector3.X,
                        status.Index == 1 ? floatValue : oldVector3.Y,
                        status.Index == 2 ? floatValue : oldVector3.Z));
                    break;
                case TweakableType.Vector4:
                    floatValue = float.Parse(status.InputString, System.Globalization.NumberFormatInfo.InvariantInfo);
                    Vector4 oldVector4 = GetVector4Value(status.Selection);
                    SetValue(status.Selection, new Vector4(
                        status.Index == 0 ? floatValue : oldVector4.X,
                        status.Index == 1 ? floatValue : oldVector4.Y,
                        status.Index == 2 ? floatValue : oldVector4.Z,
                        status.Index == 3 ? floatValue : oldVector4.W));
                    break;
                case TweakableType.Color:
                    byteValue = byte.Parse(status.InputString, System.Globalization.NumberFormatInfo.InvariantInfo);
                    Color color = GetColorValue(status.Selection);
                    SetValue(status.Selection, new Color(
                        status.Index == 0 ? byteValue : color.R,
                        status.Index == 1 ? byteValue : color.G,
                        status.Index == 2 ? byteValue : color.B,
                        status.Index == 3 ? byteValue : color.A));
                    break;
                case TweakableType.String:
                    SetValue(status.Selection, status.InputString);
                    break;
                case TweakableType.Bool:
                    SetValue(status.Selection, bool.Parse(status.InputString));
                    break;
                default:
                    throw new DDXXException("Not implemented for this TweakableType.");
            }
        }

        #endregion
    }
}
