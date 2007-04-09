using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.Graphics
{
    public interface ILine : IDisposable
    {
        // Summary:
        //     Retrieves or sets the antialiasing switch value for a line.
        bool Antialias { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        bool Disposed { get; }
        //
        // Summary:
        //     Retrieves or sets a value that switches between Microsoft Direct3D and OpenGL
        //     line-drawing modes.
        bool GlLines { get; set; }
        //
        // Summary:
        //     Retrieves or sets the line stipple pattern.
        int Pattern { get; set; }
        //
        // Summary:
        //     Retrieves or sets the stipple-pattern scale value.
        float PatternScale { get; set; }
        //
        // Summary:
        //     Retrieves or sets the thickness of a line.
        float Width { get; set; }
        // Summary:
        //     Prepares a device to draw lines.
        void Begin();
        //
        // Summary:
        //     Draws a line strip in screen space. Input is in the form of an array that
        //     defines points (of Microsoft.DirectX.Vector2) on the line strip.
        //
        // Parameters:
        //   vertexList:
        //     Array of vertices that make up the line. For more information, see Microsoft.DirectX.Vector2.
        //
        //   color:
        //     A System.Drawing.Color object that specifies the color of the line.
        void Draw(Vector2[] vertexList, Color color);
        //
        // Summary:
        //     Draws a line strip in screen space. Input is in the form of an array that
        //     defines points (of Microsoft.DirectX.Vector2) on the line strip.
        //
        // Parameters:
        //   vertexList:
        //     Array of vertices that make up the line. For more information, see Microsoft.DirectX.Vector2.
        //
        //   color:
        //     An System.Int32 color value that specifies the color of the line.
        void Draw(Vector2[] vertexList, int color);
        //
        // Summary:
        //     Draws a line strip in screen space with a specified input transformation
        //     matrix.
        //
        // Parameters:
        //   vertexList:
        //     Array of vertices that make up the line. For more information, see Microsoft.DirectX.Vector2.
        //
        //   transform:
        //     Scale, rotate, and translate (SRT) matrix for transforming the points. For
        //     more information, see Microsoft.DirectX.Matrix. If this matrix is a projection
        //     matrix, any stippled lines are drawn with a perspective-correct stippling
        //     pattern. Or, vertices can be transformed and Microsoft.DirectX.Direct3D.Line.Draw()
        //     used to draw the line with a stipple pattern and no perspective correction.
        //
        //   color:
        //     A System.Drawing.Color object that specifies the color of the line.
        void DrawTransform(Vector3[] vertexList, Matrix transform, Color color);
        //
        // Summary:
        //     Draws a line strip in screen space with a specified input transformation
        //     matrix.
        //
        // Parameters:
        //   vertexList:
        //     Array of vertices that make up the line. For more information, see Microsoft.DirectX.Vector2.
        //
        //   transform:
        //     Scale, rotate, and translate (SRT) matrix for transforming the points. For
        //     more information, see Microsoft.DirectX.Matrix. If this matrix is a projection
        //     matrix, any stippled lines are drawn with a perspective-correct stippling
        //     pattern. Or, vertices can be transformed and Microsoft.DirectX.Direct3D.Line.Draw()
        //     used to draw the line with a stipple pattern and no perspective correction.
        //
        //   color:
        //     An System.Int32 color value that specifies the color of the line.
        void DrawTransform(Vector3[] vertexList, Matrix transform, int color);
        //
        // Summary:
        //     Restores the device to the state it was in when Microsoft.DirectX.Direct3D.Line.Begin()
        //     was called.
        void End();
    }
}
