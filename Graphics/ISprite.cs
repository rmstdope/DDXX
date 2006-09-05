using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface ISprite
    {
        // Summary:
        //     Retrieves the Microsoft Direct3D device associated with a sprite object.
        Device Device { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        bool Disposed { get; }
        //
        // Summary:
        //     Retrieves or sets a Microsoft.DirectX.Matrix object.
        Matrix Transform { get; set; }
        // Summary:
        //     Prepares a device for drawing sprites.
        //
        // Parameters:
        //   flags:
        //     Combination of zero or more values from the Microsoft.DirectX.Direct3D.SpriteFlags
        //     enumeration that describe sprite rendering options.
        void Begin(SpriteFlags flags);
        //
        // Summary:
        //     Immediately releases the unmanaged resources used by the Microsoft.DirectX.Direct3D.Sprite
        //     object.
        void Dispose();
        //
        // Summary:
        //     Adds a sprite to the list of batched sprites.
        //
        // Parameters:
        //   srcTexture:
        //     A Microsoft.DirectX.Direct3D.ITexture object that represents the sprite ITexture.
        //
        //   center:
        //     A Microsoft.DirectX.Vector3 structure that identifies the center of the sprite.
        //     A value of (0,0,0) indicates the upper-left corner.
        //
        //   position:
        //     A Microsoft.DirectX.Vector3 structure that identifies the position of the
        //     sprite. A value of (0,0,0) indicates the upper-left corner.
        //
        //   color:
        //     Color value represented as an integer. The color and alpha channels are modulated
        //     by this value. A value of 16777215 maintains the original source color and
        //     alpha data.
        void Draw(ITexture srcTexture, Vector3 center, Vector3 position, int color);
        //
        // Summary:
        //     Adds a sprite to the list of batched sprites.
        //
        // Parameters:
        //   srcTexture:
        //     A Microsoft.DirectX.Direct3D.ITexture object that represents the sprite ITexture.
        //
        //   srcRectangle:
        //     A System.Drawing.Rectangle object that indicates the portion of the source
        //     ITexture to use for the sprite. Specify System.Drawing.Rectangle.Empty to
        //     use the entire source image for the sprite.
        //
        //   center:
        //     A Microsoft.DirectX.Vector3 structure that identifies the center of the sprite.
        //     A value of (0,0,0) indicates the upper-left corner.
        //
        //   position:
        //     A Microsoft.DirectX.Vector3 structure that identifies the position of the
        //     sprite. A value of (0,0,0) indicates the upper-left corner.
        //
        //   color:
        //     A System.Drawing.Color structure. The color and alpha channels are modulated
        //     by this value. The System.Drawing.Color value maintains the original source
        //     color and alpha data.
        void Draw(ITexture srcTexture, Rectangle srcRectangle, Vector3 center, Vector3 position, Color color);
        //
        // Summary:
        //     Adds a sprite to the list of batched sprites.
        //
        // Parameters:
        //   srcTexture:
        //     A Microsoft.DirectX.Direct3D.ITexture object that represents the sprite ITexture.
        //
        //   srcRectangle:
        //     A System.Drawing.Rectangle object that indicates the portion of the source
        //     ITexture to use for the sprite. Specify System.Drawing.Rectangle.Empty to
        //     use the entire source image for the sprite.
        //
        //   center:
        //     A Microsoft.DirectX.Vector3 structure that identifies the center of the sprite.
        //     A value of (0,0,0) indicates the upper-left corner.
        //
        //   position:
        //     A Microsoft.DirectX.Vector3 structure that identifies the position of the
        //     sprite. A value of (0,0,0) indicates the upper-left corner.
        //
        //   color:
        //     Color value represented as an integer. The color and alpha channels are modulated
        //     by this value. A value of 16777215 maintains the original source color and
        //     alpha data.
        void Draw(ITexture srcTexture, Rectangle srcRectangle, Vector3 center, Vector3 position, int color);
        void Draw2D(ITexture srcTexture, PointF rotationCenter, float rotationAngle, PointF position, Color color);
        void Draw2D(ITexture srcTexture, PointF rotationCenter, float rotationAngle, PointF position, int color);
        void Draw2D(ITexture srcTexture, Rectangle srcRectangle, SizeF destinationSize, PointF position, Color color);
        void Draw2D(ITexture srcTexture, Rectangle srcRectangle, SizeF destinationSize, PointF position, int color);
        void Draw2D(ITexture srcTexture, Rectangle srcRectangle, SizeF destinationSize, PointF rotationCenter, float rotationAngle, PointF position, Color color);
        void Draw2D(ITexture srcTexture, Rectangle srcRectangle, SizeF destinationSize, PointF rotationCenter, float rotationAngle, PointF position, int color);
        //
        // Summary:
        //     Restores the device to the state it was in before Microsoft.DirectX.Direct3D.Sprite.Microsoft.DirectX.Direct3D.Sprite.Begin(Microsoft.DirectX.Direct3D.SpriteFlags)
        //     was called.
        void End();
        //
        // Summary:
        //     Forces all batched sprites to be submitted to the device.
        void Flush();
        //
        // Summary:
        //     Sets the left-handed world-view transform for a sprite.  A call to this method
        //     is required before billboarding or sorting sprites.
        //
        // Parameters:
        //   world:
        //     A Microsoft.DirectX.Matrix object that contains a world transform.
        //
        //   view:
        //     A Microsoft.DirectX.Matrix object that contains a view transform.
        void SetWorldViewLH(Matrix world, Matrix view);
        //
        // Summary:
        //     Sets the right-handed world-view transform for a sprite.  A call to this
        //     method is required before billboarding or sorting sprites.
        //
        // Parameters:
        //   world:
        //     A Microsoft.DirectX.Matrix object that contains a world transform.
        //
        //   view:
        //     A Microsoft.DirectX.Matrix object that contains a view transform.
        void SetWorldViewRH(Matrix world, Matrix view);
    }
}
