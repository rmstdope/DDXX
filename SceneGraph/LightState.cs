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
        private Vector4[] positionList;
        private ColorValue[] diffuseList;
        private ColorValue[] specularList;

        internal LightState()
        {
            positionList = new Vector4[MaxNumLights];
            diffuseList = new ColorValue[MaxNumLights];
            specularList = new ColorValue[MaxNumLights];
        }

        internal int NumLights
        {
            get { return numLights; }
        }

        internal Vector4[] Positions
        {
            get 
            {
                Vector4[] newArray = new Vector4[numLights];
                Array.Copy(positionList, newArray, numLights);
                return newArray;
            }
        }

        internal ColorValue[] DiffuseColor
        {
            get
            {
                ColorValue[] newArray = new ColorValue[numLights];
                Array.Copy(diffuseList, newArray, numLights);
                return newArray;
            }
        }

        internal ColorValue[] SpecularColor
        {
            get
            {
                ColorValue[] newArray = new ColorValue[numLights];
                Array.Copy(specularList, newArray, numLights);
                return newArray;
            }
        }

        internal void NewState(Vector3 position, ColorValue diffuse, ColorValue specular)
        {
            if (numLights == MaxNumLights)
                throw new DDXXException("Too many lights in the scene. Maximum number is " + MaxNumLights);
            positionList[numLights] = new Vector4(position.X, position.Y, position.Z, 1.0f);
            diffuseList[numLights] = diffuse;
            specularList[numLights] = specular;
            numLights++;
        }
    }
}
