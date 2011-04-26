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
        private int height;
        private int width;
        private int x;
        private int y;
        private Vector2 textureCoordinateRename;
        private Vector2 texelSizeRename;
        private List<Vector4[,]> inputs;

        public int Height { get { return height; } }
        public int Width { get { return width; } }
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public Vector2 textureCoordinate { get { return textureCoordinateRename; } }
        public Vector2 texelSize { get { return texelSizeRename; } }

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

        protected Vector4 Vector4FromFloat(float value)
        {
            return new Vector4(value, value, value, value);
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

        protected abstract Vector4 GetPixel();

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
            float x = textureCoordinate.X * (width - 1);
            float y = textureCoordinate.Y * (height - 1);
            int x1 = (int)x;
            int y1 = (int)y;
            int x2 = x1 + 1;
            int y2 = y1 + 1;
            if (x2 >= Width)
                x2 -= Width;
            if (y2 >= Height)
                y2 -= Height;
            float fracX = x - x1;
            float fracY = y - y1;
            Vector4 v1 = inputs[inputPin][x1, y1];
            Vector4 v2 = inputs[inputPin][x2, y1];
            Vector4 v3 = inputs[inputPin][x1, y2];
            Vector4 v4 = inputs[inputPin][x2, y2];
            return Vector4.Lerp(Vector4.Lerp(v1, v2, fracX), Vector4.Lerp(v3, v4, fracX), fracY);
        }

        protected Vector4 GetInputPixel(int inputPin, int offsetX, int offsetY)
        {
            ValidateInputPin(inputPin);
            if (inputPins[inputPin] == null)
                throw new ArgumentException("Input " + inputPin + "has not been connected yet.");
            int newX = x + offsetX;
            int newY = y + offsetY;
            if (newX < 0)
                newX += Width;
            if (newX >= Width)
                newX -= Width;
            if (newY < 0)
                newY += Height;
            if (newY >= Height)
                newY -= Height;
            return inputs[inputPin][newX, newY];
        }

        private void ValidateInputPin(int inputPin)
        {
            if (inputPin < 0)
                throw new ArgumentOutOfRangeException("inputPin", "Must not be negative. Was " + inputPin);
            if (inputPin >= numInputPins)
                throw new ArgumentOutOfRangeException("inputPin", "Must be less than number of input pins. Was " + inputPin);
        }

        public int NumGeneratorsInChain
        {
            get 
            {
                int num = 1;
                foreach (ITextureGenerator generator in inputPins)
                    num += generator.NumGeneratorsInChain;
                return num; 
            }
        }

        public Vector4[,] GenerateTexture(int width, int height)
        {
            Vector4[,] data = new Vector4[width, height];
            Initialize(width, height);
            for (y = 0; y < height; y++)
            {
                for (x = 0; x < width; x++)
                {
                    data[x, y] = GetPixel();
                    textureCoordinateRename.X += texelSizeRename.X;
                }
                textureCoordinateRename.X = texelSizeRename.X / 2;
                textureCoordinateRename.Y += texelSizeRename.Y;
            }
            Finalize(data);
            return data;
        }

        private void Initialize(int width, int height)
        {
            this.width = width;
            this.height = height;
            texelSizeRename = new Vector2(1.0f / width, 1.0f / height);
            textureCoordinateRename = texelSizeRename / 2;
            inputs = new List<Vector4[,]>();
            for (int i = 0; i < numInputPins; i++)
                inputs.Add(inputPins[i].GenerateTexture(width, height));
            StartGeneration();
        }

        private void Finalize(Vector4[,] data)
        {
            EndGeneration(data, width, height);
            inputs = null;
        }

        protected virtual void StartGeneration() { }
        protected virtual void EndGeneration(Vector4[,] pixels, int width, int height) { }

    }
}
