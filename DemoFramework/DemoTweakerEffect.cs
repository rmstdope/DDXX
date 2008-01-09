using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Dope.DDXX.Input;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweakerEffect : DemoTweakerBase, IDemoTweaker
    {
        private ITweakableContainer currentContainer;
        private BoxControl tweakableWindow;

        private int currentVariable;
        private int currentSelection;

        private string inputString = "";

        public ITweakableContainer CurrentContainer
        {
            get { return currentContainer; }
            set
            {
                if (value != currentContainer)
                    CurrentVariable = 0;
                currentContainer = value;
            }
        }

        public int CurrentVariable
        {
            get { return currentVariable; }
            set
            {
                if (value != currentVariable)
                    currentSelection = 0;
                currentVariable = value;
            }
        }

        public DemoTweakerEffect(ITweakerSettings settings)
            : base(settings)
        {
            currentVariable = 0;
            currentSelection = 0;
        }

        public void KeyTab()
        {
            int dimension = 1;
            if (currentContainer.GetTweakableType(CurrentVariable) == TweakableType.Color)
                dimension = 4;
            else if (currentContainer.GetTweakableType(CurrentVariable) == TweakableType.Vector2)
                dimension = 2;
            else if (currentContainer.GetTweakableType(CurrentVariable) == TweakableType.Vector3)
                dimension = 3;
            else if (currentContainer.GetTweakableType(CurrentVariable) == TweakableType.Vector4)
                dimension = 4;
            currentSelection++;
            if (currentSelection == dimension)
                currentSelection = 0;
        }

        private void KeyPageUp()
        {
            ChangeValue(currentContainer.GetStepSize(CurrentVariable));
        }

        private void KeyPageDown()
        {
            ChangeValue(-currentContainer.GetStepSize(CurrentVariable));
        }

        private void ChangeValue(float stepValue)
        {
            string variableName = currentContainer.GetTweakableName(CurrentVariable);

            switch (currentContainer.GetTweakableType(CurrentVariable))
            {
                case TweakableType.Float:
                    float newValue = (currentContainer.GetFloatValue(CurrentVariable) + stepValue);
                    currentContainer.SetValue(CurrentVariable, newValue);
                    break;
                case TweakableType.Integer:
                    currentContainer.SetValue(CurrentVariable, (currentContainer.GetIntValue(CurrentVariable) + (int)stepValue));
                    break;
                case TweakableType.Vector2:
                    Vector2 vector2 = currentContainer.GetVector2Value(currentVariable);
                    switch (currentSelection)
                    {
                        case 0:
                            vector2 += new Vector2(stepValue, 0);
                            break;
                        case 1:
                            vector2 += new Vector2(0, stepValue);
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, vector2);
                    break;
                case TweakableType.Vector3:
                    Vector3 vector3 = currentContainer.GetVector3Value(currentVariable);
                    switch (currentSelection)
                    {
                        case 0:
                            vector3 += new Vector3(stepValue, 0, 0);
                            break;
                        case 1:
                            vector3 += new Vector3(0, stepValue, 0);
                            break;
                        case 2:
                            vector3 += new Vector3(0, 0, stepValue);
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, vector3);
                    break;
                case TweakableType.Vector4:
                    Vector4 vector4 = currentContainer.GetVector4Value(currentVariable);
                    switch (currentSelection)
                    {
                        case 0:
                            vector4 += new Vector4(stepValue, 0, 0, 0);
                            break;
                        case 1:
                            vector4 += new Vector4(0, stepValue, 0, 0);
                            break;
                        case 2:
                            vector4 += new Vector4(0, 0, stepValue, 0);
                            break;
                        case 3:
                            vector4 += new Vector4(0, 0, 0, stepValue);
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, vector4);
                    break;
                case TweakableType.Color:
                    Color color = currentContainer.GetColorValue(CurrentVariable);
                    switch (currentSelection)
                    {
                        case 0:
                            color = new Color((byte)Math.Max(0, Math.Min(color.R + stepValue, 255)),
                                              color.G, color.B, color.A);
                            break;
                        case 1:
                            color = new Color(color.R,
                                              (byte)Math.Max(0, Math.Min(color.G + (int)stepValue, 255)),
                                              color.B, color.A);
                            break;
                        case 2:
                            color = new Color(color.R, color.G,
                                              (byte)Math.Max(0, Math.Min(color.B + (int)stepValue, 255)),
                                              color.A);
                            break;
                        case 3:
                            color = new Color(color.R, color.G, color.B,
                                              (byte)Math.Max(0, Math.Min(color.A + (int)stepValue, 255)));
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, color);
                    break;
                case TweakableType.String:
                    // Next value can not be performed on strings
                    break;
                case TweakableType.Bool:
                    currentContainer.SetValue(CurrentVariable, !currentContainer.GetBoolValue(CurrentVariable));
                    break;
                default:
                    throw new DDXXException("Not implemented for this TweakableType.");
            }
        }

        private void SetValue(float value)
        {
            switch (currentContainer.GetTweakableType(CurrentVariable))
            {
                case TweakableType.Float:
                    currentContainer.SetValue(CurrentVariable, value);
                    break;
                case TweakableType.Integer:
                    currentContainer.SetValue(CurrentVariable, (int)value);
                    break;
                case TweakableType.Vector2:
                    Vector2 vector2 = currentContainer.GetVector2Value(currentVariable);
                    switch (currentSelection)
                    {
                        case 0:
                            vector2.X = value;
                            break;
                        case 1:
                            vector2.Y = value;
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, vector2);
                    break;
                case TweakableType.Vector3:
                    Vector3 vector3 = currentContainer.GetVector3Value(currentVariable);
                    switch (currentSelection)
                    {
                        case 0:
                            vector3.X = value;
                            break;
                        case 1:
                            vector3.Y = value;
                            break;
                        case 2:
                            vector3.Z = value;
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, vector3);
                    break;
                case TweakableType.Vector4:
                    Vector4 vector4 = currentContainer.GetVector4Value(currentVariable);
                    switch (currentSelection)
                    {
                        case 0:
                            vector4.X = value;
                            break;
                        case 1:
                            vector4.Y = value;
                            break;
                        case 2:
                            vector4.Z = value;
                            break;
                        case 3:
                            vector4.W = value;
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, vector4);
                    break;
                case TweakableType.Color:
                    byte r = currentContainer.GetColorValue(CurrentVariable).R;
                    byte g = currentContainer.GetColorValue(CurrentVariable).G;
                    byte b = currentContainer.GetColorValue(CurrentVariable).B;
                    byte a = currentContainer.GetColorValue(CurrentVariable).A;
                    switch (currentSelection)
                    {
                        case 0:
                            r = (byte)Math.Max(0, Math.Min((int)value, 255));
                            break;
                        case 1:
                            g = (byte)Math.Max(0, Math.Min((int)value, 255));
                            break;
                        case 2:
                            b = (byte)Math.Max(0, Math.Min((int)value, 255));
                            break;
                        case 3:
                            a = (byte)Math.Max(0, Math.Min((int)value, 255));
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, new Color(a, r, g, b));
                    break;
                case TweakableType.String:
                    // Not applicable for this type.
                    break;
                default:
                    throw new DDXXException("Not implemented for this TweakableType.");
            }
        }

        private void KeyUp()
        {
            CurrentVariable--;
            if (CurrentVariable == -1)
                CurrentVariable++;
        }

        private void KeyDown()
        {
            CurrentVariable++;
            if (CurrentVariable == currentContainer.GetNumTweakables())
                CurrentVariable--;
        }

        #region IDemoTweaker Members

        public bool Quit
        {
            get { return false; }
        }

        public bool Exiting
        {
            get { return false; }
        }

        public void Draw()
        {
            DrawWindow();
            DrawInputWindow();

            UserInterface.DrawControl(MainWindow);
        }

        private void DrawInputWindow()
        {
            string displayText = "<No Input>";
            BoxControl inputBox = new BoxControl(new Vector4(0, 0.95f, 1, 0.05f),
                Settings.Alpha, Settings.TitleColor, MainWindow);
            if (inputString != "")
                displayText = "Input: " + inputString;
            TextControl text = new TextControl(displayText, new Vector2(0.5f, 0.5f),
                TextFormatting.Center | TextFormatting.VerticalCenter, 255,
                Color.White, inputBox);
        }

        private void DrawWindow()
        {
            const int NumVisableVariables = 13;
            //CreateBaseControls();
            tweakableWindow = new BoxControl(new Vector4(0, 0.05f, 1, 0.90f),
                Settings.Alpha, Settings.TimeColor, MainWindow);

            Color boxColor;
            //Color textColor;
            int startVariable = 0;
            float y = 0.05f;
            byte alpha;
            for (int i = startVariable; i < startVariable + NumVisableVariables; i++)
            {
                if (i >= currentContainer.GetNumTweakables())
                    continue;
                GetColors(i, out boxColor, out alpha);
                if (boxColor != Color.Black)
                    new BoxControl(new Vector4(0, y, 1, 0.05f), alpha, Color.Black, tweakableWindow);
                new TextControl(currentContainer.GetTweakableName(i), new Vector4(0, y, 0.45f, 0.05f), TextFormatting.Right | TextFormatting.VerticalCenter, alpha, Color.White, tweakableWindow);
                DrawValue(i, alpha, 0.55f, y, 0.45f, 0.05f);
                y += 0.075f;
            }
        }

        private void DrawValue(int num, byte alpha, float x, float y, float w, float h)
        {
            switch (currentContainer.GetTweakableType(num))
            {
                case TweakableType.Float:
                    new TextControl(currentContainer.GetFloatValue(num).ToString("N6"), new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    break;
                case TweakableType.Integer:
                    new TextControl(currentContainer.GetIntValue(num).ToString(), new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    break;
                case TweakableType.Vector2:
                    new TextControl("X: " + currentContainer.GetVector2Value(num).X.ToString("N3"), new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    new TextControl("Y: " + currentContainer.GetVector2Value(num).Y.ToString("N3"), new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(1, alpha), tweakableWindow);
                    break;
                case TweakableType.Vector3:
                    new TextControl("X: " + currentContainer.GetVector3Value(num).X.ToString("N3"), new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    new TextControl("Y: " + currentContainer.GetVector3Value(num).Y.ToString("N3"), new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(1, alpha), tweakableWindow);
                    new TextControl("Z: " + currentContainer.GetVector3Value(num).Z.ToString("N3"), new Vector4(x + 2 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(2, alpha), tweakableWindow);
                    break;
                case TweakableType.Vector4:
                    new TextControl("X: " + currentContainer.GetVector4Value(num).X.ToString("N3"), new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    new TextControl("Y: " + currentContainer.GetVector4Value(num).Y.ToString("N3"), new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(1, alpha), tweakableWindow);
                    new TextControl("Z: " + currentContainer.GetVector4Value(num).Z.ToString("N3"), new Vector4(x + 2 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(2, alpha), tweakableWindow);
                    new TextControl("W: " + currentContainer.GetVector4Value(num).W.ToString("N3"), new Vector4(x + 3 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(3, alpha), tweakableWindow);
                    break;
                case TweakableType.Color:
                    new TextControl("R: " + currentContainer.GetColorValue(num).R.ToString(), new Vector4(x + 0 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    new TextControl("G: " + currentContainer.GetColorValue(num).G.ToString(), new Vector4(x + 1 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(1, alpha), tweakableWindow);
                    new TextControl("B: " + currentContainer.GetColorValue(num).B.ToString(), new Vector4(x + 2 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(2, alpha), tweakableWindow);
                    new TextControl("A: " + currentContainer.GetColorValue(num).A.ToString(), new Vector4(x + 3 * w / 5, y, w / 5, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(3, alpha), tweakableWindow);
                    Color alphaColor = new Color(currentContainer.GetColorValue(num).A, currentContainer.GetColorValue(num).A, currentContainer.GetColorValue(num).A, currentContainer.GetColorValue(num).A);
                    new BoxControl(new Vector4(x + 4 * w / 5 + 0 * w / 10, y, w / 10, h), 255, currentContainer.GetColorValue(num), tweakableWindow);
                    new BoxControl(new Vector4(x + 4 * w / 5 + 1 * w / 10, y, w / 10, h), 255, alphaColor, tweakableWindow);
                    break;
                case TweakableType.String:
                    new TextControl(currentContainer.GetStringValue(num), new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    break;
                case TweakableType.Bool:
                    new TextControl(currentContainer.GetBoolValue(num) ? "true" : "false", new Vector4(x, y, w, h), TextFormatting.Center | TextFormatting.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    break;
                default:
                    break;
            }
        }

        private Color GetSelectionColor(int index, byte alpha)
        {
            if (index == currentSelection && alpha == 200)
                return Color.Red;
            else
                return Color.White;
        }

        private void GetColors(int i, out Color boxColor, out byte alpha)
        {
            if (i == CurrentVariable)
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

        public IDemoTweaker HandleInput(IInputDriver inputDriver)
        {
            if (inputDriver.UpPressedNoRepeat())
            {
                KeyUp();
            }
            if (inputDriver.DownPressedNoRepeat())
            {
                KeyDown();
            }
            if (inputDriver.KeyPressedNoRepeat(Keys.Tab))
            {
                KeyTab();
            }
            if (inputDriver.KeyPressedSlowRepeat(Keys.PageUp))
            {   
                KeyPageUp();
            }
            if (inputDriver.KeyPressedSlowRepeat(Keys.PageDown))
            {
                KeyPageDown();
            }
            CheckForInput(inputDriver);
            return null;
        }

        private bool CheckForInput(IInputDriver inputDriver)
        {
            Keys[] digitKeys = new Keys[] { 
                Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, 
                Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 
            };
            Keys[] numPadDigitKeys = new Keys[] { 
                Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, 
                Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9
            };

            for (int i = 0; i < digitKeys.Length; i++)
            {
                if (inputDriver.KeyPressedNoRepeat(numPadDigitKeys[i]) ||
                    inputDriver.KeyPressedNoRepeat(digitKeys[i]))
                {
                    inputString += i;
                    return true;
                }
            }
            if (inputDriver.KeyPressedNoRepeat(Keys.Decimal) || 
                inputDriver.KeyPressedNoRepeat(Keys.OemPeriod))
            {
                inputString += ".";
                return true;
            }
            if (inputDriver.KeyPressedNoRepeat(Keys.Subtract) ||
                inputDriver.KeyPressedNoRepeat(Keys.OemMinus))
            {
                inputString = "-";
                return true;
            }
            if (inputDriver.KeyPressedNoRepeat(Keys.Enter))
            {
                try
                {
                    SetValue(float.Parse(inputString, System.Globalization.NumberFormatInfo.InvariantInfo));
                }
                catch (FormatException) { }
                inputString = "";
                return true;
            }

            return false;
        }

        public object IdentifierToChild()
        {
            return 0;
        }

        public void IdentifierFromParent(object id)
        {
            if ((ITweakableContainer)id == null)
                throw new DDXXException("Incorrect type received from parent.");
            CurrentContainer = (ITweakableContainer)id;
        }

        #endregion

        public bool ShouldSave()
        {
            return true;
        }

    }
}
