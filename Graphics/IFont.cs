using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.Graphics
{
    public interface IFont
    {
        // Summary:
        //     Retrieves a description of the current font object.
        FontDescription Description { get; }
        //
        // Summary:
        //     Draws formatted text.
        //
        // Parameters:
        //   sprite:
        //     A Microsoft.DirectX.Direct3D.Sprite object that contains the string. Can
        //     be null, in which case Microsoft Direct3D renders the string with its own
        //     sprite object.If Microsoft.DirectX.Direct3D.Font.DrawText() will be called
        //     more than once in a row, a sprite object should be specified to improve efficiency.
        //
        //   text:
        //     String to draw.
        //
        //   pos:
        //     A System.Drawing.Point structure that specifies the upper left-hand coordinates
        //     of where to draw the font.
        //
        //   color:
        //     Color of the text. For more information, see System.Drawing.Color.
        //
        // Returns:
        //     If the function succeeds, the return value is the height of the text in logical
        //     units. If Microsoft.DirectX.Direct3D.DrawTextFormat.Center or Microsoft.DirectX.Direct3D.DrawTextFormat.Bottom
        //     is specified in the Microsoft.DirectX.Direct3D.Font.DrawText() parameter,
        //     the return value is the offset from System.Drawing.Rectangle.Top to the bottom
        //     of the drawn text. If the function fails, the return value is 0.
        int DrawText(ISprite sprite, string text, Point pos, Color color);
        //
        // Summary:
        //     Draws formatted text.
        //
        // Parameters:
        //   sprite:
        //     A Microsoft.DirectX.Direct3D.Sprite object that contains the string. Can
        //     be null, in which case Microsoft Direct3D renders the string with its own
        //     sprite object.If Microsoft.DirectX.Direct3D.Font.DrawText() will be called
        //     more than once in a row, a sprite object should be specified to improve efficiency.
        //
        //   text:
        //     String to draw.
        //
        //   pos:
        //     A System.Drawing.Point structure that specifies the upper left-hand coordinates
        //     of where to draw the font.
        //
        //   color:
        //     Integer color value.
        //
        // Returns:
        //     If the function succeeds, the return value is the height of the text in logical
        //     units. If Microsoft.DirectX.Direct3D.DrawTextFormat.Center or Microsoft.DirectX.Direct3D.DrawTextFormat.Bottom
        //     is specified in the Microsoft.DirectX.Direct3D.Font.DrawText() parameter,
        //     the return value is the offset from System.Drawing.Rectangle.Top to the bottom
        //     of the drawn text. If the function fails, the return value is 0.
        int DrawText(ISprite sprite, string text, Point pos, int color);
        //
        // Summary:
        //     Draws formatted text.
        //
        // Parameters:
        //   sprite:
        //     A Microsoft.DirectX.Direct3D.Sprite object that contains the string. Can
        //     be null, in which case Microsoft Direct3D renders the string with its own
        //     sprite object.If Microsoft.DirectX.Direct3D.Font.DrawText() will be called
        //     more than once in a row, a sprite object should be specified to improve efficiency.
        //
        //   text:
        //     String to draw.
        //
        //   x:
        //     The x-coordinate where to draw the font.
        //
        //   y:
        //     The y-coordinate where to draw the font.
        //
        //   color:
        //     Color of the text. For more information, see System.Drawing.Color.
        //
        // Returns:
        //     If the function succeeds, the return value is the height of the text in logical
        //     units. If Microsoft.DirectX.Direct3D.DrawTextFormat.Center or Microsoft.DirectX.Direct3D.DrawTextFormat.Bottom
        //     is specified in the Microsoft.DirectX.Direct3D.Font.DrawText() parameter,
        //     the return value is the offset from System.Drawing.Rectangle.Top to the bottom
        //     of the drawn text. If the function fails, the return value is 0.
        int DrawText(ISprite sprite, string text, int x, int y, Color color);
        //
        // Summary:
        //     Draws formatted text.
        //
        // Parameters:
        //   sprite:
        //     A Microsoft.DirectX.Direct3D.Sprite object that contains the string. Can
        //     be null, in which case Microsoft Direct3D renders the string with its own
        //     sprite object.If Microsoft.DirectX.Direct3D.Font.DrawText() will be called
        //     more than once in a row, a sprite object should be specified to improve efficiency.
        //
        //   text:
        //     String to draw.
        //
        //   x:
        //     The x-coordinate where to draw the font.
        //
        //   y:
        //     The y-coordinate where to draw the font.
        //
        //   color:
        //     Integer color value.
        //
        // Returns:
        //     If the function succeeds, the return value is the height of the text in logical
        //     units. If Microsoft.DirectX.Direct3D.DrawTextFormat.Center or Microsoft.DirectX.Direct3D.DrawTextFormat.Bottom
        //     is specified in the Microsoft.DirectX.Direct3D.Font.DrawText() parameter,
        //     the return value is the offset from System.Drawing.Rectangle.Top to the bottom
        //     of the drawn text. If the function fails, the return value is 0.
        int DrawText(ISprite sprite, string text, int x, int y, int color);
        //
        // Summary:
        //     Draws formatted text.
        //
        // Parameters:
        //   sprite:
        //     A Microsoft.DirectX.Direct3D.Sprite object that contains the string. Can
        //     be null, in which case Microsoft Direct3D renders the string with its own
        //     sprite object.If Microsoft.DirectX.Direct3D.Font.DrawText() will be called
        //     more than once in a row, a sprite object should be specified to improve efficiency.
        //
        //   text:
        //     String to draw.
        //
        //   rect:
        //     A System.Drawing.Rectangle structure that contains the rectangle, in logical
        //     coordinates, in which the text is being formatted.
        //
        //   format:
        //     Method of formatting the text; can be any combination of values from the
        //     Microsoft.DirectX.Direct3D.DrawTextFormat enumeration.
        //
        //   color:
        //     Color of the text. For more information, see System.Drawing.Color.
        //
        // Returns:
        //     If the function succeeds, the return value is the height of the text in logical
        //     units. If Microsoft.DirectX.Direct3D.DrawTextFormat.Center or Microsoft.DirectX.Direct3D.DrawTextFormat.Bottom
        //     is specified in the Microsoft.DirectX.Direct3D.Font.DrawText() parameter,
        //     the return value is the offset from System.Drawing.Rectangle.Top to the bottom
        //     of the drawn text. If the function fails, the return value is 0.
        int DrawText(ISprite sprite, string text, Rectangle rect, DrawTextFormat format, Color color);
        //
        // Summary:
        //     Draws formatted text.
        //
        // Parameters:
        //   sprite:
        //     A Microsoft.DirectX.Direct3D.Sprite object that contains the string. Can
        //     be null, in which case Microsoft Direct3D renders the string with its own
        //     sprite object.If Microsoft.DirectX.Direct3D.Font.DrawText() will be called
        //     more than once in a row, a sprite object should be specified to improve efficiency.
        //
        //   text:
        //     String to draw.
        //
        //   rect:
        //     A System.Drawing.Rectangle structure that contains the rectangle, in logical
        //     coordinates, in which the text is being formatted.
        //
        //   format:
        //     Method of formatting the text; can be any combination of values from the
        //     Microsoft.DirectX.Direct3D.DrawTextFormat enumeration.
        //
        //   color:
        //     Integer color value.
        //
        // Returns:
        //     If the function succeeds, the return value is the height of the text in logical
        //     units. If Microsoft.DirectX.Direct3D.DrawTextFormat.Center or Microsoft.DirectX.Direct3D.DrawTextFormat.Bottom
        //     is specified in the Microsoft.DirectX.Direct3D.Font.DrawText() parameter,
        //     the return value is the offset from System.Drawing.Rectangle.Top to the bottom
        //     of the drawn text. If the function fails, the return value is 0.
        int DrawText(ISprite sprite, string text, Rectangle rect, DrawTextFormat format, int color);
        //
        // Summary:
        //     Returns information about the placement and orientation of a glyph in a character
        //     cell.
        //
        // Parameters:
        //   glyph:
        //     Glyph identifier.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Texture object that contains the glyph.
        Texture GetGlyphData(int glyph);
        //
        // Summary:
        //     Returns information about the placement and orientation of a glyph in a character
        //     cell.
        //
        // Parameters:
        //   glyph:
        //     Glyph identifier.
        //
        //   blackBox:
        //     Smallest System.Drawing.Rectangle object that completely encloses the glyph
        //     (its black box).
        //
        //   cellInc:
        //     A Microsoft.DirectX.Vector2 structure that connects the origin of the current
        //     character cell to the origin of the next character cell.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Texture object that contains the glyph.
        Texture GetGlyphData(int glyph, out Rectangle blackBox, out Point cellInc);
        //
        // Summary:
        //     Measures the rectangular dimensions of the specified text string when drawn
        //     with the specified Microsoft.DirectX.Direct3D.Sprite object and formatted
        //     with the specified Microsoft.DirectX.Direct3D.DrawTextFormat method of formatting
        //     text.
        //
        // Parameters:
        //   sprite:
        //     A Microsoft.DirectX.Direct3D.Sprite object that contains the string.
        //
        //   text:
        //     String to measure.
        //
        //   format:
        //     Method of formatting the text; can be any combination of values from the
        //     Microsoft.DirectX.Direct3D.DrawTextFormat enumeration.
        //
        //   color:
        //     Color of the text. For more information, see System.Drawing.Color.
        //
        // Returns:
        //     A System.Drawing.Rectangle structure that contains the rectangle, in logical
        //     coordinates, that encompasses the formatted text string.
        Rectangle MeasureString(ISprite sprite, string text, DrawTextFormat format, Color color);
        //
        // Summary:
        //     Measures the rectangular dimensions of the specified text string when drawn
        //     with the specified Microsoft.DirectX.Direct3D.Sprite object and formatted
        //     with the specified Microsoft.DirectX.Direct3D.DrawTextFormat method of formatting
        //     text.
        //
        // Parameters:
        //   sprite:
        //     A Microsoft.DirectX.Direct3D.Sprite object that contains the string.
        //
        //   text:
        //     String to measure.
        //
        //   format:
        //     Method of formatting the text; can be any combination of values from the
        //     Microsoft.DirectX.Direct3D.DrawTextFormat enumeration.
        //
        //   color:
        //     Integer color value.
        //
        // Returns:
        //     A System.Drawing.Rectangle structure that contains the rectangle, in logical
        //     coordinates, that encompasses the formatted text string.
        Rectangle MeasureString(ISprite sprite, string text, DrawTextFormat format, int color);
        //
        // Summary:
        //     Loads a series of characters into video memory to improve the efficiency
        //     of rendering to a device.
        //
        // Parameters:
        //   first:
        //     Identifier of the first character to load into video memory.
        //
        //   last:
        //     Identifier of the last character to load into video memory.
        void PreloadCharacters(int first, int last);
        //
        // Summary:
        //     Loads a series of glyphs into video memory to improve the efficiency of rendering
        //     to a device.
        //
        // Parameters:
        //   first:
        //     Identifier of the first glyph to load into video memory.
        //
        //   last:
        //     Identifier of the last glyph to load into video memory.
        void PreloadGlyphs(int first, int last);
        //
        // Summary:
        //     Loads formatted text into video memory to improve the efficiency of rendering
        //     to a device.
        //
        // Parameters:
        //   text:
        //     String of characters to load into video memory.
        void PreloadText(string text);
    }
}
