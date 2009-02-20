using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public abstract class Generator : ITextureGenerator
    {
        private int numInputPins;
        private ITextureGenerator output;
        private ITextureGenerator[] inputPins;

        public int NumInputPins
        {
            get { return numInputPins; }
        }

        public Generator(int numInputPins)
        {
            if (numInputPins < 0)
                throw new ArgumentOutOfRangeException("numInputPins", "Must be greater or equal to zero. Was " + numInputPins);
            this.numInputPins = numInputPins;
            inputPins = new ITextureGenerator[numInputPins];
        }

        protected Vector4 ColorToRgba(Color color1)
        {
            return new Vector4(color1.R / 255f, color1.G / 255f, color1.B / 255f, color1.A / 255f);
        }

        protected Vector4 HslaToRgba(Vector4 hsla)
        {
            float r = 0, g = 0, b = 0;
            float temp1, temp2;
            if (hsla.Z == 0)
            {
                r = g = b = 0;
            }
            else
            {
                if (hsla.Y == 0)
                {
                    r = g = b = hsla.Z;
                }
                else
                {
                    temp2 = ((hsla.Z <= 0.5f) ? hsla.Z * (1.0f + hsla.Y) : hsla.Z + hsla.Y - (hsla.Z * hsla.Y));
                    temp1 = 2.0f * hsla.Z - temp2;
                    float[] t3 = new float[] { hsla.X + 1.0f / 3.0f, hsla.X, hsla.X - 1.0f / 3.0f };
                    float[] clr = new float[] { 0, 0, 0 };
                    for (int i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0)
                            t3[i] += 1.0f;
                        if (t3[i] > 1)
                            t3[i] -= 1.0f;
                        if (6.0 * t3[i] < 1.0f)
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0f;
                        else if (2.0f * t3[i] < 1.0f)
                            clr[i] = temp2;
                        else if (3.0f * t3[i] < 2.0f)
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0f / 3.0f) - t3[i]) * 6.0f);
                        else
                            clr[i] = temp1;
                    }
                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }
            return new Vector4(r, g, b, hsla.W);
        }

        public abstract Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize);

        public ITextureGenerator Output 
        {
            get { return output; }
            set { output = value; }
        }

        public int GetInputIndex(ITextureGenerator generator)
        {
            for (int i = 0; i < NumInputPins; i++)
                if (generator == inputPins[i])
                    return i;
            throw new ArgumentException("The specified generator is not an input of this generator.");
        }

        public void ConnectToInput(int inputPin, ITextureGenerator outputGenerator)
        {
            ValidateInputPin(inputPin);
            inputPins[inputPin] = outputGenerator;
            if (outputGenerator != null)
                outputGenerator.Output = this;
        }

        public ITextureGenerator GetInput(int inputPin)
        {
            ValidateInputPin(inputPin);
            if (inputPins[inputPin] == null)
                throw new ArgumentException("Input " + inputPin + "has not been connected yet.");
            return inputPins[inputPin];
        }

        protected Vector4 GetInputPixel(int inputPin, Vector2 textureCoordinate, Vector2 texelSize)
        {
            ValidateInputPin(inputPin);
            if (inputPins[inputPin] == null)
                throw new ArgumentException("Input " + inputPin + "has not been connected yet.");
            return inputPins[inputPin].GetPixel(textureCoordinate, texelSize);
        }

        private void ValidateInputPin(int inputPin)
        {
            if (inputPin < 0)
                throw new ArgumentOutOfRangeException("inputPin", "Must not be negative. Was " + inputPin);
            if (inputPin >= numInputPins)
                throw new ArgumentOutOfRangeException("inputPin", "Must be less than number of input pins. Was " + inputPin);
        }

    }
}
