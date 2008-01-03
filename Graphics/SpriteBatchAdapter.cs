using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class SpriteBatchAdapter : ISpriteBatch
    {
        private SpriteBatch spriteBatch;

        public SpriteBatchAdapter(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        #region ISpriteBatch Members

        public IGraphicsDevice GraphicsDevice
        {
            get { return new GraphicsDeviceAdapter(spriteBatch.GraphicsDevice); }
        }

        public string Name
        {
            get
            {
                return spriteBatch.Name;
            }
            set
            {
                spriteBatch.Name = value;
            }
        }

        public object Tag
        {
            get
            {
                return spriteBatch.Tag;
            }
            set
            {
                spriteBatch.Tag = value;
            }
        }

        public void Begin()
        {
            spriteBatch.Begin();
        }

        public void Begin(SpriteBlendMode blendMode)
        {
            spriteBatch.Begin(blendMode);
        }

        public void Begin(SpriteBlendMode blendMode, SpriteSortMode sortMode, SaveStateMode stateMode)
        {
            spriteBatch.Begin(blendMode, sortMode, stateMode);
        }

        public void Begin(SpriteBlendMode blendMode, SpriteSortMode sortMode, SaveStateMode stateMode, Matrix transformMatrix)
        {
            spriteBatch.Begin(blendMode, sortMode, stateMode, transformMatrix);
        }

        public void Draw(ITexture2D texture, Rectangle destinationRectangle, Color color)
        {
            spriteBatch.Draw((texture as Texture2DAdapter).DxTexture2D, destinationRectangle, color);
        }

        public void Draw(ITexture2D texture, Vector2 position, Color color)
        {
            spriteBatch.Draw((texture as Texture2DAdapter).DxTexture2D, position, color);
        }

        public void Draw(ITexture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            spriteBatch.Draw((texture as Texture2DAdapter).DxTexture2D, destinationRectangle, sourceRectangle, color);
        }

        public void Draw(ITexture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            spriteBatch.Draw((texture as Texture2DAdapter).DxTexture2D, position, sourceRectangle, color);
        }

        public void Draw(ITexture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw((texture as Texture2DAdapter).DxTexture2D, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        public void Draw(ITexture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw((texture as Texture2DAdapter).DxTexture2D, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void Draw(ITexture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw((texture as Texture2DAdapter).DxTexture2D, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString((spriteFont as SpriteFontAdapter).DxSpriteFont, text, position, color);
        }

        public void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.DrawString((spriteFont as SpriteFontAdapter).DxSpriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void DrawString(ISpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.DrawString((spriteFont as SpriteFontAdapter).DxSpriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void End()
        {
            spriteBatch.End();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            spriteBatch.Dispose();
        }

        #endregion
    }
}
