using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface ISpriteBatch : IDisposable
    {
        // Summary:
        //     Gets the graphics device associated with this SpriteBatch.
        //
        // Returns:
        //     The graphics device associated with this SpriteBatch.
        IGraphicsDevice GraphicsDevice { get; }
        //
        // Summary:
        //     Gets or sets the name of this sprite batch.
        //
        // Returns:
        //     The name of this sprite batch.
        string Name { get; set; }
        //
        // Summary:
        //     Gets or sets an object that uniquely identifies this sprite batch.
        //
        // Returns:
        //     An object that uniquely identifies this sprite batch.
        object Tag { get; set; }
        // Summary:
        //     Prepares the graphics device for drawing sprites.
        void Begin();
        //
        // Summary:
        //     Prepares the graphics device for drawing sprites with specified blending
        //     options.
        //
        // Parameters:
        //   blendMode:
        //     Blending options to use when rendering.
        void Begin(SpriteBlendMode blendMode);
        //
        // Summary:
        //     Prepares the graphics device for drawing sprites with specified blending,
        //     sorting, and render state options.
        //
        // Parameters:
        //   blendMode:
        //     Blending options to use when rendering.
        //
        //   sortMode:
        //     Sorting options to use when rendering.
        //
        //   stateMode:
        //     Rendering state options.
        void Begin(SpriteBlendMode blendMode, SpriteSortMode sortMode, SaveStateMode stateMode);
        //
        // Summary:
        //     Prepares the graphics device for drawing sprites with specified blending,
        //     sorting, and render state options.
        //
        // Parameters:
        //   blendMode:
        //     Blending options to use when rendering.
        //
        //   sortMode:
        //     Sorting options to use when rendering.
        //
        //   stateMode:
        //     Rendering state options.
        //
        //   transformMatrix:
        //     A matrix to apply to position, rotation, scale, and depth data passed to
        //     SpriteBatch.Draw.
        void Begin(SpriteBlendMode blendMode, SpriteSortMode sortMode, SaveStateMode stateMode, Matrix transformMatrix);
        //
        // Summary:
        //     Adds a sprite to the batch of sprites to be rendered, specifying the texture,
        //     destination rectangle, and color tint.
        //
        // Parameters:
        //   texture:
        //     The sprite texture.
        //
        //   destinationRectangle:
        //     A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        //     If this rectangle is not the same size as sourcerectangle, the sprite is
        //     scaled to fit.
        //
        //   color:
        //     The color channel modulation to use. Use Color.White for full color with
        //     no tinting.
        void Draw(ITexture2D texture, Rectangle destinationRectangle, Color color);
        //
        // Summary:
        //     Adds a sprite to the batch of sprites to be rendered, specifying the texture,
        //     screen position, and color tint.
        //
        // Parameters:
        //   texture:
        //     The sprite texture.
        //
        //   position:
        //     The location, in screen coordinates, where the sprite will be drawn.
        //
        //   color:
        //     The color channel modulation to use. Use Color.White for full color with
        //     no tinting.
        void Draw(ITexture2D texture, Vector2 position, Color color);
        //
        // Summary:
        //     Adds a sprite to the batch of sprites to be rendered, specifying the texture,
        //     destination and source rectangles, and color tint.
        //
        // Parameters:
        //   texture:
        //     The sprite texture.
        //
        //   destinationRectangle:
        //     A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        //     If this rectangle is not the same size as sourcerectangle the sprite will
        //     be scaled to fit.
        //
        //   sourceRectangle:
        //     A rectangle specifying, in texels, which section of the rectangle to draw.
        //     Use null to draw the entire texture.
        //
        //   color:
        //     The color channel modulation to use. Use Color.White for full color with
        //     no tinting.
        void Draw(ITexture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color);
        //
        // Summary:
        //     Adds a sprite to the batch of sprites to be rendered, specifying the texture,
        //     screen position, source rectangle, and color tint.
        //
        // Parameters:
        //   texture:
        //     The sprite texture.
        //
        //   position:
        //     The location, in screen coordinates, where the sprite will be drawn.
        //
        //   sourceRectangle:
        //     A rectangle specifying, in texels, which section of the rectangle to draw.
        //     Use null to draw the entire texture.
        //
        //   color:
        //     The color channel modulation to use. Use Color.White for full color with
        //     no tinting.
        void Draw(ITexture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color);
        //
        // Summary:
        //     Adds a sprite to the batch of sprites to be rendered, specifying the texture,
        //     destination and source rectangles, color tint, rotation, origin, effects,
        //     and sort depth.
        //
        // Parameters:
        //   texture:
        //     The sprite texture.
        //
        //   destinationRectangle:
        //     A rectangle specifying, in screen coordinates, where the sprite will be drawn.
        //     If this rectangle is not the same size as sourcerectangle, the sprite is
        //     scaled to fit.
        //
        //   sourceRectangle:
        //     A rectangle specifying, in texels, which section of the rectangle to draw.
        //     Use null to draw the entire texture.
        //
        //   color:
        //     The color channel modulation to use. Use Color.White for full color with
        //     no tinting.
        //
        //   rotation:
        //     The angle, in radians, to rotate the sprite around the origin.
        //
        //   origin:
        //     The origin of the sprite. Specify (0,0) for the upper-left corner.
        //
        //   effects:
        //     Rotations to apply prior to rendering.
        //
        //   layerDepth:
        //     The sorting depth of the sprite, between 0 (front) and 1 (back).
        void Draw(ITexture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth);
        //
        // Summary:
        //     Adds a sprite to the batch of sprites to be rendered, specifying the texture,
        //     screen position, optional source rectangle, color tint, rotation, origin,
        //     scale, effects, and sort depth.
        //
        // Parameters:
        //   texture:
        //     The sprite texture.
        //
        //   position:
        //     The location, in screen coordinates, where the sprite will be drawn.
        //
        //   sourceRectangle:
        //     A rectangle specifying, in texels, which section of the rectangle to draw.
        //     Use null to draw the entire texture.
        //
        //   color:
        //     The color channel modulation to use. Use Color.White for full color with
        //     no tinting.
        //
        //   rotation:
        //     The angle, in radians, to rotate the sprite around the origin.
        //
        //   origin:
        //     The origin of the sprite. Specify (0,0) for the upper-left corner.
        //
        //   scale:
        //     Uniform multiple by which to scale the sprite width and height.
        //
        //   effects:
        //     Rotations to apply prior to rendering.
        //
        //   layerDepth:
        //     The sorting depth of the sprite, between 0 (front) and 1 (back).
        void Draw(ITexture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
        //
        // Summary:
        //     Adds a sprite to the batch of sprites to be rendered, specifying the texture,
        //     screen position, source rectangle, color tint, rotation, origin, scale, effects,
        //     and sort depth.
        //
        // Parameters:
        //   texture:
        //     The sprite texture.
        //
        //   position:
        //     The location, in screen coordinates, where the sprite will be drawn.
        //
        //   sourceRectangle:
        //     A rectangle specifying, in texels, which section of the rectangle to draw.
        //     Use null to draw the entire texture.
        //
        //   color:
        //     The color channel modulation to use. Use Color.White for full color with
        //     no tinting.
        //
        //   rotation:
        //     The angle, in radians, to rotate the sprite around the origin.
        //
        //   origin:
        //     The origin of the sprite. Specify (0,0) for the upper-left corner.
        //
        //   scale:
        //     Vector containing separate scalar multiples for the x- and y-axes of the
        //     sprite.
        //
        //   effects:
        //     Rotations to apply before rendering.
        //
        //   layerDepth:
        //     The sorting depth of the sprite, between 0 (front) and 1 (back).
        void Draw(ITexture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);
        //
        // Summary:
        //     Adds a sprite string to the batch of sprites to be rendered, specifying the
        //     font, output text, screen position, and color tint.
        //
        // Parameters:
        //   spriteFont:
        //     The sprite font.
        //
        //   text:
        //     The string to draw.
        //
        //   position:
        //     The location, in screen coordinates, where the text will be drawn.
        //
        //   color:
        //     The desired color of the text.
        void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color);
        //
        // Summary:
        //     Adds a sprite string to the batch of sprites to be rendered, specifying the
        //     font, output text, screen position, color tint, rotation, origin, scale,
        //     and effects.
        //
        // Parameters:
        //   spriteFont:
        //     The sprite font.
        //
        //   text:
        //     The string to draw.
        //
        //   position:
        //     The location, in screen coordinates, where the text will be drawn.
        //
        //   color:
        //     The desired color of the text.
        //
        //   rotation:
        //     The angle, in radians, to rotate the text around the origin.
        //
        //   origin:
        //     The origin of the string. Specify (0,0) for the upper-left corner.
        //
        //   scale:
        //     Uniform multiple by which to scale the sprite width and height.
        //
        //   effects:
        //     Rotations to apply prior to rendering.
        //
        //   layerDepth:
        //     The sorting depth of the sprite, between 0 (front) and 1 (back).
        void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
        //
        // Summary:
        //     Adds a sprite string to the batch of sprites to be rendered, specifying the
        //     font, output text, screen position, color tint, rotation, origin, scale,
        //     effects, and depth.
        //
        // Parameters:
        //   spriteFont:
        //     The sprite font.
        //
        //   text:
        //     The string to draw.
        //
        //   position:
        //     The location, in screen coordinates, where the text will be drawn.
        //
        //   color:
        //     The desired color of the text.
        //
        //   rotation:
        //     The angle, in radians, to rotate the text around the origin.
        //
        //   origin:
        //     The origin of the string. Specify (0,0) for the upper-left corner.
        //
        //   scale:
        //     Vector containing separate scalar multiples for the x- and y-axes of the
        //     sprite.
        //
        //   effects:
        //     Rotations to apply prior to rendering.
        //
        //   layerDepth:
        //     The sorting depth of the sprite, between 0 (front) and 1 (back).
        void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth);
        //
        // Summary:
        //     Flushes the sprite batch and restores the device state to how it was before
        //     SpriteBatch.Begin was called.
        void End();
    }
}
