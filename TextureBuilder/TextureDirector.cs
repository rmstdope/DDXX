using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class TextureDirector
    {
        Stack<IGenerator> generatorStack;
        ITextureBuilder builder;
        ITextureFactory textureFactory;

        public TextureDirector(ITextureBuilder builder, ITextureFactory textureFactory)
        {
            generatorStack = new Stack<IGenerator>();
            this.builder = builder;
            this.textureFactory = textureFactory;
        }

        public ITexture2D Generate(int width, int height, int numMipLevels, SurfaceFormat format)
        {
            return builder.Generate(generatorStack.Pop(), width, height, numMipLevels, format);
        }

        public void CreatePerlinNoise(int baseFrequncy, int numOctaves, float persistance)
        {
            PerlinNoise noise = new PerlinNoise();
            noise.BaseFrequency = baseFrequncy;
            noise.NumOctaves = numOctaves;
            noise.Persistence = persistance;
            generatorStack.Push(noise);
        }

        public void CreateCircle(float innerRadius, float outerRadius)
        {
            Circle circle = new Circle();
            circle.InnerRadius = innerRadius;
            circle.OuterRadius = outerRadius;
            generatorStack.Push(circle);
        }

        public void CreateSquare(float size)
        {
            Square square = new Square();
            square.Size = size;
            generatorStack.Push(square);
        }

        public void CreateConstant(Vector4 color)
        {
            Constant constant = new Constant();
            constant.Color = color;
            generatorStack.Push(constant);
        }

        public void CreateBrushNoise(int pointsPerLine)
        {
            BrushNoise noise = new BrushNoise();
            noise.PointsPerLine = pointsPerLine;
            generatorStack.Push(noise);
        }

        public void CreateBricks(int numBricksX, int numBricksY, float gapWidth)
        {
            Bricks bricks = new Bricks();
            bricks.NumBricksX = numBricksX;
            bricks.NumBricksY = numBricksY;
            bricks.GapWidth = gapWidth;
            generatorStack.Push(bricks);
        }

        public void GaussianBlur()
        {
            Convolution convolution = new Convolution();
            convolution.Kernel = new float[,] {{ 
                0.002216f, 0.008764f, 0.026995f, 0.064759f, 0.120985f, 0.176033f, 0.199471f, 
                0.176033f, 0.120985f, 0.064759f, 0.026995f, 0.008764f, 0.002216f,
            }};
            ConnectFromStack(convolution, 1);
            generatorStack.Push(convolution);
            convolution = new Convolution();
            convolution.Kernel = new float[,] { 
                {0.002216f}, {0.008764f}, {0.026995f}, {0.064759f}, {0.120985f}, {0.176033f}, {0.199471f}, 
                {0.176033f}, {0.120985f}, {0.064759f}, {0.026995f}, {0.008764f}, {0.002216f},
            };
            ConnectFromStack(convolution, 1);
            generatorStack.Push(convolution);
        }

        public void Modulate()
        {
            Modulate modulate = new Modulate();
            ConnectFromStack(modulate, 2);
            generatorStack.Push(modulate);
        }

        public void ModulateColor(Vector4 color)
        {
            ColorModulation modulate = new ColorModulation();
            modulate.Color = color;
            ConnectFromStack(modulate, 1);
            generatorStack.Push(modulate);
        }

        public void ColorBlend(Vector4 zeroColor, Vector4 oneColor)
        {
            ColorBlend blend = new ColorBlend();
            blend.ZeroColor = zeroColor;
            blend.OneColor = oneColor;
            ConnectFromStack(blend, 1);
            generatorStack.Push(blend);
        }

        public void Add()
        {
            Add add = new Add();
            ConnectFromStack(add, 2);
            generatorStack.Push(add);
        }

        public void Subtract()
        {
            Madd madd = new Madd();
            madd.Add = 0;
            madd.Mul = -1;
            ConnectFromStack(madd, 1);
            generatorStack.Push(madd);
            Add add = new Add();
            ConnectFromStack(add, 2);
            generatorStack.Push(add);
        }

        public void Madd(float mul, float add)
        {
            Madd madd = new Madd();
            madd.Add = add;
            madd.Mul = mul;
            ConnectFromStack(madd, 1);
            generatorStack.Push(madd);
        }

        public void FactorBlend(float factor)
        {
            FactorBlend blend = new FactorBlend();
            blend.Factor = factor;
            ConnectFromStack(blend, 2);
            generatorStack.Push(blend);
        }

        public void FromFile(string filename)
        {
            FromFile fromFile = new FromFile();
            fromFile.Filename = filename;
            fromFile.TextureFactory = textureFactory;
            generatorStack.Push(fromFile);
        }

        public void NormalMap()
        {
            NormalMap normalMap = new NormalMap();
            ConnectFromStack(normalMap, 1);
            generatorStack.Push(normalMap);
        }

        private void ConnectFromStack(IGenerator modulate, int numInputs)
        {
            EnsureStackSize(numInputs);
            for (int i = 0; i < numInputs; i++)
                modulate.ConnectToInput(i, generatorStack.Pop());
        }

        private void EnsureStackSize(int size)
        {
            if (generatorStack.Count < size)
                throw new DDXXException("Not enough elements on the stack.");
        }

        public ITexture2D GenerateChain(int width, int height)
        {
            List<IGenerator> list = new List<IGenerator>();
            IGenerator[] generators = generatorStack.ToArray();
            foreach (IGenerator traversing in generators)
                AddGeneratorRecursively(traversing, list);
            SideBySideGenerator generator = new SideBySideGenerator(list.Count);
            for (int i = 0; i < list.Count; i++)
                generator.ConnectToInput(i, list[i]);

            return builder.Generate(generator, width, height, 1, SurfaceFormat.Color);
        }

        private void AddGeneratorRecursively(IGenerator generator, List<IGenerator> list)
        {
            for (int i = 0; i < generator.NumInputPins; i++)
                AddGeneratorRecursively(generator.GetInput(i), list);
            list.Add(generator);
        }

        private class SideBySideGenerator : Generator
        {
            private int dimension;

            public SideBySideGenerator(int numInputs)
                : base(numInputs)
            {
                dimension = (int)Math.Ceiling(Math.Sqrt(numInputs));
            }

            public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
            {
                float width = (1.0f / dimension);
                float height = (1.0f / dimension);
                int xNum = (int)(textureCoordinate.X / width);
                int yNum = (int)(textureCoordinate.Y / height);
                int num = xNum + yNum * dimension;
                if (num >= NumInputPins)
                    return new Vector4(0, 0, 0, 0);
                Vector2 newCoord = textureCoordinate;
                newCoord.X -= xNum * width;
                newCoord.X /= width;
                newCoord.Y -= yNum * height;
                newCoord.Y /= height;
                return GetInputPixel(num, newCoord, texelSize);
            }
        }

    }
}
