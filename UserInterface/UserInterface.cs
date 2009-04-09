using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.UserInterface
{
    public class UserInterface : IUserInterface, IDrawResources
    {
        private ISpriteBatch spriteBatch;
        private ITexture2D whiteTexture;
        private Dictionary<FontSize, ISpriteFont> spriteFonts;
        private float aspectRatio;

        public UserInterface()
        {
            spriteFonts = new Dictionary<FontSize, ISpriteFont>();
        }

        public void Initialize(IGraphicsFactory graphicsFactory, ITextureFactory textureFactory)
        {
            spriteBatch = graphicsFactory.CreateSpriteBatch();
            if (!spriteFonts.ContainsKey(FontSize.Medium))
                spriteFonts[FontSize.Medium] = graphicsFactory.SpriteFontFromFile("Content/fonts/TweakerFont");
            whiteTexture = textureFactory.WhiteTexture;
            aspectRatio = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth /
                (float)spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
        }

        public void DrawControl(IControl control)
        {
            spriteBatch.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            spriteBatch.GraphicsDevice.RenderState.BlendFunction = BlendFunction.Add;
            spriteBatch.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            spriteBatch.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            int num = control.DrawControl(this);
            Console.WriteLine("Drew {0} controls.", num);
        }


        public void SetFont(FontSize size, ISpriteFont font)
        {
            spriteFonts[size] = font;
        }


        #region IDrawResources Members

        public ISpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public ISpriteFont GetSpriteFont(FontSize size)
        {
            return spriteFonts[size];
        }

        public ITexture2D WhiteTexture
        {
            get { return whiteTexture; }
        }

        public float AspectRatio
        {
            get { return aspectRatio; }
        }

        public IDrawResources DrawResources
        {
            get { return this; }
        }

        #endregion
    }
}
