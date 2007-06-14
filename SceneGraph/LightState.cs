using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public class LightState
    {
        private const int MaxNumLights = 4;

        private int numLights;
        private Vector4[] directionList;
        private Vector4[] positionList;
        private ColorValue[] diffuseList;
        private ColorValue[] specularList;

        public LightState()
        {
            positionList = new Vector4[MaxNumLights];
            directionList = new Vector4[MaxNumLights];
            diffuseList = new ColorValue[MaxNumLights];
            specularList = new ColorValue[MaxNumLights];
        }

        public int NumLights
        {
            get { return numLights; }
        }

        public Vector4[] Positions
        {
            get 
            {
                Vector4[] newArray = new Vector4[numLights];
                Array.Copy(positionList, newArray, numLights);
                return newArray;
            }
        }

        public Vector4[] Directions
        {
            get
            {
                Vector4[] newArray = new Vector4[numLights];
                Array.Copy(directionList, newArray, numLights);
                return newArray;
            }
        }

        public ColorValue[] DiffuseColor
        {
            get
            {
                ColorValue[] newArray = new ColorValue[numLights];
                Array.Copy(diffuseList, newArray, numLights);
                return newArray;
            }
        }

        public ColorValue[] SpecularColor
        {
            get
            {
                ColorValue[] newArray = new ColorValue[numLights];
                Array.Copy(specularList, newArray, numLights);
                return newArray;
            }
        }

        public void NewState(Vector3 position, Vector3 direction, ColorValue diffuse, ColorValue specular)
        {
            if (numLights == MaxNumLights)
                throw new DDXXException("Too many lights in the scene. Maximum number is " + MaxNumLights);
            positionList[numLights] = new Vector4(position.X, position.Y, position.Z, 1.0f);
            directionList[numLights] = new Vector4(direction.X, direction.Y, direction.Z, 1.0f);
            diffuseList[numLights] = diffuse;
            specularList[numLights] = specular;
            numLights++;
        }
    }
}
