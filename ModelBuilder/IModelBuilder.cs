using System;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.ModelBuilder
{
    public interface IModelBuilder
    {
        IModel CreateModel(IModifier generator, IMaterialHandler modelMaterial);
        IModel CreateModel(IModifier generator, string material);
        IMaterialHandler GetMaterial(string name);
        void SetAmbientColor(string materialName, Color color);
        void SetDiffuseColor(string materialName, Color color);
        void SetDiffuseTexture(string materialName, ITexture2D texture);
        void SetDiffuseTexture(string materialName, string fileName);
        void SetEffect(string materialName, string effectName);
        void SetMaterial(string materialName, IMaterialHandler material);
        void SetNormalTexture(string materialName, ITexture2D texture);
        void SetNormalTexture(string materialName, string fileName);
        void SetReflectiveFactor(string materialName, float factor);
        void SetReflectiveTexture(string materialName, string fileName);
        void SetShininess(string materialName, float shininess);
        void SetSpecularColor(string materialName, Color color);
        void SetSpecularPower(string materialName, float power);
    }
}
