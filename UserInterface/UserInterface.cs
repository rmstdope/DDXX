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
        private SpriteBatch spriteBatch;
        private Texture2D whiteTexture;
        private Dictionary<FontSize, SpriteFont> spriteFonts;
        private float aspectRatio;
        private BlendState blendState;

        public UserInterface()
        {
            spriteFonts = new Dictionary<FontSize, SpriteFont>();
            blendState = new BlendState();
            blendState.ColorBlendFunction = BlendFunction.Add;
            blendState.ColorSourceBlend = Blend.SourceAlpha;
            blendState.ColorDestinationBlend = Blend.InverseSourceAlpha;
            blendState.AlphaBlendFunction = BlendFunction.Add;
            blendState.AlphaSourceBlend = Blend.SourceAlpha;
            blendState.AlphaDestinationBlend = Blend.InverseSourceAlpha;
        }

        public void Initialize(IGraphicsFactory graphicsFactory, TextureFactory textureFactory)
        {
            spriteBatch = new SpriteBatch(graphicsFactory.GraphicsDevice);
            if (!spriteFonts.ContainsKey(FontSize.Medium))
                spriteFonts[FontSize.Medium] = graphicsFactory.SpriteFontFromFile("Content/fonts/TweakerFont");
            whiteTexture = textureFactory.WhiteTexture;
            aspectRatio = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth /
                (float)spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
        }

        public void DrawControl(IControl control)
        {
            spriteBatch.GraphicsDevice.BlendState = blendState;
            int num = control.DrawControl(this);
            Console.WriteLine("Drew {0} controls.", num);
        }


        public void SetFont(FontSize size, SpriteFont font)
        {
            spriteFonts[size] = font;
        }


        #region IDrawResources Members

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public SpriteFont GetSpriteFont(FontSize size)
        {
            return spriteFonts[size];
        }

        public Texture2D WhiteTexture
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
