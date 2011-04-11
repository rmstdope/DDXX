using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace EngineTest
{
    public class IntroText : BaseDemoEffect
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public IntroText(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = GraphicsFactory.SpriteFontFromFile("Content\\fonts\\Porscha");
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            string destString = "dope returns!";
            string s = "";
            float time = Time.CurrentTime - StartTime;

            float destTime = 2;
            foreach (char ch in destString)
            {
                if (time > destTime || ch == ' ')
                    s += ch;
                else
                    s += Rand.Char('a', 'z');
                destTime += 0.4f;
            }
            spriteBatch.Begin();
            spriteBatch.DrawString(font, s, position, Color.White);
            spriteBatch.End();
        }
    }
}
