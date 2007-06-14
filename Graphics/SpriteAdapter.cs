using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    class SpriteAdapter : ISprite
    {
        private Sprite sprite;

        public SpriteAdapter(Device device)
        {
            this.sprite = new Sprite(device);
        }

        public Sprite DXSprite
        {
            get { return sprite; }
        }

        #region ISprite Members

        public Device Device
        {
            get { return sprite.Device; }
        }

        public bool Disposed
        {
            get { return sprite.Disposed; }
        }

        public Matrix Transform
        {
            get
            {
                return sprite.Transform;
            }
            set
            {
                sprite.Transform = value;
            }
        }

        public void Begin(SpriteFlags flags)
        {
            sprite.Begin(flags);
        }

        public void Dispose()
        {
            sprite.Dispose();
        }

        public void Draw(ITexture srcTexture, Vector3 center, Vector3 position, int color)
        {
            sprite.Draw(((TextureAdapter)srcTexture).TextureDX, center, position, color);
        }

        public void Draw(ITexture srcTexture, Rectangle srcRectangle, Vector3 center, Vector3 position, Color color)
        {
            sprite.Draw(((TextureAdapter)srcTexture).TextureDX, srcRectangle, center, position, color);
        }

        public void Draw(ITexture srcTexture, Rectangle srcRectangle, Vector3 center, Vector3 position, int color)
        {
            sprite.Draw(((TextureAdapter)srcTexture).TextureDX, srcRectangle, center, position, color);
        }

        public void Draw2D(ITexture srcTexture, PointF rotationCenter, float rotationAngle, PointF position, Color color)
        {
            sprite.Draw2D(((TextureAdapter)srcTexture).TextureDX, rotationCenter, rotationAngle, position, color);
        }

        public void Draw2D(ITexture srcTexture, PointF rotationCenter, float rotationAngle, PointF position, int color)
        {
            sprite.Draw2D(((TextureAdapter)srcTexture).TextureDX, rotationCenter, rotationAngle, position, color);
        }

        public void Draw2D(ITexture srcTexture, Rectangle srcRectangle, SizeF destinationSize, PointF position, Color color)
        {
            sprite.Draw2D(((TextureAdapter)srcTexture).TextureDX, srcRectangle, destinationSize, position, color);
        }

        public void Draw2D(ITexture srcTexture, Rectangle srcRectangle, SizeF destinationSize, PointF position, int color)
        {
            sprite.Draw2D(((TextureAdapter)srcTexture).TextureDX, srcRectangle, destinationSize, position, color);
        }

        public void Draw2D(ITexture srcTexture, Rectangle srcRectangle, SizeF destinationSize, PointF rotationCenter, float rotationAngle, PointF position, Color color)
        {
            sprite.Draw2D(((TextureAdapter)srcTexture).TextureDX, srcRectangle, destinationSize, rotationCenter, rotationAngle, position, color);
        }

        public void Draw2D(ITexture srcTexture, Rectangle srcRectangle, SizeF destinationSize, PointF rotationCenter, float rotationAngle, PointF position, int color)
        {
            sprite.Draw2D(((TextureAdapter)srcTexture).TextureDX, srcRectangle, destinationSize, rotationCenter, rotationAngle, position, color);
        }

        public void End()
        {
            sprite.End();
        }

        public void Flush()
        {
            sprite.Flush();
        }

        public void SetWorldViewLH(Matrix world, Matrix view)
        {
            sprite.SetWorldViewLH(world, view);
        }

        public void SetWorldViewRH(Matrix world, Matrix view)
        {
            sprite.SetWorldViewRH(world, view);
        }

        #endregion
    }
}
