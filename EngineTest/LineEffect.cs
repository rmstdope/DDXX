using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.ModelBuilder;
using Dope.DDXX.TextureBuilder;

namespace EngineTest
{
    public class LineEffect : BaseDemoEffect
    {
        private class TextData
        {
            private string text;
            private int textPosition;
            private float fraction;
            private float speed;
            public float Fraction { get { return fraction; } }
            public TextData(string text, float speed)
            {
                this.text = text;
                this.speed = speed;
                this.textPosition = 0;
                this.fraction = 0;
            }
            public string Text
            {
                get
                {
                    if (text.Length - textPosition >= 25)
                        return text.Substring(textPosition, 25);
                    return text.Substring(textPosition) + text.Substring(0, 30 - (text.Length - textPosition));
                }
            }
            public void Advance(ISpriteFont font)
            {
                fraction += speed;
                if (fraction >= font.MeasureString(text[textPosition].ToString()).X + font.Spacing)
                {
                    fraction = 0;
                    textPosition++;
                    if (textPosition == text.Length)
                        textPosition = 0;
                }
            }
            public float GetHeight(ISpriteFont font)
            {
                return (int)(font.MeasureString("A").Y + 0.5f);
            }
        }

        private CameraNode camera;
        private LineNode[] lines;
        private ISpline<InterpolatedVector3>[] splines;
        private IRenderTarget2D textRenderTarget;
        private IRenderTarget2D highlightRenderTarget;
        private ISpriteBatch spriteBatch;
        private ISpriteFont font;
        private List<TextData> textData;
        private List<BlitSine> highlights;

        private const int NumLines = 50;

        private ITexture2D circleTexture;

        public LineEffect(string name, float start, float end)
            : base(name, start, end)
        {
            splines = new ISpline<InterpolatedVector3>[NumLines];
            lines = new LineNode[NumLines];
            textData = new List<TextData>();
            highlights = new List<BlitSine>();
        }

        protected override void Initialize()
        {
            spriteBatch = GraphicsFactory.CreateSpriteBatch();
            textRenderTarget = GraphicsFactory.CreateRenderTarget2D(256, 256, 1, SurfaceFormat.Color, MultiSampleType.None, 0);
            highlightRenderTarget = GraphicsFactory.CreateRenderTarget2D(256, 256, 1, SurfaceFormat.Color, MultiSampleType.None, 0);
            font = GraphicsFactory.SpriteFontFromFile("Content\\fonts\\PorschaSmall");

            CreateStandardCamera(out camera, 15);

            //for (int i = 0; i < NumLines; i++)
            //{
            //    splines[i] = new SimpleCubicSpline<InterpolatedVector3>();
            //    AddKeyFrame(i, 0, new Vector3(0, 0, 0));
            //    AddKeyFrame(i, 1, new Vector3(4, 2, 0));
            //    AddKeyFrame(i, 2, new Vector3(-7, -5, 0));
            //    AddKeyFrame(i, 3, new Vector3(-2, 8, 0));
            //    AddKeyFrame(i, 4, new Vector3(4, -6, 0));
            //    splines[i].Calculate();
            //    IMaterialHandler material = new MaterialHandler(EffectFactory.CreateFromFile("Content\\effects\\ColorLine"), new EffectConverter());
            //    material.DiffuseColor = new Color(new Vector3((i + 1) / (float)NumLines, (i + 1) / (float)NumLines, (i + 1) / (float)NumLines));
            //    lines[i] = new LineNode("Line", GraphicsFactory,
            //        material,
            //        splines[i], 100);
            //    Scene.AddNode(lines[i]);
            //}

            textData.Add(new TextData("Dope greets the following heros: asd-outbreak-farbrausch-tbl-fairlight.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Why are you reading this text? Get a life!  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Dope - Don't try this at home!  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("The stars in heaven shine just as bright as your silver dollar.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Dope greets the following heros: asd-outbreak-farbrausch-tbl-fairlight.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("The micro organisms that will feed on your dead corpse are just a small part of the big game of life.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Dope - Don't try this at home!  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("The stars in heaven shine just as bright as your silver dollar.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Dope greets the following heros: asd-outbreak-farbrausch-tbl-fairlight.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("The micro organisms that will feed on your dead corpse are just a small part of the big game of life.  ", Rand.Float(0.5f, 1.5f)));

            //TextureDirector.CreateCircle(0.05f, 0.5f);
            circleTexture = TextureFactory.CreateFromName("Circle");
            //TextureDirector.Generate("Circle", 64, 64, 0, SurfaceFormat.Color);

            highlights.Add(new BlitSine());
            highlights.Add(new BlitSine());
            highlights.Add(new BlitSine());
            highlights.Add(new BlitSine());
            highlights.Add(new BlitSine());
            highlights.Add(new BlitSine());
            highlights.Add(new BlitSine());
            highlights.Add(new BlitSine());
            highlights.Add(new BlitSine());
            highlights.Add(new BlitSine());
        }

        private void AddKeyFrame(int i, float t, Vector3 vector)
        {
            vector += Rand.Vector3(0.2f);
            splines[i].AddKeyFrame(new KeyFrame<InterpolatedVector3>(t, new InterpolatedVector3(vector)));
        }

        public override void Step()
        {
            const float OffsetY = 3;
            IRenderTarget2D oldRt = GraphicsDevice.GetRenderTarget(0) as IRenderTarget2D;
            GraphicsDevice.SetRenderTarget(0, highlightRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);            
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            GraphicsDevice.RenderState.DestinationBlend = Blend.One;
            GraphicsDevice.RenderState.SourceBlend = Blend.One;
            GraphicsDevice.RenderState.AlphaTestEnable = false;
            spriteBatch.Draw(circleTexture, new Rectangle(0, 0, 256, 256), Color.White);
            foreach (BlitSine sine in highlights)
            {
                sine.Draw(spriteBatch, circleTexture);
            }
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(0, textRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
            spriteBatch.Begin();
            float y = -OffsetY;
            foreach (TextData data in textData)
            {
                spriteBatch.DrawString(font, data.Text, new Vector2(-data.Fraction, y), Color.White);
                y += data.GetHeight(font) - OffsetY;
                data.Advance(font);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            GraphicsDevice.RenderState.DestinationBlend = Blend.Zero;
            GraphicsDevice.RenderState.SourceBlend = Blend.DestinationColor;
            spriteBatch.Draw(highlightRenderTarget.GetTexture(), new Rectangle(0, 0, 256, 256), Color.White);
            spriteBatch.End();
            
            GraphicsDevice.SetRenderTarget(0, oldRt);

            //for (int i = 0; i < NumLines; i++)
            //{
            //    lines[i].WorldState.Turn(Time.DeltaTime * 1.2f);
            //    lines[i].WorldState.Tilt(Time.DeltaTime * 0.5f);
            //    lines[i].EndTime = Time.CurrentTime;
            //}
            Scene.Step();
        }

        public override void Render()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(textRenderTarget.GetTexture(), 
                new Rectangle(BackbufferWidth / 2 - 128, BackbufferHeight / 2 - 128, 256, 256), Color.White);
            spriteBatch.End();
            Scene.Render();
        }
    }
}
