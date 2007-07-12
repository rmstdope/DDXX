using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class TextureDirector
    {
        Stack<IGenerator> generatorStack;
        ITextureBuilder builder;

        public TextureDirector(ITextureBuilder builder)
        {
            generatorStack = new Stack<IGenerator>();
            this.builder = builder;
        }

        public ITexture Generate(int width, int height, int numMipLevels, Format format)
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

        public void Modulate()
        {
            Modulate modulate = new Modulate();
            ConnectFromStack(modulate, 2);
            generatorStack.Push(modulate);
        }

        public void Madd(float mul, float add)
        {
            Madd madd = new Madd();
            madd.Add = add;
            madd.Mul = mul;
            ConnectFromStack(madd, 1);
            generatorStack.Push(madd);
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
    }
}
