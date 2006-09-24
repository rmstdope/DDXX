using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public interface ISphericalHarmonics
    {
        // Summary:
        //     Adds two spherical harmonic (SH) vectors together; in other words, Out[i]
        //     = A[i] + B[i].
        //
        // Parameters:
        //   output:
        //     An array of floating point values that represent the output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Add()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Add()
        //     - 1.
        //
        //   vectorA:
        //     An array of floating point values that represent the first spherical harmonic
        //     (SH) vector to add to the second vector.
        //
        //   vectorB:
        //     An array of floating point values that represent the second spherical harmonic
        //     (SH) vector to add to the first vector.
        void Add(float[] output, int order, float[] vectorA, float[] vectorB);
        //
        // Summary:
        //     Adds two spherical harmonic (SH) vectors together; in other words, Out[i]
        //     = A[i] + B[i].
        //
        // Parameters:
        //   output:
        //     An array of floating point values that represent the output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Add()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Add()
        //     - 1.
        //
        //   vectorA:
        //     A Microsoft.DirectX.GraphicsStream object that represents the first spherical
        //     harmonic (SH) vector to add to the second vector.
        //
        //   vectorB:
        //     A Microsoft.DirectX.GraphicsStream object that represents the second spherical
        //     harmonic (SH) vector to add to the first vector.
        void Add(float[] output, int order, GraphicsStream vectorA, GraphicsStream vectorB);
        //
        // Summary:
        //     Adds two spherical harmonic (SH) vectors together; in other words, Out[i]
        //     = A[i] + B[i].
        //
        // Parameters:
        //   output:
        //     A Microsoft.DirectX.GraphicsStream object that represents the output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Add()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Add()
        //     - 1.
        //
        //   vectorA:
        //     An array of floating point values that represent the first spherical harmonic
        //     (SH) vector to add to the second vector.
        //
        //   vectorB:
        //     An array of floating point values that represent the second spherical harmonic
        //     (SH) vector to add to the first vector.
        void Add(GraphicsStream output, int order, float[] vectorA, float[] vectorB);
        //
        // Summary:
        //     Adds two spherical harmonic (SH) vectors together; in other words, Out[i]
        //     = A[i] + B[i].
        //
        // Parameters:
        //   output:
        //     A Microsoft.DirectX.GraphicsStream object that represents the output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Add()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Add()
        //     - 1.
        //
        //   vectorA:
        //     A Microsoft.DirectX.GraphicsStream object that represents the first spherical
        //     harmonic (SH) vector to add to the second vector.
        //
        //   vectorB:
        //     A Microsoft.DirectX.GraphicsStream object that represents the second spherical
        //     harmonic (SH) vector to add to the first vector.
        void Add(GraphicsStream output, int order, GraphicsStream vectorA, GraphicsStream vectorB);
        //
        // Summary:
        //     Computes the dot product of two spherical harmonic (SH) vectors.
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Dot()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Dot()
        //     - 1.
        //
        //   vectorA:
        //     An array of floating point values that represent the first spherical harmonic
        //     (SH) vector.
        //
        //   vectorB:
        //     An array of floating point values that represent the second spherical harmonic
        //     (SH) vector.
        //
        // Returns:
        //     A floating point value that represents the dot product of the two vectors.
        float Dot(int order, float[] vectorA, float[] vectorB);
        //
        // Summary:
        //     Computes the dot product of two spherical harmonic (SH) vectors.
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Dot()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Dot()
        //     - 1.
        //
        //   vectorA:
        //     A Microsoft.DirectX.GraphicsStream object that represents the first spherical
        //     harmonic (SH) vector.
        //
        //   vectorB:
        //     A Microsoft.DirectX.GraphicsStream object that represents the second spherical
        //     harmonic (SH) vector.
        //
        // Returns:
        //     A floating point value that represents the dot product of the two vectors.
        float Dot(int order, GraphicsStream vectorA, GraphicsStream vectorB);
        //
        // Summary:
        //     Evaluates a light that is a cone of constant intensity and returns spectral
        //     spherical harmonic (SH) data.
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateConeLight()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateConeLight()
        //     - 1.
        //
        //   direction:
        //     The (x, y, z) hemisphere axis direction vector in which to evaluate the spherical
        //     harmonic (SH) basis functions. See Remarks.
        //
        //   radius:
        //     Radius of the light cone in radians.
        //
        //   redIntensity:
        //     The red intensity of the light.
        //
        //   greenIntensity:
        //     The green intensity of the light.
        //
        //   blueIntensity:
        //     The blue intensity of the light.
        //
        //   redOutput:
        //     Output spherical harmonic (SH) vector for the red component.
        //
        //   greenOutput:
        //     Output spherical harmonic (SH) vector for the green component.
        //
        //   blueOutput:
        //     Output spherical harmonic (SH) vector for the red component.
        void EvaluateConeLight(int order, Vector3 direction, float radius, float redIntensity, float greenIntensity, float blueIntensity, float[] redOutput, float[] greenOutput, float[] blueOutput);
        //
        // Summary:
        //     Evaluates a light that is a cone of constant intensity and returns spectral
        //     spherical harmonic (SH) data.
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateConeLight()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateConeLight()
        //     - 1.
        //
        //   direction:
        //     The (x, y, z) hemisphere axis direction vector in which to evaluate the spherical
        //     harmonic (SH) basis functions. See Remarks.
        //
        //   radius:
        //     Radius of the light cone in radians.
        //
        //   redIntensity:
        //     The red intensity of the light.
        //
        //   greenIntensity:
        //     The green intensity of the light.
        //
        //   blueIntensity:
        //     The blue intensity of the light.
        //
        //   redOutput:
        //     Output spherical harmonic (SH) vector for the blue component.
        //
        //   greenOutput:
        //     Output spherical harmonic (SH) vector for the green component.
        //
        //   blueOutput:
        //     Output spherical harmonic (SH) vector for the blue component.
        void EvaluateConeLight(int order, Vector3 direction, float radius, float redIntensity, float greenIntensity, float blueIntensity, GraphicsStream redOutput, GraphicsStream greenOutput, GraphicsStream blueOutput);
        //
        // Summary:
        //     Evaluates the spherical harmonic (SH) basis functions from an input direction
        //     vector.
        //
        // Parameters:
        //   output:
        //     spherical harmonic (SH) output coefficients. The basis function Ylm is stored
        //     at location l2 + m + l.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateDirection()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateDirection()
        //     - 1.
        //
        //   direction:
        //     (x, y, z) direction vector in which to evaluate the spherical harmonic (SH)
        //     basis functions. Must be normalized. See Remarks.
        void EvaluateDirection(float[] output, int order, Vector3 direction);
        //
        // Summary:
        //     Evaluates the spherical harmonic (SH) basis functions from an input direction
        //     vector.
        //
        // Parameters:
        //   output:
        //     spherical harmonic (SH) output coefficients. The basis function Ylm is stored
        //     at location l2 + m + l.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateDirection()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateDirection()
        //     - 1.
        //
        //   direction:
        //     (x, y, z) direction vector in which to evaluate the spherical harmonic (SH)
        //     basis functions. Must be normalized. See Remarks.
        void EvaluateDirection(GraphicsStream output, int order, Vector3 direction);
        //
        // Summary:
        //     Evaluates a directional light and returns spectral spherical harmonic (SH)
        //     data.
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateDirectionalLight()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateDirectionalLight()
        //     - 1.
        //
        //   direction:
        //     The (x, y, z) hemisphere axis direction vector in which to evaluate the spherical
        //     harmonic (SH) basis functions. See Remarks.
        //
        //   redIntensity:
        //     The red intensity of the light.
        //
        //   greenIntensity:
        //     The green intensity of the light.
        //
        //   blueIntensity:
        //     The blue intensity of the light.
        //
        //   redOutput:
        //     Output spherical harmonic (SH) vector for the red component.
        //
        //   greenOutput:
        //     Output spherical harmonic (SH) vector for the green component.
        //
        //   blueOutput:
        //     Output spherical harmonic (SH) vector for the red component.
        void EvaluateDirectionalLight(int order, Vector3 direction, float redIntensity, float greenIntensity, float blueIntensity, float[] redOutput, float[] greenOutput, float[] blueOutput);
        //
        // Summary:
        //     Evaluates a directional light and returns spectral spherical harmonic (SH)
        //     data.
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateDirectionalLight()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateDirectionalLight()
        //     - 1.
        //
        //   direction:
        //     The (x, y, z) hemisphere axis direction vector in which to evaluate the spherical
        //     harmonic (SH) basis functions. See Remarks.
        //
        //   redIntensity:
        //     The red intensity of the light.
        //
        //   greenIntensity:
        //     The green intensity of the light.
        //
        //   blueIntensity:
        //     The blue intensity of the light.
        //
        //   redOutput:
        //     Output spherical harmonic (SH) vector for the blue component.
        //
        //   greenOutput:
        //     Output spherical harmonic (SH) vector for the green component.
        //
        //   blueOutput:
        //     Output spherical harmonic (SH) vector for the blue component.
        void EvaluateDirectionalLight(int order, Vector3 direction, float redIntensity, float greenIntensity, float blueIntensity, GraphicsStream redOutput, GraphicsStream greenOutput, GraphicsStream blueOutput);
        //
        // Summary:
        //     Evaluates a light that is a linear interpolation between two colors over
        //     the sphere.
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateHemisphereLight()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateHemisphereLight()
        //     - 1.
        //
        //   direction:
        //     The (x, y, z) hemisphere axis direction vector in which to evaluate the spherical
        //     harmonic (SH) basis functions. See Remarks.
        //
        //   top:
        //     The sky color.
        //
        //   bottom:
        //     The ground color.
        //
        //   redOutput:
        //     Output spherical harmonic (SH) vector for the red component.
        //
        //   greenOutput:
        //     Output spherical harmonic (SH) vector for the green component.
        //
        //   blueOutput:
        //     Output spherical harmonic (SH) vector for the red component.
        void EvaluateHemisphereLight(int order, Vector3 direction, ColorValue top, ColorValue bottom, float[] redOutput, float[] greenOutput, float[] blueOutput);
        //
        // Summary:
        //     Evaluates a light that is a linear interpolation between two colors over
        //     the sphere.
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateHemisphereLight()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateHemisphereLight()
        //     - 1.
        //
        //   direction:
        //     The (x, y, z) hemisphere axis direction vector in which to evaluate the spherical
        //     harmonic (SH) basis functions. See Remarks.
        //
        //   top:
        //     The sky color.
        //
        //   bottom:
        //     The ground color.
        //
        //   redOutput:
        //     Output spherical harmonic (SH) vector for the blue component.
        //
        //   greenOutput:
        //     Output spherical harmonic (SH) vector for the green component.
        //
        //   blueOutput:
        //     Output spherical harmonic (SH) vector for the blue component.
        void EvaluateHemisphereLight(int order, Vector3 direction, ColorValue top, ColorValue bottom, GraphicsStream redOutput, GraphicsStream greenOutput, GraphicsStream blueOutput);
        //
        // Summary:
        //     Evaluates a spherical light and returns spectral spherical harmonic (SH)
        //     data.
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateSphericalLight()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateSphericalLight()
        //     - 1.
        //
        //   position:
        //     The (x, y, z) hemisphere axis direction vector in which to evaluate the spherical
        //     harmonic (SH) basis functions. See Remarks.
        //
        //   radius:
        //     Radius of the spherical light source.
        //
        //   redIntensity:
        //     The red intensity of the light.
        //
        //   greenIntensity:
        //     The green intensity of the light.
        //
        //   blueIntensity:
        //     The blue intensity of the light.
        //
        //   redOutput:
        //     Output spherical harmonic (SH) vector for the red component.
        //
        //   greenOutput:
        //     Output spherical harmonic (SH) vector for the green component.
        //
        //   blueOutput:
        //     Output spherical harmonic (SH) vector for the red component.
        void EvaluateSphericalLight(int order, Vector3 position, float radius, float redIntensity, float greenIntensity, float blueIntensity, float[] redOutput, float[] greenOutput, float[] blueOutput);
        //
        // Summary:
        //     Evaluates a spherical light and returns spectral spherical harmonic (SH)
        //     data.
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateSphericalLight()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.EvaluateSphericalLight()
        //     - 1.
        //
        //   position:
        //     The (x, y, z) hemisphere axis direction vector in which to evaluate the spherical
        //     harmonic (SH) basis functions. See Remarks.
        //
        //   radius:
        //     Radius of the spherical light source.
        //
        //   redIntensity:
        //     The red intensity of the light.
        //
        //   greenIntensity:
        //     The green intensity of the light.
        //
        //   blueIntensity:
        //     The blue intensity of the light.
        //
        //   redOutput:
        //     Output spherical harmonic (SH) vector for the blue component.
        //
        //   greenOutput:
        //     Output spherical harmonic (SH) vector for the green component.
        //
        //   blueOutput:
        //     Output spherical harmonic (SH) vector for the blue component.
        void EvaluateSphericalLight(int order, Vector3 position, float radius, float redIntensity, float greenIntensity, float blueIntensity, GraphicsStream redOutput, GraphicsStream greenOutput, GraphicsStream blueOutput);
        //
        // Summary:
        //     Projects a function represented on a cube map into spherical harmonics (SH).
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.ProjectCubeMap()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.ProjectCubeMap()
        //     - 1.
        //
        //   cubeMap:
        //     A Microsoft.DirectX.Direct3D.CubeTexture object used as the source cube map.
        //
        //   redOutput:
        //     Output spherical harmonic (SH) vector for the red component.
        //
        //   greenOutput:
        //     Output spherical harmonic (SH) vector for the green component.
        //
        //   blueOutput:
        //     Output spherical harmonic (SH) vector for the red component.
        void ProjectCubeMap(int order, ICubeTexture cubeMap, float[] redOutput, float[] greenOutput, float[] blueOutput);
        //
        // Summary:
        //     Projects a function represented on a cube map into spherical harmonics (SH).
        //
        // Parameters:
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.ProjectCubeMap()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.ProjectCubeMap()
        //     - 1.
        //
        //   cubeMap:
        //     A Microsoft.DirectX.Direct3D.CubeTexture object used as the source cube map.
        //
        //   redOutput:
        //     Output spherical harmonic (SH) vector for the blue component.
        //
        //   greenOutput:
        //     Output spherical harmonic (SH) vector for the green component.
        //
        //   blueOutput:
        //     Output spherical harmonic (SH) vector for the blue component.
        void ProjectCubeMap(int order, ICubeTexture cubeMap, GraphicsStream redOutput, GraphicsStream greenOutput, GraphicsStream blueOutput);
        //
        // Summary:
        //     Rotates the spherical harmonic (SH) vector by the given matrix.
        //
        // Parameters:
        //   output:
        //     An array of floating point values that represent spherical harmonic (SH)
        //     output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Rotate()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Rotate()
        //     - 1.
        //
        //   rotation:
        //     The rotation Microsoft.DirectX.Matrix. The rotation sub-matrix must be orthogonal,
        //     with a unit determinant.
        //
        //   input:
        //     An array of floating point values that represent rotated spherical harmonic
        //     (SH) coefficients.
        void Rotate(float[] output, int order, Matrix rotation, float[] input);
        //
        // Summary:
        //     Rotates the spherical harmonic (SH) vector by the given matrix.
        //
        // Parameters:
        //   output:
        //     An array of floating point values that represent spherical harmonic (SH)
        //     output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Rotate()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Rotate()
        //     - 1.
        //
        //   rotation:
        //     The rotation Microsoft.DirectX.Matrix. The rotation sub-matrix must be orthogonal,
        //     with a unit determinant.
        //
        //   input:
        //     A Microsoft.DirectX.GraphicsStream object that represents rotated spherical
        //     harmonic (SH) coefficients.
        void Rotate(float[] output, int order, Matrix rotation, GraphicsStream input);
        //
        // Summary:
        //     Rotates the spherical harmonic (SH) vector by the given matrix.
        //
        // Parameters:
        //   output:
        //     A Microsoft.DirectX.GraphicsStream object that represents spherical harmonic
        //     (SH) output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Rotate()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Rotate()
        //     - 1.
        //
        //   rotation:
        //     The rotation Microsoft.DirectX.Matrix. The rotation sub-matrix must be orthogonal,
        //     with a unit determinant.
        //
        //   input:
        //     An array of floating point values that represent rotated spherical harmonic
        //     (SH) coefficients.
        void Rotate(GraphicsStream output, int order, Matrix rotation, float[] input);
        //
        // Summary:
        //     Rotates the spherical harmonic (SH) vector by the given matrix.
        //
        // Parameters:
        //   output:
        //     A Microsoft.DirectX.GraphicsStream object that represents spherical harmonic
        //     (SH) output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Rotate()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Rotate()
        //     - 1.
        //
        //   rotation:
        //     The rotation Microsoft.DirectX.Matrix. The rotation sub-matrix must be orthogonal,
        //     with a unit determinant.
        //
        //   input:
        //     A Microsoft.DirectX.GraphicsStream object that represents rotated spherical
        //     harmonic (SH) coefficients.
        void Rotate(GraphicsStream output, int order, Matrix rotation, GraphicsStream input);
        //
        // Summary:
        //     Rotates the spherical harmonic (SH) vector in the z-axis by the given angle.
        //
        // Parameters:
        //   output:
        //     An array of floating point values that represent spherical harmonic (SH)
        //     output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.RotateZ()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.RotateZ()
        //     - 1.
        //
        //   angle:
        //     Rotation angle in radians. The rotation is performed around the z-axis.
        //
        //   input:
        //     An array of floating point values that represent rotated spherical harmonic
        //     (SH) coefficients.
        void RotateZ(float[] output, int order, float angle, float[] input);
        //
        // Summary:
        //     Rotates the spherical harmonic (SH) vector in the z-axis by the given angle.
        //
        // Parameters:
        //   output:
        //     An array of floating point values that represent spherical harmonic (SH)
        //     output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.RotateZ()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.RotateZ()
        //     - 1.
        //
        //   angle:
        //     Rotation angle in radians. The rotation is performed around the z-axis.
        //
        //   input:
        //     A Microsoft.DirectX.GraphicsStream object that represents rotated spherical
        //     harmonic (SH) coefficients.
        void RotateZ(float[] output, int order, float angle, GraphicsStream input);
        //
        // Summary:
        //     Rotates the spherical harmonic (SH) vector in the z-axis by the given angle.
        //
        // Parameters:
        //   output:
        //     A Microsoft.DirectX.GraphicsStream object that represents spherical harmonic
        //     (SH) output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.RotateZ()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.RotateZ()
        //     - 1.
        //
        //   angle:
        //     Rotation angle in radians. The rotation is performed around the z-axis.
        //
        //   input:
        //     An array of floating point values that represent rotated spherical harmonic
        //     (SH) coefficients.
        void RotateZ(GraphicsStream output, int order, float angle, float[] input);
        //
        // Summary:
        //     Rotates the spherical harmonic (SH) vector in the z-axis by the given angle.
        //
        // Parameters:
        //   output:
        //     A Microsoft.DirectX.GraphicsStream object that represents spherical harmonic
        //     (SH) output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.RotateZ()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.RotateZ()
        //     - 1.
        //
        //   angle:
        //     Rotation angle in radians. The rotation is performed around the z-axis.
        //
        //   input:
        //     A Microsoft.DirectX.GraphicsStream object that represents rotated spherical
        //     harmonic (SH) coefficients.
        void RotateZ(GraphicsStream output, int order, float angle, GraphicsStream input);
        //
        // Summary:
        //     Scales a spherical harmonic (SH) vector, in other words, Out[i] = A[i] *
        //     Scale.
        //
        // Parameters:
        //   output:
        //     An array of floating point values that represent spherical harmonic (SH)
        //     output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Scale()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Scale()
        //     - 1.
        //
        //   input:
        //     An array of floating point values that represents the spherical harmonic
        //     (SH) vector to scale.
        //
        //   scaleFactor:
        //     The scale value.
        void Scale(float[] output, int order, float[] input, float scaleFactor);
        //
        // Summary:
        //     Scales a spherical harmonic (SH) vector, in other words, Out[i] = A[i] *
        //     Scale.
        //
        // Parameters:
        //   output:
        //     An array of floating point values that represent spherical harmonic (SH)
        //     output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Scale()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Scale()
        //     - 1.
        //
        //   input:
        //     A Microsoft.DirectX.GraphicsStream object that represents the spherical harmonic
        //     (SH) vector to scale.
        //
        //   scaleFactor:
        //     The scale value.
        void Scale(float[] output, int order, GraphicsStream input, float scaleFactor);
        //
        // Summary:
        //     Scales a spherical harmonic (SH) vector, in other words, Out[i] = A[i] *
        //     Scale.
        //
        // Parameters:
        //   output:
        //     A Microsoft.DirectX.GraphicsStream object that represents spherical harmonic
        //     (SH) output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Scale()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Scale()
        //     - 1.
        //
        //   input:
        //     An array of floating point values that represents the spherical harmonic
        //     (SH) vector to scale.
        //
        //   scaleFactor:
        //     The scale value.
        void Scale(GraphicsStream output, int order, float[] input, float scaleFactor);
        //
        // Summary:
        //     Scales a spherical harmonic (SH) vector, in other words, Out[i] = A[i] *
        //     Scale.
        //
        // Parameters:
        //   output:
        //     A Microsoft.DirectX.GraphicsStream object that represents spherical harmonic
        //     (SH) output coefficients.
        //
        //   order:
        //     Order of the spherical harmonic (SH) evaluation. Must be in the range of
        //     Microsoft.DirectX.Direct3D.SphericalHarmonics.MinimumOrder to Microsoft.DirectX.Direct3D.SphericalHarmonics.MaximumOrder,
        //     inclusive. The evaluation generates Microsoft.DirectX.Direct3D.SphericalHarmonics.Scale()2
        //     coefficients. The degree of the evaluation is Microsoft.DirectX.Direct3D.SphericalHarmonics.Scale()
        //     - 1.
        //
        //   input:
        //     A Microsoft.DirectX.GraphicsStream object that represents the spherical harmonic
        //     (SH) vector to scale.
        //
        //   scaleFactor:
        //     The scale value.
        void Scale(GraphicsStream output, int order, GraphicsStream input, float scaleFactor);
    }
}
