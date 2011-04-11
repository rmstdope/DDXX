using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public class TextNode : NodeBase
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private string text;

        public TextNode(string name, SpriteBatch spriteBatch, SpriteFont spriteFont)
            : base(name)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.text = name;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(IScene scene)
        {
            Vector4 screenPos = Vector4.Transform(new Vector4(Position, 1), scene.ActiveCamera.ViewMatrix * scene.ActiveCamera.ProjectionMatrix);
            screenPos = screenPos / screenPos.W;
            screenPos.X += 1f;
            screenPos.Y *= -1;
            screenPos.Y += 1f;
            screenPos.X *= 0.5f * spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            screenPos.Y *= 0.5f * spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
            Vector2 size = spriteFont.MeasureString(text);
            screenPos.X -= size.X / 2;
            screenPos.Y -= size.Y / 2;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.DrawString(spriteFont, text, new Vector2(screenPos.X, screenPos.Y), Color.White);
            spriteBatch.End();
        }
    }
}
