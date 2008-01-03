using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public abstract class ModifierBase : IModifier
    {
        private int numInputPins;
        private IModifier[] inputPins;

        public ModifierBase(int numInputPins)
        {
            if (numInputPins < 0)
                throw new ArgumentOutOfRangeException("numInputPins", "Must be greater or equal to zero. Was " + numInputPins);
            this.numInputPins = numInputPins;
            inputPins = new IModifier[numInputPins];
        }

        public abstract IPrimitive Generate();

        public void ConnectToInput(int inputPin, IModifier outputGenerator)
        {
            ValidateInputPin(inputPin);
            inputPins[inputPin] = outputGenerator;
        }

        protected IPrimitive GetInput(int inputPin)
        {
            ValidateInputPin(inputPin);
            if (inputPins[inputPin] == null)
                throw new ArgumentException("Input " + inputPin + "has not been connected yet.");
            return inputPins[inputPin].Generate();
        }

        private void ValidateInputPin(int inputPin)
        {
            if (inputPin < 0)
                throw new ArgumentOutOfRangeException("inputPin", "Must not be negative. Was " + inputPin);
            if (inputPin >= numInputPins)
                throw new ArgumentOutOfRangeException("inputPin", "Must be less than number of input pins. Was " + inputPin);
        }

        protected void ComputeNormals(IPrimitive primitive)
        {
            Vector3[] normals = new Vector3[primitive.Vertices.Length];
            for (int i = 0; i < primitive.Indices.Length; i += 3)
            {
                Vector3 p0 = primitive.Vertices[primitive.Indices[i + 0]].Position;
                Vector3 p1 = primitive.Vertices[primitive.Indices[i + 1]].Position;
                Vector3 p2 = primitive.Vertices[primitive.Indices[i + 2]].Position;
                Vector3 normal = Vector3.Cross(p2 - p0, p1 - p0);
                normal.Normalize();
                normals[primitive.Indices[i + 0]] += normal;
                normals[primitive.Indices[i + 1]] += normal;
                normals[primitive.Indices[i + 2]] += normal;
            }
            for (int j = 0; j < primitive.Vertices.Length; j++)
            {
                normals[j].Normalize();
                primitive.Vertices[j].Normal = normals[j];
            }
        }

    }
}
