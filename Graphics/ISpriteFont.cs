using System;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface ISpriteFont
    {
        // Summary:
        //     Gets the height of a line using these font characters.
        //
        // Returns:
        //     The height of one line, in pixels.
        int LineSpacing { get; }
        //
        // Summary:
        //     Gets or sets the spacing of the font characters.
        //
        // Returns:
        //     The spacing, in pixels, of the font characters.
        float Spacing { get; set; }
        // Summary:
        //     Returns the height and width of a given string as a Framework.Vector2.
        //
        // Parameters:
        //   text:
        //     The string to measure.
        //
        // Returns:
        //     The height and width, in pixels, of text, when it is rendered.
        Vector2 MeasureString(string text);
    }
}
