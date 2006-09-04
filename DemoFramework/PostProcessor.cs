using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public class PostProcessor
    {
        public enum TextureID
        {
            FULLSIZE_TEXTURE_1 = 0,
            FULLSIZE_TEXTURE_2,
            INPUT_TEXTURE
        };

        private IDevice device;
        private TextureID lastUsedTexture;
        private ITexture inputTexture;
        private ITexture[] textures;
        private IEffect effect;

        private EffectHandle monochromeHandle;
        private EffectHandle sourceTextureParameter;

        public PostProcessor()
        {
        }

        public void Initialize(IDevice device)
        {
            this.device = device;
            effect = D3DDriver.EffectFactory.CreateFromFile("../../../Effects/PostEffects.fxo");

            monochromeHandle = effect.GetTechnique("Monochrome");
            if (monochromeHandle == null)
                throw new DDXXException("Could not find Monochrome technique in PostEffects effect file.");

            sourceTextureParameter = effect.GetParameter(null, "SourceTexture");

            textures = new ITexture[(int)TextureID.INPUT_TEXTURE];
            for (int i = 0; i < (int)TextureID.INPUT_TEXTURE; i++)
            {
                textures[i] = D3DDriver.TextureFactory.CreateFullsizeRenderTarget();
            }
        }

        public ITexture OutputTexture
        {
            get
            {
                if (lastUsedTexture == TextureID.INPUT_TEXTURE)
                    return inputTexture;
                return textures[(int)lastUsedTexture];
            }
        }

        public TextureID OutputTextureID
        {
            get { return lastUsedTexture; }
        }

        public void StartFrame(ITexture startTexture)
        {
            inputTexture = startTexture;
            lastUsedTexture = TextureID.INPUT_TEXTURE;
        }

        private ITexture GetTexture(TextureID texture)
        {
            if (texture == TextureID.INPUT_TEXTURE)
                return inputTexture;
            return textures[(int)texture];
        }

        public void Monochrome(TextureID source, TextureID destination)
        {
            int height = device.PresentationParameters.BackBufferHeight;
            int width = device.PresentationParameters.BackBufferWidth;
            CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[4];
            vertices[0].Position = new Vector4(-0.5f, -0.5f, 1.0f, 1.0f);
            vertices[1].Position = new Vector4(width - 0.5f, -0.5f, 1.0f, 1.0f);
            vertices[2].Position = new Vector4(-0.5f, height - 0.5f, 1.0f, 1.0f);
            vertices[3].Position = new Vector4(width - 0.5f, height - 0.5f, 1.0f, 1.0f);
            vertices[0].Tu = 0.0f;
            vertices[0].Tv = 0.0f;
            vertices[1].Tu = 1.0f;
            vertices[1].Tv = 0.0f;
            vertices[2].Tu = 0.0f;
            vertices[2].Tv = 1.0f;
            vertices[3].Tu = 1.0f;
            vertices[3].Tv = 1.0f;

            device.SetRenderTarget(0, GetTexture(destination).GetSurfaceLevel(0));
            effect.SetValue(sourceTextureParameter, GetTexture(source));
            effect.Technique = monochromeHandle;
            device.VertexFormat = CustomVertex.TransformedTextured.Format;

            device.BeginScene();
            int passes = effect.Begin(FX.None);
            for (int i = 0; i < passes; i++)
            {
                effect.BeginPass(i);
                device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, vertices);
                effect.EndPass();
            }
            effect.End();
            device.EndScene();

            lastUsedTexture = destination;
        }


        internal void DebugWriteAllTextures()
        {
            TextureLoader.Save("INPUT.jpg", ImageFileFormat.Jpg, ((TextureAdapter)inputTexture).TextureDX);
            TextureLoader.Save("FULLSCREEN_1.jpg", ImageFileFormat.Jpg, ((TextureAdapter)textures[(int)TextureID.FULLSIZE_TEXTURE_1]).TextureDX);
            TextureLoader.Save("FULLSCREEN_2.jpg", ImageFileFormat.Jpg, ((TextureAdapter)textures[(int)TextureID.FULLSIZE_TEXTURE_2]).TextureDX);
        }
    }
}
