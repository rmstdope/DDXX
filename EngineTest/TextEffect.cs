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
    public class TextEffect : BaseDemoEffect
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
                    const int numChars = 35;
                    if (text.Length - textPosition >= numChars)
                        return text.Substring(textPosition, numChars);
                    return text.Substring(textPosition) + text.Substring(0, numChars - (text.Length - textPosition));
                }
            }
            public void Advance(SpriteFont font)
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
            public float GetHeight(SpriteFont font)
            {
                return (int)(font.MeasureString("A").Y + 0.5f);
            }
        }

        private CameraNode camera;
        private LineNode[] lines;
        private ISpline<InterpolatedVector3>[] splines;
        private RenderTarget2D textRenderTarget;
        private RenderTarget2D highlightRenderTarget;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private List<TextData> textData;
        private List<BlitSine> highlights;
        private BlendState blend1;
        private BlendState blend2;

        private const int NumLines = 50;

        private Texture2D circleTexture;
        private CustomModel model;

        public TextEffect(string name, float start, float end)
            : base(name, start, end)
        {
            splines = new ISpline<InterpolatedVector3>[NumLines];
            lines = new LineNode[NumLines];
            textData = new List<TextData>();
            highlights = new List<BlitSine>();
            blend1 = new BlendState();
            blend1.AlphaBlendFunction = blend1.ColorBlendFunction = BlendFunction.Add;
            blend1.AlphaSourceBlend = blend1.ColorSourceBlend = Blend.One;
            blend1.AlphaDestinationBlend = blend1.ColorDestinationBlend = Blend.One;
            blend2 = new BlendState();
            blend2.AlphaBlendFunction = blend2.ColorBlendFunction = BlendFunction.Add;
            blend2.AlphaSourceBlend = blend2.ColorSourceBlend = Blend.DestinationColor;
            blend2.AlphaDestinationBlend = blend2.ColorDestinationBlend = Blend.Zero;
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsFactory.GraphicsDevice);
            textRenderTarget = new RenderTarget2D(GraphicsFactory.GraphicsDevice, 256, 256, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            highlightRenderTarget = new RenderTarget2D(GraphicsFactory.GraphicsDevice, 256, 256, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
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
            //    MaterialHandler material = new MaterialHandler(EffectFactory.CreateFromFile("Content\\effects\\ColorLine"), new EffectConverter());
            //    material.DiffuseColor = new Color(new Vector3((i + 1) / (float)NumLines, (i + 1) / (float)NumLines, (i + 1) / (float)NumLines));
            //    lines[i] = new LineNode("Line", GraphicsFactory,
            //        material,
            //        splines[i], 100);
            //    Scene.AddNode(lines[i]);
            //}

            textData.Add(new TextData("Dope greets the following heros: asd-outbreak-farbrausch-tbl-fairlight.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Why are you reading this text? Get a life!  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Dope - Don't try this at home!      ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("The stars in heaven shine just as bright as your silver dollar.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Dope greets the following heros: asd-outbreak-farbrausch-tbl-fairlight.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("The micro organisms that will feed on your dead corpse are just a small part of the big game of life.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Dope - Don't try this at home!      ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("The stars in heaven shine just as bright as your silver dollar.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Dope greets the following heros: asd-outbreak-farbrausch-tbl-fairlight.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("The micro organisms that will feed on your dead corpse are just a small part of the big game of life.  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Why are you reading this text? Get a life!  ", Rand.Float(0.5f, 1.5f)));
            textData.Add(new TextData("Dope - Don't try this at home!      ", Rand.Float(0.5f, 1.5f)));

            circleTexture = TextureFactory.CreateFromName("Circle");

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

            ModelDirector.CreatePlane(3, 3, 1, 1);
            ModelBuilder.SetEffect("Default", "Content\\effects\\HighlightedText");
            ModelBuilder.SetDiffuseTexture("Default", circleTexture);
            ModelBuilder.SetDiffuseColor("Default", new Color(150, 150, 150));
            model = ModelDirector.Generate("Default");

            CreatePlane(0.0f);
            CreatePlane(MathHelper.PiOver4);
            CreatePlane(-MathHelper.PiOver4);
            CreatePlane(MathHelper.PiOver2);
            CreatePlane(-MathHelper.PiOver2);

            ModelDirector.CreateBox(20, 20, 3);
            ModelDirector.Translate(0, 1.5f, 0);
            ModelDirector.NormalFlip();
            ModelBuilder.CreateMaterial("Room");
            ModelBuilder.SetDiffuseColor("Room", new Color(150, 150, 150));
            ModelBuilder.SetDiffuseTexture("Room", "Content\\textures\\yellowswirls_untiled");
            ModelBuilder.SetEffect("Room", "Content\\effects\\HighlightedText");
            CustomModel roomModel = ModelDirector.Generate("Room");
            ModelNode modelNode = new ModelNode("Room", roomModel, GraphicsDevice);
            Scene.AddNode(modelNode);
        }

        private void CreatePlane(float rotation)
        {
            ModelNode modelNode = new ModelNode("Plane", model, GraphicsDevice);
            modelNode.WorldState.Turn(rotation);
            modelNode.WorldState.MoveUp(1.5f);
            modelNode.WorldState.MoveForward(5.0f);
            MirrorNode mirror = new MirrorNode(modelNode);
            mirror.Brightness = 0.2f;
            Scene.AddNode(modelNode);
            Scene.AddNode(mirror);
        }

        private void AddKeyFrame(int i, float t, Vector3 vector)
        {
            vector += Rand.Vector3(0.2f);
            splines[i].AddKeyFrame(new KeyFrame<InterpolatedVector3>(t, new InterpolatedVector3(vector)));
        }

        public override void Step()
        {
            const float OffsetY = 3;
            RenderTargetBinding[] oldRt = GraphicsDevice.GetRenderTargets();
            GraphicsDevice.SetRenderTarget(highlightRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target, new Color(20, 20, 20), 0, 0);            
            spriteBatch.Begin(SpriteSortMode.Immediate, blend1);
            spriteBatch.Draw(circleTexture, new Rectangle(0, 0, 256, 256), Color.White);
            foreach (BlitSine sine in highlights)
            {
                sine.Draw(spriteBatch, circleTexture);
            }
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(textRenderTarget);
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
            spriteBatch.Begin(SpriteSortMode.Immediate, blend2);
            spriteBatch.Draw(highlightRenderTarget, new Rectangle(0, 0, 256, 256), Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTargets(oldRt);
            //System.IO.Stream stream = System.IO.File.Open("test.jpg", System.IO.FileMode.Create);
            //textRenderTarget.SaveAsJpeg(stream, 256, 256);

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
            //spriteBatch.Begin();
            //spriteBatch.Draw(textRenderTarget.GetTexture(), 
            //    new Rectangle(0, 0, 256, 256), Color.White);
            //spriteBatch.Draw(highlightRenderTarget.GetTexture(),
            //    new Rectangle(256, 0, 256, 256), Color.White);
            //spriteBatch.End();
            model.Meshes[0].MeshParts[0].MaterialHandler.DiffuseTexture = textRenderTarget;
            //model.Meshes[0].MeshParts[0].MaterialHandler.AmbientColor = new Color(50, 50, 50);
            Scene.Render();
        }
    }
}
