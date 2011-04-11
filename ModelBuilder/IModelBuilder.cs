using System;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public interface IModelBuilder
    {
        TextureFactory TextureFactory { get; }
        EffectFactory EffectFactory { get; }
        GraphicsDevice GraphicsDevice { get; }
        void CreateMaterial(string materialName);
        CustomModel CreateModel(IModifier generator, MaterialHandler modelMaterial);
        CustomModel CreateModel(IModifier generator, string material);
        MaterialHandler GetMaterial(string name);
        void SetAmbientColor(string materialName, Color color);
        void SetBlendMode(string materialName, BlendFunction blendFunction, Blend sourceBlend, Blend destinationBlend);
        void SetDiffuseColor(string materialName, Color color);
        void SetDiffuseTexture(string materialName, Texture2D texture);
        void SetDiffuseTexture(string materialName, string fileName);
        void SetEffect(string materialName, string effectName);
        void SetMaterial(string materialName, MaterialHandler material);
        void SetNormalTexture(string materialName, Texture2D texture);
        void SetNormalTexture(string materialName, string fileName);
        void SetReflectiveFactor(string materialName, float factor);
        void SetTransparency(string materialName, float factor);
        void SetReflectiveTexture(string materialName, string fileName);
        void SetShininess(string materialName, float shininess);
        void SetSpecularColor(string materialName, Color color);
        void SetSpecularPower(string materialName, float power);
    }
}
