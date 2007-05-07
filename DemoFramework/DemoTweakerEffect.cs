using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Dope.DDXX.Input;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.DirectInput;
using Microsoft.DirectX;

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
            else if (currentContainer.GetTweakableType(CurrentVariable) == TweakableType.Vector3)
                dimension = 3;
            currentSelection++;
            if (currentSelection == dimension)
                currentSelection = 0;
        }

        private void KeyPlus()
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
                case TweakableType.Vector3:
                    Vector3 vector = currentContainer.GetVector3Value(currentVariable);
                    switch (currentSelection)
                    {
                        case 0:
                            vector += new Vector3(stepValue, 0, 0);
                            break;
                        case 1:
                            vector += new Vector3(0, stepValue, 0);
                            break;
                        case 2:
                            vector += new Vector3(0, 0, stepValue);
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, vector);
                    break;
                case TweakableType.Color:
                    Color color = currentContainer.GetColorValue(CurrentVariable);
                    switch (currentSelection)
                    {
                        case 0:
                            color = Color.FromArgb(color.A,
                                                   Math.Max(0, Math.Min(color.R + (int)stepValue, 255)),
                                                   color.G,
                                                   color.B);
                            break;
                        case 1:
                            color = Color.FromArgb(color.A,
                                                   color.R,
                                                   Math.Max(0, Math.Min(color.G + (int)stepValue, 255)),
                                                   color.B);
                            break;
                        case 2:
                            color = Color.FromArgb(color.A,
                                                   color.R,
                                                   color.G,
                                                   Math.Max(0, Math.Min(color.B + (int)stepValue, 255)));
                            break;
                        case 3:
                            color = Color.FromArgb(Math.Max(0, Math.Min(color.A + (int)stepValue, 255)),
                                                   color.R,
                                                   color.G,
                                                   color.B);
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, color);
                    break;
                case TweakableType.String:
                    // Next value can not be performed on strings
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
                case TweakableType.Vector3:
                    Vector3 vector = currentContainer.GetVector3Value(currentVariable);
                    switch (currentSelection)
                    {
                        case 0:
                            vector.X = value;
                            break;
                        case 1:
                            vector.Y = value;
                            break;
                        case 2:
                            vector.Z = value;
                            break;
                    }
                    currentContainer.SetValue(CurrentVariable, vector);
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
                    currentContainer.SetValue(CurrentVariable, Color.FromArgb(a, r, g, b));
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

        public void Draw()
        {
            DrawWindow();

            D3DDriver.GetInstance().Device.BeginScene();
            UserInterface.DrawControl(MainWindow);
            D3DDriver.GetInstance().Device.EndScene();
        }

        private void DrawWindow()
        {
            const int NumVisableVariables = 13;
            CreateBaseControls();
            tweakableWindow = new BoxControl(new RectangleF(0.0f, 0.05f, 1.0f, 0.95f),
                Settings.Alpha, Settings.TimeColor, MainWindow);

            Color boxColor;
            //Color textColor;
            int startVariable = 0;
            float y = 0.05f;
            float alpha;
            for (int i = startVariable; i < startVariable + NumVisableVariables; i++)
            {
                if (i >= currentContainer.GetNumTweakables())
                    continue;
                GetColors(i, out boxColor, out alpha);
                if (boxColor != Color.Black)
                    new BoxControl(new RectangleF(0.0f, y, 1.0f, 0.05f), alpha, Color.Black, tweakableWindow);
                new TextControl(currentContainer.GetTweakableName(i), new RectangleF(0.0f, y, 0.45f, 0.05f), DrawTextFormat.Right | DrawTextFormat.VerticalCenter, alpha, Color.White, tweakableWindow);
                DrawValue(i, alpha, 0.55f, y, 0.45f, 0.05f);
                y += 0.075f;
            }
        }

        private void DrawValue(int num, float alpha, float x, float y, float w, float h)
        {
            switch (currentContainer.GetTweakableType(num))
            {
                case TweakableType.Float:
                    new TextControl(currentContainer.GetFloatValue(num).ToString("N6"), new RectangleF(x, y, w, h), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    break;
                case TweakableType.Integer:
                    new TextControl(currentContainer.GetIntValue(num).ToString(), new RectangleF(x, y, w, h), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    break;
                case TweakableType.Vector3:
                    new TextControl("X: " + currentContainer.GetVector3Value(num).X.ToString("N3"), new RectangleF(x + 0 * w / 5.0f, y, w / 5.0f, h), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    new TextControl("Y: " + currentContainer.GetVector3Value(num).Y.ToString("N3"), new RectangleF(x + 1 * w / 5.0f, y, w / 5.0f, h), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, GetSelectionColor(1, alpha), tweakableWindow);
                    new TextControl("Z: " + currentContainer.GetVector3Value(num).Z.ToString("N3"), new RectangleF(x + 2 * w / 5.0f, y, w / 5.0f, h), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, GetSelectionColor(2, alpha), tweakableWindow);
                    break;
                case TweakableType.Color:
                    new TextControl("R: " + currentContainer.GetColorValue(num).R.ToString(), new RectangleF(x + 0 * w / 5.0f, y, w / 5.0f, h), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    new TextControl("G: " + currentContainer.GetColorValue(num).G.ToString(), new RectangleF(x + 1 * w / 5.0f, y, w / 5.0f, h), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, GetSelectionColor(1, alpha), tweakableWindow);
                    new TextControl("B: " + currentContainer.GetColorValue(num).B.ToString(), new RectangleF(x + 2 * w / 5.0f, y, w / 5.0f, h), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, GetSelectionColor(2, alpha), tweakableWindow);
                    new TextControl("A: " + currentContainer.GetColorValue(num).A.ToString(), new RectangleF(x + 3 * w / 5.0f, y, w / 5.0f, h), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, GetSelectionColor(3, alpha), tweakableWindow);
                    Color alphaColor = Color.FromArgb(currentContainer.GetColorValue(num).A, currentContainer.GetColorValue(num).A, currentContainer.GetColorValue(num).A, currentContainer.GetColorValue(num).A);
                    new BoxControl(new RectangleF(x + 4 * w / 5.0f + 0 * w / 10.0f, y, w / 10.0f, h), 1.0f, currentContainer.GetColorValue(num), tweakableWindow);
                    new BoxControl(new RectangleF(x + 4 * w / 5.0f + 1 * w / 10.0f, y, w / 10.0f, h), 1.0f, alphaColor, tweakableWindow);
                    break;
                case TweakableType.String:
                    new TextControl(currentContainer.GetStringValue(num), new RectangleF(x, y, w, h), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, GetSelectionColor(0, alpha), tweakableWindow);
                    break;
                default:
                    throw new DDXXException("Unknown value type.");
            }
        }

        private Color GetSelectionColor(int index, float alpha)
        {
            if (index == currentSelection && alpha == 0.8f)
                return Color.Red;
            else
                return Color.White;
        }

        private void GetColors(int i, out Color boxColor, out float alpha)
        {
            if (i == CurrentVariable)
            {
                boxColor = Color.White;
                alpha = 0.8f;
            }
            else
            {
                boxColor = Color.Black;
                alpha = 0.3f;
            }
        }

        public bool HandleInput(IInputDriver inputDriver)
        {
            bool handled = false;
            if (inputDriver.KeyPressedNoRepeat(Key.UpArrow))
            {
                KeyUp();
                handled = true;
            }
            if (inputDriver.KeyPressedNoRepeat(Key.DownArrow))
            {
                KeyDown();
                handled = true;
            }
            if (inputDriver.KeyPressedNoRepeat(Key.Tab))
            {
                KeyTab();
                handled = true;
            }
            if (inputDriver.KeyPressedSlowRepeat(Key.PageUp))
            {   
                KeyPlus();
                handled = true;
            }
            if (inputDriver.KeyPressedSlowRepeat(Key.PageDown))
            {
                KeyPageDown();
                handled = true;
            }
            handled = CheckForInput(inputDriver) || handled;

            return handled;
        }

        private bool CheckForInput(IInputDriver inputDriver)
        {
            Key[] numberKeys = new Key[] { Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9 };

            for (int i = 0; i < numberKeys.Length; i++)
            {
                if (inputDriver.KeyPressedNoRepeat(numberKeys[i]))
                {
                    inputString += i;
                    return true;
                }
            }
            if (inputDriver.KeyPressedNoRepeat(Key.NumPadPeriod))
            {
                inputString += ".";
                return true;
            }
            if (inputDriver.KeyPressedNoRepeat(Key.NumPadMinus))
            {
                inputString = "-";
                return true;
            }
            if (inputDriver.KeyPressedNoRepeat(Key.NumPadEnter))
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

        public bool ShouldSave(IInputDriver inputDriver)
        {
            return true;
        }

    }
}
