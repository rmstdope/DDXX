using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class LightState
    {
        private const int MaxNumLights = 4;

        private int numLights;
        private Vector3[] directionList;
        private Vector3[] positionList;
        private Vector3[] diffuseColorList;
        private Vector3[] specularColorList;

        public LightState()
        {
            positionList = new Vector3[MaxNumLights];
            directionList = new Vector3[MaxNumLights];
            diffuseColorList = new Vector3[MaxNumLights];
            specularColorList = new Vector3[MaxNumLights];
        }

        public int NumLights
        {
            get { return numLights; }
        }

        public Vector3[] Positions
        {
            get 
            {
                Vector3[] newArray = new Vector3[numLights];
                Array.Copy(positionList, newArray, numLights);
                return newArray;
            }
        }

        public Vector3[] Directions
        {
            get
            {
                Vector3[] newArray = new Vector3[numLights];
                Array.Copy(directionList, newArray, numLights);
                return newArray;
            }
        }

        public Vector3[] DiffuseColor
        {
            get
            {
                Vector3[] newArray = new Vector3[numLights];
                Array.Copy(diffuseColorList, newArray, numLights);
                return newArray;
            }
        }

        public Vector3[] SpecularColor
        {
            get
            {
                Vector3[] newArray = new Vector3[numLights];
                Array.Copy(specularColorList, newArray, numLights);
                return newArray;
            }
        }

        public void NewState(Vector3 position, Vector3 direction, Color diffuseColor, Color specularColor)
        {
            if (numLights == MaxNumLights)
                throw new DDXXException("Too many lights in the scene. Maximum number is " + MaxNumLights);
            positionList[numLights] = position;
            directionList[numLights] = direction;
            diffuseColorList[numLights] = diffuseColor.ToVector3();
            specularColorList[numLights] = specularColor.ToVector3();
            numLights++;
        }
    }
}
