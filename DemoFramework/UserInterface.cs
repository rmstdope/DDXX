using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public class UserInterface : IUserInterface
    {
        private ISpriteBatch spriteBatch;
        private ITexture2D whiteTexture;
        private ISpriteFont spriteFont;

        public UserInterface()
        {
        }

        public void Initialize(IGraphicsFactory graphicsFactory, ITextureFactory textureFactory)
        {
            spriteBatch = graphicsFactory.CreateSpriteBatch();
            spriteFont = graphicsFactory.SpriteFontFromFile("Content/fonts/TweakerFont");
            whiteTexture = textureFactory.WhiteTexture;
        }

        public void DrawControl(BaseControl control)
        {
            spriteBatch.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            spriteBatch.GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add;
            spriteBatch.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            spriteBatch.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            control.DrawControl(spriteBatch, spriteFont, whiteTexture);
        }

    }
}
