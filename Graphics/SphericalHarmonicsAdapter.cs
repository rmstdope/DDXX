using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public class SphericalHarmonicsAdapter : ISphericalHarmonics
    {
        #region ISphericalHarmonics Members

        public void Add(float[] output, int order, float[] vectorA, float[] vectorB)
        {
            SphericalHarmonics.Add(output, order, vectorA, vectorB);
        }

        public void Add(float[] output, int order, GraphicsStream vectorA, GraphicsStream vectorB)
        {
            SphericalHarmonics.Add(output, order, vectorA, vectorB);
        }

        public void Add(GraphicsStream output, int order, float[] vectorA, float[] vectorB)
        {
            SphericalHarmonics.Add(output, order, vectorA, vectorB);
        }

        public void Add(GraphicsStream output, int order, GraphicsStream vectorA, GraphicsStream vectorB)
        {
            SphericalHarmonics.Add(output, order, vectorA, vectorB);
        }

        public float Dot(int order, float[] vectorA, float[] vectorB)
        {
            return SphericalHarmonics.Dot(order, vectorA, vectorB);
        }

        public float Dot(int order, GraphicsStream vectorA, GraphicsStream vectorB)
        {
            return SphericalHarmonics.Dot(order, vectorA, vectorB);
        }

        public void EvaluateConeLight(int order, Vector3 direction, float radius, float redIntensity, float greenIntensity, float blueIntensity, float[] redOutput, float[] greenOutput, float[] blueOutput)
        {
            SphericalHarmonics.EvaluateConeLight(order, direction, radius, redIntensity, greenIntensity, blueIntensity, redOutput, greenOutput, blueOutput);
        }

        public void EvaluateConeLight(int order, Vector3 direction, float radius, float redIntensity, float greenIntensity, float blueIntensity, GraphicsStream redOutput, GraphicsStream greenOutput, GraphicsStream blueOutput)
        {
            SphericalHarmonics.EvaluateConeLight(order, direction, radius, redIntensity, greenIntensity, blueIntensity, redOutput, greenOutput, blueOutput);
        }

        public void EvaluateDirection(float[] output, int order, Vector3 direction)
        {
            SphericalHarmonics.EvaluateDirection(output, order, direction);
        }

        public void EvaluateDirection(GraphicsStream output, int order, Vector3 direction)
        {
            SphericalHarmonics.EvaluateDirection(output, order, direction);
        }

        public void EvaluateDirectionalLight(int order, Vector3 direction, float redIntensity, float greenIntensity, float blueIntensity, float[] redOutput, float[] greenOutput, float[] blueOutput)
        {
            SphericalHarmonics.EvaluateDirectionalLight(order, direction, redIntensity, greenIntensity, blueIntensity, redOutput, greenOutput, blueOutput);
        }

        public void EvaluateDirectionalLight(int order, Vector3 direction, float redIntensity, float greenIntensity, float blueIntensity, GraphicsStream redOutput, GraphicsStream greenOutput, GraphicsStream blueOutput)
        {
            SphericalHarmonics.EvaluateDirectionalLight(order, direction, redIntensity, greenIntensity, blueIntensity, redOutput, greenOutput, blueOutput);
        }

        public void EvaluateHemisphereLight(int order, Vector3 direction, ColorValue top, ColorValue bottom, float[] redOutput, float[] greenOutput, float[] blueOutput)
        {
            SphericalHarmonics.EvaluateHemisphereLight(order, direction, top, bottom, redOutput, greenOutput, blueOutput);
        }

        public void EvaluateHemisphereLight(int order, Vector3 direction, ColorValue top, ColorValue bottom, GraphicsStream redOutput, GraphicsStream greenOutput, GraphicsStream blueOutput)
        {
            SphericalHarmonics.EvaluateHemisphereLight(order, direction, top, bottom, redOutput, greenOutput, blueOutput);
        }

        public void EvaluateSphericalLight(int order, Vector3 position, float radius, float redIntensity, float greenIntensity, float blueIntensity, float[] redOutput, float[] greenOutput, float[] blueOutput)
        {
            SphericalHarmonics.EvaluateSphericalLight(order, position, radius, redIntensity, greenIntensity, blueIntensity, redOutput, greenOutput, blueOutput);
        }

        public void EvaluateSphericalLight(int order, Vector3 position, float radius, float redIntensity, float greenIntensity, float blueIntensity, GraphicsStream redOutput, GraphicsStream greenOutput, GraphicsStream blueOutput)
        {
            SphericalHarmonics.EvaluateSphericalLight(order, position, radius, redIntensity, greenIntensity, blueIntensity, redOutput, greenOutput, blueOutput);
        }

        public void ProjectCubeMap(int order, ICubeTexture cubeMap, float[] redOutput, float[] greenOutput, float[] blueOutput)
        {
            SphericalHarmonics.ProjectCubeMap(order, ((CubeTextureAdapter)cubeMap).CubeTextureDX, redOutput, greenOutput, blueOutput);
        }

        public void ProjectCubeMap(int order, ICubeTexture cubeMap, GraphicsStream redOutput, GraphicsStream greenOutput, GraphicsStream blueOutput)
        {
            SphericalHarmonics.ProjectCubeMap(order, ((CubeTextureAdapter)cubeMap).CubeTextureDX, redOutput, greenOutput, blueOutput);
        }

        public void Rotate(float[] output, int order, Matrix rotation, float[] input)
        {
            SphericalHarmonics.Rotate(output, order, rotation, input);
        }

        public void Rotate(float[] output, int order, Matrix rotation, GraphicsStream input)
        {
            SphericalHarmonics.Rotate(output, order, rotation, input);
        }

        public void Rotate(GraphicsStream output, int order, Matrix rotation, float[] input)
        {
            SphericalHarmonics.Rotate(output, order, rotation, input);
        }

        public void Rotate(GraphicsStream output, int order, Matrix rotation, GraphicsStream input)
        {
            SphericalHarmonics.Rotate(output, order, rotation, input);
        }

        public void RotateZ(float[] output, int order, float angle, float[] input)
        {
            SphericalHarmonics.RotateZ(output, order, angle, input);
        }

        public void RotateZ(float[] output, int order, float angle, GraphicsStream input)
        {
            SphericalHarmonics.RotateZ(output, order, angle, input);
        }

        public void RotateZ(GraphicsStream output, int order, float angle, float[] input)
        {
            SphericalHarmonics.RotateZ(output, order, angle, input);
        }

        public void RotateZ(GraphicsStream output, int order, float angle, GraphicsStream input)
        {
            SphericalHarmonics.RotateZ(output, order, angle, input);
        }

        public void Scale(float[] output, int order, float[] input, float scaleFactor)
        {
            SphericalHarmonics.Scale(output, order, input, scaleFactor);
        }

        public void Scale(float[] output, int order, GraphicsStream input, float scaleFactor)
        {
            SphericalHarmonics.Scale(output, order, input, scaleFactor);
        }

        public void Scale(GraphicsStream output, int order, float[] input, float scaleFactor)
        {
            SphericalHarmonics.Scale(output, order, input, scaleFactor);
        }

        public void Scale(GraphicsStream output, int order, GraphicsStream input, float scaleFactor)
        {
            SphericalHarmonics.Scale(output, order, input, scaleFactor);
        }

        #endregion
    }
}
